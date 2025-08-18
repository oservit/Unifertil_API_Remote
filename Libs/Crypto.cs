using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Libs
{
    public static class Crypto
    {
        private const string KeySeed = "T9v!xK2$zQw7#LcM";

        public static string Encrypt(string plainText)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(KeySeed);

                aes.GenerateIV();

                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream();

                ms.Write(aes.IV, 0, aes.IV.Length);

                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }

            catch
            {
                throw;
            }
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                var fullCipher = Convert.FromBase64String(encryptedText);

                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(KeySeed);

                var iv = new byte[16];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                aes.IV = iv;

                var cipherText = new byte[fullCipher.Length - iv.Length];
                Array.Copy(fullCipher, iv.Length, cipherText, 0, cipherText.Length);

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(cipherText);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }

            catch(Exception ex)
            {
                throw new Exception($"Erro ao descriptografar senha: {ex.Message}");
            }
        }
    }
}
