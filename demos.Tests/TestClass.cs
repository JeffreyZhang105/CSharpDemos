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
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            var target = new AsyncAndAwait();

            var result = target.TestAsync();

            Assert.Pass();
        }
    }
}
