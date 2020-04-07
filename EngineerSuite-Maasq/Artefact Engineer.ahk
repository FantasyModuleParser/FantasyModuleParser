;~ ######################################################
;~ #                                                    #
;~ #               ~ Artefact Engineer ~                #
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
ArtefactEngineer("self")
return

ArtefactEngineer(what) {
	global
	SplashImageGUI("Artefact Engineer.png", "Center", "Center", 300, true)
	
	;~ ART_LoadPause:= 1
	ART_Release_Version:= "0.06.04"
	ART_Release_Date:= "24/11/19"
	ART_Ref:= what
	
	Gosub ART_Startup
	ART_Initialise()
	
	ART_PrepareGUI()
	If (LaunchProject AND FileExist(ProjectPath)) {
		flags.project:= 1
		PROE_Ref:= "ART_Main"
		Gosub Project_Load
	}
	FGcatList(ART_Hwnd.FGcat)
	If (LaunchArte AND FileExist(ArtePath)) {
		flags.Arte:= 1
		Arte:= ObjLoad(ArteSavePath)
		ART_SetVars()
	}

	ART_GetVars()
	ART_RH_Box()
	
	win.Artefactengineer:= 1
	Gui, ART_Main:Show, w1150 h665, Artefact Engineer
	ART_Toolbar:= ART_CreateToolBar()

	;~ ART_LoadPause:= 0
	
	Hotkey,IfWinActive,ahk_id %ART_Main%
	hotkey, ^b, SetFontStyle2
	hotkey, ^i, SetFontStyle2
	hotkey, ^u, SetFontStyle2
	hotkey, ^v, SpPaste
	Hotkey, ^J, De_PDF
	Hotkey, ~LButton, ART_Vup
	Hotkey, ~Wheeldown, ART_VScrDwn
	Hotkey, ~Wheelup, ART_VScrUp
	hotkey, esc, EscapeHandle

	OnMessage(0x200, "WM_MOUSEMOVE")

	WM_SETCURSOR := 0x0020
	CHAND := DllCall("User32.dll\LoadCursor", "Ptr", NULL, "Int", 32649, "UPtr")
	OnMessage(WM_SETCURSOR, "SetCursor")
}


;~ ######################################################
;~ #                       Labels                       #
;~ ######################################################



ART_Startup:				;{
	Critical
	;~ InitialDirCreate()	
	;~ CommonInitialise()
	;~ Load_Options()
	;~ OptionsDirCreate()
return						;}

ART_MainGuiClose:			;{
	File:= A_AppData "\NPC Engineer\NPC Engineer.ini"
	Stub:= "Paths"
	IniWrite, %ProjectPath%, %File%, %Stub%, ProjectPath
	;~ IniWrite, %NPCSavePath%, %File%, %Stub%, NPCSavePath
	win.Artefactengineer:= 0
	If (ART_Ref = "self") {
		ExitApp
	} Else {
		Gui, ART_Main:Destroy
		Gui, %ART_Ref%:show
		StBar(ART_Ref)
	}
return						;}

ART_MainUpdate:				;{
	ART_GetVars()
	If 	(Instr(Arte.WeaponProp, "Ammunition")) OR (Instr(Arte.WeaponProp, "Thrown")) OR (Instr(Arte.WeaponProp, "Range")) {
		GuiControl, ART_Main:Enable, % ART_Hwnd.WeaponNormalRangeText
		GuiControl, ART_Main:Enable, % ART_Hwnd.WeaponNormalRange
		GuiControl, ART_Main:Enable, % ART_Hwnd.WeaponLongRangeText
		GuiControl, ART_Main:Enable, % ART_Hwnd.WeaponLongRange
	} Else {
		GuiControl, ART_Main:Disable, % ART_Hwnd.WeaponNormalRangeText
		GuiControl, ART_Main:Disable, % ART_Hwnd.WeaponNormalRange
		GuiControl, ART_Main:Disable, % ART_Hwnd.WeaponLongRangeText
		GuiControl, ART_Main:Disable, % ART_Hwnd.WeaponLongRange
	}
	if Arte.NoID {
		GuiControl, ART_Main:Enable, % ART_Hwnd.NoIDNotes
		GuiControl, ART_Main:Enable, % ART_Hwnd.NoIDNotesText
	} Else {
		GuiControl, ART_Main:Disable, % ART_Hwnd.NoIDNotes
		GuiControl, ART_Main:Disable, % ART_Hwnd.NoIDNotesText
	}
	ART_RH_Box()
return						;}

ART_Project_Manage:			;{
	Critical
	Gui, ART_Main:+disabled
	ProjectEngineer("ART_Main")
return						;}

Manage_ART_JSON:			;{
	Critical
	If (ProjectLive != 1) {
		MsgBox, 16, No Project, You must load a project *.ini`nto manipulate Artefacts in their JSON file., 3
		gosub, Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			ART_GUIJSON()
			Gui, ART_Main:+disabled
			Gui, ART_JSON:Show, w320 h210, Edit or Delete Artefact in the JSON file
		} else {
			MsgBox, 16, Engineer Suite Parser only, This function can only be carried out whilst using the Engineer Suite Parser., 3
		}
	}
 return						;}

ART_JSON_Cancel:
ART_JSONGuiClose:			;{
	Gui, ART_Main:-disabled
	Gui, ART_JSON:Destroy
return						;}

ART_JSON_Choose:			;{
	JSONtemp:= Gget(ART_Hwnd.ART_JSONChoose)
	JSON_ART_Name:= ""
	For a, b in FCT.object()
	{
		if (fct[a].name == JSONtemp) {
			JSON_ART_Name:= a
		}
	}
	JSON_This_Text:= fct[JSON_ART_Name].Name Chr(10)
	JSON_This_Text .= fct[JSON_ART_Name].type "|" fct[JSON_ART_Name].subtype
	Gset(ART_Hwnd.ART_JSONselected, JSON_This_Text)
 return						;}

ART_JSON_Del:				;{
	If JSON_ART_Name {
		FCT.delete(JSON_ART_Name)
		FCT.save(true)
		Artefact_list:= "|Choose an item from the JSON file||"
		For a, b in FCT.object()
		{
			Artefact_list:= Artefact_list fct[a].name "|"
		}
		temp:= ART_Hwnd.ART_JSONChoose
		GuiControl, , %temp%, %Artefact_list%
		JSON_ART_Name:= ""
		Gset(ART_Hwnd.ART_JSONselected, JSON_ART_Name)
		ART_RH_Box()
	}
return						;}

ART_JSON_Edit:				;{
	;~ If JSON_Sp_Name {
		;~ Edit_Sp_JSON(JSON_Sp_Name)
		;~ Gui, SPE_Main:-disabled
		;~ Gui, SPE_JSON:Destroy
	;~ }
return						;}

ART_text:					;{
	tempsp:= Tokenise(ET1.GetRTF(False))
	StringReplace, tempsp, tempsp, <p></p>, , All
	StringReplace, tempsp, tempsp, `r`n`r`n, `r`n, All
	Arte.text:= RegexReplace(tempsp, "^\s+|\s+$" )

	ART_RH_Box()
return						;}

New_Artefact:				;{
	For a,b in Arte {
		Arte[a]:= ""
	}
	Arte.type:= "Wondrous Item"
	Arte.subtype:= "Ammunition"
	Arte.rarity:= "Common"
	Arte.attune:= 0
	Arte.ArmorStealth:= "-"
	
	Arte.WeaponType:= "Melee Weapon Attack"
	Arte.WeaponDamageDie:= 6
	Arte.WeaponDamageType:= "slashing"
	Arte.WeaponBDamageDie:= 6
	Arte.WeaponBDamageAdd:= 0
	Arte.WeaponPrReroll:= 0
	Arte.WeaponPrCritRange:= 20
	
	Arte.CostUnit:= "gp"

Gosub ART_ClearImage

	Arte.FGcat:= Modname
	Arte.Locked:= 1
	ART_SetVars()
	ART_ScrollPoint:= 0
	ART_ScrollEnd:= 0
	ART_ItemType()
return						;}

Open_Artefact:				;{
	if ArteModSaveDir {
		SpModSaveDir:= "\" Modname
		TempDest:= ArtePath . SpModSaveDir . "\"
		Ifnotexist, %TempDest% 
			FileCreateDir, %TempDest% 
	} Else {
		SpModSaveDir:= ""
	TempDest:= ArtePath
	}
	ART_LoadPause:= 1
	TempWorkingDir:= A_WorkingDir
	FileSelectFile, SelectedFile, 2, %TempDest%, Load Artefact Item, (*.fct)
	if (FileExist(SelectedFile)) {
		Arte:= ObjLoad(SelectedFile)
		If !Arte.FGcat
			Arte.FGcat:= Modname
		Gset(ART_Hwnd.type, Arte.type)
		temp:= Arte.subtype
		ART_ItemType()
		Arte.subtype:= temp
		temp:= ""
		ART_SetVars()
		Gosub Load_ART_Image
		EquipSavePath:= SelectedFile
		SetWorkingDir %TempWorkingDir%
		ART_LoadPause:= 0
		ART_RH_Box()
	}
return						;}

Save_Artefact:				;{
	If Arte.filename {
		ClearAllBut(Arte.type)
		if ArteModSaveDir {
			SpModSaveDir:= "\" Modname
			TempDest:= ArtePath . SpModSaveDir . "\"
			Ifnotexist, %TempDest% 
				FileCreateDir, %TempDest% 
		} Else {
			SpModSaveDir:= ""
		}			
		TempWorkingDir:= A_WorkingDir
		SelectedFile:= ArtePath . SpModSaveDir . "\" Arte.filename ".fct"
		If FileExist(SelectedFile)
			FileDelete, %SelectedFile%
		ART_SavePic()
		sz:= ObjDump(SelectedFile, Arte)
		SetWorkingDir %TempWorkingDir%
		Toast(Arte.name " saved successfully.")
	}
return						;}

Next_Artefact:				;{
	if (Mod_Parser == 1) {
		ARTNameTemp:= Gget(ART_Hwnd.name)
		FlagTemp:= 0
		olda:= ""
		For a, b in ART.object()
		{
			if flagtemp {
				stringreplace ArteSavePath, ArteSavePath, %olda%.fct, %a%.fct
				Arte:= ObjLoad(ArteSavePath)
				ART_SetVars()
				FlagTemp:= ""
				olda:= ""
				ARTNameTemp:= ""
				return
			}
			if (ART[a].name = ARTNameTemp) {
				FlagTemp:= 1
				olda:= a
			}
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This Artefact is not in the current Project., 2
	} else {
		MsgBox, 16, Engineer Suite parser only, This function can only be carried out whilst using Engineer Suite's parser., 3
	}
return						;}

Prev_Artefact:				;{
	if (Mod_Parser == 1) {
		ARTNameTemp:= Gget(ART_Hwnd.name)
		FlagTemp:= 0
		olda:= ""
		For a, b in ART.object()
		{
			if (A_Index = 1)
				olda:= a
			if (ART[a].name = ARTNameTemp) {
				FlagTemp:= 1
			}
			if flagtemp {
				stringreplace ArteSavePath, ArteSavePath, %a%.fct, %olda%.fct
				Arte:= ObjLoad(ArteSavePath)
				ART_SetVars()
				FlagTemp:= ""
				olda:= ""
				ARTNameTemp:= ""
				return
			}
			olda:= a
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This Artefact is not in the current Project., 2
	} else {
		MsgBox, 16, NPC Engineer Parser only, This function can only be carried out whilst using NPC Engineer's Parser., 3
	}
return						;}

ART_Image:					;{
	TempImagePath:= Arte.imagepath
	FileSelectFile, TempImagePath, , , Select an image., (*.jpg)
	If (FileExist(TempImagePath)) {
		Arte.imagepath:= TempImagePath
	} else {
		return
	}
Load_ART_Image:
	hBM:= LoadPicture(Arte.imagepath)
	IfEqual, hBM, 0, Return

	BITMAP := getHBMinfo( hBM )                                ; Extract Width andh height of image 
	New := ScaleRect( BITMAP.Width, BITMAP.Height, 586, 396 )  ; Derive best-fit W x H for source image 

	DllCall( "DeleteObject", "Ptr",hBM )                       ; Delete Image handle ...         
	hBM := LoadPicture(Arte.imagepath, "GDI+ w" New.W . " h" . New.H )  ; ..and get a new one with correct W x H

	GuiControl, ART_Main:, % ART_Hwnd.jpeg,  *w0 *h0 HBITMAP:%hBM%

	Arte.ImageLink:= 1
	Gset(ART_Hwnd.ImageLink, Arte.ImageLink)
	ART_RH_Box()
return						;}

ART_ClearImage:				;{
	GuiControl, ART_Main:hide, % ART_Hwnd.jpeg
	GuiControl, , % ART_Hwnd.jpeg
	GuiControl, ART_Main:show, % ART_Hwnd.jpeg
	Arte.imagepath:= ""
	Arte.ImageLink:= 0
	Gset(ART_Hwnd.ImageLink, Arte.ImageLink)
	ART_RH_Box()
return						;}



Import_ART_Text:			;{
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

ART_RH_Box() {
	global
	local qc
	Critical
	If !ART_LoadPause {
		ART_ScrollEnd:= ART_VP.document.body.scrollHeight - 500
		If (ART_ScrollEnd < 0) {
			ART_ScrollEnd:= 0
		}
		ART_Graphical("ART_VP", ART_ScrollPoint)
		Gui, ART_Main:Default
		WinTNPC:= "Item: " . Arte.name
		If Modname {
			qc:= FCT.SetCapacity(0)
			if !qc
				qc:= 0
			SB_SetText(" " Modname " (" qc " items)", 1)
		}
		NameTemp:= Gget(ART_Hwnd.name)
		FlagTemp:= 0
		For a, b in FCT.object()
		{
			if (FCT[a].name = NameTemp)
				FlagTemp:= 1
		}
		If FlagTemp
			GuiControl,, % ART_Hwnd.append, Update Project
		else
			GuiControl,, % ART_Hwnd.append, Add to Project
		Gui, ART_Main:Show
	}
}	

ART_Initialise() {
	global
	Arte:= {}
	ART_Hwnd:= {}
	bullets:= []
	ART_ScrollPoint:= 0
	ImScrollPoint:= 0
	flags:=[]
	flags.project:= 0
}

ART_MainLoop(RawSpell) {
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

ART_CommonProblems(chunk) {
	
}
	
ART_GetText(chunk) {
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

ART_backup() {
	Global
	spellBU:= objfullyclone(spell)
	ImScrollPoint:= 0
	spell:= {}
}

ART_Restore() {
	Global
	spell:= objfullyclone(spellBU)
	spellBU:= ""
}



;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |            Input/Output Functions            |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

ART_Append() {
	Global
	If !Arte.Name {
		return
	}
	If (ProjectLive != 1) or !IsObject(FCT) {
		MsgBox, 16, No Project, The following must be true to add items to a project:`n`n * You have created a new project or loaded a project *.ini.`n * You have enabled magical items by clicking the checkbox.
		gosub, ART_Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			ART_EPAppend()
		;~ } else if (Mod_Parser == 2) {
			;~ ART_Par5e_Append()
		;~ } else if (Mod_Parser == 3) {
			;~ SpFG5EP_Append()
		}
		ART_RH_Box()
	}
}

ART_EPAppend() {
	global
	ART_JSONFile()
	JSON_Ob_Exist:= ""
	For a, b in FCT.object()
	{
		if (a == Arte.filename) {
			JSON_Ob_Exist:= a
		}
	}

	If JSON_Ob_Exist {
		tempname:= fct[JSON_Ob_Exist].name
		MsgBox, 292, Overwrite Artefact, The Artefact '%tempname%' already exists in the project's JSON file.`nDo you wish to overwrite it with this data? This is unrecoverable!
		IfMsgBox Yes
		{
			FCT.delete(JSON_Ob_Exist)
			FCT.fill(Objart)
			FCT.save(true)

			notify:= Arte.Name " updated in " ModName "."
			Toast(notify)
		}
	} else {
		FCT.fill(Objart)
		FCT.save(true)

		notify:= Arte.Name " added to " ModName "."
		Toast(notify)
	}
	
}

ART_SavePic() {
	Global
	if ArteCopyPics {
		If (Arte.ImagePath and Arte.filename) {
			Ifexist, % Arte.ImagePath
			{
				ThumbDest:= ArtePath . SpModSaveDir . "\" . Arte.filename . ".*"
				FileCopy, % Arte.ImagePath, %ThumbDest%, 1
				Arte.ImagePath:= ArtePath . SpModSaveDir . "\" . Arte.filename . ".jpg"
			}
		}
	}		
}



;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |           General Purpose Functions          |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

ART_GetVars() {
	global
	local tempvar
	Arte.name:= Gget(ART_Hwnd.name)
	Arte.NoID:= Gget(ART_Hwnd.NoID)
	Arte.NoIDNotes:= Gget(ART_Hwnd.NoIDNotes)
	Arte.type:= Gget(ART_Hwnd.type)
	Arte.subtype:= Gget(ART_Hwnd.subtype)
	Arte.cost:= Gget(ART_Hwnd.cost)
	Arte.costUnit:= Gget(ART_Hwnd.costUnit)
	Arte.Weight:= Gget(ART_Hwnd.Weight)
	Arte.Rarity:= Gget(ART_Hwnd.Rarity)
	Arte.attune:= Gget(ART_Hwnd.attune)
	
	Arte.ArmorAC:= Gget(ART_Hwnd.ArmorAC)
	Arte.ArmorStrength:= Gget(ART_Hwnd.ArmorStrength)
	Arte.ArmorStealth:= Gget(ART_Hwnd.ArmorStealth)
	
	Arte.WeaponDamageNumber:= Gget(ART_Hwnd.WeaponDamageNumber)
	Arte.WeaponDamageDie:= Gget(ART_Hwnd.WeaponDamageDie)
	Arte.WeaponDamageBonus:= Gget(ART_Hwnd.WeaponDamageBonus)
	Arte.WeaponDamageType:= Gget(ART_Hwnd.WeaponDamageType)
	Arte.WeaponBDamageNumber:= Gget(ART_Hwnd.WeaponBDamageNumber)
	Arte.WeaponBDamageDie:= Gget(ART_Hwnd.WeaponBDamageDie)
	Arte.WeaponBDamageBonus:= Gget(ART_Hwnd.WeaponBDamageBonus)
	Arte.WeaponBDamageType:= Gget(ART_Hwnd.WeaponBDamageType)
	Arte.WeaponBDamageAdd:= Gget(ART_Hwnd.WeaponBDamageAdd)
	Arte.WeaponNormalRange:= Gget(ART_Hwnd.WeaponNormalRange)
	Arte.WeaponLongRange:= Gget(ART_Hwnd.WeaponLongRange)
	Arte.WeaponPrReroll:= Gget(ART_Hwnd.WeaponPrReroll)
	Arte.WeaponPrCritRange:= Gget(ART_Hwnd.WeaponPrCritRange)
	
	Arte.Bonus:= Gget(ART_Hwnd.Bonus)

	Arte.jpeg:= Gget(ART_Hwnd.jpeg)
	Arte.ImageLink:= Gget(ART_Hwnd.ImageLink)
	Arte.AddTitle:= Gget(ART_Hwnd.AddTitle)
	Arte.Artist:= Gget(ART_Hwnd.Artist)
	Arte.ArtistLink:= Gget(ART_Hwnd.ArtistLink)
	Arte.FGcat:= Gget(ART_Hwnd.FGcat)
	Arte.Locked:= Gget(ART_Hwnd.Locked)
	
	tempvar:= Arte.name
	StringLower tempvar, tempvar
	tempvar:= RegExReplace(tempvar, "[^a-zA-Z0-9]", "")
	Arte.filename:= tempvar
	
	tempvar:= Gget(ART_Hwnd.WeaponProp)
	Arte.WeaponProp:= ""
	Loop, Parse, Tempvar, |
	{
		Arte.WeaponProp .= A_Loopfield ", "
	}
	If (SubStr(Arte.WeaponProp, -1) = ", ")
		Arte.WeaponProp:= SubStr(Arte.WeaponProp, 1, -2)

	tempvar:= Gget(ART_Hwnd.WeaponMat)
	Arte.WeaponMat:= ""
	Loop, Parse, Tempvar, |
	{
		Arte.WeaponMat .= A_Loopfield ", "
	}
	If (SubStr(Arte.WeaponMat, -1) = ", ")
		Arte.WeaponMat:= SubStr(Arte.WeaponMat, 1, -2)
}

ART_SetVars() {
	Global
	local tempvar, tempvar2
	Gset(ART_Hwnd.name, Arte.name)
	Gset(ART_Hwnd.NoID, Arte.NoID)
	Gset(ART_Hwnd.NoIDNotes, Arte.NoIDNotes)
	Gset(ART_Hwnd.type, Arte.type)
	Gset(ART_Hwnd.subtype, Arte.subtype)
	Gset(ART_Hwnd.cost, Arte.cost)
	Gset(ART_Hwnd.costUnit, Arte.costUnit)
	Gset(ART_Hwnd.Weight, Arte.Weight)
	Gset(ART_Hwnd.Rarity, Arte.Rarity)
	Gset(ART_Hwnd.attune, Arte.attune)
	
	Gset(ART_Hwnd.ArmorAC, Arte.ArmorAC)
	Gset(ART_Hwnd.ArmorStrength, Arte.ArmorStrength)
	Gset(ART_Hwnd.ArmorStealth, Arte.ArmorStealth)

	Gset(ART_Hwnd.Bonus, Arte.Bonus)

	Gset(ART_Hwnd.WeaponDamageNumber, Arte.WeaponDamageNumber)
	Gset(ART_Hwnd.WeaponDamageDie, Arte.WeaponDamageDie)
	Gset(ART_Hwnd.WeaponDamageBonus, Arte.WeaponDamageBonus)
	Gset(ART_Hwnd.WeaponDamageType, Arte.WeaponDamageType)
	Gset(ART_Hwnd.WeaponBDamageNumber, Arte.WeaponBDamageNumber)
	Gset(ART_Hwnd.WeaponBDamageDie, Arte.WeaponBDamageDie)
	Gset(ART_Hwnd.WeaponBDamageBonus, Arte.WeaponBDamageBonus)
	Gset(ART_Hwnd.WeaponBDamageType, Arte.WeaponBDamageType)
	Gset(ART_Hwnd.WeaponBDamageAdd, Arte.WeaponBDamageAdd)
	Gset(ART_Hwnd.WeaponNormalRange, Arte.WeaponNormalRange)
	Gset(ART_Hwnd.WeaponLongRange, Arte.WeaponLongRange)
	Gset(ART_Hwnd.WeaponPrReroll, Arte.WeaponPrReroll)
	Gset(ART_Hwnd.WeaponPrCritRange, Arte.WeaponPrCritRange)

	Gset(ART_Hwnd.jpeg, Arte.jpeg)
	Gset(ART_Hwnd.ImageLink, Arte.ImageLink)
	Gset(ART_Hwnd.AddTitle, Arte.AddTitle)
	Gset(ART_Hwnd.Artist, Arte.Artist)
	Gset(ART_Hwnd.ArtistLink, Arte.ArtistLink)
	Gset(ART_Hwnd.FGcat, Arte.FGcat)
	Gset(ART_Hwnd.Locked, Arte.Locked)

	tempvar:= Arte.WeaponProp
	tempvar2:= "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile"
	Loop, Parse, tempvar, `,, %A_Space%
	{
		Stringreplace, tempvar2, tempvar2, %A_LoopField%, %A_LoopField%||	
	}
	Stringreplace, tempvar2, tempvar2, |||, ||, All
	Gset(ART_Hwnd.WeaponProp, tempvar2)
	
	tempvar:= Arte.WeaponMat
	tempvar2:= "|Silver|Adamantine|Cold-forged iron"
	Loop, Parse, tempvar, `,, %A_Space%
	{
		Stringreplace, tempvar2, tempvar2, %A_LoopField%, %A_LoopField%||	
	}
	Stringreplace, tempvar2, tempvar2, |||, ||, All
	Gset(ART_Hwnd.WeaponMat, tempvar2)
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

ART_SetTT() {
	Global
	if !isobject(hTTip){
		hTTip:= []
	}
	hTTip[ART_Hwnd.name]:= "Enter a name for your table."
}

ART_ItemType() {
	global
	local EQtype
	EQtype:= Gget(ART_Hwnd.type)
	If (EQtype = "Armor") {
		Art_Hideall()
		ART_ClearAllBut("Armor")
		
		GuiControl, Show, % ART_hwnd.Bonustext
		GuiControl, Show, % ART_hwnd.Bonus
		GuiControl, Show, % ART_hwnd.BonusEdit
		GuiControl, Show, % ART_hwnd.Weighttext
		GuiControl, Show, % ART_hwnd.Weight
		GuiControl, Show, % ART_hwnd.WeightUnit
		GuiControl, Show, % ART_hwnd.ArmorACtext
		GuiControl, Show, % ART_hwnd.ArmorAC
		GuiControl, Show, % ART_hwnd.ArmorStrengthtext
		GuiControl, Show, % ART_hwnd.ArmorStrength
		GuiControl, Show, % ART_hwnd.ArmorStealthtext
		GuiControl, Show, % ART_hwnd.ArmorStealth
		GuiControl,, % ART_hwnd.subtype, |Light Armor||Medium Armor|Heavy Armor|Shield
		GuiControl, Enable, % ART_hwnd.subtype
	}
	If (EQtype = "Weapon") {
		Art_Hideall()
		ART_ClearAllBut("Weapon")
		GuiControl, Show, % ART_hwnd.Bonustext
		GuiControl, Show, % ART_hwnd.Bonus
		GuiControl, Show, % ART_hwnd.BonusEdit
		GuiControl, Show, % ART_hwnd.Weighttext
		GuiControl, Show, % ART_hwnd.Weight
		GuiControl, Show, % ART_hwnd.WeightUnit
		GuiControl, Show, % ART_hwnd.WeaponDamageText
		GuiControl, Show, % ART_hwnd.WeaponDamageNumberText
		GuiControl, Show, % ART_hwnd.WeaponDamageNumber
		GuiControl, Show, % ART_hwnd.WeaponDamageDieText
		GuiControl, Show, % ART_hwnd.WeaponDamageDText
		GuiControl, Show, % ART_hwnd.WeaponDamageDie
		GuiControl, Show, % ART_hwnd.WeaponDamagePlusText
		GuiControl, Show, % ART_hwnd.WeaponDamageBonusText
		GuiControl, Show, % ART_hwnd.WeaponDamageBonus
		GuiControl, Show, % ART_hwnd.WeaponDamageTypeText
		GuiControl, Show, % ART_hwnd.WeaponDamageType
		GuiControl, Show, % ART_hwnd.WeaponBDamageText
		GuiControl, Show, % ART_hwnd.WeaponBDamageNumber
		GuiControl, Show, % ART_hwnd.WeaponBDamageDText
		GuiControl, Show, % ART_hwnd.WeaponBDamageDie
		GuiControl, Show, % ART_hwnd.WeaponBDamagePlusText
		GuiControl, Show, % ART_hwnd.WeaponBDamageBonus
		GuiControl, Show, % ART_hwnd.WeaponBDamageType
		GuiControl, Show, % ART_hwnd.WeaponBDamageAdd
		GuiControl, Show, % ART_hwnd.WeaponNormalRangeText
		GuiControl, Show, % ART_hwnd.WeaponNormalRange
		GuiControl, Show, % ART_hwnd.WeaponLongRangeText
		GuiControl, Show, % ART_hwnd.WeaponLongRange
		GuiControl, Show, % ART_hwnd.WeaponPropText
		GuiControl, Show, % ART_hwnd.WeaponProp
		GuiControl, Show, % ART_hwnd.WeaponMatText
		GuiControl, Show, % ART_hwnd.WeaponMat
		GuiControl, Show, % ART_hwnd.WeaponPrRerollText
		GuiControl, Show, % ART_hwnd.WeaponPrReroll
		GuiControl, Show, % ART_hwnd.WeaponPrCritRangeText
		GuiControl, Show, % ART_hwnd.WeaponPrCritRange
		GuiControl,, % ART_hwnd.subtype, |Simple Melee Weapons||Simple Ranged Weapons|Martial Melee Weapons|Martial Ranged Weapons
		GuiControl, Enable, % ART_hwnd.subtype
	}
	If (EQtype = "Potion") {
		Art_Hideall()
		ART_ClearAllBut("Potion")
		GuiControl, Show, % ART_hwnd.Weighttext
		GuiControl, Show, % ART_hwnd.Weight
		GuiControl, Show, % ART_hwnd.WeightUnit
		GuiControl,, % ART_hwnd.subtype, |--
		GuiControl, Disable, % ART_hwnd.subtype
	}
	If (EQtype = "Scroll") {
		Art_Hideall()
		ART_ClearAllBut("Scroll")
		GuiControl, Show, % ART_hwnd.Weighttext
		GuiControl, Show, % ART_hwnd.Weight
		GuiControl, Show, % ART_hwnd.WeightUnit
		GuiControl,, % ART_hwnd.subtype, |--
		GuiControl, Disable, % ART_hwnd.subtype
	}
	If (EQtype = "Ring") {
		Art_Hideall()
		ART_ClearAllBut("Ring")
		GuiControl, Show, % ART_hwnd.Weighttext
		GuiControl, Show, % ART_hwnd.Weight
		GuiControl, Show, % ART_hwnd.WeightUnit
		GuiControl,, % ART_hwnd.subtype, |--
		GuiControl, Disable, % ART_hwnd.subtype
	}
	If (EQtype = "Rod") {
		Art_Hideall()
		ART_ClearAllBut("Rod")
		GuiControl, Show, % ART_hwnd.Weighttext
		GuiControl, Show, % ART_hwnd.Weight
		GuiControl, Show, % ART_hwnd.WeightUnit
		GuiControl,, % ART_hwnd.subtype, |--
		GuiControl, Disable, % ART_hwnd.subtype
	}
	If (EQtype = "Staff") {
		Art_Hideall()
		ART_ClearAllBut("Staff")
		GuiControl, Show, % ART_hwnd.Bonustext
		GuiControl, Show, % ART_hwnd.Bonus
		GuiControl, Show, % ART_hwnd.BonusEdit
		GuiControl, Show, % ART_hwnd.Weighttext
		GuiControl, Show, % ART_hwnd.Weight
		GuiControl, Show, % ART_hwnd.WeightUnit
		GuiControl, Show, % ART_hwnd.WeaponDamageText
		GuiControl, Show, % ART_hwnd.WeaponDamageNumberText
		GuiControl, Show, % ART_hwnd.WeaponDamageNumber
		GuiControl, Show, % ART_hwnd.WeaponDamageDieText
		GuiControl, Show, % ART_hwnd.WeaponDamageDText
		GuiControl, Show, % ART_hwnd.WeaponDamageDie
		GuiControl, Show, % ART_hwnd.WeaponDamagePlusText
		GuiControl, Show, % ART_hwnd.WeaponDamageBonusText
		GuiControl, Show, % ART_hwnd.WeaponDamageBonus
		GuiControl, Show, % ART_hwnd.WeaponDamageTypeText
		GuiControl, Show, % ART_hwnd.WeaponDamageType
		GuiControl, Show, % ART_hwnd.WeaponPropText
		GuiControl, Show, % ART_hwnd.WeaponProp
		GuiControl, Show, % ART_hwnd.WeaponPrRerollText
		GuiControl, Show, % ART_hwnd.WeaponPrReroll
		GuiControl, Show, % ART_hwnd.WeaponPrCritRangeText
		GuiControl, Show, % ART_hwnd.WeaponPrCritRange
		GuiControl,, % ART_hwnd.subtype, |--||Weapon
		GuiControl, Enable, % ART_hwnd.subtype
	}
	If (EQtype = "Wand") {
		Art_Hideall()
		ART_ClearAllBut("Wand")
		GuiControl, Show, % ART_hwnd.Weighttext
		GuiControl, Show, % ART_hwnd.Weight
		GuiControl, Show, % ART_hwnd.WeightUnit
		GuiControl,, % ART_hwnd.subtype, |--
		GuiControl, Disable, % ART_hwnd.subtype
	}
	If (EQtype = "Wondrous Item") {
		Art_Hideall()
		ART_ClearAllBut("Scroll")
		GuiControl, Show, % ART_hwnd.Weighttext
		GuiControl, Show, % ART_hwnd.Weight
		GuiControl, Show, % ART_hwnd.WeightUnit
		GuiControl,, % ART_hwnd.subtype, |--
		GuiControl, Disable, % ART_hwnd.subtype
	}
	ART_Subtype()
}

Art_Hideall() {
	global
	GuiControl, Hide, % ART_hwnd.Bonustext
	GuiControl, Hide, % ART_hwnd.Bonus
	GuiControl, Hide, % ART_hwnd.BonusEdit
	
	GuiControl, Hide, % ART_hwnd.ArmorACtext
	GuiControl, Hide, % ART_hwnd.ArmorAC
	GuiControl, Hide, % ART_hwnd.ArmorStrengthtext
	GuiControl, Hide, % ART_hwnd.ArmorStrength
	GuiControl, Hide, % ART_hwnd.ArmorStealthtext
	GuiControl, Hide, % ART_hwnd.ArmorStealth

	GuiControl, Hide, % ART_hwnd.WeaponDamageText
	GuiControl, Hide, % ART_hwnd.WeaponDamageNumberText
	GuiControl, Hide, % ART_hwnd.WeaponDamageNumber
	GuiControl, Hide, % ART_hwnd.WeaponDamageDieText
	GuiControl, Hide, % ART_hwnd.WeaponDamageDText
	GuiControl, Hide, % ART_hwnd.WeaponDamageDie
	GuiControl, Hide, % ART_hwnd.WeaponDamagePlusText
	GuiControl, Hide, % ART_hwnd.WeaponDamageBonusText
	GuiControl, Hide, % ART_hwnd.WeaponDamageBonus
	GuiControl, Hide, % ART_hwnd.WeaponDamageTypeText
	GuiControl, Hide, % ART_hwnd.WeaponDamageType
	GuiControl, Hide, % ART_hwnd.WeaponBDamageText
	GuiControl, Hide, % ART_hwnd.WeaponBDamageNumber
	GuiControl, Hide, % ART_hwnd.WeaponBDamageDText
	GuiControl, Hide, % ART_hwnd.WeaponBDamageDie
	GuiControl, Hide, % ART_hwnd.WeaponBDamagePlusText
	GuiControl, Hide, % ART_hwnd.WeaponBDamageBonus
	GuiControl, Hide, % ART_hwnd.WeaponBDamageType
	GuiControl, Hide, % ART_hwnd.WeaponBDamageAdd
	GuiControl, Hide, % ART_hwnd.WeaponNormalRangeText
	GuiControl, Hide, % ART_hwnd.WeaponNormalRange
	GuiControl, Hide, % ART_hwnd.WeaponLongRangeText
	GuiControl, Hide, % ART_hwnd.WeaponLongRange
	GuiControl, Hide, % ART_hwnd.WeaponPropText
	GuiControl, Hide, % ART_hwnd.WeaponProp
	GuiControl, Hide, % ART_hwnd.WeaponMatText
	GuiControl, Hide, % ART_hwnd.WeaponMat
	GuiControl, Hide, % ART_hwnd.WeaponPrRerollText
	GuiControl, Hide, % ART_hwnd.WeaponPrReroll
	GuiControl, Hide, % ART_hwnd.WeaponPrCritRangeText
	GuiControl, Hide, % ART_hwnd.WeaponPrCritRange
}

Art_ClearAllBut(nm) {
	Global Arte, ART_Hwnd
	If (nm = "Armor") {
		Arte.WeaponType:= "Melee Weapon Attack"
		Arte.WeaponDamageDie:= 6
		Arte.WeaponDamageType:= "slashing"
		Arte.WeaponBDamageDie:= 6
		Arte.WeaponBDamageAdd:= 0
		Arte.WeaponPrReroll:= 0
		Arte.WeaponPrCritRange:= 20
		Arte.WeaponDamageNumber:= ""
		Arte.WeaponDamageBonus:= ""
		Arte.WeaponBDamageNumber:= ""
		Arte.WeaponBDamageBonus:= ""
		Arte.WeaponBDamageType:= ""
		Arte.WeaponNormalRange:= ""
		Arte.WeaponLongRange:= ""
		Gset(ART_Hwnd.Weight, Arte.Weight)
		
		Gset(ART_Hwnd.WeaponType, Arte.WeaponType)
		Gset(ART_Hwnd.WeaponDamageNumber, Arte.WeaponDamageNumber)
		Gset(ART_Hwnd.WeaponDamageDie, Arte.WeaponDamageDie)
		Gset(ART_Hwnd.WeaponDamageBonus, Arte.WeaponDamageBonus)
		Gset(ART_Hwnd.WeaponDamageType, Arte.WeaponDamageType)
		Gset(ART_Hwnd.WeaponBDamageNumber, Arte.WeaponBDamageNumber)
		Gset(ART_Hwnd.WeaponBDamageDie, Arte.WeaponBDamageDie)
		Gset(ART_Hwnd.WeaponBDamageBonus, Arte.WeaponBDamageBonus)
		Gset(ART_Hwnd.WeaponBDamageType, Arte.WeaponBDamageType)
		Gset(ART_Hwnd.WeaponBDamageAdd, Arte.WeaponBDamageAdd)
		Gset(ART_Hwnd.WeaponNormalRange, Arte.WeaponNormalRange)
		Gset(ART_Hwnd.WeaponLongRange, Arte.WeaponLongRange)
		Gset(ART_Hwnd.WeaponPrReroll, Arte.WeaponPrReroll)
		Gset(ART_Hwnd.WeaponPrCritRange, Arte.WeaponPrCritRange)
		
		Gset(ART_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(ART_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Weapon") {
		Arte.ArmorAC:= ""
		Arte.ArmorStrength:= ""
		Arte.ArmorStealth:= "-"
		Gset(ART_Hwnd.Weight, Arte.Weight)
		
		Gset(ART_Hwnd.ArmorAC, Arte.ArmorAC)
		Gset(ART_Hwnd.ArmorStrength, Arte.ArmorStrength)
		Gset(ART_Hwnd.ArmorStealth, Arte.ArmorStealth)
	}
	If (nm = "Potion") {
		Arte.Bonus:= ""
		
		Arte.ArmorAC:= ""
		Arte.ArmorStrength:= ""
		Arte.ArmorStealth:= "-"
		
		Arte.WeaponType:= "Melee Weapon Attack"
		Arte.WeaponDamageDie:= 6
		Arte.WeaponDamageType:= "slashing"
		Arte.WeaponBDamageDie:= 6
		Arte.WeaponBDamageAdd:= 0
		Arte.WeaponPrReroll:= 0
		Arte.WeaponPrCritRange:= 20
		Arte.WeaponDamageNumber:= ""
		Arte.WeaponDamageBonus:= ""
		Arte.WeaponBDamageNumber:= ""
		Arte.WeaponBDamageBonus:= ""
		Arte.WeaponBDamageType:= ""
		Arte.WeaponNormalRange:= ""
		Arte.WeaponLongRange:= ""
		
		Gset(ART_Hwnd.ArmorAC, Arte.ArmorAC)
		Gset(ART_Hwnd.ArmorStrength, Arte.ArmorStrength)
		Gset(ART_Hwnd.ArmorStealth, Arte.ArmorStealth)
		
		Gset(ART_Hwnd.WeaponType, Arte.WeaponType)
		Gset(ART_Hwnd.WeaponDamageNumber, Arte.WeaponDamageNumber)
		Gset(ART_Hwnd.WeaponDamageDie, Arte.WeaponDamageDie)
		Gset(ART_Hwnd.WeaponDamageBonus, Arte.WeaponDamageBonus)
		Gset(ART_Hwnd.WeaponDamageType, Arte.WeaponDamageType)
		Gset(ART_Hwnd.WeaponBDamageNumber, Arte.WeaponBDamageNumber)
		Gset(ART_Hwnd.WeaponBDamageDie, Arte.WeaponBDamageDie)
		Gset(ART_Hwnd.WeaponBDamageBonus, Arte.WeaponBDamageBonus)
		Gset(ART_Hwnd.WeaponBDamageType, Arte.WeaponBDamageType)
		Gset(ART_Hwnd.WeaponBDamageAdd, Arte.WeaponBDamageAdd)
		Gset(ART_Hwnd.WeaponNormalRange, Arte.WeaponNormalRange)
		Gset(ART_Hwnd.WeaponLongRange, Arte.WeaponLongRange)
		Gset(ART_Hwnd.WeaponPrReroll, Arte.WeaponPrReroll)
		Gset(ART_Hwnd.WeaponPrCritRange, Arte.WeaponPrCritRange)
		
		Gset(ART_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(ART_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Scroll") {
		Arte.Bonus:= ""
		
		Arte.ArmorAC:= ""
		Arte.ArmorStrength:= ""
		Arte.ArmorStealth:= "-"
		
		Arte.WeaponType:= "Melee Weapon Attack"
		Arte.WeaponDamageDie:= 6
		Arte.WeaponDamageType:= "slashing"
		Arte.WeaponBDamageDie:= 6
		Arte.WeaponBDamageAdd:= 0
		Arte.WeaponPrReroll:= 0
		Arte.WeaponPrCritRange:= 20
		Arte.WeaponDamageNumber:= ""
		Arte.WeaponDamageBonus:= ""
		Arte.WeaponBDamageNumber:= ""
		Arte.WeaponBDamageBonus:= ""
		Arte.WeaponBDamageType:= ""
		Arte.WeaponNormalRange:= ""
		Arte.WeaponLongRange:= ""
		
		Gset(ART_Hwnd.ArmorAC, Arte.ArmorAC)
		Gset(ART_Hwnd.ArmorStrength, Arte.ArmorStrength)
		Gset(ART_Hwnd.ArmorStealth, Arte.ArmorStealth)
		
		Gset(ART_Hwnd.WeaponType, Arte.WeaponType)
		Gset(ART_Hwnd.WeaponDamageNumber, Arte.WeaponDamageNumber)
		Gset(ART_Hwnd.WeaponDamageDie, Arte.WeaponDamageDie)
		Gset(ART_Hwnd.WeaponDamageBonus, Arte.WeaponDamageBonus)
		Gset(ART_Hwnd.WeaponDamageType, Arte.WeaponDamageType)
		Gset(ART_Hwnd.WeaponBDamageNumber, Arte.WeaponBDamageNumber)
		Gset(ART_Hwnd.WeaponBDamageDie, Arte.WeaponBDamageDie)
		Gset(ART_Hwnd.WeaponBDamageBonus, Arte.WeaponBDamageBonus)
		Gset(ART_Hwnd.WeaponBDamageType, Arte.WeaponBDamageType)
		Gset(ART_Hwnd.WeaponBDamageAdd, Arte.WeaponBDamageAdd)
		Gset(ART_Hwnd.WeaponNormalRange, Arte.WeaponNormalRange)
		Gset(ART_Hwnd.WeaponLongRange, Arte.WeaponLongRange)
		Gset(ART_Hwnd.WeaponPrReroll, Arte.WeaponPrReroll)
		Gset(ART_Hwnd.WeaponPrCritRange, Arte.WeaponPrCritRange)
		
		Gset(ART_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(ART_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Ring") {
		Arte.Bonus:= ""
		
		Arte.ArmorAC:= ""
		Arte.ArmorStrength:= ""
		Arte.ArmorStealth:= "-"
		
		Arte.WeaponType:= "Melee Weapon Attack"
		Arte.WeaponDamageDie:= 6
		Arte.WeaponDamageType:= "slashing"
		Arte.WeaponBDamageDie:= 6
		Arte.WeaponBDamageAdd:= 0
		Arte.WeaponPrReroll:= 0
		Arte.WeaponPrCritRange:= 20
		Arte.WeaponDamageNumber:= ""
		Arte.WeaponDamageBonus:= ""
		Arte.WeaponBDamageNumber:= ""
		Arte.WeaponBDamageBonus:= ""
		Arte.WeaponBDamageType:= ""
		Arte.WeaponNormalRange:= ""
		Arte.WeaponLongRange:= ""
		
		Gset(ART_Hwnd.ArmorAC, Arte.ArmorAC)
		Gset(ART_Hwnd.ArmorStrength, Arte.ArmorStrength)
		Gset(ART_Hwnd.ArmorStealth, Arte.ArmorStealth)
		
		Gset(ART_Hwnd.WeaponType, Arte.WeaponType)
		Gset(ART_Hwnd.WeaponDamageNumber, Arte.WeaponDamageNumber)
		Gset(ART_Hwnd.WeaponDamageDie, Arte.WeaponDamageDie)
		Gset(ART_Hwnd.WeaponDamageBonus, Arte.WeaponDamageBonus)
		Gset(ART_Hwnd.WeaponDamageType, Arte.WeaponDamageType)
		Gset(ART_Hwnd.WeaponBDamageNumber, Arte.WeaponBDamageNumber)
		Gset(ART_Hwnd.WeaponBDamageDie, Arte.WeaponBDamageDie)
		Gset(ART_Hwnd.WeaponBDamageBonus, Arte.WeaponBDamageBonus)
		Gset(ART_Hwnd.WeaponBDamageType, Arte.WeaponBDamageType)
		Gset(ART_Hwnd.WeaponBDamageAdd, Arte.WeaponBDamageAdd)
		Gset(ART_Hwnd.WeaponNormalRange, Arte.WeaponNormalRange)
		Gset(ART_Hwnd.WeaponLongRange, Arte.WeaponLongRange)
		Gset(ART_Hwnd.WeaponPrReroll, Arte.WeaponPrReroll)
		Gset(ART_Hwnd.WeaponPrCritRange, Arte.WeaponPrCritRange)
		
		Gset(ART_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(ART_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Rod") {
		Arte.Bonus:= ""
		
		Arte.ArmorAC:= ""
		Arte.ArmorStrength:= ""
		Arte.ArmorStealth:= "-"
		
		Arte.WeaponType:= "Melee Weapon Attack"
		Arte.WeaponDamageDie:= 6
		Arte.WeaponDamageType:= "slashing"
		Arte.WeaponBDamageDie:= 6
		Arte.WeaponBDamageAdd:= 0
		Arte.WeaponPrReroll:= 0
		Arte.WeaponPrCritRange:= 20
		Arte.WeaponDamageNumber:= ""
		Arte.WeaponDamageBonus:= ""
		Arte.WeaponBDamageNumber:= ""
		Arte.WeaponBDamageBonus:= ""
		Arte.WeaponBDamageType:= ""
		Arte.WeaponNormalRange:= ""
		Arte.WeaponLongRange:= ""
		
		Gset(ART_Hwnd.ArmorAC, Arte.ArmorAC)
		Gset(ART_Hwnd.ArmorStrength, Arte.ArmorStrength)
		Gset(ART_Hwnd.ArmorStealth, Arte.ArmorStealth)
		
		Gset(ART_Hwnd.WeaponType, Arte.WeaponType)
		Gset(ART_Hwnd.WeaponDamageNumber, Arte.WeaponDamageNumber)
		Gset(ART_Hwnd.WeaponDamageDie, Arte.WeaponDamageDie)
		Gset(ART_Hwnd.WeaponDamageBonus, Arte.WeaponDamageBonus)
		Gset(ART_Hwnd.WeaponDamageType, Arte.WeaponDamageType)
		Gset(ART_Hwnd.WeaponBDamageNumber, Arte.WeaponBDamageNumber)
		Gset(ART_Hwnd.WeaponBDamageDie, Arte.WeaponBDamageDie)
		Gset(ART_Hwnd.WeaponBDamageBonus, Arte.WeaponBDamageBonus)
		Gset(ART_Hwnd.WeaponBDamageType, Arte.WeaponBDamageType)
		Gset(ART_Hwnd.WeaponBDamageAdd, Arte.WeaponBDamageAdd)
		Gset(ART_Hwnd.WeaponNormalRange, Arte.WeaponNormalRange)
		Gset(ART_Hwnd.WeaponLongRange, Arte.WeaponLongRange)
		Gset(ART_Hwnd.WeaponPrReroll, Arte.WeaponPrReroll)
		Gset(ART_Hwnd.WeaponPrCritRange, Arte.WeaponPrCritRange)
		
		Gset(ART_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(ART_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Staff") {
		Arte.ArmorAC:= ""
		Arte.ArmorStrength:= ""
		Arte.ArmorStealth:= "-"
		Gset(ART_Hwnd.Weight, Arte.Weight)
		
		Gset(ART_Hwnd.ArmorAC, Arte.ArmorAC)
		Gset(ART_Hwnd.ArmorStrength, Arte.ArmorStrength)
		Gset(ART_Hwnd.ArmorStealth, Arte.ArmorStealth)
	}
	If (nm = "Wand") {
		Arte.Bonus:= ""
		
		Arte.ArmorAC:= ""
		Arte.ArmorStrength:= ""
		Arte.ArmorStealth:= "-"
		
		Arte.WeaponType:= "Melee Weapon Attack"
		Arte.WeaponDamageDie:= 6
		Arte.WeaponDamageType:= "slashing"
		Arte.WeaponBDamageDie:= 6
		Arte.WeaponBDamageAdd:= 0
		Arte.WeaponPrReroll:= 0
		Arte.WeaponPrCritRange:= 20
		Arte.WeaponDamageNumber:= ""
		Arte.WeaponDamageBonus:= ""
		Arte.WeaponBDamageNumber:= ""
		Arte.WeaponBDamageBonus:= ""
		Arte.WeaponBDamageType:= ""
		Arte.WeaponNormalRange:= ""
		Arte.WeaponLongRange:= ""
		
		Gset(ART_Hwnd.ArmorAC, Arte.ArmorAC)
		Gset(ART_Hwnd.ArmorStrength, Arte.ArmorStrength)
		Gset(ART_Hwnd.ArmorStealth, Arte.ArmorStealth)
		
		Gset(ART_Hwnd.WeaponType, Arte.WeaponType)
		Gset(ART_Hwnd.WeaponDamageNumber, Arte.WeaponDamageNumber)
		Gset(ART_Hwnd.WeaponDamageDie, Arte.WeaponDamageDie)
		Gset(ART_Hwnd.WeaponDamageBonus, Arte.WeaponDamageBonus)
		Gset(ART_Hwnd.WeaponDamageType, Arte.WeaponDamageType)
		Gset(ART_Hwnd.WeaponBDamageNumber, Arte.WeaponBDamageNumber)
		Gset(ART_Hwnd.WeaponBDamageDie, Arte.WeaponBDamageDie)
		Gset(ART_Hwnd.WeaponBDamageBonus, Arte.WeaponBDamageBonus)
		Gset(ART_Hwnd.WeaponBDamageType, Arte.WeaponBDamageType)
		Gset(ART_Hwnd.WeaponBDamageAdd, Arte.WeaponBDamageAdd)
		Gset(ART_Hwnd.WeaponNormalRange, Arte.WeaponNormalRange)
		Gset(ART_Hwnd.WeaponLongRange, Arte.WeaponLongRange)
		Gset(ART_Hwnd.WeaponPrReroll, Arte.WeaponPrReroll)
		Gset(ART_Hwnd.WeaponPrCritRange, Arte.WeaponPrCritRange)
		
		Gset(ART_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(ART_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
	If (nm = "Wondrous Item") {
		Arte.Bonus:= ""
		
		Arte.ArmorAC:= ""
		Arte.ArmorStrength:= ""
		Arte.ArmorStealth:= "-"
		
		Arte.WeaponType:= "Melee Weapon Attack"
		Arte.WeaponDamageDie:= 6
		Arte.WeaponDamageType:= "slashing"
		Arte.WeaponBDamageDie:= 6
		Arte.WeaponBDamageAdd:= 0
		Arte.WeaponPrReroll:= 0
		Arte.WeaponPrCritRange:= 20
		Arte.WeaponDamageNumber:= ""
		Arte.WeaponDamageBonus:= ""
		Arte.WeaponBDamageNumber:= ""
		Arte.WeaponBDamageBonus:= ""
		Arte.WeaponBDamageType:= ""
		Arte.WeaponNormalRange:= ""
		Arte.WeaponLongRange:= ""
		
		Gset(ART_Hwnd.ArmorAC, Arte.ArmorAC)
		Gset(ART_Hwnd.ArmorStrength, Arte.ArmorStrength)
		Gset(ART_Hwnd.ArmorStealth, Arte.ArmorStealth)
		
		Gset(ART_Hwnd.WeaponType, Arte.WeaponType)
		Gset(ART_Hwnd.WeaponDamageNumber, Arte.WeaponDamageNumber)
		Gset(ART_Hwnd.WeaponDamageDie, Arte.WeaponDamageDie)
		Gset(ART_Hwnd.WeaponDamageBonus, Arte.WeaponDamageBonus)
		Gset(ART_Hwnd.WeaponDamageType, Arte.WeaponDamageType)
		Gset(ART_Hwnd.WeaponBDamageNumber, Arte.WeaponBDamageNumber)
		Gset(ART_Hwnd.WeaponBDamageDie, Arte.WeaponBDamageDie)
		Gset(ART_Hwnd.WeaponBDamageBonus, Arte.WeaponBDamageBonus)
		Gset(ART_Hwnd.WeaponBDamageType, Arte.WeaponBDamageType)
		Gset(ART_Hwnd.WeaponBDamageAdd, Arte.WeaponBDamageAdd)
		Gset(ART_Hwnd.WeaponNormalRange, Arte.WeaponNormalRange)
		Gset(ART_Hwnd.WeaponLongRange, Arte.WeaponLongRange)
		Gset(ART_Hwnd.WeaponPrReroll, Arte.WeaponPrReroll)
		Gset(ART_Hwnd.WeaponPrCritRange, Arte.WeaponPrCritRange)
		
		Gset(ART_Hwnd.WeaponProp, "|Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile")
		Gset(ART_Hwnd.WeaponMat, "|Silver|Adamantine|Cold-forged iron")
	}
}

ART_Subtype() {
	global
	local EQtype, EQsubtype
	EQtype:= Gget(ART_Hwnd.type)
	EQsubtype:= Gget(ART_Hwnd.subtype)
	Arte.subtype:= Gget(ART_Hwnd.subtype)
	ART_RH_Box()
}

ART_BonusDamage() {
	Global ART_Hwnd
	WBD:= Gget(ART_Hwnd.WeaponBDamageAdd)
	GuiControl, ART_Main:Enable%WBD%, % ART_Hwnd.WeaponBDamageText
	GuiControl, ART_Main:Enable%WBD%, % ART_Hwnd.WeaponBDamageNumber
	GuiControl, ART_Main:Enable%WBD%, % ART_Hwnd.WeaponBDamageDText
	GuiControl, ART_Main:Enable%WBD%, % ART_Hwnd.WeaponBDamageDie
	GuiControl, ART_Main:Enable%WBD%, % ART_Hwnd.WeaponBDamagePlusText
	GuiControl, ART_Main:Enable%WBD%, % ART_Hwnd.WeaponBDamageBonus
	GuiControl, ART_Main:Enable%WBD%, % ART_Hwnd.WeaponBDamageType
}

ART_Notes() {
	AddNotes("Arte", "ART")
}

ART_Vup() {
	global ART_VP, ART_ScrollPoint, ART_ScrollEnd, ART_buttonup, ART_buttondn
	MouseGetPos,,,,ctrl, 2
	while (ctrl=ART_buttonup && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		ART_VP.Document.parentWindow.eval("scrollBy(0, -2);")
		ART_ScrollPoint -= 2
		If (ART_ScrollPoint < 0) {
			ART_ScrollPoint:= 0
		}
	}
	while (ctrl=ART_buttondn && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		ART_VP.Document.parentWindow.eval("scrollBy(0, 2);")
		ART_ScrollPoint += 2
		If (ART_ScrollPoint > ART_ScrollEnd) {
			ART_ScrollPoint:= ART_ScrollEnd
		}
	}
}

ART_VScrUp() {
	global ART_VP, IMSpell, ART_ScrollPoint, IMScrollPoint, ART_Main, ART_Import
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" ART_Main) {
		ART_VP.Document.parentWindow.eval("scrollBy(0, -50);")
		ART_ScrollPoint -= 50
		If (ART_ScrollPoint < 0) {
			ART_ScrollPoint:= 0
		}
	}
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" ART_Import) {
		IMSpell.Document.parentWindow.eval("scrollBy(0, -50);")
		IMScrollPoint -= 50
		If (IMScrollPoint < 0) {
			IMScrollPoint:= 0
		}
	}
}

ART_VScrDwn() {
	global ART_VP, IMSpell, ART_ScrollPoint, IMScrollPoint, ART_ScrollEnd, IMScrollEnd, ART_Main, ART_Import
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" ART_Main) {
		ART_VP.Document.parentWindow.eval("scrollBy(0, 50);")
		ART_ScrollPoint += 50
		If (ART_ScrollPoint > ART_ScrollEnd) {
			ART_ScrollPoint:= ART_ScrollEnd
		}
	}
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" ART_Import) {
		IMSpell.Document.parentWindow.eval("scrollBy(0, 50);")
		IMScrollPoint += 50
		If (IMScrollPoint > IMScrollEnd) {
			IMScrollPoint:= IMScrollEnd
		}
	}
}

ART_JSONFile() {
	global Arte, Objart
	Objart:= {}
	ObjName:= Arte.filename
	Objart[ObjName]:= {}
	Objart[ObjName].name:= Arte.name
	Objart[ObjName].type:= Arte.type
	Objart[ObjName].subtype:= Arte.subtype
	Objart[ObjName].NoID:= Arte.NoID
	Objart[ObjName].NoIDNotes:= Arte.NoIDNotes
	Objart[ObjName].cost:= Arte.cost " " Arte.costUnit
	Objart[ObjName].Weight:= Arte.Weight
	Objart[ObjName].Rarity:= Arte.Rarity
	Objart[ObjName].Attune:= Arte.Attune
	
	If (Arte.type = "Armor") {
		Objart[ObjName].Weight:= Arte.Weight
		Objart[ObjName].AC:= Arte.ArmorAC
		If Arte.ArmorStrength is number {
			Objart[ObjName].armourstr:= "Str " Arte.ArmorStrength
		} Else {
			Objart[ObjName].armourstr:= "-"
		}
		Objart[ObjName].armourstealth:= Arte.ArmorStealth
		Objart[ObjName].armourdexbonus:= Arte.dexbonus
		Objart[ObjName].bonus:= Arte.bonus
	}
	If (Arte.type = "Weapon") {
		Objart[ObjName].Weight:= Arte.Weight
		If Arte.WeaponDamageNumber {
			Objart[ObjName].Damage:= Arte.WeaponDamageNumber "d" Arte.WeaponDamageDie
			If Arte.WeaponDamageBonus
				Objart[ObjName].Damage .= "+" Arte.WeaponDamageBonus
			Objart[ObjName].Damage .= " " Arte.WeaponDamageType
		}
		If (Arte.WeaponProp) AND (Arte.WeaponMat) {
			EQEWeapProp:= Arte.WeaponProp ", " Arte.WeaponMat
		} Else if (Arte.WeaponProp) AND (!Arte.WeaponMat) {
			EQEWeapProp:= Arte.WeaponProp
		} Else if !(Arte.WeaponProp) AND (Arte.WeaponMat) {
			EQEWeapProp:= Arte.WeaponMat
		} Else {
			EQEWeapProp:= ""
		}
		EQErange:= "(range " Arte.WeaponNormalRange "/" Arte.WeaponLongRange ")"
		stringreplace, EQEWeapProp, EQEWeapProp, Ammunition, Ammunition %EQErange%
		stringreplace, EQEWeapProp, EQEWeapProp, Thrown, Thrown %EQErange%
		If (Arte.WeaponDamageDie = 4) {
			EQEvers:= "Versatile (1d6)"
		} else if (Arte.WeaponDamageDie = 6) {
			EQEvers:= "Versatile (1d8)"
		} else if (Arte.WeaponDamageDie = 8) {
			EQEvers:= "Versatile (1d10)"
		} else if (Arte.WeaponDamageDie = 10) {
			EQEvers:= "Versatile (1d12)"
		} else if (Arte.WeaponDamageDie = 12) {
			EQEvers:= "Versatile (1d20)"
		}
		stringreplace, EQEWeapProp, EQEWeapProp, Versatile, %EQEvers%
		Objart[ObjName].Properties:= EQEWeapProp
		Objart[ObjName].bonus:= Arte.bonus
	}
	Objart[ObjName].jpeg:= Arte.jpeg
	Objart[ObjName].ImageLink:= Arte.ImageLink
	Objart[ObjName].Artist:= Arte.Artist
	Objart[ObjName].ArtistLink:= Arte.ArtistLink
	Objart[ObjName].FGcat:= Arte.FGcat
	Objart[ObjName].Locked:= Arte.Locked
	

	SpText:= "`t`t`t`t`t<description type=""formattedtext"">" Arte.notes

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
		
	Objart[ObjName].description:= TKN
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                 GUI Functions                |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

ART_Graphical(UI, Scr) {
	Global ART_VP, IMspell, Arte
	
	;{ Data for Statblock
	EQEName:= Arte.name
	EQENonid:= Arte.NoID
	EQENonidDesc:= Arte.NoIDNotes
	EQEtype:= Arte.type
	If Arte.subtype
		EQEsubtype:= Arte.subtype
	else
		EQEsubtype:= "--"
	If Arte.cost
		EQEcost:= Arte.cost " " Arte.CostUnit
	EQEarmourclass:= Arte.ArmorAC
	If Arte.ArmorStrength is number {
		EQEarmourstr:= "Str " Arte.ArmorStrength
	} Else {
		EQEarmourstr:= "-"
	}		
	EQEarmourstealth:= Arte.ArmorStealth
	EQEarmourdexbon:= Arte.dexbonus
	If Arte.Weight
		EQEweight:= Arte.weight " lb."
	if (Arte.bonus>0) {
		magbonus:= "+" Arte.bonus
	} else if (Arte.bonus<0) {
		magbonus:= Arte.bonus
	} else if (Arte.bonus=0) {
		magbonus:= "-"
	}
	notes:= Arte.notes "<br>"
	StringReplace, notes, notes, <frame>, %frtable%, all
	StringReplace, notes, notes, </frame>, %frtablend%, all
	StringReplace, notes, notes, <h>, <span class=`"heading`">, all
	StringReplace, notes, notes, </h>, </span>, all

	SpText .= notes
	SpText:= LinkHTML(SpText)

	If Arte.WeaponDamageNumber {
		EQEWeapDam:= Arte.WeaponDamageNumber "d" Arte.WeaponDamageDie
		If Arte.WeaponDamageBonus
			EQEWeapDam .= "+" Arte.WeaponDamageBonus
		EQEWeapDam .= " " Arte.WeaponDamageType
	}
	
	If (Arte.WeaponProp) AND (Arte.WeaponMat) {
		EQEWeapProp:= Arte.WeaponProp ", " Arte.WeaponMat
	} Else if (Arte.WeaponProp) AND (!Arte.WeaponMat) {
		EQEWeapProp:= Arte.WeaponProp
	} Else if !(Arte.WeaponProp) AND (Arte.WeaponMat) {
		EQEWeapProp:= Arte.WeaponMat
	} Else {
		EQEWeapProp:= ""
	}
	EQErange:= "(range " Arte.WeaponNormalRange "/" Arte.WeaponLongRange ")"
	stringreplace, EQEWeapProp, EQEWeapProp, Ammunition, Ammunition %EQErange%
	stringreplace, EQEWeapProp, EQEWeapProp, Thrown, Thrown %EQErange%
	If (Arte.WeaponDamageDie = 4) {
		EQEvers:= "Versatile (1d6)"
	} else if (Arte.WeaponDamageDie = 6) {
		EQEvers:= "Versatile (1d8)"
	} else if (Arte.WeaponDamageDie = 8) {
		EQEvers:= "Versatile (1d10)"
	} else if (Arte.WeaponDamageDie = 10) {
		EQEvers:= "Versatile (1d12)"
	} else if (Arte.WeaponDamageDie = 12) {
		EQEvers:= "Versatile (1d20)"
	}
	stringreplace, EQEWeapProp, EQEWeapProp, Versatile, %EQEvers%
	
	EQErarity:= arte.rarity
	if arte.attune
		EQEattune:= "Required"
	else
		EQEattune:= "No"
	;}

	#include HTML_Artefact_Engineer.ahk
	HTML_Spell:= css . htmtop
	If (Arte.type = "Armor") {
		HTML_Spell .= htmMid2
	}
	If (Arte.type = "Weapon") {
		HTML_Spell .= htmMid3
	}
	If (Arte.type = "Staff") {
		HTML_Spell .= htmMid3
	}
	HTML_Spell .= htmbottom
	
	
	
	
	stringreplace, HTML_Spell, HTML_Spell, \r, <br>, All

	documentz:= %UI%.Document
	documentz.open()
	documentz.write(HTML_Spell)
	documentz.close()
	%UI%.Document.parentWindow.eval("scrollTo(0, " Scr ");")
}


ART_PrepareGUI() {
	global
	ART_GUIMain()
	If (ART_Ref = "self") {
		GUI_Project()
	}
	ART_SetTT()
}

ART_GUIMain()	 {
	global
	local tempy, jsonpath, weaplist
	
	Gui, ART_Main:-MaximizeBox
	Gui, ART_Main:+hwndART_Main
	Gui, ART_Main:Color, 0xE2E1E8
	Gui, ART_Main:font, S10 c000000, Arial
	
; Menu system
;{
	Menu ART_FileMenu, Add, &New Artefact Item`tCtrl+N, New_Artefact
	Menu ART_FileMenu, Icon, &New Artefact Item`tCtrl+N, NPC Engineer.dll, 1
	Menu ART_FileMenu, Add, &Open Artefact Item`tCtrl+O, Open_Artefact
	Menu ART_FileMenu, Icon, &Open Artefact Item`tCtrl+O, NPC Engineer.dll, 2
	Menu ART_FileMenu, Add, &Save Artefact Item`tCtrl+S, Save_Artefact
	Menu ART_FileMenu, Icon, &Save Artefact Item`tCtrl+S, NPC Engineer.dll, 3
	Menu ART_FileMenu, Add
	;~ Menu ART_FileMenu, Add, Save as Text, Save_Txt
	;~ Menu ART_FileMenu, Add, Save as XML, Save_XML
	;~ Menu ART_FileMenu, Add, Save as HTML, Save_HTML
	;~ Menu ART_FileMenu, Add, Save as RTF, Save_RTF
	;~ Menu ART_FileMenu, Add
	;~ Menu ART_FileMenu, Add, Place BBCode on Clipboard, Copy_BB
	;~ Menu ART_FileMenu, Add
	Menu ART_FileMenu, Add, E&xit`tESC, ART_MainGuiClose
	Menu ART_FileMenu, Icon, E&xit`tESC, NPC Engineer.dll, 17
	Menu ArtefactEngineerMenu, Add, File, :ART_FileMenu
	
	Menu ART_OptionsMenu, Add, &Import Text`tCtrl+I, Import_ART_Text
	Menu ART_OptionsMenu, Icon, &Import Text`tCtrl+I, NPC Engineer.dll, 4
	Menu ART_OptionsMenu, Add, &Create Module `tCtrl+P, ParseProject
	Menu ART_OptionsMenu, Icon, &Create Module `tCtrl+P, NPC Engineer.dll, 6
	Menu ART_OptionsMenu, Add
	Menu ART_OptionsMenu, Add, Manage Categories `tCtrl+K, GUI_Categories
	Menu ART_OptionsMenu, Icon, Manage Categories `tCtrl+K, NPC Engineer.dll, 25
	Menu ART_OptionsMenu, Add, Manage Artefact File `tCtrl+M, Manage_ART_JSON
	Menu ART_OptionsMenu, Add
	Menu ART_OptionsMenu, Add, Settings`tF11, GUI_Options
	Menu ART_OptionsMenu, Icon, Settings`tF11, NPC Engineer.dll, 9
	Menu ArtefactEngineerMenu, Add, Options, :ART_OptionsMenu
	
	Component_Menu("ART_ComponentMenu", "Artefact")
	Menu ArtefactEngineerMenu, Add, Engineer Suite, :ART_ComponentMenu
	
	Explorer_Menu("ART_ExplorerMenu")
	Menu ArtefactEngineerMenu, Add, Directories, :ART_ExplorerMenu
	
	Backup_Menu("ART_BackupMenu")
	Menu ArtefactEngineerMenu, Add, Backup, :ART_BackupMenu

	Help_Menu("ART_HelpMenu", "Artefact Engineer")
	Menu ArtefactEngineerMenu, Add, Information, :ART_HelpMenu
	Gui, ART_Main:Menu, ArtefactEngineerMenu
;}

; Tab 3 system for all Item input
	Gui, ART_Main:Add, Tab3, x7 y45 w606 h550, Artefact Info|Image

;  ================================================
;  |         GUI for 'Artefact info' tab         |
;  ================================================
;{
	Gui, ART_Main:Tab, 1
	
	Gui, ART_Main:Add, Text, x15 y83 w85 h17 Right, Name:
	Gui, ART_Main:Add, Text, x15 y110 w85 h17 Right, Non-ID Name:
	Gui, ART_Main:Add, Text, x425 y83 w85 h17 Right, Rarity:
	Gui, ART_Main:Add, Text, x15 y137 w85 h17 Disabled HwndTempy Right, Non-ID Notes:
		ART_Hwnd.NoIDNotesText:= Tempy
	Gui, ART_Main:Add, Edit, x105 y80 w300 h23 HwndTempy gART_MainUpdate,
		ART_Hwnd.name:= Tempy
	Gui, ART_Main:Add, Edit, x105 y107 w300 h23 HwndTempy gART_MainUpdate,
		ART_Hwnd.NoID:= Tempy
	Gui, ART_Main:Add, Edit, x105 y134 w500 h23 Disabled HwndTempy gART_MainUpdate,
		ART_Hwnd.NoIDNotes:= Tempy
	Gui, ART_Main:Add, ComboBox, x515 y80 w90 HwndTempy gART_MainUpdate, Common||Uncommon|Rare|Very rare|Legendary
		ART_hwnd.rarity:= Tempy
	Gui, ART_Main:Add, CheckBox, x440 y110 w165 h17 +0x20 Right HwndTempy Checked0 gART_MainUpdate, Requires attunement?: 
		ART_Hwnd.attune:= Tempy
		
	Gui, ART_Main:Add, Text, x15 y169 w85 h17 Right, Type:
	Gui, ART_Main:Add, Text, x341 y169 w55 h17 Right, Subtype:
	Gui, ART_Main:Add, Text, x15 y197 w85 h17 Right, Cost:
	Gui, ART_Main:Add, Text, x250 y197 w55 h17 HwndTempy Right, Weight:
		ART_hwnd.Weighttext:= Tempy
	Gui, ART_Main:Add, Text, x450 y197 w100 h17 HwndTempy Hidden Right, Magical Bonus:
		ART_hwnd.Bonustext:= Tempy

	Gui, ART_Main:Add, ComboBox, x105 y166 w230 HwndTempy gART_ItemType, Armor||Weapon|Potion|Scroll|Ring|Rod|Staff|Wand|Wondrous Item||
		ART_Hwnd.type:= Tempy
	Gui, ART_Main:Add, ComboBox, x400 y166 w205 Disabled HwndTempy gART_SubType, --
		ART_hwnd.subtype:= Tempy
	Gui, ART_Main:Add, Edit, x105 y194 w70 h23 Center HwndTempy gART_MainUpdate, 
		ART_hwnd.cost:= Tempy
	Gui, ART_Main:Add, ComboBox, x180 y194 w55 HwndTempy gART_MainUpdate, cp|sp|ep|gp||pp
		ART_hwnd.CostUnit:= Tempy
	Gui, ART_Main:Add, Edit, x310 y194 w70 h23 Center HwndTempy gART_MainUpdate,  
		ART_hwnd.Weight:= Tempy
	Gui, ART_Main:Add, Text, x385 y197 w30 HwndTempy, lb.
		ART_hwnd.WeightUnit:= Tempy
	Gui, ART_Main:Add, Edit, x555 y194 w50 h23 Hidden Center HwndTempy,  
		ART_hwnd.BonusEdit:= Tempy
	Gui, ART_Main:Add, UpDown, Hidden HwndTempy gART_MainUpdate Range-10-10, 0
		ART_hwnd.bonus:= Tempy
	
;{ GUI for armor
	Gui, ART_Main:Add, Text, x15 y245 w85 h17 Hidden HwndTempy Right, Armor Class:
		ART_hwnd.ArmorACtext:= Tempy
	Gui, ART_Main:Add, Edit, x105 y242 w50 h23 Hidden HwndTempy gART_MainUpdate Center, 
		ART_hwnd.ArmorAC:= Tempy

	Gui, ART_Main:Add, Text, x160 y245 w60 h17 Hidden HwndTempy Right, Strength:
		ART_hwnd.ArmorStrengthtext:= Tempy
	Gui, ART_Main:Add, Edit, x225 y242 w50 h23 Hidden HwndTempy gART_MainUpdate Center, 
		ART_hwnd.ArmorStrength:= Tempy

	Gui, ART_Main:Add, Text, x280 y245 w55 h17 Hidden HwndTempy Right, Stealth:
		ART_hwnd.ArmorStealthtext:= Tempy
	Gui, ART_Main:Add, ComboBox, x340 y242 w110 Hidden HwndTempy gART_MainUpdate, -||Disadvantage
		ART_hwnd.ArmorStealth:= Tempy
;}		

;{ GUI for weapons
	Gui, ART_Main:Add, Text, x15 y245 w85 h17 Hidden HwndTempy Right, Damage:
		ART_hwnd.WeaponDamageText:= Tempy
	Gui, ART_Main:Add, Text, x105 y223 w50 h17 Hidden HwndTempy Center, Number
		ART_hwnd.WeaponDamageNumberText:= Tempy
	Gui, ART_Main:Add, Edit, x105 y242 w50 h24 Hidden HwndTempy gART_MainUpdate Center, 
		ART_hwnd.WeaponDamageNumber:= Tempy

	Gui, ART_Main:Add, Text, x165 y223 w40 h17 Hidden HwndTempy Center, Die
		ART_hwnd.WeaponDamageDieText:= Tempy
	Gui, ART_Main:Add, Text, x156 y245 w8 h17 Hidden HwndTempy Center, d
		ART_hwnd.WeaponDamageDText:= Tempy
	Gui, ART_Main:Add, ComboBox, x165 y242 w40 Hidden HwndTempy gART_MainUpdate Center, 0|4|6||8|10|12|20
		ART_hwnd.WeaponDamageDie:= Tempy

	Gui, ART_Main:Add, Text, x206 y245 w8 h17 Hidden HwndTempy Center, +
		ART_hwnd.WeaponDamagePlusText:= Tempy
	Gui, ART_Main:Add, Text, x215 y223 w50 h17 Hidden HwndTempy Center, Bonus
		ART_hwnd.WeaponDamageBonusText:= Tempy
	Gui, ART_Main:Add, Edit, x215 y242 w50 h24 Hidden HwndTempy gART_MainUpdate Center, 
		ART_hwnd.WeaponDamageBonus:= Tempy

	Gui, ART_Main:Add, Text, x275 y223 w110 h17 Hidden HwndTempy Center, Damage Type
		ART_hwnd.WeaponDamageTypeText:= Tempy
	Gui, ART_Main:Add, ComboBox, x275 y242 w110 Hidden HwndTempy gART_MainUpdate Left, bludgeoning|piercing|slashing||acid|cold|fire|force|lightning|necrotic|poison|psychic|radiant|thunder
		ART_hwnd.WeaponDamageType:= Tempy



	Gui, ART_Main:Add, Text, x15 y275 w85 h17 disabled Hidden HwndTempy Right, Bonus:
		ART_hwnd.WeaponBDamageText:= Tempy
	Gui, ART_Main:Add, Edit, x105 y272 w50 h24 disabled Hidden HwndTempy gART_MainUpdate Center, 
		ART_hwnd.WeaponBDamageNumber:= Tempy
	Gui, ART_Main:Add, Text, x156 y275 w8 h17 disabled Hidden HwndTempy Center, d
		ART_hwnd.WeaponBDamageDText:= Tempy
	Gui, ART_Main:Add, ComboBox, x165 y272 w40 disabled Hidden HwndTempy gART_MainUpdate Center, 0|4|6||8|10|12|20
		ART_hwnd.WeaponBDamageDie:= Tempy
	Gui, ART_Main:Add, Text, x206 y275 w8 h17 disabled Hidden HwndTempy Center, +
		ART_hwnd.WeaponBDamagePlusText:= Tempy
	Gui, ART_Main:Add, Edit, x215 y272 w50 h24 disabled Hidden HwndTempy gART_MainUpdate Center, 
		ART_hwnd.WeaponBDamageBonus:= Tempy
	Gui, ART_Main:Add, ComboBox, x275 y272 w110 disabled Hidden HwndTempy gART_MainUpdate Left, acid|cold|fire|force|lightning|necrotic|poison|psychic|radiant|thunder
		ART_hwnd.WeaponBDamageType:= Tempy

	Gui, ART_Main:Add, Checkbox, x400 y275 w100 h17 Hidden HwndTempy gART_BonusDamage, Add
		ART_hwnd.WeaponBDamageAdd:= Tempy



	Gui, ART_Main:Add, Text, x10 y310 w90 h17 Disabled Hidden HwndTempy Right, Normal Range:
		ART_hwnd.WeaponNormalRangeText:= Tempy
	Gui, ART_Main:Add, Text, x15 y337 w85 h17 Disabled Hidden HwndTempy Right, Long Range:
		ART_hwnd.WeaponLongRangeText:= Tempy
	Gui, ART_Main:Add, Edit, x105 y307 w50 h23 Disabled Hidden HwndTempy gART_MainUpdate Center, 
		ART_hwnd.WeaponNormalRange:= Tempy
	Gui, ART_Main:Add, Edit, x105 y334 w50 h23 Disabled Hidden HwndTempy gART_MainUpdate Center, 
		ART_hwnd.WeaponLongRange:= Tempy

	Gui, ART_Main:Add, Text, x275 y310 w65 h17 Hidden HwndTempy Right, Reroll:
		ART_hwnd.WeaponPrRerollText:= Tempy
	Gui, ART_Main:Add, ComboBox, x345 y307 w40 Hidden HwndTempy gART_MainUpdate Center, 0||1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20
		ART_hwnd.WeaponPrReroll:= Tempy

	Gui, ART_Main:Add, Text, x275 y337 w65 h17 Hidden HwndTempy Right, Crit Range:
		ART_hwnd.WeaponPrCritRangeText:= Tempy
	Gui, ART_Main:Add, ComboBox, x345 y334 w40 Hidden HwndTempy gART_MainUpdate Center, 1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20||
		ART_hwnd.WeaponPrCritRange:= Tempy


	Gui, ART_Main:Add, Text, x505 y225 w100 h17 Hidden HwndTempy Center, Properties:
		ART_hwnd.WeaponPropText:= Tempy
	Gui, ART_Main:Add, Text, x505 y406 w100 h17 Hidden HwndTempy Center, Material:
		ART_hwnd.WeaponMatText:= Tempy
		
	Gui, ART_Main:Add, ListBox, 8 x505 y242 R10 w100 sort Hidden HwndTempy gART_MainUpdate, Ammunition|Finesse|Heavy|Light|Loading|Reach|Special|Thrown|Two-handed|Versatile
		ART_hwnd.WeaponProp:= Tempy
	Gui, ART_Main:Add, ListBox, 8 x505 y423 R3 w100 sort Hidden HwndTempy gART_MainUpdate, Silver|Adamantine|Cold-forged iron
		ART_hwnd.WeaponMat:= Tempy
;}

	Gui, ART_Main:Add, CheckBox, x20 y568 w125 h17 +0x20 Right HwndTempy Checked1 gART_MainUpdate, Lock Item  in FG: 
		ART_Hwnd.locked:= Tempy
	Gui, ART_Main:Add, Button, x485 y560 w120 h25 +border +NoTab HwndTempy gART_Notes, Add Description
		ART_Hwnd.notebutton:= Tempy

;}

;  ================================================
;  |           GUI for the 'image' tab            |
;  ================================================
;{
	Gui, ART_Main:Tab, 2	; All the GUI controls for the 'Image' tab.
	

	Gui, ART_Main:Add, Text, x15 y77 w590 h20 center, Click below to select Item image
	PicWinOptions := ( SS_BITMAP := 0xE ) | ( SS_CENTERIMAGE := 0x200 )
	Gui, ART_Main:Add, Picture, x15 y97 w590 h400 %PicWinOptions% BackgroundTrans Border HwndTempy gART_image,
		ART_Hwnd.jpeg:= Tempy

	Gui, ART_Main:Add, Button, x505 y502 w100 h20 +border gART_ClearImage, Clear Image
	
	Gui, ART_Main:Add, Text, x15 y533 w75 h17 Right, Artist Name:
	Gui, ART_Main:Add, Text, x15 y563 w75 h17 Right, Link:
	Gui, ART_Main:Add, Edit, x95 y530 w400 h23 HwndTempy gART_MainUpdate,
		ART_Hwnd.Artist:= Tempy
	Gui, ART_Main:Add, Edit, x95 y560 w400 h23 HwndTempy gART_MainUpdate,
		ART_Hwnd.ArtistLink:= Tempy
;}

Gui, ART_Main:Tab		; End of tab3 system.


	Gui, ART_Main:Add, ActiveX, x620 y45 w500 h550 E0x200 +0x8000000 vART_VP, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	
	;~ Gui, ART_Main:Add, Button, x8 y605 w115 h30 +border vSpImport gImport_Spell_Text, Import Text
	
	Gui, ART_Main:Add, Text, x328 y610 w80 h17 Right, FG Category:
	Gui, ART_Main:Add, Combobox, x413 y607 w200 HwndTempy gART_MainUpdate, 
		ART_Hwnd.FGcat:= Tempy

	Gui, ART_Main:Add, CheckBox, x622 y601 w200 h18 HwndTempy gART_MainUpdate, Add Title to Description
		ART_Hwnd.AddTitle:= Tempy
	Gui, ART_Main:Add, CheckBox, x622 y619 w200 h18 HwndTempy gART_MainUpdate, Add Image Link to Description
		ART_Hwnd.ImageLink:= Tempy
	
	Gui, ART_Main:Add, Button, x880 y605 w115 h30 HwndTempy +border gSave_Artefact, Save Item
		ART_Hwnd.save:= Tempy
	Gui, ART_Main:Add, Button, x1005 y605 w115 h30 HwndTempy +border gART_Append, Add to Project
		ART_Hwnd.append:= Tempy

	
	Gui, ART_Main:font, S18 c000000, Arial
	Gui, ART_Main:Add, Button, x1125 y545 w24 h24 hwndART_buttonup -Tabstop, % Chr(11165)
	Gui, ART_Main:Add, Button, x1125 y571 w24 h24 hwndART_buttondn -Tabstop, % Chr(11167)

	Gui, ART_Main:font, S9 c000000, Segoe UI
	Gui, ART_Main:Add, StatusBar
	Gui, ART_Main:Default
	SB_SetParts(450, 250)
	SB_SetText(" " WinTProj, 1)
	Gui, ART_Main:font, S10 c000000, Arial
	
}

ART_GUIImport()	 {
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

ART_GUIJSON()	 {
	global
	local tempy
	
; Settings for manage JSON window (ART_JSON)
	Gui, ART_JSON:+OwnerART_Main
	Gui, ART_JSON:-SysMenu
	Gui, ART_JSON:+hwndART_JSON
	Gui, ART_JSON:Color, 0xE2E1E8
	Gui, ART_JSON:font, S10 c000000, Arial
	Gui, ART_JSON:margin, 5, 1

	Artefact_list:= "Choose an item from the JSON file||"
	For a, b in FCT.object()
	{
		Artefact_list:= Artefact_list fct[a].name "|"
	}
	Gui, ART_JSON:Add, ComboBox, x10 y10 w300 gART_JSON_Choose hwndTempy, %Artefact_list%
		ART_Hwnd.ART_JSONChoose:= Tempy
	Gui, ART_JSON:Add, Edit, x10 y40 w300 h60 hwndTempy +ReadOnly, 
		ART_Hwnd.ART_JSONselected:= Tempy
	Gui, ART_JSON:Add, Button, x75 y105 w80 h25 +border gART_JSON_Del hwndTempy, Delete Item
		ART_Hwnd.ART_JSONDeleteButton:= Tempy
	Gui, ART_JSON:Add, Button, x165 y105 w80 h25 +border gART_JSON_Edit hwndTempy, Edit Item
		ART_Hwnd.ART_JSONEditButton:= Tempy
	Gui, ART_JSON:Add, Button, x180 y170 w130 h30 +border gART_JSON_Cancel hwndTempy, Close
		ART_Hwnd.ART_JSONCancelButton:= Tempy
}

ART_CreateToolbar() {
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
		New Artefact
		Open Artefact
		Save Artefact
		-
		Open Previous Artefact
		Open Next Artefact
		-
		Manage Artefacts
		-
		Import Artefact Text
		-
		Manage Project
		-
		Create Module
	)

	Return ToolbarCreate("OnToolbar", Buttons, "ART_Main", ImageList, "Flat List Tooltips Border")
}

ART_MainGuiDropFiles(GuiHwnd, FileArray, CtrlHwnd, X, Y) {
	Global
	if (ART_Hwnd.jpeg = CtrlHwnd) {
		arte.imagepath:= filearray[1]
		Gosub Load_ART_Image
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
#Include Equipment Engineer.ahk

/* ========================================================
 *                  End of Include Files
 * ========================================================
*/

;~ ######################################################
;~ #                    Program End.                    #
;~ ######################################################