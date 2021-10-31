using System.ComponentModel;
using System.Runtime.CompilerServices;
using FantasyModuleParser.Classes.Model;
using System.Windows;
using System.Windows.Controls;
using FantasyModuleParser.Classes.Enums;
using log4net;

namespace FantasyModuleParser.Classes.UserControls
{
    /// <summary>
    /// Interaction logic for ClassHeaderUC.xaml
    /// </summary>
    public partial class ClassHeaderUC : UserControl, INotifyPropertyChanged
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ClassHeaderUC));
        public ClassHeaderUC()
		{
			InitializeComponent();
            ClassHeaderLayout.DataContext = this;
        }

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(ClassHeaderUC));


        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
        }



        // public string Name
        // {
        //     get => ClassModelValue != null ? ClassModelValue.Name : string.Empty;
        //     set
        //     {
        //         ClassModelValue.Name = value;
        //         RaisePropertyChanged(nameof(Name));
        //     }
        // }
        // public bool IsLocked
        // {
        //     get => ClassModelValue != null ? ClassModelValue.IsLocked : false;
        //     set
        //     {
        //         ClassModelValue.IsLocked = value;
        //         RaisePropertyChanged(nameof(IsLocked));
        //     }
        // }

        // public ClassHitDieEnum HitPointDiePerLevel
        // {
        //     get => ClassModelValue != null ? ClassModelValue.HitPointDiePerLevel : ClassHitDieEnum.D6;
        //     set
        //     {
        //         ClassModelValue.HitPointDiePerLevel = value;
        //         RaisePropertyChanged(nameof(HitPointDiePerLevel));
        //     }
        // }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
