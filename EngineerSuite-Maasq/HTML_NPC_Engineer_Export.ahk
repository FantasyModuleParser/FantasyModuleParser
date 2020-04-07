/*
Component:	NPCE Include HTML
  Version:	1.0.0
     Date:	09/01/18
 Revision:	All HTML and CSS set
 */

local css, htmtop, htmmiddle, htmbottom, htmsave, htmskill, htmdamvul, htmdamres, htmdamimm, htmconimm

css =
(
	<head>
		<style>
			div.cap {
				margin: auto;
          		height: 0.1cm;
				width: 8.3cm;
				background: #E69A28;
				border-style: solid;
				border-right-width: 1px;
				border-left-width: 1px;
				border-top-width: 1px;
				border-bottom-width: 1px;
				border-right-color: black;
				border-left-color: black;
				border-top-color: black;
				border-bottom-color: black;
				box-shadow: 3px 3px #DDDDDD;
			}
			div.npcname {
				margin: auto;
				width: 8cm;
				background: #FDF1DC;
				color: #7A200D;
				font-family: "Mr. Eaves Small Caps";
				font-size: 21pt;
				border-left-style: solid;
				border-right-style: solid;
				border-top-style: none;
				border-bottom-style: none;
				border-right-width: 1px;
				border-left-width: 1px;
				border-right-color: #D5C9B4;
				border-left-color: #D5C9B4;
				padding: 0cm 0.15cm 0cm 0.15cm;
				box-shadow: 3px 3px #DDDDDD;
			}
			div.npctype {
				margin: auto;
				width: 8cm;
				background: #FDF1DC;
				color: #000000;
				font-family: "Century Gothic";
				font-size: 9pt;
				border-left-style: solid;
				border-right-style: solid;
				border-top-style: none;
				border-bottom-style: none;
				border-right-width: 1px;
				border-left-width: 1px;
				border-right-color: #D5C9B4;
				border-left-color: #D5C9B4;
				padding: 0cm 0.15cm 0cm 0.15cm;
				box-shadow: 3px 3px #DDDDDD;
			}
			div.npctop {
				margin: auto;
				width: 8cm;
				background: #FDF1DC;
				color: #7A200D;
				font-family: "Century Gothic";
				font-size: 10.5pt;
				border-left-style: solid;
				border-right-style: solid;
				border-top-style: none;
				border-bottom-style: none;
				border-right-width: 1px;
				border-left-width: 1px;
				border-right-color: #D5C9B4;
				border-left-color: #D5C9B4;
				padding: 0cm 0.15cm 0cm 0.15cm;
				box-shadow: 3px 3px #DDDDDD;
			}
			div.npcbottom {
				margin: auto;
				width: 8cm;
				background: #FDF1DC;
				color: #000000;
				font-family: "Century Gothic";
				font-size: 10pt;
				border-left-style: solid;
				border-right-style: solid;
				border-top-style: none;
				border-bottom-style: none;
				border-right-width: 1px;
				border-left-width: 1px;
				border-right-color: #D5C9B4;
				border-left-color: #D5C9B4;
				padding: 0cm 0.15cm 0cm 0.15cm;
				box-shadow: 3px 3px #DDDDDD;
			}
			div.npcdescrip {
				margin: auto;
				width: 8cm;
				background: #FFFFFF;
				color: #000000;
				font-family: "Century Gothic";
				font-size: 10pt;
				padding: 0cm 0.15cm 0cm 0.15cm;
			}
			div.npcdiv {
				margin: auto;
				width: 8cm;
				background: #FDF1DC;
				border-left-style: solid;
				border-right-style: solid;
				border-top-style: none;
				border-bottom-style: none;
				border-right-width: 1px;
				border-left-width: 1px;
				border-right-color: #D5C9B4;
				border-left-color: #D5C9B4;
				padding: 0.2cm 0.15cm 0.2cm 0.15cm;
				box-shadow: 3px 3px #DDDDDD;
			}
			.whoosh {
				width: 100`%;    
			}
			table.attr {
				background: #FDF1DC;
				color: #7A200D;
				font-family: "Calibri";
				font-size: 11pt;
				border-style: none;
				border-collapse: collapse;
				text-align: center;
				padding: 0cm, 0cm, 0cm, 0cm;
			}
			div.npcdiv2 {
				margin: auto;
				width: 8cm;
				background: #FDF1DC;
				border-left-style: solid;
				border-right-style: solid;
				border-top-style: none;
				border-bottom-style: none;
				border-right-width: 1px;
				border-left-width: 1px;
				border-right-color: #D5C9B4;
				border-left-color: #D5C9B4;
				padding: 0cm 0.15cm 0.2cm 0.15cm;
				box-shadow: 3px 3px #DDDDDD;
			}
		</style>
	</head>
)

htmtop = 
(
	<body>
		<div class="cap"></div>
		<div class="npcname"><b>%NPCName%</b></div>
		<div class="npctype"><i>%NPC_size_etc%</i></div>
		<div class="npcdiv">
			<svg viewBox="0 0 200 5" preserveAspectRatio="none" width=100`% height="5">
				<polyline points="0,0 200,2.5 0,5" fill="#922610" class="whoosh"></polyline>
			</svg>
		</div>
		<div class="npctop"><b>Armor Class</b> %NPCac%</div>
		<div class="npctop"><b>Hit Points</b> %NPChp%</div>
		<div class="npctop"><b>Speed</b> %NPCspeed%</div>
		<div class="npcdiv">
			<svg viewBox="0 0 200 5" preserveAspectRatio="none" width=100`% height="5">
				<polyline points="0,0 200,2.5 0,5" fill="#922610" class="whoosh"></polyline>
			</svg>
		</div>
		<div class="npctop">
			<table class="attr" width=100`%>
				<tbody>
					<tr valign="middle">
						<td><b>STR</b></td>
						<td><b>DEX</b></td>
						<td><b>CON</b></td>
						<td><b>INT</b></td>
						<td><b>WIS</b></td>
						<td><b>CHA</b></td>
					</tr>
					<tr valign="middle">
						<td>%NPCstr% (%NPCstrbo%)</td>
						<td>%NPCdex% (%NPCdexbo%)</td>
						<td>%NPCcon% (%NPCconbo%)</td>
						<td>%NPCint% (%NPCintbo%)</td>
						<td>%NPCwis% (%NPCwisbo%)</td>
						<td>%NPCcha% (%NPCchabo%)</td>
					</tr>
				</tbody>
			</table>
		</div>
		<div class="npcdiv">
			<svg viewBox="0 0 200 5" preserveAspectRatio="none" width=100`% height="5">
				<polyline points="0,0 200,2.5 0,5" fill="#922610" class="whoosh"></polyline>
			</svg>
		</div>
)

htmsave = 
(
		<div class="npctop"><b>Saving Throws</b> %NPCSave%</div>
)

htmskill = 
(
		<div class="npctop"><b>Skills</b> %NPCskill%</div>
)

htmdamvul = 
(
		<div class="npctop"><b>Damage Vulnerabilities</b> %NPCdamvul%</div>
)

htmdamres = 
(
		<div class="npctop"><b>Damage Resistances</b> %NPCdamres%</div>
)

htmdamimm = 
(
		<div class="npctop"><b>Damage Immunities</b> %NPCdamimm%</div>
)

htmconimm = 
(
		<div class="npctop"><b>Condition Immunities</b> %NPCconimm%</div>
)

htmmiddle = 
(
		<div class="npctop"><b>Senses</b> %NPCSense%</div>
		<div class="npctop"><b>Languages</b> %NPCLang%</div>
		<div class="npctop"><b>Challenge</b> %NPCcharat% (%NPCxp% XP)</div>
		<div class="npcdiv">
			<svg viewBox="0 0 200 5" preserveAspectRatio="none" width=100`% height="5">
				<polyline points="0,0 200,2.5 0,5" fill="#922610" class="whoosh"></polyline>
			</svg>
		</div>
)

htmtraits = 
(
		<div class="npcbottom">%NPCTraitsH%</div>
		<div class="npcbottom">%NPCinspellH%</div>
		<div class="npcbottom">%NPCspellH%</div>
)

htmactions = 
(
		<div class="npctop"><br><span style="font-size: 17pt;">A</span><span style="font-size: 12pt;">CTIONS</span>
		<span style="display:block;margin-bottom:-14px;"></span></div>
		<div class="npcdiv2">
			<svg viewBox="0 0 200 1" preserveAspectRatio="none" width=100`% height="1">
				<polyline points="0,0 200,0 200,1 0,1" fill="#7A200D" class="whoosh"></polyline>
			</svg>
		</div>
		<div class="npcbottom">%NPCActionsH%</div>
)

htmreactions = 
(
		<div class="npctop"><br><span style="font-size: 17pt;">R</span><span style="font-size: 12pt;">EACTIONS</span>
		<span style="display:block;margin-bottom:-14px;"></span></div>
		<div class="npcdiv2">
			<svg viewBox="0 0 200 1" preserveAspectRatio="none" width=100`% height="1">
				<polyline points="0,0 200,0 200,1 0,1" fill="#7A200D" class="whoosh"></polyline>
			</svg>
		</div>
		<div class="npcbottom">%NPCreactionsH%</div>
)

htmlegactions = 
(
		<div class="npctop"><br><span style="font-size: 17pt;">L</span><span style="font-size: 12pt;">EGENDARY </span><span style="font-size: 17pt;">A</span><span style="font-size: 12pt;">CTIONS</span>
		<span style="display:block;margin-bottom:-14px;"></span></div>
		<div class="npcdiv2">
			<svg viewBox="0 0 200 1" preserveAspectRatio="none" width=100`% height="1">
				<polyline points="0,0 200,0 200,1 0,1" fill="#7A200D" class="whoosh"></polyline>
			</svg>
		</div>
		<div class="npcbottom">%NPClegactionsH%</div>
)

htmlairactions = 
(
		<div class="npctop"><br><span style="font-size: 17pt;">L</span><span style="font-size: 12pt;">AIR </span><span style="font-size: 17pt;">A</span><span style="font-size: 12pt;">CTIONS</span>
		<span style="display:block;margin-bottom:-14px;"></span></div>
		<div class="npcdiv2">
			<svg viewBox="0 0 200 1" preserveAspectRatio="none" width=100`% height="1">
				<polyline points="0,0 200,0 200,1 0,1" fill="#7A200D" class="whoosh"></polyline>
			</svg>
		</div>
		<div class="npcbottom">%NPClairactionsH%</div>
)

htmbottom = 
(
		<div class="npcbottom">&nbsp;</div>
		<div class="cap"></div>
)

htmdesc =
(
		<div class="npcdescrip">%NPCdescript%</div>

)

htmend = 
(
	</body>
</html>
)