using FantasyModuleParser.Tables.ViewModels.Enums;
using System;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Tables.Models
{
    public class TableModel
    {
        public string Name;
        public string Description;
        public OutputTypeEnum OutputType;
        public RollMethodEnum RollMethod;
        public int GridHeight = 8;
        public int GridWidth = 8;
        public ObservableCollection<ObservableCollection<string>> GridData;

        public TableModel()
        {

        }
    }
}
