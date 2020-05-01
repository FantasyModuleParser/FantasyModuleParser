using EngineeringSuite.NPC.Models.NPCAction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EngineeringSuite.NPC.ViewModel
{
    public class ActionViewModel : ViewModelBase
    {
        public ObservableCollection<ActionModelBase> NPCActions { get; } = new ObservableCollection<ActionModelBase>();

        private Multiattack _multiattack;
        Multiattack Multiattack { get => _multiattack; set => SetProperty(ref _multiattack, value); }

        private WeaponAttack _weaponAttack;
        WeaponAttack WeaponAttack { get => _weaponAttack; set => SetProperty(ref _weaponAttack, value); }

        private OtherAction _otherAction;
        OtherAction OtherAction { get => _otherAction; set => SetProperty(ref _otherAction, value); }

        public ActionViewModel()
        {
            Initialize();
        }

        public ActionViewModel(ObservableCollection<ActionModelBase> nPCActions)
        {
            Initialize();
            NPCActions = nPCActions;
        }

        private void Initialize()
        {
            NPCActions.CollectionChanged += NPCActions_CollectionChanged;
        }

        // Intended to be invoked by the "Update" button on the Add / Edit Actions window (NPCActions.xaml)
        public void updateMultiAttack(Multiattack multiattack)
        {
            // Quick and dirty method to update the collection
            removeNPCAction(new Multiattack("Nothing goes here."));
            updateNPCActions(multiattack);
            //TODO: Force a sorting of the collection to put MultiAttack at the top!
        }

        public void removeMultiAttack()
        {
            removeNPCAction(new Multiattack("Nothing goes here."));
        }

        public void updateWeaponAttack(WeaponAttack weaponAttack)
        {
            updateNPCActions(weaponAttack);
            //TODO: Force a sorting of the collection to put WeaponAttacks in the middle
        }

        public void updateOtherAction(OtherAction otherAction)
        {
            updateNPCActions(otherAction);
            //TODO: Force a sorting of the collection to put Other Actions at the bottom!
        }

        private void updateNPCActions(ActionModelBase action)
        {
            if (NPCActions.Contains(action))
            {
                var obj = NPCActions.FirstOrDefault(x => x == action);
                if (obj != null) obj = action;
            }
            else
            {
                NPCActions.Add(action);
            }
        }

        private void removeNPCAction(ActionModelBase action)
        {
            NPCActions.Remove(action);
        }

        private void NPCActions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("NPC Actions Collection has changed!");
            if (e.NewItems != null)
                foreach (ActionModelBase item in e.NewItems)
                    item.PropertyChanged += ItemPropertyChanged;

            if (e.OldItems != null)
                foreach (ActionModelBase item in e.OldItems)
                    item.PropertyChanged -= ItemPropertyChanged;
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("NpcActionList");
        }

    }
}
