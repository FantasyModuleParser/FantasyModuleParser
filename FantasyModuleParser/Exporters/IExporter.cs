using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using FantasyModuleParser.Spells.Models;

namespace FantasyModuleParser.Exporters
{
    public interface IExporter
    {
        void CreateModule(ModuleModel moduleModel);
    }
}
