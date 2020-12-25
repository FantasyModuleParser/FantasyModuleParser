using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using log4net;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class NPCOptionControlViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NPCOptionControlViewModel));
        private ModuleService _moduleService;
        public ModuleModel ModuleModel { get; set; }
        public ObservableCollection<CategoryModel> ViewModelCategories { get; set; }
        public DescriptionUCViewModel DescriptionUCViewModel { get; set; }
        public NPCOptionControlViewModel()
        {
            _moduleService = new ModuleService();
            ModuleModel = _moduleService.GetModuleModel();
            ViewModelCategories = ModuleModel.Categories;
            DescriptionUCViewModel = new DescriptionUCViewModel();
        }
        public void Refresh()
        {
            ModuleModel = _moduleService.GetModuleModel();
            ViewModelCategories = ModuleModel.Categories;
            RaisePropertyChanged("NPCOptionControlViewModel");
        }

        public void AddNPCToCategory(NPCModel npcModel, string categoryValue)
        {
            if (categoryValue == null || categoryValue.Length == 0)
            {
                log.Error("Category  value is null; Cannot save NPC");
                throw new InvalidDataException("Category value is null;  Cannot save NPC");
            }  
            if (npcModel == null)
            {
                log.Error("NPC Model data object is null");
                throw new InvalidDataException("NPC Model data object is null");
            }

            ModuleModel _moduleModel = _moduleService.GetModuleModel();
            CategoryModel categoryModel = _moduleModel.Categories.FirstOrDefault(item => item.Name.Equals(categoryValue));
            if (categoryModel == null)
            {
                log.Error("NPC " + npcModel.NPCName + " must have a valid Category selected");
                throw new InvalidDataException("Category Value is not in the Module Model data object!");
            }
            else if (categoryModel.NPCModels.FirstOrDefault(npc => npc.NPCName.Equals(npcModel.NPCName)) == null)
            {
                if (string.IsNullOrEmpty(npcModel.NPCType))
                    log.Warn("NPC Type is missing from " + npcModel.NPCName);
                if (string.IsNullOrEmpty(npcModel.Size))
                    log.Warn("Size is missing from " + npcModel.NPCName);
                if (string.IsNullOrEmpty(npcModel.AC))
                    log.Warn("AC is missing from " + npcModel.NPCName);
                if (string.IsNullOrEmpty(npcModel.Alignment))
                    log.Warn("Alignment is missing from " + npcModel.NPCName);
                if (string.IsNullOrEmpty(npcModel.ChallengeRating))
                    log.Warn("Challenge Rating is missing from " + npcModel.NPCName);
                if (!string.IsNullOrEmpty(npcModel.HP))
                    log.Warn("Hit Points are missing from " + npcModel.NPCName);
                if (!string.IsNullOrEmpty(npcModel.LanguageOptions))
                    log.Warn("Language Option (usually No special conditions) is missing from " + npcModel.NPCName);
                if (npcModel.Telepathy == true && string.IsNullOrEmpty(npcModel.TelepathyRange))
                    log.Warn("Telepathy Range is missing from " + npcModel.NPCName);
                if (npcModel.InnateSpellcastingSection == true && !string.IsNullOrEmpty(npcModel.InnateSpellcastingAbility))
                    log.Warn("Innate Spellcasting Ability is missing from " + npcModel.NPCName);
                if (npcModel.SpellcastingSection == true)
                {
                    if (string.IsNullOrEmpty(npcModel.SCSpellcastingAbility))
                        log.Warn("Spellcasting Ability is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.CantripSpellList) && string.IsNullOrEmpty(npcModel.CantripSpells))
                        log.Warn("Number of Cantrip slots is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.FirstLevelSpellList) && string.IsNullOrEmpty(npcModel.FirstLevelSpells))
                        log.Warn("Number of First Level Spell slots is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.SecondLevelSpellList) && string.IsNullOrEmpty(npcModel.SecondLevelSpells))
                        log.Warn("Number of Second Level Spell slots is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.ThirdLevelSpellList) && string.IsNullOrEmpty(npcModel.ThirdLevelSpells))
                        log.Warn("Number of Third Level Spell slots is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.FourthLevelSpellList) && string.IsNullOrEmpty(npcModel.FourthLevelSpells))
                        log.Warn("Number of Fourth Level Spell slots is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.FifthLevelSpellList) && string.IsNullOrEmpty(npcModel.FifthLevelSpells))
                        log.Warn("Number of Fifth Level Spell slots is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.SixthLevelSpellList) && string.IsNullOrEmpty(npcModel.SixthLevelSpells))
                        log.Warn("Number of Sixth Level Spell slots is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.SeventhLevelSpellList) && string.IsNullOrEmpty(npcModel.SeventhLevelSpells))
                        log.Warn("Number of Seventh Level Spell slots is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.EighthLevelSpellList) && string.IsNullOrEmpty(npcModel.EighthLevelSpells))
                        log.Warn("Number of Eighth Level Spell slots is missing from " + npcModel.NPCName);
                    if (string.IsNullOrEmpty(npcModel.NinthLevelSpellList) && string.IsNullOrEmpty(npcModel.NinthLevelSpells))
                        log.Warn("Number of Ninth Level Spell slots is missing from " + npcModel.NPCName);
                }
                categoryModel.NPCModels.Add(npcModel);  // The real magic is here
                log.Info("NPC has been added to project successfully");
            }          
        }
    }
}
