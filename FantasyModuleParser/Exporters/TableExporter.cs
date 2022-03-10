using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Tables.Models;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class TableExporter
	{
		public static void DatabaseXML_Root_Tables(XmlWriter xmlWriter, ModuleModel module)
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
				Tables_Category_TableName(xmlWriter, FatTableList);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Tables_Category_TableName(XmlWriter xmlWriter, List<TableModel> FatTableList)
		{
			int tableID = 1;
			foreach (TableModel tableModel in FatTableList)
			{
				xmlWriter.WriteStartElement("id-" + tableID.ToString("D4"));
				WriteTableDescription(xmlWriter, tableModel);
				WriteTableRollDice(xmlWriter, tableModel);
				WriteTableHideRolls(xmlWriter, tableModel);
				WriteColumnLabels(xmlWriter, tableModel);
				WriteTableLocked(xmlWriter, tableModel);
				WriteTableRollModifier(xmlWriter, tableModel);
				WriteTableName(xmlWriter, tableModel);
				WriteTableNotes(xmlWriter, tableModel);
				WriteTableOutput(xmlWriter, tableModel);
				WriteResultsColumn(xmlWriter, tableModel);
				WriteTableRows(xmlWriter, tableModel.tableDataTable);
				xmlWriter.WriteEndElement();
				tableID++;
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
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteString(tableModel.IsLocked ? "1" : "0");
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableName(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(tableModel.Name);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableDescription(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("description");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(tableModel.Description);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableOutput(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("output");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(tableModel.OutputType.GetDescription().ToLower());
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableNotes(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("notes");
			CommonMethods.Type_FormattedText(xmlWriter);
			xmlWriter.WriteString(tableModel.Notes);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableHideRolls(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("hiderollresults");
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteString(tableModel.ShowResultsInChat ? "1" : "0");
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteTableRollModifier(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("mod");
			CommonMethods.Type_Number(xmlWriter);
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
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(columnHeaderValue);
				xmlWriter.WriteEndElement();
			}
		}
		
		static public void WriteResultsColumn(XmlWriter xmlWriter, TableModel tableModel)
		{
			int resultHeaders = tableModel.ColumnHeaderLabels.Count - 2;
			xmlWriter.WriteStartElement("resultscols");
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(resultHeaders);
			xmlWriter.WriteEndElement();
		}

		static private void WriteTableRows(XmlWriter xmlWriter, DataTable dataTable)
        {
			xmlWriter.WriteStartElement("tablerows");

			for(int rowIdx = 0; rowIdx < dataTable.Rows.Count; rowIdx++)
            {
				WriteTableRows_RowId(xmlWriter, dataTable, rowIdx);
            }

			xmlWriter.WriteEndElement();
        }

		static private void WriteTableRows_RowId(XmlWriter xmlWriter, DataTable dataTable, int rowIdx)
        {
			xmlWriter.WriteStartElement("id-" + (rowIdx + 1).ToString("D5"));
			WriteTableRows_RowData(xmlWriter, dataTable, rowIdx);
			xmlWriter.WriteEndElement();
		}

		//Special Note:  Since columns 0 and 1 are guaranteed, data extraction starts at index 2 in the DataRow
		static private void WriteTableRows_RowData(XmlWriter xmlWriter, DataTable dataTable, int rowIdx)
		{
			WriteTableRows_RowData_FromRange(xmlWriter, dataTable.Rows[rowIdx]);
			xmlWriter.WriteStartElement("results");
			for(int cellIdx = 2; cellIdx < dataTable.Columns.Count; cellIdx++)
            {
				WriteTableRows_RowData_CellId(xmlWriter, dataTable.Rows[rowIdx], cellIdx);
			}
			xmlWriter.WriteEndElement();
			WriteTableRows_RowData_ToRange(xmlWriter, dataTable.Rows[rowIdx]);
		}

		static private void WriteTableRows_RowData_FromRange(XmlWriter xmlWriter, DataRow dataRow)
		{
			xmlWriter.WriteStartElement("fromrange");
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(dataRow[0].ToString());
			xmlWriter.WriteEndElement();
		}

		static private void WriteTableRows_RowData_ToRange(XmlWriter xmlWriter, DataRow dataRow)
		{
			xmlWriter.WriteStartElement("torange");
			CommonMethods.Type_Number(xmlWriter);
			// Null check
			int toRangeValue = dataRow[1] == null ? 0 : int.Parse(dataRow[1].ToString());
			xmlWriter.WriteValue(toRangeValue);
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// Special Note:  Since columns 0 and 1 are guaranteed, data extraction starts at index 2 in the DataRow
		/// </summary>
		/// <param name="xmlWriter"></param>
		/// <param name="dataRow"></param>
		/// <param name="cellIdx"></param>
		static private void WriteTableRows_RowData_CellId(XmlWriter xmlWriter, DataRow dataRow, int cellIdx)
		{
			xmlWriter.WriteStartElement("id-" + (cellIdx - 1).ToString("D5"));
			WriteTableRows_RowData_CellData_Result(xmlWriter, dataRow, cellIdx);
			WriteTableRows_RowData_CellData_ResultLink(xmlWriter, dataRow, cellIdx);
			xmlWriter.WriteEndElement();
		}

		static private void WriteTableRows_RowData_CellData_Result(XmlWriter xmlWriter, DataRow dataRow, int cellIdx)
		{
			xmlWriter.WriteStartElement("result");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(dataRow[cellIdx].ToString());
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// TODO:  This becomes relevant when linking other source material!!!
		/// 
		/// For now, this returns a hard-coded result
		/// </summary>
		/// <param name="xmlWriter"></param>
		/// <param name="dataRow"></param>
		/// <param name="cellIdx"></param>
		static private void WriteTableRows_RowData_CellData_ResultLink(XmlWriter xmlWriter, DataRow dataRow, int cellIdx)
		{
			xmlWriter.WriteStartElement("resultlink");
			CommonMethods.Type_WindowReference(xmlWriter);
			WriteTableRows_RowData_CellData_ResultLink_Class(xmlWriter, dataRow, cellIdx);
			WriteTableRows_RowData_CellData_ResultLink_RecordName(xmlWriter, dataRow, cellIdx);
			xmlWriter.WriteEndElement();
		}

		static private void WriteTableRows_RowData_CellData_ResultLink_Class(XmlWriter xmlWriter, DataRow dataRow, int cellIdx)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteEndElement();
		}

		static private void WriteTableRows_RowData_CellData_ResultLink_RecordName(XmlWriter xmlWriter, DataRow dataRow, int cellIdx)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteEndElement();
		}
	}
}
