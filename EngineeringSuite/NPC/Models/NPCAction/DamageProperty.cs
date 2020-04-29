using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction
{
    public class DamageProperty
    {
        int numOfDice;
        int dieType; // valid values:  4, 6, 8, 10, 12, 20
        int bonus;
        String damageType;

        public DamageProperty(int numOfDice, int dieType, int bonus, string damageType)
        {
            this.numOfDice = numOfDice;
            this.dieType = dieType;
            this.bonus = bonus;
            this.damageType = damageType;
        }

        public override bool Equals(object obj)
        {
            return obj is DamageProperty property &&
                   numOfDice == property.numOfDice &&
                   dieType == property.dieType &&
                   bonus == property.bonus &&
                   damageType == property.damageType;
        }

        public override int GetHashCode()
        {
            int hashCode = 1657412010;
            hashCode = hashCode * -1521134295 + numOfDice.GetHashCode();
            hashCode = hashCode * -1521134295 + dieType.GetHashCode();
            hashCode = hashCode * -1521134295 + bonus.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(damageType);
            return hashCode;
        }
    }
}
