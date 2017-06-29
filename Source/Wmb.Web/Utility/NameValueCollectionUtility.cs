using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Diagnostics;

namespace Wmb.Web {
    /// <summary>
    /// The NameValueCollectionUtility class holds the extensions and/or helpermethods for the NameValueCollection class.
    /// </summary>
    internal static class NameValueCollectionUtility {
        private const string hashKey = "ha";

        /// <summary>
        /// Determines whether the specified name value collection is valid by checking it's hash.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="salt">The salt that is added before the hash is calculated.</param>
        /// <returns>
        /// 	<c>true</c> if the specified name value collection is valid; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsValid(this NameValueCollection nameValueCollection, string salt) {
            if (nameValueCollection == null) {
                throw new ArgumentNullException("nameValueCollection");
            }

            if (string.IsNullOrEmpty(salt)) {
                throw new ArgumentNullException("salt");
            }

            bool retVal = false;
            string fullQueryString = nameValueCollection.ToQueryString();
            string hash = nameValueCollection.Get(hashKey);
            
            if (!string.IsNullOrEmpty(hash)) {
                Regex hashRegex = new Regex(string.Concat("&", hashKey, "=", hash, "$"));
                string queryString = hashRegex.Replace(fullQueryString, string.Empty);

                string saltedQueryString = String.Concat(queryString, salt);
                string queryStringHashCode = saltedQueryString.GetHashCode().ToString(CultureInfo.InvariantCulture);
                retVal = hash.Equals(queryStringHashCode);
            }

            if (!retVal) {
                Trace.TraceWarning("NameValueCollectionUtility: The querystring: '{0}' seems to be tampered with.", fullQueryString);
            }

            return retVal;
        }

        private static Encoding webEncoding = new UTF8Encoding(false);
        /// <summary>
        /// Converts the NameValueCollection to a query string.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="appendHash">if set to <c>true</c> a hash wil be appended which can be used to verify is the QueryString is valid.</param>
        /// <param name="salt">The salt that is added to the querystring before the hash is calculated.</param>
        /// <returns>
        /// The name value collection as a query string
        /// </returns>
        internal static string ToQueryString(this NameValueCollection nameValueCollection, bool appendHash, string salt) {
            if (nameValueCollection == null) {
                throw new ArgumentNullException("nameValueCollection");
            }

            string retVal = string.Empty;

            if (appendHash) {
                nameValueCollection.Remove(hashKey);
            }

            if (nameValueCollection.HasKeys()) {
                retVal = nameValueCollection.ToQueryString();

                if (appendHash) {
                    string saltedRetVal = String.Concat(retVal, salt);
                    string hashCode = saltedRetVal.GetHashCode().ToString(CultureInfo.InvariantCulture);
                    retVal += string.Concat("&",
                                            HttpUtility.UrlEncode(hashKey, webEncoding),
                                            "=",
                                            HttpUtility.UrlEncode(hashCode, webEncoding));
                }
            }

            return retVal;
        }

        /// <summary>
        /// Converts the NameValueCollection to a query string.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns>The name value collection as a query string</returns>
        internal static string ToQueryString(this NameValueCollection nameValueCollection) {
            if (nameValueCollection == null) {
                throw new ArgumentNullException("nameValueCollection");
            }

            string retVal = null;

            if (nameValueCollection.HasKeys()) {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < nameValueCollection.Count; i++) {
                    string value = nameValueCollection.Get(i);
                    string key = nameValueCollection.GetKey(i);
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value)) {
                        stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&{0}={1}",
                                                   HttpUtility.UrlEncode(key, webEncoding),
                                                   HttpUtility.UrlEncode(value, webEncoding));
                    }
                }

                retVal = stringBuilder.ToString();
            }

            return retVal;
        }

        internal static T Get<T>(this NameValueCollection nameValueCollection, string key, T defaultValue) {
            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("key");
            }

            T retVal = defaultValue;

            object o = nameValueCollection[key];
            if (o != null) {
                if (retVal is Enum) {
                    retVal = (T)Enum.Parse(typeof(T), (string)o);
                } else {
                    retVal = (T)Convert.ChangeType(o,
                                                   typeof(T),
                                                   CultureInfo.InvariantCulture);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Sets the specified key to the given value.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        internal static void Set(this NameValueCollection nameValueCollection, string key, object value) {
            if (nameValueCollection == null) {
                throw new ArgumentNullException("nameValueCollection");
            }

            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("key");
            }

            nameValueCollection.Set(key, Convert.ToString(value, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets the specified key to the given value.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="encrypt">if set to <c>true</c> the value wil be encrypted.</param>
        /// <param name="password">The password used for encryption.</param>
        /// <param name="salt">The salt used for encryption.</param>
        internal static void Set(this NameValueCollection nameValueCollection, string key, string value, bool encrypt, string password, string salt) {
            if (nameValueCollection == null) {
                throw new ArgumentNullException("nameValueCollection");
            }

            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("key");
            }

            if (encrypt) {
                if (string.IsNullOrEmpty(password)) {
                    throw new ArgumentNullException("password");
                }

                if (string.IsNullOrEmpty(salt)) {
                    throw new ArgumentNullException("salt");
                }

                value = value.UrlEncodedPasswordEncrypt(password, salt);
            }

            nameValueCollection.Set(key, value);
        }
    }
}
