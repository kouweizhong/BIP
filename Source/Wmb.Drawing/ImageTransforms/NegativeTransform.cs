using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// The negative transform can be used to turn an image into negative.
    /// </summary>
    public class NegativeTransform : ColorMatrixTransform {
        /// <summary>
        /// Initializes a new instance of the <see cref="NegativeTransform"/> class.
        /// </summary>
        public NegativeTransform()
            : base() {
        }

        /// <summary>
        /// Gets the color matrix.
        /// </summary>
        /// <returns></returns>
        public override ColorMatrix GetColorMatrix() {
            ColorMatrix retVal = new ColorMatrix(new float[][]{
                            new float[] {-1f,  0f,  0f, 0f, 0f},
                            new float[] { 0f, -1f,  0f, 0f, 0f},
                            new float[] { 0f,  0f, -1f, 0f, 0f},
                            new float[] { 0f,  0f,  0f, 1f, 0f},
                            new float[] { 1f,  1f,  1f, 0f, 1f}});

            if (!DisableColorCompression) {
                ColorMatrix compressionMatrix = new CompressionTransform().GetColorMatrix();
                retVal = compressionMatrix.Multiply(retVal);
            }

            return retVal;
        }

        /// <summary>
        /// Gets the position where the transform should took place in respect to other transforms.
        /// </summary>
        /// <value>300</value>
        public override int Position {
            get {
                return 300;
            }
        }

        /// <summary>
        /// <para>Gets or sets a value indicating whether to disable color compression.</para>
        /// <para>Color compression delivers a reversible negative a the loss of the the highest color values.</para>
        /// </summary>
        /// <value>
        /// 	<c>true</c> to disable color compression; otherwise, <c>false</c>.
        /// </value>
        public bool DisableColorCompression {
            get;
            set;
        }
    }
}