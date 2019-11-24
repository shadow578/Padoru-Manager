using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace PadoruManager.Util
{
    public static class Utils
    {
        /// <summary>
        /// check if b is equals a, ignoring case
        /// </summary>
        /// <param name="a">the string to check in</param>
        /// <param name="b">the string to check for</param>
        /// <returns>is b equals a</returns>
        public static bool EqualsIgnoreCase(this string a, string b)
        {
            return a.ToUpper().Equals(b.ToUpper());
        }

        /// <summary>
        /// check if b is contained in a, ignoring case
        /// </summary>
        /// <param name="a">the string to check in</param>
        /// <param name="b">the string to check for</param>
        /// <returns>is b contained in a</returns>
        public static bool ContainsIgnoreCase(this string a, string b)
        {
            return a.ToUpper().Contains(b.ToUpper());
        }

        /// <summary>
        /// Has this list any item that matches the given function
        /// </summary>
        /// <typeparam name="T">the list type</typeparam>
        /// <param name="list">the list to check</param>
        /// <param name="matchFunc">the functions at least one item has to match</param>
        /// <returns>does at least one item match the given function?</returns>
        public static bool HasAnyWhere<T>(this List<T> list, Func<T, bool> matchFunc)
        {
            foreach (T item in list)
            {
                if (matchFunc(item)) return true;
            }
            return false;
        }

        /// <summary>
        /// Resize a image to a new size
        /// </summary>
        /// <param name="srcImg">the source image to scale</param>
        /// <param name="newSize">the new size the image should be scaled to</param>
        /// <returns>the scaled image</returns>
        public static Image Resize(this Image srcImg, Size newSize)
        {
            //adjust new size to match aspect ratio
            newSize = ResizeKeepAspect(srcImg.Size.Width, srcImg.Size.Height, newSize.Width, newSize.Height);

            //create target (new) image in given size
            Bitmap destImg = new Bitmap(newSize.Width, newSize.Height);

            //copy source image onto target image, scaling it accordingly
            using (Graphics g = Graphics.FromImage(destImg))
            {
                //use high quality scaling
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //draw the source image onto the target
                g.DrawImage(srcImg, new Rectangle(new Point(0, 0), newSize));
            }

            //return the scaled image
            return destImg;
        }

        /// <summary>
        /// Resize a Size to be within the given maximum width/height while retaining the aspect ratio
        /// Taken from https://stackoverflow.com/questions/1940581/c-sharp-image-resizing-to-different-size-while-preserving-aspect-ratio (by fubo)
        /// </summary>
        /// <param name="srcWidth">the original width</param>
        /// <param name="srcHeight">the original height</param>
        /// <param name="maxWidth">the maximum width</param>
        /// <param name="maxHeight">the maximum height</param>
        /// <param name="enlarge">if true, part of the image may be cut off to retain aspect ratio</param>
        /// <returns>the new size</returns>
        public static Size ResizeKeepAspect(int srcWidth, int srcHeight, int maxWidth, int maxHeight, bool enlarge = false)
        {
            maxWidth = enlarge ? maxWidth : Math.Min(maxWidth, srcWidth);
            maxHeight = enlarge ? maxHeight : Math.Min(maxHeight, srcHeight);

            double scaleFactor = Math.Min(maxWidth / (double)srcWidth, maxHeight / (double)srcHeight);
            return new Size((int)Math.Round(srcWidth * scaleFactor), (int)Math.Round(srcHeight * scaleFactor));
        }

        /// <summary>
        /// convert the size into a string in format wxh
        /// </summary>
        /// <param name="size">the size to convert</param>
        /// <returns>the size string</returns>
        public static string SizeToString(this Size size)
        {
            return $"{size.Width}x{size.Height}";
        }

        /// <summary>
        /// parse a size from a string  in format wxh(that was created using SizeToString function)
        /// </summary>
        /// <param name="str">the string to parse</param>
        /// <returns>the parsed size, or Size.Empty if parse failed</returns>
        public static Size StringToSize(string str)
        {
            try
            {
                var a = str.Split(new char[] { 'x' });
                return new Size()
                {
                    Width = int.Parse(a[0]),
                    Height = int.Parse(a[1])
                };
            }
            catch (Exception) { }
            return Size.Empty;
        }

        /// <summary>
        /// Create the directory the file path is in, if needed
        /// </summary>
        /// <param name="filePath">the path to create the direcotry for</param>
        public static void CreateFileDir(string filePath)
        {
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        }
    }
}
