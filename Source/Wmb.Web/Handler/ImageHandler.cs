using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;

using Wmb.Drawing;
using Wmb.Web.Caching;
using System.Diagnostics;

namespace Wmb.Web {
    /// <summary>
    /// The ImageHandler handles all requests for resized Images.
    /// </summary>
    public sealed class ImageHandler : IHttpHandler, IDisposable {
        private static BetterImageConfigurationSection config = BetterImageConfigurationSection.Current;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageHandler"/> class.
        /// </summary>
        public ImageHandler() {
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable {
            get {
                return false;
            }
        }

        private ImageSettings _imageSettings;
        private ImageSettings ImageSettings {
            get {
                if (_imageSettings == null) {
                    _imageSettings = new ImageSettings(HttpContext.Current.Request.QueryString);
                }

                return _imageSettings;
            }
        }

        private ImageRetriever _imageRetriever;
        private ImageRetriever ImageRetriever {
            get {
                if (_imageRetriever == null) {
                    string imageRetrieverClassName = config.GetImageRetrieverClassName(ImageSettings.ImageRetriever);

                    string imageSrc = HttpContext.Current.Request.GetQueryStringValue("src", string.Empty, ImageSettings.EncryptSrc, config.Password, config.Salt);
                    if (string.IsNullOrEmpty(imageSrc)) {
                        throw new HttpException((int)HttpStatusCode.BadRequest, "Can't serve an image without a source.");
                    }

                    _imageRetriever = ImageRetrieverFactory.Create(imageSrc, imageRetrieverClassName);
                    if (_imageRetriever is ICustomDataConsumer) {
                        ((ICustomDataConsumer)_imageRetriever).SetCustomData(ImageSettings.CustomData);
                    }
                }

                return _imageRetriever;
            }
        }

        private ImageCacheBroker _imageCacheBroker;
        private ImageCacheBroker ImageCacheBroker {
            get {
                if (_imageCacheBroker == null) {
                    string hash = HttpContext.Current.Request.GetQueryStringValue<string>("ha", null);
                    if (string.IsNullOrEmpty(hash)) {
                        throw new HttpException((int)HttpStatusCode.BadRequest, "Can't serve an image without a hash key.");
                    }

                    string imageCacheBrokerName = ImageSettings.ImageCacheBroker;
                    string imageCacheBrokerClassName = config.GetImageCacheBrokerClassName(imageCacheBrokerName);

                    DateTime utcExpiry = DateTime.Now.AddMinutes(ImageSettings.ServerCacheTimeout);

                    _imageCacheBroker = ImageCacheBrokerFactory.Create(imageCacheBrokerClassName,
                                                                       hash, utcExpiry);

                    if (_imageCacheBroker is ICustomDataConsumer) {
                        ((ICustomDataConsumer)_imageCacheBroker).SetCustomData(ImageSettings.CustomData);
                    }
                }

                return _imageCacheBroker;
            }
        }

        private ImageMetadata _imageMetaData;
        private ImageMetadata ImageMetadata {
            get {
                if (_imageMetaData == null) {
                    _imageMetaData = ImageCacheBroker.GetImageMetadata();

                    if(_imageMetaData == null){
                        _imageMetaData = ImageMetadata.Create(this.ImageRetriever, ImageSettings.SaveName);
                        
                        if (ImageSettings.ServerCacheTimeout > 0) {
                            ImageCacheBroker.AddImageMetadata(_imageMetaData);
                        }
                    }
                }
                return _imageMetaData;
            }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context) {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            Uri requestUri = request.Url;

            Trace.TraceInformation("ImageHandler: Started processing the request for: '{0}'.", requestUri);

            response.Clear();
            response.BufferOutput = false;

            try {
                //validate the request
                if (!request.IsValid(config.Salt, ImageSettings.LeechProtect)) {
                    throw new HttpException((int)HttpStatusCode.BadRequest, "The request did not pass validation.");
                }

                //Check if we can return a simple not modified status...
                if (TryProcessRequestBySendingNotModifiedResponse(request, response)) {
                    Trace.TraceInformation("ImageHandler: Request for: '{0}'\nwas served by sending a not modified response.", requestUri);
                    return;
                }

                //Check if we can return the image from cache...
                if (TryProcessRequestFromCache(response)) {
                    Trace.TraceInformation("ImageHandler: Request for: '{0}'\nwas served from cache.", requestUri);
                    return;
                }

                //If we cannot return a simple not modified status or from cache we have to process the full request 
                ProcessFullRequest(response);
                Trace.TraceInformation("ImageHandler: Request for: '{0}'\nwas served by fully generating the image.", requestUri);
            }
            catch (FileNotFoundException fileNotFoundException) {
                Trace.TraceError("ImageHandler: Request for: '{0}'\nthrew a FileNotFoundException: '{1}'", requestUri, fileNotFoundException.ToString());
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            catch (HttpException httpException) {
                Trace.TraceError("ImageHandler: Request for: '{0}'\nthrew a HttpException: '{1}'", requestUri, httpException.ToString());
                response.StatusCode = httpException.GetHttpCode();
            }
            catch (Exception ex) {
                Trace.TraceError("ImageHandler: Request for: '{0}'\nthrew an Exception: '{1}'", requestUri, ex.ToString());
                throw;
            }
            finally {
                response.Flush();
                Trace.TraceInformation("ImageHandler: Finished processing the request for: '{0}'.", requestUri);
            }
        }

        private void ProcessFullRequest(HttpResponse response) {
            using (MemoryStream memoryStream = new MemoryStream()) {
                ImageTransformer imageTransformer = new ImageTransformer();
                imageTransformer.GraphicsQuality = ImageSettings.GraphicsQuality;
                imageTransformer.Copyright = ImageSettings.Copyright;
                imageTransformer.CopyrightSize = ImageSettings.CopyrightSize;
                imageTransformer.MaxWidth = ImageSettings.MaxWidth;
                imageTransformer.MaxHeight = ImageSettings.MaxHeight;
                imageTransformer.Grayscale = ImageSettings.Grayscale;
                imageTransformer.Negative = ImageSettings.Negative;
                imageTransformer.Sepia = ImageSettings.Sepia;
                imageTransformer.Clip = ImageSettings.Clip;
                imageTransformer.Quantize = ImageSettings.Quantize;
                imageTransformer.MaintainPalette = ImageSettings.MaintainPalette;
                imageTransformer.Brightness = ImageSettings.Brightness;
                imageTransformer.Contrast = ImageSettings.Contrast;
                imageTransformer.Opacity = ImageSettings.Opacity;

                string customTransform = ImageSettings.CustomTransform;
                if (!string.IsNullOrEmpty(customTransform)) {
                    string customTransformClassName = config.GetImageTransformClassName(customTransform);
                    ImageTransform customTransformClass = ImageTransformFactory.Create(customTransformClassName);

                    ICustomDataConsumer customTransFormClassAsCustomDataConsumer = customTransformClass as ICustomDataConsumer;
                    if (customTransFormClassAsCustomDataConsumer != null) {
                        customTransFormClassAsCustomDataConsumer.SetCustomData(ImageSettings.CustomData);
                    }

                    imageTransformer.CustomTransforms.Add(customTransformClass);
                }
                
                ImageRetriever.EnsureImage();
                using (Image originalImage = ImageRetriever.GetImage())
                using (Image resizedBitmap = imageTransformer.Transform(originalImage)) {
                    long outputQuality = ImageSettings.OutputQuality;
                    ImageFormat imageFormat = ImageMetadata.ImageFormat;

                    resizedBitmap.SaveToMemoryStream(memoryStream, imageFormat, outputQuality);
                }

                memoryStream.Capacity = (int)memoryStream.Position;

                

                if (memoryStream.Capacity > 0) {
                    if (ImageSettings.ServerCacheTimeout > 0) {
                        ImageCacheBroker.AddImageBytes(memoryStream.ToArray());
                    }

                    response.ContentType = ImageMetadata.ContentType;
                    response.AppendHeader("Content-Disposition", string.Concat("inline; filename=\"", ImageMetadata.SaveName, "\""));
                    response.AppendHeader("Content-Length", memoryStream.Capacity.ToString(CultureInfo.InvariantCulture));
                    response.Cache.SetCacheability(HttpCacheability.Public);
                    response.Cache.SetAllowResponseInBrowserHistory(true);
                    response.Cache.SetLastModified(ImageMetadata.LastModifiedDate);
                    response.Cache.SetValidUntilExpires(true);
                    response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(ImageSettings.ClientCacheTimeout));

                    memoryStream.WriteTo(response.OutputStream);
                }
            }
        }

        private bool TryProcessRequestFromCache(HttpResponse response) {
            bool retVal = false;
            
            byte[] imageBytes = ImageCacheBroker.GetImageBytes();
            if (imageBytes != null && imageBytes.Length > 0) {
                response.ContentType = ImageMetadata.ContentType;
                response.AppendHeader("Content-Disposition", string.Concat("inline; filename=\"", ImageMetadata.SaveName, "\""));
                response.AppendHeader("Content-Length", imageBytes.Length.ToString(CultureInfo.InvariantCulture));
                response.Cache.SetCacheability(HttpCacheability.Public);
                response.Cache.SetAllowResponseInBrowserHistory(true);
                response.Cache.SetLastModified(ImageMetadata.LastModifiedDate);
                response.Cache.SetValidUntilExpires(true);
                response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(ImageSettings.ClientCacheTimeout));

                response.OutputStream.Write(imageBytes, 0, imageBytes.Length);
                retVal = true;
            }

            return retVal;
        }

        private bool TryProcessRequestBySendingNotModifiedResponse(HttpRequest request, HttpResponse response) {
            bool retVal = false;
            string modifiedSinceRequestHeader = request.Headers["If-Modified-Since"];
            if (!string.IsNullOrEmpty(modifiedSinceRequestHeader)) {
                DateTime lastModifiedDate = ImageMetadata.LastModifiedDate;
                DateTime lastModifiedRequest = DateTime.ParseExact(modifiedSinceRequestHeader, "r", CultureInfo.InvariantCulture);

                if (lastModifiedRequest == lastModifiedDate) {
                    response.StatusCode = (int)HttpStatusCode.NotModified;
                    response.StatusDescription = "Not Modified";
                    response.ContentType = ImageMetadata.ContentType;
                    retVal = true;
                }
            }
            return retVal;
        }

        private bool disposed;
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            if (!disposed) {
                if (_imageSettings != null) {
                    _imageSettings.Dispose();
                }
                
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}