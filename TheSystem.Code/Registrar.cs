using System;
using System.Collections.Generic;

namespace TheSystem.Code
{
    public class Registrar : IRegistrar
    {
        private Dictionary<Type, Func<object, object>> _factories = new Dictionary<Type, Func<object, object>>();

        public void Register<T>(Func<object, T> factory) where T : class
        {
            _factories.Add(typeof(T), factory);
        }

        public Func<object, T> ResolveFactory<T>()
        {
            return _factories[typeof(T)] as Func<object, T>;
        }
    }
}