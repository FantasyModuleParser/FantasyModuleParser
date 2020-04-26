using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC
{
    public class NPCDataModel
    {
        public string NPCName { get; set; }
        public string Size { get; set; }
        public string NPCType { get; set; } 
        public string Tag { get; set; }
        public string Alignment { get; set; }
        public string AC { get; set; }
        public string HP { get; set; }
        public string NPCGender { get; set; }
        public bool Unique { get; set; }
        public bool NPCNamed { get; set; }
        public string Speed { get; set; }
        public string Burrow { get; set; }
        public string Climb { get; set; }
        public string Fly { get; set; }
        public bool Hover { get; set; }
        public string Swim { get; set; }
    }
}
