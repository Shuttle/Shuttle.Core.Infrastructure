using System;
using System.IO;

namespace Shuttle.Core.Infrastructure
{
    public static class StreamExtensions
    {
        /// <summary>
        ///     Creates an array of bytes from the given stream.  The stream position is not reset once the operation has
        ///     completed.
        /// </summary>
        /// <param name="stream">Input stream</param>
        /// <returns>An array of bytes</returns>
        public static byte[] ToBytesOnly(this Stream stream)
        {
            Guard.AgainstNull(stream, "stream");

            var readBuffer = new byte[4096];

            var totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead != readBuffer.Length)
                {
                    continue;
                }

                var nextByte = stream.ReadByte();

                if (nextByte == -1)
                {
                    continue;
                }

                var temp = new byte[readBuffer.Length*2];

                Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                Buffer.SetByte(temp, totalBytesRead, (byte) nextByte);

                readBuffer = temp;

                totalBytesRead++;
            }

            var buffer = readBuffer;

            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];

                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }

            return buffer;
        }

        /// <summary>
        ///     Creates an array of bytes from the given stream.  The stream position is reset once the operation has completed.
        /// </summary>
        /// <param name="stream">Input stream</param>
        /// <returns>An array of bytes</returns>
        public static byte[] ToBytes(this Stream stream)
        {
            Guard.AgainstNull(stream, "stream");

            var originalPosition = stream.Position;

            try
            {
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format(InfrastructureResources.StreamCannotSeek, "StreamExtensions.ToBytes"), ex);
            }

            try
            {
                return stream.ToBytesOnly();
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }

        public static Stream Copy(this Stream stream)
        {
            var result = new MemoryStream();
            if (!stream.CanSeek)
            {
                throw new InvalidOperationException(
                    string.Format(InfrastructureResources.StreamCannotSeek, "StreamExtensions.Copy"));
            }

            result.Capacity = (int) stream.Length;

            var originalPosition = stream.Position;

            try
            {
                stream.Seek(0, SeekOrigin.Begin);

                stream.CopyTo(result);

                result.Seek(0, SeekOrigin.Begin);
            }
            finally
            {
                stream.Seek(originalPosition, SeekOrigin.Begin);
            }

            return result;
        }
    }
}