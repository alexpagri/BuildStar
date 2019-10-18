using System.Drawing;
using System;
using System.Collections.Generic;

namespace BuildStar
{
    class OperationMapper
    {
        private int bx, by, bz, w, h;
        bool bk, rt;
        public Orientation orientation;

        public (long, long, long) compute(long lw, long lh)
        {
            byte o = (byte)orientation;

            long x = 0, y = 0, z = 0;

            // TODO: move out boolean to setBase

            bool widthMinus = (o == 0 || o == 3 || o == 4 || o == 7);
            bool widthComponentX = ((o & 1) == 0);
            bool widthComponentZ = !widthComponentX;
            bool widthComponentY = !widthComponentX && !widthComponentZ;

            bool heightMinus = (o != 2 && o != 3);
            bool heightComponentX = (o == 1 || o == 3);
            bool heightComponentY = (o > 3);
            bool heightComponentZ = !heightComponentX && !heightComponentY;

            lw -= (rt) ? 0 : (w - 1);
            lh -= (bk) ? 0 : (h - 1);

            lw = (widthMinus) ? -lw : lw;
            lh = (heightMinus) ? -lh : lh;

            x += (widthComponentX) ? lw : 0;
            x += (heightComponentX) ? lh : 0;

            y += (widthComponentY) ? lw : 0;
            y += (heightComponentY) ? lh : 0;

            z += (widthComponentZ) ? lw : 0;
            z += (heightComponentZ) ? lh : 0;

            return (bx + x, by + y, bz + z);
        }

        public OperationMapper()
        {
            orientation = Orientation.minusX_minusZ;
        }

        public void setBase(int x, int y, int z, int w, int h, bool back = true, bool right = true)
        {
            bx = x;
            by = y;
            bz = z;
            this.w = w;
            this.h = h;
            bk = back;
            rt = right;
        }
    }
    class BlockPainter : IBlockPainter
    {
        private List<IBlock> blocks;

        private OperationMapper mapper;

        private IBlockMatcher matcher;

        public BlockPainter(IBlockMatcher matcher, List<IBlock> blocks)
        {
            this.blocks = blocks;
            this.matcher = matcher;
            mapper = new OperationMapper();
        }

        public void setOrientation(Orientation orientation)
        {
            mapper.orientation = orientation;
        }

        public List<(long, long, long, IBlock)> paintBitmapFrom(Bitmap image, int x, int y, int z, bool back = true, bool right = true)
        {
            // TODO: preprocessing heavy
            Color[][] cache = BitmapConverter.getAsMatrix(image);

            mapper.setBase(x, y, z, cache[0].Length, cache.Length, back, right);
            List<(long, long, long, IBlock)> commands = new List<(long, long, long, IBlock)>();

            // TODO: processing heavy

            int length = cache.Length;
            for (int i = 0; i < length; i++)
            {
                Color[] row = cache[i];
                int rowLength = row.Length;
                for (int j = 0; j < rowLength; j++)
                {
                    (long a, long b, long c) = mapper.compute(j, i);
                    //Console.Out.WriteLine("for j: " + j + " and i: " + i + " => x: " +
                    //    a + " y: " + b + " z: " + c);
                    commands.Add((a, b, c, blocks[matcher.matchColor(row[j], blocks)]));
                }
            }

            return commands;
        }
    }
}
