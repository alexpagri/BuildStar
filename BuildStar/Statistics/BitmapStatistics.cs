using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuildStar
{
    class BitmapStatistics : IBitmapStatistics
    {
        public float cachedVariance { get; private set; }
        public IPointMath3D cachedMean { get; private set; }
        private Color[][] cache;

        public BitmapStatistics(Color[][] cache)
        {
            this.cache = cache;
        }

        public IPointMath3D getMean()
        {
            uint R = 0, G = 0, B = 0, count = 0;

            // TODO: parallel with atomic add

            Color[][] mat = cache;

            foreach (Color[] row in mat)
            {
                foreach (Color pixel in row)
                {
                    R += pixel.R;
                    G += pixel.G;
                    B += pixel.B;
                    ++count;
                }
            }

            return cachedMean = new PointMath3D(R / (float)count, G / (float)count, B / (float)count);
        }

        public float getVariance()
        {
            uint count = 0;
            double distanceSquared = 0;

            // TODO: bool variable that checks if mean computed

            getMean();

            // TODO: parallel with atomic add

            Color[][] mat = cache;

            foreach (Color[] row in mat)
            {
                foreach (Color pixel in row)
                {
                    distanceSquared += new PointMath3D(pixel).distanceSquared(cachedMean);
                    ++count;
                }
            }

            return cachedVariance = (float)(distanceSquared / count);
        }
    }
}
