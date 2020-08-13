using FantasyModuleParser.NPC;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Main.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public ObservableCollection<NPCModel> NPCModels { get; set; } = new ObservableCollection<NPCModel>();
    }
}
