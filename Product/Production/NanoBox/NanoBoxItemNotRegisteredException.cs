using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace NanoBox
{
    public class NanoBoxItemNotRegisteredException : Exception
    {
        public NanoBoxItemNotRegisteredException()
        {
        }

        public NanoBoxItemNotRegisteredException(string message) : base(message)
        {
        }

        public NanoBoxItemNotRegisteredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NanoBoxItemNotRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

