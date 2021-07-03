using FantasyModuleParser.Equipment.Enums.Weapon;
using FantasyModuleParser.NPC.Models.Action;
using System.Collections.Generic;

namespace FantasyModuleParser.Equipment.UserControls.Models
{
    public class WeaponModel
    {
        public DamageProperty PrimaryDamage { get; set; }
        public DamageProperty BonusDamage { get; set; }

        public List<WeaponPropertyEnum> WeaponProperties;

        public List<WeaponMaterialEnum> MaterialProperties;

        public int ShortRange;
        public int LongRange;

        public WeaponModel()
        {
            PrimaryDamage = new DamageProperty();
            PrimaryDamage.NumOfDice = 1;
            PrimaryDamage.DieType = NPC.Models.Action.Enums.DieType.D6;
            PrimaryDamage.Bonus = 0;

            BonusDamage = new DamageProperty();
            BonusDamage.NumOfDice = 0;
            BonusDamage.DieType = NPC.Models.Action.Enums.DieType.D6;
            BonusDamage.Bonus = 0;

            WeaponProperties = new List<WeaponPropertyEnum>();
            MaterialProperties = new List<WeaponMaterialEnum>();
        }
    }
}
