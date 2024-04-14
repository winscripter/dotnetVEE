using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;

namespace dotnetVEE.Computation.ImageUtils
{
    /// <summary>
    /// Allows zooming in and out of an image.
    /// </summary>
    public class ImageScalingHelper
    {
        /// <summary>
        /// Background color when image resizing results
        /// in the image out of bounds.
        /// </summary>
        public readonly Color PadColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageScalingHelper" /> class.
        /// </summary>
        /// <param name="padColor">The pad color - that is, background color when image resizing results in the image out of bounds.</param>
        public ImageScalingHelper(Color? padColor = default)
        {
            PadColor = padColor ?? Color.Black;
        }

        #region Image method overloads
        private void ZoomOut(ref Image image, int width, int height)
        {
            int oldWidth = image.Width;
            int oldHeight = image.Height;

            width = image.Width + width;
            height = image.Height + height;

            image.Mutate(x => x.Pad(width, height, PadColor));
            image.Mutate(x => x.Resize(oldWidth, oldHeight));
        }

        private void ZoomIn(ref Image image, int cropWidth, int cropHeight)
        {
            int oldWidth = image.Width;
            int oldHeight = image.Height;

            int centerX = image.Width / 2;
            int centerY = image.Height / 2;

            cropWidth = image.Width - cropWidth;
            cropHeight = image.Height - cropHeight;

            Rectangle cropRect = new(
                centerX - cropWidth / 2,
                centerY - cropHeight / 2,
                cropWidth, cropHeight
            );

            image.Mutate(x => x.Crop(cropRect));
            image.Mutate(x => x.Resize(oldWidth, oldHeight));
        }

        /// <summary>
        /// Zooms in or out of an image by X.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="newX">Zoom X</param>
        public void ZoomX(ref Image image, int newX)
        {
            if (newX < 0)
            {
                ZoomOut(ref image, Math.Abs(newX), 0);
            }
            else
            {
                ZoomIn(ref image, newX, 0);
            }
        }

        /// <summary>
        /// Zooms in or out of an image by Y.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="newY">Zoom Y</param>
        public void ZoomY(ref Image image, int newY)
        {
            if (newY < 0)
            {
                ZoomOut(ref image, 0, Math.Abs(newY));
            }
            else
            {
                ZoomIn(ref image, 0, newY);
            }
        }

        /// <summary>
        /// Zooms in our out of an image by X and Y.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="x">Zoom X.</param>
        /// <param name="y">Zoom Y.</param>
        public void ZoomXY(ref Image image, int x, int y)
        {
            ZoomX(ref image, x);
            ZoomY(ref image, y);
        }

        /// <summary>
        /// Zooms in our out of an image by X and Y.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="x">Zoom X.</param>
        /// <param name="y">Zoom Y.</param>
        public void ZoomImage(ref Image image, int x, int y)
        {
            ZoomXY(ref image, x, y);
        }
        #endregion

        #region Image<Rgba32> method overloads
        private void ZoomOut(ref Image<Rgba32> image, int width, int height)
        {
            int oldWidth = image.Width;
            int oldHeight = image.Height;

            width = image.Width + width;
            height = image.Height + height;

            image.Mutate(x => x.Pad(width, height, PadColor));
            image.Mutate(x => x.Resize(oldWidth, oldHeight));
        }

        private void ZoomIn(ref Image<Rgba32> image, int cropWidth, int cropHeight)
        {
            int oldWidth = image.Width;
            int oldHeight = image.Height;

            int centerX = image.Width / 2;
            int centerY = image.Height / 2;

            cropWidth = image.Width - cropWidth;
            cropHeight = image.Height - cropHeight;

            Rectangle cropRect = new(
                centerX - cropWidth / 2,
                centerY - cropHeight / 2,
                cropWidth, cropHeight
            );

            image.Mutate(x => x.Crop(cropRect));
            image.Mutate(x => x.Resize(oldWidth, oldHeight));
        }

        /// <summary>
        /// Zooms in or out of an image by X.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="newX">Zoom X</param>
        public void ZoomX(ref Image<Rgba32> image, int newX)
        {
            if (newX < 0)
            {
                ZoomOut(ref image, Math.Abs(newX), 0);
            }
            else
            {
                ZoomIn(ref image, newX, 0);
            }
        }

        /// <summary>
        /// Zooms in or out of an image by Y.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="newY">Zoom Y</param>
        public void ZoomY(ref Image<Rgba32> image, int newY)
        {
            if (newY < 0)
            {
                ZoomOut(ref image, 0, Math.Abs(newY));
            }
            else
            {
                ZoomIn(ref image, 0, newY);
            }
        }

        /// <summary>
        /// Zooms in our out of an image by X and Y.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="x">Zoom X.</param>
        /// <param name="y">Zoom Y.</param>
        public void ZoomXY(ref Image<Rgba32> image, int x, int y)
        {
            ZoomX(ref image, x);
            ZoomY(ref image, y);
        }

        /// <summary>
        /// Zooms in our out of an image by X and Y.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="x">Zoom X.</param>
        /// <param name="y">Zoom Y.</param>
        public void ZoomImage(ref Image<Rgba32> image, int x, int y)
        {
            ZoomXY(ref image, x, y);
        }
        #endregion
    }
}
