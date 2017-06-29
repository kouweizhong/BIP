using System.ComponentModel;
using System.Drawing;
using System.Web.UI;

namespace Wmb.Web.WebControls {
    /// <summary>
    /// <para>BetterImage control inherits from System.Web.UI.WebControls.Image</para>
    /// </summary>
    [
    Designer(typeof(IBetterImageDesigner)),
    DefaultProperty("ImageUrl"),
    ToolboxData("<{0}:BetterImage runat=\"server\"></{0}:BetterImage>"),
    ToolboxBitmap(typeof(System.Web.UI.WebControls.Image)),
    ParseChildren(ChildrenAsProperties = true)
    ]
    public class BetterImage : System.Web.UI.WebControls.Image, IBetterImage {
        /// <summary>
        /// Initializes a new instance of the <see cref="BetterImage"/> class.
        /// </summary>
        public BetterImage()
            : base() {
        }

        private ImageSettings imageSettings;
        /// <summary>
        /// <para>The ImageSettings for the ImageHandler.</para>
        /// </summary>
        [
        Category("ImageSettings"),
        Description("The ImageSettings for the ImageHandler."),
        PersistenceMode(PersistenceMode.InnerProperty)
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

        /// <exclude />
        protected override void AddAttributesToRender(HtmlTextWriter writer) {
            if (base.ImageUrl.Length > 0 && !DesignMode) {
                string resolvedUrl = ResolveUrl(base.ImageUrl);
                base.ImageUrl = ImageSettings.ToImgUrl(resolvedUrl);
            }

            if (ImageSettings.DisableRightClick) {
                writer.AddAttribute("oncontextmenu", "return false;");
            }

            base.AddAttributesToRender(writer);
        }

        /// <summary>
        /// Binds a data source to the server control's child controls.
        /// </summary>
        protected override void DataBindChildren() {
            base.DataBindChildren();
            ImageSettings.DataBind();
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
