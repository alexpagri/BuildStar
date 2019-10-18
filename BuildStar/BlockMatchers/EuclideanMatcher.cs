using System;
using System.Collections.Generic;
using System.Drawing;

namespace BuildStar
{
    class EuclideanMatcher : IBlockMatcher
    {
        public int matchColor(Color color, List<IBlock> blocks)
        {
            int length = blocks.Count;
            double min = double.PositiveInfinity;

            IPointMath3D colorPrecise = new PointMath3D(color);

            // TODO: parallel, separate work to multiple threads, then collect minimum and compare
            // with final thread

            int save = 0;

            for (int i = 0; i < length; i++)
            {
                float local = colorPrecise.distanceSquared(blocks[i].Stats.getMean());

                if (min > local)
                {
                    min = local;
                    save = i;
                }

                //Console.Out.WriteLine(blocks[i].Name + " at distance of " + local);
            }

            return save;
        }
    }
}
