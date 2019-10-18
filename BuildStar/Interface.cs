using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildStar
{
    //used in BitmapStatistics
    //basically Color but with float instead of byte
    //precision
    interface IPointMath3D
    {
        float x { get; }
        float y { get; }
        float z { get; }

        float distanceSquared(IPointMath3D to);
        Color XYZtoColorRGB();
    }

    //Color[][] is the base storage of Block data
    //just for converting Bitmap to Color[][]
    //efficiency
    interface IBitmapConverter
    {
        Color[][] getAsMatrix();
    }

    //utility class, used in Block
    //for discarding visually incorrect images
    //correctness
    interface IBitmapStatistics
    {
        //in terms of euclidean distance
        float getVariance();

        //colorwise mean
        IPointMath3D getMean();
    }

    //used as color picking strategy
    //implementation free of any constraints!
    //creativity
    interface IBlockMatcher
    {
        int matchColor(Color color, List<IBlock> blocks);
    }

    //used as main block entity
    //holds name, bitmap and cache of bitmap and statistics
    //base
    interface IBlock
    {
        string Name { get; set; }

        //warning! same constraint as Cache field, do not get until set
        //this field must be set before object usage
        Bitmap Image { get; set; }

        //warning! do not get the cache field unless you are sure that Image has been set
        //this constraint boosts efficiency
        Color[][] Cache { get; }

        //warning! same constraint as Cache field
        IBitmapStatistics Stats { get; }
    }

    public enum Orientation : byte
    {
        //assuming image is WxH (Width, Height)
        //ex: for minusX_plusZ => pixelToBeDrawn = Color[i][j]; [i:Height][j:Width], (when j increases, X decreases), (when i increases, Z decreases)

        //horizontal plane (top + bottom) (image builds back right)
        minusX_minusZ = 0, // 0
        plusZ_minusX = 1, // 90
        plusX_plusZ = 2, // 180
        minusZ_plusX = 3, // 270
        //sides facing
        minusX_minusY = 4, // 0
        plusZ_minusY = 5, // 90
        plusX_minusY = 6, // 180
        minusZ_minusY = 7, // 270
    }

    //uses pretty much everything
    interface IBlockPainter
    {
        //sets an instance var to be used in paintBitmapFrom
        void setOrientation(Orientation orientation);

        //use IBitmapConverter to use Color[][] array directly
        List<(long, long, long, IBlock)> paintBitmapFrom(Bitmap image, int x, int y, int z, bool back = true, bool right = true);
    }
}
