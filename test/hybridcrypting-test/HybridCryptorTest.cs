using System;
using Xunit;
using HybridCrypting;

namespace HybridCrypting.Test
{
    public class HybridCryptorTest
    {
        [Fact]
        public void TestEncryptAndDecrypt()
        {
            var publicKeyPath = "keys/public_key.pem";
            var privateKeyPath = "keys/private_key.pem";
            var text = "Hello World!";
            var cipher = HybridCryptor.Encrypt(publicKeyPath, text);
            var decrypted = HybridCryptor.Decrypt(privateKeyPath, cipher);
            Assert.Equal(text, decrypted);
        }
    }
}
