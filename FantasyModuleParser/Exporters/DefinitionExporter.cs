using FantasyModuleParser.Main.Models;
using System.IO;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class DefinitionExporter
	{
		public static string DefinitionXML(ModuleModel moduleModel, StringWriter sw, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement("root");
			xmlWriter.WriteAttributeString("version", "4");
			DefinitionXML_Name(moduleModel, xmlWriter);
			DefinitionXML_Category(moduleModel, xmlWriter);
			DefinitionXML_Author(moduleModel, xmlWriter);
			DefinitionXML_Ruleset(xmlWriter);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			xmlWriter.Close();
			return sw.ToString();
		}

		public static string CampaignXML(StringWriter sw, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement("root");
			xmlWriter.WriteAttributeString("version", "4");
			CampaignXML_Password(xmlWriter);
			DefinitionXML_Ruleset(xmlWriter);
			CampaignXML_Username(xmlWriter);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			xmlWriter.Close();
			return sw.ToString();
		}

		private static void CampaignXML_Password(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("password");
			xmlWriter.WriteEndElement();
		}

		private static void CampaignXML_Username(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("username");
			xmlWriter.WriteString("DM");
			xmlWriter.WriteEndElement();
		}

		private static void DefinitionXML_Author(ModuleModel moduleModel, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("author");
			xmlWriter.WriteString(moduleModel.Author);
			xmlWriter.WriteEndElement();
		}

		private static void DefinitionXML_Category(ModuleModel moduleModel, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("category");
			xmlWriter.WriteString(moduleModel.Category);
			xmlWriter.WriteEndElement();
		}

		private static void DefinitionXML_Name(ModuleModel moduleModel, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteString(moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void DefinitionXML_Ruleset(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("ruleset");
			xmlWriter.WriteString("5E");
			xmlWriter.WriteEndElement();
		}
	}
}
