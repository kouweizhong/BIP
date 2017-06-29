using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// The Compression Transform compresses the color space.
    /// </summary>
    public class CompressionTransform: ColorMatrixTransform {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionTransform"/> class.
        /// </summary>
        public CompressionTransform()
            : base() {
        }

        /// <summary>
        /// Gets the color matrix.
        /// </summary>
        /// <returns></returns>
        public override ColorMatrix GetColorMatrix() {
            return new ColorMatrix(new float[][]{
                            new float[] {0.992f,     0f,     0f, 0f, 0f},
                            new float[] {    0f, 0.992f,     0f, 0f, 0f},
                            new float[] {    0f,     0f, 0.992f, 0f, 0f},
                            new float[] {    0f,     0f,     0f, 1f, 0f},
                            new float[] {0.004f, 0.004f, 0.004f, 0f, 1f}});
        }
    }
}
