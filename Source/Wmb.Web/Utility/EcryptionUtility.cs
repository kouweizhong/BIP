using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Wmb.Web {
    /// <summary>
    /// The StringUtility class holds the extensions and/or helpermethods for the String class.
    /// </summary>
    public static class EcryptionUtility {
        /// <summary>
        /// Encrypts the value by password and salt.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The encrypted bytes</returns>
        public static byte[] PasswordEncrypt(this byte[] value, string password, string salt) {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(password)) {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrEmpty(salt)) {
                throw new ArgumentNullException("salt");
            }

            byte[] retVal = null;
            Rijndael rijndaelAlg = CreateRijndael(password, salt);

            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                               rijndaelAlg.CreateEncryptor(),
                                                               CryptoStreamMode.Write)) {
                cryptoStream.Write(value, 0, value.Length);
                cryptoStream.Close();
                retVal = memoryStream.ToArray();
            }

            return retVal;
        }


        /// <summary>
        /// Decrypts the value by password and salt.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The decrypted bytes</returns>
        public static byte[] PasswordDecrypt(this byte[] value, string password, string salt) {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(password)) {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrEmpty(salt)) {
                throw new ArgumentNullException("salt");
            }

            byte[] retVal = null;
            Rijndael rijndaelAlg = CreateRijndael(password, salt);

            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                               rijndaelAlg.CreateDecryptor(),
                                                               CryptoStreamMode.Write)) {
                cryptoStream.Write(value, 0, value.Length);
                cryptoStream.Close();
                retVal = memoryStream.ToArray();
            }

            return retVal;
        }

        /// <summary>
        /// Ecrypts the value to a url encoded string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The encrypted and url encoded string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification="This method does not return a Uri.")]
        public static string UrlEncodedPasswordEncrypt(this string value, string password, string salt) {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(password)) {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrEmpty(salt)) {
                throw new ArgumentNullException("salt");
            }

            string retVal = null;

            byte[] bytesToEncrypt = Encoding.Unicode.GetBytes(value);
            byte[] encryptedValue = bytesToEncrypt.PasswordEncrypt(password, salt);
            retVal = HttpServerUtility.UrlTokenEncode(encryptedValue);

            return retVal;
        }

        /// <summary>
        /// Decrypts the url encoded value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The decrypted and url decoded string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification="This method does not return a Uri.")]
        public static string UrlEncodedPasswordDecrypt(this string value, string password, string salt) {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(password)) {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrEmpty(salt)) {
                throw new ArgumentNullException("salt");
            }

            string retVal = null;

            byte[] bytesToDecrypt = HttpServerUtility.UrlTokenDecode(value);
            byte[] decryptedValue = bytesToDecrypt.PasswordDecrypt(password, salt);
            retVal = Encoding.Unicode.GetString(decryptedValue);

            return retVal;
        }

        private static Rijndael CreateRijndael(string password, string salt) {
            byte[] saltBytes = Encoding.Unicode.GetBytes(salt);

            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(password,
                                                                              saltBytes);

            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = passwordDeriveBytes.GetBytes(32);
            rijndael.IV = passwordDeriveBytes.GetBytes(16);

            return rijndael;
        }
    }
}
