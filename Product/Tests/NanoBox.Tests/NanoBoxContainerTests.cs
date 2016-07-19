using FluentXUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NanoBox;



namespace NanoBoxTests
{
    public class NanoBoxContainerTests
    {
        [Fact]
        public void SetResponseForType()
        {
            IoC.Set<ISampleInterface, SampleInterfaceImplementer>();
            IoC.Remove<ISampleInterface, SampleInterfaceImplementer>();
        }

        [Fact]
        public void RemoveRegisteredType()
        {
            IoC.Set<ISampleInterface, SampleInterfaceImplementer>();
            IoC.Remove<ISampleInterface, SampleInterfaceImplementer>();
            XAssert.That(() => IoC.Get<ISampleInterface>(), Is.ErroringWith<NanoBoxItemNotRegisteredException>());
        }

        [Fact]
        public void GetCorrectType()
        {
            IoC.Set<ISampleInterface, SampleInterfaceImplementer>();
            var result = IoC.Get<ISampleInterface>();
            IoC.Remove<ISampleInterface, SampleInterfaceImplementer>();
            XAssert.That(result, IsNot.Null());
            XAssert.That(result.GetType(), Is.EqualTo(typeof(SampleInterfaceImplementer)));
            XAssert.That(result.Success(), Is.True());
        }

        [Fact]
        public void UseCorrectConstructor()
        {
            IoC.Set<ISampleInterface, SampleInterfaceImplementer>();
            var result = IoC.Get<ISampleInterface>(new object[] { false });
            IoC.Remove<ISampleInterface, SampleInterfaceImplementer>();
            XAssert.That(result.Success(), Is.False());
        }

        [Fact]
        public void ErrorOnNotFoundConstructor()
        {
            IoC.Set<ISampleInterface, SampleInterfaceImplementer>();
            XAssert.That(()=> 
                IoC.Get<ISampleInterface>(new object[] { "test" }), Is.ErroringWith<NanoBoxConstructorNotFoundException>());
        }

        [Fact]
        public void ResolvesConstructorArguments()
        {
            IoC.Set<ISampleInterface, SampleInterfaceImplementer>();
            IoC.Set<IDependencyImplementer, SampleDependency>();
            var result = IoC.Get<IDependencyImplementer>();
            XAssert.That(result, IsNot.Null());
            XAssert.That(result.Answer(), Is.True());
        }
    }
}
