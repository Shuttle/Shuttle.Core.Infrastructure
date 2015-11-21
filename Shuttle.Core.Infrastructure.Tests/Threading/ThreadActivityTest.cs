using System;
using Moq;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
    [TestFixture]
    public class ThreadActivityTest
    {
        [Test]
        public void Should_be_able_to_have_the_thread_wait()
        {
            var activity = new ThreadActivity(new[]
            {
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromMilliseconds(500)
            });

            var start = DateTime.Now;

            var mockState = new Mock<IThreadState>();

            mockState.Setup(mock => mock.Active).Returns(true);

            activity.Waiting(mockState.Object);

            Assert.IsTrue((DateTime.Now - start).TotalMilliseconds >= 250);

            activity.Waiting(mockState.Object);

            Assert.IsTrue((DateTime.Now - start).TotalMilliseconds >= 750);
        }
    }
}