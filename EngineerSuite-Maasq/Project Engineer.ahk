/*
Component:	Project Engineer
  Version:	0.5.2
     Date:	17/08/19
 Revision:	Addition of categories.
 */

;~ ######################################################
;~ #                       Labels                       #
;~ ######################################################

NPCE_Project_SClose:
	GUI, NPCE_Project:submit, NoHide
	If (ModPath and ModName and ModFile) {
		Project_Do_Save()
	} Else {
		MsgBox, 48, More information needed, You must enter a project name, filename and path before you can create it.
		Return
	}
NPCE_Project_Close:
NPCE_ProjectGuiClose:		;{
	Gui, %PROE_Ref%:-disabled
	Gui, NPCE_Project:Hide
	Gui, %PROE_Ref%:Default
	StBar(PROE_Ref)
	File:= A_AppData "\NPC Engineer\NPC Engineer.ini"
	Stub:= "Paths"
	IniWrite, %ProjectPath%, %File%, %Stub%, ProjectPath
return						;}

Project_Create:				;{
	GUI, NPCE_Project:submit, NoHide
	If (ModPath and ModName and ModFile) {
		Project_Create()
	} Else {
		MsgBox, 48, More information needed, You must enter a project name, filename and path before you can create it.
	}
return						;}

Project_File:				;{
	GUI, NPCE_Project:submit, NoHide
	GuiControl, NPCE_Project:, FN, %ModFile%.mod
return						;}

Project_GetFGPath:			;{
	FGPath:= Get_A_Path(FGPath, "Fantasy Grounds module")
	If (SubStr(FGPath, 0, 1) != "\") and FGPath {
		FGPath:= FGPath "\"
	}
	GuiControl, NPCE_Project:, FGPath, %FGPath%
return						;}

Project_GetPath:			;{
	If ModStem {
		ModStem:= Get_A_Path(ModStem, "module")
	} Else {
		ModStem:= Get_A_Path(ProjPath, "module")
	}
Project_Path:
	GUI, NPCE_Project:submit, NoHide
	If (SubStr(ModStem, 0, 1) != "\") and ModStem {
		ModStem:= ModStem "\"
	}
	If (AddPath and ModName) {
		ModPath:= ModStem ModName "\"
	} Else {
		ModPath:= ModStem
	}
	Modfile:=regexreplace(modname,"[^a-zA-Z_ 0-9]", "")
	GuiControl, NPCE_Project:, ModPath, %ModPath%
	GuiControl, NPCE_Project:, Modfile, %Modfile%
return						;}

Project_Load:				;{
	Project_Load()
	If ProjSaveCheck {
		Inject_Project()
		Gosub Load_Thumb
		ProjectLive:= 1
		If (ModMonst and Mod_Parser = 1) {
			jsonpath:= ModPath "input\npcs.json"
			if fileexist(jsonpath) {
				NPC:= new JSONfile(jsonpath)
			} else {
				msgbox The selected project encountered an error. Please check that the path in its *.ini file still exists.
				Gosub Project_New
			}
		}
		If (ModSpell and Mod_Parser = 1) {
			jsonpath:= ModPath "input\spells.json"
			if fileexist(jsonpath) {
				SPL:= new JSONfile(jsonpath)
			} else {
				msgbox The selected project encountered an error. Please check that the path in its *.ini file still exists.
				Gosub Project_New
			}
		}
		If (ModEquip and Mod_Parser = 1) {
			jsonpath:= ModPath "input\equipment.json"
			if fileexist(jsonpath) {
				EQP:= new JSONfile(jsonpath)
			} else {
				msgbox The selected project encountered an error. Please check that the path in its *.ini file still exists.
				Gosub Project_New
			}
		}
		If (ModMitem and Mod_Parser = 1) {
			jsonpath:= ModPath "input\artefacts.json"
			if fileexist(jsonpath) {
				FCT:= new JSONfile(jsonpath)
			} else {
				msgbox The selected project encountered an error. Please check that the path in its *.ini file still exists.
				Gosub Project_New
			}
		}
		If (ModTable and Mod_Parser = 1) {
			jsonpath:= ModPath "input\tables.json"
			if fileexist(jsonpath) {
				TBL:= new JSONfile(jsonpath)
			} else {
				msgbox The selected project encountered an error. Please check that the path in its *.ini file still exists.
				Gosub Project_New
			}
		}
		jsonpath:= ModPath "input\categories.json"
		if fileexist(jsonpath) {
			CAT:= new JSONfile(jsonpath)
		} else {
			Pr_Cr_File("input\categories.json", 1)
			jsonpath:= ModPath "input\categories.json"
			CAT:= new JSONfile(jsonpath)
			Armoury:= {}
			Armoury[Modname]:= {}
			Armoury[Modname].Name:= Trim(Modname)
			CAT.fill(Armoury)
			CAT.save(true)
		}
	}
return						;}

Project_New:				;{
	Initialise_Other()
	Inject_Project()
	ModThum:= ""
	GuiControl, NPCE_Project:show, Going
	GuiControl, NPCE_Project:hide, ModThum
	GuiControl, NPCE_Project:, ModThum
	GuiControl, NPCE_Project:show, ModThum
return						;}

Project_Save:				;{
	GUI, NPCE_Project:submit, NoHide
	Project_Do_Save()
return						;}

Project_Thumbnail:			;{
	FileSelectFile, ThumbPath, , , Select an image., (*.png)
Load_Thumb:
	hBM := LoadPicture( ThumbPath )
	IfEqual, hBM, 0, Return

	BITMAP := getHBMinfo( hBM )                                ; Extract Width andh height of image 
	New := ScaleRect( BITMAP.Width, BITMAP.Height, 120, 120 )  ; Derive best-fit W x H for source image 

	DllCall( "DeleteObject", "Ptr",hBM )                       ; Delete Image handle ...         
	hBM := LoadPicture( ThumbPath, "GDI+ w" New.W . " h" . New.H )  ; ..and get a new one with correct W x H

	GuiControl, NPCE_Project:Hide, Going
	GuiControl, NPCE_Project:, ModThum,  *w0 *h0 HBITMAP:%hBM% 
return						;}



;~ ######################################################
;~ #                   Function List.                   #
;~ ######################################################

ProjectEngineer(what) {
	global
	PROE_Ref:= what
	Gui, NPCE_Project:Show, w930 h550, Project Management
	hToolbar := CreateToolBar2()
}

Initialise_Other() {
	global
	RegRead, FGPath, HKEY_CURRENT_USER\Software\Fantasy Grounds\2.0, DataDir
	StringReplace, FGPath, FGPath, /, \, All
	FGPath:= FGPath "modules\"
	ProjectPath:= ""
	
	ModName:= ""
	ModCate:= ""
	ModAuth:= ""
	ModFile:= ""
	ModPath:= ProjPath
	ModGmon:= 0
	ModLock:= 0
	AddPath:= 1

	ThumbPath:= ""
	ModStem:= ProjPath

	Mod_Parser:= 1
	ProjectLive:= 0
	
	ModImage:= 0
	ModToken:= 0
	ModMonst:= 0
	ModRaces:= 0
	ModClass:= 0
	ModBackg:= 0
	ModFeats:= 0
	ModSkill:= 0
	ModSpell:= 0
	ModEquip:= 0
	ModMItem:= 0
	ModEncou:= 0
	ModRanEn:= 0
	ModQuest:= 0
	ModStory:= 0
	ModStTem:= 0
	ModParcl:= 0
	ModIPins:= 0
	ModIGrid:= 0
	ModPregn:= 0
	ModRefMn:= 0
	ModTable:= 0

	If (SubStr(ModStem, 0, 1) != "\") and ModStem {
		ModStem:= ModStem "\"
	}
	If (AddPath and ModName) {
		ModPath:= ModStem ModName "\"
	} Else {
		ModPath:= ModStem
	}

	WinTProj:= "None loaded."
}

Project_Do_Save() {
	global
	FileSelectFile, SelectedFile, S2, %Datadir%\%ModName%.ini, Save Project, (*.ini)
	SplitPath, SelectedFile, f_name, f_dir, f_ext, f_name_no_ext, f_drive
	If SelectedFile {
		if (f_ext != "ini")
		{
			SelectedFile = %SelectedFile%.ini
			f_name = %f_name%.ini
		}
		If FileExist(SelectedFile)
		{
			MsgBox, 52, Confirm Save As..., %f_name% already exists.`nDo you want to replace it?
			IfMsgBox Yes
			{
				FileDelete, %SelectedFile%
				Project_SaveIni(SelectedFile)
				Project_Create()
			}
		} else {
			Project_SaveIni(SelectedFile)
			Project_Create()
		}
	}
}

Project_SaveIni(file) {
	global
	
	ProjectPath:= file
	
	Stub:= "Project"
	IniWrite, %ModName%, %File%, %Stub%, ModName
	IniWrite, %ModCate%, %File%, %Stub%, ModCate
	IniWrite, %ModAuth%, %File%, %Stub%, ModAuth
	IniWrite, %ModFile%, %File%, %Stub%, ModFile
	IniWrite, %ModGmon%, %File%, %Stub%, ModGmon
	IniWrite, %ModLock%, %File%, %Stub%, ModLock
	IniWrite, %AddPath%, %File%, %Stub%, AddPath
	IniWrite, %ModPath%, %File%, %Stub%, ModPath
	IniWrite, %FGPath%, %File%, %Stub%, FGPath
	IniWrite, %ModStem%, %File%, %Stub%, ModStem
	IniWrite, %ThumbPath%, %File%, %Stub%, ThumbPath
	IniWrite, %Mod_Parser%, %File%, %Stub%, Mod_Parser
	
	Stub:= "Include Section"
	IniWrite, %ModImage%, %File%, %Stub%, ModImage
	IniWrite, %ModToken%, %File%, %Stub%, ModToken
	IniWrite, %ModMonst%, %File%, %Stub%, ModMonst
	IniWrite, %ModRaces%, %File%, %Stub%, ModRaces
	IniWrite, %ModClass%, %File%, %Stub%, ModClass
	IniWrite, %ModBackg%, %File%, %Stub%, ModBackg
	IniWrite, %ModFeats%, %File%, %Stub%, ModFeats
	IniWrite, %ModSkill%, %File%, %Stub%, ModSkill
	IniWrite, %ModSpell%, %File%, %Stub%, ModSpell
	IniWrite, %ModEquip%, %File%, %Stub%, ModEquip
	IniWrite, %ModMItem%, %File%, %Stub%, ModMItem
	IniWrite, %ModEncou%, %File%, %Stub%, ModEncou
	IniWrite, %ModRanEn%, %File%, %Stub%, ModRanEn
	IniWrite, %ModQuest%, %File%, %Stub%, ModQuest
	IniWrite, %ModStory%, %File%, %Stub%, ModStory
	IniWrite, %ModStTem%, %File%, %Stub%, ModStTem
	IniWrite, %ModParcl%, %File%, %Stub%, ModParcl
	IniWrite, %ModIPins%, %File%, %Stub%, ModIPins
	IniWrite, %ModIGrid%, %File%, %Stub%, ModIGrid
	IniWrite, %ModPregn%, %File%, %Stub%, ModPregn
	IniWrite, %ModRefMn%, %File%, %Stub%, ModRefMn
	IniWrite, %ModTable%, %File%, %Stub%, ModTable
}

Project_Load() {
	global
	ProjSaveCheck:= 0
	If (flags.project = 0) {
		FileSelectFile, SelectedFile, 2, %DataDir%, Load Project, (*.ini)
		ProjectPath:= SelectedFile
	} else {
		 SelectedFile:= ProjectPath
		 flags.project:= 0
	}
	
	If SelectedFile {
		ProjSaveCheck:= 1
		Stub:= "Project"
		IniRead, ModName, %SelectedFile%, %Stub%, ModName, %A_Space%
		IniRead, ModCate, %SelectedFile%, %Stub%, ModCate, %A_Space%
		IniRead, ModAuth, %SelectedFile%, %Stub%, ModAuth, %A_Space%
		IniRead, ModFile, %SelectedFile%, %Stub%, ModFile, %A_Space%
		IniRead, ModGmon, %SelectedFile%, %Stub%, ModGmon, %A_Space%
		IniRead, ModLock, %SelectedFile%, %Stub%, ModLock, 0
		IniRead, AddPath, %SelectedFile%, %Stub%, AddPath, %A_Space%
		IniRead, ModPath, %SelectedFile%, %Stub%, ModPath, %A_Space%
		IniRead, FGPath, %SelectedFile%, %Stub%, FGPath, %A_Space%
		IniRead, ModStem, %SelectedFile%, %Stub%, ModStem, %A_Space%
		IniRead, ThumbPath, %SelectedFile%, %Stub%, ThumbPath, %A_Space%
		IniRead, Mod_Parser, %SelectedFile%, %Stub%, Mod_Parser, %A_Space%
		
		Stub:= "Include Section"
		IniRead, ModImage, %SelectedFile%, %Stub%, ModImage, %A_Space%
		IniRead, ModToken, %SelectedFile%, %Stub%, ModToken, %A_Space%
		IniRead, ModMonst, %SelectedFile%, %Stub%, ModMonst, %A_Space%
		IniRead, ModRaces, %SelectedFile%, %Stub%, ModRaces, %A_Space%
		IniRead, ModClass, %SelectedFile%, %Stub%, ModClass, %A_Space%
		IniRead, ModBackg, %SelectedFile%, %Stub%, ModBackg, %A_Space%
		IniRead, ModFeats, %SelectedFile%, %Stub%, ModFeats, %A_Space%
		IniRead, ModSkill, %SelectedFile%, %Stub%, ModSkill, %A_Space%
		IniRead, ModSpell, %SelectedFile%, %Stub%, ModSpell, %A_Space%
		IniRead, ModEquip, %SelectedFile%, %Stub%, ModEquip, %A_Space%
		IniRead, ModMItem, %SelectedFile%, %Stub%, ModMItem, %A_Space%
		IniRead, ModEncou, %SelectedFile%, %Stub%, ModEncou, %A_Space%
		IniRead, ModRanEn, %SelectedFile%, %Stub%, ModRanEn, %A_Space%
		IniRead, ModQuest, %SelectedFile%, %Stub%, ModQuest, %A_Space%
		IniRead, ModStory, %SelectedFile%, %Stub%, ModStory, %A_Space%
		IniRead, ModStTem, %SelectedFile%, %Stub%, ModStTem, %A_Space%
		IniRead, ModParcl, %SelectedFile%, %Stub%, ModParcl, %A_Space%
		IniRead, ModIPins, %SelectedFile%, %Stub%, ModIPins, %A_Space%
		IniRead, ModIGrid, %SelectedFile%, %Stub%, ModIGrid, %A_Space%
		IniRead, ModPregn, %SelectedFile%, %Stub%, ModPregn, %A_Space%
		IniRead, ModRefMn, %SelectedFile%, %Stub%, ModRefMn, %A_Space%
		IniRead, ModTable, %SelectedFile%, %Stub%, ModTable, %A_Space%
	}
}

Project_Create() {
	global
	Pr_Cr_Folder("")
	Pr_Cr_Folder("input\")
	Pr_Cr_Folder("input\images\")
	Pr_Cr_Folder("input\tokens\")
	Pr_Cr_Folder("output\")
	Pr_Cr_Folder("output\images\")
	Pr_Cr_Folder("output\tokens\")
	
	If ModMonst {
		if (Mod_Parser == 1) {
			Pr_Cr_File("input\npcs.json", ModMonst)
			jsonpath:= ModPath "input\npcs.json"
			NPC:= new JSONfile(jsonpath)
		} else {
			Pr_Cr_File("input\npcs.txt", ModMonst)
		}
	}
	If ModSpell {
		if (Mod_Parser == 1) {
			Pr_Cr_File("input\spells.json", ModSpell)
			jsonpath:= ModPath "input\spells.json"
			SPL:= new JSONfile(jsonpath)
		} else {
			Pr_Cr_File("input\spells.txt", ModSpell)
		}
	}
	If ModEquip {
		if (Mod_Parser == 1) {
			Pr_Cr_File("input\equipment.json", ModEquip)
			jsonpath:= ModPath "input\equipment.json"
			EQP:= new JSONfile(jsonpath)
		} else {
			Pr_Cr_File("input\equipment.txt", ModEquip)
		}
	}
	If ModMitem {
		if (Mod_Parser == 1) {
			Pr_Cr_File("input\artefacts.json", ModMitem)
			jsonpath:= ModPath "input\artefacts.json"
			FCT:= new JSONfile(jsonpath)
		} else {
			Pr_Cr_File("input\artefacts.txt", ModMitem)
		}
	}
	If ModTable {
		if (Mod_Parser == 1) {
			Pr_Cr_File("input\tables.json", ModTable)
			jsonpath:= ModPath "input\tables.json"
			TBL:= new JSONfile(jsonpath)
		} else {
			Pr_Cr_File("input\table.txt", ModTable)
		}
	}
	
	If ThumbPath {
		Ifexist, %ThumbPath%
		{
			ThumbDest:= ModPath . "thumbnail.png"
			FileCopy, %ThumbPath%, %ThumbDest%, 1
			ThumbPath:= ThumbDest
		}
	}
	Pr_Cr_File("input\categories.json", 1)
	jsonpath:= ModPath "input\categories.json"
	CAT:= new JSONfile(jsonpath)
	Armoury:= {}
	Armoury[Modname]:= {}
	Armoury[Modname].Name:= Trim(Modname)
	CAT.fill(Armoury)
	CAT.save(true)

	notify:= ModName " created successfully."
	Toast(notify)
	ProjectLive:= 1
	Gui, %PROE_Ref%:Default
	StBar(PROE_Ref)
}

Pr_Cr_Folder(Folder) {
	global
	path:= ModPath . Folder
	Ifnotexist, %path%
		FileCreateDir, %path%
}
	
Pr_Cr_File(File, Switch) {
	global
	If Switch {
		path:= ModPath . File
		Ifnotexist, %path%
			FileAppend, , %path%
	}
}

Inject_Project() {
	global
	GuiControl, NPCE_Project:, ModPath, %ModPath%
	GuiControl, NPCE_Project:, FGPath, %FGPath%
	GuiControl, NPCE_Project:, ModName, %ModName%
	GuiControl, NPCE_Project:, ModAuth, %ModAuth%
	GuiControl, NPCE_Project:, ModFile, %ModFile%
	GuiControl, NPCE_Project:, ModCate, %ModCate%
	GuiControl, NPCE_Project:, ModGmon, %ModGmon%
	GuiControl, NPCE_Project:, ModLock, %ModLock%
	GuiControl, NPCE_Project:, AddPath, %AddPath%

	GuiControl, NPCE_Project:Choose, Mod_Parser, %Mod_Parser%

	GuiControl, NPCE_Project:, ModImage, %ModImage%
	GuiControl, NPCE_Project:, ModToken, %ModToken%
	GuiControl, NPCE_Project:, ModMonst, %ModMonst%
	GuiControl, NPCE_Project:, ModRaces, %ModRaces%
	GuiControl, NPCE_Project:, ModClass, %ModClass%
	GuiControl, NPCE_Project:, ModBackg, %ModBackg%
	GuiControl, NPCE_Project:, ModFeats, %ModFeats%
	GuiControl, NPCE_Project:, ModSkill, %ModSkill%
	GuiControl, NPCE_Project:, ModSpell, %ModSpell%
	GuiControl, NPCE_Project:, ModEquip, %ModEquip%
	GuiControl, NPCE_Project:, ModMItem, %ModMItem%
	GuiControl, NPCE_Project:, ModEncou, %ModEncou%
	GuiControl, NPCE_Project:, ModRanEn, %ModRanEn%
	GuiControl, NPCE_Project:, ModQuest, %ModQuest%
	GuiControl, NPCE_Project:, ModStory, %ModStory%
	GuiControl, NPCE_Project:, ModStTem, %ModStTem%
	GuiControl, NPCE_Project:, ModParcl, %ModParcl%
	GuiControl, NPCE_Project:, ModIPins, %ModIPins%
	GuiControl, NPCE_Project:, ModIGrid, %ModIGrid%
	GuiControl, NPCE_Project:, ModPregn, %ModPregn%
	GuiControl, NPCE_Project:, ModRefMn, %ModRefMn%
	GuiControl, NPCE_Project:, ModTable, %ModTable%
	Gui, %PROE_Ref%:Default
	WinTProj:= ModName
	SB_SetText("  " WinTProj, 1)
}

GUI_Project() {
	global
	If (SubStr(ModStem, 0, 1) != "\") and ModStem {
		ModStem:= ModStem "\"
	}
	If (AddPath and ModName) {
		ModPath:= ModStem ModName "\"
	} Else {
		ModPath:= ModStem
	}

	
; Settings for project management window (NPCE_Project)
	Gui, NPCE_Project:+hwndNPCE_Project
	Gui, NPCE_Project:Color, 0xE2E1E8
	Gui, NPCE_Project:font, S10 c000000, Arial

	; Titles
	Gui, NPCE_Project:font, S10, Arial Bold
	Gui, NPCE_Project:Add, Text, x477 y62 w130 h17, Image Sections
	Gui, NPCE_Project:Add, Text, x477 y114 w130 h17, Data Sections
	Gui, NPCE_Project:Add, Text, x477 y276 w130 h17, Adventure Sections
	Gui, NPCE_Project:Add, Text, x477 y438 w130 h17, Utility Sections

	; Groupboxes
	Gui, NPCE_Project:font, S10 c727178, Arial Bold
	Gui, NPCE_Project:Add, GroupBox, x8 y40 w452 h312, Module Information
	Gui, NPCE_Project:Add, GroupBox, x470 y40 w452 h470, Parser Information

	Gui, NPCE_Project:font, S10 c000000, Arial

	; Module Information

	Gui, NPCE_Project:Add, Edit, x120 y60 w204 h23 vModName  gProject_Path, %ModName%
	Gui, NPCE_Project:Add, Edit, x120 y90 w204 h23 vModCate, %ModCate%
	Gui, NPCE_Project:Add, Edit, x120 y120 w204 h23 vModAuth, %ModAuth%
	Gui, NPCE_Project:Add, Edit, x120 y150 w204 h23 vModFile gProject_File, %ModFile%
	
	Gui, NPCE_Project:Add, Text, x15 y63 w100 h17 Right, Module Title
	Gui, NPCE_Project:Add, Text, x15 y93 w100 h17 Right, Module Category
	Gui, NPCE_Project:Add, Text, x15 y123 w100 h17 Right, Module Author
	Gui, NPCE_Project:Add, Text, x15 y153 w100 h17 Right, Module Filename
	Gui, NPCE_Project:Add, Text, x15 y212 w75 h17, Module Path
	Gui, NPCE_Project:Add, Text, x15 y254 w200 h17, Fantasy Grounds Modules Path

	Gui, NPCE_Project:font, S7 c000000, Arial
	Gui, NPCE_Project:Add, Text, x120 y174 w204 h12 vFN Right, %ModFile%.mod
	Gui, NPCE_Project:font, S10 c000000, Arial

	Gui, NPCE_Project:font, S14 c000000, Arial
	Gui, NPCE_Project:Add, Text, x335 y85 w112 vGoing Center, Click to select thumbnail
	Gui, NPCE_Project:font, S10 c000000, Arial
	
	Gui, NPCE_Project:Add, CheckBox, x15 y186 w119 h17 +0x20 Right vModGmon Checked%ModGmon%, GM View Only%A_Space%
	Gui, NPCE_Project:Add, CheckBox, x140 y186 w119 h17 +0x20 Right vModLock Checked%ModLock%, Lock Records%A_Space%

	Gui, NPCE_Project:font, S8 c000000, Calibri
	Gui, NPCE_Project:Add, CheckBox, x221 y212 w200 h17 +0x20 Right vAddPath gProject_Path Checked%AddPath%, Add Module Name to Path%A_Space%
	Gui, NPCE_Project:Add, Edit, x15 y230 w407 h20 vModPath, %ModPath%
	Gui, NPCE_Project:Add, Edit, x15 y272 w407 h20 vFGPath, %FGPath%
	Gui, NPCE_Project:font, S10 c000000, Arial

	Gui, NPCE_Project:Add, Button, x429 y228 w24 h24 hwndMPB1 vMPB1 gProject_GetPath -Tabstop 
	GuiButtonIcon(MPB1, "shell32.dll", 310, "s16")
	Gui, NPCE_Project:Add, Button, x429 y270 w24 h24 hwndMPB2 vMPB2 gProject_GetFGPath -Tabstop 
	GuiButtonIcon(MPB2, "shell32.dll", 310, "s16")
	
	PicWinOptions := ( SS_BITMAP := 0xE ) | ( SS_CENTERIMAGE := 0x200 )
	Gui, NPCE_Project:Add, Picture, x331 y60 w120 h120 %PicWinOptions% BackgroundTrans Border vModThum gProject_Thumbnail,

	Gui, NPCE_Project:Add, DropDownList, x120 y315 w204 vMod_Parser AltSubmit, Engineer Suite||Par5e
	Gui, NPCE_Project:Add, Text, x15 y318 w100 h17 Right, Target Parser


	Gui, NPCE_Project:font, S9 c000000, Arial
	Gui, NPCE_Project:Add, CheckBox, x500 y80 w140 h15 vModImage Checked%ModImage%, Images
	Gui, NPCE_Project:Add, CheckBox, x500 y96 w140 h15 vModToken Checked%ModToken%, Tokens
	Gui, NPCE_Project:Add, CheckBox, x500 y130 w140 h15 vModMonst Checked%ModMonst%, NPCs / Monsters
	Gui, NPCE_Project:Add, CheckBox, x500 y146 w140 h15 Disabled vModRaces Checked%ModRaces%, Races
	Gui, NPCE_Project:Add, CheckBox, x500 y162 w140 h15 Disabled vModClass Checked%ModClass%, Classes
	Gui, NPCE_Project:Add, CheckBox, x500 y178 w140 h15 Disabled vModBackg Checked%ModBackg%, Backgrounds
	Gui, NPCE_Project:Add, CheckBox, x500 y194 w140 h15 Disabled vModFeats Checked%ModFeats%, Feats
	Gui, NPCE_Project:Add, CheckBox, x500 y210 w140 h15 Disabled vModSkill Checked%ModSkill%, Skills
	Gui, NPCE_Project:Add, CheckBox, x500 y226 w140 h15 vModSpell Checked%ModSpell%, Spells
	Gui, NPCE_Project:Add, CheckBox, x500 y242 w140 h15 vModEquip Checked%ModEquip%, Equipment
	Gui, NPCE_Project:Add, CheckBox, x500 y258 w140 h15 vModMItem Checked%ModMItem%, Magic Items
	Gui, NPCE_Project:Add, CheckBox, x500 y292 w140 h15 Disabled vModEncou Checked%ModEncou%, Encounters
	Gui, NPCE_Project:Add, CheckBox, x500 y308 w140 h15 Disabled vModRanEn Checked%ModRanEn%, Random Encounters
	Gui, NPCE_Project:Add, CheckBox, x500 y324 w140 h15 Disabled vModQuest Checked%ModQuest%, Quests
	Gui, NPCE_Project:Add, CheckBox, x500 y340 w140 h15 Disabled vModStory Checked%ModStory%, Story Entries
	Gui, NPCE_Project:Add, CheckBox, x500 y356 w140 h15 Disabled vModStTem Checked%ModStTem%, Story Templates
	Gui, NPCE_Project:Add, CheckBox, x500 y372 w140 h15 Disabled vModParcl Checked%ModParcl%, Treasure Parcels
	Gui, NPCE_Project:Add, CheckBox, x500 y388 w140 h15 Disabled vModIPins Checked%ModIPins%, Image Pins
	Gui, NPCE_Project:Add, CheckBox, x500 y404 w140 h15 Disabled vModIGrid Checked%ModIGrid%, Image Grids
	Gui, NPCE_Project:Add, CheckBox, x500 y420 w150 h15 Disabled vModPregn Checked%ModPregn%, Pregenerated Chars
	Gui, NPCE_Project:Add, CheckBox, x500 y454 w150 h15 vModRefMn Checked%ModRefMn%, Reference Manual
	Gui, NPCE_Project:Add, CheckBox, x500 y470 w150 h15 vModTable Checked%ModTable%, Tables

	Gui, NPCE_Project:font, S9 c525158, Arial
	Gui, NPCE_Project:Add, Text, x660 y80 w250 h15, module \ input \ images \
	Gui, NPCE_Project:Add, Text, x660 y96 w250 h15, module \ input \ tokens \
	Gui, NPCE_Project:Add, Text, x660 y130 w250 h15, module \ input \ npcs.json
	Gui, NPCE_Project:Add, Text, x660 y146 w250 h15 Disabled, module \ input \ races.json
	Gui, NPCE_Project:Add, Text, x660 y162 w250 h15 Disabled, module \ input \ class.json
	Gui, NPCE_Project:Add, Text, x660 y178 w250 h15 Disabled, module \ input \ backgrounds.json
	Gui, NPCE_Project:Add, Text, x660 y194 w250 h15 Disabled, module \ input \ feats.json
	Gui, NPCE_Project:Add, Text, x660 y210 w250 h15 Disabled, module \ input \ skills.json
	Gui, NPCE_Project:Add, Text, x660 y226 w250 h15, module \ input \ spells.json
	Gui, NPCE_Project:Add, Text, x660 y242 w250 h15, module \ input \ equipment.json
	Gui, NPCE_Project:Add, Text, x660 y258 w250 h15, module \ input \ artefacts.json
	Gui, NPCE_Project:Add, Text, x660 y292 w250 h15 Disabled, module \ input \ encounters.json
	Gui, NPCE_Project:Add, Text, x660 y308 w250 h15 Disabled, module \ input \ randomencounters.json
	Gui, NPCE_Project:Add, Text, x660 y324 w250 h15 Disabled, module \ input \ quests.json
	Gui, NPCE_Project:Add, Text, x660 y340 w250 h15 Disabled, module \ input \ story.json
	Gui, NPCE_Project:Add, Text, x660 y356 w250 h15 Disabled, module \ input \ storytemplates.json
	Gui, NPCE_Project:Add, Text, x660 y372 w250 h15 Disabled, module \ input \ parcels.json
	Gui, NPCE_Project:Add, Text, x660 y388 w250 h15 Disabled, module \ input \ imagepins.json
	Gui, NPCE_Project:Add, Text, x660 y404 w250 h15 Disabled, module \ input \ imagegrids.json
	Gui, NPCE_Project:Add, Text, x660 y420 w250 h15 Disabled, module \ input \ pregens.json
	Gui, NPCE_Project:Add, Text, x660 y454 w250 h15 Disabled, module \ input \ referencemanual.json
	Gui, NPCE_Project:Add, Text, x660 y470 w250 h15, module \ input \ tables.json

	Gui, NPCE_Project:font, S10 c000000, Arial
	Gui, NPCE_Project:Add, Button, x792 y515 w130 h30 +border +default vNPCE_Project_Close gNPCE_Project_Close, Close
	Gui, NPCE_Project:Add, Button, x652 y515 w130 h30 +border vNPCE_Project_SClose gNPCE_Project_SClose, Save Changes
	
	Gui, NPCE_Project:font, S9 c525158, Arial
	Gui, NPCE_Project:Add, Text, x15 y380 w400 h14, Use the buttons in the toolbar to manage your Project.
	Gui, NPCE_Project:Add, Text, x30 y400 w400 h14, 1. 'New Project' resets all fields.
	Gui, NPCE_Project:Add, Text, x30 y418 w400 h14, 2. 'Load Project' loads a previously-saved ini file.
	Gui, NPCE_Project:Add, Text, x30 y436 w400 h14, 3. 'Save Project' stores your information in an ini file.
	Gui, NPCE_Project:Add, Text, x30 y454 w400 h14, 4. 'Create Project' creates the directory structure and
	Gui, NPCE_Project:Add, Text, x45 y468 w400 h14, blank text documents for the selected sections.
	
	Gui, NPCE_Project:font, S10 c000000, Arial

}

CreateToolbar2() {
	ImageList := IL_Create(3)
	IL_Add(ImageList, "NPC Engineer.dll", 1)
	IL_Add(ImageList, "NPC Engineer.dll", 2)
	IL_Add(ImageList, "NPC Engineer.dll", 3)

	Buttons = 
	(LTrim
		New Project
		Load Project
		Save Project
	)

	Return ToolbarCreate("OnToolbar", Buttons, "NPCE_Project", ImageList, "Flat List Tooltips Border")
}


