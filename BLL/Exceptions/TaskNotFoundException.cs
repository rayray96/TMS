using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException()
        {
        }

        public TaskNotFoundException(string message)
            : base(message)
        {
        }

        public TaskNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TaskNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
