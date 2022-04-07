namespace FantasyModuleParser.Main.Models
{
    public class SettingsModel : ModelBase
    {
        public string MainFolderLocation { get; set; }
        public string ProjectFolderLocation { get; set; }
        public string FGModuleFolderLocation { get; set; }
        public string FGCampaignFolderLocation { get; set; }
        public string NPCFolderLocation { get; set; }
        public string SpellFolderLocation { get; set; }
        public string EquipmentFolderLocation { get; set; }
        public string ArtifactFolderLocation { get; set; }
        public string TableFolderLocation { get; set; }
        public string ParcelFolderLocation { get; set; }
        public bool PersistentWindow { get; set; }
        public string DefaultGUISelection { get; set; }
        public string LogFolderLocation { get; set; }
        public string LogLevel { get; set; }
        public bool LoadLastProject { get; set; }
        public string LastProject { get; set; }
    }
}
