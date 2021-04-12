using System.Collections.ObjectModel;

namespace FantasyModuleParser.Main.Models
{
    public class ModuleModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string ModFilename { get; set; }
        public string ThumbnailPath { get; set; }
        public bool IsGMOnly { get; set; }
        public bool IsLockedRecords { get; set; }
        public ObservableCollection<CategoryModel> Categories { get; set; }
        public bool IncludeImages { get; set; }
        public bool IncludeTokens { get; set; }
        public bool IncludeNPCs { get; set; }
        public bool IncludeSpells { get; set; }
        public bool IncludeTables { get; set; }
    }
}
