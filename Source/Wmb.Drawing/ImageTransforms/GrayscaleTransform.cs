using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// The grayscale transform can be used to turn an image into grayscale.
    /// </summary>
    public class GrayscaleTransform : ColorMatrixTransform {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayscaleTransform"/> class.
        /// </summary>
        public GrayscaleTransform()
            : base() {
        }

        /// <summary>
        /// Gets the color matrix.
        /// </summary>
        /// <returns></returns>
        public override ColorMatrix GetColorMatrix() {
            return new ColorMatrix(new float[][]{
                            new float[] {0.3086f, 0.3086f, 0.3086f, 0f, 0f},
                            new float[] {0.6094f, 0.6094f, 0.6094f, 0f, 0f},
                            new float[] {0.0820f, 0.0820f, 0.0820f, 0f, 0f},
                            new float[] {     0f,      0f,      0f, 1f, 0f},
                            new float[] {     0f,      0f,      0f, 0f, 1f}});
        }

        /// <summary>
        /// Gets the position where the transform should took place in respect to other transforms.
        /// </summary>
        /// <value>200</value>
        public override int Position {
            get {
                return 200;
            }
        }
    }
}