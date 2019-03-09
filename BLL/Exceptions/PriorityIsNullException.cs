using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class PriorityIsNullException : Exception
    {
        public PriorityIsNullException()
        {
        }

        public PriorityIsNullException(string message)
            : base(message)
        {
        }

        public PriorityIsNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PriorityIsNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
