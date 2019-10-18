using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BuildStar
{
    class BitmapConverter : IBitmapConverter
    {
        private Color[][] image;

        private struct ColorStruct
        {
            //reverse order from Marshal
            volatile public byte B, G, R, A;

            public Color toColor()
            {
                return Color.FromArgb(A, R, G, B);
            }
        }

        public BitmapConverter(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;

            BitmapData bmd = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int bmdHeight = bmd.Height;
            int bmdWidth = bmd.Width;
            int elemSize = bmd.Stride / bmd.Width;
            IntPtr scan0 = bmd.Scan0;

            // TODO: parallel [medium]

            Color[][] colors = new Color[bmdHeight][];

            for (int i = 0; i < bmdHeight; i++)
            {
                colors[i] = new Color[bmdWidth];
                for (int j = 0; j < bmdWidth; j++)
                {
                    ColorStruct cs = Marshal.PtrToStructure<ColorStruct>(scan0 + elemSize * (j + i * bmdWidth));
                    colors[i][j] = cs.toColor();
                }
            }

            image.UnlockBits(bmd);

            this.image = colors;
        }

        public Color[][] getAsMatrix()
        {
            return image;
        }

        public static Color[][] getAsMatrix(Bitmap image)
        {
            return new BitmapConverter(image).getAsMatrix();
        }
    }
}
