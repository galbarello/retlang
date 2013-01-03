using System;
using System.Collections.Generic;
using NUnit.Framework;
using Retlang.Channels;

namespace Retlang.Test.Channels
{
    [TestFixture]
    public class BatchReceiverFixture : BaseReceiverFixture<IList<int>>
    {
        protected override void SetUp()
        {
            base.SetUp();

            _receiver = new BatchReceiver<int>(_fiber, Receive, 50);
        }

        [Test]
        public void Receive_MultipleMessages_ReceivesOneBatch()
        {
            for (var x = 0; x < 5; x++)
            {
                _receiver.Receive(x);
            }

            var signaled = _handle.WaitOne(1000);
            Assert.IsTrue(signaled);
            
            Assert.AreEqual(1, _received.Count);
            Assert.AreEqual(new int[] { 0, 1, 2, 3, 4 }, _received[0]);
        }
    }
}

