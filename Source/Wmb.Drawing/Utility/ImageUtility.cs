using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

namespace Wmb.Drawing {
    /// <summary>
    /// <para>The ImageUtility class holds the extensions and/or helpermethods for the Image class.</para>
    /// </summary>
    public static class ImageUtility {
        /// <summary>
        /// <para>Transforms the specified image.</para>
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="imageTransform">The image transform.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the imageTransform parameter is null.</exception>
        public static Image Transform(this Image image, ImageTransform imageTransform) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if (imageTransform == null) {
                throw new ArgumentNullException("imageTransform");
            }

            imageTransform.Transform(image);

            return image;
        }

        /// <summary>
        /// <para>Transforms the specified image.</para>
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="imageTransforms">The image transforms.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the imageTransforms parameter is null.</exception>
        public static Image Transform(this Image image, IEnumerable<ImageTransform> imageTransforms) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if (imageTransforms == null) {
                throw new ArgumentNullException("imageTransforms");
            }

            Queue<ColorMatrix> colorMatrices = new Queue<ColorMatrix>();
            foreach (ImageTransform imageTransform in imageTransforms) {
                ColorMatrixTransform colorMatrixTransform = imageTransform as ColorMatrixTransform;
                if (colorMatrixTransform != null) {
                    ColorMatrix colorMatrix = colorMatrixTransform.GetColorMatrix();
                    colorMatrices.Enqueue(colorMatrix);
                }
                else {
                    image.Transform(colorMatrices);

                    imageTransform.Transform(image);
                }
            }

            image.Transform(colorMatrices);

            return image;
        }

        /// <summary>
        /// Transforms the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="colorMatrices">The color matrices to apply to the image.</param>
        /// <returns></returns>
        public static Image Transform(this Image image, Queue<ColorMatrix> colorMatrices)
        {
            if (colorMatrices.Count > 0) {
                ColorMatrix multipliedMatrices = colorMatrices.Dequeue();
                while (colorMatrices.Count > 0) {
                    multipliedMatrices.Multiply(colorMatrices.Dequeue());
                }

                image.ApplyColorMatrix(multipliedMatrices);
            }

            return image;
        }

        /// <summary>
        /// <para>Applies a color matrix to an Image without clearing the graphics object.</para>
        /// </summary>
        /// <param name="image">The Image to apply the maxtrix to.</param>
        /// <param name="colorMatrix">The color matrix.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the colorMatrix parameter is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the image has an indexed pixel format.</exception>
        public static void ApplyColorMatrix(this Image image, ColorMatrix colorMatrix) {
            ApplyColorMatrix(image, colorMatrix, false);
        }

        /// <summary>
        /// <para>Applies a color matrix to an Image.</para>
        /// </summary>
        /// <param name="image">The Image to apply the maxtrix to.</param>
        /// <param name="colorMatrix">The color matrix.</param>
        /// <param name="clearGraphics">Whether or not you would like to clear the graphics object before redrawing over it. This is necesary if you adjust the Opacity for instance.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the colorMatrix parameter is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the image has an indexed pixel format.</exception>
        public static void ApplyColorMatrix(this Image image, ColorMatrix colorMatrix, bool clearGraphics) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if (colorMatrix == null) {
                throw new ArgumentNullException("colorMatrix");
            }

            if (image.HasIndexedPixelFormat()) {
                throw new ArgumentException("A Graphics object cannot be created from an image that has an indexed pixel format. This makes it impossible to apply a colormatrix.");
            }

            using (Graphics graphics = Graphics.FromImage(image))
            using (ImageAttributes imageAttributes = new ImageAttributes()) {
                imageAttributes.SetColorMatrix(colorMatrix);

                Image imageToDraw = image;
                if (clearGraphics) {
                    imageToDraw = image.Clone() as Image;
                    graphics.Clear(Color.Transparent);
                }

                graphics.DrawImage(imageToDraw, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                graphics.Save();

                if (clearGraphics) {
                    imageToDraw.Dispose();
                }
            }
        }

        /// <summary>
        /// <para>Returns an octree quantized Image with a ColorPalette of 255, 8 bit colors.</para>
        /// </summary>
        /// <param name="image">The Image on which to apply an octree quantization.</param>
        /// <returns>The quantized Image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        public static Image OctreeQuantize(this Image image) {
            return OctreeQuantize(image, 255, 8);
        }

        /// <summary>
        /// <para>Returns an octree quantized Image.</para>
        /// </summary>
        /// <param name="image">The Image on which to apply an octree quantization.</param>
        /// <param name="maxColors">The maximum amount of colors to be used for the ColorPalette with a maximum of 255.</param>
        /// <param name="maxColorBits">The maximum amount of bits the colors in the ColorPalette can be with a maximum of 8.</param>
        /// <returns>The quantized Image.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the maxColors parameter greater than 256.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the maxColorBits parameter is less than 1 or greater than 8.</exception>
        public static Image OctreeQuantize(this Image image, int maxColors, int maxColorBits) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if (maxColors > 255) {
                throw new ArgumentOutOfRangeException("maxColors", maxColors, "The number of colors should be less than 256");
            }

            if ((maxColorBits < 1) | (maxColorBits > 8)) {
                throw new ArgumentOutOfRangeException("maxColorBits", maxColorBits, "This should be between 1 and 8");
            }

            OctreeQuantizer octreeQuantizer = new OctreeQuantizer(maxColors, maxColorBits);
            return octreeQuantizer.Quantize(image);
        }

        /// <summary>
        /// <para>Returns a palette quantized Image.</para>
        /// </summary>
        /// <param name="image">The image on which to apply a palette quantization.</param>
        /// <param name="palette">The palette being used for this quantization.</param>
        /// <returns>The quantized Image.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the palette parameter is null or does not contain any colors.</exception>
        public static Image PaletteQuantize(this Image image, ICollection palette) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if (palette == null || palette.Count == 0) {
                throw new ArgumentNullException("palette");
            }

            PaletteQuantizer paletteQuantizer = new PaletteQuantizer(palette);
            return paletteQuantizer.Quantize(image);
        }

        /// <summary>
        /// <para>Gets a resized Image from a Stream. With GraphicsQuality.Medium en not maintaining the(if any) palette.</para>
        /// </summary>
        /// <param name="stream">The Stream containing an Image</param>
        /// <param name="maxWidth">The maximum width for the resulting Image. Set to 0 if there's no maximum width.</param>
        /// <param name="maxHeight">The maximum height for the resulting Image. Set to 0 if there's no maximum height.</param>
        /// <returns>Image</returns>
        public static Image GetResizedImageFromStream(Stream stream, int maxWidth, int maxHeight) {
            return GetResizedImagedFromStream(stream, maxWidth, maxHeight, GraphicsQuality.Medium, false);
        }

        /// <summary>
        /// <para>Gets a resized Image from a Stream. Not maintaining the(if any) palette.</para>
        /// </summary>
        /// <param name="stream">The Stream containing an Image</param>
        /// <param name="maxWidth">The maximum width for the resulting Image. Set to 0 if there's no maximum width.</param>
        /// <param name="maxHeight">The maximum height for the resulting Image. Set to 0 if there's no maximum height.</param>
        /// <param name="graphicsQuality">The GraphicsQuality for the resulting Image.</param>
        /// <returns>Image</returns>
        public static Image GetResizedImageFromStream(Stream stream, int maxWidth, int maxHeight, GraphicsQuality graphicsQuality) {
            return GetResizedImagedFromStream(stream, maxWidth, maxHeight, graphicsQuality, false);
        }

        /// <summary>
        /// <para>Gets a resized Image from a Stream.</para>
        /// </summary>
        /// <param name="stream">The Stream containing an image</param>
        /// <param name="maxWidth">The maximum width for the resulting Image. Set to 0 if there's no maximum width.</param>
        /// <param name="maxHeight">The maximum height for the resulting Image. Set to 0 if there's no maximum height.</param>
        /// <param name="graphicsQuality">The GraphicsQuality for the resulting Image.</param>
        /// <param name="maintainPalette">If set to <c>true</c> the(if any) palette will be maintained. Transparency will be saved.</param>
        /// <returns>The resized Image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the stream parameter is null</exception>
        public static Image GetResizedImagedFromStream(Stream stream, int maxWidth, int maxHeight, GraphicsQuality graphicsQuality, bool maintainPalette) {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }

            Image retVal = null;

            using (Bitmap sourceBitmap = new Bitmap(stream)) {
                retVal = Resize(sourceBitmap, maxWidth, maxHeight, graphicsQuality, maintainPalette);
            }

            return retVal;
        }

        /// <summary>
        /// <para>Returns a resized Image.</para>
        /// </summary>
        /// <param name="image">The Image to resize.</param>
        /// <param name="maxWidth">The maximum width for the resulting Image. Set to 0 if there's no maximum width.</param>
        /// <param name="maxHeight">The maximum height for the resulting Image. Set to 0 if there's no maximum height.</param>
        /// <returns>The resized Image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        public static Image Resize(this Image image, int maxWidth, int maxHeight) {
            return Resize(image, maxWidth, maxHeight, GraphicsQuality.Medium, false);
        }

        /// <summary>
        /// <para>Returns a resized Image.</para>
        /// </summary>
        /// <param name="image">The Image to resize.</param>
        /// <param name="maxWidth">The maximum width for the resulting Image. Set to 0 if there's no maximum width.</param>
        /// <param name="maxHeight">The maximum height for the resulting Image. Set to 0 if there's no maximum height.</param>
        /// <param name="graphicsQuality">The graphics quality for the resulting Image.</param>
        /// <returns>The resized Image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        public static Image Resize(this Image image, int maxWidth, int maxHeight, GraphicsQuality graphicsQuality) {
            return Resize(image, maxWidth, maxHeight, graphicsQuality, false);
        }

        /// <summary>
        /// <para>Returns a resized Image.</para>
        /// </summary>
        /// <param name="image">The Image to resize.</param>
        /// <param name="maxWidth">The maximum width for the resulting Image. Set to 0 if there's no maximum width.</param>
        /// <param name="maxHeight">The maximum height for the resulting Image. Set to 0 if there's no maximum height.</param>
        /// <param name="graphicsQuality">The graphics quality for the resulting Image.</param>
        /// <param name="maintainPalette">If set to <c>true</c> the(if any) palette will be maintained. Transparency will be saved.</param>
        /// <returns>The resized Image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        public static Image Resize(this Image image, int maxWidth, int maxHeight, GraphicsQuality graphicsQuality, bool maintainPalette) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            Image retVal = image;

            Size thumbnailSize = image.CalculateThumbSize(maxWidth, maxHeight);
            if (thumbnailSize.Width < image.Width || thumbnailSize.Height < image.Height || (image.HasIndexedPixelFormat() && !maintainPalette)) {
                Bitmap bitmap = new Bitmap(thumbnailSize.Width, thumbnailSize.Height);

                using (Graphics graphics = Graphics.FromImage(bitmap)) {
                    graphics.ApplyGraphicsQualitySetting(graphicsQuality);
                    graphics.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    graphics.Save();
                }

                retVal = bitmap;
                if (maintainPalette && image.HasIndexedPixelFormat()) {
                    retVal = bitmap.PaletteQuantize(image.Palette.Entries);
                    bitmap.Dispose();
                }
            }

            return retVal;
        }

        /// <summary>
        /// Clips the specified image.
        /// </summary>
        /// <param name="image">The image to clip.</param>
        /// <param name="maxWidth">The maximum width for the resulting Image. Set to 0 if there's no maximum width.</param>
        /// <param name="maxHeight">The maximum height for the resulting Image. Set to 0 if there's no maximum height.</param>
        /// <returns>The clipped Image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        public static Image Clip(this Image image, int maxWidth, int maxHeight) {
            return Clip(image, maxWidth, maxHeight, GraphicsQuality.Medium, false);
        }

        /// <summary>
        /// <para>Clips the specified image.</para>
        /// </summary>
        /// <param name="image">The image to clip.</param>
        /// <param name="maxWidth">The maximum width for the resulting Image. Set to 0 if there's no maximum width.</param>
        /// <param name="maxHeight">The maximum height for the resulting Image. Set to 0 if there's no maximum height.</param>
        /// <param name="graphicsQuality">The graphics quality for the resulting Image.</param>
        /// <returns>The clipped Image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        public static Image Clip(this Image image, int maxWidth, int maxHeight, GraphicsQuality graphicsQuality) {
            return Clip(image, maxWidth, maxHeight, graphicsQuality, false);
        }

        /// <summary>
        /// <para>Clips the specified image.</para>
        /// </summary>
        /// <param name="image">The image to clip.</param>
        /// <param name="maxWidth">The maximum width for the resulting Image. Set to 0 if there's no maximum width.</param>
        /// <param name="maxHeight">The maximum height for the resulting Image. Set to 0 if there's no maximum height.</param>
        /// <param name="graphicsQuality">The graphics quality for the resulting Image.</param>
        /// <param name="maintainPalette">If set to <c>true</c> the(if any) palette will be maintained. Transparency will be saved.</param>
        /// <returns>The clipped Image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        public static Image Clip(this Image image, int maxWidth, int maxHeight, GraphicsQuality graphicsQuality, bool maintainPalette) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if (maxWidth <= 0) {
                maxWidth = image.Width;
            }

            if (maxHeight <= 0) {
                maxHeight = image.Height;
            }

            Image retVal = image;

            Bitmap bitmap = new Bitmap(maxWidth, maxHeight);
            using (Graphics graphics = Graphics.FromImage(bitmap)) {
                graphics.ApplyGraphicsQualitySetting(graphicsQuality);
                graphics.DrawImage(image, image.CalculateClipRectangle(maxWidth, maxHeight));
                graphics.Save();
            }

            retVal = bitmap;
            if (maintainPalette && image.HasIndexedPixelFormat()) {
                retVal = bitmap.PaletteQuantize(image.Palette.Entries);
                bitmap.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// <para>Determines whether the specified image has an indexed pixel format.</para>
        /// </summary>
        /// <param name="image">The Image.</param>
        /// <returns>
        /// 	<c>true</c> if the specified image has an indexed pixel format; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        public static bool HasIndexedPixelFormat(this Image image) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            return ((image.PixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed);
        }

        /// <summary>
        /// <para>Returns a redrawn Image.</para>
        /// </summary>
        /// <param name="image">The Image to redraw.</param>
        /// <param name="graphicsQuality">The graphics quality for the resulting Image.</param>
        /// <param name="pixelFormat">The pixel format for the resulting Image.</param>
        /// <returns>The redrawn Image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when the pixel format of the image parameter is indexed</exception>
        public static Image Redraw(this Image image, GraphicsQuality graphicsQuality, PixelFormat pixelFormat) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if ((pixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed) {
                throw new ArgumentException("Images can only be redrawn to NON-Indexed pixel formats", "pixelFormat");
            }

            Image retVal = new Bitmap(image.Width, image.Height, pixelFormat);

            using (Graphics graphics = Graphics.FromImage(retVal)) {
                graphics.ApplyGraphicsQualitySetting(graphicsQuality);
                graphics.DrawImage(image, new Rectangle(0, 0, retVal.Width, retVal.Height));
                graphics.Save();
            }

            return retVal;
        }

        /// <summary>
        /// <para>Calculates the size for a thumbnail.</para>
        /// </summary>
        /// <param name="image">The Image of which to calculate the size for a thumbnail.</param>
        /// <param name="maxWidth">The maximum width for the resulting thumbnail.</param>
        /// <param name="maxHeight">The maximum height for the resulting thumbnail.</param>
        /// <returns>The thumbnail Size</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        public static Size CalculateThumbSize(this Image image, int maxWidth, int maxHeight) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            double scaleWidth = 0D;
            if (maxWidth > 0) {
                scaleWidth = (double)maxWidth / (double)image.Width;
            }

            double scaleHeight = 0D;
            if (maxHeight > 0) {
                scaleHeight = (double)maxHeight / (double)image.Height;
            }

            double scale = 1D;
            if (scaleWidth > 0 && scaleWidth < 1 && scaleHeight > 0 && scaleHeight < 1) {
                scale = (scaleHeight < scaleWidth) ? scaleHeight : scaleWidth;
            } else if (scaleWidth > 0 && scaleWidth < 1) {
                scale = scaleWidth;
            } else if (scaleHeight > 0 && scaleHeight < 1) {
                scale = scaleHeight;
            }

            int newWidth = (int)(scale * image.Width);
            int newHeight = (int)(scale * image.Height);

            return new Size(newWidth, newHeight);
        }

        /// <summary>
        /// <para>Calculates the size of the clipping.</para>
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="maxWidth">Maximum width of the image.</param>
        /// <param name="maxHeight">Maximum height of the image.</param>
        /// <returns>The clipping size</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the maxWidth parameter is less than or equal to 0</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the maxHeight parameter is less than or equal to 0</exception>
        public static Size CalculateClippingSize(this Image image, int maxWidth, int maxHeight) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if (maxWidth <= 0) {
                throw new ArgumentOutOfRangeException("maxWidth", "maxWidth can not be less than or equal to 0");
            }

            if (maxHeight <= 0) {
                throw new ArgumentOutOfRangeException("maxHeight", "maxHeight can not be less than or equal to 0");
            }

            double scaleWidth = (double)maxWidth / (double)image.Width;
            double scaleHeight = (double)maxHeight / (double)image.Height;

            double scale = 1D;
            if (scaleWidth > 0 && scaleWidth < 1 && scaleHeight > 0 && scaleHeight < 1) {
                scale = (scaleHeight < scaleWidth) ? scaleWidth : scaleHeight;
            }

            int newWidth = (int)(scale * image.Width);
            int newHeight = (int)(scale * image.Height);

            return new Size(newWidth, newHeight);
        }

        /// <summary>
        /// <para>Calculates the clip rectangle.</para>
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="maxWidth">Maximum width of the image.</param>
        /// <param name="maxHeight">Maximum height of the image.</param>
        /// <returns>The clip rectangle</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the maxWidth parameter is less than or equal to 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the maxHeight parameter is less than or equal to 0.</exception>
        public static Rectangle CalculateClipRectangle(this Image image, int maxWidth, int maxHeight) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if (maxWidth <= 0) {
                throw new ArgumentOutOfRangeException("maxWidth", "maxWidth can not be less than or equal to 0");
            }

            if (maxHeight <= 0) {
                throw new ArgumentOutOfRangeException("maxHeight", "maxHeight can not be less than or equal to 0");
            }

            Point topLeft = new Point();
            Size clippingSize = image.CalculateClippingSize(maxWidth, maxHeight);

            if (clippingSize.Width != maxWidth) {
                int widthDif = maxWidth - clippingSize.Width;
                topLeft.X = widthDif / 2;
            }

            if (clippingSize.Height != maxHeight) {
                int heightDif = maxHeight - clippingSize.Height;
                topLeft.Y = heightDif / 2;
            }

            return new Rectangle(topLeft, clippingSize);
        }

        /// <summary>
        /// <para>Saves the Image to a Stream.</para>
        /// </summary>
        /// <param name="image">The Image you would like to save.</param>
        /// <param name="stream">The Stream you would like to save the image to.</param>
        /// <param name="imageFormat">The ImageFormat you would like to use.</param>
        /// <param name="outputQuality">The output quality you would like to use. A value between 0L and 100L.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the stream parameter is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the imageFormat parameter is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the outputQuality parameter is less than 0 or greater than 100.</exception>
        public static void SaveToMemoryStream(this Image image, Stream stream, ImageFormat imageFormat, long outputQuality) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            if (stream == null) {
                throw new ArgumentNullException("stream");
            }

            if (imageFormat == null) {
                throw new ArgumentNullException("imageFormat");
            }

            if (outputQuality < 0 || outputQuality > 100) {
                throw new ArgumentOutOfRangeException("outputQuality", "outputQuality can not be less than 0 or greater than 100");
            }

            if (imageFormat.Equals(ImageFormat.Jpeg) && outputQuality != 75L) {
                using (EncoderParameters encoderParameters = new EncoderParameters(1)) {
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, outputQuality);
                    image.Save(stream, imageFormat.GetImageCodecInfo(), encoderParameters);
                }
            } else {
                image.Save(stream, imageFormat);
            }
        }

        /// <summary>
        /// Determines whether or not the image is quantizable.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>True if the image is Quantizeable; otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        public static bool Quantizeable(this Image image) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            bool retVal = false;

            if (image.RawFormat.Guid == ImageFormat.Png.Guid || image.RawFormat.Guid == ImageFormat.Gif.Guid) {
                retVal = true;
            }

            return retVal;
        }
    }
}