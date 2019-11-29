using System;
using System.Runtime.Serialization;

namespace Core.Exceptions
{
    public class ServiceUnavailableException : Exception
    {
        public readonly object Arguments;

        internal ServiceUnavailableException()
        {
        }

        public ServiceUnavailableException(string message) : base(message)
        {
        }

        public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ServiceUnavailableException(string message, object arguments = null) : base(message) => Arguments = arguments;

        public ServiceUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}