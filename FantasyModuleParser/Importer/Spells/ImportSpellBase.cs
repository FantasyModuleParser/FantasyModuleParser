using FantasyModuleParser.Extensions;
using FantasyModuleParser.Spells.Enums;
using FantasyModuleParser.Spells.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        break;
                    case ImportStatEnum.RANGE:
                        break;
                    case ImportStatEnum.COMPONENTS:
                        break;
                    case ImportStatEnum.DURATION:
                        break;
                    case ImportStatEnum.CAST_BY:
                        break;
                    case ImportStatEnum.DESCRIPTION:
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
    }
}
