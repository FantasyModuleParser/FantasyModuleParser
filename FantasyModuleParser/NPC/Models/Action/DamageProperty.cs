﻿using System;
using FantasyModuleParser.NPC.Models.Action.Enums;

namespace FantasyModuleParser.NPC.Models.Action
{
    public class DamageProperty : IEquatable<DamageProperty>
    {
	    public int NumOfDice { get; set; }

        public DieType DieType { get; set; }

        public int Bonus { get; set; }

        public DamageType DamageType { get; set; }

        public DamageProperty()
        {
        }

        public DamageProperty(int numOfDice, DieType dieType, int bonus, DamageType damageType)
        {
            NumOfDice = numOfDice;
            DieType = dieType;
            Bonus = bonus;
            DamageType = damageType;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DamageProperty);
        }

        public bool Equals(DamageProperty other)
        {
            return other != null &&
                   NumOfDice == other.NumOfDice &&
                   DieType == other.DieType &&
                   Bonus == other.Bonus &&
                   DamageType == other.DamageType;
        }
    }
}
