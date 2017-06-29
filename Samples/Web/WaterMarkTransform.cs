using System.Drawing;
using System.Drawing.Imaging;
using System.Web;

using Wmb.Web;
using Wmb.Drawing;

namespace Wmb.TestWeb {
    public class WaterMarkTransform : ImageTransform, ICustomDataConsumer {
        public WaterMarkTransform()
            : base() {
            Opacity = 0.5F;
        }

        private float Opacity {
            get;
            set;
        }

        protected override void TransformCore(Image image) {
            string waterMarkImagePath = HttpContext.Current.Server.MapPath(@"~\imgs\confidential.png");
            using (Image waterMarkImage = Bitmap.FromFile(waterMarkImagePath))
            using (Graphics graphics = Graphics.FromImage(image)) {
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(OpacityTransform.CreateOpacityMatrix(Opacity));

                graphics.ApplyGraphicsQualitySetting(GraphicsQuality.High);
                graphics.DrawImage(waterMarkImage, new Rectangle(0, 0, image.Width, image.Height), 0, 0, waterMarkImage.Width, waterMarkImage.Height, GraphicsUnit.Pixel, attributes);
                graphics.Save();
            }
        }

        public void SetCustomData(string data) {
            float opacity;
            if (float.TryParse(data, out opacity)) {
                Opacity = opacity;
            }
        }
    }
}
