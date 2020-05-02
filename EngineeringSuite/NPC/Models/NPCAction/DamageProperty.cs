using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction
{
    public class DamageProperty
    {
        int numOfDice;
        int dieType; // valid values:  4, 6, 8, 10, 12, 20
        int bonus;

        //TODO: Change this to an Enum, if possible!!
        String damageType;

        public int NumOfDice
        {
            get => numOfDice;
            set => numOfDice = value;
        }
        
        public int DieType
        {
            get => dieType;
            set => dieType = value;
        }

        public int Bonus
        {
            get => bonus;
            set => bonus = value;
        }

        public string DamageType
        {
            get => damageType;
            set => damageType = value;
        }

        public DamageProperty()
        {
        }

        public DamageProperty(int numOfDice, int dieType, int bonus, string damageType)
        {
            this.numOfDice = numOfDice;
            this.dieType = dieType;
            this.bonus = bonus;
            this.damageType = damageType;
        }

        #region Damage Combobox populations
        // This snippet will create a collection of items used by the DamageProperty
        // Namely, a collection of valid Die types (4,6,8,10,12,20) and 
        // Damage Types
        private List<int> _dieTypeList;
        public List<int> DieTypeList
        {
            get
            {
                if(_dieTypeList == null)
                {
                    _dieTypeList = new List<int>();
                    _dieTypeList.Add(4);
                    _dieTypeList.Add(6);
                    _dieTypeList.Add(8);
                    _dieTypeList.Add(10);
                    _dieTypeList.Add(12);
                    _dieTypeList.Add(20);
                }
                return _dieTypeList;
            }
        }
        private List<string> _damageTypeList;
        public List<string> DamageTypeList
        {
            get
            {
                if(_damageTypeList == null)
                {
                    _damageTypeList = new List<string>();
                    _damageTypeList.Add("bludgeoning");
                    _damageTypeList.Add("piercing");
                    _damageTypeList.Add("slashing");
                    _damageTypeList.Add("acid");
                    _damageTypeList.Add("cold");
                    _damageTypeList.Add("fire");
                    _damageTypeList.Add("force");
                    _damageTypeList.Add("lightning");
                    _damageTypeList.Add("necrotic");
                    _damageTypeList.Add("poison");
                    _damageTypeList.Add("psychic");
                    _damageTypeList.Add("radiant");
                    _damageTypeList.Add("thunder");
                }
                return _damageTypeList;
            }
        }
            
            
            #endregion

        public override bool Equals(object obj)
        {
            return obj is DamageProperty property &&
                   numOfDice == property.numOfDice &&
                   dieType == property.dieType &&
                   bonus == property.bonus &&
                   damageType == property.damageType;
        }

        public override int GetHashCode()
        {
            int hashCode = 1657412010;
            hashCode = hashCode * -1521134295 + numOfDice.GetHashCode();
            hashCode = hashCode * -1521134295 + dieType.GetHashCode();
            hashCode = hashCode * -1521134295 + bonus.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(damageType);
            return hashCode;
        }
    }
}
