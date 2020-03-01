using System;

namespace TheSystem.Code
{
    public class AnotherQuery : IQuery<int>
    {
        private AnotherQuery(int age)
        { }

        public static Func<object, AnotherQuery> CreateInstance => age => new AnotherQuery((int)age);
    }
}