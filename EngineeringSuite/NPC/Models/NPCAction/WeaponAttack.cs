﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction
{
    enum WeaponType
    {
        MWA,    //Melee Weapon Attack
        MRA,    //Melee Ranged Attack  
        MWRA,   // Melee Weapon or Range Attack
        MSA,    // Melee Spell Attack
        RSA,    // Range Spell Attack
        MRSA    // Melee or Range Spell Attack
    };

    public class WeaponAttack : ActionModelBase
    {
        public string WeaponName { get { return ActionName; } set { ActionName = value; } }
        WeaponType weaponType { get; set; }

        // Weapon Unique Properties
        bool isMagic;
        bool isSilver;
        bool isAdamantine;
        bool isColdForgedIron;
        bool isVersatile;

        // General Stats
        int toHit { get; set; }
        int reach { get; set; }
        int weaponRangeShort { get; set; }
        int weaponRangeLong { get; set; }
        String targetType { get; set; }

        //Damage Die Stats
        List<DamageProperty> damageProperties { get; set; }
        
        // TODO: Other Text
        String otherText;



        public String GenerateWeaponAttackDescription()
        {
            ActionDescription = "Weapon Attack Description Goes Here.";
            return ActionDescription;
        }


        #region Equals and HashCode
        public override bool Equals(object obj)
        {
            return obj is WeaponAttack attack &&
                   WeaponName == attack.WeaponName;
        }

        public override int GetHashCode()
        {
            return 39142378 + EqualityComparer<string>.Default.GetHashCode(WeaponName);
        }
        #endregion
    }
}
