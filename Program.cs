using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Open.Numeric.Primes;


namespace ImageThrasher
{
    class Program
    {
        static void Main(string[] args)
        {
            PixelFormat format = PixelFormat.Format24bppRgb;
            int bpp = 3;

            bool alpha = false;
            if (args.FirstOrDefault(x => x == "--alpha") != default)
            {
                format = PixelFormat.Format32bppArgb;
                bpp = 4;
                alpha = true;
            }

            if (args.FirstOrDefault(x => x == "--decode") != default)
            {
                if (alpha)
                {
                    Console.WriteLine("--alpha and --decode are not supported when used together");
                    return;
                }
                File.WriteAllBytes(args[1], new ImageBuilder(bpp, format).ImageToBytes(Image.FromFile(args[0])));
                Console.WriteLine($"File saved to {args[1]}");
            }
            else
            {
                using var img = new ImageBuilder(bpp, format).BytesToImage(File.ReadAllBytes(args[0]));
                img.Save(args[1], ImageFormat.Png);
                Console.WriteLine($"Image saved to {args[1]}");
            }
        }
    }
}
