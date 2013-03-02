using System;
using NUnit.Framework;
using Retlang.Fibers;
using System.Threading;

namespace Retlang.Test.Fibers
{
    [TestFixture]
    public class ThreadFiberFixture : MoqTestFixture
    {
        private ThreadFiber _fiber;

        protected override void SetUp()
        {
            _fiber = new ThreadFiber();
            _fiber.Start();
        }

        [Test]
        public void Start_CalledTwice_Exception()
        {
            Assert.Throws<ThreadStateException>(_fiber.Start);
        }
    }
}

