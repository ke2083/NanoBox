using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBox
{
    public static class IoC
    {
        public static T Get<T>()
        {
            return Get<T>(new object[0]);
        }

        public static TDefinition Get<TDefinition>(object[] constructorArguments)
        {
            return NanoBoxManager.Instance.Get<TDefinition>(constructorArguments);
        }

        public static void Remove<TDefinition, TImplementation>()
        {
            NanoBoxManager.Instance.Remove<TDefinition, TImplementation>();
        }

        public static void Set<TDefinition, TImplementation>()
        {
            NanoBoxManager.Instance.Set<TDefinition, TImplementation>();
        }
    }
}

