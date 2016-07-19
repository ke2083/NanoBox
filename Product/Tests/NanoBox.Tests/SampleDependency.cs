using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoBoxTests
{
    public class SampleDependency : IDependencyImplementer
    {
        private readonly ISampleInterface dependency;

        public SampleDependency(ISampleInterface dep)
        {
            dependency = dep;
        }

        public bool Answer()
        {
            return dependency.Success();
        }
    }
}

