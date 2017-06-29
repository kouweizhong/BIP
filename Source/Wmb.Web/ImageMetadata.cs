using System;
using System.Drawing.Imaging;

using Wmb.Drawing;

namespace Wmb.Web {
    /// <summary>
    /// The image meta data class is used to reflect information about an image, needed for responding to the client without getting back to the image retriever.
    /// </summary>
    [Serializable]
    public sealed class ImageMetadata {
        private ImageMetadata() {
            this.LastModifiedDate = DateTime.MinValue;
        }

        /// <summary>
        /// Creates a new ImageMetadata object based on the specified image retriever and save name.
        /// </summary>
        /// <param name="imageRetriever">The image retriever.</param>
        /// <param name="saveName">Name of the save.</param>
        /// <returns>An instance of te image metadata</returns>
        public static ImageMetadata Create(ImageRetriever imageRetriever, string saveName) {
            if(imageRetriever == null){
                throw new ArgumentNullException("imageRetriever");
            }

            imageRetriever.EnsureMetadata();

            ImageMetadata retVal = new ImageMetadata();

            string filename = imageRetriever.FileNameWithoutExtension;
            retVal.SaveName = (string.IsNullOrEmpty(saveName)) ? filename : saveName;
            
            retVal.Extension = imageRetriever.Extension.ToUpperInvariant();
            retVal.ImageFormat = ImageFormatUtility.GetImageFormatByExtension(retVal.Extension);
            retVal.ContentType = retVal.ImageFormat.GetContentType();
            
            DateTime lastModifiedDate = imageRetriever.LastModifiedDate;
            retVal.LastModifiedDate = new DateTime(lastModifiedDate.Year,
                                                  lastModifiedDate.Month,
                                                  lastModifiedDate.Day,
                                                  lastModifiedDate.Hour,
                                                  lastModifiedDate.Minute,
                                                  lastModifiedDate.Second,
                                                  DateTimeKind.Utc
                                    );

            return retVal;
        }

        /// <summary>
        /// Gets the initial name of the image when the client saves it.
        /// </summary>
        /// <value>The name of the save.</value>
        public string SaveName { get; private set; }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <value>The extension.</value>
        public string Extension { get; private set; }

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime LastModifiedDate { get; private set; }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gets the image format.
        /// </summary>
        /// <value>The image format.</value>
        public ImageFormat ImageFormat { get; private set; }
    }
}
