using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wmb.Drawing {
    /// <summary>
    /// GraphicsQuality enumerator is used for determining the quality of the graphics object.
    /// </summary>
    public enum GraphicsQuality {
        /// <summary>
        /// <para>PixelOffsetMode.Default; SmoothingMode.None; InterpolationMode.Bilinear;</para>
        /// </summary>
        Default = 0,

        /// <summary>
        /// <para>PixelOffsetMode.HighSpeed; SmoothingMode.HighSpeed; InterpolationMode.Low;</para>
        /// </summary>
        Low = 1,

        /// <summary>
        /// <para>PixelOffsetMode.Half; SmoothingMode.HighQuality; InterpolationMode.High;</para>
        /// </summary>
        Medium = 2,

        /// <summary>
        /// <para>PixelOffsetMode.HighQuality; SmoothingMode.AntiAlias; InterpolationMode.HighQualityBicubic;</para>
        /// </summary>
        High = 3
    }
}
