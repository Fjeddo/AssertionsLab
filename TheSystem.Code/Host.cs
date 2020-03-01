using System;

namespace TheSystem.Code
{
    public class Host
    {
        private readonly IRegistrar _registrar;

        public Host(IRegistrar registrar)
        {
            _registrar = registrar;
        }

        public void RegisterAll()
        {
            _registrar.Register(SomeQuery.CreateInstance);
            _registrar.Register(AnotherQuery.CreateInstance);
        }
    }
}