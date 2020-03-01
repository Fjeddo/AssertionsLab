using System;

namespace TheSystem.Code
{
    public interface IRegistrar
    {
        void Register<T>(Func<object, T> factory) where T : class;
    }
}