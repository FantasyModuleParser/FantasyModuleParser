using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using Markdig;
using Markdig.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace FantasyModuleParser.NPC.Controllers
{
    public class NPCController
	{
		private static NPCModel _npcModel;
		public NPCModel GetNPCModel()
		{
			if(_npcModel == null)
			{
				_npcModel = InitializeNPCModel();
			}
			return _npcModel;
		}

		public void ClearNPCModel() { NPCController._npcModel = InitializeNPCModel(); }
		public void LoadNPCModel(NPCModel npcModel)
        {
			_npcModel = npcModel;
        }

		public void Save(string path, NPCModel npcModel)
		{
			using (StreamWriter file = File.CreateText(@path))
			{ 
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;
				serializer.TypeNameHandling = TypeNameHandling.Objects;
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
		public NPCModel Load(string path)
		{
			string jsonData = File.ReadAllText(@path);
			NPCModel npcModel = JsonConvert.DeserializeObject<NPCModel>(jsonData, new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.Auto
			});
			_npcModel = npcModel;
			OnLoadNpcModelEvent(EventArgs.Empty);
			return _npcModel;
		}

		public void UpdateNPCModel(NPCModel npcModel)
        {
			_npcModel = npcModel;
        }

		public NPCModel InitializeNPCModel()
		{
			NPCModel npcModel = new NPCModel();

			npcModel.DamageResistanceModelList = GetSelectableActionModelList(typeof(DamageType));
			npcModel.DamageVulnerabilityModelList = GetSelectableActionModelList(typeof(DamageType));
			npcModel.DamageImmunityModelList = GetSelectableActionModelList(typeof(DamageType));
			npcModel.ConditionImmunityModelList = GetSelectableActionModelList(typeof(ConditionType));
			npcModel.SpecialWeaponImmunityModelList = GetSelectableActionModelList(typeof(WeaponImmunity));
			npcModel.SpecialWeaponDmgImmunityModelList = GetSelectableActionModelList(typeof(DamageType));
			npcModel.SpecialWeaponResistanceModelList = GetSelectableActionModelList(typeof(WeaponResistance));
			npcModel.SpecialWeaponDmgResistanceModelList = GetSelectableActionModelList(typeof(DamageType));

			// For SpecialWeaponDmgResistance & Immunity lists, make the default selected to "No special weapon immunity / resistance", which is the first in each list
			npcModel.SpecialWeaponImmunityModelList.First().Selected = true;
			npcModel.SpecialWeaponResistanceModelList.First().Selected = true;

			// Setup Langauges for passing Unit Tests
			npcModel.StandardLanguages = new System.Collections.ObjectModel.ObservableCollection<Models.Skills.LanguageModel>();
			npcModel.MonstrousLanguages = new System.Collections.ObjectModel.ObservableCollection<Models.Skills.LanguageModel>();
			npcModel.ExoticLanguages = new System.Collections.ObjectModel.ObservableCollection<Models.Skills.LanguageModel>();
			npcModel.UserLanguages = new System.Collections.ObjectModel.ObservableCollection<Models.Skills.LanguageModel>();

			return npcModel;
		}

		// Need to decide if this lives with NPCController or FantasyGroundsExporter,
		//	as the function is used in DescriptionUC
		public string GenerateFantasyGroundsDescriptionXML(string descriptionText)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!String.IsNullOrEmpty(descriptionText)) 
			{ 
				var result = Markdig.Markdown.ToHtml(descriptionText, BuildPipeline());

				stringBuilder.Append(_replaceHtmlTagsToFGCompliance(result).Trim());
				
			}
			return stringBuilder.ToString();
		}

		private static MarkdownPipeline BuildPipeline()
		{
			return new MarkdownPipelineBuilder()
				.UseSupportedExtensions()
				.Build();
		}
		private string _replaceHtmlTagsToFGCompliance(string input)
		{
			input = tagReplace(input, "strong", "b");
			input = tagReplace(input, "em", "i");
			input = tagReplace(input, "h1", "h");
			input = tagReplace(input, "code", "frame");
			input = tagReplace(input, "ul", "list");
			input = tagReplace(input, "ins", "u");

			return input;
		}

		private string tagReplace(string input, string existingTag, string newTag)
		{
			input = input.Replace("<" + existingTag + ">", "<" + newTag + ">");
			input = input.Replace("</" + existingTag + ">", "</" + newTag + ">");
			return input;
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
