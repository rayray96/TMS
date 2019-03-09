using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class TaskAccessException : Exception
    {
        public TaskAccessException()
        {
        }

        public TaskAccessException(string message)
            : base(message)
        {
        }

        public TaskAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TaskAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
