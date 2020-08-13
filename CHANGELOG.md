# Fantasy Module Parser Change Log
## Version 0.1.0.8-Alpha
#### ADDED FUNCTIONALITY
- Fixed multiple issues with the Engineer Suite Importer
- Fixed issue with Preview NPC Window not showing Spellcasting section
#### STILL NOT WORKING
- Image tab
- Description tab
## Version 0.1.0.7-Alpha
#### ADDED FUNCTIONALITY
- NPC Tokens selector now works
- Thumbnails path now works
- Engineer Suite Importer is located in Import Text button
#### STILL NOT WORKING
- Image tab
- Description tab
## Version 0.1.0.6-Alpha
#### BUG FIXED
- FMPBUG-7: If you have a space in the NPC Name, FMP will crash at Create Module
  - Reason: XML hates spaces inside the <...>
## Version 0.1.0.5-Alpha
#### BUG FIXED
- Multiple instances of the same window opening by clicking an option multiple times
## Version 0.1.0.4-Alpha
#### BUGS FIXED
- User Interface adjustments with textboxes
## Version 0.1.0.3-Alpha
####BUGS FIXED
- None
#### NOTES
- Fixed some Github issues with Commits
- Forgot to mention .mod files SHOULD work in both FGC and FGU without the unzip/rezip workaround.
## Version 0.1.0.2-Alpha
#### BUGS FIXED
- Preview Window would add (blind beyond this radius) to all Senses
- Negative numbers able to be input for Abilities, Speed, Senses, and Experience
## Version 0.1.0.1-Alpha
#### FUNCTIONAL
- Options > Manage Project/Create Module/Manage Categories
- Directories > All but Fantasy Module Parser system folder
- Information > About/Supporters
- Add NPCs / Monsters
#### NON-FUNCTIONAL
- Options > Settings
- Directories > Fantasy Module Parser system folder
#### NOTES
- Creating Module
  1) Manage Project
  2) Add or Load NPC
  3) Add to Project (Current bug: Will duplicate NPC in XML if NPC has same name)
  4) Repeat for additional NPCs after you click New NPC
  5) Create Module
