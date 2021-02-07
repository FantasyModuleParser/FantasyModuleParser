using FantasyModuleParser.Extensions;
using FantasyModuleParser.Importer.Enums;
using FantasyModuleParser.Spells.Enums;
using FantasyModuleParser.Spells.Models;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace FantasyModuleParser.Importer.Spells
{
    public class ImportSpellBase : IImportSpell
    {

        public SpellModel ImportTextToSpellModel(string importData)
        {
            SpellModel resultSpellModel = new SpellModel();

            string formattedImportData = FormatSpellPDFService.FormatData(importData);

            ImportSpellState importStatEnum = ImportSpellState.INITIAL;

            StringReader stringReader = new StringReader(formattedImportData);
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                //importStatEnum = getImportStateEnum(importStatEnum, importData);
                switch (importStatEnum)
                {
                    case ImportSpellState.INITIAL:
                        resultSpellModel.SpellName = line;
                        importStatEnum = ImportSpellState.LEVEL_SCHOOL;
                        break;
                    case ImportSpellState.LEVEL_SCHOOL:
                        resultSpellModel.SpellLevel = ParseSpellLevel(line);
                        resultSpellModel.SpellSchool = ParseSpellSchool(line);
                        resultSpellModel.IsRitual = CheckIfRitual(line);
                        importStatEnum = ImportSpellState.CASTING_TIME;
                        break;
                    case ImportSpellState.CASTING_TIME:
                        ParseCastingTime(line, resultSpellModel);
                        importStatEnum = ImportSpellState.RANGE;
                        break;
                    case ImportSpellState.RANGE:
                        ParseRange(line, resultSpellModel);
                        importStatEnum = ImportSpellState.COMPONENTS;
                        break;
                    case ImportSpellState.COMPONENTS:
                        ParseComponents(line, resultSpellModel);
                        importStatEnum = ImportSpellState.DURATION;
                        break;
                    case ImportSpellState.DURATION:
                        ParseDuration(line, resultSpellModel);
                        importStatEnum = ImportSpellState.CAST_BY;
                        break;
                    case ImportSpellState.CAST_BY:
                        if (line.StartsWith("Classes:"))
						{
                            ParseCastByClasses(line, resultSpellModel);
                        }                            
                        else
						{
                            resultSpellModel.Description = line + " "; // No character class to associate with
                        }                            
                        importStatEnum = ImportSpellState.DESCRIPTION;
                        break;
                    case ImportSpellState.DESCRIPTION:
                        if(line.EndsWith(".", StringComparison.Ordinal)) 
                        {
                            resultSpellModel.Description += line;
                            resultSpellModel.Description += Environment.NewLine;    
                        } 
                        else if (line.StartsWith("At Higher Levels.", StringComparison.Ordinal))
                        {
                            resultSpellModel.Description += "**At Higher Levels.**";
                            resultSpellModel.Description += line.Substring(17);
                        }
                        else
						{
                            resultSpellModel.Description += line;
                        }
                        break;
                    default:
                        break;
                }
            }

            // Trim off the Description, as it will have a leading newLine character
            resultSpellModel.Description = resultSpellModel.Description.TrimEnd();

            return resultSpellModel;
        }

        public SpellLevel ParseSpellLevel(string importData)
        {
            if(importData.ToLower(CultureInfo.CurrentCulture).Contains("cantrip"))
			{
                return SpellLevel.Cantrip;
            }                

            foreach(SpellLevel spellLevelEnum in Enum.GetValues(typeof(SpellLevel)))
            {
                if (importData.Contains(EnumDescription.GetDescription(spellLevelEnum)))
				{
                    return spellLevelEnum;
                }                    
            }

            return SpellLevel.Cantrip;

        }
        #pragma warning disable CA1822 // Cannot mark static due to ImportSpellLineParserTest
        public SpellSchool ParseSpellSchool(string importData)
        #pragma warning restore CA1822 // Cannot mark static due to ImportSpellLineParserTest
        {
            string importDataLower = importData.ToLower(CultureInfo.CurrentCulture);
            foreach (SpellSchool spellSchoolEnum in Enum.GetValues(typeof(SpellSchool)))
            {
                if (importDataLower.Contains(EnumDescription.GetDescription(spellSchoolEnum).ToLower(CultureInfo.CurrentCulture)))
				{
                    return spellSchoolEnum;
                }                    
            }
            return SpellSchool.Abjuration;
        }
        #pragma warning disable CA1822 // Cannot mark static due to ImportSpellLineParserTest
        public bool CheckIfRitual(string importData)
        #pragma warning restore CA1822 // Cannot mark static due to ImportSpellLineParserTest
        {
            if (!String.IsNullOrWhiteSpace(importData))
			{
                return importData.ToLower(CultureInfo.CurrentCulture).Contains("(ritual)");
            }                
            return false;
        }
        #pragma warning disable CA1822 // Cannot mark static due to ImportSpellLineParserTest
        public void ParseCastingTime(string importData, SpellModel spellModel)
        #pragma warning restore CA1822 // Cannot mark static due to ImportSpellLineParserTest
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
				{
                    reactionDescription += reactionDescriptionArray[idx].Trim() + ",";
                }                    

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
				{
                    actionType += castingTimeArray[idx] + " ";
                }                    

                actionType = actionType.TrimEnd();
                
                // Remove the 's' character (e.g. hours to hour, minutes to minute)
                if (actionType.EndsWith("s"))
				{
                    actionType = actionType.Substring(0, actionType.Length - 1);
                }                    

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
        #pragma warning disable CA1822 // Cannot mark static due to ImportSpellLineParserTest
        public void ParseRange(string importData, SpellModel spellModel)
        #pragma warning restore CA1822 // Cannot mark static due to ImportSpellLineParserTest
        {
            string lowerImportData = importData.ToLower(CultureInfo.CurrentCulture);

            // Remove 'range: ' from the description
            lowerImportData = lowerImportData.Substring(7).Trim();

            if (lowerImportData.Contains("self"))
            {
                spellModel.RangeType = RangeType.Self;
                if (lowerImportData.Contains("foot") || lowerImportData.Contains("ft"))
                {
                    string[] selfFields = lowerImportData.Split(' ');
                    string[] rangeFields = selfFields[1].Split('-');
                    spellModel.Range = int.Parse(rangeFields[0].Substring(1));
                    spellModel.Unit = UnitType.Foot;
                    if (selfFields[2].ToString() == "sphere)")
					{
                        spellModel.SelfType = SelfType.Sphere;
                    }                        
                    else if (selfFields[2].ToString() == "radius)")
					{
                        spellModel.SelfType = SelfType.Radius;
                    }                        
                    else if (selfFields[2].ToString() == "line)")
					{
                        spellModel.SelfType = SelfType.Line;
                    }                        
                    else if (selfFields[2].ToString() == "cone)")
					{
                        spellModel.SelfType = SelfType.Cone;
                    }                        
                    else if (selfFields[2].ToString() == "cube)")
					{
                        spellModel.SelfType = SelfType.Cube;
                    }                        
                }
            }
            else
            {
                if (lowerImportData.Equals("unlimited"))
				{
                    spellModel.RangeType = RangeType.Unlimited;
                }                    
                if (lowerImportData.Equals("touch"))
				{
                    spellModel.RangeType = RangeType.Touch;
                }                    
                if (lowerImportData.Equals("sight"))
				{
                    spellModel.RangeType = RangeType.Sight;
                }                    
                if (lowerImportData.Contains("feet") || lowerImportData.Contains("ft"))
                {
                    // Typically it is 60 feet, 60 ft., etc...  Will still use regex incase 60feet is entered.
                    Regex regex = new Regex("(?<Alphabet>[a-zA-Z]*)(?<Numeric>[0-9]*)");
                    string numMatch = regex.Match(lowerImportData).Groups["Numeric"].Value;
                    if (!String.IsNullOrEmpty(numMatch))
					{
                        spellModel.Range = int.Parse(numMatch);
                    }                        
                    spellModel.RangeType = RangeType.Ranged;
                }
            }
        }
        #pragma warning disable CA1822 // Cannot mark static due to ImportSpellLineParserTest
        public void ParseComponents(string importData, SpellModel spellModel)
        #pragma warning restore CA1822 // Cannot mark static due to ImportSpellLineParserTest
        {
            if (importData.ToUpper().StartsWith("COMPONENTS:"))
			{
                importData = importData.Substring("Components:".Length);
            }                
            string[] componentArray = importData.Split('(');

            // If Material component is unselected, then componentArray will be length of 1.  Otherwise, length of 2
            spellModel.IsVerbalComponent = componentArray[0].ToUpper(CultureInfo.CurrentCulture).Contains("V");
            spellModel.IsSomaticComponent = componentArray[0].ToUpper(CultureInfo.CurrentCulture).Contains("S");
            spellModel.IsMaterialComponent= componentArray[0].ToUpper(CultureInfo.CurrentCulture).Contains("M");

            if(componentArray.Length == 2 && spellModel.IsMaterialComponent)
            {
                string rawComponentText = componentArray[1].Trim();
                // Removes the leading '(' and ending ')' characters
                spellModel.ComponentText = rawComponentText.Substring(0, rawComponentText.Length - 1);
            }
        }
        #pragma warning disable CA1822 // Cannot mark static due to ImportSpellLineParserTest
        public void ParseDuration(string importData, SpellModel spellModel)
        #pragma warning restore CA1822 // Cannot mark static due to ImportSpellLineParserTest
        {
            string formattedData = importData.Substring("Duration: ".Length);
            if(formattedData.StartsWith("Up to ", StringComparison.OrdinalIgnoreCase))
            {
                spellModel.DurationType = DurationType.Concentration;
                spellModel.DurationTime = int.Parse(formattedData.Split(' ')[2]);
                spellModel.DurationUnit = ParseDurationUnit(formattedData.Split(' ')[3]);
            } 
            else if (formattedData.StartsWith("Concentration, up to ", StringComparison.OrdinalIgnoreCase))
            {
                spellModel.DurationType = DurationType.Concentration;
                spellModel.DurationTime = int.Parse(formattedData.Split(' ')[3]);
                spellModel.DurationUnit = ParseDurationUnit(formattedData.Split(' ')[4]);
            }
            else if (formattedData.Equals("Instantaneous"))
            {
                spellModel.DurationType = DurationType.Instantaneous;
                spellModel.DurationTime = 0;
                spellModel.DurationUnit = DurationUnit.None;
            }
            else if (formattedData.Equals("Until dispelled or triggered"))
            {
                spellModel.DurationType = DurationType.UntilDispelledOrTriggered;
                spellModel.DurationTime = 0;
                spellModel.DurationUnit = DurationUnit.None;
            }
            else if (formattedData.Equals("Until dispelled"))
            {
                spellModel.DurationType = DurationType.UntilDispelled;
                spellModel.DurationTime = 0;
                spellModel.DurationUnit = DurationUnit.None;
            }
            else
            {
                spellModel.DurationType = DurationType.Time;
                spellModel.DurationTime = int.Parse(formattedData.Split(' ')[0]);
                spellModel.DurationUnit = ParseDurationUnit(formattedData.Split(' ')[1]);
            }
        }
        static private DurationUnit ParseDurationUnit(string data)
        {
            string lowerCaseData = data.ToLower();
            if (lowerCaseData.StartsWith("round", StringComparison.Ordinal))
			{
                return DurationUnit.Round;
            }                
            if (lowerCaseData.StartsWith("minute", StringComparison.Ordinal))
			{
                return DurationUnit.Minute;
            }                
            if (lowerCaseData.StartsWith("hour", StringComparison.Ordinal))
			{
                return DurationUnit.Hour;
            }                
            if (lowerCaseData.StartsWith("day", StringComparison.Ordinal))
			{
                return DurationUnit.Day;
            }                
            return DurationUnit.None;
        }
        #pragma warning disable CA1822 // Cannot mark static due to ImportSpellLineParserTest 
        public void ParseCastByClasses(string importData, SpellModel spellModel)
        #pragma warning restore CA1822 // Cannot mark static due to ImportSpellLineParserTest
        {
            spellModel.CastBy = importData.Substring(9);
        }
        #pragma warning disable CA1822 // Cannot mark static due to ImportSpellLineParserTest
        public string ParseDescription(string importData)
        #pragma warning restore CA1822 // Cannot mark static due to ImportSpellLineParserTest
        {
            const string AT_HIGHER_LEVELS = "At Higher Levels";
            if (importData.StartsWith(AT_HIGHER_LEVELS, StringComparison.Ordinal))
			{
                return "**" + AT_HIGHER_LEVELS + "**" + importData.Substring(AT_HIGHER_LEVELS.Length);
            }                
            if (importData.EndsWith(".", StringComparison.Ordinal))
			{
                return importData + "\r\n";
            }
            return importData;
        }
    }
}
