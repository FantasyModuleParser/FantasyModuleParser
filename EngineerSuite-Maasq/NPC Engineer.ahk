;~ ######################################################
;~ #                                                    #
;~ #                  ~ NPC Engineer ~                  #
;~ #                                                    #
;~ #                                  Written by Maasq. #
;~ ######################################################

#NoEnv
#Persistent
#SingleInstance Force
#NoTrayIcon
SendMode Input
SetWorkingDir %A_ScriptDir%
SetWinCheck()
Gosub StartupRoutine
NPCEngineer("self")
return

NPCEngineer(what) {
	global
	SplashImageGUI("NPC Engineer.png", "Center", "Center", 300, true)
	wc:= new WinClip

	LoadPause:= 1
	Release_Version:= "1.2.60"
	Release_Date:= "24/11/19"
	NPCE_Ref:= what
	
	Gosub Startup
	Prepare_GUI()
	If (LaunchProject AND FileExist(ProjectPath)) {
		flags.project:= 1
		PROE_Ref:= "NPCE_Main"
		Gosub Project_Load
	}
	FGcatList(NPCFGcat)
	If (LaunchNPC AND FileExist(NPCPath)) {
		flags.NPC:= 1
		Gosub Open_NPC
	}
	LoadPause:= 0
	
	win.npcengineer:= 1
	Gui, NPCE_Main:Show, w1150 h665, NPC Engineer
	hToolbar:= CreateToolBar()
	ThemeMenu(NPCskin, 0, 0)
	
	Hotkey, IfWinActive, ahk_id %NPCE_Import%
	Hotkey, ^v, Clipimp
	
	Hotkey,IfWinActive,ahk_id %NPCE_Main%
	for Index,Key in ["F1","F2","F3","F4","F5","F6","F7","F8","F9"]
		Hotkey,%Key%,KeyPress,On
	
	Hotkey,IfWinActive,ahk_id %NPCE_Main%
	hotkey, ^b, SetFontStyle2
	hotkey, ^i, SetFontStyle2
	hotkey, ^u, SetFontStyle2
	hotkey, ^h, REHeader
	hotkey, ^t, REBody
	Hotkey, ^f, ReFrame
	Hotkey, ^l, ReBullet
	hotkey, ^v, Paste
	Hotkey, ^J, De_PDF
	Hotkey, ~LButton, Vup
	Hotkey, ~Wheeldown, VScrDwn
	Hotkey, ~Wheelup, VScrUp
	hotkey, esc, EscapeHandle
	hotkey, RButton, RLink

	Hotkey,IfWinActive,ahk_id %NPCE_Import%
	Hotkey, ^J, De_PDF
	Hotkey,IfWinActive,ahk_id %NPCE_Reactions%
	Hotkey, ^J, De_PDF
	Hotkey,IfWinActive,ahk_id %NPCE_LegActions%
	Hotkey, ^J, De_PDF
	Hotkey,IfWinActive,ahk_id %NPCE_LairActions%
	Hotkey, ^J, De_PDF


	OnMessage(0x200, "WM_MOUSEMOVE")

	WM_SETCURSOR := 0x0020
	CHAND := DllCall("User32.dll\LoadCursor", "Ptr", NULL, "Int", 32649, "UPtr")
	OnMessage(WM_SETCURSOR, "SetCursor")

	WM_LBUTTONDBLCLK := 0x203
	OnMessage(WM_LBUTTONDBLCLK, "LBUTTONDBLCLK")
	DCEdits := ["NPChp"]
}


;~ ######################################################
;~ #                       Labels                       #
;~ ######################################################

RButton:					;{
	MouseGetPos, , , , OutputVarControl, 2
	GuiControlGet, controlvar, NPCE_ActAdd:FocusV
	If ((OutputVarControl = OAText) AND (controlvar = "OtherActionText")) {
		Menu, ActionSub, Show	
	}
	GuiControlGet, controlvar, NPCE_TraitAdd:FocusV
	If ((OutputVarControl = TAText) AND (controlvar = "TraitNew")) {
		Menu, ActionSub, Show	
	}
Return						;}

Startup:					;{
	Critical
	;~ InitialDirCreate()
	Prelude()
	;~ CommonInitialise()
	Initialise()
	;~ Initialise_Other()
	Declare_Vars()
	;~ Load_Options()
	;~ OptionsDirCreate()
	
	If !FileExist(DataDir "\weapons.json") {
		FileCopy, Defaults\defaultweapons.json, %DataDir%\weapons.json, 1
	}
	If !FileExist(DataDir "\traits.json") {
		FileCopy, Defaults\defaulttraits.json, %DataDir%\traits.json, 1
	}
	If !FileExist(DataDir "\actions.json") {
		FileCopy, Defaults\defaultactions.json, %DataDir%\actions.json, 1
	}
	If !FileExist(DataDir "\languages.json") {
		FileCopy, Defaults\defaultlanguages.json, %DataDir%\languages.json, 1
	}
	Menu, ActionSub, Add, NPC Name: Capitalized, MH1
	Menu, ActionSub, Add, NPC Name: Body text, MH2
	Menu, ActionSub, Add  ; Add a separator line.
	Menu, ActionSub, Add, He/She/It: Capitalized, MH3
	Menu, ActionSub, Add, He/She/It: Body text, MH4
	Menu, ActionSub, Add  ; Add a separator line.
	Menu, ActionSub, Add, Him/Her/It: Capitalized, MH5
	Menu, ActionSub, Add, Him/Her/It: Body text, MH6
	Menu, ActionSub, Add  ; Add a separator line.
	Menu, ActionSub, Add, His/Her/Its: Capitalized, MH7
	Menu, ActionSub, Add, His/Her/Its: Body text, MH8
	Menu, ActionSub, Add  ; Add a separator line.
	Menu, ActionSub, Add, Himself/Herself/Itself, MH9
	
	jsonpath:= DataDir "\languages.json"
	
	langDB:= new JSONfile(jsonpath)
	For key, val in langDB.object()
		LangUser.push(key)

return						;}

MH1:						;{
	Cpp("<NU>")
return						;}

MH2:						;{
	Cpp("<NL>")
return						;}

MH3:						;{
	Cpp("<GU1>")
return						;}

MH4:						;{
	Cpp("<GL1>")
return						;}

MH5:						;{
	Cpp("<GU2>")
return						;}

MH6:						;{
	Cpp("<GL2>")
return						;}

MH7:						;{
	Cpp("<GU3>")
return						;}

MH8:						;{
	Cpp("<GL3>")
return						;}

MH9:						;{
	Cpp("<GL4>")
return						;}


Add_Weapon:					;{
	GUI, NPCE_Weapons:submit, NoHide
	
	JSON_wp_Name:= ""
	Temp_Type:= ""
	StringReplace, Temp_Type, WA_Type, %A_Space%Weapon Attack, ,
	For a, b in WeapDB.object()
	{
		if (a == WA_Name) {
			JSON_wp_Name:= a
			WeapDB[JSON_wp_Name].WeaponType:= Trim(Temp_Type)
			WeapDB[JSON_wp_Name].ToHit:= Trim(WA_ToHit)
			WeapDB[JSON_wp_Name].Reach:= Trim(WA_Reach)
			WeapDB[JSON_wp_Name].RangeNormal:= Trim(WA_RNorm)
			WeapDB[JSON_wp_Name].RangeLong:= Trim(WA_RLong)
			WeapDB[JSON_wp_Name].Target:= Trim(WA_Target)
			WeapDB[JSON_wp_Name].DiceNumber:= Trim(WA_NoDice)
			WeapDB[JSON_wp_Name].DiceType:= Trim(WA_Dice)
			WeapDB[JSON_wp_Name].DamageBonus:= Trim(WA_Dambon)
			WeapDB[JSON_wp_Name].DamageType:= Trim(WA_DamType)
			WeapDB[JSON_wp_Name].Magic:= Trim(WA_Magic)
			WeapDB[JSON_wp_Name].AddBonus:= Trim(WA_BonAdd)
			if WA_BonAdd {
				WeapDB[JSON_wp_Name].BonusDiceNumber:= Trim(WA_BonNoDice)
				WeapDB[JSON_wp_Name].BonusDiceType:= Trim(WA_BonDice)
				WeapDB[JSON_wp_Name].BonusDamageBonus:= Trim(WA_BonDamBon)
				WeapDB[JSON_wp_Name].BonusDamageType:= Trim(WA_BonDamType)
			} else {
				WeapDB[JSON_wp_Name].BonusDiceNumber:= ""
				WeapDB[JSON_wp_Name].BonusDiceType:= ""
				WeapDB[JSON_wp_Name].BonusDamageBonus:= ""
				WeapDB[JSON_wp_Name].BonusDamageType:= ""
			}
			WeapDB[JSON_wp_Name].Silver:= Trim(WA_Silver)
			WeapDB[JSON_wp_Name].Adamantine:= Trim(WA_Adaman)
			WeapDB[JSON_wp_Name].CFIron:= Trim(WA_cfiron)
			WeapDB[JSON_wp_Name].AddOtherText:= Trim(WA_OtherTextAdd)
			if WA_OtherTextAdd {
				WeapDB[JSON_wp_Name].OtherText:= Trim(WA_OtherText)
			} else {
				WeapDB[JSON_wp_Name].OtherText:= ""
			}
			WeapDB[JSON_wp_Name].Versatile:= Trim(WA_Versatile)
			WeapDB.save(true)
		}
	}
	
	If !JSON_wp_Name {
		Armoury:= {}
		Armoury[WA_Name]:= {}
		Armoury[WA_Name].WeaponType:= Trim(Temp_Type)
		Armoury[WA_Name].ToHit:= Trim(WA_ToHit)
		Armoury[WA_Name].Reach:= Trim(WA_Reach)
		Armoury[WA_Name].RangeNormal:= Trim(WA_RNorm)
		Armoury[WA_Name].RangeLong:= Trim(WA_RLong)
		Armoury[WA_Name].Target:= Trim(WA_Target)
		Armoury[WA_Name].DiceNumber:= Trim(WA_NoDice)
		Armoury[WA_Name].DiceType:= Trim(WA_Dice)
		Armoury[WA_Name].DamageBonus:= Trim(WA_Dambon)
		Armoury[WA_Name].DamageType:= Trim(WA_DamType)
		Armoury[WA_Name].Magic:= Trim(WA_Magic)
		Armoury[WA_Name].AddBonus:= Trim(WA_BonAdd)
		if WA_BonAdd {
			Armoury[WA_Name].BonusDiceNumber:= Trim(WA_BonNoDice)
			Armoury[WA_Name].BonusDiceType:= Trim(WA_BonDice)
			Armoury[WA_Name].BonusDamageBonus:= Trim(WA_BonDamBon)
			Armoury[WA_Name].BonusDamageType:= Trim(WA_BonDamType)
		} else {
			Armoury[WA_Name].BonusDiceNumber:= ""
			Armoury[WA_Name].BonusDiceType:= ""
			Armoury[WA_Name].BonusDamageBonus:= ""
			Armoury[WA_Name].BonusDamageType:= ""
		}				
		Armoury[WA_Name].Silver:= Trim(WA_Silver)
		Armoury[WA_Name].Adamantine:= Trim(WA_Adaman)
		Armoury[WA_Name].CFIron:= Trim(WA_cfiron)
		Armoury[WA_Name].Versatile:= Trim(WA_Versatile)
		Armoury[WA_Name].AddOtherText:= Trim(WA_OtherTextAdd)
		if WA_OtherTextAdd {
			Armoury[WA_Name].OtherText:= Trim(WA_OtherText)
		} else {
			Armoury[WA_Name].OtherText:= ""
		}
		WeapDB.fill(Armoury)
		WeapDB.save(true)
		weaplist:= "|"
		For a, b in WeapDB.object()
		{
			weaplist:= weaplist a "|"
		}
		GuiControl, NPCE_Weapons:, WA_Name, %weaplist%
	}
	Blank_WA()
return						;}

Delete_Weapon:				;{
	GUI, NPCE_Weapons:submit, NoHide
	If WA_Name {
		WeapDB.delete(WA_Name)
		WeapDB.save(true)
		weaplist:= "|"
		For a, b in WeapDB.object()
		{
			weaplist:= weaplist a "|"
		}
		GuiControl, NPCE_Weapons:, WA_Name, %weaplist%
		WA_Name:= ""
		Blank_WA()
	}

return						;}

Act_MultiAttack:			;{
	GUI, NPCE_Actions:submit, NoHide
	Loop % NPC_Actions.MaxIndex()
	{
		If (NPC_Actions[A_Index, "Name"] == "Multiattack") {
			RemovedValue := NPC_Actions.RemoveAt(A_Index)
		}
	}
	If Multi_Attack {
		multi_attack_Text:= RegExReplace(multi_attack_Text, "^(.)", "$u1")
		multi_attack_Text:= multi_attack_Text "."
		Stringreplace, multi_attack_Text, multi_attack_Text, `.`., `., All
		NPC_Actions.push({Name: "Multiattack", Action: multi_attack_Text})
		Multi_attack:= 1
		position += 1
	}
	Actionworks()
	Actionworks2()
	Actionworks3()
return						;}

Act_WeaponAttack:			;{
	GUI, NPCE_Actions:submit, NoHide
	If WA_Name {
		StringLower WA_Name, WA_Name, T
		WA_Name := RegExReplace(WA_Name, "^\W+", "")
		WA_Name := RegExReplace(WA_Name, "[^\w)]+$", "")
		WA_Descrip:= RegExReplace(WA_Descrip, "^(.)", "$u1")
		
		over:= 0
		For key, value in NPC_Actions
		{
			If (WA_Name = NPC_Actions[key, "Name"]) {
				NPC_Actions[key, "Action"]:= WA_Descrip
				over:= 1
			}
		}

		If !over
			NPC_Actions.push({Name: WA_Name, Action: WA_Descrip})
		
		position += 1
	}
	Actionworks()
	Actionworks2()
	Actionworks3()
return						;}

Act_Other:					;{
	GUI, NPCE_Actions:submit, NoHide
	Loop % NPC_Actions.MaxIndex()
	{
		If (NPC_Actions[A_Index, "Name"] = OtherActionName) {
			RemovedValue := NPC_Actions.RemoveAt(A_Index)
		}
	}
	If (OtherActionName and OtherActionText) {
		StringLower OtherActionName, OtherActionName, T
		OtherActionName := RegExReplace(OtherActionName, "^\W+", "")
		OtherActionName := RegExReplace(OtherActionName, "[^\w)]+$", "")
		OtherActionText:= RegExReplace(OtherActionText, "^(.)", "$u1")
		OtherActionText:= OtherActionText "."
		Stringreplace, OtherActionText, OtherActionText, `.`., `., All
		NPC_Actions.push({Name: OtherActionName, Action: OtherActionText})
		GuiControl, NPCE_Actions:choose, OtherActionName, 0
		GuiControl, NPCE_Actions:, OtherActionText, %NPCEnull%
		GuiControl, NPCE_Actions:Focus, OtherActionText
		Send ^{End}
		position += 1
	}
	Actionworks()
	Actionworks2()
	Actionworks3()
return						;}

Act_BWA:					;{
	GUI, NPCE_Actions:submit, NoHide
	GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_BonNoDice
	GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_1
	GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_2
	GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_BonDice
	GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_BonDamBon
	GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_BonDamType
	GuiControl, NPCE_Actions:Enable%WA_OtherTextAdd%, WA_OtherText

	WA_Descrip:= WA_Type ": +" WA_ToHit " to hit, "
	If (WA_Type == "Melee Weapon Attack") {
		WA_Descrip:= WA_Descrip "reach " WA_Reach " ft., "
	} else if (WA_Type == "Ranged Weapon Attack") {
		WA_Descrip:= WA_Descrip "range " WA_Rnorm "/" WA_Rlong " ft., "
	} else if (WA_Type == "Melee or Ranged Weapon Attack") {
		WA_Descrip:= WA_Descrip  "reach " WA_Reach " ft. or range " WA_Rnorm "/" WA_Rlong " ft., "
	} else if (WA_Type == "Melee Spell Attack") {
		WA_Descrip:= WA_Descrip "reach " WA_Reach " ft., "
	} else if (WA_Type == "Ranged Spell Attack") {
		WA_Descrip:= WA_Descrip "range " WA_Rnorm " ft., "
	} else if (WA_Type == "Melee or Ranged Spell Attack") {
		WA_Descrip:= WA_Descrip "reach " WA_Reach " ft. or range " WA_Rnorm " ft., "
	}
	WA_Damage:= WA_NoDice * WA_Dice + WA_NoDice
	WA_Damage:= WA_Damage/2
	WA_Damage:= WA_Damage + WA_DamBon
	WA_Damage:= Floor(WA_Damage)
	if (WA_DamBon < 0)
		WaSign:= " - "
	else
		WaSign:= " + "
	
	magwep:= ""
	if WA_Silver
		magwep .= ", silver"
	if WA_Adaman
		magwep .= ", adamantine"
	if WA_cfiron
		magwep .= ", cold-forged iron"
	if WA_Magic
		magwep .= ", magic"
	
	if WA_Damage{
		if WA_DamBon
			tempdam:= WaSign abs(WA_DamBon)
		else
			tempdam:= ""
		If WA_Dice
			WA_Descrip:= WA_Descrip WA_Target ". Hit: " WA_Damage " (" WA_NoDice "d" WA_Dice tempdam ") " WA_DamType magwep " damage"
		else
			WA_Descrip:= WA_Descrip WA_Target ". Hit: " WA_Damage " " WA_DamType magwep " damage"
	} else {
		WA_Descrip:= WA_Descrip WA_Target
	}
	
	If WA_BonAdd {
		WA_Damage:= WA_BonNoDice * WA_BonDice + WA_BonNoDice
		WA_Damage:= WA_Damage/2
		WA_Damage:= WA_Damage + WA_BonDamBon
		WA_Damage:= Floor(WA_Damage)
		if (WA_BonDamBon < 0)
			WaSign2:= " - "
		else
			WaSign2:= " + "
		tempdam:= ""
		if WA_BonDamBon
			tempdam:= WaSign2 abs(WA_BonDamBon)
		WA_Descrip:= WA_Descrip  " plus " WA_Damage " (" WA_BonNoDice "d" WA_BonDice tempdam ") " WA_BonDamType " damage"
	}
	
	If (WA_Type == "Melee Weapon Attack" and WA_Versatile) {
		If (WA_Dice = 4) {
			Wa_Dice2:= 6
		} else if (WA_Dice = 6) {
			Wa_Dice2:= 8
		} else if (WA_Dice = 8) {
			Wa_Dice2:= 10
		} else if (WA_Dice = 10) {
			Wa_Dice2:= 12
		} else if (WA_Dice = 12) {
			Wa_Dice2:= 20
		}
		WA_Damage:= WA_NoDice * WA_Dice2 + WA_NoDice
		WA_Damage:= WA_Damage/2
		WA_Damage:= WA_Damage + WA_DamBon
		WA_Damage:= Floor(WA_Damage)
		if (WA_DamBon < 0)
			WaSign:= " - "
		else
			WaSign:= " + "
		
		magwep:= ""
		if WA_Silver
			magwep .= ", silver"
		if WA_Adaman
			magwep .= ", adamantine"
		if WA_cfiron
			magwep .= ", cold-forged iron"
		if WA_Magic
			magwep .= ", magic"
		
		if WA_Damage{
			if WA_DamBon
				tempdam:= WaSign abs(WA_DamBon)
			else
				tempdam:= ""
			WA_Descrip:= WA_Descrip  " or " WA_Damage " (" WA_NoDice "d" WA_Dice2 tempdam ") " WA_DamType magwep " damage"
		}
		
		If WA_BonAdd {
			WA_Damage:= WA_BonNoDice * WA_BonDice + WA_BonNoDice
			WA_Damage:= WA_Damage/2
			WA_Damage:= WA_Damage + WA_BonDamBon
			WA_Damage:= Floor(WA_Damage)
			if (WA_BonDamBon < 0)
				WaSign2:= " - "
			else
				WaSign2:= " + "
			tempdam:= ""
			if WA_BonDamBon
				tempdam:= WaSign2 abs(WA_BonDamBon)
			WA_Descrip:= WA_Descrip  " plus " WA_Damage " (" WA_BonNoDice "d" WA_BonDice tempdam ") " WA_BonDamType " damage"
		}
		WA_Descrip:= WA_Descrip " if used with two hands"
	}
	
	WA_Descrip:= WA_Descrip "."	
	
	If (WA_Type == "Melee or Ranged Weapon Attack") {
		if WA_DamBon
			tempdam:= WaSign abs(WA_DamBon)
		else
			tempdam:= ""
		WA_Descrip:= "Melee Weapon Attack: +" WA_ToHit " to hit, reach " WA_Reach " ft., " WA_Target ". Hit: " WA_Damage " (" WA_NoDice "d" WA_Dice tempdam ") " WA_DamType magwep " damage"

		If WA_BonAdd {
			WA_Damage:= WA_BonNoDice * WA_BonDice + WA_BonNoDice
			WA_Damage:= WA_Damage/2
			WA_Damage:= WA_Damage + WA_BonDamBon
			WA_Damage:= Floor(WA_Damage)
			if (WA_BonDamBon < 0)
				WaSign2:= " - "
			else
				WaSign2:= " + "
			tempdam:= ""
			if WA_BonDamBon
				tempdam:= WaSign2 abs(WA_BonDamBon)
			WA_Descrip:= WA_Descrip  " plus " WA_Damage " (" WA_BonNoDice "d" WA_BonDice tempdam ") " WA_BonDamType " damage"
		}

		If WA_Versatile {
			If (WA_Dice = 4) {
				Wa_Dice2:= 6
			} else if (WA_Dice = 6) {
				Wa_Dice2:= 8
			} else if (WA_Dice = 8) {
				Wa_Dice2:= 10
			} else if (WA_Dice = 10) {
				Wa_Dice2:= 12
			} else if (WA_Dice = 12) {
				Wa_Dice2:= 20
			}
			WA_Damage:= WA_NoDice * WA_Dice2 + WA_NoDice
			WA_Damage:= WA_Damage/2
			WA_Damage:= WA_Damage + WA_DamBon
			WA_Damage:= Floor(WA_Damage)
			if (WA_DamBon < 0)
				WaSign:= " - "
			else
				WaSign:= " + "
			
			magwep:= ""
			if WA_Silver
				magwep .= ", silver"
			if WA_Adaman
				magwep .= ", adamantine"
			if WA_cfiron
				magwep .= ", cold-forged iron"
			if WA_Magic
				magwep .= ", magic"
			
			if WA_Damage{
				if WA_DamBon
					tempdam:= WaSign abs(WA_DamBon)
				else
					tempdam:= ""
				WA_Descrip:= WA_Descrip  " or " WA_Damage " (" WA_NoDice "d" WA_Dice2 tempdam ") " WA_DamType magwep " damage"
			}
			
			If WA_BonAdd {
				WA_Damage:= WA_BonNoDice * WA_BonDice + WA_BonNoDice
				WA_Damage:= WA_Damage/2
				WA_Damage:= WA_Damage + WA_BonDamBon
				WA_Damage:= Floor(WA_Damage)
				if (WA_BonDamBon < 0)
					WaSign2:= " - "
				else
					WaSign2:= " + "
				tempdam:= ""
				if WA_BonDamBon
					tempdam:= WaSign2 abs(WA_BonDamBon)
				WA_Descrip:= WA_Descrip  " plus " WA_Damage " (" WA_BonNoDice "d" WA_BonDice tempdam ") " WA_BonDamType " damage"
			}
			WA_Descrip:= WA_Descrip " if used with two hands"
		}

		if (WA_BonDamBon < 0)
			WaSign2:= " - "
		else
			WaSign2:= " + "
		tempdam:= ""
		if WA_BonDamBon
			tempdam:= WaSign2 abs(WA_BonDamBon)

		WA_Descrip:= WA_Descrip ". Or Ranged Weapon Attack: +" WA_ToHit " to hit, range " WA_Rnorm "/" WA_Rlong " ft., " WA_Target ". Hit: " WA_Damage " (" WA_NoDice "d" WA_Dice tempdam ") " WA_DamType magwep " damage"
		
		If WA_BonAdd {
			WA_Damage:= WA_BonNoDice * WA_BonDice + WA_BonNoDice
			WA_Damage:= WA_Damage/2
			WA_Damage:= WA_Damage + WA_BonDamBon
			WA_Damage:= Floor(WA_Damage)
			WA_Descrip:= WA_Descrip  " plus " WA_Damage " (" WA_BonNoDice "d" WA_BonDice " + " WA_BonDamBon ") " WA_BonDamType " damage"
		}
		WA_Descrip:= WA_Descrip "."
	}

	If WA_OtherTextAdd {
		WA_Descrip:= WA_Descrip WA_OtherText
	}
	
	weapon_attack_Text:= WA_Name ". " WA_Descrip

	GuiControl, NPCE_Actions:, weapon_attack_Text, %weapon_attack_Text%
return						;}

Act_BWA2:					;{
	GUI, NPCE_Actions:submit, NoHide
	
	JSON_wp_Name:= ""
	For a, b in WeapDB.object()
	{
		if (a == WA_Name) {
			JSON_wp_Name:= a
		}
	}
	If JSON_wp_Name {
		jack:= WeapDB[JSON_wp_Name].WeaponType " Weapon Attack"
		GuiControl, NPCE_Actions:Text, WA_Type, %jack%
		jack:= WeapDB[JSON_wp_Name].ToHit
		GuiControl, NPCE_Actions:, WA_ToHit, %jack%
		jack:= WeapDB[JSON_wp_Name].Reach
		GuiControl, NPCE_Actions:, WA_Reach, %jack%
		jack:= WeapDB[JSON_wp_Name].RangeNormal
		GuiControl, NPCE_Actions:, WA_RNorm, %jack%
		jack:= WeapDB[JSON_wp_Name].RangeLong
		GuiControl, NPCE_Actions:, WA_RLong, %jack%
		jack:= WeapDB[JSON_wp_Name].Target
		GuiControl, NPCE_Actions:Text, WA_Target, %jack%
		jack:= WeapDB[JSON_wp_Name].DiceNumber
		GuiControl, NPCE_Actions:, WA_NoDice, %jack%
		jack:= WeapDB[JSON_wp_Name].DiceType
		GuiControl, NPCE_Actions:Text, WA_Dice, %jack%
		jack:= WeapDB[JSON_wp_Name].DamageBonus
		GuiControl, NPCE_Actions:, WA_Dambon, %jack%
		jack:= WeapDB[JSON_wp_Name].DamageType
		GuiControl, NPCE_Actions:Text, WA_DamType, %jack%
		jack:= WeapDB[JSON_wp_Name].Magic
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Actions:, WA_Magic, %jack%
		jack:= WeapDB[JSON_wp_Name].Silver
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Actions:, WA_Silver, %jack%
		jack:= WeapDB[JSON_wp_Name].Adamantine
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Actions:, WA_Adaman, %jack%
		jack:= WeapDB[JSON_wp_Name].CFIron
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Actions:, WA_cfiron, %jack%
		jack:= WeapDB[JSON_wp_Name].Versatile
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Actions:, Versatile, %jack%
		jack:= WeapDB[JSON_wp_Name].AddBonus
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Actions:, WA_BonAdd, %jack%
		WA_BonAdd:= jack
		GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_BonNoDice
		GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_1
		GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_2
		GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_BonDice
		GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_BonDamBon
		GuiControl, NPCE_Actions:Enable%WA_BonAdd%, WA_BonDamType
		If WA_BonAdd {
			jack:= WeapDB[JSON_wp_Name].BonusDiceNumber
			GuiControl, NPCE_Actions:, WA_BonNoDice, %jack%
			jack:= WeapDB[JSON_wp_Name].BonusDiceType
			GuiControl, NPCE_Actions:Text, WA_BonDice, %jack%
			jack:= WeapDB[JSON_wp_Name].BonusDamageBonus
			GuiControl, NPCE_Actions:, WA_BonDambon, %jack%
			jack:= WeapDB[JSON_wp_Name].BonusDamageType
			GuiControl, NPCE_Actions:Text, WA_BonDamType, %jack%
		} else {
			jack:= ""
			GuiControl, NPCE_Actions:, WA_BonNoDice, %jack%
			jack:= ""
			GuiControl, NPCE_Actions:Text, WA_BonDice, %jack%
			jack:= ""
			GuiControl, NPCE_Actions:, WA_BonDambon, %jack%
			jack:= ""
			GuiControl, NPCE_Actions:Text, WA_BonDamType, %jack%
		}
		jack:= WeapDB[JSON_wp_Name].AddOtherText
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Actions:, WA_OtherTextAdd, %jack%
		WA_OtherTextAdd:= jack
		GuiControl, NPCE_Actions:Enable%WA_OtherTextAdd%, WA_OtherText
		If WA_OtherTextAdd {
			jack:= WeapDB[JSON_wp_Name].OtherText
			GuiControl, NPCE_Actions:, WA_OtherText, %jack%
		} else {
			jack:= ""
			GuiControl, NPCE_Actions:, WA_OtherText, %jack%
		}
	}
	gosub Act_BWA	
return						;}

BonusDamage:				;{
	GUI, NPCE_Weapons:submit, NoHide
	GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_BonNoDice
	GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_1
	GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_2
	GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_BonDice
	GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_BonDamBon
	GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_BonDamType
	GuiControl, NPCE_Weapons:Enable%WA_OtherTextAdd%, WA_OtherText
return						;}

New_Weapon:					;{
	GUI, NPCE_Weapons:submit, NoHide
	
	JSON_wp_Name:= ""
	For a, b in WeapDB.object()
	{
		if (a == WA_Name) {
			JSON_wp_Name:= a
		}
	}
	If JSON_wp_Name {
		jack:= WeapDB[JSON_wp_Name].WeaponType " Weapon Attack"
		GuiControl, NPCE_Weapons:Text, WA_Type, %jack%
		jack:= WeapDB[JSON_wp_Name].ToHit
		GuiControl, NPCE_Weapons:, WA_ToHit, %jack%
		jack:= WeapDB[JSON_wp_Name].Reach
		GuiControl, NPCE_Weapons:, WA_Reach, %jack%
		jack:= WeapDB[JSON_wp_Name].RangeNormal
		GuiControl, NPCE_Weapons:, WA_RNorm, %jack%
		jack:= WeapDB[JSON_wp_Name].RangeLong
		GuiControl, NPCE_Weapons:, WA_RLong, %jack%
		jack:= WeapDB[JSON_wp_Name].Target
		GuiControl, NPCE_Weapons:Text, WA_Target, %jack%
		jack:= WeapDB[JSON_wp_Name].DiceNumber
		GuiControl, NPCE_Weapons:, WA_NoDice, %jack%
		jack:= WeapDB[JSON_wp_Name].DiceType
		GuiControl, NPCE_Weapons:Text, WA_Dice, %jack%
		jack:= WeapDB[JSON_wp_Name].DamageBonus
		GuiControl, NPCE_Weapons:, WA_Dambon, %jack%
		jack:= WeapDB[JSON_wp_Name].DamageType
		GuiControl, NPCE_Weapons:Text, WA_DamType, %jack%
		jack:= WeapDB[JSON_wp_Name].Magic
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Weapons:, WA_Magic, %jack%
		jack:= WeapDB[JSON_wp_Name].Silver
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Weapons:, WA_Silver, %jack%
		jack:= WeapDB[JSON_wp_Name].Adamantine
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Weapons:, WA_Adaman, %jack%
		jack:= WeapDB[JSON_wp_Name].CFIron
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Weapons:, WA_cfiron, %jack%
		jack:= WeapDB[JSON_wp_Name].Versatile
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Weapons:, Versatile, %jack%
		jack:= WeapDB[JSON_wp_Name].AddBonus
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Weapons:, WA_BonAdd, %jack%
		WA_BonAdd:= jack
		GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_BonNoDice
		GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_1
		GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_2
		GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_BonDice
		GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_BonDamBon
		GuiControl, NPCE_Weapons:Enable%WA_BonAdd%, WA_BonDamType
		If WA_BonAdd {
			jack:= WeapDB[JSON_wp_Name].BonusDiceNumber
			GuiControl, NPCE_Weapons:, WA_BonNoDice, %jack%
			jack:= WeapDB[JSON_wp_Name].BonusDiceType
			GuiControl, NPCE_Weapons:Text, WA_BonDice, %jack%
			jack:= WeapDB[JSON_wp_Name].BonusDamageBonus
			GuiControl, NPCE_Weapons:, WA_BonDambon, %jack%
			jack:= WeapDB[JSON_wp_Name].BonusDamageType
			GuiControl, NPCE_Weapons:Text, WA_BonDamType, %jack%
		} else {
			jack:= ""
			GuiControl, NPCE_Weapons:, WA_BonNoDice, %jack%
			jack:= ""
			GuiControl, NPCE_Weapons:Text, WA_BonDice, %jack%
			jack:= ""
			GuiControl, NPCE_Weapons:, WA_BonDambon, %jack%
			jack:= ""
			GuiControl, NPCE_Weapons:Text, WA_BonDamType, %jack%
		}
		jack:= WeapDB[JSON_wp_Name].AddOtherText
		If !(jack = 1)
			jack:= 0
		GuiControl, NPCE_Weapons:, WA_OtherTextAdd, %jack%
		WA_OtherTextAdd:= jack
		GuiControl, NPCE_Weapons:Enable%WA_OtherTextAdd%, WA_OtherText
		If WA_OtherTextAdd {
			jack:= WeapDB[JSON_wp_Name].OtherText
			GuiControl, NPCE_Weapons:, WA_OtherText, %jack%
		} else {
			jack:= ""
			GuiControl, NPCE_Weapons:, WA_OtherText, %jack%
		}
	}
return						;}

ReAct_Other:				;{
	GUI, NPCE_ReActions:submit, NoHide
	Loop % NPC_ReActions.MaxIndex()
	{
		If (NPC_ReActions[A_Index, "Name"] == OtherReActionName) {
			RemovedValue := NPC_ReActions.RemoveAt(A_Index)
		}
	}
	If (OtherReActionName and OtherReActionText) {
		StringLower OtherReActionName, OtherReActionName, T
		OtherReActionName := RegExReplace(OtherReActionName, "^\W+", "")
		OtherReActionName := RegExReplace(OtherReActionName, "[^\w)]+$", "")
		OtherReActionText:= RegExReplace(OtherReActionText, "^(.)", "$u1")
		OtherReActionText:= OtherReActionText "."
		Stringreplace, OtherReActionText, OtherReActionText, `.`., `., All
		NPC_ReActions.push({Name: OtherReActionName, Reaction: OtherReActionText})
		GuiControl, NPCE_ReActions:Choose, OtherReActionName, 0
		GuiControl, NPCE_ReActions:, OtherReActionText, %NPCEnull%
		GuiControl, NPCE_ReActions:Focus, OtherReActionText
		Send ^{End}
		position += 1
	}
	Reactionworks()
return						;}

LgAction_Options:			;{
	GUI, NPCE_LegActions:submit, NoHide
	Loop % NPC_Legendary_Actions.MaxIndex()
	{
		If (NPC_Legendary_Actions[A_Index, "Name"] == "Options") {
			RemovedValue := NPC_Legendary_Actions.RemoveAt(A_Index)
		}
	}
	LgActionOptions:= RegexReplace( LgActionOptions, "^\s+|\s+$" )
	LgActionOptions:= RegExReplace(LgActionOptions, "^(.)", "$u1")
	LgActionOptions:= LgActionOptions "."
	Stringreplace, LgActionOptions, LgActionOptions, `.`., `., All
	NPC_Legendary_Actions.push({Name: "Options", LegAction: LgActionOptions})
	Lgactionworks()
	Lgactionworks2()
return						;}

LgAct_Other:				;{
	GUI, NPCE_LegActions:submit, NoHide
	Loop % NPC_Legendary_Actions.MaxIndex()
	{
		If (NPC_Legendary_Actions[A_Index, "Name"] == OtherLgActionName) {
			RemovedValue := NPC_Legendary_Actions.RemoveAt(A_Index)
		}
	}
	If (OtherLgActionName and OtherLgActionText) {
		StringLower OtherLgActionName, OtherLgActionName, T
		OtherLgActionName := RegExReplace(OtherLgActionName, "^\W+", "")
		OtherLgActionName := RegExReplace(OtherLgActionName, "[^\w)]+$", "")
		OtherLgActionText:= RegExReplace(OtherLgActionText, "^(.)", "$u1")
		OtherLgActionText:= OtherLgActionText "."
		Stringreplace, OtherLgActionText, OtherLgActionText, `.`., `., All
		NPC_Legendary_Actions.push({Name: OtherLgActionName, LegAction: OtherLgActionText})
		GuiControl, NPCE_LegActions:Choose, OtherLgActionName, 0
		GuiControl, NPCE_LegActions:, OtherLgActionText, %NPCEnull%
		GuiControl, NPCE_LegActions:Focus, OtherLgActionText
		Send ^{End}
		position += 1
	}
	Lgactionworks()
	Lgactionworks2()
return						;}

LrAction_Options:			;{
	GUI, NPCE_LairActions:submit, NoHide
	Loop % NPC_Lair_Actions.MaxIndex()
	{
		If (NPC_Lair_Actions[A_Index, "Name"] == "Options") {
			RemovedValue := NPC_Lair_Actions.RemoveAt(A_Index)
		}
	}
	LrActionOptions:= RegexReplace( LrActionOptions, "^\s+|\s+$" )
	LrActionOptions:= RegExReplace(LrActionOptions, "^(.)", "$u1")
	LrActionOptions:= LrActionOptions ":"
	Stringreplace, LrActionOptions, LrActionOptions, `:`:, `:, All
	Stringreplace, LrActionOptions, LrActionOptions, `.`.`:, `:, All
	Stringreplace, LrActionOptions, LrActionOptions, `.`:, `:, All
	Stringreplace, LrActionOptions, LrActionOptions, `.`., `:, All
	NPC_Lair_Actions.push({Name: "Options", LairAction: LrActionOptions})
	Lractionworks()
	Lractionworks2()
return						;}

LrAct_Other:				;{
	GUI, NPCE_LairActions:submit, NoHide
	Loop % NPC_Lair_Actions.MaxIndex()
	{
		If (NPC_Lair_Actions[A_Index, "Name"] == OtherLrActionName) {
			RemovedValue := NPC_Lair_Actions.RemoveAt(A_Index)
		}
	}
	If (OtherLrActionName and OtherLrActionText) {
		StringLower OtherLrActionName, OtherLrActionName, T
		OtherLrActionName := RegExReplace(OtherLrActionName, "^\W+", "")
		OtherLrActionName := RegExReplace(OtherLrActionName, "[^\w)]+$", "")
		OtherLrActionText:= RegExReplace(OtherLrActionText, "^(.)", "$u1")
		OtherLrActionText:= OtherLrActionText "."
		Stringreplace, OtherLrActionText, OtherLrActionText, `.`., `., All
		NPC_Lair_Actions.push({Name: OtherLrActionName, LairAction: OtherLrActionText})
		GuiControl, NPCE_LairActions:Choose, OtherLrActionName, 0
		GuiControl, NPCE_LairActions:, OtherLrActionText, %NPCEnull%
		GuiControl, NPCE_LairActions:Focus, OtherLrActionName
		Send ^{End}
		position += 1
	}
	Lractionworks()
	Lractionworks2()
return						;}

Delete_Trait:				;{
	GUI, NPCE_Main:submit, NoHide
	StringReplace Dummy, A_GUIControl, TDB, 
	RemovedValue:= NPC_Traits.RemoveAt(dummy)
	Traitworks()
	Build_RH_Box()
return						;}

Edit_Trait:					;{
	GUI, NPCE_Main:submit, NoHide
	StringReplace Dummy, A_GUIControl, TEB, 
	traitnameNew:= NPC_Traits[Dummy, "Name"]
	traitNew:= NPC_Traits[Dummy, "Trait"]
	GuiControl, NPCE_Main:Text, TraitNameNew, %traitnameNew%
	GuiControl, NPCE_Main:, TraitNew, %traitNew%
return						;}

TraitUp:					;{
	GUI, NPCE_Actions:submit, NoHide
	StringReplace Dummy, A_GUIControl, THB, 
	If (Dummy > 1) And NPC_Traits[Dummy, "name"]{
		tname:= NPC_Traits[Dummy, "name"]
		taction:= NPC_Traits[Dummy, "trait"]
		NPC_Traits[Dummy, "Name"]:= NPC_Traits[Dummy-1, "Name"]
		NPC_Traits[Dummy, "trait"]:= NPC_Traits[Dummy-1, "trait"]
		NPC_Traits[Dummy-1, "Name"]:= tname
		NPC_Traits[Dummy-1, "trait"]:= taction
		Traitworks()
		Build_RH_Box()
	}
return						;}

TraitDown:					;{
	GUI, NPCE_Actions:submit, NoHide
	StringReplace Dummy, A_GUIControl, TLB, 
	If (Dummy < NPC_Traits.MaxIndex()) {
		tname:= NPC_Traits[Dummy, "name"]
		taction:= NPC_Traits[Dummy, "trait"]
		NPC_Traits[Dummy, "Name"]:= NPC_Traits[Dummy+1, "Name"]
		NPC_Traits[Dummy, "trait"]:= NPC_Traits[Dummy+1, "trait"]
		NPC_Traits[Dummy+1, "Name"]:= tname
		NPC_Traits[Dummy+1, "trait"]:= taction
		Traitworks()
		Build_RH_Box()
	}
return						;}

Delete_Action:				;{
	GUI, NPCE_Main:submit, NoHide
	StringReplace Dummy, A_GUIControl, ActDB, 
	RemovedValue:= NPC_Actions.RemoveAt(dummy)
	Actionworks2()
	Actionworks3()
	Build_RH_Box()
return						;}

Delete_Action2:				;{
	GUI, NPCE_Actions:submit, NoHide
	StringReplace Dummy, A_GUIControl, ActDB, 
	RemovedValue:= NPC_Actions.RemoveAt(dummy)
	Actionworks2()
	Actionworks3()
	Build_RH_Box()
return						;}

Edit_Action:				;{
	GUI, NPCE_Actions:submit, NoHide
	StringReplace Dummy, A_GUIControl, ActEB, 
	
	If (NPC_Actions[Dummy, "Name"] = "Multiattack") {
		
	} else if (instr(NPC_Actions[Dummy, "Action"], "melee weapon attack:") or instr(NPC_Actions[Dummy, "Action"], "ranged weapon attack:") or instr(NPC_Actions[Dummy, "Action"], "melee spell attack:") or instr(NPC_Actions[Dummy, "Action"], "ranged spell attack:")) {
		parse_attack(Dummy)
	} Else {
		otheractionname:= NPC_Actions[Dummy, "Name"]
		otheractiontext:= NPC_Actions[Dummy, "Action"]
		GuiControl, NPCE_Actions:, otheractionname, %otheractionname%
		GuiControl, NPCE_Actions:ChooseString, otheractionname, %otheractionname%
		GuiControl, NPCE_Actions:, otheractiontext, %otheractiontext%
	}
return						;}

ActionUp:					;{
	GUI, NPCE_Actions:submit, NoHide
	StringReplace Dummy, A_GUIControl, ActHB, 
	If (Dummy > 1) And NPC_Actions[Dummy, "name"]{
		tname:= NPC_Actions[Dummy, "name"]
		taction:= NPC_Actions[Dummy, "Action"]
		NPC_Actions[Dummy, "Name"]:= NPC_Actions[Dummy-1, "Name"]
		NPC_Actions[Dummy, "Action"]:= NPC_Actions[Dummy-1, "Action"]
		NPC_Actions[Dummy-1, "Name"]:= tname
		NPC_Actions[Dummy-1, "Action"]:= taction
		Actionworks()
		Actionworks2()
		Actionworks3()
		Build_RH_Box()
		if WinExist(ahk_id %NPCE_Actions%)
			WinActivate
	}
return						;}

ActionDown:					;{
	GUI, NPCE_Actions:submit, NoHide
	StringReplace Dummy, A_GUIControl, ActLB, 
	If (Dummy < NPC_Actions.MaxIndex()) {
		tname:= NPC_Actions[Dummy, "name"]
		taction:= NPC_Actions[Dummy, "Action"]
		NPC_Actions[Dummy, "Name"]:= NPC_Actions[Dummy+1, "Name"]
		NPC_Actions[Dummy, "Action"]:= NPC_Actions[Dummy+1, "Action"]
		NPC_Actions[Dummy+1, "Name"]:= tname
		NPC_Actions[Dummy+1, "Action"]:= taction
		Actionworks()
		Actionworks2()
		Actionworks3()
		Build_RH_Box()
		if WinExist(ahk_id %NPCE_Actions%)
			WinActivate
	}
return						;}

Edit_ReAction:				;{
	GUI, NPCE_ReActions:submit, NoHide
	StringReplace Dummy, A_GUIControl, ReActEB, 
	
	OtherReactionName:= NPC_Reactions[Dummy, "Name"]
	OtherReactionText:= NPC_Reactions[Dummy, "Reaction"]
	GuiControl, NPCE_ReActions:Text, OtherReactionName, %OtherReactionName%
	GuiControl, NPCE_ReActions:, OtherReactionText, %OtherReactionText%
return						;}

Delete_ReAction:			;{
	GUI, NPCE_Main:submit, NoHide
	StringReplace Dummy, A_GUIControl, ReActDB, 
	RemovedValue:= NPC_ReActions.RemoveAt(dummy)
	reactionworks()
	Build_RH_Box()
return						;}

Delete_ReAction2:			;{
	GUI, NPCE_Reactions:submit, NoHide
	StringReplace Dummy, A_GUIControl, ReActDB, 
	RemovedValue:= NPC_ReActions.RemoveAt(dummy)
	reactionworks()
	Build_RH_Box()
return						;}

ReActionUp:					;{
	GUI, NPCE_ReActions:submit, NoHide
	StringReplace Dummy, A_GUIControl, ReActHB, 
	If (Dummy > 1) And NPC_ReActions[Dummy, "name"]{
		tname:= NPC_ReActions[Dummy, "name"]
		taction:= NPC_ReActions[Dummy, "ReAction"]
		NPC_ReActions[Dummy, "Name"]:= NPC_ReActions[Dummy-1, "Name"]
		NPC_ReActions[Dummy, "ReAction"]:= NPC_ReActions[Dummy-1, "ReAction"]
		NPC_ReActions[Dummy-1, "Name"]:= tname
		NPC_ReActions[Dummy-1, "ReAction"]:= taction
		reactionworks()
		Build_RH_Box()
		if WinExist(ahk_id %NPCE_ReActions%)
			WinActivate
	}
return						;}

ReActionDown:				;{
	GUI, NPCE_ReActions:submit, NoHide
	StringReplace Dummy, A_GUIControl, ReActLB, 
	If (Dummy < NPC_ReActions.MaxIndex()) {
		tname:= NPC_ReActions[Dummy, "name"]
		taction:= NPC_ReActions[Dummy, "ReAction"]
		NPC_ReActions[Dummy, "Name"]:= NPC_ReActions[Dummy+1, "Name"]
		NPC_ReActions[Dummy, "ReAction"]:= NPC_ReActions[Dummy+1, "ReAction"]
		NPC_ReActions[Dummy+1, "Name"]:= tname
		NPC_ReActions[Dummy+1, "ReAction"]:= taction
		reactionworks()
		Build_RH_Box()
		if WinExist(ahk_id %NPCE_ReActions%)
			WinActivate
	}
return						;}

Delete_LgAction:			;{
	GUI, NPCE_Main:submit, NoHide
	StringReplace Dummy, A_GUIControl, lgActDB, 
	RemovedValue:= NPC_Legendary_Actions.RemoveAt(dummy)
	lgactionworks()
	lgactionworks2()
	Build_RH_Box()
return						;}

Delete_LgAction2:			;{
	GUI, NPCE_LegActions:submit, NoHide
	StringReplace Dummy, A_GUIControl, lgActDB, 
	RemovedValue:= NPC_Legendary_Actions.RemoveAt(dummy)
	lgactionworks()
	lgactionworks2()
	Build_RH_Box()
return						;}

LgActionUp:					;{
	GUI, NPCE_LegActions:submit, NoHide
	StringReplace Dummy, A_GUIControl, LgActHB, 
	If (Dummy > 2) And NPC_Legendary_Actions[Dummy, "name"]{
		tname:= NPC_Legendary_Actions[Dummy, "name"]
		taction:= NPC_Legendary_Actions[Dummy, "LegAction"]
		NPC_Legendary_Actions[Dummy, "Name"]:= NPC_Legendary_Actions[Dummy-1, "Name"]
		NPC_Legendary_Actions[Dummy, "LegAction"]:= NPC_Legendary_Actions[Dummy-1, "LegAction"]
		NPC_Legendary_Actions[Dummy-1, "Name"]:= tname
		NPC_Legendary_Actions[Dummy-1, "LegAction"]:= taction
		lgactionworks()
		lgactionworks2()
		lgactionworks3()
		Build_RH_Box()
		if WinExist(ahk_id %NPCE_LegActions%)
			WinActivate
	}
return						;}

LgActionDown:				;{
	GUI, NPCE_LegActions:submit, NoHide
	StringReplace Dummy, A_GUIControl, LgActLB, 
	If (Dummy < NPC_Legendary_Actions.MaxIndex()) {
		tname:= NPC_Legendary_Actions[Dummy, "name"]
		taction:= NPC_Legendary_Actions[Dummy, "LegAction"]
		NPC_Legendary_Actions[Dummy, "Name"]:= NPC_Legendary_Actions[Dummy+1, "Name"]
		NPC_Legendary_Actions[Dummy, "LegAction"]:= NPC_Legendary_Actions[Dummy+1, "LegAction"]
		NPC_Legendary_Actions[Dummy+1, "Name"]:= tname
		NPC_Legendary_Actions[Dummy+1, "LegAction"]:= taction
		lgactionworks()
		lgactionworks2()
		lgactionworks3()
		Build_RH_Box()
		if WinExist(ahk_id %NPCE_LegActions%)
			WinActivate
	}
return						;}

Delete_LrAction:			;{
	GUI, NPCE_Main:submit, NoHide
	StringReplace Dummy, A_GUIControl, LrActDB, 
	RemovedValue:= NPC_Lair_Actions.RemoveAt(dummy)
	lractionworks()
	lractionworks2()
	Build_RH_Box()
return						;}

Delete_LrAction2:			;{
	GUI, NPCE_LairActions:submit, NoHide
	StringReplace Dummy, A_GUIControl, LrActDB, 
	RemovedValue:= NPC_Lair_Actions.RemoveAt(dummy)
	lractionworks()
	lractionworks2()
	Build_RH_Box()
return						;}

LrActionUp:					;{
	GUI, NPCE_LairActions:submit, NoHide
	StringReplace Dummy, A_GUIControl, LrActHB, 
	If (Dummy > 1) And NPC_Lair_Actions[Dummy, "name"]{
		tname:= NPC_Lair_Actions[Dummy, "name"]
		taction:= NPC_Lair_Actions[Dummy, "LairAction"]
		NPC_Lair_Actions[Dummy, "Name"]:= NPC_Lair_Actions[Dummy-1, "Name"]
		NPC_Lair_Actions[Dummy, "LairAction"]:= NPC_Lair_Actions[Dummy-1, "LairAction"]
		NPC_Lair_Actions[Dummy-1, "Name"]:= tname
		NPC_Lair_Actions[Dummy-1, "LairAction"]:= taction
		lractionworks()
		lractionworks2()
		lractionworks3()
		Build_RH_Box()
		if WinExist(ahk_id %NPCE_LairActions%)
			WinActivate
	}
return						;}

LrActionDown:				;{
	GUI, NPCE_LairActions:submit, NoHide
	StringReplace Dummy, A_GUIControl, LrActLB, 
	If (Dummy < NPC_Lair_Actions.MaxIndex()) {
		tname:= NPC_Lair_Actions[Dummy, "name"]
		taction:= NPC_Lair_Actions[Dummy, "LairAction"]
		NPC_Lair_Actions[Dummy, "Name"]:= NPC_Lair_Actions[Dummy+1, "Name"]
		NPC_Lair_Actions[Dummy, "LairAction"]:= NPC_Lair_Actions[Dummy+1, "LairAction"]
		NPC_Lair_Actions[Dummy+1, "Name"]:= tname
		NPC_Lair_Actions[Dummy+1, "LairAction"]:= taction
		lractionworks()
		lractionworks2()
		lractionworks3()
		Build_RH_Box()
		if WinExist(ahk_id %NPCE_LairActions%)
			WinActivate
	}
return						;}

NPCE_ActAdd_Close:
NPCE_ActAddGuiClose:		;{
	Gui, NPCE_Main:-disabled
	Gui, NPCE_ActAdd:Destroy
	Hotkey, RButton, Off
return						;}


ActAdd:						;{
	GUI, NPCE_ActAdd:submit, NoHide
	JSON_act_Name:= ""
	For a, b in ActDB.object()
	{
		if (a == OtherActionName) {
			JSON_act_Name:= a
		}
	}
	If JSON_act_Name {
		jack:= ActDB[JSON_act_Name].Action
		GuiControl, NPCE_ActAdd:, OtherActionText, %jack%
	}
return						;}

Delete_Actoth:				;{
	GUI, NPCE_ActAdd:submit, NoHide
	If OtherActionName {
		ActDB.delete(OtherActionName)
		ActDB.save(true)
		Actlist:= "|"
		For a, b in ActDB.object()
		{
			Actlist:= Actlist a "|"
		}
		GuiControl, NPCE_ActAdd:, OtherActionName, %Actlist%
		OtherActionName:= ""
		GuiControl, NPCE_ActAdd:, OtherActionText, 
		Actlist:= SubStr(Actlist, 2)
	}
return						;}

Add_Actoth:					;{
	GUI, NPCE_ActAdd:submit, NoHide
	
	JSON_act_Name:= ""
	For a, b in ActDB.object()
	{
		if (a == OtherActionName) {
			JSON_act_Name:= a
			ActDB[JSON_act_Name].Name:= Trim(OtherActionName)
			ActDB[JSON_act_Name].Action:= Trim(OtherActionText)
			ActDB.save(true)
		}
	}
	
	If !JSON_act_Name {
		Armoury:= {}
		Armoury[OtherActionName]:= {}
		Armoury[OtherActionName].Name:= Trim(OtherActionName)
		Armoury[OtherActionName].Action:= Trim(OtherActionText)
		ActDB.fill(Armoury)
		ActDB.save(true)
		Actlist:= "|"
		For a, b in ActDB.object()
		{
			Actlist:= Actlist a "|"
		}
		GuiControl, NPCE_ActAdd:, OtherActionName, %Actlist%
		GuiControl, NPCE_ActAdd:, OtherActionText, 
		Actlist:= SubStr(Actlist, 2)
	}
return						;}

OtherActionName:			;{
	GUI, NPCE_Actions:submit, NoHide
	JSON_act_Name:= ""
	For a, b in ActDB.object()
	{
		if (a == OtherActionName) {
			JSON_act_Name:= a
		}
	}
	If JSON_act_Name {
		jack:= ActDB[JSON_act_Name].Action
		jack:= GenderReplace(jack)
		GuiControl, NPCE_Actions:, OtherActionText, %jack%
	}
return						;}

OtherReActionName:			;{
	GUI, NPCE_ReActions:submit, NoHide
	JSON_act_Name:= ""
	For a, b in ActDB.object()
	{
		if (a == OtherReActionName) {
			JSON_act_Name:= a
		}
	}
	If JSON_act_Name {
		jack:= ActDB[JSON_act_Name].Action
		jack:= GenderReplace(jack)
		GuiControl, NPCE_ReActions:, OtherReActionText, %jack%
	}
return						;}

OtherLgActionName:			;{
	GUI, NPCE_LegActions:submit, NoHide
	JSON_act_Name:= ""
	For a, b in ActDB.object()
	{
		if (a == OtherLgActionName) {
			JSON_act_Name:= a
		}
	}
	If JSON_act_Name {
		jack:= ActDB[JSON_act_Name].Action
		jack:= GenderReplace(jack)
		GuiControl, NPCE_LegActions:, OtherLgActionText, %jack%
	}
return						;}

OtherLrActionName:			;{
	GUI, NPCE_LairActions:submit, NoHide
	JSON_act_Name:= ""
	For a, b in ActDB.object()
	{
		if (a == OtherLrActionName) {
			JSON_act_Name:= a
		}
	}
	If JSON_act_Name {
		jack:= ActDB[JSON_act_Name].Action
		jack:= GenderReplace(jack)
		GuiControl, NPCE_LairActions:, OtherLrActionText, %jack%
	}
return						;}

NPCE_TraitAdd_Close:
NPCE_TraitAddGuiClose:		;{
	Gui, NPCE_Main:-disabled
	Gui, NPCE_TraitAdd:Destroy
	Hotkey, RButton, Off
return						;}

NPCE_Terrain_Close:
NPCE_TerrainGuiClose:		;{
	GUI, NPCE_Terrain:submit, NoHide
	Sort.terrain:= ""
	loop % terraintype.length() {
		If (Terr%A_Index% = 1) {
			If (sort.terrain != "")
				Sort.terrain .= ", "
			Sort.terrain .= terraintype[A_Index]
		}
	}
	Sort.lore:= ""
	loop % originlore.length() {
		If (Orig%A_Index% = 1) {
			If (sort.lore != "")
				Sort.lore .= ", "
			Sort.lore .= originlore[A_Index]
		}
	}
	Gui, NPCE_Main:-disabled
	Gui, NPCE_Terrain:Destroy
return						;}


TraitAdd:					;{
	GUI, NPCE_TraitAdd:submit, NoHide
	JSON_trait_Name:= ""
	For a, b in traitDB.object()
	{
		if (a == TraitNameNew) {
			JSON_trait_Name:= a
		}
	}
	If JSON_trait_Name {
		jack:= traitDB[JSON_trait_Name].Trait
		GuiControl, NPCE_TraitAdd:, TraitNew, %jack%
	}
return						;}

Delete_TraitAdd:			;{
	GUI, NPCE_TraitAdd:submit, NoHide
	If TraitNameNew {
		traitDB.delete(TraitNameNew)
		traitDB.save(true)
		Traitlist:= "|"
		For a, b in traitDB.object()
		{
			Traitlist:= Traitlist a "|"
		}
		GuiControl, NPCE_TraitAdd:, TraitNameNew, %Traitlist%
		OtherActionName:= ""
		GuiControl, NPCE_TraitAdd:, TraitNew, 
		GuiControl, NPCE_Main:, TraitNameNew, %Traitlist%
		Traitlist:= SubStr(Traitlist, 2)
	}
return						;}

Add_TraitAdd:				;{
	GUI, NPCE_TraitAdd:submit, NoHide
	
	JSON_trait_Name:= ""
	For a, b in traitDB.object()
	{
		if (a == TraitNameNew) {
			JSON_trait_Name:= a
			traitDB[a].Trait:= Trim(TraitNew)
			traitDB.save(true)
		}
	}
	
	If !JSON_act_Name {
		Armoury:= {}
		Armoury[TraitNameNew]:= {}
		Armoury[TraitNameNew].Name:= Trim(TraitNameNew)
		Armoury[TraitNameNew].Trait:= Trim(TraitNew)
		traitDB.fill(Armoury)
		traitDB.save(true)
		Traitlist:= "|"
		For a, b in traitDB.object()
		{
			Traitlist:= Traitlist a "|"
		}
		GuiControl, NPCE_TraitAdd:, TraitNameNew, %Traitlist%
		GuiControl, NPCE_TraitAdd:, TraitNew, 
		GuiControl, NPCE_Main:, TraitNameNew, %Traitlist%
		Traitlist:= SubStr(Traitlist, 2)
	}
return						;}

LangDelete:					;{
	GUI, NPCE_LangAdd:submit, NoHide
	If LangDelList {
		LangDB.delete(LangDelList)
		LangDB.save(true)
		
		For key, value in LangUser {
			if (value = LangDelList) {
				del:= LangUser.removeat(key)
			}
		}
		
		temp:= "|"
		For a, b in LangDB.object()
		{
			temp:= temp a "|"
		}
		GuiControl, NPCE_LangAdd:, LangDelList, %temp%
		GuiControl, NPCE_LangAdd:, NewLang, 
		GuiControl, NPCE_Main:, UserLangs, %temp%
		temp:= SubStr(temp, 2)
	}
return						;}

LangAdd:					;{
	GUI, NPCE_LangAdd:submit, NoHide
	
	JSON_act_Name:= ""
	For a, b in LangDB.object()
	{
		if (a == NewLang) {
			JSON_act_Name:= a
		}
	}
	If !JSON_act_Name {
		Armoury:= {}
		Armoury[NewLang]:= {}
		Armoury[NewLang].Name:= Trim(NewLang)
		LangDB.fill(Armoury)
		LangDB.save(true)
		LangUser.push(NewLang)
		
		
		temp:= "|"
		For a, b in LangDB.object()
		{
			temp:= temp a "|"
		}
		GuiControl, NPCE_LangAdd:, LangDelList, %temp%
		GuiControl, NPCE_LangAdd:, NewLang, 
		GuiControl, NPCE_Main:, UserLangs, %temp%
		temp:= SubStr(temp, 2)
	}
return						;}

NPCE_LangAdd_Close:
NPCE_LangAddGuiClose:		;{
	Gui, NPCE_Main:-disabled
	Gui, NPCE_LangAdd:Destroy
return						;}

FSF:						;{
    TVM_ENSUREVISIBLE := 0x1114
    TVM_GETNEXTITEM := 0x110A
    TVGN_NEXTSELECTED := 0x000B
    AHK_CLASS := "ahk_class #32770"
    WinWaitActive % AHK_CLASS
    SendMessage, %TVM_GETNEXTITEM%, %TVGN_NEXTSELECTED%, 0, SysTreeView321, % AHK_CLASS
    SendMessage, %TVM_ENSUREVISIBLE%, 0, %ErrorLevel%, SysTreeView321, % AHK_CLASS
return						;}

HP_Average:					;{
	GUI, NPCE_HP:submit, NoHide
	regexmatch(HPstring, "\((\d+)d(\d+) *([+-]) *(\d+)\)", pattern)
	if (pattern3 = "+") {
		setHP:= Floor(pattern1 * ((1 + pattern2)/2)) + pattern4
	} else if (pattern3 = "-") {
		setHP:= Floor(pattern1 * ((1 + pattern2)/2)) - pattern4
	}
	HPstring:= setHP " (" pattern1 "d" pattern2 " " pattern3 " " pattern4 ")"
	GuiControl, NPCE_HP:Text, HPstring, %HPstring%
return						;}

HP_Roll:					;{
	GUI, NPCE_HP:submit, NoHide
	regexmatch(HPstring, "\((\d+)d(\d+) *([+-]) *(\d+)\)", pattern)
	setHP:= 0
	Loop, %pattern1%
	{
		Random, randomHP, 1, pattern2
		setHP:= setHP + randomHP
	}
	if (pattern3 = "+") {
		setHP += pattern4
	} else if (pattern3 = "-") {
		setHP -= pattern4
	}
	HPstring:= setHP " (" pattern1 "d" pattern2 " " pattern3 " " pattern4 ")"
	GuiControl, NPCE_HP:Text, HPstring, %HPstring%
return						;}

MakeHP:						;{
	GUI, NPCE_HP:submit, NoHide
	If (HPbo < 0)
		HPstring:= "(" HPno "d" HPdi " - " abs(HPbo) ")"
	else
		HPstring:= "(" HPno "d" HPdi " + " HPbo ")"
	GuiControl, NPCE_HP:Text, HPstring, %HPstring%
return						;}

Import_Text:				;{
Import_Text(value)
return						;}

JSONchoose:					;{
	GUI, NPCE_JSON:submit, NoHide
	JSON_Ob_Name:= ""
	For a, b in npc.object()
	{
		if (NPC[a].name == JSONchoose) {
			JSON_Ob_Name:= a
		}
	}
	JSON_This_Text:= NPC[JSON_Ob_Name].Name Chr(10)
	JSON_This_Text:= JSON_This_Text NPC[JSON_Ob_Name].Size " " NPC[JSON_Ob_Name].Type Chr(10)
	JSON_This_Text:= JSON_This_Text NPC[JSON_Ob_Name].Alignment
	GuiControl, NPCE_JSON:Text, JSONselected, %JSON_This_Text%
 return						;}

KeyPress:					;{
	GuiControl, NPCE_Main:Choose,SysTabControl321,% RegExReplace(A_ThisHotkey,"F")
return						;}

Manage_JSON:				;{
	Critical
	If (ProjectLive != 1) {
		MsgBox, 16, No Project, You must load a project *.ini`nto manipulate NPCs in its JSON file., 3
		gosub, Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			GUI_JSON()
			Gui, NPCE_Main:+disabled
			Gui, NPCE_JSON:Show, w320 h210, Edit or Delete NPCs in the JSON file
		} else {
			MsgBox, 16, Engineer Suite Parser only, This function can only be carried out whilst using the Engineer Suite Parser., 3
		}
	}
 return						;}

Manage_Weapons:				;{
	Critical
	GUI_Weapons()
	Gui, NPCE_Main:+disabled
Return						;}

MenuHandler:
Return

New_NPC:					;{
	Critical
	Initialise()
	Declare_Vars()
	Parser()
	ScrollPoint:= 0
	ScrollEnd:= 0
	NPCSavePath:= ""
	Save_File:= ""
	NPCgender:= "Neutral"
	NPCunique:= "0"
	NPCpropername:= "0"
	Chosen_Desc_Text:= ""
	FGcat:= Modname
	GuiControl, NPCE_Main:, Chosen_Desc_Text, %Chosen_Desc_Text%
	GuiControl, NPCE_Main:, NPC_FS_STR, %NPC_FS_STR%
	GuiControl, NPCE_Main:, NPC_FS_DEX, %NPC_FS_DEX%
	GuiControl, NPCE_Main:, NPC_FS_CON, %NPC_FS_CON%
	GuiControl, NPCE_Main:, NPC_FS_INT, %NPC_FS_INT%
	GuiControl, NPCE_Main:, NPC_FS_WIS, %NPC_FS_WIS%
	GuiControl, NPCE_Main:, NPC_FS_CHA, %NPC_FS_CHA%
	If !pdid {
		LangSelect:= 1
		GuiControl, NPCE_Main:Choose, LangSelect, %LangSelect%
		Lang_Set()
		Inject_Vars()
	}
	Loopvar:= 0
	While (Loopvar < 10) {
		Tempone:= Loopvar
		InSp_%Loopvar%_spells:= ""
		Sp_%Loopvar%_spells:= ""
		TempThree:= Sp_%Loopvar%_casts
		GuiControl, NPCE_Main:Text, Sp_%Loopvar%_casts, 
		temptwo:= InSp_%Loopvar%_spells
		GuiControl, NPCE_Main:, InSp_%Loopvar%_spells, %TempTwo%
		temptwo:= Sp_%Loopvar%_spells
		GuiControl, NPCE_Main:, Sp_%Loopvar%_spells, %TempTwo%
		Loopvar+=1
	}
	Loopvar:= 1
	While (Loopvar < 10) {
		GuiControl, NPCE_Main:, traitname%Loopvar%, 
		GuiControl, NPCE_Main:, trait%Loopvar%, 
		Loopvar += 1
	}
	GuiControl, NPCE_Main:show, GoingToken
	GuiControl, NPCE_Main:hide, NPCToken
	GuiControl, , NPCToken
	GuiControl, NPCE_Main:show, NPCToken
	GuiControl, NPCE_Main:hide, NPCImage
	GuiControl, , NPCImage
	GuiControl, NPCE_Main:show, NPCImage
	GuiControl, NPCE_Actions:, Multi_attack, %Multi_attack%
	GuiControl, NPCE_Actions:, multi_attack_Text, %multi_attack_Text%
return						;}

Reset_ArtInf:				;{
	NpcArtPref:= "Artwork by"
	GuiControl, NPCE_Options:, NpcArtPref, %NpcArtPref%
return						;}

NPCE_Help_Option:			;{
return						;}

NPCE_HP_Cancel:
NPCE_HPGuiClose:			;{
	Build_RH_Box()
	Gui, NPCE_Main:-disabled
	Gui, NPCE_HP:Hide
return						;}

NPCE_HP_Return:				;{
	GUI, NPCE_HP:submit, NoHide
	NPChp:= HPstring
	GuiControl, NPCE_Main:Text, NPChp, %NPChp%
	Build_RH_Box()
	Gui, NPCE_Main:-disabled
	Gui, NPCE_HP:Hide
return						;}

NPCE_Actions_Close:
NPCE_ActionsGuiClose:		;{
	Build_RH_Box()
	Gui, NPCE_Main:-disabled
	Gui, NPCE_Actions:Destroy
return						;}

NPCE_Reactions_Close:
NPCE_ReactionsGuiClose:		;{
	Build_RH_Box()
	Gui, NPCE_Main:-disabled
	Gui, NPCE_Reactions:Destroy
return						;}

NPCE_Weapons_Close:
NPCE_WeaponsGuiClose:		;{
	Build_RH_Box()
	Gui, NPCE_Main:-disabled
	Gui, NPCE_Weapons:Destroy
return						;}

NPCE_Legactions_Close:
	GUI, NPCE_Legactions:submit, NoHide
	Loop % NPC_Legendary_Actions.MaxIndex()
	{
		If (NPC_Legendary_Actions[A_Index, "Name"] != "Options") {
			NPC_Legendary_Actions[A_Index, "Name"]:= lgactionnameB%A_Index%
			NPC_Legendary_Actions[A_Index, "LegAction"]:= lgactionB%A_Index%
		}
	}
	Lgactionworks()
	Lgactionworks2()
NPCE_LegactionsGuiClose:	;{
	Build_RH_Box()
	Gui, NPCE_Main:-disabled
	Gui, NPCE_Legactions:Destroy
return						;}

NPCE_Lairactions_Close:
	GUI, NPCE_Lairactions:submit, NoHide
	Loop % NPC_Lair_Actions.MaxIndex()
	{
		If (NPC_Lair_Actions[A_Index, "Name"] != "Options") {
			NPC_Lair_Actions[A_Index, "Name"]:= lractionnameB%A_Index%
			NPC_Lair_Actions[A_Index, "LairAction"]:= lractionB%A_Index%
		}
	}
	Lractionworks()
	Lractionworks2()
NPCE_LairactionsGuiClose:	;{
	Build_RH_Box()
	Gui, NPCE_Main:-disabled
	Gui, NPCE_Lairactions:Destroy
return						;}


NPCE_Descrip_Update_Output: ;{
	Form_Desc:= Tokenise(RE1.GetRTF(False))
	Chosen_Desc_Text:= RegExReplace(Form_Desc, "U)<.*>", "")
	Chosen_Desc_Text:= RegexReplace(Chosen_Desc_Text, "^\s+|\s+$" )
	if (NPCE_changecheck != Chosen_Desc_Text) or (changeform = "1")
		Build_RH_Box()
	NPCE_changecheck:= Chosen_Desc_Text
	changeform:= 0
return						;}

NPCE_Import_Append:			;{
	GUI, NPCE_Import:submit, NoHide
	Cap_text:= Cap_Text . Clipboard
	Clipimp:= Clipboard
	Clipboard:= ""
	Cap_text := RegExReplace(Cap_text, "\R", "`r`n") 
	Cap_text:= RegExReplace(Cap_text,"\s*$","") ; remove trailing newlines
	Cap_text:= Cap_text Chr(13) Chr(10)
	GuiControl, NPCE_Import:, Cap_Text, %Cap_Text%
	WorkingString:= Cap_Text
	Main_Loop()
	Parser()
	GraphicalRTF("FT2")
	GuiControl, NPCE_Import:Focus, Cap_text
	Send ^{End}
return						;}


NPCE_JSON_Cancel:
NPCE_JSONGuiClose:			;{
	Gui, NPCE_Main:-disabled
	Gui, NPCE_JSON:Destroy
return						;}

NPCE_Import_Cancel:
NPCE_ImportGuiClose:		;{
	If Clipimp
		Clipboard:= Clipimp
	GuiControl, NPCE_Import:, Cap_text
	GuiControl, NPCE_Import:, Fix_text
	NPC_Restore()
	GetCoords(NPCE_Import,"NPC")
	Gui, NPCE_Main:-disabled
	Gui, NPCE_Import:Destroy
return						;}

NPCE_Import_Delete:			;{
	Cap_Text:= ""
	Fix_text:= ""
	If Clipimp
		Clipboard:= Clipimp
	GuiControl, NPCE_Import:, Cap_text
	GuiControl, NPCE_Import:, Fix_text
	Getvars_Main()
return						;}

NPCE_Import_Return:			;{
	Critical
	If Clipimp
		Clipboard:= Clipimp
	GetCoords(NPCE_Import,"NPC")
	Gui, NPCE_Main:-disabled
	Gui, NPCE_Import:Destroy
	Inject_Vars()
	Build_RH_Box()
	notify:= NPCName " imported successfully."
	Toast(notify)
return						;}

NPCE_Import_Update_Output:	;{
	GUI, NPCE_Import:submit, NoHide
	If Cap_Text {
		WorkingString:= Cap_Text
		StringReplace, WorkingString, WorkingString,`n,`r`n, All
		Main_Loop()
		Parser()
		GraphicalRTF("FT2")
		GuiControl, NPCE_Import:Focus, Cap_text
	}
return						;}

NPCE_JSON_Del:				;{
	If JSON_Ob_Name {
		NPC.delete(JSON_Ob_Name)
		NPC.save(true)
		npc_list:= "|Choose an NPC from the JSON file||"
		For a, b in npc.object()
		{
			npc_list:= npc_list NPC[a].name "|"
		}
		GuiControl, NPCE_JSON:, JSONChoose, %npc_list%
		JSON_Ob_Name:= ""
		GuiControl, NPCE_JSON:, JSONselected, %JSON_Ob_Name%
		Build_RH_Box()
	}
return						;}

NPCE_JSON_Edit:				;{
	If JSON_Ob_Name {
		Edit_NPC_JSON(JSON_Ob_Name)
		Gui, NPCE_Main:-disabled
		Gui, NPCE_JSON:Destroy
	}
return						;}

NPCE_MainGuiClose:			;{
	File:= A_AppData "\NPC Engineer\NPC Engineer.ini"
	Stub:= "Paths"
	IniWrite, %ProjectPath%, %File%, %Stub%, ProjectPath
	IniWrite, %NPCSavePath%, %File%, %Stub%, NPCSavePath
	Stub:= "LaunchNPCE"
	IniWrite, %NPCskin%, %File%, %Stub%, NPCskin
	win.npcengineer:= 0
	If (NPCE_Ref = "self") {
		ExitApp
	} Else {
		Gui, NPCE_Main:Destroy
		Gui, NPCE_Import:Destroy
		Gui, NPCE_Actions:Destroy
		Gui, NPCE_Reactions:Destroy
		Gui, NPCE_LegActions:Destroy
		Gui, NPCE_LairActions:Destroy
		Gui, NPCE_About:Destroy
		Gui, NPCE_HP:Destroy
		Gui, NPCE_Options:Destroy
		If (NPCE_Ref != "self") {
			Gui, %NPCE_Ref%:show
		}
		StBar(NPCE_Ref)
	}
return						;}

NPCImage:					;{
	TempImagePath:= NPCImagePath
	FileSelectFile, NPCImagePath, , , Select an image., (*.jpg)
	If !(FileExist(NPCImagePath)) {
		NPCImagePath:= TempImagePath
		return
	}
Load_NPCImage:
	hBM := LoadPicture( NPCImagePath )
	IfEqual, hBM, 0, Return

	BITMAP := getHBMinfo( hBM )                                ; Extract Width andh height of image 
	New := ScaleRect( BITMAP.Width, BITMAP.Height, 586, 396 )  ; Derive best-fit W x H for source image 

	DllCall( "DeleteObject", "Ptr",hBM )                       ; Delete Image handle ...         
	hBM := LoadPicture( NPCImagePath, "GDI+ w" New.W . " h" . New.H )  ; ..and get a new one with correct W x H

	GuiControl, NPCE_Main:, NPCImage,  *w0 *h0 HBITMAP:%hBM%
	Desc_Image_Link:= 1
	vDesc_Image_Link.set(Desc_Image_Link)
	Build_RH_Box()
return						;}

NpcClearImage:				;{
	NPCImage:= ""
	GuiControl, NPCE_Main:hide, NPCImage
	GuiControl, , NPCImage
	GuiControl, NPCE_Main:show, NPCImage
	NPCImagePath:= ""
	Desc_Image_Link:= 0
	vDesc_Image_Link.set(Desc_Image_Link)
	Build_RH_Box()
return						;}

NPCToken:					;{
	FileSelectFile, NPCTokenPath, , , Select an image., (*.png)
	If !(FileExist(NPCTokenPath)) {
		return
	}
Load_NPCToken:
	hBM := LoadPicture( NPCTokenPath )
	IfEqual, hBM, 0, Return

	BITMAP := getHBMinfo( hBM )                                ; Extract Width andh height of image 
	New := ScaleRect( BITMAP.Width, BITMAP.Height, 70, 70 )  ; Derive best-fit W x H for source image 

	DllCall( "DeleteObject", "Ptr",hBM )                       ; Delete Image handle ...         
	hBM := LoadPicture( NPCTokenPath, "GDI+ w" New.W . " h" . New.H )  ; ..and get a new one with correct W x H

	GuiControl, NPCE_Main:Hide, GoingToken
	GuiControl, NPCE_Main:, NPCToken,  *w0 *h0 HBITMAP:%hBM% 
return						;}

NPCE_Changelog:				;{
	Run http://www.masq.net/2017/11/new-version-changes.html
return						;}

Open_NPC:					;{
	Critical
	TempWorkingDir:= A_WorkingDir
	If NPCCopyPics
		SetWorkingDir %DataDir%
	NPC_Load()
	SetWorkingDir %TempWorkingDir%
	NPCpassperc:= tempPP
	GuiControl, NPCE_Main:, NPCpassperc, %NPCpassperc%
return						;}

Next_NPC:					;{
	Critical
	if (Mod_Parser == 1) {
		NPCNameTemp:= vNPCname.get()
		FlagTemp:= 0
		olda:= ""
		For a, b in npc.object()
		{
			if flagtemp {
				flags.NPC:= 1
				stringreplace NPCSavePath, NPCSavePath, %olda%.npc, %a%.npc
				Gosub Open_NPC
				FlagTemp:= ""
				olda:= ""
				return
			}
			if (NPC[a].name = NPCNameTemp) {
				FlagTemp:= 1
				olda:= a
			}
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This NPC is not in the current Project., 2
	} else {
		MsgBox, 16, NPC Engineer Parser only, This function can only be carried out whilst using NPC Engineer's Parser., 3
	}
return						;}

Prev_NPC:					;{
	if (Mod_Parser == 1) {
		NPCNameTemp:= vNPCname.get()
		FlagTemp:= 0
		Counttemp:= 0
		olda:= ""
		For a, b in npc.object()
		{
			if (A_Index = 1)
				olda:= a
			if (NPC[a].name = NPCNameTemp) {
				FlagTemp:= 1
			}
			if flagtemp {
				flags.NPC:= 1
				stringreplace NPCSavePath, NPCSavePath, %a%.npc, %olda%.npc
				Gosub Open_NPC
				FlagTemp:= ""
				olda:= ""
				return
			}
			olda:= a
		}
		if !flagtemp
			MsgBox, 16, Not in Project, This NPC is not in the current Project., 2
	} else {
		MsgBox, 16, NPC Engineer Parser only, This function can only be carried out whilst using NPC Engineer's Parser., 3
	}
return						;}

ParseProject:				;{
	Critical
	If (ProjectLive != 1) {
		MsgBox, 16, No Project, You must load a project *.ini`nbefore trying to parse., 3
		gosub, Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			ParseProject()
		} else {
			MsgBox, 16, NPC Engineer Parser only, This function can only be carried out whilst using NPC Engineer's Parser., 3
		}
	}
 return						;}

Project_Manage:				;{
	Critical
	Gui, NPCE_Main:+disabled
	ProjectEngineer("NPCE_Main")
return						;}

Save_NPC:					;{
	TempWorkingDir:= A_WorkingDir
	SetWorkingDir %A_ScriptDir%
	NPCEng_Save_File()
	NPCEng_Do_Save()
	SetWorkingDir %TempWorkingDir%
	if SaveCheck {
		Toast(NPCName " saved successfully.")
	}
return						;}

Save_XML:					;{
	XML_Save_File()
	XML_Do_Save()
return						;}

Save_HTML:					;{
	ExportFormats()
	HTML_Do_Save()
return						;}

Save_RTF:					;{
	ExportFormats()
	RTF_Do_Save()
return						;}

Copy_BB:					;{
	ExportFormats()
	Clipboard:= BB_Stat_Block
return						;}

STTransfer:					;{
	If (A_GuiEvent = "DoubleClick") {
		GUI, NPCE_Main:submit, NoHide
		NPCstrsav:= RegExReplace(NPCstrbo, "\+")
		NPCdexsav:= RegExReplace(NPCdexbo, "\+")
		NPCconsav:= RegExReplace(NPCconbo, "\+")
		NPCintsav:= RegExReplace(NPCintbo, "\+")
		NPCwissav:= RegExReplace(NPCwisbo, "\+")
		NPCchasav:= RegExReplace(NPCchabo, "\+")
		vNPCstrsav.Set(NPCstrsav)
		vNPCdexsav.Set(NPCdexsav)
		vNPCconsav.Set(NPCconsav)
		vNPCintsav.Set(NPCintsav)
		vNPCwissav.Set(NPCwissav)
		vNPCchasav.Set(NPCchasav)
		Build_RH_Box()
	}
return						;}

Paste:						;{
	GUI, NPCE_Main:submit, NoHide
	if(MainTabName == "Description") {
		RE_Paste()
	} Else {
		send ^v
	}
Return						;}


;~ ######################################################
;~ #                   Function List.                   #
;~ ######################################################


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |           Formatting Input PlugIns           |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


Import_DDBeyond(ByRef Import) {
	global
	stringreplace import, import, %A_Space%`r`n, `r`n, all
	stringreplace import, import, `r`n`r`n, `r`n, all
	stringreplace import, import, `r`n`r`n, `r`n, all
}

Import_DDWiki(ByRef Import) {
	global
	Import:= RegExReplace(Import, "U)\[edit\]\r\n", Chr(13) Chr(10))
	Import:= RegExReplace(Import, "  +", " ")

	stringreplace, Import, Import, 1st-level (, 1st Level (, All
	stringreplace, Import, Import, 1st-level (, 1st Level (, All
	stringreplace, Import, Import, 2nd-level (, 2nd Level (, All
	stringreplace, Import, Import, 3rd-level (, 3rd Level (, All
	stringreplace, Import, Import, 4th-level (, 4th Level (, All
	stringreplace, Import, Import, 5th-level (, 5th Level (, All
	stringreplace, Import, Import, 6th-level (, 6th Level (, All
	stringreplace, Import, Import, 7th-level (, 7th Level (, All
	stringreplace, Import, Import, 8th-level (, 8th Level (, All
	stringreplace, Import, Import, 9th-level (, 9th Level (, All
	stringreplace, Import, Import, knows the following, has the following, 
	stringreplace, Import, Import, spells`:`r`n, spells prepared`:`r`n, 
	stringreplace, Import, Import, %A_Tab%, %A_Space%, All
	stringreplace, Import, Import, %A_Space%%A_Space%, %A_Space%, All
	stringreplace, Import, Import, %A_Space%`r`n, `r`n, All
	stringreplace, Import, Import, `r`n`r`n, `r`n, All
	stringreplace, Import, Import, �, -, All
	stringreplace, Import, Import, –, -, All
}

Import_Donjon(ByRef Import) {
	global
	stringreplace, Import, Import, %A_Tab%, %A_Space%, All
	stringreplace, Import, Import, `r`n`r`n, `r`n, All
	Impcopy:= ""
	Loop, parse, Import, `n, `r
	{
		stringreplace, thing, A_Loopfield, `:, `.
		impcopy:= Impcopy thing Chr(13) Chr(10)
	}
	
	Import:= Impcopy

	stringreplace, Import, Import, At will`., At will`:, All
	stringreplace, Import, Import, /day`., /day`:, All
	stringreplace, Import, Import, /day each`., /day each`:, All
	stringreplace, Import, Import, will`)`., will`)`:, All
	stringreplace, Import, Import, slots`)`., slots`)`:, All
	stringreplace, Import, Import, slot`)`., slot`)`:, All
}

Import_CritterDB(ByRef Import) {
	global
	Import:= RegExReplace(Import, "U)\r\n.*CR\r\n", Chr(13) Chr(10))
	Import:= RegExReplace(Import, "\r\nDescription\r\n.*", Chr(13) Chr(10))

	stringreplace, Import, Import, Cantrip (, Cantrips (, All
	stringreplace, Import, Import, %A_Space%%A_Space%, %A_Space%, All
	stringreplace, Import, Import, %A_Tab%, %A_Space%, All
	stringreplace, Import, Import, `r`n`r`n, `r`n, All
}

Import_RPGTinker(ByRef Import) {
	global
	Import:= Import Chr(13) Chr(10)
	xxfoundpos1:= RegExMatch(Import, "UP)WIS\r\n.*\)\r\n", xxlength)
	xxwisbo:= SubStr(Import, xxfoundpos1, xxlength)
	stringreplace, xxwisbo, xxwisbo, `r`n, , All
	stringreplace, xxwisbo, xxwisbo, ), , All
	xxwisbo:= RegExReplace(xxwisbo, "WIS.*\(", "")
	
	NPCUnique:= 1
	NPCpropername:= 1
	
	Import:= RegExReplace(Import, "U)\r\n.*CR\r\n", Chr(13) Chr(10))
	Import:= RegExReplace(Import, "\r\nDescription\r\n.*", Chr(13) Chr(10))

	stringreplace, Import, Import, `r`n`r`n, `r`n, All
	stringreplace, Import, Import, %A_Space%%A_Space%, %A_Space%, All
	stringreplace, Import, Import, %A_Tab%, %A_Space%, All
	stringreplace, Import, Import, %A_Tab%, %A_Space%, All
	stringreplace, Import, Import, %A_Space%`), `), All

	xxfoundpos1:= RegExMatch(Import, "STR\r\n")
	xxfoundpos2:= RegExMatch(Import, "UP)CHA\r\n.*\)\r\n", xxlength)
	xxstats:= SubStr(Import, xxfoundpos1, xxfoundpos2-xxfoundpos1+xxlength)
	xxstrep:= xxstats
	
	stringreplace, xxstrep, xxstrep, `r`n, %A_Space%, All
	
	stringreplace, xxstrep, xxstrep, STR%A_Space%, ,
	stringreplace, xxstrep, xxstrep, DEX%A_Space%, ,
	stringreplace, xxstrep, xxstrep, CON%A_Space%, ,
	stringreplace, xxstrep, xxstrep, INT%A_Space%, ,
	stringreplace, xxstrep, xxstrep, WIS%A_Space%, ,
	stringreplace, xxstrep, xxstrep, CHA%A_Space%, ,
	xxstrep:= trim(xxstrep)
	xxstrep:= "STR DEX CON INT WIS CHA" Chr(13) Chr(10) xxstrep Chr(13) Chr(10)
	StringReplace, Import, Import, %xxstats%, %xxstrep%

	stringreplace, Import, Import, Armor Class:, Armor Class
	stringreplace, Import, Import, Hit Points:, Hit Points
	stringreplace, Import, Import, Speed:, Speed
	stringreplace, Import, Import, Challenge:, Challenge

	Import:= RegExReplace(Import, "U) : .*\r\n", Chr(13) Chr(10), , 1)
	
	xxfoundpos2:= RegExMatch(Import, "UP)\r\n.*any alignment\r\n", xxlength)
	xxrace_etc:= SubStr(Import, xxfoundpos2, xxlength)
	xxrarep:= xxrace_etc
	StringSplit, Array, xxrarep, %A_Space%	
	stringreplace, NPCgender, Array1, `r`n, 
	stringreplace, xxrarep, xxrarep, %NPCgender%%A_Space%, Medium humanoid (
	stringreplace, xxrarep, xxrarep, `,, )`,
	stringreplace, xxrarep, xxrarep, any alignment, unaligned
	stringreplace, Import, Import, %xxrace_etc%, %xxrarep%
	xxfoundpos2:= RegExMatch(Import, "UP)\r\nSkills:.*\r\n", xxlength)
	xxskills:= SubStr(Import, xxfoundpos2, xxlength)
	xxskrep:= xxskills
	stringreplace, xxskrep, xxskrep, `:%A_Space%, `:
	xxskrep:= RegExReplace(xxskrep, "U) \+(\d+) ", " +$1, ")
	stringreplace, xxskrep, xxskrep, `:, %A_Space%
	stringreplace, Import, Import, %xxskills%, %xxskrep%
	
	Import:= RegExReplace(Import, "U) \(\d+m.*sqr\)", "")
	Import:= RegExReplace(Import, "U)Proficiency:.*\r\n", "")
	Import:= RegExReplace(Import, "U)Properties:.*\r\n", "")
	Import:= RegExReplace(Import, "U)Ability Modifiers:.*\r\n", "")
	Import:= RegExReplace(Import, "U)Skill Versatility:.*\r\n", "")
	Import:= RegExReplace(Import, "U)Racial Features\r\n", "")

	xxfoundpos2:= RegExMatch(Import, "UP)Languages:.*\r\n", xxlength)
	xxlangs:= SubStr(Import, xxfoundpos2, xxlength)
	stringreplace, Import, Import, %xxlangs%, 
	stringreplace, xxlangs, xxlangs, : speaks, 
	stringreplace, xxlangs, xxlangs, %A_Space%and one extra., 
	stringreplace, xxlangs, xxlangs, %A_Space%and two extra., 
	stringreplace, xxlangs, xxlangs, %A_Space%and%A_Space%, `,%A_Space%

	xxfoundpos2:= RegExMatch(Import, "UP)Darkvision:.*\r\n", xxlength)
	xxsenses:= SubStr(Import, xxfoundpos2, xxlength)
	stringreplace, Import, Import, %xxsenses%, 
	stringreplace, xxsenses, xxsenses, Darkvision:, Senses darkvision
	xxpasper:= xxwisbo+10
	If xxsenses
		stringreplace, xxsenses, xxsenses, ft, %A_Space%ft.`, passive Perception%A_Space%%xxpasper%
	else
		xxsenses:= "Senses passive Perception " xxpasper Chr(13) Chr(10)
	
	Impcopy:= ""
	Loop, parse, Import, `n, `r
	{
		stringreplace, thing, A_Loopfield, `:, `.
		impcopy:= Impcopy thing Chr(13) Chr(10)
	}
	Import:= Impcopy
	stringreplace, Import, Import, At will`., At will`:, All
	stringreplace, Import, Import, /day`., /day`:, All
	stringreplace, Import, Import, /day each`., /day each`:, All
	stringreplace, Import, Import, will`)`., will`)`:, All
	stringreplace, Import, Import, slots`)`., slots`)`:, All
	stringreplace, Import, Import, slot`)`., slot`)`:, All

	xxfoundpos1:= RegExMatch(Import, "U)\r\nChallenge.*\r\n")
	xxfirstbit:= SubStr(Import, 1, xxfoundpos1+1)
	stringreplace, Import, Import, %xxfirstbit%, 

	xxfoundpos1:= RegExMatch(Import, "U)\r\nActions\r\n")
	xxsecondbit:= SubStr(Import, 1, xxfoundpos1+1)
	stringreplace, Import, Import, %xxsecondbit%, 
	
	xxfoundpos1:= RegExMatch(Import, "U)\r\nSpells\r\n")
	If xxfoundpos1 {
		xxspells:= SubStr(Import, xxfoundpos1+2)
		stringreplace, Import, Import, %xxspells%, 
		stringreplace, xxspells, xxspells, Spells`r`n, 
		stringreplace, xxspells, xxspells, to hit with spell attacks%A_Space%, 
		FoundPos := RegExMatch(xxspells, "U)-level (.*)\.", Match)
		stringreplace, xxspells, xxspells, an, a
		stringreplace, xxspells, xxspells, level wizard, level spellcaster
		stringreplace, xxspells, xxspells, level cleric, level spellcaster
		stringreplace, xxspells, xxspells, )`r`n, %A_Space%to hit with spell attacks)`. It has the following %Match1% spells prepared:`r`n
	}
	
	Import:= xxfirstbit xxsenses xxlangs xxsecondbit xxspells import
	stringreplace, Import, Import, `r`n`r`n, `r`n, All
	stringreplace, Import, Import, `,`r`n, `r`n, All
}

Import_Incarnate(ByRef Import) {
	global
	StringReplace, Import, Import, %A_Space%##~, ##~, All
	StringReplace, Import, Import, ~##%A_Space%, ~##, All
	StringReplace, Import, Import, ##~LR~##, `r`n, All
}

Import_Word_1(ByRef Import) {
	global
	stringreplace, Import, Import, `r`n, QQQLFQQQ, All
	Import:= regexreplace(Import,"[^a-zA-Z0-9_()+\-'.,!\:;?*]+", " ")
	stringreplace, Import, Import, QQQLFQQQ, `r`n, All
	stringreplace, Import, Import, Languages-, Languages -, All
	stringreplace, Import, Import, Actions `r`n, Actions`r`n, All
	stringreplace, Import, Import, `) `r`n, `)`r`n, All
	stringreplace, Import, Import, CHA%A_Space%, CHA, All
	Loop {
		quickmatch:= regexmatch(Import,"\w\(")
		If quickmatch {
			quickchar:= SubStr(Import, quickmatch, 1)
			quickfind:= quickchar "("
			quickrepl:= quickchar " ("
			stringreplace, Import, Import, %quickfind%, %quickrepl%, All
		}
	} Until (quickmatch = FALSE)
}

Import_PDF_1(ByRef Import) {
	global
	pos:= RegExMatch(import, "Us)STR\r\n.*CHA\r\n", length)
	if length {
		StringReplace, import, import, %length%, ,
		StringReplace, length, length, `r`n, , all
		length:= RegExReplace(length, "STR ?", "")
		length:= RegExReplace(length, "DEX ?", "")
		length:= RegExReplace(length, "CON ?", "")
		length:= RegExReplace(length, "INT ?", "")
		length:= RegExReplace(length, "WIS ?", "")
		length:= RegExReplace(length, "CHA ?", "")
		length:= "STR DEX CON INT WIS CHA " length Chr(13) Chr(10)
		
		pos:= RegExMatch(import, "PU)Speed.*ft\.\r\n", opener)
		NewStr := SubStr(import, 1, pos + opener - 1)
		StringReplace, import, import, %NewStr%, ,
		import:= NewStr length import
	}
	StringReplace, import, import, AC:, Armor Class,
}

Import_PDF_2(ByRef Import) {
	global
	StringReplace, import, import, %A_SPACE%%A_SPACE%CHA%A_SPACE%, %A_SPACE%CHA, All
	StringReplace, import, import, %A_SPACE%%A_SPACE%, %A_SPACE%, All
	StringReplace, import, import, `r`n`r`n, `r`n, All
	StringReplace, import, import, (normally`r`nlawful good), `r`n, All
	if instr(import, "; DEX") {
		StringReplace, import, import, `; DEX, , All
		StringReplace, import, import, `; CON, , All
		StringReplace, import, import, `; INT, , All
		StringReplace, import, import, `; WIS, , All
		StringReplace, import, import, `; CHA, , All
		StringReplace, import, import, STR%A_SPACE%, STR DEX CON INT WIS CHA`r`n, 
	}
	StringReplace, import, import, `r`n%A_SPACE%`r`n, `r`n, All
	StringReplace, import, import, Like Ability (at will):, Like Ability (at will)., All
	StringReplace, import, import, )%A_SPACE%`r`n, )`r`n, All
	StringReplace, import, import, ACTIONS%A_SPACE%`r`n, ACTIONS`r`n, All
	import:= RegExReplace(import, "U) CR .*\r\n", Chr(13) Chr(10))
	xxfoundpos1:= RegExMatch(Import, "i)Equipment\r\n")
	if xxfoundpos1 {
		xxequipment:= Substr(Import, xxfoundpos1)
		StringReplace, Import, Import, %xxequipment%, 
		StringReplace, xxequipment, xxequipment, Equipment`r`n, Equipment.%A_SPACE%, All
		StringReplace, xxequipment, xxequipment, `r`n, , All
		xxequipment =  %xxequipment%
		If !(SubStr(xxequipment, 0, 1) = ".")
			xxequipment:= xxequipment "."
		;~ StringReplace, xxequipment, xxequipment, %A_SPACE%., ., All
		xxequipment:= xxequipment Chr(13) Chr(10)
	
		xxfoundpos1:= RegExMatch(Import, "iPU)\(.*XP\)\r\n", xxlen1)
		xxothblock:= SubStr(Import, xxfoundpos1 + xxlen1)
		StringReplace, Import, Import, %xxothblock%, 
		import:= Import xxequipment xxothblock
	}
}

Import_DDBeyondOld(ByRef Import) {
	global
	stringreplace, Import, Import, MONSTER RULES`r`n, , 
	stringreplace, Import, Import, CREATE A MONSTER`r`n, , 
	stringreplace, Import, Import, BROWSE HOMEBREW`r`n, , 
	stringreplace, Import, Import, subscribe link`r`n, , 
	stringreplace, Import, Import, `r`n`r`n, `r`n, All
	stringreplace, Import, Import, `r`n%A_Space%`r`n, `r`n, All
	stringreplace, Import, Import, %A_Space%`r`n, `r`n, All
	
	xxfoundpos1:= RegExMatch(Import, "STR\r\n")
	xxfoundpos2:= RegExMatch(Import, "CHALLENGE\r\n")
	xxname_etc:= SubStr(Import, 1, xxfoundpos1-1)
	xxstats:= SubStr(Import, xxfoundpos1, xxfoundpos2-xxfoundpos1)
	
	StringReplace, Import, Import, %xxname_etc%, 
	StringReplace, Import, Import, %xxstats%, 
	
	stringreplace, xxstats, xxstats, `r`n, %A_Space%, All
	stringreplace, xxstats, xxstats, STR%A_Space%, ,
	stringreplace, xxstats, xxstats, DEX%A_Space%, ,
	stringreplace, xxstats, xxstats, CON%A_Space%, ,
	stringreplace, xxstats, xxstats, INT%A_Space%, ,
	stringreplace, xxstats, xxstats, WIS%A_Space%, ,
	stringreplace, xxstats, xxstats, CHA%A_Space%, ,
	xxstats:= trim(xxstats)
	xxstats:= "STR DEX CON INT WIS CHA" Chr(13) Chr(10) xxstats Chr(13) Chr(10)
	
	xxfoundpos1:= RegExMatch(Import, "ARMOR CLASS\r\n")
	xxchallenge:= SubStr(Import, 1, xxfoundpos1-1)
	StringReplace, Import, Import, %xxchallenge%, 
	stringreplace, xxchallenge, xxchallenge, CHALLENGE`r`n, Challenge%A_Space%, All

	xxfoundpos1:= RegExMatch(Import, "HIT POINTS\r\n")
	xxac:= SubStr(Import, 1, xxfoundpos1-1)
	StringReplace, Import, Import, %xxac%, 
	stringreplace, xxac, xxac, ARMOR CLASS`r`n, Armor Class%A_Space%, All

	xxfoundpos1:= RegExMatch(Import, "SPEED\r\n")
	xxhp:= SubStr(Import, 1, xxfoundpos1-1)
	StringReplace, Import, Import, %xxhp%, 
	stringreplace, xxhp, xxhp, HIT POINTS`r`n, Hit Points%A_Space%, All


	stringreplace, Import, Import, Saving Throws`r`n, <XXX>Saving Throws`r`n, 
	stringreplace, Import, Import, Skills`r`n, <XXX>Skills`r`n, 
	stringreplace, Import, Import, Damage Vulnerabilities`r`n, <XXX>Damage Vulnerabilities`r`n, 
	stringreplace, Import, Import, Damage Resistances`r`n, <XXX>Damage Resistances`r`n, 
	stringreplace, Import, Import, Damage Immunities`r`n, <XXX>Damage Immunities`r`n, 
	stringreplace, Import, Import, Condition Immunities`r`n, <XXX>Condition Immunities`r`n, 
	stringreplace, Import, Import, Senses`r`n, <XXX>Senses`r`n, 
	stringreplace, Import, Import, Languages`r`n, <XXX>Languages%A_Space%, 
	stringreplace, Import, Import, (Spell DC, (spell save DC, 
	stringreplace, Import, Import, requiring no components`:, requiring no material components`:, 

	xxfoundpos1:= RegExMatch(Import, "<XXX>")
	xxspeed:= SubStr(Import, 1, xxfoundpos1+4)
	StringReplace, Import, Import, %xxspeed%, 
	stringreplace, xxspeed, xxspeed, <XXX>, , 
	stringreplace, xxspeed, xxspeed, SPEED`r`n, Speed%A_Space%, All
	stringreplace, xxspeed, xxspeed, `.%A_Space%`(, `.`,%A_Space%, All
	stringreplace, xxspeed, xxspeed, ), , All

	xxfoundpos1:= RegExMatch(Import, "Saving Throws\r\n")
	If (xxfoundpos1 == 1) {
		xxfoundpos2:= RegExMatch(Import, "<XXX>")
		xxsavs:= SubStr(Import, 1, xxfoundpos2+4)
		StringReplace, Import, Import, %xxsavs%, 
		stringreplace, xxsavs, xxsavs, <XXX>, , 
		stringreplace, xxsavs, xxsavs, Saving Throws`r`n, Saving Throws%A_Space%, 
		stringreplace, xxsavs, xxsavs, STR%A_Space%, Str%A_Space%, 
		stringreplace, xxsavs, xxsavs, DEX%A_Space%, Dex%A_Space%, 
		stringreplace, xxsavs, xxsavs, CON%A_Space%, Con%A_Space%, 
		stringreplace, xxsavs, xxsavs, WIS%A_Space%, Wis%A_Space%, 
		stringreplace, xxsavs, xxsavs, INT%A_Space%, Int%A_Space%, 
		stringreplace, xxsavs, xxsavs, CHA%A_Space%, Cha%A_Space%, 
	}

	xxfoundpos1:= RegExMatch(Import, "Skills\r\n")
	If (xxfoundpos1 == 1) {
		xxfoundpos2:= RegExMatch(Import, "<XXX>")
		xxskill:= SubStr(Import, 1, xxfoundpos2+4)
		StringReplace, Import, Import, %xxskill%, 
		stringreplace, xxskill, xxskill, <XXX>, , 
		stringreplace, xxskill, xxskill, Skills`r`n, Skills%A_Space%, 
	}

	xxfoundpos1:= RegExMatch(Import, "Damage Vulnerabilities\r\n")
	If (xxfoundpos1 == 1) {
		xxfoundpos2:= RegExMatch(Import, "<XXX>")
		xxdamvul:= SubStr(Import, 1, xxfoundpos2+4)
		StringReplace, Import, Import, %xxdamvul%, 
		stringreplace, xxdamvul, xxdamvul, <XXX>, , 
		stringreplace, xxdamvul, xxdamvul, Damage Vulnerabilities`r`n, Damage Vulnerabilities%A_Space%, 
	}

	xxfoundpos1:= RegExMatch(Import, "Damage Resistances\r\n")
	If (xxfoundpos1 == 1) {
		xxfoundpos2:= RegExMatch(Import, "<XXX>")
		xxdamres:= SubStr(Import, 1, xxfoundpos2+4)
		StringReplace, Import, Import, %xxdamres%, 
		stringreplace, xxdamres, xxdamres, <XXX>, , 
		stringreplace, xxdamres, xxdamres, Damage Resistances`r`n, Damage Resistances%A_Space%, 
	}

	xxfoundpos1:= RegExMatch(Import, "Damage Immunities\r\n")
	If (xxfoundpos1 == 1) {
		xxfoundpos2:= RegExMatch(Import, "<XXX>")
		xxdamimm:= SubStr(Import, 1, xxfoundpos2+4)
		StringReplace, Import, Import, %xxdamimm%, 
		stringreplace, xxdamimm, xxdamimm, <XXX>, , 
		stringreplace, xxdamimm, xxdamimm, Damage Immunities`r`n, Damage Immunities%A_Space%, 
	}

	xxfoundpos1:= RegExMatch(Import, "Condition Immunities\r\n")
	If (xxfoundpos1 == 1) {
		xxfoundpos2:= RegExMatch(Import, "<XXX>")
		xxconimm:= SubStr(Import, 1, xxfoundpos2+4)
		StringReplace, Import, Import, %xxconimm%, 
		stringreplace, xxconimm, xxconimm, <XXX>, , 
		stringreplace, xxconimm, xxconimm, Condition Immunities`r`n, Condition Immunities%A_Space%, 
	}

	xxfoundpos1:= RegExMatch(Import, "Senses\r\n")
	If (xxfoundpos1 == 1) {
		xxfoundpos2:= RegExMatch(Import, "<XXX>")
		xxsense:= SubStr(Import, 1, xxfoundpos2+4)
		StringReplace, Import, Import, %xxsense%, 
		stringreplace, xxsense, xxsense, <XXX>, , 
		stringreplace, xxsense, xxsense, Senses`r`n, Senses%A_Space%, 
	}

	xxfoundpos1:= RegExMatch(Import, "Languages")
	If (xxfoundpos1 == 1) {
		xxfoundpos2:= RegExMatch(Import, "\r\n")
		xxlang:= SubStr(Import, 1, xxfoundpos2+1)
		StringReplace, Import, Import, %xxlang%, 
	}

	Import:= xxname_etc xxac xxhp xxspeed xxstats xxsavs xxskill xxdamvul xxdamres xxdamimm xxconimm xxsense xxlang xxchallenge Import
}

Import_Roll20(ByRef Import) {
	global
	StringReplace, Import, Import, Spellcasting (Psionics), Spellcasting, All
	xxOut:= ""
	xxfound:= RegExMatch(Import, "U)""name"": ""(.*)"",", xx)
	xxOut .= xx1 Chr(13) Chr(10)
	xxname:= xx1
	xxfound:= RegExMatch(Import, "U)""size"": ""(.*)"",", xx)
	xxOut .= xx1 " "
	xxfound:= RegExMatch(Import, "U)""type"": ""(.*)"",", xx)
	xxOut .= xx1 ", "
	xxfound:= RegExMatch(Import, "U)""alignment"": ""(.*)"",", xx)
	xxOut .= xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""AC"": ""(.*)"",", xx)
	xxOut .= "Armor Class " xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""HP"": ""(.*)"",", xx)
	xxOut .= "Hit Points " xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""speed"": ""(.*)"",", xx)
	xxOut .= "Speed " xx1 Chr(13) Chr(10) "STR DEX CON INT WIS CHA" Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""strength"": (.*),", xx)
	xxOut .= xx1 " (+2) "
	xxfound:= RegExMatch(Import, "U)""dexterity"": (.*),", xx)
	xxOut .= xx1 " (+2) "
	xxfound:= RegExMatch(Import, "U)""constitution"": (.*),", xx)
	xxOut .= xx1 " (+2) "
	xxfound:= RegExMatch(Import, "U)""intelligence"": (.*),", xx)
	xxOut .= xx1 " (+2) "
	xxfound:= RegExMatch(Import, "U)""wisdom"": (.*),", xx)
	xxOut .= xx1 " (+2) "
	xxpassperc:= 10 + statbon(xx1)
	xxfound:= RegExMatch(Import, "U)""charisma"": (.*),", xx)
	xxOut .= xx1  " (+2)" Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""savingThrows"": ""(.*)"",", xx)
	If xxfound
		xxOut .= "Saving Throws " xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""skills"": ""(.*)"",", xx)
	If xxfound
		xxOut .= "Skills "xx1 Chr(13) Chr(10)
	
	
	xxfound:= RegExMatch(Import, "U)""damageVulnerabilities"": ""(.*)"",", xx)
	If xxfound
		xxOut .= "Damage Vulnerabilities "xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""damageResistances"": ""(.*)"",", xx)
	If xxfound
		xxOut .= "Damage Resistances "xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""damageImmunities"": ""(.*)"",", xx)
	If xxfound
		xxOut .= "Damage Immunities "xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""conditionImmunities"": ""(.*)"",", xx)
	If xxfound
		xxOut .= "Condition Immunities "xx1 Chr(13) Chr(10)
	
	
	
	xxfound:= RegExMatch(Import, "U)""senses"": ""(.*)"",", xx)
	If xxfound {
		xxOut .= "Senses " xx1 ", passive Perception " xxpassperc Chr(13) Chr(10)
	} else {
		xxOut .= "Senses passive Perception " xxpassperc Chr(13) Chr(10)
	}
	xxfound:= RegExMatch(Import, "U)""languages"": ""(.*)""", xx)
	xxOut .= "Languages " xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)""challenge"": ""(.*)"",", xx)
	xxcr:= XParray[xx1]
	xxOut .= "Challenge " xx1 "(" xxcr " XP)" Chr(13) Chr(10)

	xxfound:= RegExMatch(Import, "Us)""traits"": \[.*\]", xx)
	If xxfound {
		Loop {
			xyfound:= RegExMatch(xx, "Us){.*}", xy)
			if xyfound {
				xname:= RegExMatch(xy, "U)""name"": ""(.*)""", xn)
				xrech:= RegExMatch(xy, "U)""recharge"": ""(.*)""", xr)
				xtext:= RegExMatch(xy, "U)""text"": ""(.*)""", xt)
				if xrech {
					xn1 .= " (" xr1 ")"
				}
				StringReplace, xt1, xt1, \n, \r, All
				xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
				xx:= RegExReplace(xx, "Us){.*}", "",, 1)
			}
		} Until !xyfound
	}
	
	xxfound:= RegExMatch(Import, "Us)""actions"": \[.*\]", xx)
	If xxfound {
		xxOut .= "ACTIONS" Chr(13) Chr(10)
		Loop {
			xyfound:= RegExMatch(xx, "Us){.*}", xy)
			if xyfound {
				xname:= RegExMatch(xy, "U)""name"": ""(.*)""", xn)
				xrech:= RegExMatch(xy, "U)""recharge"": ""(.*)""", xr)
				xtext:= RegExMatch(xy, "U)""text"": ""(.*)""", xt)
				if xrech {
					xn1 .= " (" xr1 ")"
				}
				StringReplace, xt1, xt1, \n, \r, All
				xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
				xx:= RegExReplace(xx, "Us){.*}", "",, 1)
			}
		} Until !xyfound
	}
	
	xxfound:= RegExMatch(Import, "Us)""reactions"": \[.*\]", xx)
	If xxfound {
		xxOut .= "REACTIONS" Chr(13) Chr(10)
		Loop {
			xyfound:= RegExMatch(xx, "Us){.*}", xy)
			if xyfound {
				xname:= RegExMatch(xy, "U)""name"": ""(.*)""", xn)
				xrech:= RegExMatch(xy, "U)""recharge"": ""(.*)""", xr)
				xtext:= RegExMatch(xy, "U)""text"": ""(.*)""", xt)
				if xrech {
					xn1 .= " (" xr1 ")"
				}
				StringReplace, xt1, xt1, \n, \r, All
				xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
				xx:= RegExReplace(xx, "Us){.*}", "",, 1)
			}
		} Until !xyfound
	}
	
	xxfound:= RegExMatch(Import, "Us)""legendaryActions"": \[.*\]", xx)
	If xxfound {
		wwfound:= RegExMatch(Import, "Us)""legendaryPoints"": (\d)", ww)
		xxOut .= "LEGENDARY ACTIONS" Chr(13) Chr(10)
		xxOut .= "The " xxname " can take " ww1 " legendary actions, choosing from the options below. Only one legendary action option can be used at a time and only at the end of another creature's turn. The " xxname " regains spent legendary actions at the start of its turn." Chr(13) Chr(10)
		Loop {
			xyfound:= RegExMatch(xx, "Us){.*}", xy)
			if xyfound {
				xname:= RegExMatch(xy, "U)""name"": ""(.*)""", xn)
				xrech:= RegExMatch(xy, "U)""recharge"": ""(.*)""", xr)
				xtext:= RegExMatch(xy, "U)""text"": ""(.*)""", xt)
				if xrech {
					xn1 .= " (" xr1 ")"
				}
				StringReplace, xt1, xt1, \n, \r, All
				xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
				xx:= RegExReplace(xx, "Us){.*}", "",, 1)
			}
		} Until !xyfound
	}

	xxfound:= RegExMatch(Import, "Us)""lairActions"": \[.*\],", xx)
	If xxfound {
		xxOut .= "LAIR ACTIONS" Chr(13) Chr(10)
		Loop {
			xyfound:= RegExMatch(xx, "Us){.*}", xy)
			if xyfound {
				xname:= RegExMatch(xy, "U)""name"": ""(.*)""", xn)
				xrech:= RegExMatch(xy, "U)""recharge"": ""(.*)""", xr)
				xtext:= RegExMatch(xy, "U)""text"": ""(.*)""", xt)
				if xrech {
					xn1 .= " (" xr1 ")"
				}
				StringReplace, xt1, xt1, \n, \r, All
				xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
				xx:= RegExReplace(xx, "Us){.*}", "",, 1)
			}
		} Until !xyfound
	}


Import:= xxOut	
}

Import_Roll20Comp(ByRef Import) {
	global
	Import:= RegExReplace(Import, "    ", "")
	StringReplace, Import, Import, `r`n`r`n, `r`n, all
	Import:= RegExReplace(Import, " +", " ")
	pos:= RegExMatch(import, "Us)STR\r\n.*CHA\r\n", length)
	stored:= length
	if length {
		StringReplace, length, length, )`r`n, ) `r`n, all
		StringReplace, length, length, `r`n, , all
		length:= RegExReplace(length, "STR ?", "")
		length:= RegExReplace(length, "DEX ?", "")
		length:= RegExReplace(length, "CON ?", "")
		length:= RegExReplace(length, "INT ?", "")
		length:= RegExReplace(length, "WIS ?", "")
		length:= RegExReplace(length, "CHA ?", "")
		length:= "STR DEX CON INT WIS CHA " length 
		StringReplace, import, import, %stored%, %length%,
	}
	StringReplace, import, import, AC:, Armor Class,
}

Import_HeroLabs(ByRef Import) {
	global
	Import:= RegExReplace(Import, "s)Hero Lab and the .*", "")
	StringReplace, Import, Import, `r`n`r`n, `r`n, All
	StringReplace, Import, Import, no material components, no material components:, All
	Import:= RegExReplace(Import, "U)---*\r\n", "")
	Import:= RegExReplace(Import, "mU)^Lair Action: ", "")
	Import:= RegExReplace(Import, "U)STR (.*), DEX (.*), CON (.*), INT (.*), WIS (.*), CHA (.*)\r\n", "STR DEX CON INT WIS CHA $1 $2 $3 $4 $5 $6" Chr(13) Chr(10))
	p1:= regexmatch(Import, "Um)^.* Lair \(\d+/round\)")
	p2:= regexmatch(Import, "U)On initiative count 20")
	p3:= regexmatch(Import, "mUP)^Lair Actions\r\n", len)
	If (p1 and p2 and p3) {
		mark:= substr(Import, p1, p3-p1+len)
		Rep:= "Lair Actions" Chr(13) Chr(10)
		Rep .= substr(Import, p2, p3-p2)
		StringReplace, Import, Import, %mark%, %Rep%, All
	}
}

Import_FGXML(ByRef Import) {
	global
	stringreplace, import, import, %A_Tab%,,All
	stringreplace, import, import, &apos;, ',All
	xxOut:= ""
	xxfound:= RegExMatch(Import, "Us)</legendaryactions>.*<name type=""string"">(.*)</name>.*<reactions>", xx)
	If xxfound {
		xxOut .= xx1 Chr(13) Chr(10)
		xxname:= xx1
	} else {
		xxfound:= RegExMatch(Import, "U)<name type=""string"">(.*)</name>", xx)
		xxOut .= xx1 Chr(13) Chr(10)
		xxname:= xx1
	}
	xxfound:= RegExMatch(Import, "U)<size type=""string"">(.*)</size>", xx)
	xxOut .= xx1 " "
	xxfound:= RegExMatch(Import, "U)<type type=""string"">(.*)</type>", xx)
	xxOut .= xx1 ", "
	xxfound:= RegExMatch(Import, "U)<alignment type=""string"">(.*)</alignment>", xx)
	xxOut .= xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<ac type=""number"">(.*)</ac>", xx)
	xxfound:= RegExMatch(Import, "U)<actext type=""string"">(.*)</actext>", xy)
	xxOut .= "Armor Class " xx1 " " xy1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<hp type=""number"">(.*)</hp>", xx)
	xxfound:= RegExMatch(Import, "U)<hd type=""string"">(.*)</hd>", xy)
	xxOut .= "Hit Points " xx1 " " xy1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<speed type=""string"">(.*)</speed>", xx)
	xxOut .= "Speed " xx1 Chr(13) Chr(10) "STR DEX CON INT WIS CHA" Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "Us)<strength>.*<score type=""number"">(.*)</score>", xx)
	xxOut .= xx1 " (+2) "
	xxfound:= RegExMatch(Import, "Us)<dexterity>.*<score type=""number"">(.*)</score>", xx)
	xxOut .= xx1 " (+2) "
	xxfound:= RegExMatch(Import, "Us)<constitution>.*<score type=""number"">(.*)</score>", xx)
	xxOut .= xx1 " (+2) "
	xxfound:= RegExMatch(Import, "Us)<intelligence>.*<score type=""number"">(.*)</score>", xx)
	xxOut .= xx1 " (+2) "
	xxfound:= RegExMatch(Import, "Us)<wisdom>.*<score type=""number"">(.*)</score>", xx)
	xxOut .= xx1 " (+2) "
	xxfound:= RegExMatch(Import, "Us)<charisma>.*<score type=""number"">(.*)</score>", xx)
	xxOut .= xx1  " (+2)" Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<savingthrows type=""string"">(.*)</savingthrows>", xx)
	If xxfound
		xxOut .= "Saving Throws " xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<skills type=""string"">(.*)</skills>", xx)
	If xxfound
		xxOut .= "Skills "xx1 Chr(13) Chr(10)
	
	
	xxfound:= RegExMatch(Import, "U)<damagevulnerabilities type=""string"">(.*)</damagevulnerabilities>", xx)
	If xxfound
		xxOut .= "Damage Vulnerabilities "xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<damageresistances type=""string"">(.*)</damageresistances>", xx)
	If xxfound
		xxOut .= "Damage Resistances "xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<damageimmunities type=""string"">(.*)</damageimmunities>", xx)
	If xxfound
		xxOut .= "Damage Immunities "xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<conditionimmunities type=""string"">(.*)</conditionimmunities>", xx)
	If xxfound
		xxOut .= "Condition Immunities "xx1 Chr(13) Chr(10)
	
	
	
	xxfound:= RegExMatch(Import, "U)<senses type=""string"">(.*)</senses>", xx)
	If xxfound
		xxOut .= "Senses " xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<languages type=""string"">(.*)</languages>", xx)
	xxOut .= "Languages " xx1 Chr(13) Chr(10)
	xxfound:= RegExMatch(Import, "U)<cr type=""string"">(.*)</cr>", xx)
	xxcr:= XParray[xx1]
	xxOut .= "Challenge " xx1 " (" xxcr " XP)" Chr(13) Chr(10)

	xxfound:= RegExMatch(Import, "Us)<traits>(.*)</traits>", xx)
	If xxfound {
		Loop {
			xyfound:= RegExMatch(xx1, "OUs)<id.*>(.*)</id.*>", match)
			if xyfound {
				choppity:= match.value(0)
				xname:= RegExMatch(choppity, "U)<name type=""string"">(.*)</name>", xn)
				xtext:= RegExMatch(choppity, "Us)<desc type=""string"">(.*)</desc>", xt)
				StringReplace, xt1, xt1, \n, \r, All
				xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
				Stringreplace xx1, xx1, %choppity%, ,
			}
		} Until !xyfound
	}
	
	xxfound:= RegExMatch(Import, "Us)<actions>(.*)</actions>", xx)
	If xxfound {
		xxOut .= "ACTIONS" Chr(13) Chr(10)
		Loop {
			xyfound:= RegExMatch(xx1, "OUs)<id.*>(.*)</id.*>", match)
			if xyfound {
				choppity:= match.value(0)
				xname:= RegExMatch(choppity, "U)<name type=""string"">(.*)</name>", xn)
				xtext:= RegExMatch(choppity, "Us)<desc type=""string"">(.*)</desc>", xt)
				StringReplace, xt1, xt1, \n, \r, All
				xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
				Stringreplace xx1, xx1, %choppity%, ,
			}
		} Until !xyfound
	}
	
	xxfound:= RegExMatch(Import, "Us)<reactions>(.*)</reactions>", xx)
	If xxfound {
		xxOut .= "REACTIONS" Chr(13) Chr(10)
		Loop {
			xyfound:= RegExMatch(xx1, "OUs)<id.*>(.*)</id.*>", match)
			if xyfound {
				choppity:= match.value(0)
				xname:= RegExMatch(choppity, "U)<name type=""string"">(.*)</name>", xn)
				xtext:= RegExMatch(choppity, "Us)<desc type=""string"">(.*)</desc>", xt)
				StringReplace, xt1, xt1, \n, \r, All
				xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
				Stringreplace xx1, xx1, %choppity%, ,
			}
		} Until !xyfound
	}
	
	xxfound:= RegExMatch(Import, "Us)<legendaryactions>(.*)</legendaryactions>", xx)
	If xxfound {
		anything:= 0
		xx2:= xx1
		Loop {
			xyfound:= RegExMatch(xx2, "OUs)<id.*>(.*)</id.*>", match)
			if xyfound {
				anything:= 1
				choppity:= match.value(0)
				Stringreplace xx2, xx2, %choppity%, ,
			}
		} Until !xyfound
		If anything {
			xxOut .= "LEGENDARY ACTIONS" Chr(13) Chr(10)
			Loop {
				xyfound:= RegExMatch(xx1, "OUs)<id.*>(.*)</id.*>", match)
				if xyfound {
					choppity:= match.value(0)
					xname:= RegExMatch(choppity, "U)<name type=""string"">(.*)</name>", xn)
					xtext:= RegExMatch(choppity, "Us)<desc type=""string"">(.*)</desc>", xt)
					StringReplace, xt1, xt1, \n, \r, All
					xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
					Stringreplace xx1, xx1, %choppity%, ,
				}
			} Until !xyfound
		}
	}

	xxfound:= RegExMatch(Import, "Us)<lairactions>(.*)</lairactions>", xx)
	If xxfound {
		anything:= 0
		xx2:= xx1
		Loop {
			xyfound:= RegExMatch(xx2, "OUs)<id.*>(.*)</id.*>", match)
			if xyfound {
				anything:= 1
				choppity:= match.value(0)
				Stringreplace xx2, xx2, %choppity%, ,
			}
		} Until !xyfound
		If anything {
			xxOut .= "LAIR ACTIONS" Chr(13) Chr(10)
			Loop {
				xyfound:= RegExMatch(xx1, "OUs)<id.*>(.*)</id.*>", match)
				if xyfound {
					choppity:= match.value(0)
					xname:= RegExMatch(choppity, "U)<name type=""string"">(.*)</name>", xn)
					xtext:= RegExMatch(choppity, "Us)<desc type=""string"">(.*)</desc>", xt)
					StringReplace, xt1, xt1, \n, \r, All
					xxOut .= xn1 ". " xt1 Chr(13) Chr(10)
					Stringreplace xx1, xx1, %choppity%, ,
				}
			} Until !xyfound
		}
	}

	stringreplace, import, import, `r`n`r`n, `r`n, All

	import:= xxOut
}




;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                   GUI Calls                  |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


Add_Action(value) {
	Critical
	Gui, NPCE_Main:+disabled
	GUI_Actions()
}

Add_ReAction(value) {
	Critical
	Gui, NPCE_Main:+disabled
	GUI_ReActions()
}

Add_LgAction(value) {
	Critical
	Gui, NPCE_Main:+disabled
	GUI_Legactions()
}

Add_LrAction(value) {
	Critical
	Gui, NPCE_Main:+disabled
	GUI_LairActions()
}

Add_Trait(value) {
	Global
	traitnameNew:= vtraitnameNew.get()
	traitNew:= vtraitNew.get()
	Loop % NPC_Traits.MaxIndex()
	{
		If (NPC_Traits[A_Index, "Name"] = traitnameNew) {
			RemovedValue := NPC_Traits.RemoveAt(A_Index)
		}
	}
	If (traitnameNew and traitNew) {
		StringLower traitnameNew, traitnameNew, T
		traitnameNew := RegExReplace(traitnameNew, "^\W+", "")
		traitnameNew := RegExReplace(traitnameNew, "[^\w)]+$", "")
		StringReplace, traitNew, traitNew, `n, \r, all
		traitNew:= RegExReplace(traitNew, "^(.)", "$u1")
		NPC_Traits.push({Name: traitnameNew, Trait: traitNew})
		Traitworks()
		GuiControl, NPCE_Main:Text, TraitNameNew, 
		traitnameNew:= ""
		GuiControl, NPCE_Main:, TraitNew, 
		GuiControl, NPCE_Main:Focus, TraitNew
		Send ^{End}
		Build_RH_Box()
	}
}

Desc_Text_Delete(value) {
	global
	RE1.SetText("")
	GuiControl, Focus, % RE1.HWND
}

HP_Average(value) {
	global NPChp
	GetVars_Main()
	regexmatch(NPChp, "\((\d+)d(\d+) *([+-]) *(\d+)\)", pattern)
	if (pattern3 = "+") {
		setHP:= Floor(pattern1 * ((1 + pattern2)/2)) + pattern4
	} else if (pattern3 = "-") {
		setHP:= Floor(pattern1 * ((1 + pattern2)/2)) - pattern4
	}
	NPChp:= setHP " (" pattern1 "d" pattern2 " " pattern3 " " pattern4 ")"
	GuiControl, NPCE_Main:Text, NPChp, %NPChp%
}

HP_build() {
	Gui, NPCE_Main:+disabled
	Gui, NPCE_HP:Show, w225 h130, Edit NPC Hit dice
}

HP_Roll(value) {
	global NPChp
	GetVars_Main()
	regexmatch(NPChp, "\((\d+)d(\d+) *([+-]) *(\d+)\)", pattern)
	setHP:= 0
	Loop, %pattern1%
	{
		Random, randomHP, 1, pattern2
		setHP:= setHP + randomHP
	}
	if (pattern3 = "+") {
		setHP += pattern4
	} else if (pattern3 = "-") {
		setHP -= pattern4
	}
	NPChp:= setHP " (" pattern1 "d" pattern2 " " pattern3 " " pattern4 ")"
	GuiControl, NPCE_Main:Text, NPChp, %NPChp%
}

Import_Text(value) {
	Global
	Critical
	GUI_Import()
	Cap_Text:= ""
	Fix_text:= ""
	Clipimp:= ""
	NPC_Backup()
	GuiControl, NPCE_Import:, Cap_text
	GraphicalRTF("FT2")
	
	guiX:= ImpG.NPCX
	guiY:= ImpG.NPCY
	if (guix = -1) or (guiy = -1) {
		Gui, NPCE_Main:+disabled
		Gui, NPCE_Import:Show, w990 h550, Text Import
	} else {
		Gui, NPCE_Main:+disabled
		Gui, NPCE_Import:Show, x%GuiX% y%GuiY% w990 h550, Text Import
	}
	guiX:= ""
	guiY:= ""
	
	GuiControl, NPCE_Import:Focus, Cap_text
}

Output_Append(value) {
	Global
	If !NPCjpeg{
		return
	}
	If (ProjectLive!= 1) or (!IsObject(NPC) AND Mod_Parser = 1) {
		MsgBox, 16, No Project, The following must be true to add NPCs to a project:`n`n * You have created a new project or loaded a project *.ini.`n * You have enabled NPCs by clicking the checkbox.
		gosub, Project_Manage
		return
	} Else {
		if (Mod_Parser == 1) {
			NPCEP_Append()
		} else if (Mod_Parser == 2) {
			Par5e_Append()
		}
		Build_RH_Box()
	}
}

Par5e_Append() {
	global
	GUI, NPCE_Main:submit, NoHide
	path:= ModPath "input\npcs.txt"
	FileAppend, %Save_File%, %path%
	If (NPCTokenPath and NPCjpeg) {
		Ifexist, %NPCTokenPath%
		{
			ThumbDest:= ModPath . "input\tokens\" . NPCjpeg . ".*"
			FileCopy, %NPCTokenPath%, %ThumbDest%, 1
			NPCTokenPath:= ThumbDest
		}
	}
	If (NPCImagePath and NPCjpeg) {
		Ifexist, %NPCImagePath%
		{
			ThumbDest:= ModPath . "input\images\" . NPCjpeg . ".*"
			FileCopy, %NPCImagePath%, %ThumbDest%, 1
			NPCImagePath:= ThumbDest
		}
	}

	notify:= NPCName " appended to" ModName "."
	Toast(notify)
}

NPCEP_Append() {
	global
	NPCEParse_Save_File()
	JSON_Ob_Exist:= ""
	For a, b in npc.object()
	{
		if (a == NPCjpeg) {
			JSON_Ob_Exist:= a
		}
	}

	If JSON_Ob_Exist {
		tempname:= NPC[JSON_Ob_Exist].name
		MsgBox, 292, Overwrite NPC, The NPC '%tempname%' already exists in the project's JSON file.`nDo you wish to overwrite it with this data? This is unrecoverable!
		IfMsgBox Yes
		{
			NPC.delete(JSON_Ob_Exist)
			NPC.fill(ObjNPC)
			NPC.save(true)
			If (NPCTokenPath and NPCjpeg) {
				tempdir:= dataDir "\" NPCTokenPath
				Ifexist, %NPCTokenPath%
				{
					ThumbDest:= ModPath . "input\tokens\" . NPCjpeg . ".png"
					FileCopy, %NPCTokenPath%, %ThumbDest%, 1
					NPCTokenPath:= ThumbDest
				}
			}
			If (NPCImagePath and NPCjpeg) {
				Ifexist, %NPCImagePath%
				{
					ThumbDest:= ModPath . "input\images\" . NPCjpeg . ".jpg"
					FileCopy, %NPCImagePath%, %ThumbDest%, 1
					NPCImagePath:= ThumbDest
				}
			}

			notify:= NPCName " updated in " ModName "."
			Toast(notify)
		}
	} else {
		NPC.fill(ObjNPC)
		NPC.save(true)
		If (NPCTokenPath and NPCjpeg) {
			Ifexist, %NPCTokenPath%
			{
				ThumbDest:= ModPath . "input\tokens\" . NPCjpeg . ".*"
				FileCopy, %NPCTokenPath%, %ThumbDest%, 1
				NPCTokenPath:= ThumbDest
			}
		}
		If (NPCImagePath and NPCjpeg) {
			Ifexist, %NPCImagePath%
			{
				ThumbDest:= ModPath . "input\images\" . NPCjpeg . ".*"
				FileCopy, %NPCImagePath%, %ThumbDest%, 1
				NPCImagePath:= ThumbDest
			}
		}

		notify:= NPCName " added to " ModName "."
		Toast(notify)
	}
	
}

TraitAddName(value) {
	Global
	GUI, NPCE_Main:submit, NoHide
	JSON_trait_Name:= ""
	For a, b in traitDB.object()
	{
		if (a == TraitNameNew) {
			JSON_trait_Name:= a
		}
	}
	If JSON_trait_Name {
		jack:= traitDB[JSON_trait_Name].Trait
		jack:= GenderReplace(jack)
		GuiControl, NPCE_Main:, TraitNew, %jack%
	}
}

FG_Lists(value) {
	GUI_Terrain()
}

Output_to_Clipboard(value) {
	global
	Critical
	GUI, NPCE_Main:submit, NoHide
	Clipboard:= Save_File
	notify:= NPCName " copied to clipboard."
	Toast(notify)
}

SBOutput_to_Clipboard(value) {
	global
	Critical
	GUI, NPCE_Main:submit, NoHide
	ExportFormats()
	WinClip.Clear()
	WinClip.SetRTF(CopyBlock)
	WinClip.Paste()
	notify:= NPCName " copied to clipboard."
	Toast(notify)
}

Save_NPC(value) {
	Gosub Save_NPC
}

Update_Output_Main(value) {
	global
	Critical
	GetVars_Main()
	Build_RH_Box()
}

Update_Output_Main_CR(value) {
	global
	GetVars_Main()
	NPCxp:= XParray[NPCcharat]
	vNPCxp.set(NPCxp)
	Build_RH_Box()
}

Update_Output_Stats(value) {
	global
	GetVars_Main()
	NPCstrbo:= StatBon(NPCstr)
	NPCdexbo:= StatBon(NPCdex)
	NPCconbo:= StatBon(NPCcon)
	NPCintbo:= StatBon(NPCint)
	NPCwisbo:= StatBon(NPCwis)
	NPCchabo:= StatBon(NPCcha)
	vNPCstrbo.Set(NPCstrbo)
	vNPCdexbo.Set(NPCdexbo)
	vNPCconbo.Set(NPCconbo)
	vNPCintbo.Set(NPCintbo)
	vNPCwisbo.Set(NPCwisbo)
	vNPCchabo.Set(NPCchabo)
	If (A_GuiControl = "wisboy") {
		PassPer()
	}
	Build_RH_Box()
}

Update_Dmg_DV(value) {
	Global
	GetVars_Main()
	flagdamvul:= 0
	Loop, 13
	{
		if cbDV%A_Index%
			flagdamvul:= 1
	}
	Build_RH_Box()
}

Update_Dmg_DR(value) {
	Global
	GetVars_Main()
	flagdamres:= 0
	Loop, 13
	{
		if cbDR%A_Index%
			flagdamres:= 1
	}
	Build_RH_Box()
}

Update_Dmg_DI(value) {
	Global
	GetVars_Main()
	flagdamimm:= 0
	Loop, 13
	{
		if cbDI%A_Index%
			flagdamimm:= 1
	}
	Build_RH_Box()
}

Update_Dmg_CI(value) {
	Global
	GetVars_Main()
	flagconimm:= 0
	Loop, 16
	{
		if cbCI%A_Index%
			flagconimm:= 1
	}
	If cbCI16
		NPC_conimm[16]:= CI16
	else
		NPC_conimm[16]:= ""
	Build_RH_Box()
}

Update_Skills(value) {
	Global
	GetVars_Main()
	If (NPC_Skills["Perception"] != sk_perc) {
		Skill_Set()
		PassPer()
	} else {
		Skill_Set()
	}
	Build_RH_Box()
}

Update_Casting(value) {
	Global
	GetVars_Main()
	if NPCSpellStar {
		if (Substr(NPCSpellStar,1,1) != "*") {
			NPCSpellStar:= "*" NPCSpellStar
			GuiControl, NPCE_Main:, NPCSpellStar, %NPCSpellStar%
			GuiControl, NPCE_Main:Focus, NPCSpellStar
			Send ^{End}
		}
	}
	Build_SW()
	Spell_Works()
	Build_RH_Box()
}

Update_InCasting(value) {
	Global
	GetVars_Main()
	Build_ISW()
	InSpell_Works()
	Build_RH_Box()
}

Update_Psionics(value) {
	Global
	GetVars_Main()
	If NPCPsionics {
		NPCinsptext:= "requiring no components"
		GuiControl, NPCE_Main:, NPCinsptext, %NPCinsptext%
	}
	Build_ISW()
	InSpell_Works()
	Build_RH_Box()
}

ThemeMenu(Nm, Ps, Mu) {
	Global NPCskin
	Menu ThemeMenu, Uncheck, Parchment
	Menu ThemeMenu, Uncheck, Frost
	Menu ThemeMenu, Uncheck, Jungle
	Menu ThemeMenu, Uncheck, Blood
	Menu ThemeMenu, Uncheck, Flame
	Menu ThemeMenu, Check, %Nm%
	NPCskin:= Nm
	Build_RH_Box()
}

Build_RH_Box() {
	global
	Critical
	Local NPCNameTemp, FlagTemp, TickTemp, Limiter
	Ticktemp:= A_TickCount
	Limiter:= Ticktemp - NPC_Ticktick
	If (Limiter > 15) {
		NPC_Ticktick:= Ticktemp
		ScrollEnd:= viewport.document.body.scrollHeight - 500
		If (ScrollEnd < 0) {
			ScrollEnd:= 0
		}
		Name_Work()
		Parser()
		Graphical()
		Gui, NPCE_Main:Default
		WinTNPC:= NPCName
		SB_SetText(" NPC: " WinTNPC, 2)
		If Modname {
			qc:= npc.SetCapacity(0)
			if !qc
				qc:= 0
			SB_SetText(" " Modname " (" qc " items)", 1)
		}
		NPCNameTemp:= vNPCname.get()
		FlagTemp:= 0
		For a, b in npc.object()
		{
			if (NPC[a].name = NPCNameTemp)
				FlagTemp:= 1
		}
		If FlagTemp
			GuiControl, NPCE_Main:, ButtonOutputAppend, Update Project
		else
			GuiControl, NPCE_Main:, ButtonOutputAppend, Add to Project
	}
	Gui, NPCE_Main:Show
}	


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                 Main Program                 |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


Prelude() {
	global
	local skin
	NPC_Ticktick:= 0
	NPCTheme:= {}
	
	skin:= "Parchment"
	NPCTheme[skin]:= {}
	NPCTheme[skin].txStats:= "pasb.jpg"
	NPCTheme[skin].txCap:= "pacap.jpg"
	NPCTheme[skin].txBG:= "paper.jpg"
	NPCTheme[skin].capborder:= "#000000"
	NPCTheme[skin].text:= "#7A200D"
	NPCTheme[skin].mainborder:= "#BA4E3B"
	NPCTheme[skin].frame:= "#FDF1E6"
	NPCTheme[skin].whoosh:= "#922610"
	NPCTheme[skin].shadow:= "#472737"
	NPCTheme[skin].alpha:= "0.0"

	skin:= "Frost"
	NPCTheme[skin]:= {}
	NPCTheme[skin].txStats:= "frsb.jpg"
	NPCTheme[skin].txCap:= "frcap.jpg"
	NPCTheme[skin].txBG:= "frbg.jpg"
	NPCTheme[skin].capborder:= "#000000"
	NPCTheme[skin].text:= "#000065"
	NPCTheme[skin].mainborder:= "#000065"
	NPCTheme[skin].frame:= "#F0F9FF"
	NPCTheme[skin].whoosh:= "#000078"
	NPCTheme[skin].shadow:= "#000020"
	NPCTheme[skin].alpha:= "0.4"
	
	skin:= "Jungle"
	NPCTheme[skin]:= {}
	NPCTheme[skin].txStats:= "jusb.jpg"
	NPCTheme[skin].txCap:= "jucap.jpg"
	NPCTheme[skin].txBG:= "jubg.jpg"
	NPCTheme[skin].capborder:= "#000000"
	NPCTheme[skin].text:= "#005000"
	NPCTheme[skin].mainborder:= "#007000"
	NPCTheme[skin].frame:= "#D8FFF9"
	NPCTheme[skin].whoosh:= "#493A1C"
	NPCTheme[skin].shadow:= "#002000"
	NPCTheme[skin].alpha:= "0.65"
	
	skin:= "Blood"
	NPCTheme[skin]:= {}
	NPCTheme[skin].txStats:= "blsb.jpg"
	NPCTheme[skin].txCap:= "blcap.jpg"
	NPCTheme[skin].txBG:= "blbg.jpg"
	NPCTheme[skin].capborder:= "#000000"
	NPCTheme[skin].text:= "#500000"
	NPCTheme[skin].mainborder:= "#800000"
	NPCTheme[skin].frame:= "#D8FFF9"
	NPCTheme[skin].whoosh:= "#700000"
	NPCTheme[skin].shadow:= "#200000"
	NPCTheme[skin].alpha:= "0.65"
	
	skin:= "Flame"
	NPCTheme[skin]:= {}
	NPCTheme[skin].txStats:= "flsb.jpg"
	NPCTheme[skin].txCap:= "flcap.jpg"
	NPCTheme[skin].txBG:= "flbg.jpg"
	NPCTheme[skin].capborder:= "#552200"
	NPCTheme[skin].text:= "#CD0000"
	NPCTheme[skin].mainborder:= "#800000"
	NPCTheme[skin].frame:= "#FDF1E6"
	NPCTheme[skin].whoosh:= "#C25800"
	NPCTheme[skin].shadow:= "#202000"
	NPCTheme[skin].alpha:= "0.65"
	
	NPCskin:= "Parchment"
	
	
	NPCEnull:= ""
	NPCgender:= "Neutral"
	NPCunique:= "0"
	NPCpropername:= "0"
	bullets:= []
	NpcArtPref:= "Artwork by"
	FGcat:= Modname
	
	XParray:= {}
		XParray[0]:= "10"
		XParray["1/8"]:= "25"
		XParray["1/4"]:= "50"
		XParray["1/2"]:= "100"
		XParray[1]:= "200"
		XParray[2]:= "450"
		XParray[3]:= "700"
		XParray[4]:= "1,100"
		XParray[5]:= "1,800"
		XParray[6]:= "2,300"
		XParray[7]:= "2,900"
		XParray[8]:= "3,900"
		XParray[9]:= "5,000"
		XParray[10]:= "5,900"
		XParray[11]:= "7,200"
		XParray[12]:= "8,400"
		XParray[13]:= "10,000"
		XParray[14]:= "11,500"
		XParray[15]:= "13,000"
		XParray[16]:= "15,000"
		XParray[17]:= "18,000"
		XParray[18]:= "20,000"
		XParray[19]:= "22,000"
		XParray[20]:= "25,000"
		XParray[21]:= "33,000"
		XParray[22]:= "41,000"
		XParray[23]:= "50,000"
		XParray[24]:= "62,000"
		XParray[25]:= "75,000"
		XParray[26]:= "90,000"
		XParray[27]:= "105,000"
		XParray[28]:= "120,000"
		XParray[29]:= "135,000"
		XParray[30]:= "155,000"

	NPC_damres:= []
		NPC_damres[1]:= "acid"
		NPC_damres[2]:= "cold"
		NPC_damres[3]:= "fire"
		NPC_damres[4]:= "force"
		NPC_damres[5]:= "lightning"
		NPC_damres[6]:= "necrotic"
		NPC_damres[7]:= "poison"
		NPC_damres[8]:= "psychic"
		NPC_damres[9]:= "radiant"
		NPC_damres[10]:= "thunder"
		NPC_damres[11]:= "bludgeoning"
		NPC_damres[12]:= "piercing"
		NPC_damres[13]:= "slashing"
	NPC_damvul:= []
		NPC_damvul[1]:= "acid"
		NPC_damvul[2]:= "cold"
		NPC_damvul[3]:= "fire"
		NPC_damvul[4]:= "force"
		NPC_damvul[5]:= "lightning"
		NPC_damvul[6]:= "necrotic"
		NPC_damvul[7]:= "poison"
		NPC_damvul[8]:= "psychic"
		NPC_damvul[9]:= "radiant"
		NPC_damvul[10]:= "thunder"
		NPC_damvul[11]:= "bludgeoning"
		NPC_damvul[12]:= "piercing"
		NPC_damvul[13]:= "slashing"
	NPC_damimm:= []
		NPC_damimm[1]:= "acid"
		NPC_damimm[2]:= "cold"
		NPC_damimm[3]:= "fire"
		NPC_damimm[4]:= "force"
		NPC_damimm[5]:= "lightning"
		NPC_damimm[6]:= "necrotic"
		NPC_damimm[7]:= "poison"
		NPC_damimm[8]:= "psychic"
		NPC_damimm[9]:= "radiant"
		NPC_damimm[10]:= "thunder"
		NPC_damimm[11]:= "bludgeoning"
		NPC_damimm[12]:= "piercing"
		NPC_damimm[13]:= "slashing"
	NPC_conimm:= []
		NPC_conimm[1]:= "blinded"
		NPC_conimm[2]:= "charmed"
		NPC_conimm[3]:= "deafened"
		NPC_conimm[4]:= "exhaustion"
		NPC_conimm[5]:= "frightened"
		NPC_conimm[6]:= "grappled"
		NPC_conimm[7]:= "incapacitated"
		NPC_conimm[8]:= "invisible"
		NPC_conimm[9]:= "paralyzed"
		NPC_conimm[10]:= "petrified"
		NPC_conimm[11]:= "poisoned"
		NPC_conimm[12]:= "prone"
		NPC_conimm[13]:= "restrained"
		NPC_conimm[14]:= "stunned"
		NPC_conimm[15]:= "unconscious"
	Pronoun:= []
		Pronoun[1, "male"]:= "he"
		Pronoun[2, "male"]:= "him"
		Pronoun[3, "male"]:= "his"
		Pronoun[4, "male"]:= "himself"
		Pronoun[1, "female"]:= "she"
		Pronoun[2, "female"]:= "her"
		Pronoun[3, "female"]:= "her"
		Pronoun[4, "female"]:= "herself"
		Pronoun[1, "neutral"]:= "it"
		Pronoun[2, "neutral"]:= "it"
		Pronoun[3, "neutral"]:= "its"
		Pronoun[4, "neutral"]:= "itself"
		
		ScrollPoint:= 0
		
		pdid:= 0
}

Initialise() {
	global
	NPCLanguages:= []
	NPC_Actions:= []
	NPC_Reactions:= []
	NPC_Legendary_Actions:= []
	NPC_Lair_Actions:= []
	NPC_Traits:= []
	NPC_Spell_Level:= []
	NPC_Spell_Number:= []
	NPC_Spell_Slots:= []
	NPC_Spell_Names:= []
	NPC_InSpell_Number:= []
	NPC_InSpell_Slots:= []
	NPC_InSpell_Names:= []
	Sort:= []
	Sort.terrain:= ""
	Sort.lore:= ""
}

Declare_Vars() {
	global
	FGcat:= modname
	NPC_FS_STR:= "0"
	NPC_FS_DEX:= "0"
	NPC_FS_CON:= "0"
	NPC_FS_INT:= "0"
	NPC_FS_WIS:= "0"
	NPC_FS_CHA:= "0"
	NPCSpellStar:= ""
	NPCImArt:= ""
	NPCImLink:= ""
	NPCNoID:= ""
	WinTNPC:= "NPC: "
	OtherActionName:= ""
	OtherActionText:= ""
	multi_attack_Text:= ""
	Multi_attack:= 0
	Desc_Add_Text:= Defdesc1
	Desc_NPC_Title:= Defdesc2
	Desc_Image_Link:= Defdesc3
	Desc_Spell_List:= Defdesc4
	Desc_fixes:= "1"
	Desc_strip_lf:= "1"
	Desc_title:= "1"
	Raw_Desc_Text:= ""
	Fixed_Desc_Text:= ""
	NPCname:= ""
	NPCsize:= ""
	NPCtype:= ""
	NPCtag:= ""
	NPCalign:= ""
	NPCac:= ""
	NPChp:= ""
	NPCwalk:= "30"
	NPCburrow:= "0"
	NPCclimb:= "0"
	NPCfly:= "0"
	NPChover:= 0
	NPCswim:= "0"
	NPCstr:= "10"
	NPCdex:= "10"
	NPCcon:= "10"
	NPCint:= "10"
	NPCwis:= "10"
	NPCcha:= "10"
	NPCstrbo:= "0"
	NPCdexbo:= "0"
	NPCconbo:= "0"
	NPCintbo:= "0"
	NPCwisbo:= "0"
	NPCchabo:= "0"
	NPCstrsav:= ""
	NPCdexsav:= ""
	NPCconsav:= ""
	NPCintsav:= ""
	NPCwissav:= ""
	NPCchasav:= ""
	NPCblind:= ""
	NPCdark:= ""
	NPCtremor:= ""
	NPCtrue:= ""
	NPCpassperc:= 10
	NPCblindB:= 0
	NPCdarkB:= 0
	NPCtremorB:= 0
	NPCtrueB:= 0
	NPCcharat:= ""
	NPCxp:= ""
	NPCdescript:= ""

	NPCchall:= ""
	NPCinspell:= ""
	NPCspell:= ""
	
	NPCinspability:= ""
	NPCinspsave:= "0"
	NPCinsptohit:= "0"
	InSp_0_spells:= ""
	InSp_1_spells:= ""
	InSp_2_spells:= ""
	InSp_3_spells:= ""
	InSp_4_spells:= ""
	InSp_5_spells:= ""
	NPCinsptext:= "requiring no material components"

	NPCsplevel:= ""
	NPCspability:= ""
	NPCspsave:= "0"
	NPCsptohit:= "0"
	NPCspclass:= ""
	NPCspflavour:= ""
	Sp_0_casts:= "At will"
	Sp_1_casts:= ""
	Sp_2_casts:= ""
	Sp_3_casts:= ""
	Sp_4_casts:= ""
	Sp_5_casts:= ""
	Sp_6_casts:= ""
	Sp_7_casts:= ""
	Sp_8_casts:= ""
	Sp_9_casts:= ""
	Sp_0_spells:= ""
	Sp_1_spells:= ""
	Sp_2_spells:= ""
	Sp_3_spells:= ""
	Sp_4_spells:= ""
	Sp_5_spells:= ""
	Sp_6_spells:= ""
	Sp_7_spells:= ""
	Sp_8_spells:= ""
	Sp_9_spells:= ""
	
	loopvar:= 1
	While (loopvar < 17) {
		cbDV%loopvar%:= 0
		cbDR%loopvar%:= 0
		cbDI%loopvar%:= 0
		cbCI%loopvar%:= 0
		loopvar += 1
	}

	VCI16:= ""
	
	DRRadio1:= 1	
	DRRadio2:= 0	
	DRRadio3:= 0	
	DRRadio4:= 0	
	DRRadio5:= 0	
	DRRadio6:= 0	
	
	DIRadio1:= 1	
	DIRadio2:= 0	
	DIRadio3:= 0	
	DIRadio4:= 0	
	DIRadio5:= 0	
	
	Radio1:= 1	
	Radio2:= 0	
	Radio3:= 0	
	Radio4:= 0	
	Radio5:= 0	
	Radio6:= 0	
	Radio7:= 0	
	Radio8:= 0	
	
	NPCActStart:= ""
	NPCactions:= ""
	NPCreactions:= ""
	NPClegactions:= ""
	NPClairactions:= ""
	NPCtraits:= ""
	NPCHolding:= ""
	NPCjpeg:= ""
	NPCspeed:= ""
	NPCsave:= ""
	NPCskill:= ""
	NPCdamvul:= ""
	NPCdamres:= ""
	NPCdamimm:= ""
	NPCconimm:= ""
	NPCsense:= ""
	NPClang:= ""
	NPCtelep:= 0
	telrange:= ""
	langextra:= ""
	langalt:= ""
	
	traitnew:= ""
	traitnamenew:= ""

	Work:= ""
	SpellWork:=""
	InSpellWork:=""
	TextStatBlock:= ""
	TextSkillsBlock:= ""
	Source:= ""
	NameCheck:= ""
	Match:= ""
	SizeString:= ""
	FoundPos:= ""
	StatStart:= ""
	InSpellStart:= ""
	SpellStart:= ""
	Save_File:= ""

	FlagActions:= ""
	FlagReactions:= ""
	FlagLegActions:= ""
	FlagLairActions:= ""
	FlagTraits:= ""
	FlagLang:= ""
	Flagdamvul:= ""
	Flagdamres:= ""
	Flagdamimm:= ""
	Flagconimm:= ""
	FlagInSpell:= 0
	FlagSpell:= 0
	NPCPsionics:= 0
	
	GP1:= "it"
	GP2:= "it"
	GP3:= "its"
	GP4:= "itself"

	NPC_Skills:= {}
		NPC_Skills["Acrobatics"]:= 0
		NPC_Skills["Animal Handling"]:= 0
		NPC_Skills["Arcana"]:= 0
		NPC_Skills["Athletics"]:= 0
		NPC_Skills["Deception"]:= 0
		NPC_Skills["History"]:= 0
		NPC_Skills["Insight"]:= 0
		NPC_Skills["Intimidation"]:= 0
		NPC_Skills["Investigation"]:= 0
		NPC_Skills["Medicine"]:= 0
		NPC_Skills["Nature"]:= 0
		NPC_Skills["Perception"]:= 0
		NPC_Skills["Performance"]:= 0
		NPC_Skills["Persuasion"]:= 0
		NPC_Skills["Religion"]:= 0
		NPC_Skills["Sleight of Hand"]:= 0
		NPC_Skills["Stealth"]:= 0
		NPC_Skills["Survival"]:= 0

	ButtonTerrain:= ""
	ButtonImport:= ""
	ButtonAddTrait:= ""
	ButtonDesctextDelete:= ""
	NPCE_Paste:= ""
	ButtonDesctextBold:= ""
	ButtonDesctextItalic:= ""
	ButtonDesctextEmphasise:= ""
	ButtonDesctextCreateList:= ""
	ButtonToClipboard:= ""
	ButtonToText:= ""
	ButtonOutputAppend:= ""
	ButtonDesctextModify:= ""
	ButtonAddAction:= ""
	ButtonAddReAction:= ""
	ButtonAddLgAction:= ""
	ButtonAddLrAction:= ""
	ButtonAverageHP:= ""
	ButtonRollHP:= ""
	NPCE_Descrip_Appen:= ""
	ButtonSBClipboard:= ""

	vChosen_Desc_Text:= ""
	NPCtoken:= ""
	NPCtokenpath:= ""
	NPCimage:= ""
	NPCimagepath:= ""
	
	Chosen_Desc_Text:= ""
	D_Cap_text:= ""
	D_Fix_Text:= ""


	loop 12 {
		Terr%A_Index%:= 0
	}
	loop 9 {
		Orig%A_Index%:= 0
	}
}

Main_Loop() {
	global
;~ ######################################################
;~ #                    Initial work.                   #
;~ ######################################################

; Adjust for selected import type
	If (ImportChoice == "D&D Beyond") {
		Import_DDBeyond(WorkingString)
	} 
	If (ImportChoice == "CritterDB") {
		Import_CritterDB(WorkingString)
	}
	If (ImportChoice == "D&D Wiki") {
		Import_DDWiki(WorkingString)
	}
	If (ImportChoice == "Donjon") {
		Import_Donjon(WorkingString)
	}
	If (ImportChoice == "RPG Tinker") {
		MsgBox, 48, Import Information, RPG Tinker NPCs do not have a 'size' or 'type' set.`n`nNPC Engineer assigns 'medium' and 'humanoid' to these fields. Be sure to correct them if needed after the import., 4
		Import_RPGTinker(WorkingString)
	}
	If (ImportChoice == "Incarnate") {
		Import_Incarnate(WorkingString)
	}
	If (ImportChoice == "Roll20: 5E Shaped") {
		Import_Roll20(WorkingString)
	}
	If (ImportChoice == "Roll20 Compendium") {
		Import_Roll20Comp(WorkingString)
	}
	If (ImportChoice == "MS Word Table 1") {
		Import_Word_1(WorkingString)
	}
	If (ImportChoice == "PDF Alternative 1") {
		Import_PDF_1(WorkingString)
	}
	If (ImportChoice == "PDF Alternative 2") {
		Import_PDF_2(WorkingString)
	}
	If (ImportChoice == "HeroLabs") {
		MsgBox, 48, Import Information, Due to inconsistencies in the output from HeroLabs, the 'Lair Actions' section is likely to need work to format them correctly after the import stage., 4
		Import_HeroLabs(WorkingString)
	}
	If (ImportChoice == "Fantasy Grounds XML") {
		Import_FGXML(WorkingString)
	}

; Replace some common errors in the copied file.
	Common_Problems(WorkingString)
	Colon(WorkingString)
	Tag_Actions()
	Double_Spacing()
	Empty_Variables()
	
; Set Attribute block in the correct format and remove from work space
	Set_Attributes()

; Split text file into a stat block and a further information block
	StringReplace, Work, Work, `r`n<SIXX>, 
	BlockPos1 := RegExMatch(Work, "P)Challenge.*XP\)", BlockLength1, 1)
	TextStatBlock := SubStr(Work,1,BlockPos1 + Blocklength1)
	TextSkillsBlock := SubStr(Work,BlockPos1 + Blocklength1+1)
	BlockPos2 := RegExMatch(TextSkillsBlock, "P)##;", BlockLength2, 1)
	if BlockPos2 {
		TextSkillsBlock := SubStr(TextSkillsBlock,1,BlockPos2-1)
	} else {
		TextSkillsBlock := SubStr(TextSkillsBlock,1)
	}
	TextSkillsBlock:= RegExReplace(TextSkillsBlock,"\s*$","") ; remove trailing newlines

;~ ######################################################
;~ #                  Main Stat Block.                  #
;~ ######################################################

	Work := TextStatBlock
	Stat_Block()

;~ ######################################################
;~ #                Traits, Actions etc.                #
;~ ######################################################
		
; Actions etc.
	Work:= TextSkillsBlock
	Actions_Prep()

; Deal with spellcasting blocks if they exist
	Actions_InnSpellCast()
	Actions_SpellCast()
	
	Format_Chunk(work)
	Get_Actions()
	Get_Reactions()
	Get_LegendaryActions()
	Get_LairActions()
	Get_Traits()
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |          Formatting Input Functions          |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


Colon(ByRef fixme) {
	global
	StringReplace, fixme, fixme, components:, components~##~, All 
	StringReplace, fixme, fixme, component:, component~##~, All 
	StringReplace, fixme, fixme, spells:, spells~##~, All 
	StringReplace, fixme, fixme, Weapon Attack:`r`n, Weapon Attack~##~ `r`n, All 
	StringReplace, fixme, fixme, Weapon Attack:, Weapon Attack~##~, All 
	StringReplace, fixme, fixme, Spell Attack:, Spell Attack~##~, All 
	StringReplace, fixme, fixme, will:, will~##~, All 
	StringReplace, fixme, fixme, each:, each~##~, All 
	StringReplace, fixme, fixme, slots:, slots~##~, All 
	StringReplace, fixme, fixme, slot:, slot~##~, All 
	StringReplace, fixme, fixme, prepared:, prepared~##~, All 
	StringReplace, fixme, fixme, will):, will)~##~, All 
	StringReplace, fixme, fixme, each):, each)~##~, All 
	StringReplace, fixme, fixme, slots):, slots)~##~, All 
	StringReplace, fixme, fixme, slot):, slot)~##~, All 
	StringReplace, fixme, fixme, Hit:, Hit~##~, All
	StringReplace, fixme, fixme, /Day:, /day~##~, All
	StringReplace, fixme, fixme, /day:, /day~##~, All
	
	StringReplace, fixme, fixme, :, ., All 

	StringReplace, fixme, fixme, ~##~, :, All 
}

Common_Problems(Byref fixme) {
	global
	StringReplace, fixme, fixme, Armor Class:, Armor Class,
	StringReplace, fixme, fixme, Armour Class, Armor Class,
	StringReplace, fixme, fixme, armour class, Armor Class,
	StringReplace, fixme, fixme, Armour class, Armor Class,
	StringReplace, fixme, fixme, Hit Points:, Hit Points,
	StringReplace, fixme, fixme, Speed:, Speed,
	StringReplace, fixme, fixme, Saving Throws:, Saving Throws,
	StringReplace, fixme, fixme, Skills:, Skills,
	StringReplace, fixme, fixme, Senses:, Senses,
	StringReplace, fixme, fixme, Languages:, Languages,
	StringReplace, fixme, fixme, Challenge:, Challenge,
	StringReplace, fixme, fixme, Vulnerabilities:, Vulnerabilities,
	StringReplace, fixme, fixme, Resistances:, Resistances,
	StringReplace, fixme, fixme, Immunities:, Immunities,

	StringReplace, fixme, fixme, o%A_SPACE%f%A_SPACE%, of, All
	StringReplace, fixme, fixme, w%A_SPACE%ere, were, All 
	StringReplace, fixme, fixme, �, `", All
	StringReplace, fixme, fixme, �, `", All
	StringReplace, fixme, fixme, �, `', All
	StringReplace, fixme, fixme, %liga_sq1%, `', All
	StringReplace, fixme, fixme, %liga_sq2%, `', All
	StringReplace, fixme, fixme, %liga_sq3%, `', All
	StringReplace, fixme, fixme, %liga_sq4%, `', All
	StringReplace, fixme, fixme, %liga_dq1%, `", All
	StringReplace, fixme, fixme, %liga_dq2%, `", All
	StringReplace, fixme, fixme, %liga_dq3%, `", All
	StringReplace, fixme, fixme, %liga_dq4%, `", All

	StringReplace, fixme, fixme, %A_SPACE%xp, %A_SPACE%XP, All
	StringReplace, fixme, fixme, 0XP, 0%A_SPACE%XP, All
	StringReplace, fixme, fixme, `{, (, All
	StringReplace, fixme, fixme, `}, ), All
	StringReplace, fixme, fixme, Melee Weapon Attack.%A_SPACE%, Melee Weapon Attack:%A_SPACE%, All
	StringReplace, fixme, fixme, Melee Weapon Attack:`r`n, Melee Weapon Attack:%A_SPACE%, All
	StringReplace, fixme, fixme, %liga_ff%, ff, All
	StringReplace, fixme, fixme, %liga_fi%, ff, All
	StringReplace, fixme, fixme, %liga_fl%, fl, All
	StringReplace, fixme, fixme, %liga_hy%, -, All
	StringReplace, fixme, fixme, fre%A_SPACE%, fire%A_SPACE%, All
	StringReplace, fixme, fixme, f%A_SPACE%re%A_SPACE%, fire%A_SPACE%, All
	StringReplace, fixme, fixme, %A_SPACE%fi%A_SPACE%re, %A_SPACE%fire, All
	StringReplace, fixme, fixme, Dam%A_SPACE%age%A_SPACE%, Damage%A_SPACE%, All
	StringReplace, fixme, fixme, ft. `,, ft.`,, All
	StringReplace, fixme, fixme, ft ., ft., All
	StringReplace, fixme, fixme, ~, r, All
	StringReplace, fixme, fixme, JDay, /Day, All
	StringReplace, fixme, fixme, fDay, /Day, All
	StringReplace, fixme, fixme, L/Day, 1/Day, All
	StringReplace, fixme, fixme, L/day, 1/day, All
	StringReplace, fixme, fixme, l/Day, l/Day, All
	StringReplace, fixme, fixme, l/day, l/day, All
	StringReplace, fixme, fixme, fitnd, fiend, All
	StringReplace, fixme, fixme, choolic, chaotic, All
	StringReplace, fixme, fixme, Armo.r, Armor, All
	StringReplace, fixme, fixme, %A_SPACE%`,, `,, All
	StringReplace, fixme, fixme, dl0, d10, All
	StringReplace, fixme, fixme, dlO, d10, All
	StringReplace, fixme, fixme, ld1, 1d1, All
	StringReplace, fixme, fixme, ld4, 1d4, All
	StringReplace, fixme, fixme, ld6, 1d6, All
	StringReplace, fixme, fixme, ld8, 1d8, All
	StringReplace, fixme, fixme, non magical, nonmagical, All
	StringReplace, fixme, fixme, (at wi ll), (At will), 
	StringReplace, fixme, fixme, (At wi ll), (At will), 
	StringReplace, fixme, fixme, (at will), (At will), 
	StringReplace, fixme, fixme, inn ate, innate, All
	StringReplace, fixme, fixme, speilcasting, spellcasting, All
	StringReplace, fixme, fixme, %A_SPACE%lst, %A_SPACE%1st, All
	StringReplace, fixme, fixme, Whi1st, Whilst, All
	StringReplace, fixme, fixme, whi1st, whilst, All
	StringReplace, fixme, fixme, ) :, ):, All
	StringReplace, fixme, fixme, %A_SPACE%Te%A_SPACE%, %A_SPACE%The%A_SPACE%, All
	StringReplace, fixme, fixme, %A_SPACE%te%A_SPACE%, %A_SPACE%the%A_SPACE%, All
	StringReplace, fixme, fixme, %A_SPACE%Tey%A_SPACE%, %A_SPACE%They%A_SPACE%, All
	StringReplace, fixme, fixme, %A_SPACE%tey%A_SPACE%, %A_SPACE%they%A_SPACE%, All
	StringReplace, fixme, fixme, fght, fight, All
	StringReplace, fixme, fixme, %A_SPACE%ca%A_SPACE%n%A_SPACE%, %A_SPACE%can%A_SPACE%, All
	StringReplace, fixme, fixme, %A_SPACE%th%A_SPACE%e%A_SPACE%, %A_SPACE%the%A_SPACE%, All
	StringReplace, fixme, fixme, %A_SPACE%savin%A_SPACE%g%A_SPACE%, %A_SPACE%saving%A_SPACE%, All
	StringReplace, fixme, fixme, �%A_SPACE%, , All
	StringReplace, fixme, fixme, �, , All
	StringReplace, fixme, fixme, `. `r`n, `.`r`n, All
	StringReplace, fixme, fixme, `. `r`n, `.`r`n, All
	StringReplace, fixme, fixme, Actions `r`n, Actions`r`n, All
	StringReplace, fixme, fixme, ACTIONS `r`n, ACTIONS`r`n, All
	
	fixme:= RegExReplace(fixme, "U)(\d)\(", "$1 (")
	fixme:= RegExReplace(fixme, "Challenge(.*)\((\d+)\)", "Challenge$1($2 XP)")

}

Tag_Actions() {
	global
	StringCaseSense, On
	StringReplace, WorkingString, WorkingString, **--Actions--**, <ACT>ACTIONS<ION>
	StringReplace, WorkingString, WorkingString, **--Reactions--**, <ACT>REACTIONS<ION>
	StringReplace, WorkingString, WorkingString, **--Legendary Action--**, <ACT>LEGENDARY ACTIONS<ION>
	StringReplace, WorkingString, WorkingString, Lair Actions`r`n, <ACT>LAIR ACTIONS<ION>`r`n
	StringReplace, WorkingString, WorkingString, Legendary Actions`r`n, <ACT>LEGENDARY ACTIONS<ION>`r`n
	StringReplace, WorkingString, WorkingString, Reactions`r`n, <ACT>REACTIONS<ION>`r`n
	StringReplace, WorkingString, WorkingString, Actions`r`n, <ACT>ACTIONS<ION>`r`n
	StringReplace, WorkingString, WorkingString, LAIR ACTIONS`r`n, <ACT>LAIR ACTIONS<ION>`r`n
	StringReplace, WorkingString, WorkingString, LEGENDARY ACTIONS`r`n, <ACT>LEGENDARY ACTIONS<ION>`r`n
	StringReplace, WorkingString, WorkingString, REACTIONS`r`n, <ACT>REACTIONS<ION>`r`n
	StringReplace, WorkingString, WorkingString, ACTIONS`r`n, <ACT>ACTIONS<ION>`r`n
	StringCaseSense, Off
	StringReplace, WorkingString, WorkingString, \r, `r`n, all
}

Double_Spacing() {
	Global
	IfInString WorkingString, `r`n`r`nArmor Class
		StringReplace WorkingString, WorkingString, `r`n`r`n, `r`n, All
}

Set_Attributes() {
	global
	StringReplace, WorkingString, WorkingString, STR DEX CON INT WIS CHA`r`n, STATBLOCKA
	StringReplace, WorkingString, WorkingString, STR | DEX | CON | INT | WIS | CHA`r`n, STATBLOCKB
	StringReplace, WorkingString, WorkingString, STR`tDEX`tCON`tINT`tWIS`tCHA`r`n, STATBLOCKC
	StringReplace, WorkingString, WorkingString, STR DEX CON INT WIS CHA%A_SPACE%, STATBLOCKD
	StringReplace, WorkingString, WorkingString, STR`r`nDEX`r`nCON`r`nINT`r`nWIS`r`nCHA`r`n, STATBLOCKF
	WorkingString:= regexreplace(WorkingString, "m)^STR\r\n", "STRGGG", , 1)
	WorkingString:= regexreplace(WorkingString, "m)^DEX\r\n", "DEXGGG", , 1)
	WorkingString:= regexreplace(WorkingString, "m)^CON\r\n", "CONGGG", , 1)
	WorkingString:= regexreplace(WorkingString, "m)^WIS\r\n", "WISGGG", , 1)
	WorkingString:= regexreplace(WorkingString, "m)^INT\r\n", "INTGGG", , 1)
	WorkingString:= regexreplace(WorkingString, "m)^CHA\r\n", "CHAGGG", , 1)
	StringReplace, WorkingString, WorkingString, ___`r`n, , All
	StringReplace, WorkingString, WorkingString, ___%A_SPACE%`r`n, , All
	StringReplace, WorkingString, WorkingString, ___%A_SPACE%%A_SPACE%`r`n, , All
	StringReplace, WorkingString, WorkingString, ___%A_SPACE%%A_SPACE%%A_SPACE%`r`n, , All
	StringReplace, WorkingString, WorkingString, :-:|:-:|:-:|:-:|:-:|:-:|`r`n, , All
	StringReplace, WorkingString, WorkingString, |, %A_SPACE%, All

	Work := WorkingString Chr(13) Chr(10) "<SIXX>"
	Source := ""
;	STATS Method 1
		STATBLOCKE := ""
		IfInString Work, STRGGG
		{
			STATBLOCKE := STATBLOCKE "STR"
		}
		IfInString Work, DEXGGG
		{
			STATBLOCKE := STATBLOCKE "DEX"
		}
		IfInString Work, CONGGG
		{
			STATBLOCKE := STATBLOCKE "CON"
		}
		IfInString Work, INTGGG
		{
			STATBLOCKE := STATBLOCKE "INT"
		}
		IfInString Work, WISGGG
		{
			STATBLOCKE := STATBLOCKE "WIS"
		}
		IfInString Work, CHAGGG
		{
			STATBLOCKE := STATBLOCKE "CHA"
		}
		If STATBLOCKE {
			Source := "SB1"
;		NPC Strength
			IfInString Work, STRGGG
			{
				StatStart := RegExMatch(Work, "P)STRGGG.*\)\r\n", StatLength)+6
				NPCstr:= SubStr(Work,StatStart,StatLength-8)
				Chop:= SubStr(Work,StatStart-6,StatLength)
				StringReplace, Work, Work, %Chop%, 
				NPCstr:= SubStr(NPCstr,1,InStr(NPCstr," ")-1)
				NPCstr = %NPCstr%
			}
			else
			{
				NPCstr := "10"
				InputBox, NPCstr, Enter Strength, Enter your NPC's Strength., , 300, 125, , , , , %NPCstr%
			}
			NPCstrbo:= StatBon(NPCstr)

;		NPC Dexterity
			IfInString Work, DEXGGG
			{
				StatStart := RegExMatch(Work, "P)DEXGGG.*\)\r\n", StatLength)+6
				NPCdex:= SubStr(Work,StatStart,StatLength-8)
				Chop:= SubStr(Work,StatStart-6,StatLength)
				StringReplace, Work, Work, %Chop%, 
				NPCdex:= SubStr(NPCdex,1,InStr(NPCdex," ")-1)
				NPCdex = %NPCdex%
			}
			else
			{
				NPCdex := "10"
				InputBox, NPCdex, Enter Dexterity, Enter your NPC's Dexterity., , 300, 125, , , , , %NPCdex%
			}
			NPCdexbo:= StatBon(NPCdex)

;		NPC Constitution
			IfInString Work, CONGGG
			{
				StatStart := RegExMatch(Work, "P)CONGGG.*\)\r\n", StatLength)+6
				NPCcon:= SubStr(Work,StatStart,StatLength-8)
				Chop:= SubStr(Work,StatStart-6,StatLength)
				StringReplace, Work, Work, %Chop%, 
				NPCcon:= SubStr(NPCcon,1,InStr(NPCcon," ")-1)
				NPCcon = %NPCcon%
			}
			else
			{
				NPCcon := "10"
				InputBox, NPCcon, Enter Constitution, Enter your NPC's Constitution., , 300, 125, , , , , %NPCcon%
			}
			NPCconbo:= StatBon(NPCcon)

;		NPC Intelligence
			IfInString Work, INTGGG
			{
				StatStart := RegExMatch(Work, "P)INTGGG.*\)\r\n", StatLength)+6
				NPCint:= SubStr(Work,StatStart,StatLength-8)
				Chop:= SubStr(Work,StatStart-6,StatLength)
				StringReplace, Work, Work, %Chop%, 
				NPCint:= SubStr(NPCint,1,InStr(NPCint," ")-1)
				NPCint = %NPCint%
			}
			else
			{
				NPCint := "10"
				InputBox, NPCint, Enter Intelligence, Enter your NPC's Intelligence., , 300, 125, , , , , %NPCint%
			}
			NPCintbo:= StatBon(NPCint)

;		NPC Wisdom
			IfInString Work, WISGGG
			{
				StatStart := RegExMatch(Work, "P)WISGGG.*\)\r\n", StatLength)+6
				NPCwis:= SubStr(Work,StatStart,StatLength-8)
				Chop:= SubStr(Work,StatStart-6,StatLength)
				StringReplace, Work, Work, %Chop%, 
				NPCwis:= SubStr(NPCwis,1,InStr(NPCwis," ")-1)
				NPCwis = %NPCwis%
			}
			else
			{
				NPCwis := "10"
				InputBox, NPCwis, Enter Wisdom, Enter your NPC's Wisdom., , 300, 125, , , , , %NPCwis%
			}
			NPCwisbo:= StatBon(NPCwis)

;		NPC Charisma
			IfInString Work, CHAGGG
			{
				StatStart := RegExMatch(Work, "P)CHAGGG.*\)\r\n", StatLength)+6
				NPCcha:= SubStr(Work,StatStart,StatLength-8)
				Chop:= SubStr(Work,StatStart-6,StatLength)
				StringReplace, Work, Work, %Chop%, 
				NPCcha:= SubStr(NPCcha,1,InStr(NPCcha," ")-1)
				NPCcha = %NPCcha%
			}
			else
			{
				NPCcha := "10"
				InputBox, NPCcha, Enter Charisma, Enter your NPC's Charisma., , 300, 125, , , , , %NPCcha%
			}
			NPCchabo:= StatBon(NPCcha)
		}

;	STATS Method 2
		IfInString Work, STATBLOCKA
		{
			Source := "SB2"
			StatStart := RegExMatch(Work, "P)STATBLOCKA.*\)\r\n", StatLength)+10
			Stats:= SubStr(Work,StatStart,StatLength-12)
			Chop:= SubStr(Work,StatStart-10,StatLength)
			StringReplace, Work, Work, %Chop%, 

;		NPC Strength
			NPCstr:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2)
			NPCstr:= SubStr(NPCstr,1,InStr(NPCstr," ")-1)
			NPCstr = %NPCstr%
			NPCstrbo:= StatBon(NPCstr)
			
;		NPC Dexterity
			NPCdex:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCdex:= SubStr(NPCdex,1,InStr(NPCdex," ")-1)
			NPCdex = %NPCdex%
			NPCdexbo:= StatBon(NPCdex)

;		NPC Constitution
			NPCcon:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCcon:= SubStr(NPCcon,1,InStr(NPCcon," ")-1)
			NPCcon = %NPCcon%
			NPCconbo:= StatBon(NPCcon)

;		NPC Intelligence
			NPCint:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCint:= SubStr(NPCint,1,InStr(NPCint," ")-1)
			NPCint = %NPCint%
			NPCintbo:= StatBon(NPCint)

;		NPC Wisdom
			NPCwis:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCwis:= SubStr(NPCwis,1,InStr(NPCwis," ")-1)
			NPCwis = %NPCwis%
			NPCwisbo:= StatBon(NPCwis)

;		NPC Charisma
			NPCcha:= SubStr(Stats, 1,InStr(Stats,")"))
			NPCcha:= SubStr(NPCcha,1,InStr(NPCcha," ")-1)
			NPCcha = %NPCcha%
			NPCchabo:= StatBon(NPCcha)
		}

;	STATS Method 3
		IfInString Work, STATBLOCKB
		{
			Source := "SB3"
			StringReplace Work, Work, **Challenge**, Challenge
			Work:= RegExReplace(Work, " {2,}\r\n",Chr(13)Chr(10))
			StatStart := RegExMatch(Work, "P)STATBLOCKB.*\r\n", StatLength)+10
			Stats:= SubStr(Work,StatStart,StatLength-12)
			Chop:= SubStr(Work,StatStart-10,StatLength)
			StringReplace, Work, Work, %Chop%, 
;		NPC Strength
			NPCstr:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2)
			NPCstr:= SubStr(NPCstr,1,InStr(NPCstr," ")-1)
			NPCstr = %NPCstr%
			NPCstrbo:= StatBon(NPCstr)
			
;		NPC Dexterity
			NPCdex:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCdex:= SubStr(NPCdex,1,InStr(NPCdex," ")-1)
			NPCdex = %NPCdex%
			NPCdexbo:= StatBon(NPCdex)

;		NPC Constitution
			NPCcon:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCcon:= SubStr(NPCcon,1,InStr(NPCcon," ")-1)
			NPCcon = %NPCcon%
			NPCconbo:= StatBon(NPCcon)

;		NPC Intelligence
			NPCint:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCint:= SubStr(NPCint,1,InStr(NPCint," ")-1)
			NPCint = %NPCint%
			NPCintbo:= StatBon(NPCint)

;		NPC Wisdom
			NPCwis:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCwis:= SubStr(NPCwis,1,InStr(NPCwis," ")-1)
			NPCwis = %NPCwis%
			NPCwisbo:= StatBon(NPCwis)

;		NPC Charisma
			NPCcha:= SubStr(Stats, 1,InStr(Stats,")"))
			NPCcha:= SubStr(NPCcha,1,InStr(NPCcha," ")-1)
			NPCcha = %NPCcha%
			NPCchabo:= StatBon(NPCcha)
		}

;	STATS Method 4
		IfInString Work, STATBLOCKC
		{
			Source := "SB4"
			StringReplace, Work, Work, %A_TAB%, %A_SPACE%, All
			StatStart := RegExMatch(Work, "P)STATBLOCKC.*\r\n", StatLength)+10
			Stats:= SubStr(Work,StatStart,StatLength-12)
			Chop:= SubStr(Work,StatStart-10,StatLength)
			StringReplace, Work, Work, %Chop%, 

;		NPC Strength
			NPCstr:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2)
			NPCstr:= SubStr(NPCstr,1,InStr(NPCstr," ")-1)
			NPCstr = %NPCstr%
			NPCstrbo:= StatBon(NPCstr)
			
;		NPC Dexterity
			NPCdex:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCdex:= SubStr(NPCdex,1,InStr(NPCdex," ")-1)
			NPCdex = %NPCdex%
			NPCdexbo:= StatBon(NPCdex)

;		NPC Constitution
			NPCcon:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCcon:= SubStr(NPCcon,1,InStr(NPCcon," ")-1)
			NPCcon = %NPCcon%
			NPCconbo:= StatBon(NPCcon)

;		NPC Intelligence
			NPCint:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCint:= SubStr(NPCint,1,InStr(NPCint," ")-1)
			NPCint = %NPCint%
			NPCintbo:= StatBon(NPCint)

;		NPC Wisdom
			NPCwis:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCwis:= SubStr(NPCwis,1,InStr(NPCwis," ")-1)
			NPCwis = %NPCwis%
			NPCwisbo:= StatBon(NPCwis)

;		NPC Charisma
			NPCcha:= SubStr(Stats, 1,InStr(Stats,")"))
			NPCcha:= SubStr(NPCcha,1,InStr(NPCcha," ")-1)
			NPCcha = %NPCcha%
			NPCchabo:= StatBon(NPCcha)
		}
		
;	STATS Method 5
		IfInString Work, STATBLOCKD
		{
			Source := "SB5"
			StatStart := RegExMatch(Work, "P)STATBLOCKD.*\r\n", StatLength)+10
			Stats:= SubStr(Work,StatStart,StatLength-12)
			Chop:= SubStr(Work,StatStart-10,StatLength)
			StringReplace, Work, Work, %Chop%, 

;		NPC Strength
			NPCstr:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2)
			NPCstr:= SubStr(NPCstr,1,InStr(NPCstr," ")-1)
			NPCstr = %NPCstr%
			NPCstrbo:= StatBon(NPCstr)
			
;		NPC Dexterity
			NPCdex:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCdex:= SubStr(NPCdex,1,InStr(NPCdex," ")-1)
			NPCdex = %NPCdex%
			NPCdexbo:= StatBon(NPCdex)

;		NPC Constitution
			NPCcon:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCcon:= SubStr(NPCcon,1,InStr(NPCcon," ")-1)
			NPCcon = %NPCcon%
			NPCconbo:= StatBon(NPCcon)

;		NPC Intelligence
			NPCint:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCint:= SubStr(NPCint,1,InStr(NPCint," ")-1)
			NPCint = %NPCint%
			NPCintbo:= StatBon(NPCint)

;		NPC Wisdom
			NPCwis:= SubStr(Stats, 1,InStr(Stats,")"))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ", True, 1, 2) 
			NPCwis:= SubStr(NPCwis,1,InStr(NPCwis," ")-1)
			NPCwis = %NPCwis%
			NPCwisbo:= StatBon(NPCwis)

;		NPC Charisma
			NPCcha:= SubStr(Stats, 1,InStr(Stats,")"))
			NPCcha:= SubStr(NPCcha,1,InStr(NPCcha," ")-1)
			NPCcha = %NPCcha%
			NPCchabo:= StatBon(NPCcha)
		}

;	STATS Method 6
		IfInString Work, STATBLOCKF
		{
			Source := "SB6"
			StatStart := RegExMatch(Work, "P)STATBLOCKF", StatLength)+10
			work := RegExReplace(work, "U)(\d+) .*\r\n", "$1 ", ,5, StatStart)
			StatStart := RegExMatch(Work, "P)STATBLOCKF.*\)\r\n", StatLength)+10
			Stats:= SubStr(Work,StatStart,StatLength-12)
			Chop:= SubStr(Work,StatStart-10,StatLength)
			StringReplace, Work, Work, %Chop%, 
;		NPC Strength
			NPCstr:= SubStr(Stats, 1,InStr(Stats," "))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ")
			;~ NPCstr:= SubStr(NPCstr,1,InStr(NPCstr," ")-1)
			NPCstr = %NPCstr%
			NPCstrbo:= StatBon(NPCstr)
			
;		NPC Dexterity
			NPCdex:= SubStr(Stats, 1,InStr(Stats," "))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ") 
			;~ NPCdex:= SubStr(NPCdex,1,InStr(NPCdex," ")-1)
			NPCdex = %NPCdex%
			NPCdexbo:= StatBon(NPCdex)

;		NPC Constitution
			NPCcon:= SubStr(Stats, 1,InStr(Stats," "))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ") 
			;~ NPCcon:= SubStr(NPCcon,1,InStr(NPCcon," ")-1)
			NPCcon = %NPCcon%
			NPCconbo:= StatBon(NPCcon)

;		NPC Intelligence
			NPCint:= SubStr(Stats, 1,InStr(Stats," "))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ") 
			;~ NPCint:= SubStr(NPCint,1,InStr(NPCint," ")-1)
			NPCint = %NPCint%
			NPCintbo:= StatBon(NPCint)

;		NPC Wisdom
			NPCwis:= SubStr(Stats, 1,InStr(Stats," "))
			StringTrimLeft, Stats, Stats, InStr(Stats, " ") 
			;~ NPCwis:= SubStr(NPCwis,1,InStr(NPCwis," ")-1)
			NPCwis = %NPCwis%
			NPCwisbo:= StatBon(NPCwis)

;		NPC Charisma
			NPCcha:= SubStr(Stats, 1,InStr(Stats," "))
			NPCcha:= SubStr(NPCcha,1,InStr(NPCcha," "))
			NPCcha = %NPCcha%
			NPCchabo:= StatBon(NPCcha)
		}

If (Source = "")
	{
		MsgBox No usable statblock found. Try another input filter.
		return
	}
}

Stat_Block() {
	global
	local Foundlang
	
;Tagging certain strings to ensure a unique find.
	StringReplace, Work, Work, Saving Throw%A_SPACE%, Saving Throws%A_SPACE%
	StringReplace, Work, Work, Saving Throws, Saving Throws<SAVETH>
	StringReplace, Work, Work, *, , All
	StringReplace, Work, Work, Armor Class, Armor Class<ARMCLA>
	StringReplace, Work, Work, Hit Points, Hit Points<HITPOI>
	StringReplace, Work, Work, Speed, Speed<SPEEED>
	StringReplace, Work, Work, Skills, <XXX>Skills<SKILLS>
	StringReplace, Work, Work, Damage Vulnerabilities, <XXX>Damage Vulnerabilities<DAMVUL>
	StringReplace, Work, Work, Damage Resistance%A_SPACE%, <XXX>Damage Resistances%A_SPACE%
	StringReplace, Work, Work, Damage Resistances, <XXX>Damage Resistances<DAMRES>
	StringReplace, Work, Work, Damage Immunities, <XXX>Damage Immunities<DAMIMM>
	StringReplace, Work, Work, Condition Immunities, <XXX>Condition Immunities<CONIMM>
	StringReplace, Work, Work, Senses, <XXX>Senses<SENSES>
	StringReplace, Work, Work, Languages, <XXX>Languages<LANGUA>
	StringReplace, Work, Work, Challenge, <XXX>Challenge
	StringReplace, Work, Work, 0ft., 0 ft., All

; NPC Name and image name
	Work:=RegExReplace(Work,"^\s*","") ; remove leading newlines
	NameCheck := RegExMatch(Work, "sP).*\r\nArmor Class", NameLength, 1)
	NameCheck := SubStr(Work,1,Namelength)
	Match:= Chr(10)
	StringReplace NameCheck, NameCheck, %Match%, <NEWLINE>, UseErrorLevel


	If (ErrorLevel = 0)	{
		NPCname := "<NEWCREATURE>"
		NPCsize := "size"
		NPCtype := "type"
		NPCTag := ""
		NPCalign := "alignment"
		SizeString := NPCsize " " NPCtype ", " NPCalign
	}

	If (ErrorLevel = 1)	{
		NPCname := "<NEWCREATURE>"
		SizeString:= SubStr(Work,1,InStr(Work,Chr(13))-1)
		StringTrimLeft, Work, Work, InStr(Work,Chr(10))
	}

	If (ErrorLevel = 2)	{
		NPCname:= SubStr(Work,1,InStr(Work,Chr(13))-1)
		StringTrimLeft, Work, Work, InStr(Work,Chr(10))
		
		SizeString:= SubStr(Work,1,InStr(Work,Chr(13))-1)
		StringTrimLeft, Work, Work, InStr(Work,Chr(10))
	}

	Name_work()

	Work:=RegExReplace(Work,"\s*$","") ; remove trailing newlines
		
; NPC Size, Type, Tag, Alignment
	FoundPos := RegExMatch(SizeString, "i)(Tiny|Small|Medium|Large|Huge|Gargantuan)" , NPCsize)	;{
	NPCsize = %NPCsize%
	StringReplace SizeString, SizeString, %NPCsize%, 
	SizeString = %SizeString%
	NPCsize:= TC(NPCsize)
	
	FoundPos := RegExMatch(SizeString, "\((.+)\)" , NPCtag)
	StringReplace SizeString, SizeString, %NPCtag%, 
	StringReplace NPCtag, NPCtag, (, 
	StringReplace NPCtag, NPCtag, ), 
	NPCtag = %NPCtag%																			;}
		
	FoundPos := RegExMatch(SizeString, "P), .*", StatLength)+2
	NPCalign:= SubStr(SizeString,FoundPos,StatLength-2)
	NPCalign = %NPCalign%
	StringReplace NPCalign, NPCalign, `r,
	StringReplace NPCalign, NPCalign, `n,
	Chop:= SubStr(SizeString,FoundPos-2,StatLength)
	StringReplace, SizeString, SizeString, %Chop%, 
	SizeString = %SizeString%

	FoundPos := RegExMatch(SizeString, "i)(swarm of tiny aberrations|swarm of tiny beasts|swarm of tiny constructs|swarm of tiny elementals|swarm of tiny monstrosities|swarm of tiny oozes|swarm of tiny plants)" , NPCtype)
	If NPCtype {
		NPCtype = %NPCtype%
		StringReplace SizeString, SizeString, %NPCtype%, 
		SizeString = %SizeString%
		StringReplace NPCtype, NPCtype, tiny, Tiny
	} else {
		FoundPos := RegExMatch(SizeString, "i)(Aberration|Beast|Celestial|Construct|Dragon|Elemental|Fey|Fiend|Giant|Humanoid|Monstrosity|Ooze|Plant|Undead|Trap)" , NPCtype)
		NPCtype = %NPCtype%
		StringReplace SizeString, SizeString, %NPCtype%, 
		SizeString = %SizeString%
	}

; NPC Armor Class
	StatStart := RegExMatch(Work, "P)<ARMCLA> .*\r\n", StatLength)
	If StatStart	{
		StatStart := StatStart+9
		NPCac:= SubStr(Work,StatStart,StatLength-11)
		Chop:= SubStr(Work,StatStart-9-11,StatLength+11)
		StringReplace, Work, Work, %Chop%, 
		NPCac = %NPCac%
	}
	else
		NPCac:= ""

; NPC Hit Points
	StatStart := RegExMatch(Work, "P)<HITPOI> .*\r\n", StatLength)
	If StatStart	{
		StatStart := StatStart+9
		NPChp:= SubStr(Work,StatStart,StatLength-11)
		Chop:= SubStr(Work,StatStart-9-10,StatLength+10)
		StringReplace, Work, Work, %Chop%, 
		NPChp = %NPChp%
	}
	else
		NPChp:= ""

; NPC Movement Rates
	StatStart := RegExMatch(Work, "P)<SPEEED> .*\r\n", StatLength)
	If StatStart	{
		StatStart := StatStart+9
		NPCSpeed:= SubStr(Work,StatStart,StatLength-11)
		Chop:= SubStr(Work,StatStart-9-5,StatLength+5)
		StringReplace, Work, Work, %Chop%, 
		NPCSpeed = %NPCSpeed%
		NPCspeed:= "Speed " NPCspeed ","
		FlagSpeeds:= "1"
		StringLower NPCspeed, NPCspeed

		If Instr(NPCspeed, "speed") {
			Dummy:= RegExMatch(NPCspeed, "U)speed.*ft", NPCwalk)
			NPCwalk:= RegExReplace(NPCwalk, "[^\d]", "")
			NPCwalk = %NPCwalk%
		}

		If Instr(NPCspeed, "burrow") {
			Dummy:= RegExMatch(NPCspeed, "U)burrow.*ft", NPCburrow)
			NPCburrow:= RegExReplace(NPCburrow, "[^\d]", "")
			NPCburrow = %NPCburrow%
		}

		If Instr(NPCspeed, "climb") {
			Dummy:= RegExMatch(NPCspeed, "U)climb.*ft", NPCclimb)
			NPCclimb:= RegExReplace(NPCclimb, "[^\d]", "")
			NPCclimb = %NPCclimb%
		}

		If Instr(NPCspeed, "fly") {
			Dummy:= RegExMatch(NPCspeed, "U)fly.*ft", NPCfly)
			NPCfly:= RegExReplace(NPCfly, "[^\d]", "")
			NPCfly = %NPCfly%
		}

		If Instr(NPCspeed, "hover") {
			NPChover:= 1
		}
		
		If Instr(NPCspeed, "swim") {
			Dummy:= RegExMatch(NPCspeed, "U)swim.*ft", NPCswim)
			NPCswim:= RegExReplace(NPCswim, "[^\d]", "")
			NPCswim = %NPCswim%
		}
	}
	
	StringReplace, Work, Work, `r`n, %A_SPACE%, All
	StringReplace, Work, Work, <XXX>, `r`n<XXX>, All
	
; NPC saves
	StatStart := RegExMatch(Work, "P)<SAVETH> .*\r\n<XXX>", StatLength)
	If StatStart	{
		StatStart := StatStart+9
		NPCsave:= SubStr(Work,StatStart,StatLength-16)
		Chop:= SubStr(Work,StatStart-9-13,StatLength-5+13)
		StringReplace, Work, Work, %Chop%, 
		NPCsave = %NPCsave%
		StringReplace, NPCsave, NPCsave, +%A_SPACE%, +, All
		StringLower NPCsave, NPCsave
		NPCsave:= NPCsave ","
	}

	If Instr(NPCsave, "str") {
		Dummy:= RegExMatch(NPCsave, "U)str.*,", NPCstrsav)
		NPCstrsav:= RegExReplace(NPCstrsav, "[^\d]", "")
		NPCstrsav = %NPCstrsav%
	}
	If Instr(NPCsave, "dex") {
		Dummy:= RegExMatch(NPCsave, "U)dex.*,", NPCdexsav)
		NPCdexsav:= RegExReplace(NPCdexsav, "[^\d]", "")
		NPCdexsav = %NPCdexsav%
	}
	If Instr(NPCsave, "con") {
		Dummy:= RegExMatch(NPCsave, "U)con.*,", NPCconsav)
		NPCconsav:= RegExReplace(NPCconsav, "[^\d]", "")
		NPCconsav = %NPCconsav%
	}
	If Instr(NPCsave, "int") {
		Dummy:= RegExMatch(NPCsave, "U)int.*,", NPCintsav)
		NPCintsav:= RegExReplace(NPCintsav, "[^\d]", "")
		NPCintsav = %NPCintsav%
	}
	If Instr(NPCsave, "wis") {
		Dummy:= RegExMatch(NPCsave, "U)wis.*,", NPCwissav)
		NPCwissav:= RegExReplace(NPCwissav, "[^\d]", "")
		NPCwissav = %NPCwissav%
	}
	If Instr(NPCsave, "cha") {
		Dummy:= RegExMatch(NPCsave, "U)cha.*,", NPCchasav)
		NPCchasav:= RegExReplace(NPCchasav, "[^\d]", "")
		NPCchasav = %NPCchasav%
	}
	
; NPC Senses
	StatStart := RegExMatch(Work, "P)<SENSES> .*\r\n<XXX>", StatLength)
	NPCpassperc:= ""
	If StatStart	{
		StatStart:= StatStart+9
		NPCsense:= SubStr(Work,StatStart,StatLength-16)
		Chop:= SubStr(Work,StatStart-9-6,StatLength-5+6)
		StringReplace, Work, Work, %Chop%, 
		NPCsense = %NPCsense%
		DummyVar:= RegExMatch(NPCsense, "i)passive Perception", percep)
		NPCpassperc:= SubStr(NPCsense, DummyVar)
		StringReplace, NPCsense, NPCsense, %NPCpassperc%, 
		StringReplace, NPCpassperc, NPCpassperc, passive Perception, 
		NPCpassperc = %NPCpassperc%
		StringLower NPCsense, NPCsense
		
		If Instr(NPCsense, "blindsight") {
			Dummy:= RegExMatch(NPCsense, "U)blindsight.*ft", NPCblind)
			NPCblind:= RegExReplace(NPCblind, "[^\d]", "")
			NPCblind = %NPCblind%
		}
		If Instr(NPCsense, "(blind beyond") {
			NPCblindB:= 1
		}
		If Instr(NPCsense, "darkvision") {
			Dummy:= RegExMatch(NPCsense, "U)darkvision.*ft", NPCdark)
			NPCdark:= RegExReplace(NPCdark, "[^\d]", "")
			NPCdark = %NPCdark%
		}
		If Instr(NPCsense, "tremorsense") {
			Dummy:= RegExMatch(NPCsense, "U)tremorsense.*ft", NPCtremor)
			NPCtremor:= RegExReplace(NPCtremor, "[^\d]", "")
			NPCtremor = %NPCtremor%
		}
		If Instr(NPCsense, "truesight") {
			Dummy:= RegExMatch(NPCsense, "U)truesight.*ft", NPCtrue)
			NPCtrue:= RegExReplace(NPCtrue, "[^\d]", "")
			NPCtrue = %NPCtrue%
		}
	}
	

	
; NPC skills
	StatStart := RegExMatch(Work, "P)<SKILLS> .*\r\n<XXX>", StatLength)
	If StatStart
	{
		StatStart := StatStart+9
		NPCskill:= SubStr(Work,StatStart,StatLength-16)
		Chop:= SubStr(Work,StatStart-9-6,StatLength-5+6)
		StringReplace, Work, Work, %Chop%, 
		NPCskill = %NPCskill%
		StringReplace, NPCskill, NPCskill, +%A_SPACE%, +, All
	}
	Loop, parse, NPCskill, `,
	{
		NPCHolding:= A_Loopfield
		NPCHolding = %NPCHolding%
		If NPCHolding {
			skillname:= SubStr(NPCHolding, 1, InStr(NPCHolding, "+") - 2)
			skillvalue:= SubStr(NPCHolding, InStr(NPCHolding, "+") + 1)
			skillvalue = %skillvalue%
			If NPC_Skills.haskey(skillname) {
				NPC_Skills[skillname]:= skillvalue
			}
		}
	}
	
; NPC Damage Vulnerabilities
	StatStart := RegExMatch(Work, "P)<DAMVUL> .*\r\n<XXX>", StatLength)
	NPCdamvul:= ""
	If StatStart {
		StatStart := StatStart+9
		NPCdamvul:= SubStr(Work,StatStart,StatLength-16)
		Chop:= SubStr(Work,StatStart-9-22,StatLength-5+22)
		StringReplace, Work, Work, %Chop%, 
		NPCdamvul = %NPCdamvul%
		DV_Works()
	}
	
; NPC Damage Resistances
	StatStart := RegExMatch(Work, "P)<DAMRES> .*\r\n<XXX>", StatLength)
	NPCdamres:= ""
	If StatStart {
		StatStart := StatStart+9
		NPCdamres:= SubStr(Work,StatStart,StatLength-16)
		Chop:= SubStr(Work,StatStart-9-18,StatLength-5+18)
		StringReplace, Work, Work, %Chop%, 
		NPCdamres = %NPCdamres%
		DR_Works()
	}
	
; NPC Damage Immunities
	StatStart := RegExMatch(Work, "P)<DAMIMM> .*\r\n<XXX>", StatLength)
	NPCdamimm:= ""
	If StatStart {
		StatStart := StatStart+9
		NPCdamimm:= SubStr(Work,StatStart,StatLength-16)
		Chop:= SubStr(Work,StatStart-9-17,StatLength-5+17)
		StringReplace, Work, Work, %Chop%, 
		NPCdamimm = %NPCdamimm%
		DI_Works()
	}

; NPC Condition Immunities
	StatStart := RegExMatch(Work, "P)<CONIMM> .*\r\n<XXX>", StatLength)
	NPCconimm:= ""
	If StatStart {
		StatStart := StatStart+9
		NPCconimm:= SubStr(Work,StatStart,StatLength-16)
		Chop:= SubStr(Work,StatStart-9-20,StatLength-5+20)
		StringReplace, Work, Work, %Chop%, 
		NPCconimm = %NPCconimm%
		CI_Works()
	}
	
; NPC Languages
	StatStart := RegExMatch(Work, "P)<LANGUA> .*\r\n<XXX>", StatLength)
	NPClang:= ""
	foundlang:= 0
	If StatStart
	{
		StatStart := StatStart+9
		NPClang:= SubStr(Work,StatStart,StatLength-16)
		Chop:= SubStr(Work,StatStart-9-9,StatLength-5+9)
		StringReplace, Work, Work, %Chop%, 
		NPClang = %NPClang%
	}
	If InStr(NPClang, "telepathy") {
		temppos:= InStr(NPClang, "telepathy")
		tempvar:= SubStr(NPClang, temppos)
		stringreplace NPClang, NPClang, %tempvar%,
		NPClang = %NPClang%
		If (SubStr(NPClang, 0, 1) = ",") {
			StringTrimRight, NPClang, NPClang, 1
		}
		NPCtelep:= 1
		stringreplace telrange, tempvar, telepathy ,
		telrange = %telrange%
		LangSelect:= 1
	}

	If (NPClang = "" or NPClang = "-" or NPClang = "--") {
		NPClang:= ""
		LangSelect:= 2
		foundlang:= 1
	}

	If (NPClang = "All" or NPClang = "all") {
		NPClang:= ""
		LangSelect:= 3
		foundlang:= 1
	}

	If RegExMatch(NPClang, "iU)Understands.*languages of.*creator but can't speak") {
		NPClang:= ""
		LangSelect:= 6
		foundlang:= 1
	}

	If RegExMatch(NPClang, "iU)Understands.*languages.*knew in life but can't speak") {
		NPClang:= ""
		LangSelect:= 7
		foundlang:= 1
	}

	If RegExMatch(NPClang, "iU).*languages.*knew in life") {
		NPClang:= ""
		LangSelect:= 4
		foundlang:= 1
	}

	If RegExMatch(NPClang, "iU)Understands.* but can't speak") {
		NPCLanguages:= []
		For key, val in LangStan
		{
			If Instr(NPClang, val) {
				NPCLanguages.push(val)
			}
		}
		For key, val in LangExot
		{
			If Instr(NPClang, val) {
				NPCLanguages.push(val)
			}
		}
		For key, val in LangMons
		{
			If Instr(NPClang, val) {
				NPCLanguages.push(val)
			}
		}
		For key, val in LangUser
		{
			If Instr(NPClang, val) {
				NPCLanguages.push(val)
			}
		}
		LangSelect:= 5
		NPClang:= ""
		foundlang:= 1
	}

	If NPClang {
		NPCLanguages:= []
		For key, val in LangStan
		{
			If Instr(NPClang, val) {
				NPCLanguages.push(val)
			}
		}
		For key, val in LangExot
		{
			If Instr(NPClang, val) {
				NPCLanguages.push(val)
			}
		}
		For key, val in LangMons
		{
			If Instr(NPClang, val) {
				NPCLanguages.push(val)
			}
		}
		For key, val in LangUser
		{
			If Instr(NPClang, val) {
				NPCLanguages.push(val)
			}
		}
		LangSelect:= 1
		foundlang:= 1
	}

	If !foundlang {
		LangAlt:= NPClang
		LangSelect:= 8
	}
	NPClang:= ""

; NPC Challenge
	Work := Work " "
	StatStart := RegExMatch(Work, "P)Challenge .*XP\)", StatLength)
	NPCcharat:= ""
	NPCxp:= ""
	If StatStart
	{
		StatStart := StatStart+9
		NPCchall:= SubStr(Work,StatStart,StatLength-9)
		StringReplace, NPCchall, NPCchall, `r, , All
		StringReplace, NPCchall, NPCchall, `n, , All
		NPCcharat = %NPCchall%
		NPCxp = %NPCchall%
		NPCcharat:= RegExReplace(NPCcharat, "\(.*XP\)", "")
		NPCcharat = %NPCcharat%
		StringReplace, NPCxp, NPCxp, %NPCcharat%
		StringReplace, NPCxp, NPCxp, (
		StringReplace, NPCxp, NPCxp, XP)
		NPCxp = %NPCxp%
	}
}

Actions_Prep() {
	global
	Work:= RegExReplace(Work,"\s*$","") ; remove trailing newlines
	Work:= RegExReplace(Work,"^\s*","") ; remove leading newlines
	Work:= Work Chr(13) Chr(10) "<ACT>"
	Work:= RegExReplace(Work,"\r\nHit:"," Hit:")

	StringReplace Work, Work, -`r`n, , All ; Remove a dash or dot followed by newline,
	StringReplace Work, Work, �`r`n, , All ; since that's probably a hyphen.
	StringReplace Work, Work, �, -, All ; Replace dots - misread hyphens.
	StringReplace Work, Work, Cantrips:, Cantrips, All 
	;~ StringReplace Work, Work, *, , All
}

Actions_InnSpellCast() {
	global
	InSpellStart := RegExMatch(Work, "Innate Spellcasting(\.| \()")
	If InSpellStart
	{
		InSpellTextEnd:= RegExMatch(Work, "PisU)innately(\r\n| )cast(\r\n| )the(\r\n| )following(\r\n| )spells.*:\r\n", SpLength, InSpellStart)
		InSpellTextEnd:= InSpellTextEnd + SpLength - 1
		InSpellTextWork := SubStr(Work, InSpellStart, InSpellTextEnd - InSpellStart)
		InSpellWork := SubStr(Work, InSpellTextEnd+1)
		StringReplace, InSpellWork, InSpellWork, /Day, /day, All
		StringReplace, InSpellWork, InSpellWork, At Will:, At will:, All
		StringReplace, InSpellWork, InSpellWork, at Will:, At will:, All
		StringReplace, InSpellWork, InSpellWork, at will:, At will:, All

		Local PsiChk
		PsiChk := RegExMatch(InSpellTextWork, "i)Innate Spellcasting \(Psionics\)")
		If Psichk
			NPCPsionics:= 1

		ise_pos:= 0
		ise_len:= 0
		ise_test:= RegExMatch(InSpellWork, "UP)At will:.*\r\n", ise_len) + ise_len
		If (ise_test > ise_pos)
			ise_pos:= ise_test
		ise_len:= 0
		ise_test:= RegExMatch(InSpellWork, "UP)5/day( each)?:.*\r\n", ise_len) + ise_len
		If (ise_test > ise_pos)
			ise_pos:= ise_test
		ise_len:= 0
		ise_test:= RegExMatch(InSpellWork, "UP)4/day( each)?:.*\r\n", ise_len) + ise_len
		If (ise_test > ise_pos)
			ise_pos:= ise_test
		ise_len:= 0
		ise_test:= RegExMatch(InSpellWork, "UP)3/day( each)?:.*\r\n", ise_len) + ise_len
		If (ise_test > ise_pos)
			ise_pos:= ise_test
		ise_len:= 0
		ise_test:= RegExMatch(InSpellWork, "UP)2/day( each)?:.*\r\n", ise_len) + ise_len
		If (ise_test > ise_pos)
			ise_pos:= ise_test
		ise_len:= 0
		ise_test:= RegExMatch(InSpellWork, "UP)1/day( each)?:.*\r\n", ise_len) + ise_len
		If (ise_test > ise_pos)
			ise_pos:= ise_test

		InSpellEnd:= ise_pos
		InSpellWork := SubStr(InSpellWork, 1, InSpellEnd-1)
		StringLower, InSpellWork, InSpellWork
		SpellChop:= SubStr(Work, InSpellStart, InSpellTextEnd -InSpellStart + InSpellEnd)
		StringReplace Work, Work, %SpellChop%, , 
		
		StringReplace InSpellTextWork, InSpellTextWork, %A_Space% `r`n, %A_Space%, All
		StringReplace InSpellTextWork, InSpellTextWork, `r`n, %A_Space%, All
		InSpellTextWork := RegExReplace(InSpellTextWork, "\s+" , " ")
		SpellCall := RegExMatch(InSpellTextWork, "spellcasting ability is (.*) \(", Match)
		NPCinspability:= Match1
		SpellCall := RegExMatch(InSpellTextWork, "spell save DC (\d+)", Match)
		NPCinspsave:= Match1
		SpellCall := RegExMatch(InSpellTextWork, "\+(\d+) to hit with spell attacks", Match)
		If SpellCall
			NPCinsptohit:= Match1
		else
			NPCinsptohit:= ""
		SpellCall := RegExMatch(InSpellTextWork, "U)innately cast the following spells(.*):", Match)
		If SpellCall {
			NPCinsptext:= Match1
			If (Substr(NPCinsptext,1,1) = ",") {
				NPCinsptext:= Substr(NPCinsptext,3)
			}
			If (Substr(NPCinsptext, 0) = ".") {
				StringTrimRight, NPCinsptext, NPCinsptext, 1
			}
		} else {
			NPCinsptext:= ""
		}

		StringReplace InSpellWork, InSpellWork, %A_Space%`r`n, %A_Space%, All
		StringReplace InSpellWork, InSpellWork, `r`n, %A_Space%, All
		InSpellWork := RegExReplace(InSpellWork, "\s+" , " ")
		
		StringReplace, InSpellWork, InSpellWork, \rAt will, At will, 
		StringReplace, InSpellWork, InSpellWork, \rAt Will, At will, 
		StringReplace, InSpellWork, InSpellWork, \r1/day, 1/day, 
		StringReplace, InSpellWork, InSpellWork, \r2/day, 2/day, 
		StringReplace, InSpellWork, InSpellWork, \r3/day, 3/day, 
		StringReplace, InSpellWork, InSpellWork, \r4/day, 4/day, 
		StringReplace, InSpellWork, InSpellWork, \r5/day, 5/day, 
		StringReplace, InSpellWork, InSpellWork, \r6/day, 6/day, 
		StringReplace, InSpellWork, InSpellWork, \r7/day, 7/day, 
		StringReplace, InSpellWork, InSpellWork, \r8/day, 8/day, 
		StringReplace, InSpellWork, InSpellWork, \r9/day, 9/day, 
		StringReplace, InSpellWork, InSpellWork, %A_Space%1/day, `n1/day, 
		StringReplace, InSpellWork, InSpellWork, %A_Space%2/day, `n2/day, 
		StringReplace, InSpellWork, InSpellWork, %A_Space%3/day, `n3/day, 
		StringReplace, InSpellWork, InSpellWork, %A_Space%4/day, `n4/day, 
		StringReplace, InSpellWork, InSpellWork, %A_Space%5/day, `n5/day, 
		StringReplace, InSpellWork, InSpellWork, %A_Space%6/day, `n6/day, 
		StringReplace, InSpellWork, InSpellWork, %A_Space%7/day, `n7/day, 
		StringReplace, InSpellWork, InSpellWork, %A_Space%8/day, `n8/day, 
		StringReplace, InSpellWork, InSpellWork, %A_Space%9/day, `n9/day, 
		InSpellWork:= RegExReplace(InSpellWork, "U)\n([^\d])", " $1") 
		FlagInSpell:= 1
		InSpell_Works()
	}
}

Actions_SpellCast() {
	global
	SpellStart := RegExMatch(Work, "m)^Spellcasting(\.| \()")
	If SpellStart
	{
		SpellEnd := 14
		TempCall := RegExMatch(Work, "PU)Cantrips \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		TempCall := RegExMatch(Work, "PUi)1st level \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		TempCall := RegExMatch(Work, "PUi)2nd level \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		TempCall := RegExMatch(Work, "PUi)3rd level \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		TempCall := RegExMatch(Work, "PUi)4th level \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		TempCall := RegExMatch(Work, "PUi)5th level \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		TempCall := RegExMatch(Work, "PUi)6th level \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		TempCall := RegExMatch(Work, "PUi)7th level \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		TempCall := RegExMatch(Work, "PUi)8th level \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		TempCall := RegExMatch(Work, "PUi)9th level \(.*\r\n", SpLength)
		If TempCall
			SpellEnd:= TempCall + SpLength
		
		Tempwork:= SubStr(work, SpellEnd)
		SpellWork := SubStr(Work,SpellStart,SpellEnd - SpellStart)
		StringReplace Work, Work, %SpellWork%, , 

		NPCSpellStar:= ""
		TempCall := RegExMatch(Tempwork, "mUA)[\r\n]*^\*(.*)\r\n", NPCSpellStar)
		StringReplace Work, Work, %NPCSpellStar%, , 
		
		TempCall := RegExMatch(SpellWork, "9th level \(")
		If TempCall
			SpellTextEnd:= TempCall
		TempCall := RegExMatch(SpellWork, "8th level \(")
		If TempCall
			SpellTextEnd:= TempCall
		TempCall := RegExMatch(SpellWork, "7th level \(")
		If TempCall
			SpellTextEnd:= TempCall
		TempCall := RegExMatch(SpellWork, "6th level \(")
		If TempCall
			SpellTextEnd:= TempCall
		TempCall := RegExMatch(SpellWork, "5th level \(")
		If TempCall
			SpellTextEnd:= TempCall
		TempCall := RegExMatch(SpellWork, "4th level \(")
		If TempCall
			SpellTextEnd:= TempCall
		TempCall := RegExMatch(SpellWork, "3rd level \(")
		If TempCall
			SpellTextEnd:= TempCall
		TempCall := RegExMatch(SpellWork, "2nd level \(")
		If TempCall
			SpellTextEnd:= TempCall
		TempCall := RegExMatch(SpellWork, "1st level \(")
		If TempCall
			SpellTextEnd:= TempCall
		TempCall := RegExMatch(SpellWork, "Cantrips \(")
		If TempCall
			SpellTextEnd:= TempCall

		
		SpellTextEnd := SpellTextEnd-1
		SpellTextWork := SubStr(SpellWork,1,SpellTextEnd)
		StringReplace SpellWork, SpellWork, %SpellTextWork%, , 
		StringReplace SpellTextWork, SpellTextWork, %A_Space% `r`n, %A_Space%, All
		StringReplace SpellTextWork, SpellTextWork, `r`n, %A_Space%, All
		SpellTextWork := RegExReplace(SpellTextWork, "\s+" , " ")
		
		SpellCall := RegExMatch(SpellTextWork, "is an? (.*)level spellcaster.", Match)
		NPCsplevel:= Match1
		StringTrimRight, NPCsplevel, NPCsplevel, 1
		SpellCall := RegExMatch(SpellTextWork, "spellcasting ability is (.*) \(", Match)
		NPCspability:= Match1
		SpellCall := RegExMatch(SpellTextWork, "save DC (\d+)", Match)
		NPCspsave:= Match1
		SpellCall := RegExMatch(SpellTextWork, "\+(\d+) to hit", Match)
		NPCsptohit:= Match1
		SpellCall := RegExMatch(SpellTextWork, "has the following (.*) spells prepared:", Match)
		NPCspclass:= Match1
		SpellCall := RegExMatch(SpellTextWork, " knows the following (.*) spells:", Match)
		If Match1
			NPCspclass:= Match1

		TempPos := RegExMatch(SpellTextWork, "P)attacks\)\.", TempLen) + TempLen
		SpellChop := SubStr(SpellTextWork,1,TempPos)
		StringReplace SpellTextWork, SpellTextWork, %SpellChop%, , 
		gg:= RegExMatch(SpellTextWork, "\..*prepared:")
		if gg
		{
			SpellTextWork:= RegExReplace(SpellTextWork, "\..*prepared:", ".")
			NPCspflavour = %SpellTextWork%
		}
		else
			NPCspflavour := ""
		
		
		StringReplace SpellWork, SpellWork, %A_Space%`r`n, %A_Space%, All
		StringReplace SpellWork, SpellWork, `r`n, %A_Space%, All
		SpellWork := RegExReplace(SpellWork, "\s+" , " ")
		SpellWork:= RegExReplace(SpellWork,"\s*$","") ; remove trailing newlines
		SpellWork:= RegExReplace(SpellWork,"^\s*","") ; remove leading newlines
		Spellwork = %Spellwork%
		Spellwork := spellwork Chr(10)
		
		StringReplace, SpellWork, SpellWork, \rCantrips, cantrips, 
		StringReplace, SpellWork, SpellWork, \r1st, 1st, 
		StringReplace, SpellWork, SpellWork, \r2nd, 2nd, 
		StringReplace, SpellWork, SpellWork, \r3rd, 3rd, 
		StringReplace, SpellWork, SpellWork, \r4th, 4th, 
		StringReplace, SpellWork, SpellWork, \r5th, 5th, 
		StringReplace, SpellWork, SpellWork, \r6th, 6th, 
		StringReplace, SpellWork, SpellWork, \r7th, 7th, 
		StringReplace, SpellWork, SpellWork, \r8th, 8th, 
		StringReplace, SpellWork, SpellWork, \r9th, 9th, 
		StringReplace, SpellWork, SpellWork, Cantrip%A_Space%, Cantrips, 
		StringReplace, SpellWork, SpellWork, %A_Space%1st level, `n1st Level, 
		StringReplace, SpellWork, SpellWork, %A_Space%2nd level, `n2nd Level, 
		StringReplace, SpellWork, SpellWork, %A_Space%3rd level, `n3rd Level, 
		StringReplace, SpellWork, SpellWork, %A_Space%4th level, `n4th Level, 
		StringReplace, SpellWork, SpellWork, %A_Space%5th level, `n5th Level, 
		StringReplace, SpellWork, SpellWork, %A_Space%6th level, `n6th Level, 
		StringReplace, SpellWork, SpellWork, %A_Space%7th level, `n7th Level, 
		StringReplace, SpellWork, SpellWork, %A_Space%8th level, `n8th Level, 
		StringReplace, SpellWork, SpellWork, %A_Space%9th level, `n9th Level, 
		SpellWork:= RegExReplace(SpellWork, "U)\n([^\d])", " $1") 
		FlagSpell:= 1
		Spell_Works()
	}
}

Get_Actions() {
	global
	NPCActStart:= RegExMatch(Work, "UPs)<ACT>ACTIONS<ION>.*<ACT>", ActLen)
	If NPCActStart
	{
		NPCactions:= SubStr(Work, NPCActStart, ActLen-5)
		StringReplace Work, Work, %NPCactions%, 
	
		StringReplace NPCactions, NPCactions, <ACT>ACTIONS<ION>%A_Space%, 
		StringReplace NPCactions, NPCactions, <ACT>ACTIONS<ION>, 
		NPCactions:=RegExReplace(NPCactions,"\s*$","") ; remove trailing newlines
		If Source = SB3
		{
			NPCactions:= RegExReplace(NPCactions,"i)Magic\s*","Magic.`r`n")
			TestAction:= NPCactions
			RegExReplace(TestAction, "U)damage\d\).*damage\.", "<XXX>", NumberFound)
			If Numberfound
				NPCactions:= RegExReplace(NPCactions, "U)damage\d\).*damage\.", "damage.")
			
			BuildActionFile:= ""
			Loop, parse, NPCactions, `n, `r
			{
				If RegExMatch(A_Loopfield," Magic.$")
				{
					BuildAction:= RegExReplace(A_Loopfield," Magic.$","")
					StringReplace BuildAction, BuildAction, %A_Space%damage, `,%A_Space%magic damage
				}
				else
					BuildAction:= A_Loopfield
				
			BuildActionFile:= BuildActionFile BuildAction Chr(13) Chr(10)
			}
			NPCactions:= BuildActionFile
			NPCactions:=RegExReplace(NPCactions,"\s*$","") ; remove trailing newlines
		}
		
		FlagActions:= ""
		Tag_Title_2(NPCactions)
		StringReplace, NPCactions, NPCactions, `r`n\r, \r, All 
		StringReplace, NPCactions, NPCactions, `r`n \r, \r, All 
		
		Loop, parse, NPCactions, `n, `r
		{
			NPCHolding:= A_Loopfield
			If NPCHolding
			{
				NPC_Actions[A_Index, "Name"]:= SubStr(NPCHolding, 1, InStr(NPCHolding, ".") - 1)
				If (SubStr(NPC_Actions[A_Index, "Name"],1,11) = "Multiattack")
					NPC_Actions[A_Index, "Name"]:= "Multiattack"
				NPC_Actions[A_Index, "Action"]:= SubStr(NPCHolding, InStr(NPCHolding, ".") + 2)
				FlagActions:= "1"
			}
		}

	}
Actionworks()
}

Get_Reactions() {
	global
	NPCActStart:= RegExMatch(Work, "UPs)<ACT>REACTIONS<ION>.*<ACT>", ActLen)
	If NPCActStart
	{
		NPCreactions:= SubStr(Work, NPCActStart, ActLen-5)
		StringReplace Work, Work, %NPCreactions%, 
	
		StringReplace NPCreactions, NPCreactions, <ACT>REACTIONS<ION>%A_Space%, 
		StringReplace NPCreactions, NPCreactions, <ACT>REACTIONS<ION>, 
		NPCreactions:=RegExReplace(NPCreactions,"\s*$","") ; remove trailing newlines
		
		FlagReactions:= ""
		Tag_Title_2(NPCreactions)
		StringReplace, NPCreactions, NPCreactions, `r`n\r, \r, All 
		StringReplace, NPCreactions, NPCreactions, `r`n \r, \r, All 
		
		Loop, parse, NPCreactions, `n, `r
		{
			NPCHolding:= A_Loopfield
			If NPCHolding
			{
				NPC_Reactions[A_Index, "Name"]:= SubStr(NPCHolding, 1, InStr(NPCHolding, ".") - 1)
				NPC_Reactions[A_Index, "Reaction"]:= SubStr(NPCHolding, InStr(NPCHolding, ".") + 2)
				FlagReactions:= "1"
			}
		}
	}
}

Get_LegendaryActions() {
	global
	NPCActStart:= RegExMatch(Work, "UPs)<ACT>LEGENDARY ACTIONS<ION>.*<ACT>", ActLen)
	If NPCActStart
	{
		NPClegactions:= SubStr(Work, NPCActStart, ActLen-5)
		StringReplace Work, Work, %NPClegactions%, 
	
		StringReplace NPClegactions, NPClegactions, <ACT>LEGENDARY ACTIONS<ION> ,
		StringReplace NPClegactions, NPClegactions, <ACT>LEGENDARY ACTIONS<ION>,
		NPClegactions:= RegexReplace( NPClegactions, "^\s+|\s+$" )
		If (SubStr(NPClegactions, 1, 9) != "Options. ") {
			NPClegactions:= "Options. " NPClegactions
		}
		StringReplace NPClegactions, NPClegactions, %A_Space%%A_Space%, %A_Space%
		
		FlagLegActions:= ""
		Tag_Title_2(NPClegactions)
		StringReplace, NPClegactions, NPClegactions, `r`n\r, \r, All 
		StringReplace, NPClegactions, NPClegactions, `r`n \r, \r, All 
		
		Loop, parse, NPClegactions, `n, `r
		{
			NPCHolding:= A_Loopfield
			If NPCHolding
			{
				NPC_Legendary_Actions[A_Index, "Name"]:= SubStr(NPCHolding, 1, InStr(NPCHolding, ".") - 1)
				NPC_Legendary_Actions[A_Index, "LegAction"]:= SubStr(NPCHolding, InStr(NPCHolding, ".") + 2)
				FlagLegActions:= "1"
			}
		}
	}
}

Get_LairActions() {
	global
	local lairopt, lairopt2
	NPCActStart:= RegExMatch(Work, "UPs)<ACT>LAIR ACTIONS<ION>.*<ACT>", ActLen)
	If NPCActStart {
		NPClairactions:= SubStr(Work, NPCActStart, ActLen-5)
		StringReplace Work, Work, %NPClairactions%, 
	
		StringReplace NPClairactions, NPClairactions, <ACT>LAIR ACTIONS<ION> ,
		StringReplace NPClairactions, NPClairactions, <ACT>LAIR ACTIONS<ION>,
		StringReplace NPClairactions, NPClairactions, in a row:, in a row.
		StringReplace NPClairactions, NPClairactions, %A_Space%%A_Space%, %A_Space%
		lairoptions:= ""
		lairoptions:= RegExMatch(NPClairactions, "UPs)On (I|i)nitiative count 20.*`r`n", ActLen)
		lairopt:= SubStr(NPClairactions, lairoptions, ActLen-1)
		lairopt2:= SubStr(NPClairactions, 1, lairoptions+ActLen-1)
		StringReplace NPClairactions, NPClairactions, %lairopt2%, 
		NPC_Lair_Actions[1, "Name"]:= "Options"
		NPC_Lair_Actions[1, "LairAction"]:= lairopt
		NPClairactions:=RegExReplace(NPClairactions,"^\s*","") ; remove leading newlines
		NPClairactions:=RegExReplace(NPClairactions,"\s*$","") ; remove trailing newlines
		FlagLairActions:= ""
		
		Loop, parse, NPClairactions, `n, `r
		{
			NPCHolding:= A_Loopfield
			If NPCHolding
			{
				NPC_Lair_Actions[A_Index+1, "Name"]:= "Lair Action " A_Index
				NPC_Lair_Actions[A_Index+1, "LairAction"]:= NPCHolding
				FlagLairActions:= "1"
			}
		}
	}
}

Get_Traits() {
	global
	NPCtraits:= Work
	StringReplace NPCtraits, NPCtraits, <ACT>,
	NPCtraits:=RegExReplace(NPCtraits,"\s*$","") ; remove trailing newlines
	
	FlagTraits:= ""
	Tag_Title_2(NPCtraits)
	StringReplace, NPCtraits, NPCtraits, `r`n\r, \r, All 
	StringReplace, NPCtraits, NPCtraits, `r`n \r, \r, All 
	Loop, parse, NPCtraits, `n, `r
	{
		NPCHolding:= A_Loopfield
		If NPCHolding
		{
			key:= SubStr(NPCHolding, 1, InStr(NPCHolding, ".") - 1)
			StringLower, key, key , T
			NPC_Traits[A_Index, "Name"]:= key
			NPC_Traits[A_Index, "trait"]:= SubStr(NPCHolding, InStr(NPCHolding, ".") + 2)
			FlagTraits:= "1"
		}
	}
}

Name_Work() {
	global
	temp:= SubStr(NPCname, 1, 1)
	NPCname:= RegExReplace(NPCname, "^\d+", "")
	If temp is Number
		GuiControl, NPCE_Main:Text, NPCname, %NPCname%
	
	StringUpper NPCname, NPCname, T
	NPCjpeg:= NPCname
	StringLower NPCjpeg, NPCjpeg
	NPCjpeg:= RegExReplace(NPCjpeg, "[^a-zA-Z0-9]", "")
	NPCname = %NPCname%
	NPCjpeg = %NPCjpeg%
	
	GL1:= Pronoun[1, NPCGender]
	GL2:= Pronoun[2, NPCGender]
	GL3:= Pronoun[3, NPCGender]
	GL4:= Pronoun[4, NPCGender]
	StringUpper, GU1, GL1, T
	StringUpper, GU2, GL2, T
	StringUpper, GU3, GL3, T
	StringUpper, GU4, GL4, T

	StringLower NPCtextname, NPCname
	If NPCUnique or NPCpropername
		StringUpper NPCtextname, NPCtextname, T

	If NPCpropername {
		RegExMatch(NPCtextname, "\w*\b", NL)
		NU:= NL
	} else {
		NL:= "the " . NPCtextname
		NU:= "The " . NPCtextname
	}
}

Spell_Works() {
	global
	NPC_Spell_Level:= []
	NPC_Spell_Slots:= []
	NPC_Spell_Number:= []
	NPC_Spell_Names:= []
	Loop, parse, SpellWork, `n, `r
	{
		NPCHolding:= A_Loopfield
		SpellArray:= ""
		If NPCHolding {
			NPC_Spell_Level[A_Index]:= SubStr(NPCHolding, 1, InStr(NPCHolding, " ") - 1)
			NPC_Spell_Slots[A_Index]:= SubStr(NPCHolding, InStr(NPCHolding, "(")+1,InStr(NPCHolding, ")")-InStr(NPCHolding, "(")-1)
			NPCHolding:= SubStr(NPCHolding, InStr(NPCHolding, ":")+1)
			NPCHolding = %NPCHolding%
			SpellArray := StrSplit(NPCHolding, ",")
			NPC_Spell_Number[A_Index]:= SpellArray.MaxIndex()
			TempLoop:= A_Index
			Loop % SpellArray.MaxIndex()
			{
				this_spell:= Trim(SpellArray[A_Index])
				NPC_Spell_Names[TempLoop, A_Index]:= this_spell
			}
		}
	}
}

InSpell_Works() {
	global
	NPC_InSpell_Slots:= []
	NPC_InSpell_Number:= []
	NPC_InSpell_Names:= []
	Loop, parse, InSpellWork, `n, `r
	{
		NPCHolding:= A_Loopfield
		SpellArray:= ""
		If NPCHolding {
			NPC_InSpell_Slots[A_Index]:= SubStr(NPCHolding, 1, InStr(NPCHolding, ":") - 1)
			NPCHolding:= SubStr(NPCHolding, InStr(NPCHolding, ":")+1)
			NPCHolding = %NPCHolding%
			SpellArray := StrSplit(NPCHolding, ",")
			NPC_InSpell_Number[A_Index]:= SpellArray.MaxIndex()
			TempLoop:= A_Index
			Loop % SpellArray.MaxIndex()
			{
				this_spell:= Trim(SpellArray[A_Index])
				NPC_InSpell_Names[TempLoop, A_Index]:= this_spell
			}
		}
	}
}

DV_Works() {
	global
	Flagdamvul:= ""
	Loopvar:= 1
	While (Loopvar < 14) {
		If Instr(NPCdamvul, NPC_damvul[Loopvar]) {
			cbDV%Loopvar%:= 1
		Flagdamvul:= 1
		}
		Loopvar += 1
	}
}

DR_Works() {
	global
	Flagdamres:= ""
	If Instr(NPCdamres,";") {
		temparray:= StrSplit(NPCdamres, ";")
		FirstBit:= Trim(temparray[1])
		NextBit:= Trim(temparray[2])
	} else {
		FirstBit:= NPCdamres
		NextBit:= NPCdamres
	}
	Loopvar:= 1
	While (Loopvar < 11) {
		If Instr(FirstBit, NPC_damres[Loopvar]) {
			cbDR%Loopvar%:= 1
		Flagdamres:= 1
		}
		Loopvar += 1
	}

	Loopvar:= 11
	While (Loopvar < 14) {
		If Instr(NextBit, NPC_damres[Loopvar]) {
			cbDR%Loopvar%:= 1
		Flagdamres:= 1
		}
		Loopvar += 1
	}
	
	DRRadio1:= 1
	DRRadio2:= 0
	DRRadio3:= 0
	DRRadio4:= 0
	DRRadio5:= 0
	DRRadio6:= 0

	If RegExMatch(NextBit, "iU)nonmagical.*silvered") {
		DRRadio1:= 0
		DRRadio2:= 0
		DRRadio3:= 1
		DRRadio4:= 0
		DRRadio5:= 0
		DRRadio6:= 0
		NextBit:= ""
	}

	If RegExMatch(NextBit, "iU)nonmagical.*adamantine") {
		DRRadio1:= 0
		DRRadio2:= 0
		DRRadio3:= 0
		DRRadio4:= 1
		DRRadio5:= 0
		DRRadio6:= 0
		NextBit:= ""
	}

	If RegExMatch(NextBit, "iU)nonmagical.*forged iron") {
		DRRadio1:= 0
		DRRadio2:= 0
		DRRadio3:= 0
		DRRadio4:= 0
		DRRadio5:= 0
		DRRadio6:= 1
		NextBit:= ""
	}

	If RegExMatch(NextBit, "iU)nonmagical") {
		DRRadio1:= 0
		DRRadio2:= 1
		DRRadio3:= 0
		DRRadio4:= 0
		DRRadio5:= 0
		DRRadio6:= 0
		NextBit:= ""
	}

	If RegExMatch(NextBit, "iU)magic weapons") {
		DRRadio1:= 0
		DRRadio2:= 0
		DRRadio3:= 0
		DRRadio4:= 0
		DRRadio5:= 1
		DRRadio6:= 0
		NextBit:= ""
	}
	
}

DI_Works() {
	global
	Flagdamimm:= ""
	If Instr(NPCdamimm,";") {
		temparray:= StrSplit(NPCdamimm, ";")
		FirstBit:= Trim(temparray[1])
		NextBit:= Trim(temparray[2])
	} else {
		FirstBit:= NPCdamimm
		NextBit:= NPCdamimm
	}
	
	Loopvar:= 1
	While (Loopvar < 11) {
		If Instr(FirstBit, NPC_damimm[Loopvar]) {
			cbDI%Loopvar%:= 1
		Flagdamimm:= 1
		}
		Loopvar += 1
	}

	Loopvar:= 11
	While (Loopvar < 14) {
		If Instr(NextBit, NPC_damimm[Loopvar]) {
			cbDI%Loopvar%:= 1
		Flagdamimm:= 1
		}
		Loopvar += 1
	}
	
	DIRadio1:= 1
	DIRadio2:= 0
	DIRadio3:= 0
	DIRadio4:= 0
	DIRadio5:= 0

	If RegExMatch(NextBit, "iU)nonmagical.*silvered") {
		DIRadio1:= 0
		DIRadio2:= 0
		DIRadio3:= 1
		DIRadio4:= 0
		DIRadio5:= 0
		NextBit:= ""
	}

	If RegExMatch(NextBit, "iU)nonmagical.*adamantine") {
		DIRadio1:= 0
		DIRadio2:= 0
		DIRadio3:= 0
		DIRadio4:= 1
		DIRadio5:= 0
		NextBit:= ""
	}

	If RegExMatch(NextBit, "iU)nonmagical.*forged iron") {
		DIRadio1:= 0
		DIRadio2:= 0
		DIRadio3:= 0
		DIRadio4:= 0
		DIRadio5:= 1
		NextBit:= ""
	}

	If RegExMatch(NextBit, "iU)nonmagical") {
		DIRadio1:= 0
		DIRadio2:= 1
		DIRadio3:= 0
		DIRadio4:= 0
		DIRadio5:= 0
		NextBit:= ""
	}
	
}

CI_Works() {
	global
	Flagconimm:= ""
	
	Loopvar:= 1
	While (Loopvar < 16) {
		If Instr(NPCconimm, NPC_conimm[Loopvar]) {
			cbCI%Loopvar%:= 1
		Flagconimm:= 1
		}
		Loopvar += 1
	}


}

Desc_Spell_List() {
	global
	DescSpell:= ""
	If FlagInSpell {
		DescSpell:= "Innate Spellcasting." Chr(10)
		DescSpell:= DescSpell NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit
			DescSpell:= DescSpell " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks)." Chr(10)
		else
			DescSpell:= DescSpell " (spell save DC " NPCinspsave ")." Chr(10)
		DescSpell:= DescSpell GU1 " can innately cast the following spells, requiring no material components:" Chr(10)
		
		DescSpell:= DescSpell "#ts;" Chr(10)		
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			DescSpell:= DescSpell "#tr;" NPC_InSpell_Slots[A_Index] ";"
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop])
					DescSpell:= DescSpell NPC_InSpell_Names[TempLoop, A_Index] ", "
				else
					DescSpell:= DescSpell NPC_InSpell_Names[TempLoop, A_Index] Chr(10)
			}
		}
		DescSpell:= DescSpell "#te;" Chr(10)
	}
	If FlagSpell {
		DescSpell:= DescSpell "Spellcasting." Chr(10)
		DescSpell:= DescSpell NU " is a " NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		DescSpell:= DescSpell " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks)." Chr(10)
		If NPCspflavour
			DescSpell:= DescSpell NPCspflavour Chr(10)
		DescSpell:= DescSpell NU " has the following " NPCspclass " spells prepared:"Chr(10)
		
		DescSpell:= DescSpell "#ts;" Chr(10)		
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				DescSpell:= DescSpell "#tr;" NPC_Spell_Level[A_Index]
				DescSpell:= DescSpell ";(" NPC_Spell_Slots[A_Index] ");"
			} Else {
				DescSpell:= DescSpell "#tr;" NPC_Spell_Level[A_Index] " level"
				DescSpell:= DescSpell ";(" NPC_Spell_Slots[A_Index] ");"
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop])
					DescSpell:= DescSpell NPC_Spell_Names[TempLoop, A_Index] ", "
				else
					DescSpell:= DescSpell NPC_Spell_Names[TempLoop, A_Index] Chr(10)
			}
		}
		DescSpell:= DescSpell "#te;" Chr(10)
	}
}

DSpells() {
	global
	Local localNPCinsptext
	FGSpell:= ""
	If FlagInSpell {
		If NPCPsionics
			FGSpell .= "`t`t`t`t`t<p><b><i>Innate Spellcasting (Psionics).</i></b> "
		else
			FGSpell .= "`t`t`t`t`t<p><b><i>Innate Spellcasting.</i></b> "
		FGSpell .= NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit
			FGSpell .= " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks). "
		else
			FGSpell .= " (spell save DC " NPCinspsave "). "
		
		If NPCinsptext {
			localNPCinsptext:= ", " NPCinsptext
		} else {
			localNPCinsptext:= ""
		}
		
		FGSpell .= GU1 " can innately cast the following spells" localNPCinsptext ":</p>" Chr(10)
		
		FGSpell .= "`t`t`t`t`t<table class=""spell"" width=100`%>" Chr(10)
		FGSpell .= "`t`t`t`t`t`t<tr><th style=""width: 1.8cm""><b># Of Casts</b></th><th><b>Spells</b></th></tr>" Chr(10)
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			FGSpell .= "`t`t`t`t`t`t<tr><td style=""width: 1.8cm"">" NPC_InSpell_Slots[A_Index] "</td><td>"
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop])
					FGSpell .= NPC_InSpell_Names[TempLoop, A_Index] ", "
				else
					FGSpell .= NPC_InSpell_Names[TempLoop, A_Index] "</td></tr>" Chr(10)
			}
		}
		FGSpell .= "`t`t`t`t`t</table>" Chr(10)
	}
	If FlagSpell {
		FGSpell .= "`t`t`t`t`t<p><b><i>Spellcasting.</i></b> " 
		FGSpell .= NU " is a " NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		FGSpell .= " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks). "
		If NPCspflavour
			FGSpell .= NPCspflavour " "
		FGSpell .= NU " has the following " NPCspclass " spells prepared:</p>"Chr(10)
		
		FGSpell .= "`t`t`t`t`t<table class=""spell"" width=100`%>" Chr(10)		
		FGSpell .= "`t`t`t`t`t`t<tr><th style=""width: 1.8cm""><b>Spell Level</b></th><th style=""width: 1.5cm""><b>Slots</b></th><th><b>Spells</b></th></tr>" Chr(10)
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				FGSpell .= "`t`t`t`t`t`t<tr><td style=""width: 1.8cm"">" NPC_Spell_Level[A_Index]
				FGSpell .= "</td><td style=""width: 1.5cm"">" NPC_Spell_Slots[A_Index] "</td><td>"
			} Else {
				FGSpell .= "`t`t`t`t`t`t<tr><td style=""width: 1.8cm"">" NPC_Spell_Level[A_Index] " level"
				FGSpell .= "</td><td style=""width: 1.5cm"">" NPC_Spell_Slots[A_Index] "</td><td>"
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop])
					FGSpell .= NPC_Spell_Names[TempLoop, A_Index] ", "
				else
					FGSpell .= NPC_Spell_Names[TempLoop, A_Index] "</td></tr>" Chr(10)
			}
		}
		FGSpell .= "`t`t`t`t`t</table>" Chr(10)
	}
	Return FGSpell
}

FGSpells() {
	global
	local localNPCinsptext
	FGSpell:= ""
	If FlagInSpell {
		If NPCPsionics
			FGSpell .= "`t`t`t`t`t<p><b><i>Innate Spellcasting (Psionics).</i></b> "
		else
			FGSpell .= "`t`t`t`t`t<p><b><i>Innate Spellcasting.</i></b> "
		
		FGSpell .= NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit
			FGSpell .= " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks). "
		else
			FGSpell .= " (spell save DC " NPCinspsave "). "
		
		If NPCinsptext {
			localNPCinsptext:= ", " NPCinsptext
		} else {
			localNPCinsptext:= ""
		}
		
		FGSpell .= GU1 " can innately cast the following spells" localNPCinsptext ":</p>" Chr(10)
		
		FGSpell .= "`t`t`t`t`t<table>" Chr(10)
		FGSpell .= "`t`t`t`t`t`t<tr decoration=""underline""><td><b># Of Casts</b></td><td colspan=""5""><b>Spells</b></td></tr>" Chr(10)
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			FGSpell .= "`t`t`t`t`t`t<tr><td>" NPC_InSpell_Slots[A_Index] "</td><td colspan=""5"">"
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop])
					FGSpell .= NPC_InSpell_Names[TempLoop, A_Index] ", "
				else
					FGSpell .= NPC_InSpell_Names[TempLoop, A_Index] "</td></tr>" Chr(10)
			}
		}
		FGSpell .= "`t`t`t`t`t</table>" Chr(10)
	}
	If FlagSpell {
		FGSpell .= "`t`t`t`t`t<p><b><i>Spellcasting.</i></b> " 
		FGSpell .= NU " is a " NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		FGSpell .= " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks). "
		If NPCspflavour
			FGSpell .= NPCspflavour " "
		FGSpell .= NU " has the following " NPCspclass " spells prepared:</p>"Chr(10)
		
		FGSpell .= "`t`t`t`t`t<table>" Chr(10)		
		FGSpell .= "`t`t`t`t`t`t<tr decoration=""underline""><td><b>Spell Level</b></td><td><b>Slots</b></td><td colspan=""4""><b>Spells</b></td></tr>" Chr(10)
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				FGSpell .= "`t`t`t`t`t`t<tr><td>" NPC_Spell_Level[A_Index]
				FGSpell .= "</td><td>" NPC_Spell_Slots[A_Index] "</td><td colspan=""4"">"
			} Else {
				FGSpell .= "`t`t`t`t`t`t<tr><td>" NPC_Spell_Level[A_Index] " level"
				FGSpell .= "</td><td>" NPC_Spell_Slots[A_Index] "</td><td colspan=""4"">"
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop])
					FGSpell .= NPC_Spell_Names[TempLoop, A_Index] ", "
				else
					FGSpell .= NPC_Spell_Names[TempLoop, A_Index] "</td></tr>" Chr(10)
			}
		}
		FGSpell .= "`t`t`t`t`t</table>" Chr(10)
	}
}



;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |            Input/Output Functions            |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Graphical() {
	Global
	local NPCactionsX, NPCtraitsX, NPCartistetc, NPCartistframe, localNPCinsptext
	;{ Data for Statblock
	
	NPC_size_etc:= NPCsize " " NPCtype
	If NPCtag
		NPC_size_etc:= NPC_size_etc " (" NPCtag ")"
	NPC_size_2:= NPC_size_etc ", " NPCalign
	NPC_size_etc:= NPC_size_2 Chr(10)

	NPCLang:= SubStr(NPCLang, 11)

	NPCspeed:= NPCwalk " ft."
	If NPCburrow {
		NPCspeed:= NPCspeed ", burrow " NPCburrow " ft."
	}
	If NPCclimb {
		NPCspeed:= NPCspeed ", climb " NPCclimb " ft."
	}
	If NPCfly {
		NPCspeed:= NPCspeed ", fly " NPCfly " ft."
	}
	If NPChover {
		NPCspeed:= NPCspeed " (hover)"
	}
	If NPCswim {
		NPCspeed:= NPCspeed ", swim " NPCswim " ft."
	}

	NPCsave:= ""
	If (NPCstrsav or NPCdexsav or NPCconsav or NPCintsav or NPCwissav or NPCchasav or NPC_FS_STR or NPC_FS_DEX or NPC_FS_CON or NPC_FS_INT or NPC_FS_WIS or NPC_FS_CHA)	{
		
		If NPC_FS_STR {
			NPCsave:= NPCsave "Str +0, "
		} Else If (NPCstrsav < 0) {
			NPCsave:= NPCsave "Str " NPCstrsav ", "
		} Else If (NPCstrsav > 0) {
			NPCsave:= NPCsave "Str +" NPCstrsav ", "
		}
		If NPC_FS_DEX {
			NPCsave:= NPCsave "Dex +0, "
		} Else If (NPCdexsav < 0) {
			NPCsave:= NPCsave "Dex " NPCdexsav ", "
		} Else If (NPCdexsav > 0) {
			NPCsave:= NPCsave "Dex +" NPCdexsav ", "
		}
		If NPC_FS_CON {
			NPCsave:= NPCsave "Con +0, "
		} Else If (NPCconsav < 0) {
			NPCsave:= NPCsave "Con " NPCconsav ", "
		} Else If (NPCconsav > 0) {
			NPCsave:= NPCsave "Con +" NPCconsav ", "
		}
		If NPC_FS_INT {
			NPCsave:= NPCsave "Int +0, "
		} Else If (NPCintsav < 0) {
			NPCsave:= NPCsave "Int " NPCintsav ", "
		} Else If (NPCintsav > 0) {
			NPCsave:= NPCsave "Int +" NPCintsav ", "
		}
		If NPC_FS_WIS {
			NPCsave:= NPCsave "Wis +0, "
		} Else If (NPCwissav < 0) {
			NPCsave:= NPCsave "Wis " NPCwissav ", "
		} Else If (NPCwissav > 0) {
			NPCsave:= NPCsave "Wis +" NPCwissav ", "
		}
		If NPC_FS_CHA {
			NPCsave:= NPCsave "Cha +0, "
		} Else If (NPCchasav < 0) {
			NPCsave:= NPCsave "Cha " NPCchasav ", "
		} Else If (NPCchasav > 0) {
			NPCsave:= NPCsave "Cha +" NPCchasav ", "
		}
		
		StringTrimRight, NPCsave, NPCsave, 2
	}	

	NPCsense:= ""
	If (NPCblind or NPCdark or NPCtremor or NPCtrue or NPCpassperc)	{
		If NPCblindb {
			sbb:= " (blind beyond this radius)"
		} else {
			sbb:= ""
		}
		If NPCdarkb {
			sdb:= " (blind beyond this radius)"
		} else {
			sdb:= ""
		}
		If NPCtremorb {
			stb:= " (blind beyond this radius)"
		} else {
			stb:= ""
		}
		If NPCtrueb {
			szb:= " (blind beyond this radius)"
		} else {
			szb:= ""
		}
		
		If NPCblind {
			NPCsense:= NPCsense "blindsight " NPCblind " ft." sbb ", "
		}
		If NPCdark {
			NPCsense:= NPCsense "darkvision " NPCdark " ft." sdb ", "
		}
		If NPCtremor {
			NPCsense:= NPCsense "tremorsense " NPCtremor " ft." stb ", "
		}
		If NPCtrue {
			NPCsense:= NPCsense "truesight " NPCtrue " ft." szb ", "
		}
		NPCsense:= NPCsense "passive Perception " NPCpassperc
	}	

	NPCskill:= ""
	For key, value in NPC_Skills {
		If (value > 0) {
			NPCskill:= NPCskill " " key " +" value ","
		}
		If (value < 0) {
			NPCskill:= NPCskill " " key " " value ","
		}
	}
	If NPCskill {
		StringTrimRight, NPCskill, NPCskill, 1
	}

	NPCdamvul:= ""
	If Flagdamvul	{
		Loop, 10
		{
			If cbDV%A_Index% {
				NPCdamvul:= NPCdamvul NPC_damvul[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamvul, NPCdamvul, 2
		If (cbDV11 or cbDV12 or cbDV13) {
			If NPCdamvul
				NPCdamvul:= NPCdamvul "; "
			If (cbDV11 and cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning, piercing, and slashing"
			Else If (cbDV11 and cbDV12)
				NPCdamvul:= NPCdamvul "bludgeoning and piercing"
			Else If (cbDV11 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning and slashing"
			Else If (cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "piercing and slashing"
			Else If (cbDV11)
				NPCdamvul:= NPCdamvul "bludgeoning"
			Else If (cbDV12)
				NPCdamvul:= NPCdamvul "piercing"
			Else If (cbDV13)
				NPCdamvul:= NPCdamvul "slashing"
		}
	}
	
	NPCdamres:= ""
	If Flagdamres {
		Loop, 10
		{
			If cbDR%A_Index% {
				NPCdamres:= NPCdamres NPC_damres[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamres, NPCdamres, 2
		If (cbDR11 or cbDR12 or cbDR13) {
			If NPCdamres
			NPCdamres:= NPCdamres "; "
			If (cbDR11 and cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning, piercing, and slashing"
			Else If (cbDR11 and cbDR12)
				NPCdamres:= NPCdamres "bludgeoning and piercing"
			Else If (cbDR11 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning and slashing"
			Else If (cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "piercing and slashing"
			Else If (cbDR11)
				NPCdamres:= NPCdamres "bludgeoning"
			Else If (cbDR12)
				NPCdamres:= NPCdamres "piercing"
			Else If (cbDR13)
				NPCdamres:= NPCdamres "slashing"
		}
		If DRRadio1
			NPCdamres:= NPCdamres 
		If DRRadio2
			NPCdamres:= NPCdamres " from nonmagical weapons"
		If DRRadio3
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't silvered"
		If DRRadio4
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't adamantine"
		If DRRadio5
			NPCdamres:= NPCdamres " from magic weapons"
		If DRRadio6
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't cold-forged iron"
	}

	NPCdamimm:= ""
	If Flagdamimm	{
		Loop, 10
		{
			If cbDI%A_Index% {
				NPCdamimm:= NPCdamimm NPC_damimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamimm, NPCdamimm, 2
		If (cbDI11 or cbDI12 or cbDI13) {
			If NPCdamimm
				NPCdamimm:= NPCdamimm "; "
			If (cbDI11 and cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning, piercing, and slashing"
			Else If (cbDI11 and cbDI12)
				NPCdamimm:= NPCdamimm "bludgeoning and piercing"
			Else If (cbDI11 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning and slashing"
			Else If (cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "piercing and slashing"
			Else If (cbDI11)
				NPCdamimm:= NPCdamimm "bludgeoning"
			Else If (cbDI12)
				NPCdamimm:= NPCdamimm "piercing"
			Else If (cbDI13)
				NPCdamimm:= NPCdamimm "slashing"
		}
		If DIRadio1
			NPCdamimm:= NPCdamimm
		If DIRadio2
			NPCdamimm:= NPCdamimm " from nonmagical weapons"
		If DIRadio3
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't silvered"
		If DIRadio4
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't adamantine"
		If DIRadio5
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't cold-forged iron"
	}

	NPCconimm:= ""
	If Flagconimm	{
		Loop, 16
		{
			If cbCI%A_Index% {
				NPCconimm:= NPCconimm NPC_conimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCconimm, NPCconimm, 2
	}	


	NPCactionsH:= ""
	NPCactionsX:= ""
	If FlagActions	{
		Loop % NPC_Actions.Length()
		{
			NPCactionsH .= "<b><i>" NPC_Actions[A_Index, "Name"] ". </i></b>" NPC_Actions[A_Index, "Action"] "<BR><BR>"
			NPCactionsX .= "<b><i>&#13;" NPC_Actions[A_Index, "Name"] ". </i></b>" NPC_Actions[A_Index, "Action"]
		}
		StringTrimRight, NPCactionsH, NPCactionsH, 8
		StringReplace, NPCactionsH, NPCactionsH, Hit:, <i>Hit: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Melee Weapon Attack:, <i>Melee Weapon Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Ranged Weapon Attack:, <i>Ranged Weapon Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Melee or <i>Ranged Weapon Attack: </i>, <i>Melee or Ranged Weapon Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Melee Spell Attack:, <i>Melee Spell Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Ranged Spell Attack:, <i>Ranged Spell Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Melee or <i>Ranged Spell Attack: </i>, <i>Melee or Ranged Spell Attack: </i>, all
		
		StringReplace, NPCactionsX, NPCactionsX, Hit:, <i>Hit: </i>, all
		StringReplace, NPCactionsX, NPCactionsX, Melee Weapon Attack:, <i>Melee Weapon Attack: </i>, all
		StringReplace, NPCactionsX, NPCactionsX, Ranged Weapon Attack:, <i>Ranged Weapon Attack: </i>, all
		StringReplace, NPCactionsX, NPCactionsX, Melee or <i>Ranged Weapon Attack: </i>, <i>Melee or Ranged Weapon Attack: </i>, all
		StringReplace, NPCactionsX, NPCactionsX, Melee Spell Attack:, <i>Melee Spell Attack: </i>, all
		StringReplace, NPCactionsX, NPCactionsX, Ranged Spell Attack:, <i>Ranged Spell Attack: </i>, all
		StringReplace, NPCactionsX, NPCactionsX, Melee or <i>Ranged Spell Attack: </i>, <i>Melee or Ranged Spell Attack: </i>, all
	}	

	NPCreactionsH:= ""
	NPCreactionsX:= ""
	If FlagReactions	{
		Loop % NPC_Reactions.Length()
		{
			NPCreactionsH .= "<b><i>" NPC_Reactions[A_Index, "Name"] ". </i></b>" NPC_Reactions[A_Index, "Reaction"] "<BR><BR>"
			NPCreactionsX .= "<b><i>&#13;" NPC_Reactions[A_Index, "Name"] ". </i></b>" NPC_Reactions[A_Index, "Reaction"]
		}
		StringTrimRight, NPCreactionsH, NPCreactionsH, 8
	}

	NPClegactionsH:= ""
	NPClegactionsX:= ""
	If FlagLegActions	{
		Loop % NPC_Legendary_Actions.Length()
		{
			NPClegactionsH .= "<b><i>" NPC_Legendary_Actions[A_Index, "Name"] ". </i></b>" NPC_Legendary_Actions[A_Index, "LegAction"] "<BR><BR>"
			NPClegactionsX .= "<b><i>&#13;" NPC_Legendary_Actions[A_Index, "Name"] ". </i></b>" NPC_Legendary_Actions[A_Index, "LegAction"]
		}
		StringTrimRight, NPClegactionsH, NPClegactionsH, 8
	}	

	NPClairactionsH:= ""
	NPClairactionsX:= ""
	If FlagLairActions	{
		Loop % NPC_Lair_Actions.Length()
		{
			NPClairactionsH .= "<b><i>" NPC_Lair_Actions[A_Index, "Name"] ". </i></b>" NPC_Lair_Actions[A_Index, "LairAction"] "<BR><BR>"
			NPClairactionsX .= "<b><i>&#13;" NPC_Lair_Actions[A_Index, "Name"] ". </i></b>" NPC_Lair_Actions[A_Index, "LairAction"]
		}
		StringTrimRight, NPClairactionsH, NPClairactionsH, 8
	}	

	NPCtraitsH:= ""
	NPCtraitsX:= ""
	If FlagTraits	{
		For key, value in NPC_Traits
		{
			NPCtraitsH .= "<b><i>" NPC_Traits[key, "Name"] ". </i></b>" NPC_Traits[key, "trait"] "<BR><BR>"
			NPCtraitsX .= "<b><i>&#13;" NPC_Traits[key, "Name"] ". </i></b>" NPC_Traits[key, "trait"]
		}
		StringTrimRight, NPCtraitsH, NPCtraitsH, 8
		StringReplace NPCtraitsX, NPCtraitsX, &#13`;
	}	

	NPCinspellH:= ""
	NPCinspellX:= ""
	If FlagInSpell {
		If NPCPsionics {
			NPCinspellH:= "<BR><b><i>Innate Spellcasting (Psionics). </i></b>"
			NPCinspellX:= "<b><i>&#13;Innate Spellcasting (Psionics). </i></b>"
		} Else {
			NPCinspellH:= "<BR><b><i>Innate Spellcasting. </i></b>"
			NPCinspellX:= "<b><i>&#13;Innate Spellcasting. </i></b>"
		}
		NPCinspellH:= NPCinspellH NU "'s innate spellcasting ability is " NPCinspability
		NPCinspellX:= NPCinspellX NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit {
			NPCinspellH:= NPCinspellH " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks)."
			NPCinspellX:= NPCinspellX " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks)."
		} else {
			NPCinspellH:= NPCinspellH " (spell save DC " NPCinspsave ")."
			NPCinspellX:= NPCinspellX " (spell save DC " NPCinspsave ")."
		}
		If NPCinsptext {
			localNPCinsptext:= ", " NPCinsptext
		} else {
			localNPCinsptext:= ""
		}
		NPCinspellH:= NPCinspellH " " GU1 " can innately cast the following spells" localNPCinsptext ":"
		NPCinspellX:= NPCinspellX " " GU1 " can innately cast the following spells" localNPCinsptext ":"
		
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			NPCinspellH:= NPCinspellH "<BR>" NPC_InSpell_Slots[A_Index] ": <i>"
			NPCinspellX:= NPCinspellX "&#13;" NPC_InSpell_Slots[A_Index] ": <i>"
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop]) {
					NPCinspellH:= NPCinspellH NPC_InSpell_Names[TempLoop, A_Index] ", "
					NPCinspellX:= NPCinspellX NPC_InSpell_Names[TempLoop, A_Index] ", "
				} else {
					NPCinspellH:= NPCinspellH NPC_InSpell_Names[TempLoop, A_Index] " </i>"
					NPCinspellX:= NPCinspellX NPC_InSpell_Names[TempLoop, A_Index] " </i>"
				}
			}
		}
		NPCinspellH:= NPCinspellH "<BR>"
		NPCinspellX:= NPCinspellX "&#13;"
	}

	NPCspellH:= ""
	NPCspellX:= ""
	If FlagSpell {
		If (NPCsplevel = "8th") or (NPCsplevel = "11th") or (NPCsplevel = "18th") {
			splpronoun:= "an "
		} else {
			splpronoun:= "a "
		}
		NPCspellH:= "<BR><b><i>Spellcasting. </i></b>"
		NPCspellX:= "<b><i>&#13;Spellcasting. </i></b>"
		NPCspellH:= NPCspellH NU " is " splpronoun NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		NPCspellH:= NPCspellH " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks)."
		If NPCspflavour
			NPCspellH:= NPCspellH " " NPCspflavour
		NPCspellH:= NPCspellH " " NU " has the following " NPCspclass " spells prepared:"
		npcspellX:= npcspellX NU " is " splpronoun NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		npcspellX:= npcspellX " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks)."
		If NPCspflavour
			npcspellX:= npcspellX " " NPCspflavour
		npcspellX:= npcspellX " " NU " has the following " NPCspclass " spells prepared:"
		
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				NPCspellH:= NPCspellH "<BR>" NPC_Spell_Level[A_Index]
				NPCspellH:= NPCspellH " (" NPC_Spell_Slots[A_Index] "): <i>"
				npcspellX:= npcspellX "&#13;" NPC_Spell_Level[A_Index]
				npcspellX:= npcspellX " (" NPC_Spell_Slots[A_Index] "): <i>"
			} Else {
				NPCspellH:= NPCspellH "<BR>" NPC_Spell_Level[A_Index] " level"
				NPCspellH:= NPCspellH " (" NPC_Spell_Slots[A_Index] "): <i>"
				npcspellX:= npcspellX "&#13;" NPC_Spell_Level[A_Index] " level"
				npcspellX:= npcspellX " (" NPC_Spell_Slots[A_Index] "): <i>"
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop]) {
					NPCspellH:= NPCspellH NPC_Spell_Names[TempLoop, A_Index] ", "
					npcspellX:= npcspellX NPC_Spell_Names[TempLoop, A_Index] ", "
				} else {
					NPCspellH:= NPCspellH NPC_Spell_Names[TempLoop, A_Index] " </i>"
					npcspellX:= npcspellX NPC_Spell_Names[TempLoop, A_Index] " </i>"
				}
			}
		}
		If NPCSpellStar {
			NPCspellH:= NPCspellH "<BR><BR>"
			NPCspellH:= NPCspellH NPCSpellStar
			npcspellX:= npcspellX "&#13;&#13;"
			npcspellX:= npcspellX NPCSpellStar
		}
		NPCspellH:= NPCspellH "<BR>"
		npcspellX:= npcspellX "&#13;"
	}

	
	NPCdescript:= ""
	
	If Desc_Add_Text {
	speech:= "<img src=""" imgdir("speech.png") """ align=""center"" alt=""Shield"">"
	frtable =
	(
		</div>
		<div class="post-container">
			<div class="post-thumb">%speech%</div>
			<div class="post-content">
	)
	frtablend =
	(
		</div>
		</div>
		<div class="npcdescrip">
	)
	
		NPCdescript .= "<p>"
		If Desc_NPC_Title {
			NPCdescript .=  "<span class=""heading"">" NPCname "</span><br>"
		}
		If (Desc_Image_Link and NPCname) {
			NPCdescript .=  shield " Image: " NPCname "</p>"
		} else {
			NPCdescript .= "</p>"
		}
		NPCdescript .= "<p>"
		temptext:= tokenise(RE1.GetRTF(False))
		StringReplace, temptext, temptext, <frame>, %frtable%, all
		StringReplace, temptext, temptext, </frame>, %frtablend%, all
		StringReplace, temptext, temptext, <h>, <span class=`"heading`">, all
		StringReplace, temptext, temptext, </h>, </span>, all
		
		temptext:= LinkHTML(temptext)
		 
		NPCdescript .= temptext
		If (Desc_Spell_List) {
			Dspell:= DSpells()
			NPCdescript .= Dspell
		}
	}

	NPCartistetc:= ""
	NPCartistframe:= ""
	If NPCImArt
		NPCartistetc .= NpcArtPref " <b>" NPCImArt "</b>&#13;"
	If NPCImLink
		NPCartistetc .= "<link class=""url"" recordname=""" NPCImLink """>Website link</link>"
	If NPCartistetc
		NPCartistframe:= "text5"

	If NPCartistetc
		RMInfo:= "<h>&#13;&#13;" modname "</h>"
	else
		RMInfo:= "<h>" modname "</h>"
	;}

	#include HTML_NPC_Engineer.ahk
	
	;{ Building HTML_Stat_Block
	HTML_Stat_Block:= css . htmtop

	If NPCsave
		HTML_Stat_Block .= htmsave
	If NPCskill
		HTML_Stat_Block .= htmskill 
	If NPCdamvul
		HTML_Stat_Block .= htmdamvul 
	If NPCdamres
		HTML_Stat_Block .= htmdamres 
	If NPCdamimm
		HTML_Stat_Block .= htmdamimm 
	If NPCconimm
		HTML_Stat_Block .= htmconimm
	
	HTML_Stat_Block .= htmmiddle

	HTML_Stat_Block .= htmtraits

	If NPCActionsH
		HTML_Stat_Block .= htmactions
	If NPCReactionsH
		HTML_Stat_Block .= htmreactions
	If NPCLegactionsH
		HTML_Stat_Block .= htmlegactions
	If NPCLairactionsH
		HTML_Stat_Block .= htmlairactions
	
	HTML_Stat_Block .= htmbottom
	;~ HTML_Stat_Block .= "<div><img src=""" shield """ alt=""Shield""></div>"
	HTML_Stat_Block .= htmdesc htmend
	;}
	stringreplace, HTML_Stat_Block, HTML_Stat_Block, \r, <br>, All

	documentz := ViewPort.Document
	documentz.open()
	documentz.write(HTML_Stat_Block)
	documentz.close()
	ViewPort.Document.parentWindow.eval("scrollTo(0, " ScrollPoint ");")


	#include RefMan_NPC_Engineer.ahk


	;{ Building XML_Stat_Block
	XML_Stat_Block:= RM_NPCTop Chr(10)
	If Desc_Image_Link {
		If (NPCImagePath and NPCjpeg) {
			Ifexist, %NPCImagePath%
			{
				XML_Stat_Block .= RM_NPCPic
			}
		}
	}
	XML_Stat_Block .= RM_NPCTop2

	If NPCsave
		XML_Stat_Block .= RMsave
	If NPCskill
		XML_Stat_Block .= RMskill 
	If NPCdamvul
		XML_Stat_Block .= RMdamvul 
	If NPCdamres
		XML_Stat_Block .= RMdamres 
	If NPCdamimm
		XML_Stat_Block .= RMdamimm 
	If NPCconimm
		XML_Stat_Block .= RMconimm
	
	XML_Stat_Block .= RMmiddle

	If FlagTraits or FlagSpell or FlagInSpell
		XML_Stat_Block .= RMtraits

	If NPCActionsX
		XML_Stat_Block .= RMactions
	If NPCReactionsX
		XML_Stat_Block .= RMreactions
	If NPCLegactionsX
		XML_Stat_Block .= RMlegactions
	If NPCLairactionsX
		XML_Stat_Block .= RMlairactions

	XML_Stat_Block .= "`t`t`t`t`t`t`t`t`t`t" RM_NPCend
	If Desc_Image_Link
		XML_Stat_Block .= "`t`t`t`t`t`t`t`t`t`t" RM_NPCend2
	
	XML_Stat_Block .= "`t`t`t`t`t`t`t`t`t`t" RM_NPCend3
	
	If NPCartistetc
		XML_Stat_Block .= RM_NPCartist
	else
		XML_Stat_Block .= RM_NPCartist2

	XML_Stat_Block .= "`t`t`t`t`t`t`t`t`t`t" RM_NPCbase

	;}
}

GraphicalRTF(win) {
	Global
	colourtable:= "{\colortbl `;\red0\green0\blue0`;\red122\green32\blue13`;\red253\green241\blue220`;\red230\green154\blue40`;\red255\green255\blue255`;\red213\green201\blue180`;\red146\green38\blue16`;}"
	fonttable:= "{\fonttbl {\f0 Century Gothic`;}{\f1 Mr. Eaves Small Caps`;}{\f2 Calibri`;}"

	TrowTop := "\trowd\trrh-75"
	TrowTop .= "\clcbpat4\clbrdrt\brdrw1\brdrcf1\clbrdrb\brdrw1\brdrcf1\clbrdrl\brdrw1\brdrcf1\clbrdrr\brdrw1\brdrcf1\cellx6860"
	TrowTop .= " \intbl\cell\row"
	TrowDiv := "\trowd\trrh-60"
	TrowDiv .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\cellx100"
	TrowDiv .= "\clcbpat7\clbrdrt\brdrw1\brdrcf7\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\clbrdrr\brdrw1\brdrcf3\cellx6760"
	TrowDiv .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\cellx6860"
	TrowDiv .= " \intbl\cell\row"
	TrowDiv2 := "\trowd\trrh-60"
	TrowDiv2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\cellx100"
	TrowDiv2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf2\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\clbrdrr\brdrw1\brdrcf3\cellx6760"
	TrowDiv2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\cellx6860"
	TrowDiv2 .= " \intbl\cell\row"
	Tspace1 := "\trowd\trrh-80"
	Tspace1 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf1\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf6\clbrdrr\brdrw1\brdrcf6\cellx6860"
	Tspace1 .= " \intbl\cell\row"
	Tspace2 := "\trowd\trrh-100"
	Tspace2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf6\clbrdrr\brdrw1\brdrcf6\cellx6860"
	Tspace2 .= " \intbl\cell\row"
	Tspace3 := "\trowd\trrh-80"
	Tspace3 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf6\clbrdrr\brdrw1\brdrcf6\cellx6860"
	Tspace3 .= " \intbl\cell\row"
	Tspace4 := "\trowd\trrh-80"
	Tspace4 .= "\clcbpat5\clbrdrt\brdrw1\brdrcf1\clbrdrb\brdrw1\brdrcf5\clbrdrl\brdrw1\brdrcf5\clbrdrr\brdrw1\brdrcf5\cellx6860"
	Tspace4 .= " \intbl\cell\row"
	
	trow1 := "\trowd\trgaph100"
	trow1 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf6\clbrdrr\brdrw1\brdrcf6\cellx6860"
	
	trend:= " \intbl\cell\row"
	
	trow2 := "\trowd"
	trow2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf6\cellx100"
	trow2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\clbrdrr\brdrw1\brdrcf3\cellx1210"
	trow2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\clbrdrr\brdrw1\brdrcf3\cellx2320"
	trow2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\clbrdrr\brdrw1\brdrcf3\cellx3430"
	trow2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\clbrdrr\brdrw1\brdrcf3\cellx4540"
	trow2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\clbrdrr\brdrw1\brdrcf3\cellx5650"
	trow2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrb\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\clbrdrr\brdrw1\brdrcf3\cellx6760"
	trow2 .= "\clcbpat3\clbrdrt\brdrw1\brdrcf3\clbrdrl\brdrw1\brdrcf3\clbrdrr\brdrw1\brdrcf6\cellx6860"

	trow3 := "\trowd\trgaph100"
	trow3 .= "\clcbpat5\clbrdrt\brdrw1\brdrcf5\clbrdrb\brdrw1\brdrcf5\clbrdrl\brdrw1\brdrcf5\clbrdrr\brdrw1\brdrcf5\cellx6860"

	
	;{ Data for Statblock
	
	NPC_size_etc:= NPCsize " " NPCtype
	If NPCtag
		NPC_size_etc:= NPC_size_etc " (" NPCtag ")"
	NPC_size_etc:= NPC_size_etc ", " NPCalign Chr(10)

	Lang_Set()
	NPCLang:= SubStr(NPCLang, 11)
	
	NPCspeed:= NPCwalk " ft."
	If NPCburrow {
		NPCspeed:= NPCspeed ", burrow " NPCburrow " ft."
	}
	If NPCclimb {
		NPCspeed:= NPCspeed ", climb " NPCclimb " ft."
	}
	If NPCfly {
		NPCspeed:= NPCspeed ", fly " NPCfly " ft."
	}
	If NPChover {
		NPCspeed:= NPCspeed " (hover)"
	}
	If NPCswim {
		NPCspeed:= NPCspeed ", swim " NPCswim " ft."
	}

	NPCsave:= ""
	If (NPCstrsav or NPCdexsav or NPCconsav or NPCintsav or NPCwissav or NPCchasav or NPC_FS_STR or NPC_FS_DEX or NPC_FS_CON or NPC_FS_INT or NPC_FS_WIS or NPC_FS_CHA)	{
		
		If NPC_FS_STR {
			NPCsave:= NPCsave "Str +0, "
		} Else If (NPCstrsav < 0) {
			NPCsave:= NPCsave "Str " NPCstrsav ", "
		} Else If (NPCstrsav > 0) {
			NPCsave:= NPCsave "Str +" NPCstrsav ", "
		}
		If NPC_FS_DEX {
			NPCsave:= NPCsave "Dex +0, "
		} Else If (NPCdexsav < 0) {
			NPCsave:= NPCsave "Dex " NPCdexsav ", "
		} Else If (NPCdexsav > 0) {
			NPCsave:= NPCsave "Dex +" NPCdexsav ", "
		}
		If NPC_FS_CON {
			NPCsave:= NPCsave "Con +0, "
		} Else If (NPCconsav < 0) {
			NPCsave:= NPCsave "Con " NPCconsav ", "
		} Else If (NPCconsav > 0) {
			NPCsave:= NPCsave "Con +" NPCconsav ", "
		}
		If NPC_FS_INT {
			NPCsave:= NPCsave "Int +0, "
		} Else If (NPCintsav < 0) {
			NPCsave:= NPCsave "Int " NPCintsav ", "
		} Else If (NPCintsav > 0) {
			NPCsave:= NPCsave "Int +" NPCintsav ", "
		}
		If NPC_FS_WIS {
			NPCsave:= NPCsave "Wis +0, "
		} Else If (NPCwissav < 0) {
			NPCsave:= NPCsave "Wis " NPCwissav ", "
		} Else If (NPCwissav > 0) {
			NPCsave:= NPCsave "Wis +" NPCwissav ", "
		}
		If NPC_FS_CHA {
			NPCsave:= NPCsave "Cha +0, "
		} Else If (NPCchasav < 0) {
			NPCsave:= NPCsave "Cha " NPCchasav ", "
		} Else If (NPCchasav > 0) {
			NPCsave:= NPCsave "Cha +" NPCchasav ", "
		}
		
		StringTrimRight, NPCsave, NPCsave, 2
	}	

	NPCsense:= ""
	If (NPCblind or NPCdark or NPCtremor or NPCtrue or NPCpassperc)	{
		If NPCblindb {
			sbb:= " (blind beyond this radius)"
		} else {
			sbb:= ""
		}
		If NPCdarkb {
			sdb:= " (blind beyond this radius)"
		} else {
			sdb:= ""
		}
		If NPCtremorb {
			stb:= " (blind beyond this radius)"
		} else {
			stb:= ""
		}
		If NPCtrueb {
			szb:= " (blind beyond this radius)"
		} else {
			szb:= ""
		}
		
		If NPCblind {
			NPCsense:= NPCsense "blindsight " NPCblind " ft." sbb ", "
		}
		If NPCdark {
			NPCsense:= NPCsense "darkvision " NPCdark " ft." sdb ", "
		}
		If NPCtremor {
			NPCsense:= NPCsense "tremorsense " NPCtremor " ft." stb ", "
		}
		If NPCtrue {
			NPCsense:= NPCsense "truesight " NPCtrue " ft." szb ", "
		}
		NPCsense:= NPCsense "passive Perception " NPCpassperc
	}	

	NPCskill:= ""
	For key, value in NPC_Skills {
		If (value > 0) {
			NPCskill:= NPCskill " " key " +" value ","
		}
		If (value < 0) {
			NPCskill:= NPCskill " " key " " value ","
		}
	}
	If NPCskill {
		StringTrimRight, NPCskill, NPCskill, 1
	}

	NPCdamvul:= ""
	If Flagdamvul	{
		Loop, 10
		{
			If cbDV%A_Index% {
				NPCdamvul:= NPCdamvul NPC_damvul[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamvul, NPCdamvul, 2
		If (cbDV11 or cbDV12 or cbDV13) {
			If NPCdamvul
				NPCdamvul:= NPCdamvul "; "
			If (cbDV11 and cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning, piercing, and slashing"
			Else If (cbDV11 and cbDV12)
				NPCdamvul:= NPCdamvul "bludgeoning and piercing"
			Else If (cbDV11 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning and slashing"
			Else If (cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "piercing and slashing"
			Else If (cbDV11)
				NPCdamvul:= NPCdamvul "bludgeoning"
			Else If (cbDV12)
				NPCdamvul:= NPCdamvul "piercing"
			Else If (cbDV13)
				NPCdamvul:= NPCdamvul "slashing"
		}
	}
	
	NPCdamres:= ""
	If Flagdamres {
		Loop, 10
		{
			If cbDR%A_Index% {
				NPCdamres:= NPCdamres NPC_damres[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamres, NPCdamres, 2
		If (cbDR11 or cbDR12 or cbDR13) {
			If NPCdamres
			NPCdamres:= NPCdamres "; "
			If (cbDR11 and cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning, piercing, and slashing"
			Else If (cbDR11 and cbDR12)
				NPCdamres:= NPCdamres "bludgeoning and piercing"
			Else If (cbDR11 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning and slashing"
			Else If (cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "piercing and slashing"
			Else If (cbDR11)
				NPCdamres:= NPCdamres "bludgeoning"
			Else If (cbDR12)
				NPCdamres:= NPCdamres "piercing"
			Else If (cbDR13)
				NPCdamres:= NPCdamres "slashing"
		}
		If DRRadio1
			NPCdamres:= NPCdamres 
		If DRRadio2
			NPCdamres:= NPCdamres " from nonmagical weapons"
		If DRRadio3
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't silvered"
		If DRRadio4
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't adamantine"
		If DRRadio5
			NPCdamres:= NPCdamres " from magic weapons"
		If DRRadio6
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't cold-forged iron"
	}

	NPCdamimm:= ""
	If Flagdamimm	{
		Loop, 10
		{
			If cbDI%A_Index% {
				NPCdamimm:= NPCdamimm NPC_damimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamimm, NPCdamimm, 2
		If (cbDI11 or cbDI12 or cbDI13) {
			If NPCdamimm
				NPCdamimm:= NPCdamimm "; "
			If (cbDI11 and cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning, piercing, and slashing"
			Else If (cbDI11 and cbDI12)
				NPCdamimm:= NPCdamimm "bludgeoning and piercing"
			Else If (cbDI11 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning and slashing"
			Else If (cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "piercing and slashing"
			Else If (cbDI11)
				NPCdamimm:= NPCdamimm "bludgeoning"
			Else If (cbDI12)
				NPCdamimm:= NPCdamimm "piercing"
			Else If (cbDI13)
				NPCdamimm:= NPCdamimm "slashing"
		}
		If DIRadio1
			NPCdamimm:= NPCdamimm
		If DIRadio2
			NPCdamimm:= NPCdamimm " from nonmagical weapons"
		If DIRadio3
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't silvered"
		If DIRadio4
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't adamantine"
		If DIRadio5
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't cold-forged iron"
	}

	NPCconimm:= ""
	If Flagconimm	{
		Loop, 16
		{
			If cbCI%A_Index% {
				NPCconimm:= NPCconimm NPC_conimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCconimm, NPCconimm, 2
	}	


	NPCactions:= ""
	If FlagActions	{
		Loop % NPC_Actions.Length()
			NPCactions .= "\b\i " NPC_Actions[A_Index, "Name"] ". \b0\i0 " NPC_Actions[A_Index, "Action"] "\line\line"
		StringTrimRight, NPCactions, NPCactions, 5
		StringReplace, NPCactions, NPCactions, Hit:, \i Hit: \i0, all
		StringReplace, NPCactions, NPCactions, Melee Weapon Attack:, \i Melee Weapon Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Ranged Weapon Attack:, \i Ranged Weapon Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Melee or \i Ranged Weapon Attack: \i0, \i Melee or Ranged Weapon Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Melee Spell Attack:, \i Melee Spell Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Ranged Spell Attack:, \i Ranged Spell Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Melee or \i Ranged Spell Attack: \i0, \i Melee or Ranged Spell Attack: \i0, all
	}	
	stringreplace, NPCactions, NPCactions, \r, \line%A_Space%, All

	NPCreactions:= ""
	If FlagReactions	{
		Loop % NPC_Reactions.Length()
			NPCreactions .= "\b\i " NPC_Reactions[A_Index, "Name"] ". \b0\i0 " NPC_Reactions[A_Index, "Reaction"] "\line\line"
		StringTrimRight, NPCreactions, NPCreactions, 5
	}
	stringreplace, NPCreactions, NPCreactions, \r, \line%A_Space%, All

	NPClegactions:= ""
	If FlagLegActions	{
		Loop % NPC_Legendary_Actions.Length()
			NPClegactions .= "\b\i " NPC_Legendary_Actions[A_Index, "Name"] ". \b0\i0 " NPC_Legendary_Actions[A_Index, "LegAction"] "\line\line"
		StringTrimRight, NPClegactions, NPClegactions, 5
	}	
	stringreplace, NPClegactions, NPClegactions, \r, \line%A_Space%, All

	NPClairactions:= ""
	If FlagLairActions	{
		Loop % NPC_Lair_Actions.Length()
			NPClairactions .= "\b\i " NPC_Lair_Actions[A_Index, "Name"] ". \b0\i0 " NPC_Lair_Actions[A_Index, "LairAction"] "\line\line"
		StringTrimRight, NPClairactions, NPClairactions, 5
	}	
	stringreplace, NPClairactions, NPClairactions, \r, \line%A_Space%, All

	NPCtraits:= ""
	If FlagTraits {
		For key, value in NPC_Traits
			NPCtraits .= "\b\i " NPC_Traits[key, "Name"] ". \b0\i0 " NPC_Traits[key, "trait"] "\line\line"
		StringTrimRight, NPCtraits, NPCtraits, 5
		stringreplace, NPCtraits, NPCtraits, \r, \line%A_Space%, All
	}	

	NPCinspell:= ""
	If FlagInSpell {
		If NPCPsionics {
			if FlagTraits
				NPCinspell:= "\line\b\i Innate Spellcasting (Psionics). \b0\i0 "
			else
				NPCinspell:= "\b\i Innate Spellcasting (Psionics). \b0\i0 "
		} Else {
			if FlagTraits
				NPCinspell:= "\line\b\i Innate Spellcasting. \b0\i0 "
			else
				NPCinspell:= "\b\i Innate Spellcasting. \b0\i0 "
		}

		NPCinspell:= NPCinspell NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks)."
		else
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ")."
		If NPCinsptext {
			localNPCinsptext:= ", " NPCinsptext
		} else {
			localNPCinsptext:= ""
		}
		NPCinspell:= NPCinspell " " GU1 " can innately cast the following spells" localNPCinsptext ":"
		
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			NPCinspell:= NPCinspell "\line " NPC_InSpell_Slots[A_Index] ": \i "
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop])
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index] ", "
				else
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index] " \i0 "
			}
		}
		NPCinspell:= NPCinspell "\line "
	}

	NPCspell:= ""
	If FlagSpell {
		If (NPCsplevel = "8th") or (NPCsplevel = "11th") or (NPCsplevel = "18th") {
			splpronoun:= "an "
		} else {
			splpronoun:= "a "
		}
		If FlagTraits or FlagInSpell
			NPCspell:= "\line\b\i Spellcasting. \b0\i0 "
		else
			NPCspell:= "\b\i Spellcasting. \b0\i0 "
			
		NPCspell:= NPCspell NU " is " splpronoun NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		NPCspell:= NPCspell " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks)."
		If NPCspflavour
			NPCspell:= NPCspell " " NPCspflavour
		NPCspell:= NPCspell " " NU " has the following " NPCspclass " spells prepared:"
		
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				NPCspell:= NPCspell "\line " NPC_Spell_Level[A_Index]
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): \i "
			} Else {
				NPCspell:= NPCspell "\line " NPC_Spell_Level[A_Index] " level"
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): \i "
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop])
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index] ", "
				else
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index] " \i0 "
			}
		}
		If NPCSpellStar {
			NPCspell:= NPCspell "\line \line "
			NPCspell:= NPCspell NPCSpellStar
		}
		NPCspell:= NPCspell "\line "
	}

	;}
	
	Statblock := TrowTop Tspace1
	Statblock .= Trow1 "\f1\cf2\fs42\b " NPCName " \b0 \line"
	Statblock .= "\f0\cf1\fs18\i " NPC_size_etc " \i0" Trend 
	Statblock .= Tspace2 TrowDiv Tspace3
	Statblock .= Trow1 "\f0\cf2\fs21\b Armor Class \b0 " NPCac "\line" 
	Statblock .= "\b Hit Points \b0 " NPChp "\line" 
	Statblock .= "\b Speed \b0 " NPCspeed trend
	Statblock .= Tspace2 TrowDiv Tspace3

	Statblock .= Trow2 "\f0\cf2\fs21\b\qc " " \intbl\cell STR \intbl\cell DEX \intbl\cell CON \intbl\cell INT \intbl\cell WIS \intbl\cell CHA \intbl\cell\row\ql\b0"
	Statblock .= Trow2 "\f0\cf2\fs21\qc " " \intbl\cell"
	Statblock .= " " NPCstr " (" NPCstrbo ") \intbl\cell"
	Statblock .= " " NPCdex " (" NPCdexbo ") \intbl\cell"
	Statblock .= " " NPCcon " (" NPCconbo ") \intbl\cell"
	Statblock .= " " NPCint " (" NPCintbo ") \intbl\cell"
	Statblock .= " " NPCwis " (" NPCwisbo ") \intbl\cell"
	Statblock .= " " NPCcha " (" NPCchabo ") \intbl\cell\row\ql"

	Statblock .= Tspace2 TrowDiv Tspace3
	Statblock .= Trow1 "\f0\cf2\fs21"
	If NPCsave
		Statblock .= "\b Saving Throws \b0 " NPCsave "\line"
	If NPCskill
		Statblock .= "\b Skills \b0 " NPCskill "\line" 
	If NPCdamvul
		Statblock .= "\b Damage Vulnerabilities \b0 " NPCdamvul "\line" 
	If NPCdamres
		Statblock .= "\b Damage Resistances \b0 " NPCdamres "\line" 
	If NPCdamimm
		Statblock .= "\b Damage Immunities \b0 " NPCdamimm "\line" 
	If NPCconimm
		Statblock .= "\b Condition Immunities \b0 " NPCconimm "\line" 
	Statblock .= "\b Senses \b0 " NPCsense "\line" 
	Statblock .= "\b Languages \b0 " NPClang "\line" 
	Statblock .= "\b Challenge \b0 " NPCcharat " (" NPCxp " XP)" trend
	Statblock .= Tspace2 TrowDiv Tspace3
	
	Statblock .= Trow1 "\f0\cf1\fs20" NPCTraits NPCinspell NPCspell trend

	If NPCActions {
		Statblock .= Trow1 "\f0\cf2\fs34 A\fs24 CTIONS" trend TrowDiv2 Tspace3
		Statblock .= Trow1 "\f0\cf1\fs20" NPCActions trend
	}

	If NPCReactions {
		Statblock .= Trow1 "\f0\cf2\fs34 R\fs24 EACTIONS" trend TrowDiv2 Tspace3
		Statblock .= Trow1 "\f0\cf1\fs20" NPCreactions trend
	}

	If NPCLegactions {
		Statblock .= Trow1 "\f0\cf2\fs34 L\fs24 EGENDARY \fs34 A\fs24 CTIONS" trend TrowDiv2 Tspace3
		Statblock .= Trow1 "\f0\cf1\fs20" NPClegactions trend
	}

	If NPCLairactions {
		Statblock .= Trow1 "\f0\cf2\fs34 L\fs24 AIR \fs34 A\fs24 CTIONS" trend TrowDiv2 Tspace3
		Statblock .= Trow1 "\f0\cf1\fs20" NPClairactions trend
	}

	Statblock .= TrowTop Tspace4 Trow3 trend



	%win%.SetText("{\rtf1\ansi\deff0 " fonttable "}" colourtable statblock "}", ["KEEPUNDO"])
}

ExportFormats() {
	Global
	
	;{ Data for Statblock
	
	NPC_size_etc:= NPCsize " " NPCtype
	If NPCtag
		NPC_size_etc:= NPC_size_etc " (" NPCtag ")"
	NPC_size_etc:= NPC_size_etc ", " NPCalign Chr(10)

	Lang_Set()
	NPCLang:= SubStr(NPCLang, 11)
	
	NPCspeed:= NPCwalk " ft."
	If NPCburrow {
		NPCspeed:= NPCspeed ", burrow " NPCburrow " ft."
	}
	If NPCclimb {
		NPCspeed:= NPCspeed ", climb " NPCclimb " ft."
	}
	If NPCfly {
		NPCspeed:= NPCspeed ", fly " NPCfly " ft."
	}
	If NPChover {
		NPCspeed:= NPCspeed " (hover)"
	}
	If NPCswim {
		NPCspeed:= NPCspeed ", swim " NPCswim " ft."
	}

	NPCsave:= ""
	If (NPCstrsav or NPCdexsav or NPCconsav or NPCintsav or NPCwissav or NPCchasav or NPC_FS_STR or NPC_FS_DEX or NPC_FS_CON or NPC_FS_INT or NPC_FS_WIS or NPC_FS_CHA)	{
		
		If NPC_FS_STR {
			NPCsave:= NPCsave "Str +0, "
		} Else If (NPCstrsav < 0) {
			NPCsave:= NPCsave "Str " NPCstrsav ", "
		} Else If (NPCstrsav > 0) {
			NPCsave:= NPCsave "Str +" NPCstrsav ", "
		}
		If NPC_FS_DEX {
			NPCsave:= NPCsave "Dex +0, "
		} Else If (NPCdexsav < 0) {
			NPCsave:= NPCsave "Dex " NPCdexsav ", "
		} Else If (NPCdexsav > 0) {
			NPCsave:= NPCsave "Dex +" NPCdexsav ", "
		}
		If NPC_FS_CON {
			NPCsave:= NPCsave "Con +0, "
		} Else If (NPCconsav < 0) {
			NPCsave:= NPCsave "Con " NPCconsav ", "
		} Else If (NPCconsav > 0) {
			NPCsave:= NPCsave "Con +" NPCconsav ", "
		}
		If NPC_FS_INT {
			NPCsave:= NPCsave "Int +0, "
		} Else If (NPCintsav < 0) {
			NPCsave:= NPCsave "Int " NPCintsav ", "
		} Else If (NPCintsav > 0) {
			NPCsave:= NPCsave "Int +" NPCintsav ", "
		}
		If NPC_FS_WIS {
			NPCsave:= NPCsave "Wis +0, "
		} Else If (NPCwissav < 0) {
			NPCsave:= NPCsave "Wis " NPCwissav ", "
		} Else If (NPCwissav > 0) {
			NPCsave:= NPCsave "Wis +" NPCwissav ", "
		}
		If NPC_FS_CHA {
			NPCsave:= NPCsave "Cha +0, "
		} Else If (NPCchasav < 0) {
			NPCsave:= NPCsave "Cha " NPCchasav ", "
		} Else If (NPCchasav > 0) {
			NPCsave:= NPCsave "Cha +" NPCchasav ", "
		}
		
		StringTrimRight, NPCsave, NPCsave, 2
	}	

	NPCsense:= ""
	If (NPCblind or NPCdark or NPCtremor or NPCtrue or NPCpassperc)	{
		If NPCblindb {
			sbb:= " (blind beyond this radius)"
		} else {
			sbb:= ""
		}
		If NPCdarkb {
			sdb:= " (blind beyond this radius)"
		} else {
			sdb:= ""
		}
		If NPCtremorb {
			stb:= " (blind beyond this radius)"
		} else {
			stb:= ""
		}
		If NPCtrueb {
			szb:= " (blind beyond this radius)"
		} else {
			szb:= ""
		}
		
		If NPCblind {
			NPCsense:= NPCsense "blindsight " NPCblind " ft." sbb ", "
		}
		If NPCdark {
			NPCsense:= NPCsense "darkvision " NPCdark " ft." sdb ", "
		}
		If NPCtremor {
			NPCsense:= NPCsense "tremorsense " NPCtremor " ft." stb ", "
		}
		If NPCtrue {
			NPCsense:= NPCsense "truesight " NPCtrue " ft." szb ", "
		}
		NPCsense:= NPCsense "passive Perception " NPCpassperc
	}	

	NPCskill:= ""
	For key, value in NPC_Skills {
		If (value > 0) {
			NPCskill:= NPCskill " " key " +" value ","
		}
		If (value < 0) {
			NPCskill:= NPCskill " " key " " value ","
		}
	}
	If NPCskill {
		StringTrimRight, NPCskill, NPCskill, 1
	}

	NPCdamvul:= ""
	If Flagdamvul	{
		Loop, 10
		{
			If cbDV%A_Index% {
				NPCdamvul:= NPCdamvul NPC_damvul[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamvul, NPCdamvul, 2
		If (cbDV11 or cbDV12 or cbDV13) {
			If NPCdamvul
				NPCdamvul:= NPCdamvul "; "
			If (cbDV11 and cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning, piercing, and slashing"
			Else If (cbDV11 and cbDV12)
				NPCdamvul:= NPCdamvul "bludgeoning and piercing"
			Else If (cbDV11 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning and slashing"
			Else If (cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "piercing and slashing"
			Else If (cbDV11)
				NPCdamvul:= NPCdamvul "bludgeoning"
			Else If (cbDV12)
				NPCdamvul:= NPCdamvul "piercing"
			Else If (cbDV13)
				NPCdamvul:= NPCdamvul "slashing"
		}
	}
	
	NPCdamres:= ""
	If Flagdamres {
		Loop, 10
		{
			If cbDR%A_Index% {
				NPCdamres:= NPCdamres NPC_damres[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamres, NPCdamres, 2
		If (cbDR11 or cbDR12 or cbDR13) {
			If NPCdamres
			NPCdamres:= NPCdamres "; "
			If (cbDR11 and cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning, piercing, and slashing"
			Else If (cbDR11 and cbDR12)
				NPCdamres:= NPCdamres "bludgeoning and piercing"
			Else If (cbDR11 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning and slashing"
			Else If (cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "piercing and slashing"
			Else If (cbDR11)
				NPCdamres:= NPCdamres "bludgeoning"
			Else If (cbDR12)
				NPCdamres:= NPCdamres "piercing"
			Else If (cbDR13)
				NPCdamres:= NPCdamres "slashing"
		}
		If DRRadio1
			NPCdamres:= NPCdamres 
		If DRRadio2
			NPCdamres:= NPCdamres " from nonmagical weapons"
		If DRRadio3
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't silvered"
		If DRRadio4
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't adamantine"
		If DRRadio5
			NPCdamres:= NPCdamres " from magic weapons"
		If DRRadio6
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't cold-forged iron"
	}

	NPCdamimm:= ""
	If Flagdamimm	{
		Loop, 10
		{
			If cbDI%A_Index% {
				NPCdamimm:= NPCdamimm NPC_damimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamimm, NPCdamimm, 2
		If (cbDI11 or cbDI12 or cbDI13) {
			If NPCdamimm
				NPCdamimm:= NPCdamimm "; "
			If (cbDI11 and cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning, piercing, and slashing"
			Else If (cbDI11 and cbDI12)
				NPCdamimm:= NPCdamimm "bludgeoning and piercing"
			Else If (cbDI11 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning and slashing"
			Else If (cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "piercing and slashing"
			Else If (cbDI11)
				NPCdamimm:= NPCdamimm "bludgeoning"
			Else If (cbDI12)
				NPCdamimm:= NPCdamimm "piercing"
			Else If (cbDI13)
				NPCdamimm:= NPCdamimm "slashing"
		}
		If DIRadio1
			NPCdamimm:= NPCdamimm
		If DIRadio2
			NPCdamimm:= NPCdamimm " from nonmagical weapons"
		If DIRadio3
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't silvered"
		If DIRadio4
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't adamantine"
		If DIRadio5
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't cold-forged iron"
	}

	NPCconimm:= ""
	If Flagconimm	{
		Loop, 16
		{
			If cbCI%A_Index% {
				NPCconimm:= NPCconimm NPC_conimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCconimm, NPCconimm, 2
	}	


	NPCactions:= ""
	If FlagActions	{
		Loop % NPC_Actions.Length()
			NPCactions .= "\b\i " NPC_Actions[A_Index, "Name"] ". \b0\i0 " NPC_Actions[A_Index, "Action"] "\line\line"
		StringTrimRight, NPCactions, NPCactions, 5
		StringReplace, NPCactions, NPCactions, Hit:, \i Hit: \i0, all
		StringReplace, NPCactions, NPCactions, Melee Weapon Attack:, \i Melee Weapon Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Ranged Weapon Attack:, \i Ranged Weapon Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Melee or \i Ranged Weapon Attack: \i0, \i Melee or Ranged Weapon Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Melee Spell Attack:, \i Melee Spell Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Ranged Spell Attack:, \i Ranged Spell Attack: \i0, all
		StringReplace, NPCactions, NPCactions, Melee or \i Ranged Spell Attack: \i0, \i Melee or Ranged Spell Attack: \i0, all
	}	

	NPCreactions:= ""
	If FlagReactions	{
		Loop % NPC_Reactions.Length()
			NPCreactions .= "\b\i " NPC_Reactions[A_Index, "Name"] ". \b0\i0 " NPC_Reactions[A_Index, "Reaction"] "\line\line"
		StringTrimRight, NPCreactions, NPCreactions, 5
	}

	NPClegactions:= ""
	If FlagLegActions	{
		Loop % NPC_Legendary_Actions.Length()
			NPClegactions .= "\b\i " NPC_Legendary_Actions[A_Index, "Name"] ". \b0\i0 " NPC_Legendary_Actions[A_Index, "LegAction"] "\line\line"
		StringTrimRight, NPClegactions, NPClegactions, 5
	}	

	NPClairactions:= ""
	If FlagLairActions	{
		Loop % NPC_Lair_Actions.Length()
			NPClairactions .= "\b\i " NPC_Lair_Actions[A_Index, "Name"] ". \b0\i0 " NPC_Lair_Actions[A_Index, "LairAction"] "\line\line"
		StringTrimRight, NPClairactions, NPClairactions, 5
	}	

	NPCtraits:= ""
	If FlagTraits	{
		For key, value in NPC_Traits
			NPCtraits .= "\b\i " NPC_Traits[key, "name"] ". \b0\i0 " NPC_Traits[key, "trait"] "\line\line"
		StringTrimRight, NPCtraits, NPCtraits, 5
	}	

	NPCinspell:= ""
	If FlagInSpell {
		If NPCPsionics {
			NPCinspell:= "\line\b\i Innate Spellcasting (Psionics). \b0\i0 "
		} Else {
			NPCinspell:= "\line\b\i Innate Spellcasting. \b0\i0 "
		}
		NPCinspell:= NPCinspell NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks)."
		else
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ")."
		If NPCinsptext {
			localNPCinsptext:= ", " NPCinsptext
		} else {
			localNPCinsptext:= ""
		}
		NPCinspell:= NPCinspell " " GU1 " can innately cast the following spells" localNPCinsptext ":"
		
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			NPCinspell:= NPCinspell "\line " NPC_InSpell_Slots[A_Index] ": \i "
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop])
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index] ", "
				else
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index] " \i0 "
			}
		}
		NPCinspell:= NPCinspell "\line "
	}

	NPCspell:= ""
	If FlagSpell {
		If (NPCsplevel = "8th") or (NPCsplevel = "11th") or (NPCsplevel = "18th") {
			splpronoun:= "an "
		} else {
			splpronoun:= "a "
		}
		NPCspell:= "\line\b\i Spellcasting. \b0\i0 "
		NPCspell:= NPCspell NU " is " splpronoun NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		NPCspell:= NPCspell " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks)."
		If NPCspflavour
			NPCspell:= NPCspell " " NPCspflavour
		NPCspell:= NPCspell " " NU " has the following " NPCspclass " spells prepared:"
		
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				NPCspell:= NPCspell "\line " NPC_Spell_Level[A_Index]
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): \i "
			} Else {
				NPCspell:= NPCspell "\line " NPC_Spell_Level[A_Index] " level"
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): \i "
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop])
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index] ", "
				else
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index] " \i0 "
			}
		}
		If NPCSpellStar {
			NPCspell:= NPCspell "\line \line "
			NPCspell:= NPCspell NPCSpellStar
		}
		NPCspell:= NPCspell "\line "
	}

	NPCactionsH:= ""
	If FlagActions	{
		Loop % NPC_Actions.Length()
			NPCactionsH .= "<b><i>" NPC_Actions[A_Index, "Name"] ". </i></b>" NPC_Actions[A_Index, "Action"] "<BR><BR>"
		StringTrimRight, NPCactionsH, NPCactionsH, 8
		StringReplace, NPCactionsH, NPCactionsH, Hit:, <i>Hit: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Melee Weapon Attack:, <i>Melee Weapon Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Ranged Weapon Attack:, <i>Ranged Weapon Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Melee or <i>Ranged Weapon Attack: </i>, <i>Melee or Ranged Weapon Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Melee Spell Attack:, <i>Melee Spell Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Ranged Spell Attack:, <i>Ranged Spell Attack: </i>, all
		StringReplace, NPCactionsH, NPCactionsH, Melee or <i>Ranged Spell Attack: </i>, <i>Melee or Ranged Spell Attack: </i>, all
	}	

	NPCreactionsH:= ""
	If FlagReactions	{
		Loop % NPC_Reactions.Length()
			NPCreactionsH .= "<b><i>" NPC_Reactions[A_Index, "Name"] ". </i></b>" NPC_Reactions[A_Index, "Reaction"] "<BR><BR>"
		StringTrimRight, NPCreactionsH, NPCreactionsH, 8
	}

	NPClegactionsH:= ""
	If FlagLegActions	{
		Loop % NPC_Legendary_Actions.Length()
			NPClegactionsH .= "<b><i>" NPC_Legendary_Actions[A_Index, "Name"] ". </i></b>" NPC_Legendary_Actions[A_Index, "LegAction"] "<BR><BR>"
		StringTrimRight, NPClegactionsH, NPClegactionsH, 8
	}	

	NPClairactionsH:= ""
	If FlagLairActions	{
		Loop % NPC_Lair_Actions.Length()
			NPClairactionsH .= "<b><i>" NPC_Lair_Actions[A_Index, "Name"] ". </i></b>" NPC_Lair_Actions[A_Index, "LairAction"] "<BR><BR>"
		StringTrimRight, NPClairactionsH, NPClairactionsH, 8
	}	

	NPCtraitsH:= ""
	If FlagTraits	{
		For key, value in NPC_Traits
			NPCtraitsH .= "<b><i>" NPC_Traits[key, "name"] ". </i></b>" NPC_Traits[key, "trait"] "<BR><BR>"
		StringTrimRight, NPCtraitsH, NPCtraitsH, 8
	}	

	NPCinspellH:= ""
	If FlagInSpell {
		If NPCPsionics {
			NPCinspellH:= "<BR><b><i>Innate Spellcasting (Psionics). </i></b>"
		} Else {
			NPCinspellH:= "<BR><b><i>Innate Spellcasting. </i></b>"
		}
		NPCinspellH:= NPCinspellH NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit
			NPCinspellH:= NPCinspellH " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks)."
		else
			NPCinspellH:= NPCinspellH " (spell save DC " NPCinspsave ")."
		If NPCinsptext {
			localNPCinsptext:= ", " NPCinsptext
		} else {
			localNPCinsptext:= ""
		}
		NPCinspellH:= NPCinspellH " " GU1 " can innately cast the following spells" localNPCinsptext ":"
		
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			NPCinspellH:= NPCinspellH "<BR>" NPC_InSpell_Slots[A_Index] ": <i>"
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop])
					NPCinspellH:= NPCinspellH NPC_InSpell_Names[TempLoop, A_Index] ", "
				else
					NPCinspellH:= NPCinspellH NPC_InSpell_Names[TempLoop, A_Index] " </i>"
			}
		}
		NPCinspellH:= NPCinspellH "<BR>"
	}

	NPCspellH:= ""
	If FlagSpell {
		If (NPCsplevel = "8th") or (NPCsplevel = "11th") or (NPCsplevel = "18th") {
			splpronoun:= "an "
		} else {
			splpronoun:= "a "
		}
		NPCspellH:= "<BR><b><i>Spellcasting. </i></b>"
		NPCspellH:= NPCspellH NU " is " splpronoun NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		NPCspellH:= NPCspellH " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks)."
		If NPCspflavour
			NPCspellH:= NPCspellH " " NPCspflavour
		NPCspellH:= NPCspellH " " NU " has the following " NPCspclass " spells prepared:"
		
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				NPCspellH:= NPCspellH "<BR>" NPC_Spell_Level[A_Index]
				NPCspellH:= NPCspellH " (" NPC_Spell_Slots[A_Index] "): <i>"
			} Else {
				NPCspellH:= NPCspellH "<BR>" NPC_Spell_Level[A_Index] " level"
				NPCspellH:= NPCspellH " (" NPC_Spell_Slots[A_Index] "): <i>"
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop])
					NPCspellH:= NPCspellH NPC_Spell_Names[TempLoop, A_Index] ", "
				else
					NPCspellH:= NPCspellH NPC_Spell_Names[TempLoop, A_Index] " </i>"
			}
		}
		If NPCSpellStar {
			NPCspellH:= NPCspellH "<BR><BR>"
			NPCspellH:= NPCspellH NPCSpellStar
		}
		NPCspellH:= NPCspellH "<BR>"
	}

	NPCdescript:= ""
	If Desc_Add_Text {
		temptext:= RE1.GetRTF(False)
		NPCdescript:= Tokenise(temptext)

		If Desc_Spell_List {
			temptext:= Validate(DescSpell)
			NPCdescript .= temptext Chr(10)
		}
	}

	;~ ;}

	#include HTML_NPC_Engineer_Export.ahk
	
	;{ Building HTML_Stat_Block
	HTML_Stat_Block:= css . htmtop

	If NPCsave
		HTML_Stat_Block .= htmsave
	If NPCskill
		HTML_Stat_Block .= htmskill 
	If NPCdamvul
		HTML_Stat_Block .= htmdamvul 
	If NPCdamres
		HTML_Stat_Block .= htmdamres 
	If NPCdamimm
		HTML_Stat_Block .= htmdamimm 
	If NPCconimm
		HTML_Stat_Block .= htmconimm
	
	HTML_Stat_Block .= htmmiddle

	HTML_Stat_Block .= htmtraits

	If NPCActionsH
		HTML_Stat_Block .= htmactions
	If NPCReactionsH
		HTML_Stat_Block .= htmreactions
	If NPCLegactionsH
		HTML_Stat_Block .= htmlegactions
	If NPCLairactionsH
		HTML_Stat_Block .= htmlairactions
	
	HTML_Stat_Block .= htmbottom
	HTML_Stat_BlockH:= HTML_Stat_Block htmend
	stringreplace, HTML_Stat_BlockH, HTML_Stat_BlockH, <span style="display:block;margin-bottom:-14px;"></span>, , All
	HTML_Stat_Block .= htmdesc htmend
	;}

	;{ RTF information
	colourtable:= "{\colortbl `;\red0\green0\blue0`;\red122\green32\blue13`;\red253\green241\blue220`;\red230\green154\blue40`;\red255\green255\blue255`;\red213\green201\blue180`;\red146\green38\blue16`;}"
	fonttable:= "{\fonttbl {\f0 Calibri`;}{\f1 Mr. Eaves Small Caps`;}"

	
	TrowTop =
	(Ltrim
	\trowd\trrh-75
		\clbrdrt\brdrw1\brdrs\brdrcf1
		\clbrdrb\brdrw1\brdrs\brdrcf1
		\clbrdrl\brdrw1\brdrs\brdrcf1
		\clbrdrr\brdrw1\brdrs\brdrcf1
		\clcbpat4\cellx4706\intbl\cell
	\row
	)

	TrowDiv =
	(Ltrim
	\trowd\trrh-75
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf6
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx85\intbl\cell
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat7\cellx4621\intbl\cell
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf6
		\clcbpat3\cellx4706\intbl\cell
	\row
	)

	TrowDiv2 =
	(Ltrim
	\trowd\trrh-75
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf6
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx85\intbl\cell
		\clbrdrt\brdrw1\brdrs\brdrcf2
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx4621\intbl\cell
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf6
		\clcbpat3\cellx4706\intbl\cell
	\row
	)

	Trow1 =
	(Ltrim
	\trowd\trgaph85
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf6
		\clbrdrr\brdrw1\brdrs\brdrcf6
		\clcbpat3\cellx4706
	)

	Trend =
	(Ltrim
		\intbl\cell
	\row
	)

	Tspace =
	(Ltrim
	\trowd\trrh-60
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf6
		\clbrdrr\brdrw1\brdrs\brdrcf6
		\clcbpat3\cellx4706\intbl\cell
	\row
	)

	Trow2 =
	(Ltrim
	\trowd\trgaph0
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf6
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx85
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx841
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx1597
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx2353
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx3109
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx3865
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf3
		\clcbpat3\cellx4621
		\clbrdrt\brdrw1\brdrs\brdrcf3
		\clbrdrb\brdrw1\brdrs\brdrcf3
		\clbrdrl\brdrw1\brdrs\brdrcf3
		\clbrdrr\brdrw1\brdrs\brdrcf6
		\clcbpat3\cellx4706
	)
	
	;}

	;{ Building RTF file
	rtfblock := TrowTop 
	rtfblock .= Trow1 "\f1\cf2\fs42\b " NPCName " \b0 \line"
	rtfblock .= "\f0\cf1\fs18\i " NPC_size_etc " \i0" Trend 
	rtfblock .= Tspace TrowDiv Tspace
	rtfblock .= Trow1 "\f0\cf2\fs21\b Armor Class \b0 " NPCac "\line" 
	rtfblock .= "\b Hit Points \b0 " NPChp "\line" 
	rtfblock .= "\b Speed \b0 " NPCspeed trend
	rtfblock .= Tspace TrowDiv Tspace

	rtfblock .= Trow2 "\f0\cf2\fs21\b\qc " " \intbl\cell STR \intbl\cell DEX \intbl\cell CON \intbl\cell INT \intbl\cell WIS \intbl\cell CHA \intbl\cell\intbl\cell\row\ql\b0"
	rtfblock .= Trow2 "\f0\cf2\fs21\qc " " \intbl\cell"
	rtfblock .= " " NPCstr " (" NPCstrbo ") \intbl\cell"
	rtfblock .= " " NPCdex " (" NPCdexbo ") \intbl\cell"
	rtfblock .= " " NPCcon " (" NPCconbo ") \intbl\cell"
	rtfblock .= " " NPCint " (" NPCintbo ") \intbl\cell"
	rtfblock .= " " NPCwis " (" NPCwisbo ") \intbl\cell"
	rtfblock .= " " NPCcha " (" NPCchabo ") \intbl\cell\intbl\cell\row\ql"

	rtfblock .= Tspace TrowDiv Tspace
	rtfblock .= Trow1 "\f0\cf2\fs21"
	If NPCsave
		rtfblock .= "\b Saving Throws \b0 " NPCsave "\line"
	If NPCskill
		rtfblock .= "\b Skills \b0 " NPCskill "\line" 
	If NPCdamvul
		rtfblock .= "\b Damage Vulnerabilities \b0 " NPCdamvul "\line" 
	If NPCdamres
		rtfblock .= "\b Damage Resistances \b0 " NPCdamres "\line" 
	If NPCdamimm
		rtfblock .= "\b Damage Immunities \b0 " NPCdamimm "\line" 
	If NPCconimm
		rtfblock .= "\b Condition Immunities \b0 " NPCconimm "\line" 
	rtfblock .= "\b Senses \b0 " NPCsense "\line" 
	rtfblock .= "\b Languages \b0 " NPClang "\line" 
	rtfblock .= "\b Challenge \b0 " NPCcharat " (" NPCxp " XP)" trend
	rtfblock .= Tspace TrowDiv Tspace
	
	rtfblock .= Trow1 "\f0\cf1\fs20" NPCTraits NPCinspell NPCspell trend

	If NPCActions {
		rtfblock .= Trow1 "\f0\cf2\fs34 A\fs24 CTIONS" trend TrowDiv2 Tspace
		rtfblock .= Trow1 "\f0\cf1\fs20" NPCActions trend
	}

	If NPCReactions {
		rtfblock .= Trow1 "\f0\cf2\fs34 R\fs24 EACTIONS" trend TrowDiv2 Tspace
		rtfblock .= Trow1 "\f0\cf1\fs20" NPCreactions trend
	}

	If NPCLegactions {
		rtfblock .= Trow1 "\f0\cf2\fs34 L\fs24 EGENDARY \fs34 A\fs24 CTIONS" trend TrowDiv2 Tspace
		rtfblock .= Trow1 "\f0\cf1\fs20" NPClegactions trend
	}

	If NPCLairactions {
		rtfblock .= Trow1 "\f0\cf2\fs34 L\fs24 AIR \fs34 A\fs24 CTIONS" trend TrowDiv2 Tspace
		rtfblock .= Trow1 "\f0\cf1\fs20" NPClairactions trend
	}
	
	rtfblock .= TrowTop
	CopyBlock:= "{\rtf1\ansi\deff0 " fonttable "}" colourtable rtfblock "}"
	;}

	;{ Building BB_Stat_Block
	
	#include BB_NPC_Engineer_Export.ahk

	
	BB_Stat_Block:= bbtop

	If NPCsave
		BB_Stat_Block .= bbsave
	If NPCskill
		BB_Stat_Block .= bbskill 
	If NPCdamvul
		BB_Stat_Block .= bbdamvul 
	If NPCdamres
		BB_Stat_Block .= bbdamres 
	If NPCdamimm
		BB_Stat_Block .= bbdamimm 
	If NPCconimm
		BB_Stat_Block .= bbconimm
	
	BB_Stat_Block .= bbmiddle

	BB_Stat_Block .= bbtraits

	If NPCActionsH
		BB_Stat_Block .= bbactions
	If NPCReactionsH
		BB_Stat_Block .= bbreactions
	If NPCLegactionsH
		BB_Stat_Block .= bblegactions
	If NPCLairactionsH
		BB_Stat_Block .= bblairactions

	stringreplace, BB_Stat_Block, BB_Stat_Block, <, [, All
	stringreplace, BB_Stat_Block, BB_Stat_Block, >, ], All
	stringreplace, BB_Stat_Block, BB_Stat_Block, [BR], `n, All
	
	BB_Stat_Block .= Chr(10) Chr(10) "[i][size=2]Exported by [b]NPC Engineer[/b] ([url]www.masq.net[/url])[/size][/i]" Chr(10)
	;}
}

Parser() {
	global
	if (Mod_Parser == 2) {
		Par5e_Out()
	} else if (Mod_Parser == 3) {
		FG5EP_Out()
	} else if (Mod_Parser == 1) {
		NPCEP_Out()
	}
}

Par5e_Out() {
	Global		
	TempStats := ""
	Tempstats := Tempstats NPCstr " (" NPCstrbo ") "
	Tempstats := Tempstats NPCdex " (" NPCdexbo ") "
	Tempstats := Tempstats NPCcon " (" NPCconbo ") "
	Tempstats := Tempstats NPCint " (" NPCintbo ") "
	Tempstats := Tempstats NPCwis " (" NPCwisbo ") "
	Tempstats := Tempstats NPCcha " (" NPCchabo ")"

	NPCspeed:= ""
	NPCspeed:= NPCspeed "Speed " NPCwalk " ft."
	If NPCburrow {
		NPCspeed:= NPCspeed ", burrow " NPCburrow " ft."
	}
	If NPCclimb {
		NPCspeed:= NPCspeed ", climb " NPCclimb " ft."
	}
	If NPCfly {
		NPCspeed:= NPCspeed ", fly " NPCfly " ft."
	}
	If NPChover {
		NPCspeed:= NPCspeed " (hover)"
	}
	If NPCswim {
		NPCspeed:= NPCspeed ", swim " NPCswim " ft."
	}
	NPCspeed:= NPCspeed Chr(13) Chr(10)

	NPCsave:= ""
	If (NPCstrsav or NPCdexsav or NPCconsav or NPCintsav or NPCwissav or NPCchasav or NPC_FS_STR or NPC_FS_DEX or NPC_FS_CON or NPC_FS_INT or NPC_FS_WIS or NPC_FS_CHA)	{
		
		NPCsave:= "Saving Throws "
		If NPC_FS_STR {
			NPCsave:= NPCsave "Str +0, "
		} Else If (NPCstrsav < 0) {
			NPCsave:= NPCsave "Str " NPCstrsav ", "
		} Else If (NPCstrsav > 0) {
			NPCsave:= NPCsave "Str +" NPCstrsav ", "
		}
		If NPC_FS_DEX {
			NPCsave:= NPCsave "Dex +0, "
		} Else If (NPCdexsav < 0) {
			NPCsave:= NPCsave "Dex " NPCdexsav ", "
		} Else If (NPCdexsav > 0) {
			NPCsave:= NPCsave "Dex +" NPCdexsav ", "
		}
		If NPC_FS_CON {
			NPCsave:= NPCsave "Con +0, "
		} Else If (NPCconsav < 0) {
			NPCsave:= NPCsave "Con " NPCconsav ", "
		} Else If (NPCconsav > 0) {
			NPCsave:= NPCsave "Con +" NPCconsav ", "
		}
		If NPC_FS_INT {
			NPCsave:= NPCsave "Int +0, "
		} Else If (NPCintsav < 0) {
			NPCsave:= NPCsave "Int " NPCintsav ", "
		} Else If (NPCintsav > 0) {
			NPCsave:= NPCsave "Int +" NPCintsav ", "
		}
		If NPC_FS_WIS {
			NPCsave:= NPCsave "Wis +0, "
		} Else If (NPCwissav < 0) {
			NPCsave:= NPCsave "Wis " NPCwissav ", "
		} Else If (NPCwissav > 0) {
			NPCsave:= NPCsave "Wis +" NPCwissav ", "
		}
		If NPC_FS_CHA {
			NPCsave:= NPCsave "Cha +0, "
		} Else If (NPCchasav < 0) {
			NPCsave:= NPCsave "Cha " NPCchasav ", "
		} Else If (NPCchasav > 0) {
			NPCsave:= NPCsave "Cha +" NPCchasav ", "
		}
		
		StringTrimRight, NPCsave, NPCsave, 2
		NPCsave:= NPCsave Chr(13) Chr(10)
	}	

	NPCsense:= ""
	If (NPCblind or NPCdark or NPCtremor or NPCtrue or NPCpassperc)	{
		NPCsense:= "Senses "
		If NPCblindb {
			sbb:= " (blind beyond this radius)"
		} else {
			sbb:= ""
		}
		If NPCdarkb {
			sdb:= " (blind beyond this radius)"
		} else {
			sdb:= ""
		}
		If NPCtremorb {
			stb:= " (blind beyond this radius)"
		} else {
			stb:= ""
		}
		If NPCtrueb {
			szb:= " (blind beyond this radius)"
		} else {
			szb:= ""
		}
		
		If NPCblind {
			NPCsense:= NPCsense "blindsight " NPCblind " ft." sbb ", "
		}
		If NPCdark {
			NPCsense:= NPCsense "darkvision " NPCdark " ft." sdb ", "
		}
		If NPCtremor {
			NPCsense:= NPCsense "tremorsense " NPCtremor " ft." stb ", "
		}
		If NPCtrue {
			NPCsense:= NPCsense "truesight " NPCtrue " ft." szb ", "
		}
		NPCsense:= NPCsense "passive Perception " NPCpassperc Chr(13) Chr(10)
	}	

	NPCskill:= ""
	For key, value in NPC_Skills {
		If (value > 0) {
			NPCskill:= NPCskill " " key " +" value ","
		}
		If (value < 0) {
			NPCskill:= NPCskill " " key " " value ","
		}
	}
	If NPCskill {
		StringTrimRight, NPCskill, NPCskill, 1
		NPCskill:= "Skills" NPCskill Chr(13) Chr(10)
	}
	
	NPCactions:= ""
	If FlagActions	{
		NPCactions:= "ACTIONS" Chr(13) Chr(10)
		Loop % NPC_Actions.Length()
			NPCactions:= NPCactions NPC_Actions[A_Index, "Name"] ". " NPC_Actions[A_Index, "Action"] Chr(13) Chr(10)
	}	

	NPCreactions:= ""
	If FlagReactions	{
		NPCreactions:= "REACTIONS" Chr(13) Chr(10)
		Loop % NPC_Reactions.Length()
			NPCreactions:= NPCreactions NPC_Reactions[A_Index, "Name"] ". " NPC_Reactions[A_Index, "Reaction"] Chr(13) Chr(10)
	}

	NPClegactions:= ""
	If FlagLegActions	{
		NPClegactions:= "LEGENDARY ACTIONS" Chr(13) Chr(10)
		Loop % NPC_Legendary_Actions.Length()
			NPClegactions:= NPClegactions NPC_Legendary_Actions[A_Index, "Name"] ". " NPC_Legendary_Actions[A_Index, "LegAction"] Chr(13) Chr(10)
	}	

	NPClairactions:= ""
	NPClairactions2:= ""
	If FlagLairActions	{
		NPClairactions:= "LAIR ACTIONS" Chr(13) Chr(10)
		NPClairactions2:= "LAIR ACTIONS" Chr(13) Chr(10)
		Loop % NPC_Lair_Actions.Length()
		{
			NPClairactions:= NPClairactions NPC_Lair_Actions[A_Index, "Name"] ". " NPC_Lair_Actions[A_Index, "LairAction"] Chr(13) Chr(10)
			NPClairactions2:= NPClairactions2 NPC_Lair_Actions[A_Index, "LairAction"] Chr(13) Chr(10)
		}
	}	

	NPCtraits:= ""
	If FlagTraits	{
		For key, value in NPC_Traits
			NPCtraits:= NPCtraits NPC_Traits[key, "name"] ". " NPC_Traits[key, "trait"] Chr(13) Chr(10)
	}	

	NPCdamvul:= ""
	If Flagdamvul	{
		Loop, 10
		{
			If cbDV%A_Index% {
				NPCdamvul:= NPCdamvul NPC_damvul[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamvul, NPCdamvul, 2
		If (cbDV11 or cbDV12 or cbDV13) {
			If NPCdamvul
				NPCdamvul:= NPCdamvul "; "
			If (cbDV11 and cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning, piercing, and slashing"
			Else If (cbDV11 and cbDV12)
				NPCdamvul:= NPCdamvul "bludgeoning and piercing"
			Else If (cbDV11 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning and slashing"
			Else If (cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "piercing and slashing"
			Else If (cbDV11)
				NPCdamvul:= NPCdamvul "bludgeoning"
			Else If (cbDV12)
				NPCdamvul:= NPCdamvul "piercing"
			Else If (cbDV13)
				NPCdamvul:= NPCdamvul "slashing"
		}
		 NPCdamvul:= "Damage Vulnerabilities " NPCdamvul Chr(13) Chr(10)
	}
	
	NPCdamres:= ""
	If Flagdamres {
		Loop, 10
		{
			If cbDR%A_Index% {
				NPCdamres:= NPCdamres NPC_damres[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamres, NPCdamres, 2
		If (cbDR11 or cbDR12 or cbDR13) {
			If NPCdamres
			NPCdamres:= NPCdamres "; "
			If (cbDR11 and cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning, piercing, and slashing"
			Else If (cbDR11 and cbDR12)
				NPCdamres:= NPCdamres "bludgeoning and piercing"
			Else If (cbDR11 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning and slashing"
			Else If (cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "piercing and slashing"
			Else If (cbDR11)
				NPCdamres:= NPCdamres "bludgeoning"
			Else If (cbDR12)
				NPCdamres:= NPCdamres "piercing"
			Else If (cbDR13)
				NPCdamres:= NPCdamres "slashing"
		}
		If DRRadio1
			NPCdamres:= "Damage Resistances " NPCdamres Chr(13) Chr(10)
		If DRRadio2
			NPCdamres:= "Damage Resistances " NPCdamres " from nonmagical weapons" Chr(13) Chr(10)
		If DRRadio3
			NPCdamres:= "Damage Resistances " NPCdamres " from nonmagical weapons that aren't silvered" Chr(13) Chr(10)
		If DRRadio4
			NPCdamres:= "Damage Resistances " NPCdamres " from nonmagical weapons that aren't adamantine" Chr(13) Chr(10)
		If DRRadio5
			NPCdamres:= "Damage Resistances " NPCdamres " from magic weapons" Chr(10)
		If DRRadio6
			NPCdamres:= "Damage Resistances " NPCdamres " from nonmagical weapons that aren't cold-forged iron" Chr(13) Chr(10)
	}

	NPCdamimm:= ""
	If Flagdamimm	{
		Loop, 10
		{
			If cbDI%A_Index% {
				NPCdamimm:= NPCdamimm NPC_damimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamimm, NPCdamimm, 2
		If (cbDI11 or cbDI12 or cbDI13) {
			If NPCdamimm
			NPCdamimm:= NPCdamimm "; "
			If (cbDI11 and cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning, piercing, and slashing"
			Else If (cbDI11 and cbDI12)
				NPCdamimm:= NPCdamimm "bludgeoning and piercing"
			Else If (cbDI11 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning and slashing"
			Else If (cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "piercing and slashing"
			Else If (cbDI11)
				NPCdamimm:= NPCdamimm "bludgeoning"
			Else If (cbDI12)
				NPCdamimm:= NPCdamimm "piercing"
			Else If (cbDI13)
				NPCdamimm:= NPCdamimm "slashing"
		}
		If DIRadio1
			NPCdamimm:= "Damage Immunities " NPCdamimm Chr(13) Chr(10)
		If DIRadio2
			NPCdamimm:= "Damage Immunities " NPCdamimm " from nonmagical weapons" Chr(13) Chr(10)
		If DIRadio3
			NPCdamimm:= "Damage Immunities " NPCdamimm " from nonmagical weapons that aren't silvered" Chr(13) Chr(10)
		If DIRadio4
			NPCdamimm:= "Damage Immunities " NPCdamimm " from nonmagical weapons that aren't adamantine" Chr(13) Chr(10)
		If DIRadio5
			NPCdamimm:= "Damage Immunities " NPCdamimm " from nonmagical weapons that aren't cold-forged iron" Chr(13) Chr(10)
	}

	NPCconimm:= ""
	If Flagconimm	{
		Loop, 16
		{
			If cbCI%A_Index% {
				NPCconimm:= NPCconimm NPC_conimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCconimm, NPCconimm, 2
		NPCconimm:= "Condition Immunities " NPCconimm Chr(13) Chr(10)
	}	

	NPCinspell:= ""
	If FlagInSpell	{
		If NPCPsionics {
			NPCinspell:= "Innate Spellcasting (Psionics). "
		} Else {
			NPCinspell:= "Innate Spellcasting. "
		}
		NPCinspell:= NPCinspell NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks)."
		else
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ")."
		If NPCinsptext {
			localNPCinsptext:= ", " NPCinsptext
		} else {
			localNPCinsptext:= ""
		}
		NPCinspell:= NPCinspell " " GU1 " can innately cast the following spells" localNPCinsptext ":"
		
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			NPCinspell:= NPCinspell "\r" NPC_InSpell_Slots[A_Index] ": "
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop])
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index] ", "
				else
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index]
			}
		}
		NPCinspell:= NPCinspell Chr(13) Chr(10)
	}

	NPCspell:= ""
	If FlagSpell	{
		If (NPCsplevel = "8th") or (NPCsplevel = "11th") or (NPCsplevel = "18th") {
			splpronoun:= "an "
		} else {
			splpronoun:= "a "
		}
		NPCspell:= "Spellcasting. "
		NPCspell:= NPCspell NU " is " splpronoun NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		NPCspell:= NPCspell " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks)."
		If NPCspflavour
			NPCspell:= NPCspell " " NPCspflavour
		NPCspell:= NPCspell " " NU " has the following " NPCspclass " spells prepared:"
		
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				NPCspell:= NPCspell "\r" NPC_Spell_Level[A_Index]
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): "
			} Else {
				NPCspell:= NPCspell "\r" NPC_Spell_Level[A_Index] " level"
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): "
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop])
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index] ", "
				else
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index]
			}
		}
		If NPCSpellStar {
			NPCspell:= NPCspell "\r" NPCSpellStar
		}
		NPCspell:= NPCspell Chr(13) Chr(10)
	}

	NPC_size_etc:= NPCsize " " NPCtype
	If NPCtag
		NPC_size_etc:= NPC_size_etc " (" NPCtag ")"
	NPC_size_etc:= NPC_size_etc ", " NPCalign Chr(13) Chr(10)

	NPCdescript:= ""
	If Desc_Add_Text {
		NPCdescript:= NPCdescript "##;" Chr(13) Chr(10)
		If Desc_NPC_Title {
			NPCdescript:= NPCdescript "#h;" NPCname Chr(13) Chr(10)
		}
		If Desc_Image_Link {
			NPCdescript:= NPCdescript "#zl;image;" NPCjpeg ".jpg;Image: " NPCname Chr(13) Chr(10)
		}
		temptext:= RE1.GetRTF(False)
		TKN:= Tokenise(temptext)
		NPCdescript:= NPCdescript Textise(TKN)
		If Desc_Spell_List {
			Desc_Spell_List()
			NPCdescript:= NPCdescript DescSpell Chr(13) Chr(10)
		}
	}

	local tokeninfo
	tokeninfo:= ""
	If (NPCTokenPath and NPCjpeg) {
		tokeninfo:= "Token: " NPCjpeg ".png"  Chr(13) Chr(10)
	}

	Lang_Set()

	Save_File:= ""
	Save_File:= NPCname Chr(13) Chr(10)
	Save_File:= Save_File NPC_size_etc
	Save_File:= Save_File "Armor Class " NPCac Chr(13) Chr(10)
	Save_File:= Save_File "Hit Points " NPChp Chr(13) Chr(10)
	Save_File:= Save_File NPCspeed
	Save_File:= Save_File "STR DEX CON INT WIS CHA " TempStats Chr(13) Chr(10)
	Save_File:= Save_File NPCsave
	Save_File:= Save_File NPCskill
	Save_File:= Save_File NPCdamvul
	Save_File:= Save_File NPCdamres
	Save_File:= Save_File NPCdamimm
	Save_File:= Save_File NPCconimm
	Save_File:= Save_File NPCsense 
	Save_File:= Save_File NPClang
	Save_File:= Save_File "Challenge " NPCcharat " (" NPCxp " XP)" Chr(13) Chr(10)
	Save_File:= Save_File NPCtraits
	Save_File:= Save_File NPCinspell
	Save_File:= Save_File NPCspell
	Save_File:= Save_File NPCactions
	Save_File:= Save_File NPCreactions
	Save_File:= Save_File NPClegactions
	
	SaveMainBlock:= Save_File NPClairactions2
	
	Save_File:= Save_File NPClairactions
	Save_File:= Save_File tokeninfo
	Save_File:= Save_File NPCdescript
	StringReplace, Save_File, Save_File, `r`n`r`n, `r`n, all
	StringReplace, Save_File, Save_File, `r`r`n, `r`n, all
	StringReplace, Save_File, Save_File, `r`n`r, `r`n, all
	StringReplace, Save_File, Save_File, `n`r`n, `r`n, all
	StringReplace, Save_File, Save_File, `r`n`n, `r`n, all
	Save_File:= Save_File Chr(13) Chr(10)
}

FG5EP_Out() {
}

NPCEP_Out() {
	Global		
	TempStats := ""
	Tempstats := Tempstats NPCstr " (" NPCstrbo ") "
	Tempstats := Tempstats NPCdex " (" NPCdexbo ") "
	Tempstats := Tempstats NPCcon " (" NPCconbo ") "
	Tempstats := Tempstats NPCint " (" NPCintbo ") "
	Tempstats := Tempstats NPCwis " (" NPCwisbo ") "
	Tempstats := Tempstats NPCcha " (" NPCchabo ")"

	FoundPos := InStr(NPCac, "(")
	NPCacnum := SubStr(NPCac, 1, FoundPos-1)
	NPCactxt := SubStr(NPCac, FoundPos)
	
	FoundPos := InStr(NPChp, "(")
	NPChpnum := SubStr(NPChp, 1, FoundPos-1)
	NPChphd := SubStr(NPChp, FoundPos)
	
	
	NPCspeed:= ""
	NPCspeed:= NPCspeed "Speed " NPCwalk " ft."
	If NPCburrow {
		NPCspeed:= NPCspeed ", burrow " NPCburrow " ft."
	}
	If NPCclimb {
		NPCspeed:= NPCspeed ", climb " NPCclimb " ft."
	}
	If NPCfly {
		NPCspeed:= NPCspeed ", fly " NPCfly " ft."
	}
	If NPChover {
		NPCspeed:= NPCspeed " (hover)"
	}
	If NPCswim {
		NPCspeed:= NPCspeed ", swim " NPCswim " ft."
	}

	NPCsave:= ""
	If (NPCstrsav or NPCdexsav or NPCconsav or NPCintsav or NPCwissav or NPCchasav or NPC_FS_STR or NPC_FS_DEX or NPC_FS_CON or NPC_FS_INT or NPC_FS_WIS or NPC_FS_CHA)	{
		NPCsave:= "Saving Throws "
		
		If NPC_FS_STR {
			NPCsave:= NPCsave "Str +0, "
		} Else If (NPCstrsav < 0) {
			NPCsave:= NPCsave "Str " NPCstrsav ", "
		} Else If (NPCstrsav > 0) {
			NPCsave:= NPCsave "Str +" NPCstrsav ", "
		}
		If NPC_FS_DEX {
			NPCsave:= NPCsave "Dex +0, "
		} Else If (NPCdexsav < 0) {
			NPCsave:= NPCsave "Dex " NPCdexsav ", "
		} Else If (NPCdexsav > 0) {
			NPCsave:= NPCsave "Dex +" NPCdexsav ", "
		}
		If NPC_FS_CON {
			NPCsave:= NPCsave "Con +0, "
		} Else If (NPCconsav < 0) {
			NPCsave:= NPCsave "Con " NPCconsav ", "
		} Else If (NPCconsav > 0) {
			NPCsave:= NPCsave "Con +" NPCconsav ", "
		}
		If NPC_FS_INT {
			NPCsave:= NPCsave "Int +0, "
		} Else If (NPCintsav < 0) {
			NPCsave:= NPCsave "Int " NPCintsav ", "
		} Else If (NPCintsav > 0) {
			NPCsave:= NPCsave "Int +" NPCintsav ", "
		}
		If NPC_FS_WIS {
			NPCsave:= NPCsave "Wis +0, "
		} Else If (NPCwissav < 0) {
			NPCsave:= NPCsave "Wis " NPCwissav ", "
		} Else If (NPCwissav > 0) {
			NPCsave:= NPCsave "Wis +" NPCwissav ", "
		}
		If NPC_FS_CHA {
			NPCsave:= NPCsave "Cha +0, "
		} Else If (NPCchasav < 0) {
			NPCsave:= NPCsave "Cha " NPCchasav ", "
		} Else If (NPCchasav > 0) {
			NPCsave:= NPCsave "Cha +" NPCchasav ", "
		}
		
		StringTrimRight, NPCsave, NPCsave, 2
		NPCsave:= NPCsave Chr(10)
	}	

	NPCsense:= ""
	If (NPCblind or NPCdark or NPCtremor or NPCtrue or NPCpassperc)	{
		NPCsense:= "Senses "
		If NPCblindb {
			sbb:= " (blind beyond this radius)"
		} else {
			sbb:= ""
		}
		If NPCdarkb {
			sdb:= " (blind beyond this radius)"
		} else {
			sdb:= ""
		}
		If NPCtremorb {
			stb:= " (blind beyond this radius)"
		} else {
			stb:= ""
		}
		If NPCtrueb {
			szb:= " (blind beyond this radius)"
		} else {
			szb:= ""
		}
		
		If NPCblind {
			NPCsense:= NPCsense "blindsight " NPCblind " ft." sbb ", "
		}
		If NPCdark {
			NPCsense:= NPCsense "darkvision " NPCdark " ft." sdb ", "
		}
		If NPCtremor {
			NPCsense:= NPCsense "tremorsense " NPCtremor " ft." stb ", "
		}
		If NPCtrue {
			NPCsense:= NPCsense "truesight " NPCtrue " ft." szb ", "
		}
		NPCsense:= NPCsense "passive Perception " NPCpassperc Chr(10)
	}	

	NPCskill:= ""
	For key, value in NPC_Skills {
		If (value > 0) {
			NPCskill:= NPCskill " " key " +" value ","
		}
		If (value < 0) {
			NPCskill:= NPCskill " " key " " value ","
		}
	}
	If NPCskill {
		StringTrimRight, NPCskill, NPCskill, 1
		NPCskill:= "Skills" NPCskill Chr(10)
	}
	
	NPCactions:= ""
	If FlagActions	{
		NPCactions:= "ACTIONS" Chr(10)
		Loop % NPC_Actions.Length()
			NPCactions:= NPCactions NPC_Actions[A_Index, "Name"] ". " NPC_Actions[A_Index, "Action"] Chr(10)
	}	

	NPCreactions:= ""
	If FlagReactions	{
		NPCreactions:= "REACTIONS" Chr(10)
		Loop % NPC_Reactions.Length()
			NPCreactions:= NPCreactions NPC_Reactions[A_Index, "Name"] ". " NPC_Reactions[A_Index, "Reaction"] Chr(10)
	}

	NPClegactions:= ""
	If FlagLegActions	{
		NPClegactions:= "LEGENDARY ACTIONS" Chr(10)
		Loop % NPC_Legendary_Actions.Length()
			NPClegactions:= NPClegactions NPC_Legendary_Actions[A_Index, "Name"] ". " NPC_Legendary_Actions[A_Index, "LegAction"] Chr(10)
	}	

	NPClairactions:= ""
	If FlagLairActions	{
		NPClairactions:= "LAIR ACTIONS" Chr(10)
		Loop % NPC_Lair_Actions.Length()
			NPClairactions:= NPClairactions NPC_Lair_Actions[A_Index, "Name"] ". " NPC_Lair_Actions[A_Index, "LairAction"] Chr(10)
	}	

	NPCtraits:= ""
	If FlagTraits	{
		For key, value in NPC_Traits
			NPCtraits:= NPCtraits NPC_Traits[key, "name"] ". " NPC_Traits[key, "trait"] Chr(10)
	}	

	NPCdamvul:= ""
	If Flagdamvul	{
		Loop, 10
		{
			If cbDV%A_Index% {
				NPCdamvul:= NPCdamvul NPC_damvul[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamvul, NPCdamvul, 2
		If (cbDV11 or cbDV12 or cbDV13) {
			If NPCdamvul
				NPCdamvul:= NPCdamvul "; "
			If (cbDV11 and cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning, piercing, and slashing"
			Else If (cbDV11 and cbDV12)
				NPCdamvul:= NPCdamvul "bludgeoning and piercing"
			Else If (cbDV11 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning and slashing"
			Else If (cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "piercing and slashing"
			Else If (cbDV11)
				NPCdamvul:= NPCdamvul "bludgeoning"
			Else If (cbDV12)
				NPCdamvul:= NPCdamvul "piercing"
			Else If (cbDV13)
				NPCdamvul:= NPCdamvul "slashing"
		}
		 NPCdamvul:= "Damage Vulnerabilities " NPCdamvul Chr(10)
	}
	
	NPCdamres:= ""
	If Flagdamres {
		Loop, 10
		{
			If cbDR%A_Index% {
				NPCdamres:= NPCdamres NPC_damres[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamres, NPCdamres, 2
		If (cbDR11 or cbDR12 or cbDR13) {
			If NPCdamres
			NPCdamres:= NPCdamres "; "
			If (cbDR11 and cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning, piercing, and slashing"
			Else If (cbDR11 and cbDR12)
				NPCdamres:= NPCdamres "bludgeoning and piercing"
			Else If (cbDR11 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning and slashing"
			Else If (cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "piercing and slashing"
			Else If (cbDR11)
				NPCdamres:= NPCdamres "bludgeoning"
			Else If (cbDR12)
				NPCdamres:= NPCdamres "piercing"
			Else If (cbDR13)
				NPCdamres:= NPCdamres "slashing"
		}
		If DRRadio1
			NPCdamres:= "Damage Resistances " NPCdamres Chr(10)
		If DRRadio2
			NPCdamres:= "Damage Resistances " NPCdamres " from nonmagical weapons" Chr(10)
		If DRRadio3
			NPCdamres:= "Damage Resistances " NPCdamres " from nonmagical weapons that aren't silvered" Chr(10)
		If DRRadio4
			NPCdamres:= "Damage Resistances " NPCdamres " from nonmagical weapons that aren't adamantine" Chr(10)
		If DRRadio5
			NPCdamres:= "Damage Resistances " NPCdamres " from magic weapons" Chr(10)
		If DRRadio6
			NPCdamres:= "Damage Resistances " NPCdamres " from nonmagical weapons that aren't cold-forged iron" Chr(10)
	}

	NPCdamimm:= ""
	If Flagdamimm	{
		Loop, 10
		{
			If cbDI%A_Index% {
				NPCdamimm:= NPCdamimm NPC_damimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamimm, NPCdamimm, 2
		If (cbDI11 or cbDI12 or cbDI13) {
			If NPCdamimm
				NPCdamimm:= NPCdamimm "; "
			If (cbDI11 and cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning, piercing, and slashing"
			Else If (cbDI11 and cbDI12)
				NPCdamimm:= NPCdamimm "bludgeoning and piercing"
			Else If (cbDI11 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning and slashing"
			Else If (cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "piercing and slashing"
			Else If (cbDI11)
				NPCdamimm:= NPCdamimm "bludgeoning"
			Else If (cbDI12)
				NPCdamimm:= NPCdamimm "piercing"
			Else If (cbDI13)
				NPCdamimm:= NPCdamimm "slashing"
		}
		If DIRadio1
			NPCdamimm:= "Damage Immunities " NPCdamimm Chr(10)
		If DIRadio2
			NPCdamimm:= "Damage Immunities " NPCdamimm " from nonmagical weapons" Chr(10)
		If DIRadio3
			NPCdamimm:= "Damage Immunities " NPCdamimm " from nonmagical weapons that aren't silvered" Chr(10)
		If DIRadio4
			NPCdamimm:= "Damage Immunities " NPCdamimm " from nonmagical weapons that aren't adamantine" Chr(10)
		If DIRadio5
			NPCdamimm:= "Damage Immunities " NPCdamimm " from nonmagical weapons that aren't cold-forged iron" Chr(10)
	}

	NPCconimm:= ""
	If Flagconimm	{
		Loop, 16
		{
			If cbCI%A_Index% {
				NPCconimm:= NPCconimm NPC_conimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCconimm, NPCconimm, 2
		NPCconimm:= "Condition Immunities " NPCconimm Chr(10)
	}	

	NPCinspell:= ""
	If FlagInSpell	{
		If NPCPsionics {
			NPCinspell:= "Innate Spellcasting (Psionics). "
		} Else {
			NPCinspell:= "Innate Spellcasting. "
		}
		NPCinspell:= NPCinspell NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks)."
		else
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ")."
		If NPCinsptext {
			localNPCinsptext:= ", " NPCinsptext
		} else {
			localNPCinsptext:= ""
		}
		NPCinspell:= NPCinspell " " GU1 " can innately cast the following spells" localNPCinsptext ":"
		
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			NPCinspell:= NPCinspell "\r" NPC_InSpell_Slots[A_Index] ": "
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop])
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index] ", "
				else
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index]
			}
		}
		NPCinspell:= NPCinspell Chr(10)
	}

	NPCspell:= ""
	If FlagSpell	{
		If (NPCsplevel = "8th") or (NPCsplevel = "11th") or (NPCsplevel = "18th") {
			splpronoun:= "an "
		} else {
			splpronoun:= "a "
		}
		NPCspell:= "Spellcasting. "
		NPCspell:= NPCspell NU " is " splpronoun NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		NPCspell:= NPCspell " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks)."
		If NPCspflavour
			NPCspell:= NPCspell " " NPCspflavour
		NPCspell:= NPCspell " " NU " has the following " NPCspclass " spells prepared:"
		
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				NPCspell:= NPCspell "\r" NPC_Spell_Level[A_Index]
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): "
			} Else {
				NPCspell:= NPCspell "\r" NPC_Spell_Level[A_Index] " level"
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): "
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop])
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index] ", "
				else
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index]
			}
		}
		If NPCSpellStar {
			NPCspell:= NPCspell "\r" NPCSpellStar
		}
		NPCspell:= NPCspell Chr(10)
	}

	NPC_size_etc:= NPCsize " " NPCtype
	If NPCtag
		NPC_size_etc:= NPC_size_etc " (" NPCtag ")"
	NPC_size_etc:= NPC_size_etc ", " NPCalign Chr(10)

	NPCdescript:= ""
	If Desc_Add_Text {
		NPCdescript:= NPCdescript "##;" Chr(10)
		If Desc_NPC_Title {
			NPCdescript:= NPCdescript "#h;" NPCname Chr(10)
		}
		If Desc_Image_Link {
			NPCdescript:= NPCdescript "#zl;image;" NPCjpeg ".jpg;Image: " NPCname Chr(10)
		}
		temptext:= RE1.GetRTF(False)
		TKN:= Tokenise(temptext)
		NPCdescript:= NPCdescript Textise(TKN)
		If Desc_Spell_List {
			NPCdescript:= NPCdescript DescSpell Chr(10)
		}
	}

	Lang_Set()

	Save_File:= ""
	Save_File:= NPCname Chr(10)
	Save_File:= Save_File NPC_size_etc
	Save_File:= Save_File "Armor Class " NPCac Chr(10)
	Save_File:= Save_File "Hit Points " NPChp Chr(10)
	Save_File:= Save_File NPCspeed Chr(10)
	Save_File:= Save_File "STR DEX CON INT WIS CHA " TempStats Chr(10)
	Save_File:= Save_File NPCsave
	Save_File:= Save_File NPCskill
	Save_File:= Save_File NPCdamvul
	Save_File:= Save_File NPCdamres
	Save_File:= Save_File NPCdamimm
	Save_File:= Save_File NPCconimm
	Save_File:= Save_File NPCsense 
	Save_File:= Save_File NPClang
	Save_File:= Save_File "Challenge " NPCcharat " (" NPCxp " XP)" Chr(10)
	Save_File:= Save_File NPCtraits
	Save_File:= Save_File NPCinspell
	Save_File:= Save_File NPCspell
	Save_File:= Save_File NPCactions
	Save_File:= Save_File NPCreactions
	Save_File:= Save_File NPClegactions
	Save_File:= Save_File NPClairactions
	Save_File:= Save_File NPCdescript
	Save_File:= Save_File Chr(10)
}

Build_NPC_Object() {
	Global		

	FoundPos := InStr(NPCac, "(")
	If foundpos {
		NPCacnum := SubStr(NPCac, 1, FoundPos-1)
		NPCactxt := SubStr(NPCac, FoundPos)
	} else {
		NPCacnum := RegExReplace(NPCac, "\D")
		NPCactxt := ""
	}
	
	FoundPos := InStr(NPChp, "(")
	If foundpos {
		NPChpnum := SubStr(NPChp, 1, FoundPos-1)
		NPChphd := SubStr(NPChp, FoundPos)
	} else {
		NPChpnum := RegExReplace(NPChp, "\D")
		NPChphd := ""
	}
	
	If NPCtag {
		NPCtypetag:= NPCtype " (" NPCtag ")"
	} else {
		NPCtypetag:= NPCtype
	}

	NPCspeed:= NPCwalk " ft."
	If NPCburrow {
		NPCspeed:= NPCspeed ", burrow " NPCburrow " ft."
	}
	If NPCclimb {
		NPCspeed:= NPCspeed ", climb " NPCclimb " ft."
	}
	If NPCfly {
		NPCspeed:= NPCspeed ", fly " NPCfly " ft."
	}
	If NPChover {
		NPCspeed:= NPCspeed " (hover)"
	}
	If NPCswim {
		NPCspeed:= NPCspeed ", swim " NPCswim " ft."
	}

	NPCsave:= ""
	If (NPCstrsav or NPCdexsav or NPCconsav or NPCintsav or NPCwissav or NPCchasav or NPC_FS_STR or NPC_FS_DEX or NPC_FS_CON or NPC_FS_INT or NPC_FS_WIS or NPC_FS_CHA)	{
		
		If NPC_FS_STR {
			NPCsave:= NPCsave "Str +0, "
		} Else If (NPCstrsav < 0) {
			NPCsave:= NPCsave "Str " NPCstrsav ", "
		} Else If (NPCstrsav > 0) {
			NPCsave:= NPCsave "Str +" NPCstrsav ", "
		}
		If NPC_FS_DEX {
			NPCsave:= NPCsave "Dex +0, "
		} Else If (NPCdexsav < 0) {
			NPCsave:= NPCsave "Dex " NPCdexsav ", "
		} Else If (NPCdexsav > 0) {
			NPCsave:= NPCsave "Dex +" NPCdexsav ", "
		}
		If NPC_FS_CON {
			NPCsave:= NPCsave "Con +0, "
		} Else If (NPCconsav < 0) {
			NPCsave:= NPCsave "Con " NPCconsav ", "
		} Else If (NPCconsav > 0) {
			NPCsave:= NPCsave "Con +" NPCconsav ", "
		}
		If NPC_FS_INT {
			NPCsave:= NPCsave "Int +0, "
		} Else If (NPCintsav < 0) {
			NPCsave:= NPCsave "Int " NPCintsav ", "
		} Else If (NPCintsav > 0) {
			NPCsave:= NPCsave "Int +" NPCintsav ", "
		}
		If NPC_FS_WIS {
			NPCsave:= NPCsave "Wis +0, "
		} Else If (NPCwissav < 0) {
			NPCsave:= NPCsave "Wis " NPCwissav ", "
		} Else If (NPCwissav > 0) {
			NPCsave:= NPCsave "Wis +" NPCwissav ", "
		}
		If NPC_FS_CHA {
			NPCsave:= NPCsave "Cha +0, "
		} Else If (NPCchasav < 0) {
			NPCsave:= NPCsave "Cha " NPCchasav ", "
		} Else If (NPCchasav > 0) {
			NPCsave:= NPCsave "Cha +" NPCchasav ", "
		}
		
		StringTrimRight, NPCsave, NPCsave, 2
	}	

	NPCsense:= ""
	If (NPCblind or NPCdark or NPCtremor or NPCtrue or NPCpassperc)	{
		If NPCblindb {
			sbb:= " (blind beyond this radius)"
		} else {
			sbb:= ""
		}
		If NPCdarkb {
			sdb:= " (blind beyond this radius)"
		} else {
			sdb:= ""
		}
		If NPCtremorb {
			stb:= " (blind beyond this radius)"
		} else {
			stb:= ""
		}
		If NPCtrueb {
			szb:= " (blind beyond this radius)"
		} else {
			szb:= ""
		}
		
		If NPCblind {
			NPCsense:= NPCsense "blindsight " NPCblind " ft." sbb ", "
		}
		If NPCdark {
			NPCsense:= NPCsense "darkvision " NPCdark " ft." sdb ", "
		}
		If NPCtremor {
			NPCsense:= NPCsense "tremorsense " NPCtremor " ft." stb ", "
		}
		If NPCtrue {
			NPCsense:= NPCsense "truesight " NPCtrue " ft." szb ", "
		}
		NPCsense:= NPCsense "passive Perception " NPCpassperc
	}	

	NPCskill:= ""
	For key, value in NPC_Skills {
		If (value > 0) {
			NPCskill:= NPCskill " " key " +" value ","
		}
		If (value < 0) {
			NPCskill:= NPCskill " " key " " value ","
		}
	}
	If NPCskill {
		StringTrimRight, NPCskill, NPCskill, 1
	}

	NPCdamvul:= ""
	If Flagdamvul	{
		Loop, 10
		{
			If cbDV%A_Index% {
				NPCdamvul:= NPCdamvul NPC_damvul[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamvul, NPCdamvul, 2
		If (cbDV11 or cbDV12 or cbDV13) {
			If NPCdamvul
				NPCdamvul:= NPCdamvul "; "
			If (cbDV11 and cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning, piercing, and slashing"
			Else If (cbDV11 and cbDV12)
				NPCdamvul:= NPCdamvul "bludgeoning and piercing"
			Else If (cbDV11 and cbDV13)
				NPCdamvul:= NPCdamvul "bludgeoning and slashing"
			Else If (cbDV12 and cbDV13)
				NPCdamvul:= NPCdamvul "piercing and slashing"
			Else If (cbDV11)
				NPCdamvul:= NPCdamvul "bludgeoning"
			Else If (cbDV12)
				NPCdamvul:= NPCdamvul "piercing"
			Else If (cbDV13)
				NPCdamvul:= NPCdamvul "slashing"
		}
	}
	
	NPCdamres:= ""
	If Flagdamres {
		Loop, 10
		{
			If cbDR%A_Index% {
				NPCdamres:= NPCdamres NPC_damres[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamres, NPCdamres, 2
		If (cbDR11 or cbDR12 or cbDR13) {
			If NPCdamres
			NPCdamres:= NPCdamres "; "
			If (cbDR11 and cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning, piercing, and slashing"
			Else If (cbDR11 and cbDR12)
				NPCdamres:= NPCdamres "bludgeoning and piercing"
			Else If (cbDR11 and cbDR13)
				NPCdamres:= NPCdamres "bludgeoning and slashing"
			Else If (cbDR12 and cbDR13)
				NPCdamres:= NPCdamres "piercing and slashing"
			Else If (cbDR11)
				NPCdamres:= NPCdamres "bludgeoning"
			Else If (cbDR12)
				NPCdamres:= NPCdamres "piercing"
			Else If (cbDR13)
				NPCdamres:= NPCdamres "slashing"
		}
		If DRRadio2
			NPCdamres:= NPCdamres " from nonmagical weapons"
		If DRRadio3
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't silvered"
		If DRRadio4
			NPCdamres:= NPCdamres " from nonmagical weapons that aren't adamantine"
		If DRRadio5
			NPCdamres:= NPCdamres " from magic weapons"
		If DRRadio6
			NPCdamres:= "Damage Resistances " NPCdamres " from nonmagical weapons that aren't cold-forged iron" Chr(10)
	}

	NPCdamimm:= ""
	If Flagdamimm	{
		Loop, 10
		{
			If cbDI%A_Index% {
				NPCdamimm:= NPCdamimm NPC_damimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCdamimm, NPCdamimm, 2
		If (cbDI11 or cbDI12 or cbDI13) {
			If NPCdamimm
			NPCdamimm:= NPCdamimm "; "
			If (cbDI11 and cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning, piercing, and slashing"
			Else If (cbDI11 and cbDI12)
				NPCdamimm:= NPCdamimm "bludgeoning and piercing"
			Else If (cbDI11 and cbDI13)
				NPCdamimm:= NPCdamimm "bludgeoning and slashing"
			Else If (cbDI12 and cbDI13)
				NPCdamimm:= NPCdamimm "piercing and slashing"
			Else If (cbDI11)
				NPCdamimm:= NPCdamimm "bludgeoning"
			Else If (cbDI12)
				NPCdamimm:= NPCdamimm "piercing"
			Else If (cbDI13)
				NPCdamimm:= NPCdamimm "slashing"
		}
		If DIRadio2
			NPCdamimm:= NPCdamimm " from nonmagical weapons"
		If DIRadio3
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't silvered"
		If DIRadio4
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't adamantine"
		If DIRadio5
			NPCdamimm:= NPCdamimm " from nonmagical weapons that aren't cold-forged iron"
	}

	NPCconimm:= ""
	If Flagconimm	{
		Loop, 16
		{
			If cbCI%A_Index% {
				NPCconimm:= NPCconimm NPC_conimm[A_Index] ", "
			}
		}
		StringTrimRight, NPCconimm, NPCconimm, 2
	}	

	NPCinspell:= ""
	If FlagInSpell	{
		NPCinspell:= NU "'s innate spellcasting ability is " NPCinspability
		If NPCinsptohit
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ", +" NPCinsptohit " to hit with spell attacks)."
		else
			NPCinspell:= NPCinspell " (spell save DC " NPCinspsave ")."
		If NPCinsptext {
			localNPCinsptext:= ", " NPCinsptext
		} else {
			localNPCinsptext:= ""
		}
		NPCinspell:= NPCinspell " " GU1 " can innately cast the following spells" localNPCinsptext ":"
		
		Loop % NPC_InSpell_Slots.MaxIndex()
		{
			NPCinspell:= NPCinspell "\r" NPC_InSpell_Slots[A_Index] ": "
			TempLoop:= A_Index
			Loop % NPC_InSpell_Number[TempLoop]
			{
				If (A_Index < NPC_InSpell_Number[TempLoop])
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index] ", "
				else
					NPCinspell:= NPCinspell NPC_InSpell_Names[TempLoop, A_Index]
			}
		}
	}

	NPCspell:= ""
	If FlagSpell	{
		NPCspell:= NU " is a " NPCsplevel "-level spellcaster. " GU3 " spellcasting ability is " NPCspability
		NPCspell:= NPCspell " (spell save DC " NPCspsave ", +" NPCsptohit " to hit with spell attacks)."
		If NPCspflavour
			NPCspell:= NPCspell " " NPCspflavour
		NPCspell:= NPCspell " " NU " has the following " NPCspclass " spells prepared:"
		
		Loop % NPC_Spell_Level.MaxIndex()
		{
			If (NPC_Spell_Level[A_Index] = "Cantrips") {
				NPCspell:= NPCspell "\r" NPC_Spell_Level[A_Index]
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): "
			} Else {
				NPCspell:= NPCspell "\r" NPC_Spell_Level[A_Index] " level"
				NPCspell:= NPCspell " (" NPC_Spell_Slots[A_Index] "): "
			}
			TempLoop:= A_Index
			Loop % NPC_Spell_Number[TempLoop]
			{
				If (A_Index < NPC_Spell_Number[TempLoop])
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index] ", "
				else
					NPCspell:= NPCspell NPC_Spell_Names[TempLoop, A_Index]
			}
		}
		If NPCSpellStar {
			NPCspell:= NPCspell "\r\r" NPCSpellStar
		}
	}

	NPCdescript:= ""
	RefManDesc:= ""
	If Desc_Add_Text {
		NPCdescript .= "`t`t`t`t`t<text type=""formattedtext"">"
		If Desc_NPC_Title {
			NPCdescript .= "`t`t`t`t`t<h>" NPCname "</h>" Chr(10)
		}
		If Desc_Image_Link {
			If (NPCImagePath and NPCjpeg) {
				Ifexist, %NPCImagePath%
				{
					NPCdescript .= "`t`t`t`t`t<link class=""imagewindow"" recordname=""image.img_" NPCjpeg "_jpg@" ModName """>Image: " NPCname "</link>" Chr(10)
				}
			}
		}
		
		temptext:= Validate(RE1.GetRTF(False))
		TKN:= Tokenise(temptext)
		TKN:= "`t`t`t`t`t" TKN
		TKN:= RegExReplace(TKN,"U)\</p\>`n\<p\>\</([a-oq-z])\>","</$1></p>`n<p>")
		StringReplace, TKN, TKN, </p>`n<p>, </p>`n`t`t`t`t`t<p>, all
		StringReplace, TKN, TKN, <p><h>, <h>, all
		StringReplace, TKN, TKN, </h></p>, </h>, all
		StringReplace, TKN, TKN, <p>`r`n<p>, <p>, all
		StringReplace, TKN, TKN, <p><frame>, <frame>, all
		StringReplace, TKN, TKN, </frame></p>, </frame>, all
		StringReplace, TKN, TKN, <p>`r`n</frame>, </p></frame>, all
		StringReplace, TKN, TKN, <p>`r`n`r`n<ul>, <ul>, all
		StringReplace, TKN, TKN, <p>`r`n<ul>, <ul>, all
		StringReplace, TKN, TKN, <p><ul>, <ul>, all
		
		local pos, frs, fre, alldone, newstr, repstr
		pos:= 1					; Check for frames
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
				StringReplace, repstr, repstr, </p>`n`t`t`t`t`t<p>, &#13;&#13;, all
				StringReplace, repstr, repstr, </p>, , all
				StringReplace, temptkn, temptkn, %NewStr%, %repstr%
			} else {
				alldone:= 1
			}
		} until alldone = 1
		TKN:= tempTKN
		
		pos:= 1					; Check for italics
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
				StringReplace, repstr, repstr, </p>`n`t`t`t`t`t<p>, </i></p>`n`t`t`t`t`t<p><i>, all
				StringReplace, tempTKN, tempTKN, %NewStr%, %repstr%
			} else {
				alldone:= 1
			}
		} until alldone = 1
		TKN:= tempTKN

		pos:= 1					; Check for bold
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
				StringReplace, repstr, repstr, </p>`n`t`t`t`t`t<p>, </b></p>`n`t`t`t`t`t<p><b>, all
				StringReplace, temptkn, temptkn, %NewStr%, %repstr%
			} else {
				alldone:= 1
			}
		} until alldone = 1
		TKN:= tempTKN

		pos:= 1					; Check for underline
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
				StringReplace, repstr, repstr, </p>`n`t`t`t`t`t<p>, </u></p>`n`t`t`t`t`t<p><u>, all
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

		TKN:= RegExReplace(TKN, "Us)`n`t`t`t`t`t</frame>`n`t`t`t`t`t<p><b>(?!<i>)(.*)</i></b>", "</i>`n`t`t`t`t`t</frame>`n`t`t`t`t`t<p><b><i>$1</i></b>")

		stringreplace, TKN, TKN, </frame></i></p>, </i></frame>, All
		stringreplace, TKN, TKN, %A_Space%%A_Space%, %A_Space%, All

		TKN:= LinkXML(TKN)

		NPCdescript .= TKN Chr(10)
		RefManDesc:= TKN Chr(10)
		
		If Desc_Spell_List {
			FGSpells()
			NPCdescript .= FGSpell Chr(10)
		}
		NPCdescript .= "`t`t`t`t`t</text>"
	}

	If RefManDesc {
		local frstart1, frstart2, rmmatch, framecont, RM_Frame, idtag1, idtag2, idtag3, tempthing
		StringReplace RefManDesc, RefManDesc, %A_Tab%%A_Tab%%A_Tab%%A_Tab%%A_Tab%, , All
		
		pos:= 1
		alldone:= 0
		idtag1:= 40
		idtag2:= 40
		idtag3:= 40
		tempthing:= RefManDesc
		loop {
			frs:= 0
			fre:= 0
			frs:= InStr(RefManDesc, "<frame>", , pos)
			fre:= InStr(RefManDesc, "</frame>", , pos)

			if (frs and fre) {
				NewStr:= SubStr(RefManDesc, frs, fre-frs+8)
				foundpos:= RegExMatch(NewStr, "O)<frame>(.*)</frame>" , rmmatch)
				framecont:= rmmatch.value(1)
				idtag2:= idtag1 + 10
				idtag3:= idtag1 + 20
		RM_Frame = 
	(
												</text>
											</id0000%idtag1%>
											<id0000%idtag2%>
												<align type="string">left</align>
												<frame type="string">green</frame>
												<text type="formattedtext"><p>%framecont%</p>
												</text>
												<blocktype type="string">text</blocktype>
											</id0000%idtag2%>
											<id0000%idtag3%>
												<blocktype type="string">text</blocktype>
												<align type="string">left</align>
												<text type="formattedtext">
												
	)
				StringReplace, tempthing, tempthing, %NewStr%, %RM_Frame%
				idtag1 += 20
			} else {
				alldone:= 1
			}
			if fre
				pos:= fre + 8
		} until alldone = 1

		RefManDesc:= tempthing
		RefManDesc .= "</text>" Chr(10) "</id0000" idtag3 ">" Chr(10)
	} Else {
		RefManDesc .= "</text>" Chr(10) "</id000040>" Chr(10)
	}
	
	Lang_Set()
	NPClang:= SubStr(NPClang, 11)
	NPClang:= RegExReplace(NPClang,"\s*$","")

	Save_File:= ""
}

NPCEng_Save_File() {
	global
	local TempDest
	if NPCModSaveDir {
		ModSaveDir:= "\" Modname
		TempDest:= NPCPath . ModSaveDir . "\"
		Ifnotexist, %TempDest% 
			FileCreateDir, %TempDest% 
	} Else {
		ModSaveDir:= ""
	}			
	if NPCCopyPics {
		If (NPCTokenPath and NPCjpeg) {
			Ifexist, %NPCTokenPath%
			{
				ThumbDest:= NPCPath . ModSaveDir . "\" . NPCjpeg . ".*"
				FileCopy, %NPCTokenPath%, %ThumbDest%, 1
				NPCTokenPath:= NPCPath . ModSaveDir . "\" . NPCjpeg . ".png"
			}
		}
		If (NPCImagePath and NPCjpeg) {
			Ifexist, %NPCImagePath%
			{
				ThumbDest:= NPCPath . ModSaveDir . "\" . NPCjpeg . ".*"
				FileCopy, %NPCImagePath%, %ThumbDest%, 1
				NPCImagePath:= NPCPath . ModSaveDir . "\" . NPCjpeg . ".jpg"
			}
		}
	}		
			
	NPC_Save_File:= ""
	Name_Work()
	Par5e_Out()
	rtfix:= RE1.GetRTF(False)
	rtfix:= RegExReplace(rtfix, "U)\{\\fonttbl.*\}\}", "{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}")
	NPC_Save_File:= NPC_Save_File . "***Part 1***" Chr(10)
	NPC_Save_File:= NPC_Save_File . SaveMainBlock Chr(10)
	NPC_Save_File:= NPC_Save_File . "***Part 2***" Chr(10)
	NPC_Save_File:= NPC_Save_File . rtfix Chr(10)
	NPC_Save_File:= NPC_Save_File . "***Part 3***" Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPCgender: " . NPCgender Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPCunique: " . NPCunique Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPCpropername: " . NPCpropername Chr(10)
	NPC_Save_File:= NPC_Save_File . "Desc_Add_Text: " . Desc_Add_Text Chr(10)
	NPC_Save_File:= NPC_Save_File . "Desc_NPC_Title: " . Desc_NPC_Title Chr(10)
	NPC_Save_File:= NPC_Save_File . "Desc_Image_Link: " . Desc_Image_Link Chr(10)
	NPC_Save_File:= NPC_Save_File . "Desc_Spell_List: " . Desc_Spell_List Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPCimagePath: " . NPCimagePath Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPCTokenPath: " . NPCTokenPath Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPCImArt: " . NPCImArt Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPCImLink: " . NPCImLink Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPCNoID: " . NPCNoID Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPC_FS_STR: " . NPC_FS_STR Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPC_FS_DEX: " . NPC_FS_DEX Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPC_FS_CON: " . NPC_FS_CON Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPC_FS_INT: " . NPC_FS_INT Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPC_FS_WIS: " . NPC_FS_WIS Chr(10)
	NPC_Save_File:= NPC_Save_File . "NPC_FS_CHA: " . NPC_FS_CHA Chr(10)
	Num:= NPC_Lair_Actions.MaxIndex()
	NPC_Save_File:= NPC_Save_File . "NoLActions: " . Num Chr(10)
	Loop % NPC_Lair_Actions.MaxIndex()
	{
		Nam:= NPC_Lair_Actions[A_Index, "Name"]
		NPC_Save_File:= NPC_Save_File . "LAction" A_Index ": " . Nam Chr(10)
	}
	NPC_Save_File:= NPC_Save_File . "NPCRTF: " . True Chr(10)
	NPC_Save_File:= NPC_Save_File . "Terrain: " . sort.terrain Chr(10)
	NPC_Save_File:= NPC_Save_File . "Lore: " . sort.lore Chr(10)
	NPC_Save_File:= NPC_Save_File . "FGcat: " . FGcat Chr(10)
}	

XML_Save_File() {   ; Written by Zamrod for Fight Club XML
	global
	Save_File:= ""
	Save_File:= Save_File "<?xml version=""1.0"" encoding=""UTF-8""?>" Chr(10) Chr(10)
	Save_File:= Save_File "<!-- NPC built by NPC Engineer. -->" Chr(10)
	Save_File:= Save_File "<!--      Written by Maasq      -->" Chr(10) Chr(10)
	Save_File:= Save_File "<compendium version=""5"" auto_indent=""NO"">" Chr(10)
	Save_File:= Save_File "<monster>" Chr(10)
	Save_File:= Save_File Build_Save_File("name", NPCname)
	SizeStr := SubStr(NPCsize, 1 , 1)
	Save_File:= Save_File Build_Save_File("size", SizeStr)
	Save_File:= Save_File Build_Save_File("type", NPCtype)
	Save_File:= Save_File Build_Save_File("alignment", NPCalign)
	Save_File:= Save_File Build_Save_File("ac", NPCac)
	StringReplace HPStr, NPChp, %A_Space%+%A_Space%, + , All
	Save_File:= Save_File Build_Save_File("hp", HPStr)
	
	Save_File:= Save_File "`t<speed>"
	Speed_String:= ""
	Speed_String:= Speed_String NPCwalk " ft."
	If (NPCburrow)
	{
	Speed_String:= Speed_String ", burrow " NPCburrow " ft."
	}
		If (NPCclimb)
	{
	Speed_String:= Speed_String ", climb " NPCclimb " ft."
	}
			If (NPCfly)
	{
	Speed_String:= Speed_String ", fly " NPCfly " ft."
	if (NPChover)
	{
		Speed_String:= Speed_String " (hover)"
	}
	}
				If (NPCswim)
	{
	Speed_String:= Speed_String ", swim " NPCswim " ft."
	}
	Save_File:= Save_File Speed_String
	Save_File:= Save_File "</speed>" Chr(10)
	Save_File:= Save_File "`t<str>" NPCstr "</str>"
	Save_File:= Save_File "<dex>" NPCdex "</dex>"
	Save_File:= Save_File "<con>" NPCcon "</con>"
	Save_File:= Save_File "<int>" NPCint "</int>"
	Save_File:= Save_File "<wis>" NPCwis "</wis>"
	Save_File:= Save_File "<cha>" NPCcha "</cha>" Chr(10)

	Save_File:= Save_File "`t<save>"
	Saves_String:= ""
	If (NPCstrsav)
	{
	Saves_String:= Saves_String "Str +" NPCstrsav
	}
	If (NPCdexsav)
	{
		if (Saves_String)
		{
		Saves_String:= Saves_String ", "
		}
	Saves_String:= Saves_String "Dex +" NPCdexsav
	}
	If (NPCconsav)
	{
		if (Saves_String)
		{
		Saves_String:= Saves_String ", "
		}
	Saves_String:= Saves_String "Con +" NPCconsav
	}
	If (NPCintsav)
	{
		if (Saves_String)
		{
		Saves_String:= Saves_String ", "
		}
	Saves_String:= Saves_String "Int +" NPCintsav
	}
	If (NPCwissav)
	{
		if (Saves_String)
		{
		Saves_String:= Saves_String ", "
		}
	Saves_String:= Saves_String "Wis +" NPCwissav
	}
	If (NPCchasav)
	{
	if (Saves_String)
	{
		Saves_String:= Saves_String ", "
	}
	Saves_String:= Saves_String "Cha +" NPCchasav
	}
	Save_File:= Save_File Saves_String
	Save_File:= Save_File "</save>" Chr(10)
	
	Save_File:= Save_File "`t<skill>"
	Skills_String:= ""
		For key, value in NPC_Skills {
			StringReplace, key, key, %A_SPACE%, _, All
	If (value)
	{
		if (Skills_String)
		{
		Skills_String:= Skills_String ", "
		}
	Skills_String:= Skills_String key " +" value
	}
			}
	Save_File:= Save_File Skills_String
	Save_File:= Save_File "</skill>" Chr(10)

	stringreplace NPCdamres, NPCdamres, Damage Resistances%A_Space%, 
	stringreplace NPCdamres, NPCdamres, `n, 
	stringreplace NPCdamres, NPCdamres, `r, 
	Save_File:= Save_File Build_Save_File("resist", NPCdamres)
	
	stringreplace NPCdamvul, NPCdamvul, Damage Vulnerabilities%A_Space%, 
	stringreplace NPCdamvul, NPCdamvul, `n, 
	stringreplace NPCdamvul, NPCdamvul, `r, 
	Save_File:= Save_File Build_Save_File("vulnerable", NPCdamvul)

	stringreplace NPCdamimm, NPCdamimm, Damage Immunities%A_Space%, 
	stringreplace NPCdamimm, NPCdamimm, `n, 
	stringreplace NPCdamimm, NPCdamimm, `r, 
	Save_File:= Save_File Build_Save_File("immune", NPCdamimm)
	
	stringreplace NPCconimm, NPCconimm, Condition Immunities%A_Space%, 
	stringreplace NPCconimm, NPCconimm, `n, 
	stringreplace NPCconimm, NPCconimm, `r, 
	Save_File:= Save_File Build_Save_File("conditionImmune", NPCconimm)
	
	Save_File:= Save_File "`t<senses>"
	Senses_String:= ""
	If (NPCblind)
	{
		if (Senses_String)
		{
		Senses_String:= Senses_String ", "
		}
	Senses_String:= Senses_String "blindsight " NPCblind " ft."
	}
	If (NPCdark)
	{
		if (Senses_String)
		{
		Senses_String:= Senses_String ", "
		}
	Senses_String:= Senses_String "darkvision " NPCdark " ft."
	}
	If (NPCtremor)
	{
		if (Senses_String)
		{
		Senses_String:= Senses_String ", "
		}
	Senses_String:= Senses_String "tremorsense " NPCtremor " ft."
	}
	If (NPCtrue)
	{
		if (Senses_String)
		{
		Senses_String:= Senses_String ", "
		}
	Senses_String:= Senses_String "truesight " NPCtrue " ft."
	}
	Save_File:= Save_File Senses_String
	Save_File:= Save_File "</senses>" Chr(10)
	
	Save_File:= Save_File Build_Save_File("passive", NPCpassperc, 1)

	Save_File:= Save_File "`t<languages>"
	Languages_String:= ""
	For key, value in NPCLanguages
	{
		if (Languages_String)
		{
		Languages_String:= Languages_String ", "
		}
		Languages_String:= Languages_String value
	}
	If NPCtelep {
		if (Languages_String)
		{
		Languages_String:= Languages_String ", "
		}
		Languages_String:= Languages_String "telepathy " telrange
	}
	Save_File:= Save_File Languages_String
	Save_File:= Save_File "</languages>" Chr(10)

	Save_File:= Save_File Build_Save_File("cr", NPCcharat)
	
	Loop % NPC_traits.Length()	{
		p_tag1:= "name"
		p_tag2:= "text"
		p_move1:= NPC_traits[A_Index, "Name"]
		p_move2:= NPC_traits[A_Index, "trait"]
		Save_File:= Save_File "`t<trait>" Chr(10)
		Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
			Save_File:= Save_File "`t</trait>" Chr(10)
	}
	
	if (NPCsplevel>0)
	{
		;msgBox, "Found Spellcasting"
		Save_File:= Save_File "`t<trait>" Chr(10)
		p_tag1:= "name"
		p_tag2:= "text"
		p_move2:= NPCname " is a level " NPCsplevel " spellcaster. Its spellcasting ability is " NPCspability " (spell save DC " NPCspsave "; +" NPCsptohit " to hit with spell attacks). The " NPCname " has the following " NPCspclass " spells prepared:"
		Save_File:= Save_File Build_Save_File(p_tag1, "Spellcasting", 2)
		Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
		Loop % NPC_Spell_Level.MaxIndex()	{
		;Save_File:= Save_File Build_Save_File(p_tag2, " ", 2)
		Save_File:= Save_File "`t`t<text>&#8226; " NPC_Spell_Level[A_Index] " (" NPC_Spell_Slots[A_Index] "): "
		TempLoop:= A_Index
		CurrentSpellLevel_String:= ""
		Loop % NPC_Spell_Number[TempLoop]
		{
		If (CurrentSpellLevel_String)
		{
			CurrentSpellLevel_String:= CurrentSpellLevel_String ", "
		}
		CurrentSpellLevel_String:= CurrentSpellLevel_String NPC_Spell_Names[TempLoop, A_Index]
		}
		Save_File:= Save_File CurrentSpellLevel_String "</text>" Chr(10)
	}
		Save_File:= Save_File "`t</trait>" Chr(10)
	}
	
	if (NPCinspability)
	{
		Save_File:= Save_File "`t<trait>" Chr(10)
		p_tag1:= "name"
		p_tag2:= "text"
		p_move2:= NPCname "'s innate spellcasting ability is " NPCinspability " (spell save DC " NPCinspsave "; +" NPCinsptohit " to hit with spell attacks). " NPCname " can innately cast the following spells, requiring no material components:"
		Save_File:= Save_File Build_Save_File(p_tag1, "Innate Spellcasting", 2)
		Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
		Loop % NPC_InSpell_Slots.MaxIndex()	{
		;Save_File:= Save_File Build_Save_File(p_tag2, " ", 2)
		Save_File:= Save_File "`t`t<text>&#8226; " NPC_InSpell_Slots[A_Index] ": "
		TempLoop:= A_Index
		CurrentSpellLevel_String:= ""
		Loop % NPC_InSpell_Number[TempLoop]
		{
		If (CurrentSpellLevel_String)
		{
			CurrentSpellLevel_String:= CurrentSpellLevel_String ", "
		}
		CurrentSpellLevel_String:= CurrentSpellLevel_String NPC_InSpell_Names[TempLoop, A_Index]
		}
		Save_File:= Save_File CurrentSpellLevel_String "</text>" Chr(10)
	}
		Save_File:= Save_File "`t</trait>" Chr(10)
	}

	Loop % NPC_Actions.Length()	{
		p_tag1:= "name"
		p_tag2:= "text"
		Save_File:= Save_File "`t<action>" Chr(10)
		p_move1:= NPC_Actions[A_Index, "Name"]
		p_move2:= NPC_Actions[A_Index, "Action"]
		Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
		If (InStr(p_move2, " to hit", false))
		{
			AttackLocation_Num:=InStr(p_move2, " to hit", false, 1, 1)
			AttackBonus := SubStr(p_move2, AttackLocation_Num-1 , 1)
			Parse_Attack_for_output(A_Index)
			Save_File:= Save_File "`t`t<attack>" p_move1 "|" CurrentWA_ToHit "|" CurrentWA_Damage "</attack>" Chr(10)
		}
		Save_File:= Save_File "`t</action>" Chr(10)
	}
	
	if (NPC_Lair_Actions.Length())
	{
	  Save_File:= Save_File "`t<action>" Chr(10)
		Save_File:= Save_File "`t`t<name>Lair Actions</name>" Chr(10)
	Loop % NPC_Lair_Actions.Length()	{
		p_tag1:= "text"
		p_tag2:= "text"
		p_move1:= " "
		p_move2:= NPC_Lair_Actions[A_Index, "LairAction"]
		Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		Save_File:= Save_File Build_Save_File(p_tag2, "&#8226; " p_move2, 2)
		}
		Save_File:= Save_File "`t</action>" Chr(10)
	}
	
	Loop % NPC_Reactions.Length()	{
		p_tag1:= "name"
		p_tag2:= "text"
		p_move1:= NPC_Reactions[A_Index, "Name"]
		p_move2:= NPC_Reactions[A_Index, "Reaction"]
		Save_File:= Save_File "`t<reaction>" Chr(10)
		Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
		Save_File:= Save_File "`t</reaction>" Chr(10)
	}
	
	Loop % NPC_Legendary_Actions.Length()	{
		p_tag1:= "name"
		p_tag2:= "text"
		p_move1:= NPC_Legendary_Actions[A_Index, "Name"]
		p_move2:= NPC_Legendary_Actions[A_Index, "LegAction"]
		Save_File:= Save_File "`t<legendary>" Chr(10)
		Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
		Save_File:= Save_File "`t</legendary>" Chr(10)
	}

	Save_File:= Save_File "`t<spells>"
	Spells_String:= ""
	Loop % NPC_Spell_Level.MaxIndex()	{
		TempLoop:= A_Index
		Loop % NPC_Spell_Number[TempLoop]
		{
			if (Spells_String)
			{
				Spells_String:= Spells_String ", "
			}
		Spells_String:= Spells_String NPC_Spell_Names[TempLoop, A_Index]
		}
	}
	StringReplace Spells_String, Spells_String,*, , All
	Save_File:= Save_File Spells_String
if (NPCinspability)
{
	CurrentInSpellLevel_String:= ""
	Loop % NPC_InSpell_Slots.MaxIndex()	{
		TempLoop:= A_Index
		Loop % NPC_InSpell_Number[TempLoop]
		{
			If (CurrentInSpellLevel_String)
			{
			CurrentInSpellLevel_String:= CurrentInSpellLevel_String ", "
			}
			CurrentInSpellLevel_String:= CurrentInSpellLevel_String NPC_InSpell_Names[TempLoop, A_Index]
		}
}
 Save_File:= Save_File CurrentInSpellLevel_String
} 
	Save_File:= Save_File "</spells>" Chr(10)
	Save_File:= Save_File "`t<slots>"
	SpellSlot_String:= ""
	Loop % NPC_Spell_Level.MaxIndex()	{
		SpellSlotAmount_String:= NPC_Spell_Slots[A_Index]
		StringReplace SpellSlot_Num, SpellSlotAmount_String, %A_Space%slots, , All
		StringReplace SpellSlot_Num, SpellSlot_Num, %A_Space%slot, , All
		if (SpellSlot_Num <> "At will")
		{
			if (SpellSlot_String) {
				SpellSlot_String:= SpellSlot_String ", "
			}
			SpellSlot_String:= SpellSlot_String SpellSlot_Num
		}
	}
	Save_File:= Save_File SpellSlot_String
	Save_File:= Save_File "</slots>" Chr(10)
	Save_File:= Save_File "</monster>" Chr(10)
	Save_File:= Save_File "</compendium>"
}	

OLD_XML_Save_File() {  ; By Maasq; deprecated.
	;~ global
	;~ Save_File:= ""
	;~ Save_File:= Save_File "<?xml version=""1.0"" encoding=""UTF-8""?>" Chr(10) Chr(10)
	;~ Save_File:= Save_File "<!-- NPC built by NPC Engineer. -->" Chr(10)
	;~ Save_File:= Save_File "<!--      Written by Maasq      -->" Chr(10) Chr(10)
	;~ Save_File:= Save_File "<NPC>" Chr(10)
	;~ Save_File:= Save_File Build_Save_File("name", NPCname)
	;~ Save_File:= Save_File Build_Save_File("gender", NPCgender)
	;~ Save_File:= Save_File Build_Save_File("unique", NPCunique)
	;~ Save_File:= Save_File Build_Save_File("propername", NPCpropername)
	;~ Save_File:= Save_File Build_Save_File("size", NPCsize)
	;~ Save_File:= Save_File Build_Save_File("type", NPCtype)
	;~ Save_File:= Save_File Build_Save_File("tag", NPCtag)
	;~ Save_File:= Save_File Build_Save_File("alignment", NPCalign)
	;~ Save_File:= Save_File Build_Save_File("armorclass", NPCac)
	;~ Save_File:= Save_File Build_Save_File("hitpoints", NPChp)
	
	;~ Save_File:= Save_File "`t<movement>" Chr(10)
	;~ Save_File:= Save_File Build_Save_File("Speed", NPCwalk, 2)
	;~ Save_File:= Save_File Build_Save_File("Burrow", NPCburrow, 2)
	;~ Save_File:= Save_File Build_Save_File("Climb", NPCclimb, 2)
	;~ Save_File:= Save_File Build_Save_File("Fly", NPCfly, 2)
	;~ Save_File:= Save_File Build_Save_File("Hover", NPChover, 3)
	;~ Save_File:= Save_File Build_Save_File("Swim", NPCswim, 2)
	;~ Save_File:= Save_File "`t</movement>" Chr(10)
	
	;~ Save_File:= Save_File "`t<ability_scores>" Chr(10)
	;~ Save_File:= Save_File Build_Save_File("str", NPCstr, 2)
	;~ Save_File:= Save_File Build_Save_File("dex", NPCdex, 2)
	;~ Save_File:= Save_File Build_Save_File("con", NPCcon, 2)
	;~ Save_File:= Save_File Build_Save_File("int", NPCint, 2)
	;~ Save_File:= Save_File Build_Save_File("wis", NPCwis, 2)
	;~ Save_File:= Save_File Build_Save_File("cha", NPCcha, 2)
	;~ Save_File:= Save_File "`t</ability_scores>" Chr(10)

	;~ Save_File:= Save_File "`t<saving_throws>" Chr(10)
	;~ Save_File:= Save_File Build_Save_File("Str", NPCstrsav, 2)
	;~ Save_File:= Save_File Build_Save_File("Dex", NPCdexsav, 2)
	;~ Save_File:= Save_File Build_Save_File("Con", NPCconsav, 2)
	;~ Save_File:= Save_File Build_Save_File("Int", NPCintsav, 2)
	;~ Save_File:= Save_File Build_Save_File("Wis", NPCwissav, 2)
	;~ Save_File:= Save_File Build_Save_File("Cha", NPCchasav, 2)
	;~ Save_File:= Save_File "`t</saving_throws>" Chr(10)
	
	;~ Save_File:= Save_File "`t<skills>" Chr(10)
		;~ For key, value in NPC_Skills {
			;~ StringReplace, key, key, %A_SPACE%, _, All
			;~ Save_File:= Save_File Build_Save_File(key, value, 2)
			;~ }
	;~ Save_File:= Save_File "`t</skills>" Chr(10)

	;~ stringreplace NPCdamvul, NPCdamvul, Damage Vulnerabilities%A_Space%, 
	;~ stringreplace NPCdamvul, NPCdamvul, `n, 
	;~ stringreplace NPCdamvul, NPCdamvul, `r, 
	;~ Save_File:= Save_File Build_Save_File("damage_vulnerabilities", NPCdamvul)

	;~ stringreplace NPCdamres, NPCdamres, Damage Resistances%A_Space%, 
	;~ stringreplace NPCdamres, NPCdamres, `n, 
	;~ stringreplace NPCdamres, NPCdamres, `r, 
	;~ Save_File:= Save_File Build_Save_File("damage_resistances", NPCdamres)

	;~ stringreplace NPCdamimm, NPCdamimm, Damage Immunities%A_Space%, 
	;~ stringreplace NPCdamimm, NPCdamimm, `n, 
	;~ stringreplace NPCdamimm, NPCdamimm, `r, 
	;~ Save_File:= Save_File Build_Save_File("damage_immunities", NPCdamimm)
	
	;~ stringreplace NPCconimm, NPCconimm, Condition Immunities%A_Space%, 
	;~ stringreplace NPCconimm, NPCconimm, `n, 
	;~ stringreplace NPCconimm, NPCconimm, `r, 
	;~ Save_File:= Save_File Build_Save_File("condition_immunities", NPCconimm)
	
	;~ Save_File:= Save_File "`t<senses>" Chr(10)
	;~ Save_File:= Save_File Build_Save_File("blindsight", NPCblind, 2)
	;~ Save_File:= Save_File Build_Save_File("darkvision", NPCdark, 2)
	;~ Save_File:= Save_File Build_Save_File("tremorsense", NPCtremor, 2)
	;~ Save_File:= Save_File Build_Save_File("truesight", NPCtrue, 2)
	;~ Save_File:= Save_File Build_Save_File("passive_perception", NPCpassperc, 2)
	;~ Save_File:= Save_File "`t</senses>" Chr(10)

	;~ Save_File:= Save_File "`t<languages>" Chr(10)
	;~ For key, value in NPCLanguages
		;~ Save_File:= Save_File Build_Save_File("value", value, 2)
	;~ If NPCtelep {
		;~ Save_File:= Save_File Build_Save_File("telepathy", telrange, 2)
	;~ }
		
	;~ Save_File:= Save_File "`t</languages>" Chr(10)

	;~ Save_File:= Save_File Build_Save_File("challenge", NPCcharat)
	;~ Save_File:= Save_File Build_Save_File("xp", NPCxp)
	
	;~ Save_File:= Save_File "`t<traits>" Chr(10)
	;~ Loop % NPCtraits.Length()	{
		;~ p_tag1:= "Name_" A_Index
		;~ p_tag2:= "Value_" A_Index
		;~ p_move1:= NPCtraits[A_Index, "Name"]
		;~ p_move2:= NPCtraits[A_Index, "trait"]
		;~ Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		;~ Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
	;~ }
	;~ Save_File:= Save_File "`t</traits>" Chr(10)

	;~ If 	NPCPsionics
		;~ Save_File:= Save_File "`t<innate_spellcasting_psionics>" Chr(10)
	;~ Else
		;~ Save_File:= Save_File "`t<innate_spellcasting>" Chr(10)
	
	;~ Save_File:= Save_File Build_Save_File("innate_spellcasting_ability", NPCinspability, 2)
	;~ Save_File:= Save_File Build_Save_File("innate_spell_save_dc", NPCinspsave, 2)
	;~ Save_File:= Save_File Build_Save_File("innate_to_hit_bonus", NPCinsptohit, 2)
	;~ Loop % NPC_InSpell_Slots.MaxIndex()	{
		;~ Save_File:= Save_File Build_Save_File("innate_spell_slots", NPC_InSpell_Slots[A_Index], 2)
		;~ TempLoop:= A_Index
		;~ Loop % NPC_InSpell_Number[TempLoop]
		;~ {
			;~ p_tag2:= "spell_" A_Index
			;~ p_move2:= NPC_InSpell_Names[TempLoop, A_Index]
			;~ Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 3)
		;~ }
	;~ }
	;~ Save_File:= Save_File "`t</innate_spellcasting>" Chr(10)
	
	;~ Save_File:= Save_File "`t<spellcasting>" Chr(10)
	;~ Save_File:= Save_File Build_Save_File("casting_level", NPCsplevel, 2)
	;~ Save_File:= Save_File Build_Save_File("spellcasting_ability", NPCspability, 2)
	;~ Save_File:= Save_File Build_Save_File("spell_save_dc", NPCspsave, 2)
	;~ Save_File:= Save_File Build_Save_File("to_hit_bonus", NPCsptohit, 2)
	;~ Save_File:= Save_File Build_Save_File("spell_flavour_text", NPCspflavour, 2)
	;~ Save_File:= Save_File Build_Save_File("spell_class", NPCspclass, 2)
	;~ Loop % NPC_Spell_Level.MaxIndex()	{
		;~ Save_File:= Save_File Build_Save_File("spell_level", NPC_Spell_Level[A_Index], 2)
		;~ Save_File:= Save_File Build_Save_File("spell_slots", NPC_Spell_Slots[A_Index], 3)
		;~ TempLoop:= A_Index
		;~ Loop % NPC_Spell_Number[TempLoop]
		;~ {
			;~ p_tag2:= "spell_" A_Index
			;~ p_move2:= NPC_Spell_Names[TempLoop, A_Index]
			;~ Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 3)
		;~ }
	;~ }
	;~ Save_File:= Save_File "`t</spellcasting>" Chr(10)
	
	;~ Save_File:= Save_File "`t<Actions>" Chr(10)
	;~ Loop % NPC_Actions.Length()	{
		;~ p_tag1:= "Name_" A_Index
		;~ p_tag2:= "Value_" A_Index
		;~ p_move1:= NPC_Actions[A_Index, "Name"]
		;~ p_move2:= NPC_Actions[A_Index, "Action"]
		;~ Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		;~ Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
	;~ }
	;~ Save_File:= Save_File "`t</Actions>" Chr(10)
	
	;~ Save_File:= Save_File "`t<Reactions>" Chr(10)
	;~ Loop % NPC_Reactions.Length()	{
		;~ p_tag1:= "Name_" A_Index
		;~ p_tag2:= "Value_" A_Index
		;~ p_move1:= NPC_Reactions[A_Index, "Name"]
		;~ p_move2:= NPC_Reactions[A_Index, "Reaction"]
		;~ Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		;~ Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
	;~ }
	;~ Save_File:= Save_File "`t</Reactions>" Chr(10)
	
	;~ Save_File:= Save_File "`t<Legendary_Actions>" Chr(10)
	;~ Loop % NPC_LegActions.Length()	{
		;~ p_tag1:= "Name_" A_Index
		;~ p_tag2:= "Value_" A_Index
		;~ p_move1:= NPC_Legendary_Actions[A_Index, "Name"]
		;~ p_move2:= NPC_Legendary_Actions[A_Index, "LegAction"]
		;~ Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		;~ Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
	;~ }
	;~ Save_File:= Save_File "`t</Legendary_Actions>" Chr(10)

	;~ Save_File:= Save_File "`t<Lair_Actions>" Chr(10)
	;~ Loop % NPC_LairActions.Length()	{
		;~ p_tag1:= "Name_" A_Index
		;~ p_tag2:= "Value_" A_Index
		;~ p_move1:= NPC_Lair_Actions[A_Index, "Name"]
		;~ p_move2:= NPC_Lair_Actions[A_Index, "LairAction"]
		;~ Save_File:= Save_File Build_Save_File(p_tag1, p_move1, 2)
		;~ Save_File:= Save_File Build_Save_File(p_tag2, p_move2, 2)
	;~ }
	;~ Save_File:= Save_File "`t</Lair_Actions>" Chr(10)

	;~ Save_File:= Save_File Build_Save_File("image_filename", NPCjpeg)
	;~ Save_File:= Save_File Build_Save_File("token_path", NPCtokenpath)
	;~ Save_File:= Save_File Build_Save_File("description", Box_Desc_Text)
	;~ Save_File:= Save_File "</NPC>" Chr(10)
}	

Build_Save_File(Tag, Data, Level:= "1") {
	If (Level = 1)
		FileLine:=  "`t<" Tag ">" Data "</" Tag ">" Chr(10)
	
	If (Level = 2)
		FileLine:=  "`t`t<" Tag ">" Data "</" Tag ">" Chr(10)
	
	If (Level = 3)
		FileLine:=  "`t`t`t<" Tag ">" Data "</" Tag ">" Chr(10)

	Return FileLine
}

NPCEng_Do_Save() {
	global
	savecheck:= 0
	if NPCCopyPics {
		If NPCJpeg {
			SelectedFile:= NPCPath ModSaveDir "\" NPCjpeg ".npc"
			If FileExist(SelectedFile)
				FileDelete, %SelectedFile%
			FileAppend, %NPC_Save_File%, %SelectedFile%
			NPCSavePath:= SelectedFile
			savecheck:= 1
		}
	} else {
		FileSelectFile, SelectedFile, S2, %NPCname%.npc, Save NPC, Text Files (*.npc)
		SplitPath, SelectedFile, f_name, f_dir, f_ext, f_name_no_ext, f_drive
		If SelectedFile {
			if (f_ext != "npc")
			{
				SelectedFile = %SelectedFile%.npc
				f_name = %f_name%.npc
			}
			If FileExist(SelectedFile)
			{
				MsgBox, 52, Confirm Save As..., %f_name% already exists.`nDo you want to replace it?
				IfMsgBox Yes
				{
					FileDelete, %SelectedFile%
					FileAppend, %NPC_Save_File%, %SelectedFile%
				}
			}
			else
				FileAppend, %NPC_Save_File%, %SelectedFile%
			NPCSavePath:= SelectedFile
			savecheck:= 1
		}
	}
}

XML_Do_Save() {
	global
	FileSelectFile, SelectedFile, S2, %NPCjpeg%.xml, Save NPC, XML Files (*.xml)
	SplitPath, SelectedFile, f_name, f_dir, f_ext, f_name_no_ext, f_drive
	If SelectedFile {
		if (f_ext != "xml")
		{
			SelectedFile = %SelectedFile%.xml
			f_name = %f_name%.xml
		}
		If FileExist(SelectedFile)
		{
			MsgBox, 52, Confirm Save As..., %f_name% already exists.`nDo you want to replace it?
			IfMsgBox Yes
			{
				FileDelete, %SelectedFile%
				FileAppend, %Save_File%, %SelectedFile%
			}
		}
		else
		{
			FileAppend, %Save_File%, %SelectedFile%
		}
	}
}

HTML_Do_Save() {
	global
	FileSelectFile, SelectedFile, S2, %NPCjpeg%.html, Save NPC, HTML Files (*.html)
	SplitPath, SelectedFile, f_name, f_dir, f_ext, f_name_no_ext, f_drive
	If SelectedFile {
		if (f_ext != "html")
		{
			SelectedFile = %SelectedFile%.html
			f_name = %f_name%.html
		}
		If FileExist(SelectedFile)
		{
			MsgBox, 52, Confirm Save As..., %f_name% already exists.`nDo you want to replace it?
			IfMsgBox Yes
			{
				FileDelete, %SelectedFile%
				FileAppend, %HTML_Stat_BlockH%, %SelectedFile%
			}
		}
		else
		{
			FileAppend, %HTML_Stat_BlockH%, %SelectedFile%
		}
	}
}

RTF_Do_Save() {
	global
	FileSelectFile, SelectedFile, S2, %NPCjpeg%.rtf, Save NPC, RTF Files (*.rtf)
	SplitPath, SelectedFile, f_name, f_dir, f_ext, f_name_no_ext, f_drive
	If SelectedFile {
		if (f_ext != "rtf")
		{
			SelectedFile = %SelectedFile%.rtf
			f_name = %f_name%.rtf
		}
		If FileExist(SelectedFile)
		{
			MsgBox, 52, Confirm Save As..., %f_name% already exists.`nDo you want to replace it?
			IfMsgBox Yes
			{
				FileDelete, %SelectedFile%
				FileAppend, %CopyBlock%, %SelectedFile%
			}
		}
		else
		{
			FileAppend, %CopyBlock%, %SelectedFile%
		}
	}
}

NPC_Load() {
	Global
	local check, TempDest
	Critical
	If (flags.NPC = 0) {
		ModSaveDir:= "\" Modname
		TempDest:= NPCPath . ModSaveDir
		FileSelectFile, SelectedFile, 2, %TempDest%, Load NPC, (*.npc)
		NPCSavePath:= SelectedFile
	} else {
		 SelectedFile:= NPCSavePath
		 flags.NPC:= 0
		 check:= 1
	}
	
	If (FileExist(SelectedFile)) {
		TempPP:= 0
		Fileread, LoadedFile, %SelectedFile%
		
		pdid:= 1
		Gosub New_NPC
		NPCSavePath:= SelectedFile
		
		StringReplace LoadedFile, LoadedFile, `r`n, -.-, All
		StringReplace LoadedFile, LoadedFile, `n, -.-, All
		StringReplace LoadedFile, LoadedFile, -.-, `r`n, All

		LF_1:= Instr(LoadedFile, "***Part 1***")
		LF_2:= Instr(LoadedFile, "***Part 2***")
		LF_3:= Instr(LoadedFile, "***Part 3***")
		
		ImportChoice:= "PDF & Web source"
		Workingstring := SubStr(LoadedFile, LF_1 + 14, LF_2 - LF_1 - 14)
		Main_Loop()
		tempPP:= NPCpassperc

		if (InStr(LoadedFile, "NPCRTF:")) {
			RTF := SubStr(LoadedFile, LF_2 + 14, LF_3 - LF_2 - 14)
			rtfix:= RegExReplace(RTF, "U)\{\\fonttbl.*\}\}", "{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}")
			RE1.SetText(rtfix, ["KEEPUNDO"])
		} Else {
			Chosen_Desc_Text := SubStr(LoadedFile, LF_2 + 14, LF_3 - LF_2 - 14)
			GuiControl, NPCE_Main:, Chosen_Desc_Text, %Chosen_Desc_Text%
		}

		
		LF_3_Text := SubStr(LoadedFile, LF_3 + 14)
		LF_3_Text:= RegExReplace(LF_3_Text,"\s*$","")
		
		NoLActions:=""
		Terrain:= ""
		Lore:= ""
		NPCImArt:= ""
		NPCImLink:= ""
		NPCNoID:= ""
		FGcat:= Modname

		NPC_FS_STR:= 0
		NPC_FS_DEX:= 0
		NPC_FS_CON:= 0
		NPC_FS_INT:= 0
		NPC_FS_WIS:= 0
		NPC_FS_CHA:= 0

		Loop, parse, LF_3_Text, `n, `r
		{
			FoundPos:= InStr(A_LoopField, ":") 
			varnameholder:= SubStr(A_LoopField, 1, FoundPos-1)
			%varnameholder%:= SubStr(A_LoopField, FoundPos+2)
		}
	
		Gosub Load_NPCToken
		Gosub Load_NPCImage
		
		If (NoLActions > 0) {
			Loop %NoLActions%
			{
				var:= "LAction" A_Index
				NPC_Lair_Actions[A_Index, "Name"]:= %var%
				%var%:= ""
			}
			lractionworks()
			lractionworks2()
		}
		If Terrain {
			StringReplace terrain, terrain, Polar, Arctic
			StringReplace terrain, terrain, Subterranean, Underdark
			sort.terrain:= terrain
		}
		If Lore
			sort.lore:= lore
		
		pdid:= 0
		
		Inject_Vars()
		Parser()
	}
}

NPCEParse_Save_File() {
	global
	Build_NPC_Object()
	ObjNPC:= {}
	ObjName:= NPCjpeg
	ObjNPC[ObjName]:= {}
	ObjNPC[ObjName].name:= NPCName
	ObjNPC[ObjName].type:= NPCtypetag
	ObjNPC[ObjName].alignment:= NPCalign
	ObjNPC[ObjName].size:= NPCsize
	ObjNPC[ObjName].ac:= trim(StrReplace(NPCacnum, ",", "") + 0)
	ObjNPC[ObjName].actext:= NPCactxt
	ObjNPC[ObjName].hp:= trim(StrReplace(NPChpnum, ",", "") + 0)
	ObjNPC[ObjName].hd:= NPChphd
	ObjNPC[ObjName].speed:= NPCspeed

	ObjNPC[ObjName].strength:= trim(StrReplace(NPCstr, ",", "") + 0)
	ObjNPC[ObjName].strengthmod:= NPCstrbo
	ObjNPC[ObjName].dexterity:= trim(StrReplace(NPCdex, ",", "") + 0)
	ObjNPC[ObjName].dexteritymod:= NPCdexbo
	ObjNPC[ObjName].constitution:= trim(StrReplace(NPCcon, ",", "") + 0)
	ObjNPC[ObjName].constitutionmod:= NPCconbo
	ObjNPC[ObjName].intelligence:= trim(StrReplace(NPCint, ",", "") + 0)
	ObjNPC[ObjName].intelligencemod:= NPCintbo
	ObjNPC[ObjName].wisdom:= trim(StrReplace(NPCwis, ",", "") + 0)
	ObjNPC[ObjName].wisdommod:= NPCwisbo
	ObjNPC[ObjName].charisma:= trim(StrReplace(NPCcha, ",", "") + 0)
	ObjNPC[ObjName].charismamod:= NPCchabo

	ObjNPC[ObjName].savingthrows:= NPCsave
	ObjNPC[ObjName].skills:= NPCskill
	ObjNPC[ObjName].damvul:= NPCdamvul
	ObjNPC[ObjName].damres:= NPCdamres
	ObjNPC[ObjName].damimm:= NPCdamimm
	ObjNPC[ObjName].conimm:= NPCconimm
	ObjNPC[ObjName].senses:= NPCsense
	ObjNPC[ObjName].languages:= NPClang
	ObjNPC[ObjName].cr:= NPCcharat
	ObjNPC[ObjName].xp:= trim(StrReplace(NPCxp, ",", "") + 0)

	ObjNPC[ObjName].castinginnate:= NPCinspell
	ObjNPC[ObjName].casting:= NPCspell
	ObjNPC[ObjName].psionics:= NPCPsionics

	ObjNPC[ObjName].traitnumber:= NPC_Traits.Length()
	Loop % NPC_Traits.Length()
	{
		trnam:= "trait" a_index
		trtxt:= "trait" a_index "text"
		ObjNPC[ObjName][trnam]:= NPC_Traits[A_Index, "Name"]
		ObjNPC[ObjName][trtxt]:= NPC_Traits[A_Index, "trait"]
	}
	
	ObjNPC[ObjName].actionnumber:= NPC_Actions.Length()
	Loop % NPC_Actions.Length()
	{
		trnam:= "action" a_index
		trtxt:= "action" a_index "text"
		ObjNPC[ObjName][trnam]:= NPC_Actions[A_Index, "Name"]
		ObjNPC[ObjName][trtxt]:= NPC_Actions[A_Index, "Action"]
	}
	
	ObjNPC[ObjName].reactionnumber:= NPC_Reactions.Length()
	Loop % NPC_Reactions.Length()
	{
		trnam:= "reaction" a_index
		trtxt:= "reaction" a_index "text"
		ObjNPC[ObjName][trnam]:= NPC_Reactions[A_Index, "Name"]
		ObjNPC[ObjName][trtxt]:= NPC_Reactions[A_Index, "Reaction"]
	}
	
	ObjNPC[ObjName].legendaryactionnumber:= NPC_Legendary_Actions.Length()
	Loop % NPC_Legendary_Actions.Length()
	{
		trnam:= "legendaryaction" a_index
		trtxt:= "legendaryaction" a_index "text"
		ObjNPC[ObjName][trnam]:= NPC_Legendary_Actions[A_Index, "Name"]
		ObjNPC[ObjName][trtxt]:= NPC_Legendary_Actions[A_Index, "LegAction"]
	}
	
	ObjNPC[ObjName].lairactionnumber:= NPC_Lair_Actions.Length()
	Loop % NPC_Lair_Actions.Length()
	{
		trnam:= "lairaction" a_index
		trtxt:= "lairaction" a_index "text"
		ObjNPC[ObjName][trnam]:= NPC_Lair_Actions[A_Index, "Name"]
		ObjNPC[ObjName][trtxt]:= NPC_Lair_Actions[A_Index, "LairAction"]
	}
	
	ObjNPC[ObjName].description:= NPCdescript
	ObjNPC[ObjName].editabledescription:= RE1.GetRTF(False)
	ObjNPC[ObjName].token:= "tokens/" ModName "/" NPCjpeg ".png@" ModName
	ObjNPC[ObjName].terrain:= sort.terrain
	ObjNPC[ObjName].lore:= sort.lore

	If Desc_Image_Link {
		local x, y, w, h
		gui, temporary:add, picture, hwndimagehwnd, %NPCimagePath%
		ControlGetPos, x, y, w, h, , ahk_id %imagehwnd% 
		gui, temporary:destroy
		X:= 500
		y:= 350
		If (w/h > x/y) {
			y:= floor(x / w * h)
			picdim:= x "," y
			StringReplace XML_Stat_Block, XML_Stat_Block, picdimpicdim, %picdim%
		} else {
			x:= floor(y / h * w)
			picdim:= x "," y
			StringReplace XML_Stat_Block, XML_Stat_Block, picdimpicdim, %picdim%
		}
	} else {
		StringReplace XML_Stat_Block, XML_Stat_Block, picdimpicdim, 100`,100
	}
	
	StringReplace XML_Stat_Block, XML_Stat_Block, RefManDesc, %RefManDesc%
	
	ObjNPC[ObjName].XML:= XML_Stat_Block
	ObjNPC[ObjName].artist:= NPCImArt
	ObjNPC[ObjName].artistlink:= NPCImLink
	ObjNPC[ObjName].NoID:= NPCNoID
	ObjNPC[ObjName].FGcat:= FGcat
	NPCEP_Out()
}

Edit_NPC_JSON(ed) {
	global
	local build
	build:= ""
	build:= build NPC[ed].name Chr(13) Chr(10)
	build:= build NPC[ed].size " " NPC[ed].type ", " NPC[ed].alignment Chr(13) Chr(10)
	build:= build "Armor Class " NPC[ed].ac " " NPC[ed].actext Chr(13) Chr(10)
	build:= build "Hit Points " NPC[ed].hp " " NPC[ed].hd Chr(13) Chr(10)
	build:= build "Speed " NPC[ed].speed Chr(13) Chr(10)

	build:= build "STR DEX CON INT WIS CHA" Chr(13) Chr(10)
	build:= build NPC[ed].strength " (" NPC[ed].strengthmod ") "
	build:= build NPC[ed].dexterity " (" NPC[ed].dexteritymod ") "
	build:= build NPC[ed].constitution " (" NPC[ed].constitutionmod ") "
	build:= build NPC[ed].intelligence " (" NPC[ed].intelligencemod ") "
	build:= build NPC[ed].wisdom " (" NPC[ed].wisdommod ") "
	build:= build NPC[ed].charisma " (" NPC[ed].charismamod ")" Chr(13) Chr(10)

	build:= build "Saving Throws " NPC[ed].savingthrows Chr(13) Chr(10)
	build:= build "Skills " NPC[ed].skills Chr(13) Chr(10)
	If NPC[ed].damvul
		build:= build "Damage Vulnerabilities " NPC[ed].damvul Chr(13) Chr(10)
	If NPC[ed].damres
		build:= build "Damage Resistances " NPC[ed].damres Chr(13) Chr(10)
	If NPC[ed].damimm
		build:= build "Damage Immunities " NPC[ed].damimm Chr(13) Chr(10)
	If NPC[ed].conimm
		build:= build "Condition Immunities " NPC[ed].conimm Chr(13) Chr(10)
	If NPC[ed].senses
		build:= build "Senses " NPC[ed].senses Chr(13) Chr(10)
	If NPC[ed].languages
		build:= build "Languages " NPC[ed].languages Chr(13) Chr(10)
	build:= build "Challenge " NPC[ed].cr " (" NPC[ed].xp " XP)" Chr(13) Chr(10)

	If NPC[ed].castinginnate
		If 	NPC[ed].psionics
			build:= build "Innate Spellcasting (Psionics). "NPC[ed].castinginnate Chr(13) Chr(10)
		Else
			build:= build "Innate Spellcasting. "NPC[ed].castinginnate Chr(13) Chr(10)
	If NPC[ed].casting
		build:= build "Spellcasting. " NPC[ed].casting Chr(13) Chr(10)

	lp:= 0
	lp:= NPC[ed].traitnumber
	If lp {
		loop, %lp% {
			trt:= "trait" A_Index
			trttxt:= "trait" A_Index "text"
			build:= build NPC[ed][trt] ". " NPC[ed][trttxt] Chr(13) Chr(10)
		}
	}

	lp:= 0
	lp:= NPC[ed].actionnumber
	If lp {
		build:= build "Actions" Chr(13) Chr(10)
		loop, %lp% {
			trt:= "action" A_Index
			trttxt:= "action" A_Index "text"
			build:= build NPC[ed][trt] ". " NPC[ed][trttxt] Chr(13) Chr(10)
		}
	}
	
	lp:= 0
	lp:= NPC[ed].reactionnumber
	If lp {
		build:= build "Reactions" Chr(13) Chr(10)
		loop, %lp% {
			trt:= "reaction" A_Index
			trttxt:= "reaction" A_Index "text"
			build:= build NPC[ed][trt] ". " NPC[ed][trttxt] Chr(13) Chr(10)
		}
	}
	
	lp:= 0
	lp:= NPC[ed].legendaryactionnumber
	If lp {
		build:= build "Legendary Actions" Chr(13) Chr(10)
		loop, %lp% {
			trt:= "legendaryaction" A_Index
			trttxt:= "legendaryaction" A_Index "text"
			build:= build NPC[ed][trt] ". " NPC[ed][trttxt] Chr(13) Chr(10)
		}
	}
	
	lp:= 0
	lp:= NPC[ed].lairactionnumber
	If lp {
		build:= build "Lair Actions" Chr(13) Chr(10)
		loop, %lp% {
			trt:= "lairaction" A_Index
			trttxt:= "lairaction" A_Index "text"
			build:= build NPC[ed][trt] ". " NPC[ed][trttxt] Chr(13) Chr(10)
		}
	}
	
	TempRTF:= NPC[ed].editabledescription
	If TempRTF {
		RTF:= NPC[ed].editabledescription
	} else {
		RTF:= NPC[ed].description
	}
	Workingstring := build
	Main_Loop()
	sort.terrain:= NPC[ed].terrain
	sort.lore:= NPC[ed].lore
	NPCImArt:= NPC[ed].artist
	NPCImLink:= NPC[ed].artistlink
	NPCNoID:= NPC[ed].NoID
	FGcat:= NPC[ed].FGcat
	Inject_Vars()
	RE1.SetText(RTF, ["KEEPUNDO"])
	Parser()

	NPCImagePath:= ModPath . "input\images\" . NPCjpeg . ".jpg"
	NPCTokenPath:= ModPath . "input\tokens\" . NPCjpeg . ".png"
	Gosub Load_NPCToken
	Gosub Load_NPCImage

	if NPCModSaveDir {
		ModSaveDir:= "\" Modname
		TempDest:= NPCPath . ModSaveDir . "\"
		Ifnotexist, %TempDest% 
			FileCreateDir, %TempDest% 
	} Else {
		ModSaveDir:= ""
	}			
	NPCSavePath:= NPCPath ModSaveDir "\" ed ".npc"
}

;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |           General Purpose Functions          |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Cpp(doc) {
	Clipsave:= Clipboard
	Clipboard:= Doc
	Send ^v
	Clipboard:= Clipsave
}

Clp(ByRef ClipPrint) {
	ClipSaved:= Clipboard
	Clipboard:= ClipPrint
	Send ^v
	Clipboard:= ClipSaved
}

StatBon(Staat) {
	Staat := staat - 10
	Staat := staat/2
	Staat := Floor(staat)
	If Staat >= 0
		Staat := "+" Staat
	Return Staat
}

Empty_Variables() {
	global
	initialise()
	Declare_Vars()
}

GuiButtonIcon(Handle, File, Index := 1, Options := "") {
	RegExMatch(Options, "i)w\K\d+", W), (W="") ? W := 16 :
	RegExMatch(Options, "i)h\K\d+", H), (H="") ? H := 16 :
	RegExMatch(Options, "i)s\K\d+", S), S ? W := H := S :
	RegExMatch(Options, "i)l\K\d+", L), (L="") ? L := 0 :
	RegExMatch(Options, "i)t\K\d+", T), (T="") ? T := 0 :
	RegExMatch(Options, "i)r\K\d+", R), (R="") ? R := 0 :
	RegExMatch(Options, "i)b\K\d+", B), (B="") ? B := 0 :
	RegExMatch(Options, "i)a\K\d+", A), (A="") ? A := 4 :
	Psz := A_PtrSize = "" ? 4 : A_PtrSize, DW := "UInt", Ptr := A_PtrSize = "" ? DW : "Ptr"
	VarSetCapacity( button_il, 20 + Psz, 0 )
	NumPut( normal_il := DllCall( "ImageList_Create", DW, W, DW, H, DW, 0x21, DW, 1, DW, 1 ), button_il, 0, Ptr )	; Width & Height
	NumPut( L, button_il, 0 + Psz, DW )		; Left Margin
	NumPut( T, button_il, 4 + Psz, DW )		; Top Margin
	NumPut( R, button_il, 8 + Psz, DW )		; Right Margin
	NumPut( B, button_il, 12 + Psz, DW )	; Bottom Margin	
	NumPut( A, button_il, 16 + Psz, DW )	; Alignment
	SendMessage, BCM_SETIMAGELIST := 5634, 0, &button_il,, AHK_ID %Handle%
	return IL_Add( normal_il, File, Index )
}

getHBMinfo( hBM ) {
Local SzBITMAP := ( A_PtrSize = 8 ? 32 : 24 ),  BITMAP := VarSetCapacity( BITMAP, SzBITMAP )       
  If DllCall( "GetObject", "Ptr",hBM, "Int",SzBITMAP, "Ptr",&BITMAP )
    Return {  Width:      Numget( BITMAP, 4, "UInt"  ),  Height:     Numget( BITMAP, 8, "UInt"  ) 
           ,  WidthBytes: Numget( BITMAP,12, "UInt"  ),  Planes:     Numget( BITMAP,16, "UShort") 
           ,  BitsPixel:  Numget( BITMAP,18, "UShort"),  bmBits:     Numget( BITMAP,20, "UInt"  ) }
}       

ScaleRect( SW, SH, TW, TH, Upscale := 0 ) { ; By SKAN | Created: 19-July-2017  

Local  SAF := SW/SH, TAF := TW/TH ; Aspect ratios of Source and Target
  Return  ( !Upscale and SW <= TW and SH <= TH ) ? {W: SW, H: SH}  
      :   ( SAF < TAF ) ? { W: Floor( ( TW / TAF ) * SAF ), H: TH}
      :   ( SAF > TAF ) ? { W: TW, H: Floor( ( TH / SAF ) * TAF )} 
      :   { W: TW, H: TH }
}

Inject_Vars() {
	global
	local genum
	critical
	GuiControl, NPCE_Main:Text, NPCname, %NPCname%
	genum:= 1
	If (NPCgender = "male") {
		genum:= 2
	}
	If (NPCgender = "female") {
		genum:= 3
	}

	GuiControl, NPCE_Main:Choose, NPCgender, %genum%
	GuiControl, NPCE_Main:, NPCunique, %NPCunique%
	GuiControl, NPCE_Main:, NPCpropername, %NPCpropername%
	GuiControl, NPCE_Main:Text, NPCsize, %NPCsize%
	GuiControl, NPCE_Main:Text, NPCtype, %NPCtype%
	GuiControl, NPCE_Main:Text, NPCtag, %NPCtag%
	GuiControl, NPCE_Main:Text, NPCalign, %NPCalign%
	GuiControl, NPCE_Main:Text, NPChp, %NPChp%
	GuiControl, NPCE_Main:Text, NPCac, %NPCac%
	GuiControl, NPCE_Main:, NPCwalk, %NPCwalk%
	GuiControl, NPCE_Main:, NPCburrow, %NPCburrow%
	GuiControl, NPCE_Main:, NPCclimb, %NPCclimb%
	GuiControl, NPCE_Main:, NPCfly, %NPCfly%
	GuiControl, NPCE_Main:, NPChover, %NPChover%
	GuiControl, NPCE_Main:, NPCswim, %NPCswim%
	GuiControl, NPCE_Main:, NPCstr, %NPCstr%
	GuiControl, NPCE_Main:, NPCdex, %NPCdex%
	GuiControl, NPCE_Main:, NPCcon, %NPCcon%
	GuiControl, NPCE_Main:, NPCint, %NPCint%
	GuiControl, NPCE_Main:, NPCwis, %NPCwis%
	GuiControl, NPCE_Main:, NPCcha, %NPCcha%
	GuiControl, NPCE_Main:, NPCstrbo, %NPCstrbo%
	GuiControl, NPCE_Main:, NPCdexbo, %NPCdexbo%
	GuiControl, NPCE_Main:, NPCconbo, %NPCconbo%
	GuiControl, NPCE_Main:, NPCintbo, %NPCintbo%
	GuiControl, NPCE_Main:, NPCwisbo, %NPCwisbo%
	GuiControl, NPCE_Main:, NPCchabo, %NPCchabo%
	GuiControl, NPCE_Main:, NPCstrsav, %NPCstrsav%
	GuiControl, NPCE_Main:, NPCdexsav, %NPCdexsav%
	GuiControl, NPCE_Main:, NPCconsav, %NPCconsav%
	GuiControl, NPCE_Main:, NPCintsav, %NPCintsav%
	GuiControl, NPCE_Main:, NPCwissav, %NPCwissav%
	GuiControl, NPCE_Main:, NPCchasav, %NPCchasav%
	GuiControl, NPCE_Main:, NPC_FS_STR, %NPC_FS_STR%
	GuiControl, NPCE_Main:, NPC_FS_DEX, %NPC_FS_DEX%
	GuiControl, NPCE_Main:, NPC_FS_CON, %NPC_FS_CON%
	GuiControl, NPCE_Main:, NPC_FS_INT, %NPC_FS_INT%
	GuiControl, NPCE_Main:, NPC_FS_WIS, %NPC_FS_WIS%
	GuiControl, NPCE_Main:, NPC_FS_CHA, %NPC_FS_CHA%
	GuiControl, NPCE_Main:, NPCblind, %NPCblind%
	GuiControl, NPCE_Main:, NPCdark, %NPCdark%
	GuiControl, NPCE_Main:, NPCtremor, %NPCtremor%
	GuiControl, NPCE_Main:, NPCtrue, %NPCtrue%
	GuiControl, NPCE_Main:, NPCblindb, %NPCblindb%
	GuiControl, NPCE_Main:, NPCdarkb, %NPCdarkb%
	GuiControl, NPCE_Main:, NPCtremorb, %NPCtremorb%
	GuiControl, NPCE_Main:, NPCtrueb, %NPCtrueb%
	GuiControl, NPCE_Main:, NPCpassperc, %NPCpassperc%
	GuiControl, NPCE_Main:Text, NPCcharat, %NPCcharat%
	GuiControl, NPCE_Main:Text, NPCxp, %NPCxp%
	GuiControl, NPCE_Main:Text, FGcat, %FGcat%
	
	GuiControl, NPCE_Main:, sk_acro, % NPC_Skills["Acrobatics"]
	GuiControl, NPCE_Main:, sk_anim, % NPC_Skills["Animal Handling"]
	GuiControl, NPCE_Main:, sk_arca, % NPC_Skills["Arcana"]
	GuiControl, NPCE_Main:, sk_athl, % NPC_Skills["Athletics"]
	GuiControl, NPCE_Main:, sk_dece, % NPC_Skills["Deception"]
	GuiControl, NPCE_Main:, sk_hist, % NPC_Skills["History"]
	GuiControl, NPCE_Main:, sk_insi, % NPC_Skills["Insight"]
	GuiControl, NPCE_Main:, sk_inti, % NPC_Skills["Intimidation"]
	GuiControl, NPCE_Main:, sk_inve, % NPC_Skills["Investigation"]
	GuiControl, NPCE_Main:, sk_medi, % NPC_Skills["Medicine"]
	GuiControl, NPCE_Main:, sk_natu, % NPC_Skills["Nature"]
	GuiControl, NPCE_Main:, sk_perc, % NPC_Skills["Perception"]
	GuiControl, NPCE_Main:, sk_perf, % NPC_Skills["Performance"]
	GuiControl, NPCE_Main:, sk_pers, % NPC_Skills["Persuasion"]
	GuiControl, NPCE_Main:, sk_reli, % NPC_Skills["Religion"]
	GuiControl, NPCE_Main:, sk_slei, % NPC_Skills["Sleight of Hand"]
	GuiControl, NPCE_Main:, sk_stea, % NPC_Skills["Stealth"]
	GuiControl, NPCE_Main:, sk_surv, % NPC_Skills["Survival"]

	Arch1:= "|"
	Arch2:= "|"
	Arch3:= "|"
	Arch4:= "|"
	For key, val in LangStan
	{
		If Hasval(NPCLanguages, val)
			Arch1 .= val "||"
		else
			Arch1 .= val "|"
	}
	For key, val in LangExot
	{
		If Hasval(NPCLanguages, val)
			Arch2 .= val "||"
		else
			Arch2 .= val "|"
	}
	For key, val in LangMons
	{
		If Hasval(NPCLanguages, val)
			Arch3 .= val "||"
		else
			Arch3 .= val "|"
	}
	For key, val in LangUser
	{
		If Hasval(NPCLanguages, val)
			Arch4 .= val "||"
		else
			Arch4 .= val "|"
	}

	GuiControl, NPCE_Main:-Redraw, Lang1
	GuiControl, NPCE_Main:-Redraw, Lang2
	GuiControl, NPCE_Main:-Redraw, Lang3
	GuiControl, NPCE_Main:-Redraw, UserLangs
	
	GuiControl, NPCE_Main:, Lang1, %Arch1%
	GuiControl, NPCE_Main:, Lang2, %Arch2%
	GuiControl, NPCE_Main:, Lang3, %Arch3%
	GuiControl, NPCE_Main:, UserLangs, %Arch4%

	GuiControl, NPCE_Main:+Redraw, Lang1
	GuiControl, NPCE_Main:+Redraw, Lang2
	GuiControl, NPCE_Main:+Redraw, Lang3
	GuiControl, NPCE_Main:+Redraw, UserLangs


	GuiControl, NPCE_Main:Choose, LangSelect, %LangSelect%
	GuiControl, NPCE_Main:, LangAlt, %LangAlt%
	GuiControl, NPCE_Main:, NPCtelep, %NPCtelep%
	GuiControl, NPCE_Main:, telrange, %telrange%

	Lang_Set()

	Loopvar:= 1
	While (Loopvar < 14) {
		Tempvar:= cbDV%loopvar%
		GuiControl, NPCE_Main:, cbDV%loopvar%, %Tempvar%
		Tempvar:= cbDR%loopvar%
		GuiControl, NPCE_Main:, cbDR%Loopvar%, %Tempvar%
		Tempvar:= cbDI%loopvar%
		GuiControl, NPCE_Main:, cbDI%Loopvar%, %Tempvar%
		Tempvar:= cbCI%loopvar%
		GuiControl, NPCE_Main:, cbCI%Loopvar%, %Tempvar%
		Loopvar += 1
	}
	Loopvar:= 13
	While (Loopvar < 17) {
		Tempvar:= cbCI%loopvar%
		GuiControl, NPCE_Main:, cbCI%Loopvar%, %Tempvar%
		Loopvar += 1
	}

	GuiControl, NPCE_Main:, CI16, %VCI16%
	
	GuiControl, NPCE_Main:, traitnamenew, %traitnamenew%
	GuiControl, NPCE_Main:, traitnew, %traitnew%
	

	GuiControl, NPCE_Main:, DRRadio1, %DRRadio1%
	GuiControl, NPCE_Main:, DRRadio2, %DRRadio2%
	GuiControl, NPCE_Main:, DRRadio3, %DRRadio3%
	GuiControl, NPCE_Main:, DRRadio4, %DRRadio4%
	GuiControl, NPCE_Main:, DRRadio5, %DRRadio5%
	GuiControl, NPCE_Main:, DRRadio6, %DRRadio6%

	GuiControl, NPCE_Main:, DIRadio1, %DIRadio1%
	GuiControl, NPCE_Main:, DIRadio2, %DIRadio2%
	GuiControl, NPCE_Main:, DIRadio3, %DIRadio3%
	GuiControl, NPCE_Main:, DIRadio4, %DIRadio4%
	GuiControl, NPCE_Main:, DIRadio5, %DIRadio5%
	
	GuiControl, NPCE_Main:, FlagInSpell, %FlagInSpell%
	GuiControl, NPCE_Main:, NPCPsionics, %NPCPsionics%
	
	GuiControl, NPCE_Main:Text, NPCinspability, %NPCinspability%
	GuiControl, NPCE_Main:, NPCinspsave, %NPCinspsave%
	GuiControl, NPCE_Main:, NPCinsptohit, %NPCinsptohit%
	GuiControl, NPCE_Main:, NPCinsptext, %NPCinsptext%
	
	Loop % NPC_InSpell_Slots.MaxIndex()
	{
		Tempone:= NPC_InSpell_Slots[A_Index]
		StringLeft, Tempone, Tempone, 1
		If (Tempone = "A")
			Tempone:= 0
		InSp_%Tempone%_spells:= ""
		TempLoop:= A_Index
		Loop % NPC_InSpell_Number[TempLoop]
		{
			If (A_Index < NPC_InSpell_Number[TempLoop])
				InSp_%Tempone%_spells:= InSp_%Tempone%_spells NPC_InSpell_Names[TempLoop, A_Index] ", "
			else
				InSp_%Tempone%_spells:= InSp_%Tempone%_spells NPC_InSpell_Names[TempLoop, A_Index]
		}
		temptwo:= InSp_%Tempone%_spells
		GuiControl, NPCE_Main:, InSp_%Tempone%_spells, %TempTwo%
	}
	
	GuiControl, NPCE_Main:, FlagSpell, %FlagSpell%
	
	GuiControl, NPCE_Main:Text, NPCsplevel, %NPCsplevel%
	GuiControl, NPCE_Main:Text, NPCspability, %NPCspability%
	GuiControl, NPCE_Main:, NPCspsave, %NPCspsave%
	GuiControl, NPCE_Main:, NPCsptohit, %NPCsptohit%
	GuiControl, NPCE_Main:, NPCspflavour, %NPCspflavour%
	GuiControl, NPCE_Main:Text, NPCspclass, %NPCspclass%
	GuiControl, NPCE_Main:, NPCSpellStar, %NPCSpellStar%
	
	Loop % NPC_Spell_Level.MaxIndex()
	{
		;~ Tempone:= A_Index-1
		Tempone:= NPC_Spell_Level[A_Index]
		StringLeft, Tempone, Tempone, 1
		If (Tempone = "C")
			Tempone:= 0
		Sp_%Tempone%_spells:= ""
		GuiControl, NPCE_Main:Text, Sp_%Tempone%_casts, % NPC_Spell_Slots[A_Index]
		TempLoop:= A_Index
		Loop % NPC_Spell_Number[TempLoop]
		{
		If (A_Index < NPC_Spell_Number[TempLoop])
			Sp_%Tempone%_spells:= Sp_%Tempone%_spells NPC_Spell_Names[TempLoop, A_Index] ", "
		else
			Sp_%Tempone%_spells:= Sp_%Tempone%_spells NPC_Spell_Names[TempLoop, A_Index]
		}
		temptwo:= Sp_%Tempone%_spells
		GuiControl, NPCE_Main:, Sp_%Tempone%_spells, %TempTwo%
	}

	Check:= 1
	For key, value in NPC_Traits
	{
		traitname%Check%:= NPC_Traits[key, "name"]
		trait%Check%:= NPC_Traits[key, "trait"]
		TempOne:= traitname%Check%
		TempTwo:= trait%Check%
		GuiControl, NPCE_Main:, traitname%Check%, %TempOne%
		GuiControl, NPCE_Main:, trait%Check%, %TempTwo%
		Check += 1
	}
	
	Actionworks2()
	Actionworks3()
	reActionworks()
	lgactionworks()
	lgactionworks2()
	lrActionworks()
	lrActionworks2()
	
	GuiControl, NPCE_Main:, Desc_Add_Text, %Desc_Add_Text%
	GuiControl, NPCE_Main:, Desc_NPC_Title, %Desc_NPC_Title%
	GuiControl, NPCE_Main:, Desc_Image_Link, %Desc_Image_Link%
	GuiControl, NPCE_Main:, Desc_Spell_List, %Desc_Spell_List%
	GuiControl, NPCE_Main:, Desc_fixes, %Desc_fixes%
	GuiControl, NPCE_Main:, Desc_strip_lf, %Desc_strip_lf%
	GuiControl, NPCE_Main:, Desc_title, %Desc_title%
	GuiControl, NPCE_Main:, NPCImArt, %NPCImArt%
	GuiControl, NPCE_Main:, NPCImLink, %NPCImLink%
	GuiControl, NPCE_Main:, NPCNoID, %NPCNoID%
	
	
	Graphical()
}

Tag_Titles(ByRef TitleBold) {
	global
	Loop, parse, Titlebold, `n, `r 
	{
		Holding:= A_Loopfield
		If Holding	{
			SentenceArray := StrSplit(Holding, ".")				
			InnerHolding:= SentenceArray[1] "."
			StringReplace, InnerHolding, InnerHolding, <p>, 
			RepHolding:= InnerHolding
			RegExReplace(RepHolding, "\w+", "", Count)
			If (Count > 0 and Count < 5) {
				InnerHolding2:= "<b><i>" InnerHolding "</i></b>"
				StringReplace, Titlebold, Titlebold, %InnerHolding%, %InnerHolding2%
			}
		}
	}

}

Tag_Title_2(ByRef TitleBold) {
	global
	Loop, parse, Titlebold, `n, `r 
	{
		Holding:= A_Loopfield
		If Holding	{
			SentenceArray := StrSplit(Holding, ".")				
			InnerHolding:= SentenceArray[1] "."
			RepHolding:= InnerHolding
			RepHolding:= RegExReplace(RepHolding, "U)\(.*\)", "")
			RegExReplace(RepHolding, "\w+", "", Count)
			If (Count > 5) {
				InnerHolding2:= "\r" InnerHolding
				StringReplace, Titlebold, Titlebold, %InnerHolding%, %InnerHolding2%
			}
		}
	}

}

PassPer() {
	global
	Critical
	NPCpassperc:= 10
	If (NPC_Skills["Perception"] > NPCwisbo) {
		NPCpassperc += NPC_Skills["Perception"]
	} else {
		NPCpassperc += NPCwisbo
	}
	If TempPP
		NPCpassperc:= TempPP
	GuiControl, NPCE_Main:, NPCpassperc, %NPCpassperc%
	TempPP:= 0
}

Skill_Set() {
	global
	NPC_Skills["Acrobatics"]:= sk_acro
	NPC_Skills["Animal Handling"]:= sk_anim
	NPC_Skills["Arcana"]:= sk_arca
	NPC_Skills["Athletics"]:= sk_athl
	NPC_Skills["Deception"]:= sk_dece
	NPC_Skills["History"]:= sk_hist
	NPC_Skills["Insight"]:= sk_insi
	NPC_Skills["Intimidation"]:= sk_inti
	NPC_Skills["Investigation"]:= sk_inve
	NPC_Skills["Medicine"]:= sk_medi
	NPC_Skills["Nature"]:= sk_natu
	NPC_Skills["Perception"]:= sk_perc
	NPC_Skills["Performance"]:= sk_perf
	NPC_Skills["Persuasion"]:= sk_pers
	NPC_Skills["Religion"]:= sk_reli
	NPC_Skills["Sleight of Hand"]:= sk_slei
	NPC_Skills["Stealth"]:= sk_stea
	NPC_Skills["Survival"]:= sk_surv
}

Lang_Set() {
	global
	If (LangSelect = 1) {
		NPClang:= "Languages"
		sortarray(NPCLanguages)
		For key, value in NPCLanguages
		{
			NPClang .= " " value ","
		}
		If NPCtelep {
			NPClang:= NPClang " telepathy " telrange
		}
		If (SubStr(NPClang, 0, 1) = ",") {
			StringTrimRight, NPClang, NPClang, 1
		}
		NPClang:= NPClang Chr(10)
	}
	If (LangSelect = 2) {
		Arch1:= "|"
		Arch2:= "|"
		Arch3:= "|"
		Arch4:= "|"
		For key, val in LangStan
			Arch1 .= val "|"
		For key, val in LangExot
			Arch2 .= val "|"
		For key, val in LangMons
			Arch3 .= val "|"
		For key, val in LangUser
			Arch4 .= val "|"

		GuiControl, NPCE_Main:, Lang1, %Arch1%
		GuiControl, NPCE_Main:, Lang2, %Arch2%
		GuiControl, NPCE_Main:, Lang3, %Arch3%
		GuiControl, NPCE_Main:, UserLangs, %Arch4%

		
		NPCtelep:= 0
		telrange:= ""
		vNPCtelep.set(NPCtelep)
		vtelrange.set(telrange)
		NPClang:= "Languages --" Chr(10)
	}
	If (LangSelect = 3) {
		Arch1:= "|"
		Arch2:= "|"
		Arch3:= "|"
		Arch4:= "|"
		For key, val in LangStan
			Arch1 .= val "||"
		For key, val in LangExot
			Arch2 .= val "||"
		For key, val in LangMons
			Arch3 .= val "||"
		For key, val in LangUser
			Arch4 .= val "||"

		GuiControl, NPCE_Main:, Lang1, %Arch1%
		GuiControl, NPCE_Main:, Lang2, %Arch2%
		GuiControl, NPCE_Main:, Lang3, %Arch3%
		GuiControl, NPCE_Main:, UserLangs, %Arch4%
		NPClang:= "Languages All"
		If NPCtelep {
			NPClang:= NPClang ", telepathy " telrange
		}
		NPClang:= NPClang Chr(10)
	}
	If (LangSelect = 4) {
		NPClang:= "Languages The languages it knew in life"
		If NPCtelep {
			NPClang:= NPClang ", telepathy " telrange
		}
		NPClang:= NPClang Chr(10)
	}
	If (LangSelect = 5) {
		Loopvar:= 1
		NPClang:= "Languages Understands"
		sortarray(NPCLanguages)
		For key, value in NPCLanguages
		{
			NPClang .= " " value ","
		}
		If NPCtelep {
			NPClang:= NPClang " telepathy " telrange
		}
		If (SubStr(NPClang, 0, 1) = ",") {
			StringTrimRight, NPClang, NPClang, 1
		}
		NPClang:= NPClang " but can't speak"
		If NPCtelep {
			NPClang:= NPClang ", telepathy " telrange
		}
		NPClang:= NPClang Chr(10)
	}
	If (LangSelect = 6) {
		NPClang:= "Languages Understands the languages of its creator but can't speak"
		If NPCtelep {
			NPClang:= NPClang ", telepathy " telrange
		}
		NPClang:= NPClang Chr(10)
	}
	If (LangSelect = 7) {
		NPClang:= "Languages Understands the languages it knew in life but can't speak"
		If NPCtelep {
			NPClang:= NPClang ", telepathy " telrange
		}
		NPClang:= NPClang Chr(10)
	}
	If (LangSelect = 8) {
		NPClang:= "Languages " LangAlt
		If NPCtelep {
			NPClang:= NPClang ", telepathy " telrange
		}
		NPClang:= NPClang Chr(10)
	}
	
}

Build_SW() {
	global
	SpellWork:= ""
	If Sp_0_spells {
		SpellWork:= "Cantrips (" Sp_0_casts "): " Sp_0_spells Chr(10)
	}
	If Sp_1_spells {
		SpellWork:= SpellWork "1st level (" Sp_1_casts "): " Sp_1_spells Chr(10)
	}
	If Sp_2_spells {
		SpellWork:= SpellWork "2nd level (" Sp_2_casts "): " Sp_2_spells Chr(10)
	}
	If Sp_3_spells {
		SpellWork:= SpellWork "3rd level (" Sp_3_casts "): " Sp_3_spells Chr(10)
	}
	If Sp_4_spells {
		SpellWork:= SpellWork "4th level (" Sp_4_casts "): " Sp_4_spells Chr(10)
	}
	If Sp_5_spells {
		SpellWork:= SpellWork "5th level (" Sp_5_casts "): " Sp_5_spells Chr(10)
	}
	If Sp_6_spells {
		SpellWork:= SpellWork "6th level (" Sp_6_casts "): " Sp_6_spells Chr(10)
	}
	If Sp_7_spells {
		SpellWork:= SpellWork "7th level (" Sp_7_casts "): " Sp_7_spells Chr(10)
	}
	If Sp_8_spells {
		SpellWork:= SpellWork "8th level (" Sp_8_casts "): " Sp_8_spells Chr(10)
	}
	If Sp_9_spells {
		SpellWork:= SpellWork "9th level (" Sp_9_casts "): " Sp_9_spells Chr(10)
	}
}

Build_ISW() {
	global
	InSpellWork:= ""
	If InSp_0_spells {
		InSpellWork:= "At will: " InSp_0_spells Chr(10)
	}
	If InSp_5_spells {
		InSpellWork:= InSpellWork "5/day each: " InSp_5_spells Chr(10)
	}
	If InSp_4_spells {
		InSpellWork:= InSpellWork "4/day each: " InSp_4_spells Chr(10)
	}
	If InSp_3_spells {
		InSpellWork:= InSpellWork "3/day each: " InSp_3_spells Chr(10)
	}
	If InSp_2_spells {
		InSpellWork:= InSpellWork "2/day each: " InSp_2_spells Chr(10)
	}
	If InSp_1_spells {
		InSpellWork:= InSpellWork "1/day each: " InSp_1_spells Chr(10)
	}
}

Traitworks() {
	global
	Loopvar:= 1
	While (Loopvar < 10) {
		GuiControl, NPCE_Main:, traitname%Loopvar%, 
		GuiControl, NPCE_Main:, trait%Loopvar%, 
		Loopvar += 1
	}
	Check:= 1
	FlagTraits:= 0
	For key, value in NPC_Traits
	{
		traitname%Check%:= NPC_Traits[key, "Name"]
		trait%Check%:= NPC_Traits[key, "trait"]
		
		trala1:= traitname%Check%
		trala2:= trait%Check%
		GuiControl, NPCE_Main:, traitname%Check%, %trala1%
		GuiControl, NPCE_Main:, trait%Check%, %trala2%

		FlagTraits:= 1
		Check += 1
	}
}

Actionworks() {
	global
	tempArray:= []
	
	Loop % NPC_Actions.MaxIndex()
	{
		tempArray[A_Index, "Name"]:= NPC_Actions[A_Index, "Name"]
		tempArray[A_Index, "Action"]:= NPC_Actions[A_Index, "Action"]
	}
	position:= 1
	Multi_attack:= 0
	Loop % tempArray.MaxIndex()
	{
		If (tempArray[A_Index, "Name"] = "Multiattack") {
			NPC_Actions[position, "Name"]:= tempArray[A_Index, "Name"]
			NPC_Actions[position, "Action"]:= tempArray[A_Index, "Action"]
			position += 1
			RemovedValue := tempArray.RemoveAt(A_Index)
			Multi_attack:= 1
		}
	}		
	Loop % tempArray.MaxIndex()
	{
		If tempArray[A_Index, "Name"] {
			NPC_Actions[position, "Name"]:= tempArray[A_Index, "Name"]
			NPC_Actions[position, "Action"]:= tempArray[A_Index, "Action"]
			position += 1
		}
	}
	temparray:= []
}		
			
Actionworks2() {
	global
	Loopvar:= 1
	While (Loopvar < 6) {
		GuiControl, NPCE_Main:, actionnameB%Loopvar%, 
		GuiControl, NPCE_Main:, actionB%Loopvar%, 
		Loopvar += 1
	}
	Check:= 1
	FlagActions:= 0
	For key, value in NPC_Actions
	{
		actionnameB%Check%:= NPC_Actions[key, "Name"]
		actionB%Check%:= NPC_Actions[key, "Action"]

		trala1:= actionnameB%Check%
		trala2:= actionB%Check%
		GuiControl, NPCE_Main:, actionnameB%Check%, %trala1%
		GuiControl, NPCE_Main:, actionB%Check%, %trala2%
		FlagActions:= 1
		Check += 1
	}
	
}

Actionworks3() {
	global
	Loopvar:= 1
	While (Loopvar < 12) {
		GuiControl, NPCE_Actions:, actionnameB%Loopvar%, 
		GuiControl, NPCE_Actions:, actionB%Loopvar%, 
		Loopvar += 1
	}
	Check:= 1
	FlagActions:= 0
	For key, value in NPC_Actions
	{
		actionnameB%Check%:= NPC_Actions[key, "Name"]
		actionB%Check%:= NPC_Actions[key, "Action"]

		trala1:= actionnameB%Check%
		trala2:= actionB%Check%
		GuiControl, NPCE_Actions:, actionnameB%Check%, %trala1%
		GuiControl, NPCE_Actions:, actionB%Check%, %trala2%
		FlagActions:= 1
		Check += 1
		if (trala1 == "Multiattack") {
			GuiControl, NPCE_Actions:, Multi_attack, %Multi_attack%
			GuiControl, NPCE_Actions:, multi_attack_Text, %trala2%
		}
	}
	
}

ReActionworks() {
	global
	Loopvar:= 1
	While (Loopvar < 5) {
		GuiControl, NPCE_Main:, reactionnameB%Loopvar%, 
		GuiControl, NPCE_Reactions:, reactionnameB%Loopvar%, 
		GuiControl, NPCE_Reactions:, reactionB%Loopvar%, 
		Loopvar += 1
	}
	Check:= 1
	FlagreActions:= 0
	For key, value in NPC_reActions
	{
		reactionnameB%Check%:= NPC_reActions[key, "Name"]
		reactionB%Check%:= NPC_reActions[key, "Reaction"]

		trala1:= reactionnameB%Check%
		trala2:= reactionB%Check%
		GuiControl, NPCE_Main:, reactionnameB%Check%, %trala1%
		GuiControl, NPCE_Reactions:, reactionnameB%Check%, %trala1%
		GuiControl, NPCE_Reactions:, reactionB%Check%, %trala2%
		FlagreActions:= 1
		Check += 1
	}
	
}

lgactionworks() {
	global
	tempArray:= objfullyclone(NPC_Legendary_Actions)
	position:= 1
	Loop % tempArray.MaxIndex()
	{
		If (tempArray[A_Index, "Name"] == "Options") {
			NPC_Legendary_Actions[position, "Name"]:= tempArray[A_Index, "Name"]
			NPC_Legendary_Actions[position, "LegAction"]:= tempArray[A_Index, "LegAction"]
			position += 1
			RemovedValue := tempArray.RemoveAt(A_Index)
		}
	}		
	Loop % tempArray.MaxIndex()
	{
		If NPC_Legendary_Actions[position, "Name"] {
			NPC_Legendary_Actions[position, "Name"]:= tempArray[A_Index, "Name"]
			NPC_Legendary_Actions[position, "LegAction"]:= tempArray[A_Index, "LegAction"]
			position += 1
		}
	}
	temparray:= []
}		
			
lgactionworks2() {
	global
	Loopvar:= 1
	While (Loopvar < 5) {
		GuiControl, NPCE_Main:, lgactionnameB%Loopvar%, 
		Loopvar += 1
	}
	Check:= 1
	Flaglegactions:= 0
	For key, value in NPC_Legendary_Actions
	{
		lgactionnameB%Check%:= NPC_Legendary_Actions[key, "Name"]

		trala1:= lgactionnameB%Check%
		GuiControl, NPCE_Main:, lgactionnameB%Check%, %trala1%
		Flaglegactions:= 1
		Check += 1
	}
lgactionworks3()	
}

lgactionworks3() {
	global
	Loopvar:= 1
	While (Loopvar < 8) {
		GuiControl, NPCE_LegActions:, lgactionnameB%Loopvar%, 
		GuiControl, NPCE_LegActions:, lgactionB%Loopvar%, 
		Loopvar += 1
	}
	Check:= 1
	For key, value in NPC_Legendary_Actions
	{
		lgactionnameB%Check%:= NPC_Legendary_Actions[key, "Name"]
		lgactionB%Check%:= NPC_Legendary_Actions[key, "LegAction"]

		trala1:= lgactionnameB%Check%
		trala2:= lgactionB%Check%
		GuiControl, NPCE_LegActions:, lgactionnameB%Check%, %trala1%
		GuiControl, NPCE_LegActions:, lgactionB%Check%, %trala2%
		Check += 1
		if (trala1 == "Options") {
			GuiControl, NPCE_LegActions:, LgActionOptions, %trala2%
		}
	}
	
}

lractionworks() {
	global
	tempArray:= objfullyclone(NPC_Lair_Actions)
	position:= 1
	Loop % tempArray.MaxIndex()
	{
		If (tempArray[A_Index, "Name"] == "Options") {
			NPC_Lair_Actions[position, "Name"]:= tempArray[A_Index, "Name"]
			NPC_Lair_Actions[position, "LairAction"]:= tempArray[A_Index, "LairAction"]
			position += 1
			RemovedValue := tempArray.RemoveAt(A_Index)
		}
	}		
	Loop % tempArray.MaxIndex()
	{
		If NPC_Lair_Actions[position, "Name"] {
			NPC_Lair_Actions[position, "Name"]:= tempArray[A_Index, "Name"]
			NPC_Lair_Actions[position, "LairAction"]:= tempArray[A_Index, "LairAction"]
			position += 1
		}
	}
	temparray:= []
}	

lractionworks2() {
	global
	Loopvar:= 1
	While (Loopvar < 5) {
		GuiControl, NPCE_Main:, lractionnameB%Loopvar%, 
		Loopvar += 1
	}
	Check:= 1
	FlagLairActions:= 0
	For key, value in NPC_Lair_Actions
	{
		lractionnameB%Check%:= NPC_Lair_Actions[key, "Name"]

		trala1:= lractionnameB%Check%
		GuiControl, NPCE_Main:, lractionnameB%Check%, %trala1%
		FlagLairActions:= 1
		Check += 1
	}
lractionworks3()
}

lractionworks3() {
	global
	Loopvar:= 1
	While (Loopvar < 8) {
		GuiControl, NPCE_LairActions:, lractionnameB%Loopvar%, 
		GuiControl, NPCE_LairActions:, lractionB%Loopvar%, 
		Loopvar += 1
	}
	Check:= 1
	For key, value in NPC_Lair_Actions
	{
		lractionnameB%Check%:= NPC_Lair_Actions[key, "Name"]
		lractionB%Check%:= NPC_Lair_Actions[key, "LairAction"]

		trala1:= lractionnameB%Check%
		trala2:= lractionB%Check%
		GuiControl, NPCE_LairActions:, lractionnameB%Check%, %trala1%
		GuiControl, NPCE_LairActions:, lractionB%Check%, %trala2%
		Check += 1
		if (trala1 == "Options") {
			GuiControl, NPCE_LairActions:, lractionOptions, %trala2%
		}
	}
}

GetVars_Main() {
	global
	;Base Stats
	NPCname:= vNPCname.get()
	NPCgender:= vNPCgender.get()
	NPCunique:= vNPCunique.get()
	NPCpropername:= vNPCpropername.get()
	NPCsize:= vNPCsize.get()
	NPCtype:= vNPCtype.get()
	NPCtag:= vNPCtag.get()
	NPCalign:= vNPCalign.get()
	NPCac:= vNPCac.get()
	NPChp:= vNPChp.get()
	NPCwalk:= vNPCwalk.get()
	NPCburrow:= vNPCburrow.get()
	NPCclimb:= vNPCclimb.get()
	NPCfly:= vNPCfly.get()
	NPChover:= vNPChover.get()
	NPCswim:= vNPCswim.get()
	NPCstr:= vNPCstr.get()
	NPCdex:= vNPCdex.get()
	NPCcon:= vNPCcon.get()
	NPCint:= vNPCint.get()
	NPCwis:= vNPCwis.get()
	NPCcha:= vNPCcha.get()
	NPCstrsav:= vNPCstrsav.get()
	NPCdexsav:= vNPCdexsav.get()
	NPCconsav:= vNPCconsav.get()
	NPCintsav:= vNPCintsav.get()
	NPCwissav:= vNPCwissav.get()
	NPCchasav:= vNPCchasav.get()
	NPC_FS_STR:= vNPC_FS_STR.get()
	NPC_FS_DEX:= vNPC_FS_DEX.get()
	NPC_FS_CON:= vNPC_FS_CON.get()
	NPC_FS_INT:= vNPC_FS_INT.get()
	NPC_FS_WIS:= vNPC_FS_WIS.get()
	NPC_FS_CHA:= vNPC_FS_CHA.get()
	NPCblind:= vNPCblind.get()
	NPCdark:= vNPCdark.get()
	NPCtremor:= vNPCtremor.get()
	NPCtrue:= vNPCtrue.get()
	NPCpassperc:= vNPCpassperc.get()
	NPCblindB:= vNPCblindB.get()
	NPCdarkB:= vNPCdarkB.get()
	NPCtremorB:= vNPCtremorB.get()
	NPCtrueB:= vNPCtrueB.get()
	NPCcharat:= vNPCcharat.get()
	NPCxp:= vNPCxp.get()
	
	;Dmg Modifiers
	cbDV1:= vcbDV1.get()
	cbDV2:= vcbDV2.get()
	cbDV3:= vcbDV3.get()
	cbDV4:= vcbDV4.get()
	cbDV5:= vcbDV5.get()
	cbDV6:= vcbDV6.get()
	cbDV7:= vcbDV7.get()
	cbDV8:= vcbDV8.get()
	cbDV9:= vcbDV9.get()
	cbDV10:= vcbDV10.get()
	cbDV11:= vcbDV11.get()
	cbDV12:= vcbDV12.get()
	cbDV13:= vcbDV13.get()

	cbDR1:= vcbDR1.get()
	cbDR2:= vcbDR2.get()
	cbDR3:= vcbDR3.get()
	cbDR4:= vcbDR4.get()
	cbDR5:= vcbDR5.get()
	cbDR6:= vcbDR6.get()
	cbDR7:= vcbDR7.get()
	cbDR8:= vcbDR8.get()
	cbDR9:= vcbDR9.get()
	cbDR10:= vcbDR10.get()
	cbDR11:= vcbDR11.get()
	cbDR12:= vcbDR12.get()
	cbDR13:= vcbDR13.get()

	DRRadio1:= vDRRadio1.get()
	DRRadio2:= vDRRadio2.get()
	DRRadio3:= vDRRadio3.get()
	DRRadio4:= vDRRadio4.get()
	DRRadio5:= vDRRadio5.get()
	DRRadio6:= vDRRadio6.get()

	cbDI1:= vcbDI1.get()
	cbDI2:= vcbDI2.get()
	cbDI3:= vcbDI3.get()
	cbDI4:= vcbDI4.get()
	cbDI5:= vcbDI5.get()
	cbDI6:= vcbDI6.get()
	cbDI7:= vcbDI7.get()
	cbDI8:= vcbDI8.get()
	cbDI9:= vcbDI9.get()
	cbDI10:= vcbDI10.get()
	cbDI11:= vcbDI11.get()
	cbDI12:= vcbDI12.get()
	cbDI13:= vcbDI13.get()

	DIRadio1:= vDIRadio1.get()
	DIRadio2:= vDIRadio2.get()
	DIRadio3:= vDIRadio3.get()
	DIRadio4:= vDIRadio4.get()	
	DIRadio5:= vDIRadio5.get()	

	cbCI1:= vcbCI1.get()
	cbCI2:= vcbCI2.get()
	cbCI3:= vcbCI3.get()
	cbCI4:= vcbCI4.get()
	cbCI5:= vcbCI5.get()
	cbCI6:= vcbCI6.get()
	cbCI7:= vcbCI7.get()
	cbCI8:= vcbCI8.get()
	cbCI9:= vcbCI9.get()
	cbCI10:= vcbCI10.get()
	cbCI11:= vcbCI11.get()
	cbCI12:= vcbCI12.get()
	cbCI13:= vcbCI13.get()
	cbCI14:= vcbCI14.get()
	cbCI15:= vcbCI15.get()
	cbCI16:= vcbCI16.get()
	CI16:= vCI16.get()

	; Skills & Languages
	sk_acro:= vsk_acro.get()
	sk_anim:= vsk_anim.get()
	sk_arca:= vsk_arca.get()
	sk_athl:= vsk_athl.get()
	sk_dece:= vsk_dece.get()
	sk_hist:= vsk_hist.get()
	sk_insi:= vsk_insi.get()
	sk_inti:= vsk_inti.get()
	sk_inve:= vsk_inve.get()
	sk_medi:= vsk_medi.get()
	sk_natu:= vsk_natu.get()
	sk_perc:= vsk_perc.get()
	sk_perf:= vsk_perf.get()
	sk_pers:= vsk_pers.get()
	sk_reli:= vsk_reli.get()
	sk_slei:= vsk_slei.get()
	sk_stea:= vsk_stea.get()
	sk_surv:= vsk_surv.get()
	
	NPCLanguages:= []
	GuiControlGet, Lang1, NPCE_Main:, Lang1
	GuiControlGet, Lang2, NPCE_Main:, Lang2
	GuiControlGet, Lang3, NPCE_Main:, Lang3
	GuiControlGet, UserLangs, NPCE_Main:, UserLangs
	Loop, Parse, Lang1, |, %A_Space%
	{
		NPCLanguages.push(A_Loopfield)
	}
	Loop, Parse, Lang2, |, %A_Space%
	{
		NPCLanguages.push(A_Loopfield)
	}
	Loop, Parse, Lang3, |, %A_Space%
	{
		NPCLanguages.push(A_Loopfield)
	}
	Loop, Parse, UserLangs, |, %A_Space%
	{
		NPCLanguages.push(A_Loopfield)
	}

	GuiControlGet, LangSelect, NPCE_Main:, LangSelect
	
	LangAlt:= vLangAlt.get()
	NPCtelep:= vNPCtelep.get()
	telrange:= vtelrange.get()

	; innate casting
	FlagInSpell:= vFlagInSpell.get()
	NPCPsionics:= vNPCPsionics.get()
	NPCinspability:= vNPCinspability.get()
	NPCinspsave:= vNPCinspsave.get()
	NPCinsptohit:= vNPCinsptohit.get()
	NPCinsptext:= vNPCinsptext.get()
	InSp_0_spells:= vInSp_0_spells.get()
	InSp_1_spells:= vInSp_1_spells.get()
	InSp_2_spells:= vInSp_2_spells.get()
	InSp_3_spells:= vInSp_3_spells.get()
	InSp_4_spells:= vInSp_4_spells.get()
	InSp_5_spells:= vInSp_5_spells.get()

	; spellcasting
	FlagSpell:= vFlagSpell.get()
	NPCsplevel:= vNPCsplevel.get()
	NPCspability:= vNPCspability.get()
	NPCspsave:= vNPCspsave.get()
	NPCsptohit:= vNPCsptohit.get()
	NPCspclass:= vNPCspclass.get()
	NPCspflavour:= vNPCspflavour.get()
	NPCSpellStar:= vNPCSpellStar.get()
	Sp_0_casts:= vSp_0_casts.get()
	Sp_1_casts:= vSp_1_casts.get()
	Sp_2_casts:= vSp_2_casts.get()
	Sp_3_casts:= vSp_3_casts.get()
	Sp_4_casts:= vSp_4_casts.get()
	Sp_5_casts:= vSp_5_casts.get()
	Sp_6_casts:= vSp_6_casts.get()
	Sp_7_casts:= vSp_7_casts.get()
	Sp_8_casts:= vSp_8_casts.get()
	Sp_9_casts:= vSp_9_casts.get()
	Sp_0_spells:= vSp_0_spells.get()
	Sp_1_spells:= vSp_1_spells.get()
	Sp_2_spells:= vSp_2_spells.get()
	Sp_3_spells:= vSp_3_spells.get()
	Sp_4_spells:= vSp_4_spells.get()
	Sp_5_spells:= vSp_5_spells.get()
	Sp_6_spells:= vSp_6_spells.get()
	Sp_7_spells:= vSp_7_spells.get()
	Sp_8_spells:= vSp_8_spells.get()
	Sp_9_spells:= vSp_9_spells.get()

	; Actions


	; Description
	Desc_Add_Text:= vDesc_Add_Text.get()
	Desc_NPC_Title:= vDesc_NPC_Title.get()
	Desc_Image_Link:= vDesc_Image_Link.get()
	Desc_Spell_List:= vDesc_Spell_List.get()
	Desc_fixes:= vDesc_fixes.get()
	Desc_strip_lf:= vDesc_strip_lf.get()
	Desc_title:= vDesc_title.get()

	GuiControlGet, NPCImArt, NPCE_Main:, NPCImArt
	GuiControlGet, NPCImLink, NPCE_Main:, NPCImLink
	GuiControlGet, NPCNoID, NPCE_Main:, NPCNoID
	GuiControlGet, FGcat, NPCE_Main:, FGcat
}

SetVars_Main() {
	global
	vNPCname.Set(NPCname)
	vNPCgender.Set(NPCgender)
	vNPCunique.Set(NPCunique)
	vNPCpropername.Set(NPCpropername)
	vNPCsize.Set(NPCsize)
	vNPCtype.Set(NPCtype)
	vNPCtag.Set(NPCtag)
	vNPCalign.Set(NPCalign)
	vNPCac.Set(NPCac)
	vNPChp.Set(NPChp)

	vNPCwalk.Set(NPCwalk)
	vNPCburrow.Set(NPCburrow)
	vNPCclimb.Set(NPCclimb)
	vNPCfly.Set(NPCfly)
	vNPChover.Set(NPChover)
	vNPCswim.Set(NPCswim)

	vNPCstr.Set(NPCstr)
	vNPCdex.Set(NPCdex)
	vNPCcon.Set(NPCcon)
	vNPCint.Set(NPCint)
	vNPCwis.Set(NPCwis)
	vNPCcha.Set(NPCcha)

	vNPCstrsav.Set(NPCstrsav)
	vNPCdexsav.Set(NPCdexsav)
	vNPCconsav.Set(NPCconsav)
	vNPCintsav.Set(NPCintsav)
	vNPCwissav.Set(NPCwissav)
	vNPCchasav.Set(NPCchasav)

	vNPCblind.Set(NPCblind)
	vNPCdark.Set(NPCdark)
	vNPCtremor.Set(NPCtremor)
	vNPCtrue.Set(NPCtrue)
	vNPCpassperc.Set(NPCpassperc)
	vNPCblindB.Set(NPCblindB)
	vNPCdarkB.Set(NPCdarkB)
	vNPCtremorB.Set(NPCtremorB)
	vNPCtrueB.Set(NPCtrueB)

	vNPCcharat.Set(NPCcharat)
	vNPCxp.Set(NPCxp)

	vcbDV1.Set(cbDV1)
	vcbDV2.Set(cbDV2)
	vcbDV3.Set(cbDV3)
	vcbDV4.Set(cbDV4)
	vcbDV5.Set(cbDV5)
	vcbDV6.Set(cbDV6)
	vcbDV7.Set(cbDV7)
	vcbDV8.Set(cbDV8)
	vcbDV9.Set(cbDV9)
	vcbDV10.Set(cbDV10)
	vcbDV11.Set(cbDV11)
	vcbDV12.Set(cbDV12)
	vcbDV13.Set(cbDV13)

	vcbDR1.Set(cbDR1)
	vcbDR2.Set(cbDR2)
	vcbDR3.Set(cbDR3)
	vcbDR4.Set(cbDR4)
	vcbDR5.Set(cbDR5)
	vcbDR6.Set(cbDR6)
	vcbDR7.Set(cbDR7)
	vcbDR8.Set(cbDR8)
	vcbDR9.Set(cbDR9)
	vcbDR10.Set(cbDR10)
	vcbDR11.Set(cbDR11)
	vcbDR12.Set(cbDR12)
	vcbDR13.Set(cbDR13)

	vDRRadio1.Set(DRRadio1)
	vDRRadio2.Set(DRRadio2)
	vDRRadio3.Set(DRRadio3)
	vDRRadio4.Set(DRRadio4)
	vDRRadio5.Set(DRRadio5)
	vDRRadio6.Set(DRRadio6)

	vcbDI1.Set(cbDI1)
	vcbDI2.Set(cbDI2)
	vcbDI3.Set(cbDI3)
	vcbDI4.Set(cbDI4)
	vcbDI5.Set(cbDI5)
	vcbDI6.Set(cbDI6)
	vcbDI7.Set(cbDI7)
	vcbDI8.Set(cbDI8)
	vcbDI9.Set(cbDI9)
	vcbDI10.Set(cbDI10)
	vcbDI11.Set(cbDI11)
	vcbDI12.Set(cbDI12)
	vcbDI13.Set(cbDI13)

	vDIRadio1.Set(DIRadio1)
	vDIRadio2.Set(DIRadio2)
	vDIRadio3.Set(DIRadio3)
	vDIRadio4.Set(DIRadio4)
	vDIRadio5.Set(DIRadio5)

	vcbCI1.Set(cbCI1)
	vcbCI2.Set(cbCI2)
	vcbCI3.Set(cbCI3)
	vcbCI4.Set(cbCI4)
	vcbCI5.Set(cbCI5)
	vcbCI6.Set(cbCI6)
	vcbCI7.Set(cbCI7)
	vcbCI8.Set(cbCI8)
	vcbCI9.Set(cbCI9)
	vcbCI10.Set(cbCI10)
	vcbCI11.Set(cbCI11)
	vcbCI12.Set(cbCI12)
	vcbCI13.Set(cbCI13)
	vcbCI14.Set(cbCI14)
	vcbCI15.Set(cbCI15)
	vcbCI16.Set(cbCI16)
	vCI16.Set(CI16)

	vsk_acro.Set(sk_acro)
	vsk_anim.Set(sk_anim)
	vsk_arca.Set(sk_arca)
	vsk_athl.Set(sk_athl)
	vsk_dece.Set(sk_dece)
	vsk_hist.Set(sk_hist)
	vsk_insi.Set(sk_insi)
	vsk_inti.Set(sk_inti)
	vsk_inve.Set(sk_inve)
	vsk_medi.Set(sk_medi)
	vsk_natu.Set(sk_natu)
	vsk_perc.Set(sk_perc)
	vsk_perf.Set(sk_perf)
	vsk_pers.Set(sk_pers)
	vsk_reli.Set(sk_reli)
	vsk_slei.Set(sk_slei)
	vsk_stea.Set(sk_stea)
	vsk_surv.Set(sk_surv)


	vLangAlt.Set(LangAlt)
	vNPCtelep.Set(NPCtelep)
	vtelrange.Set(telrange)

	vtraitnameNew.Set(traitnameNew)
	vtraitNew.Set(traitNew)

	vFlagInSpell.Set(FlagInSpell)
	vNPCPsionics.Set(NPCPsionics)
	vNPCinspability.Set(NPCinspability)
	vNPCinspsave.Set(NPCinspsave)
	vNPCinsptohit.Set(NPCinsptohit)
	vInSp_0_spells.Set(InSp_0_spells)
	vInSp_1_spells.Set(InSp_1_spells)
	vInSp_2_spells.Set(InSp_2_spells)
	vInSp_3_spells.Set(InSp_3_spells)
	vInSp_4_spells.Set(InSp_4_spells)
	vInSp_5_spells.Set(InSp_5_spells)

	vFlagSpell.Set(FlagSpell)
	vNPCsplevel.Set(NPCsplevel)
	vNPCspability.Set(NPCspability)
	vNPCspsave.Set(NPCspsave)
	vNPCsptohit.Set(NPCsptohit)
	vNPCspclass.Set(NPCspclass)
	vNPCspflavour.Set(NPCspflavour)
	vSp_0_casts.Set(Sp_0_casts)
	vSp_1_casts.Set(Sp_1_casts)
	vSp_2_casts.Set(Sp_2_casts)
	vSp_3_casts.Set(Sp_3_casts)
	vSp_4_casts.Set(Sp_4_casts)
	vSp_5_casts.Set(Sp_5_casts)
	vSp_6_casts.Set(Sp_6_casts)
	vSp_7_casts.Set(Sp_7_casts)
	vSp_8_casts.Set(Sp_8_casts)
	vSp_9_casts.Set(Sp_9_casts)
	vSp_0_spells.Set(Sp_0_spells)
	vSp_1_spells.Set(Sp_1_spells)
	vSp_2_spells.Set(Sp_2_spells)
	vSp_3_spells.Set(Sp_3_spells)
	vSp_4_spells.Set(Sp_4_spells)
	vSp_5_spells.Set(Sp_5_spells)
	vSp_6_spells.Set(Sp_6_spells)
	vSp_7_spells.Set(Sp_7_spells)
	vSp_8_spells.Set(Sp_8_spells)
	vSp_9_spells.Set(Sp_9_spells)
	vNPCSpellStar.Set(NPCSpellStar)

	vDesc_Add_Text.Set(Desc_Add_Text)
	vDesc_NPC_Title.Set(Desc_NPC_Title)
	vDesc_Image_Link.Set(Desc_Image_Link)
	vDesc_Spell_List.Set(Desc_Spell_List)
	vDesc_fixes.Set(Desc_fixes)
	vDesc_strip_lf.Set(Desc_strip_lf)
	vDesc_title.Set(Desc_title)

}

blank_WA() {
	global
	WA_Name:= ""
	WA_Type:= "Melee Weapon Attack"
	WA_ToHit:= "0"
	WA_Reach:= "5"
	WA_Rnorm:= "30"
	WA_Rlong:= "60"
	WA_Target:= "one target"
	WA_NoDice:= "1"
	WA_Dice:= "6"
	WA_DamBon:= "0"
	WA_DamType:= "slashing"

	weapon_attack_Text:= ""

	WA_Magic:= 0
	WA_Silver:= 0
	WA_Adaman:= 0
	WA_cfiron:= 0
	WA_BonAdd:= 0
	WA_BonNoDice:= ""
	WA_BonDice:= ""
	WA_BonDamBon:= ""
	WA_BonDamType:= ""
	WA_OtherTextAdd:= 0
	WA_OtherText:= ""


	GuiControl, NPCE_Weapons:Text, WA_Name, %WA_Name%
	GuiControl, NPCE_Weapons:Text, WA_Type, %WA_Type%
	GuiControl, NPCE_Weapons:, WA_ToHit, %WA_ToHit%
	GuiControl, NPCE_Weapons:, WA_Reach, %WA_Reach%
	GuiControl, NPCE_Weapons:, WA_Rnorm, %WA_Rnorm%
	GuiControl, NPCE_Weapons:, WA_Rlong, %WA_Rlong%
	GuiControl, NPCE_Weapons:Text, WA_Target, %WA_Target%
	GuiControl, NPCE_Weapons:, WA_NoDice, %WA_NoDice%
	GuiControl, NPCE_Weapons:Text, WA_Dice, %WA_Dice%
	GuiControl, NPCE_Weapons:, WA_DamBon, %WA_DamBon%
	GuiControl, NPCE_Weapons:Text, WA_DamType, %WA_DamType%
	GuiControl, NPCE_Weapons:, WA_Magic, %WA_Magic%
	GuiControl, NPCE_Weapons:, WA_Silver, %WA_Silver%
	GuiControl, NPCE_Weapons:, WA_Adaman, %WA_Adaman%
	GuiControl, NPCE_Weapons:, WA_cfiron, %WA_cfiron%
	GuiControl, NPCE_Weapons:, WA_BonAdd, %WA_BonAdd%
	GuiControl, NPCE_Weapons:, WA_BonNoDice, %WA_BonNoDice%
	GuiControl, NPCE_Weapons:Text, WA_BonDice, %WA_BonDice%
	GuiControl, NPCE_Weapons:, WA_BonDamBon, %WA_BonDamBon%
	GuiControl, NPCE_Weapons:Text, WA_BonDamType, %WA_BonDamType%
	GuiControl, NPCE_Weapons:, WA_OtherTextAdd, %WA_OtherTextAdd%
	GuiControl, NPCE_Weapons:, WA_OtherText, %WA_OtherText%
}

parse_attack(num) {
	global
	local tempvarib
	WA_Name:= NPC_Actions[num, "Name"]
	If WA_Name {
		WA_ToHit:= 0
		WA_Reach:= 0
		WA_Rnorm:= 0
		WA_Rlong:= 0
		WA_Target:= ""
		WA_NoDice:= 0
		WA_Dice:= 0
		WA_DamBon:= 0
		WA_DamType:= 0
		WA_BonAdd:= 0
		WA_BonNoDice:= 0
		WA_BonDice:= 0
		WA_BonDamBon:= 0
		WA_BonDamType:= 0
		WA_OtherTextAdd:= 0
		WA_OtherText:= ""
	}

	TempWeapon:= NPC_Actions[num, "Action"]

	If instr(TempWeapon, "or ranged weapon attack:") {
		WA_Type:= "Melee or Ranged Weapon Attack"
		StringReplace, TempWeapon, TempWeapon, %A_Space%or%A_Space%, ,
	} Else if instr(TempWeapon, "ranged weapon attack:") {
		WA_Type:= "Ranged Weapon Attack"
	} Else if instr(TempWeapon, "melee weapon attack:") {
		WA_Type:= "Melee Weapon Attack"
	} Else if instr(TempWeapon, "or ranged spell attack:") {
		WA_Type:= "Melee or Ranged Spell Attack"
		StringReplace, TempWeapon, TempWeapon, %A_Space%or%A_Space%, ,
	} Else if instr(TempWeapon, "ranged spell attack:") {
		WA_Type:= "Ranged Spell Attack"
	} Else if instr(TempWeapon, "melee spell attack:") {
		WA_Type:= "Melee Spell Attack"
	} Else {
		WA_Type:= ""
	}
	
	If instr(TempWeapon, ", magic") {
		WA_Magic:= 1
		TempWeapon:= RegExReplace(TempWeapon, ", magic", "")
	} Else {
		WA_Magic:= 0
	}
	
	If instr(TempWeapon, ", silver") {
		WA_Silver:= 1
		TempWeapon:= RegExReplace(TempWeapon, ", silver", "")
	} Else {
		WA_Silver:= 0
	}
	
	If instr(TempWeapon, ", adamantine") {
		WA_Adaman:= 1
		TempWeapon:= RegExReplace(TempWeapon, ", adamantine", "")
	} Else {
		WA_Adaman:= 0
	}

	If instr(TempWeapon, ", cold-forged iron") {
		WA_cfiron:= 1
		TempWeapon:= RegExReplace(TempWeapon, ", cold-forged iron", "")
	} Else {
		WA_cfiron:= 0
	}

	TempWeapon:= RegExReplace(TempWeapon, "U)damage or .* damage if used with two hands.", "damage.", tempvarib)
	If tempvarib {
		WA_Versatile:= 1
	} Else {
		WA_Versatile:= 0
	}
	
	FoundIt:= RegExMatch(TempWeapon,"iU)Melee (Weapon|Spell) Attack: ?\+(\d+) to hit, reach (\d+) ?ft\., one (\w+)\. Hit: (\d+) \((\d+)d(\d++) ?(\+|-)? ?(\d+)?\) (\w+) damage", att)
	If FoundIt {
		WA_ToHit:= att2
		WA_Reach:= att3
		WA_Target:= "one " att4
		WA_NoDice:= att6
		WA_Dice:= att7
		
		If (att8 = "-")
			WA_DamBon:= 0 - att9
		else
			WA_DamBon:= att9
		WA_DamType:= att10
		StringReplace, TempWeapon, TempWeapon, %att%., ,
		StringReplace, TempWeapon, TempWeapon, %att%, ,
	}
	
	FoundIt:= RegExMatch(TempWeapon,"iU)Ranged (Weapon|Spell) Attack: ?\+(\d+) to hit, range (\d+)/(\d+) ?ft., one (\w+)\. Hit: (\d+) \((\d+)d(\d++) ?\+? ?(\d+)?\) (\w+) damage", att)
	If FoundIt {
		WA_ToHit:= att2
		WA_Rnorm:= att3
		WA_Rlong:= att4
		WA_Target:= "one " att5
		WA_NoDice:= att7
		WA_Dice:= att8
		WA_DamBon:= att9
		WA_DamType:= att10
		StringReplace, TempWeapon, TempWeapon, %att%., ,
		StringReplace, TempWeapon, TempWeapon, %att%, ,
	}

	FoundIt:= RegExMatch(TempWeapon,"iU) plus (\d+) \((\d+)d(\d++) ?\+? ?(\d+)?\) (\w+) damage", att)
	If FoundIt {
		WA_BonAdd:= 1
		WA_BonNoDice:= att2
		WA_BonDice:= att3
		WA_BonDamBon:= att4
		WA_BonDamType:= att5
		StringReplace, TempWeapon, TempWeapon, %att%., ,
		StringReplace, TempWeapon, TempWeapon, %att%, ,
}
	
	If TempWeapon {
		WA_OtherTextAdd:= 1
		WA_OtherText:= TempWeapon		
	}
	
	GuiControl, NPCE_Actions:Text, WA_Name, %WA_Name%
	GuiControl, NPCE_Actions:Text, WA_Type, %WA_Type%
	GuiControl, NPCE_Actions:, WA_Magic, %WA_Magic%
	GuiControl, NPCE_Actions:, WA_Silver, %WA_Silver%
	GuiControl, NPCE_Actions:, WA_Adaman, %WA_Adaman%
	GuiControl, NPCE_Actions:, WA_cfiron, %WA_cfiron%
	GuiControl, NPCE_Actions:, WA_Versatile, %WA_Versatile%
	GuiControl, NPCE_Actions:, WA_ToHit, %WA_ToHit%
	GuiControl, NPCE_Actions:, WA_Reach, %WA_Reach%
	GuiControl, NPCE_Actions:, WA_Rnorm, %WA_Rnorm%
	GuiControl, NPCE_Actions:, WA_Rlong, %WA_Rlong%
	GuiControl, NPCE_Actions:Text, WA_Target, %WA_Target%
	GuiControl, NPCE_Actions:, WA_NoDice, %WA_NoDice%
	GuiControl, NPCE_Actions:Text, WA_Dice, %WA_Dice%
	GuiControl, NPCE_Actions:, WA_DamBon, %WA_DamBon%
	GuiControl, NPCE_Actions:Text, WA_DamType, %WA_DamType%
	GuiControl, NPCE_Actions:, WA_BonAdd, %WA_BonAdd%
	GuiControl, NPCE_Actions:, WA_BonNoDice, %WA_BonNoDice%
	GuiControl, NPCE_Actions:Text, WA_BonDice, %WA_BonDice%
	GuiControl, NPCE_Actions:, WA_BonDamBon, %WA_BonDamBon%
	GuiControl, NPCE_Actions:Text, WA_BonDamType, %WA_BonDamType%
	GuiControl, NPCE_Actions:, WA_OtherTextAdd, %WA_OtherTextAdd%
	GuiControl, NPCE_Actions:, WA_OtherText, %WA_OtherText%
}

parse_attack_for_output(num) {  ; Written by Zamrod for Fight Club XML
	global
	local tempvarib
	WA_Name:= NPC_Actions[num, "Name"]
	If WA_Name {
		WA_ToHit:= 0
		WA_Reach:= 0
		WA_Rnorm:= 0
		WA_Rlong:= 0
		WA_Target:= ""
		WA_NoDice:= 0
		WA_Dice:= 0
		WA_DamBon:= 0
		WA_DamType:= 0
		WA_BonAdd:= 0
		WA_BonNoDice:= 0
		WA_BonDice:= 0
		WA_BonDamBon:= 0
		WA_BonDamType:= 0
		WA_OtherTextAdd:= 0
		WA_OtherText:= ""
	}

	TempWeapon:= NPC_Actions[num, "Action"]
	If instr(TempWeapon, "or ranged weapon attack:") {
		WA_Type:= "Melee or Ranged Weapon Attack"
		StringReplace, TempWeapon, TempWeapon, %A_Space%or%A_Space%, ,
	} Else if instr(TempWeapon, "ranged weapon attack:") {
		WA_Type:= "Ranged Weapon Attack"
	} Else if instr(TempWeapon, "melee weapon attack:") {
		WA_Type:= "Melee Weapon Attack"
	} Else if instr(TempWeapon, "or ranged spell attack:") {
		WA_Type:= "Melee or Ranged Spell Attack"
		StringReplace, TempWeapon, TempWeapon, %A_Space%or%A_Space%, ,
	} Else if instr(TempWeapon, "ranged spell attack:") {
		WA_Type:= "Ranged Spell Attack"
	} Else if instr(TempWeapon, "melee spell attack:") {
		WA_Type:= "Melee Spell Attack"
	} Else {
		WA_Type:= ""
	}
	
	If instr(TempWeapon, ", magic") {
		WA_Magic:= 1
		TempWeapon:= RegExReplace(TempWeapon, ", magic", "")
	} Else {
		WA_Magic:= 0
	}
	
	If instr(TempWeapon, ", silver") {
		WA_Silver:= 1
		TempWeapon:= RegExReplace(TempWeapon, ", silver", "")
	} Else {
		WA_Silver:= 0
	}
	
	If instr(TempWeapon, ", adamantine") {
		WA_Adaman:= 1
		TempWeapon:= RegExReplace(TempWeapon, ", adamantine", "")
	} Else {
		WA_Adaman:= 0
	}

	If instr(TempWeapon, ", cold-forged iron") {
		WA_cfiron:= 1
		TempWeapon:= RegExReplace(TempWeapon, ", cold-forged iron", "")
	} Else {
		WA_cfiron:= 0
	}

	TempWeapon:= RegExReplace(TempWeapon, "U)damage or .* damage if used with two hands.", "damage.", tempvarib)
	If tempvarib {
		WA_Versatile:= 1
	} Else {
		WA_Versatile:= 0
	}
	
	FoundIt:= RegExMatch(TempWeapon,"iU)Melee (Weapon|Spell) Attack: ?\+(\d+) to hit, reach (\d+) ?ft\.(.*) one (\w+)\. Hit: (\d+) \((\d+)d(\d++) ?(\+|-)? ?(\d+)?\) (\w+) damage", att)
	If FoundIt {
		WA_ToHit:= att2
		WA_Reach:= att3
		WA_Target:= "one " att5
		WA_NoDice:= att7
		WA_Dice:= att8
		
		If (att9 = "-")
			WA_DamBon:= 0 - att10
		else
			WA_DamBon:= att10
		WA_DamType:= att11
		StringReplace, TempWeapon, TempWeapon, %att%., ,
		StringReplace, TempWeapon, TempWeapon, %att%, ,
	}
	
	FoundIt:= RegExMatch(TempWeapon,"iU)Ranged (Weapon|Spell) Attack: ?\+(\d+) to hit, range (\d+)(\s.*|/)(\d+) ?ft.(.*) one (\w+)\. Hit: (\d+) \((\d+)d(\d++) ?\+? ?(\d+)?\) (\w+) damage", att)
	If FoundIt {
		WA_ToHit:= att2
		WA_Rnorm:= att3
		WA_Rlong:= att5
		WA_Target:= "one " att7
		WA_NoDice:= att9
		WA_Dice:= att10
		WA_DamBon:= att11
		WA_DamType:= att12
		StringReplace, TempWeapon, TempWeapon, %att%., ,
		StringReplace, TempWeapon, TempWeapon, %att%, ,
	}

	FoundIt:= RegExMatch(TempWeapon,"iU) plus (\d+) \((\d+)d(\d++) ?\+? ?(\d+)?\) (\w+) damage", att)
	If FoundIt {
		WA_BonAdd:= 1
		WA_BonNoDice:= att2
		WA_BonDice:= att3
		WA_BonDamBon:= att4
		WA_BonDamType:= att5
		StringReplace, TempWeapon, TempWeapon, %att%., ,
		StringReplace, TempWeapon, TempWeapon, %att%, ,
}
	
	If TempWeapon {
		WA_OtherTextAdd:= 1
		WA_OtherText:= TempWeapon		
	}
	
	CurrentWA_ToHit:= WA_ToHit
	CurrentWA_Damage:= WA_NoDice "d" WA_Dice
	if (WA_DamBon)
	{
		CurrentWA_Damage:= CurrentWA_Damage "+" WA_DamBon
	}
	if (WA_BonAdd)
	{
		CurrentWA_Damage:=CurrentWA_Damage "+" WA_BonNoDice "d" WA_BonDice
		if (WA_BonDamBon)
			CurrentWA_Damage:=CurrentWA_Damage "+" WA_BonDamBon
	}
}

GenderReplace(blk) {
	Global
	StringReplace, blk, blk, <NU>, %NU%, All
	StringReplace, blk, blk, <NL>, %NL%, All
	StringReplace, blk, blk, <GU1>, %GU1%, All
	StringReplace, blk, blk, <GU2>, %GU2%, All
	StringReplace, blk, blk, <GU3>, %GU3%, All
	StringReplace, blk, blk, <GU4>, %GU4%, All
	StringReplace, blk, blk, <GL1>, %GL1%, All
	StringReplace, blk, blk, <GL2>, %GL2%, All
	StringReplace, blk, blk, <GL3>, %GL3%, All
	StringReplace, blk, blk, <GL4>, %GL4%, All
	Return blk
}

NPC_Backup() {
	Global
	NPC_Backup:= ""
	Name_Work()
	Par5e_Out()
	NPC_Backup:= NPC_Backup . "***Part 1***" Chr(10)
	NPC_Backup:= NPC_Backup . SaveMainBlock Chr(10)
	NPC_Backup:= NPC_Backup . "***Part 2***" Chr(10)
	NPC_Backup:= NPC_Backup . RE1.GetRTF(False) Chr(10)
	NPC_Backup:= NPC_Backup . "***Part 3***" Chr(10)
	NPC_Backup:= NPC_Backup . "NPCgender: " . NPCgender Chr(10)
	NPC_Backup:= NPC_Backup . "NPCunique: " . NPCunique Chr(10)
	NPC_Backup:= NPC_Backup . "NPCpropername: " . NPCpropername Chr(10)
	NPC_Backup:= NPC_Backup . "Desc_Add_Text: " . Desc_Add_Text Chr(10)
	NPC_Backup:= NPC_Backup . "Desc_NPC_Title: " . Desc_NPC_Title Chr(10)
	NPC_Backup:= NPC_Backup . "Desc_Image_Link: " . Desc_Image_Link Chr(10)
	NPC_Backup:= NPC_Backup . "Desc_Spell_List: " . Desc_Spell_List Chr(10)
	NPC_Backup:= NPC_Backup . "NPCimagePath: " . NPCimagePath Chr(10)
	NPC_Backup:= NPC_Backup . "NPCTokenPath: " . NPCTokenPath Chr(10)
	Num:= NPC_Lair_Actions.MaxIndex()
	NPC_Backup:= NPC_Backup . "NoLActions: " . Num Chr(10)
	Loop % NPC_Lair_Actions.MaxIndex()
	{
		Nam:= NPC_Lair_Actions[A_Index, "Name"]
		NPC_Backup:= NPC_Backup . "LAction" A_Index ": " . Nam Chr(10)
	}
	NPC_Backup:= NPC_Backup . "NPCRTF: " . True Chr(10)
	NPC_Backup:= NPC_Backup . "Terrain: " . sort.terrain Chr(10)
	NPC_Backup:= NPC_Backup . "Lore: " . sort.lore Chr(10)
}

NPC_Restore() {
	Global
	StringReplace NPC_Backup, NPC_Backup, `r`n, -.-, All
	StringReplace NPC_Backup, NPC_Backup, `n, -.-, All
	StringReplace NPC_Backup, NPC_Backup, -.-, `r`n, All

	LF_1:= Instr(NPC_Backup, "***Part 1***")
	LF_2:= Instr(NPC_Backup, "***Part 2***")
	LF_3:= Instr(NPC_Backup, "***Part 3***")
	
	Workingstring := SubStr(NPC_Backup, LF_1 + 14, LF_2 - LF_1 - 14)
	Main_Loop()

	RTF := SubStr(NPC_Backup, LF_2 + 14, LF_3 - LF_2 - 14)
	RE1.SetText(RTF, ["KEEPUNDO"])
	
	LF_3_Text := SubStr(NPC_Backup, LF_3 + 14)
	LF_3_Text:= RegExReplace(LF_3_Text,"\s*$","")
	
	NoLActions:=""
	Terrain:= ""
	Lore:= ""
	
	Loop, parse, LF_3_Text, `n, `r
	{
		FoundPos:= InStr(A_LoopField, ":") 
		varnameholder:= SubStr(A_LoopField, 1, FoundPos-1)
		%varnameholder%:= SubStr(A_LoopField, FoundPos+2)
	}
	
	Gosub Load_NPCToken
	Gosub Load_NPCImage
	
	If (NoLActions > 0) {
		Loop %NoLActions%
		{
			var:= "LAction" A_Index
			NPC_Lair_Actions[A_Index, "Name"]:= %var%
			%var%:= ""
		}
		lractionworks()
		lractionworks2()
	}
	If Terrain
		sort.terrain:= terrain
	If Lore
		sort.lore:= lore

	If (NPCname = "<Newcreature>")
		NPCname:= ""
	Build_RH_Box()
	Inject_Vars()
	Parser()
	NPC_Backup:= ""
}

RE_Paste() {
	Global
	colourtable:= "{\colortbl `;\red120\green34\blue32`;\red0\green0\blue0`;}"
	Clipsave:= Clipboard
	Cliptext = %Clipboard%
	If Desc_strip_lf {
		Format_Chunk(Cliptext)
	}
	If Desc_fixes {
		Common_Problems(Cliptext)
	}
	Clipboard:= cliptext
	GuiControl, Focus, % RE1.HWND
	send ^v
	If Desc_title {
		caret:= RE1.GetSel()
		TKN:= Tokenise(RE1.GetRTF(False))
		Tag_Titles(TKN)
		RTF:= RTFise(TKN)
		RE1.SetText("{\rtf " . colourtable . RTF . "}", ["KEEPUNDO"])
		RE1.SetSel(caret.S, caret.E)
	}
	Clipboard:= Clipsave
}

Get_A_Path(Path, Box) {
	temp:= Path
	If Path {
		SetTimer, FSF, -150
		FileSelectFolder, Path, *%Path%, 3, Select your %box% folder.
	} Else {
		SetTimer, FSF, -150
		FileSelectFolder, Path, , 3, Select your %box% folder.
	}
	If !Path {
		Path:= Temp
	}
	Return Path
}

Vup() {
	global viewport, ScrollPoint, ScrollEnd, buttonup, buttondn
	MouseGetPos,,,,ctrl, 2
	while (ctrl=buttonup && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		Viewport.Document.parentWindow.eval("scrollBy(0, -2);")
		ScrollPoint -= 2
		If (ScrollPoint < 0) {
			Scrollpoint:= 0
		}
	}
	while (ctrl=buttondn && GetKeyState("LButton","p")) {
		MouseGetPos,,,,ctrl, 2
		Viewport.Document.parentWindow.eval("scrollBy(0, 2);")
		ScrollPoint += 2
		If (ScrollPoint > ScrollEnd) {
			Scrollpoint:= ScrollEnd
		}
	}
}

VScrUp() {
	global viewport, ScrollPoint
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") {
		Viewport.Document.parentWindow.eval("scrollBy(0, -50);")
		ScrollPoint -= 50
		If (ScrollPoint < 0) {
			Scrollpoint:= 0
		}
	}
}

VScrDwn() {
	global viewport, ScrollPoint, ScrollEnd
	MouseGetPos,,,,ctrl
	If (ctrl="Internet Explorer_Server1") {
		Viewport.Document.parentWindow.eval("scrollBy(0, 50);")
		ScrollPoint += 50
		If (ScrollPoint > ScrollEnd) {
			Scrollpoint:= ScrollEnd
		}
	}
}


;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
;~    |                 GUI Functions                |
;~    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


Prepare_GUI() {
	global

	GUI_Main()
	;~ GUI_Import()
	If (NPCE_Ref = "self") {
		GUI_Project()
	}
	GUI_HP()
}

GUI_Main() {
	global
	
; Settings for main window (NPCE_Main)
	Gui, NPCE_Main:-MaximizeBox
	Gui, NPCE_Main:+hwndNPCE_Main
	Gui, NPCE_Main:Color, 0xE2E1E8
	Gui, NPCE_Main:font, S10 c000000, Arial
	Gui, NPCE_Main:+OwnDialogs
	
; Menu system
;{
	Menu FileMenu, Add, &New NPC`tCtrl+N, New_NPC
	Menu FileMenu, Icon, &New NPC`tCtrl+N, NPC Engineer.dll, 1
	Menu FileMenu, Add, &Open NPC`tCtrl+O, Open_NPC
	Menu FileMenu, Icon, &Open NPC`tCtrl+O, NPC Engineer.dll, 2
	Menu FileMenu, Add, &Save NPC`tCtrl+S, Save_NPC
	Menu FileMenu, Icon, &Save NPC`tCtrl+S, NPC Engineer.dll, 3
	Menu Filemenu, Add
	Menu FileMenu, Add, Save as HTML, Save_HTML
	Menu FileMenu, Add, Save as RTF, Save_RTF
	Menu Filemenu, Add
	Menu FileMenu, Add, Save as Fight Club XML, Save_XML
	Menu FileMenu, Add, Place BBCode on Clipboard, Copy_BB
	Menu Filemenu, Add
	Menu FileMenu, Add, E&xit`tESC, NPCE_MainGuiClose
	Menu FileMenu, Icon, E&xit`tESC, NPC Engineer.dll, 17
	Menu NPCEngineerMenu, Add, File, :FileMenu
	
	Menu ThemeMenu, Add, Parchment, ThemeMenu
	Menu ThemeMenu, Add, Frost, ThemeMenu
	Menu ThemeMenu, Add, Jungle, ThemeMenu
	Menu ThemeMenu, Add, Blood, ThemeMenu
	Menu ThemeMenu, Add, Flame, ThemeMenu
	Menu ThemeMenu, Check, Parchment
	
	Menu OptionsMenu, Add, &Import Text`tCtrl+I, Import_Text
	Menu OptionsMenu, Icon, &Import Text`tCtrl+I, NPC Engineer.dll, 4
	Menu OptionsMenu, Add, &Create Module `tCtrl+P, ParseProject
	Menu OptionsMenu, Icon, &Create Module `tCtrl+P, NPC Engineer.dll, 6
	Menu OptionsMenu, Add
	Menu OptionsMenu, Add, Manage Categories `tCtrl+K, GUI_Categories
	Menu OptionsMenu, Icon, Manage Categories `tCtrl+K, NPC Engineer.dll, 25
	Menu OptionsMenu, Add, Manage NPC File `tCtrl+M, Manage_Json
	Menu OptionsMenu, Add, Manage Weapons List, Manage_Weapons
	Menu OptionsMenu, Add, Manage Actions List, GUI_ActAdd
	Menu OptionsMenu, Add, Manage Traits List, GUI_TraitAdd
	Menu OptionsMenu, Add, Manage Language List, GUI_LangAdd
	Menu OptionsMenu, Add
	Menu OptionsMenu, Add, Viewport Theme, :ThemeMenu
	Menu OptionsMenu, Add
	Menu OptionsMenu, Add, Settings`tF11, GUI_Options
	Menu OptionsMenu, Icon, Settings`tF11, NPC Engineer.dll, 9
	Menu NPCEngineerMenu, Add, Options, :OptionsMenu
	
	Component_Menu("ComponentMenu", "npc")
	Menu NPCEngineerMenu, Add, Engineer Suite, :ComponentMenu
	
	Explorer_Menu("ExplorerMenu")
	Menu NPCEngineerMenu, Add, Directories, :ExplorerMenu

	Backup_Menu("BackupMenu")
	Menu NPCEngineerMenu, Add, Backup, :BackupMenu

	Help_Menu("HelpMenu", "NPC Engineer")
	Menu NPCEngineerMenu, Add, Information, :HelpMenu
	Gui, NPCE_Main:Menu, NPCEngineerMenu
;}
	

; Tab 3 system for all NPC input
	Gui, NPCE_Main:Add, Tab3, x7 y45 w606 h550 vMainTabName, Base Stats|Resistances|Skills|Traits|Innate/Psionics|Casting|Actions|Description|Image

;  ================================================
;  |         GUI for the 'base stats' tab         |
;  ================================================
;{
	Gui, NPCE_Main:Tab, 1
	
	; Titles
	Gui, NPCE_Main:font, S10, Arial Bold
	Gui, NPCE_Main:Add, Text, x390 y132 w90 h17 Center, Speeds (ft.)
	Gui, NPCE_Main:Add, Text, x320 y309 w280 h17 Center, Sense Ranges (ft.)
	Gui, NPCE_Main:Add, Text, x28 y309 w106 h17 Center, Ability Scores
	Gui, NPCE_Main:Add, Text, x188 y309 w96 h17 Center gSTTransfer, Saving Throws

	; Groupboxes
	Gui, NPCE_Main:font, S10 c727178, Arial Bold
	Gui, NPCE_Main:Add, GroupBox, x14 y82 w592 h205, NPC Information
	Gui, NPCE_Main:Add, GroupBox, x14 y290 w290 h205, Statistics
	Gui, NPCE_Main:Add, GroupBox, x316 y290 w290 h205, Senses



	Gui, NPCE_Main:font, S10 c000000, Arial

	; Basic information
	xOptn:= "x102 y100 w204 h23 vNPCname"					;{
	xtext:= NPCname
	xFunc:= "Update_Output_Main"							;}
	vNPCname:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "x372 y100 w75 vNPCgender"						;{
	xtext:= "neutral||male|female"
	xFunc:= "Update_Output_Main"							;}
	vNPCgender:= new egui("Main", "DDL", xOptn, xtext, Func(xFunc))
	xOptn:= "x454 y101 w60 h23 vNPCunique"					;{
	xtext:= "Unique"
	xFunc:= "Update_Output_Main"							;}
	vNPCunique:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPCunique.set(NPCunique)
	xOptn:= "x520 y101 w55 h23 vNPCpropername"				;{
	xtext:= "Name"
	xFunc:= "Update_Output_Main"							;}
	vNPCpropername:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPCpropername.set(NPCpropername)
	xOptn:= "x102 y126 w204 vNPCsize"						;{
	xtext:= "Tiny|Small|Medium|Large|Huge|Gargantuan"
	xFunc:= "Update_Output_Main"							;}
	vNPCsize:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))
	xOptn:= "x102 y152 w204 vNPCtype"						;{
	xtext:= "aberration|beast|celestial|construct|dragon|elemental|fey|fiend|giant|humanoid|monstrosity|ooze|plant|undead| |swarm of tiny aberrations|swarm of tiny beasts|swarm of tiny constructs|swarm of tiny elementals|swarm of tiny monstrosities|swarm of tiny oozes|swarm of tiny plants| |trap"
	xFunc:= "Update_Output_Main"							;}
	vNPCtype:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))
	xOptn:= "x102 y178 w123 vNPCtag"							;{
	xtext:= "aarakocra|any race|bullywug|demon|derro|devil|dragonborn|dwarf|elf|firenewt|genasi|gith|gnoll|gnome|goblinoid|grimlock|grung|half-elf|half-orc|halfling|human|kenku|kobold|kuo-toa|lizardfolk|merfolk|orc|quaggoth|ratfolk|sahuagin|shapechanger|thri-kreen|tiefling|titan|troglodyte|xvart|yuan-ti|yugoloth"
	xFunc:= "Update_Output_Main"							;}
	vNPCtag:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))
	xOptn:= "x102 y204 w123 vNPCalign"						;{
	xtext:= "unaligned| |lawful good|lawful neutral|lawful evil| |neutral good|true neutral|neutral evil| |chaotic good|chaotic neutral|chaotic evil"
	xFunc:= "Update_Output_Main"							;}
	vNPCalign:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))
	xOptn:= "x102 y230 w123 h23 vNPCac"						;{
	xtext:= NPCac
	xFunc:= "Update_Output_Main"							;}
	vNPCac:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "x102 y256 w123 h23 vNPChp"						;{
	xtext:= NPChp
	xFunc:= "Update_Output_Main"							;}
	vNPChp:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))

	Gui, NPCE_Main:font, S8, Arial

	xOptn:= "x229 y256 w37 h23 +border -tabstop vButtonAverageHP"	;{
	xtext:= "Mean"
	xFunc:= "HP_Average"									;}
	vButtonAverageHP:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))
	xOptn:= "x270 y256 w37 h23 +border -tabstop vButtonRollHP"		;{
	xtext:= "Roll"
	xFunc:= "HP_Roll"										;}
	vButtonRollHP:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))

	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Text, x22 y103 w75 h17 Right, NPC Name
	Gui, NPCE_Main:Add, Text, x311 y103 w55 h17 Right, Gender
	Gui, NPCE_Main:Add, Text, x22 y129 w75 h17 Right, Size
	Gui, NPCE_Main:Add, Text, x22 y155 w75 h17 Right, Type
	Gui, NPCE_Main:Add, Text, x22 y181 w75 h17 Right, Tag
	Gui, NPCE_Main:Add, Text, x22 y207 w75 h17 Right, Alignment
	Gui, NPCE_Main:Add, Text, x22 y233 w75 h17 Right, Armor Class
	Gui, NPCE_Main:Add, Text, x22 y259 w75 h17 Right, Hit Points

	;~ ; Movement rates
	xOptn:= "x435 y152 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCwalk1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCwalk Range0-200"							;{
	xtext:= NPCwalk
	xFunc:= ""												;}
	vNPCwalk:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x435 y178 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCburrow1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCburrow Range0-200"							;{
	xtext:= NPCburrow
	xFunc:= ""												;}
	vNPCburrow:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x435 y204 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCclimb1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCclimb Range0-200"							;{
	xtext:= NPCclimb
	xFunc:= ""												;}
	vNPCclimb:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x435 y230 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCfly1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCfly Range0-200"							;{
	xtext:= NPCfly
	xFunc:= ""												;}
	vNPCfly:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
		xOptn:= "x495 y230 w60 h23 vNPChover"				;{
		xtext:= "Hover"
		xFunc:= "Update_Output_Main"						;}
		vNPChover:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
		vNPChover.set(NPChover)
	xOptn:= "x435 y256 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCswim1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCswim Range0-200"							;{
	xtext:= NPCswim
	xFunc:= ""												;}
	vNPCswim:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	
	Gui, NPCE_Main:Add, Text, x390 y155 w40 h17 Right, Speed
	Gui, NPCE_Main:Add, Text, x390 y181 w40 h17 Right, Burrow
	Gui, NPCE_Main:Add, Text, x390 y207 w40 h17 Right, Climb
	Gui, NPCE_Main:Add, Text, x390 y233 w40 h17 Right, Fly
	Gui, NPCE_Main:Add, Text, x390 y259 w40 h17 Right, Swim

	;~ ; Ability scores
	xOptn:= "x63 y330 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Stats"							;}
	vNPCstr1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCstr Range1-30"								;{
	xtext:= NPCstr
	xFunc:= ""												;}
	vNPCstr:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x63 y356 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Stats"							;}
	vNPCdex1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCdex Range1-30"								;{
	xtext:= NPCdex
	xFunc:= ""												;}
	vNPCdex:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x63 y382 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Stats"							;}
	vNPCcon1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCcon Range1-30"								;{
	xtext:= NPCcon
	xFunc:= ""												;}
	vNPCcon:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x63 y408 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Stats"							;}
	vNPCint1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCint Range1-30"								;{
	xtext:= NPCint
	xFunc:= ""												;}
	vNPCint:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	wisboy:=""
	xOptn:= "x63 y434 w41 h23 Center vwisboy"						;{
	xtext:= ""
	xFunc:= "Update_Output_Stats"							;}
	vNPCwis1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCwis Range1-30"								;{
	xtext:= NPCwis
	xFunc:= ""												;}
	vNPCwis:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x63 y460 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Stats"							;}
	vNPCcha1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCcha Range1-30"								;{
	xtext:= NPCcha
	xFunc:= ""												;}
	vNPCcha:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	
	Gui, NPCE_Main:Add, Text, x28 y334 w30 h17 Center, STR
	Gui, NPCE_Main:Add, Text, x28 y360 w30 h17 Center, DEX
	Gui, NPCE_Main:Add, Text, x28 y386 w30 h17 Center, CON
	Gui, NPCE_Main:Add, Text, x28 y412 w30 h17 Center, INT
	Gui, NPCE_Main:Add, Text, x28 y438 w30 h17 Center, WIS
	Gui, NPCE_Main:Add, Text, x28 y464 w30 h17 Center, CHA

	xOptn:= "x109 y334 w25 h17 vNPCstrbo Center"			;{
	xtext:= NPCstrbo
	xFunc:= ""												;}
	vNPCstrbo:= new egui("Main", "Text", xOptn, xtext, Func(xFunc))
	xOptn:= "x109 y360 w25 h17 vNPCdexbo Center"			;{
	xtext:= NPCdexbo
	xFunc:= ""												;}
	vNPCdexbo:= new egui("Main", "Text", xOptn, xtext, Func(xFunc))
	xOptn:= "x109 y386 w25 h17 vNPCconbo Center"			;{
	xtext:= NPCconbo
	xFunc:= ""												;}
	vNPCconbo:= new egui("Main", "Text", xOptn, xtext, Func(xFunc))
	xOptn:= "x109 y412 w25 h17 vNPCintbo Center"			;{
	xtext:= NPCintbo
	xFunc:= ""												;}
	vNPCintbo:= new egui("Main", "Text", xOptn, xtext, Func(xFunc))
	xOptn:= "x109 y438 w25 h17 vNPCwisbo Center"			;{
	xtext:= NPCwisbo
	xFunc:= ""												;}
	vNPCwisbo:= new egui("Main", "Text", xOptn, xtext, Func(xFunc))
	xOptn:= "x109 y464 w25 h17 vNPCchabo Center"			;{
	xtext:= NPCchabo
	xFunc:= ""												;}
	vNPCchabo:= new egui("Main", "Text", xOptn, xtext, Func(xFunc))

	;~ ; Saving throws
	
	xOptn:= "x228 y330 w46 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCstrsav1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCstrsav Range-20-20"						;{
	xtext:= NPCstrsav
	xFunc:= ""												;}
	vNPCstrsav:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x228 y356 w46 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCdexsav1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCdexsav Range-20-20"						;{
	xtext:= NPCdexsav
	xFunc:= ""												;}
	vNPCdexsav:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x228 y382 w46 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCconsav1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCconsav Range-20-20"						;{
	xtext:= NPCconsav
	xFunc:= ""												;}
	vNPCconsav:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x228 y408 w46 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCintsav1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCintsav Range-20-20"						;{
	xtext:= NPCintsav
	xFunc:= ""												;}
	vNPCintsav:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x228 y434 w46 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCwissav1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCwissav Range-20-20"						;{
	xtext:= NPCwissav
	xFunc:= ""												;}
	vNPCwissav:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x228 y460 w46 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCchasav1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCchasav Range-20-20"						;{
	xtext:= NPCchasav
	xFunc:= ""												;}
	vNPCchasav:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	
	Gui, NPCE_Main:Add, Text, x193 y334 w30 h17 Center, STR
	Gui, NPCE_Main:Add, Text, x193 y360 w30 h17 Center, DEX
	Gui, NPCE_Main:Add, Text, x193 y386 w30 h17 Center, CON
	Gui, NPCE_Main:Add, Text, x193 y412 w30 h17 Center, INT
	Gui, NPCE_Main:Add, Text, x193 y438 w30 h17 Center, WIS
	Gui, NPCE_Main:Add, Text, x193 y464 w30 h17 Center, CHA

	xOptn:= "x279 y334 w16 h16 -tabstop vNPC_FS_STR"		;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPC_FS_STR:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPC_FS_STR.set(NPC_FS_STR)
	xOptn:= "x279 y360 w16 h16 -tabstop vNPC_FS_DEX"		;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPC_FS_DEX:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPC_FS_DEX.set(NPC_FS_DEX)
	xOptn:= "x279 y386 w16 h16 -tabstop vNPC_FS_CON"		;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPC_FS_CON:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPC_FS_CON.set(NPC_FS_CON)
	xOptn:= "x279 y412 w16 h16 -tabstop vNPC_FS_INT"		;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPC_FS_INT:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPC_FS_INT.set(NPC_FS_INT)
	xOptn:= "x279 y438 w16 h16 -tabstop vNPC_FS_WIS"		;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPC_FS_WIS:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPC_FS_WIS.set(NPC_FS_WIS)
	xOptn:= "x279 y464 w16 h16 -tabstop vNPC_FS_CHA"		;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPC_FS_CHA:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPC_FS_CHA.set(NPC_FS_CHA)


	;~ ; Senses inc passive perception
	xOptn:= "x435 y330 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCblind1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCblind Range0-600"							;{
	xtext:= NPCblind
	xFunc:= ""												;}
	vNPCblind:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
		xOptn:= "x495 y330 w105 h23 vNPCblindB"				;{
		xtext:= "blind beyond"
		xFunc:= "Update_Output_Main"						;}
		vNPCblindB:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
		vNPCblindB.set(NPCblindB)
	xOptn:= "x435 y356 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCdark1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCdark Range0-600"							;{
	xtext:= NPCdark
	xFunc:= ""												;}
	vNPCdark:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x435 y382 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCtremor1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCtremor Range0-600"							;{
	xtext:= NPCtremor
	xFunc:= ""												;}
	vNPCtremor:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x435 y408 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCtrue1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCtrue Range0-600"							;{
	xtext:= NPCtrue
	xFunc:= ""												;}
	vNPCtrue:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x435 y434 w50 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Output_Main"							;}
	vNPCpassperc1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCpassperc Range1-40"						;{
	xtext:= NPCpassperc
	xFunc:= ""												;}
	vNPCpassperc:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))

	Gui, NPCE_Main:Add, Text, x320 y334 w110 h17 Right, Blindsight
	Gui, NPCE_Main:Add, Text, x320 y360 w110 h17 Right, Darkvision
	Gui, NPCE_Main:Add, Text, x320 y386 w110 h17 Right, Tremorsense
	Gui, NPCE_Main:Add, Text, x320 y412 w110 h17 Right, Truesight
	Gui, NPCE_Main:Add, Text, x320 y438 w110 h17 Right, passive Perception

	xOptn:= "x79 y508 w45 vNPCcharat"						;{
	xtext:= "0|1/8|1/4|1/2|1||2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30"
	xFunc:= "Update_Output_Main_CR"							;}
	vNPCcharat:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))
	
	xOptn:= "x202 y508 w68 h23 vNPCxp Right"				;{
	xtext:= NPCxp
	xFunc:= "Update_Output_Main"							;}
	vNPCxp:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	
	Gui, NPCE_Main:Add, Text, x14 y511 w60 h17 Right, Challenge
	Gui, NPCE_Main:Add, Text, x132 y511 w65 h17 Right, Experience
	Gui, NPCE_Main:Add, Text, x275 y511 w25 h17, XP
	Gui, NPCE_Main:Add, Text, x456 y511 w65 h17 Right, NPC Token
	
	;~ Gui, NPCE_Main:Add, Text, x530 y522 w72 vGoingToken Center, Click to select NPC token
	PicWinOptions := ( SS_BITMAP := 0xE ) | ( SS_CENTERIMAGE := 0x200 )
	Gui, NPCE_Main:Add, Picture, x526 y508 w80 h80 %PicWinOptions% BackgroundTrans Border hwndhwndNPCtoken vNPCToken gNPCToken,

	Gui, NPCE_Main:font, S8, Arial
	Gui, NPCE_Main:Add, Text, x15 y563 w450 h17, Use <CTRL><Page Down> and <CTRL><Page Up> to cycle through tabs.
	Gui, NPCE_Main:Add, Text, x15 y577 w450 h17, Press <F1> to <F9> to select a tab directly.
	Gui, NPCE_Main:font, S10 c000000, Arial

;}

;  ================================================
;  |        GUI for the 'resistances' tab         |
;  ================================================
;{
	Gui, NPCE_Main:Tab, 2

	;~ ; Groupboxes
	Gui, NPCE_Main:font, S10 c727178, Arial Bold
	Gui, NPCE_Main:Add, GroupBox, x14 y82 w592 h60, Damage Vulnerabilities
	Gui, NPCE_Main:Add, GroupBox, x14 y149 w592 h170, Damage Resistances
	Gui, NPCE_Main:Add, GroupBox, x14 y323 w592 h152, Damage Immunities
	Gui, NPCE_Main:Add, GroupBox, x14 y482 w592 h105, Condition Immunities
 
	Gui, NPCE_Main:font, S8 c000000, Arial

	Gui, NPCE_Main:Add, Text, x20 y102 w45 h17 Center, Acid
	Gui, NPCE_Main:Add, Text, x65 y102 w45 h17 Center, Cold
	Gui, NPCE_Main:Add, Text, x110 y102 w45 h17 Center, Fire
	Gui, NPCE_Main:Add, Text, x155 y102 w45 h17 Center, Force
	Gui, NPCE_Main:Add, Text, x200 y102 w45 h17 Center, Lightning
	Gui, NPCE_Main:Add, Text, x245 y102 w45 h17 Center, Necrotic
	Gui, NPCE_Main:Add, Text, x290 y102 w45 h17 Center, Poison
	Gui, NPCE_Main:Add, Text, x335 y102 w45 h17 Center, Psychic
	Gui, NPCE_Main:Add, Text, x380 y102 w45 h17 Center, Radiant
	Gui, NPCE_Main:Add, Text, x425 y102 w45 h17 Center, Thunder
	Gui, NPCE_Main:Add, Text, x470 y102 w45 h17 Center, Bludg'ing
	Gui, NPCE_Main:Add, Text, x515 y102 w45 h17 Center, Piercing
	Gui, NPCE_Main:Add, Text, x558 y102 w45 h17 Center, Slashing

	Gui, NPCE_Main:Add, Text, x20 y169 w45 h17 Center, Acid
	Gui, NPCE_Main:Add, Text, x65 y169 w45 h17 Center, Cold
	Gui, NPCE_Main:Add, Text, x110 y169 w45 h17 Center, Fire
	Gui, NPCE_Main:Add, Text, x155 y169 w45 h17 Center, Force
	Gui, NPCE_Main:Add, Text, x200 y169 w45 h17 Center, Lightning
	Gui, NPCE_Main:Add, Text, x245 y169 w45 h17 Center, Necrotic
	Gui, NPCE_Main:Add, Text, x290 y169 w45 h17 Center, Poison
	Gui, NPCE_Main:Add, Text, x335 y169 w45 h17 Center, Psychic
	Gui, NPCE_Main:Add, Text, x380 y169 w45 h17 Center, Radiant
	Gui, NPCE_Main:Add, Text, x425 y169 w45 h17 Center, Thunder
	Gui, NPCE_Main:Add, Text, x470 y169 w45 h17 Center, Bludg'ing
	Gui, NPCE_Main:Add, Text, x515 y169 w45 h17 Center, Piercing
	Gui, NPCE_Main:Add, Text, x558 y169 w45 h17 Center, Slashing

	Gui, NPCE_Main:Add, Text, x20 y343 w45 h17 Center, Acid
	Gui, NPCE_Main:Add, Text, x65 y343 w45 h17 Center, Cold
	Gui, NPCE_Main:Add, Text, x110 y343 w45 h17 Center, Fire
	Gui, NPCE_Main:Add, Text, x155 y343 w45 h17 Center, Force
	Gui, NPCE_Main:Add, Text, x200 y343 w45 h17 Center, Lightning
	Gui, NPCE_Main:Add, Text, x245 y343 w45 h17 Center, Necrotic
	Gui, NPCE_Main:Add, Text, x290 y343 w45 h17 Center, Poison
	Gui, NPCE_Main:Add, Text, x335 y343 w45 h17 Center, Psychic
	Gui, NPCE_Main:Add, Text, x380 y343 w45 h17 Center, Radiant
	Gui, NPCE_Main:Add, Text, x425 y343 w45 h17 Center, Thunder
	Gui, NPCE_Main:Add, Text, x470 y343 w45 h17 Center, Bludg'ing
	Gui, NPCE_Main:Add, Text, x515 y343 w45 h17 Center, Piercing
	Gui, NPCE_Main:Add, Text, x558 y343 w45 h17 Center, Slashing

	Gui, NPCE_Main:font, S10 c000000, Arial

	;~ ; Damage Vulnerability
	xOptn:= "x36 y117 w26 h16 vcbDV1"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV1:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV1.set(cbDV1)
	xOptn:= "x81 y117 w26 h16 vcbDV2"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV2:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV2.set(cbDV2)
	xOptn:= "x126 y117 w26 h16 vcbDV3"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV3:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV3.set(cbDV3)
	xOptn:= "x171 y117 w26 h16 vcbDV4"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV4:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV4.set(cbDV4)
	xOptn:= "x216 y117 w26 h16 vcbDV5"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV5:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV5.set(cbDV5)
	xOptn:= "x261 y117 w26 h16 vcbDV6"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV6:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV6.set(cbDV6)
	xOptn:= "x306 y117 w26 h16 vcbDV7"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV7:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV7.set(cbDV7)
	xOptn:= "x351 y117 w26 h16 vcbDV8"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV8:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV8.set(cbDV8)
	xOptn:= "x396 y117 w26 h16 vcbDV9"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV9:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV9.set(cbDV9)
	xOptn:= "x441 y117 w26 h16 vcbDV10"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV10:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV10.set(cbDV10)
	xOptn:= "x486 y117 w26 h16 vcbDV11"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV11:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV11.set(cbDV11)
	xOptn:= "x531 y117 w26 h16 vcbDV12"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV12:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV12.set(cbDV12)
	xOptn:= "x576 y117 w26 h16 vcbDV13"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dv"							;}
	vcbDV13:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDV13.set(cbDV13)

	;~ ; Damage Resistance
	xOptn:= "x36 y184 w26 h16 vcbDR1"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR1:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR1.set(cbDR1)
	xOptn:= "x81 y184 w26 h16 vcbDR2"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR2:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR2.set(cbDR2)
	xOptn:= "x126 y184 w26 h16 vcbDR3"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR3:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR3.set(cbDR3)
	xOptn:= "x171 y184 w26 h16 vcbDR4"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR4:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR4.set(cbDR4)
	xOptn:= "x216 y184 w26 h16 vcbDR5"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR5:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR5.set(cbDR5)
	xOptn:= "x261 y184 w26 h16 vcbDR6"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR6:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR6.set(cbDR6)
	xOptn:= "x306 y184 w26 h16 vcbDR7"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR7:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR7.set(cbDR7)
	xOptn:= "x351 y184 w26 h16 vcbDR8"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR8:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR8.set(cbDR8)
	xOptn:= "x396 y184 w26 h16 vcbDR9"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR9:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR9.set(cbDR9)
	xOptn:= "x441 y184 w26 h16 vcbDR10"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR10:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR10.set(cbDR10)
	xOptn:= "x486 y184 w26 h16 vcbDR11"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR11:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR11.set(cbDR11)
	xOptn:= "x531 y184 w26 h16 vcbDR12"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR12:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR12.set(cbDR12)
	xOptn:= "x576 y184 w26 h16 vcbDR13"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_dr"							;}
	vcbDR13:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDR13.set(cbDR13)

	xOptn:= "x36 y204 w383 h20 vDRRadio1"						;{
	xtext:= "No special weapon resistances."
	xFunc:= "Update_dmg_dr"							;}
	vDRRadio1:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDRRadio1.set(DRRadio1)

	xOptn:= "x36 y222 w383 h20 vDRRadio2"						;{
	xtext:= "Resistant to nonmagical weapons."
	xFunc:= "Update_dmg_dr"							;}
	vDRRadio2:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDRRadio2.set(DRRadio2)

	xOptn:= "x36 y240 w383 h20 vDRRadio3"						;{
	xtext:= "Resistant to nonmagical weapons that aren't silvered."
	xFunc:= "Update_dmg_dr"							;}
	vDRRadio3:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDRRadio3.set(DRRadio3)

	xOptn:= "x36 y258 w383 h20 vDRRadio4"						;{
	xtext:= "Resistant to nonmagical weapons that aren't adamantine."
	xFunc:= "Update_dmg_dr"							;}
	vDRRadio4:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDRRadio4.set(DRRadio4)

	xOptn:= "x36 y276 w383 h20 vDRRadio6"						;{
	xtext:= "Resistant to nonmagical weapons that aren't cold-forged iron."
	xFunc:= "Update_dmg_dr"							;}
	vDRRadio6:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDRRadio6.set(DRRadio6)

	xOptn:= "x36 y294 w383 h20 vDRRadio5"						;{
	xtext:= "Resistant to magic weapons."
	xFunc:= "Update_dmg_dr"							;}
	vDRRadio5:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDRRadio5.set(DRRadio5)

	;~ ; Damage Immunity
	xOptn:= "x36 y358 w26 h16 vcbDI1"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI1:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI1.set(cbDI1)
	xOptn:= "x81 y358 w26 h16 vcbDI2"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI2:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI2.set(cbDI2)
	xOptn:= "x126 y358 w26 h16 vcbDI3"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI3:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI3.set(cbDI3)
	xOptn:= "x171 y358 w26 h16 vcbDI4"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI4:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI4.set(cbDI4)
	xOptn:= "x216 y358 w26 h16 vcbDI5"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI5:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI5.set(cbDI5)
	xOptn:= "x261 y358 w26 h16 vcbDI6"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI6:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI6.set(cbDI6)
	xOptn:= "x306 y358 w26 h16 vcbDI7"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI7:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI7.set(cbDI7)
	xOptn:= "x351 y358 w26 h16 vcbDI8"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI8:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI8.set(cbDI8)
	xOptn:= "x396 y358 w26 h16 vcbDI9"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI9:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI9.set(cbDI9)
	xOptn:= "x441 y358 w26 h16 vcbDI10"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI10:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI10.set(cbDI10)
	xOptn:= "x486 y358 w26 h16 vcbDI11"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI11:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI11.set(cbDI11)
	xOptn:= "x531 y358 w26 h16 vcbDI12"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI12:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI12.set(cbDI12)
	xOptn:= "x576 y358 w26 h16 vcbDI13"						;{
	xtext:= "  "
	xFunc:= "Update_dmg_di"							;}
	vcbDI13:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbDI13.set(cbDI13)

	xOptn:= "x36 y378 w383 h20 vDIRadio1"						;{
	xtext:= "No special weapon immunities."
	xFunc:= "Update_dmg_di"							;}
	vDIRadio1:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDIRadio1.set(DIRadio1)

	xOptn:= "x36 y396 w383 h20 vDIRadio2"						;{
	xtext:= "Immune to nonmagical weapons."
	xFunc:= "Update_dmg_di"							;}
	vDIRadio2:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDIRadio2.set(DIRadio2)

	xOptn:= "x36 y414 w383 h20 vDIRadio3"						;{
	xtext:= "Immune to nonmagical weapons that aren't silvered."
	xFunc:= "Update_dmg_di"							;}
	vDIRadio3:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDIRadio3.set(DIRadio3)

	xOptn:= "x36 y432 w383 h20 vDIRadio4"						;{
	xtext:= "Immune to nonmagical weapons that aren't adamantine."
	xFunc:= "Update_dmg_di"							;}
	vDIRadio4:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDIRadio4.set(DIRadio4)
	
	xOptn:= "x36 y450 w383 h20 vDIRadio5"						;{
	xtext:= "Immune to nonmagical weapons that aren't cold-forged iron."
	xFunc:= "Update_dmg_di"							;}
	vDIRadio5:= new egui("Main", "Radio", xOptn, xtext, Func(xFunc))
	vDIRadio5.set(DIRadio5)


	;~ ; Condition Immunity
	xOptn:= "x36 y502 w100 h16 vcbCI1"						;{
	xtext:= "Blinded"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI1:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI1.set(cbCI1)
	xOptn:= "x36 y522 w100 h16 vcbCI2"						;{
	xtext:= "Charmed"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI2:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI2.set(cbCI2)
	xOptn:= "x36 y542 w100 h16 vcbCI3"						;{
	xtext:= "Deafened"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI3:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI3.set(cbCI3)
	xOptn:= "x36 y562 w100 h16 vcbCI4"						;{
	xtext:= "Exhaustion"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI4:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI4.set(cbCI4)
	xOptn:= "x170 y502 w100 h16 vcbCI5"						;{
	xtext:= "Frightened"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI5:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI5.set(cbCI5)
	xOptn:= "x170 y522 w100 h16 vcbCI6"						;{
	xtext:= "Grappled"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI6:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI6.set(cbCI6)
	xOptn:= "x170 y542 w100 h16 vcbCI7"						;{
	xtext:= "Incapacitated"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI7:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI7.set(cbCI7)
	xOptn:= "x170 y562 w100 h16 vcbCI8"						;{
	xtext:= "Invisible"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI8:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI8.set(cbCI8)
	xOptn:= "x304 y502 w100 h16 vcbCI9"						;{
	xtext:= "Paralyzed"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI9:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI9.set(cbCI9)
	xOptn:= "x304 y522 w100 h16 vcbCI10"						;{
	xtext:= "Petrified"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI10:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI10.set(cbCI10)
	xOptn:= "x304 y542 w100 h16 vcbCI11"						;{
	xtext:= "Poisoned"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI11:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI11.set(cbCI11)
	xOptn:= "x304 y562 w100 h16 vcbCI12"						;{
	xtext:= "Prone"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI12:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI12.set(cbCI12)
	xOptn:= "x438 y502 w100 h16 vcbCI13"						;{
	xtext:= "Restrained"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI13:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI13.set(cbCI13)
	xOptn:= "x438 y522 w100 h16 vcbCI14"						;{
	xtext:= "Stunned"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI14:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI14.set(cbCI14)
	xOptn:= "x438 y542 w100 h16 vcbCI15"						;{
	xtext:= "Unconscious"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI15:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI15.set(cbCI15)
	xOptn:= "x438 y562 w60 h16 vcbCI16"						;{
	xtext:= "Other:"
	xFunc:= "Update_dmg_ci"							;}
	vcbCI16:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vcbCI16.set(cbCI16)
	
	xOptn:= "x498 y560 w100 h20 vCI16"						;{
	xtext:= ""
	xFunc:= "Update_dmg_ci"							;}
	vCI16:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
;}

;  ================================================
;  |          GUI for the 'skills' tab            |
;  ================================================
;{
	Gui, NPCE_Main:Tab, 3
	
	; Titles
	Gui, NPCE_Main:font, S10, Arial Bold
	Gui, NPCE_Main:Add, Text, x360 y92 w115 h17  Center, Standard
	Gui, NPCE_Main:Add, Text, x481 y92 w115 h17  Center, Exotic
	Gui, NPCE_Main:Add, Text, x360 y250 w115 h17  Center, Monstrous
	Gui, NPCE_Main:Add, Text, x481 y250 w115 h17  Center, User

	; Groupboxes
	Gui, NPCE_Main:font, S10 c727178, Arial Bold
	Gui, NPCE_Main:Add, GroupBox, x14 y72 w158 h515, Skill bonuses
	Gui, NPCE_Main:Add, GroupBox, x352 y72 w253 h515, Languages spoken
 
	Gui, NPCE_Main:font, S10 c000000, Arial

	; Skills
	xOptn:= "x119 y90 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_acro1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_acro Range-20-35"						;{
	xtext:= % NPC_Skills[Acrobatics]
	xFunc:= ""												;}
	vsk_acro:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y116 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_anim1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_anim Range-20-35"						;{
	xtext:= % NPC_Skills[Animal Handling]
	xFunc:= ""												;}
	vsk_anim:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y142 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_arca1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_arca Range-20-35"						;{
	xtext:= % NPC_Skills[Arcana]
	xFunc:= ""												;}
	vsk_arca:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y168 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_athl1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_athl Range-20-35"						;{
	xtext:= % NPC_Skills[Athletics]
	xFunc:= ""												;}
	vsk_athl:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y194 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_dece1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_dece Range-20-35"						;{
	xtext:= % NPC_Skills[Deception]
	xFunc:= ""												;}
	vsk_dece:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y220 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_hist1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_hist Range-20-35"						;{
	xtext:= % NPC_Skills[History]
	xFunc:= ""												;}
	vsk_hist:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y246 w41 h23 Center" 						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_insi1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_insi Range-20-35"						;{
	xtext:= % NPC_Skills[Insight]
	xFunc:= ""												;}
	vsk_insi:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y272 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_inti1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_inti Range-20-35"						;{
	xtext:= % NPC_Skills[Intimidation]
	xFunc:= ""												;}
	vsk_inti:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y298 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_inve1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_inve Range-20-35"						;{
	xtext:= % NPC_Skills[Investigation]
	xFunc:= ""												;}
	vsk_inve:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y324 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_medi1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_medi Range-20-35"						;{
	xtext:= % NPC_Skills[Medicine]
	xFunc:= ""												;}
	vsk_medi:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y350 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_natu1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_natu Range-20-35"						;{
	xtext:= % NPC_Skills[Nature]
	xFunc:= ""												;}
	vsk_natu:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y376 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_perc1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_perc Range-20-35"						;{
	xtext:= % NPC_Skills[Perception]
	xFunc:= ""												;}
	vsk_perc:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y402 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_perf1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_perf Range-20-35"						;{
	xtext:= % NPC_Skills[Performance]
	xFunc:= ""												;}
	vsk_perf:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y428 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_pers1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_pers Range-20-35"						;{
	xtext:= % NPC_Skills[Persuasion]
	xFunc:= ""												;}
	vsk_pers:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y454 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_reli1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_reli Range-20-35"						;{
	xtext:= % NPC_Skills[Religion]
	xFunc:= ""												;}
	vsk_reli:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y480 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_slei1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_slei Range-20-35"						;{
	xtext:= % NPC_Skills[Sleight of Hand]
	xFunc:= ""												;}
	vsk_slei:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y506 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_stea1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_stea Range-20-35"						;{
	xtext:= % NPC_Skills[Stealth]
	xFunc:= ""												;}
	vsk_stea:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))
	xOptn:= "x119 y532 w41 h23 Center"						;{
	xtext:= ""
	xFunc:= "Update_Skills"							;}
	vsk_surv1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vsk_surv Range-20-35"						;{
	xtext:= % NPC_Skills[Survival]
	xFunc:= ""												;}
	vsk_surv:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))

	Gui, NPCE_Main:Add, Text, x19 y93 w95 h17 Right, Acrobatics
	Gui, NPCE_Main:Add, Text, x19 y119 w95 h17 Right, Animal Handling
	Gui, NPCE_Main:Add, Text, x19 y145 w95 h17 Right, Arcana
	Gui, NPCE_Main:Add, Text, x19 y171 w95 h17 Right, Athletics
	Gui, NPCE_Main:Add, Text, x19 y197 w95 h17 Right, Deception
	Gui, NPCE_Main:Add, Text, x19 y223 w95 h17 Right, History
	Gui, NPCE_Main:Add, Text, x19 y249 w95 h17 Right, Insight
	Gui, NPCE_Main:Add, Text, x19 y275 w95 h17 Right, Intimidation
	Gui, NPCE_Main:Add, Text, x19 y301 w95 h17 Right, Investigation
	Gui, NPCE_Main:Add, Text, x19 y327 w95 h17 Right, Medicine
	Gui, NPCE_Main:Add, Text, x19 y353 w95 h17 Right, Nature
	Gui, NPCE_Main:Add, Text, x19 y379 w95 h17 Right, Perception
	Gui, NPCE_Main:Add, Text, x19 y405 w95 h17 Right, Performance
	Gui, NPCE_Main:Add, Text, x19 y431 w95 h17 Right, Persuasion
	Gui, NPCE_Main:Add, Text, x19 y457 w95 h17 Right, Religion
	Gui, NPCE_Main:Add, Text, x19 y483 w95 h17 Right, Sleight of Hand
	Gui, NPCE_Main:Add, Text, x19 y509 w95 h17 Right, Stealth
	Gui, NPCE_Main:Add, Text, x19 y535 w95 h17 Right, Survival


	local arch1, arch2, arch3, arch4, key, val
	
	For key, val in LangStan
		Arch1 .= val "|"
	For key, val in LangExot
		Arch2 .= val "|"
	For key, val in LangMons
		Arch3 .= val "|"
	For key, val in LangUser
		Arch4 .= val "|"
	
	Gui, NPCE_Main:font, S9 c000000, Arial
	
	Gui, NPCE_Main:Add, ListBox, 8 x360 y109 R9 w115 sort vLang1 gUpdate_Output_Main, %arch1%
	Gui, NPCE_Main:Add, ListBox, 8 x481 y109 R9 w115 sort vLang2 gUpdate_Output_Main, %arch2%
	Gui, NPCE_Main:Add, ListBox, 8 x360 y268 R13 w115 sort vLang3 gUpdate_Output_Main, %arch3%
	Gui, NPCE_Main:Add, ListBox, 8 x481 y268 R13 w115 sort vUserLangs gUpdate_Output_Main, %arch4%
		
	Gui, NPCE_Main:font, S8 c000000, Arial
	
	Gui, NPCE_Main:Add, DropDownList, x360 y494 w236 AltSubmit vLangSelect gUpdate_Output_Main, No special conditions||Speaks no languages|Speaks all languages|Speaks languages it knew in life|Can't speak; knows selected languages|Can't speak; knows creator's languages|Can't speak; knows languages known in life|Alternative language text (enter below)
	
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Text, x360 y475 w200 h17, Language Options:
	
	xOptn:= "x360 y522 w236 h20 vLangAlt"						;{
	xtext:= LangAlt
	xFunc:= "Update_Output_Main"							;}
	vLangAlt:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))

	xOptn:= "x360 y561 w75 h20 vNPCtelep"						;{
	xtext:= "Telepathy"
	xFunc:= "Update_Output_Main"							;}
	vNPCtelep:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPCtelep.set(NPCtelep)
	
	Gui, NPCE_Main:Add, Text, x444 y562 w45 h17 Right, Range:
	
	xOptn:= "x491 y560 w105 h20 vtelrange"						;{
	xtext:= telrange
	xFunc:= "Update_Output_Main"							;}
	vtelrange:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
;}

;  ================================================
;  |          GUI for the 'traits' tab            |
;  ================================================
;{
	Gui, NPCE_Main:Tab, 4
	
	; Groupboxes
	Gui, NPCE_Main:font, S10 c727178, Arial Bold
	Gui, NPCE_Main:Add, GroupBox, x14 y72 w592 h515, NPC Traits
 
	Gui, NPCE_Main:font, S10 c000000, Arial

	; Traits
	Gui, NPCE_Main:Add, Edit, vtraitname1 x22 y92 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vtrait1 x158 y92 w440 h39 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x66 y112 w20 h20 vTEB1 gEdit_Trait -Tabstop, % Chr(9998)
	Gui, NPCE_Main:Add, Button, x86 y112 w20 h20 vTDB1 gDelete_Trait -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x134 y112 w20 h20 vTLB1 gTraitDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vtraitname2 x22 y133 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vtrait2 x158 y133 w440 h39 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x66 y153 w20 h20 vTEB2 gEdit_Trait -Tabstop, % Chr(9998)
	Gui, NPCE_Main:Add, Button, x86 y153 w20 h20 vTDB2 gDelete_Trait -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x114 y153 w20 h20 vTHB2 gTraitUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x134 y153 w20 h20 vTLB2 gTraitDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vtraitname3 x22 y174 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vtrait3 x158 y174 w440 h39 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x66 y194 w20 h20 vTEB3 gEdit_Trait -Tabstop, % Chr(9998)
	Gui, NPCE_Main:Add, Button, x86 y194 w20 h20 vTDB3 gDelete_Trait -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x114 y194 w20 h20 vTHB3 gTraitUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x134 y194 w20 h20 vTLB3 gTraitDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial
	
	Gui, NPCE_Main:Add, Edit, vtraitname4 x22 y215 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vtrait4 x158 y215 w440 h39 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x66 y235 w20 h20 vTEB4 gEdit_Trait -Tabstop, % Chr(9998)
	Gui, NPCE_Main:Add, Button, x86 y235 w20 h20 vTDB4 gDelete_Trait -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x114 y235 w20 h20 vTHB4 gTraitUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x134 y235 w20 h20 vTLB4 gTraitDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial
	
	Gui, NPCE_Main:Add, Edit, vtraitname5 x22 y256 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vtrait5 x158 y256 w440 h39 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x66 y276 w20 h20 vTEB5 gEdit_Trait -Tabstop, % Chr(9998)
	Gui, NPCE_Main:Add, Button, x86 y276 w20 h20 vTDB5 gDelete_Trait -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x114 y276 w20 h20 vTHB5 gTraitUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x134 y276 w20 h20 vTLB5 gTraitDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial
	
	Gui, NPCE_Main:Add, Edit, vtraitname6 x22 y297 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vtrait6 x158 y297 w440 h39 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x66 y317 w20 h20 vTEB6 gEdit_Trait -Tabstop, % Chr(9998)
	Gui, NPCE_Main:Add, Button, x86 y317 w20 h20 vTDB6 gDelete_Trait -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x114 y317 w20 h20 vTHB6 gTraitUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x134 y317 w20 h20 vTLB6 gTraitDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vtraitname7 x22 y338 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vtrait7 x158 y338 w440 h39 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x66 y358 w20 h20 vTEB7 gEdit_Trait -Tabstop, % Chr(9998)
	Gui, NPCE_Main:Add, Button, x86 y358 w20 h20 vTDB7 gDelete_Trait -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x114 y358 w20 h20 vTHB7 gTraitUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x134 y358 w20 h20 vTLB7 gTraitDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vtraitname8 x22 y379 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vtrait8 x158 y379 w440 h39 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x66 y399 w20 h20 vTEB8 gEdit_Trait -Tabstop, % Chr(9998)
	Gui, NPCE_Main:Add, Button, x86 y399 w20 h20 vTDB8 gDelete_Trait -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x114 y399 w20 h20 vTHB8 gTraitUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x134 y399 w20 h20 vTLB8 gTraitDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vtraitname9 x22 y420 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vtrait9 x158 y420 w440 h39 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x66 y440 w20 h20 vTEB9 gEdit_Trait -Tabstop, % Chr(9998)
	Gui, NPCE_Main:Add, Button, x86 y440 w20 h20 vTDB9 gDelete_Trait -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x114 y440 w20 h20 vTHB9 gTraitUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x134 y440 w20 h20 vTLB9 gTraitDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial


	;~ ; Edit/Add trait
	jsonpath:= DataDir "\traits.json"
	If !traitDB {
		TraitDB:= new JSONfile(jsonpath)
		Traitlist:= ""
		For a, b in TraitDB.object()
		{
			Traitlist:= Traitlist a "|"
		}
	}

	
	xOptn:= "x100 y466 w200 Left vtraitnameNew"			;{
	xtext:= TraitList
	xFunc:= "TraitAddName"						;}
	vtraitnameNew:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))

	xOptn:= "x100 y491 w498 h70 Multi Left vtraitNew"			;{
	xtext:= ""
	xFunc:= ""						;}
	vtraitNew:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))

	Gui, NPCE_Main:Add, Text, x22 y468 w73 h17 Right, Trait name:
	Gui, NPCE_Main:Add, Text, x22 y493 w73 h17 Right, Description:

	xOptn:= "x518 y563 w80 h20 +border vButtonAddTrait"	;{
	xtext:= "Add Trait"
	xFunc:= "Add_Trait"								;}
	vButtonAddTrait:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))
	
;~ ;}

;  ================================================
;  |      GUI for the 'innate casting' tab        |
;  ================================================
;{
	Gui, NPCE_Main:Tab, 5	; All the GUI controls for the 'innate casting' tab.

	; Titles
	Gui, NPCE_Main:font, S10, Arial Bold
	Gui, NPCE_Main:Add, Text, x17 y225 w93 h17 Center, # of Casts
	Gui, NPCE_Main:Add, Text, x120 y225 w75 h17 Left, Spells

	; Groupboxes
	Gui, NPCE_Main:font, S10 c727178, Arial Bold
	Gui, NPCE_Main:Add, GroupBox, x14 y104 w592 h77, Caster Information
	Gui, NPCE_Main:Add, GroupBox, x14 y199 w592 h212, Spell Selection



	Gui, NPCE_Main:font, S10 c000000, Arial

	; Caster Information
	xOptn:= "x25 y80 w275 h20 vFlagInSpell" 										;{
	xtext:= "Include 'Innate Spellcasting' section for NPC"
	xFunc:= "Update_InCasting"
	vFlagInSpell:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vFlagInSpell.set(FlagInSpell)													;}

	xOptn:= "x325 y80 w200 h20 vNPCPsionics" 										;{
	xtext:= "Mark section as 'Psionics'"
	xFunc:= "Update_Psionics"
	vNPCPsionics:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vNPCPsionics.set(NPCPsionics)													;}

	xOptn:= "x142 y123 w100 vNPCinspability" 										;{
	xtext:= "Strength|Dexterity|Constitution|Intelligence|Wisdom|Charisma"
	xFunc:= "Update_InCasting"	
	vNPCinspability:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))		;}
	
	xOptn:= "x357 y123 w60 h23 Center"												;{
	xtext:= ""
	xFunc:= "Update_inCasting"
	vNPCinspsave1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCinspsave Range0-30"
	xtext:= NPCinspsave
	xFunc:= ""
	vNPCinspsave:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x525 y123 w60 h23 Center"												;{
	xtext:= ""
	xFunc:= "Update_inCasting"
	vNPCinsptohit1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCinsptohit Range0-30"
	xtext:= NPCinsptohit
	xFunc:= ""
	vNPCinsptohit:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))			;}

	xOptn:= "x142 y151 w453 h24 vNPCinsptext" 										;{
	xtext:= NPCinsptext
	xFunc:= "Update_InCasting"
	vNPCinsptext:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))			;}

	Gui, NPCE_Main:Add, Text, x17 y126 w120 h17 Right, Spellcasting Ability
	Gui, NPCE_Main:Add, Text, x252 y126 w100 h17 Right, Spell Save DC
	Gui, NPCE_Main:Add, Text, x427 y126 w93 h17 Right, Spell Hit Bonus
	Gui, NPCE_Main:Add, Text, x17 y155 w120 h17 Right, Component Text
	
	;~ ; Spell Information
	xOptn:= "x120 y247 w475 h24 vInSp_0_spells" 									;{
	xtext:= ""
	xFunc:= "Update_InCasting"
	vInSp_0_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))			;}
	xOptn:= "x120 y273 w475 h24 vInSp_5_spells" 									;{
	xtext:= ""
	xFunc:= "Update_InCasting"
	vInSp_5_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))			;}
	xOptn:= "x120 y299 w475 h24 vInSp_4_spells" 									;{
	xtext:= ""
	xFunc:= "Update_InCasting"
	vInSp_4_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))			;}
	xOptn:= "x120 y325 w475 h24 vInSp_3_spells" 									;{
	xtext:= ""
	xFunc:= "Update_InCasting"
	vInSp_3_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))			;}
	xOptn:= "x120 y351 w475 h24 vInSp_2_spells" 									;{
	xtext:= ""
	xFunc:= "Update_InCasting"
	vInSp_2_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))			;}
	xOptn:= "x120 y377 w475 h24 vInSp_1_spells" 									;{
	xtext:= ""
	xFunc:= "Update_InCasting"
	vInSp_1_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))			;}

	Gui, NPCE_Main:Add, Text, x17 y250 w93 h17 Center, At will:
	Gui, NPCE_Main:Add, Text, x17 y276 w93 h17 Center, 5/day each:
	Gui, NPCE_Main:Add, Text, x17 y302 w93 h17 Center, 4/day each:
	Gui, NPCE_Main:Add, Text, x17 y328 w93 h17 Center, 3/day each:
	Gui, NPCE_Main:Add, Text, x17 y354 w93 h17 Center, 2/day each:
	Gui, NPCE_Main:Add, Text, x17 y380 w93 h17 Center, 1/day each:
;}

;  ================================================
;  |       GUI for the 'spellcasting' tab         |
;  ================================================
;{
	Gui, NPCE_Main:Tab, 6	; All the GUI controls for the 'spellcasting' tab.

	; Titles
	Gui, NPCE_Main:font, S10, Arial Bold
	Gui, NPCE_Main:Add, Text, x17 y253 w93 h17 Center, Spell Level
	Gui, NPCE_Main:Add, Text, x115 y253 w75 h17 Center, # of Casts
	Gui, NPCE_Main:Add, Text, x195 y253 w75 h17 Left, Spells

	; Groupboxes
	Gui, NPCE_Main:font, S10 c727178, Arial Bold
	Gui, NPCE_Main:Add, GroupBox, x14 y104 w592 h111, Caster Information
	Gui, NPCE_Main:Add, GroupBox, x14 y230 w592 h348, Spell Selection


	Gui, NPCE_Main:font, S10 c000000, Arial

	; Caster Information
	xOptn:= "x25 y80 w400 h20 vFlagSpell" Checked									;{
	xtext:= "Include 'Spellcasting' section for NPC"
	xFunc:= "Update_Casting"
	vFlagSpell:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vFlagSpell.set(FlagSpell)														;}

	xOptn:= "x115 y123 w60 vNPCsplevel" 											;{
	xtext:= "1st|2nd|3rd|4th|5th|6th|7th|8th|9th|10th|11th|12th|13th|14th|15th|16th|17th|18th|19th|20th|21st|22nd|23rd|24th|25th|26th|27th|28th|29th|30th"
	xFunc:= "Update_Casting"
	vNPCsplevel:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x310 y123 w100 vNPCspability" 											;{
	xtext:= "Strength|Dexterity|Constitution|Intelligence|Wisdom|Charisma"
	xFunc:= "Update_Casting"
	vNPCspability:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x525 y123 w60 h23 Center"												;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vNPCspsave1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCspsave Range0-30"
	xtext:= NPCspsave
	xFunc:= ""
	vNPCspsave:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x115 y151 w60 h23 Center"												;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vNPCsptohit1:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))
	xOptn:= "vNPCsptohit Range0-30"
	xtext:= NPCsptohit
	xFunc:= ""
	vNPCsptohit:= new egui("Main", "UpDown", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x310 y151 w100 vNPCspclass" 											;{
	xtext:= "Cleric|Wizard|Bard|Druid|Paladin|Ranger|Sorcerer|Warlock"
	xFunc:= "Update_Casting"
	vNPCspclass:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y182 w470 h23 vNPCspflavour" 										;{
	xtext:= NPCspflavour
	xFunc:= "Update_Casting"				
	vNPCspflavour:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}

	Gui, NPCE_Main:Add, Text, x17 y126 w93 h17 Right, Caster level
	Gui, NPCE_Main:Add, Text, x185 y126 w120 h17 Right, Spellcasting Ability
	Gui, NPCE_Main:Add, Text, x420 y126 w100 h17 Right, Spell Save DC
	Gui, NPCE_Main:Add, Text, x17 y154 w93 h17 Right, Spell Hit Bonus
	Gui, NPCE_Main:Add, Text, x185 y154 w120 h17 Right, Spell Class
	Gui, NPCE_Main:Add, Text, x17 y185 w93 h17 Right, Flavor Text


	;~ ; Spell Information
	xOptn:= "x115 y275 w75 vSp_0_casts" 											;{
	xtext:= "At will||1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"					
	vSp_0_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y301 w75 vSp_1_casts" 											;{
	xtext:= "At will|1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"
	vSp_1_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y327 w75 vSp_2_casts" 											;{
	xtext:= "At will|1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"
	vSp_2_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y353 w75 vSp_3_casts" 											;{
	xtext:= "At will|1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"
	vSp_3_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y379 w75 vSp_4_casts" 											;{
	xtext:= "At will|1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"
	vSp_4_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y405 w75 vSp_5_casts" 											;{
	xtext:= "At will|1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"
	vSp_5_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y431 w75 vSp_6_casts" 											;{
	xtext:= "At will|1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"
	vSp_6_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y457 w75 vSp_7_casts" 											;{
	xtext:= "At will|1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"
	vSp_7_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y483 w75 vSp_8_casts" 											;{
	xtext:= "At will|1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"
	vSp_8_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x115 y509 w75 vSp_9_casts" 											;{
	xtext:= "At will|1 slot|2 slots|3 slots|4 slots|5 slots|6 slots|7 slots|8 slots|9 slots"
	xFunc:= "Update_Casting"
	vSp_9_casts:= new egui("Main", "ComboBox", xOptn, xtext, Func(xFunc))			;}
	
	
	xOptn:= "x195 y275 w400 vSp_0_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_0_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x195 y301 w400 vSp_1_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_1_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x195 y327 w400 vSp_2_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_2_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x195 y353 w400 vSp_3_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_3_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x195 y379 w400 vSp_4_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_4_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x195 y405 w400 vSp_5_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_5_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x195 y431 w400 vSp_6_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_6_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x195 y457 w400 vSp_7_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_7_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x195 y483 w400 vSp_8_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_8_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	xOptn:= "x195 y509 w400 vSp_9_spells" 											;{
	xtext:= ""
	xFunc:= "Update_Casting"
	vSp_9_spells:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	
	Gui, NPCE_Main:Add, Text, x17 y278 w93 h17 Center, Cantrips
	Gui, NPCE_Main:Add, Text, x17 y304 w93 h17 Center, 1st level
	Gui, NPCE_Main:Add, Text, x17 y330 w93 h17 Center, 2nd level
	Gui, NPCE_Main:Add, Text, x17 y356 w93 h17 Center, 3rd level
	Gui, NPCE_Main:Add, Text, x17 y382 w93 h17 Center, 4th level
	Gui, NPCE_Main:Add, Text, x17 y408 w93 h17 Center, 5th level
	Gui, NPCE_Main:Add, Text, x17 y434 w93 h17 Center, 6th level
	Gui, NPCE_Main:Add, Text, x17 y460 w93 h17 Center, 7th level
	Gui, NPCE_Main:Add, Text, x17 y486 w93 h17 Center, 8th level
	Gui, NPCE_Main:Add, Text, x17 y512 w93 h17 Center, 9th level

	xOptn:= "x115 y547 w480 h23 vNPCSpellStar" 										;{
	xtext:= NPCSpellStar
	xFunc:= "Update_Casting"				
	vNPCSpellStar:= new egui("Main", "Edit", xOptn, xtext, Func(xFunc))				;}
	Gui, NPCE_Main:Add, Text, x17 y550 w93 h17 Right, Marked Spells
	
;}

;  ================================================
;  |          GUI for the 'actions' tab           |
;  ================================================
;{
	Gui, NPCE_Main:Tab, 7	; All the GUI controls for the 'actions' tab.

	; Groupboxes
	Gui, NPCE_Main:font, S10 c727178, Arial Bold
	Gui, NPCE_Main:Add, GroupBox, x14 y82 w592 h330, Actions
	Gui, NPCE_Main:Add, GroupBox, x14 y427 w190 h160, Reactions
	Gui, NPCE_Main:Add, GroupBox, x215 y427 w190 h160, Legendary Actions
	Gui, NPCE_Main:Add, GroupBox, x416 y427 w190 h160, Lair Actions
 
	Gui, NPCE_Main:font, S9 c000000, Arial
	
	; Actions
	Gui, NPCE_Main:Add, Edit, vactionnameB1 x22 y102 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vactionB1 x158 y102 w440 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x84 y123 w20 h20 vActDB1 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x132 y123 w20 h20 vActLB1 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vactionnameB2 x22 y155 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vactionB2 x158 y155 w440 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x84 y176 w20 h20 vActDB2 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x112 y176 w20 h20 vActHB2 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x132 y176 w20 h20 vActLB2 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vactionnameB3 x22 y208 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vactionB3 x158 y208 w440 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x84 y229 w20 h20 vActDB3 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x112 y229 w20 h20 vActHB3 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x132 y229 w20 h20 vActLB3 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vactionnameB4 x22 y261 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vactionB4 x158 y261 w440 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x84 y282 w20 h20 vActDB4 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x112 y282 w20 h20 vActHB4 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x132 y282 w20 h20 vActLB4 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vactionnameB5 x22 y314 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:Add, Edit, vactionB5 x158 y314 w440 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x84 y335 w20 h20 vActDB5 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Main:Add, Button, x112 y335 w20 h20 vActHB5 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Main:Add, Button, x132 y335 w20 h20 vActLB5 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Main:font, S10 c000000, Arial
	
	Gui, NPCE_Main:font, S10 c000000, Arial


	xOptn:= "x468 y374 w130 h30 +border vButtonAddAction"							;{
	xtext:= "Add / Edit Actions"
	xFunc:= "Add_Action"
	vButtonAddAction:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))		;}

	; Reactions
	Gui, NPCE_Main:Add, Edit, vreactionnameB1 x22 y447 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x176 y447 w20 h20 hwndReActDB1 vReActDB1 gDelete_ReAction -Tabstop, % Chr(10062)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vreactionnameB2 x22 y472 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x176 y472 w20 h20 hwndReActDB2 vReActDB2 gDelete_ReAction -Tabstop, % Chr(10062)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vreactionnameB3 x22 y497 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x176 y497 w20 h20 hwndReActDB3 vReActDB3 gDelete_ReAction -Tabstop, % Chr(10062)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vreactionnameB4 x22 y522 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x176 y522 w20 h20 hwndReActDB4 vReActDB4 gDelete_ReAction -Tabstop, % Chr(10062)
	Gui, NPCE_Main:font, S10 c000000, Arial

	xOptn:= "x66 y549 w130 h30 +border vButtonAddReAction"							;{
	xtext:= "Reactions"
	xFunc:= "Add_ReAction"
	vButtonAddReAction:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))		;}

	;Legendary Actions
	Gui, NPCE_Main:Add, Edit, vlgactionnameB1 x223 y447 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x377 y447 w20 h20 hwndlgactDB1 vlgactDB1 gDelete_lgaction -Tabstop, % Chr(10062)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vlgactionnameB2 x223 y472 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x377 y472 w20 h20 hwndlgactDB2 vlgactDB2 gDelete_lgaction -Tabstop, % Chr(10062)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vlgactionnameB3 x223 y497 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x377 y497 w20 h20 hwndlgactDB3 vlgactDB3 gDelete_lgaction -Tabstop, % Chr(10062)
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vlgactionnameB4 x223 y522 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x377 y522 w20 h20 hwndlgactDB4 vlgactDB4 gDelete_lgaction -Tabstop, % Chr(10062)
	Gui, NPCE_Main:font, S10 c000000, Arial

	xOptn:= "x267 y549 w130 h30 +border vButtonAddlgaction"							;{
	xtext:= "Legendary Actions"
	xFunc:= "Add_lgaction"
	vButtonAddlgaction:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))		;}

	; Lair Actions
	Gui, NPCE_Main:Add, Edit, vlractionnameB1 x424 y447 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x578 y447 w20 h20 hwndlractDB1 vlractDB1 gDelete_lraction -Tabstop, % Chr(10062) 
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vlractionnameB2 x424 y472 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x578 y472 w20 h20 hwndlractDB2 vlractDB2 gDelete_lraction -Tabstop, % Chr(10062) 
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vlractionnameB3 x424 y497 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x578 y497 w20 h20 hwndlractDB3 vlractDB3 gDelete_lraction -Tabstop, % Chr(10062) 
	Gui, NPCE_Main:font, S10 c000000, Arial

	Gui, NPCE_Main:Add, Edit, vlractionnameB4 x424 y522 w149 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Main:font, S14 c000000, Arial
	Gui, NPCE_Main:Add, Button, x578 y522 w20 h20 hwndlractDB4 vlractDB4 gDelete_lraction -Tabstop, % Chr(10062) 
	Gui, NPCE_Main:font, S10 c000000, Arial

	xOptn:= "x468 y549 w130 h30 +border vButtonAddlraction"							;{
	xtext:= "Lair Actions"
	xFunc:= "Add_lraction"
	vButtonAddlraction:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))		;}



;}

;  ================================================
;  |        GUI for the 'description' tab         |
;  ================================================
;{
	Gui, NPCE_Main:Tab, 8	; All the GUI controls for the 'description' tab.

	; Groupboxes
	Gui, NPCE_Main:font, S10 c727178, Arial Bold
	Gui, NPCE_Main:Add, GroupBox, x288 y515 w154 h73, Elements
	Gui, NPCE_Main:Add, GroupBox, x447 y515 w154 h73, Paste Edits

	Gui, NPCE_Main:font, S10 c000000, Arial

	RE1:= New RichEdit("NPCE_Main", "x15 y77 w589 h395 vChosen_Desc_Text gNPCE_Descrip_Update_Output", True)
		RE1.wordwrap(true)
		RE1.ShowScrollBar(0, False)
		REbg:= 0
		REfr:= 0
		RE1.SetBkgndColor("White")
		RFont := RE1.GetFont()
		RFont.name:= "Arial"
		RFont.Color:= "Black"
		RFont.Size:= "10"
		RE1.SetFont(RFont)
		RE1.SetOptions(["AUTOWORDSELECTION","AUTOVSCROLL"])
		Spacing:= []
		Spacing.After:= 4
		RE1.SetParaSpacing(Spacing)
	
	Gui, NPCE_Main:font, S10 c000000 norm bold, Arial
	Gui, NPCE_Main:Add, Button, x25 y478 w20 h20 vBTSTB gSetFontStyle, B
	Gui, NPCE_Main:font, S10 c000000 norm italic, Arial
	Gui, NPCE_Main:Add, Button, x50 y478 w20 h20 vBTSTI gSetFontStyle, I
	Gui, NPCE_Main:font, S10 c000000 norm underline, Arial
	Gui, NPCE_Main:Add, Button, x75 y478 w20 h20 vBTSTU gSetFontStyle, U
	Gui, NPCE_Main:font, S10 c000000 norm, Arial

	Gui, NPCE_Main:font, S10 c000000 norm, Arial
	Gui, NPCE_Main:Add, Button, x25 y503 w20 h20 vBTSTH gREHeader, H
	Gui, NPCE_Main:font, S10 c000000 norm, Arial
	Gui, NPCE_Main:Add, Button, x50 y503 w20 h20 vBTSTa gREBody, T
	Gui, NPCE_Main:font, S12 c000000 norm, Arial
	Gui, NPCE_Main:Add, Button, x75 y503 w20 h20 vBTSTF gREframe, % Chr(128172)
	Gui, NPCE_Main:Add, Button, x100 y503 w20 h20 vBTSTL gREbullet, % Chr(8801)
	Gui, NPCE_Main:font, S10 c000000 norm, Arial
	
	Gui, NPCE_Main:Add, Button, x150 y478 w20 h20 vBTSTZ gundo, % Chr(11148)
	Gui, NPCE_Main:Add, Button, x173 y478 w20 h20 vBTSTY gredo, % Chr(11150)

	Gui, NPCE_Main:Add, Button, x288 y478 w20 h20 vBTSTC gBGCol, % Chr(9635)

	Gui, NPCE_Main:Add, Button, x200 y478 w75 h20 +border vButtonDesctextDelete gDesc_Text_Delete, Clear Text
	Gui, NPCE_Main:Add, Button, x200 y503 w75 h20 +border vNPCE_Paste gPaste, Paste Text

	Gui, NPCE_Main:Add, Button, x504 y478 w100 h20 +border gValXML, Validate XML


	xOptn:= "x296 y533 w142 h18 vDesc_Add_Text" 									;{
	xtext:= "Add Descriptive Text"
	xFunc:= "Update_Output_Main"
	vDesc_Add_Text:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vDesc_Add_Text.set(Desc_Add_Text)												;}
	
	xOptn:= "x296 y551 w142 h18 vDesc_NPC_Title" 									;{
	xtext:= "Add Title"
	xFunc:= "Update_Output_Main"
	vDesc_NPC_Title:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vDesc_NPC_Title.set(Desc_NPC_Title)												;}
	
	xOptn:= "x296 y567 w142 h18 vDesc_Spell_List" 									;{
	xtext:= "Include Spell List"
	xFunc:= "Update_Output_Main"
	vDesc_Spell_List:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vDesc_Spell_List.set(Desc_Spell_List)											;}

	Gui, NPCE_Main:Add, CheckBox, x455 y533 w142 h18 vDesc_fixes Checked%Desc_fixes%, Apply common fixes
	Gui, NPCE_Main:Add, CheckBox, x455 y551 w142 h18 vDesc_strip_lf Checked%Desc_strip_lf%, Strip 'new line' codes
	Gui, NPCE_Main:Add, CheckBox, x455 y567 w142 h18 vDesc_title Checked%Desc_title% , Pick out titles

	Gui, NPCE_Main:Add, Text, x25 y534 w42 h17 Right, Non-ID:
	Gui, NPCE_Main:Add, Edit, x69 y532 w206 h55 vNPCnoid gUpdate_Output_Main,


;}

;  ================================================
;  |           GUI for the 'image' tab            |
;  ================================================
;{
	Gui, NPCE_Main:Tab, 9	; All the GUI controls for the 'Image' tab.
	
	Gui, NPCE_Main:Add, Text, x15 y77 w590 h20 center, Click below to select NPC image
	PicWinOptions := ( SS_BITMAP := 0xE ) | ( SS_CENTERIMAGE := 0x200 )
	Gui, NPCE_Main:Add, Picture, x15 y97 w590 h400 %PicWinOptions% HwndhwndNPCimage BackgroundTrans Border vNPCimage gNPCimage,
	
	Gui, NPCE_Main:Add, Text, x15 y533 w75 h17 Right, Artist Name:
	Gui, NPCE_Main:Add, Text, x15 y563 w75 h17 Right, Link:
	Gui, NPCE_Main:Add, Edit, x95 y530 w400 h23 vNPCImArt gUpdate_Output_Main,
	Gui, NPCE_Main:Add, Edit, x95 y560 w400 h23 vNPCImLink gUpdate_Output_Main,

	xOptn:= "x15 y500 w300 h18 vDesc_Image_Link" 									;{
	xtext:= "Add Image Link to NPC Description"
	xFunc:= "Update_Output_Main"
	vDesc_Image_Link:= new egui("Main", "CheckBox", xOptn, xtext, Func(xFunc))
	vDesc_Image_Link.set(Desc_Image_Link)											;}
	
	Gui, NPCE_Main:Add, Button, x505 y502 w100 h20 +border vNpcClearImage gNpcClearImage, Clear Image


;}

Gui, NPCE_Main:Tab		; End of tab3 system.


; Other GUI controls on the main window (NPCE_Main)


	Gui NPCE_Main:Add, ActiveX, x620 y45 w500 h550 E0x200 +0x8000000 vViewPort, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	Gui NPCE_Main:Add, ActiveX, x620 y45 w500 h550 E0x200 hidden +0x8000000 vViewPortB, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	vpp:= "ViewPort"


	xOptn:= "x8 y605 w130 h30 +border vButtonImport"		;{
	xtext:= "&Import Text"
	xFunc:= "Import_Text"									;}
	vButtonImport:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))

	xOptn:= "x148 y605 w130 h30 +border vButtonTerrain"		;{
	xtext:= "FG List Options"
	xFunc:= "FG_Lists"									;}
	vButtonTerrain:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))

	Gui, NPCE_Main:Add, Text, x328 y610 w80 h17 Right, FG Category:
	Gui, NPCE_Main:Add, Combobox, x413 y607 w200 HwndNPCFGcat vFGcat gUpdate_Output_Main, 



	xOptn:= "x755 y605 w115 h30 +border vButtonSBClipboard"							;{
	xtext:= "Copy Statblock"
	xFunc:= "SBOutput_to_Clipboard"
	vButtonSBClipboard:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))		;}
	
	xOptn:= "x880 y605 w115 h30 +border vButtonToText"								;{
	xtext:= "Save NPC"
	xFunc:= "Save_NPC"
	vButtonToText:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))			;}
	
	xOptn:= "x1005 y605 w115 h30 +border vButtonOutputAppend"						;{
	xtext:= "Add to Project"
	xFunc:= "Output_Append"
	vButtonOutputAppend:= new egui("Main", "Button", xOptn, xtext, Func(xFunc))		;}

	Gui, NPCE_Main:font, S18 c000000, Arial
	Gui, NPCE_Main:Add, Button, x1125 y545 w24 h24 hwndbuttonup -Tabstop, % Chr(11165)
	Gui, NPCE_Main:Add, Button, x1125 y571 w24 h24 hwndbuttondn -Tabstop, % Chr(11167)

	Gui, NPCE_Main:font, S9 c000000, Segoe UI
	Gui, NPCE_Main:Add, StatusBar
	Gui, NPCE_Main:Default
	SB_SetParts(450, 250)
	SB_SetText(" " WinTProj, 1)
	Gui, NPCE_Main:font, S10 c000000, Arial
}

GUI_Import() {
	global
	
; Settings for text import window (NPCE_Import)
	Gui, NPCE_Import:+OwnerNPCE_Main
	Gui, NPCE_Import:-SysMenu
	Gui, NPCE_Import:+hwndNPCE_Import
	Gui, NPCE_Import:Color, 0xE2E1E8
	Gui, NPCE_Import:font, S10 c000000, Arial
	
	;~ Gui, NPCE_Import:Add, ActiveX, x480 y8 w500 h500 +vscroll -Tabstop vFix_Text, about:<!DOCTYPE html><meta http-equiv="X-UA-Compatible" content="IE=edge">
	
	Gui, NPCE_Import:Add, Edit, vCap_Text gNPCE_Import_Update_Output x8 y8 w442 h474 Multi, %Cap_Text%
	
	FT2:= New RichEdit("NPCE_Import", "x480 y8 w500 h500 -Tabstop vFix_Text", True)
		FT2.wordwrap(true)
		FT2.ShowScrollBar(0, False)
		REbg:= 0
		FT2.SetBkgndColor("White")
		FFFont := FT2.GetFont()
		FFFont.name:= "Arial"
		FFFont.Color:= "Black"
		FFFont.Size:= "10"
		FT2.SetFont(FFFont)

	Gui, NPCE_Import:Add, DropDownList, x300 y485 w150 vImportChoice gNPCE_Import_Update_Output, PDF & Web source||D&D Beyond|Fantasy Grounds XML|CritterDB|HeroLabs|Donjon|RPG Tinker|D&D Wiki|Incarnate|Roll20: 5E Shaped|Roll20 Compendium|MS Word Table 1|PDF Alternative 1|PDF Alternative 2
	Gui, NPCE_Import:Add, Button, x8 y515 w130 h30 +border vNPCE_Import_Delete gNPCE_Import_Delete, Delete All Text
	Gui, NPCE_Import:Add, Button, x150 y515 w130 h30 +border vNPCE_Import_Append gNPCE_Import_Append, Append Clipboard
	Gui, NPCE_Import:Add, Button, x710 y515 w130 h30 +border vNPCE_Import_Return gNPCE_Import_Return, Import and Return
	Gui, NPCE_Import:Add, Button, x852 y515 w130 h30 +border vNPCE_Import_Cancel gNPCE_Import_Cancel, Cancel All Changes
}

GUI_Actions() {
	global
	
	; Settings for Actions window (NPCE_Actions)
	Gui, NPCE_Actions:+OwnerNPCE_Main
	Gui, NPCE_Actions:-SysMenu
	Gui, NPCE_Actions:+hwndNPCE_Actions
	Gui, NPCE_Actions:Color, 0xE2E1E8
	Gui, NPCE_Actions:font, S10 c000000, Arial

	; Groupboxes
	Gui, NPCE_Actions:font, S10 c727178, Arial Bold
	Gui, NPCE_Actions:Add, GroupBox, x8 y8 w532 h609, Actions
	Gui, NPCE_Actions:Add, GroupBox, x560 y8 w532 h111, Multiattack
	Gui, NPCE_Actions:Add, GroupBox, x560 y122 w532 h356, Weapon Attack
	Gui, NPCE_Actions:Add, GroupBox, x560 y481 w532 h137, Other Action

	Gui, NPCE_Actions:font, S9 c000000, Arial
	
	;{ Actions
	Gui, NPCE_Actions:Add, Edit, vactionnameB1 x16 y28 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB1 x152 y28 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y49 w20 h20 vActEB1 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y49 w20 h20 vActDB1 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x128 y49 w20 h20 vActLB1 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial

	Gui, NPCE_Actions:Add, Edit, vactionnameB2 x16 y81 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB2 x152 y81 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y102 w20 h20 vActEB2 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y102 w20 h20 vActDB2 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y102 w20 h20 vActHB2 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:Add, Button, x128 y102 w20 h20 vActLB2 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial

	Gui, NPCE_Actions:Add, Edit, vactionnameB3 x16 y134 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB3 x152 y134 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y155 w20 h20 vActEB3 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y155 w20 h20 vActDB3 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y155 w20 h20 vActHB3 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:Add, Button, x128 y155 w20 h20 vActLB3 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial

	Gui, NPCE_Actions:Add, Edit, vactionnameB4 x16 y187 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB4 x152 y187 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y208 w20 h20 vActEB4 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y208 w20 h20 vActDB4 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y208 w20 h20 vActHB4 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:Add, Button, x128 y208 w20 h20 vActLB4 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial

	Gui, NPCE_Actions:Add, Edit, vactionnameB5 x16 y240 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB5 x152 y240 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y261 w20 h20 vActEB5 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y261 w20 h20 vActDB5 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y261 w20 h20 vActHB5 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:Add, Button, x128 y261 w20 h20 vActLB5 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial
	
	Gui, NPCE_Actions:Add, Edit, vactionnameB6 x16 y293 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB6 x152 y293 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y314 w20 h20 vActEB6 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y314 w20 h20 vActDB6 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y314 w20 h20 vActHB6 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:Add, Button, x128 y314 w20 h20 vActLB6 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial

	Gui, NPCE_Actions:Add, Edit, vactionnameB7 x16 y346 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB7 x152 y346 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y369 w20 h20 vActEB7 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y369 w20 h20 vActDB7 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y369 w20 h20 vActHB7 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:Add, Button, x128 y369 w20 h20 vActLB7 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial

	Gui, NPCE_Actions:Add, Edit, vactionnameB8 x16 y399 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB8 x152 y399 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y420 w20 h20 vActEB8 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y420 w20 h20 vActDB8 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y420 w20 h20 vActHB8 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:Add, Button, x128 y420 w20 h20 vActLB8 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial

	Gui, NPCE_Actions:Add, Edit, vactionnameB9 x16 y452 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB9 x152 y452 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y473 w20 h20 vActEB9 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y473 w20 h20 vActDB9 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y473 w20 h20 vActHB9 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:Add, Button, x128 y473 w20 h20 vActLB9 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial

	Gui, NPCE_Actions:Add, Edit, vactionnameB10 x16 y505 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB10 x152 y505 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y526 w20 h20 vActEB10 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y526 w20 h20 vActDB10 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y526 w20 h20 vActHB10 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:Add, Button, x128 y526 w20 h20 vActLB10 gActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Actions:font, S10 c000000, Arial
	
	Gui, NPCE_Actions:Add, Edit, vactionnameB11 x16 y558 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Actions:Add, Edit, vactionB11 x152 y558 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Actions:font, S14 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x60 y579 w20 h20 vActEB11 gEdit_Action -Tabstop, % Chr(9998)
	Gui, NPCE_Actions:Add, Button, x80 y579 w20 h20 vActDB11 gDelete_Action2 -Tabstop, % Chr(10062)
	Gui, NPCE_Actions:Add, Button, x108 y579 w20 h20 vActHB11 gActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Actions:font, S10 c000000, Arial
	;}

	Gui, NPCE_Actions:font, S10 c000000, Arial

	;{ Add Actions
	Gui, NPCE_Actions:Add, CheckBox, x564 y26 w89 h17 +0x20 Right vMulti_attack gAct_MultiAttack Checked%Multi_attack%, Multiattack:
	Gui, NPCE_Actions:Add, Edit, vmulti_attack_Text x658 y26 w426 h60 Multi Left, 
	Gui, NPCE_Actions:font, S9 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x1004 y91 w80 h20 +border vAction_Multiattack gAct_MultiAttack, Update
	Gui, NPCE_Actions:font, S10 c000000, Arial


	jsonpath:= DataDir "\weapons.json"
	WeapDB:= new JSONfile(jsonpath)
	weaplist:= ""
	For a, b in WeapDB.object()
	{
		weaplist:= weaplist a "|"
	}

	Gui, NPCE_Actions:Add, Text, x568 y146 w73 h17 Right, Weapon:
	Gui, NPCE_Actions:Add, Combobox, vWA_Name gAct_BWA2 x646 y143 w230 Left, %weaplist%
	Gui, NPCE_Actions:Add, Text, x568 y177 w73 h17 Right, Type:
	Gui, NPCE_Actions:Add, ComboBox, vWA_Type gAct_BWA x646 y174 w230 Left, Melee Weapon Attack||Ranged Weapon Attack|Melee or Ranged Weapon Attack|Melee Spell Attack|Ranged Spell Attack|Melee or Ranged Spell Attack
	Gui, NPCE_Actions:Add, CheckBox, x880 y138 w60 h17 +0x20 Right vWA_Magic gAct_BWA, Magic
	Gui, NPCE_Actions:Add, CheckBox, x880 y156 w60 h17 +0x20 Right vWA_Silver gAct_BWA, Silver
	Gui, NPCE_Actions:Add, CheckBox, x940 y138 w120 h17 +0x20 Right vWA_Adaman gAct_BWA, Adamantine
	Gui, NPCE_Actions:Add, CheckBox, x940 y156 w120 h17 +0x20 Right vWA_cfiron gAct_BWA, Cold-Forged Iron
	Gui, NPCE_Actions:Add, CheckBox, x940 y178 w120 h17 +0x20 Right vWA_Versatile gAct_BWA, Versatile
	
	Gui, NPCE_Actions:Add, Text, x590 y219 w50 h17 Center, To Hit
	Gui, NPCE_Actions:Add, Edit, x590 y239 w50 h23 gAct_BWA Center, 
	Gui, NPCE_Actions:Add, UpDown, vWA_ToHit Range-10-30, 0

	Gui, NPCE_Actions:Add, Text, x685 y219 w60 h17 Center, Reach (ft.)
	Gui, NPCE_Actions:Add, Edit, x690 y239 w50 h23 gAct_BWA Center, 
	Gui, NPCE_Actions:Add, UpDown, vWA_Reach Range0-30, 5

	Gui, NPCE_Actions:Add, Text, x790 y203 w110 h17 Center, Weapon Range (ft.)
	Gui, NPCE_Actions:Add, Text, x790 y219 w50 h17 Center, Normal
	Gui, NPCE_Actions:Add, Text, x850 y219 w50 h17 Center, Long
	
	Gui, NPCE_Actions:Add, Edit, x790 y239 w50 h23 gAct_BWA Center, 
	Gui, NPCE_Actions:Add, UpDown, vWA_Rnorm Range0-500, 30

	Gui, NPCE_Actions:Add, Edit, x850 y239 w50 h23 gAct_BWA Center, 
	Gui, NPCE_Actions:Add, UpDown, vWA_Rlong Range0-1000, 60

	Gui, NPCE_Actions:Add, Text, x950 y219 w110 h17 Center, Target Type
	Gui, NPCE_Actions:Add, ComboBox, vWA_Target gAct_BWA x950 y239 w110 Left, one target||one creature

	Gui, NPCE_Actions:Add, Text, x590 y292 w160 h17 Right, Damage:
	
	Gui, NPCE_Actions:Add, Text, x760 y269 w50 h17 Center, Number
	Gui, NPCE_Actions:Add, Edit, x760 y289 w50 h24 gAct_BWA Center, 
	Gui, NPCE_Actions:Add, UpDown, vWA_NoDice Range0-30, 1

	Gui, NPCE_Actions:Add, Text, x820 y269 w40 h17 Center, Die
	Gui, NPCE_Actions:Add, Text, x811 y292 w8 h17 Center, d
	Gui, NPCE_Actions:Add, ComboBox, vWA_Dice gAct_BWA x820 y289 w40 Center, 0|4|6||8|10|12|20

	Gui, NPCE_Actions:Add, Text, x861 y292 w8 h17 Center, +
	Gui, NPCE_Actions:Add, Text, x870 y269 w50 h17 Center, Bonus
	Gui, NPCE_Actions:Add, Edit, x870 y289 w50 h24 gAct_BWA Center, 
	Gui, NPCE_Actions:Add, UpDown, vWA_DamBon Range-30-30, 0

	Gui, NPCE_Actions:Add, Text, x950 y269 w110 h17 Center, Damage Type
	Gui, NPCE_Actions:Add, ComboBox, vWA_DamType gAct_BWA x950 y289 w110 h75 Left, bludgeoning|piercing|slashing|acid|cold|fire|force|lightning|necrotic|poison|psychic|radiant|thunder


	Gui, NPCE_Actions:Add, CheckBox, x564 y321 w89 h17 +0x20 Right vWA_BonAdd gAct_BWA, Add
	Gui, NPCE_Actions:Add, Text, x658 y321 w93 h17 Right, Bonus Damage:
	
	Gui, NPCE_Actions:Add, Edit, x760 y318 w50 h24 vWA_1 gAct_BWA Center, 
	Gui, NPCE_Actions:Add, UpDown, vWA_BonNoDice Range0-30, 1

	Gui, NPCE_Actions:Add, Text, x811 y321 w8 h17 Center, d
	Gui, NPCE_Actions:Add, ComboBox, vWA_BonDice gAct_BWA x820 y318 w40 Center, 0|4|6||8|10|12|20

	Gui, NPCE_Actions:Add, Text, x861 y321 w8 h17 Center, +
	Gui, NPCE_Actions:Add, Edit, x870 y318 w50 h24 vWA_2 gAct_BWA Center, 
	Gui, NPCE_Actions:Add, UpDown, vWA_BonDamBon Range-30-30, 0

	Gui, NPCE_Actions:Add, ComboBox, vWA_BonDamType gAct_BWA x950 y318 w110 h75 Left, acid|cold|fire|force|lightning|necrotic|poison|psychic|radiant|thunder

	Gui, NPCE_Actions:Add, CheckBox, x564 y346 w89 h17 +0x20 Right vWA_OtherTextAdd gAct_BWA, Other Text
	Gui, NPCE_Actions:Add, Edit, vWA_OtherText gAct_BWA x658 y346 w426 h60 Multi Left, 
	
	Gui, NPCE_Actions:Add, Edit, vweapon_attack_Text x568 y411 w430 h60 Multi ReadOnly -TabStop Left, 
	Gui, NPCE_Actions:font, S9 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x1004 y451 w80 h20 +border vAction_Weaponattack gAct_WeaponAttack, Update
	Gui, NPCE_Actions:font, S10 c000000, Arial

	jsonpath:= DataDir "\actions.json"
	If !ActDB {
		ActDB:= new JSONfile(jsonpath)
		Actlist:= ""
		For a, b in ActDB.object()
		{
			Actlist:= Actlist a "|"
		}
	}


	Gui, NPCE_Actions:Add, Text, x568 y504 w73 h17 Right, Action Name:
	Gui, NPCE_Actions:Add, ComboBox, vOtherActionName gOtherActionName x646 y501 w438 Left, %Actlist%
	Gui, NPCE_Actions:Add, Text, x568 y530 w73 h17 Right, Description:
	Gui, NPCE_Actions:Add, Edit, vOtherActionText x646 y527 w438 h60 Multi Left, 
	
	Gui, NPCE_Actions:font, S9 c000000, Arial
	Gui, NPCE_Actions:Add, Button, x1004 y591 w80 h20 +border vAction_Other gAct_Other, Update
	Gui, NPCE_Actions:font, S10 c000000, Arial

	Gui, NPCE_Actions:Add, Button, x962 y627 w130 h30 +border vNPCE_Actions_Close gNPCE_Actions_Close, Close Window
;}

	Gui, NPCE_Main:+disabled
	Gui, NPCE_Actions:Show, w1100 h665, Add or Edit NPC Actions
	Actionworks3()
	Hotkey,IfWinActive,ahk_id %NPCE_Actions%
	Hotkey, ^J, De_PDF
}

GUI_Reactions() {
	global
	
	; Settings for Reactions window (NPCE_Reactions)
	Gui, NPCE_Reactions:+OwnerNPCE_Main
	Gui, NPCE_Reactions:-SysMenu
	Gui, NPCE_Reactions:+hwndNPCE_Reactions
	Gui, NPCE_Reactions:Color, 0xE2E1E8
	Gui, NPCE_Reactions:font, S10 c000000, Arial

	; Groupboxes
	Gui, NPCE_Reactions:font, S10 c727178, Arial Bold
	Gui, NPCE_Reactions:Add, GroupBox, x8 y8 w532 h240, Reactions
	Gui, NPCE_Reactions:Add, GroupBox, x560 y8 w532 h187, Add or Edit Reaction

	Gui, NPCE_Reactions:font, S9 c000000, Arial
	
	;{ Reactions
	Gui, NPCE_Reactions:Add, Edit, vreactionnameB1 x16 y28 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Reactions:Add, Edit, vreactionB1 x152 y28 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Reactions:font, S14 c000000, Arial
	Gui, NPCE_Reactions:Add, Button, x59 y51 w20 h20 vReActEB1 gEdit_ReAction -Tabstop, % Chr(9998)
	Gui, NPCE_Reactions:Add, Button, x79 y51 w20 h20 vReActDB1 gDelete_Reaction2 -Tabstop, % Chr(10062)
	Gui, NPCE_Reactions:Add, Button, x127 y51 w20 h20 vReActLB1 gReActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Reactions:font, S10 c000000, Arial

	Gui, NPCE_Reactions:Add, Edit, vreactionnameB2 x16 y81 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Reactions:Add, Edit, vreactionB2 x152 y81 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Reactions:font, S14 c000000, Arial
	Gui, NPCE_Reactions:Add, Button, x59 y104 w20 h20 vReActEB2 gEdit_ReAction -Tabstop, % Chr(9998)
	Gui, NPCE_Reactions:Add, Button, x79 y104 w20 h20 vReActDB2 gDelete_Reaction2 -Tabstop, % Chr(10062)
	Gui, NPCE_Reactions:Add, Button, x107 y104 w20 h20 vReActHB2 gReActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Reactions:Add, Button, x127 y104 w20 h20 vReActLB2 gReActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Reactions:font, S10 c000000, Arial

	Gui, NPCE_Reactions:Add, Edit, vreactionnameB3 x16 y134 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Reactions:Add, Edit, vreactionB3 x152 y134 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Reactions:font, S14 c000000, Arial
	Gui, NPCE_Reactions:Add, Button, x59 y157 w20 h20 vReActEB3 gEdit_ReAction -Tabstop, % Chr(9998)
	Gui, NPCE_Reactions:Add, Button, x79 y157 w20 h20 vReActDB3 gDelete_Reaction2 -Tabstop, % Chr(10062)
	Gui, NPCE_Reactions:Add, Button, x107 y157 w20 h20 vReActHB3 gReActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Reactions:Add, Button, x127 y157 w20 h20 vReActLB3 gReActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Reactions:font, S10 c000000, Arial

	Gui, NPCE_Reactions:Add, Edit, vreactionnameB4 x16 y187 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_Reactions:Add, Edit, vreactionB4 x152 y187 w380 h51 -Tabstop +ReadOnly Multi Left, 
	Gui, NPCE_Reactions:font, S14 c000000, Arial
	Gui, NPCE_Reactions:Add, Button, x59 y210 w20 h20 vReActEB4 gEdit_ReAction -Tabstop, % Chr(9998)
	Gui, NPCE_Reactions:Add, Button, x79 y210 w20 h20 vReActDB4 gDelete_Reaction2 -Tabstop, % Chr(10062)
	Gui, NPCE_Reactions:Add, Button, x107 y210 w20 h20 vReActHB4 gReActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_Reactions:Add, Button, x127 y210 w20 h20 vReActLB4 gReActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_Reactions:font, S10 c000000, Arial

	;}

	Gui, NPCE_Reactions:font, S10 c000000, Arial

	;{ Add Reactions

	jsonpath:= DataDir "\actions.json"
	If !ActDB {
		ActDB:= new JSONfile(jsonpath)
		Actlist:= ""
		For a, b in ActDB.object()
		{
			Actlist:= Actlist a "|"
		}
	}

	Gui, NPCE_Reactions:Add, Text, x568 y31 w73 h17 Right, Reaction Name:
	Gui, NPCE_Reactions:Add, ComboBox, vOtherReActionName gOtherReActionName x646 y28 w438 Left, %Actlist%
	Gui, NPCE_Reactions:Add, Text, x568 y57 w73 h17 Right, Description:
	Gui, NPCE_Reactions:Add, Edit, vOtherReActionText x646 y54 w438 h104 Multi Left, 
	Gui, NPCE_Reactions:Add, Button, x1004 y164 w80 h23 +border vReAction_Other gReAct_Other, Update

	Gui, NPCE_Reactions:Add, Button, x962 y218 w130 h30 +border +default vNPCE_Reactions_Close gNPCE_Reactions_Close, Accept and Close
;}
	reActionworks()
	Gui, NPCE_Reactions:Show, w1100 h260, Add or Edit NPC Reactions

}

GUI_LegActions() {
	global
	
	; Settings for LegActions window (NPCE_LegActions)
	Gui, NPCE_LegActions:+OwnerNPCE_Main
	Gui, NPCE_LegActions:-SysMenu
	Gui, NPCE_LegActions:+hwndNPCE_LegActions
	Gui, NPCE_LegActions:Color, 0xE2E1E8
	Gui, NPCE_LegActions:font, S10 c000000, Arial

	; Groupboxes
	Gui, NPCE_LegActions:font, S10 c727178, Arial Bold
	Gui, NPCE_LegActions:Add, GroupBox, x8 y8 w532 h399, Legendary Actions
	Gui, NPCE_LegActions:Add, GroupBox, x560 y8 w532 h176, Options
	Gui, NPCE_LegActions:Add, GroupBox, x560 y194 w532 h160, New Legendary Action

	Gui, NPCE_LegActions:font, S9 c000000, Arial
	
	;{ Legendary Actions
	Gui, NPCE_LegActions:Add, Edit, vlgactionnameB1 x16 y28 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_LegActions:Add, Edit, vlgactionB1 x152 y28 w380 h51 -Tabstop +ReadOnly Multi Left, 

	Gui, NPCE_LegActions:Add, Edit, vlgactionnameB2 x16 y81 w131 h20 -Tabstop Left, 
	Gui, NPCE_LegActions:Add, Edit, vlgactionB2 x152 y81 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LegActions:font, S14 c000000, Arial
	Gui, NPCE_LegActions:Add, Button, x79 y104 w20 h20 vLgActDB2 gDelete_LgAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LegActions:Add, Button, x127 y104 w20 h20 vLgActLB2 gLgActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LegActions:font, S10 c000000, Arial

	Gui, NPCE_LegActions:Add, Edit, vlgactionnameB3 x16 y134 w131 h20 -Tabstop Left, 
	Gui, NPCE_LegActions:Add, Edit, vlgactionB3 x152 y134 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LegActions:font, S14 c000000, Arial
	Gui, NPCE_LegActions:Add, Button, x79 y157 w20 h20 vLgActDB3 gDelete_LgAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LegActions:Add, Button, x107 y157 w20 h20 vLgActHB3 gLgActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LegActions:Add, Button, x127 y157 w20 h20 vLgActLB3 gLgActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LegActions:font, S10 c000000, Arial

	Gui, NPCE_LegActions:Add, Edit, vlgactionnameB4 x16 y187 w131 h20 -Tabstop Left, 
	Gui, NPCE_LegActions:Add, Edit, vlgactionB4 x152 y187 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LegActions:font, S14 c000000, Arial
	Gui, NPCE_LegActions:Add, Button, x79 y210 w20 h20 vLgActDB4 gDelete_LgAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LegActions:Add, Button, x107 y210 w20 h20 vLgActHB4 gLgActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LegActions:Add, Button, x127 y210 w20 h20 vLgActLB4 gLgActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LegActions:font, S10 c000000, Arial

	Gui, NPCE_LegActions:Add, Edit, vlgactionnameB5 x16 y240 w131 h20 -Tabstop Left, 
	Gui, NPCE_LegActions:Add, Edit, vlgactionB5 x152 y240 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LegActions:font, S14 c000000, Arial
	Gui, NPCE_LegActions:Add, Button, x79 y263 w20 h20 vLgActDB5 gDelete_LgAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LegActions:Add, Button, x107 y263 w20 h20 vLgActHB5 gLgActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LegActions:Add, Button, x127 y263 w20 h20 vLgActLB5 gLgActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LegActions:font, S10 c000000, Arial
	
	Gui, NPCE_LegActions:Add, Edit, vlgactionnameB6 x16 y293 w131 h20 -Tabstop Left, 
	Gui, NPCE_LegActions:Add, Edit, vlgactionB6 x152 y293 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LegActions:font, S14 c000000, Arial
	Gui, NPCE_LegActions:Add, Button, x79 y316 w20 h20 vLgActDB6 gDelete_LgAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LegActions:Add, Button, x107 y316 w20 h20 vLgActHB6 gLgActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LegActions:Add, Button, x127 y316 w20 h20 vLgActLB6 gLgActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LegActions:font, S10 c000000, Arial

	Gui, NPCE_LegActions:Add, Edit, vlgactionnameB7 x16 y346 w131 h20 -Tabstop Left, 
	Gui, NPCE_LegActions:Add, Edit, vlgactionB7 x152 y346 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LegActions:font, S14 c000000, Arial
	Gui, NPCE_LegActions:Add, Button, x79 y369 w20 h20 vLgActDB7 gDelete_LgAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LegActions:Add, Button, x107 y369 w20 h20 vLgActHB7 gLgActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LegActions:Add, Button, x127 y369 w20 h20 vLgActLB7 gLgActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LegActions:font, S10 c000000, Arial

	;}

	Gui, NPCE_LegActions:font, S10 c000000, Arial

	;{ Add Legendary Actions
	Gui, NPCE_LegActions:Add, Text, x568 y31 w73 h17 Right, Options:
	Gui, NPCE_LegActions:Add, Edit, vLgActionOptions x646 y28 w438 h120 Multi Left, 
	Gui, NPCE_LegActions:Add, Button, x1004 y153 w80 h23 +border vLgAction_Options gLgAction_Options, Update

	jsonpath:= DataDir "\actions.json"
	If !ActDB {
		ActDB:= new JSONfile(jsonpath)
		Actlist:= ""
		For a, b in ActDB.object()
		{
			Actlist:= Actlist a "|"
		}
	}

	Gui, NPCE_LegActions:Add, Text, x568 y217 w73 h17 Right, Action Name:
	Gui, NPCE_LegActions:Add, ComboBox, vOtherLgActionName gOtherLgActionName x646 y214 w438 Left, %Actlist% 
	Gui, NPCE_LegActions:Add, Text, x568 y243 w73 h17 Right, Description:
	Gui, NPCE_LegActions:Add, Edit, vOtherLgActionText x646 y240 w438 h80 Multi Left, 
	Gui, NPCE_LegActions:Add, Button, x1004 y324 w80 h23 +border vLgAction_Other gLgAct_Other, Update

	Gui, NPCE_LegActions:Add, Button, x962 y377 w130 h30 +border +default vNPCE_LegActions_Close gNPCE_LegActions_Close, Accept and Close
;}

	lgactionworks3()
	Gui, NPCE_LegActions:Show, w1100 h417, Add or Edit NPC Legendary Actions
}

GUI_LairActions() {
	global
	
	; Settings for LairActions window (NPCE_LairActions)
	Gui, NPCE_LairActions:+OwnerNPCE_Main
	Gui, NPCE_LairActions:-SysMenu
	Gui, NPCE_LairActions:+hwndNPCE_LairActions
	Gui, NPCE_LairActions:Color, 0xE2E1E8
	Gui, NPCE_LairActions:font, S10 c000000, Arial

	; Groupboxes
	Gui, NPCE_LairActions:font, S10 c727178, Arial Bold
	Gui, NPCE_LairActions:Add, GroupBox, x8 y8 w532 h399, Lair Actions
	Gui, NPCE_LairActions:Add, GroupBox, x560 y8 w532 h176, Options
	Gui, NPCE_LairActions:Add, GroupBox, x560 y194 w532 h160, New Lair Action

	Gui, NPCE_LairActions:font, S9 c000000, Arial
	
	;{ Lair Actions
	Gui, NPCE_LairActions:Add, Edit, vlractionnameB1 x16 y28 w131 h20 -Tabstop +ReadOnly Left, 
	Gui, NPCE_LairActions:Add, Edit, vlractionB1 x152 y28 w380 h51 -Tabstop +ReadOnly Multi Left, 

	Gui, NPCE_LairActions:Add, Edit, vLrActionnameB2 x16 y81 w131 h20 -Tabstop Left, 
	Gui, NPCE_LairActions:Add, Edit, vLrActionB2 x152 y81 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LairActions:font, S14 c000000, Arial
	Gui, NPCE_LairActions:Add, Button, x79 y104 w20 h20 vLrActDB2 gDelete_LrAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LairActions:Add, Button, x127 y104 w20 h20 vLrActLB2 gLrActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LairActions:font, S10 c000000, Arial

	Gui, NPCE_LairActions:Add, Edit, vLrActionnameB3 x16 y134 w131 h20 -Tabstop Left, 
	Gui, NPCE_LairActions:Add, Edit, vLrActionB3 x152 y134 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LairActions:font, S14 c000000, Arial
	Gui, NPCE_LairActions:Add, Button, x79 y157 w20 h20 vLrActDB3 gDelete_LrAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LairActions:Add, Button, x107 y157 w20 h20 vLrActHB3 gLrActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LairActions:Add, Button, x127 y157 w20 h20 vLrActLB3 gLrActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LairActions:font, S10 c000000, Arial

	Gui, NPCE_LairActions:Add, Edit, vLrActionnameB4 x16 y187 w131 h20 -Tabstop Left, 
	Gui, NPCE_LairActions:Add, Edit, vLrActionB4 x152 y187 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LairActions:font, S14 c000000, Arial
	Gui, NPCE_LairActions:Add, Button, x79 y210 w20 h20 vLrActDB4 gDelete_LrAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LairActions:Add, Button, x107 y210 w20 h20 vLrActHB4 gLrActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LairActions:Add, Button, x127 y210 w20 h20 vLrActLB4 gLrActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LairActions:font, S10 c000000, Arial

	Gui, NPCE_LairActions:Add, Edit, vLrActionnameB5 x16 y240 w131 h20 -Tabstop Left, 
	Gui, NPCE_LairActions:Add, Edit, vLrActionB5 x152 y240 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LairActions:font, S14 c000000, Arial
	Gui, NPCE_LairActions:Add, Button, x79 y263 w20 h20 vLrActDB5 gDelete_LrAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LairActions:Add, Button, x107 y263 w20 h20 vLrActHB5 gLrActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LairActions:Add, Button, x127 y263 w20 h20 vLrActLB5 gLrActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LairActions:font, S10 c000000, Arial
	
	Gui, NPCE_LairActions:Add, Edit, vLrActionnameB6 x16 y293 w131 h20 -Tabstop Left, 
	Gui, NPCE_LairActions:Add, Edit, vLrActionB6 x152 y293 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LairActions:font, S14 c000000, Arial
	Gui, NPCE_LairActions:Add, Button, x79 y316 w20 h20 vLrActDB6 gDelete_LrAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LairActions:Add, Button, x107 y316 w20 h20 vLrActHB6 gLrActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LairActions:Add, Button, x127 y316 w20 h20 vLrActLB6 gLrActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LairActions:font, S10 c000000, Arial

	Gui, NPCE_LairActions:Add, Edit, vLrActionnameB7 x16 y346 w131 h20 -Tabstop Left, 
	Gui, NPCE_LairActions:Add, Edit, vLrActionB7 x152 y346 w380 h51 -Tabstop Multi Left, 
	Gui, NPCE_LairActions:font, S14 c000000, Arial
	Gui, NPCE_LairActions:Add, Button, x79 y369 w20 h20 vLrActDB7 gDelete_LrAction2 -Tabstop, % Chr(10062)
	Gui, NPCE_LairActions:Add, Button, x107 y369 w20 h20 vLrActHB7 gLrActionUp -Tabstop, % Chr(11205)
	Gui, NPCE_LairActions:Add, Button, x127 y369 w20 h20 vLrActLB7 gLrActionDown -Tabstop, % Chr(11206)
	Gui, NPCE_LairActions:font, S10 c000000, Arial
	;}

	Gui, NPCE_LairActions:font, S10 c000000, Arial

	;{ Add Lair Actions
	
	Gui, NPCE_LairActions:Add, Text, x568 y31 w73 h17 Right, Options:
	Gui, NPCE_LairActions:Add, Edit, vLrActionOptions x646 y28 w438 h120 Multi Left, 
	Gui, NPCE_LairActions:Add, Button, x1004 y153 w80 h23 +border vLrAction_Options gLrAction_Options, Update

	jsonpath:= DataDir "\actions.json"
	If !ActDB {
		ActDB:= new JSONfile(jsonpath)
		Actlist:= ""
		For a, b in ActDB.object()
		{
			Actlist:= Actlist a "|"
		}
	}

	Gui, NPCE_LairActions:Add, Text, x568 y217 w73 h17 Right, Action Name:
	Gui, NPCE_LairActions:Add, ComboBox, vOtherLrActionName gOtherLrActionName x646 y214 w438 Left, %Actlist%
	Gui, NPCE_LairActions:Add, Text, x568 y243 w73 h17 Right, Description:
	Gui, NPCE_LairActions:Add, Edit, vOtherLrActionText x646 y240 w438 h80 Multi Left, 
	Gui, NPCE_LairActions:Add, Button, x1004 y324 w80 h23 +border vLrAction_Other gLrAct_Other, Update

	Gui, NPCE_LairActions:Add, Button, x962 y377 w130 h30 +border +default vNPCE_LairActions_Close gNPCE_LairActions_Close, Accept and Close
;}

	lrActionworks3()

	Gui, NPCE_LairActions:Show, w1100 h417, Add or Edit NPC Lair Actions
}

GUI_HP() {
	global
	
; Settings for hit point window (NPCE_HP)
	Gui, NPCE_HP:+OwnerNPCE_Main
	Gui, NPCE_HP:-SysMenu
	Gui, NPCE_HP:+hwndNPCE_HP
	Gui, NPCE_HP:Color, 0xE2E1E8
	Gui, NPCE_HP:font, S10 c000000, Arial
	
	
	;~ Gui, NPCE_HP:Add, Text, x590 y333 w170 h17 Left, Damage Dice and Bonus:
	
	Gui, NPCE_HP:Add, Text, x32 y10 w50 h17 Center, Number
	Gui, NPCE_HP:Add, Edit, x32 y30 w50 h24 gMakeHP Center, 
	Gui, NPCE_HP:Add, UpDown, vHPno Range1-30, 1

	Gui, NPCE_HP:Add, Text, x92 y10 w40 h17 Center, Die
	Gui, NPCE_HP:Add, Text, x83 y33 w8 h17 Center, d
	Gui, NPCE_HP:Add, ComboBox, vHPdi gMakeHP x92 y30 w40 Center, 4|6||8|10|12|20

	Gui, NPCE_HP:Add, Text, x133 y33 w8 h17 Center, +
	Gui, NPCE_HP:Add, Text, x142 y10 w50 h17 Center, Bonus
	Gui, NPCE_HP:Add, Edit, x142 y30 w50 h24 gMakeHP Center, 
	Gui, NPCE_HP:Add, UpDown, vHPbo Range-200-800, 0

	Gui, NPCE_HP:Add, Edit, vHPstring x10 y68 w123 h23 ReadOnly -TabStop Left,

	Gui, NPCE_HP:font, S8, Arial

	Gui, NPCE_HP:Add, Button, x137 y68 w37 h23 +border -tabstop vButtonAverageHP gHP_Average, Mean
	Gui, NPCE_HP:Add, Button, x178 y68 w37 h23 +border -tabstop vButtonRollHP gHP_Roll, Roll

	Gui, NPCE_HP:font, S10 c000000, Arial
	
	Gui, NPCE_HP:Add, Button, x115 y101 w100 h23 +border vNPCE_HP_Return gNPCE_HP_Return, Accept
	Gui, NPCE_HP:Add, Button, x10 y101 w100 h23 +border vNPCE_HP_Cancel gNPCE_HP_Cancel, Cancel
}

GUI_JSON() {
	global
	
; Settings for text import window (NPCE_JSON)
	Gui, NPCE_JSON:+OwnerNPCE_Main
	Gui, NPCE_JSON:-SysMenu
	Gui, NPCE_JSON:+hwndNPCE_JSON
	Gui, NPCE_JSON:Color, 0xE2E1E8
	Gui, NPCE_JSON:font, S10 c000000, Arial
	Gui, NPCE_JSON:margin, 5, 1

	npc_list:= "Choose an NPC from the JSON file||"
	For a, b in npc.object()
	{
		npc_list:= npc_list NPC[a].name "|"
	}
	Gui, NPCE_JSON:Add, ComboBox, x10 y10 w300 VJSONChoose gJSONchoose, %npc_list%
	Gui, NPCE_JSON:Add, Edit, x10 y40 w300 h60 VJSONselected +ReadOnly, 
	Gui, NPCE_JSON:Add, Button, x75 y105 w80 h25 +border vNPCE_JSON_Del gNPCE_JSON_Del, Delete NPC
	Gui, NPCE_JSON:Add, Button, x165 y105 w80 h25 +border vNPCE_JSON_Edit gNPCE_JSON_Edit, Edit NPC
	Gui, NPCE_JSON:Add, Button, x180 y170 w130 h30 +border vNPCE_JSON_Cancel gNPCE_JSON_Cancel, Close
}

GUI_Weapons() {
	global
	
	; Settings for Actions window (NPCE_Actions)
	Gui, NPCE_Weapons:+OwnerNPCE_Main
	Gui, NPCE_Weapons:-SysMenu
	Gui, NPCE_Weapons:+hwndNPCE_Weapons
	Gui, NPCE_Weapons:Color, 0xE2E1E8
	Gui, NPCE_Weapons:font, S10 c000000, Arial

	jsonpath:= DataDir "\weapons.json"
	If !WeapDB {
		WeapDB:= new JSONfile(jsonpath)
		weaplist:= ""
		For a, b in WeapDB.object()
		{
			weaplist:= weaplist a "|"
		}
	}

	Gui, NPCE_Weapons:Add, Text, x8 y17 w73 h17 Right, Weapon:
	Gui, NPCE_Weapons:Add, Combobox, vWA_Name gNew_Weapon x86 y14 w230 Left, %weaplist%
	Gui, NPCE_Weapons:Add, Text, x8 y50 w73 h17 Right, Type:
	Gui, NPCE_Weapons:Add, ComboBox, vWA_Type x86 y47 w230 Left, Melee Weapon Attack||Ranged Weapon Attack|Melee or Ranged Weapon Attack|Melee Spell Attack|Ranged Spell Attack|Melee or Ranged Spell Attack
	Gui, NPCE_Weapons:Add, CheckBox, x320 y8 w60 h17 +0x20 Right vWA_Magic, Magic
	Gui, NPCE_Weapons:Add, CheckBox, x320 y26 w60 h17 +0x20 Right vWA_Silver, Silver
	Gui, NPCE_Weapons:Add, CheckBox, x380 y8 w120 h17 +0x20 Right vWA_Adaman, Adamantine
	Gui, NPCE_Weapons:Add, CheckBox, x380 y26 w120 h17 +0x20 Right vWA_cfiron, Cold-Forged Iron
	Gui, NPCE_Weapons:Add, CheckBox, x380 y48 w120 h17 +0x20 Right vWA_Versatile, Versatile

	Gui, NPCE_Weapons:Add, Text, x30 y96 w50 h17 Center, To Hit
	Gui, NPCE_Weapons:Add, Edit, x30 y116 w50 h23 Center, 
	Gui, NPCE_Weapons:Add, UpDown, vWA_ToHit Range-10-30, 0

	Gui, NPCE_Weapons:Add, Text, x125 y96 w60 h17 Center, Reach (ft.)
	Gui, NPCE_Weapons:Add, Edit, x130 y116 w50 h23 Center, 
	Gui, NPCE_Weapons:Add, UpDown, vWA_Reach Range0-10, 5

	Gui, NPCE_Weapons:Add, Text, x230 y80 w110 h17 Center, Weapon Range (ft.)
	Gui, NPCE_Weapons:Add, Text, x230 y96 w50 h17 Center, Normal
	Gui, NPCE_Weapons:Add, Text, x290 y96 w50 h17 Center, Long
	
	Gui, NPCE_Weapons:Add, Edit, x230 y116 w50 h23 Center, 
	Gui, NPCE_Weapons:Add, UpDown, vWA_Rnorm Range0-500, 30

	Gui, NPCE_Weapons:Add, Edit, x290 y116 w50 h23 Center, 
	Gui, NPCE_Weapons:Add, UpDown, vWA_Rlong Range0-1000, 60

	Gui, NPCE_Weapons:Add, Text, x390 y96 w110 h17 Center, Target Type
	Gui, NPCE_Weapons:Add, ComboBox, vWA_Target x390 y116 w110 Left, one target||one creature

	Gui, NPCE_Weapons:Add, Text, x30 y173 w165 h17 Right, Damage:
	
	Gui, NPCE_Weapons:Add, Text, x200 y150 w50 h17 Center, Number
	Gui, NPCE_Weapons:Add, Edit, x200 y170 w50 h24 Center, 
	Gui, NPCE_Weapons:Add, UpDown, vWA_NoDice Range0-30, 1

	Gui, NPCE_Weapons:Add, Text, x260 y150 w40 h17 Center, Die
	Gui, NPCE_Weapons:Add, Text, x251 y173 w8 h17 Center, d
	Gui, NPCE_Weapons:Add, ComboBox, vWA_Dice x260 y170 w40 Center, 0|4|6||8|10|12|20

	Gui, NPCE_Weapons:Add, Text, x301 y173 w8 h17 Center, +
	Gui, NPCE_Weapons:Add, Text, x310 y150 w50 h17 Center, Bonus
	Gui, NPCE_Weapons:Add, Edit, x310 y170 w50 h24 Center, 
	Gui, NPCE_Weapons:Add, UpDown, vWA_DamBon Range-30-30, 0

	Gui, NPCE_Weapons:Add, Text, x390 y150 w110 h17 Center, Damage Type
	Gui, NPCE_Weapons:Add, ComboBox, vWA_DamType x390 y170 w110 Left, bludgeoning|piercing|slashing||acid|cold|fire|force|lightning|necrotic|poison|psychic|radiant|thunder


	Gui, NPCE_Weapons:Add, CheckBox, x4 y203 w89 h17 +0x20 Right vWA_BonAdd gBonusDamage, Add %A_Space%
	Gui, NPCE_Weapons:Add, Text, x98 y203 w97 h17 Right, Bonus Damage:
	
	Gui, NPCE_Weapons:Add, Edit, Disabled x200 y200 w50 h24 vWA_1 Center, 
	Gui, NPCE_Weapons:Add, UpDown, Disabled vWA_BonNoDice Range0-30, 1

	Gui, NPCE_Weapons:Add, Text, x251 y203 w8 h17 Center, d
	Gui, NPCE_Weapons:Add, ComboBox, Disabled vWA_BonDice x260 y200 w40 Center, 0|4|6||8|10|12|20

	Gui, NPCE_Weapons:Add, Text, x301 y203 w8 h17 Center, +
	Gui, NPCE_Weapons:Add, Edit, Disabled x310 y200 w50 h24 vWA_2 Center, 
	Gui, NPCE_Weapons:Add, UpDown, Disabled vWA_BonDamBon Range-30-30, 0

	Gui, NPCE_Weapons:Add, ComboBox, Disabled vWA_BonDamType x390 y200 w110 h75 Left, acid|cold|fire|force|lightning|necrotic|poison|psychic|radiant|thunder



	Gui, NPCE_Weapons:Add, CheckBox, x4 y236 w89 h17 +0x20 Right vWA_OtherTextAdd gBonusDamage, Other Text
	Gui, NPCE_Weapons:Add, Edit, vWA_OtherText hwndWAText Disabled x98 y236 w401 h60 Multi Left, 

	Gui, NPCE_Weapons:Add, Button, x330 y310 w80 h23 +border vDelete_Weapon gDelete_Weapon, Delete
	Gui, NPCE_Weapons:Add, Button, x420 y310 w80 h23 +border +default vAdd_Weapon gAdd_Weapon, Update

	Gui, NPCE_Weapons:Add, Button, x370 y340 w130 h30 +border vNPCE_Weapons_Close gNPCE_Weapons_Close, Close

	Gui, NPCE_Main:+disabled
	Gui, NPCE_Weapons:Show, w508 h380, Add or Edit Weapons
	Hotkey,IfWinActive,ahk_id %NPCE_Weapons%
	hotkey, RButton, RButton
	Hotkey, ^J, De_PDF
}

GUI_ActAdd() {
	global
	
; Settings for text import window (NPCE_ActAdd)
	Gui, NPCE_ActAdd:+OwnerNPCE_Main
	Gui, NPCE_ActAdd:-SysMenu
	Gui, NPCE_ActAdd:+hwndNPCE_ActAdd
	Gui, NPCE_ActAdd:Color, 0xE2E1E8
	Gui, NPCE_ActAdd:font, S10 c000000, Arial
	Gui, NPCE_ActAdd:margin, 5, 1

	jsonpath:= DataDir "\actions.json"
	If !ActDB {
		ActDB:= new JSONfile(jsonpath)
		Actlist:= ""
		For a, b in ActDB.object()
		{
			Actlist:= Actlist a "|"
		}
	}

	Gui, NPCE_ActAdd:Add, Text, x10 y10 w73 h17 Right, Action Name:
	Gui, NPCE_ActAdd:Add, ComboBox, vOtherActionName gActAdd x88 y7 w438 Left, %Actlist%
	Gui, NPCE_ActAdd:Add, Text, x10 y36 w73 h17 Right, Description:
	Gui, NPCE_ActAdd:Add, Edit, vOtherActionText hwndOAText x88 y33 w438 h60 Multi Left, 
	
	Gui, NPCE_ActAdd:Add, Text, x88 y110 w438 h17 Center, Right-click for a menu with generic pronoun options for your NPC.

	
	Gui, NPCE_ActAdd:Add, Button, x356 y140 w80 h23 +border vDelete_Actoth gDelete_Actoth, Delete
	Gui, NPCE_ActAdd:Add, Button, x446 y140 w80 h23 +border +default vAdd_Actoth gAdd_Actoth, Update

	Gui, NPCE_ActAdd:Add, Button, x396 y190 w130 h30 +border vNPCE_ActAdd_Close gNPCE_ActAdd_Close, Close

	Gui, NPCE_Main:+disabled
	Gui, NPCE_ActAdd:Show, w536 h230, Add or Edit Actions
	Hotkey,IfWinActive,ahk_id %NPCE_ActAdd%
	hotkey, RButton, RButton
	Hotkey, ^J, De_PDF
}

GUI_TraitAdd() {
	global
	
; Settings for text import window (NPCE_TraitAdd)
	Gui, NPCE_TraitAdd:+OwnerNPCE_Main
	Gui, NPCE_TraitAdd:-SysMenu
	Gui, NPCE_TraitAdd:+hwndNPCE_TraitAdd
	Gui, NPCE_TraitAdd:Color, 0xE2E1E8
	Gui, NPCE_TraitAdd:font, S10 c000000, Arial
	Gui, NPCE_TraitAdd:margin, 5, 1

	jsonpath:= DataDir "\traits.json"
	If !traitDB {
		TraitDB:= new JSONfile(jsonpath)
		Traitlist:= ""
		For a, b in TraitDB.object()
		{
			Traitlist:= Traitlist a "|"
		}
	}
	Gui, NPCE_TraitAdd:Add, Text, x10 y10 w73 h17 Right, Trait Name:
	Gui, NPCE_TraitAdd:Add, ComboBox, vTraitNameNew gTraitAdd x88 y7 w438 Left, %Traitlist%
	Gui, NPCE_TraitAdd:Add, Text, x10 y36 w73 h17 Right, Description:
	Gui, NPCE_TraitAdd:Add, Edit, vTraitNew hwndTAText x88 y33 w438 h60 Multi Left, 
	
	Gui, NPCE_TraitAdd:Add, Text, x88 y110 w438 h17 Center, Right-click for a menu with generic pronoun options for your NPC.

	
	Gui, NPCE_TraitAdd:Add, Button, x356 y140 w80 h23 +border vDelete_TraitAdd gDelete_TraitAdd, Delete
	Gui, NPCE_TraitAdd:Add, Button, x446 y140 w80 h23 +border +default vAdd_TraitAdd gAdd_TraitAdd, Update

	Gui, NPCE_TraitAdd:Add, Button, x396 y190 w130 h30 +border vNPCE_TraitAdd_Close gNPCE_TraitAdd_Close, Close

	Gui, NPCE_Main:+disabled
	Gui, NPCE_TraitAdd:Show, w536 h230, Add or Edit Traits
	Hotkey,IfWinActive,ahk_id %NPCE_TraitAdd%
	hotkey, RButton, RButton
	Hotkey, ^J, De_PDF
}

GUI_LangAdd() {
	global
	
; Settings for Add Language window (NPCE_LangAdd)
	Gui, NPCE_LangAdd:+OwnerNPCE_Main
	Gui, NPCE_LangAdd:-SysMenu
	Gui, NPCE_LangAdd:+hwndNPCE_LangAdd
	Gui, NPCE_LangAdd:Color, 0xE2E1E8
	Gui, NPCE_LangAdd:font, S10 c000000, Arial
	Gui, NPCE_LangAdd:margin, 5, 1

	Local key, value, Arch4
	For key, val in LangUser
		Arch4 .= val "|"
	
		
	Gui, NPCE_LangAdd:font, S9 c000000, Arial
	Gui, NPCE_LangAdd:Add, ListBox, x10 y30 R13 w120 sort vLangDelList, %arch4%
	Gui, NPCE_LangAdd:font, S10 c000000, Arial
	
	Gui, NPCE_LangAdd:Add, Text, x10 y10 w120 h17, Delete Language:
	Gui, NPCE_LangAdd:Add, Text, x150 y10 w173 h17, Add New Language:
	Gui, NPCE_LangAdd:Add, Edit, vNewLang x150 y30 w200 h23 Left, 
	
	Gui, NPCE_LangAdd:Add, Button, x30 y235 w80 h23 +border vLangDelete gLangDelete, Delete
	Gui, NPCE_LangAdd:Add, Button, x270 y58 w80 h23 +border +default vLangAdd gLangAdd, Add

	Gui, NPCE_LangAdd:Add, Button, x220 y228 w130 h30 +border vNPCE_LangAdd_Close gNPCE_LangAdd_Close, Close

	Gui, NPCE_Main:+disabled
	Gui, NPCE_LangAdd:Show, w360 h268, Add or Delete Languages
}

GUI_Terrain() {
	global
	
; Settings for Terrain/Lore window (NPCE_Terrain)
	Gui, NPCE_Terrain:+OwnerNPCE_Main
	Gui, NPCE_Terrain:-SysMenu
	Gui, NPCE_Terrain:+hwndNPCE_Terrain
	Gui, NPCE_Terrain:Color, 0xE2E1E8
	Gui, NPCE_Terrain:font, S10 c000000, Arial
	Gui, NPCE_Terrain:margin, 5, 1


	Gui, NPCE_Terrain:font, S10 c727178, Arial Bold
	Gui, NPCE_Terrain:Add, GroupBox, x10 y14 w118 h260, Terrain Type
	Gui, NPCE_Terrain:Add, GroupBox, x140 y14 w118 h260, Mythology
	;~ Gui, NPCE_Terrain:Add, GroupBox, x316 y290 w290 h205, Senses
	
	loop % terraintype.length() {
		If Instr(Sort.terrain, terraintype[A_Index]) {
			Terr%A_Index%:= 1
		} else {
			Terr%A_Index%:= 0
		}
	}	

	loop % originlore.length() {
		If Instr(Sort.lore, originlore[A_Index]) {
			Orig%A_Index%:= 1
		} else {
			Orig%A_Index%:= 0
		}
	}	

	Gui, NPCE_Terrain:font, S10 c000000, Arial
	Gui, NPCE_Terrain:Add, CheckBox, vTerr1 Checked%Terr1% x19 y32 w98 h20 right, % TerrainType[1]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr2 Checked%Terr2% x19 y52 w98 h20 right, % TerrainType[2]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr3 Checked%Terr3% x19 y72 w98 h20 right, % TerrainType[3]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr4 Checked%Terr4% x19 y92 w98 h20 right, % TerrainType[4]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr5 Checked%Terr5% x19 y112 w98 h20 right, % TerrainType[5]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr6 Checked%Terr6% x19 y132 w98 h20 right, % TerrainType[6]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr7 Checked%Terr7% x19 y152 w98 h20 right, % TerrainType[7]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr8 Checked%Terr8% x19 y172 w98 h20 right, % TerrainType[8]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr9 Checked%Terr9% x19 y192 w98 h20 right, % TerrainType[9]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr10 Checked%Terr10% x19 y212 w98 h20 right, % TerrainType[10]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr11 Checked%Terr11% x19 y232 w98 h20 right, % TerrainType[11]
	Gui, NPCE_Terrain:Add, CheckBox, vTerr12 Checked%Terr12% x19 y252 w98 h20 right, % TerrainType[12]
	
	Gui, NPCE_Terrain:Add, CheckBox, vOrig1 Checked%Orig1% x149 y32 w98 h20 right, % OriginLore[1]
	Gui, NPCE_Terrain:Add, CheckBox, vOrig2 Checked%Orig2% x149 y52 w98 h20 right, % OriginLore[2]
	Gui, NPCE_Terrain:Add, CheckBox, vOrig3 Checked%Orig3% x149 y72 w98 h20 right, % OriginLore[3]
	Gui, NPCE_Terrain:Add, CheckBox, vOrig4 Checked%Orig4% x149 y92 w98 h20 right, % OriginLore[4]
	Gui, NPCE_Terrain:Add, CheckBox, vOrig5 Checked%Orig5% x149 y112 w98 h20 right, % OriginLore[5]
	Gui, NPCE_Terrain:Add, CheckBox, vOrig6 Checked%Orig6% x149 y132 w98 h20 right, % OriginLore[6]
	Gui, NPCE_Terrain:Add, CheckBox, vOrig7 Checked%Orig7% x149 y152 w98 h20 right, % OriginLore[7]
	Gui, NPCE_Terrain:Add, CheckBox, vOrig8 Checked%Orig8% x149 y172 w98 h20 right, % OriginLore[8]
	Gui, NPCE_Terrain:Add, CheckBox, vOrig9 Checked%Orig9% x149 y192 w98 h20 right, % OriginLore[9]

	Gui, NPCE_Terrain:Add, Button, x396 y268 w130 h30 +border vNPCE_Terrain_Close gNPCE_Terrain_Close, Close

	Gui, NPCE_Main:+disabled
	Gui, NPCE_Terrain:Show, w536 h306, Change NPC Terrain Types and Origin Lore
}

NPCE_MainGuiDropFiles(GuiHwnd, FileArray, CtrlHwnd, X, Y) {
	Global
	Local extension
	splitpath, % filearray[1],,,extension,,
	if (hwndNPCtoken = CtrlHwnd) AND (extension = "png") {
		NPCTokenPath:= filearray[1]
		Gosub Load_NPCToken
	}
	if (hwndNPCimage = CtrlHwnd) AND (extension = "jpg") {
		NPCImagePath:= filearray[1]
		Gosub Load_NPCImage
	}
}


CreateToolbar() {
	ImageList:= IL_Create(6,,0)
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
		New NPC
		Open NPC
		Save NPC
		-
		Open Previous NPC
		Open Next NPC
		-
		Manage NPCs
		-
		Import NPC Text
		-
		Manage Project
		-
		Create Module
	)

	Return ToolbarCreate("OnToolbar", Buttons, "NPCE_Main", ImageList, "Flat List Tooltips Border")
}

LBUTTONDBLCLK(wParam, lParam, msg, hwnd) {
    Global DCEdits, MainTabName, AppliedDPI
    CoordMode, Mouse, Client
    MouseGetPos, checkx, checky
	scaler:= AppliedDPI/96
	checkx /= scaler
	checky /= scaler
	GUI, NPCE_Main:submit, NoHide
    for i, v in DCEdits {
        if(WithIn(v, checkx, checky) AND MainTabName == "Base Stats") {
			hp_build()
            Break
        }
    }
}

WithIn(v, x, y) {
    Global
    GuiControlGet, p, Pos, %v%
    return px <= x && x <= px + pw && py <= y && y <= py + ph
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
#Include Spell Engineer.ahk
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