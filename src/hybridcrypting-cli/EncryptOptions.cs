using System;
using CommandLine;

namespace HybridCrypting.Cli
{
    [Verb("encrypt", HelpText = "Encrypt the text")]
    class EncryptOptions
    {
        [Option(HelpText = "Public key file path")]
        public string PublicKeyPath { get; set; }

        [Value(0, HelpText = "Text to be encrypted")]
        public string Text { get; set; }
    }
}