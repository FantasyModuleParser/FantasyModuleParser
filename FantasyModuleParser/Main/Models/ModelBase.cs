namespace FantasyModuleParser.Main.Models
{
    public class ModelBase
    {
        public string Name { get; set; }
        // Added VersionNumber for generated files for debugging purposes
        public string VersionNumber
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        
    }
}
