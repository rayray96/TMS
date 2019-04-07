using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class DateIsWrongException : Exception
    {
        public DateIsWrongException()
        {
        }

        public DateIsWrongException(string message)
            : base(message)
        {
        }

        public DateIsWrongException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DateIsWrongException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
