using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class ManagerNotFoundException : Exception
    {
        public ManagerNotFoundException()
        {
        }

        public ManagerNotFoundException(string message)
            : base(message)
        {
        }

        public ManagerNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ManagerNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
