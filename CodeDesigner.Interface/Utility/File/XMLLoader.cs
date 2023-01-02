using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.Interface.Project;

namespace CodeDesigner.Interface.Utility.File
{
    public static class XMLLoader
    {
        public static DesignerProject loadProject(string ofdPath)
        {
            

            try
            {
                using (FileStream fs = new (ofdPath, FileMode.Open))
                {
                    BinaryFormatter formatter = new();
                    return (DesignerProject) formatter.Deserialize(fs);
                }
            }
            catch 
            {
                return null;
            }
        }

        public static DesignerProject createProject(string path)
        {
            DesignerProject project = new DesignerProject(Path.GetFileNameWithoutExtension(path));

            using (FileStream fs = new(path, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new();
                formatter.Serialize(fs, project);
            }

            return project;
        }
    }
}
