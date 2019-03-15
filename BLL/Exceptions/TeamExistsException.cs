using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class TeamExistsException : Exception
    {
        public TeamExistsException()
        {
        }

        public TeamExistsException(string message)
            : base(message)
        {
        }

        public TeamExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TeamExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}