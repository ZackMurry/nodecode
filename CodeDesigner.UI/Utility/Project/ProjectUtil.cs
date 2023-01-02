using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Utility.Image;

namespace CodeDesigner.UI.Utility.Project
{
    public static class ProjectUtil
    {
        public static void Save(List<BlockBase?> blocks, string name, Bitmap thumbnail)
        {
            GaussianBlur blur = new(thumbnail);
            Bitmap blurred = blur.Process(5);

            NodeMap map = new()
            {
                Blocks = blocks,
                Name = name,
                Thumbnail = blurred
            };

            BinaryFormatter formatter = new();
            
            using (FileStream stream = new(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + name + ".nodecode", FileMode.Create))
            {
                formatter.Serialize(stream, map);
            }
        }
    }
}
