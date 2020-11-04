using FantasyModuleParser.Spells.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Importer.Spells
{
    public class ImportSpellBase : IImportSpell
    {
        public SpellModel ImportTextToSpellModel(string importData)
        {
            throw new NotImplementedException();
        }
    }
}
