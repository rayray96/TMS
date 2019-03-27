using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class RefreshTokenNotFoundException : Exception
    {
        public RefreshTokenNotFoundException()
        {
        }

        public RefreshTokenNotFoundException(string message)
            : base(message)
        {
        }

        public RefreshTokenNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RefreshTokenNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
