using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Wmb.Drawing;
using System.Diagnostics;

namespace Wmb.Web {
    /// <summary>
    /// The BetterImageConfigurationSection is there to set some default options for the better image processor.
    /// </summary>
    /// <example>This is a sample web.config
    /// <code source="Wmb.Web.BetterImageConfigurationSection.Code.xml" lang="xml" /> 
    /// </example>
    public sealed class BetterImageConfigurationSection : ConfigurationSection {
        /// <summary>
        /// Initializes a new instance of the <see cref="BetterImageConfigurationSection"/> class.
        /// </summary>
        public BetterImageConfigurationSection() : base() { }

        private static BetterImageConfigurationSection current;
        /// <summary>
        /// Gets the current BetterImageConfigurationSection.
        /// </summary>
        /// <value>The current BetterImageConfigurationSection.</value>
        public static BetterImageConfigurationSection Current {
            get {
                if (current == null) {
                    current = ConfigurationManager.GetSection("BetterImageConfiguration") as BetterImageConfigurationSection;

                    if (current == null) {
                        Trace.TraceWarning("BetterImageConfigurationSection: Could not load the configuration from config file. Returning a default BetterImageConfigurationSection.");
                        current = new BetterImageConfigurationSection();
                    }
                }

                return current;
            }
        }

        private object imageCacheBrokersSyncLock = new object();
        private Dictionary<string, ImageCacheBrokerElement> cacheBrokers;
        /// <summary>
        /// Gets the name of the image cache broker class and the output cache provider it uses.
        /// </summary>
        /// <param name="imageCacheBrokerName">Name of the image cache broker.</param>
        /// <returns>The class name of the ImageCacheBroker</returns>
        public string GetImageCacheBrokerClassName(string imageCacheBrokerName) {
            if (string.IsNullOrEmpty(imageCacheBrokerName)) {
                throw new ArgumentNullException("imageCacheBrokerName");
            }

            lock (imageCacheBrokersSyncLock) {
                if (cacheBrokers == null) {
                    cacheBrokers = new Dictionary<string, ImageCacheBrokerElement>();
                    foreach (ImageCacheBrokerElement element in this.ImageCacheBrokers  ) {
                        cacheBrokers.Add(element.Name, element);
                    }
                }
            }

            if (!cacheBrokers.ContainsKey(imageCacheBrokerName)) {
                throw new ConfigurationErrorsException(
                    string.Format(CultureInfo.InvariantCulture,
                                  "Could not find an image cache broker element named:{0}.",
                                  imageCacheBrokerName));
            }

            return cacheBrokers[imageCacheBrokerName].Class;
        }

        /// <summary>
        /// Gets the name of the default image cache broker class to use for caching your images and their metadata
        /// </summary>
        /// <value>The image cache broker.</value>
        [ConfigurationProperty("imageCacheBroker", IsRequired = false, DefaultValue = "WebImageCacheBroker")]
        public string ImageCacheBroker {
            get {
                return this["imageCacheBroker"] as string;
            }
        }

        /// <summary>
        /// Gets the image cache brokers collection.
        /// </summary>
        /// <value>The image cache brokers.</value>
        [ConfigurationProperty("imageCacheBrokers", IsRequired = false, IsDefaultCollection = false)]
        public ImageCacheBrokerCollection ImageCacheBrokers {
            get { return this["imageCacheBrokers"] as ImageCacheBrokerCollection; }
        }

        private object retrieversSyncLock = new object();
        private Dictionary<string, ImageRetrieverElement> retrievers;
        /// <summary>
        /// Gets the name of the image retriever class.
        /// </summary>
        /// <param name="imageRetrieverName">Name of the image retriever.</param>
        /// <returns>The class name of the ImageRetriever</returns>
        public string GetImageRetrieverClassName(string imageRetrieverName) {
            if (string.IsNullOrEmpty(imageRetrieverName)) {
                throw new ArgumentNullException("imageRetrieverName");
            }

            lock (retrieversSyncLock) {
                if (retrievers == null) {
                    retrievers = new Dictionary<string, ImageRetrieverElement>();
                    foreach (ImageRetrieverElement element in this.ImageRetrievers) {
                        retrievers.Add(element.Name, element);
                    }
                }
            }

            if (!retrievers.ContainsKey(imageRetrieverName)) {
                throw new ConfigurationErrorsException(
                    string.Format(CultureInfo.InvariantCulture,
                                  "Could not find an image retriever element named:{0}.",
                                  imageRetrieverName));
            }

            return retrievers[imageRetrieverName].Class;
        }

        /// <summary>
        /// Gets the name of the default image retriever class to use for retrieving your images
        /// </summary>
        /// <value>The image retriever.</value>
        [ConfigurationProperty("imageRetriever", IsRequired = false, DefaultValue = "FileSystemImageRetriever")]
        public string ImageRetriever {
            get {
                return this["imageRetriever"] as string;
            }
        }

        /// <summary>
        /// Gets the image retrievers collection.
        /// </summary>
        /// <value>The image retrievers.</value>
        [ConfigurationProperty("imageRetrievers", IsRequired = false, IsDefaultCollection = false)]
        public ImageRetrieverCollection ImageRetrievers {
            get { return this["imageRetrievers"] as ImageRetrieverCollection; }
        }

        private object transformsSyncLock = new object();
        private Dictionary<string, ImageTransformElement> transforms;
        /// <summary>
        /// Gets the full class name of the image transform class.
        /// </summary>
        /// <param name="imageTransformName">Name of the image transform.</param>
        /// <returns>The full class name that is registered with this transform name</returns>
        public string GetImageTransformClassName(string imageTransformName) {
            if (string.IsNullOrEmpty(imageTransformName)) {
                throw new ArgumentNullException("imageTransformName");
            }

            lock (transformsSyncLock) {
                if (transforms == null) {
                    transforms = new Dictionary<string, ImageTransformElement>();
                    foreach (ImageTransformElement element in this.ImageTransforms) {
                        transforms.Add(element.Name, element);
                    }
                }
            }

            if (!transforms.ContainsKey(imageTransformName)) {
                throw new ConfigurationErrorsException(
                    string.Format(CultureInfo.InvariantCulture,
                                  "Could not find an image transform element named:{0}.",
                                  imageTransformName));
            }

            return transforms[imageTransformName].Class;
        }

        /// <summary>
        /// Gets the image transforms collection.
        /// </summary>
        /// <value>The image transforms.</value>
        [ConfigurationProperty("imageTransforms", IsRequired = false, IsDefaultCollection = false)]
        public ImageTransformCollection ImageTransforms {
            get { return this["imageTransforms"] as ImageTransformCollection; }
        }

        /// <summary>
        /// Gets the url of image handler.
        /// </summary>
        /// <value>The url of the image handler.</value>
        [ConfigurationProperty("imageHandler", IsRequired = false, DefaultValue = "/ImageHandler.ashx")]
        public string ImageHandler {
            get {
                return this["imageHandler"] as string;
            }
        }

        /// <summary>
        /// Gets the maximum width of the images.
        /// </summary>
        /// <value>0</value>
        [ConfigurationProperty("maxWidth", IsRequired = false, DefaultValue = 0)]
        public int MaxWidth {
            get {
                return (int)this["maxWidth"];
            }
        }

        /// <summary>
        /// Gets the maximum height of the images.
        /// </summary>
        /// <value>0</value>
        [ConfigurationProperty("maxHeight", IsRequired = false, DefaultValue = 0)]
        public int MaxHeight {
            get {
                return (int)this["maxHeight"];
            }
        }

        /// <summary>
        /// Gets the resize quality of the images.
        /// </summary>
        /// <value>The resize quality of the images.</value>
        [ConfigurationProperty("graphicsQuality", IsRequired = false, DefaultValue = GraphicsQuality.Medium)]
        public GraphicsQuality GraphicsQuality {
            get {
                return (GraphicsQuality)this["graphicsQuality"];
            }
        }

        /// <summary>
        /// Gets the output quality of the images. Works on jpeg images only!
        /// </summary>
        /// <value>The output quality.</value>
        [ConfigurationProperty("outputQuality", IsRequired = false, DefaultValue = 75L)]
        public long OutputQuality {
            get {
                return (long)this["outputQuality"];
            }
        }

        /// <summary>
        /// Whether or not you would like the images grayscaled.
        /// </summary>
        [ConfigurationProperty("grayscale", IsRequired = false, DefaultValue = false)]
        public bool Grayscale {
            get {
                return (bool)this["grayscale"];
            }
        }

        /// <summary>
        /// Whether or not you would like a negative of the images.
        /// </summary>
        [ConfigurationProperty("negative", IsRequired = false, DefaultValue = false)]
        public bool Negative {
            get {
                return (bool)this["negative"];
            }
        }

        /// <summary>
        /// Whether or not you would like to turn your images into sepia.
        /// </summary>
        [ConfigurationProperty("sepia", IsRequired = false, DefaultValue = false)]
        public bool Sepia {
            get {
                return (bool)this["sepia"];
            }
        }

        /// <summary>
        /// Whether or not you would like to maintain the original palette. This keeps the original colorpalette which is great for resizing GIF and PNG.
        /// </summary>
        [ConfigurationProperty("maintainPalette", IsRequired = false, DefaultValue = false)]
        public bool MaintainPalette {
            get {
                return (bool)this["maintainPalette"];
            }
        }

        /// <summary>
        /// Whether or not you would like the images to be clipped to size.
        /// </summary>
        [ConfigurationProperty("clip", IsRequired = false, DefaultValue = false)]
        public bool Clip {
            get {
                return (bool)this["clip"];
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not you would like to quantize your GIF images. It greatly enhances the quality at the cost of processingtime.
        /// </summary>
        [ConfigurationProperty("quantize", IsRequired = false, DefaultValue = false)]
        public bool Quantize {
            get {
                return (bool)this["quantize"];
            }
        }

        /// <summary>
        /// Adjust the brightness of your images. Choose a value between -1 and 1 where 0 is default.
        /// </summary>
        [ConfigurationProperty("brightness", IsRequired = false, DefaultValue = 0F)]
        public float Brightness {
            get {
                return (float)this["brightness"];
            }
        }

        /// <summary>
        /// Adjust the contrast of your image. Choose a value between 0 and 3 where 1 is default.
        /// </summary>
        [ConfigurationProperty("contrast", IsRequired = false, DefaultValue = 1F)]
        public float Contrast {
            get {
                return (float)this["contrast"];
            }
        }

        /// <summary>
        /// Adjust the opacity of your image. Choose a value between 0 and 1 where 0 is default.
        /// </summary>
        [ConfigurationProperty("opacity", IsRequired = false, DefaultValue = 0F)]
        public float Opacity {
            get {
                return (float)this["opacity"];
            }
        }

        /// <summary>
        /// Gets the server cache timeout in minutes.
        /// </summary>
        /// <value>The server cache timeout.</value>
        [ConfigurationProperty("serverCacheTimeout", IsRequired = false, DefaultValue = 0)]
        public int ServerCacheTimeout {
            get {
                return (int)this["serverCacheTimeout"];
            }
        }

        /// <summary>
        /// Gets the client cache timeout in minutes.
        /// </summary>
        /// <value>The client cache timeout.</value>
        [ConfigurationProperty("clientCacheTimeout", IsRequired = false, DefaultValue = 60)]
        public int ClientCacheTimeout {
            get {
                return (int)this["clientCacheTimeout"];
            }
        }

        /// <summary>
        /// Gets or sets the preset name in the Save dialog on 'Save image as'.
        /// </summary>
        /// <value>The save name.</value>
        [ConfigurationProperty("saveName", IsRequired = false)]
        public string SaveName {
            get {
                return this["saveName"] as string;
            }
        }

        /// <summary>
        /// Gets the copyright text.
        /// </summary>
        /// <value>The copyright text.</value>
        [ConfigurationProperty("copyright", IsRequired = false)]
        public string Copyright {
            get {
                return this["copyright"] as string;
            }
        }

        /// <summary>
        /// Gets the fontsize for the copyright text.
        /// </summary>
        /// <value>The fontsize for the copyright text.</value>
        [ConfigurationProperty("copyrightSize", IsRequired = false, DefaultValue = 0)]
        public int CopyrightSize {
            get {
                return (int)this["copyrightSize"];
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not you would like the src of the image to be encrypted. It helps in preventing download of the original image.
        /// </summary>
        /// <value><c>true</c> if you want to encrypt the source; otherwise, <c>false</c>.</value>
        [ConfigurationProperty("encryptSrc", IsRequired = false, DefaultValue = false)]
        public bool EncryptSrc {
            get {
                return (bool)this["encryptSrc"];
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not you would like to disable right mouse button. Works only if JavaScript is enabled.
        /// </summary>
        /// <value><c>true</c> if you would like to disable right click; otherwise, <c>false</c>.</value>
        [ConfigurationProperty("disableRightClick", IsRequired = false, DefaultValue = false)]
        public bool DisableRightClick {
            get {
                return (bool)this["disableRightClick"];
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not you would like to protect your images from leechers / deeplinkers.
        /// </summary>
        /// <value><c>true</c> if you would like to leech protect; otherwise, <c>false</c>.</value>
        [ConfigurationProperty("leechProtect", IsRequired = false, DefaultValue = false)]
        public bool LeechProtect {
            get {
                return (bool)this["leechProtect"];
            }
        }

        /// <summary>
        /// Gets the custom transform.
        /// </summary>
        /// <value>The custom transform.</value>
        [ConfigurationProperty("customTransform", IsRequired = false)]
        public string CustomTransform {
            get {
                return this["customTransform"] as string;
            }
        }

        /// <summary>
        /// Gets the custom data.
        /// </summary>
        /// <value>The custom transform.</value>
        [ConfigurationProperty("customData", IsRequired = false)]
        public string CustomData {
            get {
                return this["customData"] as string;
            }
        }

        /// <summary>
        /// Gets the password that's used for encryption.
        /// </summary>
        /// <value>The password.</value>
        [ConfigurationProperty("password", IsRequired = false, DefaultValue = "mypassword")]
        public string Password {
            get {
                return this["password"] as string;
            }
        }

        /// <summary>
        /// Gets the salt that's used for encryption.
        /// </summary>
        /// <value>The salt.</value>
        [ConfigurationProperty("salt", IsRequired = false, DefaultValue = "mysalt")]
        public string Salt {
            get {
                return this["salt"] as string;
            }
        }
    }
}