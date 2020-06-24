using System;
using Xunit;
using System.Threading.Tasks;

namespace Test
{
    public sealed class Play
    {
        [Fact]
        public async Task Do()
        {
            Console.WriteLine("Do play!");
        }
    }
}