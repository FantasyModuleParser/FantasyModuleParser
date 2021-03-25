using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Tables.ViewModels.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace FantasyModuleParser.Tables.Models
{
    public class TableModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        private string _description;
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }
        private string _notes;
        public string Notes
        {
            get => _notes;
            set => Set(ref _notes, value);
        }
        private OutputTypeEnum _outputType;
        public OutputTypeEnum OutputType
        {
            get => _outputType;
            set => Set(ref _outputType, value);
        }
        private RollMethodEnum _rollMethod;
        public RollMethodEnum RollMethod
        {
            get => _rollMethod;
            set => Set(ref _rollMethod, value);
        }
        public int RowCount = 8;
        public int ColumnCount = 8;

        private bool _isLocked;
        public bool IsLocked
        {
            get => _isLocked;
            set => Set(ref _isLocked, value);
        }
        private bool _showResultsInChat;
        public bool ShowResultsInChat
        {
            get => _showResultsInChat;
            set => Set(ref _showResultsInChat, value);
        }

        // Property Changed events are already handled within the 
        // TableOptionViewModel
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
