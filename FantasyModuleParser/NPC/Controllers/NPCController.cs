using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using Newtonsoft.Json;

namespace FantasyModuleParser.NPC.Controllers
{
	public class NPCController : ControllerBase
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

		public event EventHandler LoadNpcModelAction;

		protected virtual void OnLoadNpcModelEvent(EventArgs e)
		{
			EventHandler handler = LoadNpcModelAction;
			if(handler != null)
			{
				Console.WriteLine("Load NPC Action executed");
				handler(this, e);
			}
		}
		public void Load(string path)
		{
			string jsonData = File.ReadAllText(@path);
			NPCModel npcModel = JsonConvert.DeserializeObject<NPCModel>(jsonData);

			var application = Application.Current;

			if (application is App app)
				app.NpcModel = npcModel;

			OnLoadNpcModelEvent(EventArgs.Empty);
		}

		public NPCModel InitializeNPCModel()
		{
			NPCModel npcModel = new NPCModel();

			npcModel.DamageResistanceModelList = GetSelectableActionModelList(typeof(DamageType));
			npcModel.DamageVulnerabilityModelList = GetSelectableActionModelList(typeof(DamageType));
			npcModel.DamageImmunityModelList = GetSelectableActionModelList(typeof(DamageType));
			npcModel.ConditionImmunityModelList = GetSelectableActionModelList(typeof(ConditionType));
			npcModel.SpecialWeaponImmunityModelList = GetSelectableActionModelList(typeof(WeaponImmunity));
			npcModel.SpecialWeaponResistanceModelList = GetSelectableActionModelList(typeof(WeaponResistance));

			return npcModel;
		}

		private string GetDescription(Type EnumType, object enumValue)
		{
			var descriptionAttribute = EnumType
				.GetField(enumValue.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false)
				.FirstOrDefault() as DescriptionAttribute;


			return descriptionAttribute != null
				? descriptionAttribute.Description
				: enumValue.ToString();
		}

		public List<SelectableActionModel> GetSelectableActionModelList(Type EnumType)
		{
			List<SelectableActionModel> resultList = new List<SelectableActionModel>();
			int id = 0;
			foreach (Enum enumType in Enum.GetValues(EnumType))
			{
				resultList.Add(new SelectableActionModel(id, enumType.ToString(), GetDescription(EnumType, enumType), false));
			}

			return resultList;
		}

	}
}
