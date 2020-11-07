using FantasyModuleParser.Spells.Enums;
using FantasyModuleParser.Spells.Models;

namespace FantasyModuleParser.Importer.Spells
{
    public interface IImportSpell
    {
        SpellModel ImportTextToSpellModel(string importData);

        SpellLevel ParseSpellLevel(string importDataLine);
    }
}
