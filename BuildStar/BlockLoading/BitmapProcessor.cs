using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildStar
{
    static class BitmapProcessor
    {

        public static List<IBlock> LoadImages(string path)
        {
            List<IBlock> result = new List<IBlock>();
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles("*.*");
            foreach (var file in Files)
            {
                try
                {
                    Block auxBlock = new Block();
                    auxBlock.Name = Path.GetFileNameWithoutExtension(file.FullName);
                    auxBlock.Image = (Bitmap)Image.FromFile(file.FullName);
                    result.Add(auxBlock);
                }
                catch (Exception e)
                {

                }
            }
            return result;
        }
    }
}
