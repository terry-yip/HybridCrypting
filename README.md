# HybridCrypting

To encrypt/decrypt the configuration you don't want to commit to source control as plain text.

[![Actions Status](https://github.com/terry-yip/HybridCrypting/workflows/DotNetCore/badge.svg)](https://github.com/terry-yip/HybridCrypting/actions)

## Getting Started

1. Prepare RSA private/public keys. 
   You could generate one if you don't have. e.g.
   $ openssl genpkey -algorithm RSA -out private_key.pem -pkeyopt rsa_keygen_bits:2048
   $ openssl rsa -pubout -in private_key.pem -out public_key.pem
2. Encrypt the text with public key. 
   You could do it with following code. Pass in your public key path and the text your would like to encrypt (i.e your connection string), the result cipher is the encrypted string of your plain text.
   ```
   var cipher = HybridCryptor.Encrypt(publicKeyPath, text);
   ```
   Or you could use the hybridcrypting-cli to encrypt if you don't want to create your own console project with above code to encrypt the plain text. (cd the hybridcrypting-cli folder first) e.g.
   hybridcrypting/src/hybridcrypting-cli$ dotnet run encrypt --publickeypath="../../test/hybridcrypting-test/keys/public_key.pem" "Server=ServerName;Database=DataBaseName;UID=username;Password=password"
3. Then you could put the cipher to your config file and commit to source control as no one can read it, only except your application deployed on production environment with private key.
4. Add reference of hybridcrypting.dll to your application project.
5. Decrypt the cipher with private key in your application when you need the plain text. Pass in your private key path and cipher, the decrypted is the plain text of your connection string.
   ```
   var decrypted = HybridCryptor.Decrypt(privateKeyPath, cipher);
   ```

## Overview

The idea of HybridCrypting is inspired by TLS/SSL handshake. Use asymmetric way to encrypt/decrypt the key cipher and symmetric way to encrypt/decrypt the real content. The benifit is to keep the process both secure(only public key needed for encrypt and the key is totally new generated every time) and efficiant(the key length is only 256, and the real long content is encrypted with symmetric way).

Steps of encrypting
1. Generate a totally new key (length 256) for encrypting the real content later.
2. Use asymmetric encrypting (public key encrypting) to encrypt the key and get a key cipher.
3. Use the key to encrypt the real content with symmetric encrypting.
4. Combine the key cipher and the encrypted content together and output the encrypted bytes

Steps of decrypting
1. Split the encrypted bytes, get the key cipher and encrypted content.
2. Use asymmetric decrypting (private key decrypting) to decrypt the cipher and get the key.
3. Use the key to decrypt the encrypted content with symmetric decrypting. Then you get the real content back.

### Prerequisites

- Dotnet Core Framework

### Installing

Install-Package HybridCrypting

## Built With

* [Dotnet](https://github.com/dotnet/core) - Dotnet Core Framework
* [Nuget](https://nuget.org) - Package Management
* [Portable.BouncyCastle](https://github.com/novotnyllc/bc-csharp) - BouncyCastle portable version

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/terry-yip/HybridCrypting/tags). 

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

