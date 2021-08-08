﻿using FantasyModuleParser.Equipment.Enums.Weapon;
using FantasyModuleParser.NPC.Models.Action;
using System.Collections.Generic;

namespace FantasyModuleParser.Equipment.UserControls.Models
{
    public class WeaponModel
    {
        public DamageProperty PrimaryDamage { get; set; }
        public DamageProperty BonusDamage { get; set; }

        public HashSet<WeaponPropertyEnum> WeaponProperties;

        public HashSet<WeaponMaterialEnum> MaterialProperties;

        public int ShortRange { get; set; }
        public int LongRange { get; set; }
        public bool SecondaryDamage { get; set; }

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

            WeaponProperties = new HashSet<WeaponPropertyEnum>();
            MaterialProperties = new HashSet<WeaponMaterialEnum>();
        }
    }
}
