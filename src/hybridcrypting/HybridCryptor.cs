using System;
using System.Collections.Generic;
using System.Text;

namespace HybridCrypting
{
    public static class HybridCryptor
    {
        private const char Seperator = '_';

        public static string Encrypt(string publicKeyPath, string text)
        {
            var key = Aes256Cryptor.GetNewKey();
            var encryptedKey = RsaCryptorFactory.GetPublicKeyCryptor(publicKeyPath)
                .Encrypt(key, false);
            var encryptedText = Aes256Cryptor.Encrypt(key, text);

            return Combine(Convert.ToBase64String(encryptedKey), Convert.ToBase64String(encryptedText));
        }

        public static string Decrypt(string privateKeyPath, string cipher)
        {
            var (encryptedKey, encryptedText) = Split(cipher);
            var key = RsaCryptorFactory.GetPrivateKeyCryptor(privateKeyPath)
                .Decrypt(Convert.FromBase64String(encryptedKey), false);
            return Aes256Cryptor.Decrypt(key, Convert.FromBase64String(encryptedText));
        }

        private static string Combine(string encryptedKey, string encryptedText)
        {
            return $"{encryptedKey}{Seperator}{encryptedText}";
        }

        private static (string encryptedKey, string encryptedText) Split(string cipher)
        {
            var sections = cipher?.Split(Seperator);
            if (sections == null || sections.Length != 2)
                throw new ArgumentException($"{nameof(cipher)} is invalid.", nameof(cipher));

            return (sections[0], sections[1]);
        }
    }
}
