using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildStar
{
    class PointMath3D : IPointMath3D
    {
        public float x { get; private set; }
        public float y { get; private set; }
        public float z { get; private set; }

        public PointMath3D(Color color)
        {
            x = color.R;
            y = color.G;
            z = color.B;
        }

        public PointMath3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float distanceSquared(IPointMath3D to)
        {
            double Xr = to.x - x;
            double Yr = to.y - y;
            double Zr = to.z - z;

            return (float)(Xr * Xr + Yr * Yr + Zr * Zr);
        }

        public Color XYZtoColorRGB()
        {
            return Color.FromArgb((int)x, (int)y, (int)z);
        }
    }
}
