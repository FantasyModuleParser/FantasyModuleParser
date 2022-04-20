﻿using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Tables.ViewModels.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace FantasyModuleParser.Tables.Models
{
    public class TableModel : ModelBase
    {
        //private string _name;
        //{
        //    get => _name;
        //    set => Set(ref _name, value);
        //}
        //private string _description;
        public string Description { get; set; }
        //{
        //    get => _description;
        //    set => Set(ref _description, value);
        //}
        //private string _notes;
        public string Notes { get; set; }
        //{
        //    get => _notes;
        //    set => Set(ref _notes, value);
        //}
        //private OutputTypeEnum _outputType;
        public OutputTypeEnum OutputType { get; set; }
        //{
        //    get => _outputType;
        //    set => Set(ref _outputType, value);
        //}
        //private RollMethodEnum _rollMethod;
        public RollMethodEnum RollMethod { get; set; }
        //{
        //    get => _rollMethod;
        //    set => Set(ref _rollMethod, value);
        //}

        //private bool _isLocked;
        public bool IsLocked { get; set; }
        //{
        //    get => _isLocked;
        //    set => Set(ref _isLocked, value);
        //}
        //private bool _showResultsInChat;
        public bool ShowResultsInChat { get; set; }
        //{
        //    get => _showResultsInChat;
        //    set => Set(ref _showResultsInChat, value);
        //}

        // Property Changed events are already handled within the 
        // TableOptionViewModel
        public List<string> ColumnHeaderLabels { get; set; }
        //public List<List<string>> BasicStringGridData = new List<List<string>>();
        public DataTable tableDataTable = new DataTable();

        // Preset Range roll modifiers
        //private int _presetRangeMinimum;
        public int PresetRangeMinimum { get; set; }
        //{
        //    get => _presetRangeMinimum;
        //    set => Set(ref _presetRangeMinimum, value);
        //}
        //private int _presetRangeMaximum;
        public int PresetRangeMaximum { get; set; }
        //{
        //    get => _presetRangeMaximum;
        //    set => Set(ref _presetRangeMaximum, value);
        //}

        // Custom Range Roll Modifiers
        //private int _customRangeD4;
        public int CustomRangeD4 { get; set; }
        //{
        //    get => _customRangeD4;
        //    set => Set(ref _customRangeD4, value);
        //}
        //private int _customRangeD6;
        public int CustomRangeD6 { get; set; }
        //{
        //    get => _customRangeD6;
        //    set => Set(ref _customRangeD6, value);
        //}
        //private int _customRangeD8;
        public int CustomRangeD8 { get; set; }
        //{
        //    get => _customRangeD8;
        //    set => Set(ref _customRangeD8, value);
        //}
        //private int _customRangeD10;
        public int CustomRangeD10 { get; set; }
        //{
        //    get => _customRangeD10;
        //    set => Set(ref _customRangeD10, value);
        //}
        //private int _customRangeD12;
        public int CustomRangeD12 { get; set; }
        //{
        //    get => _customRangeD12;
        //    set => Set(ref _customRangeD12, value);
        //}
        //private int _customRangeD20;
        public int CustomRangeD20 { get; set; }
        //{
        //    get => _customRangeD20;
        //    set => Set(ref _customRangeD20, value);
        //}
        //private int _customRangeModifier;
        public int CustomRangeModifier { get; set; }
        //{
        //    get => _customRangeModifier;
        //    set => Set(ref _customRangeModifier, value);
        //}


        public TableModel()
        {
            ColumnHeaderLabels = new List<string>();
        }

        public TableModel CreateDefaultTableModel()
        {
            //ColumnHeaderLabels.Clear();
            //BasicStringGridData.Clear();
            
            //ColumnHeaderLabels.Add("From");
            //ColumnHeaderLabels.Add("To");
            //ColumnHeaderLabels.Add("My Column Header");

            return this;
        }

        public TableModel Load()
        {
            // For now, just create the default TableModel object, with the Load process coming later
            return this;
        }

        public void Save()
        {
            if (String.IsNullOrWhiteSpace(this.Name))
                throw new Exception("The table is missing a name");

            SettingsService settingsService = new SettingsService();

            // Custom work;  Need to Save the captions from the columns in the DataGrid.
            // This is because changing the DataGridColumn.ColumnName results in the column ID changing,
            // and in the UI, it 'appears' that the grid data gets lost.
            //ColumnHeaderLabels.Clear();
            //foreach(DataColumn dataColumn in tableDataTable.Columns)
            //{
            //    if (String.IsNullOrEmpty(dataColumn.Caption))
            //        ColumnHeaderLabels.Add(dataColumn.ColumnName);
            //    else
            //        ColumnHeaderLabels.Add(dataColumn.Caption);
            //}

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
            TableModel loadedTableModel = JsonConvert.DeserializeObject<TableModel>(jsonData);

            //Before passing the table Model back, the Column captions need to be from the ColumnHeaderLabels

            for(int idx = 0; idx < loadedTableModel.ColumnHeaderLabels.Count; idx++)
            {
                loadedTableModel.tableDataTable.Columns[idx].Caption = loadedTableModel.ColumnHeaderLabels[idx];
            }

            return loadedTableModel;

        }

    }
}
