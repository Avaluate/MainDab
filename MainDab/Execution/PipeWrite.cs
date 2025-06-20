using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainDabRedo.Execution
{
    public class PipeWrite
    {

        private readonly string PipeName;
        private readonly int Timeout;
        private NamedPipeClientStream Pipe;
        private readonly object Lock = new object();
        private bool Disposed = false;

        public PipeWrite(string pipeName, int timeoutMs = 5000)
        {
            PipeName = pipeName;
            Timeout = timeoutMs;
        }

        private void EnsureConnected()
        {
            lock (Lock)
            {
                if (Pipe == null || !Pipe.IsConnected)
                {
                   // Pipe.Dispose();
                    Pipe = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
                    Pipe.Connect(Timeout);
                    Console.WriteLine("connected");
                }
            }
        }

        public T SendRequest<T>(string messageType, object data)
        {
            lock (Lock) // requests are sent one at a time
            {
                if (Disposed) throw new ObjectDisposedException(nameof(PipeWrite)); // unlikely case

                EnsureConnected(); 

                try
                {
                    var Req = new RequestMessage
                    {
                        MessageType = messageType,
                        Data = JsonConvert.SerializeObject(data)
                    };

                    WriteMessage(Pipe, Req);
                    var Res = ReadMessage(Pipe);

                    if (!Res.Success)
                    {
                        throw new Exception($"error: {Res.ErrorMessage}");
                    }

                    return JsonConvert.DeserializeObject<T>(Res.Data);
                }
                catch (IOException)
                {
                    // will reconnect on next call
                    Pipe.Dispose();
                    Pipe = null;
                    throw new Exception("connection lost");
                }
            }
        }


        private void WriteMessage(NamedPipeClientStream Me, RequestMessage Req)
        {
            var MessageJson = JsonConvert.SerializeObject(Req);
            var MessageBytes = Encoding.UTF8.GetBytes(MessageJson);
            var LengthBytes = BitConverter.GetBytes(MessageBytes.Length);

            Me.Write(LengthBytes, 0, LengthBytes.Length);
            Me.Write(MessageBytes, 0, MessageBytes.Length);
            Me.Flush();
        }

        private ResponseMessage ReadMessage(NamedPipeClientStream Me)
        {
            // Read message length
            var LenBuffer = new byte[4]; // sufficient
            Me.Read(LenBuffer, 0, 4);
            var MessageLength = BitConverter.ToInt32(LenBuffer, 0);

            // Read message
            var MessageBuffer = new byte[MessageLength];
            var TotalBytesRead = 0;

            while (TotalBytesRead < MessageLength)
            {
                var BytesRead = Me.Read(
                    MessageBuffer,
                    TotalBytesRead,
                    MessageLength - TotalBytesRead);
                TotalBytesRead += BytesRead;
            }

            var messageJson = Encoding.UTF8.GetString(MessageBuffer);
            return JsonConvert.DeserializeObject<ResponseMessage>(messageJson);
        }

        public void Dispose()
        {
            lock (Lock)
            {
                if (!Disposed)
                {
                    Pipe.Dispose();
                    Disposed = true;
                }
            }
        }
    }
}
