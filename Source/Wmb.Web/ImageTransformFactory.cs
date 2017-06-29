using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;

using Wmb.Drawing;

namespace Wmb.Web {
    internal static class ImageTransformFactory {
        private static Dictionary<string, ConstructorInfo> imageTransformCache = new Dictionary<string, ConstructorInfo>();
        private static ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

        /// <summary>
        /// Creates the image transform.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>An instance of the image tranform</returns>
        internal static ImageTransform Create(string typeName) {
            if (string.IsNullOrEmpty(typeName)) {
                throw new ArgumentNullException("typeName");
            }

            ImageTransform retVal = null;
            ConstructorInfo constructorInfo = null;

            readerWriterLockSlim.EnterUpgradeableReadLock();
            try {
                if (!imageTransformCache.TryGetValue(typeName, out constructorInfo)) {
                    Type connectorType = Type.GetType(typeName, false, true);
                    if (connectorType != null && connectorType.IsSubclassOf(typeof(ImageTransform))) {
                        constructorInfo = connectorType.GetConstructor(Type.EmptyTypes);

                        if (constructorInfo == null) {
                            string errorMessage = string.Format(CultureInfo.InvariantCulture, "The given type: '{0}' does not contain a constructor that takes no arguments.", typeName);
                            throw new ArgumentException(errorMessage, "typeName");
                        }
                        
                        readerWriterLockSlim.EnterWriteLock();
                        try {
                            imageTransformCache.Add(typeName, constructorInfo);
                        } finally {
                            readerWriterLockSlim.ExitWriteLock();
                        }
                    } else {
                        string errorMessage = string.Format(CultureInfo.InvariantCulture, "The given type: '{0}' can not be found or does not implement the abstract ImageTransform class.", typeName);
                        throw new ArgumentException(errorMessage, "typeName");
                    }
                }
            } finally {
                readerWriterLockSlim.ExitUpgradeableReadLock();
            }

            retVal = constructorInfo.Invoke(null) as ImageTransform;

            return retVal;
        }
    }
}
