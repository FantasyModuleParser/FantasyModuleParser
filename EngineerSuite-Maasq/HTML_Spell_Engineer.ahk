/*
Component:	NPCE Include HTML for internal use
  Version:	1.1.2
     Date:	16/02/18
 Revision:	All HTML and CSS set; css for frame added.
 */

;~ local css, htmspell


TexSP:= imgdir("spell.jpg")
TexBG:= imgdir("spellbg.jpg")
TexSc:= imgdir("scroll.png")
SpGfx:= imgdir(SpPic)
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
			div.spellbody {
				margin: auto;
				width: 10cm;
				background: rgba(0, 0, 0, 0.0);
				color: #333333;
				font-family: "Century Gothic";
				font-size: 10pt;
				padding: 0cm 0.15cm 0.3cm 0.15cm;
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
			div.spellimage {
				float: right;
				opacity: 0.4;
			}
		</style>
	</head>
)

htmspell = 
(
	<body>
		<div class="capa"></div>
		<div class="spcontainer">
			<div class="spellimage">
				<img src="%SpGfx%" style="width:90px;height:90px;margin-right:5px;">
			</div>
			<div class="spellname"><b>%SpName%</b></div>
			<div class="spelltype"><i>%Spell_level_etc%</i></div>
			<div class="spellbody">
				<b>Casting Time: </b>%SpCtim%<br>
				<b>Range: </b>%Sprnge%<br>
				<b>Components: </b>%Spell_Components%<br>
				<b>Duration: </b>%SpDura%<br>
				%SpText%
				%SpCasters%
			</div>
		</div>
		<div class="capb"></div>
	</body>
</html>
)

