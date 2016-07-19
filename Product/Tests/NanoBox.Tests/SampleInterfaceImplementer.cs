using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoBoxTests
{

    public class SampleInterfaceImplementer : ISampleInterface
    {
        private readonly bool returnValue = true;

        public bool Success()
        {
            return returnValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleInterfaceImplementer"/> class.
        /// </summary>
        public SampleInterfaceImplementer()
        {
        }

        public SampleInterfaceImplementer(bool returnValue)
        {
            this.returnValue = returnValue;
        }
    }
}

