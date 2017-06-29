using System.Drawing;
using System;

namespace Wmb.Drawing {
    /// <summary>
    /// <para>The abstract ImageTransform class is the base class to be used for all image transform classes.</para>
    /// <para>Use the <see cref="System.Drawing.Graphics"/> class to transform an image. Do not asign a new <see cref="System.Drawing.Image"/> to the Image parameter in the Transform method. This might cause serious memory leaks!</para>
    /// </summary>
    public abstract class ImageTransform {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTransform"/> class.
        /// </summary>
        protected ImageTransform() {
        }


        /// <summary>
        /// Gets the position where the transform should took place in respect to other transforms.
        /// </summary>
        /// <value>1000</value>
        public virtual int Position {
            get {
                return 1000;
            }
        }

        /// <summary>
        /// Transforms the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the image parameter is null.</exception>
        public void Transform(Image image) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            TransformCore(image);
        }

        /// <summary>
        /// Transforms the specified image.
        /// </summary>
        /// <param name="image">The image to transform.</param>
        protected abstract void TransformCore(Image image);
    }
}