using System;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Wmb.Web.WebControls {
    /// <exclude />
    public class IBetterImageDesigner : ControlDesigner {
        /// <exclude />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We do not ever want to crash the designer. So we output any error with the help of GetErrorDesignTimeHtml(Exception ex).")]
        public override string GetDesignTimeHtml() {
            WebControl webControl = (WebControl)Component;
            IBetterImage iBetterImage = (IBetterImage)Component;
            ImageSettings imageSettings = iBetterImage.ImageSettings;

            string designTimeHtml = string.Empty;
            Unit oldWidth = webControl.Width;
            Unit oldHeight = webControl.Height;
            string oldFilter = webControl.Style["filter"];

            try {
                if (imageSettings.MaxWidth > 0 && webControl.Width == Unit.Empty) {
                    webControl.Width = Unit.Pixel(imageSettings.MaxWidth);
                }

                if (imageSettings.MaxHeight > 0 && webControl.Height == Unit.Empty) {
                    webControl.Height = Unit.Pixel(imageSettings.MaxHeight);
                }

                string basicImgTransform = string.Empty;

                if (imageSettings.Grayscale) {
                    basicImgTransform += "grayscale=1,";
                }

                if (imageSettings.Negative) {
                    basicImgTransform += "invert=1,";
                }

                if (basicImgTransform.Length > 0) {
                    webControl.Style["filter"] += "progid:DXImageTransform.Microsoft.BasicImage(" + basicImgTransform + ")";
                }

                designTimeHtml = base.GetDesignTimeHtml();
            }
            catch (Exception ex) {
                designTimeHtml = GetErrorDesignTimeHtml(ex);
            }
            finally {
                webControl.Width = oldWidth;
                webControl.Height = oldHeight;
                webControl.Style["filter"] = oldFilter;
            }

            if (designTimeHtml.Length == 0) {
                designTimeHtml = GetEmptyDesignTimeHtml();
            }

            return designTimeHtml;
        }
    }
}
