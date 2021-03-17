using System;
using System.Threading.Tasks;
using Xunit;

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