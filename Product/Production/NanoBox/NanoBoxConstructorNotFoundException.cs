using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NanoBox
{
    public class NanoBoxConstructorNotFoundException : Exception
    {
        public NanoBoxConstructorNotFoundException()
        {
        }

        public NanoBoxConstructorNotFoundException(string message)
        {
            
        }

        public NanoBoxConstructorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NanoBoxConstructorNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
