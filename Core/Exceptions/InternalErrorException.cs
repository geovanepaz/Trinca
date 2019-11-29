using System;
using System.Runtime.Serialization;

namespace Core.Exceptions
{
    public class InternalErrorException : Exception
    {
        public readonly object Arguments;

        internal InternalErrorException()
        {
        }

        public InternalErrorException(string message) : base(message)
        {
        }

        public InternalErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InternalErrorException(string message, Exception innerException, object arguments = null) : base(message, innerException) => Arguments = arguments;

        public InternalErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}