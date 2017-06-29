using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// The brightness transform can be used to adjust the brightness of an image.
    /// </summary>
    public class BrightnessTransform : ColorMatrixTransform {
        private const float defaultBrightness = 0;

        /// <summary>
        /// Creates the brightness matrix.
        /// </summary>
        /// <param name="brightness">The brightness.</param>
        /// <returns>A color matrix that can be used to change the brightness of an image</returns>
        public static ColorMatrix CreateBrightnessMatrix(float brightness) {
            ColorMatrix retVal = new ColorMatrix(new float[][]{
                            new float[] {        1f,          0,          0,  0,  0},
                            new float[] {         0,         1f,          0,  0,  0},
                            new float[] {         0,          0,         1f,  0,  0},
                            new float[] {         0,          0,          0, 1f,  0},
                            new float[] {brightness, brightness, brightness, 1f, 1f}});

            return retVal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrightnessTransform"/> class.
        /// </summary>
        public BrightnessTransform()
            : this(defaultBrightness) {
        }

        /// <summary>
        /// Gets the color matrix.
        /// </summary>
        /// <returns></returns>
        public override ColorMatrix GetColorMatrix() {
            return CreateBrightnessMatrix(this.Brightness);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrightnessTransform"/> class.
        /// </summary>
        /// <param name="brightness">The brightness.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the brightness parameter less than -1 or greater than 1.</exception>
        public BrightnessTransform(float brightness)
            : base() {
            if (brightness < -1 || brightness > 1) {
                throw new ArgumentOutOfRangeException("brightness", "Brightness can not contain a value less than -1 or greater than 1.");
            }

            this.Brightness = brightness;
        }

        /// <summary>
        /// Gets the position where the transform should took place in respect to other transforms.
        /// </summary>
        /// <value>500</value>
        public override int Position {
            get {
                return 500;
            }
        }

        private float brightness;
        /// <summary>
        /// Gets or sets the brightness.
        /// </summary>
        /// <value>Value between -1 and 1</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value is less than -1 or greater than 1.</exception>
        public float Brightness {
            get {
                return brightness;
            }
            set {
                if (value < -1 || value > 1) {
                    throw new ArgumentOutOfRangeException("value", "Brightness can not contain a value less than -1 or greater than 1.");
                }

                brightness = value;
            }
        }

        /// <summary>
        /// Transforms the specified image.
        /// </summary>
        /// <param name="image">The image to transform.</param>
        protected override void TransformCore(Image image) {
            if (this.Brightness != defaultBrightness) {
                base.TransformCore(image);
            }
        }
    }
}