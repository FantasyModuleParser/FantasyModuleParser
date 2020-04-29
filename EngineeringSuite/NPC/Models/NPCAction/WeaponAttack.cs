using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.DTO.NPCAction
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

    public class WeaponAttack
    {
        String weaponName { get; set; }
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



        #region Equals and HashCode
        public override bool Equals(object obj)
        {
            return obj is WeaponAttack attack &&
                   weaponName == attack.weaponName;
        }

        public override int GetHashCode()
        {
            return 39142378 + EqualityComparer<string>.Default.GetHashCode(weaponName);
        }
        #endregion
    }
}
