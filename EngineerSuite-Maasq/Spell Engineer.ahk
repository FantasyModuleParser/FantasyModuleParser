;~ ######################################################
;~ #                                                    #
;~ #                 ~ Spell Engineer ~                 #
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
SpellEngineer("self")
return

SpellEngineer(what) {
	global
	SplashImageGUI("Spell Engineer.png", "Center", "Center", 300, true)
	wc:= new WinClip
	
	SpLoadPause:= 1
	SPE_Release_Version:= "0.10.03"
	SPE_Release_Date:= "29/09/19"
	SPE_Ref:= what
	
	Gosub SpStartup
	SPE_Initialise()
	
	Sp_PrepareGUI()
	If (LaunchProject AND FileExist(ProjectPath)) {
		flags.project:= 1
		PROE_Ref:= "SPE_Main"
		Gosub Project_Load
	}
	FGcatList(GHwnd.FGcat)
	If (LaunchSpell AND FileExist(SpellPath)) {
		flags.Spell:= 1
		Spell:= ObjLoad(SpellSavePath)
		SpSetVars()
	}

	SpGetVars()
	SPE_RH_Box()
	
	win.spellengineer:= 1
	Gui, SPE_Main:Show, w1150 h665, Spell Engineer
	SPE_Toolbar:= CreateToolBar3()

	SpLoadPause:= 0
	
	Hotkey,IfWinActive,ahk_id %SPE_Main%
	hotkey, ^b, SetFontStyle2
	hotkey, ^i, SetFontStyle2
	hotkey, ^u, SetFontStyle2
	hotkey, ^v, SpPaste
	Hotkey, ^J, De_PDF
	Hotkey, ~LButton, SpVup
	Hotkey, ~Wheeldown, SpVScrDwn
	Hotkey, ~Wheelup, SpVScrUp
	hotkey, esc, EscapeHandle
	hotkey, RButton, RLink

	OnMessage(0x200, "WM_MOUSEMOVE")

	WM_SETCURSOR := 0x0020
	CHAND := DllCall("User32.dll\LoadCursor", "Ptr", NULL, "Int", 32649, "UPtr")
	OnMessage(WM_SETCURSOR, "SetCursor")
}


;~ ######################################################
;~ #                       Labels                       #
;~ ######################################################



SpStartup:					;{
	Critical
	;~ InitialDirCreate()
	;~ CommonInitialise()
	;~ Initialise_Other()
	;~ Load_Options()
	;~ OptionsDirCreate()
	If !FileExist(DataDir "\casters.json") {
		FileCopy, Defaults\defaultcasters.json, %DataDir%\casters.json, 1
	}
	jsonpath:= DataDir "\casters.json"
	castDB:= new JSONfile(jsonpath)
	For key, val in castDB.object()
		CastUser.push(key)
return						;}

SPE_MainGuiClose:			;{
	File:= A_AppData "\NPC Engineer\NPC Engineer.ini"
	Stub:= "Paths"
	IniWrite, %ProjectPath%, %File%, %Stub%, ProjectPath
	IniWrite, %SpellSavePath%, %File%, %Stub%, SpellSavePath
	win.spellengineer:= 0
	If (SPE_Ref = "self") {
		ExitApp
	} Else {
		Gui, SPE_Main:Destroy
		Gui, %SPE_Ref%:show
		StBar(SPE_Ref)
	}
return						;}

SPE_MainUpdate:				;{
	SpGetVars()
	If spell.material
		GuiControl, Enable, % GHwnd.component
	else
		GuiControl, Disable, % GHwnd.component
	SPE_RH_Box()
return						;}

SPE_CTUpdate:				;{
	spell.CTreaction:= Gget(GHwnd.CTreaction)
	spell.CTnumber:= Gget(GHwnd.CTnumber)
	spell.CTtype:= Gget(GHwnd.CTtype)
	If (spell.CTtype = "reaction") {
		GuiControl, Enable, % GHwnd.CTreaction
		GuiControl, Enable, % GHwnd.reacttxt
	} else {
		GuiControl, Disable, % GHwnd.CTreaction
		GuiControl, Disable, % GHwnd.reacttxt
	}
	spalt:= 0
	If spell.CTtype
		spalt:= 1
	If (spell.CTtype = "--")
		spalt:= 0
	If spell.CTnumber
		spalt:= 1
	If spalt {
		spell.castingtime:= spell.CTnumber " " spell.CTtype
		If (spell.CTnumber != 1)
			spell.castingtime .= "s"
		Gset(GHwnd.castingtime, spell.castingtime)
	}
return						;}

SPE_RUpdate:				;{
	spell.Rtype:= Gget(GHwnd.Rtype)
	spell.RDist:= Gget(GHwnd.RDist)
	If (spell.Rtype = "ranged") {
		GuiControl, Enable, % GHwnd.RDist
		GuiControl, Enable, % GHwnd.rdisttxt
	} else {
		GuiControl, Disable, % GHwnd.RDist
		GuiControl, Disable, % GHwnd.rdisttxt
	}
	spalt:= 0
	If spell.Rtype
		spalt:= 1
	If (spell.Rtype = "--")
		spalt:= 0
	If spalt {
		spell.range:= spell.Rtype
		If (spell.Rtype = "ranged")
			spell.range:= spell.RDist " feet"
		Gset(GHwnd.range, spell.range)
	}
return						;}

SPE_DUpdate:				;{
	spell.Dtype:= Gget(GHwnd.Dtype)
	spell.Dnumber:= Gget(GHwnd.Dnumber)
	spell.Dunit:= Gget(GHwnd.Dunit)
	If (spell.Dtype = "concentration") or (spell.Dtype = "time") {
		GuiControl, Enable, % GHwnd.Dnumber
		GuiControl, Enable, % GHwnd.Dnumbertxt
		GuiControl, Enable, % GHwnd.Dunit
		GuiControl, Enable, % GHwnd.Dunittxt
	} else {
		GuiControl, Disable, % GHwnd.Dnumber
		GuiControl, Disable, % GHwnd.Dnumbertxt
		GuiControl, Disable, % GHwnd.Dunit
		GuiControl, Disable, % GHwnd.Dunittxt
	}
	spalt:= 0
	If spell.Dtype
		spalt:= 1
	If (spell.Dtype = "--")
		spalt:= 0
	If spalt {
		If (spell.Dtype = "concentration") {
			spell.duration:= "Concentration, up to " spell.Dnumber " " spell.Dunit
			If (spell.Dnumber != 1)
				spell.duration .= "s"
		}
		If (spell.Dtype = "time") {
			spell.duration:= spell.Dnumber " " spell.Dunit
			If (spell.Dnumber != 1)
				spell.duration .= "s"
		}
		If (spell.Dtype = "instantaneous") {
			spell.duration:= "Instantaneous"
		}
		If (spell.Dtype = "until dispelled") {
			spell.duration:= "Until dispelled"
		}
		If (spell.Dtype = "until dispelled or triggered") {
			spell.duration:= "Until dispelled or triggered"
		}
		Gset(GHwnd.duration, spell.duration)
	}
return						;}

SPE_Spelltext:				;{
	tempsp:= Tokenise(ST1.GetRTF(False))
	StringReplace, tempsp, tempsp, <p></p>, , All
	StringReplace, tempsp, tempsp, `r`n`r`n, `r`n, All
	spell.text:= RegexReplace(tempsp, "^\s+|\s+$" )

	if (SPE_changecheck != spell.text) or (changeform = "1") {
		loco:= ST1.getRTF(False)
		
		If regexmatch(loco, "mi)^At Higher Levels(:|\.) ") {
			caret:= ST1.GetSel()
			loco:= RegExReplace(loco, "mi)^At Higher Levels(:|\.) ", "\b\i At Higher Levels.\i0\b0  ")
			ST1.SetText(loco, ["KEEPUNDO"])
			ST1.SetSel(caret.S, caret.E)
		}
		SPE_RH_Box()
	}
	SPE_changecheck:= spell.text
	changeform:= 0
return						;}

SPE_Project_Manage:			;{
	Critical
	Gui, SPE_Main:+disabled
	ProjectEngineer("SPE_Main")
return						;}

SP_GUIcastersClose:
SP_GUIcastersGuiClose:		;{
	Gui, SPE_main:-disabled
	Gui, SP_GUIcasters:Destroy
	SPE_RH_Box()
return						;}

Manage_SPE_JSON:			;{
	Critical
	If (ProjectLive != 1) {
		MsgBox, 16, No Project, You must load a project *.ini`nto manipulate spells in its JSON file., 3
		gosub, Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			Sp_GUIJSON()
			Gui, SPE_Main:+disabled
			Gui, SPE_JSON:Show, w320 h210, Edit or Delete spells in the JSON file
		} else {
			MsgBox, 16, Engineer Suite Parser only, This function can only be carried out whilst using the Engineer Suite Parser., 3
		}
	}
 return						;}

SPE_JSON_Cancel:
SPE_JSONGuiClose:			;{
	Gui, SPE_Main:-disabled
	Gui, SPE_JSON:Destroy
return						;}

SPE_JSON_Choose:			;{
	JSONtemp:= Gget(GHwnd.SpJSONChoose)
	JSON_Sp_Name:= ""
	For a, b in spl.object()
	{
		if (SPL[a].name == JSONtemp) {
			JSON_Sp_Name:= a
		}
	}
	JSON_This_Text:= SPL[JSON_Sp_Name].Name Chr(10)
	JSON_This_Text:= JSON_This_Text SPL[JSON_Sp_Name].Level " " SPL[JSON_Sp_Name].School
	Gset(GHwnd.SpJSONselected, JSON_This_Text)
 return						;}

SPE_JSON_Del:				;{
	If JSON_Sp_Name {
		SPL.delete(JSON_Sp_Name)
		SPL.save(true)
		spell_list:= "|Choose a spell from the JSON file||"
		For a, b in spl.object()
		{
			spell_list:= spell_list SPL[a].name "|"
		}
		temp:= GHwnd.SpJSONChoose
		GuiControl, , %temp%, %spell_list%
		JSON_Sp_Name:= ""
		Gset(GHwnd.SpJSONselected, JSON_Sp_Name)
		SPE_RH_Box()
	}
return						;}

SPE_JSON_Edit:				;{
	If JSON_Sp_Name {
		Gosub New_Spell
		Edit_Sp_JSON(JSON_Sp_Name)
		Gui, SPE_Main:-disabled
		Gui, SPE_JSON:Destroy
	}
return						;}


New_Spell:					;{
	For a,b in spell {
		spell[a]:= ""
	}
	spell.ritual:= 0
	spell.verbal:= 0
	spell.somatic:= 0
	spell.material:= 0
	spell.FGcat:= Modname
	SpSetVars()
	SpScrollPoint:= 0
	SpScrollEnd:= 0
	SpellSavePath:= ""
return						;}

Open_Spell:					;{
	if SpellModSaveDir {
		SpModSaveDir:= "\" Modname
		TempDest:= SpellPath . SpModSaveDir . "\"
		Ifnotexist, %TempDest% 
			FileCreateDir, %TempDest% 
	} Else {
		SpModSaveDir:= ""
		TempDest:= SpellPath
	}			
	TempWorkingDir:= A_WorkingDir
	FileSelectFile, SelectedFile, 2, %TempDest%, Load Spell, (*.spl)
	if (FileExist(SelectedFile)) {
		Spell:= ObjLoad(SelectedFile)
		If spell.higherlevel {
			spell.rtf:= regexreplace(spell.rtf,"}$", "\b \i At Higher Levels: \i0 \b0 " spell.higherlevel "\par" chr(10) chr(13) "}")
			spell.higherlevel:= ""
		}
		If !spell.FGcat
			spell.FGcat:= Modname

		SpSetVars()
		SpellSavePath:= SelectedFile
	}
	SetWorkingDir %TempWorkingDir%
return						;}

Next_Spell:					;{
	if (Mod_Parser == 1) {
		SPLNameTemp:= Gget(GHwnd.name)
		FlagTemp:= 0
		olda:= ""
		For a, b in spl.object()
		{
			if flagtemp {
				stringreplace SpellSavePath, SpellSavePath, %olda%.spl, %a%.spl
				Spell:= ObjLoad(SpellSavePath)
				If spell.higherlevel {
					spell.rtf:= regexreplace(spell.rtf,"}$", "\b \i At Higher Levels: \i0 \b0 " spell.higherlevel "\par" chr(10) chr(13) "}")
					spell.higherlevel:= ""
				}
				SpSetVars()
				FlagTemp:= ""
				olda:= ""
				SPLNameTemp:= ""
				return
			}
			if (SPL[a].name = SPLNameTemp) {
				FlagTemp:= 1
				olda:= a
			}
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This Spell is not in the current Project., 2
	} else {
		MsgBox, 16, Engineer Suite parser only, This function can only be carried out whilst using Engineer Suite's parser., 3
	}
return						;}

Prev_Spell:					;{
	if (Mod_Parser == 1) {
		SPLNameTemp:= Gget(GHwnd.name)
		FlagTemp:= 0
		olda:= ""
		For a, b in spl.object()
		{
			if (A_Index = 1)
				olda:= a
			if (SPL[a].name = SPLNameTemp) {
				FlagTemp:= 1
			}
			if flagtemp {
				stringreplace SpellSavePath, SpellSavePath, %a%.spl, %olda%.spl
				Spell:= ObjLoad(SpellSavePath)
				If spell.higherlevel {
					spell.rtf:= regexreplace(spell.rtf,"}$", "\b \i At Higher Levels: \i0 \b0 " spell.higherlevel "\par" chr(10) chr(13) "}")
					spell.higherlevel:= ""
				}
				SpSetVars()
				FlagTemp:= ""
				olda:= ""
				SPLNameTemp:= ""
				return
			}
			olda:= a
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This spell is not in the current Project., 2
	} else {
		MsgBox, 16, Engineer Suite Parser only, This function can only be carried out whilst using Engineer Suite's Parser., 3
	}

return						;}

Save_Spell:					;{
	SpGetVars()
	If spell.filename {
		if SpellModSaveDir {
			SpModSaveDir:= "\" Modname
			TempDest:= SpellPath . SpModSaveDir . "\"
			Ifnotexist, %TempDest% 
				FileCreateDir, %TempDest% 
		} Else {
			SpModSaveDir:= ""
		}	
		TempWorkingDir:= A_WorkingDir
		SelectedFile:= SpellPath . SpModSaveDir . "\" spell.filename ".spl"
		If FileExist(SelectedFile)
			FileDelete, %SelectedFile%
		sz:= ObjDump(SelectedFile, Spell)
		SpellSavePath:= SelectedFile
		SetWorkingDir %TempWorkingDir%
		Toast(spell.name " saved successfully.")
	}
return						;}

CasterDelete:				;{
	GUI, SPE_CastAdd:submit, NoHide
	If CastDelList {
		CastDB.delete(CastDelList)
		CastDB.save(true)
		
		For key, value in CastUser {
			if (value = CastDelList) {
				del:= CastUser.removeat(key)
			}
		}
		
		temp:= "|"
		For a, b in CastDB.object()
		{
			temp:= temp a "|"
		}
		GuiControl, SPE_CastAdd:, CastDelList, %temp%
		GuiControl, SPE_CastAdd:, NewCaster, 
		temp:= SubStr(temp, 2)
	}
return						;}

CasterAdd:					;{
	GUI, SPE_CastAdd:submit, NoHide
	
	JSON_act_Name:= ""
	For a, b in CastDB.object()
	{
		if (a == NewCaster) {
			JSON_act_Name:= a
		}
	}
	If !JSON_act_Name {
		Armoury:= {}
		Armoury[NewCaster]:= {}
		Armoury[NewCaster].Name:= Trim(NewCaster)
		CastDB.fill(Armoury)
		CastDB.save(true)
		CastUser.push(NewCaster)
		
		
		temp:= "|"
		For a, b in CastDB.object()
		{
			temp:= temp a "|"
		}
		GuiControl, SPE_CastAdd:, CastDelList, %temp%
		GuiControl, SPE_CastAdd:, NewCaster, 
		temp:= SubStr(temp, 2)
	}
return						;}

SPE_CasterAdd_Close:
SPE_CasterAddGuiClose:		;{
	Gui, SPE_Main:-disabled
	Gui, SPE_CastAdd:Destroy
return						;}



Import_Spell_Text:			;{
	SP_GUI_Import()
	SP_Cap_Text:= ""
	SP_Fix_text:= ""
	SP_Clipimp:= ""
	Spell_Backup()
	GuiControl, SPE_Import:, SP_Cap_text
	SP_Graphical("IMspell", ImScrollPoint)

	guiX:= ImpG.SPLX
	guiY:= ImpG.SPLY
	if (guix = -1) or (guiy = -1) {
		Gui, SPE_Main:+disabled
		Gui, SPE_Import:Show, w990 h550, Text Import
	} else {
		Gui, SPE_Main:+disabled
		Gui, SPE_Import:Show, x%GuiX% y%GuiY% w990 h550, Text Import
	}
	guiX:= ""
	guiY:= ""
return						;}

SPE_Import_Cancel:
SPE_ImportGuiClose:			;{
	If SP_Clipimp
		Clipboard:= SP_Clipimp
	Spell_Restore()
	Spell_InjectVars()
	SPE_RH_Box()
	GetCoords(SPE_Import,"SPL")
	Gui, SPE_Main:-disabled
	Gui, SPE_Import:Destroy
return						;}

SPE_Import_Delete:			;{
	SP_Cap_Text:= ""
	SP_Fix_text:= ""
	If SP_Clipimp
		Clipboard:= SP_Clipimp
	GuiControl, SPE_Import:, SP_Cap_text
	GuiControl, SPE_Import:, SP_Fix_text
	;~ Getvars_Main()
return						;}

SPE_Import_Append:			;{
	GUI, SPE_Import:submit, NoHide
	SP_Cap_text:= SP_Cap_Text . Clipboard
	SP_Clipimp:= Clipboard
	Clipboard:= ""
	SP_Cap_text := RegExReplace(SP_Cap_text, "\R", "`r`n") 
	SP_Cap_text:= RegExReplace(SP_Cap_text,"\s*$","") ; remove trailing newlines
	SP_Cap_text:= SP_Cap_text Chr(13) Chr(10)
	GuiControl, SPE_Import:, SP_Cap_Text, %SP_Cap_Text%
	SP_WorkingString:= SP_Cap_Text
	SPMainLoop(SP_WorkingString)
	GuiControl, SPE_Import:Focus, SP_Cap_text
	Send ^{End}
return						;}

SPE_Import_Return:			;{
	Critical
	If SP_Clipimp
		Clipboard:= SP_Clipimp
	GetCoords(SPE_Import,"SPL")
	Gui, SPE_Main:-disabled
	Gui, SPE_Import:Destroy
	Spell_InjectVars()
	SPE_RH_Box()
	notify:= spell.name " imported successfully."
	Toast(notify)
	If PopCaster
		SP_GUIcasters()
return						;}

SPE_Import_Update_Output:	;{
	GUI, SPE_Import:submit, NoHide
	If SP_Cap_Text {
		SP_WorkingString:= SP_Cap_Text
		StringReplace, SP_WorkingString, SP_WorkingString,`n,`r`n, All
		SPMainLoop(SP_WorkingString)
		GuiControl, SPE_Import:Focus, SP_Cap_text
	}
return						;}



;~ ######################################################
;~ #                   Function List.                   #
;~ ######################################################

SPE_RH_Box() {
	global
	local SPLnumb, SPLNameTemp, FlagTemp
	Critical
	If !SpLoadPause {
		SpScrollEnd:= VPSpell.document.body.scrollHeight - 500
		If (SpScrollEnd < 0) {
			SpScrollEnd:= 0
		}
		SP_Graphical("VPspell", SpScrollPoint)
		Gui, SPE_Main:Default
		WinTNPC:= "Spell: " . spell.name
		SB_SetText(" " WinTNPC, 2)
		If Modname {
			qc:= spl.SetCapacity(0)
			if !qc
				qc:= 0
			SB_SetText(" " Modname " (" qc " items)", 1)
		}
		SPLNameTemp:= Gget(GHwnd.name)
		FlagTemp:= 0
		For a, b in spl.object()
		{
			if (spl[a].name = SPLNameTemp)
				FlagTemp:= 1
		}
		If FlagTemp
			GuiControl, SPE_Main:, SpAppend, Update Project
		else
			GuiControl, SPE_Main:, SpAppend, Add to Project
		Gui, SPE_Main:Show
	}
}	

SPE_Initialise() {
	global
	Spell:= {}
	GHwnd:= {}
	bullets:= []
	SpScrollPoint:= 0
	ImScrollPoint:= 0
	flags:=[]
	flags.project:= 0
	Spell.text:= ""
	SPE_changecheck:= "RunOnceAndBlank"
}

SPMainLoop(RawSpell) {
	global
	Critical
	SpCommonProblems(RawSpell)
	SpGetText(RawSpell)
	
	IMScrollEnd:= IMSpell.document.body.scrollHeight - 500
	If (IMScrollEnd < 0) {
		IMScrollEnd:= 0
	}
	SP_Graphical("IMspell", ImScrollPoint)
}
	
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |               Import Functions               |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

SpCommonProblems(chunk) {
	
}
	
SpGetText(chunk) {
	global spell
	stringreplace chunk, chunk, %A_Tab%, %A_Space%, All
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

		If instr(choppity, "cantrip") {
			Stringreplace choppity, choppity, cantrip, 
			spell.level:= "cantrip"
		}
		StringReplace, choppity, choppity, -level, %A_Space%level, all
		If instr(choppity, " level"){
			Fp:= RegExMatch(choppity, "i)(1st level|2nd level|3rd level|4th level|5th level|6th level|7th level|8th level|9th level)", lvl)
			StringReplace choppity, choppity, %lvl%, 
			spell.level:= lvl
		}
		Fp:= RegExMatch(choppity, "i)(abjuration|conjuration|divination|enchantment|evocation|illusion|necromancy|transmutation)", school)
		;~ StringReplace choppity, choppity, %school%, 
		if !school {
			school:= choppity
		}
		spell.school:= school
	}
	
	;casting time
	xxfound:= RegExMatch(chunk, "iOU)Casting Time:(.*)`r`n", match)
	If xxfound {
		choppity:= match.value(0)
		Stringreplace chunk, chunk, %choppity%, ,
		choppity:= RegExReplace(choppity,"i)Casting Time: ","")
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
	
	;~ ;higher levels
	;~ chunk:= RegExReplace(chunk, "m)^At Higher Levels(:|.)", "XXXAt Higher Levels:" )
	;~ xxfound:= RegExMatch(chunk, "O)XXXAt Higher Levels:(.*)", match)
	;~ If xxfound {
		;~ choppity:= match.value(0)
		;~ Stringreplace chunk, chunk, %choppity%, ,
		;~ choppity:= RegExReplace(choppity,"XXXAt Higher Levels: ","")
		;~ choppity:= RegExReplace(choppity,"\s*$","") ; remove trailing newlines
		;~ choppity:= RegExReplace(choppity,"^\s*","") ; remove leading newlines
		;~ spell.higherlevel:= choppity
	;~ }
	
	;body text
	chunk:= RegExReplace(chunk,"m)\s*$","") ; remove trailing newlines
	chunk:= RegExReplace(chunk,"m)^\s*","") ; remove leading newlines
	chunk:= RegExReplace(chunk,"\r\n","\par" Chr(13) Chr(10))

	spell.text:= chunk
	spell.rtf:= "{\rtf1\ansi\deff0{\fonttbl{\f0\fnil Arial;}}{\colortbl `;\red0\green0\blue0;}\pard\sa80\cf1\f0\fs20 " chunk "}"
}

Spell_backup() {
	Global
	spellBU:= objfullyclone(spell)
	ImScrollPoint:= 0
	spell:= {}
}

Spell_Restore() {
	Global
	spell:= objfullyclone(spellBU)
	spellBU:= ""
}

Spell_InjectVars() {
	global
	Gset(GHwnd.name, spell.name)
	Gset(GHwnd.level, spell.level)
	Gset(GHwnd.school, spell.school)
	Gset(GHwnd.ritual, spell.ritual)
	Gset(GHwnd.castingtime, spell.castingtime)
	Gset(GHwnd.range, spell.range)
	Gset(GHwnd.verbal, spell.verbal)
	Gset(GHwnd.somatic, spell.somatic)
	Gset(GHwnd.material, spell.material)
	Gset(GHwnd.component, spell.component)
	Gset(GHwnd.duration, spell.duration)
	Gset(GHwnd.casters, spell.casters)
	
	ST1.SetText(spell.rtf, ["KEEPUNDO"])
}



;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |            Input/Output Functions            |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

SpAppend() {
	Global
	If !Spell.Name {
		return
	}
	If (ProjectLive != 1) or !IsObject(SPL) {
		MsgBox, 16, No Project, The following must be true to add spells to a project:`n`n * You have created a new project or loaded a project *.ini.`n * You have enabled spells by clicking the checkbox.
		gosub, SPE_Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			SPE_Append()
		} else if (Mod_Parser == 2) {
			SpPar5e_Append()
		} else if (Mod_Parser == 3) {
			SpFG5EP_Append()
		}
		SPE_RH_Box()
	}
}

SpPar5e_Append() {
	;~ global
	;~ GUI, NPCE_Main:submit, NoHide
	;~ path:= ModPath "input\npcs.txt"
	;~ FileAppend, %Save_File%, %path%
	;~ If (NPCTokenPath and NPCjpeg) {
		;~ Ifexist, %NPCTokenPath%
		;~ {
			;~ ThumbDest:= ModPath . "input\tokens\" . NPCjpeg . ".*"
			;~ FileCopy, %NPCTokenPath%, %ThumbDest%, 1
			;~ NPCTokenPath:= ThumbDest
		;~ }
	;~ }
	;~ If (NPCImagePath and NPCjpeg) {
		;~ Ifexist, %NPCImagePath%
		;~ {
			;~ ThumbDest:= ModPath . "input\images\" . NPCjpeg . ".*"
			;~ FileCopy, %NPCImagePath%, %ThumbDest%, 1
			;~ NPCImagePath:= ThumbDest
		;~ }
	;~ }

	;~ notify:= NPCName " appended to" ModName "."
	;~ Toast(notify)
}

SpFG5EP_Append() {

}

SPE_Append() {
	global
	SpJSONFile()
	JSON_Ob_Exist:= ""
	For a, b in SPL.object()
	{
		if (a == spell.filename) {
			JSON_Ob_Exist:= a
		}
	}

	If JSON_Ob_Exist {
		tempname:= SPL[JSON_Ob_Exist].name
		MsgBox, 292, Overwrite Spell, The Spell '%tempname%' already exists in the project's JSON file.`nDo you wish to overwrite it with this data? This is unrecoverable!
		IfMsgBox Yes
		{
			SPL.delete(JSON_Ob_Exist)
			SPL.fill(ObjSPL)
			SPL.save(true)

			notify:= Spell.Name " updated in " ModName "."
			Toast(notify)
		}
	} else {
		SPL.fill(ObjSPL)
		SPL.save(true)

		notify:= Spell.Name " added to " ModName "."
		Toast(notify)
	}
	
}

Edit_Sp_JSON(ed) {
	global
	local xxfound, xxdesc, xxhl
	spell.name:= SPL[ed].name
	spell.level:= SPL[ed].level
	spell.school:= SPL[ed].school
	spell.ritual:= SPL[ed].ritual
	spell.castingtime:= SPL[ed].castingtime
	spell.range:= SPL[ed].range
	
	if Instr(SPL[ed].components, "V") {
		spell.verbal:= 1
	} Else {
		spell.verbal:= 0
	}		
	if Instr(SPL[ed].components, "S") {
		spell.somatic:= 1
	} Else {
		spell.somatic:= 0
	}		
	if Instr(SPL[ed].components, "M") {
		spell.material:= 1
	} Else {
		spell.material:= 0
	}
	if (Instr(SPL[ed].components, "(") and spell.material) {
		xxfound:= RegExMatch(SPL[ed].components, "U)\((.*)\)", xx)
		spell.component:= xx1
	} Else {
		spell.component:= ""
	}		
	
	spell.duration:= SPL[ed].duration
	spell.casters:= RegExReplace(SPL[ed].source, ",+$", "")

	xxdesc:= SPL[ed].description
	xxdesc:= StrReplace(xxdesc, "`t", "")
	xxdesc:= StrReplace(xxdesc, "<description type=""formattedtext"">", "")
	xxdesc:= StrReplace(xxdesc, "</description>", "")
	
	spell.higherlevel:= ""
	xxfound:= RegExMatch(xxdesc, "U)<p><b><i>At Higher Levels\. .*")
	If xxfound {
		xxhl:= SubStr(xxdesc, xxfound)
		xxdesc:= StrReplace(xxdesc, xxhl, "")
		xxhl:= StrReplace(xxhl, "<p><b><i>At Higher Levels. </i></b>", "")
		xxhl:= StrReplace(xxhl, "</p>", "")
		spell.higherlevel:= xxhl
	}
	xxdesc:= RTFise(xxdesc)
	spell.rtf:= "{\rtf1\ansi\deff0{\fonttbl{\f0\fnil Arial;}}{\colortbl `;\red0\green0\blue0;}\pard\sa80\cf1\f0\fs20 " xxdesc "}"
	
	SpSetVars()
	SPE_RH_Box()
}

;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |           General Purpose Functions          |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

SpGetVars() {
	global
	local tempvar
	spell.name:= Gget(GHwnd.name)
	spell.level:= Gget(GHwnd.level)
	spell.school:= Gget(GHwnd.school)
	spell.ritual:= Gget(GHwnd.ritual)
	spell.castingtime:= Gget(GHwnd.castingtime)
	spell.range:= Gget(GHwnd.range)
	spell.verbal:= Gget(GHwnd.verbal)
	spell.somatic:= Gget(GHwnd.somatic)
	spell.material:= Gget(GHwnd.material)
	spell.component:= Gget(GHwnd.component)
	spell.duration:= Gget(GHwnd.duration)
	tempvar:= Gget(GHwnd.casters)
	spell.casters:= RegExReplace(tempvar, ",+$", "")
	spell.FGcat:= Gget(GHwnd.FGcat)
	
	temptext:= Validate(ST1.GetRTF(False))
	tempsp:= Tokenise(temptext)
	tempsp:= compactSpaces(tempsp)
	spell.text:= RegexReplace(tempsp, "^\s+|\s+$" )
	spell.rtf:= ST1.GetRTF(False)
	
	tempvar:= spell.name
	StringLower tempvar, tempvar
	tempvar:= RegExReplace(tempvar, "[^a-zA-Z0-9]", "")

	spell.filename:= tempvar
}

SpSetVars() {
	global
	Gset(GHwnd.name, spell.name)
	Gset(GHwnd.level, spell.level)
	Gset(GHwnd.school, spell.school)
	Gset(GHwnd.ritual, spell.ritual)
	Gset(GHwnd.castingtime, spell.castingtime)
	Gset(GHwnd.range, spell.range)
	Gset(GHwnd.verbal, spell.verbal)
	Gset(GHwnd.somatic, spell.somatic)
	Gset(GHwnd.material, spell.material)
	Gset(GHwnd.component, spell.component)
	Gset(GHwnd.duration, spell.duration)
	Gset(GHwnd.casters, spell.casters)
	Gset(GHwnd.FGcat, spell.FGcat)
	
	ST1.SetText(spell.rtf, ["KEEPUNDO"])
}

SpSetTT() {
	Global
	if !isobject(hTTip){
		hTTip:= []
	}
	hTTip[GHwnd.name]:= "Enter a name for your spell."
	hTTip[GHwnd.level]:= "Select the level for your spell."
	hTTip[GHwnd.school]:= "Select the school of magic that your spell belongs to." Chr(10) "The spell graphic chages to reflect your choice."
	hTTip[GHwnd.ritual]:= "Set this if your spell can be cast as a ritual."
	hTTip[GHwnd.castingtime]:= "How long does it take to cast the spell?" Chr(10) "You can type this, but to ensure you get a recognised format you can use the boxes to the right to build the casting time."
	hTTip[GHwnd.CTnumber]:= "Enter the number for the casting time. No units here!"
	hTTip[GHwnd.CTtype]:= "Select the units here. Reaction spells will have a further box become active."
	hTTip[GHwnd.CTreaction]:= "If the spell is a reaction spell this box will become active. Input the condition(s) for the spell to fire."
	hTTip[GHwnd.range]:= "What is the spell's range?" Chr(10) "You can type this, but to ensure you get a recognised format you can use the boxes to the right to build the range."
	hTTip[GHwnd.rtype]:= "Build the spell range." Chr(10) "Pick the type of range from the list. Only 'ranged' will require a number in the field to the right."
	hTTip[GHwnd.rdist]:= "If the range is set as 'ranged' this box will become active." Chr(10) "Type in a distance (in feet) - just the number, as the unit is automatically added."
	hTTip[GHwnd.verbal]:= "Set if the spell has a verbal component to cast."
	hTTip[GHwnd.somatic]:= "Set if the spell has a somatic component to cast."
	hTTip[GHwnd.material]:= "Set if the spell has a material component to cast."
	hTTip[GHwnd.component]:= "What materials are needed to cast the spell?" Chr(10) "This only becomes active if the 'material' checkbox is selected."
	hTTip[GHwnd.duration]:= "How long does the spell last?" Chr(10) "You can type this, but to ensure you get a recognised format you can use the boxes to the right to build the duration."
	hTTip[GHwnd.dtype]:= "Build the duration." Chr(10) "Select the type of duration appropriate for the spell. 'Concentration' and 'time' will make the two fields below active."
	hTTip[GHwnd.dnumber]:= "Enter the value of the duration of the spell."
	hTTip[GHwnd.dunit]:= "Enter the unit of the duration of the spell."
	hTTip[GHwnd.casters]:= "A list of the casters who can cast this spell." Chr(10) "Click on 'Select' to the left to choose the casters from a list."
	hTTip[GHwnd.casterbutton]:= "Select this to open a new window where you can select the casters." Chr(10) "You can set this window to open automatically after an import in 'options' (F11)."

	hTTip[GHwnd.bbold]:= "Make the selected text bold."
	hTTip[GHwnd.bitalics]:= "Make the selected text italicised."
	hTTip[GHwnd.bunderline]:= "Make the selected text underlined."
	hTTip[GHwnd.bbullets]:= "Format the selected text a bulleted list."

	hTTip[GHwnd.FGcat]:= "Choose the category for your Spell. The default for this is the module name." Chr(10) "Edit the items in this list using 'Options/Manage categories' or CTRL-K."
	hTTip[GHwnd.undo]:= "Undo action."
	hTTip[GHwnd.redo]:= "Redo action."
	hTTip[GHwnd.validateXML]:= "This opens a window to show the XML generated by the richtext box." Chr(10) "This allows you to check that formatting hasn't caused XML errors."
	hTTip[GHwnd.import]:= "Click to import all the values for your Spell from a PDF, text file, or web page." Chr(10) "This will update all values, so ensure you have saved any work you wish to keep before doing this!"
	hTTip[GHwnd.save]:= "Save the Spell to your drive as a *.SPL file." Chr(10) "This can be reloaded for further editing."
	hTTip[GHwnd.append]:= "Add this Spell to a parsing project (or update it if it is already part of the project)." Chr(10) "If you haven't set up a project, you are taken to the project management window."
}

SpCasters() {
	global
	local spclass, sparch1, sparch2, sparch3, sparch4, spcaster
	sparch1:= Gget(GHwnd.castarch1)
	If sparch1
		sparch1 .= "|"
	sparch2:= Gget(GHwnd.castarch2)
	If sparch2
		sparch2 .= "|"
	sparch3:= Gget(GHwnd.castarch3)
	If sparch3
		sparch3 .= "|"
	sparch4:= Gget(GHwnd.castarch4)
	If sparch4
		sparch4 .= "|"
	spclass:=  sparch1 sparch2 sparch3 sparch4
	Sort, spclass, D|
	spcaster:= Gget(GHwnd.castclass)
	If spcaster
		spclass:=  spcaster "|" spclass
	stringreplace, spclass, spclass, |, `,%A_Space%, All
	If (SubStr(spclass, -1) = ", ")
		spclass:= SubStr(spclass, 1, -2)
	If (SubStr(spclass, 0) = ",")
		spclass:= SubStr(spclass, 1, -1)
	spell.casters:= RegExReplace(spclass, ",+$", "")
	Gset(GHwnd.casters, spell.casters)
}

SP_Clear() {
	global CastClass, CastArch1, CastArch2, CastArch3, Castuser, GHwnd
	For key, val in CastClass
		classlist .= val "|"
	For key, val in CastArch1
		Arch1 .= val "|"
	For key, val in CastArch2
		Arch2 .= val "|"
	For key, val in CastArch3
		Arch3 .= val "|"
	For key, val in CastUser
		Arch4 .= val "|"

	Gset(GHwnd.castclass, "|" classlist)
	Gset(GHwnd.castarch1, "|" arch1)
	Gset(GHwnd.castarch2, "|" arch2)
	Gset(GHwnd.castarch3, "|" arch3)
	Gset(GHwnd.castarch4, "|" Arch4)
	
	SpCasters()
}

SpPaste() {
	Clipsave:= Clipboard
	Cliptext = %Clipboard%
	Format_Chunk(Cliptext)
	Common_Problems(Cliptext)
	Clipboard:= cliptext
	;~ GuiControl, Focus, % ST1.HWND
	send ^v
	Clipboard:= Clipsave
}

SpVup() {
	global VPSpell, SPScrollPoint, SPScrollEnd, spbuttonup, spbuttondn
	MouseGetPos,,,,ctrl, 2
	while (ctrl=spbuttonup && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		VPSpell.Document.parentWindow.eval("scrollBy(0, -2);")
		SPScrollPoint -= 2
		If (SPScrollPoint < 0) {
			SPScrollPoint:= 0
		}
	}
	while (ctrl=spbuttondn && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		VPSpell.Document.parentWindow.eval("scrollBy(0, 2);")
		SPScrollPoint += 2
		If (SPScrollPoint > SPScrollEnd) {
			SPScrollPoint:= SPScrollEnd
		}
	}
}

SpVScrUp() {
	global VPSpell, IMSpell, SPScrollPoint, IMScrollPoint, SPE_Main, SPE_Import
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" SPE_Main) {
		VPSpell.Document.parentWindow.eval("scrollBy(0, -50);")
		SPScrollPoint -= 50
		If (SPScrollPoint < 0) {
			SPScrollPoint:= 0
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

SpVScrDwn() {
	global VPSpell, IMSpell, SPScrollPoint, IMScrollPoint, SPScrollEnd, IMScrollEnd, SPE_Main, SPE_Import
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" SPE_Main) {
		VPSpell.Document.parentWindow.eval("scrollBy(0, 50);")
		SPScrollPoint += 50
		If (SPScrollPoint > SPScrollEnd) {
			SPScrollPoint:= SPScrollEnd
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

SpJSONFile() {
	global Spell, ObjSPL
	ObjSPL:= {}
	ObjName:= Spell.filename
	ObjSPL[ObjName]:= {}
	ObjSPL[ObjName].name:= spell.name
	ObjSPL[ObjName].level:= spell.level
	ObjSPL[ObjName].school:= spell.school
	ObjSPL[ObjName].ritual:= spell.ritual
	ObjSPL[ObjName].range:= spell.range
	ObjSPL[ObjName].duration:= spell.duration
	ObjSPL[ObjName].source:= Spell.casters

	Spell_components:= ""
	If spell.verbal
		Spell_components .= "V"
	If spell.somatic
		Spell_components .= "S"
	If spell.material
		Spell_components .= "M"
	stringreplace, Spell_components, Spell_components, VS, V`, S,
	stringreplace, Spell_components, Spell_components, VM, V`, M,
	stringreplace, Spell_components, Spell_components, SM, S`, M,
	If spell.component and spell.material
		Spell_components .= " (" spell.component ")"

	ObjSPL[ObjName].components:= Spell_components

	SpCtim:= spell.castingtime
	If (spell.CTtype = "reaction") and spell.CTreaction {
		SpCtim .= ", " spell.CTreaction
	}

	ObjSPL[ObjName].castingtime:= SpCtim
	ObjSPL[ObjName].FGcat:= spell.FGcat

	SpText:= "`t`t`t`t`t<description type=""formattedtext"">" spell.text

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
		
	ObjSPL[ObjName].description:= LinkXML(TKN)
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                 GUI Functions                |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

SP_Graphical(UI, Scr) {
	Global VPspell, IMspell, Spell
	
	;{ Data for Statblock
	Spell_level_etc:= ""
	If (Spell.level != "Cantrip")
		Spell_level_etc:= Spell.level " " Spell.school
	else
		Spell_level_etc:= TC(Spell.school) " " Spell.level
	If Spell.ritual
		Spell_level_etc .= " (ritual)"
	StringReplace, Spell_level_etc, Spell_level_etc, %A_Space%level, -level, all

	spsch:= spell.school
	if spsch in abjuration,conjuration,divination,enchantment,evocation,illusion,necromancy,transmutation	
		SpPic:= spell.school ".png"
	else
		SpPic:= "spell.png"

	Spell_components:= ""
	If spell.verbal
		Spell_components .= "V"
	If spell.somatic
		Spell_components .= "S"
	If spell.material
		Spell_components .= "M"
	stringreplace, Spell_components, Spell_components, VS, V`, S,
	stringreplace, Spell_components, Spell_components, VM, V`, M,
	stringreplace, Spell_components, Spell_components, SM, S`, M,
	If spell.component and spell.material
		Spell_components .= " (" spell.component ")"

	SpName:= spell.name
	SpCtim:= spell.castingtime
	If (spell.CTtype = "reaction") and spell.CTreaction {
		SpCtim .= ", " spell.CTreaction
	}

	Sprnge:= spell.range
	SpDura:= spell.duration
	SpText:= spell.text
	
	SpText:= LinkHTML(SpText)

	StringReplace, sptext, sptext, \par, </p><p>, all
	
	If spell.casters
		SpCasters:= Chr(13) Chr(10) "<p><b><i>Available for: </i></b>" Spell.casters "</p>"
	;}

	#include HTML_Spell_Engineer.ahk
	HTML_Spell:= css . htmspell
	stringreplace, HTML_Spell, HTML_Spell, \r, <br>, All

	documentz:= %UI%.Document
	documentz.open()
	documentz.write(HTML_Spell)
	documentz.close()
	%UI%.Document.parentWindow.eval("scrollTo(0, " Scr ");")
}


Sp_PrepareGUI() {
	global
	Sp_GUIMain()
	If (SPE_Ref = "self") {
		GUI_Project()
	}
	SpSetTT()
}

Sp_GUIMain() {
	global
	local tempy
	
	Gui, SPE_Main:-MaximizeBox
	Gui, SPE_Main:+hwndSPE_Main
	Gui, SPE_Main:Color, 0xE2E1E8
	Gui, SPE_Main:font, S10 c000000, Arial
	Gui, SPE_Main:+OwnDialogs
	
; Menu system
;{
	Menu SpFileMenu, Add, &New Spell`tCtrl+N, New_Spell
	Menu SpFileMenu, Icon, &New Spell`tCtrl+N, NPC Engineer.dll, 1
	Menu SpFileMenu, Add, &Open Spell`tCtrl+O, Open_Spell
	Menu SpFileMenu, Icon, &Open Spell`tCtrl+O, NPC Engineer.dll, 2
	Menu SpFileMenu, Add, &Save Spell`tCtrl+S, Save_Spell
	Menu SpFileMenu, Icon, &Save Spell`tCtrl+S, NPC Engineer.dll, 3
	Menu SpFileMenu, Add
	;~ Menu SpFileMenu, Add, Save as Text, Save_Txt
	;~ Menu SpFileMenu, Add, Save as XML, Save_XML
	;~ Menu SpFileMenu, Add, Save as HTML, Save_HTML
	;~ Menu SpFileMenu, Add, Save as RTF, Save_RTF
	;~ Menu SpFileMenu, Add
	;~ Menu SpFileMenu, Add, Place BBCode on Clipboard, Copy_BB
	;~ Menu SpFileMenu, Add
	Menu SpFileMenu, Add, E&xit`tESC, SPE_MainGuiClose
	Menu SpFileMenu, Icon, E&xit`tESC, NPC Engineer.dll, 17
	Menu SpellEngineerMenu, Add, File, :SpFileMenu
	
	Menu SpOptionsMenu, Add, &Import Text`tCtrl+I, Import_Spell_Text
	Menu SpOptionsMenu, Icon, &Import Text`tCtrl+I, NPC Engineer.dll, 4
	Menu SpOptionsMenu, Add, &Create Module `tCtrl+P, ParseProject
	Menu SpOptionsMenu, Icon, &Create Module `tCtrl+P, NPC Engineer.dll, 6
	Menu SpOptionsMenu, Add
	Menu SpOptionsMenu, Add, Manage Categories `tCtrl+K, GUI_Categories
	Menu SpOptionsMenu, Icon, Manage Categories `tCtrl+K, NPC Engineer.dll, 25
	Menu SpOptionsMenu, Add, Manage Spell File `tCtrl+M, Manage_SPE_JSON
	Menu SpOptionsMenu, Add, Manage Caster List, SP_GUI_CastAdd
	Menu SpOptionsMenu, Add
	Menu SpOptionsMenu, Add, Settings`tF11, GUI_Options
	Menu SpOptionsMenu, Icon, Settings`tF11, NPC Engineer.dll, 9
	Menu SpellEngineerMenu, Add, Options, :SpOptionsMenu
	
	Component_Menu("SpComponentMenu", "spell")
	Menu SpellEngineerMenu, Add, Engineer Suite, :SpComponentMenu
	
	Explorer_Menu("SpExplorerMenu")
	Menu SpellEngineerMenu, Add, Directories, :SpExplorerMenu
	
	Backup_Menu("SpBackupMenu")
	Menu SpellEngineerMenu, Add, Backup, :SpBackupMenu

	Help_Menu("SpHelpMenu", "Spell Engineer")
	Menu SpellEngineerMenu, Add, Information, :SpHelpMenu
	Gui, SPE_Main:Menu, SpellEngineerMenu
;}

	Gui, SPE_Main:Add, GroupBox, x7 y38 w606 h557, Spell Information

	Gui, SPE_Main:Add, ActiveX, x620 y45 w500 h550 E0x200 +0x8000000 vVPspell, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	
	Gui, SPE_Main:Add, Text, x20 y63 w100 h17 Right, Spell Name:
	Gui, SPE_Main:Add, Text, x20 y90 w100 h17 Right, Spell Level:
	Gui, SPE_Main:Add, Text, x220 y90 w90 h17 Right, Spell School:
	Gui, SPE_Main:Add, Text, x20 y122 w100 h17 Right, Casting Time:
	Gui, SPE_Main:Add, Text, x20 y180 w100 h17 Right, Range:
	Gui, SPE_Main:Add, Text, x20 y213 w100 h17 Right, Components:
	Gui, SPE_Main:Add, Text, x20 y246 w100 h17 Right, Duration:
	Gui, SPE_Main:Add, Text, x20 y300 w100 h17 Right, Cast by:
	Gui, SPE_Main:Add, Text, x20 y345 w100 h17 Right, Information:
	
	Gui, SPE_Main:Add, Text, x310 y122 w70 h17 Right, Time:
	Gui, SPE_Main:Add, Text, x430 y122 w50 h17 Right, Type:
	Gui, SPE_Main:Add, Text, x310 y147 w70 h17 Right disabled HwndTempy, Description:
		GHwnd.reacttxt:= Tempy

	Gui, SPE_Main:Add, Text, x310 y180 w70 h17 Right, Type:
	Gui, SPE_Main:Add, Text, x490 y180 w55 h17 Right disabled HwndTempy, Distance:
		GHwnd.rdisttxt:= Tempy

	Gui, SPE_Main:Add, Text, x310 y246 w70 h17 Right, Type:
	Gui, SPE_Main:Add, Text, x310 y271 w70 h17 Right disabled HwndTempy, Time:
		GHwnd.Dnumbertxt:= Tempy
	Gui, SPE_Main:Add, Text, x445 y271 w30 h17 Right disabled HwndTempy, Unit:
		GHwnd.Dunittxt:= Tempy
		
		
	Gui, SPE_Main:Add, Edit, x125 y60 w300 h23 HwndTempy gSPE_MainUpdate,
		GHwnd.name:= Tempy
	Gui, SPE_Main:Add, Combobox, x125 y86 w90 HwndTempy gSPE_MainUpdate, cantrip|1st level|2nd level|3rd level|4th level|5th level|6th level|7th level|8th level|9th level
		GHwnd.level:= Tempy
	Gui, SPE_Main:Add, Combobox, x315 y86 w110 HwndTempy gSPE_MainUpdate, abjuration|conjuration|divination|enchantment|evocation|illusion|necromancy|transmutation
		GHwnd.school:= Tempy
	Gui, SPE_Main:Add, CheckBox, x430 y90 w89 h17 +0x20 Right HwndTempy Checked0 gSPE_MainUpdate, Ritual:
		GHwnd.ritual:= Tempy
	Gui, SPE_Main:Add, Edit, x125 y119 w180 h23 HwndTempy gSPE_MainUpdate, 
		GHwnd.castingtime:= Tempy
	Gui, SPE_Main:Add, Edit, x385 y119 w55 h23 HwndTempy gSPE_CTUpdate, 
		GHwnd.CTnumber:= Tempy
	Gui, SPE_Main:Add, Combobox, x485 y119 w120 HwndTempy gSPE_CTUpdate, --|action|bonus action|reaction|hour|minute
		GHwnd.CTtype:= Tempy
	Gui, SPE_Main:Add, Edit, x385 y144 w220 h23 HwndTempy gSPE_CTUpdate disabled, 
		GHwnd.CTreaction:= Tempy
	Gui, SPE_Main:Add, Edit, x125 y177 w180 h23 HwndTempy gSPE_MainUpdate,
		GHwnd.range:= Tempy
	Gui, SPE_Main:Add, Combobox, x385 y177 w97 HwndTempy gSPE_RUpdate, --|self|touch|ranged|sight|unlimited
		GHwnd.RType:= Tempy
	Gui, SPE_Main:Add, Edit, x550 y177 w55 h23 HwndTempy gSPE_RUpdate disabled, 
		GHwnd.RDist:= Tempy
	Gui, SPE_Main:Add, CheckBox, x123 y213 w35 h17 +0x20 Right HwndTempy Checked0 gSPE_MainUpdate, V:
		GHwnd.verbal:= Tempy
	Gui, SPE_Main:Add, CheckBox, x173 y213 w35 h17 +0x20 Right HwndTempy Checked0 gSPE_MainUpdate, S:
		GHwnd.somatic:= Tempy
	Gui, SPE_Main:Add, CheckBox, x223 y213 w35 h17 +0x20 Right HwndTempy Checked0 gSPE_MainUpdate, M:
		GHwnd.material:= Tempy
	Gui, SPE_Main:Add, Edit, x273 y210 w200 h23 HwndTempy gSPE_MainUpdate Disabled, 
		GHwnd.component:= Tempy
	Gui, SPE_Main:Add, Edit, x125 y243 w180 h23 HwndTempy gSPE_MainUpdate, 
		GHwnd.duration:= Tempy
	Gui, SPE_Main:Add, Combobox, x385 y243 w180 HwndTempy gSPE_DUpdate, --|concentration|instantaneous|time|until dispelled|until dispelled or triggered
		GHwnd.DType:= Tempy
	Gui, SPE_Main:Add, Edit, x385 y268 w55 h23 HwndTempy gSPE_DUpdate Disabled, 
		GHwnd.Dnumber:= Tempy
	Gui, SPE_Main:Add, Combobox, x480 y268 w85 HwndTempy gSPE_DUpdate Disabled, --|round|minute|hour|day
		GHwnd.Dunit:= Tempy

	Gui, SPE_Main:Add, Button, x125 y298 w60 h25 +border HwndTempy gSP_GUIcasters, Select
		GHwnd.casterbutton:= Tempy
	Gui, SPE_Main:Add, Edit, x190 y298 w415 h40 readonly HwndTempy, 
		GHwnd.casters:= Tempy


	SpInfo:= ""
	SE_Redit:= ""
	ST1:= New RichEdit("SPE_Main", "x125 y345 w480 h210 vSpInfo gSPE_Spelltext", True)
		ST1.wordwrap(true)
		ST1.ShowScrollBar(0, False)
		ST1.SetBkgndColor("White")
		STFont := ST1.GetFont()
		STFont.name:= "Arial"
		STFont.Color:= "Black"
		STFont.Size:= "10"
		ST1.SetFont(STFont)
		ST1.SetOptions(["AUTOWORDSELECTION","AUTOVSCROLL"])
		Spacing:= []
		Spacing.After:= 4
		ST1.SetParaSpacing(Spacing)


	Gui, SPE_Main:Add, Button, x125 y560 w20 h20 HwndTempy vBTSTB gSetFontStyle, B
		GHwnd.bbold:= Tempy
	Gui, SPE_Main:font, S10 c000000 norm italic, Arial
	Gui, SPE_Main:Add, Button, x150 y560 w20 h20 HwndTempy vBTSTI gSetFontStyle, I
		GHwnd.bitalics:= Tempy
	Gui, SPE_Main:font, S10 c000000 norm underline, Arial
	Gui, SPE_Main:Add, Button, x175 y560 w20 h20 HwndTempy vBTSTU gSetFontStyle, U
		GHwnd.bunderline:= Tempy
	Gui, SPE_Main:font, S10 c000000 norm, Arial

	Gui, SPE_Main:font, S10 c000000 norm, Arial
	Gui, SPE_Main:Add, Button, x200 y560 w20 h20 HwndTempy vBTSTL gREbullet, % Chr(8801)
		GHwnd.bbullets:= Tempy
	Gui, SPE_Main:font, S10 c000000 norm, Arial
	
	Gui, SPE_Main:Add, Button, x250 y560 w20 h20 HwndTempy vBTSTZ gundo, % Chr(11148)
		GHwnd.undo:= Tempy
	Gui, SPE_Main:Add, Button, x275 y560 w20 h20 HwndTempy vBTSTY gredo, % Chr(11150)
		GHwnd.redo:= Tempy

	Gui, SPE_Main:Add, Button, x505 y560 w100 h20 HwndTempy +border gValXML, Validate XML
		GHwnd.validateXML:= Tempy
	
	Gui, SPE_Main:Add, Button, x8 y605 w115 h30 HwndTempy +border vSpImport gImport_Spell_Text, Import Text
		GHwnd.import:= Tempy
	
	Gui, SPE_Main:Add, Text, x328 y610 w80 h17 Right, FG Category:
	Gui, SPE_Main:Add, Combobox, x413 y607 w200 HwndTempy gSPE_MainUpdate, 
		GHwnd.FGcat:= Tempy

	
	Gui, SPE_Main:Add, Button, x880 y605 w115 h30 HwndTempy +border vSpSave gSave_Spell, Save Spell
		GHwnd.save:= Tempy
	Gui, SPE_Main:Add, Button, x1005 y605 w115 h30 HwndTempy +border vSpAppend gSpAppend, Add to Project
		GHwnd.append:= Tempy

	Gui, SPE_Main:font, S18 c000000, Arial
	Gui, SPE_Main:Add, Button, x1125 y545 w24 h24 hwndspbuttonup -Tabstop, % Chr(11165)
	Gui, SPE_Main:Add, Button, x1125 y571 w24 h24 hwndspbuttondn -Tabstop, % Chr(11167)

	Gui, SPE_Main:font, S9 c000000, Segoe UI
	Gui, SPE_Main:Add, StatusBar
	Gui, SPE_Main:Default
	SB_SetParts(450, 250)
	SB_SetText(" " WinTProj, 1)
	Gui, SPE_Main:font, S10 c000000, Arial
	
}

SP_GUIcasters() {
	global
	local tempy, classlist, arch1, arch2, arch3, Arch4, clcount, spellcasters, key, val
	
; Settings for class select window (SP_GUIcasters)
	Gui, SP_GUIcasters:+OwnerSPE_main
	Gui, SP_GUIcasters:-resize
	Gui, SP_GUIcasters:+hwndSP_GUIcasters
	Gui, SP_GUIcasters:Color, 0xE2E1E8
	Gui, SP_GUIcasters:font, S10 c000000, Arial
	
	Classlist:= ""
	For key, val in CastClass
		classlist .= val "|"
	For key, val in CastArch1
		Arch1 .= val "|"
	For key, val in CastArch2
		Arch2 .= val "|"
	For key, val in CastArch3
		Arch3 .= val "|"
	For key, val in CastUser
		Arch4 .= val "|"
	
	stringreplace, classlist, classlist, |, |, UseErrorLevel
	clcount:= errorlevel
	stringreplace, arch1, arch1, |, |, UseErrorLevel
	If (errorlevel > clcount)
		clcount:= errorlevel
	stringreplace, arch2, arch2, |, |, UseErrorLevel
	If (errorlevel > clcount)
		clcount:= errorlevel
	stringreplace, arch3, arch3, |, |, UseErrorLevel
	If (errorlevel > clcount)
		clcount:= errorlevel
	stringreplace, arch4, arch4, |, |, UseErrorLevel
	If (errorlevel > clcount)
		clcount:= errorlevel
	If (clcount > 40)
		clcount:= 40
	
	Loop, Parse, % Spell.casters, `,, %A_Space%
	{
		StringReplace, classlist, classlist, %A_Loopfield%|, %A_Loopfield%||, All
		StringReplace, arch1, arch1, %A_Loopfield%|, %A_Loopfield%||, All
		StringReplace, arch2, arch2, %A_Loopfield%|, %A_Loopfield%||, All
		StringReplace, arch3, arch3, %A_Loopfield%|, %A_Loopfield%||, All
		StringReplace, arch4, arch4, %A_Loopfield%|, %A_Loopfield%||, All
	}
	
	Gui, SP_GUIcasters:font, bold, Arial
	Gui, SP_GUIcasters:Add, Text, x5 y10 w170 h20 Center, Character Class
	Gui, SP_GUIcasters:Add, Text, x180 y10 w170 h20 Center, Divine Archetypes
	Gui, SP_GUIcasters:Add, Text, x355 y10 w170 h20 Center, Arcane Archetypes
	Gui, SP_GUIcasters:Add, Text, x530 y10 w170 h20 Center, Other Archetypes
	Gui, SP_GUIcasters:Add, Text, x705 y10 w170 h20 Center, Custom Casters
	Gui, SP_GUIcasters:font, norm, Arial
	
	Gui, SP_GUIcasters:font, S9 c000000, Arial
	
	Gui, SP_GUIcasters:Add, ListBox, 8 x5 y30 R%clcount% w170 sort gspcasters hwndTempy, %classlist%
		GHwnd.castclass:= Tempy
	Gui, SP_GUIcasters:Add, ListBox, 8 x180 y30 R%clcount% w170 sort gspcasters hwndTempy, %arch1%
		GHwnd.castarch1:= Tempy
	Gui, SP_GUIcasters:Add, ListBox, 8 x355 y30 R%clcount% w170 sort gspcasters hwndTempy, %arch2%
		GHwnd.castarch2:= Tempy
	Gui, SP_GUIcasters:Add, ListBox, 8 x530 y30 R%clcount% w170 sort gspcasters hwndTempy, %arch3%
		GHwnd.castarch3:= Tempy
	Gui, SP_GUIcasters:Add, ListBox, 8 x705 y30 R%clcount% w170 sort gspcasters hwndTempy, %arch4%
		GHwnd.castarch4:= Tempy
		
	Gui, SP_GUIcasters:font, S10 c000000, Arial
	
	Gui, SP_GUIcasters:Add, button, x5 w100 NoTab gSP_Clear hwndTempy, Clear All
		GHwnd.clearbutton:= Tempy
	Gui, SP_GUIcasters:Add, button, x775 yp w100 default gSP_GUIcastersClose hwndTempy, Close
		GHwnd.castbutton:= Tempy
	Gui, SPE_main:+disabled
	Gui, SP_GUIcasters:Show, , Select Classes or Archetypes
}

SP_GUI_Import() {
	global
	
; Settings for text import window (SPE_Import)
	Gui, SPE_Import:+OwnerSPE_Main
	Gui, SPE_Import:-SysMenu
	Gui, SPE_Import:+hwndSPE_Import
	Gui, SPE_Import:Color, 0xE2E1E8
	Gui, SPE_Import:font, S10 c000000, Arial
	
	Gui, SPE_Import:Add, Edit, vSP_Cap_Text gSPE_Import_Update_Output x8 y8 w442 h474 Multi, %SP_Cap_Text%
	
	Gui, SPE_Import:Add, ActiveX, x480 y8 w500 h500 E0x200 +0x8000000 vIMspell, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	
	;~ ST2:= New RichEdit("SPE_Import", "x480 y8 w500 h500 -Tabstop vSP_Fix_Text", True)
		;~ ST2.wordwrap(true)
		;~ ST2.ShowScrollBar(0, False)
		;~ REbg:= 0
		;~ ST2.SetBkgndColor("White")
		;~ ST2Font := ST2.GetFont()
		;~ ST2Font.name:= "Arial"
		;~ ST2Font.Color:= "Black"
		;~ ST2Font.Size:= "10"
		;~ ST2.SetFont(ST2Font)

	Gui, SPE_Import:Add, DropDownList, x300 y485 w150 vSP_ImportChoice gSPE_Import_Update_Output, PDF & Plain Text||
	Gui, SPE_Import:Add, Button, x8 y515 w130 h30 +border vSPE_Import_Delete gSPE_Import_Delete, Delete All Text
	Gui, SPE_Import:Add, Button, x150 y515 w130 h30 +border vSPE_Import_Append gSPE_Import_Append, Append Clipboard
	Gui, SPE_Import:Add, Button, x710 y515 w130 h30 +border vSPE_Import_Return gSPE_Import_Return, Import and Return
	Gui, SPE_Import:Add, Button, x852 y515 w130 h30 +border vSPE_Import_Cancel gSPE_Import_Cancel, Cancel All Changes

	Hotkey, IfWinActive, ahk_id %SPE_Import%
	Hotkey, ~Wheeldown, SpVScrDwn
	Hotkey, ~Wheelup, SpVScrUp
	hotkey, esc, EscapeHandle
}

SP_GUIJSON() {
	global
	local tempy
	
; Settings for text import window (SPE_JSON)
	Gui, SPE_JSON:+OwnerSPE_Main
	Gui, SPE_JSON:-SysMenu
	Gui, SPE_JSON:+hwndSPE_JSON
	Gui, SPE_JSON:Color, 0xE2E1E8
	Gui, SPE_JSON:font, S10 c000000, Arial
	Gui, SPE_JSON:margin, 5, 1

	spell_list:= "Choose a spell from the JSON file||"
	For a, b in spl.object()
	{
		spell_list:= spell_list SPL[a].name "|"
	}
	Gui, SPE_JSON:Add, ComboBox, x10 y10 w300 gSPE_JSON_Choose hwndTempy, %spell_list%
		GHwnd.SpJSONChoose:= Tempy
	Gui, SPE_JSON:Add, Edit, x10 y40 w300 h60 hwndTempy +ReadOnly, 
		GHwnd.SpJSONselected:= Tempy
	Gui, SPE_JSON:Add, Button, x75 y105 w80 h25 +border gSPE_JSON_Del hwndTempy, Delete Spell
		GHwnd.SpJSONDeleteButton:= Tempy
	Gui, SPE_JSON:Add, Button, x165 y105 w80 h25 +border gSPE_JSON_Edit hwndTempy, Edit Spell
		GHwnd.SpJSONEditButton:= Tempy
	Gui, SPE_JSON:Add, Button, x180 y170 w130 h30 +border gSPE_JSON_Cancel hwndTempy, Close
		GHwnd.SpJSONCancelButton:= Tempy
}

SP_GUI_CastAdd() {
	global
	
; Settings for Add Caster window (SPE_CastAdd)
	Gui, SPE_CastAdd:+OwnerSPE_Main
	Gui, SPE_CastAdd:-SysMenu
	Gui, SPE_CastAdd:+hwndSPE_CastAdd
	Gui, SPE_CastAdd:Color, 0xE2E1E8
	Gui, SPE_CastAdd:font, S10 c000000, Arial
	Gui, SPE_CastAdd:margin, 5, 1

	Local key, value, Arch
	For key, val in CastUser
		Arch .= val "|"
		
	Gui, SPE_CastAdd:font, S9 c000000, Arial
	Gui, SPE_CastAdd:Add, ListBox, x10 y30 R13 w120 sort vCastDelList, %arch%
	Gui, SPE_CastAdd:font, S10 c000000, Arial
	
	Gui, SPE_CastAdd:Add, Text, x10 y10 w120 h17, Delete Caster:
	Gui, SPE_CastAdd:Add, Text, x150 y10 w173 h17, Add New Caster:
	Gui, SPE_CastAdd:Add, Edit, vNewCaster x150 y30 w200 h23 Left, 
	
	Gui, SPE_CastAdd:Add, Button, x30 y235 w80 h23 +border vCasterDelete gCasterDelete, Delete
	Gui, SPE_CastAdd:Add, Button, x270 y58 w80 h23 +border +default vCasterAdd gCasterAdd, Add

	Gui, SPE_CastAdd:Add, Button, x220 y228 w130 h30 +border vSPE_CasterAdd_Close gSPE_CasterAdd_Close, Close

	Gui, NPCE_Main:+disabled
	Gui, SPE_CastAdd:Show, w360 h268, Add or Delete Casters
}


CreateToolbar3() {
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
		New Spell
		Open Spell
		Save Spell
		-
		Open Previous Spell
		Open Next Spell
		-
		Manage Spells
		-
		Import Spell Text
		-
		Manage Project
		-
		Create Module
	)

	Return ToolbarCreate("OnToolbar", Buttons, "SPE_Main", ImageList, "Flat List Tooltips Border")
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
#Include Equipment Engineer.ahk
#Include Table Engineer.ahk
#Include Artefact Engineer.ahk

/* ========================================================
 *                  End of Include Files
 * ========================================================
*/

;~ ######################################################
;~ #                    Program End.                    #
;~ ######################################################