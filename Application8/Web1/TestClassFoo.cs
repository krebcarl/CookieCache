using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Web1
{
    [TestClass]
    public class TestClassFoo
    {
        [TestInitialize]
        public void Setup()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {
            
        }

        [TestMethod]
        public void Test1()
        {
            Assert.IsFalse(true);
            Console.WriteLine("testing");
        }
    }
}
