using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace Wmb.Web {
    internal static class ImageRetrieverFactory {
        private static Dictionary<string, ConstructorInfo> imageRetrieverCache = new Dictionary<string, ConstructorInfo>();
        private static ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

        /// <summary>
        /// Creates the image retriever.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>An instance of the image retriever </returns>
        internal static ImageRetriever Create(string source, string typeName) {
            if (string.IsNullOrEmpty(typeName)) {
                throw new ArgumentNullException("typeName");
            }

            ImageRetriever retVal = null;
            ConstructorInfo constructorInfo = null;

            readerWriterLockSlim.EnterUpgradeableReadLock();
            try {
                if (!imageRetrieverCache.TryGetValue(typeName, out constructorInfo)) {
                    Type connectorType = Type.GetType(typeName, false, true);
                    if (connectorType != null && connectorType.IsSubclassOf(typeof(ImageRetriever))) {
                        Type[] argumentTypes = new Type[1];
                        argumentTypes[0] = typeof(string);
                        constructorInfo = connectorType.GetConstructor(argumentTypes);

                        if (constructorInfo == null) {
                            string errorMessage = string.Format(CultureInfo.InvariantCulture, "The given type: '{0}' does not contain a constructor that takes one argument.", typeName);
                            throw new ArgumentException(errorMessage, "typeName");
                        }

                        readerWriterLockSlim.EnterWriteLock();
                        try {
                            imageRetrieverCache.Add(typeName, constructorInfo);
                        } finally {
                            readerWriterLockSlim.ExitWriteLock();
                        }
                    } else {
                        string errorMessage = string.Format(CultureInfo.InvariantCulture, "The given type: '{0}' can not be found or does not implement the abstract ImageRetriever class.", typeName);
                        throw new ArgumentException(errorMessage, "typeName");
                    }
                }
            } finally {
                readerWriterLockSlim.ExitUpgradeableReadLock();
            }

            retVal = constructorInfo.Invoke(new object[] { source }) as ImageRetriever;

            return retVal;
        }
    }
}
