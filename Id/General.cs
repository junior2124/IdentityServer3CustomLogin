using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace Id
{
    public class General
    {
        /// <summary>
        /// AES encrypts the plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>String value.</returns>
        public static string AesEncrypt(string plainText, string salt, string aesPassword)
        {
            int passwordIterations = 2;
            string initialVector = "ISSAESEncryption";
            //This should be a string of 16 ASCII characters.
            int keySize = 256;
            //Can be 128, 192, or 256.

            if ((string.IsNullOrEmpty(plainText)))
            {
                return "";
            }

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            Rfc2898DeriveBytes rfc2898DerivedPassword = new Rfc2898DeriveBytes(aesPassword, saltValueBytes, passwordIterations);
            byte[] keyBytes = rfc2898DerivedPassword.GetBytes(keySize / 8);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform iCryptoTransform = rijndaelManaged.CreateEncryptor(keyBytes, initialVectorBytes))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                        memoryStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            rijndaelManaged.Clear();

            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// AES decrypts the cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>String value.</returns>
        public static string AesDecrypt(string cipherText, string salt, string aesPassword)
        {
            //Dim hashAlgorithm As String = "SHA1"
            int passwordIterations = 2;
            string initialVector = "ISSAESEncryption";
            int keySize = 256;

            if ((string.IsNullOrEmpty(cipherText)))
            {
                return "";

            }

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            Rfc2898DeriveBytes rfc2898DerivedPassword = new Rfc2898DeriveBytes(aesPassword, saltValueBytes, passwordIterations);
            byte[] keyBytes = rfc2898DerivedPassword.GetBytes(keySize / 8);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;

            using (ICryptoTransform iCryptoTransform = rijndaelManaged.CreateDecryptor(keyBytes, initialVectorBytes))
            {
                using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        memoryStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            rijndaelManaged.Clear();

            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }
    }
}