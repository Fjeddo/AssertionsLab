using System;

namespace TheSystem.Code
{
    public class SomeQuery : IQuery<int>
    {
        private SomeQuery(int age)
        {
        }

        public static Func<object, SomeQuery> CreateInstance => age => new SomeQuery((int) age);
    }
}