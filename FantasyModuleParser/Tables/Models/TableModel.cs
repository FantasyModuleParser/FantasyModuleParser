using FantasyModuleParser.Main.Services;
using FantasyModuleParser.Tables.ViewModels.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

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
        public DataTable tableDataTable = new DataTable();


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
            CreateDefaultTableModel();
            CreateMockData();
            return this;
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
            if (String.IsNullOrWhiteSpace(this.Name))
                throw new Exception("The table is missing a name");

            SettingsService settingsService = new SettingsService();
            string filePath = Path.Combine(settingsService.Load().TableFolderLocation, this.Name + ".tbl");

            Save(filePath);
        }

        public void Save(string filePath)
        {
            using (StreamWriter file = File.CreateText(@filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, this);
            }
        }

        // Loads a FPM Module json file based on a given filePath
        // Must call GetModuleModel() to get the loaded module (overkill bloat happening here)
        public TableModel Load(string @filePath)
        {
            string jsonData = File.ReadAllText(@filePath);
            return JsonConvert.DeserializeObject<TableModel>(jsonData);
        }

    }
}
