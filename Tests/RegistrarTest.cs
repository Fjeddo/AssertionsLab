using TheSystem.Code;
using Xunit;

namespace Tests
{
    public class RegistrarTest
    {
        [Fact]
        public void Registrar_should_resolve_factory()
        {
            var registrar = new Registrar();
            registrar.Register(SomeQuery.CreateInstance);

            var factory = registrar.ResolveFactory<SomeQuery>();

            var result = factory(123);

            Assert.IsType<SomeQuery>(result);
        }
    }
}