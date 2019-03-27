using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class RoleException : Exception
    {
        public RoleException()
        {
        }

        public RoleException(string message)
            : base(message)
        {
        }

        public RoleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RoleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
