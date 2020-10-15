using FantasyModuleParser.Spells.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Spells.Services
{
    public interface ISpellService
    {
        void Save();
        void Save(string filePath);
        SpellModel Load(string filePath);
        
    }
}
