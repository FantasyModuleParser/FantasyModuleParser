using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EngineeringSuite.NPC.Models.NPCAction;
using Newtonsoft.Json;

namespace EngineeringSuite.NPC.Controller
{
	public class NPCController
	{
		public void Save(string path, NPCModel npcModel)
		{
			using (StreamWriter file = File.CreateText(@path))
			{ 
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;
				serializer.Serialize(file, npcModel);
			}
		}

		public NPCModel LoadTemporaryGoblin()
		{
			NPCModel tempGoblin = new NPCModel();

			tempGoblin.NPCName = "Goblin";

			tempGoblin.AC = "11";
			tempGoblin.AttributeStr = 8;
			tempGoblin.AttributeDex = 10;

			WeaponAttack weaponAttack = new WeaponAttack();
			weaponAttack.WeaponName = "Dagger";
			weaponAttack.PrimaryDmgDieCount = 1;
			weaponAttack.PrimaryDieType = 6;
			weaponAttack.PrimaryBonus = 2;
			weaponAttack.PrimaryDamageType = weaponAttack.DamageTypeList[0];
			tempGoblin.NPCActions.Add(new Models.NPCAction.Multiattack("This is a MultiAttack test"));
			tempGoblin.NPCActions.Add(weaponAttack);
			return tempGoblin;
		}

		public NPCModel GetNPCModel()
		{
			var _application = Application.Current;
			if(_application is App){
				App _app = (App)_application;
				return (NPCModel)_app.NpcModelObject;
			}
			return null;
		}
	}
}
