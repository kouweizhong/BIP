using System;
using System.Globalization;
using System.Web.UI;

namespace Wmb.Web {
    /// <summary>
    /// The StateBagUtility class holds the extensions and/or helpermethods for the StateBag class.
    /// </summary>
    public static class StateBagUtility {
        /// <summary>
        /// Gets the view state value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stateBag">The state bag.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value from the state bag</returns>
        public static T GetValue<T>(this StateBag stateBag, string key, T defaultValue) {
            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("key");
            }

            T retVal = defaultValue;

            object o = stateBag[key];
            if (o != null) {
                retVal = (T)o;
            }

            return retVal;
        }

        /// <summary>
        /// Sets a key in the state bag to the specified value.
        /// </summary>
        /// <param name="stateBag">The state bag.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(this StateBag stateBag, string key, object value) {
            stateBag[key] = value;
        }
    }
}