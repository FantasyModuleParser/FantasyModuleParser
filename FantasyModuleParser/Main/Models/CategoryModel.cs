using FantasyModuleParser.NPC;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Tables.Models;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Main.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public ObservableCollection<NPCModel> NPCModels { get; set; } = new ObservableCollection<NPCModel>();
        public ObservableCollection<SpellModel> SpellModels { get; set; } = new ObservableCollection<SpellModel>();
        public ObservableCollection<TableModel> TableModels { get; set; } = new ObservableCollection<TableModel>();
    }
}
