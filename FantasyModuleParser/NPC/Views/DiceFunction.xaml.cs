using FantasyModuleParser.Main;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.Views;
using FantasyModuleParser.NPC.UserControls.NPCTabs;
using System;
using FantasyModuleParser.NPC.Models.Action.Enums;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Text;
using FantasyModuleParser.Extensions;

namespace FantasyModuleParser.NPC.Views
{
    /// <summary>
    /// Interaction logic for DiceFunction.xaml
    /// </summary>
    public partial class DiceFunction : Window
    {
        #region Controllers
        public NPCController npcController { get; set; }
        #endregion
        public DiceFunction()
        {
            InitializeComponent();

            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };

            npcController = new NPCController();
            DataContext = npcController.GetNPCModel();
            SizeDice.SelectedValue = DieType.D6;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+"); ;
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Average_Click(object sender, RoutedEventArgs e)
        {
            rollResult.Clear();
            if (Bonus.Text == "" || Bonus.Text == " ")
            {
                Bonus.Text = "0";
            }
            int diceResult = (int.Parse(NumDice.Text) * ((int)SizeDice.SelectedValue + 1) / 2) + int.Parse(Bonus.Text);
            int bonusHP = int.Parse(Bonus.Text);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(diceResult + " (" + NumDice.Text + SizeDice.SelectedValue.GetDescription());
            if (int.Parse(Bonus.Text) > 0)
            {
                stringBuilder.Append(" + " + Bonus.Text + ")");
            }
            else if (int.Parse(Bonus.Text) < 0)
            {
                stringBuilder.Append(" - " + Math.Abs(bonusHP) + ")");
            }
            else
            {
                stringBuilder.Append(")");
            }
            rollResult.Text = stringBuilder.ToString();
        }
        private void Max_Click(object sender, RoutedEventArgs e)
        {
            rollResult.Clear();
            if (Bonus.Text == "" || Bonus.Text == " ")
            {
                Bonus.Text = "0";
            }
            int diceResult = int.Parse(NumDice.Text) * (int)SizeDice.SelectedValue + int.Parse(Bonus.Text);
            int bonusHP = int.Parse(Bonus.Text);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(diceResult + " (" + NumDice.Text + SizeDice.SelectedValue.GetDescription());
            if (int.Parse(Bonus.Text) > 0)
            {
                stringBuilder.Append(" + " + Bonus.Text + ")");
            }
            else if (int.Parse(Bonus.Text) < 0)
            {
                stringBuilder.Append(" - " + Math.Abs(bonusHP) + ")");
            }
            else
            {
                stringBuilder.Append(")");
            }
            rollResult.Text = stringBuilder.ToString();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            rollResult.Clear();
            if (Bonus.Text == "" || Bonus.Text == " ")
            {
                Bonus.Text = "0";
            }
            Random rnd = new Random();
            int bonusHP = int.Parse(Bonus.Text);
            int diceResult = 0;
            for (int diceNum = 0; diceNum < int.Parse(NumDice.Text); diceNum++)
                diceResult += rnd.Next(1, (int)SizeDice.SelectedValue);
            diceResult = diceResult + bonusHP;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(diceResult + " (" + NumDice.Text + SizeDice.SelectedValue.GetDescription());
            if (int.Parse(Bonus.Text) > 0)
                stringBuilder.Append(" + " + Bonus.Text + ")");
            else if (int.Parse(Bonus.Text) < 0)
                stringBuilder.Append(" - " + Math.Abs(bonusHP) + ")");
            else
                stringBuilder.Append(")");
            rollResult.Text = stringBuilder.ToString();
        }
    }
}
