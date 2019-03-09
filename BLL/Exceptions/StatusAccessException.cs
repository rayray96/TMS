using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class StatuskAccessException : Exception
    {
        public StatuskAccessException()
        {
        }

        public StatuskAccessException(string message)
            : base(message)
        {
        }

        public StatuskAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public StatuskAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
