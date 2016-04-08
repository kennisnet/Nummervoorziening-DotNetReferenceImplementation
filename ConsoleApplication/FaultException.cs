using System;
using System.Runtime.Serialization;

namespace ConsoleNVAClient
{
    [Serializable]
    internal class FaultException : Exception
    {
        public FaultException()
        {
        }

        public FaultException(string message) : base(message)
        {
        }

        public FaultException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FaultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}