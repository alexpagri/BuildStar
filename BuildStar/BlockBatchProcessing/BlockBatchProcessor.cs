using System.Collections.Generic;

namespace BuildStar
{
    static class BlockBatchProcessor
    {
        public static void varianceFilter(List<IBlock> blocks, float variance = 500)
        {
            blocks.RemoveAll((IBlock block) =>
            {
                return block.Stats.getVariance() > variance;
            });
        }
    }
}
