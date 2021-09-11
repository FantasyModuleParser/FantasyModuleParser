# Breakdown of Classes for Module

This is a rough breakdown of the SRD client.xml from the class data.  The intention is to provide a framework that FMP will use through the backend classes.  This also may serve as a base document for driving the UI components in FMP.

The flow of this document is breaking down the tag layers from the SRD client.xml.  There will be sections that appear to repeat themselves, which is intended at this stage.

## Class Name
Every class requires a name, which acts as the unique identifier in its entirety. 

## Class Features
### Table Stats
#### Common Items
- Proficency
  - Broken down by level
- Features
  - Broken down by level

#### Unique Items
- Rages (Barbarian specifically)
- Rage Damage (Barbarian Specific)
- Spell Slots (Magic users)

### Class Specifics
#### Hit Points
Determined by a single Die type value (d6, d8, d10, d12); 
A statement about the Con Modifier is included

#### Proficiencies
##### Armor
Comes from a static list of items:
- Light Armor
- Medium Armor
- Heavy Armor
- Shields
##### Weapons
Comes from a static list of items:
- Simple Weapons
- Martial Weapons
- Specific Weapons (Longbow, Warhammer, Mace, etc...)

##### Tools
Comes from a static list of items
- Artisans Toolkit
- Leatherworking Toolkit
- Thieve's Toolkit
- etc.....

#### Saving Throws
Based on two of the six character attributes (Str, Dex, Con, Int, Wis, Cha)

#### Skills
Choice between zero and three skills to start with (based on DnD 5e Character sheet stats)

#### Starting Equipment
This one is a bit tricker to implement outside of a just a list of textboxes.  For example, the Barbarian from the SRD:
```
<li>(a) a greataxe or (b) any martial melee weapon</li>
<li>(a) two handaxes or (b) any simple weapon</li>
<li>An explorer's pack and four javelins</li>
```

### Features
#### Listlink of all features
This is on purpose so Fantasy Grounds can provide links to other features, instead of cramming all the information into one giant blob.  This may take a little time to implement, **but would likely require their own class structure in FMP**

### Unique Class Paths
Each DnD 5e class has their own subclasses that can be taken;  For Barbarians, this is Primal Paths;  For Bards, the College of \*\*\*, etc....  
**This section will likely break down into their own category**

## HP
Overall, a more in-depth description of the Hit Dice & Hit Points gained for each level.
Really, the only variable bit here is the differing die type for hit points (d6, d8, d10, d12)

## Proficiencies
As above in [Proficiencies](#proficiencies), this is a breakdown of what's available for a character starting with this class.
The section is broken down by:
- Armor
- Weapons
- Tools
- Saving Throws
- Skills

## Multiclass Proficiencies
When a character levels into this class **outside of their starting class**, the following proficiencies are included into their character.

The section is broken down by:
- Armor
- Weapons
- Tools
- Skills

## Features
The bulk of each class section!

### Generic Features
Most features fall under this category.  They include the following:
- Name
- Level Requirment
  - Range is 1 thru 20
- Text
  - Formatted via Markdown
- Specialization (Unique)
  - **Linked back to a specific class path, such as College of Lore for Bards** 

### Unique Features
#### Spellcasting
This one is a biggie, and the largest feature by far.  From what I can tell, it has the following options for a UI:

- Number of cantrips to begin with
- Spells Known of 1st Level and Higher
- Spellcasting Ability
  - Based on a singular character Attribute (Int, Wis, Cha)
- Ritual Casting
  - Optional;  Maybe a checkbox?
- Spellcasting Focus
  - Textbox describing the focus item (arcane focus, guitar, staff, etc...)

##### For Wizards Specificially
All of the items under [Spellcasting](#spellcasting) apply for Wizards, but they have a whole bunch-o-rules 
in regards to learning spells & copying into their books.

## Multiclass Features
Based on the SRD Client.xml, there are **no multiclass features**.  Will need to confer with other DM's and materials to see if this is the case moving forward.

## Abilities
Abilities is where class specific paths are written.  For Barbarian, this includes details on things like Path of the Beserker;  For Bard, this is the College of Lore.

### Name
Name of the unique ability (Path of the Berserker, College of Lore, School of Evocation, etc...)

### Level
Based on the SRD client.xml, all abilities start at level 1

### Text (Breakdown)
Includes a Description (formatted)

#### Features
In FMP, this may be broken down into it's own unique window or User-Control.  The features here are linked back to those records in the [Features](#generic-features) above


## Equipment
This is broken down the same as [Starting Equipment](#starting-equipment), just formatted differently in the client.xml


- [Breakdown of Classes for Module](#breakdown-of-classes-for-module)
  - [Class Name](#class-name)
  - [Class Features](#class-features)
    - [Table Stats](#table-stats)
      - [Common Items](#common-items)
      - [Unique Items](#unique-items)
    - [Class Specifics](#class-specifics)
      - [Hit Points](#hit-points)
      - [Proficiencies](#proficiencies)
        - [Armor](#armor)
        - [Weapons](#weapons)
        - [Tools](#tools)
      - [Saving Throws](#saving-throws)
      - [Skills](#skills)
      - [Starting Equipment](#starting-equipment)
    - [Features](#features)
      - [Listlink of all features](#listlink-of-all-features)
    - [Unique Class Paths](#unique-class-paths)
  - [HP](#hp)
  - [Proficiencies](#proficiencies-1)
  - [Multiclass Proficiencies](#multiclass-proficiencies)
  - [Features](#features-1)
    - [Generic Features](#generic-features)
    - [Unique Features](#unique-features)
      - [Spellcasting](#spellcasting)
        - [For Wizards Specificially](#for-wizards-specificially)
  - [Multiclass Features](#multiclass-features)
  - [Abilities](#abilities)
    - [Name](#name)
    - [Level](#level)
    - [Text (Breakdown)](#text-breakdown)
      - [Features](#features-2)
  - [Equipment](#equipment)