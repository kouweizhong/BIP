using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace Wmb.Web.Caching {
    internal static class ImageCacheBrokerFactory {
        private static Dictionary<string, ConstructorInfo> imageCacheBrokerCache = new Dictionary<string, ConstructorInfo>();
        private static ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

        /// <summary>
        /// Creates image cache broker by the specified type name.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="key">The key.</param>
        /// <param name="utcExpiry">The UTC expiry.</param>
        /// <returns>An instance of the cache broker</returns>
        internal static ImageCacheBroker Create(string typeName, string key, DateTime utcExpiry) {
            if (string.IsNullOrEmpty(typeName)) {
                throw new ArgumentNullException("typeName");
            }

            ImageCacheBroker retVal = null;
            ConstructorInfo constructorInfo = null;

            readerWriterLockSlim.EnterUpgradeableReadLock();
            try {
                if (!imageCacheBrokerCache.TryGetValue(typeName, out constructorInfo)) {
                    Type connectorType = Type.GetType(typeName, false, true);
                    if (connectorType != null && connectorType.IsSubclassOf(typeof(ImageCacheBroker))) {
                        Type[] types = new[] { typeof(String), typeof(DateTime) };

                        constructorInfo = connectorType.GetConstructor(types);

                        if (constructorInfo == null) {
                            string errorMessage = string.Format(CultureInfo.InvariantCulture, "The given type: '{0}' does not contain a constructor with two arguments of type String and DateTime.", typeName);
                            throw new ArgumentException(errorMessage, "typeName");
                        }

                        readerWriterLockSlim.EnterWriteLock();
                        try {
                            imageCacheBrokerCache.Add(typeName, constructorInfo);
                        } finally {
                            readerWriterLockSlim.ExitWriteLock();
                        }
                    } else {
                        string errorMessage = string.Format(CultureInfo.InvariantCulture, "The given type: '{0}' can not be found or does not inherit the abstract ImageCacheBroker class.", typeName);
                        throw new ArgumentException(errorMessage, "typeName");
                    }
                }
            } finally {
                readerWriterLockSlim.ExitUpgradeableReadLock();
            }

            object[] parameters = new object[] { key, utcExpiry };
            retVal = constructorInfo.Invoke(parameters) as ImageCacheBroker;

            return retVal;
        }
    }
}
