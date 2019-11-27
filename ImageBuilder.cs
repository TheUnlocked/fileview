using Open.Numeric.Primes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageThrasher
{
    public class ImageBuilder
    {
        int Bpp { get; }
        PixelFormat format { get; }

        public ImageBuilder(int bytesPerPixel, PixelFormat format)
        {
            Bpp = bytesPerPixel;
            this.format = format;
        }

        public Image BytesToImage(byte[] bytes)
        {
            Console.WriteLine($"Input file with {bytes.Length} bytes");
            var numPixels = (long)Math.Ceiling((double)bytes.Length / Bpp);
            var (width, height) = GetDimensions(numPixels);
            while (width * Bpp % 4 != 0)
            {
                width++;
            }
            while (width * height * Bpp < bytes.Length + sizeof(int))
            {
                height++;
            }
            Console.WriteLine($"Size: {width}, {height}");
            var bmp = new Bitmap((int)width, (int)height, format);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, format);
            Marshal.WriteInt32(bmpData.Scan0, bytes.Length);
            Marshal.Copy(bytes, 0, bmpData.Scan0 + sizeof(int), bytes.Length);
            bmp.UnlockBits(bmpData);
            Console.WriteLine($"Created bitmap with {bmp.Width * bmp.Height * Bpp} bytes");
            return bmp;
        }

        public byte[] ImageToBytes(Image img)
        {
            using var bmp = new Bitmap(img);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, format);
            var len = Marshal.ReadInt32(bmpData.Scan0);
            var data = new byte[len];
            Marshal.Copy(bmpData.Scan0 + sizeof(int), data, 0, len);
            Console.WriteLine($"Loaded {len} bytes");
            bmp.UnlockBits(bmpData);
            return data;
        }

        public (long width, long height) GetDimensions(long length)
        {
            var factors = Prime.Factors(length).ToArray();

            var sorted = factors.OrderByDescending(Util.Id);
            long width = 1, height = 1;
            foreach (var num in sorted)
            {
                // Trend towards 16:9
                if (height * 16 < width * 9)
                {
                    height *= num;
                }
                else
                {
                    width *= num;
                }
            }
            while (width > 6 * height)
            {
                width /= 2;
                height *= 2;
            }
            while (height > width * 2)
            {
                width *= 2;
                height /= 2;
            }
            return (width, height);
        }
    }
}
