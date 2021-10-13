using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Main.Models
{
    public class ModuleModel : ModelBase
    {
        public NPCController NpcController { get; set; }
        public NPCModel NpcModel { get; set; }
        public CategoryModel Categorymodel { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string ModFilename { get; set; }
        public string ThumbnailPath { get; set; }
        public bool IsGMOnly { get; set; }
        public bool IsLockedRecords { get; set; }
        public ObservableCollection<CategoryModel> Categories { get; set; }
        public bool IncludesClasses { get; set; }
        public bool IncludesEquipment { get; set; }
        public bool IncludeImages { get; set; }
        public bool IncludeTokens { get; set; }
        public bool IncludeNPCs { get; set; }
        public bool IncludeSpells { get; set; }
        public bool IncludeTables { get; set; }
        public ModuleModel ShallowCopy()
        {
            return (ModuleModel)this.MemberwiseClone();
        }
    }
}
