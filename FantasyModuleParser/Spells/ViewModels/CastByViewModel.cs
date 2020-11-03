using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Spells.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Spells.ViewModels
{
    public class CastByViewModel : ViewModelBase
    {
        private List<string> customCastersItems = new List<string>();
        public List<string> CustomCastersItems
        {
            get
            {
                return customCastersItems;
            }
        }
        public CastByViewModel()
        {
            FMPConfigurationService fmpConfigurationService = new FMPConfigurationService();
            FMPConfigurationModel fMPConfigurationModel = fmpConfigurationService.Load();
            foreach (CustomCastersModel customCastersModel in fMPConfigurationModel.CustomCasters)
                customCastersItems.Add(customCastersModel.Caster);
        }
        public List<string> SpellCharacterClassItems
        {
            get
            {
                return new List<string>()
                {
                    "Artificer",
                    "Bard",
                    "Cleric",
                    "Druid",
                    "Fighter",
                    "Monk",
                    "Paladin",
                    "Ranger",
                    "Rogue",
                    "Sorcerer",
                    "Warlock",
                    "Wizard"
                };
            }
        }
        public List<string> SpellDivineArchetypesItems
        {
            get
            {
                return new List<string>()
                {
                    "Cleric Arcana Domain",
                    "Cleric Death Domain",
                    "Cleric Forge Domain",
                    "Cleric Grave Domain",
                    "Cleric Knowledge Domain",
                    "Cleric Life Domain",
                    "Cleric Light Domain",
                    "Cleric Nature Domain",
                    "Cleric Tempest Domain",
                    "Cleric Trickery Domain",
                    "Cleric War Domain",
                    "Druid Arctic Circle",
                    "Druid Coast Circle",
                    "Druid Desert Circle",
                    "Druid Dream Circle",
                    "Druid Forest Circle",
                    "Druid Grassland Circle",
                    "Druid Moon Circle",
                    "Druid Mountain Circle",
                    "Druid Shepherd Circle",
                    "Druid Spore Circle",
                    "Druid Swamp Circle",
                    "Druid Underdark Circle"
                };
            }
        }
        public List<string> SpellArcaneArchetypesItems
        {
            get
            {
                return new List<string>()
                {
                    "Artificer (Alchemist)",
                    "Artificer (Artillerist)",
                    "Sorcerer (Divine Soul)",
                    "Sorcerer (Draconic Bloodline)",
                    "Sorcerer (Shadow Magic)",
                    "Sorcerer (Storm Sorcery)",
                    "Sorcerer (Wild Magic)",
                    "Warlock (Archfey)",
                    "Warlock (Celestial)",
                    "Warlock (Fiend)",
                    "Warlock (Great One)",
                    "Warlock (Hexblade)",
                    "Warlock (Undying)",
                    "Wizard (Abjuration)",
                    "Wizard (Bladesinging)",
                    "Wizard (Conjuration)",
                    "Wizard (Divination)",
                    "Wizard (Enchantment)",
                    "Wizard (Evocation)",
                    "Wizard (Illusion)",
                    "Wizard (Invention)",
                    "Wizard (Necromancy)",
                    "Wizard (Transmutation)",
                    "Wizard (War Magic)"
                };
            }
        }
        public List<string> SpellOtherArchetypesItems
        {
            get
            {
                return new List<string>()
                {
                    "Arcane Trickster",
                    "Bard College of Glamour",
                    "Bard College of Lore",
                    "Bard College of Swords",
                    "Bard College of Valor",
                    "Bard College of Whispers",
                    "Eldritch Knight",
                    "Paladin Oath of Conquest",
                    "Paladin Oath of Devotion",
                    "Paladin Oath of Redemption",
                    "Paladin Oath of the Ancients",
                    "Paladin Oath of the Crown",
                    "Paladin Oath of Vengeance",
                    "Paladin Oathbreaker",
                    "Ranger Beast Master",
                    "Ranger Gloom Stalker",
                    "Ranger Horizon Walker",
                    "Ranger Hunter",
                    "Ranger Monster Slayer"
                };
            }
        }
    }
    
}
