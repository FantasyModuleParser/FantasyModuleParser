using FantasyModuleParser.Tables.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Tables.Models
{
    public class TableModel
    {
        public string Name;
        public string Description;
        public OutputTypeEnum OutputType;
        public RollMethodEnum RollMethod;
        public int RowCount = 8;
        public int ColumnCount = 8;

        public List<string> ColumnHeaderLabels = new List<string>();
        public List<List<string>> BasicStringGridData = new List<List<string>>();


        public TableModel()
        {

        }

        public TableModel CreateDefaultTableModel()
        {
            ColumnHeaderLabels.Clear();
            BasicStringGridData.Clear();
            
            ColumnHeaderLabels.Add("From");
            ColumnHeaderLabels.Add("To");
            ColumnHeaderLabels.Add("My Column Header");

            return this;
        }

        public TableModel Load()
        {
            // For now, just create the default TableModel object, with the Load process coming later
            CreateMockData();
            return CreateDefaultTableModel();
        }

        public void CreateMockData()
        {
            List<string> mockDataOne = new List<string>();

            mockDataOne.Add("1");
            mockDataOne.Add("2");
            mockDataOne.Add("This is a test");

            BasicStringGridData.Add(mockDataOne);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

    }
}
