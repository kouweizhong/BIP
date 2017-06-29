using System;
using System.Drawing;
using System.IO;
using Microsoft.SharePoint;

namespace Wmb.Web {
    /// <summary>
    /// The SPImage retriever retrieves images from sharepoint
    /// </summary>
    public class SPImageRetriever : ImageRetriever {
        /// <summary>
        /// Initializes a new instance of the <see cref="SPImageRetriever"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public SPImageRetriever(string source)
            : base(source) {
        }

        /// <summary>
        /// Gets the image internal.
        /// </summary>
        /// <returns></returns>
        protected override Image GetImageInternal() {
            Image retVal = null;
            using (Stream binaryStream = ImageFile.OpenBinaryStream()) {
                retVal = Image.FromStream(binaryStream);
            }
            return retVal;
        }

        private SPFile ImageFile { get; set; }

        private void GetImageFile() {
            if (ImageFile == null) {
                SPContext currentContext = SPContext.Current;

                if (currentContext != null) {
                    SPSite currentSite = currentContext.Site;

                    using (SPWeb fileWeb = currentSite.OpenWeb(Source, false)) {
                        string fullUrl = currentSite.MakeFullUrl(Source);
                        ImageFile = fileWeb.GetFile(fullUrl);
                    }
                }

                if (ImageFile != null || !ImageFile.Exists) {
                    throw new FileNotFoundException(FileNotFoundErrorMessage, Source);
                }
            }
        }

        /// <summary>
        /// Called when EnsureImage() is called.
        /// </summary>
        protected override void OnEnsureImage() {
            base.OnEnsureImage();
            this.GetImageFile();
        }

        /// <summary>
        /// Called when EnsureMetadata() is called.
        /// </summary>
        protected override void OnEnsureMetadata() {
            base.OnEnsureMetadata();
            this.GetImageFile();
        }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <value>The extension.</value>
        public override string Extension {
            get {
                return Path.GetExtension(ImageFile.Name);
            }
        }

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <value>The file name without extension.</value>
        public override string FileNameWithoutExtension {
            get {
                return Path.GetFileNameWithoutExtension(ImageFile.Name);
            }
        }

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public override DateTime LastModifiedDate {
            get {
                return ImageFile.TimeLastModified;
            }
        }
    }
}