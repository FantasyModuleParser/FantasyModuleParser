using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.Models.Skills
{
    public class LanguageModel
    {
        public bool Selected { get; set; }
        public string Language { get; set; }

        public LanguageModel()
        {
        }

        public LanguageModel(string language)
        {
            Language = language;
            Selected = false;
        }

        public LanguageModel(bool selected, string language)
        {
            Selected = selected;
            Language = language;
        }
    }
}
