using FantasyModuleParser.NPC.Models.Skills;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.Controllers
{
    public class LanguageController
    {
        public ObservableCollection<LanguageModel> GenerateStandardLanguages()
        {
            ObservableCollection<LanguageModel> langs = new ObservableCollection<LanguageModel>();
            langs.Add(new LanguageModel("Common"));
            langs.Add(new LanguageModel("Drow Sign"));
            langs.Add(new LanguageModel("Dwarvish"));
            langs.Add(new LanguageModel("Elvish"));
            langs.Add(new LanguageModel("Giant"));
            langs.Add(new LanguageModel("Gnomish"));
            langs.Add(new LanguageModel("Goblin"));
            langs.Add(new LanguageModel("Halfling"));
            langs.Add(new LanguageModel("Orc"));
            langs.Add(new LanguageModel("Thieves' Cant"));
            return langs;
        }

        public ObservableCollection<LanguageModel> GenerateExoticLanguages()
        {
            ObservableCollection<LanguageModel> langs = new ObservableCollection<LanguageModel>();

            langs.Add(new LanguageModel("Abyssal"));
            langs.Add(new LanguageModel("Aquan"));
            langs.Add(new LanguageModel("Auran"));
            langs.Add(new LanguageModel("Beastspeech"));
            langs.Add(new LanguageModel("Druidic"));
            langs.Add(new LanguageModel("Celestial"));
            langs.Add(new LanguageModel("Deep Speech"));
            langs.Add(new LanguageModel("Draconic"));
            langs.Add(new LanguageModel("Druidic"));
            langs.Add(new LanguageModel("Ignan"));
            langs.Add(new LanguageModel("Infernal"));
            langs.Add(new LanguageModel("Primordial"));
            langs.Add(new LanguageModel("Sylvan"));
            langs.Add(new LanguageModel("Terran"));
            langs.Add(new LanguageModel("Undercommon"));
            return langs;
        }

        public ObservableCollection<LanguageModel> GenerateMonsterLanguages()
        {
            ObservableCollection<LanguageModel> langs = new ObservableCollection<LanguageModel>();

            langs.Add(new LanguageModel("Aarakocra"));
            langs.Add(new LanguageModel("Bullywug"));        
            langs.Add(new LanguageModel("Gith"));
            langs.Add(new LanguageModel("Gnoll"));
            langs.Add(new LanguageModel("Grell"));
            langs.Add(new LanguageModel("Grung"));
            langs.Add(new LanguageModel("Hook Horror"));
            langs.Add(new LanguageModel("Ice Toad"));
            langs.Add(new LanguageModel("Ixitxachitl"));
            langs.Add(new LanguageModel("Modron"));
            langs.Add(new LanguageModel("Otyugh"));
            langs.Add(new LanguageModel("Sahuagin"));
            langs.Add(new LanguageModel("Slaad"));
            langs.Add(new LanguageModel("Sphinx"));
            langs.Add(new LanguageModel("Thri-kreen"));
            langs.Add(new LanguageModel("Tlincali"));
            langs.Add(new LanguageModel("Troglodyte"));
            langs.Add(new LanguageModel("Umberhulk"));
            langs.Add(new LanguageModel("Vegepygmy"));
            langs.Add(new LanguageModel("Yeti"));

            return langs;
        }

    }
}
