using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildStar
{
    interface IBitmapConverter
    {
        Color[][] getAsMatrix();
    }
    public enum Orientation : byte
    {
        //assuming image is WxH (Width, Height)
        //ex: for minusX_plusZ => pixelToBeDrawn = Color[i][j]; [i:Height][j:Width], (when j increases, X decreases), (when i increases, Z increases)

        //horizontal plane (top + bottom)
        minusX_plusZ, // 0
        plusZ_plusX, // 90
        plusX_minusZ, // 180
        minusZ_minusX, // 270
        //sides facing
        minusX_plusY, // 0
        plusZ_plusY, // 90
        plusX_plusY, // 180
        minusZ_plusY, // 270
    }
    public interface IBlockPainter
    {
        //sets an instance var to be used in paintBitmapFrom
        void setOrientation(Orientation orientation);

        //use IBitmapCOnverter to use Color[][] array directly
        void paintBitmapFrom(Bitmap image, int x, int y, int z);
    }
}
