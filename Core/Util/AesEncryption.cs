using Core.ErrorHandling;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Core.Util
{
    public class AesEncryption
    {
        public byte[] Key { get; private set; }
        public byte[] IV { get; private set; }

        public AesEncryption()
        {
            using Aes aes = Aes.Create();
            Key = aes.Key;
            IV = aes.IV;
        }

        public string AesEncrypt(string plainText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Padding = PaddingMode.Zeros;
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new MemoryStream();
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter swEncrypt = new StreamWriter(csEncrypt);
                swEncrypt.Write(plainText);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public string AesDecrypt(byte[] cipherText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Padding = PaddingMode.Zeros;
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new MemoryStream(cipherText);
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);

            string plaintext;
            try
            {
                plaintext = srDecrypt.ReadToEnd();
            }
            catch (IOException)
            {
                throw new UnauthorizedException();
            }
            return plaintext;
        }
    }
}
