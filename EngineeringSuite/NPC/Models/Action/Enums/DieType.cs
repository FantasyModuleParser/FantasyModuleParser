﻿using System.ComponentModel;

namespace EngineeringSuite.NPC.Models.Action.Enums
{
	public enum DieType
	{
		[Description("d4")]
		D4 = 4,
		[Description("d6")]
		D6 = 6,
		[Description("d8")]
		D8 = 8,
		[Description("d10")]
		D10 = 10,
		[Description("d12")]
		D12 = 12,
		[Description("d20")]
		D20 = 20,
	}
}