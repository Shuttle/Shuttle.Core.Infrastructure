namespace Shuttle.Core.Infrastructure
{
    public class NullEncryptionAlgorithm : IEncryptionAlgorithm
    {
        public string Name => "null";

        public byte[] Encrypt(byte[] bytes)
        {
            return bytes;
        }

        public byte[] Decrypt(byte[] bytes)
        {
            return bytes;
        }
    }
}