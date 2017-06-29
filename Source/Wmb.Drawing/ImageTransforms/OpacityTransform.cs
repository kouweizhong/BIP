using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// The opacity transform can be used to adjust the opacity of an image.
    /// </summary>
    public class OpacityTransform : ColorMatrixTransform {
        private const float defaultOpacity = 0;

        /// <summary>
        /// Creates the opacity matrix.
        /// </summary>
        /// <param name="opacity">The opacity.</param>
        /// <returns>An opacity matrix that can be used to change the opacity of an image</returns>
        public static ColorMatrix CreateOpacityMatrix(float opacity) {
            float trans = (1f - opacity);

            ColorMatrix retVal = new ColorMatrix(new float[][]{
                            new float[] {1f,  0,  0,     0,  0},
                            new float[] { 0, 1f,  0,     0,  0},
                            new float[] { 0,  0, 1f,     0,  0},
                            new float[] { 0,  0,  0, trans,  0},
                            new float[] { 0,  0,  0,     0, 1f}});

            return retVal;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="OpacityTransform"/> class.
        /// </summary>
        public OpacityTransform()
            : this(defaultOpacity) {
        }

        /// <summary>
        /// Gets the color matrix.
        /// </summary>
        /// <returns></returns>
        public override ColorMatrix GetColorMatrix() {
            return CreateOpacityMatrix(this.Opacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpacityTransform"/> class.
        /// </summary>
        /// <param name="opacity">The opacity.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the opacity parameter less than 0 or greater than 1.</exception>
        public OpacityTransform(float opacity)
            : base() {
            if (opacity < 0 || opacity > 1) {
                throw new ArgumentOutOfRangeException("opacity", "Opacity can not contain a value less than 0 or greater than 1.");
            }

            this.Opacity = opacity;
        }

        /// <summary>
        /// Gets the position where the transform should took place in respect to other transforms.
        /// </summary>
        /// <value>700</value>
        public override int Position {
            get {
                return 700;
            }
        }

        private float opacity;

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value is less than 0 or greater than 1.</exception>
        public float Opacity {
            get {
                return opacity;
            }
            set {
                if (value < 0 || value > 1) {
                    throw new ArgumentOutOfRangeException("value", "Opacity can not contain a value less than 0 or greater than 1.");
                }

                opacity = value;
            }
        }

        /// <summary>
        /// Transforms the specified image.
        /// </summary>
        /// <param name="image">The image to transform.</param>
        protected override void TransformCore(Image image) {
            if (this.Opacity != defaultOpacity) {
                ColorMatrix opacityMatrix = GetColorMatrix();
                image.ApplyColorMatrix(opacityMatrix, true);
            }
        }
    }
}