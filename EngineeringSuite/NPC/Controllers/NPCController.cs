using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				serializer.Serialize(file, npcModel);
			}
		}

		public NPCModel LoadTemporaryGoblin()
		{
			NPCModel tempGoblin = new NPCModel();

			tempGoblin.AC = "11";
			tempGoblin.AttributeStr = 8;
			tempGoblin.AttributeDex = 10;

			tempGoblin.npcActions = new Models.NPCAction.ActionDataModel();
			tempGoblin.npcActions.MultiAttack = new Models.NPCAction.Multiattack("This is a multiattack test");

			return tempGoblin;
		}
	}
}
