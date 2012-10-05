namespace Areas.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            test<CodeFirstTest>();
        }

        static void test<T>()
            where T : ConsoleTestClass
        {
            var obj = typeof(T).Construct().CastTo<T>();

            obj.InitTest();
        }
    }
}
