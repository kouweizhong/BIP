using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Wmb.Drawing {
    /// <summary>
    /// The GraphicsUtility class holds the extensions and/or helpermethods for the Graphics class.
    /// </summary>
    public static class GraphicsUtility {
        /// <summary>
        /// Applies the graphics quality setting.
        /// </summary>
        /// <param name="graphics">The Graphics object which to apply the GraphicsQuality settings to.</param>
        /// <param name="graphicsQuality">The GraphicsQuality to apply.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the graphics parameter is null</exception>
        public static void ApplyGraphicsQualitySetting(this Graphics graphics, GraphicsQuality graphicsQuality) {
            if (graphics == null) {
                throw new ArgumentNullException("graphics");
            }

            switch (graphicsQuality) {
                case GraphicsQuality.Low:
                    graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    graphics.SmoothingMode = SmoothingMode.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.Low;
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    break;

                case GraphicsQuality.Medium:
                    graphics.PixelOffsetMode = PixelOffsetMode.Half;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.High;
                    graphics.CompositingQuality = CompositingQuality.Default;
                    break;

                case GraphicsQuality.High:
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    break;

                default:
                    graphics.PixelOffsetMode = PixelOffsetMode.Default;
                    graphics.SmoothingMode = SmoothingMode.None;
                    graphics.InterpolationMode = InterpolationMode.Bilinear;
                    graphics.CompositingQuality = CompositingQuality.Default;
                    break;
            }
        }
    }
}