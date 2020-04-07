;~ ######################################################
;~ #                                                    #
;~ #               ~ Equipment Engineer ~               #
;~ #                                                    #
;~ #                                  Written by Maasq. #
;~ ######################################################

#NoEnv
#Persistent
#SingleInstance Force
;~ #NoTrayIcon
SendMode Input
SetWorkingDir %A_ScriptDir%
SetWinCheck()
Gosub StartupRoutine
EquipmentEngineer("self")
return

EquipmentEngineer(what) {
	global
	SplashImageGUI("Equipment Engineer.png", "Center", "Center", 300, true)
	
	;~ EQE_LoadPause:= 1
	EQE_Release_Version:= "0.08.09"
	EQE_Release_Date:= "24/11/19"
	EQE_Ref:= what
	
	Gosub EQE_Startup
	EQE_Initialise()
	
	EQE_PrepareGUI()
	If (LaunchProject AND FileExist(ProjectPath)) {
		flags.project:= 1
		PROE_Ref:= "EQE_Main"
		Gosub Project_Load
	}
	FGcatList(EQE_Hwnd.FGcat)
	If (LaunchEquip AND FileExist(EquipPath)) {
		flags.Equip:= 1
		Equip:= ObjLoad(EquipSavePath)
		EQE_SetVars()
	}

	EQE_GetVars()
	EQE_RH_Box()
	
	win.equipmentengineer:= 1
	Gui, EQE_Main:Show, w1150 h665, Equipment Engineer
	EQE_Toolbar:= EQE_CreateToolBar()

	;~ EQE_LoadPause:= 0
	
	Hotkey,IfWinActive,ahk_id %EQE_Main%
	hotkey, ^b, SetFontStyle2
	hotkey, ^i, SetFontStyle2
	hotkey, ^u, SetFontStyle2
	hotkey, ^v, SpPaste
	Hotkey, ^J, De_PDF
	Hotkey, ~LButton, EQE_Vup
	Hotkey, ~Wheeldown, EQE_VScrDwn
	Hotkey, ~Wheelup, EQE_VScrUp
	hotkey, esc, EscapeHandle

	OnMessage(0x200, "WM_MOUSEMOVE")

	WM_SETCURSOR := 0x0020
	CHAND := DllCall("User32.dll\LoadCursor", "Ptr", NULL, "Int", 32649, "UPtr")
	OnMessage(WM_SETCURSOR, "SetCursor")
}


;~ ######################################################
;~ #                       Labels                       #
;~ ######################################################



EQE_Startup:				;{
	Critical
	;~ InitialDirCreate()	
	;~ CommonInitialise()
	;~ Load_Options()
	;~ OptionsDirCreate()
return						;}

EQE_MainGuiClose:			;{
	File:= A_AppData "\NPC Engineer\NPC Engineer.ini"
	Stub:= "Paths"
	IniWrite, %ProjectPath%, %File%, %Stub%, ProjectPath
	;~ IniWrite, %NPCSavePath%, %File%, %Stub%, NPCSavePath
	win.equipmentengineer:= 0
	If (EQE_Ref = "self") {
		ExitApp
	} Else {
		Gui, EQE_Main:Destroy
		Gui, %EQE_Ref%:show
		StBar(EQE_Ref)
	}
return						;}

EQE_MainUpdate:				;{
	EQE_GetVars()
	If 	(Instr(equip.WeaponProp, "Ammunition")) OR (Instr(equip.WeaponProp, "Thrown")) OR (Instr(equip.WeaponProp, "Range")) {
		GuiControl, EQE_Main:Enable, % EQE_Hwnd.WeaponNormalRangeText
		GuiControl, EQE_Main:Enable, % EQE_Hwnd.WeaponNormalRange
		GuiControl, EQE_Main:Enable, % EQE_Hwnd.WeaponLongRangeText
		GuiControl, EQE_Main:Enable, % EQE_Hwnd.WeaponLongRange
	} Else {
		GuiControl, EQE_Main:Disable, % EQE_Hwnd.WeaponNormalRangeText
		GuiControl, EQE_Main:Disable, % EQE_Hwnd.WeaponNormalRange
		GuiControl, EQE_Main:Disable, % EQE_Hwnd.WeaponLongRangeText
		GuiControl, EQE_Main:Disable, % EQE_Hwnd.WeaponLongRange
	}
	If equip.WeaponOtherTextTick {
		GuiControl, EQE_Main:Enable, % EQE_Hwnd.WeaponOtherText
	} Else {
		GuiControl, EQE_Main:Disable, % EQE_Hwnd.WeaponOtherText
	}
	if Equip.NoID {
		GuiControl, EQE_Main:Enable, % EQE_Hwnd.NoIDNotes
		GuiControl, EQE_Main:Enable, % EQE_Hwnd.NoIDNotesText
	} Else {
		GuiControl, EQE_Main:Disable, % EQE_Hwnd.NoIDNotes
		GuiControl, EQE_Main:Disable, % EQE_Hwnd.NoIDNotesText
	}
	EQE_RH_Box()
return						;}

EQE_Project_Manage:			;{
	Critical
	Gui, EQE_Main:+disabled
	ProjectEngineer("EQE_Main")
return						;}

Manage_EQE_JSON:			;{
	Critical
	If (ProjectLive != 1) {
		MsgBox, 16, No Project, You must load a project *.ini`nto manipulate equipment in its JSON file., 3
		gosub, Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			EQE_GUIJSON()
			Gui, EQE_Main:+disabled
			Gui, EQE_JSON:Show, w320 h210, Edit or Delete equipment in the JSON file
		} else {
			MsgBox, 16, Engineer Suite Parser only, This function can only be carried out whilst using the Engineer Suite Parser., 3
		}
	}
 return						;}

EQE_JSON_Cancel:
EQE_JSONGuiClose:			;{
	Gui, EQE_Main:-disabled
	Gui, EQE_JSON:Destroy
return						;}

EQE_JSON_Choose:			;{
	JSONtemp:= Gget(EQE_Hwnd.EQE_JSONChoose)
	JSON_EQE_Name:= ""
	For a, b in EQP.object()
	{
		if (EQP[a].name == JSONtemp) {
			JSON_EQE_Name:= a
		}
	}
	JSON_This_Text:= EQP[JSON_EQE_Name].Name Chr(10)
	JSON_This_Text .= EQP[JSON_EQE_Name].type "|" EQP[JSON_EQE_Name].subtype
	Gset(EQE_Hwnd.EQE_JSONselected, JSON_This_Text)
 return						;}

EQE_JSON_Del:				;{
	If JSON_EQE_Name {
		EQP.delete(JSON_EQE_Name)
		EQP.save(true)
		equipment_list:= "|Choose an item from the JSON file||"
		For a, b in EQP.object()
		{
			equipment_list:= equipment_list EQP[a].name "|"
		}
		temp:= EQE_Hwnd.EQE_JSONChoose
		GuiControl, , %temp%, %equipment_list%
		JSON_EQE_Name:= ""
		Gset(EQE_Hwnd.EQE_JSONselected, JSON_EQE_Name)
		EQE_RH_Box()
	}
return						;}

EQE_JSON_Edit:				;{
	;~ If JSON_Sp_Name {
		;~ Edit_Sp_JSON(JSON_Sp_Name)
		;~ Gui, SPE_Main:-disabled
		;~ Gui, SPE_JSON:Destroy
	;~ }
return						;}

EQE_text:					;{
	tempsp:= Tokenise(ET1.GetRTF(False))
	StringReplace, tempsp, tempsp, <p></p>, , All
	StringReplace, tempsp, tempsp, `r`n`r`n, `r`n, All
	equip.text:= RegexReplace(tempsp, "^\s+|\s+$" )

	EQE_RH_Box()
return						;}

New_Equipment:				;{
	For a,b in Equip {
		Equip[a]:= ""
	}
	Equip.type:= "Adventuring Gear"
	Equip.subtype:= "Ammunition"
	Equip.ArmorStealth:= "-"
	
	Equip.WeaponType:= "Melee Weapon Attack"
	Equip.WeaponDamageDie:= 6
	Equip.WeaponDamageType:= "slashing"
	Equip.WeaponBDamageDie:= 6
	Equip.WeaponBDamageAdd:= 0
	Equip.WeaponPrReroll:= 0
	Equip.WeaponPrCritRange:= 20
	
	Equip.CostUnit:= "gp"
	Equip.SpeedUnit:= "ft."

	Gosub EQE_ClearImage

	Equip.FGcat:= Modname
	Equip.Locked:= 1
	EQE_SetVars()
	EQE_ScrollPoint:= 0
	EQEScrollEnd:= 0
	EQE_ItemType()
return						;}

Open_Equipment:				;{
	if EquipModSaveDir {
		SpModSaveDir:= "\" Modname
		TempDest:= EquipPath . SpModSaveDir . "\"
		Ifnotexist, %TempDest% 
			FileCreateDir, %TempDest% 
	} Else {
		SpModSaveDir:= ""
	TempDest:= EquipPath
	}
	EQE_LoadPause:= 1
	TempWorkingDir:= A_WorkingDir
	FileSelectFile, SelectedFile, 2, %TempDest%, Load Equipment Item, (*.eqp)
	if (FileExist(SelectedFile)) {
		Equip:= ObjLoad(SelectedFile)
		If !equip.FGcat
			equip.FGcat:= Modname
		Gset(EQE_Hwnd.type, Equip.type)
		temp:= Equip.subtype
		EQE_ItemType()
		Equip.subtype:= temp
		temp:= ""
		if !Equip.AddTitle
			Equip.AddTitle:= 0
		EQE_SetVars()
		Gosub Load_EQE_Image
		SetWorkingDir %TempWorkingDir%
		EquipSavePath:= SelectedFile
		EQE_LoadPause:= 0
		EQE_RH_Box()
	}
return						;}

Save_Equipment:				;{
	If equip.filename {
		ClearAllBut(equip.type)
		if EquipModSaveDir {
			SpModSaveDir:= "\" Modname
			TempDest:= EquipPath . SpModSaveDir . "\"
			Ifnotexist, %TempDest% 
				FileCreateDir, %TempDest% 
		} Else {
			SpModSaveDir:= ""
		}			
		TempWorkingDir:= A_WorkingDir
		SelectedFile:= EquipPath . SpModSaveDir . "\" Equip.filename ".eqp"
		If FileExist(SelectedFile)
			FileDelete, %SelectedFile%
		EQE_SavePic()
		sz:= ObjDump(SelectedFile, Equip)
		SetWorkingDir %TempWorkingDir%
		Toast(Equip.name " saved successfully.")
	}
return						;}

Next_Equipment:				;{
	if (Mod_Parser == 1) {
		EQPNameTemp:= Gget(EQE_Hwnd.name)
		FlagTemp:= 0
		olda:= ""
		For a, b in eqp.object()
		{
			if flagtemp {
				stringreplace EquipSavePath, EquipSavePath, %olda%.EQP, %a%.EQP
				Equip:= ObjLoad(EquipSavePath)
				If !equip.FGcat
					equip.FGcat:= Modname
				Gset(EQE_Hwnd.type, Equip.type)
				temp:= Equip.subtype
				EQE_ItemType()
				Equip.subtype:= temp
				temp:= ""
				if !Equip.AddTitle
					Equip.AddTitle:= 0
				EQE_SetVars()
				Gosub Load_EQE_Image
				FlagTemp:= ""
				olda:= ""
				EQPNameTemp:= ""
				return
			}
			if (EQP[a].name = EQPNameTemp) {
				FlagTemp:= 1
				olda:= a
			}
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This Item is not in the current Project., 2
	} else {
		MsgBox, 16, Engineer Suite parser only, This function can only be carried out whilst using Engineer Suite's parser., 3
	}
return						;}

Prev_Equipment:				;{
	if (Mod_Parser == 1) {
		EQPNameTemp:= Gget(EQE_Hwnd.name)
		FlagTemp:= 0
		olda:= ""
		For a, b in EQP.object()
		{
			if (A_Index = 1)
				olda:= a
			if (EQP[a].name = EQPNameTemp) {
				FlagTemp:= 1
			}
			if flagtemp {
				stringreplace EquipSavePath, EquipSavePath, %a%.EQP, %olda%.EQP
				Equip:= ObjLoad(EquipSavePath)
				If !equip.FGcat
					equip.FGcat:= Modname
				Gset(EQE_Hwnd.type, Equip.type)
				temp:= Equip.subtype
				EQE_ItemType()
				Equip.subtype:= temp
				temp:= ""
				if !Equip.AddTitle
					Equip.AddTitle:= 0
				EQE_SetVars()
				Gosub Load_EQE_Image
				FlagTemp:= ""
				olda:= ""
				EQPNameTemp:= ""
				return
			}
			olda:= a
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This Item is not in the current Project., 2
	} else {
		MsgBox, 16, NPC Engineer Parser only, This function can only be carried out whilst using NPC Engineer's Parser., 3
	}
return						;}

EQE_MakePack:				;{

return						;}

EQE_Image:					;{
	TempImagePath:= Equip.imagepath
	FileSelectFile, TempImagePath, , , Select an image., (*.jpg)
	If (FileExist(TempImagePath)) {
		Equip.imagepath:= TempImagePath
	} else {
		return
	}
Load_EQE_Image:
	hBM := LoadPicture( Equip.imagepath )
	IfEqual, hBM, 0, Return

	BITMAP := getHBMinfo( hBM )                                ; Extract Width andh height of image 
	New := ScaleRect( BITMAP.Width, BITMAP.Height, 586, 396 )  ; Derive best-fit W x H for source image 

	DllCall( "DeleteObject", "Ptr",hBM )                       ; Delete Image handle ...         
	hBM := LoadPicture( Equip.imagepath, "GDI+ w" New.W . " h" . New.H )  ; ..and get a new one with correct W x H

	GuiControl, EQE_Main:, % EQE_Hwnd.jpeg,  *w0 *h0 HBITMAP:%hBM%

	Equip.ImageLink:= 1
	Gset(EQE_Hwnd.ImageLink, Equip.ImageLink)
	EQE_RH_Box()
return						;}

EQE_ClearImage:				;{
	;~ NPCImage:= ""
	GuiControl, EQE_Main:hide, % EQE_Hwnd.jpeg
	GuiControl, , % EQE_Hwnd.jpeg
	GuiControl, EQE_Main:show, % EQE_Hwnd.jpeg
	Equip.imagepath:= ""
	Equip.ImageLink:= 0
	Gset(EQE_Hwnd.ImageLink, Equip.ImageLink)
	EQE_RH_Box()
return						;}

Import_EQE_Text:			;{
	;~ SP_GUI_Import()
	;~ SP_Cap_Text:= ""
	;~ SP_Fix_text:= ""
	;~ SP_Clipimp:= ""
	;~ Spell_Backup()
	;~ GuiControl, SPE_Import:, SP_Cap_text
	;~ SP_Graphical("IMspell", ImScrollPoint)
	;~ Gui, SPE_Main:+disabled
	;~ Gui, SPE_Import:Show, w990 h550, Text Import
return						;}

;~ SPE_Import_Cancel:
;~ SPE_ImportGuiClose:			;{
	;~ If SP_Clipimp
		;~ Clipboard:= SP_Clipimp
	;~ Spell_Restore()
	;~ Spell_InjectVars()
	;~ SPE_RH_Box()
	;~ Gui, SPE_Main:-disabled
	;~ Gui, SPE_Import:Destroy
;~ return						;}

;~ SPE_Import_Delete:			;{
	;~ SP_Cap_Text:= ""
	;~ SP_Fix_text:= ""
	;~ If SP_Clipimp
		;~ Clipboard:= SP_Clipimp
	;~ GuiControl, SPE_Import:, SP_Cap_text
	;~ GuiControl, SPE_Import:, SP_Fix_text
	Getvars_Main()
;~ return						;}

;~ SPE_Import_Append:			;{
	;~ GUI, SPE_Import:submit, NoHide
	;~ SP_Cap_text:= SP_Cap_Text . Clipboard
	;~ SP_Clipimp:= Clipboard
	;~ Clipboard:= ""
	;~ SP_Cap_text := RegExReplace(SP_Cap_text, "\R", "`r`n") 
	;~ SP_Cap_text:= RegExReplace(SP_Cap_text,"\s*$","") ; remove trailing newlines
	;~ SP_Cap_text:= SP_Cap_text Chr(13) Chr(10)
	;~ GuiControl, SPE_Import:, SP_Cap_Text, %SP_Cap_Text%
	;~ SP_WorkingString:= SP_Cap_Text
	;~ SPMainLoop(SP_WorkingString)
	;~ GuiControl, SPE_Import:Focus, SP_Cap_text
	;~ Send ^{End}
;~ return						;}

;~ SPE_Import_Return:			;{
	;~ Critical
	;~ If SP_Clipimp
		;~ Clipboard:= SP_Clipimp
	;~ Gui, SPE_Main:-disabled
	;~ Gui, SPE_Import:Destroy
	;~ Spell_InjectVars()
	;~ SPE_RH_Box()
	;~ notify:= spell.name " imported successfully."
	;~ Toast(notify)
;~ return						;}

;~ SPE_Import_Update_Output:	;{
	;~ GUI, SPE_Import:submit, NoHide
	;~ If SP_Cap_Text {
		;~ SP_WorkingString:= SP_Cap_Text
		;~ StringReplace, SP_WorkingString, SP_WorkingString,`n,`r`n, All
		;~ SPMainLoop(SP_WorkingString)
		;~ GuiControl, SPE_Import:Focus, SP_Cap_text
	;~ }
;~ return						;}



;~ ######################################################
;~ #                   Function List.                   #
;~ ######################################################

EQE_RH_Box() {
	global
	local qc
	Critical
	If !EQE_LoadPause {
		EQE_ScrollEnd:= EQE_VP.document.body.scrollHeight - 500
		If (EQE_ScrollEnd < 0) {
			EQE_ScrollEnd:= 0
		}
		EQE_Graphical("EQE_VP", EQE_ScrollPoint)
		Gui, EQE_Main:Default
		WinTNPC:= "Item: " . equip.name
		If Modname {
			qc:= eqp.SetCapacity(0)
			if !qc
				qc:= 0
			SB_SetText(" " Modname " (" qc " items)", 1)
		}
		NameTemp:= Gget(EQE_Hwnd.name)
		FlagTemp:= 0
		For a, b in eqp.object()
		{
			if (eqp[a].name = NameTemp)
				FlagTemp:= 1
		}
		If FlagTemp
			GuiControl,, % EQE_Hwnd.append, Update Project
		else
			GuiControl,, % EQE_Hwnd.append, Add to Project
		Gui, EQE_Main:Show
	}
}	

EQE_Initialise() {
	global
	Equip:= {}
	EQE_Hwnd:= {}
	bullets:= []
	EQE_ScrollPoint:= 0
	ImScrollPoint:= 0
	flags:=[]
	flags.project:= 0
}

EQE_MainLoop(RawSpell) {
	;~ global
	;~ Critical
	;~ SpCommonProblems(RawSpell)
	;~ SpGetText(RawSpell)
	
	;~ IMScrollEnd:= IMSpell.document.body.scrollHeight - 500
	;~ If (IMScrollEnd < 0) {
		;~ IMScrollEnd:= 0
	;~ }
	;~ SP_Graphical("IMspell", ImScrollPoint)
}
	
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |               Import Functions               |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

EQE_CommonProblems(chunk) {
	
}
	
EQE_GetText(chunk) {
	global spell
	
	;name
	xxfound:= RegExMatch(chunk, "OU)(.*)`r`n", match)
	If xxfound {
		choppity:= match.value(0)
		Stringreplace chunk, chunk, %choppity%, ,
		choppity:= RegExReplace(choppity,"\s*$","") ; remove trailing newlines
		choppity:= RegExReplace(choppity,"^\s*","") ; remove leading newlines
		spell.name:= choppity
	}
	
	;level, school, ritual
	xxfound:= RegExMatch(chunk, "OU)(.*)`r`n", match)
	If xxfound {
		choppity:= match.value(0)
		Stringreplace chunk, chunk, %choppity%, ,
		choppity:= RegExReplace(choppity,"\s*$","") ; remove trailing newlines
		choppity:= RegExReplace(choppity,"^\s*","") ; remove leading newlines
		If instr(choppity, "(ritual)") {
			Stringreplace choppity, choppity, (ritual), 
			spell.ritual:= 1
		} else {
			spell.ritual:= 0
		}
		
		Fp:= RegExMatch(choppity, "i)(abjuration|conjuration|divination|enchantment|evocation|illusion|necromancy|transmutation)" , school)
		StringReplace choppity, choppity, %school%, 
		spell.school:= school

		If instr(choppity, "cantrip") {
			Stringreplace choppity, choppity, cantrip, 
			spell.level:= "cantrip"
		}
		StringReplace, choppity, choppity, -level, %A_Space%level, all
		If instr(choppity, " level"){
			Fp:= RegExMatch(choppity, "i)(1st level|2nd level|3rd level|4th level|5th level|6th level|7th level|8th level|9th level)", lvl)
			spell.level:= lvl
		}
	}
	
	;casting time
	xxfound:= RegExMatch(chunk, "OU)Casting Time:(.*)`r`n", match)
	If xxfound {
		choppity:= match.value(0)
		Stringreplace chunk, chunk, %choppity%, ,
		choppity:= RegExReplace(choppity,"Casting Time: ","")
		choppity:= RegExReplace(choppity,"\s*$","") ; remove trailing newlines
		choppity:= RegExReplace(choppity,"^\s*","") ; remove leading newlines
		spell.castingtime:= choppity
	}
	
	;range
	xxfound:= RegExMatch(chunk, "OU)Range:(.*)`r`n", match)
	If xxfound {
		choppity:= match.value(0)
		Stringreplace chunk, chunk, %choppity%, ,
		choppity:= RegExReplace(choppity,"Range: ","")
		choppity:= RegExReplace(choppity,"\s*$","") ; remove trailing newlines
		choppity:= RegExReplace(choppity,"^\s*","") ; remove leading newlines
		spell.range:= choppity
	}
	
	;components
	xxfound:= RegExMatch(chunk, "OU)Components:(.*)`r`n", match)
	If xxfound {
		choppity:= match.value(0)
		Stringreplace chunk, chunk, %choppity%, ,
		choppity:= RegExReplace(choppity,"Components: ","")
		choppity:= RegExReplace(choppity,"\s*$","") ; remove trailing newlines
		choppity:= RegExReplace(choppity,"^\s*","") ; remove leading newlines
		yyfound:= RegExMatch(choppity, "OU)\((.*)\)", match)
		If yyfound {
			compnt:= match.value(0)
			Stringreplace choppity, choppity, %compnt%, ,
			Stringreplace compnt, compnt, (, ,
			Stringreplace compnt, compnt, ), ,
			spell.component:= compnt
		}
		If instr(choppity, "V") {
			spell.verbal:= 1
		} else {
			spell.verbal:= 0
		}
		If instr(choppity, "S") {
			spell.somatic:= 1
		} else {
			spell.somatic:= 0
		}
		If instr(choppity, "M") {
			spell.material:= 1
		} else {
			spell.material:= 0
		}
	}
	
	;duration
	xxfound:= RegExMatch(chunk, "OU)Duration:(.*)`r`n", match)
	If xxfound {
		choppity:= match.value(0)
		Stringreplace chunk, chunk, %choppity%, ,
		choppity:= RegExReplace(choppity,"Duration: ","")
		choppity:= RegExReplace(choppity,"\s*$","") ; remove trailing newlines
		choppity:= RegExReplace(choppity,"^\s*","") ; remove leading newlines
		spell.duration:= choppity
	}
	
	Format_Chunk(chunk)
	
	;higher levels
	chunk:= RegExReplace(chunk, "m)^At Higher Levels(:|.)", "XXXAt Higher Levels:" )
	xxfound:= RegExMatch(chunk, "O)XXXAt Higher Levels:(.*)", match)
	If xxfound {
		choppity:= match.value(0)
		Stringreplace chunk, chunk, %choppity%, ,
		choppity:= RegExReplace(choppity,"XXXAt Higher Levels: ","")
		choppity:= RegExReplace(choppity,"\s*$","") ; remove trailing newlines
		choppity:= RegExReplace(choppity,"^\s*","") ; remove leading newlines
		spell.higherlevel:= choppity
	}
	
	;body text
	chunk:= RegExReplace(chunk,"\s*$","") ; remove trailing newlines
	chunk:= RegExReplace(chunk,"^\s*","") ; remove leading newlines
	spell.text:= chunk
	spell.rtf:= "{\rtf1\ansi\deff0{\fonttbl{\f0\fnil Arial;}}{\colortbl `;\red0\green0\blue0;}\pard\sa80\cf1\f0\fs20\par " chunk "}"
}

EQE_backup() {
	Global
	spellBU:= objfullyclone(spell)
	ImScrollPoint:= 0
	spell:= {}
}

EQE_Restore() {
	Global
	spell:= objfullyclone(spellBU)
	spellBU:= ""
}



;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |            Input/Output Functions            |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

EQE_Append() {
	Global
	If !Equip.Name {
		return
	}
	If (ProjectLive != 1) or !IsObject(EQP) {
		MsgBox, 16, No Project, The following must be true to add items to a project:`n`n * You have created a new project or loaded a project *.ini.`n * You have enabled items by clicking the checkbox.
		gosub, EQE_Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			EQE_EPAppend()
		;~ } else if (Mod_Parser == 2) {
			;~ EQE_Par5e_Append()
		;~ } else if (Mod_Parser == 3) {
			;~ SpFG5EP_Append()
		}
		EQE_RH_Box()
	}
}

EQE_EPAppend() {
	global
	EQE_JSONFile()
	JSON_Ob_Exist:= ""
	For a, b in EQP.object()
	{
		if (a == equip.filename) {
			JSON_Ob_Exist:= a
		}
	}

	If JSON_Ob_Exist {
		tempname:= EQP[JSON_Ob_Exist].name
		MsgBox, 292, Overwrite Equipment Item, The Item '%tempname%' already exists in the project's JSON file.`nDo you wish to overwrite it with this data? This is unrecoverable!
		IfMsgBox Yes
		{
			EQP.delete(JSON_Ob_Exist)
			EQP.fill(ObjEQP)
			EQP.save(true)

			notify:= Equip.Name " updated in " ModName "."
			Toast(notify)
		}
	} else {
		EQP.fill(ObjEQP)
		EQP.save(true)

		notify:= Equip.Name " added to " ModName "."
		Toast(notify)
	}
	
}

EQE_SavePic() {
	Global
	if EquipCopyPics {
		If (Equip.ImagePath and Equip.filename) {
			Ifexist, % Equip.ImagePath
			{
				ThumbDest:= EquipPath . SpModSaveDir . "\" . Equip.filename . ".*"
				FileCopy, % Equip.ImagePath, %ThumbDest%, 1
				Equip.ImagePath:= EquipPath . SpModSaveDir . "\" . Equip.filename . ".jpg"
			}
		}
	}		
}

EQE_ImportWeapon() {
	Global WeapDB, EQE_Hwnd, Equip
	WName:= Gget(EQE_Hwnd.WeaponImport)
	JSON_wp_Name:= ""
	For a, b in WeapDB.object()
	{
		if (a = WName) {
			JSON_wp_Name:= a
		}
	}
	If JSON_wp_Name {
		Equip.name:= JSON_wp_Name
		Equip.WeaponType:= WeapDB[JSON_wp_Name].WeaponType " Weapon Attack"
		Equip.WeaponToHit:= WeapDB[JSON_wp_Name].ToHit
		Equip.WeaponReach:= WeapDB[JSON_wp_Name].Reach
		Equip.WeaponNormalRange:= WeapDB[JSON_wp_Name].RangeNormal
		Equip.WeaponLongRange:= WeapDB[JSON_wp_Name].RangeLong
		Equip.WeaponTarget:= WeapDB[JSON_wp_Name].Target
		Equip.WeaponDamageNumber:= WeapDB[JSON_wp_Name].DiceNumber
		Equip.WeaponDamageDie:= WeapDB[JSON_wp_Name].DiceType
		Equip.WeaponDamageBonus:= WeapDB[JSON_wp_Name].DamageBonus
		Equip.WeaponDamageType:= WeapDB[JSON_wp_Name].DamageType

		Gset(EQE_Hwnd.NoID, "")
		Gset(EQE_Hwnd.Cost, "")
		Gset(EQE_Hwnd.Weight, "")

		Gset(EQE_Hwnd.name, Equip.name)
		Gset(EQE_Hwnd.WeaponDamageNumber, Equip.WeaponDamageNumber)
		Gset(EQE_Hwnd.WeaponDamageDie, Equip.WeaponDamageDie)
		Gset(EQE_Hwnd.WeaponDamageBonus, Equip.WeaponDamageBonus)
		Gset(EQE_Hwnd.WeaponDamageType, Equip.WeaponDamageType)
		Gset(EQE_Hwnd.WeaponNormalRange, Equip.WeaponNormalRange)
		Gset(EQE_Hwnd.WeaponLongRange, Equip.WeaponLongRange)

		Gset(EQE_Hwnd.WeaponType, Equip.WeaponType)
		Gset(EQE_Hwnd.WeaponToHit, Equip.WeaponToHit)
		Gset(EQE_Hwnd.WeaponReach, Equip.WeaponReach)
		Gset(EQE_Hwnd.WeaponTarget, Equip.WeaponTarget)
		
		Equip.WeaponBDamageAdd:= WeapDB[JSON_wp_Name].AddBonus
		If !(Equip.WeaponBDamageAdd = 1)
			Equip.WeaponBDamageAdd:= 0
		Gset(EQE_Hwnd.WeaponBDamageAdd, Equip.WeaponBDamageAdd)
		EQE_BonusDamage()
		If Equip.WeaponBDamageAdd {
			Equip.WeaponBDamageNumber:= WeapDB[JSON_wp_Name].BonusDiceNumber
			Equip.WeaponBDamageDie:= WeapDB[JSON_wp_Name].BonusDiceType
			Equip.WeaponBDamageBonus:= WeapDB[JSON_wp_Name].BonusDamageBonus
			Equip.WeaponBDamageType:= WeapDB[JSON_wp_Name].BonusDamageType
			Gset(EQE_Hwnd.WeaponBDamageNumber, Equip.WeaponBDamageNumber)
			Gset(EQE_Hwnd.WeaponBDamageDie, Equip.WeaponBDamageDie)
			Gset(EQE_Hwnd.WeaponBDamageBonus, Equip.WeaponBDamageBonus)
			Gset(EQE_Hwnd.WeaponBDamageType, Equip.WeaponBDamageType)
		} else {
			Gset(EQE_Hwnd.WeaponBDamageNumber, "")
			Gset(EQE_Hwnd.WeaponBDamageDie, "")
			Gset(EQE_Hwnd.WeaponBDamageBonus, "")
			Gset(EQE_Hwnd.WeaponBDamageType, "")
		}

		Equip.WeaponOtherTextTick:= WeapDB[JSON_wp_Name].AddOtherText
		If !(Equip.WeaponOtherTextTick = 1)
			Equip.WeaponOtherTextTick:= 0
		Gset(EQE_Hwnd.WeaponOtherTextTick, Equip.WeaponOtherTextTick)
		If equip.WeaponOtherTextTick {
			GuiControl, EQE_Main:Enable, % EQE_Hwnd.WeaponOtherText
		} Else {
			GuiControl, EQE_Main:Disable, % EQE_Hwnd.WeaponOtherText
		}

		If equip.WeaponOtherTextTick {
			Equip.WeaponOtherText:= WeapDB[JSON_wp_Name].OtherText
			Gset(EQE_Hwnd.WeaponOtherText, Equip.WeaponOtherText)
		} else {
			Gset(EQE_Hwnd.WeaponOtherText, "")
		}
		
		list:= ""
		jack:= WeapDB[JSON_wp_Name].Magic
		If (jack = 1)
			list .= "Magic, "
		If Instr(WeapDB[JSON_wp_Name].WeaponType, "Ranged")
			list .= "Ammunition, "
		jack:= WeapDB[JSON_wp_Name].Versatile
		If (jack = 1)
			list .= "Versatile"
		If (SubStr(list, -1) = ", ")
			list:= SubStr(list, 1, -2)

		tempvar2:= "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile"
		Loop, Parse, list, `,, %A_Space%
		{
			Stringreplace, tempvar2, tempvar2, %A_LoopField%, %A_LoopField%||	
		}
		Stringreplace, tempvar2, tempvar2, |||, ||, All
		Gset(EQE_Hwnd.WeaponProp, tempvar2)
		
		list:= ""
		jack:= WeapDB[JSON_wp_Name].Silver
		If (jack = 1)
			list .= "Silver, "
		jack:= WeapDB[JSON_wp_Name].Adamantine
		If (jack = 1)
			list .= "Adamantine, "
		jack:= WeapDB[JSON_wp_Name].CFIron
		If (jack = 1)
			list .= "Cold-forged iron"
		If (SubStr(list, -1) = ", ")
			list:= SubStr(list, 1, -2)
		
		tempvar2:= "|Silver|Adamantine|Cold-forged iron"
		Loop, Parse, list, `,, %A_Space%
		{
			Stringreplace, tempvar2, tempvar2, %A_LoopField%, %A_LoopField%||	
		}
		Stringreplace, tempvar2, tempvar2, |||, ||, All
		Gset(EQE_Hwnd.WeaponMat, tempvar2)
	}	
}

EQE_ExportWeapon() {
	Global WeapDB, EQE_Hwnd, Equip
	EQE_GetVars()
	
	JSON_wp_Name:= ""
	Temp_Type:= Equip.WeaponType
	StringReplace, Temp_Type, Temp_Type, %A_Space%Weapon Attack, ,
	For a, b in WeapDB.object()
	{
		if (a == Equip.name) {
			JSON_wp_Name:= a
			WeapDB[JSON_wp_Name].WeaponType:= Trim(Temp_Type)
			WeapDB[JSON_wp_Name].ToHit:= Trim(Equip.WeaponToHit)
			WeapDB[JSON_wp_Name].Reach:= Trim(Equip.WeaponReach)
			WeapDB[JSON_wp_Name].RangeNormal:= Trim(Equip.WeaponNormalRange)
			WeapDB[JSON_wp_Name].RangeLong:= Trim(Equip.WeaponLongRange)
			WeapDB[JSON_wp_Name].Target:= Trim(Equip.WeaponTarget)
			WeapDB[JSON_wp_Name].DiceNumber:= Trim(Equip.WeaponDamageNumber)
			WeapDB[JSON_wp_Name].DiceType:= Trim(Equip.WeaponDamageDie)
			WeapDB[JSON_wp_Name].DamageBonus:= Trim(Equip.WeaponDamageBonus)
			WeapDB[JSON_wp_Name].DamageType:= Trim(Equip.WeaponDamageType)
			WeapDB[JSON_wp_Name].AddBonus:= Trim(Equip.WeaponBDamageAdd)
			if WeaponBDamageAdd {
				WeapDB[JSON_wp_Name].BonusDiceNumber:= Trim(Equip.WeaponBDamageNumber)
				WeapDB[JSON_wp_Name].BonusDiceType:= Trim(Equip.WeaponBDamageDie)
				WeapDB[JSON_wp_Name].BonusDamageBonus:= Trim(Equip.WeaponBDamageBonus)
				WeapDB[JSON_wp_Name].BonusDamageType:= Trim(Equip.WeaponBDamageType)
			} else {
				WeapDB[JSON_wp_Name].BonusDiceNumber:= ""
				WeapDB[JSON_wp_Name].BonusDiceType:= ""
				WeapDB[JSON_wp_Name].BonusDamageBonus:= ""
				WeapDB[JSON_wp_Name].BonusDamageType:= ""
			}
			WeapDB[JSON_wp_Name].AddOtherText:= Trim(Equip.WeaponOtherTextTick)
			if WeaponOtherTextTick {
				WeapDB[JSON_wp_Name].OtherText:= Trim(Equip.WeaponOtherText)
			} else {
				WeapDB[JSON_wp_Name].OtherText:= ""
			}
			If(Instr(Equip.WeaponProp, "Versatile"))
				WeapDB[JSON_wp_Name].Versatile:= 1
			If(Instr(Equip.WeaponMat, "Silver"))
				WeapDB[JSON_wp_Name].Silver:= 1
			If(Instr(Equip.WeaponMat, "Adamantine"))
				WeapDB[JSON_wp_Name].Adamantine:= 1
			If(Instr(Equip.WeaponMat, "Cold-forged iron"))
				WeapDB[JSON_wp_Name].CFIron:= 1
			If(Instr(Equip.WeaponProp, "Magic"))
				WeapDB[JSON_wp_Name].Magic:= 1
			WeapDB.save(true)
			notify:= Equip.Name " exported to Weapons list."
			Toast(notify)
		}
	}
	
	If !JSON_wp_Name {
		WA_Name:= Equip.name
		Armoury:= {}
		Armoury[WA_Name]:= {}
		Armoury[WA_Name].WeaponType:= Trim(Temp_Type)
		Armoury[WA_Name].ToHit:= Trim(Equip.WeaponToHit)
		Armoury[WA_Name].Reach:= Trim(Equip.WeaponReach)
		Armoury[WA_Name].RangeNormal:= Trim(Equip.WeaponNormalRange)
		Armoury[WA_Name].RangeLong:= Trim(Equip.WeaponLongRange)
		Armoury[WA_Name].Target:= Trim(Equip.WeaponTarget)
		Armoury[WA_Name].DiceNumber:= Trim(Equip.WeaponDamageNumber)
		Armoury[WA_Name].DiceType:= Trim(Equip.WeaponDamageDie)
		Armoury[WA_Name].DamageBonus:= Trim(Equip.WeaponDamageBonus)
		Armoury[WA_Name].DamageType:= Trim(Equip.WeaponDamageType)
		Armoury[WA_Name].AddBonus:= Trim(Equip.WeaponBDamageAdd)
		if WeaponBDamageAdd {
			Armoury[WA_Name].BonusDiceNumber:= Trim(Equip.WeaponBDamageNumber)
			Armoury[WA_Name].BonusDiceType:= Trim(Equip.WeaponBDamageDie)
			Armoury[WA_Name].BonusDamageBonus:= Trim(Equip.WeaponBDamageBonus)
			Armoury[WA_Name].BonusDamageType:= Trim(Equip.WeaponBDamageType)
		} else {
			Armoury[WA_Name].BonusDiceNumber:= ""
			Armoury[WA_Name].BonusDiceType:= ""
			Armoury[WA_Name].BonusDamageBonus:= ""
			Armoury[WA_Name].BonusDamageType:= ""
		}
		Armoury[WA_Name].AddOtherText:= Trim(Equip.WeaponOtherTextTick)
		if WeaponOtherTextTick {
			Armoury[WA_Name].OtherText:= Trim(Equip.WeaponOtherText)
		} else {
			Armoury[WA_Name].OtherText:= ""
		}
		If(Instr(Equip.WeaponProp, "Versatile"))
			Armoury[WA_Name].Versatile:= 1
		If(Instr(Equip.WeaponMat, "Silver"))
			Armoury[WA_Name].Silver:= 1
		If(Instr(Equip.WeaponMat, "Adamantine"))
			Armoury[WA_Name].Adamantine:= 1
		If(Instr(Equip.WeaponMat, "Cold-forged iron"))
			Armoury[WA_Name].CFIron:= 1
		If(Instr(Equip.WeaponProp, "Magic"))
			Armoury[WA_Name].Magic:= 1
		WeapDB.fill(Armoury)
		WeapDB.save(true)
		notify:= Equip.Name " exported to Weapons list."
		Toast(notify)
	}
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |           General Purpose Functions          |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

EQE_GetVars() {
	global
	local tempvar
	Equip.name:= Gget(EQE_Hwnd.name)
	Equip.NoID:= Gget(EQE_Hwnd.NoID)
	Equip.NoIDNotes:= Gget(EQE_Hwnd.NoIDNotes)
	Equip.type:= Gget(EQE_Hwnd.type)
	Equip.subtype:= Gget(EQE_Hwnd.subtype)
	Equip.cost:= Gget(EQE_Hwnd.cost)
	Equip.costUnit:= Gget(EQE_Hwnd.costUnit)
	Equip.Weight:= Gget(EQE_Hwnd.Weight)
	
	Equip.ArmorAC:= Gget(EQE_Hwnd.ArmorAC)
	Equip.ArmorStrength:= Gget(EQE_Hwnd.ArmorStrength)
	Equip.ArmorStealth:= Gget(EQE_Hwnd.ArmorStealth)
	
	Equip.Speed:= Gget(EQE_Hwnd.Speed)
	Equip.SpeedUnit:= Gget(EQE_Hwnd.SpeedUnit)
	Equip.MountCapacity:= Gget(EQE_Hwnd.MountCapacity)
	
	Equip.WeaponDamageNumber:= Gget(EQE_Hwnd.WeaponDamageNumber)
	Equip.WeaponDamageDie:= Gget(EQE_Hwnd.WeaponDamageDie)
	Equip.WeaponDamageBonus:= Gget(EQE_Hwnd.WeaponDamageBonus)
	Equip.WeaponDamageType:= Gget(EQE_Hwnd.WeaponDamageType)
	Equip.WeaponBDamageNumber:= Gget(EQE_Hwnd.WeaponBDamageNumber)
	Equip.WeaponBDamageDie:= Gget(EQE_Hwnd.WeaponBDamageDie)
	Equip.WeaponBDamageBonus:= Gget(EQE_Hwnd.WeaponBDamageBonus)
	Equip.WeaponBDamageType:= Gget(EQE_Hwnd.WeaponBDamageType)
	Equip.WeaponBDamageAdd:= Gget(EQE_Hwnd.WeaponBDamageAdd)
	Equip.WeaponNormalRange:= Gget(EQE_Hwnd.WeaponNormalRange)
	Equip.WeaponLongRange:= Gget(EQE_Hwnd.WeaponLongRange)
	Equip.WeaponPrReroll:= Gget(EQE_Hwnd.WeaponPrReroll)
	Equip.WeaponPrCritRange:= Gget(EQE_Hwnd.WeaponPrCritRange)
	
	Equip.WeaponType:= Gget(EQE_Hwnd.WeaponType)
	Equip.WeaponToHit:= Gget(EQE_Hwnd.WeaponToHit)
	Equip.WeaponReach:= Gget(EQE_Hwnd.WeaponReach)
	Equip.WeaponTarget:= Gget(EQE_Hwnd.WeaponTarget)
	Equip.WeaponOtherTextTick:= Gget(EQE_Hwnd.WeaponOtherTextTick)
	Equip.WeaponOtherText:= Gget(EQE_Hwnd.WeaponOtherText)
	
	
	Equip.jpeg:= Gget(EQE_Hwnd.jpeg)
	Equip.ImageLink:= Gget(EQE_Hwnd.ImageLink)
	Equip.AddTitle:= Gget(EQE_Hwnd.AddTitle)
	Equip.Artist:= Gget(EQE_Hwnd.Artist)
	Equip.ArtistLink:= Gget(EQE_Hwnd.ArtistLink)
	Equip.FGcat:= Gget(EQE_Hwnd.FGcat)
	Equip.Locked:= Gget(EQE_Hwnd.Locked)
	
	tempvar:= Equip.name
	StringLower tempvar, tempvar
	tempvar:= RegExReplace(tempvar, "[^a-zA-Z0-9]", "")
	Equip.filename:= tempvar
	
	tempvar:= Gget(EQE_Hwnd.WeaponProp)
	Equip.WeaponProp:= ""
	Loop, Parse, Tempvar, |
	{
		Equip.WeaponProp .= A_Loopfield ", "
	}
	If (SubStr(Equip.WeaponProp, -1) = ", ")
		Equip.WeaponProp:= SubStr(Equip.WeaponProp, 1, -2)

	tempvar:= Gget(EQE_Hwnd.WeaponMat)
	Equip.WeaponMat:= ""
	Loop, Parse, Tempvar, |
	{
		Equip.WeaponMat .= A_Loopfield ", "
	}
	If (SubStr(Equip.WeaponMat, -1) = ", ")
		Equip.WeaponMat:= SubStr(Equip.WeaponMat, 1, -2)
}

EQE_SetVars() {
	Global
	local tempvar, tempvar2
	Gset(EQE_Hwnd.name, Equip.name)
	Gset(EQE_Hwnd.NoID, Equip.NoID)
	Gset(EQE_Hwnd.NoIDNotes, Equip.NoIDNotes)
	Gset(EQE_Hwnd.type, Equip.type)
	Gset(EQE_Hwnd.subtype, Equip.subtype)
	Gset(EQE_Hwnd.cost, Equip.cost)
	Gset(EQE_Hwnd.costUnit, Equip.costUnit)
	Gset(EQE_Hwnd.Weight, Equip.Weight)
	
	Gset(EQE_Hwnd.ArmorAC, Equip.ArmorAC)
	Gset(EQE_Hwnd.ArmorStrength, Equip.ArmorStrength)
	Gset(EQE_Hwnd.ArmorStealth, Equip.ArmorStealth)
	
	Gset(EQE_Hwnd.Speed, Equip.Speed)
	Gset(EQE_Hwnd.SpeedUnit, Equip.SpeedUnit)
	Gset(EQE_Hwnd.MountCapacity, Equip.MountCapacity)
	
	Gset(EQE_Hwnd.WeaponDamageNumber, Equip.WeaponDamageNumber)
	Gset(EQE_Hwnd.WeaponDamageDie, Equip.WeaponDamageDie)
	Gset(EQE_Hwnd.WeaponDamageBonus, Equip.WeaponDamageBonus)
	Gset(EQE_Hwnd.WeaponDamageType, Equip.WeaponDamageType)
	Gset(EQE_Hwnd.WeaponBDamageNumber, Equip.WeaponBDamageNumber)
	Gset(EQE_Hwnd.WeaponBDamageDie, Equip.WeaponBDamageDie)
	Gset(EQE_Hwnd.WeaponBDamageBonus, Equip.WeaponBDamageBonus)
	Gset(EQE_Hwnd.WeaponBDamageType, Equip.WeaponBDamageType)
	Gset(EQE_Hwnd.WeaponBDamageAdd, Equip.WeaponBDamageAdd)
	Gset(EQE_Hwnd.WeaponNormalRange, Equip.WeaponNormalRange)
	Gset(EQE_Hwnd.WeaponLongRange, Equip.WeaponLongRange)
	Gset(EQE_Hwnd.WeaponPrReroll, Equip.WeaponPrReroll)
	Gset(EQE_Hwnd.WeaponPrCritRange, Equip.WeaponPrCritRange)

	Gset(EQE_Hwnd.WeaponType, Equip.WeaponType)
	Gset(EQE_Hwnd.WeaponToHit, Equip.WeaponToHit)
	Gset(EQE_Hwnd.WeaponReach, Equip.WeaponReach)
	Gset(EQE_Hwnd.WeaponTarget, Equip.WeaponTarget)
	Gset(EQE_Hwnd.WeaponOtherText, Equip.WeaponOtherText)
	Gset(EQE_Hwnd.WeaponOtherTextTick, Equip.WeaponOtherTextTick)

	Gset(EQE_Hwnd.jpeg, Equip.jpeg)
	Gset(EQE_Hwnd.ImageLink, Equip.ImageLink)
	Gset(EQE_Hwnd.AddTitle, Equip.AddTitle)
	Gset(EQE_Hwnd.Artist, Equip.Artist)
	Gset(EQE_Hwnd.ArtistLink, Equip.ArtistLink)
	Gset(EQE_Hwnd.FGcat, Equip.FGcat)
	Gset(EQE_Hwnd.Locked, Equip.Locked)

	tempvar:= Equip.WeaponProp
	tempvar2:= "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile"
	Loop, Parse, tempvar, `,, %A_Space%
	{
		Stringreplace, tempvar2, tempvar2, %A_LoopField%, %A_LoopField%||	
	}
	Stringreplace, tempvar2, tempvar2, |||, ||, All
	Gset(EQE_Hwnd.WeaponProp, tempvar2)
	
	tempvar:= Equip.WeaponMat
	tempvar2:= "|Silver|Adamantine|Cold-forged iron"
	Loop, Parse, tempvar, `,, %A_Space%
	{
		Stringreplace, tempvar2, tempvar2, %A_LoopField%, %A_LoopField%||	
	}
	Stringreplace, tempvar2, tempvar2, |||, ||, All
	Gset(EQE_Hwnd.WeaponMat, tempvar2)
}

;~ SpPaste() {
	;~ Clipsave:= Clipboard
	;~ Cliptext = %Clipboard%
	;~ Format_Chunk(Cliptext)
	;~ Common_Problems(Cliptext)
	;~ Clipboard:= cliptext
	;~ GuiControl, Focus, % ST1.HWND
	;~ send ^v
	;~ Clipboard:= Clipsave
;~ }

EQE_SetTT() {
	Global
	if !isobject(hTTip){
		hTTip:= []
	}
	hTTip[EQE_Hwnd.name]:= "Enter a name for your table."
}

EQE_ItemType() {
	global
	local EQtype
	EQtype:= Gget(EQE_Hwnd.type)
	
	If (EQtype = "Adventuring Gear") {
		Hideall()
		ClearAllBut("Adventuring Gear")
		GuiControl, Show, % EQE_hwnd.Weighttext
		GuiControl, Show, % EQE_hwnd.Weight
		GuiControl, Show, % EQE_hwnd.WeightUnit
		GuiControl,, % EQE_hwnd.subtype, |Ammunition|Arcane Focus|Druidic Focus|Holy Symbol|Equipment Kits|Equipment Packs|Standard||
	}
	If (EQtype = "Armor") {
		Hideall()
		ClearAllBut("Armor")
		GuiControl, Show, % EQE_hwnd.Weighttext
		GuiControl, Show, % EQE_hwnd.Weight
		GuiControl, Show, % EQE_hwnd.WeightUnit
		GuiControl, Show, % EQE_hwnd.ArmorACtext
		GuiControl, Show, % EQE_hwnd.ArmorAC
		GuiControl, Show, % EQE_hwnd.ArmorStrengthtext
		GuiControl, Show, % EQE_hwnd.ArmorStrength
		GuiControl, Show, % EQE_hwnd.ArmorStealthtext
		GuiControl, Show, % EQE_hwnd.ArmorStealth
		GuiControl,, % EQE_hwnd.subtype, |Light Armor||Medium Armor|Heavy Armor|Shield
	}
	If (EQtype = "Weapon") {
		Hideall()
		ClearAllBut("Weapon")
		GuiControl, Show, % EQE_hwnd.Weighttext
		GuiControl, Show, % EQE_hwnd.Weight
		GuiControl, Show, % EQE_hwnd.WeightUnit
		GuiControl, Show, % EQE_hwnd.WeaponTypeText
		GuiControl, Show, % EQE_hwnd.WeaponType
		GuiControl, Show, % EQE_hwnd.WeaponDamageText
		GuiControl, Show, % EQE_hwnd.WeaponDamageNumberText
		GuiControl, Show, % EQE_hwnd.WeaponDamageNumber
		GuiControl, Show, % EQE_hwnd.WeaponDamageDieText
		GuiControl, Show, % EQE_hwnd.WeaponDamageDText
		GuiControl, Show, % EQE_hwnd.WeaponDamageDie
		GuiControl, Show, % EQE_hwnd.WeaponDamagePlusText
		GuiControl, Show, % EQE_hwnd.WeaponDamageBonusText
		GuiControl, Show, % EQE_hwnd.WeaponDamageBonus
		GuiControl, Show, % EQE_hwnd.WeaponDamageTypeText
		GuiControl, Show, % EQE_hwnd.WeaponDamageType
		GuiControl, Show, % EQE_hwnd.WeaponBDamageText
		GuiControl, Show, % EQE_hwnd.WeaponBDamageNumber
		GuiControl, Show, % EQE_hwnd.WeaponBDamageDText
		GuiControl, Show, % EQE_hwnd.WeaponBDamageDie
		GuiControl, Show, % EQE_hwnd.WeaponBDamagePlusText
		GuiControl, Show, % EQE_hwnd.WeaponBDamageBonus
		GuiControl, Show, % EQE_hwnd.WeaponBDamageType
		GuiControl, Show, % EQE_hwnd.WeaponBDamageAdd
		GuiControl, Show, % EQE_hwnd.WeaponNormalRangeText
		GuiControl, Show, % EQE_hwnd.WeaponNormalRange
		GuiControl, Show, % EQE_hwnd.WeaponLongRangeText
		GuiControl, Show, % EQE_hwnd.WeaponLongRange
		GuiControl, Show, % EQE_hwnd.WeaponPropText
		GuiControl, Show, % EQE_hwnd.WeaponProp
		GuiControl, Show, % EQE_hwnd.WeaponMatText
		GuiControl, Show, % EQE_hwnd.WeaponMat
		GuiControl, Show, % EQE_hwnd.WeaponPrRerollText
		GuiControl, Show, % EQE_hwnd.WeaponPrReroll
		GuiControl, Show, % EQE_hwnd.WeaponPrCritRangeText
		GuiControl, Show, % EQE_hwnd.WeaponPrCritRange
		GuiControl, Show, % EQE_hwnd.AddToNpcButton
		GuiControl, Show, % EQE_hwnd.WeaponGroupbox
		GuiControl, Show, % EQE_hwnd.WeaponIntroText
		GuiControl, Show, % EQE_hwnd.WeaponToHit
		GuiControl, Show, % EQE_hwnd.WeaponToHitText
		GuiControl, Show, % EQE_hwnd.WeaponReach
		GuiControl, Show, % EQE_hwnd.WeaponReachText
		GuiControl, Show, % EQE_hwnd.WeaponTarget
		GuiControl, Show, % EQE_hwnd.WeaponTargetText
		GuiControl, Show, % EQE_hwnd.WeaponOtherTextTick
		GuiControl, Show, % EQE_hwnd.WeaponOtherText
		GuiControl, Show, % EQE_hwnd.WeaponImport
		GuiControl, Show, % EQE_hwnd.WeaponImportText
		GuiControl,, % EQE_hwnd.subtype, |Simple Melee Weapons||Simple Ranged Weapons|Martial Melee Weapons|Martial Ranged Weapons
	}
	If (EQtype = "Tools") {
		Hideall()
		ClearAllBut("Tools")
		GuiControl, Show, % EQE_hwnd.Weighttext
		GuiControl, Show, % EQE_hwnd.Weight
		GuiControl, Show, % EQE_hwnd.WeightUnit
		GuiControl,, % EQE_hwnd.subtype, |Artisan's Tools||Kits/Sets|Gaming Set|Musical Instrument
	}
	If (EQtype = "Mounts and Other Animals") {
		Hideall()
		ClearAllBut("Mounts and Other Animals")
		GuiControl, Show, % EQE_hwnd.Speedtext
		GuiControl, Show, % EQE_hwnd.Speed
		GuiControl, Show, % EQE_hwnd.SpeedUnit
		GuiControl, Show, % EQE_hwnd.MountCapacitytext
		GuiControl, Show, % EQE_hwnd.MountCapacity
		GuiControl, Show, % EQE_hwnd.MountCapacityUnit
		GuiControl,, % EQE_hwnd.subtype, |Mounts & Animals||
		GuiControl,, % EQE_hwnd.SpeedUnit, |ft.||mph
		
	}
	If (EQtype = "Tack, Harness, and Drawn Vehicles") {
		Hideall()
		ClearAllBut("Tack, Harness, and Drawn Vehicles")
		GuiControl, Show, % EQE_hwnd.Weighttext
		GuiControl, Show, % EQE_hwnd.Weight
		GuiControl, Show, % EQE_hwnd.WeightUnit
		GuiControl,, % EQE_hwnd.subtype, |Tack & Harness||Drawn Vehicles|Saddle
	}
	If (EQtype = "Waterborne Vehicles") {
		Hideall()
		ClearAllBut("Waterborne Vehicles")
		GuiControl, Show, % EQE_hwnd.Speedtext
		GuiControl, Show, % EQE_hwnd.Speed
		GuiControl, Show, % EQE_hwnd.SpeedUnit
		GuiControl,, % EQE_hwnd.subtype, |Surface Vehicles||
		GuiControl,, % EQE_hwnd.SpeedUnit, |ft.|mph||
	}
	If (EQtype = "Treasure") {
		Hideall()
		ClearAllBut("Treasure")
		GuiControl, Show, % EQE_hwnd.Weighttext
		GuiControl, Show, % EQE_hwnd.Weight
		GuiControl, Show, % EQE_hwnd.WeightUnit
		GuiControl,, % EQE_hwnd.subtype, |Art Objects (25 gp)||Art Objects (250 gp)|Art Objects (750 gp)|Art Objects (2,500 gp)|Art Objects (7,500 gp)|Gemstones (10 gp)|Gemstones (50 gp)|Gemstones (100 gp)|Gemstones (500 gp)|Gemstones (1,000 gp)|Gemstones (5,000 gp)
	}
	EQE_Subtype()
}

Hideall() {
	global
	GuiControl, Hide, % EQE_hwnd.Weighttext
	GuiControl, Hide, % EQE_hwnd.Weight
	GuiControl, Hide, % EQE_hwnd.WeightUnit

	GuiControl, Hide, % EQE_hwnd.ArmorACtext
	GuiControl, Hide, % EQE_hwnd.ArmorAC
	GuiControl, Hide, % EQE_hwnd.ArmorStrengthtext
	GuiControl, Hide, % EQE_hwnd.ArmorStrength
	GuiControl, Hide, % EQE_hwnd.ArmorStealthtext
	GuiControl, Hide, % EQE_hwnd.ArmorStealth

	GuiControl, Hide, % EQE_hwnd.Speedtext
	GuiControl, Hide, % EQE_hwnd.Speed
	GuiControl, Hide, % EQE_hwnd.SpeedUnit
	GuiControl, Hide, % EQE_hwnd.MountCapacitytext
	GuiControl, Hide, % EQE_hwnd.MountCapacity
	GuiControl, Hide, % EQE_hwnd.MountCapacityUnit

	GuiControl, Hide, % EQE_hwnd.WeaponTypeText
	GuiControl, Hide, % EQE_hwnd.WeaponType
	GuiControl, Hide, % EQE_hwnd.WeaponDamageText
	GuiControl, Hide, % EQE_hwnd.WeaponDamageNumberText
	GuiControl, Hide, % EQE_hwnd.WeaponDamageNumber
	GuiControl, Hide, % EQE_hwnd.WeaponDamageDieText
	GuiControl, Hide, % EQE_hwnd.WeaponDamageDText
	GuiControl, Hide, % EQE_hwnd.WeaponDamageDie
	GuiControl, Hide, % EQE_hwnd.WeaponDamagePlusText
	GuiControl, Hide, % EQE_hwnd.WeaponDamageBonusText
	GuiControl, Hide, % EQE_hwnd.WeaponDamageBonus
	GuiControl, Hide, % EQE_hwnd.WeaponDamageTypeText
	GuiControl, Hide, % EQE_hwnd.WeaponDamageType
	GuiControl, Hide, % EQE_hwnd.WeaponBDamageText
	GuiControl, Hide, % EQE_hwnd.WeaponBDamageNumber
	GuiControl, Hide, % EQE_hwnd.WeaponBDamageDText
	GuiControl, Hide, % EQE_hwnd.WeaponBDamageDie
	GuiControl, Hide, % EQE_hwnd.WeaponBDamagePlusText
	GuiControl, Hide, % EQE_hwnd.WeaponBDamageBonus
	GuiControl, Hide, % EQE_hwnd.WeaponBDamageType
	GuiControl, Hide, % EQE_hwnd.WeaponBDamageAdd
	GuiControl, Hide, % EQE_hwnd.WeaponNormalRangeText
	GuiControl, Hide, % EQE_hwnd.WeaponNormalRange
	GuiControl, Hide, % EQE_hwnd.WeaponLongRangeText
	GuiControl, Hide, % EQE_hwnd.WeaponLongRange
	GuiControl, Hide, % EQE_hwnd.WeaponPropText
	GuiControl, Hide, % EQE_hwnd.WeaponProp
	GuiControl, Hide, % EQE_hwnd.WeaponMatText
	GuiControl, Hide, % EQE_hwnd.WeaponMat
	GuiControl, Hide, % EQE_hwnd.WeaponPrRerollText
	GuiControl, Hide, % EQE_hwnd.WeaponPrReroll
	GuiControl, Hide, % EQE_hwnd.WeaponPrCritRangeText
	GuiControl, Hide, % EQE_hwnd.WeaponPrCritRange
	GuiControl, Hide, % EQE_hwnd.AddToNpcButton
	GuiControl, Hide, % EQE_hwnd.WeaponGroupbox
	GuiControl, Hide, % EQE_hwnd.WeaponIntroText
	GuiControl, Hide, % EQE_hwnd.WeaponToHit
	GuiControl, Hide, % EQE_hwnd.WeaponToHitText
	GuiControl, Hide, % EQE_hwnd.WeaponReach
	GuiControl, Hide, % EQE_hwnd.WeaponReachText
	GuiControl, Hide, % EQE_hwnd.WeaponTarget
	GuiControl, Hide, % EQE_hwnd.WeaponTargetText
	GuiControl, Hide, % EQE_hwnd.WeaponOtherTextTick
	GuiControl, Hide, % EQE_hwnd.WeaponOtherText
	GuiControl, Hide, % EQE_hwnd.WeaponImport
	GuiControl, Hide, % EQE_hwnd.WeaponImportText
}

ClearAllBut(nm) {
	Global Equip, EQE_Hwnd
	If (nm = "Adventuring Gear") {
		Equip.ArmorAC:= ""
		Equip.ArmorStrength:= ""
		Equip.ArmorStealth:= "-"
		
		Equip.Speed:= ""
		Equip.MountCapacity:= ""
		
		Equip.WeaponType:= "Melee Weapon Attack"
		Equip.WeaponDamageDie:= 6
		Equip.WeaponDamageType:= "slashing"
		Equip.WeaponBDamageDie:= 6
		Equip.WeaponBDamageAdd:= 0
		Equip.WeaponPrReroll:= 0
		Equip.WeaponPrCritRange:= 20
		Equip.WeaponDamageNumber:= ""
		Equip.WeaponDamageBonus:= ""
		Equip.WeaponBDamageNumber:= ""
		Equip.WeaponBDamageBonus:= ""
		Equip.WeaponBDamageType:= ""
		Equip.WeaponNormalRange:= ""
		Equip.WeaponLongRange:= ""
		
		Gset(EQE_Hwnd.ArmorAC, Equip.ArmorAC)
		Gset(EQE_Hwnd.ArmorStrength, Equip.ArmorStrength)
		Gset(EQE_Hwnd.ArmorStealth, Equip.ArmorStealth)
		
		Gset(EQE_Hwnd.Speed, Equip.Speed)
		Gset(EQE_Hwnd.MountCapacity, Equip.MountCapacity)
		
		Gset(EQE_Hwnd.WeaponType, Equip.WeaponType)
		Gset(EQE_Hwnd.WeaponDamageNumber, Equip.WeaponDamageNumber)
		Gset(EQE_Hwnd.WeaponDamageDie, Equip.WeaponDamageDie)
		Gset(EQE_Hwnd.WeaponDamageBonus, Equip.WeaponDamageBonus)
		Gset(EQE_Hwnd.WeaponDamageType, Equip.WeaponDamageType)
		Gset(EQE_Hwnd.WeaponBDamageNumber, Equip.WeaponBDamageNumber)
		Gset(EQE_Hwnd.WeaponBDamageDie, Equip.WeaponBDamageDie)
		Gset(EQE_Hwnd.WeaponBDamageBonus, Equip.WeaponBDamageBonus)
		Gset(EQE_Hwnd.WeaponBDamageType, Equip.WeaponBDamageType)
		Gset(EQE_Hwnd.WeaponBDamageAdd, Equip.WeaponBDamageAdd)
		Gset(EQE_Hwnd.WeaponNormalRange, Equip.WeaponNormalRange)
		Gset(EQE_Hwnd.WeaponLongRange, Equip.WeaponLongRange)
		Gset(EQE_Hwnd.WeaponPrReroll, Equip.WeaponPrReroll)
		Gset(EQE_Hwnd.WeaponPrCritRange, Equip.WeaponPrCritRange)
		
		Gset(EQE_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(EQE_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Armor") {
		Equip.Speed:= ""
		Equip.MountCapacity:= ""
		
		Equip.WeaponType:= "Melee Weapon Attack"
		Equip.WeaponDamageDie:= 6
		Equip.WeaponDamageType:= "slashing"
		Equip.WeaponBDamageDie:= 6
		Equip.WeaponBDamageAdd:= 0
		Equip.WeaponPrReroll:= 0
		Equip.WeaponPrCritRange:= 20
		Equip.WeaponDamageNumber:= ""
		Equip.WeaponDamageBonus:= ""
		Equip.WeaponBDamageNumber:= ""
		Equip.WeaponBDamageBonus:= ""
		Equip.WeaponBDamageType:= ""
		Equip.WeaponNormalRange:= ""
		Equip.WeaponLongRange:= ""
		Gset(EQE_Hwnd.Weight, Equip.Weight)
		
		Gset(EQE_Hwnd.Speed, Equip.Speed)
		Gset(EQE_Hwnd.MountCapacity, Equip.MountCapacity)
		
		Gset(EQE_Hwnd.WeaponType, Equip.WeaponType)
		Gset(EQE_Hwnd.WeaponDamageNumber, Equip.WeaponDamageNumber)
		Gset(EQE_Hwnd.WeaponDamageDie, Equip.WeaponDamageDie)
		Gset(EQE_Hwnd.WeaponDamageBonus, Equip.WeaponDamageBonus)
		Gset(EQE_Hwnd.WeaponDamageType, Equip.WeaponDamageType)
		Gset(EQE_Hwnd.WeaponBDamageNumber, Equip.WeaponBDamageNumber)
		Gset(EQE_Hwnd.WeaponBDamageDie, Equip.WeaponBDamageDie)
		Gset(EQE_Hwnd.WeaponBDamageBonus, Equip.WeaponBDamageBonus)
		Gset(EQE_Hwnd.WeaponBDamageType, Equip.WeaponBDamageType)
		Gset(EQE_Hwnd.WeaponBDamageAdd, Equip.WeaponBDamageAdd)
		Gset(EQE_Hwnd.WeaponNormalRange, Equip.WeaponNormalRange)
		Gset(EQE_Hwnd.WeaponLongRange, Equip.WeaponLongRange)
		Gset(EQE_Hwnd.WeaponPrReroll, Equip.WeaponPrReroll)
		Gset(EQE_Hwnd.WeaponPrCritRange, Equip.WeaponPrCritRange)
		
		Gset(EQE_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(EQE_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Weapon") {
		Equip.ArmorAC:= ""
		Equip.ArmorStrength:= ""
		Equip.ArmorStealth:= "-"
		
		Equip.Speed:= ""
		Equip.MountCapacity:= ""
		Gset(EQE_Hwnd.Weight, Equip.Weight)
		
		Gset(EQE_Hwnd.ArmorAC, Equip.ArmorAC)
		Gset(EQE_Hwnd.ArmorStrength, Equip.ArmorStrength)
		Gset(EQE_Hwnd.ArmorStealth, Equip.ArmorStealth)
		
		Gset(EQE_Hwnd.Speed, Equip.Speed)
		Gset(EQE_Hwnd.MountCapacity, Equip.MountCapacity)
	}
	If (nm = "Tools") {
		Equip.ArmorAC:= ""
		Equip.ArmorStrength:= ""
		Equip.ArmorStealth:= "-"
		
		Equip.Speed:= ""
		Equip.MountCapacity:= ""
		
		Equip.WeaponType:= "Melee Weapon Attack"
		Equip.WeaponDamageDie:= 6
		Equip.WeaponDamageType:= "slashing"
		Equip.WeaponBDamageDie:= 6
		Equip.WeaponBDamageAdd:= 0
		Equip.WeaponPrReroll:= 0
		Equip.WeaponPrCritRange:= 20
		Equip.WeaponDamageNumber:= ""
		Equip.WeaponDamageBonus:= ""
		Equip.WeaponBDamageNumber:= ""
		Equip.WeaponBDamageBonus:= ""
		Equip.WeaponBDamageType:= ""
		Equip.WeaponNormalRange:= ""
		Equip.WeaponLongRange:= ""
		
		Gset(EQE_Hwnd.ArmorAC, Equip.ArmorAC)
		Gset(EQE_Hwnd.ArmorStrength, Equip.ArmorStrength)
		Gset(EQE_Hwnd.ArmorStealth, Equip.ArmorStealth)
		
		Gset(EQE_Hwnd.Speed, Equip.Speed)
		Gset(EQE_Hwnd.MountCapacity, Equip.MountCapacity)
		
		Gset(EQE_Hwnd.WeaponType, Equip.WeaponType)
		Gset(EQE_Hwnd.WeaponDamageNumber, Equip.WeaponDamageNumber)
		Gset(EQE_Hwnd.WeaponDamageDie, Equip.WeaponDamageDie)
		Gset(EQE_Hwnd.WeaponDamageBonus, Equip.WeaponDamageBonus)
		Gset(EQE_Hwnd.WeaponDamageType, Equip.WeaponDamageType)
		Gset(EQE_Hwnd.WeaponBDamageNumber, Equip.WeaponBDamageNumber)
		Gset(EQE_Hwnd.WeaponBDamageDie, Equip.WeaponBDamageDie)
		Gset(EQE_Hwnd.WeaponBDamageBonus, Equip.WeaponBDamageBonus)
		Gset(EQE_Hwnd.WeaponBDamageType, Equip.WeaponBDamageType)
		Gset(EQE_Hwnd.WeaponBDamageAdd, Equip.WeaponBDamageAdd)
		Gset(EQE_Hwnd.WeaponNormalRange, Equip.WeaponNormalRange)
		Gset(EQE_Hwnd.WeaponLongRange, Equip.WeaponLongRange)
		Gset(EQE_Hwnd.WeaponPrReroll, Equip.WeaponPrReroll)
		Gset(EQE_Hwnd.WeaponPrCritRange, Equip.WeaponPrCritRange)
		
		Gset(EQE_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(EQE_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Mounts and Other Animals") {
		Equip.Weight:= ""
		
		Equip.ArmorAC:= ""
		Equip.ArmorStrength:= ""
		Equip.ArmorStealth:= "-"
		
		Equip.WeaponType:= "Melee Weapon Attack"
		Equip.WeaponDamageDie:= 6
		Equip.WeaponDamageType:= "slashing"
		Equip.WeaponBDamageDie:= 6
		Equip.WeaponBDamageAdd:= 0
		Equip.WeaponPrReroll:= 0
		Equip.WeaponPrCritRange:= 20
		Equip.WeaponDamageNumber:= ""
		Equip.WeaponDamageBonus:= ""
		Equip.WeaponBDamageNumber:= ""
		Equip.WeaponBDamageBonus:= ""
		Equip.WeaponBDamageType:= ""
		Equip.WeaponNormalRange:= ""
		Equip.WeaponLongRange:= ""

		Gset(EQE_Hwnd.Weight, Equip.Weight)
		
		Gset(EQE_Hwnd.ArmorAC, Equip.ArmorAC)
		Gset(EQE_Hwnd.ArmorStrength, Equip.ArmorStrength)
		Gset(EQE_Hwnd.ArmorStealth, Equip.ArmorStealth)
		
		Gset(EQE_Hwnd.WeaponType, Equip.WeaponType)
		Gset(EQE_Hwnd.WeaponDamageNumber, Equip.WeaponDamageNumber)
		Gset(EQE_Hwnd.WeaponDamageDie, Equip.WeaponDamageDie)
		Gset(EQE_Hwnd.WeaponDamageBonus, Equip.WeaponDamageBonus)
		Gset(EQE_Hwnd.WeaponDamageType, Equip.WeaponDamageType)
		Gset(EQE_Hwnd.WeaponBDamageNumber, Equip.WeaponBDamageNumber)
		Gset(EQE_Hwnd.WeaponBDamageDie, Equip.WeaponBDamageDie)
		Gset(EQE_Hwnd.WeaponBDamageBonus, Equip.WeaponBDamageBonus)
		Gset(EQE_Hwnd.WeaponBDamageType, Equip.WeaponBDamageType)
		Gset(EQE_Hwnd.WeaponBDamageAdd, Equip.WeaponBDamageAdd)
		Gset(EQE_Hwnd.WeaponNormalRange, Equip.WeaponNormalRange)
		Gset(EQE_Hwnd.WeaponLongRange, Equip.WeaponLongRange)
		Gset(EQE_Hwnd.WeaponPrReroll, Equip.WeaponPrReroll)
		Gset(EQE_Hwnd.WeaponPrCritRange, Equip.WeaponPrCritRange)
		
		Gset(EQE_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(EQE_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Tack, Harness, and Drawn Vehicles") {
		Equip.ArmorAC:= ""
		Equip.ArmorStrength:= ""
		Equip.ArmorStealth:= "-"
		
		Equip.Speed:= ""
		Equip.MountCapacity:= ""
		
		Equip.WeaponType:= "Melee Weapon Attack"
		Equip.WeaponDamageDie:= 6
		Equip.WeaponDamageType:= "slashing"
		Equip.WeaponBDamageDie:= 6
		Equip.WeaponBDamageAdd:= 0
		Equip.WeaponPrReroll:= 0
		Equip.WeaponPrCritRange:= 20
		Equip.WeaponDamageNumber:= ""
		Equip.WeaponDamageBonus:= ""
		Equip.WeaponBDamageNumber:= ""
		Equip.WeaponBDamageBonus:= ""
		Equip.WeaponBDamageType:= ""
		Equip.WeaponNormalRange:= ""
		Equip.WeaponLongRange:= ""

		Gset(EQE_Hwnd.ArmorAC, Equip.ArmorAC)
		Gset(EQE_Hwnd.ArmorStrength, Equip.ArmorStrength)
		Gset(EQE_Hwnd.ArmorStealth, Equip.ArmorStealth)
		
		Gset(EQE_Hwnd.Speed, Equip.Speed)
		Gset(EQE_Hwnd.MountCapacity, Equip.MountCapacity)
		
		Gset(EQE_Hwnd.WeaponType, Equip.WeaponType)
		Gset(EQE_Hwnd.WeaponDamageNumber, Equip.WeaponDamageNumber)
		Gset(EQE_Hwnd.WeaponDamageDie, Equip.WeaponDamageDie)
		Gset(EQE_Hwnd.WeaponDamageBonus, Equip.WeaponDamageBonus)
		Gset(EQE_Hwnd.WeaponDamageType, Equip.WeaponDamageType)
		Gset(EQE_Hwnd.WeaponBDamageNumber, Equip.WeaponBDamageNumber)
		Gset(EQE_Hwnd.WeaponBDamageDie, Equip.WeaponBDamageDie)
		Gset(EQE_Hwnd.WeaponBDamageBonus, Equip.WeaponBDamageBonus)
		Gset(EQE_Hwnd.WeaponBDamageType, Equip.WeaponBDamageType)
		Gset(EQE_Hwnd.WeaponBDamageAdd, Equip.WeaponBDamageAdd)
		Gset(EQE_Hwnd.WeaponNormalRange, Equip.WeaponNormalRange)
		Gset(EQE_Hwnd.WeaponLongRange, Equip.WeaponLongRange)
		Gset(EQE_Hwnd.WeaponPrReroll, Equip.WeaponPrReroll)
		Gset(EQE_Hwnd.WeaponPrCritRange, Equip.WeaponPrCritRange)
		
		Gset(EQE_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(EQE_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Waterborne Vehicles") {
		Equip.Weight:= ""
		
		Equip.ArmorAC:= ""
		Equip.ArmorStrength:= ""
		Equip.ArmorStealth:= "-"
		
		Equip.MountCapacity:= ""
		
		Equip.WeaponType:= "Melee Weapon Attack"
		Equip.WeaponDamageDie:= 6
		Equip.WeaponDamageType:= "slashing"
		Equip.WeaponBDamageDie:= 6
		Equip.WeaponBDamageAdd:= 0
		Equip.WeaponPrReroll:= 0
		Equip.WeaponPrCritRange:= 20
		Equip.WeaponDamageNumber:= ""
		Equip.WeaponDamageBonus:= ""
		Equip.WeaponBDamageNumber:= ""
		Equip.WeaponBDamageBonus:= ""
		Equip.WeaponBDamageType:= ""
		Equip.WeaponNormalRange:= ""
		Equip.WeaponLongRange:= ""
		
		Gset(EQE_Hwnd.Weight, Equip.Weight)
		
		Gset(EQE_Hwnd.ArmorAC, Equip.ArmorAC)
		Gset(EQE_Hwnd.ArmorStrength, Equip.ArmorStrength)
		Gset(EQE_Hwnd.ArmorStealth, Equip.ArmorStealth)
		
		Gset(EQE_Hwnd.MountCapacity, Equip.MountCapacity)
		
		Gset(EQE_Hwnd.WeaponType, Equip.WeaponType)
		Gset(EQE_Hwnd.WeaponDamageNumber, Equip.WeaponDamageNumber)
		Gset(EQE_Hwnd.WeaponDamageDie, Equip.WeaponDamageDie)
		Gset(EQE_Hwnd.WeaponDamageBonus, Equip.WeaponDamageBonus)
		Gset(EQE_Hwnd.WeaponDamageType, Equip.WeaponDamageType)
		Gset(EQE_Hwnd.WeaponBDamageNumber, Equip.WeaponBDamageNumber)
		Gset(EQE_Hwnd.WeaponBDamageDie, Equip.WeaponBDamageDie)
		Gset(EQE_Hwnd.WeaponBDamageBonus, Equip.WeaponBDamageBonus)
		Gset(EQE_Hwnd.WeaponBDamageType, Equip.WeaponBDamageType)
		Gset(EQE_Hwnd.WeaponBDamageAdd, Equip.WeaponBDamageAdd)
		Gset(EQE_Hwnd.WeaponNormalRange, Equip.WeaponNormalRange)
		Gset(EQE_Hwnd.WeaponLongRange, Equip.WeaponLongRange)
		Gset(EQE_Hwnd.WeaponPrReroll, Equip.WeaponPrReroll)
		Gset(EQE_Hwnd.WeaponPrCritRange, Equip.WeaponPrCritRange)
		
		Gset(EQE_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(EQE_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Treasure") {
		Equip.ArmorAC:= ""
		Equip.ArmorStrength:= ""
		Equip.ArmorStealth:= "-"
		
		Equip.Speed:= ""
		Equip.MountCapacity:= ""
		
		Equip.WeaponType:= "Melee Weapon Attack"
		Equip.WeaponDamageDie:= 6
		Equip.WeaponDamageType:= "slashing"
		Equip.WeaponBDamageDie:= 6
		Equip.WeaponBDamageAdd:= 0
		Equip.WeaponPrReroll:= 0
		Equip.WeaponPrCritRange:= 20
		Equip.WeaponDamageNumber:= ""
		Equip.WeaponDamageBonus:= ""
		Equip.WeaponBDamageNumber:= ""
		Equip.WeaponBDamageBonus:= ""
		Equip.WeaponBDamageType:= ""
		Equip.WeaponNormalRange:= ""
		Equip.WeaponLongRange:= ""
		
		Gset(EQE_Hwnd.Weight, Equip.Weight)
		
		Gset(EQE_Hwnd.ArmorAC, Equip.ArmorAC)
		Gset(EQE_Hwnd.ArmorStrength, Equip.ArmorStrength)
		Gset(EQE_Hwnd.ArmorStealth, Equip.ArmorStealth)
		
		Gset(EQE_Hwnd.Speed, Equip.Speed)
		Gset(EQE_Hwnd.MountCapacity, Equip.MountCapacity)
		
		Gset(EQE_Hwnd.WeaponType, Equip.WeaponType)
		Gset(EQE_Hwnd.WeaponDamageNumber, Equip.WeaponDamageNumber)
		Gset(EQE_Hwnd.WeaponDamageDie, Equip.WeaponDamageDie)
		Gset(EQE_Hwnd.WeaponDamageBonus, Equip.WeaponDamageBonus)
		Gset(EQE_Hwnd.WeaponDamageType, Equip.WeaponDamageType)
		Gset(EQE_Hwnd.WeaponBDamageNumber, Equip.WeaponBDamageNumber)
		Gset(EQE_Hwnd.WeaponBDamageDie, Equip.WeaponBDamageDie)
		Gset(EQE_Hwnd.WeaponBDamageBonus, Equip.WeaponBDamageBonus)
		Gset(EQE_Hwnd.WeaponBDamageType, Equip.WeaponBDamageType)
		Gset(EQE_Hwnd.WeaponBDamageAdd, Equip.WeaponBDamageAdd)
		Gset(EQE_Hwnd.WeaponNormalRange, Equip.WeaponNormalRange)
		Gset(EQE_Hwnd.WeaponLongRange, Equip.WeaponLongRange)
		Gset(EQE_Hwnd.WeaponPrReroll, Equip.WeaponPrReroll)
		Gset(EQE_Hwnd.WeaponPrCritRange, Equip.WeaponPrCritRange)
		
		Gset(EQE_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(EQE_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
}

EQE_Subtype() {
	global
	local EQtype, EQsubtype
	EQtype:= Gget(EQE_Hwnd.type)
	EQsubtype:= Gget(EQE_Hwnd.subtype)
	
	If (EQtype = "Adventuring Gear") {
		If (EQsubtype = "Equipment Kits") or (EQsubtype = "Equipment Packs") {
			GuiControl, Show, % EQE_hwnd.BuildPackButton
		} Else {
			GuiControl, Hide, % EQE_hwnd.BuildPackButton
		}
	} Else {
		GuiControl, Hide, % EQE_hwnd.BuildPackButton
	}
	Equip.subtype:= Gget(EQE_Hwnd.subtype)
	If (equip.subtype = "Light Armor") {
		equip.dexbonus:= "Yes"
	} else if (equip.subtype = "Medium Armor") {
		equip.dexbonus:= "Yes (max 2)"
	} else {
		equip.dexbonus:= "-"
	}
	EQE_RH_Box()
}

EQE_BonusDamage() {
	Global EQE_Hwnd
	WBD:= Gget(EQE_Hwnd.WeaponBDamageAdd)
	GuiControl, EQE_Main:Enable%WBD%, % EQE_Hwnd.WeaponBDamageText
	GuiControl, EQE_Main:Enable%WBD%, % EQE_Hwnd.WeaponBDamageNumber
	GuiControl, EQE_Main:Enable%WBD%, % EQE_Hwnd.WeaponBDamageDText
	GuiControl, EQE_Main:Enable%WBD%, % EQE_Hwnd.WeaponBDamageDie
	GuiControl, EQE_Main:Enable%WBD%, % EQE_Hwnd.WeaponBDamagePlusText
	GuiControl, EQE_Main:Enable%WBD%, % EQE_Hwnd.WeaponBDamageBonus
	GuiControl, EQE_Main:Enable%WBD%, % EQE_Hwnd.WeaponBDamageType
}

EQE_Notes() {
	AddNotes("Equip", "EQE")
}

EQE_Vup() {
	global EQE_VP, EQE_ScrollPoint, EQE_ScrollEnd, EQE_buttonup, EQE_buttondn
	MouseGetPos,,,,ctrl, 2
	while (ctrl=EQE_buttonup && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		EQE_VP.Document.parentWindow.eval("scrollBy(0, -2);")
		EQE_ScrollPoint -= 2
		If (EQE_ScrollPoint < 0) {
			EQE_ScrollPoint:= 0
		}
	}
	while (ctrl=EQE_buttondn && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		EQE_VP.Document.parentWindow.eval("scrollBy(0, 2);")
		EQE_ScrollPoint += 2
		If (EQE_ScrollPoint > EQE_ScrollEnd) {
			EQE_ScrollPoint:= EQE_ScrollEnd
		}
	}
}

EQE_VScrUp() {
	global EQE_VP, IMSpell, EQE_ScrollPoint, IMScrollPoint, EQE_Main, EQE_Import
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" SPE_Main) {
		EQE_VP.Document.parentWindow.eval("scrollBy(0, -50);")
		EQE_ScrollPoint -= 50
		If (EQE_ScrollPoint < 0) {
			EQE_ScrollPoint:= 0
		}
	}
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" SPE_Import) {
		IMSpell.Document.parentWindow.eval("scrollBy(0, -50);")
		IMScrollPoint -= 50
		If (IMScrollPoint < 0) {
			IMScrollPoint:= 0
		}
	}
}

EQE_VScrDwn() {
	global EQE_VP, IMSpell, EQE_ScrollPoint, IMScrollPoint, EQE_ScrollEnd, IMScrollEnd, EQE_Main, EQE_Import
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" SPE_Main) {
		EQE_VP.Document.parentWindow.eval("scrollBy(0, 50);")
		EQE_ScrollPoint += 50
		If (EQE_ScrollPoint > EQE_ScrollEnd) {
			EQE_ScrollPoint:= EQE_ScrollEnd
		}
	}
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" SPE_Import) {
		IMSpell.Document.parentWindow.eval("scrollBy(0, 50);")
		IMScrollPoint += 50
		If (IMScrollPoint > IMScrollEnd) {
			IMScrollPoint:= IMScrollEnd
		}
	}
}

EQE_JSONFile() {
	global Equip, ObjEQP
	ObjEQP:= {}
	ObjName:= Equip.filename
	ObjEQP[ObjName]:= {}
	ObjEQP[ObjName].name:= Equip.name
	ObjEQP[ObjName].type:= Equip.type
	ObjEQP[ObjName].subtype:= Equip.subtype
	ObjEQP[ObjName].NoID:= Equip.NoID
	ObjEQP[ObjName].NoIDNotes:= Equip.NoIDNotes
	ObjEQP[ObjName].cost:= Equip.cost " " Equip.costUnit

	If (Equip.type = "Adventuring Gear") {
		ObjEQP[ObjName].Weight:= Equip.Weight
	}
	If (Equip.type = "Armor") {
		ObjEQP[ObjName].Weight:= Equip.Weight
		ObjEQP[ObjName].AC:= Equip.ArmorAC
		If Equip.ArmorStrength is number {
			ObjEQP[ObjName].armourstr:= "Str " Equip.ArmorStrength
		} Else {
			ObjEQP[ObjName].armourstr:= "-"
		}
		ObjEQP[ObjName].armourstealth:= Equip.ArmorStealth
		ObjEQP[ObjName].armourdexbonus:= Equip.dexbonus
	}
	If (Equip.type = "Weapon") {
		ObjEQP[ObjName].Weight:= Equip.Weight
		If Equip.WeaponDamageNumber {
			ObjEQP[ObjName].Damage:= Equip.WeaponDamageNumber "d" Equip.WeaponDamageDie
			If Equip.WeaponDamageBonus
				ObjEQP[ObjName].Damage .= "+" Equip.WeaponDamageBonus
			ObjEQP[ObjName].Damage .= " " Equip.WeaponDamageType
		}
		If (equip.WeaponProp) AND (equip.WeaponMat) {
			EQEWeapProp:= Equip.WeaponProp ", " equip.WeaponMat
		} Else if (equip.WeaponProp) AND (!equip.WeaponMat) {
			EQEWeapProp:= Equip.WeaponProp
		} Else if !(equip.WeaponProp) AND (equip.WeaponMat) {
			EQEWeapProp:= equip.WeaponMat
		} Else {
			EQEWeapProp:= ""
		}
		EQErange:= "(range " Equip.WeaponNormalRange "/" Equip.WeaponLongRange ")"
		stringreplace, EQEWeapProp, EQEWeapProp, Ammunition, Ammunition %EQErange%
		stringreplace, EQEWeapProp, EQEWeapProp, Thrown, Thrown %EQErange%
		If (Equip.WeaponDamageDie = 4) {
			EQEvers:= "Versatile (1d6)"
		} else if (Equip.WeaponDamageDie = 6) {
			EQEvers:= "Versatile (1d8)"
		} else if (Equip.WeaponDamageDie = 8) {
			EQEvers:= "Versatile (1d10)"
		} else if (Equip.WeaponDamageDie = 10) {
			EQEvers:= "Versatile (1d12)"
		} else if (Equip.WeaponDamageDie = 12) {
			EQEvers:= "Versatile (1d20)"
		}
		stringreplace, EQEWeapProp, EQEWeapProp, Versatile, %EQEvers%
		ObjEQP[ObjName].Properties:= EQEWeapProp
	}
	If (Equip.type = "Tools") {
		ObjEQP[ObjName].Weight:= Equip.Weight
	}
	If (Equip.type = "Mounts and Other Animals") {
		If Equip.Speed
			ObjEQP[ObjName].speed:= Equip.Speed " " Equip.SpeedUnit
		If Equip.MountCapacity
			ObjEQP[ObjName].carryingcapacity:= Equip.MountCapacity " lb."
	}
	If (Equip.type = "Tack, Harness, and Drawn Vehicles") {
		ObjEQP[ObjName].Weight:= Equip.Weight
	}
	If (Equip.type = "Waterborne Vehicles") {
		If Equip.Speed
			ObjEQP[ObjName].speed:= Equip.Speed " " Equip.SpeedUnit
	}
	If (Equip.type = "Treasure") {
		ObjEQP[ObjName].Weight:= Equip.Weight
	}

	ObjEQP[ObjName].jpeg:= Equip.jpeg
	ObjEQP[ObjName].ImageLink:= Equip.ImageLink
	ObjEQP[ObjName].Artist:= Equip.Artist
	ObjEQP[ObjName].ArtistLink:= Equip.ArtistLink
	ObjEQP[ObjName].FGcat:= Equip.FGcat
	ObjEQP[ObjName].Locked:= Equip.Locked
	

	SpText:= "`t`t`t`t`t<description type=""formattedtext"">" Equip.notes

	TKN:= compactSpaces(SpText)
	
	StringReplace, TKN, TKN, </p>`r`n<p>, </p>`n<p>, all
	StringReplace, TKN, TKN, </p>`n<p>, </p>`n`t`t`t`t`t<p>, all
	StringReplace, TKN, TKN, <p>`r`n<p>, <p>, all
	StringReplace, TKN, TKN, <p>`r`n`r`n<ul>, <ul>, all
	StringReplace, TKN, TKN, <p>`r`n<ul>, <ul>, all
	StringReplace, TKN, TKN, <p><ul>, <ul>, all
		
	pos:= 1
	alldone:= 0
	loop {
		frs:= 0
		fre:= 0
		frs:= InStr(TKN, "<i>", , pos)
		fre:= InStr(TKN, "</i>", , pos)
		
		if fre
			pos:= fre + 4
		if (frs and fre) {
			NewStr:= SubStr(TKN, frs, fre-frs+1)
			repstr:= Newstr
			StringReplace, repstr, repstr, </p>`n`t`t`t`t`t<p>, </i></p>`n`t`t`t`t`t<p><i>, all
			StringReplace, TKN, TKN, %NewStr%, %repstr%
		} else {
			alldone:= 1
		}
	} until alldone = 1

	pos:= 1
	alldone:= 0
	loop {
		frs:= 0
		fre:= 0
		frs:= InStr(TKN, "<b>", , pos)
		fre:= InStr(TKN, "</b>", , pos)
		
		if fre
			pos:= fre + 4
		if (frs and fre) {
			NewStr:= SubStr(TKN, frs, fre-frs+1)
			repstr:= Newstr
			StringReplace, repstr, repstr, </p>`n`t`t`t`t`t<p>, </b></p>`n`t`t`t`t`t<p><b>, all
			StringReplace, TKN, TKN, %NewStr%, %repstr%
		} else {
			alldone:= 1
		}
	} until alldone = 1

	pos:= 1
	alldone:= 0
	loop {
		frs:= 0
		fre:= 0
		frs:= InStr(TKN, "<u>", , pos)
		fre:= InStr(TKN, "</u>", , pos)
		
		if fre
			pos:= fre + 4
		if (frs and fre) {
			NewStr:= SubStr(TKN, frs, fre-frs+1)
			repstr:= Newstr
			StringReplace, repstr, repstr, </p>`n`t`t`t`t`t<p>, </u></p>`n`t`t`t`t`t<p><u>, all
			StringReplace, TKN, TKN, %NewStr%, %repstr%
		} else {
			alldone:= 1
		}
	} until alldone = 1

	stringreplace, TKN, TKN, <i><u>, <u><i>, All
	stringreplace, TKN, TKN, <i><b>, <b><i>, All
	stringreplace, TKN, TKN, <b><u>, <u><b>, All
	stringreplace, TKN, TKN, <i><b><u>, <u><b><i>, All

	TKN .= "</description>"
		
	ObjEQP[ObjName].description:= TKN
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                 GUI Functions                |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

EQE_Graphical(UI, Scr) {
	Global EQE_VP, IMspell, Equip, shield
	
	;{ Data for Statblock
	EQEName:= equip.name
	EQENonid:= equip.NoID
	EQEtype:= equip.type
	EQEsubtype:= equip.subtype
	If Equip.cost
		EQEcost:= equip.cost " " equip.CostUnit

	EQEarmourclass:= Equip.ArmorAC
	If Equip.ArmorStrength is number {
		EQEarmourstr:= "Str " Equip.ArmorStrength
	} Else {
		EQEarmourstr:= "-"
	}		
	EQEarmourstealth:= Equip.ArmorStealth
	EQEarmourdexbon:= Equip.dexbonus
	If Equip.Weight
		EQEweight:= equip.weight " lb."
	
	notes:= equip.notes "<br>"
	StringReplace, notes, notes, <frame>, %frtable%, all
	StringReplace, notes, notes, </frame>, %frtablend%, all
	StringReplace, notes, notes, <h>, <span class=`"heading`">, all
	StringReplace, notes, notes, </h>, </span>, all
	
	SpText:= "<p>"
	If (Equip.AddTitle and Equip.Name) {
		SpText .= "<span class=""heading"">" Equip.Name "</span><br>"
	}
	If (Equip.ImageLink and Equip.Name) {
		SpText .=  shield " Image: " Equip.Name "</p>"
	} else {
		SpText .= "</p>"
	}

	SpText .= notes
	SpText:= LinkHTML(SpText)

	If Equip.WeaponDamageNumber {
		EQEWeapDam:= Equip.WeaponDamageNumber "d" Equip.WeaponDamageDie
		If Equip.WeaponDamageBonus
			EQEWeapDam .= "+" Equip.WeaponDamageBonus
		EQEWeapDam .= " " Equip.WeaponDamageType
	}
	
	If (equip.WeaponProp) AND (equip.WeaponMat) {
		EQEWeapProp:= Equip.WeaponProp ", " equip.WeaponMat
	} Else if (equip.WeaponProp) AND (!equip.WeaponMat) {
		EQEWeapProp:= Equip.WeaponProp
	} Else if !(equip.WeaponProp) AND (equip.WeaponMat) {
		EQEWeapProp:= equip.WeaponMat
	} Else {
		EQEWeapProp:= ""
	}
	EQErange:= "(range " Equip.WeaponNormalRange "/" Equip.WeaponLongRange ")"
	stringreplace, EQEWeapProp, EQEWeapProp, Ammunition, Ammunition %EQErange%
	stringreplace, EQEWeapProp, EQEWeapProp, Thrown, Thrown %EQErange%
	If (Equip.WeaponDamageDie = 4) {
		EQEvers:= "Versatile (1d6)"
	} else if (Equip.WeaponDamageDie = 6) {
		EQEvers:= "Versatile (1d8)"
	} else if (Equip.WeaponDamageDie = 8) {
		EQEvers:= "Versatile (1d10)"
	} else if (Equip.WeaponDamageDie = 10) {
		EQEvers:= "Versatile (1d12)"
	} else if (Equip.WeaponDamageDie = 12) {
		EQEvers:= "Versatile (1d20)"
	}
	stringreplace, EQEWeapProp, EQEWeapProp, Versatile, %EQEvers%
	
	If Equip.Speed
		EQEspeed:= Equip.Speed " " Equip.SpeedUnit
	If Equip.MountCapacity
		EQEcarry:= Equip.MountCapacity " lb."

	;}

	#include HTML_Equipment_Engineer.ahk
	HTML_Spell:= css . htmtop
	If (Equip.type = "Adventuring Gear") {
		HTML_Spell .= htmMid1
	}
	If (Equip.type = "Armor") {
		HTML_Spell .= htmMid2
	}
	If (Equip.type = "Weapon") {
		HTML_Spell .= htmMid3
	}
	If (Equip.type = "Tools") {
		HTML_Spell .= htmMid4
	}
	If (Equip.type = "Mounts and Other Animals") {
		HTML_Spell .= htmMid5
	}
	If (Equip.type = "Tack, Harness, and Drawn Vehicles") {
		HTML_Spell .= htmMid6
	}
	If (Equip.type = "Waterborne Vehicles") {
		HTML_Spell .= htmMid7
	}
	If (Equip.type = "Treasure") {
		HTML_Spell .= htmMid8
	}
	HTML_Spell .= htmbottom
	
	
	
	
	stringreplace, HTML_Spell, HTML_Spell, \r, <br>, All

	documentz:= %UI%.Document
	documentz.open()
	documentz.write(HTML_Spell)
	documentz.close()
	%UI%.Document.parentWindow.eval("scrollTo(0, " Scr ");")
}


EQE_PrepareGUI() {
	global
	EQE_GUIMain()
	If (EQE_Ref = "self") {
		GUI_Project()
	}
	EQE_SetTT()
}

EQE_GUIMain()	 {
	global
	local tempy, jsonpath, weaplist
	
	Gui, EQE_Main:-MaximizeBox
	Gui, EQE_Main:+hwndEQE_Main
	Gui, EQE_Main:Color, 0xE2E1E8
	Gui, EQE_Main:font, S10 c000000, Arial
	
; Menu system
;{
	Menu EQE_FileMenu, Add, &New Equipment Item`tCtrl+N, New_Equipment
	Menu EQE_FileMenu, Icon, &New Equipment Item`tCtrl+N, NPC Engineer.dll, 1
	Menu EQE_FileMenu, Add, &Open Equipment Item`tCtrl+O, Open_Equipment
	Menu EQE_FileMenu, Icon, &Open Equipment Item`tCtrl+O, NPC Engineer.dll, 2
	Menu EQE_FileMenu, Add, &Save Equipment Item`tCtrl+S, Save_Equipment
	Menu EQE_FileMenu, Icon, &Save Equipment Item`tCtrl+S, NPC Engineer.dll, 3
	Menu EQE_FileMenu, Add
	;~ Menu EQE_FileMenu, Add, Save as Text, Save_Txt
	;~ Menu EQE_FileMenu, Add, Save as XML, Save_XML
	;~ Menu EQE_FileMenu, Add, Save as HTML, Save_HTML
	;~ Menu EQE_FileMenu, Add, Save as RTF, Save_RTF
	;~ Menu EQE_FileMenu, Add
	;~ Menu EQE_FileMenu, Add, Place BBCode on Clipboard, Copy_BB
	;~ Menu EQE_FileMenu, Add
	Menu EQE_FileMenu, Add, E&xit`tESC, EQE_MainGuiClose
	Menu EQE_FileMenu, Icon, E&xit`tESC, NPC Engineer.dll, 17
	Menu EquipmentEngineerMenu, Add, File, :EQE_FileMenu
	
	Menu EQE_OptionsMenu, Add, &Import Text`tCtrl+I, Import_EQE_Text
	Menu EQE_OptionsMenu, Icon, &Import Text`tCtrl+I, NPC Engineer.dll, 4
	Menu EQE_OptionsMenu, Add, &Create Module `tCtrl+P, ParseProject
	Menu EQE_OptionsMenu, Icon, &Create Module `tCtrl+P, NPC Engineer.dll, 6
	Menu EQE_OptionsMenu, Add
	Menu EQE_OptionsMenu, Add, Manage Categories `tCtrl+K, GUI_Categories
	Menu EQE_OptionsMenu, Icon, Manage Categories `tCtrl+K, NPC Engineer.dll, 25
	Menu EQE_OptionsMenu, Add, Manage Equipment File `tCtrl+M, Manage_EQE_JSON
	Menu EQE_OptionsMenu, Add
	Menu EQE_OptionsMenu, Add, Settings`tF11, GUI_Options
	Menu EQE_OptionsMenu, Icon, Settings`tF11, NPC Engineer.dll, 9
	Menu EquipmentEngineerMenu, Add, Options, :EQE_OptionsMenu
	
	Component_Menu("EQE_ComponentMenu", "equipment")
	Menu EquipmentEngineerMenu, Add, Engineer Suite, :EQE_ComponentMenu
	
	Explorer_Menu("EQE_ExplorerMenu")
	Menu EquipmentEngineerMenu, Add, Directories, :EQE_ExplorerMenu
	
	Backup_Menu("EQE_BackupMenu")
	Menu EquipmentEngineerMenu, Add, Backup, :EQE_BackupMenu

	Help_Menu("EQE_HelpMenu", "Equipment Engineer")
	Menu EquipmentEngineerMenu, Add, Information, :EQE_HelpMenu
	Gui, EQE_Main:Menu, EquipmentEngineerMenu
;}

; Tab 3 system for all Item input
	Gui, EQE_Main:Add, Tab3, x7 y45 w606 h550, Equipment Info|Image

;  ================================================
;  |         GUI for 'equipment info' tab         |
;  ================================================
;{
	Gui, EQE_Main:Tab, 1
	
	Gui, EQE_Main:Add, Text, x15 y83 w85 h17 Right, Name:
	Gui, EQE_Main:Add, Text, x15 y110 w85 h17 Right, Non-ID Name:
	Gui, EQE_Main:Add, Text, x15 y137 w85 h17 Disabled HwndTempy Right, Non-ID Notes:
		EQE_Hwnd.NoIDNotesText:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y80 w300 h23 HwndTempy gEQE_MainUpdate,
		EQE_Hwnd.name:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y107 w300 h23 HwndTempy gEQE_MainUpdate,
		EQE_Hwnd.NoID:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y134 w500 h23 Disabled HwndTempy gEQE_MainUpdate,
		EQE_Hwnd.NoIDNotes:= Tempy
	Gui, EQE_Main:Add, Button, x485 y80 w120 h25 +border -Tabstop Hidden HwndTempy gEQE_MakePack, Build a Pack
		EQE_hwnd.BuildPackButton:= Tempy
		
	Gui, EQE_Main:Add, Text, x15 y169 w85 h17 Right, Type:
	Gui, EQE_Main:Add, Text, x341 y169 w55 h17 Right, Subtype:
	Gui, EQE_Main:Add, Text, x15 y197 w85 h17 Right, Cost:
	Gui, EQE_Main:Add, Text, x250 y197 w55 h17 HwndTempy Right, Weight:
		EQE_hwnd.Weighttext:= Tempy

	Gui, EQE_Main:Add, ComboBox, x105 y166 w230 HwndTempy gEQE_ItemType, Adventuring Gear||Armor|Weapon|Tools|Mounts and Other Animals|Tack, Harness, and Drawn Vehicles|Waterborne Vehicles|Treasure
		EQE_Hwnd.type:= Tempy
	Gui, EQE_Main:Add, ComboBox, x400 y166 w205 HwndTempy gEQE_SubType, Ammunition|Arcane Focus|Druidic Focus|Holy Symbol|Equipment Kits|Equipment Packs|Standard||
		EQE_hwnd.subtype:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y194 w70 h23 HwndTempy gEQE_MainUpdate, 
		EQE_hwnd.cost:= Tempy
	Gui, EQE_Main:Add, ComboBox, x180 y194 w55 HwndTempy gEQE_MainUpdate, cp|sp|ep|gp||pp
		EQE_hwnd.CostUnit:= Tempy
	Gui, EQE_Main:Add, Edit, x310 y194 w70 h23 HwndTempy gEQE_MainUpdate,  
		EQE_hwnd.Weight:= Tempy
	Gui, EQE_Main:Add, Text, x385 y197 w30 HwndTempy, lb.
		EQE_hwnd.WeightUnit:= Tempy
	
;{ GUI for armor
	Gui, EQE_Main:Add, Text, x15 y245 w85 h17 Hidden HwndTempy Right, Armor Class:
		EQE_hwnd.ArmorACtext:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y242 w50 h23 Hidden HwndTempy gEQE_MainUpdate Center, 
		EQE_hwnd.ArmorAC:= Tempy

	Gui, EQE_Main:Add, Text, x160 y245 w60 h17 Hidden HwndTempy Right, Strength:
		EQE_hwnd.ArmorStrengthtext:= Tempy
	Gui, EQE_Main:Add, Edit, x225 y242 w50 h23 Hidden HwndTempy gEQE_MainUpdate Center, 
		EQE_hwnd.ArmorStrength:= Tempy

	Gui, EQE_Main:Add, Text, x280 y245 w55 h17 Hidden HwndTempy Right, Stealth:
		EQE_hwnd.ArmorStealthtext:= Tempy
	Gui, EQE_Main:Add, ComboBox, x340 y242 w110 Hidden HwndTempy gEQE_MainUpdate, -||Disadvantage
		EQE_hwnd.ArmorStealth:= Tempy
;}		

;{ GUI for mounts
	Gui, EQE_Main:Add, Text, x15 y245 w85 h17 Hidden HwndTempy Right, Speed:
		EQE_hwnd.Speedtext:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y242 w70 h23 Hidden HwndTempy gEQE_MainUpdate, 
		EQE_hwnd.Speed:= Tempy
	Gui, EQE_Main:Add, ComboBox, x180 y242 w55 Hidden HwndTempy gEQE_MainUpdate, ft.||mph
		EQE_hwnd.SpeedUnit:= Tempy

	Gui, EQE_Main:Add, Text, x250 y245 w55 h17 Hidden HwndTempy Right, Capacity:
		EQE_hwnd.MountCapacitytext:= Tempy
	Gui, EQE_Main:Add, Edit, x310 y242 w70 h23 Hidden HwndTempy gEQE_MainUpdate, 
		EQE_hwnd.MountCapacity:= Tempy
	Gui, EQE_Main:Add, Text, x385 y245 w30 Hidden HwndTempy, lb.
		EQE_hwnd.MountCapacityUnit:= Tempy
;}

;{ GUI for weapons
	Gui, EQE_Main:Add, Text, x15 y245 w85 h17 Hidden HwndTempy Right, Damage:
		EQE_hwnd.WeaponDamageText:= Tempy
	Gui, EQE_Main:Add, Text, x105 y223 w50 h17 Hidden HwndTempy Center, Number
		EQE_hwnd.WeaponDamageNumberText:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y242 w50 h24 Hidden HwndTempy gEQE_MainUpdate Center, 
		EQE_hwnd.WeaponDamageNumber:= Tempy

	Gui, EQE_Main:Add, Text, x165 y223 w40 h17 Hidden HwndTempy Center, Die
		EQE_hwnd.WeaponDamageDieText:= Tempy
	Gui, EQE_Main:Add, Text, x156 y245 w8 h17 Hidden HwndTempy Center, d
		EQE_hwnd.WeaponDamageDText:= Tempy
	Gui, EQE_Main:Add, ComboBox, x165 y242 w40 Hidden HwndTempy gEQE_MainUpdate Center, 0|4|6||8|10|12|20
		EQE_hwnd.WeaponDamageDie:= Tempy

	Gui, EQE_Main:Add, Text, x206 y245 w8 h17 Hidden HwndTempy Center, +
		EQE_hwnd.WeaponDamagePlusText:= Tempy
	Gui, EQE_Main:Add, Text, x215 y223 w50 h17 Hidden HwndTempy Center, Bonus
		EQE_hwnd.WeaponDamageBonusText:= Tempy
	Gui, EQE_Main:Add, Edit, x215 y242 w50 h24 Hidden HwndTempy gEQE_MainUpdate Center, 
		EQE_hwnd.WeaponDamageBonus:= Tempy

	Gui, EQE_Main:Add, Text, x275 y223 w110 h17 Hidden HwndTempy Center, Damage Type
		EQE_hwnd.WeaponDamageTypeText:= Tempy
	Gui, EQE_Main:Add, ComboBox, x275 y242 w110 Hidden HwndTempy gEQE_MainUpdate Left, bludgeoning|piercing|slashing||acid|cold|fire|force|lightning|necrotic|poison|psychic|radiant|thunder
		EQE_hwnd.WeaponDamageType:= Tempy



	Gui, EQE_Main:Add, Text, x15 y275 w85 h17 disabled Hidden HwndTempy Right, Bonus:
		EQE_hwnd.WeaponBDamageText:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y272 w50 h24 disabled Hidden HwndTempy gEQE_MainUpdate Center, 
		EQE_hwnd.WeaponBDamageNumber:= Tempy
	Gui, EQE_Main:Add, Text, x156 y275 w8 h17 disabled Hidden HwndTempy Center, d
		EQE_hwnd.WeaponBDamageDText:= Tempy
	Gui, EQE_Main:Add, ComboBox, x165 y272 w40 disabled Hidden HwndTempy gEQE_MainUpdate Center, 0|4|6||8|10|12|20
		EQE_hwnd.WeaponBDamageDie:= Tempy
	Gui, EQE_Main:Add, Text, x206 y275 w8 h17 disabled Hidden HwndTempy Center, +
		EQE_hwnd.WeaponBDamagePlusText:= Tempy
	Gui, EQE_Main:Add, Edit, x215 y272 w50 h24 disabled Hidden HwndTempy gEQE_MainUpdate Center, 
		EQE_hwnd.WeaponBDamageBonus:= Tempy
	Gui, EQE_Main:Add, ComboBox, x275 y272 w110 disabled Hidden HwndTempy gEQE_MainUpdate Left, acid|cold|fire|force|lightning|necrotic|poison|psychic|radiant|thunder
		EQE_hwnd.WeaponBDamageType:= Tempy

	Gui, EQE_Main:Add, Checkbox, x400 y275 w100 h17 Hidden HwndTempy gEQE_BonusDamage, Add
		EQE_hwnd.WeaponBDamageAdd:= Tempy



	Gui, EQE_Main:Add, Text, x10 y310 w90 h17 Disabled Hidden HwndTempy Right, Normal Range:
		EQE_hwnd.WeaponNormalRangeText:= Tempy
	Gui, EQE_Main:Add, Text, x15 y337 w85 h17 Disabled Hidden HwndTempy Right, Long Range:
		EQE_hwnd.WeaponLongRangeText:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y307 w50 h23 Disabled Hidden HwndTempy gEQE_MainUpdate Center, 
		EQE_hwnd.WeaponNormalRange:= Tempy
	Gui, EQE_Main:Add, Edit, x105 y334 w50 h23 Disabled Hidden HwndTempy gEQE_MainUpdate Center, 
		EQE_hwnd.WeaponLongRange:= Tempy

	Gui, EQE_Main:Add, Text, x275 y310 w65 h17 Hidden HwndTempy Right, Reroll:
		EQE_hwnd.WeaponPrRerollText:= Tempy
	Gui, EQE_Main:Add, ComboBox, x345 y307 w40 Hidden HwndTempy gEQE_MainUpdate Center, 0||1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20
		EQE_hwnd.WeaponPrReroll:= Tempy

	Gui, EQE_Main:Add, Text, x275 y337 w65 h17 Hidden HwndTempy Right, Crit Range:
		EQE_hwnd.WeaponPrCritRangeText:= Tempy
	Gui, EQE_Main:Add, ComboBox, x345 y334 w40 Hidden HwndTempy gEQE_MainUpdate Center, 1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20||
		EQE_hwnd.WeaponPrCritRange:= Tempy


	Gui, EQE_Main:Add, Text, x505 y225 w100 h17 Hidden HwndTempy Center, Properties:
		EQE_hwnd.WeaponPropText:= Tempy
	Gui, EQE_Main:Add, Text, x505 y406 w100 h17 Hidden HwndTempy Center, Material:
		EQE_hwnd.WeaponMatText:= Tempy
		
	Gui, EQE_Main:Add, ListBox, 8 x505 y242 R10 w100 sort Hidden HwndTempy gEQE_MainUpdate, Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile
		EQE_hwnd.WeaponProp:= Tempy
	Gui, EQE_Main:Add, ListBox, 8 x505 y423 R3 w100 sort Hidden HwndTempy gEQE_MainUpdate, Silver|Adamantine|Cold-forged iron
		EQE_hwnd.WeaponMat:= Tempy

	Gui, EQE_Main:font, S10 c727178, Arial Bold
	Gui, EQE_Main:Add, GroupBox, x17 y360 w430 h202 Hidden HwndTempy, Optional Weapon Properties
		EQE_hwnd.WeaponGroupbox:= Tempy
	
	Gui, EQE_Main:font, S8 c000000, Arial
	Gui, EQE_Main:Add, Text, x27 y376 w250 h17 Hidden HwndTempy, (Only required for NPC Engineer Actions)
		EQE_hwnd.WeaponIntroText:= Tempy
	Gui, EQE_Main:font, S10 c000000, Arial

	Gui, EQE_Main:Add, Text, x26 y401 w85 h17 Hidden HwndTempy Right, Weapon Type:
		EQE_hwnd.WeaponTypeText:= Tempy
	Gui, EQE_Main:Add, ComboBox, x116 y398 w230 Hidden HwndTempy gEQE_MainUpdate Left, Melee Weapon Attack||Ranged Weapon Attack|Melee or Ranged Weapon Attack|Melee Spell Attack|Ranged Spell Attack|Melee or Ranged Spell Attack
		EQE_hwnd.WeaponType:= Tempy

	Gui, EQE_Main:Add, Text, x26 y431 w85 h17 Hidden HwndTempy Right, To Hit:
		EQE_hwnd.WeaponToHitText:= Tempy
	Gui, EQE_Main:Add, Edit, x116 y428 w40 h23 Hidden HwndTempy gEQE_MainUpdate Center
		EQE_hwnd.WeaponToHit:= Tempy
	Gui, EQE_Main:Add, Text, x166 y431 w65 h17 Hidden HwndTempy Right, Reach (ft.):
		EQE_hwnd.WeaponReachText:= Tempy
	Gui, EQE_Main:Add, Edit, x236 y428 w40 h23 Hidden HwndTempy gEQE_MainUpdate Center, 5
		EQE_hwnd.WeaponReach:= Tempy
	Gui, EQE_Main:Add, Text, x286 y431 w45 h17 Hidden HwndTempy Right, Target:
		EQE_hwnd.WeaponTargetText:= Tempy
	Gui, EQE_Main:Add, ComboBox, x336 y428 w100 Hidden HwndTempy gEQE_MainUpdate Left, one target||one creature
		EQE_hwnd.WeaponTarget:= Tempy
	Gui, EQE_Main:Add, CheckBox, x26 y458 w82 h17 +0x20 Right Hidden HwndTempy gEQE_MainUpdate, Other Text
		EQE_hwnd.WeaponOtherTextTick:= Tempy
	Gui, EQE_Main:Add, Edit, x116 y458 w320 h60 Disabled Hidden HwndTempy Multi Left, 
		EQE_hwnd.WeaponOtherText:= Tempy


	jsonpath:= DataDir "\weapons.json"
	If !WeapDB {
		WeapDB:= new JSONfile(jsonpath)
		weaplist:= "From NPC Action list.||"
		For a, b in WeapDB.object()
		{
			weaplist:= weaplist a "|"
		}
	}

	Gui, EQE_Main:Add, Text, x26 y531 w85 h17 Hidden HwndTempy Right, Import:
		EQE_hwnd.WeaponImportText:= Tempy
	Gui, EQE_Main:Add, ComboBox, x116 y528 w170 Hidden HwndTempy gEQE_ImportWeapon Left, %weaplist%
		EQE_hwnd.WeaponImport:= Tempy

	Gui, EQE_Main:Add, Button, x316 y528 w120 h25 +border -Tabstop Hidden HwndTempy gEQE_ExportWeapon, Save to NPC List
		EQE_hwnd.AddToNpcButton:= Tempy
;}

	Gui, EQE_Main:Add, CheckBox, x20 y568 w125 h17 +0x20 Right HwndTempy Checked1 gTAB_MainUpdate, Lock Item  in FG: 
		EQE_Hwnd.locked:= Tempy
	Gui, EQE_Main:Add, Button, x485 y560 w120 h25 +border +NoTab HwndTempy gEQE_Notes, Add Description
		EQE_Hwnd.notebutton:= Tempy

;}

;  ================================================
;  |           GUI for the 'image' tab            |
;  ================================================
;{
	Gui, EQE_Main:Tab, 2	; All the GUI controls for the 'Image' tab.
	

	Gui, EQE_Main:Add, Text, x15 y77 w590 h20 center, Click below to select Item image
	PicWinOptions := ( SS_BITMAP := 0xE ) | ( SS_CENTERIMAGE := 0x200 )
	Gui, EQE_Main:Add, Picture, x15 y97 w590 h400 %PicWinOptions% BackgroundTrans Border HwndTempy gEQE_image,
		EQE_Hwnd.jpeg:= Tempy

	Gui, EQE_Main:Add, Button, x505 y502 w100 h20 +border gEQE_ClearImage, Clear Image
	
	Gui, EQE_Main:Add, Text, x15 y533 w75 h17 Right, Artist Name:
	Gui, EQE_Main:Add, Text, x15 y563 w75 h17 Right, Link:
	Gui, EQE_Main:Add, Edit, x95 y530 w400 h23 HwndTempy gEQE_MainUpdate,
		EQE_Hwnd.Artist:= Tempy
	Gui, EQE_Main:Add, Edit, x95 y560 w400 h23 HwndTempy gEQE_MainUpdate,
		EQE_Hwnd.ArtistLink:= Tempy
;}

Gui, EQE_Main:Tab		; End of tab3 system.


	Gui, EQE_Main:Add, ActiveX, x620 y45 w500 h550 E0x200 +0x8000000 vEQE_VP, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	
	;~ Gui, EQE_Main:Add, Button, x8 y605 w115 h30 +border vSpImport gImport_Spell_Text, Import Text
	
	Gui, EQE_Main:Add, Text, x328 y610 w80 h17 Right, FG Category:
	Gui, EQE_Main:Add, Combobox, x413 y607 w200 HwndTempy gEQE_MainUpdate, 
		EQE_Hwnd.FGcat:= Tempy

	Gui, EQE_Main:Add, CheckBox, x622 y601 w200 h18 HwndTempy gEQE_MainUpdate, Add Title to Description
		EQE_Hwnd.AddTitle:= Tempy
	Gui, EQE_Main:Add, CheckBox, x622 y619 w200 h18 HwndTempy gEQE_MainUpdate, Add Image Link to Description
		EQE_Hwnd.ImageLink:= Tempy

	
	Gui, EQE_Main:Add, Button, x880 y605 w115 h30 HwndTempy +border gSave_Equipment, Save Item
		EQE_Hwnd.save:= Tempy
	Gui, EQE_Main:Add, Button, x1005 y605 w115 h30 HwndTempy +border gEQE_Append, Add to Project
		EQE_Hwnd.append:= Tempy

	
	Gui, EQE_Main:font, S18 c000000, Arial
	Gui, EQE_Main:Add, Button, x1125 y545 w24 h24 hwndeqbuttonup -Tabstop, % Chr(11165)
	Gui, EQE_Main:Add, Button, x1125 y571 w24 h24 hwndeqbuttondn -Tabstop, % Chr(11167)

	Gui, EQE_Main:font, S9 c000000, Segoe UI
	Gui, EQE_Main:Add, StatusBar
	Gui, EQE_Main:Default
	SB_SetParts(450, 250)
	SB_SetText(" " WinTProj, 1)
	Gui, EQE_Main:font, S10 c000000, Arial
	
}

EQE_GUIImport()	 {
	;~ global
	
;~ ; Settings for text import window (SPE_Import)
	;~ Gui, SPE_Import:+OwnerSPE_Main
	;~ Gui, SPE_Import:-SysMenu
	;~ Gui, SPE_Import:+hwndSPE_Import
	;~ Gui, SPE_Import:Color, 0xE2E1E8
	;~ Gui, SPE_Import:font, S10 c000000, Arial
	
	;~ Gui, SPE_Import:Add, Edit, vSP_Cap_Text gSPE_Import_Update_Output x8 y8 w442 h474 Multi, %SP_Cap_Text%
	
	;~ Gui, SPE_Import:Add, ActiveX, x480 y8 w500 h500 E0x200 +0x8000000 vIMspell, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	

	;~ Gui, SPE_Import:Add, DropDownList, x300 y485 w150 vSP_ImportChoice gSPE_Import_Update_Output, PDF & Plain Text||
	;~ Gui, SPE_Import:Add, Button, x8 y515 w130 h30 +border vSPE_Import_Delete gSPE_Import_Delete, Delete All Text
	;~ Gui, SPE_Import:Add, Button, x150 y515 w130 h30 +border vSPE_Import_Append gSPE_Import_Append, Append Clipboard
	;~ Gui, SPE_Import:Add, Button, x710 y515 w130 h30 +border vSPE_Import_Return gSPE_Import_Return, Import and Return
	;~ Gui, SPE_Import:Add, Button, x852 y515 w130 h30 +border vSPE_Import_Cancel gSPE_Import_Cancel, Cancel All Changes

	;~ Hotkey, IfWinActive, ahk_id %SPE_Import%
	;~ Hotkey, ~Wheeldown, SpVScrDwn
	;~ Hotkey, ~Wheelup, SpVScrUp
	;~ hotkey, esc, EscapeHandle
}

EQE_GUIJSON()	 {
	global
	local tempy
	
; Settings for manage JSON window (EQE_JSON)
	Gui, EQE_JSON:+OwnerEQE_Main
	Gui, EQE_JSON:-SysMenu
	Gui, EQE_JSON:+hwndEQE_JSON
	Gui, EQE_JSON:Color, 0xE2E1E8
	Gui, EQE_JSON:font, S10 c000000, Arial
	Gui, EQE_JSON:margin, 5, 1

	equipment_list:= "Choose an item from the JSON file||"
	For a, b in EQP.object()
	{
		equipment_list:= equipment_list EQP[a].name "|"
	}
	Gui, EQE_JSON:Add, ComboBox, x10 y10 w300 gEQE_JSON_Choose hwndTempy, %equipment_list%
		EQE_Hwnd.EQE_JSONChoose:= Tempy
	Gui, EQE_JSON:Add, Edit, x10 y40 w300 h60 hwndTempy +ReadOnly, 
		EQE_Hwnd.EQE_JSONselected:= Tempy
	Gui, EQE_JSON:Add, Button, x75 y105 w80 h25 +border gEQE_JSON_Del hwndTempy, Delete Item
		EQE_Hwnd.EQE_JSONDeleteButton:= Tempy
	Gui, EQE_JSON:Add, Button, x165 y105 w80 h25 +border gEQE_JSON_Edit hwndTempy, Edit Item
		EQE_Hwnd.EQE_JSONEditButton:= Tempy
	Gui, EQE_JSON:Add, Button, x180 y170 w130 h30 +border gEQE_JSON_Cancel hwndTempy, Close
		EQE_Hwnd.EQE_JSONCancelButton:= Tempy
}

EQE_CreateToolbar() {
	ImageList := IL_Create(6)
	IL_Add(ImageList, "NPC Engineer.dll", 1)
	IL_Add(ImageList, "NPC Engineer.dll", 2)
	IL_Add(ImageList, "NPC Engineer.dll", 3)
	IL_Add(ImageList, "NPC Engineer.dll", 10)
	IL_Add(ImageList, "NPC Engineer.dll", 8)
	IL_Add(ImageList, "NPC Engineer.dll", 27)
	IL_Add(ImageList, "NPC Engineer.dll", 4)
	IL_Add(ImageList, "NPC Engineer.dll", 5)
	IL_Add(ImageList, "NPC Engineer.dll", 6)

	Buttons = 
	(LTrim
		New Equipment Item
		Open Equipment Item
		Save Equipment Item
		-
		Open Previous Equipment Item
		Open Next Equipment Item
		-
		Manage Equipment Items
		-
		Import Equipment Text
		-
		Manage Project
		-
		Create Module
	)

	Return ToolbarCreate("OnToolbar", Buttons, "EQE_Main", ImageList, "Flat List Tooltips Border")
}

EQE_MainGuiDropFiles(GuiHwnd, FileArray, CtrlHwnd, X, Y) {
	Global
	if (EQE_Hwnd.jpeg = CtrlHwnd) {
		Equip.imagepath:= filearray[1]
		Gosub Load_EQE_Image
	}
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
#Include Table Engineer.ahk
#Include Artefact Engineer.ahk

/* ========================================================
 *                  End of Include Files
 * ========================================================
*/

;~ ######################################################
;~ #                    Program End.                    #
;~ ######################################################