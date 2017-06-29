using System;
using System.Globalization;
using System.Web;
using System.Diagnostics;

namespace Wmb.Web {
    /// <summary>
    /// The HttpRequestUtility class holds the extensions and/or helpermethods for the HttpRequest class.
    /// </summary>
    public static class HttpRequestUtility {
        /// <summary>
        /// Determines whether the specified HttpRequest is leeched.
        /// </summary>
        /// <param name="httpRequest">The HttpRequest.</param>
        /// <returns>
        /// 	<c>true</c> if the specified HttpRequest is leeched; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLeeched(this HttpRequest httpRequest) {
            if (httpRequest == null) {
                throw new ArgumentNullException("httpRequest");
            }

            bool retVal = true;

            if (httpRequest.UrlReferrer != null && httpRequest.UrlReferrer.Host.Length > 0) {
                string referrerHost = httpRequest.UrlReferrer.Host;
                string requestHost = httpRequest.Url.Host;
                if (referrerHost.Equals(requestHost)) {
                    retVal = false;
                }
                else {
                    Trace.TraceWarning("HttpRequestUtility: The request seems to be leeched.\nReferrer = '{0}' != '{1}' = Reqeust",
                                        referrerHost, requestHost);
                }
            }

            return retVal;
        }


        /// <summary>
        /// Gets a query string value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest">The HttpRequest.</param>
        /// <param name="key">The key of the value.</param>
        /// <param name="defaultValue">The default value if none found.</param>
        /// <param name="decrypt">if set to <c>true</c> the value gets decrypted.</param>
        /// <param name="password">The password used for decryption.</param>
        /// <param name="salt">The salt used for decryption.</param>
        /// <returns>The value from the query string</returns>
        public static T GetQueryStringValue<T>(this HttpRequest httpRequest, string key, T defaultValue, bool decrypt, string password, string salt) {
            if (httpRequest == null) {
                throw new ArgumentNullException("httpRequest");
            }

            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("key");
            }

            if (decrypt) {
                if (string.IsNullOrEmpty(password)) {
                    throw new ArgumentNullException("password");
                }

                if (string.IsNullOrEmpty(salt)) {
                    throw new ArgumentNullException("salt");
                }
            }

            T retVal = defaultValue;

            string queryStringValue = httpRequest.QueryString[key];
            if (!string.IsNullOrEmpty(queryStringValue)) {
                if (decrypt) {
                    queryStringValue = queryStringValue.UrlEncodedPasswordDecrypt(password, salt);
                }

                retVal = (T)Convert.ChangeType(queryStringValue,
                                               typeof(T),
                                               CultureInfo.InvariantCulture);
            }

            return retVal;
        }

        /// <summary>
        /// Gets a query string value without decrypting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest">The HttpRequest.</param>
        /// <param name="key">The key of the value.</param>
        /// <param name="defaultValue">The default value if none found.</param>
        /// <returns>The value from the query string</returns>
        public static T GetQueryStringValue<T>(this HttpRequest httpRequest, string key, T defaultValue) {
            if (httpRequest == null) {
                throw new ArgumentNullException("httpRequest");
            }

            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("key");
            }

            return httpRequest.GetQueryStringValue<T>(key, defaultValue, false, null, null);
        }

        /// <summary>
        /// Determines whether this request is valid
        /// </summary>
        /// <param name="httpRequest">The HttpRequest.</param>
        /// <param name="salt">The salt that is added before the hash is calculated.</param>
        /// <param name="leechProtect">if set to <c>true</c> it protects you from leechers.</param>
        /// <returns>
        /// 	<c>true</c> if [is request valid] [the specified HTTP request]; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsValid(this HttpRequest httpRequest, string salt, bool leechProtect) {
            if (httpRequest == null) {
                throw new ArgumentNullException("httpRequest");
            }

            if (string.IsNullOrEmpty(salt)) {
                throw new ArgumentNullException("salt");
            }

            bool retVal = httpRequest.QueryString.IsValid(salt);

            if (retVal){
                if (leechProtect && httpRequest.IsLeeched()) {
                    retVal = false;
                }
            }

            return retVal;
        }
    }
}
