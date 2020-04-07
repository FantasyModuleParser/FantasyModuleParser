;~ ######################################################
;~ #                                                    #
;~ #                 ~ Table Engineer ~                 #
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
TableEngineer("self")
return

TableEngineer(what) {
	global
	SplashImageGUI("Table Engineer.png", "Center", "Center", 300, true)
	
	;~ TAB_LoadPause:= 1
	TAB_Release_Version:= "0.10.06"
	TAB_Release_Date:= "27/09/19"
	TAB_Ref:= what
	
	Gosub TAB_Startup
	TAB_Initialise()
	
	TAB_PrepareGUI()
	If (LaunchProject AND FileExist(ProjectPath)) {
		flags.project:= 1
		PROE_Ref:= "TAB_Main"
		Gosub Project_Load
	}
	FGcatList(TAB_Hwnd.FGcat)
	If (LaunchTable AND FileExist(TablePath)) {
		flags.Table:= 1
		Table:= ObjLoad(TableSavePath)
		TAB_SetVars()
	}

	gosub TAB_Rollmethod
	TAB_GetVars()
	TAB_RH_Box()
	
	win.TableEngineer:= 1
	Gui, TAB_Main:Show, w1150 h665, Table Engineer
	TAB_Toolbar:= TAB_CreateToolBar()

	;~ TAB_LoadPause:= 0
	
	Hotkey,IfWinActive,ahk_id %TAB_Main%
	hotkey, ^b, SetFontStyle2
	hotkey, ^i, SetFontStyle2
	hotkey, ^u, SetFontStyle2
	hotkey, ^v, TAB_Paste
	Hotkey, ^J, De_PDF
	Hotkey, ~LButton, TAB_Vup
	Hotkey, ~Wheeldown, TAB_VScrDwn
	Hotkey, ~Wheelup, TAB_VScrUp
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



TAB_Startup:				;{
	Critical
	;~ InitialDirCreate()	
	;~ CommonInitialise()
	;~ Initialise_Other()
	;~ Load_Options()
	;~ OptionsDirCreate()
return						;}

TAB_MainGuiClose:			;{
	File:= A_AppData "\NPC Engineer\NPC Engineer.ini"
	Stub:= "Paths"
	IniWrite, %ProjectPath%, %File%, %Stub%, ProjectPath
	IniWrite, %TableSavePath%, %File%, %Stub%, TableSavePath
	win.TableEngineer:= 0
	If (TAB_Ref = "self") {
		ExitApp
	} Else {
		Gui, TAB_Main:Destroy
		Gui, %TAB_Ref%:show
		StBar(TAB_Ref)
	}
return						;}

TAB_MainUpdate:				;{
	temprows:= table.rows
	tempcols:= table.columns
	TAB_GetVars()
	if (table.rows < temprows) {
		Loop, % table.columns + 2 {
			table.arr[temprows, A_Index]:= ""
		}
	}
	if (table.columns < tempcols) {
		Loop, % table.rows {
			table.arr[A_Index, tempcols + 2]:= ""
		}
		table.col[tempcols]:= ""
	}
	TAB_RTF()
	tempvar:= Tokenise(TT1.GetRTF(False))
	StringReplace, tempvar, tempvar, <p></p>, , All
	StringReplace, tempvar, tempvar, `r`n`r`n, `r`n, All
	table.text:= RegexReplace(tempvar, "^\s+|\s+$" )

	TAB_Parsetable(table.text)
	TAB_RH_Box()
	temprows:= ""
	tempcols:= ""
return						;}

TAB_Project_Manage:			;{
	Critical
	Gui, TAB_Main:+disabled
	ProjectEngineer("TAB_Main")
return						;}

Manage_TAB_JSON:			;{
	Critical
	If (ProjectLive != 1) {
		MsgBox, 16, No Project, You must load a project *.ini`nto manipulate equipment in its JSON file., 3
		gosub, Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			TAB_GUIJSON()
			Gui, TAB_Main:+disabled
			Gui, TAB_JSON:Show, w320 h210, Edit or Delete tables in the JSON file
		} else {
			MsgBox, 16, Engineer Suite Parser only, This function can only be carried out whilst using the Engineer Suite Parser., 3
		}
	}
 return						;}

TAB_JSON_Cancel:
TAB_JSONGuiClose:			;{
	Gui, TAB_Main:-disabled
	Gui, TAB_JSON:Destroy
return						;}

TAB_JSON_Choose:			;{
	JSONtemp:= Gget(TAB_Hwnd.TAB_JSONChoose)
	JSON_Sp_Name:= ""
	For a, b in tbl.object()
	{
		if (TBL[a].name == JSONtemp) {
			JSON_Sp_Name:= a
		}
	}
	JSON_This_Text:= TBL[JSON_Sp_Name].Name Chr(10)
	JSON_This_Text:= JSON_This_Text TBL[JSON_Sp_Name].description
	Gset(TAB_Hwnd.TAB_JSONselected, JSON_This_Text)
 return						;}

TAB_JSON_Del:				;{
	If JSON_Sp_Name {
		TBL.delete(JSON_Sp_Name)
		TBL.save(true)
		table_list:= "|Choose a table from the JSON file||"
		For a, b in tbl.object()
		{
			table_list:= table_list TBL[a].name "|"
		}
		temp:= TAB_Hwnd.TAB_JSONChoose
		GuiControl, , %temp%, %table_list%
		JSON_Sp_Name:= ""
		Gset(TAB_Hwnd.TAB_JSONselected, JSON_Sp_Name)
		TAB_RH_Box()
	}
return						;}

TAB_JSON_Edit:				;{
	;~ If JSON_Sp_Name {
		;~ Edit_Sp_JSON(JSON_Sp_Name)
		;~ Gui, TAB_Main:-disabled
		;~ Gui, TAB_JSON:Destroy
	;~ }
return						;}


New_Table:					;{
	Table:= {}
	table.min:= 1
	table.max:= 1
	table.dicemodifier:= 0
	table.output:= "chat"
	table.rows:= 2
	table.columns:= 1
	table.d4:= 0
	table.d6:= 0
	table.d8:= 0
	table.d10:= 0
	table.d12:= 0
	table.d20:= 0
	table.rolltype:= "Based on table values"
	table.locked:= 1
	table.showroll:= 0
	table.FGcat:= Modname
	TAB_SetVars()
	TAB_ScrollPoint:= 0
	TAB_ScrollEnd:= 0
return						;}

Open_Table:					;{
	if TableModSaveDir {
		SpModSaveDir:= "\" Modname
		TempDest:= TablePath . SpModSaveDir . "\"
		Ifnotexist, %TempDest% 
			FileCreateDir, %TempDest% 
	} Else {
		SpModSaveDir:= ""
		TempDest:= TablePath
	}			
	TempWorkingDir:= A_WorkingDir
	FileSelectFile, SelectedFile, 2, %TempDest%, Load Table, (*.tbl)
	if (FileExist(SelectedFile)) {
		Table:= ObjLoad(SelectedFile)
		If !table.showroll
			table.showroll:= 0
		If !table.FGcat
			table.FGcat:= Modname
		TAB_SetVars()
		Gosub TAB_Rollmethod
		TableSavePath:= SelectedFile
	}
	SetWorkingDir %TempWorkingDir%
return						;}

Save_Table:					;{
	TAB_GetVars()
	
	tempvar:= Tokenise(TT1.GetRTF(False))
	StringReplace, tempvar, tempvar, <p></p>, , All
	StringReplace, tempvar, tempvar, `r`n`r`n, `r`n, All
	table.text:= RegexReplace(tempvar, "^\s+|\s+$" )
	TAB_Parsetable(table.text)
	
	If table.filename {
		if TableModSaveDir {
			SpModSaveDir:= "\" Modname
			TempDest:= TablePath . SpModSaveDir . "\"
			Ifnotexist, %TempDest% 
				FileCreateDir, %TempDest% 
		} Else {
			SpModSaveDir:= ""
		}
		TempWorkingDir:= A_WorkingDir
		SelectedFile:= TablePath . SpModSaveDir . "\" table.filename ".tbl"
		If FileExist(SelectedFile)
			FileDelete, %SelectedFile%
		sz:= ObjDump(SelectedFile, Table)
		SetWorkingDir %TempWorkingDir%
		Toast(table.name " saved successfully.")
	}
return						;}

Next_Table:					;{
	if (Mod_Parser == 1) {
		TBLNameTemp:= Gget(TAB_Hwnd.name)
		FlagTemp:= 0
		olda:= ""
		For a, b in tbl.object()
		{
			if flagtemp {
				stringreplace TableSavePath, TableSavePath, %olda%.tbl, %a%.tbl
				Table:= ObjLoad(TableSavePath)
				TAB_SetVars()
				FlagTemp:= ""
				olda:= ""
				TBLNameTemp:= ""
				return
			}
			if (TBL[a].name = TBLNameTemp) {
				FlagTemp:= 1
				olda:= a
			}
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This Table is not in the current Project., 2
	} else {
		MsgBox, 16, Engineer Suite parser only, This function can only be carried out whilst using Engineer Suite's parser., 3
	}
return						;}

Prev_Table:					;{
	if (Mod_Parser == 1) {
		TBLNameTemp:= Gget(TAB_Hwnd.name)
		FlagTemp:= 0
		olda:= ""
		For a, b in tbl.object()
		{
			if (A_Index = 1)
				olda:= a
			if (TBL[a].name = TBLNameTemp) {
				FlagTemp:= 1
			}
			if flagtemp {
				stringreplace TableSavePath, TableSavePath, %a%.tbl, %olda%.tbl
				Table:= ObjLoad(TableSavePath)
				TAB_SetVars()
				FlagTemp:= ""
				olda:= ""
				TBLNameTemp:= ""
				return
			}
			olda:= a
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This Table is not in the current Project., 2
	} else {
		MsgBox, 16, Engineer Suite Parser only, This function can only be carried out whilst using Engineer Suite's Parser., 3
	}
return						;}

TAB_TextBox:				;{
	tempsp:= Tokenise(TT1.GetRTF(False))
	StringReplace, tempsp, tempsp, <p></p>, , All
	StringReplace, tempsp, tempsp, `r`n`r`n, `r`n, All
	table.text:= RegexReplace(tempsp, "^\s+|\s+$" )
	if (TAB_changecheck != table.text) or (changeform = "1") {
		TAB_Parsetable(table.text)
		TAB_val()
		TAB_RH_Box()
	}
	TAB_changecheck:= table.text
	changeform:= 0
return						;}

Import_Table_Text:			;{
	TempWorkingDir:= A_WorkingDir
	FileSelectFile, SelectedFile, 2, %A_MyDocuments%, Load CSV file, (*.csv)
	if (FileExist(SelectedFile)) {
		Fileread TableCSV, %Selectedfile%
		TAB_ImportCSV(TableCSV)
	}
	TableCSV:= ""
	SetWorkingDir %TempWorkingDir%
return						;}

TAB_Rollmethod:				;{
	table.rolltype:= Gget(TAB_Hwnd.rolltype)
	If (table.rolltype = "Custom dice roll") {
		GuiControl, Show, % TAB_Hwnd.d4
		GuiControl, Show, % TAB_Hwnd.d6
		GuiControl, Show, % TAB_Hwnd.d8
		GuiControl, Show, % TAB_Hwnd.d10
		GuiControl, Show, % TAB_Hwnd.d12
		GuiControl, Show, % TAB_Hwnd.d20
		GuiControl, Show, % TAB_Hwnd.d4edit
		GuiControl, Show, % TAB_Hwnd.d6edit
		GuiControl, Show, % TAB_Hwnd.d8edit
		GuiControl, Show, % TAB_Hwnd.d10edit
		GuiControl, Show, % TAB_Hwnd.d12edit
		GuiControl, Show, % TAB_Hwnd.d20edit
		GuiControl, Show, % TAB_Hwnd.d4txt
		GuiControl, Show, % TAB_Hwnd.d6txt
		GuiControl, Show, % TAB_Hwnd.d8txt
		GuiControl, Show, % TAB_Hwnd.d10txt
		GuiControl, Show, % TAB_Hwnd.d12txt
		GuiControl, Show, % TAB_Hwnd.d20txt
		GuiControl, Show, % TAB_Hwnd.dicemodifier
		GuiControl, Show, % TAB_Hwnd.dicemodifiertxt
		GuiControl, Show, % TAB_Hwnd.dicemodifieredit
		
		GuiControl, Hide, % TAB_Hwnd.min
		GuiControl, Hide, % TAB_Hwnd.max
		GuiControl, Hide, % TAB_Hwnd.mintxt
		GuiControl, Hide, % TAB_Hwnd.maxtxt
		GuiControl, Hide, % TAB_Hwnd.minedit
		GuiControl, Hide, % TAB_Hwnd.maxedit
	}
	If (table.rolltype = "Preset range") {
		GuiControl, Show, % TAB_Hwnd.min
		GuiControl, Show, % TAB_Hwnd.max
		GuiControl, Show, % TAB_Hwnd.mintxt
		GuiControl, Show, % TAB_Hwnd.maxtxt
		GuiControl, Show, % TAB_Hwnd.minedit
		GuiControl, Show, % TAB_Hwnd.maxedit

		GuiControl, Hide, % TAB_Hwnd.d4
		GuiControl, Hide, % TAB_Hwnd.d6
		GuiControl, Hide, % TAB_Hwnd.d8
		GuiControl, Hide, % TAB_Hwnd.d10
		GuiControl, Hide, % TAB_Hwnd.d12
		GuiControl, Hide, % TAB_Hwnd.d20
		GuiControl, Hide, % TAB_Hwnd.d4edit
		GuiControl, Hide, % TAB_Hwnd.d6edit
		GuiControl, Hide, % TAB_Hwnd.d8edit
		GuiControl, Hide, % TAB_Hwnd.d10edit
		GuiControl, Hide, % TAB_Hwnd.d12edit
		GuiControl, Hide, % TAB_Hwnd.d20edit
		GuiControl, Hide, % TAB_Hwnd.d4txt
		GuiControl, Hide, % TAB_Hwnd.d6txt
		GuiControl, Hide, % TAB_Hwnd.d8txt
		GuiControl, Hide, % TAB_Hwnd.d10txt
		GuiControl, Hide, % TAB_Hwnd.d12txt
		GuiControl, Hide, % TAB_Hwnd.d20txt
		GuiControl, Hide, % TAB_Hwnd.dicemodifier
		GuiControl, Hide, % TAB_Hwnd.dicemodifiertxt
		GuiControl, Hide, % TAB_Hwnd.dicemodifieredit
	}
	If (table.rolltype = "Based on table values") {
		GuiControl, Hide, % TAB_Hwnd.min
		GuiControl, Hide, % TAB_Hwnd.max
		GuiControl, Hide, % TAB_Hwnd.mintxt
		GuiControl, Hide, % TAB_Hwnd.maxtxt
		GuiControl, Hide, % TAB_Hwnd.minedit
		GuiControl, Hide, % TAB_Hwnd.maxedit

		GuiControl, Hide, % TAB_Hwnd.d4
		GuiControl, Hide, % TAB_Hwnd.d6
		GuiControl, Hide, % TAB_Hwnd.d8
		GuiControl, Hide, % TAB_Hwnd.d10
		GuiControl, Hide, % TAB_Hwnd.d12
		GuiControl, Hide, % TAB_Hwnd.d20
		GuiControl, Hide, % TAB_Hwnd.d4edit
		GuiControl, Hide, % TAB_Hwnd.d6edit
		GuiControl, Hide, % TAB_Hwnd.d8edit
		GuiControl, Hide, % TAB_Hwnd.d10edit
		GuiControl, Hide, % TAB_Hwnd.d12edit
		GuiControl, Hide, % TAB_Hwnd.d20edit
		GuiControl, Hide, % TAB_Hwnd.d4txt
		GuiControl, Hide, % TAB_Hwnd.d6txt
		GuiControl, Hide, % TAB_Hwnd.d8txt
		GuiControl, Hide, % TAB_Hwnd.d10txt
		GuiControl, Hide, % TAB_Hwnd.d12txt
		GuiControl, Hide, % TAB_Hwnd.d20txt
		GuiControl, Hide, % TAB_Hwnd.dicemodifier
		GuiControl, Hide, % TAB_Hwnd.dicemodifiertxt
		GuiControl, Hide, % TAB_Hwnd.dicemodifieredit
	}
	TAB_RH_Box()
return						;}




;~ ######################################################
;~ #                   Function List.                   #
;~ ######################################################

TAB_RH_Box() {
	global
	local NameTemp, FlagTemp, qc
	Critical
	If !TAB_LoadPause {
		TAB_ScrollEnd:= VPTable.document.body.scrollHeight - 500
		If (TAB_ScrollEnd < 0) {
			TAB_ScrollEnd:= 0
		}
		TAB_Graphical("VPTable", TAB_ScrollPoint)
		Gui, TAB_Main:Default
		WinTNPC:= "Table: " . table.name
		SB_SetText(" " WinTNPC, 2)
		If Modname {
			qc:= tbl.SetCapacity(0)
			if !qc
				qc:= 0
			SB_SetText(" " Modname " (" qc " items)", 1)
		}
		NameTemp:= Gget(TAB_Hwnd.name)
		FlagTemp:= 0
		For a, b in tbl.object()
		{
			if (tbl[a].name = NameTemp)
				FlagTemp:= 1
		}
		If FlagTemp
			GuiControl, TAB_Main:, TbAppend, Update Project
		else
			GuiControl, TAB_Main:, TbAppend, Add to Project
		Gui, TAB_Main:Show
	}
}	

TAB_Initialise() {
	global
	Table:= {}
	TAB_Hwnd:= {}
	bullets:= []
	TAB_ScrollPoint:= 0
	ImScrollPoint:= 0
	flags:=[]
	flags.project:= 0
	Gosub New_Table
}

TAB_MainLoop(RawSpell) {
	;~ global
	;~ Critical
	;~ TAB_CommonProblems(RawSpell)
	;~ TAB_GetText(RawSpell)
	
	IMScrollEnd:= IMtable.document.body.scrollHeight - 500
	If (IMScrollEnd < 0) {
		IMScrollEnd:= 0
	}
	SP_Graphical("IMspell", ImScrollPoint)
}
	
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |               Import Functions               |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	
TAB_backup() {
	Global
	spellBU:= objfullyclone(spell)
	ImScrollPoint:= 0
	spell:= {}
}

TAB_Restore() {
	Global
	spell:= objfullyclone(spellBU)
	spellBU:= ""
}

TAB_ImportCSV(csv) {
	Global Table, RTFCaretPos, TAB_Hwnd
	pst:= []
	pst.rows:= 0
	pst.cols:= 0
	newContent := ""
	csv:= csv "`r`n"
	stringreplace csv, csv, `r`n`r`n, `r`n, all
	Loop, parse, csv, `n, `r
	{
		if RegExMatch(A_LoopField, "[^,]+", match) ; match anything but comma
			newContent .= A_LoopField "`n"
		else
			continue
	}
	csv:= newcontent
	Loop, parse, csv, `n, `r
	{
		j:= A_Index
		csvfield:= RegExReplace(A_Loopfield, "^,+", "")
		Loop, Parse, csvfield, CSV
		{
			k:= A_Index
			pst[j, k]:= A_LoopField
			if (A_Index > pst.cols) {
				pst.cols:= A_Index
			}
		}
		if (A_Index > pst.rows) {
			pst.rows:= A_Index - 1
		}
	}
	if (pst.rows = 0) {
		pst.rows:= 1
	}
	cr:= 1
	cc:= 1
	Loop % table.rows {
		table.arr[A_Index]:= []
	}
	table.col:= []
	table.rows:= pst.rows + cr - 2 
	Gset(TAB_Hwnd.rows, table.rows)
	table.columns:= pst.cols + cc - 3
	Gset(TAB_Hwnd.columns, table.columns)
	if (cr = 1) AND (cc < 3) {
		cj:= cr - 1
		ck:= cc - 2
		loop % pst.cols {
			dk:= A_Index
			If (dk + ck + 1 > 2) {
				table.col[dk + ck - 1]:= pst[1][dk]
			}
		}
		loop % pst.rows - 1 {
			dj:= A_Index + 1
			loop % pst.cols {
				dk:= A_Index
				element:= pst[dj][dk]
				colno:= dk + ck + 1
				If (colno < 3) {
					If element is number
					{
						table.arr[dj + cj - 1, dk + ck + 1]:= element
					} Else If element is space
					{
						table.arr[dj + cj - 1, dk + ck + 1]:= element
					} Else {
						
					}
				} Else {
					table.arr[dj + cj - 1, dk + ck + 1]:= element
				}
			}
		}
		TAB_RTF()
		TAB_RH_Box()
	} Else If (cr = 1) AND (cc > 2){
		cj:= cr - 1
		ck:= cc - 2
		loop % pst.cols {
			dk:= A_Index
			table.col[dk + ck - 1]:= pst[1][dk]
		}
		loop % pst.rows - 1 {
			dj:= A_Index + 1
			loop % pst.cols {
				dk:= A_Index
				table.arr[dj + cj - 1, dk + ck + 1]:= pst[dj][dk]
			}
		}
		TAB_RTF()
		TAB_RH_Box()
	} Else If (cr > 1) AND (cc < 3){
		cj:= cr - 1
		ck:= cc - 2
		loop % pst.rows {
			dj:= A_Index
			loop % pst.cols {
				dk:= A_Index
				element:= pst[dj][dk]
				colno:= dk + ck + 1
				If (colno < 3) {
					If element is number
					{
						table.arr[dj + cj - 1, dk + ck + 1]:= element
					} Else If element is space
					{
						table.arr[dj + cj - 1, dk + ck + 1]:= element
					} Else {
						
					}
				} Else {
					table.arr[dj + cj - 1, dk + ck + 1]:= element
				}
			}
		}
		TAB_RTF()
		TAB_RH_Box()
	} Else {
		cj:= cr - 1
		ck:= cc - 2
		loop % pst.rows {
			dj:= A_Index
			loop % pst.cols {
				dk:= A_Index
				table.arr[dj + cj - 1, dk + ck + 1]:= pst[dj][dk]
			}
		}
		TAB_RTF()
		TAB_RH_Box()
	}
}

;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |            Input/Output Functions            |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

TAB_Append() {
	Global
	If !table.Name {
		return
	}
	If (ProjectLive != 1) or !IsObject(TBL) {
		MsgBox, 16, No Project, The following must be true to add tables to a project:`n`n * You have created a new project or loaded a project *.ini.`n * You have enabled tables by clicking the checkbox.
		gosub, TAB_Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			TAB_TbAppend()
		} else if (Mod_Parser == 2) {
			TAB_Par5e_Append()
		}
		TAB_RH_Box()
	}
}

TAB_TbAppend() {
	global
	Local JSON_Ob_Exist
	TAB_JSONFile()
	JSON_Ob_Exist:= ""
	For a, b in TBL.object()
	{
		if (a == table.filename) {
			JSON_Ob_Exist:= a
		}
	}

	If JSON_Ob_Exist {
		tempname:= TBL[JSON_Ob_Exist].name
		MsgBox, 292, Overwrite Table, The Table '%tempname%' already exists in the project's JSON file.`nDo you wish to overwrite it with this data? This is unrecoverable!
		IfMsgBox Yes
		{
			TBL.delete(JSON_Ob_Exist)
			TBL.fill(ObjTAB)
			TBL.save(true)

			notify:= table.Name " updated in " ModName "."
			Toast(notify)
			TAB_RH_Box()
		}
	} else {
		TBL.fill(ObjTAB)
		TBL.save(true)

		notify:= table.Name " added to " ModName "."
		Toast(notify)
	}
}

TAB_Par5e_Append() {
	
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |           General Purpose Functions          |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

TAB_GetVars() {
	global
	local tempvar
	table.name:= Gget(TAB_Hwnd.name)
	table.description:= Gget(TAB_Hwnd.description)
	table.dicemodifier:= Gget(TAB_Hwnd.dicemodifier)
	table.output:= Gget(TAB_Hwnd.output)
	table.rows:= Gget(TAB_Hwnd.rows)
	table.columns:= Gget(TAB_Hwnd.columns)
	table.d4:= Gget(TAB_Hwnd.d4)
	table.d6:= Gget(TAB_Hwnd.d6)
	table.d8:= Gget(TAB_Hwnd.d8)
	table.d10:= Gget(TAB_Hwnd.d10)
	table.d12:= Gget(TAB_Hwnd.d12)
	table.d20:= Gget(TAB_Hwnd.d20)
	table.rolltype:= Gget(TAB_Hwnd.rolltype)
	table.min:= Gget(TAB_Hwnd.min)
	table.max:= Gget(TAB_Hwnd.max)
	table.locked:= Gget(TAB_Hwnd.locked)
	table.showroll:= Gget(TAB_Hwnd.showroll)
	table.FGcat:= Gget(TAB_Hwnd.FGcat)

	tempvar:= table.name
	StringLower tempvar, tempvar
	tempvar:= RegExReplace(tempvar, "[^a-zA-Z0-9]", "")

	table.filename:= tempvar
}

TAB_SetVars() {
	global
	Gset(TAB_Hwnd.name, table.name)
	Gset(TAB_Hwnd.description, table.description)
	Gset(TAB_Hwnd.dicemodifier, table.dicemodifier)
	Gset(TAB_Hwnd.output, table.output)
	Gset(TAB_Hwnd.rows, table.rows)
	Gset(TAB_Hwnd.columns, table.columns)
	Gset(TAB_Hwnd.d4, table.d4)
	Gset(TAB_Hwnd.d6, table.d6)
	Gset(TAB_Hwnd.d8, table.d8)
	Gset(TAB_Hwnd.d10, table.d10)
	Gset(TAB_Hwnd.d12, table.d12)
	Gset(TAB_Hwnd.d20, table.d20)
	Gset(TAB_Hwnd.rolltype, table.rolltype)
	Gset(TAB_Hwnd.min, table.min)
	Gset(TAB_Hwnd.max, table.max)
	Gset(TAB_Hwnd.locked, table.locked)
	Gset(TAB_Hwnd.showroll, table.showroll)
	Gset(TAB_Hwnd.FGcat, table.FGcat)
	TAB_RTF()
}

TAB_SetTT() {
	Global
	if !isobject(hTTip){
		hTTip:= []
	}
	hTTip[TAB_Hwnd.name]:= "Enter a name for your table."
	hTTip[TAB_Hwnd.description]:= "Enter a short description for your table."
	hTTip[TAB_Hwnd.dicemodifier]:= ""
	hTTip[TAB_Hwnd.output]:= "This determines where the result of the roll on your table will be sent to."
	hTTip[TAB_Hwnd.rows]:= "The number of rows in the table below. This doesn't include the header row."
	hTTip[TAB_Hwnd.columns]:= "The number of results columns in the table below. This doesn't include the two columns 'From' and 'To'."
	hTTip[TAB_Hwnd.d4]:= "How many d4 should be rolled for this table?"
	hTTip[TAB_Hwnd.d6]:= "How many d6 should be rolled for this table?"
	hTTip[TAB_Hwnd.d8]:= "How many d8 should be rolled for this table?"
	hTTip[TAB_Hwnd.d10]:= "How many d10 should be rolled for this table?"
	hTTip[TAB_Hwnd.d12]:= "How many d12 should be rolled for this table?"
	hTTip[TAB_Hwnd.d20]:= "How many d20 should be rolled for this table?"
	hTTip[TAB_Hwnd.rolltype]:= "This setting determines how the range is worked out for your table. The GUI will change depending on what you choose." Chr(10) "'Based on table values' sets the range and offset based on the minimum and maximum values in the first two columns." Chr(10) "'Preset range' allows you to set a fixed minimum and maximum." Chr(10) "'Custom dice roll' allows you to select how many of each die to roll."
	hTTip[TAB_Hwnd.min]:= "The minimum value for the roll (and table)."
	hTTip[TAB_Hwnd.max]:= "The maximum value for the roll (and table)."
	hTTip[TAB_Hwnd.locked]:= "Should your table appear as locked in FG? This will usually be yes (ticked)." Chr(10) "Setting this to '0' will open the table in edit mode in FG."
	hTTip[TAB_Hwnd.showroll]:= "Set this to '1' to allow all table rolls to show in the FG chat window." Chr(10) "This overrides the FG Option."
	hTTip[TAB_Hwnd.ftd]:= "Clicking here will convert any automatically rollable dice rolls ([1d6]) back to pure text (1d6)."
	hTTip[TAB_Hwnd.frd]:= "Clicking here will convert all dice rolls (1d6) to the proper format for FG to see them as automatically rollable (ie [1d6])."

	hTTip[TAB_Hwnd.FGcat]:= "Choose the category for your Table. The default for this is the module name." Chr(10) "Edit the items in this list using 'Options/Manage categories' or CTRL-K."
	hTTip[TAB_Hwnd.notebutton]:= "This opens a new window that allows you to enter the information that will appear on the Table's 'Notes' tab." Chr(10) "It is a RichText control, so formatting & links are allowed."
	hTTip[TAB_Hwnd.undo]:= "Undo action."
	hTTip[TAB_Hwnd.redo]:= "Redo action."
	hTTip[TAB_Hwnd.validateXML]:= "This opens a window to show the XML generated by the richtext box." Chr(10) "This allows you to check that formatting hasn't caused XML errors."
	hTTip[TAB_Hwnd.import]:= "Click to import all the values for your Table from a CSV file on your drive." Chr(10) "This will update all values, so ensure you have saved any work you wish to keep before doing this!"
	hTTip[TAB_Hwnd.save]:= "Save the Table to your drive as a *.TBL file." Chr(10) "This can be reloaded for further editing."
	hTTip[TAB_Hwnd.append]:= "Add this Table to a parsing project (or update it if it is already part of the project)." Chr(10) "If you haven't set up a project, you are taken to the project management window."
}

TAB_Vup() {
	global VPTable, TAB_ScrollPoint, TAB_ScrollEnd, TAB_buttonup, TAB_buttondn
	MouseGetPos,,,,ctrl, 2
	while (ctrl=TAB_buttonup && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		VPtable.Document.parentWindow.eval("scrollBy(0, -2);")
		TAB_ScrollPoint -= 2
		If (TAB_ScrollPoint < 0) {
			TAB_ScrollPoint:= 0
		}
	}
	while (ctrl=TAB_buttondn && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		VPtable.Document.parentWindow.eval("scrollBy(0, 2);")
		TAB_ScrollPoint += 2
		If (TAB_ScrollPoint > TAB_ScrollEnd) {
			TAB_ScrollPoint:= TAB_ScrollEnd
		}
	}
}

TAB_VScrUp() {
	global VPTable, IMSpell, TAB_ScrollPoint, IMScrollPoint, TAB_Main, TAB_Import
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" TAB_Main) {
		VPtable.Document.parentWindow.eval("scrollBy(0, -50);")
		TAB_ScrollPoint -= 50
		If (TAB_ScrollPoint < 0) {
			TAB_ScrollPoint:= 0
		}
	}
	;~ If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" TAB_Import) {
		;~ IMtable.Document.parentWindow.eval("scrollBy(0, -50);")
		;~ IMScrollPoint -= 50
		;~ If (IMScrollPoint < 0) {
			;~ IMScrollPoint:= 0
		;~ }
	;~ }
}

TAB_VScrDwn() {
	global VPTable, IMSpell, TAB_ScrollPoint, IMScrollPoint, TAB_ScrollEnd, IMScrollEnd, TAB_Main, TAB_Import
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" TAB_Main) {
		VPtable.Document.parentWindow.eval("scrollBy(0, 50);")
		TAB_ScrollPoint += 50
		If (TAB_ScrollPoint > TAB_ScrollEnd) {
			TAB_ScrollPoint:= TAB_ScrollEnd
		}
	}
	;~ If (ctrl="Internet Explorer_Server1") AND WinActive("ahk_id" TAB_Import) {
		;~ IMtable.Document.parentWindow.eval("scrollBy(0, 50);")
		;~ IMScrollPoint += 50
		;~ If (IMScrollPoint > IMScrollEnd) {
			;~ IMScrollPoint:= IMScrollEnd
		;~ }
	;~ }
}

TAB_JSONFile() {
	global table, ObjTAB
	ObjTAB:= {}
	ObjName:= table.filename
	ObjTAB[ObjName]:= {}
	ObjTAB[ObjName].name:= table.name
	ObjTAB[ObjName].description:= table.description
	ObjTAB[ObjName].dicemodifier:= table.dicemodifier
	ObjTAB[ObjName].output:= table.output
	ObjTAB[ObjName].rows:= table.rows
	ObjTAB[ObjName].columns:= table.columns
	ObjTAB[ObjName].rolltype:= table.rolltype
	ObjTAB[ObjName].min:= table.min
	ObjTAB[ObjName].max:= table.max
	ObjTAB[ObjName].locked:= table.locked
	ObjTAB[ObjName].showroll:= table.showroll
	ObjTAB[ObjName].FGcat:= table.FGcat

	tempdice:= ""
	if (table.rolltype = "Custom dice roll") {
		if % table.d4 {
			loop, % table.d4 {
				tempdice .= ",d4"
			}
		}
		if % table.d6 {
			loop, % table.d6 {
				tempdice .= ",d6"
			}
		}
		if % table.d8 {
			loop, % table.d8 {
				tempdice .= ",d8"
			}
		}
		if % table.d10 {
			loop, % table.d10 {
				tempdice .= ",d10"
			}
		}
		if % table.d12 {
			loop, % table.d12 {
				tempdice .= ",d12"
			}
		}
		if % table.d20 {
			loop, % table.d20 {
				tempdice .= ",d20"
			}
		}
		tempdice:= RegExReplace(tempdice, "^,+", "")
	}

	ObjTAB[ObjName].dice:= tempdice

	Loop, % table.columns {
		lcat:= "labelcol" . A_Index
		ObjTAB[ObjName, lcat]:= table.col[A_Index]
	}

	Loop, % table.rows {
		rowv:= A_Index
		Loop, % table.columns + 2 {
			ObjTAB[ObjName, "Arr" rowv]:= table.arr[rowv]
		}
	}

	SpText:= "`t`t`t`t`t<notes type=""formattedtext"">" table.notes

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

	TKN .= "</notes>"
		
	ObjTAB[ObjName].notes:= TKN
}

TAB_Parsetable(zz) {
	Global Table, TT1
	row:= table.rows
	col:= table.columns

	zz:= RegExReplace(zz, "Us)\r\n", "")
	FoundPos:= RegExMatch(zz, "Uis)<tr>.*</tr>", rtf)
	If rtf {
		StringReplace zz, zz, %rtf%, 
		StringReplace rtf, rtf, <td>From</td>, 
		StringReplace rtf, rtf, <td>To</td>, 
	}
	
	Loop, %col% {
		FoundPos:= RegExMatch(rtf, "Uis)<td>.*</td>", rtf2)
		StringReplace rtf, rtf, %rtf2%, 
		StringReplace rtf2, rtf2, <td>, 
		StringReplace rtf2, rtf2, </td>, 
		table.col[A_Index]:= rtf2
	}
	
	Loop, %row% {
		FoundPos:= RegExMatch(zz, "Uis)<tr>.*</tr>", rtf)
		StringReplace zz, zz, %rtf%, 
		rowv:= A_Index
		Loop % col + 2 {
			FoundPos:= RegExMatch(rtf, "Uis)<td>.*</td>", rtf2)
			StringReplace rtf, rtf, %rtf2%, 
			StringReplace rtf2, rtf2, <td>, 
			StringReplace rtf2, rtf2, </td>, 
			If (A_Index < 3) {
				If rtf2 is number
				{
					table.Arr[rowv, A_Index]:= rtf2
				} Else If rtf2 is space
				{
					table.Arr[rowv, A_Index]:= rtf2
				} Else {
					caret:= TT1.GetSel()
					TAB_RTF()
					TT1.SetSel(caret.S-1, caret.E)
				}
			} Else {
				table.Arr[rowv, A_Index]:= rtf2
			}
		}
	}
}

TAB_Notes() {
	AddNotes("table", "TAB")
}

TAB_val() {
	;~ Global table
	;~ row:= table.rows
	;~ col:= table.cols
	;~ If (table.rolltype = "Custom dice roll") {

	;~ }
	;~ If (table.rolltype = "Preset range") {
		;~ table.Arr[1, 1]:= table.min
		;~ Loop, %row% {
			;~ If ((table.Arr[A_Index][2] = "") And (table.Arr[A_Index+1][1] = "")) {
				;~ table.Arr[A_Index, 2]:= table.Max
				;~ break
			;~ }
		;~ }
		;~ TAB_RTF()
	;~ }
	;~ If (table.rolltype = "Based on table values") {
		
	;~ }
}

TAB_FRD() {
	Global Table
	Loop, % table.rows {
		rowv:= A_Index
		Loop % table.columns {
			colv:= A_Index + 2
			cellv:= table.arr[rowv][colv]
			cellv:= regexreplace(cellv, "(\d+d\d+)", "[$1]")
			stringreplace, cellv, cellv, `[`[, `[, All
			stringreplace, cellv, cellv, `]`], `], All
			table.Arr[rowv, colv]:= cellv
		}
	}
	TAB_RTF()
	TAB_RH_Box()
}

TAB_FTD() {
	Global Table
	Loop, % table.rows {
		rowv:= A_Index
		Loop % table.columns {
			colv:= A_Index + 2
			cellv:= table.arr[rowv][colv]
			cellv:= regexreplace(cellv, "(\d+d\d+)", "[$1]")
			stringreplace, cellv, cellv, `[`[, , All
			stringreplace, cellv, cellv, `]`], , All
			table.Arr[rowv, colv]:= cellv
		}
	}
	TAB_RTF()
	TAB_RH_Box()
	
}



;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |           Table Specific Functions           |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


CellPosition(ByRef cellrow, ByRef cellcol) {
	global TT1, table
	mark:= Chr(124)
	Clp(mark)
	zz:= TT1.GetRTF(False)
	send {BS}
	zz:= Tokenise(zz)
	StringReplace, zz, zz, <p></p>, , All
	StringReplace, zz, zz, `r`n`r`n, `r`n, All
	zz:= RegexReplace(zz, "^\s+|\s+$" )

	row:= table.rows
	col:= table.columns

	zz:= RegExReplace(zz, "Us)\r\n", "")
	
	Loop, % row + 1 {
		FoundPos:= RegExMatch(zz, "Uis)<tr>.*</tr>", rtf)
		StringReplace zz, zz, %rtf%, 
		rowv:= A_Index
		Loop % col + 2 {
			FoundPos:= RegExMatch(rtf, "Uis)<td>.*</td>", rtf2)
			StringReplace rtf, rtf, %rtf2%, 
			If Instr(rtf2, mark) {
				cellrow:= rowv
				cellcol:= A_Index
				return
			}
		}
	}
	cellrow:= 0
	cellcol:= 0
}

TableSubCreate(cr, cc) {
	If (cr > 0) AND (cc > 0) {
		if (cr = 1) AND (cc < 3) {
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Clear header row, RC_ClearRow
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Clear column, RC_ClearColumn
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Clear table, RC_ClearTable
		} Else If (cr = 1) AND (cc > 2){
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Clear header row, RC_ClearRow
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Insert column, RC_InsertColumn
			Menu LinkSub, Add, Delete column, RC_DeleteColumn
			Menu LinkSub, Add, Clear column, RC_ClearColumn
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Clear cell, RC_ClearCell
			Menu LinkSub, Add, Clear table, RC_ClearTable
		} Else If (cr > 1) AND (cc < 3){
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Insert row, RC_InsertRow
			Menu LinkSub, Add, Delete row, RC_DeleteRow
			Menu LinkSub, Add, Clear row, RC_ClearRow
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Clear column, RC_ClearColumn
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Clear cell, RC_ClearCell
			Menu LinkSub, Add, Clear table, RC_ClearTable
		} Else {
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Insert row, RC_InsertRow
			Menu LinkSub, Add, Delete row, RC_DeleteRow
			Menu LinkSub, Add, Clear row, RC_ClearRow
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Insert column, RC_InsertColumn
			Menu LinkSub, Add, Delete column, RC_DeleteColumn
			Menu LinkSub, Add, Clear column, RC_ClearColumn
			Menu LinkSub, Add, 
			Menu LinkSub, Add, Clear cell, RC_ClearCell
			Menu LinkSub, Add, Clear table, RC_ClearTable
		}
	}
}

RC_ClearCell() {
	global
	local hero
	If (cr = 1) {
		table.col[cc-2]:= ""
	} else {
		table.arr[cr-1, cc]:= ""
	}
	TAB_RTF()
	Click %click_X%, %click_Y%
	Sleep, 50
	TAB_RH_Box()
}

RC_ClearColumn() {
	global
	local hero
	
	Loop % table.rows {
		table.arr[A_Index, cc]:= ""
	}
	If (cc > 2) {
		table.col[cc - 2]:= ""
	}
	TAB_RTF()
	Click %click_X%, %click_Y%
	Sleep, 50
	TAB_RH_Box()
}

RC_ClearRow() {
	global
	local hero
	
	Loop % table.columns + 2 {
		table.arr[cr-1, A_Index]:= ""
	}
	TAB_RTF()
	Click %click_X%, %click_Y%
	Sleep, 50
	TAB_RH_Box()
}

RC_ClearHeadings() {
	global
	local hero
	
	Loop % table.columns {
		table.col[A_Index]:= ""
	}
	TAB_RTF()
	Click %click_X%, %click_Y%
	Sleep, 50
	TAB_RH_Box()
}

RC_DeleteColumn() {
	global
	local loops, hero, localcol
	if (table.columns > 1) {
		hero:= cc
		loops:= table.columns + 2 - hero + 1
		Loop %loops% {
			localcol:= A_Index + hero -1
			loop % table.rows {
					table.arr[A_Index, localcol]:= table.arr[A_Index][localcol + 1]
			}
			table.col[A_Index]:= table.col[A_Index + 1]
		}
		table.columns--
		Gset(TAB_Hwnd.columns, table.columns)
		TAB_RTF()
		Click %click_X%, %click_Y%
		Sleep, 50
		TAB_RH_Box()
	}
}

RC_DeleteRow() {
	global
	local loops, hero
	If (table.rows > 1) {
		hero:= cr - 1
		loops:= table.rows - hero + 1
		Loop %loops% {
			table.arr[A_Index + hero - 1]:= table.arr[A_Index + hero]
		}
		table.rows--
		Gset(TAB_Hwnd.rows, table.rows)
		TAB_RTF()
		Click %click_X%, %click_Y%
		Sleep, 50
		TAB_RH_Box()
	}
}

RC_InsertColumn() {
	global
	local loops, hero, localcol
	if (table.columns > 1) {
		hero:= cc
		loops:= table.columns + 2 - hero + 1
		Loop %loops% {
			localcol:= table.columns + 2 - A_Index + 1
			loop % table.rows {
					table.arr[A_Index, localcol + 1]:= table.arr[A_Index][localcol]
					table.arr[A_Index, localcol]:= []
			}
			table.col[localcol - 1]:= table.col[localcol - 2]
			table.col[localcol - 2]:= ""
		}
		table.columns++
		Gset(TAB_Hwnd.columns, table.columns)
		TAB_RTF()
		Click %click_X%, %click_Y%
		Sleep, 50
		TAB_RH_Box()
	}
}

RC_InsertRow() {
	global
	local loops, hero, current
		hero:= cr - 1
		loops:= table.rows - hero + 1
		Loop %loops% {
			current:= table.rows - A_Index + 1
			table.arr[current + 1]:= table.arr[current]
			table.arr[current]:= []
		}
		table.rows++
		Gset(TAB_Hwnd.rows, table.rows)
		TAB_RTF()
		Click %click_X%, %click_Y%
		Sleep, 50
		TAB_RH_Box()
}

RC_ClearTable() {
	global
	local loops, hero, current
		Loop % table.rows {
			table.arr[A_Index]:= []
		}
		table.col:= []
		TAB_RTF()
		Click %click_X%, %click_Y%
		Sleep, 50
		TAB_RH_Box()
}

TAB_Paste() {
	Global Table, RTFCaretPos, TAB_Hwnd
	controlgetfocus, fcs
	if(fcs = "RICHEDIT50W1") {
		pst:= []
		pst.rows:= 0
		pst.cols:= 0
		var:= clipboard "`r`n"
		stringreplace var, var, `r`n`r`n, `r`n, all
		Loop, parse, var, `n, `r
		{
			j:= A_Index
			Loop, Parse, A_Loopfield, %A_Tab%
			{
				k:= A_Index
				pst[j, k]:= A_LoopField
				if (A_Index > pst.cols) {
					pst.cols:= A_Index
				}
			}
			if (A_Index > pst.rows) {
				pst.rows:= A_Index - 1
			}
		}
		if (pst.rows = 0) {
			pst.rows:= 1
		}
		If (pst.rows = 1) AND (pst.cols = 1) {
			Send ^v
			return
		}
		CellPosition(cr, cc)
		if (pst.rows + cr - 2 > table.rows) {
			table.rows:= pst.rows + cr - 2 
			Gset(TAB_Hwnd.rows, table.rows)
		}
		if (pst.cols + cc - 3 > table.columns) {
			table.columns:= pst.cols + cc - 3
			Gset(TAB_Hwnd.columns, table.columns)
		}
		If (cr > 0) AND (cc > 0) {
			if (cr = 1) AND (cc < 3) {
				cj:= cr - 1
				ck:= cc - 2
				loop % pst.cols {
					dk:= A_Index
					If (dk + ck + 1 > 2) {
						table.col[dk + ck - 1]:= pst[1][dk]
					}
				}
				loop % pst.rows - 1 {
					dj:= A_Index + 1
					loop % pst.cols {
						dk:= A_Index
						element:= pst[dj][dk]
						colno:= dk + ck + 1
						If (colno < 3) {
							If element is number
							{
								table.arr[dj + cj - 1, dk + ck + 1]:= element
							} Else If element is space
							{
								table.arr[dj + cj - 1, dk + ck + 1]:= element
							} Else {
								
							}
						} Else {
							table.arr[dj + cj - 1, dk + ck + 1]:= element
						}
					}
				}
				TAB_RTF()
				TAB_RH_Box()
			} Else If (cr = 1) AND (cc > 2){
				cj:= cr - 1
				ck:= cc - 2
				loop % pst.cols {
					dk:= A_Index
					table.col[dk + ck - 1]:= pst[1][dk]
				}
				loop % pst.rows - 1 {
					dj:= A_Index + 1
					loop % pst.cols {
						dk:= A_Index
						table.arr[dj + cj - 1, dk + ck + 1]:= pst[dj][dk]
					}
				}
				TAB_RTF()
				TAB_RH_Box()
			} Else If (cr > 1) AND (cc < 3){
				cj:= cr - 1
				ck:= cc - 2
				loop % pst.rows {
					dj:= A_Index
					loop % pst.cols {
						dk:= A_Index
						element:= pst[dj][dk]
						colno:= dk + ck + 1
						If (colno < 3) {
							If element is number
							{
								table.arr[dj + cj - 1, dk + ck + 1]:= element
							} Else If element is space
							{
								table.arr[dj + cj - 1, dk + ck + 1]:= element
							} Else {
								
							}
						} Else {
							table.arr[dj + cj - 1, dk + ck + 1]:= element
						}
					}
				}
				TAB_RTF()
				TAB_RH_Box()
			} Else {
				cj:= cr - 1
				ck:= cc - 2
				loop % pst.rows {
					dj:= A_Index
					loop % pst.cols {
						dk:= A_Index
						table.arr[dj + cj - 1, dk + ck + 1]:= pst[dj][dk]
					}
				}
				TAB_RTF()
				TAB_RH_Box()
			}
		}
	} Else {
		send ^v
	}
}



;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                 GUI Functions                |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


TAB_Graphical(UI, Scr) {
	Global VPspell, IMspell, Table, TT1


	;{ Data for Statblock

	SpName:= table.name

	TABdecsr:= table.description
	TABout:= table.output
	
	row:= table.rows
	col:= table.columns
	
	TH1 = 
(
	<table class="spell">
		<thead>
			<tr>
				<th style="width: 0.9cm">From</th>
				<th style="width: 0.9cm">To</th>
)
	TH2 = 
(
			</tr>
		</thead>
)
	TH3 = 
(
	</table>
)

	Loop, %col% {
		TH1 .= "<th>" table.col[A_Index] "</th>"
	}
	TB:= ""
	Loop, %row% {
		TR:= "<tr>"
		rowv:= A_Index
		Loop, % col + 2 {
			If (A_Index < 3) {
				TR .= "<td align=""center"" class=""numbers"">" table.arr[rowv][A_Index] "</td>"
			} Else {
				TR .= "<td>" table.arr[rowv][A_Index] "</td>"
			}
		}
		TB .= TR . "</tr>"
	}

	SpText:= TH1 . TH2 . TB . TH3
	
	If (table.rolltype = "Custom dice roll") {
		diceroll:= ""
		dicermin:= 0
		dicermax:= 0
		if (table.d4 > 0) {
			if diceroll
				diceroll .= "+"
			diceroll .= table.d4 "d4"
			dicermin += table.d4
			dicermax += table.d4 * 4
		}
		if (table.d6 > 0) {
			if diceroll
				diceroll .= "+"
			diceroll .= table.d6 "d6"
			dicermin += table.d6
			dicermax += table.d6 * 6
		}
		if (table.d8 > 0) {
			if diceroll
				diceroll .= "+"
			diceroll .= table.d8 "d8"
			dicermin += table.d8
			dicermax += table.d8 * 8
		}
		if (table.d10 > 0) {
			if diceroll
				diceroll .= "+"
			diceroll .= table.d10 "d10"
			dicermin += table.d10
			dicermax += table.d10 * 10
		}
		if (table.d12 > 0) {
			if diceroll
				diceroll .= "+"
			diceroll .= table.d12 "d12"
			dicermin += table.d12
			dicermax += table.d12 * 12
		}
		if (table.d20 > 0) {
			if diceroll
				diceroll .= "+"
			diceroll .= table.d20 "d20"
			dicermin += table.d20
			dicermax += table.d20 * 20
		}
		If (table.dicemodifier > 0) {
			diceroll .= "+" table.dicemodifier
			dicermin += table.dicemodifier
			dicermax += table.dicemodifier
		}
		If (table.dicemodifier < 0) {
			diceroll .= table.dicemodifier
			dicermin += table.dicemodifier
			dicermax += table.dicemodifier
		}
		diceroll:= dicermin " to " dicermax " (" diceroll ")"
	} Else If (table.rolltype = "Based on table values") {
		diceroll:= ""
		dicemin:= 50,000,000
		dicemax:= 1
		table.dicemodifier:= 0
		loop, %row% {
			temp:= table.arr[A_Index][1]
			if (temp = "")
				temp:= 0
			If ((temp < dicemin) and (temp > 0))
				dicemin:= temp
			If (temp > dicemax)
				dicemax:= temp
			If (table.arr[A_Index][2] > dicemax)
				dicemax:= table.arr[A_Index][2]
		}
		If (dicemin = 50,000,000)
			dicemin:= 1
		if (dicemin > 1) {
			table.dicemodifier:= dicemin - 1
			dicemin:= 1
			dicemax:= dicemax - table.dicemodifier
		}
		dicermin:= dicemin + table.dicemodifier
		dicermax:= dicemax + table.dicemodifier
		diceroll .= dicermin " to " dicermax " (1d" dicemax
		If (table.dicemodifier > 0)
			diceroll .= "+" table.dicemodifier
		If (table.dicemodifier < 0)
			diceroll .= table.dicemodifier
		diceroll .= ")"
	} else if (table.rolltype = "Preset range") {
		diceroll:= ""
		dicemin:= table.min
		dicemax:= table.max
		table.dicemodifier:= 0
		if (dicemin > 1) {
			table.dicemodifier:= dicemin - 1
			dicemin:= 1
			dicemax:= dicemax - table.dicemodifier
		}
		dicermin:= dicemin + table.dicemodifier
		dicermax:= dicemax + table.dicemodifier
		diceroll .= dicermin " to " dicermax " (1d" dicemax
		If (table.dicemodifier > 0)
			diceroll .= "+" table.dicemodifier
		If (table.dicemodifier < 0)
			diceroll .= table.dicemodifier
		diceroll .= ")"
	}		

	rolltype:= table.rolltype

	notes:= table.notes
	StringReplace, notes, notes, <frame>, %frtable%, all
	StringReplace, notes, notes, </frame>, %frtablend%, all
	StringReplace, notes, notes, <h>, <span class=`"heading`">, all
	StringReplace, notes, notes, </h>, </span>, all

	SpText .= notes
	SpText:= LinkHTML(SpText)
	
	StringReplace, SpText, SpText, </p>`r`n<p>, <br>`r`n, All
	StringReplace, SpText, SpText, </p>`n<p>, <br>`r`n, All
	;~ StringReplace, SpText, SpText, \par, <br>`r`n, All
	
	;}

	#include HTML_Table_Engineer.ahk
	HTML_Spell:= css . htmspell
	stringreplace, HTML_Spell, HTML_Spell, \r, <br>, All

	documentz:= %UI%.Document
	documentz.open()
	documentz.write(HTML_Spell)
	documentz.close()
	%UI%.Document.parentWindow.eval("scrollTo(0, " Scr ");")
}

TAB_RTF() {
	Global
	Local fonth, fontb, rtfdoc, statblock
	fonth:= 22
	fontb:= 20
	fontn:= 26
	colourtable:= "{\colortbl `;\red0\green0\blue0`;\red255\green255\blue255`;\red200\green180\blue160`;\red230\green154\blue40`;\red233\green221\blue200`;\red150\green10\blue10`;}"
	fonttable:= "{\fonttbl {\f0 Arial`;}"

	local cols, rows, twips, posn, headers, line
	cols:= table.columns
	twips:= (7860 - 1600) / cols
	if twips < 1550
		twips:= 1500

	tabtop := "\trowd\trrh400"
	tabtop .= "\clvertalc\clcbpat3\clbrdrt\brdrw1\clbrdrb\brdrw1\clbrdrl\brdrw1\clbrdrr\brdrw1\cellx800"
	tabtop .= "\clvertalc\clcbpat3\clbrdrt\brdrw1\clbrdrb\brdrw1\clbrdrl\brdrw1\clbrdrr\brdrw1\cellx1600"

	loop, %cols% {
		posn:= Floor((twips * A_Index) + 1600)
		tabtop .= "\clvertalc\clcbpat3\clbrdrt\brdrw1\clbrdrb\brdrw1\clbrdrl\brdrw1\clbrdrr\brdrw1\cellx" posn
	}
	
	tabrow := "\trowd\trrh400"
	tabrow .= "\clvertalc\clcbpat5\clbrdrt\brdrw1\clbrdrb\brdrw1\clbrdrl\brdrw1\clbrdrr\brdrw1\cellx800"
	tabrow .= "\clvertalc\clcbpat5\clbrdrt\brdrw1\clbrdrb\brdrw1\clbrdrl\brdrw1\clbrdrr\brdrw1\cellx1600"

	loop, %cols% {
		posn:= Floor((twips * A_Index) + 1600)
		tabrow .= "\trgaph150\clvertalc\clcbpat2\clbrdrt\brdrw1\clbrdrb\brdrw1\clbrdrl\brdrw1\clbrdrr\brdrw1\cellx" posn
	}

	headers:= " From\intbl\cell To\intbl\cell"
	loop, %cols% {
		headers .= " " table.Col[A_Index] "\intbl\cell"
	}
	
	Statblock:= tabtop "\f0\cf0\fs" fonth "\b\qc" headers "\row\ql\b0" 
	
	loop, % table.rows {
		line:= ""
		rows:= A_Index
		loop, %cols% {
			line .= " " table.Arr[rows][A_Index + 2] "\intbl\cell"
		}
		Statblock .= tabrow "\f0\cf6\fs" fontn "\b\qc " table.Arr[rows][1] "\intbl\cell " table.Arr[rows][2] "\intbl\cell\ql\b0" 
		Statblock .= "\f0\cf0\fs" fontb line "\row" 
	}
	
	rtfdoc:= "{\rtf1\ansi\deff0 " fonttable "}" colourtable statblock "}"

	TT1.SetText(rtfdoc, ["KEEPUNDO"])
}

TAB_PrepareGUI() {
	global
	TAB_GUIMain()
	If (TAB_Ref = "self") {
		GUI_Project()
	}
	TAB_SetTT()
}

TAB_GUIMain()	 {
	global
	local tempy
	
	Gui, TAB_Main:-MaximizeBox
	Gui, TAB_Main:+hwndTAB_Main
	Gui, TAB_Main:Color, 0xE2E1E8
	Gui, TAB_Main:font, S10 c000000, Arial
	
; Menu system
;{
	Menu TAB_FileMenu, Add, &New Table`tCtrl+N, New_Table
	Menu TAB_FileMenu, Icon, &New Table`tCtrl+N, NPC Engineer.dll, 1
	Menu TAB_FileMenu, Add, &Open Table`tCtrl+O, Open_Table
	Menu TAB_FileMenu, Icon, &Open Table`tCtrl+O, NPC Engineer.dll, 2
	Menu TAB_FileMenu, Add, &Save Table`tCtrl+S, Save_Table
	Menu TAB_FileMenu, Icon, &Save Table`tCtrl+S, NPC Engineer.dll, 3
	Menu TAB_FileMenu, Add
	;~ Menu TAB_FileMenu, Add, Save as Text, Save_Txt
	;~ Menu TAB_FileMenu, Add, Save as XML, Save_XML
	;~ Menu TAB_FileMenu, Add, Save as HTML, Save_HTML
	;~ Menu TAB_FileMenu, Add, Save as RTF, Save_RTF
	;~ Menu TAB_FileMenu, Add
	;~ Menu TAB_FileMenu, Add, Place BBCode on Clipboard, Copy_BB
	;~ Menu TAB_FileMenu, Add
	Menu TAB_FileMenu, Add, E&xit`tESC, TAB_MainGuiClose
	Menu TAB_FileMenu, Icon, E&xit`tESC, NPC Engineer.dll, 17
	Menu TableEngineerMenu, Add, File, :TAB_FileMenu
	
	Menu TAB_OptionsMenu, Add, &Import CSV, Import_Table_Text
	Menu TAB_OptionsMenu, Icon, &Import CSV, NPC Engineer.dll, 4
	Menu TAB_OptionsMenu, Add, &Create Module `tCtrl+P, ParseProject
	Menu TAB_OptionsMenu, Icon, &Create Module `tCtrl+P, NPC Engineer.dll, 6
	Menu TAB_OptionsMenu, Add
	Menu TAB_OptionsMenu, Add, Manage Categories `tCtrl+K, GUI_Categories
	Menu TAB_OptionsMenu, Icon, Manage Categories `tCtrl+K, NPC Engineer.dll, 25
	Menu TAB_OptionsMenu, Add, Manage Table File`tCtrl+M, Manage_TAB_JSON
	Menu TAB_OptionsMenu, Add
	Menu TAB_OptionsMenu, Add, Settings`tF11, GUI_Options
	Menu TAB_OptionsMenu, Icon, Settings`tF11, NPC Engineer.dll, 9
	Menu TableEngineerMenu, Add, Options, :TAB_OptionsMenu
	
	Component_Menu("TAB_ComponentMenu", "table")
	Menu TableEngineerMenu, Add, Engineer Suite, :TAB_ComponentMenu
	
	Explorer_Menu("TAB_ExplorerMenu")
	Menu TableEngineerMenu, Add, Directories, :TAB_ExplorerMenu
	
	Backup_Menu("TAB_BackupMenu")
	Menu TableEngineerMenu, Add, Backup, :TAB_BackupMenu

	Help_Menu("TAB_HelpMenu", "Table Engineer")
	Menu TableEngineerMenu, Add, Information, :TAB_HelpMenu
	Gui, TAB_Main:Menu, TableEngineerMenu
;}


;  ================================================
;  |            GUI for 'Table info' tab          |
;  ================================================
;{

	Gui, TAB_Main:Add, GroupBox, x7 y38 w606 h557, Table Information

	Gui, TAB_Main:Add, Text, x10 y63 w80 h17 Right, Table Name:
	Gui, TAB_Main:Add, Text, x10 y90 w80 h17 Right, Description:
	Gui, TAB_Main:Add, Text, x445 y63 w70 h17 Right, Output to:
	Gui, TAB_Main:Add, Text, x10 y132 w80 h17 Right, Roll Method:
		
	Gui, TAB_Main:Add, Edit, x95 y60 w340 h23 HwndTempy gTAB_MainUpdate,
		TAB_Hwnd.name:= Tempy
	Gui, TAB_Main:Add, Combobox, x520 y60 w80 HwndTempy gTAB_MainUpdate, chat||story|parcel|encounter
		TAB_Hwnd.output:= Tempy
	Gui, TAB_Main:Add, Edit, x95 y86 w340 h40 Multi HwndTempy gTAB_MainUpdate,
		TAB_Hwnd.description:= Tempy
	Gui, TAB_Main:Add, Combobox, x95 y129 w190 HwndTempy gTAB_Rollmethod, Preset range|Based on table values||Custom dice roll
		TAB_Hwnd.rolltype:= Tempy
	Gui, TAB_Main:Add, Button, x490 y106 w110 h20 +border +NoTab HwndTempy gTAB_Notes, Add Description
		TAB_Hwnd.notebutton:= Tempy

; Custom Dice controls
;{
	Gui, TAB_Main:Add, Text, x137 y164 w24 h17 Hidden HwndTempy, d4
		TAB_Hwnd.d4txt:= Tempy
	Gui, TAB_Main:Add, Edit, x95 y160 w40 h24 Center Hidden HwndTempy, 
		TAB_Hwnd.d4edit:= Tempy
	Gui, TAB_Main:Add, UpDown, Hidden HwndTempy gTAB_MainUpdate Range0-99, 0
		TAB_Hwnd.d4:= Tempy

	Gui, TAB_Main:Add, Text, x207 y164 w24 h17 Hidden HwndTempy, d6
		TAB_Hwnd.d6txt:= Tempy
	Gui, TAB_Main:Add, Edit, x165 y160 w40 h24 Center Hidden HwndTempy, 
		TAB_Hwnd.d6edit:= Tempy
	Gui, TAB_Main:Add, UpDown, Hidden HwndTempy gTAB_MainUpdate Range0-99, 0
		TAB_Hwnd.d6:= Tempy

	Gui, TAB_Main:Add, Text, x277 y164 w24 h17 Hidden HwndTempy, d8
		TAB_Hwnd.d8txt:= Tempy
	Gui, TAB_Main:Add, Edit, x235 y160 w40 h24 Center Hidden HwndTempy, 
		TAB_Hwnd.d8edit:= Tempy
	Gui, TAB_Main:Add, UpDown, Hidden HwndTempy gTAB_MainUpdate Range0-99, 0
		TAB_Hwnd.d8:= Tempy

	Gui, TAB_Main:Add, Text, x347 y164 w24 h17 Hidden HwndTempy, d10
		TAB_Hwnd.d10txt:= Tempy
	Gui, TAB_Main:Add, Edit, x305 y160 w40 h24 Center Hidden HwndTempy, 
		TAB_Hwnd.d10edit:= Tempy
	Gui, TAB_Main:Add, UpDown, Hidden HwndTempy gTAB_MainUpdate Range0-99, 0
		TAB_Hwnd.d10:= Tempy

	Gui, TAB_Main:Add, Text, x417 y164 w24 h17 Hidden HwndTempy, d12
		TAB_Hwnd.d12txt:= Tempy
	Gui, TAB_Main:Add, Edit, x375 y160 w40 h24 Center Hidden HwndTempy, 
		TAB_Hwnd.d12edit:= Tempy
	Gui, TAB_Main:Add, UpDown, Hidden HwndTempy gTAB_MainUpdate Range0-99, 0
		TAB_Hwnd.d12:= Tempy

	Gui, TAB_Main:Add, Text, x487 y164 w24 h17 Hidden HwndTempy, d20
		TAB_Hwnd.d20txt:= Tempy
	Gui, TAB_Main:Add, Edit, x445 y160 w40 h24 Center Hidden HwndTempy, 
		TAB_Hwnd.d20edit:= Tempy
	Gui, TAB_Main:Add, UpDown, Hidden HwndTempy gTAB_MainUpdate Range0-99, 0
		TAB_Hwnd.d20:= Tempy

	Gui, TAB_Main:Add, Text, x517 y164 w38 h17 Right Hidden HwndTempy, Mod:
		TAB_Hwnd.dicemodifiertxt:= Tempy
	Gui, TAB_Main:Add, Edit, x560 y160 w40 h24 Center Hidden HwndTempy, 
		TAB_Hwnd.dicemodifieredit:= Tempy
	Gui, TAB_Main:Add, UpDown, Hidden HwndTempy gTAB_MainUpdate Range-30-30, 0
		TAB_Hwnd.dicemodifier:= Tempy
;}
	
; Preset Range controls
;{
	Gui, TAB_Main:Add, Text, x157 y164 w24 h17 Hidden HwndTempy, Min
		TAB_Hwnd.mintxt:= Tempy
	Gui, TAB_Main:Add, Edit, x95 y160 w60 h24 Center Hidden HwndTempy, 
		TAB_Hwnd.minedit:= Tempy
	Gui, TAB_Main:Add, UpDown, Hidden HwndTempy gTAB_MainUpdate Range0-10000, 1
		TAB_Hwnd.min:= Tempy

	Gui, TAB_Main:Add, Text, x247 y164 w24 h17 Hidden HwndTempy, Max
		TAB_Hwnd.maxtxt:= Tempy
	Gui, TAB_Main:Add, Edit, x185 y160 w60 h24 Center Hidden HwndTempy, 
		TAB_Hwnd.maxedit:= Tempy
	Gui, TAB_Main:Add, UpDown, Hidden HwndTempy gTAB_MainUpdate Range0-10000, 1
		TAB_Hwnd.max:= Tempy

;}

	Gui, TAB_Main:Add, Text, x20 y205 w40 h17 Right, Rows:
	Gui, TAB_Main:Add, Edit, x65 y202 w50 h24 Center, 
	Gui, TAB_Main:Add, UpDown, HwndTempy gTAB_MainUpdate Range1-999, 2
		TAB_Hwnd.rows:= Tempy

	Gui, TAB_Main:Add, Text, x120 y205 w60 h17 Right, Columns:
	Gui, TAB_Main:Add, Edit, x185 y202 w50 h24 Center, 
	Gui, TAB_Main:Add, UpDown, HwndTempy gTAB_MainUpdate Range1-20, 1
		TAB_Hwnd.columns:= Tempy

	Gui, TAB_Main:Add, Button, x390 y204 w100 h20 +border +NoTab HwndTempy gTAB_FTD, Text Dice
		TAB_Hwnd.ftd:= Tempy
	Gui, TAB_Main:Add, Button, x500 y204 w100 h20 +border +NoTab HwndTempy gTAB_FRD, Rollable Dice
		TAB_Hwnd.frd:= Tempy

	SpInfo:= ""
	TT1:= New RichEdit("TAB_Main", "x20 y230 w580 h330 HwndTempy gTAB_TextBox", True)
		TAB_Hwnd.RTF:= Tempy
		TT1.wordwrap(true)
		TT1.SetBkgndColor("White")
		TTFont := TT1.GetFont()
		TTFont.name:= "Arial"
		TTFont.Color:= "Black"
		TTFont.Size:= "10"
		TT1.SetFont(TTFont)
		Spacing:= []
		Spacing.After:= 0
		TT1.SetParaSpacing(Spacing)
	TAB_RTF()
	
	Gui, TAB_Main:Add, CheckBox, x20 y568 w125 h17 +0x20 Right HwndTempy Checked1 gTAB_MainUpdate, Lock Table in FG: 
		TAB_Hwnd.locked:= Tempy
	Gui, TAB_Main:Add, CheckBox, x155 y568 w170 h17 +0x20 Right HwndTempy Checked0 gTAB_MainUpdate, Show roll results in Chat: 
		TAB_Hwnd.showroll:= Tempy

	Gui, TAB_Main:Add, Button, x425 y568 w20 h20 HwndTempy vBTSTZ gundo, % Chr(11148)
		TAB_Hwnd.undo:= Tempy
	Gui, TAB_Main:Add, Button, x450 y568 w20 h20 HwndTempy vBTSTY gredo, % Chr(11150)
		TAB_Hwnd.redo:= Tempy

	Gui, TAB_Main:Add, Button, x500 y568 w100 h20 HwndTempy +border +NoTab gValXML, Validate XML
		TAB_Hwnd.validateXML:= Tempy
;}


	Gui, TAB_Main:Add, ActiveX, x620 y45 w500 h550 E0x200 +0x8000000 vVPTable, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	
	Gui, TAB_Main:Add, Button, x8 y605 w115 h30 +border HwndTempy vTbImport gImport_Table_Text, Import CSV
		TAB_Hwnd.import:= Tempy
	
	Gui, TAB_Main:Add, Text, x328 y610 w80 h17 Right, FG Category:
	Gui, TAB_Main:Add, Combobox, x413 y607 w200 HwndTempy gTAB_MainUpdate, 
		TAB_Hwnd.FGcat:= Tempy

	Gui, TAB_Main:Add, Button, x880 y605 w115 h30 HwndTempy +border vTbSave gSave_Table, Save Table
		TAB_Hwnd.save:= Tempy
	Gui, TAB_Main:Add, Button, x1005 y605 w115 h30 HwndTempy +border vTbAppend gTAB_Append, Add to Project
		TAB_Hwnd.append:= Tempy

	Gui, TAB_Main:font, S18 c000000, Arial
	Gui, TAB_Main:Add, Button, x1125 y545 w24 h24 hwndTAB_buttonup -Tabstop, % Chr(11165)
	Gui, TAB_Main:Add, Button, x1125 y571 w24 h24 hwndTAB_buttondn -Tabstop, % Chr(11167)

		
	Gui, TAB_Main:font, S9 c000000, Segoe UI
	Gui, TAB_Main:Add, StatusBar
	Gui, TAB_Main:Default
	SB_SetParts(450, 250)
	SB_SetText(" " WinTProj, 1)
	Gui, TAB_Main:font, S10 c000000, Arial

}

TAB_GUIImport()	 {
	;~ global
	
;~ ; Settings for text import window (TAB_Import)
	;~ Gui, TAB_Import:+OwnerTAB_Main
	;~ Gui, TAB_Import:-SysMenu
	;~ Gui, TAB_Import:+hwndTAB_Import
	;~ Gui, TAB_Import:Color, 0xE2E1E8
	;~ Gui, TAB_Import:font, S10 c000000, Arial
	
	;~ Gui, TAB_Import:Add, Edit, vSP_Cap_Text gTAB_Import_Update_Output x8 y8 w442 h474 Multi, %SP_Cap_Text%
	
	;~ Gui, TAB_Import:Add, ActiveX, x480 y8 w500 h500 E0x200 +0x8000000 vIMspell, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	

	;~ Gui, TAB_Import:Add, DropDownList, x300 y485 w150 vSP_ImportChoice gTAB_Import_Update_Output, PDF & Plain Text||
	;~ Gui, TAB_Import:Add, Button, x8 y515 w130 h30 +border vTAB_Import_Delete gTAB_Import_Delete, Delete All Text
	;~ Gui, TAB_Import:Add, Button, x150 y515 w130 h30 +border vTAB_Import_Append gTAB_Import_Append, Append Clipboard
	;~ Gui, TAB_Import:Add, Button, x710 y515 w130 h30 +border vTAB_Import_Return gTAB_Import_Return, Import and Return
	;~ Gui, TAB_Import:Add, Button, x852 y515 w130 h30 +border vTAB_Import_Cancel gTAB_Import_Cancel, Cancel All Changes

	;~ Hotkey, IfWinActive, ahk_id %TAB_Import%
	;~ Hotkey, ~Wheeldown, SpVScrDwn
	;~ Hotkey, ~Wheelup, SpVScrUp
	;~ hotkey, esc, EscapeHandle
}

TAB_GUIJSON()	 {
	global
	local tempy
	
; Settings for text import window (TAB_JSON)
	Gui, TAB_JSON:+OwnerTAB_Main
	Gui, TAB_JSON:-SysMenu
	Gui, TAB_JSON:+hwndTAB_JSON
	Gui, TAB_JSON:Color, 0xE2E1E8
	Gui, TAB_JSON:font, S10 c000000, Arial
	Gui, TAB_JSON:margin, 5, 1

	table_list:= "Choose a table from the JSON file||"
	For a, b in tbl.object()
	{
		table_list:= table_list TBL[a].name "|"
	}
	Gui, TAB_JSON:Add, ComboBox, x10 y10 w300 gTAB_JSON_Choose hwndTempy, %table_list%
		TAB_Hwnd.TAB_JSONChoose:= Tempy
	Gui, TAB_JSON:Add, Edit, x10 y40 w300 h60 hwndTempy +ReadOnly, 
		TAB_Hwnd.TAB_JSONselected:= Tempy
	Gui, TAB_JSON:Add, Button, x75 y105 w80 h25 +border gTAB_JSON_Del hwndTempy, Delete Table
		TAB_Hwnd.TAB_JSONDeleteButton:= Tempy
	Gui, TAB_JSON:Add, Button, x165 y105 w80 h25 +border gTAB_JSON_Edit hwndTempy Hidden, Edit Table
		TAB_Hwnd.TAB_JSONEditButton:= Tempy
	Gui, TAB_JSON:Add, Button, x180 y170 w130 h30 +border gTAB_JSON_Cancel hwndTempy, Close
		TAB_Hwnd.TAB_JSONCancelButton:= Tempy
}

TAB_CreateToolbar() {
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
		New Table
		Open Table
		Save Table
		-
		Open Previous Table
		Open Next Table
		-
		Manage Tables
		-
		Import CSV
		-
		Manage Project
		-
		Create Module
	)

	Return ToolbarCreate("OnToolbar", Buttons, "TAB_Main", ImageList, "Flat List Tooltips Border")
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
#Include Artefact Engineer.ahk

/* ========================================================
 *                  End of Include Files
 * ========================================================
*/

;~ ######################################################
;~ #                    Program End.                    #
;~ ######################################################