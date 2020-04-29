using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction
{
    public class ActionDataModel : INotifyPropertyChanged
    {

        private Multiattack _multiAttack;
        private List<WeaponAttack> _weaponAttackList;
        private List<OtherAction> _otherActionList;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Intended to be invoked by the "Update" button on the Add / Edit Actions window (NPCActions.xaml)
        public void updateMultiAttack(Multiattack multiattack)
        {
            _multiAttack = multiattack;
            OnPropertyChanged("MultiAttack");

            //TODO:  Is this where the action list of items update on the left side get invoked?
        }

        public void updateWeaponAttack(WeaponAttack weaponAttack)
        {
            if(_weaponAttackList == null)
            {
                _weaponAttackList = new List<WeaponAttack>();
            }

            if (_weaponAttackList.Contains(weaponAttack))
            {
                //Basically, if a WeaponAttack object of the same WeaponName exists in the list,
                // remove it from the list & replace it with the updated Version

                // This should work because the Equals & HashCode for WeaponAttack is based
                // soley off of the weaponName attribute (matches functionality of current NPC Engineer app)
                _weaponAttackList.Remove(weaponAttack);
                _weaponAttackList.Add(weaponAttack);
            }

        }

        public void updateOtherAction(OtherAction otherAction)
        {
            if (_otherActionList == null)
            {
                _otherActionList = new List<OtherAction>();
            }

            if (_otherActionList.Contains(otherAction))
            {
                //Basically, if a WeaponAttack object of the same WeaponName exists in the list,
                // remove it from the list & replace it with the updated Version

                // This should work because the Equals & HashCode for WeaponAttack is based
                // soley off of the weaponName attribute (matches functionality of current NPC Engineer app)
                _otherActionList.Remove(otherAction);
                _otherActionList.Add(otherAction);
            }
        }

        #region Property Getters and Setters

        public Multiattack MultiAttack
        {
            get { return _multiAttack; }
            set
            {
                _multiAttack = value;
                OnPropertyChanged("MultiAttack");
            }
        }

        public List<WeaponAttack> WeaponAttacks
        {
            get { return _weaponAttackList;  }
            set
            {
                _weaponAttackList = value;
                OnPropertyChanged("WeaponAttack");
            }
        }

        public List<OtherAction> OtherActions
        {
            get { return _otherActionList; }
            set
            {
                _otherActionList = value;
                OnPropertyChanged("OtherAction");
            }
        }

        #endregion
    }
}
