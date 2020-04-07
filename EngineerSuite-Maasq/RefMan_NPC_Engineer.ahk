/*
Component:	NPCE Include XML for internal use
  Version:	0.1.1
     Date:	27/01/18
 Revision:	
 */

local RM_NPCTop, RM_NPCTop2, RMsave, RMskill, RMdamvul, RMdamres, RMdamimm, RMconimm, RMmiddle, RMtraits, RMactions, RMReactions, RMLegactions, RMLairactions, RMNPCName

stringreplace, RMNPCName, NPCName, &, `&amp`;, All

RM_NPCTop =
(
									<name type="string">%RMNPCName%</name>
									<blocks>
)
RM_NPCTop:= "`t`t`t`t`t`t`t`t`t" RM_NPCTop

RM_NPCPic =
(
										<id000010>
											<align type="string">center</align>
											<size type="string">picdimpicdim</size>
											<image type="image"><bitmap>images/%NPCjpeg%.jpg</bitmap></image>
											<imagelink type="windowreference">
												<class>imagewindow</class>
												<recordname>image.img_%NPCjpeg%_jpg@%ModName%</recordname>
											</imagelink>
											<blocktype type="string">image</blocktype>
										</id000010>
)
RM_NPCPic:= "`t`t`t`t`t`t`t`t`t" RM_NPCPic

RM_NPCTop2 =
(
										<id000030>
											<blocktype type="string">text</blocktype>
											<align type="string">left</align>
											<text type="formattedtext"><h>&#13;%RMNPCName%</h>
											<p><i>%NPC_size_2%</i>
											<b>&#13;Armor Class</b> %NPCac%
											<b>&#13;Hit Points</b> %NPChp%
											<b>&#13;Speed</b> %NPCspeed%</p>
											<table>
											<tr>
												<td><b>STR</b></td>
												<td><b>DEX</b></td>
												<td><b>CON</b></td>
												<td><b>INT</b></td>
												<td><b>WIS</b></td>
												<td><b>CHA</b></td>
											</tr>
											<tr>
												<td>%NPCstr% (%NPCstrbo%)</td>
												<td>%NPCdex% (%NPCdexbo%)</td>
												<td>%NPCcon% (%NPCconbo%)</td>
												<td>%NPCint% (%NPCintbo%)</td>
												<td>%NPCwis% (%NPCwisbo%)</td>
												<td>%NPCcha% (%NPCchabo%)</td>
											</tr>
											</table>
											<p>											
)
RM_NPCTop2:= "`t`t`t`t`t`t`t`t`t`t`t" RM_NPCTop2

RMsave = 
(
<b>&#13;Saving Throws</b> %NPCSave%
)
RMsave:= "`t`t`t`t`t`t`t`t`t`t`t" RMsave

RMskill = 
(
<b>&#13;Skills</b> %NPCskill%
)
RMskill:= "`t`t`t`t`t`t`t`t`t`t`t" RMskill

RMdamvul = 
(
<b>&#13;Damage Vulnerabilities</b> %NPCdamvul%
)
RMdamvul:= "`t`t`t`t`t`t`t`t`t`t`t" RMdamvul

RMdamres = 
(
<b>&#13;Damage Resistances</b> %NPCdamres%
)
RMdamres:= "`t`t`t`t`t`t`t`t`t`t`t" RMdamres

RMdamimm = 
(
<b>&#13;Damage Immunities</b> %NPCdamimm%
)
RMdamimm:= "`t`t`t`t`t`t`t`t`t`t`t" RMdamimm

RMconimm = 
(
<b>&#13;Condition Immunities</b> %NPCconimm%
)
RMconimm:= "`t`t`t`t`t`t`t`t`t`t`t" RMconimm

RMmiddle = 
(
											<b>&#13;Senses</b> %NPCSense%
											<b>&#13;Languages</b> %NPCLang%
											<b>&#13;Challenge</b> %NPCcharat% (%NPCxp% XP)</p>

)
RMmiddle:= "`t`t`t`t`t`t`t`t`t`t`t" RMmiddle

RMtraits = 
(
											<p>%NPCTraitsX%
											%NPCinspellX%
											%NPCspellX%</p>
											
)
RMtraits:= "`t`t`t`t`t`t`t`t`t`t`t" RMtraits

RMactions = 
(
											<p><u>ACTIONS</u>
											%NPCActionsX%</p>
											
)
RMActions:= "`t`t`t`t`t`t`t`t`t`t`t" RMactions

RMReactions = 
(
											<p><u>REACTIONS</u>
											%NPCReactionsX%</p>
											
)
RMReactions:= "`t`t`t`t`t`t`t`t`t`t`t" RMReactions

RMLegactions = 
(
											<p><u>LEGENDARY ACTIONS</u>
											%NPCLegActionsX%</p>
											
)
RMLegactions:= "`t`t`t`t`t`t`t`t`t`t`t" RMLegactions

RMLairactions = 
(
											<p><u>LAIR ACTIONS</u>
											%NPCLairActionsX%</p>
											
)
RMLairactions:= "`t`t`t`t`t`t`t`t`t`t`t" RMLairactions


RM_NPCend = 
(
											</text>
										</id000030>
										<id000040>
											<blocktype type="string">text</blocktype>
											<align type="string">left</align>
											<text type="formattedtext"><h>Description</h>
											RefManDesc
										<id100050>
											<blocktype type="string">text</blocktype>
											<align type="string">center</align>
											<text type="formattedtext">
				
)

RM_NPCend2 = 
(
												<link type="windowreference" class="imagewindow" recordname="image.img_%NPCjpeg%_jpg@%ModName%"><b>Full size image:</b> %RMNPCName%.</link>
				
)

RM_NPCend3 = 
(
												<link type="windowreference" class="npc" recordname="reference.npcdata.%NPCjpeg%@%ModName%"><b>Fantasy Grounds stat block:</b> %RMNPCName%.</link>&#13;&#13;
											</text>
										</id100050>
				
)

RM_NPCartist = 
(
										<id100055>
											<blocktype type="string">image</blocktype>
											<align type="string">center</align>
											<size type="string">400,1</size>
											<image type="image"><bitmap>images/refmandiv.jpg</bitmap></image>
										</id100055>
										<id100060>
											<align type="string">left,right</align>
											<frame type="string">%NPCartistframe%</frame>
											<blocktype type="string">sidebarright</blocktype>
											<text type="formattedtext">
											<p>%RMInfo%</p>
											</text>
											<text2 type="formattedtext">
											<p>%NPCartistetc%</p>
											</text2>
										</id100060>
										<id100070>
											<blocktype type="string">image</blocktype>
											<align type="string">center</align>
											<size type="string">400,1</size>
											<image type="image"><bitmap>images/refmandiv.jpg</bitmap></image>
											<caption type="string">Built by NPC Engineer (www.masq.net)&#13;</caption>
										</id100070>
				
)

RM_NPCartist2 = 
(
										<id100070>
											<blocktype type="string">image</blocktype>
											<align type="string">center</align>
											<size type="string">400,1</size>
											<image type="image"><bitmap>images/refmandiv.jpg</bitmap></image>
											<caption type="string">Built by Engineer Suite(www.masq.net)&#13;</caption>
										</id100070>
				
)

RM_NPCbase = 
(
									</blocks>
				
)

