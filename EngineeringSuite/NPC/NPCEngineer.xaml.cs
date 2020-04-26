using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace EngineeringSuite.NPC
{
    /// <summary>
    /// NPC Engineer back-end C# coding
    /// Function openfolder controls creating all the required folders if they don't exist already
    /// </summary>
    public partial class NPCEngineer : Window
    {
        public NPCEngineer()
        {
            InitializeComponent();
        }
        private void openfolder(string strPath, string strFolder)
        {
            var fPath = Path.Combine(strPath, strFolder);
            System.IO.Directory.CreateDirectory(fPath);
            System.Diagnostics.Process.Start(fPath);
        }
        private void AppData_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite");
        }
        private void Projects_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Projects");
        }
        private void Artifacts_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Artifacts");
        }
        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Equipment");
        }
        private void NPC_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/NPC");
        }
        private void Parcel_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Parcel");
        }
        private void Spell_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Spell");
        }
        private void Table_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Table");
        }
        private void FG_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fantasy Grounds");
        }
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            var menuitem = (MenuItem)sender;
            switch (menuitem.Name)
            {
                case "About":
                    new ESAbout().Show();
                    break;
                case "Exit":
                    this.Close();
                    break;
                case "ManageCategories":
                    new ESManageCategories().Show();
                    break;
                case "ManageProject":
                    new ESProjectManagement().Show();
                    break;
                case "ProjectManagement":
                    new ESProjectManagement().Show();
                    break;
                case "Settings":
                    new ESSettings().Show();
                    break;
                case "RefManEngineer":
                    new NPCEngineer().Show();
                    break;
                case "SpellEngineer":
                    new NPCEngineer().Show();
                    break;
                case "Supporters":
                    new ESSupporters().Show();
                    break;
                case "TableEngineer":
                    new NPCEngineer().Show();
                    break;
            }
        }
        #region MenuOptions
        private void ESEditDeleteNPC_Click(object sender, RoutedEventArgs e)
        {
            ESEditDeleteNPC win2 = new ESEditDeleteNPC();
            win2.Show();
        }
        #endregion
        #region ActionTabs
        private void NPCELairActions_Click(object sender, RoutedEventArgs e)
        {
            NPCELairActions win2 = new NPCELairActions();
            win2.Show();
        }
        private void NPCEActions_Click(object sender, RoutedEventArgs e)
        {
            NPCEActions win3 = new NPCEActions();
            win3.Show();
        }
        private void NPCEReactions_Click(object sender, RoutedEventArgs e)
        {
            NPCEReactions win3 = new NPCEReactions();
            win3.Show();
        }
        private void NPCELegAction_Click(object sender, RoutedEventArgs e)
        {
            NPCELegActions win4 = new NPCELegActions();
            win4.Show();
        }
        #endregion
        #region NPCE_Actions
        private void ImportText_Click(object sender, RoutedEventArgs e)
        {
            NPCEImportText win2 = new NPCEImportText();
            win2.Show();
        }
        private void NPCEFGListOptions_Click(object sender, RoutedEventArgs e)
        {
            NPCEFGListOptions win2 = new NPCEFGListOptions();
            win2.Show();
        }
        #endregion
        #region NPCE_Up
        private void Up2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up10_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region NPCE_Down
        private void Down1_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region NPCE_Edit
        private void Edit1_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit10_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region NPCE_Cancel
        private void Cancel1_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel10_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        private void StrengthScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrStr.Text, out num))
            {
                strModStr1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModStr.Content = "";
                }
                else
                {
                    strModStr.Content = "+";
                }
            }    
        }
        private void DexterityScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrDex.Text, out num))
            {
                strModDex1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModDex.Content = "";
                }
                else
                {
                    strModDex.Content = "+";
                }
            }
        }
        private void ConstitutionScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrCon.Text, out num))
            {
                strModCon1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModCon.Content = "";
                }
                else
                {
                    strModCon.Content = "+";
                }
            }
        }
        private void IntelligenceScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrInt.Text, out num))
            {
                strModInt1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModInt.Content = "";
                }
                else
                {
                    strModInt.Content = "+";
                }
            }
        }
        private void WisdomScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrWis.Text, out num))
            {
                strModWis1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModWis.Content = "";
                }
                else
                {
                    strModWis.Content = "+";
                }
            }
        }
        private void CharismaScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrCha.Text, out num))
            {
                strModCha1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModCha.Content = "";
                }
                else
                {
                    strModCha.Content = "+";
                }
            }
        }
        private void CreateNPCFile(object sender, RoutedEventArgs e)
        {
            string strPath1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string strFolder1 = "Engineer Suite/NPC";
            string fName = NPC_name.Text;
            string tPath = Path.Combine(strPath1, strFolder1, fName + ".json");

            if (File.Exists(tPath))
            {
                File.Delete(tPath);
            }

            var npcDataModel = new NPCDataModel();
            npcDataModel.NPCName = NPC_name.Text;
            npcDataModel.Size = strSize.Text;
            npcDataModel.NPCType = strType.Text;
            npcDataModel.Tag = strTag.Text;
            npcDataModel.Alignment = strAlignment.Text;
            npcDataModel.AC = strAC.Text;
            npcDataModel.HP = strHP.Text;
            npcDataModel.NPCGender = strGender.Text;
            npcDataModel.Unique = chkUnique.IsChecked.Value;
            npcDataModel.NPCNamed = chkNamed.IsChecked.Value;
            npcDataModel.Speed = intSpeed.Text;
            npcDataModel.Burrow = intBurrow.Text;
            npcDataModel.Climb = intClimb.Text;
            npcDataModel.Fly = intFly.Text;
            npcDataModel.Hover = boolHover.IsChecked.Value;
            npcDataModel.Swim = intSwim.Text;

            var jsonString = JsonSerializer.Serialize<NPCDataModel>(npcDataModel, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(tPath, jsonString);
        }
    }
}
