using System;
using Metabase.GraphQl.Databases;

namespace import
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("About to import data from LBNL!");
            var resolvers = new DatabaseResolvers(
                new HttpClientFactory(),
                new Logger<DatabaseResolvers>()
            );
        }
    }
}
