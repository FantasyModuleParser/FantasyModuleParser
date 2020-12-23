using FantasyModuleParser.Spells.Models;

namespace FantasyModuleParser.Spells.Services
{
    public interface ISpellService
    {
        void Save();
        void Save(string filePath);
        SpellModel Load(string filePath);
        
    }
}
