using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainDabRedo.Execution
{
    // named pipes are used for interprocess communication. due to the nature of WRD api & c# threads this is necessary
    // (local server would've worked too)

    public class CoreData
    {
        public string MessageType { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class RequestMessage : CoreData
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
    }

    public class ResponseMessage : CoreData
    {
        public string RequestId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    // Type of requests
    public class InjectionRequest
    {
        public bool InjectionSuccessful { get; set; }
        public string AdditionalData { get; set; }
    }

    public class ExecutionRequest
    {
        public string Script { get; set; }
        public string AdditionalData { get; set; }

    }

    public class IsInjectedRequest
    {
        public bool IsInjected { get; set; }
        public string AdditionalData { get; set; }
    }
}
