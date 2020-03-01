using System;
using NSubstitute;
using TheSystem.Code;
using Xunit;

namespace Tests
{
    public class HostTests
    {
        [Fact]
        public void Register_Received_Call()
        {
            var registrar = Substitute.For<IRegistrar>();
            var host = new Host(registrar);

            host.RegisterAll();
            
            registrar.Received().Register(Arg.Any<Func<object, SomeQuery>>());
        }

        [Fact]
        public void RegisterAll_RegistersAll()
        {
            var registrar = Substitute.For<IRegistrar>();
            var host = new Host(registrar);

            host.RegisterAll();

            registrar.Received().Register(SomeQuery.CreateInstance);
        }
    }
}