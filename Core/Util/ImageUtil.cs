using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Core.Util
{
    public class ImageUtil
    {
        public static readonly Dictionary<string, ImageFormat> ImageExtensions =
            new Dictionary<string, ImageFormat> {
                { ".jpg", ImageFormat.Jpeg }
                , { ".bmp", ImageFormat.Bmp }
                , { ".gif", ImageFormat.Gif }
                , { ".png", ImageFormat.Png }
            };

        public static byte[] GetImageThumbnail(byte[] data, string extension, ThumbnailDimensions dimensions)
        {
            if (dimensions == null)
                return data;
            Image thumbnail = GenerateThumbnail(data, dimensions);
            using MemoryStream ms = new MemoryStream();
            thumbnail.Save(ms, ImageExtensions[extension]);
            return ms.ToArray();
        }

        private static bool ThumbnailCallback() => false;

        private static Image GenerateThumbnail(byte[] data, ThumbnailDimensions thumbnailDimensions)
        {
            Image.GetThumbnailImageAbort thumbnailCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            using MemoryStream ms = new MemoryStream(data);
            using Bitmap bmp = new Bitmap(ms);
            return bmp.GetThumbnailImage(thumbnailDimensions.Width, thumbnailDimensions.Height, thumbnailCallback, IntPtr.Zero);
        }
    }

    /// <summary>
    /// This class defines dimensions used for converting image to a thumbnail.
    /// </summary>
    public class ThumbnailDimensions
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
