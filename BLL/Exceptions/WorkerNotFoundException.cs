using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class WorkerNotFoundException : Exception
    {
        public WorkerNotFoundException()
        {
        }

        public WorkerNotFoundException(string message)
            : base(message)
        {
        }

        public WorkerNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public WorkerNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
