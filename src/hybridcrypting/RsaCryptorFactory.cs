using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace HybridCrypting
{
    public static class RsaCryptorFactory
    {
        public static RSACryptoServiceProvider GetPrivateKeyCryptor(string filePath)
        {
            RsaPrivateCrtKeyParameters privateKeyParams = ReadAsymmetricKeyParameter(filePath) as RsaPrivateCrtKeyParameters;

            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            RSAParameters parms = new RSAParameters();

            parms.Modulus = privateKeyParams.Modulus.ToByteArrayUnsigned();
            parms.P = privateKeyParams.P.ToByteArrayUnsigned();
            parms.Q = privateKeyParams.Q.ToByteArrayUnsigned();
            parms.DP = privateKeyParams.DP.ToByteArrayUnsigned();
            parms.DQ = privateKeyParams.DQ.ToByteArrayUnsigned();
            parms.InverseQ = privateKeyParams.QInv.ToByteArrayUnsigned();
            parms.D = privateKeyParams.Exponent.ToByteArrayUnsigned();
            parms.Exponent = privateKeyParams.PublicExponent.ToByteArrayUnsigned();

            cryptoServiceProvider.ImportParameters(parms);

            return cryptoServiceProvider;
        }

        public static RSACryptoServiceProvider GetPublicKeyCryptor(string filePath)
        {
            RsaKeyParameters publicKeyParam = ReadAsymmetricKeyParameter(filePath) as RsaKeyParameters;

            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            RSAParameters parms = new RSAParameters();

            parms.Modulus = publicKeyParam.Modulus.ToByteArrayUnsigned();
            parms.Exponent = publicKeyParam.Exponent.ToByteArrayUnsigned();

            cryptoServiceProvider.ImportParameters(parms);

            return cryptoServiceProvider;
        }

        private static AsymmetricKeyParameter ReadAsymmetricKeyParameter(string filePath)
        {
            using (TextReader textReader = new StringReader(File.ReadAllText(filePath)))
            {
                var obj = new PemReader(textReader).ReadObject();
                switch (obj)
                {
                    case AsymmetricCipherKeyPair p:
                        return p.Private;
                    case AsymmetricKeyParameter p:
                        return p;
                    default:
                        throw new ArgumentException($"{filePath} is not a valid private key.");
                }
            }
        }
    }
}
