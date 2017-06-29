using System;
using System.Drawing;
using System.IO;

namespace Wmb.Web {
    /// <summary>
    /// This base class is used for implementing different filesources for the imagehandler
    /// </summary>
    public abstract class ImageRetriever {
        /// <exclude />
        protected static readonly string FileNotFoundErrorMessage = "ImageRetriever could not retrieve the file.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRetriever"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        protected ImageRetriever(string source) {
            if (string.IsNullOrEmpty(source)) {
                throw new ArgumentNullException("source");
            }

            this.Source = source;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>The source.</value>
        protected string Source { get; private set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <returns>The image</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification="Retrieving an image will probably be a costly operation.")]
        public Image GetImage() {
            Image retVal = null;
            try {
                retVal = GetImageInternal();
                if (retVal == null) {
                    throw new FileNotFoundException(FileNotFoundErrorMessage, Source);
                }
            }
            catch (Exception ex) {
                throw new FileNotFoundException(FileNotFoundErrorMessage, Source, ex);
            }

            return retVal;
        }

        private bool isImageEnsured;
        /// <summary>
        /// Ensures the image is loaded from the backing store.
        /// </summary>
        public void EnsureImage() {
            if (!isImageEnsured) {
                this.OnEnsureImage();
                isImageEnsured = true;
            }
        }

        /// <summary>
        /// Called when EnsureImage() is called.
        /// </summary>
        protected virtual void OnEnsureImage() {
        }

        private bool isMetadataEnsured;
        /// <summary>
        /// Ensures the metadata is loaded from the backing store.
        /// </summary>
        public void EnsureMetadata() {
            if (!isMetadataEnsured) {
                this.OnEnsureMetadata();
                isMetadataEnsured = true;
            }
        }

        /// <summary>
        /// Called when EnsureMetadata() is called.
        /// </summary>
        protected virtual void OnEnsureMetadata() {
        }

        /// <summary>
        /// Gets the image internal.
        /// </summary>
        /// <returns>The image</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "I expect that retrieving an image will be a costly operation.")]
        protected abstract Image GetImageInternal();


        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <value>The extension.</value>
        public abstract string Extension { get; }

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <value>The file name without extension.</value>
        public abstract string FileNameWithoutExtension { get; }

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public abstract DateTime LastModifiedDate { get; }
    }
}