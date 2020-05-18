using System;
using CommandLine;

namespace HybridCrypting.Cli
{
    [Verb("decrypt", HelpText = "Decrypt the cipher")]
    public class DecryptOptions
    {
        [Option(HelpText = "Private key file path")]
        public string PrivateKeyPath { get; set; }

        [Value(0, HelpText = "Cipher to be decrypted")]
        public string Cipher { get; set; }
    }
}
