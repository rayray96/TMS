using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException()
        {
        }

        public TeamNotFoundException(string message)
            : base(message)
        {
        }

        public TeamNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TeamNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
