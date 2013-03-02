using System;
using NUnit.Framework;
using Retlang.Fibers;
using System.Threading;

namespace Retlang.Test.Fibers
{
    [TestFixture]
    public class PoolFiberFixture : MoqTestFixture
    {
        private PoolFiber _fiber;

        protected override void SetUp()
        {
            _fiber = new PoolFiber();
            _fiber.Start();
        }

        [Test]
        public void Start_CalledTwice_Exception()
        {
            Assert.Throws<ThreadStateException>(_fiber.Start);
        }
    }
}

