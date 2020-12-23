using FantasyModuleParser.NPC;

namespace FantasyModuleParser.Importer.NPC
{
    public interface IImportNPC
    {
        NPCModel ImportTextToNPCModel(string importTextContent);
    }
}
