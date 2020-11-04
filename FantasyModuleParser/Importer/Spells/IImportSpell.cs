using FantasyModuleParser.Spells.Models;

namespace FantasyModuleParser.Importer.Spells
{
    public interface IImportSpell
    {
        SpellModel ImportTextToSpellModel(string importData);
    }
}
