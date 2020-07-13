using FantasyModuleParser.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Main.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public List<NPCModel> NPCModels { get; set; } = new List<NPCModel>();

        public string ToString()
        {
            return Name;
        }
    }
}
