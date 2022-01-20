
using FantasyModuleParser.Classes.Enums;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace FantasyModuleParser.Classes.Converters
{
    [ValueConversion(typeof(ObservableCollection<SavingThrowAttributeEnum>), typeof(SavingThrowAttributeEnum))]
    public class ProficiencySavingThrowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<SavingThrowAttributeEnum> enumValues = value as ObservableCollection<SavingThrowAttributeEnum>;
            if (enumValues == null) 
                return false;

            if (parameter == null || !(parameter is SavingThrowAttributeEnum)) return false;

            return enumValues.Contains((SavingThrowAttributeEnum)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
