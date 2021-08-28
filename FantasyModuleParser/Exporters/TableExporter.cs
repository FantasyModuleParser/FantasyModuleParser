using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Tables.Models;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class TableExporter
	{
		public static void DatabaseXML_Root_Reference_Tables(XmlWriter xmlWriter, ModuleModel module)
		{
			if (module.IncludeTables)
			{
				xmlWriter.WriteStartElement("tables");
				Tables_Category(xmlWriter, module);
				xmlWriter.WriteEndElement();
			}
		}
		
		private static void Tables_Category(XmlWriter xmlWriter, ModuleModel module)
		{
			List<TableModel> FatTableList = CommonMethods.GenerateFatTableList(module);
			FatTableList.Sort((tableOne, tableTwo) => tableOne.Name.CompareTo(tableTwo.Name));

			foreach (CategoryModel category in module.Categories)
			{
				xmlWriter.WriteStartElement("category");
				xmlWriter.WriteAttributeString("name", category.Name);
				xmlWriter.WriteAttributeString("baseicon", "0");
				xmlWriter.WriteAttributeString("decalicon", "0");

				Tables_Category_TableName(xmlWriter, FatTableList);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Tables_Category_TableName(XmlWriter xmlWriter, List<TableModel> FatTableList)
		{
			foreach (TableModel tableModel in FatTableList)
			{
				xmlWriter.WriteStartElement(TableNameToXMLFormat(tableModel));
				WriteTableLocked(xmlWriter, tableModel);
				WriteTableName(xmlWriter, tableModel);
				WriteTableDescription(xmlWriter, tableModel);
				WriteTableOutput(xmlWriter, tableModel);
				WriteTableNotes(xmlWriter, tableModel);
				WriteTableHideRolls(xmlWriter, tableModel);
				WriteTableRollModifier(xmlWriter, tableModel);
				WriteTableRollDice(xmlWriter, tableModel);
				WriteColumnLabels(xmlWriter, tableModel);
				WriteResultsColumn(xmlWriter, tableModel);
				xmlWriter.WriteEndElement();
			}
		}

		static public string TableNameToXMLFormat(TableModel tableModel)
		{
			string name = tableModel.Name.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("(", "_").Replace(")", "");
		}
		
		static public void WriteTableLocked(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("locked");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteString(tableModel.IsLocked ? "1" : "0");
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableName(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(tableModel.Name);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableDescription(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(tableModel.Description);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableOutput(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("output");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(tableModel.OutputType.GetDescription().ToLower());
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableNotes(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("notes");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteString(tableModel.Notes);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableHideRolls(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("hiderollresults");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteString(tableModel.ShowResultsInChat ? "1" : "0");
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableRollModifier(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("mod");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(tableModel.CustomRangeModifier);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableRollDice(XmlWriter xmlWriter, TableModel tableModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (tableModel.RollMethod.GetDescription() == "Custom dice roll")
			{
				for (int counter = 0; counter < tableModel.CustomRangeD4; counter++) { stringBuilder.Append("d4,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD6; counter++) { stringBuilder.Append("d6,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD8; counter++) { stringBuilder.Append("d8,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD10; counter++) { stringBuilder.Append("d10,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD12; counter++) { stringBuilder.Append("d12,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD20; counter++) { stringBuilder.Append("d20,"); }
			}
			xmlWriter.WriteStartElement("dice");
			xmlWriter.WriteAttributeString("type", "dice");
			xmlWriter.WriteString(stringBuilder.ToString().TrimEnd(','));
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteColumnLabels(XmlWriter xmlWriter, TableModel tableModel)
		{
			for (int columnHeaderIndex = 2; columnHeaderIndex < tableModel.ColumnHeaderLabels.Count; columnHeaderIndex++)
			{
				string columnHeaderValue = tableModel.ColumnHeaderLabels[columnHeaderIndex];
				xmlWriter.WriteStartElement(string.Format("labelcol{0}", columnHeaderIndex - 1));
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(columnHeaderValue);
				xmlWriter.WriteEndElement();
			}
		}
		
		static public void WriteResultsColumn(XmlWriter xmlWriter, TableModel tableModel)
		{
			int resultHeaders = tableModel.ColumnHeaderLabels.Count - 2;
			xmlWriter.WriteStartElement("resultscols");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(resultHeaders);
			xmlWriter.WriteEndElement();
		}
	}
}
