using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.NPC.UserControls.Action
{
    /// <summary>
    /// Interaction logic for MultiAttackControl.xaml
    /// </summary>
    public partial class MultiAttackControl : UserControl
    {
        private Multiattack Multiattack;
        private ActionController actionController;

	    public MultiAttackControl()
        {
            InitializeComponent();
            actionController = new ActionController();
            Multiattack = new Multiattack();
            // The finkicky thing is this command here:
            DataContext = Multiattack;

            // What setting the DataContext to the Multiattack object does is it
            // allows us to setup the binding of the xaml to this object
            
        }
        
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Now, with sender, coming into this we can find out that it's of the Button type

            Button button = (sender as Button);

            // Because the button originated from this View, and because we set the DataContext to be
            // a Multiattack type, then we can cast it as such
            if(button.DataContext is Multiattack)
            {
                actionController.UpdateMultiAttack(button.DataContext as Multiattack);
            }
        }
    }
}
