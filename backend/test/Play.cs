using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public sealed class Play
    {
        
        [Test]
        public async Task Do()
        {
            // arrange
            var actual = true;           
            
            // act

            // assert
            Assert.IsTrue(actual, "actual should be true.");

        }
    }
}