using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// <para>The ImageFormatUtility class holds the extensions and/or helpermethods for the ImageFormat class.</para>
    /// </summary>
    public static class ImageFormatUtility {
        /// <summary>
        /// Gets the image codec info.
        /// </summary>
        /// <param name="imageFormat">The image format.</param>
        /// <returns>The image codec info for the given format</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the imageFormat parameter is null.</exception>
        public static ImageCodecInfo GetImageCodecInfo(this ImageFormat imageFormat) {
            if (imageFormat == null) {
                throw new ArgumentNullException("imageFormat");
            }

            ImageCodecInfo retVal = null;

            foreach (ImageCodecInfo imageCodecInfo in ImageCodecInfo.GetImageEncoders()) {
                if  (imageCodecInfo.FormatID == imageFormat.Guid) {
                    retVal = imageCodecInfo;
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Gets the image format by file extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public static ImageFormat GetImageFormatByExtension(string extension) {
            extension = extension.ToUpperInvariant();

            ImageFormat retVal;

            switch (extension) {
                case ".PNG":
                    retVal = ImageFormat.Png;
                    break;

                case ".GIF":
                    retVal = ImageFormat.Gif;
                    break;

                default:
                    retVal = ImageFormat.Jpeg;
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Gets the content type for the given ImageFormat.
        /// </summary>
        /// <param name="imageFormat">The image format.</param>
        /// <returns></returns>
        public static string GetContentType(this ImageFormat imageFormat) {
            string retVal;

            if (imageFormat == ImageFormat.Png) {
                retVal = "image/png";
            }
            else if (imageFormat == ImageFormat.Gif) {
                retVal = "image/gif";
            }
            else {
                retVal = "image/jpeg";
            }

            return retVal;
        }
    }
}
