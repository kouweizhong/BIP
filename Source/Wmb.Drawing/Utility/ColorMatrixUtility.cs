using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// The ColorMatrixUtility class holds the extensions and/or helpermethods for the ColorMatrix class.
    /// </summary>
    public static class ColorMatrixUtility {
        /// <summary>
        /// Multiplies the specified ColorMatrices.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static ColorMatrix Multiply(this ColorMatrix left, ColorMatrix right) {
            if(left == null) {
                throw new ArgumentNullException("left");
            }

            if(right == null) {
                throw new ArgumentNullException("right");
            }

            ColorMatrix retVal = new ColorMatrix();
            int size = 5;

            float[] column = new float[5];
            for (int j = 0; j < 5; j++) {
                for (int k = 0; k < 5; k++) {
                    column[k] = left[k, j];
                }

                for (int i = 0; i < 5; i++) {
                    float s = 0;
                    for (int k = 0; k < size; k++) {
                        s += right[i, k] * column[k];
                    }
                    retVal[i, j] = s;
                }
            }

            return retVal;
        }
    }
}
