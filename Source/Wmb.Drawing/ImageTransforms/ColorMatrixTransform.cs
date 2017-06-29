using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace Wmb.Drawing {
    /// <summary>
    /// The abstract base class for image transforms that apply a color matrix
    /// </summary>
    public abstract class ColorMatrixTransform : ImageTransform {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorMatrixTransform"/> class.
        /// </summary>
        protected ColorMatrixTransform():base() {
        }

        /// <summary>
        /// Gets the color matrix.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification="It is not appropriate because a property can not be dependent on another property. In many transforms the matrix get calculated based on other properties.")]
        public abstract ColorMatrix GetColorMatrix();

        /// <summary>
        /// Transforms the specified image.
        /// </summary>
        /// <param name="image">The image to transform.</param>
        protected override void TransformCore(Image image) {
            image.ApplyColorMatrix(GetColorMatrix());
        }
    }
}
