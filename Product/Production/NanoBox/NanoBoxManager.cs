using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NanoBox
{
    internal sealed class NanoBoxManager
    {
        private static NanoBoxManager instance;
        private static readonly object locker = new object();
        private readonly IDictionary<Type, Type> Definitions;

        private ConstructorInfo[] GetConstructors<TDefinition>()
        {
            var constructors = GetConstructors(typeof(TDefinition));
            return constructors;
        }

        public TDefinition Get<TDefinition>()
        {
            var constructors = GetConstructors<TDefinition>();
            var result = constructors.First().Invoke(new object[] { });
            return (TDefinition)result;
        }

        private static ConstructorInfo FindConstructorMatchingArguments(object[] constructorArguments, ConstructorInfo[] constructors)
        {
            return constructors.FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();

                    if (parameters.Count() != constructorArguments.Length)
                        return false;

                    for (var parameterId = 0; parameterId <= constructorArguments.Length - 1 && parameterId <= parameters.Count() - 1; parameterId++)
                    {
                        if (parameters[parameterId].ParameterType != constructorArguments[parameterId].GetType())
                        {
                            return false;
                        }
                    }

                    return true;
                });
        }

        private ConstructorInfo FindConstructorBasedOnDependencies(ConstructorInfo[] constructors)
        {
            var match = constructors.FirstOrDefault(c =>
                    c.GetParameters().All(p => Definitions.ContainsKey(p.ParameterType))
                 );

            return match;
        }

        private object[] FindParametersForConstructor(ConstructorInfo matchingConstructor)
        {
            var parameters = matchingConstructor.GetParameters().Select(p => Get(p.ParameterType, new object[0])).ToArray();
            return parameters;
        }

        public ConstructorInfo[] GetConstructors(Type T)
        {
            var match = Definitions.FirstOrDefault(d => d.Key == T);
            if (match.Equals(default(KeyValuePair<Type, Type>)))
                throw new NanoBoxItemNotRegisteredException("NanoBox has not been set what to do with a type " + T.ToString());

            var typeToCreate = match.Value;
            var constructors = typeToCreate.GetConstructors();
            if (!constructors.Any())
                throw new NanoBoxConstructorNotFoundException(string.Format("A constructor could not be found for type {0}.", T.ToString()));

            return constructors;
        }

        public object Get(Type resolvableType, object[] constructorArguments)
        {
            var mutableArguments = constructorArguments.ToList().ToArray();
            var constructors = GetConstructors(resolvableType);
            ConstructorInfo matchingConstructor = null;
            if (constructorArguments.Any())
            {
                matchingConstructor = FindConstructorMatchingArguments(mutableArguments, constructors);
                if (matchingConstructor == null)
                    throw new NanoBoxConstructorNotFoundException(string.Format("A constructor could not be found for type {0} with parameters: {1}", resolvableType, string.Join(",", constructorArguments.Select(c => c.GetType().ToString()))));
            }
                
            if (matchingConstructor == null)
            {
                matchingConstructor = FindConstructorBasedOnDependencies(constructors);
                if (matchingConstructor == null)
                    throw new NanoBoxConstructorNotFoundException(string.Format("A constructor could not be found for type {0} with parameters: {1}", resolvableType, string.Join(",", constructorArguments.Select(c => c.GetType().ToString()))));

                mutableArguments = FindParametersForConstructor(matchingConstructor);
            }

            var instance = matchingConstructor.Invoke(mutableArguments);
            return instance;
        }

        public TDefinition Get<TDefinition>(object[] constructorArguments)
        {
            var instance = Get(typeof(TDefinition), constructorArguments);
            return (TDefinition)instance;
        }

        public void Remove<TDefinition, TImplementer>()
        {
            var match = Definitions.FirstOrDefault(d => d.Key == typeof(TDefinition) && d.Value == typeof(TImplementer));
            if (match.Equals(default(KeyValuePair<Type, Type>)))
                return;

            Definitions.Remove(match);
        }

        public void Set<TDefinition, TImplementation>()
        {
            if (Definitions.ContainsKey(typeof(TDefinition)))
                Definitions[typeof(TDefinition)] = typeof(TImplementation);
            else
                Definitions.Add(typeof(TDefinition), typeof(TImplementation));
        }

        private NanoBoxManager()
        {
            Definitions = new Dictionary<Type, Type>();
        }

        internal static NanoBoxManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        instance = new NanoBoxManager();
                    }
                }

                return instance;
            }
        }
    }
}

