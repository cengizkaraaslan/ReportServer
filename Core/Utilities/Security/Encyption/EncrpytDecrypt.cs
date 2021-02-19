using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Core.Utilities.Security.Encyption
{
    public class EncrpytDecrypt
    {
        private const String initVector = "TedasBaseKeyUret";

        private const int keySize = 256;
        public static string EncryptString(string text, string keyString)
        {
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(text);
                PasswordDeriveBytes password = new PasswordDeriveBytes(keyString, null);
                byte[] keyBytes = password.GetBytes(keySize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] Encrypted = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                return Convert.ToBase64String(Encrypted);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static string DecryptString(string cipherText, string keyString)
        {
            try
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] DeEncryptedText = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(keyString, null);
                byte[] keyBytes = password.GetBytes(keySize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(DeEncryptedText);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[DeEncryptedText.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
