using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wmb.Web.WebControls {
    /// <summary>
    /// <para>ImageLink control inherits from System.Web.UI.WebControls.HyperLink</para>
    /// </summary>
    [
    ToolboxData("<{0}:ImageLink runat=\"server\"></{0}:ImageLink>"),
    ToolboxBitmap(typeof(HyperLink))
    ]
    public class ImageLink : HyperLink, IBetterImage {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLink"/> class.
        /// </summary>
        public ImageLink()
            : base() {
        }

        private ImageSettings imageSettings;
        /// <summary>
        /// <para>The ImageSettings for the ImageHandler.</para>
        /// </summary>
        [
        Category("ImageSettings"),
        Description("The ImageSettings for the ImageHandler."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true)
        ]
        public ImageSettings ImageSettings {
            get {
                if (imageSettings == null) {
                    imageSettings = new ImageSettings();

                    if (IsTrackingViewState) {
                        ((IStateManager)imageSettings).TrackViewState();
                    }
                }

                return imageSettings;
            }
        }

        /// <summary>
        /// Binds a data source to the server control's child controls.
        /// </summary>
        protected override void DataBindChildren() {
            base.DataBindChildren();
            ImageSettings.DataBind();
        }

        /// <exclude />
        protected override void AddAttributesToRender(HtmlTextWriter writer) {
            if (base.NavigateUrl.Length > 0 && !DesignMode) {
                string resolvedUrl = ResolveUrl(base.NavigateUrl);
                base.NavigateUrl = ImageSettings.ToImgUrl(resolvedUrl);
            }

            if (ImageSettings.DisableRightClick) {
                writer.AddAttribute("oncontextmenu", "return false;");
            }

            base.AddAttributesToRender(writer);
        }

        #region state management
        /// <exclude />
        protected override void LoadViewState(object savedState) {
            Pair p = savedState as Pair;

            base.LoadViewState(p.First);
            ((IStateManager)ImageSettings).LoadViewState(p.Second);
        }

        /// <exclude />
        protected override object SaveViewState() {
            object baseState = base.SaveViewState();
            object thisState = null;

            if (imageSettings != null) {
                thisState = ((IStateManager)imageSettings).SaveViewState();
            }

            return new Pair(baseState, thisState);
        }

        /// <exclude />
        protected override void TrackViewState() {
            if (imageSettings != null) {
                ((IStateManager)imageSettings).TrackViewState();
            }

            base.TrackViewState();
        }
        #endregion
    }
}
