/*
Component:	NPCE Include HTML for internal use
  Version:	1.1.2
     Date:	21/07/19
 Revision:	All HTML and CSS set; css for frame added.
 */


	Hheader:= "#" "2C3965"
	Hmaincell:= "#" "FEFFFF"
	Hborder:= "#" "000000"
	Hnumbercell:= "#" "D3DAEA"

TexSP:= imgdir("spell.jpg")
TexBG:= imgdir("tablebg.jpg")
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
			div.spellbody {
				margin: auto;
				width: 10cm;
				background: rgba(0, 0, 0, 0.0);
				color: #333333;
				font-family: "Century Gothic";
				font-size: 10pt;
				padding: 0cm 0.15cm 0.3cm 0.15cm;
				overflow:auto;			}
			div.spelltext {
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
			table.spell {
				background: %Hmaincell%;
				color: #000000;
				font-family: "Arial";
				font-size: 10pt;
				border: 1px solid %Hborder%;
				border-collapse: collapse;
				text-align: left;
				padding: 0.2cm, 0.2cm, 0.2cm, 0.2cm;
				box-shadow: 3px 3px #DDDDDD;
				table-layout: fixed;
				width: 100`%;
			}
			table.spell th {
				background-color: %Hheader%;
				color: #FFFFFF;
				padding: 0.1cm;
				border: 1px solid %Hborder%;
				border-collapse: collapse;
				min-height:16px
			}
			table.spell td {
				padding: 0.1cm;
				border: 1px solid %Hborder%;
				border-collapse: collapse;
				min-height:16px
			}
			.numbers{   background: %Hnumbercell%; }
		</style>
	</head>
)

htmspell = 
(
	<body>
		<div class="capa"></div>
		<div class="spcontainer">
			<div class="spellname"><b>%SpName%</b></div>
			<div class="spellbody">
				<b>Description: </b>%TABdecsr%<br>
				<b>Output to: </b>%TABout%<br>
				<b>Dice roll type: </b>%rolltype%<br>
				<b>Range: </b>%diceroll%<br>
			</div>
			<div class="spellbody">
				%TH4%
				%SpText%
			</div>
		</div>
		<div class="capb"></div>
	</body>
</html>
)

