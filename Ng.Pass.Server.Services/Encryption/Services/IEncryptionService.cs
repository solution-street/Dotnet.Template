namespace Ng.Pass.Server.Services.Encryption.Services;

public interface IEncryptionService
{
    string Encrypt(string plaintext, string passphrase);

    string Decrypt(string encrypted, string passphrase);
}
