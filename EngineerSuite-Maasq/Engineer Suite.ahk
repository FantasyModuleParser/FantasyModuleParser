.


#NoEnv
#Persistent
#SingleInstance Force
;~ #NoTrayIcon
SendMode Input
SetWorkingDir %A_ScriptDir%
SetWinCheck()

	ES_Release_Version:= "0.6.31"
	ES_Release_Date:= "24 November 2019"
	
	;~ Gosub ES_Startup
	Gosub StartupRoutine
	GUI_Project()
	
	If (LaunchProject AND FileExist(ProjectPath)) {
		flags.project:= 1
		PROE_Ref:= "Eng_Suite"
		Gosub Project_Load
	}

	if (A_NowUTC > UDCdate) {
		CheckVersion()
	}

	ES_GUI()

	If UpdateMe {
		UpdateMe()
	}

	Gui, Eng_Suite:Show, , Engineer Suite
	If LaunchGUI{
		If (DefaultModule = 1) {
			Gosub ES_Artefact
		} Else If (DefaultModule = 2) {
			Gosub ES_Equipment
		} Else If (DefaultModule = 3) {
			Gosub ES_NPC
		} Else If (DefaultModule = 4) {
			Gosub ES_Parcel
		} Else If (DefaultModule = 5) {
			Gosub ES_Spell
		} Else If (DefaultModule = 6) {
			Gosub ES_Table
		}
	}
	
	If set.BUschedule {
		If Backup_CompareDate() {
			If set.BUask {
				MsgBox, 36, Scheduled Backup, You have a scheduled backup due.`nDo you wish to proceed with the backup now?
				IfMsgBox, No
					Return 
			}
			BAC_BuildZip()
		}
	}
	OnMessage(0x200, "WM_MOUSEMOVE")

	WM_SETCURSOR := 0x0020
	CHAND := DllCall("User32.dll\LoadCursor", "Ptr", NULL, "Int", 32649, "UPtr")
	OnMessage(WM_SETCURSOR, "SetCursor")
	
return

ES_NPC:								;{
	If (Win.npcengineer = 1) {
		NPCE_Ref:= "Eng_Suite"
		Gui, Eng_Suite:hide
		Gui, NPCE_Main:show
	} Else {
		Gui, Eng_Suite:Hide
		NPCEngineer("Eng_Suite")
	}
Return								;}

ES_Spell:							;{
	If (Win.spellengineer = 1) {
		SPE_Ref:= "Eng_Suite"
		Gui, Eng_Suite:hide
		Gui, SPE_Main:show
	} Else {
		Gui, Eng_Suite:Hide
		SpellEngineer("Eng_Suite")
	}
Return								;}

ES_Equipment:						;{
	If (Win.equipmentengineer = 1) {
		EQE_Ref:= "Eng_Suite"
		Gui, Eng_Suite:hide
		Gui, EQE_Main:show
	} Else {
		Gui, Eng_Suite:Hide
		EquipmentEngineer("Eng_Suite")
	}
Return								;}

ES_Table:							;{
	If (Win.tableengineer = 1) {
		EQE_Ref:= "Eng_Suite"
		Gui, Eng_Suite:hide
		Gui, EQE_Main:show
	} Else {
		Gui, Eng_Suite:Hide
		TableEngineer("Eng_Suite")
	}
Return								;}

ES_Artefact:						;{
	If (Win.artefactengineer = 1) {
		ART_Ref:= "Eng_Suite"
		Gui, Eng_Suite:hide
		Gui, ART_Main:show
	} Else {
		Gui, Eng_Suite:Hide
		ArtefactEngineer("Eng_Suite")
	}
Return								;}

ES_Parcel:							;{
	;~ If (Win.parcelengineer = 1) {
		;~ PAR_Ref:= "Eng_Suite"
		;~ Gui, Eng_Suite:hide
		;~ Gui, PAR_Main:show
	;~ } Else {
		;~ Gui, Eng_Suite:Hide
		;~ ParcelEngineer("Eng_Suite")
	;~ }
Return								;}

ES_RefMan:							;{
Return								;}

ES_Project:							;{
	Gui, Eng_Suite:+disabled
	ProjectEngineer("Eng_Suite")
Return								;}

Eng_SuiteGuiClose:					;{
	exitapp
Return								;}



ES_GUI(){
	global
	Gui, Eng_Suite:+hwndEng_Suite
	Gui, Eng_Suite:Color, 0xE2E1E8
	Gui, Eng_Suite:font, S10 c000000, Arial
	Gui, Eng_Suite:Add, Button, x10 y10 w140 h140 hwndES_NPC vES_NPC gES_NPC
	Gui, Eng_Suite:Add, Button, x160 y10 w140 h140 hwndES_Spell vES_Spell gES_Spell
	Gui, Eng_Suite:Add, Button, x310 y10 w140 h140 hwndES_Artefact vES_Artefact gES_Artefact
	Gui, Eng_Suite:Add, Button, x460 y10 w140 h140 hwndES_Project vES_Project gES_Project
	Gui, Eng_Suite:Add, Button, x10 y160 w140 h140 hwndES_Table vES_Table gES_Table
	Gui, Eng_Suite:Add, Button, x160 y160 w140 h140 hwndES_Parcel vES_Parcel gES_Parcel
	Gui, Eng_Suite:Add, Button, x310 y160 w140 h140 hwndES_Equipment vES_Equipment gES_Equipment
	Gui, Eng_Suite:Add, Button, x460 y160 w140 h140 hwndES_RefMan vES_RefMan gES_RefMan
	Gui, Eng_Suite:Add, Button, x545 y310 w55 h55 hwndES_Parse vES_Parse gParseProject
	GuiButtonIcon(ES_NPC, "NPC Engineer.dll", 18, "s128")
	GuiButtonIcon(ES_Spell, "NPC Engineer.dll", 19, "s128")
	GuiButtonIcon(ES_Project, "NPC Engineer.dll", 20, "s128")
	GuiButtonIcon(ES_Equipment, "NPC Engineer.dll", 22, "s128")
	GuiButtonIcon(ES_Table, "NPC Engineer.dll", 23, "s128")
	GuiButtonIcon(ES_RefMan, "NPC Engineer.dll", 21, "s128")
	GuiButtonIcon(ES_Artefact, "NPC Engineer.dll", 28, "s128")
	GuiButtonIcon(ES_Parcel, "NPC Engineer.dll", 24, "s128")
	GuiButtonIcon(ES_Parse, "NPC Engineer.dll", 7, "s48")
	Gui, Eng_Suite:Add, Text, x350 y322 w190 h31 Right, Click here to create a module for the current project.

; Menu system
;{
	Menu SuiteOptionsMenu, Add, &Create Module `tCtrl+P, ParseProject
	Menu SuiteOptionsMenu, Icon, &Create Module `tCtrl+P, NPC Engineer.dll, 6
	Menu SuiteOptionsMenu, Add
	Menu SuiteOptionsMenu, Add, Manage Categories `tCtrl+K, GUI_Categories
	Menu SuiteOptionsMenu, Icon, Manage Categories `tCtrl+K, NPC Engineer.dll, 25
	Menu SuiteOptionsMenu, Add
	Menu SuiteOptionsMenu, Add, Settings`tF11, GUI_Options
	Menu SuiteOptionsMenu, Icon, Settings`tF11, NPC Engineer.dll, 9
	Menu SuiteOptionsMenu, Add, E&xit`tESC, Eng_SuiteGuiClose
	Menu SuiteOptionsMenu, Icon, E&xit`tESC, NPC Engineer.dll, 17
	Menu EngineerSuiteMenu, Add, Options, :SuiteOptionsMenu
	
	Explorer_Menu("SuiteExplorerMenu")
	Menu EngineerSuiteMenu, Add, Directories, :SuiteExplorerMenu
	
	Backup_Menu("SuiteBackupMenu")
	Menu EngineerSuiteMenu, Add, Backup, :SuiteBackupMenu

	Help_Menu("SuiteHelpMenu", "Engineer Suite")
	Menu EngineerSuiteMenu, Add, Information, :SuiteHelpMenu
	Gui, Eng_Suite:Menu, EngineerSuiteMenu
;}
	Gui, Eng_Suite:font, S9 c000000, Segoe UI
	Gui, Eng_Suite:Add, StatusBar
	Gui, Eng_Suite:Default
	SB_SetParts(300, 100)
	SB_SetText(" " WinTProj, 1)
	Gui, Eng_Suite:font, S10 c000000, Arial
	
}



/* ========================================================
 *                     Include Files
 * ========================================================
*/

#Include NPCE_Common.ahk
#Include Project Engineer.ahk
#Include Parse Engineer.ahk
#Include WinClipAPI.ahk
#Include WinClip.ahk
#Include ES_Options.ahk
#Include NPC Engineer.ahk
#Include Spell Engineer.ahk
#Include Equipment Engineer.ahk
#Include Table Engineer.ahk
#Include Artefact Engineer.ahk


/* ========================================================
 *                  End of Include Files
 * ========================================================
*/
