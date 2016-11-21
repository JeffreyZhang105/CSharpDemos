using demos.Asynchronism;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demos.Tests
{
    [TestFixture]
    public class AsynchronismTest
    {
        [Test]
        public void TestAsyncTest()
        {
            var target = new AsyncAndAwait();
            target.TestAsync();
            while (true)
            {

            }
        }
    }
}
