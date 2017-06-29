using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// The sepia transform can be used to turn an image into sepia.
    /// </summary>
    public class SepiaTransform : ColorMatrixTransform {
        /// <summary>
        /// Initializes a new instance of the <see cref="SepiaTransform"/> class.
        /// </summary>
        public SepiaTransform()
            : base() {
        }

        /// <summary>
        /// Gets the color matrix.
        /// </summary>
        /// <returns></returns>
        public override ColorMatrix GetColorMatrix() {
            return new ColorMatrix(new float[][]{
                            new float[] {0.393f, 0.349f, 0.272f, 0f, 0f},
                            new float[] {0.769f, 0.686f, 0.534f, 0f, 0f},
                            new float[] {0.189f, 0.168f, 0.131f, 0f, 0f},
                            new float[] {    0f,     0f,     0f, 1f, 0f},
                            new float[] {    0f,     0f,     0f, 0f, 1f}});
        }

        /// <summary>
        /// Gets the position where the transform should took place in respect to other transforms.
        /// </summary>
        /// <value>400</value>
        public override int Position {
            get {
                return 400;
            }
        }
    }
}