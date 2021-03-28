using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using log4net;
using System.Collections.ObjectModel;
using System.Windows;

namespace FantasyModuleParser.Main.Models
{
    public class ModuleModel
    {
        // private static readonly ILog log = LogManager.GetLogger(typeof(ModuleModel));

        public NPCController NpcController { get; set; }
        public NPCModel NpcModel { get; set; }
        public CategoryModel Categorymodel { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string ModFilename { get; set; }
        public string ThumbnailPath { get; set; }
        public bool IsGMOnly { get; set; }
        public bool IsLockedRecords { get; set; }
        public ObservableCollection<CategoryModel> Categories { get; set; }
        public bool IncludeImages { get; set; }
        public bool IncludeTokens { get; set; }
        public bool IncludeNPCs { get; set; }
        public bool IncludeSpells { get; set; }

		//public string OkToCreateModule(object sender, RoutedEventArgs e)
		//{
		//	// TODO each of these if blocks, has 2 lines of the same text, create a method to handle it
		//	string warningMessageDoNotSave = "";

		//	foreach (CategoryModel category in Categories)
		//	{
		//		foreach (NPCModel npcModel in category.NPCModels)
		//		{
		//			if (string.IsNullOrEmpty(NpcModel.NPCType))
		//			{
		//				log.Warn("NPC Type is missing from " + NpcModel.NPCName);
		//				warningMessageDoNotSave += "NPC Type is missing from " + NpcModel.NPCName + "\n";
		//			}
		//			if (string.IsNullOrEmpty(NpcModel.Size))
		//			{
		//				log.Warn("Size is missing from " + NpcModel.NPCName);
		//				warningMessageDoNotSave += "Size is missing from " + NpcModel.NPCName + "\n";
		//			}
		//			if (string.IsNullOrEmpty(NpcModel.AC))
		//			{
		//				log.Warn("AC is missing from " + NpcModel.NPCName);
		//				warningMessageDoNotSave += "AC is missing from " + NpcModel.NPCName + "\n";
		//			}
		//			if (string.IsNullOrEmpty(NpcModel.Alignment))
		//			{
		//				log.Warn("Alignment is missing from " + NpcModel.NPCName);
		//				warningMessageDoNotSave += "Alignment is missing from " + NpcModel.NPCName + "\n";
		//			}
		//			if (string.IsNullOrEmpty(NpcModel.ChallengeRating))
		//			{
		//				log.Warn("Challenge Rating is missing from " + NpcModel.NPCName);
		//				warningMessageDoNotSave += "Challenge Rating is missing from " + NpcModel.NPCName + "\n";
		//			}
		//			if (string.IsNullOrEmpty(NpcModel.HP))
		//			{
		//				log.Warn("Hit Points are missing from " + NpcModel.NPCName);
		//				warningMessageDoNotSave += "Hit Points are missing from " + NpcModel.NPCName + "\n";
		//			}
		//			if (string.IsNullOrEmpty(NpcModel.LanguageOptions))
		//			{
		//				log.Warn("Language Option (usually No special conditions) is missing from " + NpcModel.NPCName);
		//				warningMessageDoNotSave += "Language Option (usually No special conditions) is missing from " + NpcModel.NPCName + "\n";
		//			}
		//			if (NpcModel.Telepathy == true && string.IsNullOrEmpty(NpcModel.TelepathyRange))
		//			{
		//				log.Warn("Telepathy Range is missing from " + NpcModel.NPCName);
		//				warningMessageDoNotSave += "Telepathy Range is missing from " + NpcModel.NPCName + "\n";
		//			}
		//			if (NpcModel.InnateSpellcastingSection == true && string.IsNullOrEmpty(NpcModel.InnateSpellcastingAbility))
		//			{
		//				log.Warn("Innate Spellcasting Ability is missing from " + NpcModel.NPCName);
		//				warningMessageDoNotSave += "Innate Spellcasting Ability is missing from " + NpcModel.NPCName + "\n";
		//			}
		//			if (NpcModel.SpellcastingSection == true)
		//			{
		//				if (string.IsNullOrEmpty(NpcModel.SpellcastingCasterLevel))
		//				{
		//					log.Warn("Spellcaster Level is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "What level spellcaster is " + NpcModel.NPCName + "\n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.SCSpellcastingAbility))
		//				{
		//					log.Warn("Spellcasting Ability is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Spellcasting Ability is missing from " + NpcModel.NPCName + "\n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.SpellcastingSpellClass))
		//				{
		//					log.Warn("Spellcasting Class is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "What class of spells does " + NpcModel.NPCName + " know \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.CantripSpells) && !string.IsNullOrEmpty(NpcModel.CantripSpellList))
		//				{
		//					log.Warn("Number of Cantrip slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many Cantrip spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.FirstLevelSpells) && !string.IsNullOrEmpty(NpcModel.FirstLevelSpellList))
		//				{
		//					log.Warn("Number of First Level Spell slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many First Level Spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.SecondLevelSpells) && !string.IsNullOrEmpty(NpcModel.SecondLevelSpellList))
		//				{
		//					log.Warn("Number of Second Level Spell slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many Second Level Spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.ThirdLevelSpells) && !string.IsNullOrEmpty(NpcModel.ThirdLevelSpellList))
		//				{
		//					log.Warn("Number of Third Level Spell slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many Third Level Spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.FourthLevelSpells) && !string.IsNullOrEmpty(NpcModel.FourthLevelSpellList))
		//				{
		//					log.Warn("Number of Fourth Level Spell slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many Fourth Level Spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.FifthLevelSpells) && !string.IsNullOrEmpty(NpcModel.FifthLevelSpellList))
		//				{
		//					log.Warn("Number of Fifth Level Spell slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many Fifth Level Spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.SixthLevelSpells) && !string.IsNullOrEmpty(NpcModel.SixthLevelSpellList))
		//				{
		//					log.Warn("Number of Sixth Level Spell slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many Sixth Level Spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.SeventhLevelSpells) && !string.IsNullOrEmpty(NpcModel.SeventhLevelSpellList))
		//				{
		//					log.Warn("Number of Seventh Level Spell slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many Seventh Level Spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.EighthLevelSpells) && !string.IsNullOrEmpty(NpcModel.EighthLevelSpellList))
		//				{
		//					log.Warn("Number of Eighth Level Spell slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many Eighth Level Spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//				if (string.IsNullOrEmpty(NpcModel.NinthLevelSpells) && !string.IsNullOrEmpty(NpcModel.NinthLevelSpellList))
		//				{
		//					log.Warn("Number of Ninth Level Spell slots is missing from " + NpcModel.NPCName);
		//					warningMessageDoNotSave += "Choose how many Ninth Level Spell slots " + NpcModel.NPCName + " has \n";
		//				}
		//			}
		//			if (npcModel.NPCImage.StartsWith("file:///"))
		//			{
		//				log.Warn("Invalid start of filename. Remove file:/// from " + npcModel.NPCName + "'s Image location and retry.");
		//				warningMessageDoNotSave += "Remove file:/// from " + npcModel.NPCName + "'s Image location and retry. \n";
		//			}
		//		}
		//		return warningMessageDoNotSave;
		//	}
		//}
	}
}
