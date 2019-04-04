using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class PriorityNotFoundException : Exception
    {
        public PriorityNotFoundException()
        {
        }

        public PriorityNotFoundException(string message)
            : base(message)
        {
        }

        public PriorityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PriorityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
