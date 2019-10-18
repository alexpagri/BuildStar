using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildStar
{
    class Block : IBlock
    {
        public string Name { get; set; }

        //when setting, automatically compute cache
        public Bitmap Image { get => _Image; set { _Image = value; _Cache = new BitmapConverter(value); Stats = new BitmapStatistics(_Cache.getAsMatrix()); } }

        //when getting, return from cache
        public Color[][] Cache { get => _Cache.getAsMatrix(); }

        public IBitmapStatistics Stats { get; private set; }

        private IBitmapConverter _Cache;

        //need this... otherwise stack overflows since Image actually does not exist
        private Bitmap _Image;
    }
}
