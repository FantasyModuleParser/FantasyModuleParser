using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class ResistanceUserControlViewModel : ViewModelBase
    {
        public NPCModel NPCModel { get; set; }
        private NPCController npcController;
        public ResistanceUserControlViewModel()
        {
            npcController = new NPCController();
            NPCModel = npcController.GetNPCModel();

            initializeDamageAndConditionSets(NPCModel);
            RaisePropertyChanged("NPCModel");
        }

		public void Refresh()
		{
			NPCModel = npcController.GetNPCModel();
			initializeDamageAndConditionSets(NPCModel);
			RaisePropertyChanged("NPCModel");
		}

		private void initializeDamageAndConditionSets(NPCModel npcModel)
		{
			npcModel.DamageImmunityModelList = initSpecificDamageSet(npcController.GetSelectableActionModelList(typeof(DamageType)), npcModel.DamageImmunityModelList);
			npcModel.DamageResistanceModelList = initSpecificDamageSet(npcController.GetSelectableActionModelList(typeof(DamageType)), npcModel.DamageResistanceModelList);
			npcModel.DamageVulnerabilityModelList = initSpecificDamageSet(npcController.GetSelectableActionModelList(typeof(DamageType)), npcModel.DamageVulnerabilityModelList);
			npcModel.ConditionImmunityModelList = initSpecificDamageSet(npcController.GetSelectableActionModelList(typeof(ConditionType)), npcModel.ConditionImmunityModelList);

			npcModel.SpecialWeaponImmunityModelList =
				initSpecificDamageSet(npcController.GetSelectableActionModelList(typeof(WeaponImmunity)), npcModel.SpecialWeaponImmunityModelList);
			npcModel.SpecialWeaponResistanceModelList =
				initSpecificDamageSet(npcController.GetSelectableActionModelList(typeof(WeaponResistance)), npcModel.SpecialWeaponResistanceModelList);
		}

		// Merges an existing set of NPC language models w/ the default set (whether hardcoded or defined by users)
		private List<SelectableActionModel> initSpecificDamageSet(List<SelectableActionModel> defaultList, List<SelectableActionModel> npcModelList)
		{
			foreach (SelectableActionModel selectableActionModel in defaultList)
				selectableActionModel.Selected = false;

			if (npcModelList == null || npcModelList.Count == 0)
			{
				return defaultList;
			}
			else
			{
				foreach (SelectableActionModel selectableAction in npcModelList)
				{
					if (!selectableAction.Selected) continue;
					SelectableActionModel selectableActionModel = defaultList.FirstOrDefault(item =>
						item.ActionDescription.ToLower().Equals(selectableAction.ActionDescription.ToLower()));
					if (selectableActionModel != null)
						selectableActionModel.Selected = true;
				}
			}
			return defaultList;
		}
	}
}