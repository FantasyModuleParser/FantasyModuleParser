using FantasyModuleParser.Extensions;
using FantasyModuleParser.Spells.Enums;
using FantasyModuleParser.Spells.Models;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Markup;

namespace FantasyModuleParser.Importer.Spells
{
    public class ImportSpellBase : IImportSpell
    {
        private enum ImportStatEnum
        {
            INITIAL, LEVEL_SCHOOL, CAST_BY, CASTING_TIME, COMPONENTS, RANGE, DURATION, DESCRIPTION
        }
        public SpellModel ImportTextToSpellModel(string importData)
        {
            SpellModel resultSpellModel = new SpellModel();       
            ImportStatEnum importStatEnum = ImportStatEnum.INITIAL;

            StringReader stringReader = new StringReader(importData);
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                //importStatEnum = getImportStateEnum(importStatEnum, importData);
                switch (importStatEnum)
                {
                    case ImportStatEnum.INITIAL:
                        resultSpellModel.SpellName = line;
                        importStatEnum = ImportStatEnum.LEVEL_SCHOOL;
                        break;
                    case ImportStatEnum.LEVEL_SCHOOL:
                        resultSpellModel.SpellLevel = ParseSpellLevel(importData);
                        resultSpellModel.SpellSchool = parseSpellSchool(importData);
                        resultSpellModel.IsRitual = checkIfRitual(importData);
                        importStatEnum = ImportStatEnum.CASTING_TIME;
                        break;
                    case ImportStatEnum.CASTING_TIME:
                        ParseCastingTime(importData, resultSpellModel);
                        importStatEnum = ImportStatEnum.RANGE;
                        break;
                    case ImportStatEnum.RANGE:
                        ParseRange(importData, resultSpellModel);
                        importStatEnum = ImportStatEnum.COMPONENTS;
                        break;
                    case ImportStatEnum.COMPONENTS:
                        ParseComponents(importData, resultSpellModel);
                        importStatEnum = ImportStatEnum.DURATION;
                        break;
                    case ImportStatEnum.DURATION:
                        ParseDuration(importData, resultSpellModel);
                        importStatEnum = ImportStatEnum.CAST_BY;
                        break;
                    case ImportStatEnum.CAST_BY:
                        ParseCastByClasses(importData, resultSpellModel);
                        importStatEnum = ImportStatEnum.DESCRIPTION;
                        break;
                    case ImportStatEnum.DESCRIPTION:
                        resultSpellModel.Description += importData;
                        break;
                    default:
                        break;
                }
            }

            return resultSpellModel;
        }

        public SpellLevel ParseSpellLevel(string importData)
        {
            if(importData.ToLower(CultureInfo.CurrentCulture).Contains("cantrip"))
                return SpellLevel.Cantrip;

            foreach(SpellLevel spellLevelEnum in Enum.GetValues(typeof(SpellLevel)))
            {
                if (importData.Contains(EnumDescription.GetDescription(spellLevelEnum)))
                    return spellLevelEnum;
            }

            return SpellLevel.Cantrip;

        }
        public SpellSchool parseSpellSchool(string importData)
        {
            string importDataLower = importData.ToLower(CultureInfo.CurrentCulture);
            foreach (SpellSchool spellSchoolEnum in Enum.GetValues(typeof(SpellSchool)))
            {
                if (importDataLower.Contains(EnumDescription.GetDescription(spellSchoolEnum).ToLower(CultureInfo.CurrentCulture)))
                    return spellSchoolEnum;
            }
            return SpellSchool.Abjuration;
        }
        public bool checkIfRitual(string importData)
        {
            if (!String.IsNullOrWhiteSpace(importData))
                return importData.ToLower(CultureInfo.CurrentCulture).Contains("(ritual)");
            return false;
        }
        public void ParseCastingTime(string importData, SpellModel spellModel)
        {
            string lowerImportData = importData.ToLower(CultureInfo.CurrentCulture);

            // Remove 'Casting Time: '
            lowerImportData = lowerImportData.Substring(14);
            if (lowerImportData.Contains("reaction"))
            {
                spellModel.CastingTime = 1;
                spellModel.CastingType = CastingType.Reaction;
                string[] reactionDescriptionArray = lowerImportData.Split(',');
                string reactionDescription = "";
                for (int idx = 1; idx < reactionDescriptionArray.Length; idx++)
                    reactionDescription += reactionDescriptionArray[idx].Trim() + ",";

                //Remove the last comma
                reactionDescription = reactionDescription.Substring(0, reactionDescription.Length - 1);
                spellModel.ReactionDescription = reactionDescription;
            }
            else
            {
                string[] castingTimeArray = lowerImportData.Split(' ');
                int actionValue = int.Parse(castingTimeArray[0]);
                string actionType = "";
                for (int idx = 1; idx < castingTimeArray.Length; idx++)
                    actionType += castingTimeArray[idx] + " ";

                actionType = actionType.TrimEnd();
                
                // Remove the 's' character (e.g. hours to hour, minutes to minute)
                if (actionType.EndsWith("s"))
                    actionType = actionType.Substring(0, actionType.Length - 1);

                foreach(CastingType castingType in Enum.GetValues(typeof(CastingType)))
                {
                    if (actionType.Equals(EnumDescription.GetDescription(castingType)))
                    {
                        spellModel.CastingType = castingType;
                        break;
                    }
                }
                spellModel.CastingTime = actionValue;
            }
        }
        public void ParseRange(string importData, SpellModel spellModel)
        {
            string lowerImportData = importData.ToLower(CultureInfo.CurrentCulture);

            // Remove 'range: ' from the description
            lowerImportData = lowerImportData.Substring(7).Trim();

            if (lowerImportData.Equals("self"))
                spellModel.RangeType = RangeType.Self;
            if (lowerImportData.Equals("unlimited"))
                spellModel.RangeType = RangeType.Unlimited;
            if (lowerImportData.Equals("touch"))
                spellModel.RangeType = RangeType.Touch;
            if (lowerImportData.Equals("sight"))
                spellModel.RangeType = RangeType.Sight;
            if (lowerImportData.Contains("feet") || lowerImportData.Contains("ft"))
            {
                // Typically it is 60 feet, 60 ft., etc...  Will still use regex incase 60feet is entered.
                Regex regex = new Regex("(?<Alphabet>[a-zA-Z]*)(?<Numeric>[0-9]*)");
                string numMatch = regex.Match(lowerImportData).Groups["Numeric"].Value;
                if (!String.IsNullOrEmpty(numMatch))
                    spellModel.Range = int.Parse(numMatch);
                spellModel.RangeType = RangeType.Ranged;
            }


        }
        public void ParseComponents(string importData, SpellModel spellModel)
        {
            string[] componentArray = importData.Split('(');

            // If Material component is unselected, then componentArray will be length of 1.  Otherwise, length of 2
            spellModel.IsVerbalComponent = componentArray[0].ToUpper(CultureInfo.CurrentCulture).Contains("V");
            spellModel.IsSomaticComponent = componentArray[0].ToUpper(CultureInfo.CurrentCulture).Contains("S");
            spellModel.IsMaterialComponent= componentArray[0].ToUpper(CultureInfo.CurrentCulture).Contains("M");

            if(componentArray.Length == 2 && spellModel.IsMaterialComponent)
            {
                // Removes the leading '(' and ending ')' characters
                spellModel.ComponentText = componentArray[1].Substring(0, componentArray[1].Length - 1);
            }
        }
        public void ParseDuration(string importData, SpellModel spellModel)
        {
            // Do nothing right now...
        }
        public void ParseCastByClasses(string importData, SpellModel spellModel)
        {
            spellModel.CastBy = importData.Substring(9);
        }

        public void ParseDescription(string importData, SpellModel spellModel)
        {
            // Do nothing right now....
        }
    }
}
