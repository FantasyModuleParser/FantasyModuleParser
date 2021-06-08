using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.UserControls.Models;
using FantasyModuleParser.NPC.ViewModel;
using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Models
{
    public class EquipmentModel : ViewModelBase
    {
        public PrimaryEquipmentEnum PrimaryEquipmentEnumType;

        #region Secondary Panel Options
        public ArmorEnum ArmorEnumType;

        #endregion Secondary Panel Options

        public ArmorModel Armor = new ArmorModel();

        public EquipmentModel()
        {
        }
    }
}
