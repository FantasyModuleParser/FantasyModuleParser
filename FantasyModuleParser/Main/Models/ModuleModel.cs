using System.Collections.ObjectModel;

namespace FantasyModuleParser.Main.Models
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
        public ObservableCollection<CategoryModel> Categories { get; set; }

    }
}
