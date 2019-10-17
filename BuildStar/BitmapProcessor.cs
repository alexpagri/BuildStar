using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildStar
{
    class BitmapProcessor
    {

        public static List<BlockModel> LoadImages(string path)
        {
            List<BlockModel> result = new List<BlockModel>();
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles("*.*");
            foreach (var file in Files)
            {
                try
                {
                    BlockModel auxBlock = new BlockModel();
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
