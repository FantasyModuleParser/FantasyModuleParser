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
            if (int.TryParse(intSpeed.Text, out int integerSpeed))
            {
                npcDataModel.Speed = integerSpeed;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(intBurrow.Text, out int integerBurrow))
            {
                npcDataModel.Burrow = integerBurrow;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(intClimb.Text, out int integerClimb))
            {
                npcDataModel.Climb = integerClimb;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(intFly.Text, out int integerFly))
            {
                npcDataModel.Fly = integerFly;
            }
            else
            {
                //Do something if it fails to parse
            }
            npcDataModel.Hover = boolHover.IsChecked.Value;
            if (int.TryParse(intSwim.Text, out int integerSwim))
            {
                npcDataModel.Swim = integerSwim;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strAttrStr.Text, out int integerAttrStr))
            {
                npcDataModel.AttributeStr = integerAttrStr;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strAttrDex.Text, out int integerAttrDex))
            {
                npcDataModel.AttributeDex = integerAttrDex;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strAttrCon.Text, out int integerAttrCon))
            {
                npcDataModel.AttributeCon = integerAttrCon;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strAttrInt.Text, out int integerAttrInt))
            {
                npcDataModel.AttributeInt = integerAttrInt;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strAttrWis.Text, out int integerAttrWis))
            {
                npcDataModel.AttributeWis = integerAttrWis;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strAttrCha.Text, out int integerAttrCha))
            {
                npcDataModel.AttributeCha = integerAttrCha;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strSaveStr.Text, out int integerSaveStr))
            {
                npcDataModel.SavingThrowStr = integerSaveStr;
            }
            else
            {
                //Do something if it fails to parse
            }
            npcDataModel.SavingThrowStr0 = chkSave0Str.IsChecked.Value;
            if (int.TryParse(strSaveDex.Text, out int integerSaveDex))
            {
                npcDataModel.SavingThrowDex = integerSaveDex;
            }
            else
            {
                //Do something if it fails to parse
            }
            npcDataModel.SavingThrowDex0 = chkSave0Dex.IsChecked.Value;
            if (int.TryParse(strSaveCon.Text, out int integerSaveCon))
            {
                npcDataModel.SavingThrowCon = integerSaveCon;
            }
            else
            {
                //Do something if it fails to parse
            }
            npcDataModel.SavingThrowCon0 = chkSave0Con.IsChecked.Value;
            if (int.TryParse(strSaveInt.Text, out int integerSaveInt))
            {
                npcDataModel.SavingThrowInt = integerSaveInt;
            }
            else
            {
                //Do something if it fails to parse
            }
            npcDataModel.SavingThrowInt0 = chkSave0Int.IsChecked.Value;
            if (int.TryParse(strSaveWis.Text, out int integerSaveWis))
            {
                npcDataModel.SavingThrowWis = integerSaveWis;
            }
            else
            {
                //Do something if it fails to parse
            }
            npcDataModel.SavingThrowWis0 = chkSave0Wis.IsChecked.Value;
            if (int.TryParse(strSaveCha.Text, out int integerSaveCha))
            {
                npcDataModel.SavingThrowCha = integerSaveCha;
            }
            else
            {
                //Do something if it fails to parse
            }
            npcDataModel.SavingThrowCha0 = chkSave0Cha.IsChecked.Value;
            if (int.TryParse(strBlindsight.Text, out int integerBlindsight))
            {
                npcDataModel.Blindsight = integerBlindsight;
            }
            else
            {
                //Do something if it fails to parse
            }
            npcDataModel.BlindBeyond = chkBlindBeyond.IsChecked.Value;
            if (int.TryParse(strDarkvision.Text, out int integerDarkvision))
            {
                npcDataModel.Darkvision = integerDarkvision;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strTremorsense.Text, out int integerTremorsense))
            {
                npcDataModel.Tremorsense = integerTremorsense;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strTruesight.Text, out int integerTruesight))
            {
                npcDataModel.Truesight = integerTruesight;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strPassivePerception.Text, out int integerPassivePerception))
            {
                npcDataModel.PassivePerception = integerPassivePerception;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strChallenge.Text, out int integerChallengeRating))
            {
                npcDataModel.ChallengeRating = integerChallengeRating;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strExperience.Text, out int integerExperience))
            {
                npcDataModel.XP = integerExperience;
            }
            else
            {
                //Do something if it fails to parse
            }
            npcDataModel.NPCToken = strNPCToken.Text;
            npcDataModel.VulnerabilityAcid = chkAcidVuln.IsChecked.Value;
            npcDataModel.VulnerabilityCold = chkColdVuln.IsChecked.Value;
            npcDataModel.VulnerabilityFire = chkFireVuln.IsChecked.Value;
            npcDataModel.VulnerabilityForce = chkForceVuln.IsChecked.Value;
            npcDataModel.VulnerabilityLightning = chkLightningVuln.IsChecked.Value;
            npcDataModel.VulnerabilityNecrotic = chkNecroticVuln.IsChecked.Value;
            npcDataModel.VulnerabilityPoison = chkPoisonVuln.IsChecked.Value;
            npcDataModel.VulnerabilityPsychic = chkPsychicVuln.IsChecked.Value;
            npcDataModel.VulnerabilityRadiant = chkRadiantVuln.IsChecked.Value;
            npcDataModel.VulnerabilityThunder = chkThunderVuln.IsChecked.Value;
            npcDataModel.VulnerabilityBludgeoning = chkBludgeoningVuln.IsChecked.Value;
            npcDataModel.VulnerabilityPiercing = chkPiercingVuln.IsChecked.Value;
            npcDataModel.VulnerabilitySlashing = chkSlashingVuln.IsChecked.Value;
            npcDataModel.ResistanceAcid = chkAcidResist.IsChecked.Value;
            npcDataModel.ResistanceCold = chkColdResist.IsChecked.Value;
            npcDataModel.ResistanceFire = chkFireResist.IsChecked.Value;
            npcDataModel.ResistanceForce = chkForceResist.IsChecked.Value;
            npcDataModel.ResistanceLightning = chkLightningResist.IsChecked.Value;
            npcDataModel.ResistanceNecrotic = chkNecroticResist.IsChecked.Value;
            npcDataModel.ResistancePoison = chkPoisonResist.IsChecked.Value;
            npcDataModel.ResistancePsychic = chkPsychicResist.IsChecked.Value;
            npcDataModel.ResistanceRadiant = chkRadiantResist.IsChecked.Value;
            npcDataModel.ResistanceThunder = chkThunderResist.IsChecked.Value;
            npcDataModel.ResistanceBludgeoning = chkBludgeoningResist.IsChecked.Value;
            npcDataModel.ResistancePiercing = chkPiercingResist.IsChecked.Value;
            npcDataModel.ResistanceSlashing = chkSlashingResist.IsChecked.Value;
            npcDataModel.ResistanceNoSpecialWeapon = radioNoSpecResist.IsChecked.Value;
            npcDataModel.ResistanceWeaponNonmagical = radioResistNonmagic.IsChecked.Value;
            npcDataModel.ResistanceWeaponNonmagicalSilvered = radioResistNonmagicSilver.IsChecked.Value;
            npcDataModel.ResistanceWeaponNonmagicalAdamantine = radioResistNonmagicAdamant.IsChecked.Value;
            npcDataModel.ResistanceWeaponNonmagicalColdForgedIron = radioResistNonmagicColdforged.IsChecked.Value;
            npcDataModel.ResistanceWeaponMagical = radioResistMagic.IsChecked.Value;
            npcDataModel.ImmunityAcid = chkAcidImmune.IsChecked.Value;
            npcDataModel.ImmunityCold = chkColdImmune.IsChecked.Value;
            npcDataModel.ImmunityFire = chkFireImmune.IsChecked.Value;
            npcDataModel.ImmunityForce = chkForceImmune.IsChecked.Value;
            npcDataModel.ImmunityLightning = chkLightningImmune.IsChecked.Value;
            npcDataModel.ImmunityNecrotic = chkNecroticImmune.IsChecked.Value;
            npcDataModel.ImmunityPoison = chkPoisonImmune.IsChecked.Value;
            npcDataModel.ImmunityPsychic = chkPsychicImmune.IsChecked.Value;
            npcDataModel.ImmunityRadiant = chkRadiantImmune.IsChecked.Value;
            npcDataModel.ImmunityThunder = chkThunderImmune.IsChecked.Value;
            npcDataModel.ImmunityBludgeoning = chkBludgeoningImmune.IsChecked.Value;
            npcDataModel.ImmunityPiercing = chkPiercingImmune.IsChecked.Value;
            npcDataModel.ImmunitySlashing = chkSlashingImmune.IsChecked.Value;
            npcDataModel.ImmunityNoSpecialWeapon = radioNoSpecImmune.IsChecked.Value;
            npcDataModel.ImmunityWeaponNonmagical = radioImmuneNonmagic.IsChecked.Value;
            npcDataModel.ImmunityWeaponNonmagicalSilvered = radioImmuneNonmagicSilver.IsChecked.Value;
            npcDataModel.ImmunityWeaponNonmagicalAdamantine = radioImmuneNonmagicAdamant.IsChecked.Value;
            npcDataModel.ImmunityWeaponNonmagicalColdForgedIron = radioImmuneNonmagicColdforged.IsChecked.Value;
            npcDataModel.ConditionBlinded = chkBlinded.IsChecked.Value;
            npcDataModel.ConditionCharmed = chkCharmed.IsChecked.Value;
            npcDataModel.ConditionDeafened = chkDeafened.IsChecked.Value;
            npcDataModel.ConditionExhaustion = chkExhaustion.IsChecked.Value;
            npcDataModel.ConditionFrightened = chkFrightened.IsChecked.Value;
            npcDataModel.ConditionGrappled = chkGrappled.IsChecked.Value;
            npcDataModel.ConditionIncapacitated = chkIncapacitated.IsChecked.Value;
            npcDataModel.ConditionInvisible = chkInvisible.IsChecked.Value;
            npcDataModel.ConditionParalyzed = chkParalyzed.IsChecked.Value;
            npcDataModel.ConditionPetrified = chkPetrified.IsChecked.Value;
            npcDataModel.ConditionPoisoned = chkPoisoned.IsChecked.Value;
            npcDataModel.ConditionProne = chkProne.IsChecked.Value;
            npcDataModel.ConditionRestrained = chkRestrained.IsChecked.Value;
            npcDataModel.ConditionStunned = chkStunned.IsChecked.Value;
            npcDataModel.ConditionUnconscious = chkUnconscious.IsChecked.Value;
            npcDataModel.ConditionOther = chkUnconscious.IsChecked.Value;
            npcDataModel.ConditionOtherText = strOther.Text;
            if (int.TryParse(strAcrobatics.Text, out int integerAcrobatics))
            {
                npcDataModel.Acrobatics = integerAcrobatics;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strAnimalHandling.Text, out int integerAnimalHandling))
            {
                npcDataModel.AnimalHandling = integerAnimalHandling;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strArcana.Text, out int integerArcana))
            {
                npcDataModel.Arcana = integerArcana;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strAthletics.Text, out int integerAthletics))
            {
                npcDataModel.Athletics = integerAthletics;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strDeception.Text, out int integerDeception))
            {
                npcDataModel.Deception = integerDeception;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strHistory.Text, out int integerHistory))
            {
                npcDataModel.History = integerHistory;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strInsight.Text, out int integerInsight))
            {
                npcDataModel.Insight = integerInsight;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strIntimidation.Text, out int integerIntimidation))
            {
                npcDataModel.Intimidation = integerIntimidation;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strInvestigation.Text, out int integerInvestigation))
            {
                npcDataModel.Investigation = integerInvestigation;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strMedicine.Text, out int integerMedicine))
            {
                npcDataModel.Medicine = integerMedicine;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strNature.Text, out int integerNature))
            {
                npcDataModel.Nature = integerNature;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strPerformance.Text, out int integerPerformance))
            {
                npcDataModel.Performance = integerPerformance;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strPersuasion.Text, out int integerPersuasion))
            {
                npcDataModel.Persuasion = integerPersuasion;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strReligion.Text, out int integerReligion))
            {
                npcDataModel.Religion = integerReligion;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strSleightofHand.Text, out int integerSleightOfHand))
            {
                npcDataModel.SleightOfHand = integerSleightOfHand;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strStealth.Text, out int integerStealth))
            {
                npcDataModel.Stealth = integerStealth;
            }
            else
            {
                //Do something if it fails to parse
            }
            if (int.TryParse(strSurvival.Text, out int integerSurvival))
            {
                npcDataModel.Survival = integerSurvival;
            }
            else
            {
                //Do something if it fails to parse
            }

            var jsonString = JsonSerializer.Serialize<NPCDataModel>(npcDataModel, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(tPath, jsonString);
        }
    }
}
