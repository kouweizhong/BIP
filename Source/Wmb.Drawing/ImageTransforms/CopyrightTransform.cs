using System;
using System.Drawing;
using System.Drawing.Text;

namespace Wmb.Drawing {
    /// <summary>
    /// The copyright transform can be used to add a copyright text to an image.
    /// </summary>
    public class CopyrightTransform : ImageTransform {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyrightTransform"/> class.
        /// </summary>
        /// <param name="copyrightText">The copyright text.</param>
        public CopyrightTransform(string copyrightText)
            : this(copyrightText, 0) {
        }

        /// <summary>
        /// Gets the position where the transform should took place in respect to other transforms.
        /// </summary>
        /// <value>100</value>
        public override int Position {
            get {
                return 100;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyrightTransform"/> class.
        /// </summary>
        /// <param name="copyrightText">The copyright text.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the copyrightText parameter is null or string.Empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the fontSize parameter is less than 0.</exception>
        public CopyrightTransform(string copyrightText, int fontSize) {
            if (string.IsNullOrEmpty(copyrightText)) {
                throw new ArgumentNullException("copyrightText");
            }

            if (fontSize < 0) {
                throw new ArgumentOutOfRangeException("fontSize", "fontSize can not be less than 0.");
            }

            this.CopyrightText = copyrightText;
            this.FontSize = fontSize;
        }

        private string copyrightText;
        /// <summary>
        /// Gets or sets the copyright text.
        /// </summary>
        /// <value>The copyright text.</value>
        /// <exception cref="System.ArgumentNullException">Thrown when the value is null or string.Empty.</exception>
        public string CopyrightText {
            get {
                return copyrightText;
            }
            set {
                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentNullException("value");
                }

                copyrightText = value;
            }
        }

        private int fontSize;
        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value is less than 0.</exception>
        public int FontSize {
            get {
                return fontSize;
            }
            set {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException("value", "FontSize can not be less then 0.");
                }

                fontSize = value;
            }
        }

        /// <summary>
        /// Transforms the specified image.
        /// </summary>
        /// <param name="image">The image to transform.</param>
        /// <exception cref="System.ArgumentException">Thrown when the image has an indexed pixel format.</exception>
        protected override void TransformCore(Image image) {
            if (image.HasIndexedPixelFormat()) {
                throw new ArgumentException("A Graphics object cannot be created from an image that has an indexed pixel format. This makes it impossible to add a copyright text.");
            }

            using (Graphics graphics = Graphics.FromImage(image)) {
                int imageHeight = image.Height;
                int imageWidth = image.Width;

                SizeF sizeF = new SizeF();

                if (fontSize == 0) {
                    int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
                    for (int i = 0; i < sizes.Length; i++) {
                        fontSize = sizes[i];

                        using (Font font = new Font("arial", fontSize, FontStyle.Bold)) {
                            sizeF = graphics.MeasureString(copyrightText, font);
                        }

                        if ((ushort)sizeF.Width < (ushort)imageWidth) {
                            break;
                        }
                    }
                }

                using (Font font = new Font("arial", fontSize, FontStyle.Bold)) {
                    if (sizeF.Width == 0) {
                        sizeF = graphics.MeasureString(copyrightText, font);
                    }

                    int pixelsFromBottom = (int)(imageHeight * .05);
                    float yPosFromBottom = ((imageHeight - pixelsFromBottom) - (sizeF.Height / 1.5f));
                    float xCenterOfImg = (imageWidth / 2);

                    using (StringFormat stringFormat = new StringFormat { Alignment = StringAlignment.Center }) {
                        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                        using (SolidBrush shadowSolidBrush = new SolidBrush(Color.FromArgb(153, 0, 0, 0))) {
                            graphics.DrawString(copyrightText, font, shadowSolidBrush, new PointF(xCenterOfImg + 1, yPosFromBottom + 1), stringFormat);
                        }

                        using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255))) {
                            graphics.DrawString(copyrightText, font, solidBrush, new PointF(xCenterOfImg, yPosFromBottom), stringFormat);
                        }
                    }

                    graphics.Save();
                }
            }
        }
    }
}