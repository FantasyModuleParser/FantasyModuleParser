/*
Component:	NPCE Include BB
  Version:	1.0.0
     Date:	14/02/18
 Revision:	All BB code
 */

local bbtop, bbmiddle, bbsave, bbskill, bbdamvul, bbdamres, bbdamimm, bbconimm, bbtraits, bbactions, bbreactions, bblegactions, bblairactions, bbdesc

bbtop = 
(
[b][COLOR=#800000][SIZE=3]%NPCName%[/SIZE][/COLOR][/b]
[i]%NPC_size_etc%[/i]
[HR][/HR][b]Armor Class[/b] %NPCac%
[b]Hit Points[/b] %NPChp%
[b]Speed[/b] %NPCspeed%
[HR][/HR][TABLE="width: 500"]
[tr]
	[td][b]STR[/b][/td]
	[td][b]DEX[/b][/td]
	[td][b]CON[/b][/td]
	[td][b]INT[/b][/td]
	[td][b]WIS[/b][/td]
	[td][b]CHA[/b][/td]
[/tr]
[tr]
	[td]%NPCstr% (%NPCstrbo%)[/td]
	[td]%NPCdex% (%NPCdexbo%)[/td]
	[td]%NPCcon% (%NPCconbo%)[/td]
	[td]%NPCint% (%NPCintbo%)[/td]
	[td]%NPCwis% (%NPCwisbo%)[/td]
	[td]%NPCcha% (%NPCchabo%)[/td]
[/tr]
[/TABLE]
[HR][/HR]
)

bbsave = 
(
[b]Saving Throws[/b] %NPCSave%

)

bbskill = 
(
[b]Skills[/b] %NPCskill%

)

bbdamvul = 
(
[b]Damage Vulnerabilities[/b] %NPCdamvul%

)

bbdamres = 
(
[b]Damage Resistances[/b] %NPCdamres%

)

bbdamimm = 
(
[b]Damage Immunities[/b] %NPCdamimm%

)

bbconimm = 
(
[b]Condition Immunities[/b] %NPCconimm%

)

bbmiddle = 
(
[b]Senses[/b] %NPCSense%
[b]Languages[/b] %NPCLang%[b]Challenge[/b] %NPCcharat% (%NPCxp% XP)
[HR][/HR]
)

bbtraits = 
(
%NPCTraitsH%
%NPCinspellH%
%NPCspellH%
)

bbactions = 
(
[COLOR=#800000]ACTIONS[/COLOR]
[HR][/HR]%NPCActionsH%
)

bbreactions = 
(
[COLOR=#800000]REACTIONS[/COLOR]
[HR][/HR]%NPCreactionsH%
)

bblegactions = 
(
[COLOR=#800000]LEGENDARY ACTIONS[/COLOR]
[HR][/HR]%NPClegactionsH%
)

bblairactions = 
(
[COLOR=#800000]LAIR ACTIONS[/COLOR]
[HR][/HR]%NPClairactionsH%
)

;~ bbdesc =
;~ (
;~ [COLOR=#800000]DECSRIPTION[/COLOR]
;~ [HR][/HR]
;~ %NPCdescript%

;~ )
