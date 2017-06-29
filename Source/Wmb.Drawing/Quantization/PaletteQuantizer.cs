/* 
  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF 
  ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
  THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A 
  PARTICULAR PURPOSE. 
  
    This is sample code and is freely distributable. 
*/

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wmb.Drawing {
    /// <summary>
    /// Quantizes by using a palette.
    /// </summary>
    public class PaletteQuantizer : Quantizer {
        /// <summary>
        /// Construct the palette quantizer
        /// </summary>
        /// <param name="palette">The color palette to quantize to</param>
        /// <remarks>
        /// Palette quantization only requires a single quantization step
        /// </remarks>
        public PaletteQuantizer(ICollection palette)
            : base(true) {
            if (palette == null || palette.Count == 0) {
                throw new ArgumentNullException("palette");
            }

            this.colorMap = new Hashtable();

            this.colors = new Color[palette.Count];
            palette.CopyTo(this.colors, 0);
        }

        /// <summary>
        /// Override this to process the pixel in the second pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <returns>The quantized value</returns>
        protected override byte QuantizePixel(Color32 pixel) {
            byte colorIndex = 0;
            int colorHash = pixel.ARGB;

            // Check if the color is in the lookup table
            if (this.colorMap.ContainsKey(colorHash)) {
                colorIndex = (byte)this.colorMap[colorHash];
            }
            else {
                // Not found - loop through the palette and find the nearest match.
                // Firstly check the alpha value - if 0, lookup the transparent color
                if (0 == pixel.Alpha) {
                    // Transparent. Lookup the first color with an alpha value of 0
                    for (int index = 0; index < this.colors.Length; index++) {
                        if (0 == this.colors[index].A) {
                            colorIndex = (byte)index;
                            break;
                        }
                    }
                }
                else {
                    // Not transparent...
                    int leastDistance = int.MaxValue;
                    int red = pixel.Red;
                    int green = pixel.Green;
                    int blue = pixel.Blue;

                    // Loop through the entire palette, looking for the closest color match
                    for (int index = 0; index < this.colors.Length; index++) {
                        Color paletteColor = this.colors[index];

                        int redDistance = paletteColor.R - red;
                        int greenDistance = paletteColor.G - green;
                        int blueDistance = paletteColor.B - blue;

                        int distance = (redDistance * redDistance) +
                                           (greenDistance * greenDistance) +
                                           (blueDistance * blueDistance);

                        if (distance < leastDistance) {
                            colorIndex = (byte)index;
                            leastDistance = distance;

                            // And if it's an exact match, exit the loop
                            if (0 == distance) {
                                break;
                            }
                        }
                    }
                }

                // Now I have the color, pop it into the hashtable for next time
                this.colorMap.Add(colorHash, colorIndex);
            }

            return colorIndex;
        }

        /// <summary>
        /// Retrieve the palette for the quantized image
        /// </summary>
        /// <param name="original">Any old palette, this is overrwritten</param>
        /// <returns>The new color palette</returns>
        protected override ColorPalette GetPalette(ColorPalette original) {
            if (original == null) {
                throw new ArgumentNullException("original");
            }

            for (int index = 0; index < this.colors.Length; index++) {
                original.Entries[index] = this.colors[index];
            }

            return original;
        }

        /// <summary>
        /// Lookup table for colors
        /// </summary>
        private Hashtable colorMap;

        /// <summary>
        /// List of all colors in the palette
        /// </summary>
        private Color[] colors;
    }
}
