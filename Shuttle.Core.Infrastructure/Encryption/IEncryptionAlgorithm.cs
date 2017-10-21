namespace Shuttle.Core.Infrastructure
{
    public interface IEncryptionAlgorithm
    {
        string Name { get; }

        byte[] Encrypt(byte[] bytes);
        byte[] Decrypt(byte[] bytes);
    }
}