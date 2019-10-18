using System.Collections.Generic;

namespace BuildStar
{
    static class CommandBuilder
    {
        public static string make(long x, long y, long z, IBlock block, bool keep = true)
        {
            return "setblock " + x + " " + y + " " + z + " minecraft:" + block.Name + " " + ((keep) ? "keep" : "replace");
        }

        public static List<string> makeMultiple(List<(long x, long y, long z, IBlock block)> commands, bool keep = true)
        {
            List<string> stringCommands = new List<string>();

            foreach ((long x, long y, long z, IBlock block) in commands)
            {
                stringCommands.Add(make(x, y, z, block, keep));
            }

            return stringCommands;
        }
    }
}
