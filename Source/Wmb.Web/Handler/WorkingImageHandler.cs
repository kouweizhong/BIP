using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using Wmb.Drawing;
using System.Net;
using System.Drawing.Imaging;

namespace Wmb.Web {
    /// <summary>
    /// The ImageHandler handles all requests for resized Images.
    /// </summary>
    public sealed class ImageHandler : IHttpHandler {
        private static readonly int browserCacheExpirationMinutes = 60;
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable {
            get { return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context) {
            context.Response.Clear();
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetAllowResponseInBrowserHistory(true);
            context.Response.Cache.SetValidUntilExpires(true);
            context.Response.BufferOutput = false;

            try {
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;

                bool leechProtect = request.GetQueryStringValue<bool>("lp", false);

                //validate the request
                if (!request.IsValid(true, leechProtect)) {
                    throw new HttpException((int)HttpStatusCode.BadRequest, "The request did not pass validation.");
                }


                //retrieve the filepath of the image we would like to retrieve
                string imageSrc = GetImageSourceQueryStringValue(context);

                //create an image retriever and get the file extension
                ImageRetriever imageRetriever = ImageRetrieverFactory.CreateImageRetriever(imageSrc);
                string filename = imageRetriever.GetFileNameWithoutExtension();
                string extension = imageRetriever.GetExtension().ToUpperInvariant();

                //set the content type of the response
                response.ContentType = GetContentType(extension);

                //set the content-disposition header to the given SaveName or the filename
                string saveName = request.GetQueryStringValue<string>("sn", filename);
                response.AppendHeader("Content-Disposition", string.Concat("inline; filename=\"", saveName, "\""));

                using (MemoryStream memoryStream = new MemoryStream()) {
                    string hash = request.GetQueryStringValue<string>("ha", null);
                    bool cacheOutput = request.GetQueryStringValue<bool>("co", false);

                    bool fromCache = false;
                    if (cacheOutput) {
                        object cache = context.Cache[hash];
                        if (cache != null) {
                            fromCache = true;
                            byte[] buffer = (byte[])cache;
                            memoryStream.Write(buffer, 0, buffer.Length);
                        }
                    }

                    if (!fromCache) {
                        int width = request.GetQueryStringValue<int>("w", 0);
                        int height = request.GetQueryStringValue<int>("h", 0);
                        GraphicsQuality graphicsQuality = (GraphicsQuality)request.GetQueryStringValue<int>("rq", 2);
                        bool maintainPalette = request.GetQueryStringValue<bool>("mp", false);
                        bool clip = request.GetQueryStringValue<bool>("cl", false);

                        using (Image originalImage = imageRetriever.GetImage())
                        using (Image resizedBitmap = (clip) ? originalImage.Clip(width, height, graphicsQuality, maintainPalette)
                                                            : originalImage.Resize(width, height, graphicsQuality, maintainPalette)) {
                            if (!resizedBitmap.HasIndexedPixelFormat()) {
                                string copyrightText = request.GetQueryStringValue<string>("ct", null);
                                if (!string.IsNullOrEmpty(copyrightText)) {
                                    int copyrightFontSize = request.GetQueryStringValue<int>("cts", 0);
                                    resizedBitmap.AddCopyrightText(copyrightText, copyrightFontSize);
                                }

                                if (request.GetQueryStringValue("g", false)) {
                                    resizedBitmap.ApplyGrayscaleColorMatrix();
                                }

                                if (request.GetQueryStringValue("n", false)) {
                                    resizedBitmap.ApplyNegativeColorMatrix();
                                }

                                if (request.GetQueryStringValue("s", false)) {
                                    resizedBitmap.ApplySepiaColorMatrix();
                                }

                                float brightness = request.GetQueryStringValue<float>("b", 0F);
                                if (brightness != 0) {
                                    resizedBitmap.SetBrightness(brightness);
                                }

                                float contrast = request.GetQueryStringValue<float>("c", 0F);
                                if (contrast != 0) {
                                    resizedBitmap.SetContrast(contrast);
                                }
                            }

                            ImageFormat imageFormat = GetImageFormatByExtension(extension);
                            long outputQuality = request.GetQueryStringValue<long>("oq", 75L);

                            if (QuantizeableExtension(extension) && request.GetQueryStringValue<bool>("q", false)) {
                                using (Image quantizedBitmap = resizedBitmap.OctreeQuantize()) {
                                    quantizedBitmap.SaveToMemoryStream(memoryStream, imageFormat, outputQuality);
                                }
                            }
                            else {
                                resizedBitmap.SaveToMemoryStream(memoryStream, imageFormat, outputQuality);
                            }
                        }

                    }

                    memoryStream.Capacity = (int)memoryStream.Position;
                    if (memoryStream.Capacity > 0) {
                        response.Cache.SetExpires(DateTime.Now.AddMinutes(browserCacheExpirationMinutes));
                        response.Cache.SetLastModified(imageRetriever.GetLastModifiedDate());
                        
                        memoryStream.WriteTo(context.Response.OutputStream);

                        if (cacheOutput && !fromCache) {
                            context.Cache[hash] = memoryStream.ToArray();
                        }
                    }
                }
            }
            catch (FileNotFoundException) {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            catch (HttpException httpException) {
                context.Response.StatusCode = httpException.GetHttpCode();
            }
            finally {
                context.Response.Flush();
                context.Response.End();
            }
        }

        private static string GetImageSourceQueryStringValue(HttpContext context) {
            HttpRequest request = context.Request;

            bool isImageSrcEncrypted = request.GetQueryStringValue<bool>("e", false);
            string retVal = request.GetQueryStringValue<string>("src", null, isImageSrcEncrypted);

            if (string.IsNullOrEmpty(retVal)) {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Can't serve an image without a source.");
            }

            return retVal;
        }

        private static string GetContentType(string extension) {
            if (string.IsNullOrEmpty(extension)) {
                throw new ArgumentNullException("extension");
            }

            string retVal;

            switch (extension) {
                case ".PNG":
                    retVal = "image/png";
                    break;

                case ".GIF":
                    retVal = "image/gif";
                    break;

                default:
                    retVal = "image/jpeg";
                    break;
            }

            return retVal;
        }

        private static ImageFormat GetImageFormatByExtension(string extension) {
            if (string.IsNullOrEmpty(extension)) {
                throw new ArgumentNullException("extension");
            }

            ImageFormat retVal;

            switch (extension.ToUpperInvariant()) {
                case ".PNG":
                    retVal = ImageFormat.Png;
                    break;

                case ".GIF":
                    retVal = ImageFormat.Gif;
                    break;

                default:
                    retVal = ImageFormat.Jpeg;
                    break;
            }

            return retVal;
        }

        private static bool QuantizeableExtension(string extension) {
            if (string.IsNullOrEmpty(extension)) {
                throw new ArgumentNullException("extension");
            }

            bool retVal = false;

            switch (extension.ToUpperInvariant()) {
                case ".PNG":
                case ".GIF":
                    retVal = true;
                    break;

                default:
                    retVal = false;
                    break;
            }

            return retVal;
        }
    }
}
