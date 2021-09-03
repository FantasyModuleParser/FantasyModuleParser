using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
    public class FantasyGroundsCampaign : ICampaign
    {
        private readonly SettingsService settingsService;

		/// <summary>
		/// 
		/// </summary>
        public FantasyGroundsCampaign()
        {
            settingsService = new SettingsService();
        }
        public void CreateCampaign(ModuleModel moduleModel)
        {
            if (string.IsNullOrEmpty(settingsService.Load().FGCampaignFolderLocation))
            {
                throw new ApplicationException("No Campaign folder has been set");
            }

            if (string.IsNullOrEmpty(moduleModel.Name))
            {
                throw new ApplicationException("No Module Name has been set");
            }

            string campaignFolderPath = Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.ModFilename);
            Directory.CreateDirectory(campaignFolderPath);

            string dbXmlFileContent = GenerateDBXmlFile(moduleModel);
            string campaignXmlFileContent = GenerateCampaignXmlContent();

			// TODO place these calls in a try catch block since they may easily throw any of 7 exceptions
			if (File.Exists(@Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "campaign.xml")))
			{
				File.Delete(@Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "campaign.xml"));
			}
			if (File.Exists(@Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "db.xml")))
			{
				File.Delete(@Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "db.xml"));
			}

			using (StreamWriter outputFile = new StreamWriter(Path.Combine(campaignFolderPath, "db.xml")))
            {
                outputFile.WriteLine(dbXmlFileContent);
            }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(campaignFolderPath, "campaign.xml")))
            {
                outputFile.WriteLine(campaignXmlFileContent);
            }
        }
		public string GenerateDBXmlFile(ModuleModel moduleModel)
		{
			NPCExporter.Save_NPC_Tokens(moduleModel, settingsService);
			NPCExporter.Save_NPC_Images(moduleModel, settingsService);

			using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
			using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
			{
				DatabaseExporter.DatabaseXML(xmlWriter, moduleModel);
				return sw.ToString();
			}
		}

		public string GenerateCampaignXmlContent()
		{
			using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
			using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
			{
				return DefinitionExporter.CampaignXML(sw, xmlWriter);
			}
		}

		private XmlWriterSettings GetXmlWriterSettings()
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				ConformanceLevel = ConformanceLevel.Document,
				OmitXmlDeclaration = false,
				CloseOutput = true,
				Indent = true,
				IndentChars = "  ",
				NewLineHandling = NewLineHandling.Replace
			};

			return settings;
		}
		private string WriteXmlDocumentToString(XmlDocument xmlDocument)
		{
			XmlWriterSettings settings = GetXmlWriterSettings();
			string document = "";
			using (StringWriter sw = new StringWriterWithEncoding())
			using (XmlWriter writer = XmlWriter.Create(sw, settings))
			{
				xmlDocument.WriteContentTo(writer);
				writer.Close();
				document = sw.ToString();
			}
			return document;
		}
		public sealed class StringWriterWithEncoding : StringWriter
		{
			private readonly Encoding encoding;
			public StringWriterWithEncoding() { }
			public StringWriterWithEncoding(Encoding encoding)
			{
				this.encoding = encoding;
			}
			public override Encoding Encoding
			{
				get { return encoding; }
			}
		}
	}
}
