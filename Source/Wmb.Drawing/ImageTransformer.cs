using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Wmb.Drawing {
    /// <summary>
    /// The ImageFactory class is a so called aggregate component with which you can create resized and transformed images in the simplest way.
    /// </summary>
    public class ImageTransformer {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTransformer"/> class.
        /// </summary>
        public ImageTransformer() {
            this.GraphicsQuality = GraphicsQuality.Default;
            this.Contrast = 1F;
            CustomTransforms = new ImageTransformCollection();
        }

        /// <summary>
        /// <para>The resize quality of the resulting image.</para>
        /// </summary>
        public GraphicsQuality GraphicsQuality { get; set; }

        /// <summary>
        /// <para>Gets or sets the copyright text to write on the resulting image.</para>
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// <para>Gets or sets the size of the copyright text to write on the resulting image.</para>
        /// </summary>
        public int CopyrightSize { get; set; }

        /// <summary>
        /// <para>The maximum width of the resulting image.</para>
        /// </summary>
        public int MaxWidth { get; set; }

        /// <summary>
        /// <para>The maximum height of the resulting image.</para>
        /// </summary>
        public int MaxHeight { get; set; }

        /// <summary>
        /// <para>Whether or not you would like your resulting image to be grayscaled.</para>
        /// </summary>
        public bool Grayscale { get; set; }

        /// <summary>
        /// <para>Whether or not you would like your resulting image to be a negative.</para>
        /// </summary>
        public bool Negative { get; set; }

        /// <summary>
        /// <para>Whether or not you would like to turn the resulting image into sepia.</para>
        /// </summary>
        public bool Sepia { get; set; }

        /// <summary>
        /// <para>Whether or not you would like the image to be clipped to size.</para>
        /// </summary>
        public bool Clip { get; set; }

        /// <summary>
        /// <para>Whether or not you would like to quantize your GIF images.</para>
        /// <remarks>It greatly enhances the quality at the cost of processingtime.</remarks>
        /// </summary>
        public bool Quantize { get; set; }

        /// <summary>
        /// <para>Whether or not you would like to maintain the original palette.</para>
        /// <remarks>This keeps the original colorpalette which is great for resizing GIF and PNG.</remarks>
        /// </summary>
        public bool MaintainPalette { get; set; }

        /// <summary>
        /// The custom transforms to apply after the default transforms have been applied.
        /// </summary>
        /// <value>The custom transforms.</value>
        public ImageTransformCollection CustomTransforms { get; private set; }

        private float m_brightness;
        /// <summary>
        /// <para>The brightness of the resulting image.</para>
        /// <remarks>Choose a value between -1 and 1 where 0 is default.</remarks>
        /// </summary>
        public float Brightness {
            get { return m_brightness; }
            set {
                if (m_brightness != value) {
                    if (value < -1 || value > 1) {
                        throw new ArgumentOutOfRangeException("value", "Brightness can contain a value between -1 and 1.");
                    }

                    m_brightness = value;
                }
            }
        }

        private float m_contrast;
        /// <summary>
        /// <para>The contrast of the resulting image.</para>
        /// <remarks>Choose a value between 0 and 3 where 1 is default.</remarks>
        /// </summary>
        public float Contrast {
            get { return m_contrast; }
            set {
                if (m_contrast != value) {
                    if (value < 0 || value > 3) {
                        throw new ArgumentOutOfRangeException("value", "Contrast can contain a value between 0 and 3.");
                    }

                    m_contrast = value;
                }
            }
        }

        private float m_opacity;
        /// <summary>
        /// <para>The opacity of the resulting image.</para>
        /// <remarks>Choose a value between 0 and 1 where 0 is default.</remarks>
        /// </summary>
        public float Opacity {
            get { return m_opacity; }
            set {
                if (m_opacity != value) {
                    if (value < 0 || value > 1) {
                        throw new ArgumentOutOfRangeException("value", "Opacity can contain a value between 0 and 1.");
                    }

                    m_opacity = value;
                }
            }
        }

        /// <summary>
        /// Creates a new image from the specified input.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The transformed image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        public Image Transform(Image image) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            Image retVal = (this.Clip) ? image.Clip(this.MaxWidth, this.MaxHeight, this.GraphicsQuality, this.MaintainPalette)
                                     : image.Resize(this.MaxWidth, this.MaxHeight, this.GraphicsQuality, this.MaintainPalette);

            if (!retVal.HasIndexedPixelFormat()) {
                IEnumerable<ImageTransform> imageTransformers = BuiltImageTransformList();
                retVal.Transform(imageTransformers);
            }

            if (image.Quantizeable() && this.Quantize) {
                Image quantizedImage = retVal.OctreeQuantize();
                retVal.Dispose();

                return quantizedImage;
            } else {
                return retVal;
            }
        }

        private IEnumerable<ImageTransform> BuiltImageTransformList() {
            ImageTransformCollection retVal = new ImageTransformCollection();

            string copyrightText = this.Copyright;
            if (!string.IsNullOrEmpty(copyrightText)) {
                int copyrightFontSize = this.CopyrightSize;
                CopyrightTransform copyrightTransform = new CopyrightTransform(copyrightText, copyrightFontSize);
                retVal.Add(copyrightTransform);
            }

            if (this.Grayscale) {
                GrayscaleTransform grayscaleTransform = new GrayscaleTransform();
                retVal.Add(grayscaleTransform);
            }

            if (this.Negative) {
                NegativeTransform negativeTransform = new NegativeTransform();
                retVal.Add(negativeTransform);
            }

            if (this.Sepia) {
                SepiaTransform sepiaTransform = new SepiaTransform();
                retVal.Add(sepiaTransform);
            }

            float brightness = this.Brightness;
            if (brightness != 0) {
                BrightnessTransform brightnessTransform = new BrightnessTransform(brightness);
                retVal.Add(brightnessTransform);
            }

            float contrast = this.Contrast;
            if (contrast != 1) {
                ContrastTransform contrastTransform = new ContrastTransform(contrast);
                retVal.Add(contrastTransform);
            }

            float opacity = this.Opacity;
            if (opacity != 0) {
                OpacityTransform opacityTransform = new OpacityTransform(opacity);
                retVal.Add(opacityTransform);
            }

            return retVal.Union(CustomTransforms).OrderBy(it=>it.Position);
        }
    }
}
