namespace MainDabRedo.Execution
{
    public class PipeSync
    {
        private readonly PipeWrite Pipe;

        public PipeSync(string PipeName, int Timeout = 5000)
        {
            Pipe = new PipeWrite(PipeName, Timeout);
        }

        public T SendRequest<T>(string messageType, object data)
        {
            return Pipe.SendRequest<T>(messageType, data);
        }

        public void Dispose()
        {
            Pipe.Dispose();
        }
    }
}
