using System;
using CommandLine;

namespace HybridCrypting.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<EncryptOptions, DecryptOptions>(args)
                .MapResult(
                    (EncryptOptions opts) => HandleEncrypt(opts),
                    (DecryptOptions opts) => HandleDecrypt(opts),
                    errs => 1);
        }

        static int HandleEncrypt(EncryptOptions o)
        {
            var cipher = HybridCryptor.Encrypt(o.PublicKeyPath, o.Text);
            Console.WriteLine("----------BEGIN CIPHER----------");
            Console.WriteLine(cipher);
            Console.WriteLine("----------END CHIPHER----------");
            return 0;
        }

        static int HandleDecrypt(DecryptOptions o)
        {
            var text = HybridCryptor.Decrypt(o.PrivateKeyPath, o.Cipher);
            Console.WriteLine("----------BEGIN TEXT----------");
            Console.WriteLine(text);
            Console.WriteLine("----------END TEXT----------");
            return 0;
        }
    }
}
