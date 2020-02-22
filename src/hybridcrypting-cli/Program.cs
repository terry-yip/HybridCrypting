using System;
using CommandLine;

namespace HybridCrypting.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<EncryptOptions, object>(args)
                .MapResult(
                    (EncryptOptions opts) => HandleEncrypt(opts),
                    _ => 1);
        }
        static int HandleEncrypt(EncryptOptions o)
        {
            var cipher = HybridCryptor.Encrypt(o.PublicKeyPath, o.Text);
            Console.WriteLine("----------BEGIN CIPHER----------");
            Console.WriteLine(cipher);
            Console.WriteLine("----------END CHIPHER----------");
            return 0;
        }
    }
}
