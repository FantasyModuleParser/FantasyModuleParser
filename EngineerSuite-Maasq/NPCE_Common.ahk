/*
Component:	NPCE Common
  Version:	0.6.18
     Date:	18/11/19
 Revision:	De_pdf2
 */


EscapeHandle:				;{
return						;}

RLink:						;{
	MouseGetPos, click_X, click_Y, , OutputVarControl, 0
	If (OutputVarControl = "RICHEDIT50W1") {
	Click %click_X%, %click_Y%
		Sleep, 20
		If WinActive("ahk_id" TAB_Main) {
			cr:= 0
			cc:= 0
			CellPosition(cr, cc)
		}
		LinkSubCreate()
		If WinActive("ahk_id" TAB_Main) {
			TableSubCreate(cr, cc)
		}
		Menu, LinkSub, Show	
	}
Return						;}

Undo:
	RichEditChoose()
	%RichText%.Undo()
	changeform:= 1
	GuiControl, Focus, % %RichText%.HWND
Return

Redo:
	RichEditChoose()
	%RichText%.Redo()
	changeform:= 1
	GuiControl, Focus, % %RichText%.HWND
Return

SetFontStyle:
	RichEditChoose()
	%RichText%.ToggleFontStyle(SubStr(A_GuiControl, 0))
	changeform:= 1
	GuiControl, Focus, % %RichText%.HWND
Return

SetFontStyle2:
	RichEditChoose()
	%RichText%.ToggleFontStyle(SubStr(A_ThisHotkey, 0))
	changeform:= 1
	GuiControl, Focus, % %RichText%.HWND
Return

BGCol:
	;~ m(RE1.GetRTF(False))
	;~ temptext:= tokenise(RE1.GetRTF(False))
	;~ m(temptext)
   IF (REbg:= !REbg)
	  RE1.SetBkgndColor("0xD0CBC0")
   else
	  RE1.SetBkgndColor("White")
   GuiControl, Focus, % RE1.HWND
Return

REHeader:
	RichEditChoose()
	XFont:= %RichText%.GetFont()
	XFont.Color:= "0x782220"
	XFont.Size:= "16"
	%RichText%.SetFont(XFont)
	changeform:= 1
	GuiControl, Focus, % %RichText%.HWND
Return

REBody:
	RichEditChoose()
	XFont:= %RichText%.GetFont()
	XFont.Color:= "Black"
	XFont.Size:= "10"
	%RichText%.SetFont(XFont)
	changeform:= 1
	GuiControl, Focus, % %RichText%.HWND
Return

REFrame:
	pf2:= RE1.getparaformat()
	measurement:= RE1.GetMeasurement()
	Spacing:= []
	If (pf2.numbering = 1) {
		bullets.type:= ""
		RE1.SetParaNumbering()
		BuIndent:= {Start: 0, Right: 0, Offset: 0}
		RE1.SetParaIndent(BuIndent)
		Spacing.After:= 4
		Spacing.Before:= 0
		RE1.SetParaSpacing(Spacing)
		pf2.startindent:= 0
	}
	IF (pf2.startindent = 0) {
		if (measurement = 1) {
			FrIndent:= {Start: 0.8, Right: 0.8, Offset: 0}
		} else {
			FrIndent:= {Start: 2, Right: 2, Offset: 0}
		}
		RE1.SetParaIndent(FrIndent)
		RE1.AlignText(4)
		Spacing.After:= 8
		Spacing.Before:= 8
		RE1.SetParaSpacing(Spacing)
	} else {
		FrIndent:= {Start: 0, Right: 0, Offset: 0}
		RE1.SetParaIndent(FrIndent)
		RE1.AlignText(1)
		Spacing.After:= 4
		Spacing.Before:= 0
		RE1.SetParaSpacing(Spacing)
	}
	GuiControl, Focus, % RE1.HWND
Return

REBullet:
	RichEditChoose()
	pf2:= %RichText%.getparaformat()
	IF (pf2.startindent != 0) {
		FrIndent:= {Start: 0, Right: 0, Offset: 0}
		%RichText%.SetParaIndent(FrIndent)
		%RichText%.AlignText(1)
		Spacing.After:= 4
		Spacing.Before:= 0
		%RichText%.SetParaSpacing(Spacing)
	}
	If (pf2.numbering = 1) {
		bullets.type:= ""
		%RichText%.SetParaNumbering()
		BuIndent:= {Start: 0, Right: 0, Offset: 0}
		%RichText%.SetParaIndent(BuIndent)
		Spacing.After:= 4
		Spacing.Before:= 0
		%RichText%.SetParaSpacing(Spacing)
	} else {	
		bullets.type:= "bullet"
		%RichText%.SetParaNumbering(bullets)
		if (measurement = 1) {
			BuIndent:= {Start: 0.2, Right: 0, Offset: 0}
		} else {
			BuIndent:= {Start: 0.5, Right: 0, Offset: 0.25}
		}
		%RichText%.SetParaIndent(BuIndent)
		Spacing.After:= 0
		Spacing.Before:= 0
		%RichText%.SetParaSpacing(Spacing)
	}
	GuiControl, Focus, % %RichText%.HWND
Return

LaunchWebsite:
	Gosub HelpBoxGUIClose
	Run www.masq.net
return

LaunchEmail:
	Gosub HelpBoxGUIClose
	Run mailto:maasq@outlook.com
return

LaunchDiscord:
	Gosub HelpBoxGUIClose
	Run https://discord.gg/uH8NJ4x
return

LaunchGithub:
	Gosub HelpBoxGUIClose
	Run https://github.com/Maasq/NPC-Engineer/issues
return

LaunchPayPal:
	Run https://paypal.me/Maasq
return

ReferMePlease:				;{
	WhoReferredMe:= ""
	WhereToGo:= ""
	BackWeGo:= ""
	stringreplace, WhereToGo, A_Thismenuitem, %A_Space%, , All
	
	WhoReferredMe:= CheckWin()
	
	If (WhereToGo = "NPCEngineer") {
		BackWeGo:= "NPCE_Main"
	} Else 	If (WhereToGo = "SpellEngineer") {
		BackWeGo:= "SPE_Main"
	} Else 	If (WhereToGo = "EquipmentEngineer") {
		BackWeGo:= "EQE_Main"
	} Else 	If (WhereToGo = "ArtefactEngineer") {
		BackWeGo:= "ART_Main"
	} Else 	If (WhereToGo = "TableEngineer") {
		BackWeGo:= "TAB_Main"
	}

	If (Win[WhereToGo] = 1) {
		Gui, %WhoReferredMe%:hide
		Gui, %BackWeGo%:show
		Return
	}
	
	If (WhoReferredMe AND WhereToGo) {
		Gui, %WhoReferredMe%:hide
		%WhereToGo%(WhoReferredMe)
	}

Return						;}

NPCE_Update:				;{
	Checkversion()
	If UpdateMe {
		UpdateMe()
	} Else {
		MsgBox, 64, Engineer Suite Updater, You are already running the most up-to-date version.
	}
return						;}

NPCE_Settings:				;{
	;~ Gui, NPCE_Main:+disabled
	GUI_Options()
return						;}

DataPaths:					;{
	GUI, NPCE_Options:submit, NoHide
	If(SubStr(A_GuiControl, -1) = "01") {
		DataDir:= Get_A_Path(DataDir, "main data")
		GuiControl, NPCE_Options:, DataDir, %DataDir%
	}
	If(SubStr(A_GuiControl, -1) = "02") {
		ProjPath:= Get_A_Path(ProjPath, "project data")
		GuiControl, NPCE_Options:, ProjPath, %ProjPath%
	}
	If(SubStr(A_GuiControl, -1) = "03") {
		NPCPath:= Get_A_Path(NPCPath, "NPC save")
		GuiControl, NPCE_Options:, NPCPath, %NPCPath%
	}
	If(SubStr(A_GuiControl, -1) = "04") {
		SpellPath:= Get_A_Path(SpellPath, "spell save")
		GuiControl, NPCE_Options:, SpellPath, %SpellPath%
	}
	If(SubStr(A_GuiControl, -1) = "05") {
		EquipPath:= Get_A_Path(EquipPath, "equipment save")
		GuiControl, NPCE_Options:, EquipPath, %EquipPath%
	}
	If(SubStr(A_GuiControl, -1) = "06") {
		TablePath:= Get_A_Path(TablePath, "table save")
		GuiControl, NPCE_Options:, TablePath, %TablePath%
	}
	If(SubStr(A_GuiControl, -1) = "07") {
		ParcelPath:= Get_A_Path(ParcelPath, "parcel save")
		GuiControl, NPCE_Options:, ParcelPath, %ParcelPath%
	}
	If(SubStr(A_GuiControl, -1) = "08") {
		ArtePath:= Get_A_Path(ArtePath, "artefact save")
		GuiControl, NPCE_Options:, ArtePath, %ArtePath%
	}
return						;}

Reset_DataPath:				;{
	If(SubStr(A_GuiControl, -1) = "01") {
		DataDir:= A_Appdata "\NPC Engineer"
		GuiControl, NPCE_Options:, DataDir, %DataDir%
	}
	If(SubStr(A_GuiControl, -1) = "02") {
		ProjPath:= A_Appdata "\NPC Engineer\Saved Project Files"
		GuiControl, NPCE_Options:, ProjPath, %ProjPath%
	}
	If(SubStr(A_GuiControl, -1) = "03") {
		NPCPath:= A_Appdata "\NPC Engineer\Saved NPC Files"
		GuiControl, NPCE_Options:, NPCPath, %NPCPath%
	}
	If(SubStr(A_GuiControl, -1) = "04") {
		SpellPath:= A_Appdata "\NPC Engineer\Saved Spell Files"
		GuiControl, NPCE_Options:, SpellPath, %SpellPath%
	}
	If(SubStr(A_GuiControl, -1) = "05") {
		EquipPath:= A_Appdata "\NPC Engineer\Saved Equipment Files"
		GuiControl, NPCE_Options:, EquipPath, %EquipPath%
	}
	If(SubStr(A_GuiControl, -1) = "06") {
		TablePath:= A_Appdata "\NPC Engineer\Saved Table Files"
		GuiControl, NPCE_Options:, TablePath, %TablePath%
	}
	If(SubStr(A_GuiControl, -1) = "07") {
		ParcelPath:= A_Appdata "\NPC Engineer\Saved Parcel Files"
		GuiControl, NPCE_Options:, ParcelPath, %ParcelPath%
	}
	If(SubStr(A_GuiControl, -1) = "08") {
		ArtePath:= A_Appdata "\NPC Engineer\Saved Artefact Files"
		GuiControl, NPCE_Options:, ArtePath, %ArtePath%
	}
return						;}



GUIlaunch:					;{
	GUI, NPCE_Options:submit, NoHide
	GuiControl, NPCE_Options:Enable%LaunchGUI%, DefaultModule
return						;}

HelpBoxGUIClose:			;{
	Gui, %ReturnTo%:-disabled
	Gui, %ReturnTo%_About:Destroy
return						;}

BuildLinkGUI_Accept:		;{
	temp1:= Gget(LHwnd.BLObject)
	temp3:= Gget(LHwnd.BLDispName)
	temp4:= LHwnd.type
	Gui, %LinkRefer%:-disabled
	Gui, BuildLinkGUI:Destroy
	Gui, %LinkRefer%:Show
	LinkInserter2(temp4, temp1, temp3)
	temp1:= ""
	temp2:= ""
	temp3:= ""
	temp4:= ""
return						;}

BuildLinkGUI_Cancel:
BuildLinkGuiClose:			;{
	Gui, %LinkRefer%:-disabled
	Gui, BuildLinkGUI:Destroy
return						;}

BuildZLinkGUI_Accept:		;{
	temp1:= Gget(LHwnd.BZLObject)
	temp2:= Gget(LHwnd.BZLModule)
	temp3:= Gget(LHwnd.BZLDispName)
	temp4:= LHwnd.type
	Gui, %LinkRefer%:-disabled
	Gui, BuildZLinkGUI:Destroy
	Gui, %LinkRefer%:Show
	ZLinkInserter(temp4, temp1, temp2, temp3)
	temp1:= ""
	temp2:= ""
	temp3:= ""
	temp4:= ""
return						;}

BuildZLinkGUI_Cancel:
BuildZLinkGUIClose:			;{
	Gui, %LinkRefer%:-disabled
	Gui, BuildZLinkGUI:Destroy
return						;}

ValXMLGuiClose:
ValXML_Close:				;{
	Gui, ValXML:submit, NoHide
	Gui, ValXML:Destroy
Return						;}

QuickstartGuiClose:
Quickstart_Close:			;{
	Gui, Quickstart:Destroy
Return						;}

LinkGuideGuiClose:
LinkGuide_Close:			;{
	Gui, LinkGuide:Destroy
Return						;}

AddNotesGuiClose:
AddNotes_Close:				;{
	Gui, AddNotes:Destroy
Return						;}

AddNotes_Return:			;{
	temp:= Validate(ANT.GetRTF(False))
	temp:= Tokenise(temp)
	temp:= compactSpaces(temp)
	StringReplace, temp, temp, <p></p>, , All
	StringReplace, temp, temp, `r`n`r`n, `r`n, All
	%RTFNotesWindow%.notes:= RegexReplace(temp, "^\s+|\s+$" )
	%RTFNotesWindow%.notesRTF:= ANT.GetRTF(False)
	%RTFRHbox%_RH_Box()
	Gui, AddNotes:Destroy
	temp:= ""
Return						;}

CategDelete:				;{
	GUI, NPCE_Categories:submit, NoHide
	If CategoryList AND (CategoryList != Modname){
		CAT.delete(CategoryList)
		CAT.save(true)
		
		temp:= "|"
		For a, b in CAT.object()
		{
			temp:= temp CAT[a].name "|"
		}
		GuiControl, NPCE_Categories:, CategoryList, %temp%
		GuiControl, NPCE_Categories:, NewCategory, 
		stringreplace temp, temp, %modname%, %modname%|,
		if NPCE_Main {
			GuiControl, , %NPCFGcat%, %temp%
		}
		if TAB_Main {
			GuiControl, , % TAB_Hwnd.FGcat, %temp%
		}
		if SPE_Main {
			GuiControl, , % GHwnd.FGcat, %temp%
		}
		if EQE_Main {
			GuiControl, , % EQE_Hwnd.FGcat, %temp%
		}
	}
return						;}

CategAdd:					;{
	GUI, NPCE_Categories:submit, NoHide
	
	JSON_act_Name:= ""
	For a, b in CAT.object()
	{
		if (a == NewCategory) {
			JSON_act_Name:= a
		}
	}
	If !JSON_act_Name {
		Armoury:= {}
		Armoury[NewCategory]:= {}
		Armoury[NewCategory].Name:= Trim(NewCategory)
		CAT.fill(Armoury)
		CAT.save(true)
		
		temp:= "|"
		For a, b in CAT.object()
		{
			temp:= temp CAT[a].name "|"
		}
		GuiControl, NPCE_Categories:, CategoryList, %temp%
		GuiControl, NPCE_Categories:, NewCategory, 
		stringreplace temp, temp, %modname%, %modname%|,
		if NPCE_Main {
			GuiControl, , %NPCFGcat%, %temp%
		}
		if TAB_Main {
			GuiControl, , % TAB_Hwnd.FGcat, %temp%
		}
		if SPE_Main {
			GuiControl, , % GHwnd.FGcat, %temp%
		}
		if EQE_Main {
			GuiControl, , % EQE_Hwnd.FGcat, %temp%
		}
	}
return						;}

NPCE_Categories_Close:
NPCE_CategoriesGuiClose:	;{
	Gui, %gowner%:-disabled
	Gui, NPCE_Categories:Destroy
return						;}



;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                    Common                    |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


SetWinCheck(){
	global
	win:=[]
	win.npcengineer:= 0
	win.spellengineer:= 0
	win.equipmentengineer:= 0
	win.artefactengineer:= 0
	win.tableengineer:= 0
	win.parcelengineer:= 0
}


GetCoords(handle, part){
	Global ImpG
	LocalX:= part . "X"
	LocalY:= part . "Y"
	WinGetPos, guiX, guiY, , , ahk_id %handle%
	if (GuiX < 0) {
		GuiX:= -1
	}
	if (GuiY < 0) {
		GuiY:= -1
	}
	ImpG[LocalX]:= guiX
	ImpG[LocalY]:= guiY
	
	File:= A_AppData "\NPC Engineer\NPC Engineer.ini"
	Stub:= "Coordinates"
	IniWrite, %guiX%, %File%, %Stub%, %LocalX%
	IniWrite, %guiY%, %File%, %Stub%, %LocalY%
}


LinkSubCreate() {
	Global
	local countit
	
	If menuran {
		menu, linksub, deleteall
	}

	Countit:= 0
	Loop, Files, %ModPath%input\images\*.jpg, F
	{
		tempy:=  A_LoopFileName
		Menu, LinkSubCImages, Add, %tempy%, ProjectLink
		countit:= 1
	}
	if !Countit
		Menu, LinkSubCImages, Add
	Countit:= 0
	
	For a, b in npc.object()
	{
		tempy:= NPC[a].name
		Menu, LinkSubCNPCs, Add, %tempy%, ProjectLink
		countit:= 1
	}
	if !Countit
		Menu, LinkSubCNPCs, Add
	Countit:= 0
	
	For a, b in spl.object()
	{
		tempy:= SPL[a].name
		Menu, LinkSubCSpells, Add, %tempy%, ProjectLink
		countit:= 1
	}
	if !Countit
		Menu, LinkSubCSpells, Add
	Countit:= 0
	
	For a, b in tbl.object()
	{
		tempy:= TBL[a].name
		Menu, LinkSubCTables, Add, %tempy%, ProjectLink
		countit:= 1
	}
	if !Countit
		Menu, LinkSubCTables, Add
	Countit:= 0

	
	Menu, LinkSubA, Add, Image, LinkManager
	Menu, LinkSubA, Add, NPC, LinkManager
	Menu, LinkSubA, Add, Spell, LinkManager
	Menu, LinkSubA, Add, Table, LinkManager
	Menu, LinkSubA, Add, Parcel, LinkManager
	Menu, LinkSubA, Add, Item, LinkManager
	
	Menu, LinkSubB, Add, Image, ZLinkManager
	Menu, LinkSubB, Add, NPC, ZLinkManager
	Menu, LinkSubB, Add, Spell, ZLinkManager
	Menu, LinkSubB, Add, Table, ZLinkManager
	Menu, LinkSubB, Add, Parcel, ZLinkManager
	Menu, LinkSubB, Add, Item, ZLinkManager
	
	Menu, LinkSubC, Add, Images, :LinkSubCImages
	Menu, LinkSubC, Add, NPCs, :LinkSubCNPCs
	Menu, LinkSubC, Add, Spells, :LinkSubCSpells
	Menu, LinkSubC, Add, Tables, :LinkSubCTables
	Menu, LinkSubC, Add, Parcels, :LinkSubCNPCs
	Menu, LinkSubC, Disable, Parcels
	Menu, LinkSubC, Add, Items, :LinkSubCNPCs
	Menu, LinkSubC, Disable, Items

	Menu LinkSub, Add, Select link within this Project, :LinkSubC
	Menu LinkSub, Add, 
	Menu LinkSub, Add, Build internal link, :LinkSubA
	Menu LinkSub, Add, Build external link, :LinkSubB
	
	menuran:= 1
}

LinkManager(Nm, Ps, Mu) {
	global
	Local Drop, Chars
	chars:= StrLen(Nm) + 8
	LinkRefer:= CheckWin()
	If linkGUION {
		Gui, %LinkRefer%:+disabled
		BuildLinkGUI(modname, Nm)
		Gui, BuildLinkGUI:Show, w320 h144, Build a link to an object in this module
	} else {
		Drop:= "[link]" Nm ": object@" modname " - displaytext[/link]\Par"
	}
	RichEditChoose()
	caret:= %RichText%.GetSel()
	caret.S += chars
	caret.E += chars +6
	WinClip.Clear()
	WinClip.SetRTF("{\rtf " . Drop . "}")
	WinClip.Paste()
	%RichText%.SetSel(caret.S, caret.E)
}

ZLinkManager(Nm, Ps, Mu) {
	global
	Local Drop, Chars
	LinkRefer:= CheckWin()
	If linkGUION {
		Gui, %LinkRefer%:+disabled
		BuildZLinkGUI(Nm)
		Gui, BuildZLinkGUI:Show, w320 h191, Build a link to an object in any module
	} else {
		Drop:= "[zlink]" Nm ": object@modulename - displaytext[/zlink]\Par"
	}
	RichEditChoose()
	caret:= %RichText%.GetSel()
	caret.S += StrLen(Nm) +8
	caret.E += StrLen(Nm) +14
	WinClip.Clear()
	WinClip.SetRTF("{\rtf " . Drop . "}")
	WinClip.Paste()
	%RichText%.SetSel(caret.S, caret.E)
}

LinkInserter(Type, object) {
	global
	Local Drop, displaytext
	
	If (type = "images") {
		displaytext:= object
		stringreplace object, object, .jpg,
		type:= "Image"
	}
	If (type = "NPCs") {
		displaytext:= object
		type:= "NPC"
		For a, b in npc.object()
		{
			if (NPC[a].name = object) {
				object:= a
				break
			}
		}
	}
	If (type = "Spells") {
		displaytext:= object
		type:= "Spell"
		For a, b in spl.object()
		{
			if (spl[a].name = object) {
				object:= a
				break
			}
		}
	}
	If (type = "Tables") {
		displaytext:= object
		type:= "Table"
		For a, b in tbl.object()
		{
			if (tbl[a].name = object) {
				object:= a
				break
			}
		}
	}
	
	Drop:= "[link]" type ": " object "@" Modname " - " displaytext "[/link]\Par" 
	
	RichEditChoose()
	caret:= %RichText%.GetSel()
	caret.S += StrLen(Drop) - 3
	caret.E += StrLen(Drop) - 3
	WinClip.Clear()
	WinClip.SetRTF("{\rtf " . Drop . "}")
	WinClip.Paste()
	%RichText%.SetSel(caret.S, caret.E)
}

LinkInserter2(Type, object, displaytext) {
	Local Drop
	
	Drop:= "[link]" type ": " object "@" Modname " - " displaytext "[/link]\Par" 
	
	RichEditChoose()
	caret:= %RichText%.GetSel()
	caret.S += StrLen(Drop) - 4
	caret.E += StrLen(Drop) - 4
	WinClip.Clear()
	WinClip.SetRTF("{\rtf " . Drop . "}")
	WinClip.Paste()
	%RichText%.SetSel(caret.S, caret.E)
}

ZLinkInserter(Type, object, module, displaytext) {
	Local Drop
	
	Drop:= "[zlink]" type ": " object "@" module " - " displaytext "[/zlink]\Par" 
	
	RichEditChoose()
	caret:= %RichText%.GetSel()
	caret.S += StrLen(Drop) - 4
	caret.E += StrLen(Drop) - 4
	WinClip.Clear()
	WinClip.SetRTF("{\rtf " . Drop . "}")
	WinClip.Paste()
	%RichText%.SetSel(caret.S, caret.E)
}

ProjectLink(Nm, Ps, Mu) {
	Global
	LinkRefer:= CheckWin()
	If (ProjectLive != 1) {
		MsgBox, 16, No Project, You must load a project *.ini`nbefore you can add objects from it., 3
		gosub, Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			StringReplace, Mu, Mu, LinkSubC,
			LinkInserter(Mu, Nm)
		} else {
			MsgBox, 16, Engineer Suite Parser only, This function can only be carried out whilst using the Engineer Suite Parser., 3
		}
	}
}

LinkHTML(lnk) {
	Local targe
	targe:= "<img src=""" imgdir("shield.png") """ align=""center"" alt=""Shield"">"
	lnk:= RegExReplace(lnk, "U)\[link](.*)?:(.*)? - (.*)?\[/link]", targe "<b>$1</b>: $3")
	lnk:= RegExReplace(lnk, "U)\[zlink](.*)?:(.*)? - (.*)?\[/zlink]", targe "<b>$1</b>: $3")
	Return lnk
}

LinkXML(lnk) {
	global
	stringreplace, lnk, lnk, `t`t`t`t`t<p>[link], <p>[link], all
	stringreplace, lnk, lnk, `t`t`t`t`t<p>[zlink], <p>[zlink], all
	
	lnk:= LinkRegex(lnk)

	lnk:= RegExReplace(lnk, "\<link class(.*)\</link\>", Chr(10) "`t`t`t`t`t`t<linklist>" Chr(10) "<link class$1</link>" Chr(10) "`t`t`t`t`t`t</linklist>" Chr(10))
	stringreplace, lnk, lnk, <link class, `t`t`t`t`t`t`t<link class, all
	stringreplace, lnk, lnk, <p></p>, , all
	stringreplace, lnk, lnk, </description>, %A_Tab%%A_Tab%%A_Tab%%A_Tab%%A_Tab%</description>
	
	Return lnk
}

LinkRegex(lnk) {
	global modname
	stringreplace, lnk, lnk, <p>[link], [link], all
	stringreplace, lnk, lnk, [/link]</p>, [/link], all
	stringreplace, lnk, lnk, <p>[zlink], [zlink], all
	stringreplace, lnk, lnk, [/zlink]</p>, [/zlink], all

	lnk:= RegExReplace(lnk, "U)\Q[link]\EImage: (.*)@(.*) - (.*)\Q[/link]\E", "<link class=""imagewindow"" recordname=""image.img_$1_jpg@$2""><b>Image:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[link]\ENPC: (.*)@(.*) - (.*)\Q[/link]\E", "<link class=""npc"" recordname=""reference.npcdata.$1@$2""><b>NPC:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[link]\ESpell: (.*)@(.*) - (.*)\Q[/link]\E", "<link class=""power"" recordname=""reference.spelldata.$1@$2""><b>Spell:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[link]\ETable: (.*)@(.*) - (.*)\Q[/link]\E", "<link class=""table"" recordname=""tables.$1@$2""><b>Table:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[link]\EItem: (.*)@(.*) - (.*)\Q[/link]\E", "<link class=""item"" recordname=""item.$1@$2""><b>Item:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[link]\EParcel: (.*)@(.*) - (.*)\Q[/link]\E", "<link class=""treasureparcel"" recordname=""treasureparcels.$1@$2""><b>Parcel:</b> $3</link>")

	needle:= "Ui)\Q<link\E(.*)@" modname "(.*)\Q</link>\E"
	lnk:= RegExReplace(lnk, needle, "<link$1$2</link>")

	lnk:= RegExReplace(lnk, "U)\Q[zlink]\EImage: (.*)@(.*) - (.*)\Q[/zlink]\E", "<link class=""imagewindow"" recordname=""image.img_$1_jpg@$2""><b>Image:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[zlink]\ENPC: (.*)@(.*) - (.*)\Q[/zlink]\E", "<link class=""npc"" recordname=""reference.npcdata.$1@$2""><b>NPC:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[zlink]\ESpell: (.*)@(.*) - (.*)\Q[/zlink]\E", "<link class=""power"" recordname=""reference.spelldata.$1@$2""><b>Spell:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[zlink]\ETable: (.*)@(.*) - (.*)\Q[/zlink]\E", "<link class=""table"" recordname=""tables.$1@$2""><b>Table:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[zlink]\EItem: (.*)@(.*) - (.*)\Q[/zlink]\E", "<link class=""item"" recordname=""item.$1@$2""><b>Item:</b> $3</link>")
	lnk:= RegExReplace(lnk, "U)\Q[zlink]\EParcel: (.*)@(.*) - (.*)\Q[/zlink]\E", "<link class=""treasureparcel"" recordname=""treasureparcels.$1@$2""><b>Parcel:</b> $3</link>")

	Return lnk
}

BuildLinkGUI(Nm, type) {
	global
	local tempy, work
	If !isobject(LHwnd)
		LHwnd:= {}
	LHwnd.type:= type
	
; Settings for BuildLink window (BuildLinkGUI)
	Gui, BuildLinkGUI:-SysMenu
	Gui, BuildLinkGUI:+hwndBuildLinkGUI
	Gui, BuildLinkGUI:Color, 0xE2E1E8
	Gui, BuildLinkGUI:font, S10 c000000, Arial
	Gui, BuildLinkGUI:margin, 5, 1
	
	work:= "Insert " type " XML name:"
	Gui, BuildLinkGUI:Add, Text, x10 y7 w250 h18, %work%
	work:= "Display name for the " type ":"
	Gui, BuildLinkGUI:Add, Text, x10 y52 w250 h18, %work%
	Gui, BuildLinkGUI:Add, Edit, x10 y25 w300 hwndTempy, 
		LHwnd.BLObject:= Tempy
	Gui, BuildLinkGUI:Add, Edit, x10 y70 w300 hwndTempy, 
		LHwnd.BLDispName:= Tempy
	Gui, BuildLinkGUI:Add, Button, x100 y104 w100 h30 +border gBuildLinkGUI_Cancel hwndTempy, Cancel
		LHwnd.BLcancel:= Tempy
	Gui, BuildLinkGUI:Add, Button, x210 y104 w100 h30 +border +Default gBuildLinkGUI_Accept hwndTempy, Accept
		LHwnd.BLaccept:= Tempy
}

BuildZLinkGUI(type) {
	global
	local tempy, work
	If !isobject(LHwnd)
		LHwnd:= {}
	LHwnd.type:= type
	
; Settings for BuildZLink window (BuildZLinkGUI)
	Gui, BuildZLinkGUI:-SysMenu
	Gui, BuildZLinkGUI:+hwndBuildZLinkGUI
	Gui, BuildZLinkGUI:Color, 0xE2E1E8
	Gui, BuildZLinkGUI:font, S10 c000000, Arial
	Gui, BuildZLinkGUI:margin, 5, 1
	
	
	work:= "Insert " type " XML name:"
	Gui, BuildZLinkGUI:Add, Text, x10 y7 w250 h18, %work%
	Gui, BuildZLinkGUI:Add, Text, x10 y52 w250 h18, Insert Module XML name:
	work:= "Display name for the " type ":"
	Gui, BuildZLinkGUI:Add, Text, x10 y99 w250 h18, %work%
	Gui, BuildZLinkGUI:Add, Edit, x10 y25 w300 hwndTempy, 
		LHwnd.BZLObject:= Tempy
	Gui, BuildZLinkGUI:Add, Edit, x10 y70 w300 hwndTempy, 
		LHwnd.BZLModule:= Tempy
	Gui, BuildZLinkGUI:Add, Edit, x10 y115 w300 hwndTempy, 
		LHwnd.BZLDispName:= Tempy
	Gui, BuildZLinkGUI:Add, Button, x100 y151 w100 h30 +border gBuildZLinkGUI_Cancel hwndTempy, Cancel
		LHwnd.BZLcancel:= Tempy
	Gui, BuildZLinkGUI:Add, Button, x210 y151 w100 h30 +border +Default gBuildZLinkGUI_Accept hwndTempy, Accept
		LHwnd.BZLaccept:= Tempy
}

ObjChoose() {
	global
	local hero
	hero:= Gget(LHwnd.OType)
	hero:= "|" . LHwnd[hero]
	GuiControl, , % LHwnd.OName, %hero%
}



Gget(hwn) {
	GuiControlGet, value, , %hwn%
Return value
}

Gset(hwn, nw) {
	typ:= GetClassName(hwn)
	If (typ = "ComboBox")
		GuiControl, text, %hwn%, %nw%
	Else
		GuiControl, , %hwn%, %nw%
}

Tokenise(zx) {
	zx:= RegExReplace(zx, "(\r\n)+", Chr(13) Chr(10))
	zx:= RegExReplace(zx,"Us)\\pard\\li\d{4}\\ri\d{4}\\sb160\\sa160\\qj (.*)\\pard\\sa80","<p><frame>$1</frame></p>" Chr(10) "<p>")
	zx:= RegExReplace(zx,"Us)\\pard\\li\d{4}\\ri\d{4}\\sb160\\sa160\\qj(.*)\\pard\\sa80","<p><frame>$1</frame></p>" Chr(10) "<p>")
	zx:= RegExReplace(zx,"Us)\\pard\\li\d{4}\\ri\d{4}\\sb160\\sa160\\qj(.*)}","<p><frame>$1</frame></p>")
	zx:= RegExReplace(zx, "U)\\pard\{\\pntext.*\\'B7\\tab}\{\\\*\\pn\\pnlvlblt\\pnf\d\\pnindent0\{\\pntxtb\\'B7}}\\fi-\d+\\li\d+?(.*)\\par\r\n", "<ul><li>$1</li>")
	zx:= RegExReplace(zx, "U)\{\\pntext.*\\'B7\\tab}(.*)\\par\r\n", "<li>$1</li>")
	zx:= RegExReplace(zx, "Us)<ul><li>(.*)\\pard\\sa80", "<ul><li>$1</ul>" Chr(13) Chr(10) "<p>")
	zx:= RegExReplace(zx, "U)</li>(\r\n)*}", "</li></ul>")
	zx:= RegExReplace(zx, "\\f\d ", "")
	zx:= RegExReplace(zx, "\\f\d", "")
	zx:= RegExReplace(zx, "U)\{\\rtf.*Arial;}{.*?}}\r\n", "")
	zx:= RegExReplace(zx, "U)\{\\rtf.*Arial;}}\r\n", "")
	zx:= RegExReplace(zx, "U)\{\\colortbl.*}\r\n", "")
	zx:= RegExReplace(zx, "U)\{\\.*uc1 \r\n", "")
	zx:= RegExReplace(zx, "U)\{\\.*uc1\\", "\")
	
	FoundPos:= RegExMatch(zx, "is)\\trowd.*\\row", rtftable)
	If rtftable {
		StringReplace zx, zx, %rtftable%, <--TABDATA-->
		rtftable:= RegExReplace(rtftable, "Us)\r\n", "")
		rtftable:= RegExReplace(rtftable, "Us)\\trgaph.*\\", "\")
		rtftable:= RegExReplace(rtftable, "s)\\lang\d*", "")
		rtftable:= RegExReplace(rtftable, "s)\\fs\d*", "")
		rtftable:= RegExReplace(rtftable, "s)\\trrh\d*", "")
		rtftable:= RegExReplace(rtftable, "s)\\cellx\d*", "")
		rtftable:= RegExReplace(rtftable, "s)\\clvertalc", "")
		rtftable:= RegExReplace(rtftable, "s)\\brdrw\d*", "")
		rtftable:= RegExReplace(rtftable, "s)\\clbrdr\w\d*", "")
		rtftable:= RegExReplace(rtftable, "s)\\brdrs", "")
		rtftable:= RegExReplace(rtftable, "s)\\clcbpat\d*", "")
		rtftable:= RegExReplace(rtftable, "s)\\trpadd\w*\d*", "")
		rtftable:= RegExReplace(rtftable, "Us)\\pard\\intbl(.*)\\cell", "<td>$1</td>"  Chr(13) Chr(10) "<K>")
		rtftable:= RegExReplace(rtftable, "Us)\\trowd(.*)\\row", "<tr>$1</tr>" Chr(13) Chr(10))
		rtftable:= RegExReplace(rtftable, "Us)<tr>\s*<td>", "<tr>" Chr(13) Chr(10) "<td>")
		rtftable:= RegExReplace(rtftable, "s)\\qc", "")
		rtftable:= RegExReplace(rtftable, "s)\\cf\d*", "")
		rtftable:= RegExReplace(rtftable, "s)\\b0", "")
		rtftable:= RegExReplace(rtftable, "s)\\b", "")
		loop, 19 {
			rtftable:= RegExReplace(rtftable, "U)<K>(.*)\\cell", "<td>$1</td>"  Chr(13) Chr(10) "<K>")
		}
		rtftable:= RegExReplace(rtftable, "<K>", "")
		rtftable:= RegExReplace(rtftable, "s)gdkhorizpat\d*\\shading\d*", "")
		rtftable:= RegExReplace(rtftable, "> *", ">")
		rtftable:= RegExReplace(rtftable, " *<", "<")
		rtftable:= RegExReplace(rtftable, "\\par", "</p>" Chr(10) "<p>")
		StringReplace zx, zx, <--TABDATA-->, %rtftable%
	}
	
	zx:= RegExReplace(zx, "Us)\\pard\\bgd.*\\par", "")
	
	zx:= RegExReplace(zx,"\s*$","")
	zx:= RegExReplace(zx,"\\pard","<p>")
	zx:= RegExReplace(zx,"\\par\r\n}","</p>")
	zx:= RegExReplace(zx,"\\par\r\n","</p>" Chr(10) "<p>")
	zx:= RegExReplace(zx,"\\i\\fs20 ","\fs20<i>")
	zx:= RegExReplace(zx,"\\b\\fs20 ","\fs20<b>")
	zx:= RegExReplace(zx,"\\ul\\fs20 ","\fs20<u>")
	zx:= RegExReplace(zx,"\\ulnone ?","</u>")
	zx:= RegExReplace(zx,"\\b0 ?","</b>")
	zx:= RegExReplace(zx,"\\i0 ?","</i>")
	zx:= RegExReplace(zx,"\\i ?","<i>")
	zx:= RegExReplace(zx,"\\b ?","<b>")
	zx:= RegExReplace(zx,"\\ul ?","<u>")
	stringreplace, zx, zx, </u></b></i>, </i></b></u>, All
	stringreplace, zx, zx, </u></i>, </i></u>, All
	stringreplace, zx, zx, </b></i>, </i></b>, All
	stringreplace, zx, zx, </u></b>, </b></u>, All



	zx:= RegExReplace(zx,"\\lang\d+","")
	zx:= RegExReplace(zx,"s)\\cf\d\\fs32 (.*?)\\cf\d(\\f1)?\\fs20 ?","<h>$1</h>")
	zx:= RegExReplace(zx,"s)\\cf\d\\fs32 (.*?)\\par","<h>$1</h>")
	zx:= RegExReplace(zx,"\\sa80\\cf\d\\fs20 ?","")
	zx:= RegExReplace(zx,"\\cf\d ?","")
	zx:= RegExReplace(zx,"\\fs20 ?","")
	zx:= RegExReplace(zx,"\\sa80 ?","")
	zx:= RegExReplace(zx,"\\rquote ?","'")
	zx:= RegExReplace(zx,"\\endash ", Chr(8211))
	zx:= RegExReplace(zx,"\\emdash ", Chr(8212))
	;~ zx:= StrReplace(zx, "</p>`n<p></frame></p>", "</frame></p>")
	zx:= StrReplace(zx, "</p>`n<p><ul><li></h>", "</h></p>" Chr(10) "<p><ul><li>")
	zx:= StrReplace(zx, "</p>`n<p></h>", "</h></p>" Chr(10) "<p>")
	zx:= StrReplace(zx, "<p><p>", "<p>")
	zx:= StrReplace(zx, "<p><ul>", "<ul>")
	zx:= StrReplace(zx, "\'c0", "À")
	zx:= StrReplace(zx, "\'c1", "Á")
	zx:= StrReplace(zx, "\'c2", "Â")
	zx:= StrReplace(zx, "\'c3", "Ã")
	zx:= StrReplace(zx, "\'c4", "Ä")
	zx:= StrReplace(zx, "\'c5", "Å")
	zx:= StrReplace(zx, "\'c6", "Æ")
	zx:= StrReplace(zx, "\'c7", "Ç")
	zx:= StrReplace(zx, "\'c8", "È")
	zx:= StrReplace(zx, "\'c9", "É")
	zx:= StrReplace(zx, "\'ca", "Ê")
	zx:= StrReplace(zx, "\'cb", "Ë")
	zx:= StrReplace(zx, "\'cc", "Ì")
	zx:= StrReplace(zx, "\'cd", "Í")
	zx:= StrReplace(zx, "\'ce", "Î")
	zx:= StrReplace(zx, "\'cf", "Ï")
	zx:= StrReplace(zx, "\'d0", "Ð")
	zx:= StrReplace(zx, "\'d1", "Ñ")
	zx:= StrReplace(zx, "\'d2", "Ò")
	zx:= StrReplace(zx, "\'d3", "Ó")
	zx:= StrReplace(zx, "\'d4", "Ô")
	zx:= StrReplace(zx, "\'d5", "Õ")
	zx:= StrReplace(zx, "\'d6", "Ö")
	zx:= StrReplace(zx, "\'d8", "Ø")
	zx:= StrReplace(zx, "\'d9", "Ù")
	zx:= StrReplace(zx, "\'da", "Ú")
	zx:= StrReplace(zx, "\'db", "Û")
	zx:= StrReplace(zx, "\'dc", "Ü")
	zx:= StrReplace(zx, "\'dd", "Ý")
	zx:= StrReplace(zx, "\'de", "Þ")
	zx:= StrReplace(zx, "\'df", "ß")
	zx:= StrReplace(zx, "\'e0", "à")
	zx:= StrReplace(zx, "\'e1", "á")
	zx:= StrReplace(zx, "\'e2", "â")
	zx:= StrReplace(zx, "\'e3", "ã")
	zx:= StrReplace(zx, "\'e4", "ä")
	zx:= StrReplace(zx, "\'e5", "å")
	zx:= StrReplace(zx, "\'e6", "æ")
	zx:= StrReplace(zx, "\'e7", "ç")
	zx:= StrReplace(zx, "\'e8", "è")
	zx:= StrReplace(zx, "\'e9", "é")
	zx:= StrReplace(zx, "\'ea", "ê")
	zx:= StrReplace(zx, "\'eb", "ë")
	zx:= StrReplace(zx, "\'ec", "ì")
	zx:= StrReplace(zx, "\'ed", "í")
	zx:= StrReplace(zx, "\'ee", "î")
	zx:= StrReplace(zx, "\'ef", "ï")
	zx:= StrReplace(zx, "\'f0", "ð")
	zx:= StrReplace(zx, "\'f1", "ñ")
	zx:= StrReplace(zx, "\'f2", "ò")
	zx:= StrReplace(zx, "\'f3", "ó")
	zx:= StrReplace(zx, "\'f4", "ô")
	zx:= StrReplace(zx, "\'f5", "õ")
	zx:= StrReplace(zx, "\'f6", "ö")
	zx:= StrReplace(zx, "\'f8", "ø")
	zx:= StrReplace(zx, "\'f9", "ù")
	zx:= StrReplace(zx, "\'fa", "ú")
	zx:= StrReplace(zx, "\'fb", "û")
	zx:= StrReplace(zx, "\'fc", "ü")
	zx:= StrReplace(zx, "\'fd", "ý")
	zx:= StrReplace(zx, "\'fe", "þ")
	zx:= StrReplace(zx, "\'ff", "ÿ")
	zx:= RegExReplace(zx, "(\r\n)+", Chr(13) Chr(10))
	zx:= RegExReplace(zx, "}$", "")
	zx:= StrReplace(zx, "<p></p>", "")
	Return zx
}

RTFise(zx) {
	If (SubStr(zx, 1, 3) == "<p>")
		zx:= SubStr(zx, 4)
	zx:= RegExReplace(zx,"<i>","\i ")
	zx:= RegExReplace(zx,"<b>","\b ")
	zx:= RegExReplace(zx,"<u>","\ul ")
	zx:= RegExReplace(zx,"</i>","\i0 ")
	zx:= RegExReplace(zx,"</b>","\b0 ")
	zx:= RegExReplace(zx,"</u>","\ulnone ")
	zx:= RegExReplace(zx,"</p>\n<p>","\par" Chr(13) Chr(10))
	zx:= RegExReplace(zx,"</p>","\par" Chr(13) Chr(10))
	
	zx:= RegExReplace(zx,"Us)<h>(.*)</h>","\cf1\fs32 $1\cf2\fs20")
	zx:= RegExReplace(zx,"\<p\>\<frame\>","\pard\li800\ri800\sb80\sa160\qj\f2\cf1")
	zx:= RegExReplace(zx,"\<frame\>","\pard\li800\ri800\sb80\sa160\qj\f2\cf1")
	zx:= RegExReplace(zx,"\</frame\>\\par","\pard\sa80\f0\cf2\li0\ri0")
	;~ zx:= RegExReplace(zx,"<p>","\pard\li0\ri0")
	;~ zx:= RegExReplace(zx,"\\f0","")
	;~ zx:= RegExReplace(zx,"\\sa120\\cf1\\fs20 ?","")
	;~ zx:= RegExReplace(zx,"\\f1","")
	;~ zx:= RegExReplace(zx,"\\rquote ?","'")
Return zx
}

Textise(zx) {
	If (SubStr(zx, 1, 3) == "<p>")
		zx:= SubStr(zx, 4)
	;~ zx:= RegExReplace(zx,"<i>","")
	;~ zx:= RegExReplace(zx,"<b>","")
	;~ zx:= RegExReplace(zx,"<u>","")
	;~ zx:= RegExReplace(zx,"</i>","")
	;~ zx:= RegExReplace(zx,"</b>","")
	;~ zx:= RegExReplace(zx,"</u>","")
	zx:= RegExReplace(zx,"Us)<h>(.*)</h>","#h;$1")

	stringreplace, zx, zx, <frame>, #zfs`;`r`n#zft`;, All
	stringreplace, zx, zx, </frame>, `r`n#zfe`;, All

	stringreplace, zx, zx, <ul>, #ls`;`r`n, All
	stringreplace, zx, zx, </ul>, #le`;`r`n, All

	zx:= RegExReplace(zx,"Us)<li>(.*)</li>","#li;$1" Chr(13) Chr(10))

	zx:= RegExReplace(zx,"</p>\n<p>","" Chr(13) Chr(10))
	zx:= RegExReplace(zx,"</p>","" Chr(13) Chr(10))
	stringreplace, zx, zx, <p>, , All
	
	stringreplace, zx, zx, `r`n`r`n, `r`n, All


Return zx
}

Paste(value) {
	Gosub paste
}

De_PDF() {
	Window:= WinExist("A")
	GuiControlGet, var, %Window%:FocusV
	GuiControlGet, ControlHWND, %Window%:Hwnd, % var
	cclass:= GetClassName(ControlHWND)
	If (cclass == "Edit") {
		ControlGetText, ControlText, , ahk_id %ControlHwnd%
		Format_Chunk(ControlText)
		ControlSetText, ,%ControlText%, ahk_id %ControlHwnd%
	}
}

De_PDF2() {
	Global ant
	Window:= WinExist("A")
	GuiControlGet, var, %Window%:Focus
	GuiControlGet, ControlHWND, %Window%:Hwnd, % var
	If (ControlHWND == ANT.HWND) {
		ControlGetText, ControlText, , ahk_id %ControlHwnd%
		Format_Chunk(ControlText)
		ControlSetText, ,%ControlText%, ahk_id %ControlHwnd%
	}
}

Format_Chunk(ByRef StripLF) {
; Tag paragraph breaks with a special combinations
	StringReplace StripLF, StripLF, .`r`n, -.-, All
	StringReplace StripLF, StripLF, :`r`n, -.-, All
	StringReplace StripLF, StripLF, `r`n`r`n, <-*->, All

	StringReplace StripLF, StripLF, .%A_Space%`r`n, -.-, All
	StringReplace StripLF, StripLF, :%A_Space%`r`n, -.-, All

; Replace a single newline with a space
	StringReplace StripLF, StripLF, %A_Space% `r`n, %A_Space%, All
	StringReplace StripLF, StripLF, `r`n, %A_Space%, All

; Replace multiple adjacent spaces with a single one
	StripLF := RegExReplace(StripLF, "\s+" , " ")

; Replace the paragraph break codes with newlines
	StringReplace StripLF, StripLF, -.-, .`r`n, All
	StringReplace StripLF, StripLF, <-*->, `r`n, All
	StringReplace StripLF, StripLF, `r`n%A_Space%, `r`n, All
}

GetClassName(hwnd) {
	VarSetCapacity( buff, 256, 0 )
	DllCall("GetClassName", "uint", hwnd, "str", buff, "int", 255 )
	return buff
}

compactSpaces(blurb) {
   blurb := RegExReplace(blurb, "S) +", A_Space)
   return blurb
}

startLogging(w:=300,h:=100,title:="Parse") {
	global
	Console:= ""
	If (logging!=1){
		logging:=1
		Firstline:= 0
		local innerW, innerH
		innerW:=w-20
		innerH:=h-40
		buttonx:= (w-100)/2
		buttony:= h-30
		title:= title " Log"
		Gui, PARE_Parse:+hwndPARE_Parse
		Gui, PARE_Parse:Color, 0xE2E1E8
		Gui, PARE_Parse:font, S10 c000000, Tahoma
		Gui, PARE_Parse:Add, Button, x%buttonx% y%buttony% w100 h20 +border vPARE_Parse_Cancel gPARE_Parse_Close, Close
		Gui, PARE_Parse:Add, Text, cBlack x10 y10 w%innerW% h20 vL1Text, 
		;~ RP1:= New RichEdit("PARE_Parse", "x10 y10 w" innerW " h" innerH " vConsole", True)
			;~ RP1.wordwrap(true)
			;~ RP1.ShowScrollBar(0, False)
			;~ RP1.SetBkgndColor("White")
			;~ PFont := RP1.GetFont()
			;~ PFont.name:= "Arial"
			;~ PFont.Color:= "black"
			;~ PFont.Size:= "10"
			;~ RP1.SetFont(PFont)

		Gui, PARE_Parse:Show, w%w% h%h%, %title%
		;~ GuiControl, Focus, % RP1.HWND
	}
}

log(msg, Xlev, msgcolor:="black", msgstyle:="N") {
	global
	;~ clipsave:= Clipboard
	;~ FormatTime, TimeString, A_Now, HH:mm:ss	; Generates a time stamp
	;~ If !Firstline {
		;~ Firstline:= 1
		;~ Timestring:= " " Timestring
	;~ }
	
	;~ PFont.Color:= "navy"
	;~ Pfont.Style:= "B"
	;~ RP1.SetFont(PFont)
	;~ Clipboard:= Timestring
	;~ Send ^v
	
	;~ FileLine:= ""
	;~ Loop, %XLev%
		;~ FileLine .= "   "

	;~ FileLine := "  " FileLine msg "`n " 
	
	;~ PFont.Color:= msgcolor
	;~ Pfont.Style:= msgstyle
	;~ RP1.SetFont(PFont)
	;~ Clipboard:= Fileline
	;~ Send ^v
	
	;~ Clipboard:= Clipsave
	;~ PFont.Color:= "black"
	;~ Pfont.Style:= "N"
	;~ RP1.SetFont(PFont)
	
	if (xlev = 0){
		GuiControl, PARE_Parse:, L1Text, %Msg%
	} 		
}

endLogging() {
	global
	Gui, PARE_Parse:Destroy
	logging:=""
}


Validate(Xinp) {
	StringReplace, Xinp, Xinp, &, &amp;, All
	StringReplace, Xinp, Xinp, <, &lt;, All
	StringReplace, Xinp, Xinp, >, &gt;, All
	StringReplace, Xinp, Xinp, `", &quot;, All
	;~ StringReplace, Xinp, Xinp, `', &apos;, All
	Return Xinp
}

WM_MOUSEMOVE() {
	static CurrControl, PrevControl, tttmr, CurrHwnd, nHwnd ; _TT is kept blank for use by the ToolTip command below.
	Global TTip, TTipTime, hTTip
	tttmr:= TTipTime * 1000
	If (Checkwin() == "NPCE_Main") OR (Checkwin() == "Eng_Suite"){
		CurrControl := A_GuiControl
		If (CurrControl <> PrevControl and not InStr(CurrControl, " ")) {
			ToolTip  ; Turn off any previous tooltip.
			SetTimer, DisplayToolTip, 200
			PrevControl := CurrControl
		}
	} Else {
		MouseGetPos,,,, nHwnd, 2
		If (CurrHwnd == nHwnd) {
			return
		}
		ToolTip	  ; Turn off any previous tooltip.
		SetTimer, DisplayToolTip2, 200
		CurrHwnd:= nHwnd
		return
	}
	return

	DisplayToolTip:
	SetTimer, DisplayToolTip, Off
	TempCurrControl:= RegExReplace(CurrControl, "[^a-zA-Z_0-9]", "")
	TTText:= TTip[TempCurrControl]
	ToolTip % TTip[TempCurrControl]
	SetTimer, RemoveToolTip, %tttmr%
	return

	RemoveToolTip:
	SetTimer, RemoveToolTip, Off
	ToolTip
	return
	
	DisplayToolTip2:
	SetTimer, DisplayToolTip2, Off
	ToolTip % hTTip[nHwnd]
	SetTimer, RemoveToolTip2, %tttmr%
	return

	RemoveToolTip2:
	SetTimer, RemoveToolTip2, Off
	ToolTip
	return
}


LC(var) {
	StringLower, var, var
	Return var
}

UC(var) {
	StringUpper, var, var
	Return var
}

TC(var) {
	StringLower, var, var, T
	Return var
}

XC(var) {
	StringLower, var, var
	var:= RegExReplace(var, "[^a-zA-Z0-9]", "")
	var:= RegExReplace(var, "^\d", "xx")
	
	Return var
}

Para_Chunk(ByRef zx) {
	zx:= RegExReplace(zx,"\r\n","</p>" Chr(10) "<p>") . "</p>"
	return zx
}

SetCursor(W, L, M, H) {
   Global CHAND, HTEXT1, HTEXT2, HTEXT3, HTEXT4
   Static OnCtrl:= -1
   If (W = HTEXT1) or (W = HTEXT2) or (W = HTEXT3) or (W = HTEXT4) {
      If (W <> OnCtrl) {
         DllCall("User32.dll\SetCursor", "Ptr", CHAND)
         OnCtrl:= W
      }
      Return True
   }
   OnCtrl:= W
}

Toast(Text, Title:= "NPC Engineer") {
	global
	IDtoast:= Checkwin()
	if toaston {
		Menu, Tray, Icon
		TrayTip, %Title%, %Text%,, 16
		SetTimer, RemoveToast, 2000
		return
	} else {
		SetTimer, RemoveSB, 2000
		Gui, %IDtoast%:Default
		SB_SetText("`t`t" text "  ", 3)
		return
	}

	RemoveSB:
	SetTimer, RemoveSB, Off
	Gui, %IDtoast%:Default
	SB_SetText(" ", 3)
	return
	
	RemoveToast:
	SetTimer, RemoveToast, Off
	Menu, Tray, NoIcon
	return
	
}

CheckVersion() {
	global ES_Release_Version, Cver, UpdateMe, datadir
	UrlDownloadToFile, http://www.masq.net/2017/11/npc-engineer-information.html, %DataDir%\temp.htm
	FileRead, version, %DataDir%\temp.htm
	UpdateMe:= 0
	foundpos:= RegExMatch(version, "Current release Version: (\d+\.\d+\.\d+)", Current_Version)
	FileDelete, %DataDir%\temp.htm

	If foundpos {
		Nr := 0
		For each, subNr in StrSplit(ES_Release_Version,".")
			Nr := Nr + SubNr * 1000**(3-each)
		Rver:= Nr
		StringReplace, Current_Version, Current_Version, Current release Version:%A_Space%, 
		Cver:= Current_Version
		Nr := 0
		For each, subNr in StrSplit(Current_Version,".")
			Nr := Nr + SubNr * 1000**(3-each)
		Current_Version:= Nr
		
		If (Current_Version > Rver) {
			UpdateMe:= 1
		}
		UDCdate:= A_NowUTC
		File:= A_Appdata "\NPC Engineer\NPC Engineer.ini"
		Stub:= "Update"
		IniWrite, %UDCdate%, %File%, %Stub%, UDCdate
	}
}

UpdateMe() {
	global Cver
	MsgBox, 36, Engineer Suite Updater, % "A new update is available: v" . Cver . "`nDo you want to go to the website to download it?"
	IfMsgBox Yes
		Run https://www.masq.net/2017/11/npc-engineer-information.html
}

Clipimp() {
	send ^v
	Clipimp:= Clipboard
	Clipboard:= ""
}

SplashImageGUI(Picture, X, Y, Duration, Transparent = false) {
	Gui, XPT99:Margin , 0, 0
	Gui, XPT99:Add, Picture,, %Picture%
	Gui, XPT99:Color, ECE9D8
	Gui, XPT99:+LastFound -Caption +AlwaysOnTop +ToolWindow -Border
	If Transparent {
		Winset, TransColor, ECE9D8
	}
	Gui, XPT99:Show, x%X% y%Y% NoActivate
	SetTimer, DestroySplashGUI, -%Duration%
	return

	DestroySplashGUI:
	Gui, XPT99:Destroy
	return
}

ImgDir(file) {
	gd:= A_ScriptDir "\gfx\" file
	StringReplace, gd, gd, \, /, all
	StringReplace, gd, gd, %A_Space%, `%20, all
	gd:= "file:///" gd
	return gd
}

RichEditChoose() {
	global
	RichText:= ""
	If (WinExist("A") = NPCE_Main) {
		RichText:= "RE1"
	}
	If (WinExist("A") = SPE_Main) {
		RichText:= "ST1"
	}
	If (WinExist("A") = EQE_Main) {
		RichText:= "ET1"
	}
	If (WinExist("A") = ART_Main) {
		RichText:= "AT1"
	}
	If (WinExist("A") = TAB_Main) {
		RichText:= "TT1"
	}
	If (WinExist("A") = AddNotes) {
		RichText:= "ANT"
	}
	If (A_GUI = "NPCE_Main") {
		RichText:= "RE1"
	}
	If (A_GUI = "SPE_Main") {
		RichText:= "ST1"
	}
	If (A_GUI = "EQE_Main") {
		RichText:= "ET1"
	}
	If (A_GUI = "ART_Main") {
		RichText:= "AT1"
	}
	If (A_GUI = "TAB_Main") {
		RichText:= "TT1"
	}
	If (A_GUI = "AddNotes") {
		RichText:= "ANT"
	}
}

CheckWin() {
	Local ID
	If (Winexist("A") = NPCE_Main) {
		ID:= "NPCE_Main"
	} Else 	If (Winexist("A") = SPE_Main) {
		ID:= "SPE_Main"
	} Else 	If (Winexist("A") = EQE_Main) {
		ID:= "EQE_Main"
	} Else 	If (Winexist("A") = ART_Main) {
		ID:= "ART_Main"
	} Else 	If (Winexist("A") = TAB_Main) {
		ID:= "TAB_Main"
	} Else 	If (Winexist("A") = NPCE_Project) {
		ID:= "NPCE_Project"
	} Else 	If (Winexist("A") = Eng_Suite) {
		ID:= "Eng_Suite"
	}
	Return ID
}

Quickstart() {
	global
	local innerW, innerH, outerW, outerH, buttonx, buttony, options, FilePath
	outerW:= 700
	outerH:= 450
	innerW:= outerW-20
	innerH:= outerH-60
	buttonx:= (outerW-100)/2
	buttony:= outerH-40
	
	Filepath:= A_ScriptDir "\Quickstart.rtf"
	
	ValXMLText:= ""
	options:= "x10 y10 w" innerW " h" innerH "vQuickstart"
	
	Gui, Quickstart:+hwndQuickstart
	Gui, Quickstart:Color, 0xE2E1E8
	Gui, Quickstart:font, S10 c000000, Tahoma
	Gui, Quickstart:Add, Button, x%buttonx% y%buttony% w100 h30 +border gQuickstart_Close Default, Close
	QST:= New RichEdit("Quickstart", options, True)
		QST.wordwrap(true)
		QST.ShowScrollBar(0, False)
		QST.SetBkgndColor("White")
		QSFont:= QST.GetFont()
		QSFont.name:= "Calibri"
		QSFont.Color:= "Black"
		QSFont.Size:= "10"
		QST.SetFont(QSFont)
		QST.SetOptions(["AUTOWORDSELECTION","AUTOVSCROLL","READONLY"])
		QST.LoadRTF(FilePath, False)
	Gui, Quickstart:Show, w%outerW% h%outerH%, Quickstart Guide

}

LinkGuide() {
	global
	local innerW, innerH, outerW, outerH, buttonx, buttony, options, FilePath
	outerW:= 700
	outerH:= 450
	innerW:= outerW-20
	innerH:= outerH-60
	buttonx:= (outerW-100)/2
	buttony:= outerH-40
	
	Filepath:= A_ScriptDir "\LinkGuide.rtf"
	
	ValXMLText:= ""
	options:= "x10 y10 w" innerW " h" innerH "vLinkGuide"
	
	Gui, LinkGuide:+hwndLinkGuide
	Gui, LinkGuide:Color, 0xE2E1E8
	Gui, LinkGuide:font, S10 c000000, Tahoma
	Gui, LinkGuide:Add, Button, x%buttonx% y%buttony% w100 h30 +border gLinkGuide_Close Default, Close
	QST:= New RichEdit("LinkGuide", options, True)
		QST.wordwrap(true)
		QST.ShowScrollBar(0, False)
		QST.SetBkgndColor("White")
		QSFont:= QST.GetFont()
		QSFont.name:= "Calibri"
		QSFont.Color:= "Black"
		QSFont.Size:= "10"
		QST.SetFont(QSFont)
		QST.SetOptions(["AUTOWORDSELECTION","AUTOVSCROLL","READONLY"])
		QST.LoadRTF(FilePath, False)
	Gui, LinkGuide:Show, w%outerW% h%outerH%, Help with links

}

AddNotes(txt, rt) {
	global
	local innerW, innerH, outerW, outerH, buttonx1, buttonx2, buttony, options, controlsy1, controlsy2

	if WinExist("ahk_id" AddNotes) {
		WinActivate
	} Else {
		RTFNotesWindow:= txt
		RTFRHbox:= rt
		RTFNotesContent:= %txt%.notesrtf
		if (RTFNotesContent = "") {
			RTFNotesContent:="{\rtf{\colortbl `;\red120\green34\blue32`;\red0\green0\blue0`;}{\fonttbl {\f0 Arial`;}}}"
		}
		outerW:= 600
		outerH:= 470
		innerW:= outerW-20
		innerH:= outerH-80
		buttonx1:= outerW-220
		buttonx2:= outerW-110
		buttony:= outerH-40
		controlsy1:= outerH-65
		controlsy2:= outerH-45
		
		options:= "x10 y10 w" innerW " h" innerH "vAddnotesText"
		
		Gui, AddNotes:+hwndAddNotes
		Gui, AddNotes:Color, 0xE2E1E8
		Gui, AddNotes:font, S10 c000000 norm, Arial
		ANT:= New RichEdit("AddNotes", options, True)
			ANT.wordwrap(true)
			ANT.ShowScrollBar(0, False)
			ANT.SetBkgndColor("White")
			ANTFont:= ANT.GetFont()
			ANTFont.name:= "Arial"
			ANTFont.Color:= "Black"
			ANTFont.Size:= "10"
			ANT.SetFont(ANTFont)
			Spacing:= []
			Spacing.After:= 4
			ANT.SetParaSpacing(Spacing)

		Gui, AddNotes:font, S10 c000000 norm bold, Arial
		Gui, AddNotes:Add, Button, x10 y%controlsy1% w20 h20 vBTTTB gSetFontStyle, B
		Gui, AddNotes:font, S10 c000000 norm italic, Arial
		Gui, AddNotes:Add, Button, x35 y%controlsy1% w20 h20 vBTTTI gSetFontStyle, I
		Gui, AddNotes:font, S10 c000000 norm underline, Arial
		Gui, AddNotes:Add, Button, x60 y%controlsy1% w20 h20 vBTTTU gSetFontStyle, U
		Gui, AddNotes:font, S10 c000000 norm, Arial

		Gui, AddNotes:font, S10 c000000 norm, Arial
		Gui, AddNotes:Add, Button, x10 y%controlsy2% w20 h20 vBTTTH gREHeader, H
		Gui, AddNotes:font, S10 c000000 norm, Arial
		Gui, AddNotes:Add, Button, x35 y%controlsy2% w20 h20 vBTTTa gREBody, T
		Gui, AddNotes:font, S12 c000000 norm, Arial
		Gui, AddNotes:Add, Button, x60 y%controlsy2% w20 h20 vBTTTL gREbullet, % Chr(8801)
		Gui, AddNotes:font, S10 c000000 norm, Arial

		Gui, AddNotes:Add, Button, x135 y%controlsy1% w20 h20 vBTTTZ gundo, % Chr(11148)
		Gui, AddNotes:Add, Button, x160 y%controlsy1% w20 h20 vBTTTY gredo, % Chr(11150)

		Gui, AddNotes:Add, Button, x%buttonx2% y%controlsy1% w100 h20 +border gValXML, Validate XML

		Gui, AddNotes:Add, Button, x%buttonx1% y%buttony% w100 h30 +border gAddNotes_Close Default, Cancel
		Gui, AddNotes:Add, Button, x%buttonx2% y%buttony% w100 h30 +border gAddNotes_Return Default, Accept
		
		Gui, AddNotes:Show, w%outerW% h%outerH%, Add Notes
		
		Hotkey,IfWinActive,ahk_id %AddNotes%
		hotkey, ^b, SetFontStyle2
		hotkey, ^i, SetFontStyle2
		hotkey, ^u, SetFontStyle2
		Hotkey, ^J, De_PDF2
		
		ANT.SetText(RTFNotesContent, ["KEEPUNDO"])
		GuiControl, Focus, % ANT.HWND
	}
}

FGcatList(myhwnd){
	Global
	Local catlst
	catlst:= ""
	For a, b in CAT.object() {
		catlst .= CAT[a].name "|"
	}
	stringreplace catlst, catlst, %modname%, %modname%|,
	GuiControl, , %myhwnd%, %catlst%
}


OnToolbar(hWnd, Event, Text, Pos, Id) {
	global NPCE_Main, SPE_Main, EQE_Main, TAB_Main, ART_Main
	If (Event != "Click") {
		Return
	}

	If (Text == "Open NPC") {
		gosub Open_NPC
	} Else If (Text == "Save NPC") {
		Gosub Save_NPC
	} Else If (Text == "Create Module") {
		Gosub ParseProject
	} Else If (Text == "Import NPC Text") {
		Gosub Import_Text
	} Else If (Text == "New NPC") {
		Gosub New_NPC
	} Else If (Text == "Open Next NPC") {
		Gosub Next_NPC
	} Else If (Text == "Open Previous NPC") {
		Gosub Prev_NPC
	} Else If (Text == "Manage NPCs") {
		Gosub Manage_JSON
	} Else If (Text == "Manage Project") {
		If (Winexist("A") = NPCE_Main) {
			Gosub Project_Manage
		}
		If (Winexist("A") = SPE_Main) {
			Gosub SPE_Project_Manage
		}
		If (Winexist("A") = EQE_Main) {
			Gosub EQE_Project_Manage
		}
		If (Winexist("A") = ART_Main) {
			Gosub ART_Project_Manage
		}
		If (Winexist("A") = TAB_Main) {
			Gosub TAB_Project_Manage
		}
	} Else If (Text == "New Project") {
		Gosub Project_New
	} Else If (Text == "Load Project") {
		Gosub Project_Load
	} Else If (Text == "Save Project") {
		Gosub Project_Save
	} Else If (Text == "Create Project") {
		Gosub Project_Create
	} Else If (Text == "Open Spell") {
		gosub Open_Spell
	} Else If (Text == "Save Spell") {
		Gosub Save_Spell
	} Else If (Text == "New Spell") {
		Gosub New_Spell
	} Else If (Text == "Open Next Spell") {
		Gosub Next_Spell
	} Else If (Text == "Open Previous Spell") {
		Gosub Prev_Spell
	} Else If (Text == "Import Spell Text") {
		Gosub Import_Spell_Text
	} Else If (Text == "Manage Spells") {
		Gosub Manage_SPE_JSON
	} Else If (Text == "Open Equipment Item") {
		gosub Open_Equipment
	} Else If (Text == "Save Equipment Item") {
		Gosub Save_Equipment
	} Else If (Text == "New Equipment Item") {
		Gosub New_Equipment
	} Else If (Text == "Open Next Equipment Item") {
		Gosub Next_Equipment
	} Else If (Text == "Open Previous Equipment Item") {
		Gosub Prev_Equipment
	} Else If (Text == "Import Equipment Text") {
		;~ Gosub Import_Spell_Text
	} Else If (Text == "Manage Equipment Items") {
		Gosub Manage_EQE_JSON
	} Else If (Text == "Open Artefact") {
		gosub Open_Artefact
	} Else If (Text == "Save Artefact") {
		Gosub Save_Artefact
	} Else If (Text == "New Artefact") {
		Gosub New_Artefact
	} Else If (Text == "Open Next Artefact") {
		Gosub Next_Artefact
	} Else If (Text == "Open Previous Artefact") {
		Gosub Prev_Artefact
	} Else If (Text == "Import Artefact") {
		;~ Gosub Import_Artefact_Text
	} Else If (Text == "Manage Artefacts") {
		Gosub Manage_ART_JSON
	} Else If (Text == "Open Table") {
		gosub Open_Table
	} Else If (Text == "Save Table") {
		Gosub Save_Table
	} Else If (Text == "New Table") {
		Gosub New_Table
	} Else If (Text == "Open Next Table") {
		Gosub Next_Table
	} Else If (Text == "Open Previous Table") {
		Gosub Prev_Table
	} Else If (Text == "Import CSV") {
		Gosub Import_Table_Text
	} Else If (Text == "Manage Tables") {
		Gosub Manage_TAB_JSON
	}
}

ToolbarCreate(Handler, Buttons, Window, ImageList := "", Options := "Flat List ToolTips", Pos := "") {
	Static TOOLTIPS := 0x100, WRAPABLE := 0x200, FLAT := 0x800, LIST := 0x1000, TABSTOP := 0x10000,  BORDER := 0x800000, TEXTONLY := 0
	Static BOTTOM := 0x3, ADJUSTABLE := 0x20, NODIVIDER := 0x40, VERTICAL := 0x80
	Static CHECKED := 1, HIDDEN := 8, WRAP := 32, DISABLED := 0 ; States
	Static CHECK := 2, CHECKGROUP := 6, DROPDOWN := 8, AUTOSIZE := 16, NOPREFIX := 32, SHOWTEXT := 64, WHOLEDROPDOWN := 128 ; Styles

	StrReplace(Options, "SHOWTEXT", "", fShowText, 1)
	fTextOnly := InStr(Options, "TEXTONLY")

	Styles := 0
	Loop Parse, Options, %A_Tab%%A_Space%, %A_Tab%%A_Space% ; Parse toolbar styles
		IfEqual A_LoopField,, Continue
		Else Styles |= A_LoopField + 0 ? A_LoopField : %A_LoopField%

	If (Pos != "") {
		Styles |= 0x4C ; CCS_NORESIZE | CCS_NOPARENTALIGN | CCS_NODIVIDER
	}

	Gui, %Window%:Add, Custom, ClassToolbarWindow32 hWndhWnd g@ToolbarHandler -Tabstop %Pos% %Styles%
	@ToolbarStorage(hWnd, Handler)

	TBBUTTON_Size := A_PtrSize == 8 ? 32 : 20
	Buttons := StrSplit(Buttons, "`n")
	cButtons := Buttons.Length()
	VarSetCapacity(TBBUTTONS, TBBUTTON_Size * cButtons , 0)

	Index := 0
	Loop %cButtons% {
		Button := StrSplit(Buttons[A_Index], ",", " `t")

		If (Button[1] == "-") {
			iBitmap := 0
			idCommand := 0
			fsState := 0
			fsStyle := 1 ; BTNS_SEP
			iString := -1
		} Else {
			Index++
			iBitmap := (fTextOnly) ? -1 : (Button[2] != "" ? Button[2] - 1 : Index - 1)
			idCommand := (Button[5]) ? Button[5] : 10000 + Index

			fsState := InStr(Button[3], "DISABLED") ? 0 : 4 ; TBSTATE_ENABLED
			Loop Parse, % Button[3], %A_Tab%%A_Space%, %A_Tab%%A_Space% ; Parse button states
				IfEqual A_LoopField,, Continue
				Else fsState |= %A_LoopField%

			fsStyle := fTextOnly || fShowText ? SHOWTEXT : 0
			Loop Parse, % Button[4], %A_Tab%%A_Space%, %A_Tab%%A_Space% ; Parse button styles
				IfEqual A_LoopField,, Continue
				Else fsStyle |= %A_LoopField%

			iString := &(ButtonText%Index% := Button[1])
		}

		Offset := (A_Index - 1) * TBBUTTON_Size
		NumPut(iBitmap, TBBUTTONS, Offset, "Int")
		NumPut(idCommand, TBBUTTONS, Offset + 4, "Int")
		NumPut(fsState, TBBUTTONS, Offset + 8, "UChar")
		NumPut(fsStyle, TBBUTTONS, Offset + 9, "UChar")
		NumPut(iString, TBBUTTONS, Offset + (A_PtrSize == 8 ? 24 : 16), "Ptr")
	}

	ExtendedStyle := 0x89 ; (mixed buttons, draw DD arrows, double buffer)
	SendMessage 0x454, 0, %ExtendedStyle%,, ahk_id %hWnd% ; TB_SETEXTENDEDSTYLE
	SendMessage 0x430, 0, %ImageList%,, ahk_id %hWnd% ; TB_SETIMAGELIST
	SendMessage % A_IsUnicode ? 0x444 : 0x414, %cButtons%, % &TBBUTTONS,, ahk_id %hWnd% ; TB_ADDBUTTONS

	If (InStr(Options, "VERTICAL")) {
		VarSetCapacity(SIZE, 8, 0)
		SendMessage 0x453, 0, &SIZE,, ahk_id %hWnd% ; TB_GETMAXSIZE
	} Else {
		SendMessage 0x421, 0, 0,, ahk_id %hWnd% ;TB_AUTOSIZE
	}

	Return hWnd
}

@ToolbarStorage(hWnd, Callback := "") {
	Static o := {}
	Return (o[hWnd] != "") ? o[hWnd] : o[hWnd] := Callback
}

@ToolbarHandler(hWnd) {
	Static n := {-2: "Click", -5: "RightClick", -20: "LDown", -713: "Hot", -710: "DropDown"}

	Handler := @ToolbarStorage(hWnd)

	Code := NumGet(A_EventInfo + 0, A_PtrSize * 2, "Int")

	If (Code != -713) {
		ButtonId := NumGet(A_EventInfo + (3 * A_PtrSize))
	} Else {
		ButtonId := NumGet(A_EventInfo, A_PtrSize == 8 ? 28 : 16, "Int") ; NMTBHOTITEM idNew
	}

	SendMessage 0x419, ButtonId,,, ahk_id %hWnd% ; TB_COMMANDTOINDEX
	Pos := ErrorLevel + 1

	VarSetCapacity(Text, 128)
	SendMessage % A_IsUnicode ? 0x44B : 0x42D, ButtonId, &Text,, ahk_id %hWnd% ; TB_GETBUTTONTEXT

	Event := (n[Code] != "") ? n[Code] : Code

	%Handler%(hWnd, Event, Text, Pos, ButtonId)
}

StBar(wind) {
	global
	local itemnumb
	Gui, %wind%:Default
	If Modname {
		If (Wind = "Eng_Suite") {
			SB_SetText(" " Modname, 1)
		}
		If (Wind = "NPCE_Main") {
			qc:= npc.SetCapacity(0)
			if !qc
				qc:= 0
			SB_SetText(" " Modname " (" qc " items)", 1)
		}
		If (Wind = "SPE_Main") {
			qc:= spl.SetCapacity(0)
			if !qc
				qc:= 0
			SB_SetText(" " Modname " (" qc " items)", 1)
		}
		If (Wind = "EQE_Main") {
			qc:= eqp.SetCapacity(0)
			if !qc
				qc:= 0
			SB_SetText(" " Modname " (" qc " items)", 1)
		}
		If (Wind = "ART_Main") {
			qc:= art.SetCapacity(0)
			if !qc
				qc:= 0
			SB_SetText(" " Modname " (" qc " items)", 1)
		}
	}
}



;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                  Validation                  |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


ValXML() {
	global
	local NPCdescript, temptext, tkn
	NPCdescript:= ""
	RichEditChoose()
	temptext:= Validate(%richtext%.GetRTF(False))
	TKN:= Tokenise(temptext)
	TKN:= RegExReplace(TKN,"U)\</p\>`n\<p\>\</([a-oq-z])\>","</$1></p>`n<p>")
	StringReplace, TKN, TKN, <p><h>, <h>, all
	StringReplace, TKN, TKN, </h></p>, </h>, all
	StringReplace, TKN, TKN, <p>`r`n<p>, <p>, all
	StringReplace, TKN, TKN, <p><frame>, <frame>, all
	StringReplace, TKN, TKN, </frame></p>, </frame>, all
	StringReplace, TKN, TKN, <p>`r`n</frame>, </p></frame>, all
	StringReplace, TKN, TKN, <p>`r`n`r`n<ul>, <ul>, all
	StringReplace, TKN, TKN, <p>`r`n<ul>, <ul>, all
	StringReplace, TKN, TKN, <p><ul>, <ul>, all
	
	local pos, frs, fre, alldone, newstr, repstr, tempTKN
	pos:= 1
	alldone:= 0
	temptkn:= tkn
	loop {
		frs:= 0
		fre:= 0
		frs:= InStr(TKN, "<frame>", , pos)
		fre:= InStr(TKN, "</frame>", , pos)
		
		if fre
			pos:= fre + 8
		if (frs and fre) {
			NewStr:= SubStr(TKN, frs, fre-frs+1)
			repstr:= Newstr
			StringReplace, repstr, repstr, </p>`n<p>, &#13;&#13;, all
			StringReplace, repstr, repstr, </p>, , all
			StringReplace, temptkn, temptkn, %NewStr%, %repstr%
		} else {
			alldone:= 1
		}
	} until alldone = 1
	TKN:= tempTKN
	
	pos:= 1
	alldone:= 0
	temptkn:= tkn
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
			StringReplace, repstr, repstr, </p>`n<p>, </i></p>`n<p><i>, all
			StringReplace, tempTKN, tempTKN, %NewStr%, %repstr%
		} else {
			alldone:= 1
		}
	} until alldone = 1
	TKN:= tempTKN

	pos:= 1
	alldone:= 0
	temptkn:= tkn
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
			StringReplace, repstr, repstr, </p>`n<p>, </b></p>`n<p><b>, all
			StringReplace, temptkn, temptkn, %NewStr%, %repstr%
		} else {
			alldone:= 1
		}
	} until alldone = 1
	TKN:= tempTKN

	pos:= 1
	alldone:= 0
	temptkn:= tkn
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
			StringReplace, repstr, repstr, </p>`n<p>, </u></p>`n<p><u>, all
			StringReplace, temptkn, temptkn, %NewStr%, %repstr%
		} else {
			alldone:= 1
		}
	} until alldone = 1
	TKN:= tempTKN

	stringreplace, TKN, TKN, <i><u>, <u><i>, All
	stringreplace, TKN, TKN, <i><b>, <b><i>, All
	stringreplace, TKN, TKN, <b><u>, <u><b>, All
	stringreplace, TKN, TKN, <i><b><u>, <u><b><i>, All

	TKN:= RegExReplace(TKN, "Us)`n</frame>`n<p><b>(?!<i>)(.*)</i></b>", "</i>`n</frame>`n<p><b><i>$1</i></b>")

	stringreplace, TKN, TKN, </frame></i></p>, </i></frame>, All
	stringreplace, TKN, TKN, %A_Space%%A_Space%, %A_Space%, All

	TKN:= LinkRegex(TKN)

	TKN:= RegExReplace(TKN, "\<link class(.*)\</link\>", Chr(10) "<linklist>" Chr(10) "<link class$1</link>" Chr(10) "</linklist>" Chr(10))

	;~ TKN:= RegExReplace(TKN, "\<link class(.*)\</link\>", "<linklist><link class$1</link></linklist>")

	NPCdescript .= TKN Chr(10)
	ValidateXML(NPCdescript)
}

ValidateXML(var) {
	global
	local innerW, innerH, outerW, outerH, buttonx, buttony, options
	outerW:= 900
	outerH:= 500
	innerW:= outerW-20
	innerH:= outerH-50
	buttonx:= (outerW-100)/2
	buttony:= outerH-30
	
	ValXMLText:= ""
	options:= "x10 y10 w" innerW " h" innerH "vValXMLText"
	
	Gui, ValXML:+hwndValXML
	Gui, ValXML:Color, 0xE2E1E8
	Gui, ValXML:font, S10 c000000, Tahoma
	Gui, ValXML:Add, Button, x%buttonx% y%buttony% w100 h20 +border vValXML_Close gValXML_Close, Close
	;~ Gui, ValXML:Add, Edit, cBlack x10 y10 w%innerW% h%innerH% vValXMLText, %var%
	ReVal:= New RichEdit("ValXML", options, True)
		ReVal.wordwrap(true)
		ReVal.ShowScrollBar(0, False)
		ReVal.SetBkgndColor("White")
		RFont := ReVal.GetFont()
		RFont.name:= "Calibri"
		RFont.Color:= "Black"
		RFont.Size:= "10"
		ReVal.SetFont(RFont)
		ReVal.SetOptions(["AUTOWORDSELECTION","AUTOVSCROLL"])
		ValRTF(var)

	Gui, ValXML:Show, w%outerW% h%outerH%, XML Validation
}

ValRTF(rtf) {
	local face, hues, tabstops
	hues:= "{\colortbl `;\red0\green0\blue0`;\red250\green0\blue0`;\red0\green200\blue0`;\red0\green0\blue250`;\red255\green155\blue0`;\red255\green255\blue0`;\red0\green170\blue250`;\red155\green94\blue250`;}"
	face:= "{\fonttbl {\f0 Calibri`;}"
	tabstops:= "\tx200"

; \highlight6 turns on highlighter & \highlight0 turns it off again.

	StringReplace, rtf, rtf, <p>, \line\cf4\b<p>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </p>, \cf4\b</p>\b0\cf0%A_Space%, All

	StringReplace, rtf, rtf, <frame>, \line\cf4\b<frame>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </frame>, \cf4\b</frame>\b0\cf0%A_Space%, All

	StringReplace, rtf, rtf, <h>, \line\cf4\b<h>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </h>, \cf4\b</h>\b0\cf0%A_Space%, All
	
	StringReplace, rtf, rtf, <ul>, \line\cf4\b<ul>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </ul>, \line\cf4\b</ul>\b0\cf0%A_Space%, All
	
	StringReplace, rtf, rtf, <li>, \line\tab\cf7\b<li>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </li>, \cf7\b</li>\b0\cf0%A_Space%, All
	
	StringReplace, rtf, rtf, <tr>, \line\cf4\b<tr>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </tr>, \line\cf4\b</tr>\b0\cf0%A_Space%, All
	
	StringReplace, rtf, rtf, <td>, \line\tab\cf7\b<td>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </td>, \cf7\b</td>\b0\cf0%A_Space%, All
	
	StringReplace, rtf, rtf, <b>, \cf2\b<b>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </b>, \cf2\b</b>\b0\cf0%A_Space%, All

	StringReplace, rtf, rtf, <i>, \cf2\b<i>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </i>, \cf2\b</i>\b0\cf0%A_Space%, All

	StringReplace, rtf, rtf, <u>, \cf2\b<u>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </u>, \cf2\b</u>\b0\cf0%A_Space%, All

	StringReplace, rtf, rtf, &quot`;, \cf5\b&quot`;\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, &#13`;, \cf5\b&#13`;\b0\cf0%A_Space%, All

	StringReplace, rtf, rtf, <linklist>, \line\cf4\b<linklist>\b0\cf0%A_Space%, All
	StringReplace, rtf, rtf, </linklist>, \line\cf4\b</linklist>\b0\cf0%A_Space%, All
	
	rtf:= RegExReplace(rtf, "U)\Q<link \E(.*)\Q>\E", "\line\tab\cf7\b<link \b0\cf0$1\cf7\b>\b0\cf0 ")
	StringReplace, rtf, rtf, </link>, \cf7\b</link>\b0\cf0, All

	;~ rtf:= RegExReplace(rtf, "U)\Q[link \E(.*)\Q]\E", "\line\tab\cf7\b<link \b0\cf0$1\cf7\b>\b0\cf0 ")
	;~ StringReplace, rtf, rtf, </link>, \cf7\b</link>\b0\cf0, All
	
	rtf:= RegExReplace(rtf, "U)=""(.*)""", "=\cf8""$1""\cf0 ")

	Reval.SetText("{\rtf1\ansi\deff0 " face "}" hues tabstops rtf "}", ["KEEPUNDO"])
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                  Menus & GUI                 |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


Component_Menu(ui, rf){
	Menu %ui%, Add, Manage Project, Project_Manage
	Menu %ui%, Icon, Manage Project, NPC Engineer.dll, 5
	Menu %ui%, Add
	If (rf != "artefact") {
		Menu %ui%, Add, Artefact Engineer, ReferMePlease
		Menu %ui%, Icon, Artefact Engineer, NPC Engineer.dll, 28
	}
	If (rf != "equipment") {
		Menu %ui%, Add, Equipment Engineer, ReferMePlease
		Menu %ui%, Icon, Equipment Engineer, NPC Engineer.dll, 22
	}
	If (rf != "npc") {
		Menu %ui%, Add, NPC Engineer, ReferMePlease
		Menu %ui%, Icon, NPC Engineer, NPC Engineer.dll, 18
	}
	If (rf != "spell") {
		Menu %ui%, Add, Spell Engineer, ReferMePlease
		Menu %ui%, Icon, Spell Engineer, NPC Engineer.dll, 19
	}
	If (rf != "table") {
		Menu %ui%, Add, Table Engineer, ReferMePlease
		Menu %ui%, Icon, Table Engineer, NPC Engineer.dll, 23
	}
}

Explorer_Menu(ui){
	Menu %ui%, Add, View Appdata folder, ExplorerMenu
	Menu %ui%, Add, View Projects folder, ExplorerMenu
	Menu %ui%, Add
	Menu %ui%, Add, View Artefacts folder, ExplorerMenu
	Menu %ui%, Add, View Equipment folder, ExplorerMenu
	Menu %ui%, Add, View NPCs folder, ExplorerMenu
	Menu %ui%, Add, View Parcels folder, ExplorerMenu
	Menu %ui%, Add, View Spells folder, ExplorerMenu
	Menu %ui%, Add, View Tables folder, ExplorerMenu
	Menu %ui%, Add
	Menu %ui%, Add, View Fantasy Grounds folder, ExplorerMenu
	Menu %ui%, Add, View Engineer Suite system folder, ExplorerMenu
}

Backup_Menu(ui){
	Menu %ui%, Add, Manage Backup Settings, Backup_Settings
	Menu %ui%, Icon, Manage Backup Settings, NPC Engineer.dll, 30
	Menu %ui%, Add, Schedule Backups, Backup_Schedule
	Menu %ui%, Icon, Schedule Backups, NPC Engineer.dll, 31
	Menu %ui%, Add, Perform Backup Now, BAC_BuildZip
	Menu %ui%, Icon, Perform Backup Now, NPC Engineer.dll, 29
}

Help_Menu(ui, rf){
	Menu %ui%, Add, About %rf%, HelpBox
	Menu %ui%, Icon, About %rf%, NPC Engineer.dll, 11
	Menu %ui%, Add, Quickstart Guide`tF12, Quickstart
	Menu %ui%, Icon, Quickstart Guide`tF12, NPC Engineer.dll, 16
	Menu %ui%, Add, Links in FG, LinkGuide
	Menu %ui%, Icon, Links in FG, NPC Engineer.dll, 16
	Menu %ui%, Add
	Menu %ui%, Add, Buy me a coffee, LaunchPayPal
}



HelpBox(){
	global
	Local WhereTo, HBRV, HBRD
	stringreplace, WhereTo, A_Thismenuitem, About%A_Space%, , All
	
	If (WhereTo = "NPC Engineer") {
		ReturnTo:= "NPCE_Main"
		HBRV:= Release_Version
		HBRD:= Release_Date
	} Else If (WhereTo = "Spell Engineer") {
		ReturnTo:= "SPE_Main"
		HBRV:= SPE_Release_Version
		HBRD:= SPE_Release_Date
	} Else If (WhereTo = "Equipment Engineer") {
		ReturnTo:= "EQE_Main"
		HBRV:= EQE_Release_Version
		HBRD:= EQE_Release_Date
	} Else If (WhereTo = "Artefact Engineer") {
		ReturnTo:= "ART_Main"
		HBRV:= ART_Release_Version
		HBRD:= ART_Release_Date
	} Else If (WhereTo = "Table Engineer") {
		ReturnTo:= "TAB_Main"
		HBRV:= TAB_Release_Version
		HBRD:= TAB_Release_Date
	} Else If (WhereTo = "Engineer Suite") {
		ReturnTo:= "Eng_Suite"
		HBRV:= ES_Release_Version
		HBRD:= ES_Release_Date
	}
	
	Gui, %ReturnTo%_About:-MinimizeBox -MaximizeBox +AlwaysOnTop
	Gui, %ReturnTo%_About:Color, 0xE2E1E8
	Gui, %ReturnTo%_About:Add, Button, x196 y146 w80 h23 Default Border gHelpBoxGuiClose, &OK
	Gui, %ReturnTo%_About:Add, GroupBox, x12 y10 w264 h98
	Gui, %ReturnTo%_About:Font, s18 cNavy
	Gui, %ReturnTo%_About:Add, Text, x15 y18 w258 h30 Center, %WhereTo%
	Gui, %ReturnTo%_About:Font
	Gui, %ReturnTo%_About:Add, Text, x211 y45 w40 h14 Right, v%HBRV%
	Gui, %ReturnTo%_About:Add, Text, x57 y65 w170 h2 0x10
	Gui, %ReturnTo%_About:Add, Text, x38 y70 w207 h28 Center, Written by Maasq.`nVersion released %HBRD%.
	Gui, %ReturnTo%_About:Font, s9 Bold
	Gui, %ReturnTo%_About:Add, Text, x12 y109 w47 h20 right, Web:
	Gui, %ReturnTo%_About:Add, Text, x12 y125 w47 h20 right, Discord:
	Gui, %ReturnTo%_About:Add, Text, x12 y141 w47 h20 right, Email:
	Gui, %ReturnTo%_About:Add, Text, x12 y157 w47 h20 right, Issues:
	Gui, %ReturnTo%_About:Font, s9 cBlue normal
	Gui, %ReturnTo%_About:Add, Text, x65 y109 w70 h16 gLaunchWebsite hwndHTEXT1, www.masq.net
	Gui, %ReturnTo%_About:Add, Text, x65 y125 w75 h16 gLaunchDiscord hwndHTEXT2, Engineer Suite
	Gui, %ReturnTo%_About:Add, Text, x65 y141 w100 h16 gLaunchEmail hwndHTEXT3, maasq@outlook.com
	Gui, %ReturnTo%_About:Add, Text, x65 y157 w100 h16 gLaunchGithub hwndHTEXT4, Github issue tracker
	Gui, %ReturnTo%_About:Font, s10 normal
	
	Gui, %ReturnTo%:+disabled
	Gui, %ReturnTo%_About:Show, w290 h179, About %WhereTo%
	
}


GUI_Categories(){
	global
	
	gowner:= CheckWin()
; Settings for Add Category window (NPCE_Categories)
	Gui, NPCE_Categories:+Owner%gowner%
	Gui, NPCE_Categories:-SysMenu
	Gui, NPCE_Categories:+hwndNPCE_Categories
	Gui, NPCE_Categories:Color, 0xE2E1E8
	Gui, NPCE_Categories:font, S10 c000000, Arial
	Gui, NPCE_Categories:margin, 5, 1
	
	Local catlst, mdtext
	For a, b in CAT.object() {
		catlst .= CAT[a].name "|"
	}
	
	mdtext:= "'" modname "' is the default category for this module and so cannot be removed from the list."
		
	Gui, NPCE_Categories:font, S9 c000000, Arial
	Gui, NPCE_Categories:Add, ListBox, 8 x10 y30 R13 w200 sort vCategoryList, %catlst%
	Gui, NPCE_Categories:font, S10 c000000, Arial
	
	Gui, NPCE_Categories:Add, Text, x10 y10 w120 h17, Delete Category:
	Gui, NPCE_Categories:Add, Text, x230 y10 w173 h17, Add New Category:
	Gui, NPCE_Categories:Add, Edit, vNewCategory x230 y30 w200 h23 Left, 
	
	Gui, NPCE_Categories:Add, Text, x230 y100 w200 h51, %mdtext%
	
	Gui, NPCE_Categories:Add, Button, x30 y235 w80 h23 +border gCategDelete, Delete
	Gui, NPCE_Categories:Add, Button, x3500 y58 w80 h23 +border +default gCategAdd, Add

	Gui, NPCE_Categories:Add, Button, x300 y228 w130 h30 +border gNPCE_Categories_Close, Close

	Gui, NPCE_Categories:Show, w440 h268, Add or Delete Categories
}

ExplorerMenu() {
	global
	If (A_ThisMenuItem = "View Appdata folder") {
		if DataDir {
			Run, explore %DataDir%
		} Else {
			MsgBox, 48, , You have no data folder set! Please enter one in 'Options'.`n`nHaving no data folder will cause issues.
		}
	} Else If (A_ThisMenuItem = "View Projects folder") {
		if NPCPath {
			Run, explore %ProjPath%
		} Else {
			MsgBox, 48, , You have no Project folder set! Please enter one in 'Options'.
		}
	} Else If (A_ThisMenuItem = "View NPCs folder") {
		if NPCPath {
			Run, explore %NPCPath%
		} Else {
			MsgBox, 48, , You have no NPC folder set! Please enter one in 'Options'.
		}
	} Else If (A_ThisMenuItem = "View Spells folder") {
		if SpellPath {
			Run, explore %SpellPath%
		} Else {
			MsgBox, 48, , You have no Spell folder set! Please enter one in 'Options'.
		}
	} Else If (A_ThisMenuItem = "View Equipment folder") {
		if EquipPath {
			Run, explore %EquipPath%
		} Else {
			MsgBox, 48, , You have no Equipment folder set! Please enter one in 'Options'.
		}
	} Else If (A_ThisMenuItem = "View Artefacts folder") {
		if ArtePath {
			Run, explore %ArtePath%
		} Else {
			MsgBox, 48, , You have no Artefact folder set! Please enter one in 'Options'.
		}
	} Else If (A_ThisMenuItem = "View Tables folder") {
		if TablePath {
			Run, explore %TablePath%
		} Else {
			MsgBox, 48, , You have no Tables folder set! Please enter one in 'Options'.
		}
	} Else If (A_ThisMenuItem = "View Parcels folder") {
		if ParcelPath {
			Run, explore %ParcelPath%
		} Else {
			MsgBox, 48, , You have no Parcels folder set! Please enter one in 'Options'.
		}
	} Else If (A_ThisMenuItem = "View Fantasy Grounds folder") {
		if FGPath {
			Run, explore %FGPath%
		} Else {
			MsgBox, 48, , You haven't set your Fantasy Grounds path yet! Please create or open a project.
		}
	} Else If (A_ThisMenuItem = "View Engineer Suite system folder") {
		Run, explore %A_ScriptDir%
	} 
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                    Objects                   |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


mm(x*) {
	for a, b in x
		text .= (IsObject(b)?pa(b):b) "`n"
	MsgBox, 0, msgbox, % text
}

pa(array, depth=5, indentLevel:="   ") {
	try {
		for k,v in Array {
			lst.= indentLevel "[" k "]"
			if (IsObject(v) && depth>1)
				lst.="`n" pa(v, depth-1, indentLevel . "    ")
			else
				lst.=" -> " v
			lst.="`n"
		} return rtrim(lst, "`r`n `t")    
	} return
}

ObjFullyClone(obj) {
	nobj := obj.Clone()
	for k,v in nobj
		if IsObject(v)
			nobj[k] := A_ThisFunc.(v)
	return nobj
}

SortArray(Array, Order="A") {
    ;Order A: Ascending, D: Descending, R: Reverse
    MaxIndex := ObjMaxIndex(Array)
    If (Order = "R") {
        count := 0
        Loop, % MaxIndex 
            ObjInsert(Array, ObjRemove(Array, MaxIndex - count++))
        Return
    }
    Partitions := "|" ObjMinIndex(Array) "," MaxIndex
    Loop {
        comma := InStr(this_partition := SubStr(Partitions, InStr(Partitions, "|", False, 0)+1), ",")
        spos := pivot := SubStr(this_partition, 1, comma-1) , epos := SubStr(this_partition, comma+1)    
        if (Order = "A") {    
            Loop, % epos - spos {
                if (Array[pivot] > Array[A_Index+spos])
                    ObjInsert(Array, pivot++, ObjRemove(Array, A_Index+spos))    
            }
        } else {
            Loop, % epos - spos {
                if (Array[pivot] < Array[A_Index+spos])
                    ObjInsert(Array, pivot++, ObjRemove(Array, A_Index+spos))    
            }
        }
        Partitions := SubStr(Partitions, 1, InStr(Partitions, "|", False, 0)-1)
        if (pivot - spos) > 1    ;if more than one elements
            Partitions .= "|" spos "," pivot-1        ;the left partition
        if (epos - pivot) > 1    ;if more than one elements
            Partitions .= "|" pivot+1 "," epos        ;the right partition
    } Until !Partitions
}

HasVal(haystack, needle) {
	if !(IsObject(haystack)) || (haystack.Length() = 0)
		return 0
	for index, value in haystack
		if (value = needle)
			return index
	return 0
}

trimArray(arr) { ; Hash O(n) 
    hash:= {}, newArr := []

    for e, v in arr
        if (!hash.Haskey(v))
            hash[(v)]:= 1, newArr.push(v)
    return newArr
}

;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                    Classes                   |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


class Egui {
	__New(window, ctrltype, options := "", text := "", callback := 0){
		window:= "NPCE_" window
		Gui, %window%:Add, % ctrltype, % "hwndhwnd " options, % text
		this.hwnd := hwnd
		this.callback := callback
		if (callback != 0){
			fn := this.EditChanged.Bind(this)
			GuiControl, +g, % this.hwnd, % fn
			this.CallbackFn := fn
		}
	}
	
	Get(){
		GuiControlGet, val, , % this.hwnd
		return val
	}
	
	Set(value){
		if (this.callback != 0){
			GuiControl, -g, % this.hwnd
		}
		GuiControl, , % this.hwnd, % value
		if (this.callback != 0){
			fn := this.callbackFn
			GuiControl, +g, % this.hwnd, % fn
		}
	}
	
	EditChanged(){
		if (this.callback != 0){
			value := this.Get()
			this.callback.call(value)
		}
	}
}

Class JSONFile {
	static Instances := []
	
/*
	Class JSONFile
	Written by Runar "RUNIE" Borge
	
	Dependencies:
	JSON loader/dumper by cocobelgica: https://github.com/cocobelgica/AutoHotkey-JSON
	However the class can easily be modified to use another JSON dump/load lib.
	
	To create a new JSON file wrapper:
	MyJSON := new JSONFile(filepath)
	
	And to destroy it:
	MyJSON := ""
	
	Methods:
		.Save(Prettify := false) - save object to file
		.JSON(Prettify := false) - Get JSON text
		.Fill(Object) - fill keys in from another object into the instance object
		
	Instance variables:
		.File() - get file path
		.Object() - get data object
*/
	
	__New(File) {
		FileExist := FileExist(File)
		JSONFile.Instances[this] := {File: File, Object: {}}
		ObjRelease(&this)
		FileObj := FileOpen(File, "rw")
		if !IsObject(FileObj)
			throw Exception("Can't access file for JSONFile instance: " File, -1)
		if FileExist {
			try
				JSONFile.Instances[this].Object := JSON.Load(FileObj.Read())
			catch e {
				this.__Delete()
				throw e
			} if (JSONFile.Instances[this].Object = "")
				JSONFile.Instances[this].Object := {}
		} else
			JSONFile.Instances[this].IsNew := true
		return this
	}
	
	__Delete() {
		if JSONFile.Instances.HasKey(this) {
			ObjAddRef(&this)
			JSONFile.Instances.Delete(this)
		}
	}
	
	__Call(Func, Param*) {
		; return instance value (File, Object, FileObj, IsNew)
		if JSONFile.Instances[this].HasKey(Func)
			return JSONFile.Instances[this][Func]
		
		; return formatted json
		if (Func = "JSON")
			return StrReplace(JSON.Dump(this.Object(),, Param.1 ? A_Tab : ""), "`n", "`r`n")
		
		; save the json file
		if (Func = "Save") {
			try
				New := this.JSON(Param.1)
			catch e
				return false
			FileObj := FileOpen(this.File(), "w")
			FileObj.Length := 0
			FileObj.Write(New)
			FileObj.__Handle
			return true
		}
		
		; fill from specified array into the JSON array
		if (Func = "Fill") {
			if !IsObject(Param.2)
				Param.2 := []
			for Key, Val in Param.1 {
				if (A_Index > 1)
					Param.2.Pop()
				HasKey := Param.2.MaxIndex()
						? this.Object()[Param.2*].HasKey(Key) 
						: this.Object().HasKey(Key)
				Param.2.Push(Key)
				if IsObject(Val) && HasKey
					this.Fill(Val, Param.2), Param.2.Pop()
				else if !HasKey
					this.Object()[Param.2*] := Val
			} return
		}
		
		return Obj%Func%(this.Object(), Param*)
	}
	
	__Set(Key, Val) {
		return this.Object()[Key] := Val
	}
	
	__Get(Key) {
		return this.Object()[Key]
	}
}

class JSON {
	/**
	 * Method: Load
	 *     Parses a JSON string into an AHK value
	 * Syntax:
	 *     value := JSON.Load( text [, reviver ] )
	 * Parameter(s):
	 *     value      [retval] - parsed value
	 *     text    [in, ByRef] - JSON formatted string
	 *     reviver   [in, opt] - function object, similar to JavaScript's
	 *                           JSON.parse() 'reviver' parameter
	 */
	class Load extends JSON.Functor
	{
		Call(self, ByRef text, reviver:="")
		{
			this.rev := IsObject(reviver) ? reviver : false
		; Object keys(and array indices) are temporarily stored in arrays so that
		; we can enumerate them in the order they appear in the document/text instead
		; of alphabetically. Skip if no reviver function is specified.
			this.keys := this.rev ? {} : false

			static quot := Chr(34), bashq := "\" . quot
				 , json_value := quot . "{[01234567890-tfn"
				 , json_value_or_array_closing := quot . "{[]01234567890-tfn"
				 , object_key_or_object_closing := quot . "}"

			key := ""
			is_key := false
			root := {}
			stack := [root]
			next := json_value
			pos := 0

			while ((ch := SubStr(text, ++pos, 1)) != "") {
				if InStr(" `t`r`n", ch)
					continue
				if !InStr(next, ch, 1)
					this.ParseError(next, text, pos)

				holder := stack[1]
				is_array := holder.IsArray

				if InStr(",:", ch) {
					next := (is_key := !is_array && ch == ",") ? quot : json_value

				} else if InStr("}]", ch) {
					ObjRemoveAt(stack, 1)
					next := stack[1]==root ? "" : stack[1].IsArray ? ",]" : ",}"

				} else {
					if InStr("{[", ch) {
					; Check if Array() is overridden and if its return value has
					; the 'IsArray' property. If so, Array() will be called normally,
					; otherwise, use a custom base object for arrays
						static json_array := Func("Array").IsBuiltIn || ![].IsArray ? {IsArray: true} : 0
					
					; sacrifice readability for minor(actually negligible) performance gain
						(ch == "{")
							? ( is_key := true
							  , value := {}
							  , next := object_key_or_object_closing )
						; ch == "["
							: ( value := json_array ? new json_array : []
							  , next := json_value_or_array_closing )
						
						ObjInsertAt(stack, 1, value)

						if (this.keys)
							this.keys[value] := []
					
					} else {
						if (ch == quot) {
							i := pos
							while (i := InStr(text, quot,, i+1)) {
								value := StrReplace(SubStr(text, pos+1, i-pos-1), "\\", "\u005c")

								static tail := A_AhkVersion<"2" ? 0 : -1
								if (SubStr(value, tail) != "\")
									break
							}

							if (!i)
								this.ParseError("'", text, pos)

							  value := StrReplace(value,  "\/",  "/")
							, value := StrReplace(value, bashq, quot)
							, value := StrReplace(value,  "\b", "`b")
							, value := StrReplace(value,  "\f", "`f")
							, value := StrReplace(value,  "\n", "`n")
							, value := StrReplace(value,  "\r", "`r")
							, value := StrReplace(value,  "\t", "`t")

							pos := i ; update pos
							
							i := 0
							while (i := InStr(value, "\",, i+1)) {
								if !(SubStr(value, i+1, 1) == "u")
									this.ParseError("\", text, pos - StrLen(SubStr(value, i+1)))

								uffff := Abs("0x" . SubStr(value, i+2, 4))
								if (A_IsUnicode || uffff < 0x100)
									value := SubStr(value, 1, i-1) . Chr(uffff) . SubStr(value, i+6)
							}

							if (is_key) {
								key := value, next := ":"
								continue
							}
						
						} else {
							value := SubStr(text, pos, i := RegExMatch(text, "[\]\},\s]|$",, pos)-pos)

							static number := "number", integer :="integer"
							if value is %number%
							{
								if value is %integer%
									value += 0
							}
							else if (value == "true" || value == "false")
								value := %value% + 0
							else if (value == "null")
								value := ""
							else
							; we can do more here to pinpoint the actual culprit
							; but that's just too much extra work.
								this.ParseError(next, text, pos, i)

							pos += i-1
						}

						next := holder==root ? "" : is_array ? ",]" : ",}"
					} ; If InStr("{[", ch) { ... } else

					is_array? key := ObjPush(holder, value) : holder[key] := value

					if (this.keys && this.keys.HasKey(holder))
						this.keys[holder].Push(key)
				}
			
			} ; while ( ... )

			return this.rev ? this.Walk(root, "") : root[""]
		}

		ParseError(expect, ByRef text, pos, len:=1)
		{
			static quot := Chr(34), qurly := quot . "}"
			
			line := StrSplit(SubStr(text, 1, pos), "`n", "`r").Length()
			col := pos - InStr(text, "`n",, -(StrLen(text)-pos+1))
			msg := Format("{1}`n`nLine:`t{2}`nCol:`t{3}`nChar:`t{4}"
			,     (expect == "")     ? "Extra data"
				: (expect == "'")    ? "Unterminated string starting at"
				: (expect == "\")    ? "Invalid \escape"
				: (expect == ":")    ? "Expecting ':' delimiter"
				: (expect == quot)   ? "Expecting object key enclosed in double quotes"
				: (expect == qurly)  ? "Expecting object key enclosed in double quotes or object closing '}'"
				: (expect == ",}")   ? "Expecting ',' delimiter or object closing '}'"
				: (expect == ",]")   ? "Expecting ',' delimiter or array closing ']'"
				: InStr(expect, "]") ? "Expecting JSON value or array closing ']'"
				:                      "Expecting JSON value(string, number, true, false, null, object or array)"
			, line, col, pos)

			static offset := A_AhkVersion<"2" ? -3 : -4
			throw Exception(msg, offset, SubStr(text, pos, len))
		}

		Walk(holder, key)
		{
			value := holder[key]
			if IsObject(value) {
				for i, k in this.keys[value] {
					; check if ObjHasKey(value, k) ??
					v := this.Walk(value, k)
					if (v != JSON.Undefined)
						value[k] := v
					else
						ObjDelete(value, k)
				}
			}
			
			return this.rev.Call(holder, key, value)
		}
	}

	/**
	 * Method: Dump
	 *     Converts an AHK value into a JSON string
	 * Syntax:
	 *     str := JSON.Dump( value [, replacer, space ] )
	 * Parameter(s):
	 *     str        [retval] - JSON representation of an AHK value
	 *     value          [in] - any value(object, string, number)
	 *     replacer  [in, opt] - function object, similar to JavaScript's
	 *                           JSON.stringify() 'replacer' parameter
	 *     space     [in, opt] - similar to JavaScript's JSON.stringify()
	 *                           'space' parameter
	 */
	class Dump extends JSON.Functor
	{
		Call(self, value, replacer:="", space:="")
		{
			this.rep := IsObject(replacer) ? replacer : ""

			this.gap := ""
			if (space) {
				static integer := "integer"
				if space is %integer%
					Loop, % ((n := Abs(space))>10 ? 10 : n)
						this.gap .= " "
				else
					this.gap := SubStr(space, 1, 10)

				this.indent := "`n"
			}

			return this.Str({"": value}, "")
		}

		Str(holder, key)
		{
			value := holder[key]

			if (this.rep)
				value := this.rep.Call(holder, key, ObjHasKey(holder, key) ? value : JSON.Undefined)

			if IsObject(value) {
			; Check object type, skip serialization for other object types such as
			; ComObject, Func, BoundFunc, FileObject, RegExMatchObject, Property, etc.
				static type := A_AhkVersion<"2" ? "" : Func("Type")
				if (type ? type.Call(value) == "Object" : ObjGetCapacity(value) != "") {
					if (this.gap) {
						stepback := this.indent
						this.indent .= this.gap
					}

					is_array := value.IsArray
				; Array() is not overridden, rollback to old method of
				; identifying array-like objects. Due to the use of a for-loop
				; sparse arrays such as '[1,,3]' are detected as objects({}). 
					if (!is_array) {
						for i in value
							is_array := i == A_Index
						until !is_array
					}

					str := ""
					if (is_array) {
						Loop, % value.Length() {
							if (this.gap)
								str .= this.indent
							
							v := this.Str(value, A_Index)
							str .= (v != "") ? v . "," : "null,"
						}
					} else {
						colon := this.gap ? ": " : ":"
						for k in value {
							v := this.Str(value, k)
							if (v != "") {
								if (this.gap)
									str .= this.indent

								str .= this.Quote(k) . colon . v . ","
							}
						}
					}

					if (str != "") {
						str := RTrim(str, ",")
						if (this.gap)
							str .= stepback
					}

					if (this.gap)
						this.indent := stepback

					return is_array ? "[" . str . "]" : "{" . str . "}"
				}
			
			} else ; is_number ? value : "value"
				return ObjGetCapacity([value], 1)=="" ? value : this.Quote(value)
		}

		Quote(string)
		{
			static quot := Chr(34), bashq := "\" . quot

			if (string != "") {
				  string := StrReplace(string,  "\",  "\\")
				; , string := StrReplace(string,  "/",  "\/") ; optional in ECMAScript
				, string := StrReplace(string, quot, bashq)
				, string := StrReplace(string, "`b",  "\b")
				, string := StrReplace(string, "`f",  "\f")
				, string := StrReplace(string, "`n",  "\n")
				, string := StrReplace(string, "`r",  "\r")
				, string := StrReplace(string, "`t",  "\t")

				static rx_escapable := A_AhkVersion<"2" ? "O)[^\x20-\x7e]" : "[^\x20-\x7e]"
				while RegExMatch(string, rx_escapable, m)
					string := StrReplace(string, m.Value, Format("\u{1:04x}", Ord(m.Value)))
			}

			return quot . string . quot
		}
	}

	/**
	 * Property: Undefined
	 *     Proxy for 'undefined' type
	 * Syntax:
	 *     undefined := JSON.Undefined
	 * Remarks:
	 *     For use with reviver and replacer functions since AutoHotkey does not
	 *     have an 'undefined' type. Returning blank("") or 0 won't work since these
	 *     can't be distnguished from actual JSON values. This leaves us with objects.
	 *     Replacer() - the caller may return a non-serializable AHK objects such as
	 *     ComObject, Func, BoundFunc, FileObject, RegExMatchObject, and Property to
	 *     mimic the behavior of returning 'undefined' in JavaScript but for the sake
	 *     of code readability and convenience, it's better to do 'return JSON.Undefined'.
	 *     Internally, the property returns a ComObject with the variant type of VT_EMPTY.
	 */
	Undefined[]
	{
		get {
			static empty := {}, vt_empty := ComObject(0, &empty, 1)
			return vt_empty
		}
	}

	class Functor
	{
		__Call(method, ByRef arg, args*)
		{
		; When casting to Call(), use a new instance of the "function object"
		; so as to avoid directly storing the properties(used across sub-methods)
		; into the "function object" itself.
			if IsObject(method)
				return (new this).Call(method, arg, args*)
			else if (method == "")
				return (new this).Call(arg, args*)
		}
	}
}

Class RichEdit {
   ; ===================================================================================================================
   ; Class variables - do not change !!!
   ; ===================================================================================================================
   ; RichEdit v4.1 (Unicode)
   Static Class := "RICHEDIT50W"
   ; RichEdit v4.1 (Unicode)
   Static DLL := "Msftedit.dll"
   ; DLL handle
   Static Instance := DllCall("Kernel32.dll\LoadLibrary", "Str", RichEdit.DLL, "UPtr")
   ; Callback function handling RichEdit messages
   Static SubclassCB := 0
   ; Number of controls/instances
   Static Controls := 0
   ; ===================================================================================================================
   ; Instance variables - do not change !!!
   ; ===================================================================================================================
   GuiName := ""
   GuiHwnd := ""
   HWND := ""
   DefFont := ""
   ; ===================================================================================================================
   ; CONSTRUCTOR
   ; ===================================================================================================================
   __New(GuiName, Options, MultiLine := True) {
	  Static WS_TABSTOP := 0x10000, WS_HSCROLL := 0x100000, WS_VSCROLL := 0x200000, WS_VISIBLE := 0x10000000
		   , WS_CHILD := 0x40000000
		   , WS_EX_CLIENTEDGE := 0x200, WS_EX_STATICEDGE := 0x20000
		   , ES_MULTILINE := 0x0004, ES_AUTOVSCROLL := 0x40, ES_AUTOHSCROLL := 0x80, ES_NOHIDESEL := 0x0100
		   , ES_WANTRETURN := 0x1000, ES_DISABLENOSCROLL := 0x2000, ES_SUNKEN := 0x4000, ES_SAVESEL := 0x8000
		   , ES_SELECTIONBAR := 0x1000000
	  ; Check for Unicode
	  If !(SubStr(A_AhkVersion, 1, 1) > 1) && !(A_IsUnicode) {
		 MsgBox, 16, % A_ThisFunc, % This.__Class . " requires a unicode version of AHK!"
		 Return False
	  }
	  ; Do not instantiate instances of RichEdit
	  If (This.Base.HWND)
		 Return False
	  ; Determine the HWND of the GUI
	  Gui, %GuiName%:+LastFoundExist
	  GuiHwnd := WinExist()
	  If !(GuiHwnd) {
		 ErrorLevel := "ERROR: Gui " . GuiName . " does not exist!"
		 Return False
	  }
	  ; Load library if necessary
	  If (This.Base.Instance = 0) {
		 This.Base.Instance := DllCall("Kernel32.dll\LoadLibrary", "Str", This.Base.DLL, "UPtr")
		 If (ErrorLevel) {
			ErrorLevel := "ERROR: Error loading " . This.Base.DLL . " - " . ErrorLevel
			Return False
		 }
	  }
	  ; Specify default styles & exstyles
	  Styles := WS_TABSTOP | WS_VISIBLE | WS_CHILD | ES_AUTOHSCROLL
	  If (MultiLine)
		 Styles |= WS_HSCROLL | WS_VSCROLL | ES_MULTILINE | ES_AUTOVSCROLL | ES_NOHIDESEL | ES_WANTRETURN
				 | ES_DISABLENOSCROLL | ES_SAVESEL ; | ES_SELECTIONBAR does not work properly
	  ExStyles := WS_EX_STATICEDGE
	  ; Create the control
	  CtrlClass := This.Class
	  Gui, %GuiName%:Add, Custom, Class%CtrlClass% %Options% hwndHWND +%Styles% +E%ExStyles%
	  If (MultiLine) {
		 ; Adjust the formatting rectangle for multiline controls to simulate a selection bar
		 ; EM_GETRECT = 0xB2, EM_SETRECT = 0xB3
		 VarSetCapacity(RECT, 16, 0)
		 SendMessage, 0xB2, 0, &RECT, , ahk_id %HWND%
		 NumPut(NumGet(RECT, 0, "Int") + 10, RECT, 0, "Int")
		 NumPut(NumGet(RECT, 4, "Int") + 2,  RECT, 4, "Int")
		 SendMessage, 0xB3, 0, &RECT, , ahk_id %HWND%
		 ; Set advanced typographic options
		 ; EM_SETTYPOGRAPHYOPTIONS = 0x04CA (WM_USER + 202)
		 ; TO_ADVANCEDTYPOGRAPHY	= 1, TO_ADVANCEDLAYOUT = 8 ? not documented
		 SendMessage, 0x04CA, 0x01, 0x01, , ahk_id %HWND%
	  }
	  ; Initialize control
	  ; EM_SETLANGOPTIONS = 0x0478 (WM_USER + 120)
	  ; IMF_AUTOKEYBOARD = 0x01, IMF_AUTOFONT = 0x02
	  SendMessage, 0x0478, 0, 0x03, , ahk_id %HWND%
	  ; Subclass the control to get Tab key and prevent Esc from sending a WM_CLOSE message to the parent window.
	  ; One of majkinetor's splendid discoveries!
	  ; Initialize SubclassCB
	  If (This.Base.SubclassCB = 0)
		 This.Base.SubclassCB := RegisterCallback("RichEdit.SubclassProc")
	  DllCall("Comctl32.dll\SetWindowSubclass", "Ptr", HWND, "Ptr", This.Base.SubclassCB, "Ptr", HWND, "Ptr", 0)
	  This.GuiName := GuiName
	  This.GuiHwnd := GuiHwnd
	  This.HWND := HWND
	  This.DefFont := This.GetFont(1)
	  This.DefFont.Default := 1
	  ; Correct AHK font size setting, if necessary
	  If (Round(This.DefFont.Size) <> This.DefFont.Size) {
		 This.DefFont.Size := Round(This.DefFont.Size)
		 This.SetDefaultFont()
	  }
	  This.Base.Controls += 1
	  ; Initialize the print margins
	  This.GetMargins()
	  ; Initialize the text limit
	  This.LimitText(2147483647)
   }
   ; ===================================================================================================================
   ; DESTRUCTOR
   ; ===================================================================================================================
   __Delete() {
	  If (This.HWND) {
		 DllCall("Comctl32.dll\RemoveWindowSubclass", "Ptr", This.HWND, "Ptr", This.Base.SubclassCB, "Ptr", 0)
		 DllCall("User32.dll\DestroyWindow", "Ptr", This.HWND)
		 This.HWND := 0
		 This.Base.Controls -= 1
		 If (This.Base.Controls = 0) {
			DllCall("Kernel32.dll\FreeLibrary", "Ptr", This.Base.Instance)
			This.Base.Instance := 0
		 }
	  }
   }
   ; ===================================================================================================================
   ; ===================================================================================================================
   ; INTERNAL CLASSES ==================================================================================================
   ; ===================================================================================================================
   ; ===================================================================================================================
   Class CF2 { ; CHARFORMAT2 structure -> http://msdn.microsoft.com/en-us/library/bb787883(v=vs.85).aspx
	  __New() {
		 Static CF2_Size := 116
		 This.Insert(":", {Mask: {O: 4, T: "UInt"}, Effects: {O: 8, T: "UInt"}
						 , Height: {O: 12, T: "Int"}, Offset: {O: 16, T: "Int"}
						 , TextColor: {O: 20, T: "Int"}, CharSet: {O: 24, T: "UChar"}
						 , PitchAndFamily: {O: 25, T: "UChar"}, FaceName: {O: 26, T: "Str32"}
						 , Weight: {O: 90, T: "UShort"}, Spacing: {O: 92, T: "Short"}
						 , BackColor: {O: 96, T: "UInt"}, LCID: {O: 100, T: "UInt"}
						 , Cookie: {O: 104, T: "UInt"}, Style: {O: 108, T: "Short"}
						 , Kerning: {O: 110, T: "UShort"}, UnderlineType: {O: 112, T: "UChar"}
						 , Animation: {O: 113, T: "UChar"}, RevAuthor: {O: 114, T: "UChar"}
						 , UnderlineColor: {O: 115, T: "UChar"}})
		 This.Insert(".")
		 This.SetCapacity(".", CF2_Size)
		 Addr :=  This.GetAddress(".")
		 DllCall("Kernel32.dll\RtlZeroMemory", "Ptr", Addr, "Ptr", CF2_Size)
		 NumPut(CF2_Size, Addr + 0, 0, "UInt")
	  }
	  __Get(Name) {
		 Addr := This.GetAddress(".")
		 If (Name = "CF2")
			Return Addr
		 If !This[":"].HasKey(Name)
			Return ""
		 Attr := This[":"][Name]
		 If (Name <> "FaceName")
			Return NumGet(Addr + 0, Attr.O, Attr.T)
		 Return StrGet(Addr + Attr.O, 32)
	  }
	  __Set(Name, Value) {
		 Addr := This.GetAddress(".")
		 If !This[":"].HasKey(Name)
			Return ""
		 Attr := This[":"][Name]
		 If (Name <> "FaceName")
			NumPut(Value, Addr + 0, Attr.O, Attr.T)
		 Else
			StrPut(Value, Addr + Attr.O, 32)
		 Return Value
	  }
   }
   ; -------------------------------------------------------------------------------------------------------------------
   Class PF2 { ; PARAFORMAT2 structure -> http://msdn.microsoft.com/en-us/library/bb787942(v=vs.85).aspx
	  __New() {
		 Static PF2_Size := 188
		 This.Insert(":", {Mask: {O: 4, T: "UInt"}, Numbering: {O: 8, T: "UShort"}
						 , StartIndent: {O: 12, T: "Int"}, RightIndent: {O: 16, T: "Int"}
						 , Offset: {O: 20, T: "Int"}, Alignment: {O: 24, T: "UShort"}
						 , TabCount: {O: 26, T: "UShort"}, Tabs: {O: 28, T: "UInt"}
						 , SpaceBefore: {O: 156, T: "Int"}, SpaceAfter: {O: 160, T: "Int"}
						 , LineSpacing: {O: 164, T: "Int"}, Style: {O: 168, T: "Short"}
						 , LineSpacingRule: {O: 170, T: "UChar"}, OutlineLevel: {O: 171, T: "UChar"}
						 , ShadingWeight: {O: 172, T: "UShort"}, ShadingStyle: {O: 174, T: "UShort"}
						 , NumberingStart: {O: 176, T: "UShort"}, NumberingStyle: {O: 178, T: "UShort"}
						 , NumberingTab: {O: 180, T: "UShort"}, BorderSpace: {O: 182, T: "UShort"}
						 , BorderWidth: {O: 184, T: "UShort"}, Borders: {O: 186, T: "UShort"}})
		 This.Insert(".")
		 This.SetCapacity(".", PF2_Size)
		 Addr :=  This.GetAddress(".")
		 DllCall("Kernel32.dll\RtlZeroMemory", "Ptr", Addr, "Ptr", PF2_Size)
		 NumPut(PF2_Size, Addr + 0, 0, "UInt")
	  }
	  __Get(Name) {
		 Addr := This.GetAddress(".")
		 If (Name = "PF2")
			Return Addr
		 If !This[":"].HasKey(Name)
			Return ""
		 Attr := This[":"][Name]
		 If (Name <> "Tabs")
			Return NumGet(Addr + 0, Attr.O, Attr.T)
		 Tabs := []
		 Offset := Attr.O - 4
		 Loop, 32
			Tabs[A_Index] := NumGet(Addr + 0, Offset += 4, "UInt")
		 Return Tabs
	  }
	  __Set(Name, Value) {
		 Addr := This.GetAddress(".")
		 If !This[":"].HasKey(Name)
			Return ""
		 Attr := This[":"][Name]
		 If (Name <> "Tabs") {
			NumPut(Value, Addr + 0, Attr.O, Attr.T)
			Return Value
		 }
		 If !IsObject(Value)
			Return ""
		 Offset := Attr.O - 4
		 For Each, Tab In Value
			NumPut(Tab, Addr + 0, Offset += 4, "UInt")
		 Return Tabs
	  }
   }
   ; ===================================================================================================================
   ; ===================================================================================================================
   ; PRIVATE METHODS ===================================================================================================
   ; ===================================================================================================================
   ; ===================================================================================================================
   GetBGR(RGB) { ; Get numeric BGR value from numeric RGB value or HTML color name
	  Static HTML := {BLACK:  0x000000, SILVER: 0xC0C0C0, GRAY:   0x808080, WHITE:   0xFFFFFF
					, MAROON: 0x000080, RED:    0x0000FF, PURPLE: 0x800080, FUCHSIA: 0xFF00FF
					, GREEN:  0x008000, LIME:   0x00FF00, OLIVE:  0x008080, YELLOW:  0x00FFFF
					, NAVY:   0x800000, BLUE:   0xFF0000, TEAL:   0x808000, AQUA:    0xFFFF00}
	  If HTML.HasKey(RGB)
		 Return HTML[RGB]
	  Return ((RGB & 0xFF0000) >> 16) + (RGB & 0x00FF00) + ((RGB & 0x0000FF) << 16)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetRGB(BGR) {  ; Get numeric RGB value from numeric BGR-Value
	  Return ((BGR & 0xFF0000) >> 16) + (BGR & 0x00FF00) + ((BGR & 0x0000FF) << 16)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetMeasurement() { ; Get locale measurement (metric / inch)
	  ; LOCALE_USER_DEFAULT = 0x0400, LOCALE_IMEASURE = 0x0D, LOCALE_RETURN_NUMBER = 0x20000000
	  Static Metric := 2.54  ; centimeters
		   , Inches := 1.00  ; inches
		   , Measurement := ""
		   , Len := A_IsUnicode ? 2 : 4
	  If (Measurement = "") {
		 VarSetCapacity(LCD, 4, 0)
		 DllCall("Kernel32.dll\GetLocaleInfo", "UInt", 0x400, "UInt", 0x2000000D, "Ptr", &LCD, "Int", Len)
		 Measurement := NumGet(LCD, 0, "UInt") ? Inches : Metric
	  }
	  Return Measurement
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SubclassProc(M, W, L, I, R) { ; RichEdit subclassproc
	  ; Left out first parameter HWND, will be found in "This" when called by system
	  ; See -> http://msdn.microsoft.com/en-us/library/bb776774%28VS.85%29.aspx
	  If (M = 0x87) ; WM_GETDLGCODE
		 Return 4   ; DLGC_WANTALLKEYS
	  Return DllCall("Comctl32.dll\DefSubclassProc", "Ptr", This, "UInt", M, "Ptr", W, "Ptr", L)
   }
   ; ===================================================================================================================
   ; ===================================================================================================================
   ; PUBLIC METHODS ====================================================================================================
   ; ===================================================================================================================
   ; ===================================================================================================================
   ; -------------------------------------------------------------------------------------------------------------------
   ; Methods to be used by advanced users only
   ; -------------------------------------------------------------------------------------------------------------------
   GetCharFormat() { ; Retrieves the character formatting of the current selection.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb787883(v=vs.85).aspx.
	  ; Returns a 'CF2' object containing the formatting settings.
	  ; EM_GETCHARFORMAT = 0x043A
	  CF2 := New This.CF2
	  SendMessage, 0x043A, 1, % CF2.CF2, , % "ahk_id " . This.HWND
	  Return (CF2.Mask ? CF2 : False)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetCharFormat(CF2) { ; Sets character formatting of the current selection.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb787883(v=vs.85).aspx.
	  ; CF2 : CF2 object like returned by GetCharFormat().
	  ; EM_SETCHARFORMAT = 0x0444
	  SendMessage, 0x0444, 1, % CF2.CF2, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetParaFormat() { ; Retrieves the paragraph formatting of the current selection.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb787942(v=vs.85).aspx.
	  ; Returns a 'PF2' object containing the formatting settings.
	  ; EM_GETPARAFORMAT = 0x043D
	  PF2 := New This.PF2
	  SendMessage, 0x043D, 0, % PF2.PF2, , % "ahk_id " . This.HWND
	  Return (PF2.Mask ? PF2 : False)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetParaFormat(PF2) { ; Sets the  paragraph formatting for the current selection.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb787942(v=vs.85).aspx.
	  ; PF2 : PF2 object like returned by GetParaFormat().
	  ; EM_SETPARAFORMAT = 0x0447
	  SendMessage, 0x0447, 0, % PF2.PF2, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Control specific
   ; -------------------------------------------------------------------------------------------------------------------
   IsModified() { ; Has the control been  modified?
	  ; EM_GETMODIFY = 0xB8
	  SendMessage, 0xB8, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetModified(Modified := False) { ; Sets or clears the modification flag for an edit control.
	  ; EM_SETMODIFY = 0xB9
	  SendMessage, 0xB9, % !!Modified, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetEventMask(Events := "") { ; Set the events which shall send notification codes control's owner
	  ; Events : Array containing one or more of the keys defined in 'ENM'.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb774238(v=vs.85).aspx
	  ; EM_SETEVENTMASK	= 	0x0445
	  Static ENM := {NONE: 0x00, CHANGE: 0x01, UPDATE: 0x02, SCROLL: 0x04, SCROLLEVENTS: 0x08, DRAGDROPDONE: 0x10
				   , PARAGRAPHEXPANDED: 0x20, PAGECHANGE: 0x40, KEYEVENTS: 0x010000, MOUSEEVENTS: 0x020000
				   , REQUESTRESIZE: 0x040000, SELCHANGE: 0x080000, DROPFILES: 0x100000, PROTECTED: 0x200000
				   , LINK: 0x04000000}
	  If !IsObject(Events)
		 Events := ["NONE"]
	  Mask := 0
	  For Each, Event In Events {
		 If ENM.HasKey(Event)
			Mask |= ENM[Event]
		 Else
			Return False
	  }
	  SendMessage, 0x0445, 0, %Mask%, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Loading and storing RTF format
   ; -------------------------------------------------------------------------------------------------------------------
   GetRTF(Selection := False) { ; Gets the whole content of the control as rich text.
	  ; Selection = False : whole contents (default)
	  ; Selection = True  : current selection
	  ; EM_STREAMOUT = 0x044A
	  ; SF_TEXT = 0x1, SF_RTF = 0x2, SF_RTFNOOBJS = 0x3, SF_UNICODE = 0x10, SF_USECODEPAGE =	0x0020
	  ; SFF_PLAINRTF = 0x4000, SFF_SELECTION = 0x8000
	  ; UTF-8 = 65001, UTF-16 = 1200
	  Static GetRTFCB := 0
	  Flags := 0x4022 | (1200 << 16) | (Selection ? 0x8000 : 0)
	  GetRTFCB := RegisterCallback("RichEdit.GetRTFProc")
	  VarSetCapacity(ES, (A_PtrSize * 2) + 4, 0) ; EDITSTREAM structure
	  NumPut(This.HWND, ES, 0, "Ptr")            ; dwCookie
	  NumPut(GetRTFCB, ES, A_PtrSize + 4, "Ptr") ; pfnCallback
	  SendMessage, 0x044A, %Flags%, &ES, , % "ahk_id " . This.HWND
	  DllCall("Kernel32.dll\GlobalFree", "Ptr", GetRTFCB)
	  Return This.GetRTFProc("Get", 0, 0)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetRTFProc(pbBuff, cb, pcb) { ; Callback procedure for GetRTF
	  ; left out first parameter dwCookie, will be passed in "This" when called by system
	  Static RTF := ""
	  If (cb > 0) {
		 RTF .= StrGet(pbBuff, cb, "CP0")
		 Return 0
	  }
	  If (pbBuff = "Get") {
		 Out := RTF
		 VarSetCapacity(RTF, 0)
		 Return Out
	  }
	  Return 1
   }
   ; -------------------------------------------------------------------------------------------------------------------
   LoadRTF(FilePath, Selection := False) { ; Loads RTF file into the control.
	  ; FilePath = file path
	  ; Selection = False : whole contents (default)
	  ; Selection = True  : current selection
	  ; EM_STREAMIN = 0x0449
	  ; SF_TEXT = 0x1, SF_RTF = 0x2, SF_RTFNOOBJS = 0x3, SF_UNICODE = 0x10, SF_USECODEPAGE =	0x0020
	  ; SFF_PLAINRTF = 0x4000, SFF_SELECTION = 0x8000
	  ; UTF-16 = 1200
	  Static LoadRTFCB := RegisterCallback("RichEdit.LoadRTFProc")
	  Flags := 0x4002 | (Selection ? 0x8000 : 0) ; | (1200 << 16)
	  If !(File := FileOpen(FilePath, "r"))
		 Return False
	  VarSetCapacity(ES, (A_PtrSize * 2) + 4, 0)  ; EDITSTREAM structure
	  NumPut(File.__Handle, ES, 0, "Ptr")         ; dwCookie
	  NumPut(LoadRTFCB, ES, A_PtrSize + 4, "Ptr") ; pfnCallback
	  SendMessage, 0x0449, %Flags%, &ES, , % "ahk_id " . This.HWND
	  Result := ErrorLevel
	  File.Close()
	  Return Result
   }
   ; -------------------------------------------------------------------------------------------------------------------
   LoadRTFProc(pbBuff, cb, pcb) { ; Callback procedure for LoadRTF
	  ; Left out first parameter dwCookie, will be passed in "This" when called by system
	  Return !DllCall("ReadFile", "Ptr", This, "Ptr", pbBuff, "UInt", cb, "Ptr", pcb, "Ptr", 0)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Scrolling
   ; -------------------------------------------------------------------------------------------------------------------
   GetScrollPos() { ; Obtains the current scroll position.
	  ; Returns on object with keys 'X' and 'Y' containing the scroll position.
	  ; EM_GETSCROLLPOS = 0x04DD
	  VarSetCapacity(PT, 8, 0)
	  SendMessage, 0x04DD, 0, &PT, , % "ahk_id " . This.HWND
	  Return {X: NumGet(PT, 0, "Int"), Y: NumGet(PT, 4, "Int")}
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetScrollPos(X, Y) { ; Scrolls the contents of a rich edit control to the specified point.
	  ; X : x-position to scroll to.
	  ; Y : y-position to scroll to.
	  ; EM_SETSCROLLPOS = 0x04DE
	  VarSetCapacity(PT, 8, 0)
	  NumPut(X, PT, 0, "Int")
	  NumPut(Y, PT, 4, "Int")
	  SendMessage, 0x04DE, 0, &PT, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ScrollCaret() { ; Scrolls the caret into view.
	  ; EM_SCROLLCARET = 0x00B7
	  SendMessage, 0x00B7, 0, 0, , % "ahk_id " . This.HWND
	  Return True
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ShowScrollBar(SB, Mode := True) { ; Shows or hides one of the scroll bars of a rich edit control.
	  ; SB   : Identifies which scroll bar to display: horizontal or vertical.
	  ;        This parameter must be 1 (SB_VERT) or 0 (SB_HORZ).
	  ; Mode : Specify TRUE to show the scroll bar and FALSE to hide it.
	  ; EM_SHOWSCROLLBAR = 0x0460 (WM_USER + 96)
	  SendMessage, 0x0460, %SB%, %Mode%, , % "ahk_id " . This.HWND
	  Return True
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Text and selection
   ; -------------------------------------------------------------------------------------------------------------------
   FindText(Find, Mode := "") { ; Finds Unicode text within a rich edit control.
	  ; Find : Text to search for.
	  ; Mode : Optional array containing one or more of the keys specified in 'FR'.
	  ;        For details see http://msdn.microsoft.com/en-us/library/bb788013(v=vs.85).aspx.
	  ; Returns True if the text was found; otherwise false.
	  ; EM_FINDTEXTEXW = 0x047C, EM_SCROLLCARET = 0x00B7
	  Static FR:= {DOWN: 1, WHOLEWORD: 2, MATCHCASE: 4}
	  Flags := 0
	  For Each, Value In Mode
		 If FR.HasKey(Value)
			Flags |= FR[Value]
	  Sel := This.GetSel()
	  Min := (Flags & FR.DOWN) ? Sel.E : Sel.S
	  Max := (Flags & FR.DOWN) ? -1 : 0
	  VarSetCapacity(FTX, 16 + A_PtrSize, 0)
	  NumPut(Min, FTX, 0, "Int")
	  NumPut(Max, FTX, 4, "Int")
	  NumPut(&Find, FTX, 8, "Ptr")
	  SendMessage, 0x047C, %Flags%, &FTX, , % "ahk_id " . This.HWND
	  S := NumGet(FTX, 8 + A_PtrSize, "Int"), E := NumGet(FTX, 12 + A_PtrSize, "Int")
	  If (S = -1) && (E = -1)
		 Return False
	  This.SetSel(S, E)
	  This.ScrollCaret()
	  Return
   }
   ; -------------------------------------------------------------------------------------------------------------------
   FindWordBreak(CharPos, Mode := "Left") { ; Finds the next word break before or after the specified character position
											; or retrieves information about the character at that position.
	  ; CharPos : Character position.
	  ; Mode    : Can be one of the keys specified in 'WB'.
	  ; Returns the character index of the word break or other values depending on 'Mode'.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb788018(v=vs.85).aspx.
	  ; EM_FINDWORDBREAK = 0x044C (WM_USER + 76)
	  Static WB := {LEFT: 0, RIGHT: 1, ISDELIMITER: 2, CLASSIFY: 3, MOVEWORDLEFT: 4, MOVEWORDRIGHT: 5, LEFTBREAK: 6
				  , RIGHTBREAK: 7}
	  Option := WB.HasKey(Mode) ? WB[Mode] : 0
	  SendMessage, 0x044C, %Option%, %CharPos%, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetSelText() { ; Retrieves the currently selected text as plain text.
	  ; Returns selected text.
	  ; EM_GETSELTEXT = 0x043E, EM_EXGETSEL = 0x0434
	  VarSetCapacity(CR, 8, 0)
	  SendMessage, 0x0434, 0, &CR, , % "ahk_id " . This.HWND
	  L := NumGet(CR, 4, "Int") - NumGet(CR, 0, "Int") + 1
	  If (L > 1) {
		 VarSetCapacity(Text, L * 2, 0)
		 SendMessage, 0x043E, 0, &Text, , % "ahk_id " . This.HWND
		 VarSetCapacity(Text, -1)
	  }
	  Return Text
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetSel() { ; Retrieves the starting and ending character positions of the selection in a rich edit control.
	  ; Returns an object containing the keys S (start of selection) and E (end of selection)).
	  ; EM_EXGETSEL = 0x0434
	  VarSetCapacity(CR, 8, 0)
	  SendMessage, 0x0434, 0, &CR, , % "ahk_id " . This.HWND
	  Return {S: NumGet(CR, 0, "Int"), E: NumGet(CR, 4, "Int")}
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetText() { ; Gets the whole content of the control as plain text.
	  ; EM_GETTEXTEX = 0x045E
	  Text := ""
	  If (Length := This.GetTextLen() * 2) {
		 VarSetCapacity(GTX, (4 * 4) + (A_PtrSize * 2), 0) ; GETTEXTEX structure
		 NumPut(Length + 2, GTX, 0, "UInt") ; cb
		 NumPut(1200, GTX, 8, "UInt")       ; codepage = Unicode
		 VarSetCapacity(Text, Length + 2, 0)
		 SendMessage, 0x045E, &GTX, &Text, , % "ahk_id " . This.HWND
		 VarSetCapacity(Text, -1)
	  }
	  Return Text
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetTextLen() { ; Calculates text length in various ways.
	  ; EM_GETTEXTLENGTHEX = 0x045F
	  VarSetCapacity(GTL, 8, 0)     ; GETTEXTLENGTHEX structure
	  NumPut(1200, GTL, 4, "UInt")  ; codepage = Unicode
	  SendMessage, 0x045F, &GTL, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetTextRange(Min, Max) { ; Retrieves a specified range of characters from a rich edit control.
	  ; Min : Character position index immediately preceding the first character in the range.
	  ;       Integer value to store as cpMin in the CHARRANGE structure.
	  ; Max : Character position immediately following the last character in the range.
	  ;       Integer value to store as cpMax in the CHARRANGE structure.
	  ; CHARRANGE -> http://msdn.microsoft.com/en-us/library/bb787885(v=vs.85).aspx
	  ; EM_GETTEXTRANGE = 0x044B
	  If (Max <= Min)
		 Return ""
	  VarSetCapacity(Text, (Max - Min) << !!A_IsUnicode, 0)
	  VarSetCapacity(TEXTRANGE, 16, 0) ; TEXTRANGE Struktur
	  NumPut(Min, TEXTRANGE, 0, "UInt")
	  NumPut(Max, TEXTRANGE, 4, "UInt")
	  NumPut(&Text, TEXTRANGE, 8, "UPtr")
	  SendMessage, 0x044B, 0, % &TEXTRANGE, , % "ahk_id " . This.HWND
	  VarSetCapacity(Text, -1) ; Lï¿½e des Zeichenspeichers korrigieren 
	  Return Text
   }
   ; -------------------------------------------------------------------------------------------------------------------
   HideSelection(Mode) { ; Hides or shows the selection.
	  ; Mode : True to hide or False to show the selection.
	  ; EM_HIDESELECTION = 0x043F (WM_USER + 63)
	  SendMessage, 0x043F, %Mode%, 0, , % "ahk_id " . This.HWND
	  Return True
   }
   ; -------------------------------------------------------------------------------------------------------------------
   LimitText(Limit)  { ; Sets an upper limit to the amount of text the user can type or paste into a rich edit control.
	  ; Limit : Specifies the maximum amount of text that can be entered.
	  ;         If this parameter is zero, the default maximum is used, which is 64K characters.
	  ; EM_EXLIMITTEXT =  0x435 (WM_USER + 53)
	  SendMessage, 0x0435, 0, %Limit%, , % "ahk_id " . This.HWND
	  Return True
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ReplaceSel(Text := "") { ; Replaces the selected text with the specified text.
	  ; EM_REPLACESEL = 0xC2
	  SendMessage, 0xC2, 1, &Text, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetText(ByRef Text := "", Mode := "") { ; Replaces the selection or the whole content of the control.
	  ; Mode : Option flags. It can be any reasonable combination of the keys defined in 'ST'.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb774284(v=vs.85).aspx.
	  ; EM_SETTEXTEX = 0x0461, CP_UNICODE = 1200
	  ; ST_DEFAULT = 0, ST_KEEPUNDO = 1, ST_SELECTION = 2, ST_NEWCHARS = 4 ???
	  Static ST := {DEFAULT: 0, KEEPUNDO: 1, SELECTION: 2}
	  Flags := 0
	  For Each, Value In Mode
		 If ST.HasKey(Value)
			Flags |= ST[Value]
	  CP := 1200
	  BufAddr := &Text
	  ; RTF formatted text has to be passed as ANSI!!!
	  If (SubStr(Text, 1, 5) = "{\rtf") || (SubStr(Text, 1, 5) = "{urtf") {
		 Len := StrPut(Text, "CP0")
		 VarSetCapacity(Buf, Len, 0)
		 StrPut(Text, &Buf, "CP0")
		 BufAddr := &Buf
		 CP := 0
	  }
	  VarSetCapacity(STX, 8, 0)     ; SETTEXTEX structure
	  NumPut(Flags, STX, 0, "UInt") ; flags
	  NumPut(CP  ,  STX, 4, "UInt") ; codepage
	  SendMessage, 0x0461, &STX, BufAddr, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetSel(Start, End) { ; Selects a range of characters.
	  ; Start : zero-based start index
	  ; End   : zero-beased end index (-1 = end of text))
	  ; EM_EXSETSEL = 0x0437
	  VarSetCapacity(CR, 8, 0)
	  NumPut(Start, CR, 0, "Int")
	  NumPut(End,   CR, 4, "Int")
	  SendMessage, 0x0437, 0, &CR, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Appearance, styles, and options
   ; -------------------------------------------------------------------------------------------------------------------
   AutoURL(On) { ; Turn AutoURLDetection on/off
	  ; EM_AUTOURLDETECT = 0x45B
	  SendMessage, 0x45B, % !!On, 0, , % "ahk_id " . This.HWND
	  WinSet, Redraw, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetOptions() { ; Retrieves rich edit control options.
	  ; Returns an array of currently the set options as the keys defined in 'ECO'.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb774178(v=vs.85).aspx.
	  ; EM_GETOPTIONS = 0x044E
	  Static ECO := {AUTOWORDSELECTION: 0x01, AUTOVSCROLL: 0x40, AUTOHSCROLL: 0x80, NOHIDESEL: 0x100
				   , READONLY: 0x800, WANTRETURN: 0x1000, SAVESEL: 0x8000, SELECTIONBAR: 0x01000000
				   , VERTICAL: 0x400000}
	  SendMessage, 0x044E, 0, 0, , % "ahk_id " . This.HWND
	  O := ErrorLevel
	  Options := []
	  For Key, Value In ECO
		 If (O & Value)
			Options.Insert(Key)
	  Return Options
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetStyles() { ; Retrieves the current edit style flags.
	  ; Returns an object containing keys as defined in 'SES'.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb788031(v=vs.85).aspx.
	  ; EM_GETEDITSTYLE	= 0x04CD (WM_USER + 205)
	  Static SES := {1: "EMULATESYSEDIT", 1: "BEEPONMAXTEXT", 4: "EXTENDBACKCOLOR", 32: "NOXLTSYMBOLRANGE", 64: "USEAIMM"
				   , 128: "NOIME", 256: "ALLOWBEEPS", 512: "UPPERCASE", 1024: "LOWERCASE", 2048: "NOINPUTSEQUENCECHK"
				   , 4096: "BIDI", 8192: "SCROLLONKILLFOCUS", 16384: "XLTCRCRLFTOCR", 32768: "DRAFTMODE"
				   , 0x0010000: "USECTF", 0x0020000: "HIDEGRIDLINES", 0x0040000: "USEATFONT", 0x0080000: "CUSTOMLOOK"
				   , 0x0100000: "LBSCROLLNOTIFY", 0x0200000: "CTFALLOWEMBED", 0x0400000: "CTFALLOWSMARTTAG"
				   , 0x0800000: "CTFALLOWPROOFING"}
	  SendMessage, 0x04CD, 0, 0, , % "ahk_id " . This.HWND
	  Result := ErrorLevel
	  Styles := []
	  For Key, Value In SES
		 If (Result & Key)
			Styles.Insert(Value)
	  Return Styles
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetZoom() { ; Gets the current zoom ratio.
	  ; Returns the zoom ratio in percent.
	  ; EM_GETZOOM = 0x04E0
	  VarSetCapacity(N, 4, 0), VarSetCapacity(D, 4, 0)
	  SendMessage, 0x04E0, &N, &D, , % "ahk_id " . This.HWND
	  N := NumGet(N, 0, "Int"), D := NumGet(D, 0, "Int")
	  Return (N = 0) && (D = 0) ? 100 : Round(N / D * 100)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetBkgndColor(Color) { ; Sets the background color.
	  ; Color : RGB integer value or HTML color name or
	  ;         "Auto" to reset to system default color.
	  ; Returns the prior background color.
	  ; EM_SETBKGNDCOLOR = 0x0443
	  If (Color = "Auto")
		 System := True, Color := 0
	  Else
		 System := False, Color := This.GetBGR(Color)
	  SendMessage, 0x0443, %System%, %Color%, , % "ahk_id " . This.HWND
	  Return This.GetRGB(ErrorLevel)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetOptions(Options, Mode := "SET") { ; Sets the options for a rich edit control.
	  ; Options : Array of options as the keys defined in 'ECO'.
	  ; Mode    : Settings mode: SET, OR, AND, XOR
	  ; For details see http://msdn.microsoft.com/en-us/library/bb774254(v=vs.85).aspx.
	  ; EM_SETOPTIONS = 0x044D
	  Static ECO := {AUTOWORDSELECTION: 0x01, AUTOVSCROLL: 0x40, AUTOHSCROLL: 0x80, NOHIDESEL: 0x100, READONLY: 0x800
				   , WANTRETURN: 0x1000, SAVESEL: 0x8000, SELECTIONBAR: 0x01000000, VERTICAL: 0x400000}
		   , ECOOP := {SET: 0x01, OR: 0x02, AND: 0x03, XOR: 0x04}
	  If !ECOOP.HasKey(Mode)
		 Return False
	  O := 0
	  For Each, Option In Options {
		 If ECO.HasKey(Option)
			O |= ECO[Option]
		 Else
			Return False
	  }
	  SendMessage, 0x044D, % ECOOP[Mode], %O%, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetStyles(Styles) { ; Sets the current edit style flags for a rich edit control.
	  ; Styles : Object containing on or more of the keys defined in 'SES'.
	  ;          If the value is 0 the style will be removed, otherwise it will be added.
	  ; For details see http://msdn.microsoft.com/en-us/library/bb774236(v=vs.85).aspx.
	  ; EM_SETEDITSTYLE	= 0x04CC (WM_USER + 204)
	  Static SES = {EMULATESYSEDIT: 1, BEEPONMAXTEXT: 2, EXTENDBACKCOLOR: 4, NOXLTSYMBOLRANGE: 32, USEAIMM: 64
				  , NOIME: 128, ALLOWBEEPS: 256, UPPERCASE: 512, LOWERCASE: 1024, NOINPUTSEQUENCECHK: 2048
				  , BIDI: 4096, SCROLLONKILLFOCUS: 8192, XLTCRCRLFTOCR: 16384, DRAFTMODE: 32768
				  , USECTF: 0x0010000, HIDEGRIDLINES: 0x0020000, USEATFONT: 0x0040000, CUSTOMLOOK: 0x0080000
				  , LBSCROLLNOTIFY: 0x0100000, CTFALLOWEMBED: 0x0200000, CTFALLOWSMARTTAG: 0x0400000
				  , CTFALLOWPROOFING: 0x0800000}
	  Flags := Mask := 0
	  For Style, Value In Styles {
		 If SES.HasKey(Style) {
			Mask |= SES[Style]
			If (Value <> 0)
			   Flags |= SES[Style]
		 }
	  }
	  If (Mask) {
		 SendMessage, 0x04CC, %Flags%, %Mask%, ,, % "ahk_id " . This.HWND
		 Return ErrorLevel
	  }
	  Return False
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetZoom(Ratio := "") { ; Sets the zoom ratio of a rich edit control.
	  ; Ratio : Float value between 100/64 and 6400; a ratio of 0 turns zooming off.
	  ; EM_SETZOOM = 0x4E1
	  SendMessage, 0x4E1, % (Ratio > 0 ? Ratio : 100), 100, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Copy, paste, etc.
   ; -------------------------------------------------------------------------------------------------------------------
   CanRedo() { ; Determines whether there are any actions in the control redo queue.
	  ; EM_CANREDO = 0x0455 (WM_USER + 85)
	  SendMessage, 0x0455, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   CanUndo() { ; Determines whether there are any actions in an edit control's undo queue.
	  ; EM_CANUNDO = 0x00C6
	  SendMessage, 0x00C6, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   Clear() {
	  ; WM_CLEAR = 0x303
	  SendMessage, 0x303, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   Copy() {
	  ; WM_COPY = 0x301
	  SendMessage, 0x301, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   Cut() {
	  ; WM_CUT = 0x300
	  SendMessage, 0x300, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   Paste() {
	  ; WM_PASTE = 0x302
	  SendMessage, 0x302, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   Redo() {
	  ; EM_REDO := 0x454
	  SendMessage, 0x454, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   Undo() {
	  ; EM_UNDO = 0xC7
	  SendMessage, 0xC7, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SelAll() {
	  ; Select all
	  Return This.SetSel(0, -1)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   Deselect() {
	  ; Deselect all
	  Sel := This.GetSel()
	  Return This.SetSel(Sel.S, Sel.S)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Font & colors
   ; -------------------------------------------------------------------------------------------------------------------
   ChangeFontSize(Diff) { ; Change font size
	  ; Diff : any positive or negative integer, positive values are treated as +1, negative as -1.
	  ; Returns new size.
	  ; EM_SETFONTSIZE = 0x04DF
	  ; Font size changes by 1 in the range 4 - 11 pt, by 2 for 12 - 28 pt, afterward to 36 pt, 48 pt, 72 pt, 80 pt,
	  ; and by 10 for > 80 pt. The maximum value is 160 pt, the minimum is 4 pt
	  Font := This.GetFont()
	  If (Diff > 0 && Font.Size < 160) || (Diff < 0 && Font.Size > 4)
		 SendMessage, 0x04DF, % (Diff > 0 ? 1 : -1), 0, , % "ahk_id " . This.HWND
	  Else
		 Return False
	  Font := This.GetFont()
	  Return Font.Size
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetFont(Default := False) { ; Get current font
	  ; Set Default to True to get the default font.
	  ; Returns an object containing current options (see SetFont())
	  ; EM_GETCHARFORMAT = 0x043A
	  ; BOLD_FONTTYPE = 0x0100, ITALIC_FONTTYPE = 0x0200
	  ; CFM_BOLD = 1, CFM_ITALIC = 2, CFM_UNDERLINE = 4, CFM_STRIKEOUT = 8, CFM_PROTECTED = 16, CFM_SUBSCRIPT = 0x30000
	  ; CFM_BACKCOLOR = 0x04000000, CFM_CHARSET := 0x08000000, CFM_FACE = 0x20000000, CFM_COLOR = 0x40000000
	  ; CFM_SIZE = 0x80000000
	  ; CFE_SUBSCRIPT = 0x10000, CFE_SUPERSCRIPT = 0x20000, CFE_AUTOBACKCOLOR = 0x04000000, CFE_AUTOCOLOR = 0x40000000
	  ; SCF_SELECTION = 1
	  Static Mask := 0xEC03001F
	  Static Effects := 0xEC000000
	  CF2 := New This.CF2
	  CF2.Mask := Mask
	  CF2.Effects := Effects
	  SendMessage, 0x043A, % (Default ? 0 : 1), % CF2.CF2, , % "ahk_id " . This.HWND
	  Font := {}
	  Font.Name := CF2.FaceName
	  Font.Size := CF2.Height / 20
	  CFS := CF2.Effects
	  Style := (CFS & 1 ? "B" : "") . (CFS & 2 ? "I" : "") . (CFS & 4 ? "U" : "") . (CFS & 8 ? "S" : "")
			 . (CFS & 0x10000 ? "L" : "") . (CFS & 0x20000 ? "H" : "") . (CFS & 16 ? "P" : "")
	  Font.Style := Style = "" ? "N" : Style
	  Font.Color := This.GetRGB(CF2.TextColor)
	  If (CF2.Effects & 0x04000000)
		 Font.BkColor := "Auto"
	  Else
		 Font.BkColor := This.GetRGB(CF2.BackColor)
	  Font.CharSet := CF2.CharSet
	  Return Font
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetDefaultFont(Font := "") { ; Set default font
	  ; Font : Optional object - see SetFont().
	  If IsObject(Font) {
		 For Key, Value In Font
			If This.DefFont.HasKey(Key)
			   This.DefFont[Key] := Value
	  }
	  Return This.SetFont(This.DefFont)
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetFont(Font) { ; Set current/default font
	  ; Font : Object containing the following keys
	  ;        Name    : optional font name
	  ;        Size    : optional font size in points
	  ;        Style   : optional string of one or more of the following styles
	  ;                  B = bold, I = italic, U = underline, S = strikeout, L = subscript
	  ;                  H = superschript, P = protected, N = normal
	  ;        Color   : optional text color as RGB integer value or HTML color name
	  ;                  "Auto" for "automatic" (system's default) color
	  ;        BkColor : optional text background color (see Color)
	  ;                  "Auto" for "automatic" (system's default) background color
	  ;        CharSet : optional font character set
	  ;                  1 = DEFAULT_CHARSET, 2 = SYMBOL_CHARSET
	  ;        Empty parameters preserve the corresponding properties
	  ; EM_SETCHARFORMAT = 0x0444
	  ; SCF_DEFAULT = 0, SCF_SELECTION = 1
	  CF2 := New This.CF2
	  Mask := Effects := 0
	  If (Font.Name != "") {
		 Mask |= 0x20000000, Effects |= 0x20000000 ; CFM_FACE, CFE_FACE
		 CF2.FaceName := Font.Name
	  }
	  Size := Font.Size
	  If (Size != "") {
		 If (Size < 161)
			Size *= 20
		 Mask |= 0x80000000, Effects |= 0x80000000 ; CFM_SIZE, CFE_SIZE
		 CF2.Height := Size
	  }
	  If (Font.Style != "") {
		 Mask |= 0x3001F           ; all font styles
		 If InStr(Font.Style, "B")
			Effects |= 1           ; CFE_BOLD
		 If InStr(Font.Style, "I")
			Effects |= 2           ; CFE_ITALIC
		 If InStr(Font.Style, "U")
			Effects |= 4           ; CFE_UNDERLINE
		 If InStr(Font.Style, "S")
			Effects |= 8           ; CFE_STRIKEOUT
		 If InStr(Font.Style, "P")
			Effects |= 16          ; CFE_PROTECTED
		 If InStr(Font.Style, "L")
			Effects |= 0x10000     ; CFE_SUBSCRIPT
		 If InStr(Font.Style, "H")
			Effects |= 0x20000     ; CFE_SUPERSCRIPT
	  }
	  If (Font.Color != "") {
		 Mask |= 0x40000000        ; CFM_COLOR
		 If (Font.Color = "Auto")
			Effects |= 0x40000000  ; CFE_AUTOCOLOR
		 Else
			CF2.TextColor := This.GetBGR(Font.Color)
	  }
	  If (Font.BkColor != "") {
		 Mask |= 0x04000000        ; CFM_BACKCOLOR
		 If (Font.BkColor = "Auto")
			Effects |= 0x04000000  ; CFE_AUTOBACKCOLOR
		 Else
			CF2.BackColor := This.GetBGR(Font.BkColor)
	  }
	  If (Font.CharSet != "") {
		 Mask |= 0x08000000, Effects |= 0x08000000 ; CFM_CHARSET, CFE_CHARSET
		 CF2.CharSet := Font.CharSet = 2 ? 2 : 1 ; SYMBOL|DEFAULT
	  }
	  If (Mask != 0) {
		 Mode := Font.Default ? 0 : 1
		 CF2.Mask := Mask
		 CF2.Effects := Effects
		 SendMessage, 0x0444, %Mode%, % CF2.CF2, , % "ahk_id " . This.HWND
		 Return ErrorLevel
	  }
	  Return False
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ToggleFontStyle(Style) { ; Toggle single font style
	  ; Style : one of the following styles
	  ;         B = bold, I = italic, U = underline, S = strikeout, L = subscript, H = superschript, P = protected,
	  ;         N = normal (reset all other styles)
	  ; EM_GETCHARFORMAT = 0x043A, EM_SETCHARFORMAT = 0x0444
	  ; CFM_BOLD = 1, CFM_ITALIC = 2, CFM_UNDERLINE = 4, CFM_STRIKEOUT = 8, CFM_PROTECTED = 16, CFM_SUBSCRIPT = 0x30000
	  ; CFE_SUBSCRIPT = 0x10000, CFE_SUPERSCRIPT = 0x20000, SCF_SELECTION = 1
	  CF2 :=This.GetCharFormat()
	  CF2.Mask := 0x3001F ; FontStyles
	  If (Style = "N")
		 CF2.Effects := 0
	  Else
		 CF2.Effects ^= Style = "B" ? 1 : Style = "I" ? 2 : Style = "U" ? 4 : Style = "S" ? 8
					  : Style = "H" ? 0x20000 : Style = "L" ? 0x10000 : 0
	  SendMessage, 0x0444, 1, % CF2.CF2, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Paragraph formatting
   ; -------------------------------------------------------------------------------------------------------------------
   AlignText(Align := 1) { ; Set paragraph's alignment
	  ; Note: Values greater 3 doesn't seem to work though they should as documented
	  ; Align: may contain one of the following numbers:
	  ;        PFA_LEFT             1
	  ;        PFA_RIGHT            2
	  ;        PFA_CENTER           3
	  ;        PFA_JUSTIFY          4 // New paragraph-alignment option 2.0 (*)
	  ;        PFA_FULL_INTERWORD   4 // These are supported in 3.0 with advanced
	  ;        PFA_FULL_INTERLETTER 5 // typography enabled
	  ;        PFA_FULL_SCALED      6
	  ;        PFA_FULL_GLYPHS      7
	  ;        PFA_SNAP_GRID        8
	  ; EM_SETPARAFORMAT = 0x0447, PFM_ALIGNMENT = 0x08
	  If (Align >= 1) && (ALign <= 8) {
		 PF2 := New This.PF2    ; PARAFORMAT2 struct
		 PF2.Mask := 0x08       ; dwMask
		 PF2.Alignment := Align ; wAlignment
		 SendMessage, 0x0447, 0, % PF2.PF2, , % "ahk_id " . This.HWND
		 Return True
	  }
	  Return False
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetBorder(Widths, Styles) { ; Set paragraph's borders
	  ; Borders are not displayed in RichEdit, so the call of this function has no visible result.
	  ; Even WordPad distributed with Win7 does not show them, but e.g. Word 2007 does.
	  ; Widths : Array of the 4 border widths in the range of 1 - 15 in order left, top, right, bottom; zero = no border
	  ; Styles : Array of the 4 border styles in the range of 0 - 7 in order left, top, right, bottom (see remarks)
	  ; Note:
	  ; The description on MSDN at http://msdn.microsoft.com/en-us/library/bb787942(v=vs.85).aspx is wrong!
	  ; To set borders you have to put the border width into the related nibble (4 Bits) of wBorderWidth
	  ; (in order: left (0 - 3), top (4 - 7), right (8 - 11), and bottom (12 - 15). The values are interpreted as
	  ; half points (i.e. 10 twips). Border styles are set in the related nibbles of wBorders.
	  ; Valid styles seem to be:
	  ;     0 : \brdrdash (dashes)
	  ;     1 : \brdrdashsm (small dashes)
	  ;     2 : \brdrdb (double line)
	  ;     3 : \brdrdot (dotted line)
	  ;     4 : \brdrhair (single/hair line)
	  ;     5 : \brdrs ? looks like 3
	  ;     6 : \brdrth ? looks like 3
	  ;     7 : \brdrtriple (triple line)
	  ; EM_SETPARAFORMAT = 0x0447, PFM_BORDER = 0x800
	  If !IsObject(Widths)
		 Return False
	  W := S := 0
	  For I, V In Widths {
		 If (V)
			W |= V << ((A_Index - 1) * 4)
		 If Styles[I]
			S |= Styles[I] << ((A_Index - 1) * 4)
	  }
	  PF2 := New This.PF2
	  PF2.Mask := 0x800
	  PF2.BorderWidth := W
	  PF2.Borders := S
	  SendMessage, 0x0447, 0, % PF2.PF2, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetLineSpacing(Lines) { ; Sets paragraph's line spacing.
	  ; Lines : number of lines as integer or float.
	  ; SpacingRule = 5:
	  ; The value of dyLineSpacing / 20 is the spacing, in lines, from one line to the next. Thus, setting
	  ; dyLineSpacing to 20 produces single-spaced text, 40 is double spaced, 60 is triple spaced, and so on.
	  ; EM_SETPARAFORMAT = 0x0447, PFM_LINESPACING = 0x100
	  PF2 := New This.PF2
	  PF2.Mask := 0x100
	  PF2.LineSpacing := Abs(Lines) * 20
	  PF2.LineSpacingRule := 5
	  SendMessage, 0x0447, 0, % PF2.PF2, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetParaIndent(Indent := "Reset") { ; Sets space left/right of the paragraph.
	  ; Indent : Object containing up to three keys:
	  ;          - Start  : Optional - Absolute indentation of the paragraph's first line.
	  ;          - Right  : Optional - Indentation of the right side of the paragraph, relative to the right margin.
	  ;          - Offset : Optional - Indentation of the second and subsequent lines, relative to the indentation
	  ;                                of the first line.
	  ;          Values are interpreted as centimeters/inches depending on the user's locale measurement settings.
	  ;          Call without passing a parameter to reset indentation.
	  ; EM_SETPARAFORMAT = 0x0447
	  ; PFM_STARTINDENT  = 0x0001
	  ; PFM_RIGHTINDENT  = 0x0002
	  ; PFM_OFFSET       = 0x0004
	  Static PFM := {STARTINDENT: 0x01, RIGHTINDENT: 0x02, OFFSET: 0x04}
	  Measurement := This.GetMeasurement()
	  PF2 := New This.PF2
	  If (Indent = "Reset")
		 PF2.Mask := 0x07 ; reset indentation
	  Else If !IsObject(Indent)
		 Return False
	  Else {
		 PF2.Mask := 0
		 If (Indent.HasKey("Start")) {
			PF2.Mask |= PFM.STARTINDENT
			PF2.StartIndent := Round((Indent.Start / Measurement) * 1440)
		 }
		 If (Indent.HasKey("Offset")) {
			PF2.Mask |= PFM.OFFSET
			PF2.Offset := Round((Indent.Offset / Measurement) * 1440)
		 }
		 If (Indent.HasKey("Right")) {
			PF2.Mask |= PFM.RIGHTINDENT
			PF2.RightIndent := Round((Indent.Right / Measurement) * 1440)
		 }
	  }
	  If (PF2.Mask) {
		 SendMessage, 0x0447, 0, % PF2.PF2, , % "ahk_id " . This.HWND
		 Return ErrorLevel
	  }
	  Return False
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetParaNumbering(Numbering := "Reset") {
	  ; Numbering : Object containing up to four keys:
	  ;             - Type  : Options used for bulleted or numbered paragraphs.
	  ;             - Style : Optional - Numbering style used with numbered paragraphs.
	  ;             - Tab   : Optional - Minimum space between a paragraph number and the paragraph text.
	  ;             - Start : Optional - Sequence number used for numbered paragraphs (e.g. 3 for C or III)
	  ;             Tab is interpreted as centimeters/inches depending on the user's locale measurement settings.
	  ;             Call without passing a parameter to reset numbering.
	  ; EM_SETPARAFORMAT = 0x0447
	  ; PARAFORMAT numbering options
	  ; PFN_BULLET   1 ; tomListBullet
	  ; PFN_ARABIC   2 ; tomListNumberAsArabic:   0, 1, 2,	...
	  ; PFN_LCLETTER 3 ; tomListNumberAsLCLetter: a, b, c,	...
	  ; PFN_UCLETTER 4 ; tomListNumberAsUCLetter: A, B, C,	...
	  ; PFN_LCROMAN  5 ; tomListNumberAsLCRoman:  i, ii, iii,	...
	  ; PFN_UCROMAN  6 ; tomListNumberAsUCRoman:  I, II, III,	...
	  ; PARAFORMAT2 wNumberingStyle options
	  ; PFNS_PAREN     0x0000 ; default, e.g.,                 1)
	  ; PFNS_PARENS    0x0100 ; tomListParentheses/256, e.g., (1)
	  ; PFNS_PERIOD    0x0200 ; tomListPeriod/256, e.g.,       1.
	  ; PFNS_PLAIN     0x0300 ; tomListPlain/256, e.g.,        1
	  ; PFNS_NONUMBER  0x0400 ; used for continuation w/o number
	  ; PFNS_NEWNUMBER 0x8000 ; start new number with wNumberingStart
	  ; PFM_NUMBERING      0x0020
	  ; PFM_NUMBERINGSTYLE 0x2000
	  ; PFM_NUMBERINGTAB   0x4000
	  ; PFM_NUMBERINGSTART 0x8000
	  Static PFM := {Type: 0x0020, Style: 0x2000, Tab: 0x4000, Start: 0x8000}
	  Static PFN := {Bullet: 1, Arabic: 2, LCLetter: 3, UCLetter: 4, LCRoman: 5, UCRoman: 6}
	  Static PFNS := {Paren: 0x0000, Parens: 0x0100, Period: 0x0200, Plain: 0x0300, None: 0x0400, New: 0x8000}
	  PF2 := New This.PF2
	  If (Numbering = "Reset")
		 PF2.Mask := 0xE020
	  Else If !IsObject(Numbering)
		 Return False
	  Else {
		 If (Numbering.HasKey("Type")) {
			PF2.Mask |= PFM.Type
			PF2.Numbering := PFN[Numbering.Type]
		 }
		 If (Numbering.HasKey("Style")) {
			PF2.Mask |= PFM.Style
			PF2.NumberingStyle := PFNS[Numbering.Style]
		 }
		 If (Numbering.HasKey("Tab")) {
			PF2.Mask |= PFM.Tab
			PF2.NumberingTab := Round((Numbering.Tab / This.GetMeasurement()) * 1440)
		 }
		 If (Numbering.HasKey("Start")) {
			PF2.Mask |= PFM.Start
			PF2.NumberingStart := Numbering.Start
		 }
	  }
	  If (PF2.Mask) {
		 SendMessage, 0x0447, 0, % PF2.PF2, , % "ahk_id " . This.HWND
		 Return ErrorLevel
	  }
	  Return False
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetParaSpacing(Spacing := "Reset") { ; Set space before / after the paragraph
	  ; Spacing : Object containing one or two keys:
	  ;           - Before : additional space before the paragraph in points
	  ;           - After  : additional space after the paragraph in points
	  ;           Call without passing a parameter to reset spacing to zero.
	  ; EM_SETPARAFORMAT = 0x0447
	  ; PFM_SPACEBEFORE  = 0x0040
	  ; PFM_SPACEAFTER   = 0x0080
	  Static PFM := {Before: 0x40, After: 0x80}
	  PF2 := New This.PF2
	  If (Spacing = "Reset")
		 PF2.Mask := 0xC0 ; reset spacing
	  Else If !IsObject(Spacing)
		 Return False
	  Else {
		 If (Spacing.Before >= 0) {
			PF2.Mask |= PFM.Before
			PF2.SpaceBefore := Round(Spacing.Before * 20)
		 }
		 If (Spacing.After >= 0) {
			PF2.Mask |= PFM.After
			PF2.SpaceAfter := Round(Spacing.After * 20)
		 }
	  }
	  If (PF2.Mask) {
		 SendMessage, 0x0447, 0, % PF2.PF2, , % "ahk_id " . This.HWND
		 Return ErrorLevel
	  }
	  Return False
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetDefaultTabs(Distance) { ; Set default tabstops
	  ; Distance will be interpreted as inches or centimeters depending on the current user's locale.
	  ; EM_SETTABSTOPS = 0xCB
	  Static DUI := 64      ; dialog units per inch
		   , MinTab := 0.20 ; minimal tab distance
		   , MaxTab := 3.00 ; maximal tab distance
	  IM := This.GetMeasurement()
	  StringReplace, Distance, Distance, `,, .
	  Distance := Round(Distance / IM, 2)
	  If (Distance < MinTab)
		 Distance := MinTab
	  If (Distance > MaxTab)
		 Distance := MaxTab
	  VarSetCapacity(TabStops, 4, 0)
	  NumPut(Round(DUI * Distance), TabStops, "Int")
	  SendMessage, 0xCB, 1, &TabStops, , % "ahk_id " . This.HWND
	  Result := ErrorLevel
	  DllCall("User32.dll\UpdateWindow", "Ptr", This.HWND)
	  Return Result
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SetTabStops(TabStops := "Reset") { ; Set paragraph's tabstobs
	  ; TabStops is an object containing the integer position as hundredth of inches/centimeters as keys
	  ; and the alignment ("L", "C", "R", or "D") as values.
	  ; The position will be interpreted as hundredth of inches or centimeters depending on the current user's locale.
	  ; Call without passing a  parameter to reset to default tabs.
	  ; EM_SETPARAFORMAT = 0x0447, PFM_TABSTOPS = 0x10
	  Static MinT := 30                ; minimal tabstop in hundredth of inches
	  Static MaxT := 830               ; maximal tabstop in hundredth of inches
	  Static Align := {L: 0x00000000   ; left aligned (default)
					 , C: 0x01000000   ; centered
					 , R: 0x02000000   ; right aligned
					 , D: 0x03000000}  ; decimal tabstop
	  Static MAX_TAB_STOPS := 32
	  IC := This.GetMeasurement()
	  PF2 := New This.PF2
	  PF2.Mask := 0x10
	  If (TabStops = "Reset") {
		 SendMessage, 0x0447, 0, % PF2.PF2, , % "ahk_id " . This.HWND
		 Return !!(ErrorLevel)
	  }
	  If !IsObject(TabStops)
		 Return False
	  TabCount := 0
	  Tabs  := []
	  For Position, Alignment In TabStops {
		 Position /= IC
		 If (Position < MinT) Or (Position > MaxT)
		 Or !Align.HasKey(Alignment) Or (A_Index > MAX_TAB_STOPS)
			Return False
		 Tabs[A_Index] := (Align[Alignment] | Round((Position / 100) * 1440))
		 TabCount := A_Index
	  }
	  If (TabCount) {
		 PF2.TabCount := TabCount
		 PF2.Tabs := Tabs
		 SendMessage, 0x0447, 0, % PF2.PF2, , % "ahk_id " . This.HWND
		 Return ErrorLevel
	  }
	  Return False
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Line handling
   ; -------------------------------------------------------------------------------------------------------------------
   GetLineCount() { ; Get the total number of lines

	  ; EM_GETLINECOUNT = 0xBA
	  SendMessage, 0xBA, 0, 0, , % "ahk_id " . This.HWND
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetCaretLine() { ; Get the line containing the caret
	  ; EM_LINEINDEX = 0xBB, EM_EXLINEFROMCHAR = 0x0436
	  SendMessage, 0xBB, -1, 0, , % "ahk_id " . This.HWND
	  SendMessage, 0x0436, 0, %ErrorLevel%, , % "ahk_id " . This.HWND
	  Return ErrorLevel + 1
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Statistics
   ; -------------------------------------------------------------------------------------------------------------------
   GetStatistics() { ; Get some statistic values
	  ; Get the line containing the caret, it's position in this line, the total amount of lines, the absulute caret
	  ; position and the total amount of characters.
	  ; EM_GETSEL = 0xB0, EM_LINEFROMCHAR = 0xC9, EM_LINEINDEX = 0xBB, EM_GETLINECOUNT = 0xBA
	  Stats := {}
	  VarSetCapacity(GTL, 8, 0)  ; GETTEXTLENGTHEX structure
	  SB := 0
	  SendMessage, 0xB0, &SB, 0, , % "ahk_id " . This.HWND
	  SB := NumGet(SB, 0, "UInt") + 1
	  SendMessage, 0xBB, -1, 0, , % "ahk_id " . This.HWND
	  Stats.LinePos := SB - ErrorLevel
	  SendMessage, 0xC9, -1, 0, , % "ahk_id " . This.HWND
	  Stats.Line := ErrorLevel + 1
	  SendMessage, 0xBA, 0, 0, , % "ahk_id " . This.HWND
	  Stats.LineCount := ErrorLevel
	  Stats.CharCount := This.GetTextLen()
	  Return Stats
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Layout
   ; -------------------------------------------------------------------------------------------------------------------
   WordWrap(On) { ; Turn wordwrapping on/off
	  ; EM_SCROLLCARET = 0xB7, EM_SETTARGETDEVICE = 0x0448
	  Sel := This.GetSel()
	  SendMessage, 0x0448, 0, % (On ? 0 : -1), , % "ahk_id " . This.HWND
	  This.SetSel(Sel.S, Sel.E)
	  SendMessage, 0xB7, 0, 0,  % "ahk_id " . This.HWND
	  Return On
   }
   ; -------------------------------------------------------------------------------------------------------------------
   WYSIWYG(On) { ; Show control as printed (WYSIWYG)
	  ; Text measuring is based on the default printer's capacities, thus changing the printer may produce different
	  ; results. See remarks/comments in Print() also.
	  ; EM_SCROLLCARET = 0xB7, EM_SETTARGETDEVICE = 0x0448
	  ; PD_RETURNDC = 0x0100, PD_RETURNDEFAULT = 0x0400
	  Static PDC := 0
	  Static PD_Size := (A_PtrSize = 4 ? 66 : 120)
	  Static OffFlags := A_PtrSize * 5
	  Sel := This.GetSel()
	  If !(On) {
		 DllCall("User32.dll\LockWindowUpdate", "Ptr", This.HWND)
		 DllCall("Gdi32.dll\DeleteDC", "Ptr", PDC)
		 SendMessage, 0x0448, 0, -1, , % "ahk_id " . This.HWND
		 This.SetSel(Sel.S, Sel.E)
		 SendMessage, 0xB7, 0, 0,  % "ahk_id " . This.HWND
		 DllCall("User32.dll\LockWindowUpdate", "Ptr", 0)
		 Return ErrorLevel
	  }
	  Numput(VarSetCapacity(PD, PD_Size, 0), PD)
	  NumPut(0x0100 | 0x0400, PD, A_PtrSize * 5, "UInt") ; PD_RETURNDC | PD_RETURNDEFAULT
	  If !DllCall("Comdlg32.dll\PrintDlg", "Ptr", &PD, "Int")
		 Return
	  DllCall("Kernel32.dll\GlobalFree", "Ptr", NumGet(PD, A_PtrSize * 2, "UPtr"))
	  DllCall("Kernel32.dll\GlobalFree", "Ptr", NumGet(PD, A_PtrSize * 3, "UPtr"))
	  PDC := NumGet(PD, A_PtrSize * 4, "UPtr")
	  DllCall("User32.dll\LockWindowUpdate", "Ptr", This.HWND)
	  Caps := This.GetPrinterCaps(PDC)
	  ; Set up page size and margins in pixel
	  UML := This.Margins.LT                   ; user margin left
	  UMR := This.Margins.RT                   ; user margin right
	  PML := Caps.POFX                         ; physical margin left
	  PMR := Caps.PHYW - Caps.HRES - Caps.POFX ; physical margin right
	  LPW := Caps.HRES                         ; logical page width
	  ; Adjust margins
	  UML := UML > PML ? (UML - PML) : 0
	  UMR := UMR > PMR ? (UMR - PMR) : 0
	  LineLen := LPW - UML - UMR
	  SendMessage, 0x0448, %PDC%, %LineLen%, , % "ahk_id " . This.HWND
	  This.SetSel(Sel.S, Sel.E)
	  SendMessage, 0xB7, 0, 0,  % "ahk_id " . This.HWND
	  DllCall("User32.dll\LockWindowUpdate", "Ptr", 0)
	  Return ErrorLevel
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; File handling
   ; -------------------------------------------------------------------------------------------------------------------
   LoadFile(File, Mode = "Open") { ; Load file
	  ; File : file name
	  ; Mode : Open / Add / Insert
	  ;        Open   : Replace control's content
	  ;        Append : Append to conrol's content
	  ;        Insert : Insert at / replace current selection
	  If !FileExist(File)
		 Return False
	  SplitPath, File, , , Ext
	  If (Ext = "rtf") {
		 If (Mode = "Open") {
			Selection := False
		 } Else If (Mode = "Insert") {
			Selection := True
		 } Else If (Mode = "Append") {
			This.SetSel(-1, -2)
			Selection := True
		 }
		 This.LoadRTF(File, Selection)
	  } Else {
		 FileRead, Text, %File%
		 If (Mode = "Open") {
			This.SetText(Text)
		 } Else If (Mode = "Insert") {
			This.ReplaceSel(Text)
		 } Else If (Mode = "Append") {
			This.SetSel(-1, -2)
			This.ReplaceSel(Text)
		 }
	  }
	  Return True
   }
   ; -------------------------------------------------------------------------------------------------------------------
   SaveFile(File) { ; Save file
	  ; File : file name
	  ; Returns True on success, otherwise False.
	  GuiName := This.GuiName
	  Gui, %GuiName%:+OwnDialogs
	  SplitPath, File, , , Ext
	  Text := Ext = "rtf" ? This.GetRTF() : This.GetText()
	  If IsObject(FileObj := FileOpen(File, "w")) {
		 FileObj.Write(Text)
		 FileObj.Close()
		 Return True
	  }
	  Return False
   }
   ; -------------------------------------------------------------------------------------------------------------------
   ; Printing
   ; THX jballi ->  http://www.autohotkey.com/board/topic/45513-function-he-print-wysiwyg-print-for-the-hiedit-control/
   ; -------------------------------------------------------------------------------------------------------------------
   Print() {
	  ; ----------------------------------------------------------------------------------------------------------------
	  ; Static variables
	  Static PD_ALLPAGES := 0x00, PD_SELECTION := 0x01, PD_PAGENUMS := 0x02, PD_NOSELECTION := 0x04
		   , PD_RETURNDC := 0x0100, PD_USEDEVMODECOPIES := 0x040000, PD_HIDEPRINTTOFILE := 0x100000
		   , PD_NONETWORKBUTTON := 0x200000, PD_NOCURRENTPAGE := 0x800000
		   , MM_TEXT := 0x1
		   , EM_FORMATRANGE := 0x0439, EM_SETTARGETDEVICE := 0x0448
		   , DocName := "AHKRichEdit"
		   , PD_Size := (A_PtrSize = 8 ? (13 * A_PtrSize) + 16 : 66)
	  ErrorMsg := ""
	  ; ----------------------------------------------------------------------------------------------------------------
	  ; Prepare to call PrintDlg
	  ; Define/Populate the PRINTDLG structure
	  VarSetCapacity(PD, PD_Size, 0)
	  Numput(PD_Size, PD, 0, "UInt")  ; lStructSize
	  Numput(This.GuiHwnd, PD, A_PtrSize, "UPtr") ; hwndOwner
	  ; Collect Start/End select positions
	  Sel := This.GetSel()
	  ; Determine/Set Flags
	  Flags := PD_ALLPAGES | PD_RETURNDC | PD_USEDEVMODECOPIES | PD_HIDEPRINTTOFILE | PD_NONETWORKBUTTON
			 | PD_NOCURRENTPAGE
	  If (Sel.S = Sel.E)
		 Flags |= PD_NOSELECTION
	  Else
		 Flags |= PD_SELECTION
	  Offset := A_PtrSize * 5
	  NumPut(Flags, PD, Offset, "UInt")       ; Flags
	  ; Page and copies
	  NumPut( 1, PD, Offset += 4, "UShort")   ; nFromPage
	  NumPut( 1, PD, Offset += 2, "UShort")   ; nToPage
	  NumPut( 1, PD, Offset += 2, "UShort")   ; nMinPage
	  NumPut(-1, PD, Offset += 2, "UShort")   ; nMaxPage
	  NumPut( 1, PD, Offset += 2, "UShort")   ; nCopies
	  ; Note: Use -1 to specify the maximum page number (65535).
	  ; Programming note: The values that are loaded to these fields are critical. The Print dialog will not
	  ; display (returns an error) if unexpected values are loaded to one or more of these fields.
	  ; ----------------------------------------------------------------------------------------------------------------
	  ; Print dialog box
	  ; Open the Print dialog.  Bounce If the user cancels.
	  If !DllCall("Comdlg32.dll\PrintDlg", "Ptr", &PD, "UInt") {
		 ErrorLevel := "Function: " . A_ThisFunc . " - DLLCall of 'PrintDlg' failed."
		 Return False
	  }
	  ; Get the printer device context.  Bounce If not defined.
	  If !(PDC := NumGet(PD, A_PtrSize * 4, "UPtr")) { ; hDC
		 ErrorLevel := "Function: " . A_ThisFunc . " - Couldn't get a printer's device context."
		 Return False
	  }
	  ; Free global structures created by PrintDlg
	  DllCall("Kernel32.dll\GlobalFree", "Ptr", NumGet(PD, A_PtrSize * 2, "UPtr"))
	  DllCall("Kernel32.dll\GlobalFree", "Ptr", NumGet(PD, A_PtrSize * 3, "UPtr"))
	  ; ----------------------------------------------------------------------------------------------------------------
	  ; Prepare to print
	  ; Collect Flags
	  Offset := A_PtrSize * 5
	  Flags := NumGet(PD, OffSet, "UInt")           ; Flags
	  ; Determine From/To Page
	  If (Flags & PD_PAGENUMS) {
		 PageF := NumGet(PD, Offset += 4, "UShort") ; nFromPage (first page)
		 PageL := NumGet(PD, Offset += 2, "UShort") ; nToPage (last page)
	  } Else {
		 PageF := 1
		 PageL := 65535
	  }
	  ; Collect printer capacities
	  Caps := This.GetPrinterCaps(PDC)
	  ; Set up page size and margins in Twips (1/20 point or 1/1440 of an inch)
	  UML := This.Margins.LT                   ; user margin left
	  UMT := This.Margins.TT                   ; user margin top
	  UMR := This.Margins.RT                   ; user margin right
	  UMB := This.Margins.BT                   ; user margin bottom
	  PML := Caps.POFX                         ; physical margin left
	  PMT := Caps.POFY                         ; physical margin top
	  PMR := Caps.PHYW - Caps.HRES - Caps.POFX ; physical margin right
	  PMB := Caps.PHYH - Caps.VRES - Caps.POFY ; physical margin bottom
	  LPW := Caps.HRES                         ; logical page width
	  LPH := Caps.VRES                         ; logical page height
	  ; Adjust margins
	  UML := UML > PML ? (UML - PML) : 0
	  UMT := UMT > PMT ? (UMT - PMT) : 0
	  UMR := UMR > PMR ? (UMR - PMR) : 0
	  UMB := UMB > PMB ? (UMB - PMB) : 0
	  ; Define/Populate the FORMATRANGE structure
	  VarSetCapacity(FR, (A_PtrSize * 2) + (4 * 10), 0)
	  NumPut(PDC, FR, 0, "UPtr")         ; hdc
	  NumPut(PDC, FR, A_PtrSize, "UPtr") ; hdcTarget
	  ; Define FORMATRANGE.rc
	  ; rc is the area to render to (rcPage - margins), measured in twips (1/20 point or 1/1440 of an inch).
	  ; If the user-defined margins are smaller than the printer's margins (the unprintable areas at the edges of each
	  ; page), the user margins are set to the printer's margins. In addition, the user-defined margins must be adjusted
	  ; to account for the printer's margins.
	  ; For example: If the user requests a 3/4 inch (19.05 mm) left margin but the printer's left margin is 1/4 inch
	  ; (6.35 mm), rc.Left is set to 720 twips (1/2 inch or 12.7 mm).
	  Offset := A_PtrSize * 2
	  NumPut(UML, FR, Offset += 0, "Int")       ; rc.Left
	  NumPut(UMT, FR, Offset += 4, "Int")       ; rc.Top
	  NumPut(LPW - UMR, FR, Offset += 4, "Int") ; rc.Right
	  NumPut(LPH - UMB, FR, Offset += 4, "Int") ; rc.Bottom
	  ; Define FORMATRANGE.rcPage
	  ; rcPage is the entire area of a page on the rendering device, measured in twips (1/20 point or 1/1440 of an inch)
	  ; Note: rc defines the maximum printable area which does not include the printer's margins (the unprintable areas
	  ; at the edges of the page). The unprintable areas are represented by PHYSICALOFFSETX and PHYSICALOFFSETY.
	  NumPut(0, FR, Offset += 4, "Int")         ; rcPage.Left
	  NumPut(0, FR, Offset += 4, "Int")         ; rcPage.Top
	  NumPut(LPW, FR, Offset += 4, "Int")       ; rcPage.Right
	  NumPut(LPH, FR, Offset += 4, "Int")       ; rcPage.Bottom
	  ; Determine print range.
	  ; If "Selection" option is chosen, use selected text, otherwise use the entire document.
	  If (Flags & PD_SELECTION) {
		 PrintS := Sel.S
		 PrintE := Sel.E
	  } Else {
		 PrintS := 0
		 PrintE := -1            ; (-1 = Select All)
	  }
	  Numput(PrintS, FR, Offset += 4, "Int")    ; cr.cpMin
	  NumPut(PrintE, FR, Offset += 4, "Int")    ; cr.cpMax
	  ; Define/Populate the DOCINFO structure
	  VarSetCapacity(DI, A_PtrSize * 5, 0)
	  NumPut(A_PtrSize * 5, DI, 0, "UInt")
	  NumPut(&DocName, DI, A_PtrSize, "UPtr")     ; lpszDocName
	  NumPut(0       , DI, A_PtrSize * 2, "UPtr") ; lpszOutput
	  ; Programming note: All other DOCINFO fields intentionally left as null.
	  ; Determine MaxPrintIndex
	  If (Flags & PD_SELECTION) {
		  PrintM := Sel.E
	  } Else {
		  PrintM := This.GetTextLen()
	  }
	  ; Be sure that the printer device context is in text mode
	  DllCall("Gdi32.dll\SetMapMode", "Ptr", PDC, "Int", MM_TEXT)
	  ; ----------------------------------------------------------------------------------------------------------------
	  ; Print it!
	  ; Start a print job.  Bounce If there is a problem.
	  PrintJob := DllCall("Gdi32.dll\StartDoc", "Ptr", PDC, "Ptr", &DI, "Int")
	  If (PrintJob <= 0) {
		 ErrorLevel := "Function: " . A_ThisFunc . " - DLLCall of 'StartDoc' failed."
		 Return False
	  }
	  ; Print page loop
	  PageC  := 0 ; current page
	  PrintC := 0 ; current print index
	  While (PrintC < PrintM) {
		 PageC++
		 ; Are we done yet?
		 If (PageC > PageL)
			Break
		 If (PageC >= PageF) && (PageC <= PageL) {
			; StartPage function.  Break If there is a problem.
			If (DllCall("Gdi32.dll\StartPage", "Ptr", PDC, "Int") <= 0) {
			   ErrorMsg := "Function: " . A_ThisFunc . " - DLLCall of 'StartPage' failed."
			   Break
			}
		 }
		 ; Format or measure page
		 If (PageC >= PageF) && (PageC <= PageL)
			Render := True
		 Else
			Render := False
		 SendMessage, %EM_FORMATRANGE%, %Render%, &FR, , % "ahk_id " . This.HWND
		 PrintC := ErrorLevel
		 If (PageC >= PageF) && (PageC <= PageL) {
			; EndPage function. Break If there is a problem.
			If (DllCall("Gdi32.dll\EndPage", "Ptr", PDC, "Int") <= 0) {
			   ErrorMsg := "Function: " . A_ThisFunc . " - DLLCall of 'EndPage' failed."
			   Break
			}
		 }
		 ; Update FR for the next page
		 Offset := (A_PtrSize * 2) + (4 * 8)
		 Numput(PrintC, FR, Offset += 0, "Int") ; cr.cpMin
		 NumPut(PrintE, FR, Offset += 4, "Int") ; cr.cpMax
	  }
	  ; ----------------------------------------------------------------------------------------------------------------
	  ; End the print job
	  DllCall("Gdi32.dll\EndDoc", "Ptr", PDC)
	  ; Delete the printer device context
	  DllCall("Gdi32.dll\DeleteDC", "Ptr", PDC)
	  ; Reset control (free cached information)
	  SendMessage %EM_FORMATRANGE%, 0, 0, , % "ahk_id " . This.HWND
	  ; Return to sender
	  If (ErrorMsg) {
		 ErrorLevel := ErrorMsg
		 Return False
	  }
	  Return True
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetMargins() { ; Get the default print margins
	  Static PSD_RETURNDEFAULT := 0x00000400, PSD_INTHOUSANDTHSOFINCHES := 0x00000004
		   , I := 1000 ; thousandth of inches
		   , M := 2540 ; hundredth of millimeters
		   , PSD_Size := (4 * 10) + (A_PtrSize * 11)
		   , PD_Size := (A_PtrSize = 8 ? (13 * A_PtrSize) + 16 : 66)
		   , OffFlags := 4 * A_PtrSize
		   , OffMargins := OffFlags + (4 * 7)
	  If !This.HasKey("Margins") {
		 VarSetCapacity(PSD, PSD_Size, 0) ; PAGESETUPDLG structure
		 NumPut(PSD_Size, PSD, 0, "UInt")
		 NumPut(PSD_RETURNDEFAULT, PSD, OffFlags, "UInt")
		 If !DllCall("Comdlg32.dll\PageSetupDlg", "Ptr", &PSD, "UInt")
			Return false
		 DllCall("Kernel32.dll\GobalFree", UInt, NumGet(PSD, 2 * A_PtrSize, "UPtr"))
		 DllCall("Kernel32.dll\GobalFree", UInt, NumGet(PSD, 3 * A_PtrSize, "UPtr"))
		 Flags := NumGet(PSD, OffFlags, "UInt")
		 Metrics := (Flags & PSD_INTHOUSANDTHSOFINCHES) ? I : M
		 Offset := OffMargins
		 This.Margins := {}
		 This.Margins.L := NumGet(PSD, Offset += 0, "Int")           ; Left
		 This.Margins.T := NumGet(PSD, Offset += 4, "Int")           ; Top
		 This.Margins.R := NumGet(PSD, Offset += 4, "Int")           ; Right
		 This.Margins.B := NumGet(PSD, Offset += 4, "Int")           ; Bottom
		 This.Margins.LT := Round((This.Margins.L / Metrics) * 1440) ; Left in twips
		 This.Margins.TT := Round((This.Margins.T / Metrics) * 1440) ; Top in twips
		 This.Margins.RT := Round((This.Margins.R / Metrics) * 1440) ; Right in twips
		 This.Margins.BT := Round((This.Margins.B / Metrics) * 1440) ; Bottom in twips
	  }
	  Return True
   }
   ; -------------------------------------------------------------------------------------------------------------------
   GetPrinterCaps(DC) { ; Get printer's capacities
	  Static HORZRES         := 0x08, VERTRES         := 0x0A
		   , LOGPIXELSX      := 0x58, LOGPIXELSY      := 0x5A
		   , PHYSICALWIDTH   := 0x6E, PHYSICALHEIGHT  := 0x6F
		   , PHYSICALOFFSETX := 0x70, PHYSICALOFFSETY := 0x71
		   , Caps := {}
	  ; Number of pixels per logical inch along the page width and height
	  LPXX := DllCall("Gdi32.dll\GetDeviceCaps", "Ptr", DC, "Int", LOGPIXELSX, "Int")
	  LPXY := DllCall("Gdi32.dll\GetDeviceCaps", "Ptr", DC, "Int", LOGPIXELSY, "Int")
	  ; The width and height of the physical page, in twips.
	  Caps.PHYW := Round((DllCall("Gdi32.dll\GetDeviceCaps", "Ptr", DC, "Int", PHYSICALWIDTH, "Int") / LPXX) * 1440)
	  Caps.PHYH := Round((DllCall("Gdi32.dll\GetDeviceCaps", "Ptr", DC, "Int", PHYSICALHEIGHT, "Int") / LPXY) * 1440)
	  ; The distance from the left/right edge (PHYSICALOFFSETX) and the top/bottom edge (PHYSICALOFFSETY) of the
	  ; physical page to the edge of the printable area, in twips.
	  Caps.POFX := Round((DllCall("Gdi32.dll\GetDeviceCaps", "Ptr", DC, "Int", PHYSICALOFFSETX, "Int") / LPXX) * 1440)
	  Caps.POFY := Round((DllCall("Gdi32.dll\GetDeviceCaps", "Ptr", DC, "Int", PHYSICALOFFSETY, "Int") / LPXY) * 1440)
	  ; Width and height of the printable area of the page, in twips.
	  Caps.HRES := Round((DllCall("Gdi32.dll\GetDeviceCaps", "Ptr", DC, "Int", HORZRES, "Int") / LPXX) * 1440)
	  Caps.VRES := Round((DllCall("Gdi32.dll\GetDeviceCaps", "Ptr", DC, "Int", VERTRES, "Int") / LPXY) * 1440)
	  Return Caps
   }
}