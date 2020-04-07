/*
Component:	Equipment Engineer Include HTML for internal use
  Version:	1.1.0
     Date:	21/08/19
 Revision:	All HTML and CSS set; css for frame added.
 */

;~ local css, htmspell


TexSP:= imgdir("spell.jpg")
TexBG:= imgdir("artebg.jpg")
TexSc:= imgdir("scroll.png")
css =
(
	<head>
		<style>
			body {
				background-image: url("%TexBG%");
				background-color: #cccccc;
			}
			div.spcontainer {
				margin: auto;
 				width: 10.3cm;
				border-left-style: solid;
				border-left-width: 1px;
				border-left-color: #440000;
				border-right-style: solid;
				border-right-width: 1px;
				border-right-color: #440000;
				box-shadow: 8px 8px 20px #444455;
				background-image: url("%TexSP%");
			}
			div.capa {
				margin: auto;
          		height: 0.7cm;
				width: 12cm;
				background: rgba(0, 0, 0, 0.0);
				padding: 0cm 0.15cm 0cm 0.15cm;
				background-image: url("%TexSc%");
				background-repeat: no-repeat;
				background-size: cover;
				position: relative;
				top: 2px;
			}
			div.capb {
				margin: auto;
          		height: 0.7cm;
				width: 12cm;
				background: rgba(0, 0, 0, 0.0);
				padding: 0cm 0.15cm 0cm 0.15cm;
				background-image: url("%TexSc%");
				position: relative;
				background-repeat: no-repeat;
				background-size: cover;
				top: -2px;
			}
			div.spellname {
				margin: auto;
				width: 10cm;
				background: rgba(0, 0, 0, 0.0);
				color: #7A200D;
				font-family: "Mr. Eaves Small Caps";
				font-size: 16pt;
				padding: 0.3cm 0.15cm 0cm 0.15cm;
			}
			div.spelltype {
				margin: auto;
				width: 10cm;
				background: rgba(0, 0, 0, 0.0);
				color: #000000;
				font-family: "Calibri";
				font-size: 10pt;
				padding: 0cm 0.15cm 0cm 0.15cm;
				position: relative; top: -5px;
			}
			div.divider {
				margin: auto;
				width: 10cm;
				background: rgba(0, 0, 0, 0.0);
				padding: 0cm 0.15cm 0cm 0.15cm;
			}
			.whoosh {
				width: 100`%;    
			}
			div.spellbody {
				margin: auto;
				width: 10cm;
				background: rgba(0, 0, 0, 0.0);
				color: #333333;
				font-family: "Century Gothic";
				font-size: 10pt;
				padding: 0cm 0.15cm 0cm 0.15cm;
			}
			.heading {
				background: rgba(0, 0, 0, 0.0);
				color: #7A200D;
				font-size: 14pt;
				font-weight: bold;
			}
			div.spelltext {
				margin: auto;
				width: 10cm;
				background: rgba(0, 0, 0, 0.0);
				color: #333333;
				font-family: "Century Gothic";
				font-size: 10pt;
				padding: 0cm 0.15cm 0.3cm 0.15cm;
			}
			table.attr {
				background: rgba(0, 0, 0, 0.0);
				color: %Htext%;
				font-family: "Calibri";
				font-size: 11pt;
				border-style: none;
				border-collapse: collapse;
				text-align: center;
				padding: 0cm, 0cm, 0cm, 0cm;
			}
			img {
				float: right;
				opacity: 0.4;
			}
			div.nonid {
				margin: auto;
				width: 10cm;
				background: rgba(0, 0, 0, 0.0);
				color: #000000;
				font-family: "Century Gothic";
				font-size: 9pt;
				padding: 0cm 0.15cm 0cm 0.15cm;
			}
		</style>
	</head>
)

htmtop = 
(
	<body>
		<div class="capa"></div>
		<div class="spcontainer">
			<div class="spellname"><b>%EQEName%</b></div>
			<div class="nonid">
				<table class="nonid">
					<tr>
						<td style="width: 2.5cm; text-align: right; vertical-align: top"><b>Non-ID Name: </b></td>
						<td style="width: 8.0cm; text-align: left; vertical-align: top">%EQENonid%</td>
					</tr>
					<tr>
						<td style="width: 2.5cm; text-align: right; vertical-align: top"><b>Non-ID Notes: </b></td>
						<td style="width: 8.0cm; text-align: left; vertical-align: top">%EQENonidDesc%</td>
					</tr>
				</table>
			</div>
			<div class="divider">
				<svg viewBox="0 0 200 5" preserveAspectRatio="none" width=100`% height="5">
					<polyline points="0,0 200,2.5 0,5" fill="#922610" class="whoosh"></polyline>
				</svg>
			</div>
			<div class="spellbody">
				<b>Type: </b>%EQEtype%<br>
				<b>Subtype: </b>%EQEsubtype%<br>
				<b>Rarity: </b>%EQErarity%<br>
				<b>Attunement: </b>%EQEattune%<br>
			</div>
			<div class="divider">
				<svg viewBox="0 0 200 5" preserveAspectRatio="none" width=100`% height="5">
					<polyline points="0,0 200,2.5 0,5" fill="#922610" class="whoosh"></polyline>
				</svg>
			</div>
			<div class="spellbody">
)


htmMid2 = 
(
				<b>Magical Bonus: </b>%magbonus%<br>
				<b>Armor Class: </b>%EQEarmourclass%<br>
				<b>Dexterity Bonus: </b>%EQEarmourdexbon%<br>
				<b>Strength: </b>%EQEarmourstr%<br>
				<b>Stealth: </b>%EQEarmourstealth%<br>
)

htmMid3 = 
(
				<b>Magical Bonus: </b>%magbonus%<br>
				<b>Damage: </b>%EQEWeapDam%<br>
				<b>Properties: </b>%EQEWeapProp%<br>
)


htmbottom = 
(
				<b>Cost: </b>%EQEcost%<br>
				<b>Weight: </b>%EQEweight%<br>
			</div>
			<div class="divider">
				<svg viewBox="0 0 200 5" preserveAspectRatio="none" width=100`% height="5">
					<polyline points="0,0 200,2.5 0,5" fill="#922610" class="whoosh"></polyline>
				</svg>
			</div>
			<div class="spellbody">
				%SpText%
			</div>
		</div>
		<div class="capb"></div>
	</body>
</html>
)


