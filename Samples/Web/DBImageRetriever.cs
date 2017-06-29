using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.Configuration;
using Wmb.Web;

namespace Wmb.TestWeb {
    public class DBImageRetriever : ImageRetriever, ICustomDataConsumer {
        public DBImageRetriever(string source)
            : base(source) {
        }

        /// <summary>
        /// Gets or sets the database id of the image.
        /// </summary>
        /// <value>The image id.</value>
        private int ImageId {
            get;
            set;
        }

        /// <summary>
        /// Sets the custom data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetCustomData(string data) {
            int imageId;
            if (int.TryParse(data, out imageId)) {
                ImageId = imageId;
            }
        }

        private Image Image { get; set; }

        protected override void OnEnsureImage() {
            base.OnEnsureImage();

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["PhotoDbConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand("select Image from Photos where Id = @id", conn)) {
                SqlParameter idParam = new SqlParameter("@id", this.ImageId);
                command.Parameters.Add(idParam);

                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleRow)) {
                    if (reader.HasRows) {
                        reader.Read();

                        using (MemoryStream stream = new MemoryStream((byte[])reader["Image"])) {
                            this.Image = Image.FromStream(stream);
                        }
                    }
                    else {
                        throw new FileNotFoundException(FileNotFoundErrorMessage, Source);
                    }
                }
                conn.Close();
            }
        }

        private string FileName { get; set; }

        protected override void OnEnsureMetadata() {
            base.OnEnsureMetadata();

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["PhotoDbConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand("select FileName from Photos where Id = @id", conn)) {
                SqlParameter idParam = new SqlParameter("@id", this.ImageId);
                command.Parameters.Add(idParam);

                conn.Open();
                object returnValue = command.ExecuteScalar();

                if (returnValue != null) {
                    this.FileName = (string)returnValue;
                }
                else {
                    throw new FileNotFoundException(FileNotFoundErrorMessage, Source);
                }

                conn.Close();
            }
        }


        /// <summary>
        /// Gets the image internal.
        /// </summary>
        /// <returns>The image</returns>
        protected override Image GetImageInternal() {
            return this.Image;
        }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <value>The extension.</value>
        public override string Extension {
            get {
                this.EnsureMetadata();
                return Path.GetExtension(this.FileName);
            }
        }

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <value>The file name without extension.</value>
        public override string FileNameWithoutExtension {
            get {
                this.EnsureMetadata();
                return Path.GetFileNameWithoutExtension(this.FileName);
            }
        }

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public override DateTime LastModifiedDate {
            get {
                //images in the database don't ever change so we set a fixed last modified date
                return new DateTime(2001, 1, 1);
            }
        }

    }
}