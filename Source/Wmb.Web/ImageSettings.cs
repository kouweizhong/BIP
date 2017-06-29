using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;

using Wmb.Drawing;

namespace Wmb.Web {
    /// <summary>
    /// This class holds all image settings for the IBetterImage controls
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),
    Browsable(true),
    Category("Behavior"),
    RefreshProperties(RefreshProperties.Repaint),
    ToolboxItem(false),
    Description("ImageSettings")]
    public sealed class ImageSettings :Control, IStateManager {
        #region ViewState
        private bool isTrackingViewState;
        private StateBag viewState;

        /// <exclude />
        private new StateBag ViewState {
            get {
                if (viewState == null) {
                    viewState = new StateBag(false);

                    if (isTrackingViewState) {
                        ((IStateManager)viewState).TrackViewState();
                    }
                }

                return viewState;
            }
        }

        bool IStateManager.IsTrackingViewState {
            get {
                return isTrackingViewState;
            }
        }

        void IStateManager.LoadViewState(object savedState) {
            if (savedState != null) {
                ((IStateManager)ViewState).LoadViewState(savedState);
            }
        }

        object IStateManager.SaveViewState() {
            object savedState = null;

            if (viewState != null) {
                savedState =
                   ((IStateManager)viewState).SaveViewState();
            }

            return savedState;
        }

        void IStateManager.TrackViewState() {
            isTrackingViewState = true;

            if (viewState != null) {
                ((IStateManager)viewState).TrackViewState();
            }
        }
        #endregion

        private static readonly BetterImageConfigurationSection config = BetterImageConfigurationSection.Current;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSettings"/> class.
        /// </summary>
        public ImageSettings()
            : base() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSettings"/> class.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        public ImageSettings(NameValueCollection nameValueCollection) {
            this.CustomData = nameValueCollection.Get("cd", config.CustomData);
            this.CustomTransform = nameValueCollection.Get("t", config.CustomTransform);
            this.LeechProtect = nameValueCollection.Get("lp", config.LeechProtect);
            this.EncryptSrc = nameValueCollection.Get("e", config.EncryptSrc);
            this.Copyright = nameValueCollection.Get("ct", config.Copyright);
            this.CopyrightSize = nameValueCollection.Get("cts", config.CopyrightSize);
            this.SaveName = nameValueCollection.Get("sn", config.SaveName);
            this.ImageRetriever = nameValueCollection.Get("r", config.ImageRetriever);
            this.ImageCacheBroker = nameValueCollection.Get("icb", config.ImageCacheBroker);
            this.MaxWidth = nameValueCollection.Get("w", config.MaxWidth);
            this.MaxHeight = nameValueCollection.Get("h", config.MaxHeight);
            this.GraphicsQuality = nameValueCollection.Get("gq", config.GraphicsQuality);
            this.OutputQuality = nameValueCollection.Get("oq", config.OutputQuality);
            this.Grayscale = nameValueCollection.Get("g", config.Grayscale);
            this.Negative = nameValueCollection.Get("n", config.Negative);
            this.Sepia = nameValueCollection.Get("s", config.Sepia);
            this.Clip = nameValueCollection.Get("cl", config.Clip);
            this.Brightness = nameValueCollection.Get("b", config.Brightness);
            this.Contrast = nameValueCollection.Get("c", config.Contrast);
            this.Opacity = nameValueCollection.Get("o", config.Opacity);
            this.Quantize = nameValueCollection.Get("q", config.Quantize);
            this.MaintainPalette = nameValueCollection.Get("mp", config.MaintainPalette);
            this.ServerCacheTimeout = nameValueCollection.Get("sct", config.ServerCacheTimeout);
            this.ClientCacheTimeout = nameValueCollection.Get("cct", config.ClientCacheTimeout);
        }

        /// <summary>
        /// Gets or sets the image retriever.
        /// </summary>
        /// <value>The image retriever.</value>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue("FileSystemImageRetriever"),
        Description("The image retriever."),
        NotifyParentProperty(true)
        ]
        public string ImageRetriever {
            get {
                return ViewState.GetValue("ImageRetriever", config.ImageRetriever);
            }
            set {
                if (ImageRetriever != value) {
                    ViewState.SetValue("ImageRetriever", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the image cache broker.
        /// </summary>
        /// <value>The image cache broker.</value>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue("WebImageCacheBroker"),
        Description("The image cache broker."),
        NotifyParentProperty(true)
        ]
        public string ImageCacheBroker {
            get {
                return ViewState.GetValue("ImageCacheBroker", config.ImageCacheBroker);
            }
            set {
                if (ImageCacheBroker != value) {
                    ViewState.SetValue("ImageCacheBroker", value);
                }
            }
        }

        /// <summary>
        /// <para>The maximum width of the image.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(0),
        Description("The maximum width of the image."),
        NotifyParentProperty(true)
        ]
        public int MaxWidth {
            get {
                return ViewState.GetValue("MaxWidth", config.MaxWidth);
            }
            set {
                if (MaxWidth != value) {
                    ViewState.SetValue("MaxWidth", value);
                }
            }
        }

        /// <summary>
        /// <para>The maximum height of the image.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(0),
        Description("The maximum height of the image."),
        NotifyParentProperty(true)
        ]
        public int MaxHeight {
            get {
                return ViewState.GetValue("MaxHeight", config.MaxHeight);
            }
            set {
                if (MaxHeight != value) {
                    ViewState.SetValue("MaxHeight", value);
                }
            }
        }

        /// <summary>
        /// <para>The resize quality of the image.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(GraphicsQuality.Medium),
        Description("The quality of the image."),
        NotifyParentProperty(true)
        ]
        public GraphicsQuality GraphicsQuality {
            get {
                return ViewState.GetValue("GraphicsQuality", config.GraphicsQuality);
            }
            set {
                if (GraphicsQuality != value) {
                    ViewState.SetValue("GraphicsQuality", value);
                }
            }
        }

        /// <summary>
        /// <para>The output quality of the image.</para>
        /// <remarks>Works on jpeg images only!</remarks>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(75L),
        Description("The output quality of the image. Works on jpeg images only!"),
        NotifyParentProperty(true)
        ]
        public long OutputQuality {
            get {
                return ViewState.GetValue("OutputQuality", config.OutputQuality);
            }
            set {
                if (OutputQuality != value) {
                    if (value < 0 || value > 100) {
                        throw new ArgumentOutOfRangeException("value", "OutputQuality can contain a value between 0 and 100.");
                    }

                    ViewState.SetValue("OutputQuality", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like the image grayscaled.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether or not you would like the image grayscaled."),
        NotifyParentProperty(true)
        ]
        public bool Grayscale {
            get {
                return ViewState.GetValue("Grayscale", config.Grayscale);
            }
            set {
                if (Grayscale != value) {
                    ViewState.SetValue("Grayscale", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like a negative of the image.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether or not you would like a negative of the image."),
        NotifyParentProperty(true)
        ]
        public bool Negative {
            get {
                return ViewState.GetValue("Negative", config.Negative);
            }
            set {
                if (Negative != value) {
                    ViewState.SetValue("Negative", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like to turn your image into sepia.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether or not you would like to turn your image into sepia."),
        NotifyParentProperty(true)
        ]
        public bool Sepia {
            get {
                return ViewState.GetValue("Sepia", config.Sepia);
            }
            set {
                if (Sepia != value) {
                    ViewState.SetValue("Sepia", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like to maintain the original palette.</para>
        /// <remarks>This keeps the original colorpalette which is great for resizing GIF and PNG.</remarks>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether or not you would like to maintain the original palette. This keeps the original colorpalette which is great for resizing GIF and PNG."),
        NotifyParentProperty(true)
        ]
        public bool MaintainPalette {
            get {
                return ViewState.GetValue("MaintainPalette", config.MaintainPalette);
            }
            set {
                if (MaintainPalette != value) {
                    ViewState.SetValue("MaintainPalette", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like the image to be clipped to size.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether or not you would like the image to be clipped to size."),
        NotifyParentProperty(true)
        ]
        public bool Clip {
            get {
                return ViewState.GetValue("Clip", config.Clip);
            }
            set {
                if (Clip != value) {
                    ViewState.SetValue("Clip", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like to quantize your GIF images.</para>
        /// <remarks>It greatly enhances the quality at the cost of processingtime.</remarks>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether or not you would like to quantize your GIF images. It greatly enhances the quality at the cost of processingtime."),
        NotifyParentProperty(true)
        ]
        public bool Quantize {
            get {
                return ViewState.GetValue("Quantize", config.Quantize);
            }
            set {
                if (Quantize != value) {
                    ViewState.SetValue("Quantize", value);
                }
            }
        }

        /// <summary>
        /// <para>Adjust the brightness of your image.</para>
        /// <remarks>Choose a value between -1 and 1 where 0 is default.</remarks>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(0F),
        Description("Adjust the brightness of your image. Choose a value between -1 and 1 where 0 is default."),
        NotifyParentProperty(true)
        ]
        public float Brightness {
            get {
                return ViewState.GetValue("Brightness", config.Brightness);
            }
            set {
                if (Brightness != value) {
                    if (value < -1 || value > 1) {
                        throw new ArgumentOutOfRangeException("value", "Brightness can contain a value between -1 and 1.");
                    }

                    ViewState.SetValue("Brightness", value);
                }
            }
        }

        /// <summary>
        /// <para>Adjust the contrast of your image.</para>
        /// <remarks>Choose a value between 0 and 3 where 1 is default.</remarks>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(1F),
        Description("Adjust the contrast of your image. Choose a value between 0 and 3 where 1 is default."),
        NotifyParentProperty(true)
        ]
        public float Contrast {
            get {
                return ViewState.GetValue("Contrast", config.Contrast);
            }
            set {
                if (Contrast != value) {
                    if (value < 0 || value > 3) {
                        throw new ArgumentOutOfRangeException("value", "Contrast can contain a value between 0 and 3.");
                    }

                    ViewState.SetValue("Contrast", value);
                }
            }
        }

        /// <summary>
        /// <para>Adjust the opacity of your image.</para>
        /// <remarks>Choose a value between 0 and 1 where 0 is default.</remarks>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(0F),
        Description("Adjust the opacity of your image. Choose a value between 0 and 1 where 0 is default."),
        NotifyParentProperty(true)
        ]
        public float Opacity {
            get {
                return ViewState.GetValue("Opacity", config.Opacity);
            }
            set {
                if (Opacity != value) {
                    if (value < 0 || value > 1) {
                        throw new ArgumentOutOfRangeException("value", "Opacity can contain a value between 0 and 1.");
                    }

                    ViewState.SetValue("Opacity", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like the output cached. Can be very powerfull but be shure to know what you're doing. Don't cache 300 large images.</para>
        /// <para>Use the ImageCacheController property to define the cache controller that's being used.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(0),
        Description("Whether or not you would like the output cached. Can be very powerfull but be shure to know what you're doing. Don't cache 300 large images."),
        NotifyParentProperty(true)
        ]
        public int ServerCacheTimeout {
            get {
                return ViewState.GetValue("ServerCacheTimeout", config.ServerCacheTimeout);
            }
            set {
                if (ServerCacheTimeout != value) {
                    ViewState.SetValue("ServerCacheTimeout", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the client cache timeout in minutes.
        /// </summary>
        /// <value>The client cache timeout.</value>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(60),
        Description("The client cache timeout in minutes."),
        NotifyParentProperty(true)
        ]
        public int ClientCacheTimeout {
            get {
                return ViewState.GetValue("ClientCacheTimeout", config.ClientCacheTimeout);
            }
            set {
                if (ClientCacheTimeout != value) {
                    ViewState.SetValue("ClientCacheTimeout", value);
                }
            }
        }

        /// <summary>
        /// <para>Gets or sets the preset name in the Save dialog on 'Save image as'.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(""),
        Description("Gets or sets the preset name in the Save dialog on 'Save image as'."),
        NotifyParentProperty(true)
        ]
        public string SaveName {
            get {
                return ViewState.GetValue("SaveName", config.SaveName);
            }
            set {
                if (SaveName != value) {
                    ViewState.SetValue("SaveName", value);
                }
            }
        }

        /// <summary>
        /// <para>Gets or sets the copyright text.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(""),
        Description("Gets or sets the copyright text."),
        NotifyParentProperty(true)
        ]
        public string Copyright {
            get {
                return ViewState.GetValue("Copyright", config.Copyright);
            }
            set {
                if (Copyright != value) {
                    ViewState.SetValue("Copyright", value);
                }
            }
        }

        /// <summary>
        /// <para>The fontsize for the copyright text.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(0),
        Description("The fontsize for the copyright text."),
        NotifyParentProperty(true)
        ]
        public int CopyrightSize {
            get {
                return ViewState.GetValue("CopyrightSize", config.CopyrightSize);
            }
            set {
                if (CopyrightSize != value) {
                    if (value < 0) {
                        throw new ArgumentOutOfRangeException("value");
                    }

                    ViewState.SetValue("CopyrightSize", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like the src of the image to be encrypted. It helps in preventing download of the original image.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether or not you would like the src of the image to be encrypted. It helps in preventing download of the original image."),
        NotifyParentProperty(true)
        ]
        public bool EncryptSrc {
            get {
                return ViewState.GetValue("EncryptSrc", config.EncryptSrc);
            }
            set {
                if (EncryptSrc != value) {
                    ViewState.SetValue("EncryptSrc", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like to disable right mouse button. Works only if JavaScript is enabled.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether or not you would like to disable right mouse button. Works only if JavaScript is enabled."),
        NotifyParentProperty(true)
        ]
        public bool DisableRightClick {
            get {
                return ViewState.GetValue("DisableRightClick", config.DisableRightClick);
            }
            set {
                if (DisableRightClick != value) {
                    ViewState.SetValue("DisableRightClick", value);
                }
            }
        }

        /// <summary>
        /// <para>Whether or not you would like to protect your images from leechers / deeplinkers.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether or not you would like to protect your images from leechers / deeplinkers."),
        NotifyParentProperty(true)
        ]
        public bool LeechProtect {
            get {
                return ViewState.GetValue("LeechProtect", config.LeechProtect);
            }
            set {
                if (LeechProtect != value) {
                    ViewState.SetValue("LeechProtect", value);
                }
            }
        }

        /// <summary>
        /// <para>Gets or sets the custom image transform to apply.</para>
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(""),
        Description("Gets or sets the custom image transform to apply."),
        NotifyParentProperty(true)
        ]
        public string CustomTransform {
            get {
                return ViewState.GetValue("CustomTransform", config.CustomTransform);
            }
            set {
                if (CustomTransform != value) {
                    ViewState.SetValue("CustomTransform", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom data to pass to your the participating ICustomDataConsumers.
        /// </summary>
        /// <value>The custom data.</value>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(""),
        Description("Gets or sets the custom data to pass to all the participating ICustomDataConsumers."),
        NotifyParentProperty(true)
        ]
        public string CustomData {
            get {
                return ViewState.GetValue("CustomData", config.CustomData);
            }
            set {
                if (CustomData != value) {
                    ViewState.SetValue("CustomData", value);
                }
            }
        }

        /// <summary>
        /// Convert these settings to a NameValueCollection.
        /// </summary>
        /// <returns>NameValueCollection</returns>
        public NameValueCollection ToNameValueCollection() {
            NameValueCollection nameValueCollection = new NameValueCollection();

            if (this.CustomData != config.CustomData) {
                nameValueCollection.Set("cd", this.CustomData);
            }

            if (this.CustomTransform != config.CustomTransform) {
                nameValueCollection.Set("t", this.CustomTransform);
            }

            if (this.LeechProtect != config.LeechProtect) {
                nameValueCollection.Set("lp", this.LeechProtect);
            }

            if (this.EncryptSrc != config.EncryptSrc) {
                nameValueCollection.Set("e", this.EncryptSrc);
            }

            if (this.Copyright != config.Copyright) {
                nameValueCollection.Set("ct", this.Copyright);
            }

            if (this.CopyrightSize != config.CopyrightSize) {
                nameValueCollection.Set("cts", this.CopyrightSize);
            }

            if (this.SaveName != config.SaveName) {
                nameValueCollection.Set("sn", this.SaveName);
            }

            if (this.ImageRetriever != config.ImageRetriever) {
                nameValueCollection.Set("r", this.ImageRetriever);
            }

            if (this.ImageCacheBroker != config.ImageCacheBroker) {
                nameValueCollection.Set("icb", this.ImageCacheBroker);
            }

            if (this.MaxWidth != config.MaxWidth) {
                nameValueCollection.Set("w", this.MaxWidth);
            }

            if (this.MaxHeight != config.MaxHeight) {
                nameValueCollection.Set("h", this.MaxHeight);
            }

            if (this.GraphicsQuality != config.GraphicsQuality) {
                nameValueCollection.Set("gq", this.GraphicsQuality);
            }

            if (this.OutputQuality != config.OutputQuality) {
                nameValueCollection.Set("oq", this.OutputQuality);
            }

            if (this.Grayscale != config.Grayscale) {
                nameValueCollection.Set("g", this.Grayscale);
            }

            if (this.Negative != config.Negative) {
                nameValueCollection.Set("n", this.Negative);
            }

            if (this.Sepia != config.Sepia) {
                nameValueCollection.Set("s", this.Sepia);
            }

            if (this.Clip != config.Clip) {
                nameValueCollection.Set("cl", this.Clip);
            }

            if (this.Brightness != config.Brightness) {
                nameValueCollection.Set("b", this.Brightness);
            }

            if (this.Contrast != config.Contrast) {
                nameValueCollection.Set("c", this.Contrast);
            }

            if (this.Opacity != config.Opacity) {
                nameValueCollection.Set("o", this.Opacity);
            }

            if (this.Quantize != config.Quantize) {
                nameValueCollection.Set("q", this.Quantize);
            }

            if (this.MaintainPalette != config.MaintainPalette) {
                nameValueCollection.Set("mp", this.MaintainPalette);
            }

            if (this.ServerCacheTimeout != config.ServerCacheTimeout) {
                nameValueCollection.Set("sct", this.ServerCacheTimeout);
            }

            if (this.ClientCacheTimeout != config.ClientCacheTimeout) {
                nameValueCollection.Set("cct", this.ClientCacheTimeout);
            }

            return nameValueCollection;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() {
            string retVal = this.ToNameValueCollection().ToQueryString();

            if (string.IsNullOrEmpty(retVal)) {
                retVal = "All settings are set to their defaults";
            }

            return retVal;
        }

        /// <summary>
        /// Convert the image URL to one that redirects to the ImageHandler.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns>An URL that points to the image handler and has all settings</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification="No control can retrieve this value as a URI. Implementing it as URI would only cause senseless back and fort convertions.")]
        public string ToImgUrl(string src) {
            NameValueCollection qry = this.ToNameValueCollection();
            qry.Set("src", src, this.EncryptSrc, config.Password, config.Salt);

            return string.Concat(config.ImageHandler,
                                 "?",
                                 qry.ToQueryString(true, config.Salt));
        }
    }
}