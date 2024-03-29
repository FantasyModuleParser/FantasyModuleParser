﻿using System.ComponentModel;

namespace FantasyModuleParser.NPC.Models.Action.Enums
{
    public enum WeaponResistance
    {
		[Description("No special weapon resistance")]
		NoSpecial = 0,
		
		[Description("Resistant to nonmagical weapons")]
		Nonmagical = 1,
		
		[Description("Resistant to nonmagical weapons that aren't silvered")]
		NonmagicalSilvered = 2,
		
		[Description("Resistant to nonmagical weapons that aren't adamantine")]
		NonmagicalAdamantine = 3,
		
		[Description("Resistant to nonmagical weapons that aren't cold-forged iron")]
		NonmagicalColdForgedIron = 4,
		
		[Description("Resistant to magic weapons")]
		Magical = 5,
	}
}
