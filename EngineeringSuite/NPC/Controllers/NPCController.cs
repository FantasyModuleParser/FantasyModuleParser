using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EngineeringSuite.NPC.Models.Action;
using Newtonsoft.Json;

namespace EngineeringSuite.NPC.Controllers
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

		public void Load(string path)
		{
			string jsonData = File.ReadAllText(path);
			JsonSerializer serializer = new JsonSerializer();
		}

		public NPCModel GetNPCModel()
		{
			var application = Application.Current;

			if (application is App app)
				return (NPCModel)app.NpcModel;

			return null;
		}
	}
}
