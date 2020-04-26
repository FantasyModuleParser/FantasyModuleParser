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
        public int Speed { get; set; }
        public int Burrow { get; set; }
        public int Climb { get; set; }
        public int Fly { get; set; }
        public bool Hover { get; set; }
        public int Swim { get; set; }
        public int AttributeStr { get; set; }
        public int AttributeDex { get; set; }
        public int AttributeCon { get; set; }
        public int AttributeInt { get; set; }
        public int AttributeWis { get; set; }
        public int AttributeCha { get; set; }
        public int SavingThrowStr { get; set; }
        public bool SavingThrowStr0 { get; set; }
        public int SavingThrowDex { get; set; }
        public bool SavingThrowDex0 { get; set; }
        public int SavingThrowCon { get; set; }
        public bool SavingThrowCon0 { get; set; }
        public int SavingThrowInt { get; set; }
        public bool SavingThrowInt0 { get; set; }
        public int SavingThrowWis { get; set; }
        public bool SavingThrowWis0 { get; set; }
        public int SavingThrowCha { get; set; }
        public bool SavingThrowCha0 { get; set; }
    }
}
