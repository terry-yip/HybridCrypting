using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HybridCrypting
{
    public static class Aes256Cryptor
    {
        public static byte[] GetNewKey()
        {
            using (var aes = Aes.Create())
            {
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                return aes.Key;
            }
        }

        public static byte[] Encrypt(byte[] key, string text)
        {
            using (var aes = Aes.Create())
            {
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                var iv = aes.IV;
                using (var encryptor = aes.CreateEncryptor(key, aes.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text);
                            }
                            var encrypted = msEncrypt.ToArray();

                            var result = new byte[iv.Length + encrypted.Length];
                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

                            return result;
                        }
                    }
                }
            }
        }

        public static string Decrypt(byte[] key, byte[] fullCipher)
        {
            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using (var aes = Aes.Create())
            {
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;
                using (var decryptor = aes.CreateDecryptor(key, aes.IV))
                {
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                var result = srDecrypt.ReadToEnd();
                                return result;
                            }
                        }
                    }
                }
            }
        }
    }
}
