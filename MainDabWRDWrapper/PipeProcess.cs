using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainDabWRDWrapper
{
    public class PipeProcess
    {
        private string PipeName;
        private CancellationTokenSource Cancellation;
        private NamedPipeServerStream _pipeServer;
        private bool IsRun;

        public PipeProcess(string pipeName)
        {
            PipeName = pipeName;
            Cancellation = new CancellationTokenSource();
        }

        public async Task StartAsync()
        {
            IsRun = true;

            while (IsRun && !Cancellation.Token.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("Creating new pipe instance...");
                    _pipeServer = new NamedPipeServerStream(
                        PipeName,
                        PipeDirection.InOut,
                        1,
                        PipeTransmissionMode.Byte,
                        PipeOptions.Asynchronous);

                    Console.WriteLine("Waiting for client connection...");
                    await _pipeServer.WaitForConnectionAsync(Cancellation.Token);
                    Console.WriteLine("Client connected!");

                    // Handle multiple requests from the same client
                    await HandleClientSessionAsync(_pipeServer);

                    Console.WriteLine("Client disconnected, cleaning up...");
                    _pipeServer.Dispose();
                    _pipeServer = null;

                    // Small delay to ensure clean pipe closure
                    await Task.Delay(100);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Server operation cancelled");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Server error: {ex.Message}");

                    // Clean up current pipe
                    if (_pipeServer != null)
                    {
                        try
                        {
                            _pipeServer.Dispose();
                            _pipeServer = null;
                        }
                        catch { }
                    }

                    await Task.Delay(1000); // Wait before retrying
                }
            }
        }

        private async Task HandleClientSessionAsync(NamedPipeServerStream pipeServer)
        {
            try
            {
                while (pipeServer.IsConnected && !Cancellation.Token.IsCancellationRequested)
                {
                    Console.WriteLine("Waiting for request...");

                    // Read request
                    var request = await ReadMessageAsync(pipeServer);
                    if (request == null)
                    {
                        Console.WriteLine("Failed to read message or client disconnected");
                        break;
                    }

                    Console.WriteLine($"Received request: {request.MessageType} (ID: {request.RequestId})");

                    // Process request
                    var response = await ProcessRequestAsync(request);

                    // Send response
                    await WriteMessageAsync(pipeServer, response);
                    Console.WriteLine($"Sent response for request ID: {request.RequestId}");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO Exception (client likely disconnected): {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client session error: {ex.Message}");
            }
        }

        // me when stackoverflow
        private async Task<RequestMessage> ReadMessageAsync(NamedPipeServerStream pipeServer)
        {
            try
            {
                // Read message length
                var LengthBuffer = new byte[4];
                await pipeServer.ReadAsync(LengthBuffer, 0, 4);
                var MessageLength = BitConverter.ToInt32(LengthBuffer, 0);

                // Read message
                var MessageBuffer = new byte[MessageLength];
                var TotalBytesRead = 0;

                while (TotalBytesRead < MessageLength)
                {
                    var read = await pipeServer.ReadAsync(
                        MessageBuffer,
                        TotalBytesRead,
                        MessageLength - TotalBytesRead);
                    TotalBytesRead += read;
                }

                return JsonConvert.DeserializeObject<RequestMessage>(Encoding.UTF8.GetString(MessageBuffer));
            }
            catch
            {
                return null;
            }
        }

        private async Task WriteMessageAsync(NamedPipeServerStream Pipe, ResponseMessage Response)
        {
            var MessageJson = JsonConvert.SerializeObject(Response);
            var MessageBytes = Encoding.UTF8.GetBytes(MessageJson);
            var Len = BitConverter.GetBytes(MessageBytes.Length);

            // Write message length then message
            await Pipe.WriteAsync(Len, 0, Len.Length);
            await Pipe.WriteAsync(MessageBytes, 0, MessageBytes.Length);
            await Pipe.FlushAsync();
        }

        private async Task<ResponseMessage> ProcessRequestAsync(RequestMessage request)
        {
            var response = new ResponseMessage
            {
                RequestId = request.RequestId,
                MessageType = $"{request.MessageType}_Response",
                Success = true
            };

            try
            {
                // would be wise
                switch (request.MessageType)
                {
                    case "Inject":
                        // initilise fake server
                        Console.WriteLine("inject");
                        if (!WRDCert.EnsureBypassConfigurationEstablished())
                        {
                            response.Data = JsonConvert.SerializeObject(new InjectionRequest {InjectionSuccessful = false, AdditionalData = "Failed to create/install a locally signed certificate for mboost.me (required to bypass key system)"});
                            Console.WriteLine("tx inject Failed to create/install a locally signed certificate for mboost.me (required to bypass key system)");
                            break;
                        }

                        if (WRDCert.LaunchNodeBypassServer() == null)
                        {
                            response.Data = JsonConvert.SerializeObject(new InjectionRequest { InjectionSuccessful = false, AdditionalData = "Failed to start up the WRD API key system bypass local server" });
                            Console.WriteLine("Failed to start up the WRD API key system bypass local server");
                            break;
                        }

                        WRDAPI.initialize();
                        response.Data = JsonConvert.SerializeObject(new InjectionRequest { InjectionSuccessful = true });
                        Console.WriteLine("tx inject ok");
                        break;

                    case "Execute":
                        var ExecuteReq = JsonConvert.DeserializeObject<ExecutionRequest>(request.Data);
                        WRDAPI.Execute(ExecuteReq.Script);
                        response.Data = JsonConvert.SerializeObject(new ExecutionRequest { AdditionalData = "success" });
                        Console.WriteLine("tx execute ok");
                        break;

                    case "IsInjected":
                        var InjectReq = JsonConvert.DeserializeObject<IsInjectedRequest>(request.Data);
                        response.Data = JsonConvert.SerializeObject(new IsInjectedRequest { IsInjected = WRDAPI.IsInjected()});
                        Console.WriteLine("tx isinjected ok");
                        break;

                    default:
                        response.Success = false;
                        response.ErrorMessage = $"message type: {request.MessageType}";
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        // probably not needed
        public void Stop()
        {
            IsRun = false;
            Cancellation.Cancel();
        }

    }
}
