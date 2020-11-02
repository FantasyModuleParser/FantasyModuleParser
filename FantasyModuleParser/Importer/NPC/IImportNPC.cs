using FantasyModuleParser.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Importer.NPC
{
    public interface IImportNPC
    {
        NPCModel ImportTextToNPCModel(string importTextContent);
    }
}
