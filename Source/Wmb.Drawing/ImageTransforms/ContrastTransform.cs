using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// The contrast transform can be used to adjust the contrast of an image.
    /// </summary>
    public class ContrastTransform : ColorMatrixTransform {
        private const float defaultContrast = 1;

        /// <summary>
        /// Creates the contrast matrix.
        /// </summary>
        /// <param name="contrast">The contrast.</param>
        /// <returns>A contrast matrix that can be used to change the contrast of an image</returns>
        public static ColorMatrix CreateContrastMatrix(float contrast) {
            float trans = (1f - contrast) / 2f;

            ColorMatrix retVal = new ColorMatrix(new float[][]{
                            new float[] {contrast,       0f,       0f, 0f, 0f},
                            new float[] {      0f, contrast,       0f, 0f, 0f},
                            new float[] {      0f,       0f, contrast, 0f, 0f},
                            new float[] {      0f,       0f,       0f, 1f, 0f},
                            new float[] {   trans,    trans,    trans, 0f, 1f}});

            return retVal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContrastTransform"/> class.
        /// </summary>
        public ContrastTransform()
            : this(defaultContrast) {
        }

        /// <summary>
        /// Gets the color matrix.
        /// </summary>
        /// <returns></returns>
        public override ColorMatrix GetColorMatrix() {
            return CreateContrastMatrix(this.Contrast);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContrastTransform"/> class.
        /// </summary>
        /// <param name="contrast">The contrast.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the contrast parameter less than 0 or greater than 3.</exception>
        public ContrastTransform(float contrast)
            : base() {
            if (contrast < 0 || contrast > 3) {
                throw new ArgumentOutOfRangeException("contrast", "Contrast can not contain a value less than 0 or greater than 3.");
            }

            this.Contrast = contrast;
        }

        /// <summary>
        /// Gets the position where the transform should took place in respect to other transforms.
        /// </summary>
        /// <value>600</value>
        public override int Position {
            get {
                return 600;
            }
        }

        private float contrast;
        /// <summary>
        /// Gets or sets the contrast.
        /// </summary>
        /// <value>The contrast.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value is less than 0 or greater than 3.</exception>
        public float Contrast {
            get {
                return contrast;
            }
            set {
                if (value < 0 || value > 3) {
                    throw new ArgumentOutOfRangeException("value", "Contrast can not contain a value less than 0 or greater than 3.");
                }

                contrast = value;
            }
        }

        /// <summary>
        /// Transforms the specified image.
        /// </summary>
        /// <param name="image">The image to transform.</param>
        protected override void TransformCore(Image image) {
            if (this.Contrast != defaultContrast) {
                base.TransformCore(image);
            }
        }
    }
}