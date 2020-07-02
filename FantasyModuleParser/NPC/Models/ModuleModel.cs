using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.Models
{
    public class ModuleModel
    {
        // Useful for when saving back to the same filepath location
        public string SaveFilePath { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string ModFilename { get; set; }
        public string ThumbnailPath { get; set; }
        public bool IsGMOnly { get; set; }
        public bool IsLockedRecords { get; set; }
        public string ModulePath { get; set; }
        public List<NPCModel> NPCModels { get; set; }
        // Indicates the currently loaded NPC Model data object used in all NPC Engineer panels
        List<NPCModel> LoadedNPCModel { get; set; }

    }
}
