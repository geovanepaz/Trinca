using System;
using System.Runtime.Serialization;

namespace Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        internal UnauthorizedException()
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}