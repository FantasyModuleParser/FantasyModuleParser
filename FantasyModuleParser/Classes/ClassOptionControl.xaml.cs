using FantasyModuleParser.Classes.ViewModels;
using System.Windows.Controls;

namespace FantasyModuleParser.Classes
{
	/// <summary>
	/// Interaction logic for ClassOptionControl.xaml
	/// </summary>
	public partial class ClassOptionControl : UserControl
	{
		public ClassOptionControl()
		{
			InitializeComponent();
		}

        private void EquipmentFooterUC_SaveEquipmentAction(object sender, System.EventArgs e)
        {
			if(DataContext is ClassOptionControllViewModel)
            {
				((ClassOptionControllViewModel)DataContext).Save();
            }
        }
    }
}
