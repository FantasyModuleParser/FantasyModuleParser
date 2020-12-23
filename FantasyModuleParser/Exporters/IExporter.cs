using FantasyModuleParser.Main.Models;

namespace FantasyModuleParser.Exporters
{
    public interface IExporter
    {
        void CreateModule(ModuleModel moduleModel);
    }
}
