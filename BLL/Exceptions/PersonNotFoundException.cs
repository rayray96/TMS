using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    public class PersonNotFoundException : Exception
    {
        public PersonNotFoundException()
        {
        }

        public PersonNotFoundException(string message)
            : base(message)
        {
        }

        public PersonNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PersonNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
