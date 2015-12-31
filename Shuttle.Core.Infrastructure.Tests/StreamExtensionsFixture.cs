using System.IO;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
    [TestFixture]
    public class StreamExtensionsFixture
    {
        [Test]
        public void Should_be_able_to_convert_a_stream_to_an_array_of_bytes()
        {
            var stream = new MemoryStream(new byte[] {0, 1, 2, 3, 4});
            var bytes = stream.ToBytes();

            Assert.AreEqual(5, bytes.Length);
            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(4, bytes[4]);
        }

        [Test]
        public void Should_be_able_to_make_a_copy_of_a_stream()
        {
            var bytes = new byte[] {0, 1, 2, 3, 4};
            var stream = new MemoryStream(bytes);
            var output = new MemoryStream();

            stream.CopyTo(output);

            Assert.AreEqual(5, output.Length);
            Assert.AreEqual(5, output.Position);
            Assert.AreEqual(5, stream.Position);

            var copy = stream.Copy();

            Assert.AreEqual(5, copy.Length);
            Assert.AreEqual(0, copy.Position);
            Assert.AreEqual(0, copy.Position);
        }
    }
}