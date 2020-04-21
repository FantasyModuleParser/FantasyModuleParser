using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using EngineeringSuite.NPC;

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

            using (StreamWriter fs = new StreamWriter(tPath))
            {
                fs.Write("{\n");
                fs.Write("\"Name\": " + NPC_name.Text + ",\n");
                fs.Write("\"Size\": " + strSize.Text + ",\n");
                fs.Write("\"Type\": " + strType.Text + ",\n");
                fs.Write("\"Tag\": " + strTag.Text + ",\n");
                fs.Write("\"Alignment\": " + strAlignment.Text + ",\n");
                fs.Write("\"AC\": " + strAC.Text + ",\n");
                fs.Write("\"HP\": " + strHP.Text + ",\n");
                fs.Write("\"Gender\": " + strGender.Text + ",\n");
                fs.Write("\"Unique\": " + chkUnique.IsChecked + ",\n");
                fs.Write("\"Proper Name\": " + chkNamed.IsChecked + ",\n");
                fs.Write("\"Speed\": " + strSpeed.Text + ",\n");
                fs.Write("\"Burrow\": " + strBurrow.Text + ",\n");
                fs.Write("\"Climb\": " + strClimb.Text + ",\n");
                fs.Write("\"Fly\": " + strFly.Text + ",\n");
                fs.Write("\"Hover\": " + chkHover.IsChecked + ",\n");
                fs.Write("\"Swim\": " + strSwim.Text + ",\n");
                fs.Write("\"Strength\": " + strAttrStr.Text + ",\n");
                fs.Write("\"Strength Modifier\": " + strModStr.Content +  strModStr1.Content + ",\n");
                fs.Write("\"Dexterity\": " + strAttrDex.Text + ",\n");
                fs.Write("\"Dexterity Modifier\": " + strModDex.Content + strModDex1.Content + ",\n");
                fs.Write("\"Constitution\": " + strAttrCon.Text + ",\n");
                fs.Write("\"Constitution Modifier\": " + strModCon.Content + strModCon1.Content + ",\n");
                fs.Write("\"Intelligence\": " + strAttrInt.Text + ",\n");
                fs.Write("\"Intelligence Modifier\": " + strModInt.Content + strModInt1.Content + ",\n");
                fs.Write("\"Wisdom\": " + strAttrWis.Text + ",\n");
                fs.Write("\"Wisdom Modifier\": " + strModWis.Content + strModWis1.Content + ",\n");
                fs.Write("\"Charisma\": " + strAttrCha.Text + ",\n");
                fs.Write("\"Charisma Modifier\": " + strModCha.Content + strModCha1.Content + ",\n");
                fs.Write("\"Strength Saving Throw\": " + strSaveStr.Text + ",\n");
                fs.Write("\"Strength Saving Throw Set 0\": " + chkSave0Str.IsChecked + ",\n");
                fs.Write("\"Dexterity Saving Throw\": " + strSaveDex.Text + ",\n");
                fs.Write("\"Dexterity Saving Throw Set 0\": " + chkSave0Dex.IsChecked + ",\n");
                fs.Write("\"Constitution Saving Throw\": " + strSaveCon.Text + ",\n");
                fs.Write("\"Constitution Saving Throw Set 0\": " + chkSave0Con.IsChecked + ",\n");
                fs.Write("\"Intelligence Saving Throw\": " + strSaveInt.Text + ",\n");
                fs.Write("\"Intelligence Saving Throw Set 0\": " + chkSave0Int.IsChecked + ",\n");
                fs.Write("\"Wisdom Saving Throw\": " + strSaveWis.Text + ",\n");
                fs.Write("\"Wisdom Saving Throw Set 0\": " + chkSave0Wis.IsChecked + ",\n");
                fs.Write("\"Charisma Saving Throw\": " + strSaveCha.Text + ",\n");
                fs.Write("\"Charisma Saving Throw Set 0\": " + chkSave0Cha.IsChecked + ",\n");
                fs.Write("\"Blindsight Range\": " + strBlindsight.Text + ",\n");
                fs.Write("\"Blind Beyond\": " + chkBlindBeyond.IsChecked + ",\n");
                fs.Write("\"Darkvision Range\": " + strDarkvision.Text + ",\n");
                fs.Write("\"Tremorsense Range\": " + strTremorsense.Text + ",\n");
                fs.Write("\"Truesight Range\": " + strTruesight.Text + ",\n");
                fs.Write("\"Passive Perception Score\": " + strPassivePerception.Text + ",\n");
                fs.Write("\"Challenge Rating\": " + strChallenge.Text + ",\n");
                fs.Write("\"Experience Points\": " + strExperience.Text + ",\n");
                fs.Write("\"NPC Token Location\": " + strNPCToken.Text + ",\n");
                fs.Write("\"Acid Vulnerability\": " + chkAcidVuln.IsChecked + ",\n");
                fs.Write("\"Cold Vulnerability\": " + chkColdVuln.IsChecked + ",\n");
                fs.Write("\"Fire Vulnerability\": " + chkFireVuln.IsChecked + ",\n");
                fs.Write("\"Force Vulnerability\": " + chkForceVuln.IsChecked + ",\n");
                fs.Write("\"Lightning Vulnerability\": " + chkLightningVuln.IsChecked + ",\n");
                fs.Write("\"Necrotic Vulnerability\": " + chkNecroticVuln.IsChecked + ",\n");
                fs.Write("\"Poison Vulnerability\": " + chkPoisonVuln.IsChecked + ",\n");
                fs.Write("\"Psychic Vulnerability\": " + chkPsychicVuln.IsChecked + ",\n");
                fs.Write("\"Radiant Vulnerability\": " + chkRadiantVuln.IsChecked + ",\n");
                fs.Write("\"Thunder Vulnerability\": " + chkThunderVuln.IsChecked + ",\n");
                fs.Write("\"Bludgeoning Vulnerability\": " + chkBludgeoningVuln.IsChecked + ",\n");
                fs.Write("\"Piercing Vulnerability\": " + chkPiercingVuln.IsChecked + ",\n");
                fs.Write("\"Slashing Vulnerability\": " + chkSlashingVuln.IsChecked + ",\n");
                fs.Write("\"Acid Resistance\": " + chkAcidResist.IsChecked + ",\n");
                fs.Write("\"Cold Resistance\": " + chkColdResist.IsChecked + ",\n");
                fs.Write("\"Fire Resistance\": " + chkFireResist.IsChecked + ",\n");
                fs.Write("\"Force Resistance\": " + chkForceResist.IsChecked + ",\n");
                fs.Write("\"Lightning Resistance\": " + chkLightningResist.IsChecked + ",\n");
                fs.Write("\"Necrotic Resistance\": " + chkNecroticResist.IsChecked + ",\n");
                fs.Write("\"Poison Resistance\": " + chkPoisonResist.IsChecked + ",\n");
                fs.Write("\"Psychic Resistance\": " + chkPsychicResist.IsChecked + ",\n");
                fs.Write("\"Radiant Resistance\": " + chkRadiantResist.IsChecked + ",\n");
                fs.Write("\"Thunder Resistance\": " + chkThunderResist.IsChecked + ",\n");
                fs.Write("\"Bludgeoning Resistance\": " + chkBludgeoningResist.IsChecked + ",\n");
                fs.Write("\"Piercing Resistance\": " + chkPiercingResist.IsChecked + ",\n");
                fs.Write("\"Slashing Resistance\": " + chkSlashingResist.IsChecked + ",\n");
                fs.Write("\"No Special Weapon Resistances\": " + radioNoSpecResist.IsChecked + ",\n");
                fs.Write("\"Resistant to Nonmagical Weapons\": " + radioResistNonmagic.IsChecked + ",\n");
                fs.Write("\"Resistant to Nonmagical Silvered Weapons\": " + radioResistNonmagicSilver.IsChecked + ",\n");
                fs.Write("\"Resistant to Nonmagical Adamantine Weapons\": " + radioResistNonmagicAdamant.IsChecked + ",\n");
                fs.Write("\"Resistant to Nonmagical Cold-forged Weapons\": " + radioResistNonmagicColdforged.IsChecked + ",\n");
                fs.Write("\"Resistant to Magic Weapons\": " + radioResistMagic.IsChecked + ",\n");
                fs.Write("\"Acid Immunity\": " + chkAcidImmune.IsChecked + ",\n");
                fs.Write("\"Cold Immunity\": " + chkColdImmune.IsChecked + ",\n");
                fs.Write("\"Fire Immunity\": " + chkFireImmune.IsChecked + ",\n");
                fs.Write("\"Force Immunity\": " + chkForceImmune.IsChecked + ",\n");
                fs.Write("\"Lightning Immunity\": " + chkLightningImmune.IsChecked + ",\n");
                fs.Write("\"Necrotic Immunity\": " + chkNecroticImmune.IsChecked + ",\n");
                fs.Write("\"Poison Immunity\": " + chkPoisonImmune.IsChecked + ",\n");
                fs.Write("\"Psychic Immunity\": " + chkPsychicImmune.IsChecked + ",\n");
                fs.Write("\"Radiant Immunity\": " + chkRadiantImmune.IsChecked + ",\n");
                fs.Write("\"Thunder Immunity\": " + chkThunderImmune.IsChecked + ",\n");
                fs.Write("\"Bludgeoning Immunity\": " + chkBludgeoningImmune.IsChecked + ",\n");
                fs.Write("\"Piercing Immunity\": " + chkPiercingImmune.IsChecked + ",\n");
                fs.Write("\"Slashing Immunity\": " + chkSlashingImmune.IsChecked + ",\n");
                fs.Write("\"No Special Weapon Immunity\": " + radioNoSpecResist.IsChecked + ",\n");
                fs.Write("\"Immunity to Nonmagical Weapons\": " + radioImmuneNonmagic.IsChecked + ",\n");
                fs.Write("\"Immunity to Nonmagical Silvered Weapons\": " + radioImmuneNonmagicSilver.IsChecked + ",\n");
                fs.Write("\"Immunity to Nonmagical Adamantine Weapons\": " + radioImmuneNonmagicAdamant.IsChecked + ",\n");
                fs.Write("\"Immunity to Nonmagical Cold-forged Weapons\": " + radioImmuneNonmagicColdforged.IsChecked + ",\n");
                fs.Write("\"Blinded Immunity\": " + chkBlinded.IsChecked + ",\n");
                fs.Write("\"Charmed Immunity\": " + chkCharmed.IsChecked + ",\n");
                fs.Write("\"Deafened Immunity\": " + chkDeafened.IsChecked + ",\n");
                fs.Write("\"Exhausted Immunity\": " + chkExhaustion.IsChecked + ",\n");
                fs.Write("\"Frightened Immunity\": " + chkFrightened.IsChecked + ",\n");
                fs.Write("\"Grappled Immunity\": " + chkGrappled.IsChecked + ",\n");
                fs.Write("\"Incapacated Immunity\": " + chkIncapacitated.IsChecked + ",\n");
                fs.Write("\"Invisible Immunity\": " + chkInvisible.IsChecked + ",\n");
                fs.Write("\"Paralyzed Immunity\": " + chkParalyzed.IsChecked + ",\n");
                fs.Write("\"Petrified Immunity\": " + chkPetrified.IsChecked + ",\n");
                fs.Write("\"Poisoned Immunity\": " + chkPoisoned.IsChecked + ",\n");
                fs.Write("\"Prone Immunity\": " + chkProne.IsChecked + ",\n");
                fs.Write("\"Restrained Immunity\": " + chkRestrained.IsChecked + ",\n");
                fs.Write("\"Stunned Immunity\": " + chkStunned.IsChecked + ",\n");
                fs.Write("\"Unconscious Immunity\": " + chkUnconscious.IsChecked + ",\n");
                fs.Write("\"Other Immunity\": " + chkOther.IsChecked + ",\n");
                fs.Write("\"Other Immunity Text\": " + strOther.Text + ",\n");
                fs.Write("}");
            }
        }
    }
}
