using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace XMLSchemes
{
    public struct Author
    {
        public string name;
        public string steampage;
    }

    public struct Pin
    {
        public int id;
        public bool inverted;
        public string name;
        public Vector3 offset;
    }

    public class ModuleSettings
    {
        public string name;
        public string type;
        public string workshop;
        public Author author;
        public string description;
        public Vector3 offset;
        public Pin[] pins;
    }
}
