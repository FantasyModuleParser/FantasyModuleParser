using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction
{

    public class WeaponAttack : ActionModelBase
    {
        public WeaponAttack()
        {
            _primaryDamage = new DamageProperty();
            _secondaryDamage = new DamageProperty();
        }

        public string WeaponName { get { return ActionName; } set { ActionName = value; } }

        private List<string> _weaponTypeList;
        public List<string> WeaponTypeList
        {
            get
            {
                if (_weaponTypeList == null)
                {
                    _weaponTypeList = new List<string>();
                    _weaponTypeList.Add("Melee Weapon Attack");
                    _weaponTypeList.Add("Ranged Weapon Attack");
                    _weaponTypeList.Add("Melee or Ranged Weapon Attack");
                    _weaponTypeList.Add("Melee Spell Attack");
                    _weaponTypeList.Add("Ranged Spell Attack");
                    _weaponTypeList.Add("Melee or Ranged Spell Attack");
                }
                return _weaponTypeList;
            }
        }

        private string _weaponType;
        public string WeaponType
        {
            get => _weaponType;
            set
            {
                _weaponType = value;
                GenerateWeaponAttackDescription();
            }
        }

        // Weapon Unique Properties
        private bool _isMagic;
        public bool IsMagic
        {
            get => _isMagic;
            set
            {
                SetProperty(ref _isMagic, value);
                GenerateWeaponAttackDescription();
            }
        }
        private bool _isSilver;
        public bool IsSilver
        {
            get => _isSilver;
            set
            {
                SetProperty(ref _isSilver, value);
                GenerateWeaponAttackDescription();
            }
        }
        private bool _isAdamantine;
        public bool IsAdamantine
        {
            get => _isAdamantine;
            set
            {
                SetProperty(ref _isAdamantine, value);
                GenerateWeaponAttackDescription();
            }
        }
        private bool _isColdForgedIron;
        public bool IsColdForgedIron
        {
            get => _isColdForgedIron;
            set
            {
                SetProperty(ref _isColdForgedIron, value);
                GenerateWeaponAttackDescription();
            }
        }
        private bool _isVersatile;
        public bool IsVersatile
        {
            get => _isVersatile;
            set
            {
                SetProperty(ref _isVersatile, value);
                GenerateWeaponAttackDescription();
            }
        }

        // General Stats
        private int _toHit;
        public int ToHit
        {
            get => _toHit;
            set
            {
                SetProperty(ref _toHit, value);
                GenerateWeaponAttackDescription();
            }
        }
        private int _reach;
        public int Reach
        {
            get => _reach;
            set
            {
                SetProperty(ref _reach, value);
                GenerateWeaponAttackDescription();
            }
        }
        private int _weaponRangeShort;
        public int WeaponRangeShort
        {
            get => _weaponRangeShort;
            set
            {
                SetProperty(ref _weaponRangeShort, value);
                GenerateWeaponAttackDescription();
            }
        }
        private int _weaponRangeLong;
        public int WeaponRangeLong
        {
            get => _weaponRangeLong;
            set
            {
                SetProperty(ref _weaponRangeLong, value);
                GenerateWeaponAttackDescription();
            }
        }
        private string _targetType;
        public String TargetType
        {
            get => _targetType;
            set
            {
                SetProperty(ref _targetType, value);
                GenerateWeaponAttackDescription();
            }
        }

        //Damage Die Stats
        public List<DamageProperty> damageProperties { get; set; }

        #region Damage Property getter setters
        // This section exists because I thought it might be easier to maintain the damage
        // stats as their own separate class entity.  However, since most (if not all) action damage
        // has two types, no need to create an expanded damage section... for now!

        private DamageProperty _primaryDamage;
        private DamageProperty _secondaryDamage;

        public List<int> DieTypeList
        {
            get => _primaryDamage.DieTypeList;
        }

        public List<string> DamageTypeList
        {
            get => _primaryDamage.DamageTypeList;
        }

        #region Primary Damage gets sets
        public int PrimaryDmgDieCount
        {
            get => _primaryDamage.NumOfDice;
            set
            {
                _primaryDamage.NumOfDice = value;
                GenerateWeaponAttackDescription();
            }
        }

        public int PrimaryDieType
        {
            get => _primaryDamage.DieType;
            set
            {
                _primaryDamage.DieType = value;
                GenerateWeaponAttackDescription();
            }
        }

        public int PrimaryBonus
        {
            get => _primaryDamage.Bonus;
            set
            {
                _primaryDamage.Bonus = value;
                GenerateWeaponAttackDescription();
            }
        }

        public string PrimaryDamageType
        {
            get => _primaryDamage.DamageType;
            set
            {
                _primaryDamage.DamageType = value;
                GenerateWeaponAttackDescription();
            }
        }
        #endregion

        #region Secondary Damage gets sets
        public int SecondaryDmgDieCount
        {
            get => _secondaryDamage.NumOfDice;
            set
            {
                _secondaryDamage.NumOfDice = value;
                GenerateWeaponAttackDescription();
            }
        }

        public int SecondaryDieType
        {
            get => _secondaryDamage.DieType;
            set
            {
                _secondaryDamage.DieType = value;
                GenerateWeaponAttackDescription();
            }
        }

        public int SecondaryBonus
        {
            get => _secondaryDamage.Bonus;
            set
            {
                _secondaryDamage.Bonus = value;
                GenerateWeaponAttackDescription();
            }
        }

        public string SecondaryDamageType
        {
            get => _secondaryDamage.DamageType;
            set
            {
                _secondaryDamage.DamageType = value;
                GenerateWeaponAttackDescription();
            }
        }
        #endregion

        #endregion

        // TODO: Other Text
        private string _otherText;

        public String OtherText
        {
            get => _otherText;
            set
            {
                SetProperty(ref _otherText, value);
                GenerateWeaponAttackDescription();
            }
        }

        public void GenerateWeaponAttackDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ActionName);
            sb.Append(": ");
            if (_isMagic)
            {
                sb.Append("Magic,");
            }
            ActionDescription = sb.ToString();
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
