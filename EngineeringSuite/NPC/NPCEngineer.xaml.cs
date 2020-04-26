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
                fs.Write("\"name\": " + NPC_name.Text + ",\n");
                fs.Write("\"size\": " + strSize.Text + ",\n");
                fs.Write("\"type\": " + strType.Text + ",\n");
                fs.Write("\"tag\": " + strTag.Text + ",\n");
                fs.Write("\"alignment\": " + strAlignment.Text + ",\n");
                fs.Write("\"ac\": " + strAC.Text + ",\n");
                fs.Write("\"hp\": " + strHP.Text + ",\n");
                fs.Write("\"gender\": " + strGender.Text + ",\n");
                fs.Write("\"unique\": " + chkUnique.IsChecked + ",\n");
                fs.Write("\"properName\": " + chkNamed.IsChecked + ",\n");
                fs.Write("\"speed\": " + strSpeed.Text + ",\n");
                fs.Write("\"burrow\": " + strBurrow.Text + ",\n");
                fs.Write("\"climb\": " + strClimb.Text + ",\n");
                fs.Write("\"fly\": " + strFly.Text + ",\n");
                fs.Write("\"hover\": " + chkHover.IsChecked + ",\n");
                fs.Write("\"swim\": " + strSwim.Text + ",\n");
                fs.Write("\"strength\": " + strAttrStr.Text + ",\n");
                fs.Write("\"dexterity\": " + strAttrDex.Text + ",\n");
                fs.Write("\"constitution\": " + strAttrCon.Text + ",\n");
                fs.Write("\"intelligence\": " + strAttrInt.Text + ",\n");
                fs.Write("\"wisdom\": " + strAttrWis.Text + ",\n");
                fs.Write("\"charisma\": " + strAttrCha.Text + ",\n");
                fs.Write("\"strengthSavingThrow\": " + strSaveStr.Text + ",\n");
                fs.Write("\"strengthSavingThrowSet0\": " + chkSave0Str.IsChecked + ",\n");
                fs.Write("\"dexteritySavingThrow\": " + strSaveDex.Text + ",\n");
                fs.Write("\"dexteritySavingThrowSet0\": " + chkSave0Dex.IsChecked + ",\n");
                fs.Write("\"constitutionSavingThrow\": " + strSaveCon.Text + ",\n");
                fs.Write("\"constitutionSavingThrowSet0\": " + chkSave0Con.IsChecked + ",\n");
                fs.Write("\"intelligenceSavingThrow\": " + strSaveInt.Text + ",\n");
                fs.Write("\"intelligenceSavingThrowSet0\": " + chkSave0Int.IsChecked + ",\n");
                fs.Write("\"wisdomSavingThrow\": " + strSaveWis.Text + ",\n");
                fs.Write("\"wisdomSavingThrowSet0\": " + chkSave0Wis.IsChecked + ",\n");
                fs.Write("\"charismaSavingThrow\": " + strSaveCha.Text + ",\n");
                fs.Write("\"charismaSavingThrowSet0\": " + chkSave0Cha.IsChecked + ",\n");
                fs.Write("\"blindsightRange\": " + strBlindsight.Text + ",\n");
                fs.Write("\"blindBeyond\": " + chkBlindBeyond.IsChecked + ",\n");
                fs.Write("\"darkvisionRange\": " + strDarkvision.Text + ",\n");
                fs.Write("\"tremorsenseRange\": " + strTremorsense.Text + ",\n");
                fs.Write("\"truesightRange\": " + strTruesight.Text + ",\n");
                fs.Write("\"passivePerceptionScore\": " + strPassivePerception.Text + ",\n");
                fs.Write("\"challengeRating\": " + strChallenge.Text + ",\n");
                fs.Write("\"experiencePoints\": " + strExperience.Text + ",\n");
                fs.Write("\"npcTokenLocation\": " + strNPCToken.Text + ",\n");
                fs.Write("\"acidVulnerability\": " + chkAcidVuln.IsChecked + ",\n");
                fs.Write("\"coldVulnerability\": " + chkColdVuln.IsChecked + ",\n");
                fs.Write("\"fireVulnerability\": " + chkFireVuln.IsChecked + ",\n");
                fs.Write("\"forceVulnerability\": " + chkForceVuln.IsChecked + ",\n");
                fs.Write("\"lightningVulnerability\": " + chkLightningVuln.IsChecked + ",\n");
                fs.Write("\"necroticVulnerability\": " + chkNecroticVuln.IsChecked + ",\n");
                fs.Write("\"poisonVulnerability\": " + chkPoisonVuln.IsChecked + ",\n");
                fs.Write("\"psychicVulnerability\": " + chkPsychicVuln.IsChecked + ",\n");
                fs.Write("\"radiantVulnerability\": " + chkRadiantVuln.IsChecked + ",\n");
                fs.Write("\"thunderVulnerability\": " + chkThunderVuln.IsChecked + ",\n");
                fs.Write("\"bludgeoningVulnerability\": " + chkBludgeoningVuln.IsChecked + ",\n");
                fs.Write("\"piercingVulnerability\": " + chkPiercingVuln.IsChecked + ",\n");
                fs.Write("\"slashingVulnerability\": " + chkSlashingVuln.IsChecked + ",\n");
                fs.Write("\"acidResistance\": " + chkAcidResist.IsChecked + ",\n");
                fs.Write("\"coldResistance\": " + chkColdResist.IsChecked + ",\n");
                fs.Write("\"fireResistance\": " + chkFireResist.IsChecked + ",\n");
                fs.Write("\"forceResistance\": " + chkForceResist.IsChecked + ",\n");
                fs.Write("\"lightningResistance\": " + chkLightningResist.IsChecked + ",\n");
                fs.Write("\"necroticResistance\": " + chkNecroticResist.IsChecked + ",\n");
                fs.Write("\"poisonResistance\": " + chkPoisonResist.IsChecked + ",\n");
                fs.Write("\"psychicResistance\": " + chkPsychicResist.IsChecked + ",\n");
                fs.Write("\"radiantResistance\": " + chkRadiantResist.IsChecked + ",\n");
                fs.Write("\"thunderResistance\": " + chkThunderResist.IsChecked + ",\n");
                fs.Write("\"bludgeoningResistance\": " + chkBludgeoningResist.IsChecked + ",\n");
                fs.Write("\"piercingResistance\": " + chkPiercingResist.IsChecked + ",\n");
                fs.Write("\"slashingResistance\": " + chkSlashingResist.IsChecked + ",\n");
                fs.Write("\"noSpecialWeaponResistances\": " + radioNoSpecResist.IsChecked + ",\n");
                fs.Write("\"resistantToNonmagicalWeapons\": " + radioResistNonmagic.IsChecked + ",\n");
                fs.Write("\"resistantToNonmagicalSilveredWeapons\": " + radioResistNonmagicSilver.IsChecked + ",\n");
                fs.Write("\"resistantToNonmagicalAdamantineWeapons\": " + radioResistNonmagicAdamant.IsChecked + ",\n");
                fs.Write("\"resistantToNonmagicalColdForgedWeapons\": " + radioResistNonmagicColdforged.IsChecked + ",\n");
                fs.Write("\"resistantToMagicWeapons\": " + radioResistMagic.IsChecked + ",\n");
                fs.Write("\"acidImmunity\": " + chkAcidImmune.IsChecked + ",\n");
                fs.Write("\"coldImmunity\": " + chkColdImmune.IsChecked + ",\n");
                fs.Write("\"fireImmunity\": " + chkFireImmune.IsChecked + ",\n");
                fs.Write("\"forceImmunity\": " + chkForceImmune.IsChecked + ",\n");
                fs.Write("\"lightningImmunity\": " + chkLightningImmune.IsChecked + ",\n");
                fs.Write("\"necroticImmunity\": " + chkNecroticImmune.IsChecked + ",\n");
                fs.Write("\"poisonImmunity\": " + chkPoisonImmune.IsChecked + ",\n");
                fs.Write("\"psychicImmunity\": " + chkPsychicImmune.IsChecked + ",\n");
                fs.Write("\"radiantImmunity\": " + chkRadiantImmune.IsChecked + ",\n");
                fs.Write("\"thunderImmunity\": " + chkThunderImmune.IsChecked + ",\n");
                fs.Write("\"bludgeoningImmunity\": " + chkBludgeoningImmune.IsChecked + ",\n");
                fs.Write("\"piercingImmunity\": " + chkPiercingImmune.IsChecked + ",\n");
                fs.Write("\"slashingImmunity\": " + chkSlashingImmune.IsChecked + ",\n");
                fs.Write("\"noSpecialWeaponImmunity\": " + radioNoSpecResist.IsChecked + ",\n");
                fs.Write("\"immunityToNonmagicalWeapons\": " + radioImmuneNonmagic.IsChecked + ",\n");
                fs.Write("\"immunityToNonmagicalSilveredWeapons\": " + radioImmuneNonmagicSilver.IsChecked + ",\n");
                fs.Write("\"immunityToNonmagicalAdamantineWeapons\": " + radioImmuneNonmagicAdamant.IsChecked + ",\n");
                fs.Write("\"immunityToNonmagicalColdForgedWeapons\": " + radioImmuneNonmagicColdforged.IsChecked + ",\n");
                fs.Write("\"blindedImmunity\": " + chkBlinded.IsChecked + ",\n");
                fs.Write("\"charmedImmunity\": " + chkCharmed.IsChecked + ",\n");
                fs.Write("\"deafenedImmunity\": " + chkDeafened.IsChecked + ",\n");
                fs.Write("\"exhaustedImmunity\": " + chkExhaustion.IsChecked + ",\n");
                fs.Write("\"frightenedImmunity\": " + chkFrightened.IsChecked + ",\n");
                fs.Write("\"grappledImmunity\": " + chkGrappled.IsChecked + ",\n");
                fs.Write("\"incapacatedImmunity\": " + chkIncapacitated.IsChecked + ",\n");
                fs.Write("\"invisibleImmunity\": " + chkInvisible.IsChecked + ",\n");
                fs.Write("\"paralyzedImmunity\": " + chkParalyzed.IsChecked + ",\n");
                fs.Write("\"petrifiedImmunity\": " + chkPetrified.IsChecked + ",\n");
                fs.Write("\"poisonedImmunity\": " + chkPoisoned.IsChecked + ",\n");
                fs.Write("\"proneImmunity\": " + chkProne.IsChecked + ",\n");
                fs.Write("\"restrainedImmunity\": " + chkRestrained.IsChecked + ",\n");
                fs.Write("\"stunnedImmunity\": " + chkStunned.IsChecked + ",\n");
                fs.Write("\"unconsciousImmunity\": " + chkUnconscious.IsChecked + ",\n");
                fs.Write("\"otherImmunity\": " + chkOther.IsChecked + ",\n");
                fs.Write("\"otherImmunityText\": " + strOther.Text + ",\n");
                fs.Write("\"acrobatics\": " + strAcrobatics.Text + ",\n");
                fs.Write("\"animalHandling\": " + strAnimalHandling.Text + ",\n");
                fs.Write("\"arcana\": " + strArcana.Text + ",\n");
                fs.Write("\"athletics\": " + strAthletics.Text + ",\n");
                fs.Write("\"deception\": " + strDeception.Text + ",\n");
                fs.Write("\"history\": " + strHistory.Text + ",\n");
                fs.Write("\"insight\": " + strInsight.Text + ",\n");
                fs.Write("\"intimidation\": " + strIntimidation.Text + ",\n");
                fs.Write("\"investigation\": " + strInvestigation.Text + ",\n");
                fs.Write("\"medicine\": " + strMedicine.Text + ",\n");
                fs.Write("\"nature\": " + strNature.Text + ",\n");
                fs.Write("\"perception\": " + strPerception.Text + ",\n");
                fs.Write("\"performance\": " + strPerformance.Text + ",\n");
                fs.Write("\"persuasion\": " + strPersuasion.Text + ",\n");
                fs.Write("\"religion\": " + strReligion.Text + ",\n");
                fs.Write("\"sleightOfHand\": " + strSleightofHand.Text + ",\n");
                fs.Write("\"stealth\": " + strStealth.Text + ",\n");
                fs.Write("\"survival\": " + strSurvival.Text + ",\n");
                fs.Write("\"standardLanguagesKnown\": " + chkCommon.IsSelected + chkDwarvish.IsSelected + chkElvish.IsSelected + chkGiant.IsSelected + chkGnomish.IsSelected + chkGoblin.IsSelected + chkHalfling.IsSelected + chkOrc.IsSelected + chkThievesCant.IsSelected + ",\n");
                fs.Write("}");
            }
        }
    }
}
