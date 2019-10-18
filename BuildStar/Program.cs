using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System;
using System.Net.Sockets;

namespace BuildStar
{
    class rgb
    {
        public Color[][] rgbs { get; private set; }

        public rgb(Bitmap bitmap)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            rgbs = new Color[height][];
            for (int i = 0; i < height; i++)
            {
                rgbs[i] = new Color[width];
                for (int j = 0; j < width; j++)
                {
                    rgbs[0][0] = bitmap.GetPixel(i, j);
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect("192.168.2.1", 25575);

            var comms = new Comms(socket);

            comms.login("123454321Testmynewpassword");

            List<IBlock> blocks = BitmapProcessor.LoadImages("../../blocks/");
            blocks.AddRange(BitmapProcessor.LoadImages("../../blocks/sands"));

            BlockBatchProcessor.varianceFilter(blocks, 100000);

            //int found = BlockMatcher.matchColor(Color.Black, blocks);

            //Console.Out.WriteLine(blocks[found].Name);

            var pt = new BlockPainter(new EuclideanMatcher(), blocks);

            pt.setOrientation(Orientation.minusX_minusY);

            var commands = pt.paintBitmapFrom((Bitmap)Image.FromFile("../../Untitled-2.png"), 28, 92, 273, false, true);
            
            List<string> stringCommands = CommandBuilder.makeMultiple(commands, false);

            foreach (string command in stringCommands)
            {
                Console.Out.WriteLine(command);
                comms.execute(command);
            }

            Console.ReadKey();
        }
    }
}
