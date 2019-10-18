using System;
using System.Collections.Generic;
using System.Drawing;

namespace BuildStar
{
    class DebugMatcher : IBlockMatcher
    {
        private int at = 0;
        public void reset()
        {
            at = 0;
        }
        public int matchColor(Color color, List<IBlock> blocks)
        {
            if (at >= blocks.Count) reset();
            return at++;
        }
    }
}
