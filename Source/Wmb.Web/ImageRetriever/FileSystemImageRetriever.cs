using System;
using System.Drawing;
using System.IO;
using System.Web;

namespace Wmb.Web {
    /// <summary>
    /// The FileSystemImageRetriever retriever retrieves images from the filesystem
    /// </summary>
    public sealed class FileSystemImageRetriever : ImageRetriever {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemImageRetriever"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public FileSystemImageRetriever(string source)
            : base(source) {
        }
        
        private FileInfo ImageFile{get; set;}

        private void GetImageFile() {
            if (ImageFile == null) {
                string filename = Source;

                HttpContext currentContext = HttpContext.Current;
                if (currentContext != null) {
                    filename = HttpContext.Current.Server.MapPath(Source);
                }

                if (!File.Exists(filename)) {
                    throw new FileNotFoundException(FileNotFoundErrorMessage, Source);
                }

                ImageFile = new FileInfo(filename);
            }
        }

        /// <summary>
        /// Called when EnsureImage() is called.
        /// </summary>
        protected override void OnEnsureImage() {
            base.OnEnsureImage();
            GetImageFile();
        }

        /// <summary>
        /// Called when EnsureMetadata() is called.
        /// </summary>
        protected override void OnEnsureMetadata() {
            base.OnEnsureMetadata();
            GetImageFile();
        }

        /// <summary>
        /// Gets the image internal.
        /// </summary>
        /// <returns>The image</returns>
        protected override Image GetImageInternal() {
            return Image.FromFile(ImageFile.FullName);
        }


        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public override DateTime LastModifiedDate {
            get {
                return ImageFile.LastWriteTimeUtc;
            }
        }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <value>The extension.</value>
        public override string Extension {
            get {
                return ImageFile.Extension;
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
    }
}
