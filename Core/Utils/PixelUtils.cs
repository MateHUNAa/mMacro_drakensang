using mMacro.Core.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace mMacro.Core.Utils
{
    public static class PixelUtils
    {
        public static bool CheckPixel(Color pixel, Dictionary<string, ColorRange> ColorRanges)
        {
            return ColorRanges.Values.Any(c => c.IsInRange(pixel));
        }
        public static bool CheckPixel(Color pixel, ColorRange colorRange)
        {
            return colorRange.IsInRange(pixel);
        }
        public static bool CheckPixel(Color pixel, List<ColorRange> colorRanges)
        {
            return colorRanges.Any(c => c.IsInRange(pixel));
        }
        public static bool CheckPixel(Color pixel, Color color)
        {
            ColorRange colorRange = new ColorRange
            {
                R=(color.R-1, color.R+1),
                G=(color.G-1, color.G+1),
                B=(color.B-1, color.B+1)
            };

            return colorRange.IsInRange(pixel);
        }
        public static Color GetPixelColor(int x, int y)
        {
            using var bmp = new Bitmap(1, 1);
            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen(x, y, 0, 0, new Size(1, 1));
            return bmp.GetPixel(0, 0);
        }
        public static Color GetPixelColor(Vector2 pos)
        {
            using var bmp = new Bitmap(1, 1);
            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen((int)pos.X, (int)pos.Y, 0, 0, new Size(1, 1));
            return bmp.GetPixel(0, 0);
        }
        public static Color GetPixelColor(Point pos)
        {
            using var bmp = new Bitmap(1, 1);
            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen((int)pos.X, (int)pos.Y, 0, 0, new Size(1, 1));
            return bmp.GetPixel(0, 0);
        }
        public static Color GetPixelColor(float x, float  y)
        {
            using var bmp = new Bitmap(1, 1);
            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen((int)x, (int)y, 0, 0, new Size(1, 1));
            return bmp.GetPixel(0, 0);
        }

        public struct ColorRange
        {
            public (int Min, int Max) R;
            public (int Min, int Max) G;
            public (int Min, int Max) B;

            public bool IsInRange(System.Drawing.Color color)
            {
                return color.R >= R.Min-1 && color.R <= R.Max+1 &&
                       color.G >= G.Min-1 && color.G <= G.Max+1 &&
                       color.B >= B.Min-1 && color.B <= B.Max+1;
            }
        }
    }
}
