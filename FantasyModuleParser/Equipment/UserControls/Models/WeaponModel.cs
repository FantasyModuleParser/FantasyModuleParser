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
        }
    }
}
