/*
Component:	ES Options
  Version:	0.6.20
     Date:	06/11/19
 Revision:	Scheduling complete.
 */


StartupRoutine:				;{
	Critical
	InitialDirCreate()
	CommonInitialise()
	Initialise_Other()
	Set_Tooltips()
	GetDirectories()
	Load_Options()
	OptionsDirCreate()
	;~ flags.project:= 0
Return						;}

ES_Backup_Return:			;{
	BAC_GetVars()
	Save_Settings()
	Gui, ES_Backup:Destroy
return						;}

BU_AddDate:					;{
	set.BUdateadd:= Gget(SET_Hwnd.BUdateadd)
	set.BUfilestem:= Gget(SET_Hwnd.BUfilestem)
	If set.BUdateadd {
		set.BUfilename:= set.BUfilestem " " A_Year "-" A_Mon "-" A_MDay ".zip"
		Gset(SET_Hwnd.BUfilename, set.BUfilename)
	} else {
		set.BUfilename:= set.BUfilestem ".zip"
		Gset(SET_Hwnd.BUfilename, set.BUfilename)
	}
return						;}

ES_Backup_Cancel:
ES_BackupGuiClose:			;{
	Gui, ES_Backup:Destroy
return						;}

NPCE_Options_Return:		;{
	GUI, NPCE_Options:submit, NoHide
	If !FileExist(DataDir)
		FileCreateDir, %DataDir%
	If !FileExist(NPCPath)
		FileCreateDir, %NPCPath%
	If !FileExist(ProjPath)
		FileCreateDir, %ProjPath%
	If !FileExist(SpellPath)
		FileCreateDir, %SpellPath%
	If !FileExist(EquipPath)
		FileCreateDir, %EquipPath%
	If !FileExist(ArtePath)
		FileCreateDir, %ArtePath%
	Save_Options()
	GuiControl, NPCE_Project:, ModPath, %ProjPath%
	;~ Gui, NPCE_Main:-disabled
	Gui, NPCE_Options:Destroy
	;~ Build_RH_Box()	
return						;}

NPCE_Options_Cancel:
NPCE_OptionsGuiClose:		;{
	;~ Gui, NPCE_Main:-disabled
	Gui, NPCE_Options:Destroy
return						;}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |               Options Routines               |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Save_Settings() {
	global
	local File, TempWorkingDir, sz
	File:= A_Appdata "\NPC Engineer\Engineer Suite Options.opt"
	TempWorkingDir:= A_WorkingDir
	If FileExist(File)
		FileDelete, %File%
	sz:= ObjDump(File, set)
	SetWorkingDir %TempWorkingDir%
}

Load_Settings() {
	global
	local File, TempWorkingDir, TempSettings
	File:= A_Appdata "\NPC Engineer\Engineer Suite Options.opt"
	TempWorkingDir:= A_WorkingDir
	if (FileExist(File)) {
		TempSettings:= ObjLoad(File)
		For key, val in TempSettings {
			set[key]:= val
		}
	}
	SetWorkingDir %TempWorkingDir%
}

Save_Options() {
	global
	local File
	File:= A_Appdata "\NPC Engineer\NPC Engineer.ini"

	Stub:= "SaveNPC"
	IniWrite, %NPCCopyPics%, %File%, %Stub%, NPCCopyPics
	IniWrite, %DataDir%, %File%, %Stub%, DataDirStored
	IniWrite, %ProjPath%, %File%, %Stub%, ProjPathStored
	IniWrite, %NPCPath%, %File%, %Stub%, NPCPathStored
	IniWrite, %SpellPath%, %File%, %Stub%, SpellPathStored
	IniWrite, %EquipPath%, %File%, %Stub%, EquipPathStored
	IniWrite, %TablePath%, %File%, %Stub%, TablePathStored
	IniWrite, %NPCModSaveDir%, %File%, %Stub%, NPCModSaveDir

	Stub:= "SaveSpell"
	IniWrite, %SpellModSaveDir%, %File%, %Stub%, SpellModSaveDir
	IniWrite, %LaunchSpell%, %File%, %Stub%, LaunchSpell
	IniWrite, %PopCaster%, %File%, %Stub%, PopCaster

	Stub:= "SaveEquipment"
	IniWrite, %EquipCopyPics%, %File%, %Stub%, EquipCopyPics
	IniWrite, %EquipModSaveDir%, %File%, %Stub%, EquipModSaveDir
	IniWrite, %LaunchEquip%, %File%, %Stub%, LaunchEquip

	Stub:= "SaveArtefact"
	IniWrite, %ArteCopyPics%, %File%, %Stub%, ArteCopyPics
	IniWrite, %ArteModSaveDir%, %File%, %Stub%, ArteModSaveDir
	IniWrite, %LaunchArte%, %File%, %Stub%, LaunchArte

	Stub:= "SaveTable"
	IniWrite, %TableModSaveDir%, %File%, %Stub%, TableModSaveDir
	IniWrite, %LaunchTable%, %File%, %Stub%, LaunchTable

	Stub:= "LaunchNPCE"
	IniWrite, %LaunchProject%, %File%, %Stub%, LaunchProject
	IniWrite, %LaunchNPC%, %File%, %Stub%, LaunchNPC
	IniWrite, %NPCskin%, %File%, %Stub%, NPCskin
	IniWrite, %DefDesc1%, %File%, %Stub%, DefDesc1
	IniWrite, %DefDesc2%, %File%, %Stub%, DefDesc2
	IniWrite, %DefDesc3%, %File%, %Stub%, DefDesc3
	IniWrite, %DefDesc4%, %File%, %Stub%, DefDesc4
	IniWrite, %NpcArtPref%, %File%, %Stub%, NpcArtPref
	IniWrite, %LaunchGUI%, %File%, %Stub%, LaunchGUI
	IniWrite, %DefaultModule%, %File%, %Stub%, DefaultModule
	IniWrite, %TTipTime%, %File%, %Stub%, TTipTime
	IniWrite, %ToastOn%, %File%, %Stub%, ToastOn
	IniWrite, %linkGUION%, %File%, %Stub%, linkGUION
}

Load_Options() {
	global
	local File, startdate
	
	startdate:= A_NowUTC
	startdate += -3500, days
	File:= A_Appdata "\NPC Engineer\NPC Engineer.ini"

	Stub:= "SaveNPC"
	IniRead, NPCCopyPics, %File%, %Stub%, NPCCopyPics, %A_Space%
	IniRead, DataDirStored, %File%, %Stub%, DataDirStored, %A_Space%
	IniRead, ProjPathStored, %File%, %Stub%, ProjPathStored, %A_Space%
	IniRead, NPCPathStored, %File%, %Stub%, NPCPathStored, %A_Space%
	IniRead, SpellPathStored, %File%, %Stub%, SpellPathStored, %A_Space%
	IniRead, EquipPathStored, %File%, %Stub%, EquipPathStored, %A_Space%
	IniRead, TablePathStored, %File%, %Stub%, TablePathStored, %A_Space%
	IniRead, NPCModSaveDir, %File%, %Stub%, NPCModSaveDir, x

	Stub:= "SaveSpell"
	IniRead, SpellModSaveDir, %File%, %Stub%, SpellModSaveDir, x
	IniRead, LaunchSpell, %File%, %Stub%, LaunchSpell, x
	IniRead, PopCaster, %File%, %Stub%, PopCaster, 1

	Stub:= "SaveEquipment"
	IniRead, EquipModSaveDir, %File%, %Stub%, EquipModSaveDir, x
	IniRead, LaunchEquip, %File%, %Stub%, LaunchEquip, x
	IniRead, EquipCopyPics, %File%, %Stub%, EquipCopyPics, %A_Space%

	Stub:= "SaveArtefact"
	IniRead, ArteModSaveDir, %File%, %Stub%, ArteModSaveDir, x
	IniRead, LaunchArte, %File%, %Stub%, LaunchArte, x
	IniRead, ArteCopyPics, %File%, %Stub%, ArteCopyPics, %A_Space%

	Stub:= "SaveTable"
	IniRead, TableModSaveDir, %File%, %Stub%, TableModSaveDir, x
	IniRead, LaunchTable, %File%, %Stub%, LaunchTable, x

	Stub:= "LaunchNPCE"
	IniRead, LaunchProject, %File%, %Stub%, LaunchProject, %A_Space%
	IniRead, LaunchNPC, %File%, %Stub%, LaunchNPC, %A_Space%
	IniRead, NPCskin, %File%, %Stub%, NPCskin, Parchment
	IniRead, DefDesc1, %File%, %Stub%, DefDesc1, x
	IniRead, DefDesc2, %File%, %Stub%, DefDesc2, x
	IniRead, DefDesc3, %File%, %Stub%, DefDesc3, x
	IniRead, DefDesc4, %File%, %Stub%, DefDesc4, x
	IniRead, NpcArtPref, %File%, %Stub%, NpcArtPref, No Default
	IniRead, LaunchGUI, %File%, %Stub%, LaunchGUI, 0
	IniRead, DefaultModule, %File%, %Stub%, DefaultModule, 1
	IniRead, TTipTime, %File%, %Stub%, TTipTime, 3
	IniRead, ToastOn, %File%, %Stub%, ToastOn, 1
	IniRead, linkGUION, %File%, %Stub%, linkGUION, 1

	Stub:= "Paths"
	IniRead, ProjectPath, %File%, %Stub%, ProjectPath, %A_Space%
	IniRead, NPCSavePath, %File%, %Stub%, NPCSavePath, %A_Space%
	IniRead, SpellSavePath, %File%, %Stub%, SpellSavePath, %A_Space%
	IniRead, EquipSavePath, %File%, %Stub%, EquipSavePath, %A_Space%
	IniRead, TableSavePath, %File%, %Stub%, TableSavePath, %A_Space%

	Stub:= "Update"
	IniRead, UDCdate, %File%, %Stub%, UDCdate, %startdate%
	EnvAdd, UDCdate, 7, Days
	
	Stub:= "Coordinates"
	IniRead, guiX, %File%, %Stub%, SPLX, -1
	IniRead, guiY, %File%, %Stub%, SPLY, -1
	ImpG.SPLX:= guiX
	ImpG.SPLY:= guiY
	IniRead, guiX, %File%, %Stub%, NPCX, -1
	IniRead, guiY, %File%, %Stub%, NPCY, -1
	ImpG.NPCX:= guiX
	ImpG.NPCY:= guiY
	
	
	If NPCPathStored
		NPCPath:= NPCPathStored
	If DataDirStored
		DataDir:= DataDirStored
	If ProjPathStored {
		ProjPath:= ProjPathStored
		ModPath:= ProjPathStored
		ModStem:= ProjPathStored
	}
	If SpellPathStored
		SpellPath:= SpellPathStored
	If EquipPathStored
		EquipPath:= EquipPathStored
	If TablePathStored
		TablePath:= TablePathStored

	If (Defdesc1 = "x")
		Defdesc1:= 1
	Desc_Add_Text:= Defdesc1
	If (Defdesc2 = "x")
		Defdesc2:= 1
	Desc_NPC_Title:= Defdesc2
	If (Defdesc3 = "x")
		Defdesc3:= 1
	Desc_Image_Link:= Defdesc3
	If (Defdesc4 = "x")
		Defdesc4:= 0
	Desc_Spell_List:= Defdesc4
	
	If (NPCModSaveDir = "x")
		NPCModSaveDir:= 1
	If (SpellModSaveDir = "x")
		SpellModSaveDir:= 1
	If (EquipModSaveDir = "x")
		EquipModSaveDir:= 1
	If (ArteModSaveDir = "x")
		ArteModSaveDir:= 1
	If (LaunchSpell = "x")
		LaunchSpell:= 0
	If (LaunchEquip = "x")
		LaunchEquip:= 0
	If (LaunchArte = "x")
		LaunchArte:= 0
	If (LaunchTable = "x")
		LaunchTable:= 0
	If (EquipCopyPics = "x")
		EquipCopyPics:= 1

	Load_Settings()
}

BAC_GetVars() {
	global
	set.BUfiles:= Gget(SET_Hwnd.BUfiles)
	set.BUsettings:= Gget(SET_Hwnd.BUsettings)
	set.BUprojects:= Gget(SET_Hwnd.BUprojects)
	set.BUmodules:= Gget(SET_Hwnd.BUmodules)
	set.BUdropbox:= Gget(SET_Hwnd.BUdropbox)
	set.BUdropboxPath:= Gget(SET_Hwnd.BUdropboxPath)
	set.BUonedrive:= Gget(SET_Hwnd.BUonedrive)
	set.BUonedrivePath:= Gget(SET_Hwnd.BUonedrivePath)
	set.BUgoogle:= Gget(SET_Hwnd.BUgoogle)
	set.BUgooglePath:= Gget(SET_Hwnd.BUgooglePath)
	set.BUlocal:= Gget(SET_Hwnd.BUlocal)
	set.BUlocalPath:= Gget(SET_Hwnd.BUlocalPath)
	set.BUfilestem:= Gget(SET_Hwnd.BUfilestem)
	set.BUdateadd:= Gget(SET_Hwnd.BUdateadd)

	set.BUschedule:= Gget(SET_Hwnd.BUschedule)
	set.BUfrequency:= Gget(SET_Hwnd.BUfrequency)
	set.BUask:= Gget(SET_Hwnd.BUask)
}

BAC_SetVars() {
	Global
	Gset(SET_Hwnd.BUfiles, set.BUfiles)
}



GUI_Options() {
	global
	
	Gui, NPCE_Options:-SysMenu
	Gui, NPCE_Options:+hwndNPCE_Options
	Gui, NPCE_Options:Color, 0xE2E1E8
	Gui, NPCE_Options:font, S10 c000000, Arial
	
; Tab 3 system for all options
	Gui, NPCE_Options:Add, Tab3, x7 y7 w750 h356 vOptionsTabName, Suite|NPCs|Spells|Tables|Equipment|Artefacts|Parcels

;  ================================================
;  |           GUI for the 'Suite' tab            |
;  ================================================
;{
	Gui, NPCE_Options:Tab, 1
	
	Gui, NPCE_Options:font, S10 c727178, Arial Bold
	Gui, NPCE_Options:Add, GroupBox, x17 y34 w480 h226, Default Data Paths
	Gui, NPCE_Options:Add, GroupBox, x507 y34 w240 h300, Engineer Suite Settings

	Gui, NPCE_Options:font, S10 c000000, Arial
	Gui, NPCE_Options:Add, Text, x26 y57 w65 h20 right, Main:
	Gui, NPCE_Options:Add, Text, x26 y82 w65 h20 right, Project:
	Gui, NPCE_Options:Add, Text, x26 y107 w65 h20 right, NPC:
	Gui, NPCE_Options:Add, Text, x26 y132 w65 h20 right, Spell:
	Gui, NPCE_Options:Add, Text, x26 y157 w65 h20 right, Equipment:
	Gui, NPCE_Options:Add, Text, x26 y182 w65 h20 right, Artefact:
	Gui, NPCE_Options:Add, Text, x26 y207 w65 h20 right, Tables:
	Gui, NPCE_Options:Add, Text, x26 y232 w65 h20 right, Parcels:
	Gui, NPCE_Options:font, S8 c000000, Calibri
	Gui, NPCE_Options:Add, Edit, x96 y55 w338 h20 vDataDir, %DataDir%
	Gui, NPCE_Options:Add, Edit, x96 y80 w338 h20 vProjPath, %ProjPath%
	Gui, NPCE_Options:Add, Edit, x96 y105 w338 h20 vNPCPath, %NPCPath%
	Gui, NPCE_Options:Add, Edit, x96 y130 w338 h20 vSpellPath, %SpellPath%
	Gui, NPCE_Options:Add, Edit, x96 y155 w338 h20 vEquipPath, %EquipPath%
	Gui, NPCE_Options:Add, Edit, x96 y180 w338 h20 vArtePath, %ArtePath%
	Gui, NPCE_Options:Add, Edit, x96 y205 w338 h20 vTablePath, %TablePath%
	Gui, NPCE_Options:Add, Edit, x96 y230 w338 h20 vParcelPath, %ParcelPath%
	Gui, NPCE_Options:font, S10 c000000, Arial


	Gui, NPCE_Options:font, S15 c000000, Arial
	Gui, NPCE_Options:Add, Button, x437 y53 w24 h24 vOPB01 gDataPaths -Tabstop, % Chr(128193)
	Gui, NPCE_Options:Add, Button, x437 y78 w24 h24 vOPB02 gDataPaths -Tabstop, % Chr(128193)
	Gui, NPCE_Options:Add, Button, x437 y103 w24 h24 vOPB03 gDataPaths -Tabstop, % Chr(128193)
	Gui, NPCE_Options:Add, Button, x437 y128 w24 h24 vOPB04 gDataPaths -Tabstop, % Chr(128193)
	Gui, NPCE_Options:Add, Button, x437 y153 w24 h24 vOPB05 gDataPaths -Tabstop, % Chr(128193)
	Gui, NPCE_Options:Add, Button, x437 y178 w24 h24 vOPB08 gDataPaths -Tabstop, % Chr(128193)
	Gui, NPCE_Options:Add, Button, x437 y203 w24 h24 vOPB06 gDataPaths -Tabstop, % Chr(128193)
	Gui, NPCE_Options:Add, Button, x437 y228 w24 h24 vOPB07 gDataPaths -Tabstop, % Chr(128193)
	Gui, NPCE_Options:font, S18 c000000, Arial
	Gui, NPCE_Options:Add, Button, x464 y53 w24 h24 vOPR01 gReset_DataPath -Tabstop, % Chr(9100)
	Gui, NPCE_Options:Add, Button, x464 y78 w24 h24 vOPR02 gReset_DataPath -Tabstop, % Chr(9100)
	Gui, NPCE_Options:Add, Button, x464 y103 w24 h24 vOPR03 gReset_DataPath -Tabstop, % Chr(9100)
	Gui, NPCE_Options:Add, Button, x464 y128 w24 h24 vOPR04 gReset_DataPath -Tabstop, % Chr(9100)
	Gui, NPCE_Options:Add, Button, x464 y153 w24 h24 vOPR05 gReset_DataPath -Tabstop, % Chr(9100)
	Gui, NPCE_Options:Add, Button, x464 y178 w24 h24 vOPR08 gReset_DataPath -Tabstop, % Chr(9100)
	Gui, NPCE_Options:Add, Button, x464 y203 w24 h24 vOPR06 gReset_DataPath -Tabstop, % Chr(9100)
	Gui, NPCE_Options:Add, Button, x464 y228 w24 h24 vOPR07 gReset_DataPath -Tabstop, % Chr(9100)
	Gui, NPCE_Options:font, S10 c000000, Arial
	
	Gui, NPCE_Options:Add, CheckBox, vLaunchProject Checked%LaunchProject% x517 y57 w220 h20, Reload last Project on launch.
	Gui, NPCE_Options:Add, CheckBox, vLaunchGUI Checked%LaunchGUI% gGUIlaunch x517 y82 w220 h20, Select default GUI on launch.
	Gui, NPCE_Options:Add, DropDownList, AltSubmit x537 y107 w150 R5 vDefaultModule Disabled, Artefact Engineer|Equipment Engineer|NPC Engineer||Parcel Engineer|Spell Engineer|Table Engineer
	GuiControl, NPCE_Options:Enable%LaunchGUI%, DefaultModule
	GuiControl, NPCE_Options:Choose, DefaultModule, %DefaultModule%

	Gui, NPCE_Options:Add, Text, x517 y152 w110 h20, Tooltip timeout (s): 
	Gui, NPCE_Options:Add, Edit, x632 y149 w50 h23 Center
	Gui, NPCE_Options:Add, UpDown, vTTipTime Range0-10, %TTipTime%

	Gui, NPCE_Options:Add, CheckBox, vToastOn Checked%ToastOn% x517 y180 w220 h20, Use Windows Toast notifications.
	Gui, NPCE_Options:Add, CheckBox, vlinkGUION Checked%linkGUION% x517 y205 w220 h20, Use helper GUI for links.
;}


;  ================================================
;  |           GUI for the 'NPCs' tab             |
;  ================================================
;{
	Gui, NPCE_Options:Tab, 2
	
	
	Gui, NPCE_Options:font, S10 c727178, Arial Bold
	Gui, NPCE_Options:Add, GroupBox, x17 y34 w360 h228, NPC Engineer Settings
	
	Gui, NPCE_Options:font, S10 c000000, Arial
	Gui, NPCE_Options:Add, CheckBox, vNPCCopyPics Checked%NPCCopyPics% x26 y54 w340 h20, Rename image and token files and copy to NPC folder.
	Gui, NPCE_Options:Add, CheckBox, vNPCModSaveDir Checked%NPCModSaveDir% x26 y74 w340 h20, Save NPCs in a Project subfolder.
	Gui, NPCE_Options:Add, CheckBox, vLaunchNPC Checked%LaunchNPC% x26 y94 w340 h20, Reload last item edited on NPC Engineer launch.
	Gui, NPCE_Options:Add, CheckBox, vDefDesc1 Checked%DefDesc1% x26 y124 w340 h20, 'Add Descriptive Text' on by default.
	Gui, NPCE_Options:Add, CheckBox, vDefDesc2 Checked%DefDesc2% x26 y144 w340 h20, 'Add Title' on by default.
	Gui, NPCE_Options:Add, CheckBox, vDefDesc3 Checked%DefDesc3% x26 y164 w340 h20, 'Add Image Link' on by default.
	Gui, NPCE_Options:Add, CheckBox, vDefDesc4 Checked%DefDesc4% x26 y184 w340 h20, 'Include Spell List' on by default.
	
	Gui, NPCE_Options:font, S8 c000000, Arial
	Gui, NPCE_Options:Add, Text, x26 y214 w313 h15, Leading text for artwork in Reference Manual page:
	Gui, NPCE_Options:font, S8 c000000, Calibri
	Gui, NPCE_Options:Add, Edit, x26 y231 w313 h20 vNpcArtPref, %NpcArtPref%
	Gui, NPCE_Options:font, S10 c000000, Arial

	Gui, NPCE_Options:Add, Text, x26 y275 w313 h40, 'Save Fight Club XML' routine written and kindly submitted for inclusion by Zamrod. 

	Gui, NPCE_Options:font, S18 c000000, Arial
	Gui, NPCE_Options:Add, Button, x342 y229 w24 h24 vOPR25 gReset_ArtInf -Tabstop, % Chr(9100)
	Gui, NPCE_Options:font, S10 c000000, Arial
	
	Gui, NPCE_Options:Add, Picture, x464 y94 w200 h200, NPC Engineer.png
;}


;  ================================================
;  |          GUI for the 'Spells' tab            |
;  ================================================
;{
	Gui, NPCE_Options:Tab, 3
	
	Gui, NPCE_Options:font, S10 c727178, Arial Bold
	Gui, NPCE_Options:Add, GroupBox, x17 y34 w360 h86, Spell Engineer Settings
	
	Gui, NPCE_Options:font, S10 c000000, Arial
	Gui, NPCE_Options:Add, CheckBox, vSpellModSaveDir Checked%SpellModSaveDir% x26 y54 w340 h20, Save spells in a Project subfolder.
	Gui, NPCE_Options:Add, CheckBox, vLaunchSpell Checked%LaunchSpell% x26 y74 w340 h20, Reload last item edited on Spell Engineer launch.
	Gui, NPCE_Options:Add, CheckBox, vPopCaster Checked%PopCaster% x26 y94 w340 h20, Pop up caster box after spell import.

	Gui, NPCE_Options:Add, Picture, x464 y94 w200 h200, Spell Engineer.png
;}


;  ================================================
;  |          GUI for the 'Tables' tab            |
;  ================================================
;{
	Gui, NPCE_Options:Tab, 4
	
	Gui, NPCE_Options:font, S10 c727178, Arial Bold
	Gui, NPCE_Options:Add, GroupBox, x17 y34 w360 h86, Table Engineer Settings
	
	Gui, NPCE_Options:font, S10 c000000, Arial
	Gui, NPCE_Options:Add, CheckBox, vTableModSaveDir Checked%TableModSaveDir% x26 y54 w340 h20, Save Tables in a Project subfolder.
	Gui, NPCE_Options:Add, CheckBox, vLaunchTable Checked%LaunchTable% x26 y74 w340 h20, Reload last item edited on Table Engineer launch.

	Gui, NPCE_Options:Add, Picture, x464 y94 w200 h200, Table Engineer.png
;}


;  ================================================
;  |         GUI for the 'Equipment' tab          |
;  ================================================
;{
	Gui, NPCE_Options:Tab, 5
	
	Gui, NPCE_Options:font, S10 c727178, Arial Bold
	Gui, NPCE_Options:Add, GroupBox, x17 y34 w360 h86, Equipment Engineer Settings
	
	Gui, NPCE_Options:font, S10 c000000, Arial
	Gui, NPCE_Options:Add, CheckBox, vEquipCopyPics Checked%EquipCopyPics% x26 y54 w340 h20, Rename image files and copy to Equipment folder.
	Gui, NPCE_Options:Add, CheckBox, vEquipModSaveDir Checked%EquipModSaveDir% x26 y74 w340 h20, Save Equipment in a Project subfolder.
	Gui, NPCE_Options:Add, CheckBox, vLaunchEquip Checked%LaunchEquip% x26 y94 w340 h20, Reload last item edited on Equipment Engineer launch.

	Gui, NPCE_Options:Add, Picture, x464 y94 w200 h200, Equipment Engineer.png
;}


;  ================================================
;  |         GUI for the 'Artefacts' tab          |
;  ================================================
;{
	Gui, NPCE_Options:Tab, 6
	
	Gui, NPCE_Options:font, S10 c727178, Arial Bold
	Gui, NPCE_Options:Add, GroupBox, x17 y34 w360 h86, Artefact Engineer Settings
	
	Gui, NPCE_Options:font, S10 c000000, Arial
	Gui, NPCE_Options:Add, CheckBox, vArteCopyPics Checked%ArteCopyPics% x26 y54 w340 h20, Rename image files and copy to Artefact folder.
	Gui, NPCE_Options:Add, CheckBox, vArteModSaveDir Checked%ArteModSaveDir% x26 y74 w340 h20, Save Artefacts in a Project subfolder.
	Gui, NPCE_Options:Add, CheckBox, vLaunchArte Checked%LaunchArte% x26 y94 w340 h20, Reload last Artefact edited on Artefact Engineer launch.

	Gui, NPCE_Options:Add, Picture, x464 y94 w200 h200, Artefact Engineer.png
;}


;  ================================================
;  |         GUI for the 'Parcels' tab            |
;  ================================================
;{
	Gui, NPCE_Options:Tab, 7
	
	Gui, NPCE_Options:font, S10 c727178, Arial Bold
	Gui, NPCE_Options:Add, GroupBox, x17 y34 w360 h86, Parcel Engineer Settings
	
	Gui, NPCE_Options:font, S10 c000000, Arial
	Gui, NPCE_Options:Add, CheckBox, vParcelModSaveDir Checked%ParcelModSaveDir% x26 y54 w340 h20, Save Parcels in a Project subfolder.
	Gui, NPCE_Options:Add, CheckBox, vLaunchParcel Checked%LaunchParcel% x26 y74 w340 h20, Reload last item edited on Parcel Engineer launch.

	Gui, NPCE_Options:Add, Picture, x464 y94 w200 h200, Parcel Engineer.png
;}



	Gui, NPCE_Options:Tab		; End of tab3 system.

; Other GUI controls on the Options window (NPCE_Options)

	Gui, NPCE_Options:Add, Button, x640 y370 w100 h23 +border vNPCE_Options_Return gNPCE_Options_Return, Accept
	Gui, NPCE_Options:Add, Button, x530 y370 w100 h23 +border vNPCE_Options_Cancel gNPCE_Options_Cancel, Cancel
	Gui, NPCE_Options:Show, w764 h400, Engineer Suite Settings
	
}

GUI_Backup(tb) {
	global
	Local stringthing, on, schedstring
	stringthing:= "Settings|Schedule|Restore|"
	stringreplace, stringthing, stringthing, %tb%, %tb%`|
	
	Gui, ES_Backup:-SysMenu
	Gui, ES_Backup:+hwndES_Backup
	Gui, ES_Backup:Color, 0xE2E1E8
	Gui, ES_Backup:font, S10 c000000, Arial
	
; Tab 3 system for all options
	Gui, ES_Backup:Add, Tab3, x7 y7 w750 h356 vOptionsTabName, %stringthing%

;  ================================================
;  |         GUI for the 'Settings' tab           |
;  ================================================
;{
	Gui, ES_Backup:Tab, 1

	Gui, ES_Backup:font, S10 c727178, Arial Bold
	Gui, ES_Backup:Add, GroupBox, x17 y34 w360 h100, Included Items
	Gui, ES_Backup:Add, GroupBox, x387 y34 w360 h100, File Options
	Gui, ES_Backup:Add, GroupBox, x17 y144 w730 h200, Backup Locations

	Gui, ES_Backup:font, S10 c000000, Arial
	on:= set.BUfiles
	Gui, ES_Backup:Add, CheckBox, x28 y55 w340 h17 HwndTempy Checked%on%, Saved Files (NPCs, spells, artefacts etc.)
		SET_Hwnd.BUfiles:= Tempy
	on:= set.BUsettings
	Gui, ES_Backup:Add, CheckBox, x28 y74 w340 h17 HwndTempy Checked%on%, Settings (*.ini and *.json files)
		SET_Hwnd.BUsettings:= Tempy
	on:= set.BUprojects
	Gui, ES_Backup:Add, CheckBox, x28 y93 w340 h17 HwndTempy Checked%on%, Projects (Raw files for Engineer Suite to parse)
		SET_Hwnd.BUprojects:= Tempy
	on:= set.BUmodules
	Gui, ES_Backup:Add, CheckBox, x28 y112 w340 h17 HwndTempy Checked%on%, FG Modules (*.mod files from your FG modules folder)
		SET_Hwnd.BUmodules:= Tempy


	Gui, ES_Backup:Add, Text, x400 y55 w65 h20 right, Filename:
	Gui, ES_Backup:Add, Edit, x470 y52 w260 h23 HwndTempy gBU_AddDate, % set.BUfilestem
		SET_Hwnd.BUfilestem:= Tempy
	Gui, ES_Backup:Add, Text, x400 y77 w65 h20 right, Add Date:
	on:= set.BUdateadd
	Gui, ES_Backup:Add, CheckBox, x470 y77 HwndTempy Checked%on% gBU_AddDate,  
		SET_Hwnd.BUdateadd:= Tempy
	
	If set.BUdateadd {
		set.BUfilename:= set.BUfilestem " " A_Year "-" A_Mon "-" A_MDay ".zip"
		Gset(SET_Hwnd.BUfilename, set.BUfilename)
	} else {
		set.BUfilename:= set.BUfilestem ".zip"
		Gset(SET_Hwnd.BUfilename, set.BUfilename)
	}
	Gui, ES_Backup:Add, Edit, x470 y102 w260 h23 HwndTempy -TabStop ReadOnly, % set.BUfilename
		SET_Hwnd.BUfilename:= Tempy


	Gui, ES_Backup:font, S10 c000000, Arial
	on:= set.BUdropbox
	Gui, ES_Backup:Add, CheckBox, x28 y170 w85 h17 HwndTempy Checked%on%, Dropbox: 
		SET_Hwnd.BUdropbox:= Tempy
	Gui, ES_Backup:font, S8 c000000, Calibri
	Gui, ES_Backup:Add, Edit, x135 y169 w400 h20 HwndTempy, % set.BUdropboxPath
		SET_Hwnd.BUdropboxPath:= Tempy
	Gui, ES_Backup:font, S10 c000000, Arial
	Gui, ES_Backup:Add, Button, x540 y167 w24 h24 hwndMPB1 vMPB01 gBackupPath -Tabstop 
	GuiButtonIcon(MPB1, "shell32.dll", 310, "s16")
	on:= set.BUonedrive
	Gui, ES_Backup:Add, CheckBox, x28 y194 w85 h17 HwndTempy Checked%on%, OneDrive: 
		SET_Hwnd.BUonedrive:= Tempy
	Gui, ES_Backup:font, S8 c000000, Calibri
	Gui, ES_Backup:Add, Edit, x135 y193 w400 h20 HwndTempy, % set.BUonedrivePath
		SET_Hwnd.BUonedrivePath:= Tempy
	Gui, ES_Backup:font, S10 c000000, Arial
	Gui, ES_Backup:Add, Button, x540 y191 w24 h24 hwndMPB2 vMPB02 gBackupPath -Tabstop 
	GuiButtonIcon(MPB2, "shell32.dll", 310, "s16")
	on:= set.BUgoogle
	Gui, ES_Backup:Add, CheckBox, x28 y218 w100 h17 HwndTempy Checked%on%, Google Sync: 
		SET_Hwnd.BUgoogle:= Tempy
	Gui, ES_Backup:font, S8 c000000, Calibri
	Gui, ES_Backup:Add, Edit, x135 y217 w400 h20 HwndTempy, % set.BUgooglePath
		SET_Hwnd.BUgooglePath:= Tempy
	Gui, ES_Backup:font, S10 c000000, Arial
	Gui, ES_Backup:Add, Button, x540 y215 w24 h24 hwndMPB3 vMPB03 gBackupPath -Tabstop 
	GuiButtonIcon(MPB3, "shell32.dll", 310, "s16")
	on:= set.BUlocal
	Gui, ES_Backup:Add, CheckBox, x28 y242 w85 h17 HwndTempy Checked%on%, Local: 
		SET_Hwnd.BUlocal:= Tempy
	Gui, ES_Backup:font, S8 c000000, Calibri
	Gui, ES_Backup:Add, Edit, x135 y241 w400 h20 HwndTempy, % set.BUlocalPath
		SET_Hwnd.BUlocalPath:= Tempy
	Gui, ES_Backup:font, S10 c000000, Arial
	Gui, ES_Backup:Add, Button, x540 y239 w24 h24 hwndMPB4 vMPB04 gBackupPath -Tabstop 
	GuiButtonIcon(MPB4, "shell32.dll", 310, "s16")
;}


;  ================================================
;  |         GUI for the 'Schedule' tab           |
;  ================================================
;{
	Gui, ES_Backup:Tab, 2
	
	Gui, ES_Backup:font, S10 c727178, Arial Bold
	Gui, ES_Backup:Add, GroupBox, x17 y34 w360 h228, Schedule Settings

	Gui, ES_Backup:font, S10 c000000, Arial
	on:= set.BUschedule
	Gui, ES_Backup:Add, CheckBox, x28 y60 w340 h17 HwndTempy Checked%on%, Schedule backups.
		SET_Hwnd.BUschedule:= Tempy
	on:= set.BUask
	Gui, ES_Backup:Add, CheckBox, x28 y85 w340 h17 HwndTempy Checked%on%, Ask before starting scheduled backup.
		SET_Hwnd.BUask:= Tempy

	on:= set.BUfrequency
	schedstring:= "1|2|3|5|7|14|21|28|"
	stringreplace, schedstring, schedstring, %on%, %on%|
	Gui, ES_Backup:Add, Text, x28 y120 w240 h20 right, Select number of days between backups:
	Gui, ES_Backup:Add, DDL, x274 y116 w50 Center HwndTempy, %schedstring%
		SET_Hwnd.BUfrequency:= Tempy
;}


;  ================================================
;  |         GUI for the 'Restore' tab            |
;  ================================================
;{
	Gui, ES_Backup:Tab, 3
	
	
	;~ Gui, ES_Backup:font, S10 c727178, Arial Bold
	;~ Gui, ES_Backup:Add, GroupBox, x17 y34 w360 h228, NPC Engineer Settings

;}


	Gui, ES_Backup:Tab		; End of tab3 system.

; Other GUI controls on the Options window (ES_Backup)

	Gui, ES_Backup:font, S10 c000000, Arial
	Gui, ES_Backup:Add, Button, x640 y370 w100 h23 +border vES_Backup_Return gES_Backup_Return, Save
	Gui, ES_Backup:Add, Button, x530 y370 w100 h23 +border vES_Backup_Cancel gES_Backup_Cancel, Cancel
	Gui, ES_Backup:Show, w764 h400, Engineer Suite Backup & Restore Options
}




InitialDirCreate(){
	global
	DataDir:= A_Appdata "\NPC Engineer"
	NPCPath:= DataDir "\Saved NPC Files"
	SpellPath:= DataDir "\Saved Spell Files"
	ProjPath:= DataDir "\Saved Project Files"
	EquipPath:= DataDir "\Saved Equipment Files"
	ArtePath:= DataDir "\Saved Artefact Files"
	TablePath:= DataDir "\Saved Table Files"
	ParcelPath:= DataDir "\Saved Parcel Files"
	If !FileExist(DataDir) {
		FileCreateDir %Datadir%
	}
	If !FileExist(NPCPath) {
		FileCreateDir %NPCPath%
	}
	If !FileExist(SpellPath) {
		FileCreateDir %SpellPath%
	}
	If !FileExist(ProjPath) {
		FileCreateDir %ProjPath%
	}
	If !FileExist(EquipPath) {
		FileCreateDir %EquipPath%
	}
	If !FileExist(ArtePath) {
		FileCreateDir %ArtePath%
	}
	If !FileExist(TablePath) {
		FileCreateDir %TablePath%
	}
	If !FileExist(ParcelPath) {
		FileCreateDir %ParcelPath%
	}
	If !FileExist(A_Appdata "\NPC Engineer\NPC Engineer.ini") {
		FileCopy, Defaults\defaultoptions.ini, %A_Appdata% \NPC Engineer\NPC Engineer.ini, 1
	}
	RegRead, AppliedDPI, HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics, AppliedDPI	
}

OptionsDirCreate(){
	global
	If !FileExist(DataDir) {
		FileCreateDir %Datadir%
	}
	If !FileExist(NPCPath) {
		FileCreateDir %NPCPath%
	}
	If !FileExist(SpellPath) {
		FileCreateDir %SpellPath%
	}
	If !FileExist(ProjPath) {
		FileCreateDir %ProjPath%
	}
	If !FileExist(EquipPath) {
		FileCreateDir %EquipPath%
	}
	If !FileExist(ArtePath) {
		FileCreateDir %ArtePath%
	}
	If !FileExist(TablePath) {
		FileCreateDir %TablePath%
	}
	If !FileExist(ParcelPath) {
		FileCreateDir %ParcelPath%
	}
}

CommonInitialise() {
	global
	shield:= "<img src=""" imgdir("shield.png") """ align=""center"" alt=""Shield"">"
	CastClass:= ["Artificer", "Barbarian", "Bard", "Cleric", "Druid", "Fighter", "Monk", "Paladin", "Ranger", "Rogue", "Sorcerer", "Warlock", "Wizard"]
	CastArch1:= ["Cleric Arcana Domain", "Cleric Death Domain", "Cleric Forge Domain", "Cleric Grave Domain", "Cleric Knowledge Domain", "Cleric Life Domain", "Cleric Light Domain", "Cleric Nature Domain", "Cleric Tempest Domain", "Cleric Trickery Domain", "Cleric War Domain", "Druid Arctic Circle", "Druid Coast Circle", "Druid Desert Circle", "Druid Forest Circle", "Druid Grassland Circle", "Druid Mountain Circle", "Druid Swamp Circle", "Druid Underdark Circle", "Druid Spore Circle", "Druid Dream Circle", "Druid Moon Circle", "Druid Shepherd Circle"]
	CastArch2:= ["Artificer (Alchemist)", "Artificer (Artillerist)", "Sorcerer (Divine Soul)", "Sorcerer (Draconic Bloodline)", "Sorcerer (Shadow Magic)", "Sorcerer (Storm Sorcery)", "Sorcerer (Wild Magic)", "Warlock (Archfey)", "Warlock (Celestial)", "Warlock (Fiend)", "Warlock (Great Old One)", "Warlock (Hexblade)", "Warlock (Undying)", "Wizard (Bladesinging)", "Wizard (War Magic)", "Wizard (Abjuration)", "Wizard (Conjuration)", "Wizard (Divination)", "Wizard (Enchantment)", "Wizard (Evocation)", "Wizard (Illusion)", "Wizard (Invention)", "Wizard (Necromancy)", "Wizard (Transmutation)"]
	CastArch3:= ["Bard College of Glamour", "Bard College of Lore", "Bard College of Swords", "Bard College of Valor", "Bard College of Whispers", "Paladin Oath of Conquest", "Paladin Oath of Vengeance", "Paladin Oath of the Crown", "Paladin Oath of Devotion", "Paladin Oath of Redemption", "Paladin Oath of the Ancients", "Paladin Oathbreaker", "Ranger Beast Master", "Ranger Gloom Stalker", "Ranger Horizon Walker", "Ranger Hunter", "Ranger Monster Slayer", "Arcane Trickster", "Eldritch Knight"]
	SpCasters:=[]
	SpCasters.push(CastClass*)
	SpCasters.push(CastArch1*)
	SpCasters.push(CastArch2*)
	SpCasters.push(CastArch3*)

	liga_fl:= Chr(64258)
	liga_fi:= Chr(64257)
	liga_ff:= Chr(64256)
	liga_hy:= Chr(8208)
	
	liga_sq1:= Chr(8216)
	liga_sq2:= Chr(8217)
	liga_sq3:= Chr(8219)
	liga_sq4:= Chr(8242)
	
	liga_dq1:= Chr(8220)
	liga_dq2:= Chr(8221)
	liga_dq3:= Chr(8223)
	liga_dq4:= Chr(8243)

	TerrainType:= ["Arctic", "Coastal", "Desert", "Forest", "Grassland", "Hill", "Mountain", "Swamp", "Underdark", "Underwater", "Urban", "Planes"]
	OriginLore:= ["Fantasy", "Aztec", "Celtic", "Egyptian", "Finnish", "Greek", "Inca", "Norse", "Roman"]

	flags:=[]
	flags.project:= 0
	flags.npc:= 0
	flags.spell:= 0
	flags.equip:= 0
	flags.arte:= 0
	
	NPCCopyPics:= 1
	NPCModSaveDir:= 0
	LaunchNPC:= 0

	SpellModSaveDir:= 0
	LaunchSpell:= 0

	EquipCopyPics:= 1
	EquipModSaveDir:= 0
	LaunchEquip:= 0

	ArteCopyPics:= 1
	ArteModSaveDir:= 0
	LaunchArte:= 0

	TableCopyPics:= 1
	TableModSaveDir:= 0
	LaunchTable:= 0
	
	ParcelModSaveDir:= 0
	LaunchParcel:= 0

	LaunchGUI:= 0
	DefaultModule:= NPC Engineer
	
	TTipTime:= 3
	
	LangStan:= ["Common", "Dwarvish", "Elvish", "Giant", "Gnomish", "Goblin", "Halfling", "Orc", "Thieves' cant"]
	LangExot:= ["Abyssal", "Celestial", "Draconic", "Deep speech", "Infernal", "Primordial", "Sylvan", "Druidic", "Undercommon"]
	LangMons:= ["Aarakocra", "Aquan", "Auran", "Bullywug", "Gith", "Gnoll", "Grell", "Grung", "Hook horror", "Ice toad", "Ignan", "Ixitxachitl", "Modron", "Otyugh", "Sahuagin", "Slaad", "Sphinx", "Terran", "Thri-kreen", "Tlincalli", "Troglodyte", "Umber hulk", "Vegepygmy", "Yeti"]
	LangUser:= []
	
	CastUser:= []
	
	ImpG:= {}
	ImpG.SPLX:= -1
	ImpG.SPLY:= -1
	ImpG.NPCX:= -1
	ImpG.NPCY:= -1
	
	set:= {}
	SET_Hwnd:= {}
	DefaultOptions()
}

Set_Tooltips() {
	global
	TTip:= []
	TTip.ButtonToClipboard:= "Copy the NPC text to the clipboard ready for pasting into another document." Chr(10) "This will use the format in the right-hand pane."
	TTip.ButtonToText:= "Save the NPC to your drive as a *.NPC file." Chr(10) "This can be reloaded for further editing."
	TTip.ButtonOutputAppend:= "Add this NPC to a parsing project (or update it if it is already part of the project) depending on the settings chosen in your project." Chr(10) "If you haven't set up a project, you are taken to the project management window."
	
; Base Stats Tab
;{
	TTip.NPCname := "Enter the NPC's name." Chr(10) "This will also set the main image name and token name, and default save name."
	TTip.NPCgender:= "Enter or select the NPC's gender." Chr(10) "This will insert gender-specific pronouns in the descriptive text."
	TTip.NPCunique:= "Check if the NPC is unique for any reason." Chr(10) "This will capitalise its name in the descriptive text."
	TTip.NPCpropername:= "Check if the name entered is a proper name." Chr(10) "This will use just the first word of the name in the decsriptive text." Chr(10) "('Maasq Hammerheart' will be referred to as 'Maasq' throughout; 'Ironbane, Destroyer of Worlds' as 'Ironbane') "
	TTip.NPCsize:= "Select from the standard size categories or enter your own."
	TTip.NPCtype:= "Select from the standard creature types or enter your own."
	TTip.NPCtag:= "Select a specific sub-type of creature or enter your own." Chr(10) "Example: a sub-type of humanoid may be half-elf." Chr(10) "You can leave this box blank if desired."
	TTip.NPCalign:= "Select from the standard alignments or enter your own."
	TTip.NPCac:= "Enter the NPC's armor class. You can also enter the type of armor in brackets." Chr(10) "Example: 12 (leather armor)"
	TTip.NPChp:= "Enter the NPC's hit points, followed by its hit dice in brackets." Chr(10) "Example: 16 (3d6 + 6)" Chr(10) "Double-click to open a window that will help build a correct expression."
	TTip.ButtonAverageHP:= "Click on this to calculate and apply the average hit points for the range." Chr(10) "This is the value stated in official products for NPC hit points."
	TTip.ButtonRollHP:= "Click on this button to roll hit points." Chr(10) "This will give each NPC an individual total."
	TTip.NPCwalk:= "Enter the NPC's base movement speed in feet (ft) or use the arrows to increase/decrease the value." Chr(10) "A value of 0 indicates the NPC cannot move in this way."
	TTip.NPCburrow:= "Enter the NPC's burrowing speed in feet (ft) or use the arrows to increase/decrease the value." Chr(10) "A value of 0 indicates the NPC cannot move in this way."
	TTip.NPCclimb:= "Enter the NPC's climbing speed in feet (ft) or use the arrows to increase/decrease the value." Chr(10) "A value of 0 indicates the NPC cannot move in this way."
	TTip.NPCfly:= "Enter the NPC's flying speed in feet (ft) or use the arrows to increase/decrease the value." Chr(10) "A value of 0 indicates the NPC cannot move in this way."
	TTip.NPChover:= "Check this box if the NPC hovers rather than flies."
	TTip.NPCswim:= "Enter the NPC's swimming speed in feet (ft) or use the arrows to increase/decrease the value." Chr(10) "A value of 0 indicates the NPC cannot move in this way."
	TTip.NPCstr:= "Enter the NPC's strength ability score or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30." Chr(10) "The ability modifier is calculated and shown to the right of this box."
	TTip.NPCdex:= "Enter the NPC's dexterity ability score or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30." Chr(10) "The ability modifier is calculated and shown to the right of this box."
	TTip.NPCcon:= "Enter the NPC's constitution ability score or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30." Chr(10) "The ability modifier is calculated and shown to the right of this box."
	TTip.NPCint:= "Enter the NPC's intelligence ability score or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30." Chr(10) "The ability modifier is calculated and shown to the right of this box."
	TTip.NPCwis:= "Enter the NPC's wisdom ability score or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30." Chr(10) "The ability modifier is calculated and shown to the right of this box."
	TTip.NPCcha:= "Enter the NPC's charisma ability score or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30." Chr(10) "The ability modifier is calculated and shown to the right of this box."
	TTip.NPCstrsav:= "Enter the NPC's saving throw bonus for strength saves or use the arrows to increase/decrease the value."
	TTip.NPCdexsav:= "Enter the NPC's saving throw bonus for dexterity saves or use the arrows to increase/decrease the value."
	TTip.NPCconsav:= "Enter the NPC's saving throw bonus for constitution saves or use the arrows to increase/decrease the value."
	TTip.NPCintsav:= "Enter the NPC's saving throw bonus for intelligence saves or use the arrows to increase/decrease the value."
	TTip.NPCwissav:= "Enter the NPC's saving throw bonus for wisdom saves or use the arrows to increase/decrease the value."
	TTip.NPCchasav:= "Enter the NPC's saving throw bonus for charisma saves or use the arrows to increase/decrease the value."
	TTip.NPC_FS_STR:= "Check here if this saving throw should be set to show as '+0' in the statblock."
	TTip.NPC_FS_DEX:= "Check here if this saving throw should be set to show as '+0' in the statblock."
	TTip.NPC_FS_CON:= "Check here if this saving throw should be set to show as '+0' in the statblock."
	TTip.NPC_FS_INT:= "Check here if this saving throw should be set to show as '+0' in the statblock."
	TTip.NPC_FS_WIS:= "Check here if this saving throw should be set to show as '+0' in the statblock."
	TTip.NPC_FS_CHA:= "Check here if this saving throw should be set to show as '+0' in the statblock."
	
	TTip.NPCblind:= "Enter the NPC's maximum blindsight range in feet (ft) or use the arrows to increase/decrease the value." Chr(10) "A value of 0 indicates the NPC has no ability with this sense."
	TTip.NPCdark:= "Enter the NPC's maximum blindsight range in feet (ft) or use the arrows to increase/decrease the value." Chr(10) "A value of 0 indicates the NPC has no ability with this sense."
	TTip.NPCtremor:= "Enter the NPC's maximum blindsight range in feet (ft) or use the arrows to increase/decrease the value." Chr(10) "A value of 0 indicates the NPC has no ability with this sense."
	TTip.NPCtrue:= "Enter the NPC's maximum blindsight range in feet (ft) or use the arrows to increase/decrease the value." Chr(10) "A value of 0 indicates the NPC has no ability with this sense."
	TTip.NPCpassperc:= "Enter the NPC's passive Perception or use the arrows to increase/decrease the value."
	TTip.NPCblindB:= "Check this box if the NPC is blind beyond the radius given."
	TTip.NPCdarkB:= "Check this box if the NPC is blind beyond the radius given."
	TTip.NPCtremorB:= "Check this box if the NPC is blind beyond the radius given."
	TTip.NPCtrueB:= "Check this box if the NPC is blind beyond the radius given."
	TTip.NPCcharat:= "Select the appropriate challenge rating for your NPC."
	TTip.NPCxp:= "This will be calculated automatically based on the challenge rating chosen; you can override it by typing a new XP value."
	TTip.ButtonImport:= "Select this button to import all the values for your NPC from a PDF, text file, or web page." Chr(10) "This will update values on ALL tabs, so ensure you have saved any work you wish to keep before doing this!"
	TTip.NPCToken:= "Select the image you wish to use as your NPC's token in Fantasy Grounds." Chr(10) "NPC Engineer will copy the token from anywhere on your system to your module's /input/tokens folder."
	TTip.ButtonTerrain:= "This opens a new dialogue box where you can choose terrain types that your creature may be found in, or state the mythology that it comes from." Chr(10) "This gives new sorting options within Fantasy Grounds."
	

;}

; Dmg Modifiers Tab
;{
	TTip.cbDV1:= "Check here if your NPC is vulnerable to acid damage."
	TTip.cbDV2:= "Check here if your NPC is vulnerable to cold damage."
	TTip.cbDV3:= "Check here if your NPC is vulnerable to fire damage."
	TTip.cbDV4:= "Check here if your NPC is vulnerable to force damage."
	TTip.cbDV5:= "Check here if your NPC is vulnerable to lightning damage."
	TTip.cbDV6:= "Check here if your NPC is vulnerable to necrotic damage."
	TTip.cbDV7:= "Check here if your NPC is vulnerable to poison damage."
	TTip.cbDV8:= "Check here if your NPC is vulnerable to psychic damage."
	TTip.cbDV9:= "Check here if your NPC is vulnerable to radiant damage."
	TTip.cbDV10:= "Check here if your NPC is vulnerable to thunder damage."
	TTip.cbDV11:= "Check here if your NPC is vulnerable to bludgeoning weapon damage."
	TTip.cbDV12:= "Check here if your NPC is vulnerable to piercing weapon damage."
	TTip.cbDV13:= "Check here if your NPC is vulnerable to slashing weapon damage."

	TTip.cbDR1:= "Check here if your NPC is resistant to acid damage."
	TTip.cbDR2:= "Check here if your NPC is resistant to cold damage."
	TTip.cbDR3:= "Check here if your NPC is resistant to fire damage."
	TTip.cbDR4:= "Check here if your NPC is resistant to force damage."
	TTip.cbDR5:= "Check here if your NPC is resistant to lightning damage."
	TTip.cbDR6:= "Check here if your NPC is resistant to necrotic damage."
	TTip.cbDR7:= "Check here if your NPC is resistant to poison damage."
	TTip.cbDR8:= "Check here if your NPC is resistant to psychic damage."
	TTip.cbDR9:= "Check here if your NPC is resistant to radiant damage."
	TTip.cbDR10:= "Check here if your NPC is resistant to thunder damage."
	TTip.cbDR11:= "Check here if your NPC is resistant to bludgeoning weapon damage."
	TTip.cbDR12:= "Check here if your NPC is resistant to piercing weapon damage."
	TTip.cbDR13:= "Check here if your NPC is resistant to slashing weapon damage."

	TTip.DRRadio1:= "Select this option if your NPC has no special weapon resistances beyond those noted above."
	TTip.DRRadio2:= "Select this option if your NPC is resistant to nonmagical weapons."
	TTip.DRRadio3:= "Select this option if your NPC is resistant to nonmagical weapons that haven't been silvered."
	TTip.DRRadio4:= "Select this option if your NPC is resistant to nonmagical weapons that aren't made of adamantine."
	TTip.DRRadio5:= "Select this option if your NPC is resistant to magical weapons."
	TTip.DRRadio6:= "Select this option if your NPC is resistant to nonmagical weapons that aren't made of cold-forged iron."

	TTip.cbDI1:= "Check here if your NPC is immune to acid damage."
	TTip.cbDI2:= "Check here if your NPC is immune to cold damage."
	TTip.cbDI3:= "Check here if your NPC is immune to fire damage."
	TTip.cbDI4:= "Check here if your NPC is immune to force damage."
	TTip.cbDI5:= "Check here if your NPC is immune to lightning damage."
	TTip.cbDI6:= "Check here if your NPC is immune to necrotic damage."
	TTip.cbDI7:= "Check here if your NPC is immune to poison damage."
	TTip.cbDI8:= "Check here if your NPC is immune to psychic damage."
	TTip.cbDI9:= "Check here if your NPC is immune to radiant damage."
	TTip.cbDI10:= "Check here if your NPC is immune to thunder damage."
	TTip.cbDI11:= "Check here if your NPC is immune to bludgeoning weapon damage."
	TTip.cbDI12:= "Check here if your NPC is immune to piercing weapon damage."
	TTip.cbDI13:= "Check here if your NPC is immune to slashing weapon damage."

	TTip.DIRadio1:= "Select this option if your NPC has no special weapon immunities beyond those noted above."
	TTip.DIRadio2:= "Select this option if your NPC is immune to nonmagical weapons."
	TTip.DIRadio3:= "Select this option if your NPC is immune to nonmagical weapons that haven't been silvered."
	TTip.DIRadio4:= "Select this option if your NPC is immune to nonmagical weapons that aren't made of adamantine."
	TTip.DIRadio5:= "Select this option if your NPC is immune to nonmagical weapons that aren't made of cold-forged iron."

	TTip.cbCI1:= "Check here if your NPC cannot be blinded."
	TTip.cbCI2:= "Check here if your NPC cannot be charmed."
	TTip.cbCI3:= "Check here if your NPC cannot be deafened."
	TTip.cbCI4:= "Check here if your NPC cannot suffer from exhaustion."
	TTip.cbCI5:= "Check here if your NPC cannot be frightened."
	TTip.cbCI6:= "Check here if your NPC cannot be grappled."
	TTip.cbCI7:= "Check here if your NPC cannot be incapacitated."
	TTip.cbCI8:= "Check here if your NPC cannot be made invisible."
	TTip.cbCI9:= "Check here if your NPC cannot be paralyzed."
	TTip.cbCI10:= "Check here if your NPC cannot be petrified."
	TTip.cbCI11:= "Check here if your NPC cannot be poisoned."
	TTip.cbCI12:= "Check here if your NPC cannot be knocked prone."
	TTip.cbCI13:= "Check here if your NPC cannot be restrained."
	TTip.cbCI14:= "Check here if your NPC cannot be stunned."
	TTip.cbCI15:= "Check here if your NPC cannot be rendered unconscious."
	TTip.cbCI16:= "Check here if your NPC is immune to any other condition (type it in the box)."
	TTip.CI16:= "Type in the name of any other condition that your NPC is immune to."
;}

; Skills Tab
;{
	TTip.sk_acro:= "Enter the NPC's Acrobatics skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_anim:= "Enter the NPC's Animal Handling skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_arca:= "Enter the NPC's Arcana skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_athl:= "Enter the NPC's Athletics skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_dece:= "Enter the NPC's Deception skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_hist:= "Enter the NPC's History skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_insi:= "Enter the NPC's Insight skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_inti:= "Enter the NPC's Intimidation skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_inve:= "Enter the NPC's Investigation skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_medi:= "Enter the NPC's Medicine skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_natu:= "Enter the NPC's Nature skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_perc:= "Enter the NPC's Perception skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_perf:= "Enter the NPC's Performance skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_pers:= "Enter the NPC's Persuasion skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_reli:= "Enter the NPC's Religion skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_slei:= "Enter the NPC's Sleight of Hand skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_stea:= "Enter the NPC's Stealth skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."
	TTip.sk_surv:= "Enter the NPC's Survival skill score or use the arrows to increase/decrease the value." Chr(10) "The range is -20 to 35." Chr(10) "Leave set at zero if the NPC doesn't have the skill."

	TTip.Lang1:= "Select the standard languages that your NPC knows." Chr(10) "You can select as many as you like."
	TTip.Lang2:= "Select the exotic languages that your NPC knows." Chr(10) "You can select as many as you like."
	TTip.Lang3:= "Select the monstrous languages that your NPC knows." Chr(10) "You can select as many as you like."
	TTip.UserLangs:= "Select the user-defined languages that your NPC knows." Chr(10) "To modify this list, select 'Options|Manage language list' from the menu."
	TTip.LangSelect:= "Select any special options for your NPC's language skills." Chr(10) "Choosing 'No special conditions' uses the languages selected above."
	
	TTip.LangAlt:= "Fill in any text you want to use for the language skill in place of the standard texts above."
	TTip.NPCtelep:= "Check this box if your NPC has Telepathy."
	TTip.telrange:= "Fill in the range (and unit) of your NPC's Telepathy." Chr(10) "For example: 150 ft. or 5 miles."
;}

; Traits Tab
;{
	TTip.Traitname1:= "The name of the NPC's first Trait."
	TTip.Traitname2:= "The name of the NPC's second Trait."
	TTip.Traitname3:= "The name of the NPC's third Trait."
	TTip.Traitname4:= "The name of the NPC's fourth Trait."
	TTip.Traitname5:= "The name of the NPC's fifth Trait."
	TTip.Traitname6:= "The name of the NPC's sixth Trait."
	TTip.Trait1:= "The descriptive text for the NPC's first Trait."
	TTip.Trait2:= "The descriptive text for the NPC's second Trait."
	TTip.Trait3:= "The descriptive text for the NPC's third Trait."
	TTip.Trait4:= "The descriptive text for the NPC's fourth Trait."
	TTip.Trait5:= "The descriptive text for the NPC's fifth Trait."
	TTip.Trait6:= "The descriptive text for the NPC's sixth Trait."
	TTip.TEB1:= "Edit this Trait below."
	TTip.TEB2:= "Edit this Trait below."
	TTip.TEB3:= "Edit this Trait below."
	TTip.TEB4:= "Edit this Trait below."
	TTip.TEB5:= "Edit this Trait below."
	TTip.TEB6:= "Edit this Trait below."
	TTip.TDB1:= "Delete this Trait."
	TTip.TDB2:= "Delete this Trait."
	TTip.TDB3:= "Delete this Trait."
	TTip.TDB4:= "Delete this Trait."
	TTip.TDB5:= "Delete this Trait."
	TTip.TDB6:= "Delete this Trait."
	TTip.TraitnameNew:= "Enter a name for the new or edited Trait. This should be short."
	TTip.TraitNew:= "Enter the descriptive text for the new or edited Trait."
	TTip.ButtonAddTrait:= "Add this Trait to the NPC's Trait List, or update it if it already exists."
;}

; Innate Casting Tab
;{ 
	TTip.FlagInSpell:= "Check this box to include an 'innate spellcasting' section in your NPC." Chr(10) "Uncheck it to have no 'innate spellcasting' section."
	TTip.NPCPsionics:= "Check this box to mark this section as 'innate spellcasting (Psionics)' for your NPC."
	TTip.NPCinspability:= "Select the ability that is your NPC's innate spellcasting ability."
	TTip.NPCinspsave:= "Enter the NPC's spell save difficulty check score or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30."
	TTip.NPCinsptext:= "Enter information about the components required to cast this spell."
	TTip.NPCinsptohit:= "Enter the NPC's spell 'to hit' bonus if appropriate or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30." Chr(10) "Leave at zero if the NPC should not have a 'to hit' bonus mentioned."
	TTip.InSp_0_spells:= "Type the name of the spells the NPC can use at will (ie as often as required)." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.InSp_1_spells:= "Type the name of the spells the NPC can use once each day." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.InSp_2_spells:= "Type the name of the spells the NPC can use twice each day." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.InSp_3_spells:= "Type the name of the spells the NPC can use three times each day." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.InSp_4_spells:= "Type the name of the spells the NPC can use four times each day." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.InSp_5_spells:= "Type the name of the spells the NPC can use five times each day." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
;}

; Spellcasting Tab
;{
	TTip.FlagSpell:= "Check this box to include a 'spellcasting' section in your NPC." Chr(10) "Uncheck it to have no 'spellcasting' section."
	TTip.NPCsplevel:= "Select the NPC's caster level."
	TTip.NPCspability:= "Select the ability that is your NPC's spellcasting ability."
	TTip.NPCspsave:= "Enter the NPC's spell save difficulty check score or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30."
	TTip.NPCsptohit:= "Enter the NPC's spell 'to hit' bonus if appropriate or use the arrows to increase/decrease the value." Chr(10) "The range is 0 to 30."
	TTip.NPCspclass:= "Select the class of spells that your NPC is able to cast."
	TTip.NPCspflavour:= "Enter any flavour text about your NPC's casting. This is not essential and can be left blank."
	TTip.Sp_0_casts:= "Choose how many spell slots the NPC has at this level. For cantrips, this is usually 'at will' (meaning 'as many as required')."
	TTip.Sp_1_casts:= "Choose how many spell slots the NPC has at 1st level."
	TTip.Sp_2_casts:= "Choose how many spell slots the NPC has at 2nd level."
	TTip.Sp_3_casts:= "Choose how many spell slots the NPC has at 3rd level."
	TTip.Sp_4_casts:= "Choose how many spell slots the NPC has at 4th level."
	TTip.Sp_5_casts:= "Choose how many spell slots the NPC has at 5th level."
	TTip.Sp_6_casts:= "Choose how many spell slots the NPC has at 6th level."
	TTip.Sp_7_casts:= "Choose how many spell slots the NPC has at 7th level."
	TTip.Sp_8_casts:= "Choose how many spell slots the NPC has at 8th level."
	TTip.Sp_9_casts:= "Choose how many spell slots the NPC has at 9th level."
	TTip.Sp_0_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.Sp_1_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.Sp_2_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.Sp_3_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.Sp_4_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.Sp_5_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.Sp_6_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.Sp_7_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.Sp_8_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.Sp_9_spells:= "Type the name of the spells the NPC can use at this level." Chr(10) "Separate spells in your list with a comma and then a space to ensure Fantasy Grounds pickes them up properly." Chr(10) "(e.g. Magic Missile, Light, Shield)"
	TTip.NPCSpellStar:= "If you have any spells marked with an asterisk (*), the footnote goes here." Chr(10) "This is not essential and can be left blank." Chr(10) "NPCE will enforce an asterisk at the start."
;}

; Actions Tab
;{
	TTip.actionnameB1:= "The name of the NPC's first Action. This is not editable here, although you can delete it."
	TTip.actionnameB2:= "The name of the NPC's second Action. This is not editable here, although you can delete it."
	TTip.actionnameB3:= "The name of the NPC's third Action. This is not editable here, although you can delete it."
	TTip.actionnameB4:= "The name of the NPC's fourth Action. This is not editable here, although you can delete it."
	TTip.actionnameB5:= "The name of the NPC's fifth Action. This is not editable here, although you can delete it."
	TTip.actionnameB6:= "The name of the NPC's sixth Action. This is not editable here, although you can delete it."
	TTip.actionnameB7:= "The name of the NPC's seventh Action. This is not editable here, although you can delete it."
	TTip.actionnameB8:= "The name of the NPC's eighth Action. This is not editable here, although you can delete it."
	TTip.actionnameB9:= "The name of the NPC's ninth Action. This is not editable here, although you can delete it."
	TTip.actionnameB10:= "The name of the NPC's tenth Action. This is not editable here, although you can delete it."
	TTip.actionnameB11:= "The name of the NPC's eleventh Action. This is not editable here, although you can delete it."
	TTip.actionB1:= "The descriptive text for the NPC's first Action. This is not editable here, although you can delete it."
	TTip.actionB2:= "The descriptive text for the NPC's second Action. This is not editable here, although you can delete it."
	TTip.actionB3:= "The descriptive text for the NPC's third Action. This is not editable here, although you can delete it."
	TTip.actionB4:= "The descriptive text for the NPC's fourth Action. This is not editable here, although you can delete it."
	TTip.actionB5:= "The descriptive text for the NPC's fifth Action. This is not editable here, although you can delete it."
	TTip.actionB6:= "The descriptive text for the NPC's sixth Action. This is not editable here, although you can delete it."
	TTip.actionB7:= "The descriptive text for the NPC's seventh Action. This is not editable here, although you can delete it."
	TTip.actionB8:= "The descriptive text for the NPC's eighth Action. This is not editable here, although you can delete it."
	TTip.actionB9:= "The descriptive text for the NPC's ninth Action. This is not editable here, although you can delete it."
	TTip.actionB10:= "The descriptive text for the NPC's tenth Action. This is not editable here, although you can delete it."
	TTip.actionB11:= "The descriptive text for the NPC's eleventh Action. This is not editable here, although you can delete it."
	TTip.ActDB1:= "Delete this Action from the list."
	TTip.ActDB2:= "Delete this Action from the list."
	TTip.ActDB3:= "Delete this Action from the list."
	TTip.ActDB4:= "Delete this Action from the list."
	TTip.ActDB5:= "Delete this Action from the list."
	TTip.ActDB6:= "Delete this Action from the list."
	TTip.ActDB7:= "Delete this Action from the list."
	TTip.ActDB8:= "Delete this Action from the list."
	TTip.ActDB9:= "Delete this Action from the list."
	TTip.ActDB10:= "Delete this Action from the list."
	TTip.ActDB11:= "Delete this Action from the list."
	TTip.ButtonAddAction:= "Click here to see a full list of Actions, edit them, and add new ones."
	TTip.reactionnameB1:= "The name of the NPC's first Reaction, if it has any. This is not editable here, although you can delete it."
	TTip.reactionnameB2:= "The name of the NPC's second Reaction, if it has any. This is not editable here, although you can delete it."
	TTip.reactionnameB3:= "The name of the NPC's third Reaction, if it has any. This is not editable here, although you can delete it."
	TTip.reactionnameB4:= "The name of the NPC's fourth Reaction, if it has any. This is not editable here, although you can delete it."
	TTip.ReActDB1:= "Delete this Reaction from the list."
	TTip.ReActDB2:= "Delete this Reaction from the list."
	TTip.ReActDB3:= "Delete this Reaction from the list."
	TTip.ReActDB4:= "Delete this Reaction from the list."
	TTip.ButtonAddReAction:= "Click here to see a full list of Reactions, edit them, and add new ones."
	TTip.lgactionnameB1:= "The name of the NPC's first Legendary Action, if it has any. This is not editable here, although you can delete it."
	TTip.lgactionnameB2:= "The name of the NPC's second Legendary Action, if it has any. This is not editable here, although you can delete it."
	TTip.lgactionnameB3:= "The name of the NPC's third Legendary Action, if it has any. This is not editable here, although you can delete it."
	TTip.lgactionnameB4:= "The name of the NPC's fourth Legendary Action, if it has any. This is not editable here, although you can delete it."
	TTip.lgactionnameB5:= "The name of the NPC's fifth Legendary Action, if it has any. This is not editable here, although you can delete it."
	TTip.lgactionnameB6:= "The name of the NPC's sixth Legendary Action, if it has any. This is not editable here, although you can delete it."
	TTip.lgactionnameB7:= "The name of the NPC's seventh Legendary Action, if it has any. This is not editable here, although you can delete it."
	TTip.lgactDB1:= "Delete this Legendary Action from the list."
	TTip.lgactDB2:= "Delete this Legendary Action from the list."
	TTip.lgactDB3:= "Delete this Legendary Action from the list."
	TTip.lgactDB4:= "Delete this Legendary Action from the list."
	TTip.lgactDB5:= "Delete this Legendary Action from the list."
	TTip.lgactDB6:= "Delete this Legendary Action from the list."
	TTip.lgactDB7:= "Delete this Legendary Action from the list."
	TTip.ButtonAddlgaction:= "Click here to see a full list of Legendary Actions, edit them, and add new ones."
	TTip.lractionnameB1:= "The name of the NPC's first Lair Action, if it has any."
	TTip.lractionnameB2:= "The name of the NPC's second Lair Action, if it has any."
	TTip.lractionnameB3:= "The name of the NPC's third Lair Action, if it has any."
	TTip.lractionnameB4:= "The name of the NPC's fourth Lair Action, if it has any."
	TTip.lractionnameB5:= "The name of the NPC's fifth Lair Action, if it has any."
	TTip.lractionnameB6:= "The name of the NPC's sixth Lair Action, if it has any."
	TTip.lractionnameB7:= "The name of the NPC's seventh Lair Action, if it has any."
	TTip.lractDB1:= "Delete this Lair Action from the list."
	TTip.lractDB2:= "Delete this Lair Action from the list."
	TTip.lractDB3:= "Delete this Lair Action from the list."
	TTip.lractDB4:= "Delete this Lair Action from the list."
	TTip.lractDB5:= "Delete this Lair Action from the list."
	TTip.lractDB6:= "Delete this Lair Action from the list."
	TTip.lractDB7:= "Delete this Lair Action from the list."
	TTip.ButtonAddlraction:= "Click here to see a full list of Lair Actions, edit them, and add new ones."
;}

; Description Tab
;{
	TTip.Desc_Add_Text:= "Include markup and any descriptive text in the edit box."
	TTip.Desc_NPC_Title:= "Add the markup for a title (the NPC's name) at the start of the description section."
	TTip.Desc_Spell_List:= "If the NPC is a caster, have a formatted list of its spells in the description section in addition to where it normally appears."
	TTip.Chosen_Desc_Text:= "This box contains the formatted descriptive text that will be added to your NPC."
	TTip.ButtonDesctextDelete:= "Clear all text from the window above." Chr(10) "This is not recoverable; be sure you want to do this!"
	TTip.NPCE_Paste:= "Add any text from the clipboard to the 'edit' window at the cursor position."
	TTip.BTSTB:= "Make the selected text bold."
	TTip.BTSTI:= "Make the selected text italicised."
	TTip.BTSTU:= "Make the selected text underlined."
	TTip.BTSTH:= "Format the selected text as a header."
	TTip.BTSTa:= "Format the selected text as body text."
	TTip.BTSTF:= "Format the selected paragraph(s) as 'Chat Frame Text' in Fantasy Grounds. This will indent the paragraph(s) in NPC Engineer as a visual cue that they have been modified." Chr(10) "Note that if two or more consecutive paragraphs are selected, Fantasy Grounds will treat it as a single continuous paragraph."
	TTip.BTSTL:= "Format the selected text a bulleted list."
	TTip.BTSTZ:= "Undo action."
	TTip.BTSTY:= "Redo action."
	TTip.BTSTC:= "Toggle the background colour between white and a colour similar to Fantasy Ground's "
	TTip.Desc_fixes:= "Check to apply common fixes to any text pasted in to the edit box."
	TTip.Desc_strip_lf:= "Check to strip extra 'new line' codes on pasting text from a PDF."
	TTip.Desc_title:= "Check to pick out titles in the text being pasted." Chr(10) "Any short sentence (4 words or less) at the start of a paragraph will be tagged as a title."
	TTip.NPCnoid:= "(OPTIONAL) Any text put here will show up instead of the NPC's name until the GM ID's the NPC in Fantasy Grounds." Chr(10) "If this is blank, the NPC's name will always show."	
;}

; Image Tab
;{
	TTip.Desc_Image_Link:= "Add the markup and link for an image with the NPC's name." Chr(10) "This is switched on automatically if you load an image, but can be turned off again."
	TTip.NPCImage:= "Select the image you wish to use for your NPC." Chr(10) "This will show up on the 'Other' tab in Fantasy Grounds and in the Reference Manual page." Chr(10) "NPC Engineer will copy this image from anywhere on your system to your module's /input/images folder."
	TTip.NPCImArt:= "Enter the artist's name here." Chr(10) "It will be shown on the Reference Manual page for the NPC."
	TTip.NPCImLink:= "Enter the artist's email or web address here." Chr(10) "It will be shown on the Reference Manual page for the NPC."
	TTip.NpcClearImage:= "Clear the image file."
;}

; Main Window
;{
	TTip.Final_Text:= "This window shows how NPC Engineer's output will look." Chr(10) "If you see issues here, you can apply fixes within the tabs to the left."
	TTip.Output_to_Clipboard:= "Copy all text from the output box to the clipboard, ready for pasting in another application."
	TTip.ButtonSBClipboard:= "Copy the graphical statblock to the clipboard, ready for pasting in another application."
	TTip.Output_to_Text:= "Save all information from the output box as a plain text file."
	TTip.Output_Append:= "Append all information from the output box to an existing file." Chr(10) "This allows the build-up of a 'bestiary file' for parsing into Fantasy Grounds."

;}

; Import Window
;{
	TTip.Fix_Text:= "This window contains the processed text from the left. Many fixes have been applied." Chr(10) "This shows how NPC Engineer's output will look." Chr(10) "If you see issues here, you can apply fixes to the raw text in the left window."
	TTip.NPCE_Import_Delete:= "Clear all text from the window above." Chr(10) "This is not recoverable; be sure you want to do this!"
	TTip.NPCE_Import_Append:= "Add any text from the clipboard to the 'edit' window at the cursor position."
	TTip.NPCE_Import_Return:= "Accept the text in the right-hand window, and return to the main NPC Engineer window." Chr(10) "All values will be placed in their appropriate place."
	TTip.NPCE_Import_Cancel:= "Cancel all editing and return to the main NPC Engineer window." Chr(10) "No changes will be made to the variables."
	TTip.ImportChoice:= "Select the source of your copied text." Chr(10) "Unless you know for sure that you are using one of the others, stick to the default option."
	TTip.NPCE_Import_ReturnDescript:= "Accept the text in the right-hand window, and move on to the Descriptive Text window." Chr(10) "All values from here will be placed in their appropriate place in the main window."
;}

; Project Window
;{
	TTip.NPCE_Import_Delete:= "Clear all text from the window above." Chr(10) "This is not recoverable; be sure you want to do this!"
	TTip.ModName:= "Type in the title of your module." Chr(10) "(eg. Baleful Bestiary)" Chr(10) "THIS IS REQUIRED TO CREATE A MODULE."
	TTip.ModCate:= "Type in the Category you wish Fantasy Grounds to display your module under." Chr(10) "(eg. Core Rules, Supplement, Adventure)"
	TTip.ModAuth:= "Type in the author or publisher."
	TTip.ModFile:= "Type in the filename for the module (*******.mod)." Chr(10) "This can be the same as the module title if you wish." Chr(10) "THIS IS REQUIRED TO CREATE A MODULE."
	TTip.ModGmon:= "Check this box if the module is intended to be read only by the GM."
	TTip.ModLock:= "Check this box if the module should be locked to prevent editing." Chr(10) "LOCKED records cannot be edited, and won't pull in information automatically (spells, for instance) until added to the campaign." Chr(10) "UNLOCKED records can be edited and will pull in information from elsewhere, but this will be the master record that is being edited."
	TTip.AddPath:= "Check this box if you wish to add the module title to the end of the path below." Chr(10) "This is useful for creating new Projects."
	TTip.ModPath:= "Type in the path you wish to use to save your project." Chr(10) "In most cases, it will be simpler to use the button to the right." Chr(10) "THIS IS REQUIRED TO CREATE A MODULE."
	TTip.FGPath:= "This should show the path for your Fantasy Grounds installation." Chr(10) "You can use the button to the right to correct this if you wish." Chr(10) "Note that this isn't used yet; it is in place for when parsing functions get added."
	TTip.MPB1:= "Bring up a folder selection window to select the module path."
	TTip.MPB2:= "Bring up a folder selection window to select the Fantasy Grounds module path."
	TTip.ModThum:= "Select an image anywhere on your system to use as the thumbnail for your module." Chr(10) "FG documentation recommends 100 x 100 pixels." Chr(10) "Only PNG files can be used in this version of NPC Engineer." Chr(10) "The file will be copied to the Project folder when Create Project is clicked."
	TTip.Mod_Parser:= "Choose the parser you wish to use for this project." Chr(10) "NPC Engineer will change its output to suit the parser chosen." 
	TTip.ModImage:= "Check this box to include images when parsing your Project." Chr(10) "The images folder will always be created."
	TTip.ModToken:= "Check this box to include NPC tokens when parsing your Project." Chr(10) "The tokens folder will always be created."
	TTip.ModMonst:= "Check this box to include NPCs / monsters when parsing your Project." Chr(10) "The npcs.txt file will only be created if this box is ticked."
	TTip.ModRaces:= "Not yet implemented."
	TTip.ModClass:= "Not yet implemented."
	TTip.ModBackg:= "Not yet implemented."
	TTip.ModFeats:= "Not yet implemented."
	TTip.ModSkill:= "Not yet implemented."
	TTip.ModSpell:= "Not yet implemented."
	TTip.ModEquip:= "Not yet implemented."
	TTip.ModMItem:= "Not yet implemented."
	TTip.ModEncou:= "Not yet implemented."
	TTip.ModRanEn:= "Not yet implemented."
	TTip.ModQuest:= "Not yet implemented."
	TTip.ModStory:= "Not yet implemented."
	TTip.ModStTem:= "Not yet implemented."
	TTip.ModParcl:= "Not yet implemented."
	TTip.ModIPins:= "Not yet implemented."
	TTip.ModIGrid:= "Not yet implemented."
	TTip.ModPregn:= "Not yet implemented."
	TTip.ModRefMn:= "Not yet implemented."
	TTip.ModTable:= "Not yet implemented."
	TTip.NPCE_Project_Close:= "Return to the main NPC Engineer window with the current information active."
;}

; Actions Window
;{
	TTip.WA_Name:= "Insert the name of the weapon/natural weapon here." Chr(10) "For example: 'Shortsword' or 'Claws'."
	TTip.WA_Type:= "Is this a melee or ranged attack, or can it be used as both?"
	TTip.WA_ToHit:= "This is the bonus to the 'to hit' roll for this attack."
	TTip.WA_Reach:= "This is the weapon's reach if it is a melee attack." Chr(10) "The usual value is 5 feet."
	TTip.WA_Rnorm:= "This is the limit of a ranged weapon's normal range."
	TTip.WA_Rlong:= "This is the limit of a ranged weapon's long range." Chr(10) "Long range gives disadvantage on the attack roll."
	TTip.WA_Target:= "Choose the target type for your weapon attack."
	TTip.WA_NoDice:= "How many damage dice of the type selected should be rolled?"
	TTip.WA_Dice:= "Which type of dice should be rolled for damage?"
	TTip.WA_DamBon:= "Are there any bonuses to be added to the damage?"
	TTip.WA_DamType:= "Select the type of damage delivered by your action."

	TTip.weapon_attack_Text:= "This shows how your compiled weapon attack will look, allowing you to tweak it before you add it."

	TTip.Multi_attack:= "Check this box if your NPC has the 'multiattack' Action." Chr(10) "This will add the text below as the multiattack text." Chr(10) "Unchecking this box will remove the 'multiattack' Action from the list."
	TTip.multi_attack_Text:= "This is the descriptive text for the 'multiattack' Action."
	TTip.Action_Multiattack:= "Click this button to update changed multiattack text."

	TTip.WA_Magic:= "Check this box if the weapon attack is magical."
	TTip.WA_Silver:= "Check this box if the weapon is silvered or made of silver."
	TTip.WA_Adaman:= "Check this box if the weapon is adamantine."
	TTip.WA_cfiron:= "Check this box if the weapon is made from cold-forged iron."
	TTip.WA_BonAdd:= "Check this box if your weapon attack has bonus damage." Chr(10) "For example '1d8 slashing + 1d6 radiant.'"
	TTip.WA_BonNoDice:= "How many damage dice of the type selected should be rolled?"
	TTip.WA_BonDice:= "Which type of dice should be rolled for damage?"
	TTip.WA_BonDamBon:= "Are there any bonuses to be added to the damage?"
	TTip.WA_BonDamType:= "Select the type of damage delivered by your bonus damage."
	
	TTip.WA_OtherTextAdd:= "Check this box if your weapon attack has extra important text."
	TTip.WA_OtherText:= "Enter the important text here." Chr(10) "For example 'Target must succeed on a DC 12 Constitution saving throw or be cursed with lycanthropy.'"
	
	TTip.ActEB1:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB2:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB3:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB4:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB5:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB6:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB7:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB8:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB9:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB10:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ActEB11:= "Click this button to edit your Action." Chr(10) "It's information will appear in the appropriate box to the right."

	TTip.ActHB1:= "Click to move this Action one place up the list."
	TTip.ActHB2:= "Click to move this Action one place up the list."
	TTip.ActHB3:= "Click to move this Action one place up the list."
	TTip.ActHB4:= "Click to move this Action one place up the list."
	TTip.ActHB5:= "Click to move this Action one place up the list."
	TTip.ActHB6:= "Click to move this Action one place up the list."
	TTip.ActHB7:= "Click to move this Action one place up the list."
	TTip.ActHB8:= "Click to move this Action one place up the list."
	TTip.ActHB9:= "Click to move this Action one place up the list."
	TTip.ActHB10:= "Click to move this Action one place up the list."
	TTip.ActHB11:= "Click to move this Action one place up the list."

	TTip.ActLB1:= "Click to move this Action one place down the list."
	TTip.ActLB2:= "Click to move this Action one place down the list."
	TTip.ActLB3:= "Click to move this Action one place down the list."
	TTip.ActLB4:= "Click to move this Action one place down the list."
	TTip.ActLB5:= "Click to move this Action one place down the list."
	TTip.ActLB6:= "Click to move this Action one place down the list."
	TTip.ActLB7:= "Click to move this Action one place down the list."
	TTip.ActLB8:= "Click to move this Action one place down the list."
	TTip.ActLB9:= "Click to move this Action one place down the list."
	TTip.ActLB10:= "Click to move this Action one place down the list."
	TTip.ActLB11:= "Click to move this Action one place down the list."
	
	TTip.OtherActionName:= "Input a name for your Action."
	TTip.OtherActionText:= "Input the descriptive text for your Action."

	TTip.NPCE_Actions_Close:= "Accept the Actions list as it is and return to the main NPC Engineer window."
	TTip.Action_Weaponattack:= "Click this button to add the weapon attack to the list of Actions." Chr(10) "If you add the same weapon attack more than once, it will appear more than once. This is by design - take care when adding!"
	TTip.Action_Other:= "Click this button to add the other Action to the Action list." Chr(10) "If the Action already exists, its text will be updated."
;}

; Reactions Window
;{
	TTip.reactionB1:= "The descriptive text for the NPC's first Reaction. This is not editable here, although you can delete it."
	TTip.reactionB2:= "The descriptive text for the NPC's second Reaction. This is not editable here, although you can delete it."
	TTip.reactionB3:= "The descriptive text for the NPC's third Reaction. This is not editable here, although you can delete it."
	TTip.reactionB4:= "The descriptive text for the NPC's fourth Reaction. This is not editable here, although you can delete it."
	TTip.OtherReActionName:= "Input a name for your Reaction."
	TTip.OtherReActionText:= "Input the descriptive text for your Reaction."
	TTip.ReAction_Other:= "Click this button to add the Reaction to the Reaction list." Chr(10) "If the Reaction already exists, its text will be updated."
	TTip.NPCE_Reactions_Close:= "Accept the Reactions list as it is and return to the main NPC Engineer window."

	TTip.ReActEB1:= "Click this button to edit your Reaction." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ReActEB2:= "Click this button to edit your Reaction." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ReActEB3:= "Click this button to edit your Reaction." Chr(10) "It's information will appear in the appropriate box to the right."
	TTip.ReActEB4:= "Click this button to edit your Reaction." Chr(10) "It's information will appear in the appropriate box to the right."

	TTip.ReActHB2:= "Click to move this Reaction one place up the list."
	TTip.ReActHB3:= "Click to move this Reaction one place up the list."
	TTip.ReActHB4:= "Click to move this Reaction one place up the list."

	TTip.ReActLB1:= "Click to move this Reaction one place down the list."
	TTip.ReActLB2:= "Click to move this Reaction one place down the list."
	TTip.ReActLB3:= "Click to move this Reaction one place down the list."
	TTip.ReActLB4:= "Click to move this Reaction one place down the list."
;}

; Legendary Actions Window
;{
	TTip.lgactionB1:= "The descriptive text for the NPC's first Legendary Action. This is not editable here, although you can delete it."
	TTip.lgactionB2:= "The descriptive text for the NPC's second Legendary Action. This is not editable here, although you can delete it."
	TTip.lgactionB3:= "The descriptive text for the NPC's third Legendary Action. This is not editable here, although you can delete it."
	TTip.lgactionB4:= "The descriptive text for the NPC's fourth Legendary Action. This is not editable here, although you can delete it."
	TTip.lgactionB5:= "The descriptive text for the NPC's fifth Legendary Action. This is not editable here, although you can delete it."
	TTip.lgactionB6:= "The descriptive text for the NPC's sixth Legendary Action. This is not editable here, although you can delete it."
	TTip.lgactionB7:= "The descriptive text for the NPC's seventh Legendary Action. This is not editable here, although you can delete it."
	TTip.LgActionOptions:= "Type in the Options text for the NPC's Legendary Actions."
	TTip.LgAction_Options:= "Accept the Options text and add it to the list of Legendary Actions."
	TTip.OtherLgActionName:= "Input a name for your Legendary Action."
	TTip.OtherLgActionText:= "Input the descriptive text for your Legendary Action."
	TTip.LgAction_Other:= "Click this button to add the Legendary Action to the Legendary Action list." Chr(10) "If the Legendary Action already exists, its text will be updated."
	TTip.NPCE_LegActions_Close:= "Accept the Legendary Actions list as it is and return to the main NPC Engineer window."

	TTip.LgActHB2:= "Click to move this Legendary Action one place up the list."
	TTip.LgActHB3:= "Click to move this Legendary Action one place up the list."
	TTip.LgActHB4:= "Click to move this Legendary Action one place up the list."
	TTip.LgActHB5:= "Click to move this Legendary Action one place up the list."
	TTip.LgActHB6:= "Click to move this Legendary Action one place up the list."
	TTip.LgActHB7:= "Click to move this Legendary Action one place up the list."

	TTip.LgActLB1:= "Click to move this Legendary Action one place down the list."
	TTip.LgActLB2:= "Click to move this Legendary Action one place down the list."
	TTip.LgActLB3:= "Click to move this Legendary Action one place down the list."
	TTip.LgActLB4:= "Click to move this Legendary Action one place down the list."
	TTip.LgActLB5:= "Click to move this Legendary Action one place down the list."
	TTip.LgActLB6:= "Click to move this Legendary Action one place down the list."
	TTip.LgActLB7:= "Click to move this Legendary Action one place down the list."
;}

; Lair Actions Window
;{
	TTip.lractionB1:= "The descriptive text for the NPC's first Lair Action. You can edit this if needed."
	TTip.lractionB2:= "The descriptive text for the NPC's first Lair Action. You can edit this if needed."
	TTip.lractionB3:= "The descriptive text for the NPC's first Lair Action. You can edit this if needed."
	TTip.lractionB4:= "The descriptive text for the NPC's first Lair Action. You can edit this if needed."
	TTip.lractionB5:= "The descriptive text for the NPC's first Lair Action. You can edit this if needed."
	TTip.lractionB6:= "The descriptive text for the NPC's first Lair Action. You can edit this if needed."
	TTip.lractionB7:= "The descriptive text for the NPC's first Lair Action. You can edit this if needed."
	TTip.LrAction_Options:= "Type in the Options text for the NPC's Lair Actions."
	TTip.OtherLrActionName:= "Input a name for your new Lair Action."
	TTip.OtherLrActionText:= "Input the descriptive text for your new Lair Action."
	TTip.LrAct_Other:= "Click this button to add the Lair Action to the Lair Action list." Chr(10) "If the Lair Action already exists, its text will be updated."
	TTip.NPCE_LairActions_Close:= "Accept the Lair Actions list as it is and return to the main NPC Engineer window."

	TTip.LrActHB2:= "Click to move this Legendary Action one place up the list."
	TTip.LrActHB3:= "Click to move this Legendary Action one place up the list."
	TTip.LrActHB4:= "Click to move this Legendary Action one place up the list."
	TTip.LrActHB5:= "Click to move this Legendary Action one place up the list."
	TTip.LrActHB6:= "Click to move this Legendary Action one place up the list."
	TTip.LrActHB7:= "Click to move this Legendary Action one place up the list."

	TTip.LrActLB1:= "Click to move this Legendary Action one place down the list."
	TTip.LrActLB2:= "Click to move this Legendary Action one place down the list."
	TTip.LrActLB3:= "Click to move this Legendary Action one place down the list."
	TTip.LrActLB4:= "Click to move this Legendary Action one place down the list."
	TTip.LrActLB5:= "Click to move this Legendary Action one place down the list."
	TTip.LrActLB6:= "Click to move this Legendary Action one place down the list."
	TTip.LrActLB7:= "Click to move this Legendary Action one place down the list."
;}

; HP Window
;{
	TTip.HPno:= "How many hit dice of the type selected should be rolled?"
	TTip.HPdi:= "Which type of dice should be rolled for hit points?"
	TTip.HPbo:= "Are there any bonuses to be added to the hit points?"
	TTip.HPstring:= "This shows how your hit point expression will look, allowing you to teak it before you add it."
	TTip.NPCE_HP_Return:= "Accept your hit point expression and return to the main NPC window."
	TTip.NPCE_HP_Cancel:= "Cancel all changes and close the window."
;}

; Edit JSON Window
;{
	TTip.JSONChoose:= "Select the NPC from your JSON file that you wish to edit or delete." Chr(10) "(This is your npcs.json file - the file used by the parser to create a module)."
	TTip.JSONselected:= "This box gives brief information about your chosen NPC."
	TTip.NPCE_JSON_Del:= "Delete the NPC from the JSON file. THIS IS PERMANENT AND UNRECOVERABLE!"
	TTip.NPCE_JSON_Edit:= "Edit your NPC in NPC Engineer." Chr(10) "Remember to add it to the project again after editing!"
	TTip.NPCE_JSON_Cancel:= "Close this window and return to the main NPC window."
	TTip.Delete_Weapon:= "Delete the current weapon from your list."
	TTip.Add_Weapon:= "Add this weapon to your list."
	TTip.NPCE_Weapons_Close:= "Close this window and return to the main NPC window."
;}

; Edit Languages Window
;{
	TTip.LangDelList:= "Select the language you wish to delete." Chr(10) "You must delete them one at a time."
	TTip.NewLang:= "Enter the name of the language you wish you add."
	TTip.LangDelete:= "Delete the language selected above. THIS IS PERMANENT AND UNRECOVERABLE!"
	TTip.LangAdd:= "Add the language."
	TTip.NPCE_LangAdd_Close:= "Close this window and return to the main NPC window."
;}

; Options Window
;{
	TTip.DataDir:= "This is the current data folder for NPC Engineer. The default is the appdata folder in your user profile."
	TTip.NPCPath:= "This is the current NPC save folder."
	TTip.ProjPath:= "This is the current Project save folder."
	TTip.SpellPath:= "This is the current Spell save folder."
	TTip.EquipPath:= "This is the current Equipment save folder."
	TTip.ArtePath:= "This is the current Artefact (magic item) save folder."
	TTip.OPB01:= "Click here to set a new default data save path."
	TTip.OPB02:= "Click here to set a new project save folder."
	TTip.OPB03:= "Click here to set a new NPC save folder."
	TTip.OPB04:= "Click here to set a new spell save folder."
	TTip.OPB05:= "Click here to set a new equipment save folder."
	TTip.OPB06:= "Click here to set a new artefact save folder."
	TTip.OPR01:= "Reset to the default value."
	TTip.OPR02:= "Reset to the default value."
	TTip.OPR03:= "Reset to the default value."
	TTip.OPR04:= "Reset to the default value."
	TTip.OPR05:= "Reset to the default value."
	TTip.OPR06:= "Reset to the default value."
	TTip.OPR25:= "Reset to the default value."
	TTip.LaunchProject:= "Check this box to load the current project every time Engineer Suite launches."
	TTip.LaunchGUI:= "Check this box jump to the selected component whenever Engineer Suite launches." Chr(10) "You will need to clear this option again to see the Engineer Suite launch GUI." Chr(10) "This will only be useful if you are inputting a number of items of the same type (NPCs, Spells, etc)."
	TTip.DefaultModule:= "Select the default component to launch."
	TTip.TTipTime:= "Change how long tooltips stay visible onscreen from 0 seconds to 10 seconds."
	TTip.ToastOn:= "Select this to have notifications delivered as Windows 'Toast' notifications." Chr(10) "Deselect to have the notifications appear in the right hand side of the status bar of Engineer Suite."
	TTip.linkGUION:= "Select this to have a window pop up when you enter an XML link. This will guide you to the proper structure." Chr(10) "Deselecting puts a boilerplate link in and leaves you to figure it out. This is for experts only!" Chr(10) "NOTE: read the help file on links before trying to use them!"
	
	TTip.NPCCopyPics:= "Should NPC Engineer copy image files and token files to the NPC save directory on save? This keeps all NPC files in one place."
	TTip.NPCModSaveDir:= "Should NPC Engineer create a subfolder for your project in the NPC save directory?" Chr(10) "NOTE: This setting must be 'ON' to use the 'Previous' and 'Next' buttons in the button bar."
	TTip.LaunchNPC:= "Check this box to reload the last edited NPC every time NPC Engineer launches. Exit the application with no NPC loaded to clear this!"
	TTip.DefDesc1:= "Check this box to add a descriptive text element to your NPC by default - only disable if you never intend having description, pictures or links."
	TTip.DefDesc2:= "Check this box to add a title in your descriptive text by default. This is the standard format for 5E NPCs in Fantasy Grounds."
	TTip.DefDesc3:= "For best results leave this unchecked. It is set automatically if you add an image."
	TTip.DefDesc4:= "If an NPC is a caster, this will include a table of their spells at the end of the decription."
	TTip.NPCE_Options_Return:= "Accept your option choices and return to the main NPC window."
	TTip.NPCE_Options_Cancel:= "Cancel all changes and close the window."
	TTip.NpcArtPref:= "Change the text preceding the artist's name in the reference manual attribution box, or remove it completely." Chr(10) "This shouldn't be done lightly!" Chr(10) "NPCs will need to be re-added to the project for this change to show up."
	
	TTip.SpellModSaveDir:= "Should Spell Engineer create a subfolder for your project in the Spell save directory?" Chr(10) "NOTE: This setting must be 'ON' to use the 'Previous' and 'Next' buttons in the button bar."
	TTip.LaunchSpell:= "Check this box to reload the last edited Spell every time Spell Engineer launches. Exit the application with no Spell loaded to clear this!"
	TTip.PopCaster:= "There is no way to automatically tag allowed casters of a spell on import." Chr(10) "Spell Engineer will open the Caster Select GUI immediately after import if this box is checked."
	
	TTip.EquipCopyPics:= "Should Equipment Engineer copy image files and token files to the Equipment save directory on save? This keeps all Equipment files in one place."
	TTip.EquipModSaveDir:= "Should Equipment Engineer create a subfolder for your project in the Equipment save directory?" Chr(10) "NOTE: This setting must be 'ON' to use the 'Previous' and 'Next' buttons in the button bar."
	TTip.LaunchEquip:= "Check this box to reload the last edited item every time Equipment Engineer launches. Exit the application with no item loaded to clear this!"

	TTip.TableCopyPics:= "Should Table Engineer copy image files and token files to the Table save directory on save? This keeps all Table files in one place."
	TTip.TableModSaveDir:= "Should Table Engineer create a subfolder for your project in the Table save directory?" Chr(10) "NOTE: This setting must be 'ON' to use the 'Previous' and 'Next' buttons in the button bar."
	TTip.LaunchTable:= "Check this box to reload the last edited Table every time Table Engineer launches. Exit the application with no Table loaded to clear this!"

;}

; Engineer Suite Window
;{
	TTip.ES_NPC:= "NPC Engineer." Chr(10) "Import, build & save NPCs and monsters."
	TTip.ES_Spell:= "Spell Engineer." Chr(10) "Import, build & save spells."
	TTip.ES_Project:= "Project Engineer." Chr(10) "Create and manage projects."
	TTip.ES_Equipment:= "Equipment Engineer." Chr(10) "Build & save mundane equipment."
	TTip.ES_Artefact:= "Artefact Engineer." Chr(10) "Build & save magic items." Chr(10) "Not fully functional yet." Chr(10) "DON'T TRY TO ADD ITEMS TO YOUR PROJECT YET!"
	TTip.ES_Parcel:= "Parcel Engineer." Chr(10) "Not yet implemented."
	TTip.ES_Table:= "Table Engineer." Chr(10) "Build & save rollable tables for FG."
	TTip.ES_RefMan:= "RefMan Engineer." Chr(10) "Not yet implemented."
	TTip.ES_Parse:= "Create a Fantasy Grounds module from the currently selected project."
;}

}


GetDirectories() {
	Global set
	
;{ Dropbox	
	isit:= 0
	rpath:= A_Appdata "\Dropbox\info.json"
	stringreplace, lpath, rpath, roaming, local
	If (FileExist(rpath)) {
		FileRead, dropbox, %rpath%
		set.BUdropboxPath:= rpath
		isit:= 1
	}
	If (FileExist(lpath)) {
		FileRead, dropbox, %lpath%
		set.BUdropboxPath:= lpath
		isit:= 1
	}
	If isit {
		dummy:= regexmatch(dropbox, "OU)\Q""path"": ""\E(.*)\Q""\E", match)
		dummy:= Match.Value(1)
		StringReplace, dummy, dummy, \\, \, All
		set.BUdropboxPath:= dummy
		set.BUdropbox:= 1
	}
;}	

;{ Onedrive
	RegRead, onedrive, HKEY_CURRENT_USER\Software\Microsoft\OneDrive, UserFolder
	StringReplace, onedrive, onedrive, /, \, All
	if (!onedrive) {
		RegRead, onedrive, HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SkyDrive, UserFolder
		StringReplace, onedrive, onedrive, /, \, All
	}
	if (!onedrive) {
		RegRead, onedrive, HKEY_CURRENT_USER\Software\Microsoft\SkyDrive, UserFolder
		StringReplace, onedrive, onedrive, /, \, All
	}
	if onedrive
		set.BUonedrivePath:= onedrive
		set.BUonedrive:= 1
;}

;{ Google Drive	
	;~ isit:= 0
	;~ rpath:= A_Appdata "\Google\Drive\user_default\sync_log.log"
	;~ stringreplace, rpath, rpath, roaming, local
	;~ If (FileExist(rpath)) {
		;~ FileRead, google, %rpath%
		;~ isit:= 1
	;~ }
	;~ If isit {
		;~ dummy:= RegExMatch(google,"m`as).*^(Sync root:.*?)$",r)
		;~ dummy:= r1
		;~ StringReplace, dummy, dummy, Sync root: \\?\, , All
		;~ set.BUgooglePath:= dummy
		;~ set.BUgoogle:= 1
	;~ }
;}	

;{ Local	
	set.BUlocalPath:= A_MyDocuments
;}	

}

BackupPath() {
	global set, SET_Hwnd
	GUI, ES_Backup:submit, NoHide
	If(SubStr(A_GuiControl, -1) = "01") {
		set.BUdropboxPath:= Get_A_Path(set.BUdropboxPath, "Dropbox")
		GSet(SET_Hwnd.BUdropboxPath, set.BUdropboxPath)
	}
	If(SubStr(A_GuiControl, -1) = "02") {
		set.BUonedrivePath:= Get_A_Path(set.BUonedrivePath, "OneDrive")
		GSet(SET_Hwnd.BUonedrivePath, set.BUonedrivePath)
	}
	If(SubStr(A_GuiControl, -1) = "03") {
		set.BUgooglePath:= Get_A_Path(set.BUgooglePath, "Google Sync")
		GSet(SET_Hwnd.BUgooglePath, set.BUgooglePath)
	}
	If(SubStr(A_GuiControl, -1) = "04") {
		set.BUlocalPath:= Get_A_Path(set.BUlocalPath, "Local")
		GSet(SET_Hwnd.BUlocalPath, set.BUlocalPath)
	}
}

DefaultOptions() {
	Global set

;{ Backup Settings
	set.BUfiles:= 1
	set.BUsettings:= 1
	set.BUprojects:= 1
	set.BUmodules:= 0
	set.BUdropbox:= 0
	set.BUdropboxPath:= ""
	set.BUonedrive:= 1
	set.BUonedrivePath:= ""
	set.BUgoogle:= 0
	set.BUgooglePath:= ""
	set.BUlocal:= 1
	set.BUlocalPath:= ""
	set.BUfilestem:= "ES Backup"
	set.BUdateadd:= 0
	
	set.BUlastYear:= 2019
	set.BUlastMonth:= 09
	set.BUlastDay:= 23
	set.BUlastHour:= 07

	set.BUschedule:= 0
	set.BUfrequency:= 5
	set.BUask:= 1
	
	If set.BUdateadd {
		set.BUfilename:= set.BUfilestem " " A_Year "-" A_Mon "-" A_MDay ".zip"
	} else {
		set.BUfilename:= set.BUfilestem ".zip"
	}
	
;}
	
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                    Backup                    |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


Backup_Settings() {
	GUI_Backup("Settings")
}

Backup_Schedule() {
	GUI_Backup("Schedule")
}

Backup_CompareDate() {
	Global set
	last:= set.BUlastYear*365*24 + set.BUlastMonth*30*24 + set.BUlastDay*24 + set.BUlastHour
	test:= 0
	If set.BUschedule
		test:= 24*set.BUfrequency
	last += test
	Now:= A_YYYY*365*24 + A_MM*30*24 + A_DD*24 + A_Hour
	if (Now > Last) {
		Return 1
	} Else {
		Return 0
	}
}


BAC_BuildZip() {
	Global
	Local zipname, hzip, ZipDir, ZipWin, tempdir, zl, zipnname

	zipname:= datadir "\" set.BUfilename
	If (FileExist(zipname)) {
		FileDelete, %zipname%
	}
	hZip:= ZipCreateFile(zipname)
	Toast("Backup started.")

	If set.BUfiles {
		Toast("Backing up saved files.")
		If FileExist(ArtePath) {
			tempdir:= A_WorkingDir
			SetWorkingDir %ArtePath%
			BAC_CopyFiles(hZip, "Saved Artefact Files")
			SetWorkingDir %tempdir%
		}
		If FileExist(EquipPath) {
			tempdir:= A_WorkingDir
			SetWorkingDir %EquipPath%
			BAC_CopyFiles(hZip, "Saved Equipment Files")
			SetWorkingDir %tempdir%
		}
		If FileExist(NPCPath) {
			tempdir:= A_WorkingDir
			SetWorkingDir %NPCPath%
			BAC_CopyFiles(hZip, "Saved NPC Files")
			SetWorkingDir %tempdir%
		}
		If FileExist(ParcelPath) {
			tempdir:= A_WorkingDir
			SetWorkingDir %ParcelPath%
			BAC_CopyFiles(hZip, "Saved Parcel Files")
			SetWorkingDir %tempdir%
		}
		If FileExist(SpellPath) {
			tempdir:= A_WorkingDir
			SetWorkingDir %Spellpath%
			BAC_CopyFiles(hZip, "Saved Spell Files")
			SetWorkingDir %tempdir%
		}
		If FileExist(TablePath) {
			tempdir:= A_WorkingDir
			SetWorkingDir %TablePath%
			BAC_CopyFiles(hZip, "Saved Table Files")
			SetWorkingDir %tempdir%
		}
	}
	If set.BUsettings {
		Toast("Backing up set.")
		tempdir:= A_WorkingDir
		SetWorkingDir %Datadir%
		Loop, Files, *.json
		{
			ZipDir:= A_LoopFileName
			ZipWin:= ZipAddFile(hZip, A_LoopFileFullPath, ZipDir)
		}
		Loop, Files, *.opt
		{
			ZipDir:= A_LoopFileName
			ZipWin:= ZipAddFile(hZip, A_LoopFileFullPath, ZipDir)
		}
		Loop, Files, NPC Engineer.ini
		{
			ZipDir:= A_LoopFileName
			ZipWin:= ZipAddFile(hZip, A_LoopFileFullPath, ZipDir)
		}
		SetWorkingDir %tempdir%
	}
	If set.BUprojects {
		Toast("Backing up project files.")
		If FileExist(ProjPath) {
			tempdir:= A_WorkingDir
			SetWorkingDir %ProjPath%
			BAC_CopyFiles(hZip, "Saved Project Files")
			SetWorkingDir %Datadir%
			Loop, Files, *.ini
			{
				If !(A_LoopFileName = "NPC Engineer.ini") {
					ZipDir:= A_LoopFileName
					ZipWin:= ZipAddFile(hZip, A_LoopFileFullPath, ZipDir)
				}
			}
			SetWorkingDir %tempdir%
		}
	}
	If set.BUmodules {
		Toast("Backing up FG modules.")
		tempdir:= A_WorkingDir
		SetWorkingDir %Datadir%
		zl:= []
		
		Loop, Files, *.ini
		{
			If !(A_LoopFileName = "NPC Engineer.ini") {
				stringreplace file, A_LoopFileName, .ini, .mod
				If !HasVal(zl, file)
					zl.push(file)
			}
		}
		SetWorkingDir %FGPath%
		Loop, Files, *.mod
		{
			If HasVal(zl, A_LoopFileName) {
					ZipDir:= "Backup of mod files\" A_LoopFileName
					ZipWin:= ZipAddFile(hZip, A_LoopFileFullPath, ZipDir)
			}
		}
		SetWorkingDir %tempdir%
	}

	success:= ZipCloseFile(hzip)
	
	backupdone:= 0
	If (set.BUdropbox) AND (FileExist(set.BUdropboxPath)) {
		zipnname:= set.BUdropboxPath "\" set.BUfilename
		FileCopy, %zipname%, %zipnname%, 1
		backupdone:= 1
	}	
	If (set.BUonedrive) AND (FileExist(set.BUonedrivePath)) {
		zipnname:= set.BUonedrivePath "\" set.BUfilename
		FileCopy, %zipname%, %zipnname%, 1
		backupdone:= 1
	}	
	If (set.BUgoogle) AND (FileExist(set.BUgooglePath)) {
		zipnname:= set.BUgooglePath "\" set.BUfilename
		FileCopy, %zipname%, %zipnname%, 1
		backupdone:= 1
	}	
	If (set.BUlocal) AND (FileExist(set.BUlocalPath)) {
		zipnname:= set.BUlocalPath "\" set.BUfilename
		FileCopy, %zipname%, %zipnname%, 1
		backupdone:= 1
	}	
	
	if backupdone {
		set.BUlastYear:= A_YYYY
		set.BUlastMonth:= A_MM
		set.BUlastDay:= A_DD
		set.BUlastHour:= A_Hour
		Save_Settings()
	}
	
	FileDelete %zipname%
	Toast("Backup completed.")
}

BAC_CopyFiles(hZip, folder) {
	Loop, Files, *.*, FR
	{
		If A_LoopFileDir {
			ZipDir:= folder "\" A_LoopFileDir "\" A_LoopFileName
		} else {
			ZipDir:= folder "\" A_LoopFileName
		}
		ZipWin:= ZipAddFile(hZip, A_LoopFileFullPath, ZipDir)
	}
}
