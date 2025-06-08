using System.Security.Cryptography;
using System.Text;

namespace Ng.Pass.Server.Services.Encryption.Services;

public class EncryptionService : IEncryptionService
{
    private const int KeySize = 32; // 256 bits
    private const int NonceSize = 12; // 96 bits (recommended for GCM)
    private const int TagSize = 16; // 128 bits
    private const int SaltSize = 16; // 128 bits
    private const int Iterations = 100000; // PBKDF2 iterations

    /// <summary>
    /// Encrypts plaintext using AES-256-GCM with a passphrase
    /// </summary>
    /// <param name="plaintext">The text to encrypt</param>
    /// <param name="passphrase">The passphrase used for encryption</param>
    /// <returns>Base64 encoded encrypted data (salt + nonce + ciphertext + tag)</returns>
    public string Encrypt(string plaintext, string passphrase)
    {
        if (string.IsNullOrEmpty(plaintext))
            throw new ArgumentException("Plaintext cannot be null or empty", nameof(plaintext));

        if (string.IsNullOrEmpty(passphrase))
            throw new ArgumentException("Passphrase cannot be null or empty", nameof(passphrase));

        // Generate random salt and nonce
        byte[] salt = new byte[SaltSize];
        byte[] nonce = new byte[NonceSize];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
            rng.GetBytes(nonce);
        }

        // Derive key from passphrase using PBKDF2
        byte[] key = DeriveKey(passphrase, salt);

        // Convert plaintext to bytes
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

        // Encrypt using AES-GCM
        byte[] ciphertext = new byte[plaintextBytes.Length];
        byte[] tag = new byte[TagSize];

        using (var aes = new AesGcm(key.AsSpan()))
        {
            aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);
        }

        // Combine all components: salt + nonce + ciphertext + tag
        byte[] result = new byte[SaltSize + NonceSize + ciphertext.Length + TagSize];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(nonce, 0, result, SaltSize, NonceSize);
        Buffer.BlockCopy(ciphertext, 0, result, SaltSize + NonceSize, ciphertext.Length);
        Buffer.BlockCopy(tag, 0, result, SaltSize + NonceSize + ciphertext.Length, TagSize);

        // Clear sensitive data
        Array.Clear(key, 0, key.Length);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Decrypts encrypted data using AES-256-GCM with a passphrase
    /// </summary>
    /// <param name="encryptedData">Base64 encoded encrypted data</param>
    /// <param name="passphrase">The passphrase used for decryption</param>
    /// <returns>The decrypted plaintext</returns>
    public string Decrypt(string encryptedData, string passphrase)
    {
        if (string.IsNullOrEmpty(encryptedData))
            throw new ArgumentException("Encrypted data cannot be null or empty", nameof(encryptedData));

        if (string.IsNullOrEmpty(passphrase))
            throw new ArgumentException("Passphrase cannot be null or empty", nameof(passphrase));

        try
        {
            // Decode from Base64
            byte[] data = Convert.FromBase64String(encryptedData);

            // Validate minimum length
            if (data.Length < SaltSize + NonceSize + TagSize)
                throw new ArgumentException("Invalid encrypted data format");

            // Extract components
            byte[] salt = new byte[SaltSize];
            byte[] nonce = new byte[NonceSize];
            byte[] ciphertext = new byte[data.Length - SaltSize - NonceSize - TagSize];
            byte[] tag = new byte[TagSize];

            Buffer.BlockCopy(data, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(data, SaltSize, nonce, 0, NonceSize);
            Buffer.BlockCopy(data, SaltSize + NonceSize, ciphertext, 0, ciphertext.Length);
            Buffer.BlockCopy(data, SaltSize + NonceSize + ciphertext.Length, tag, 0, TagSize);

            // Derive key from passphrase using the same salt
            byte[] key = DeriveKey(passphrase, salt);

            // Decrypt using AES-GCM
            byte[] plaintextBytes = new byte[ciphertext.Length];

            using (var aes = new AesGcm(key.AsSpan()))
            {
                aes.Decrypt(nonce, ciphertext, tag, plaintextBytes);
            }

            // Clear sensitive data
            Array.Clear(key, 0, key.Length);

            return Encoding.UTF8.GetString(plaintextBytes);
        }
        catch (CryptographicException)
        {
            throw new UnauthorizedAccessException("Decryption failed. Invalid passphrase or corrupted data.");
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid encrypted data format");
        }
    }

    /// <summary>
    /// Derives a cryptographic key from a passphrase using PBKDF2
    /// </summary>
    private static byte[] DeriveKey(string passphrase, byte[] salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(passphrase, salt, Iterations, HashAlgorithmName.SHA256))
        {
            return pbkdf2.GetBytes(KeySize);
        }
    }
}
