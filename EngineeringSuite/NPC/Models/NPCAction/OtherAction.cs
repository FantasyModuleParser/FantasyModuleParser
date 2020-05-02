using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction
{
    public class OtherAction : ActionModelBase
    {
        public OtherAction Clone()
        {
            OtherAction clone = new OtherAction();
            clone.ActionID = this.ActionID;
            clone.ActionName = this.ActionName;
            clone.ActionDescription = this.ActionDescription;

            return clone;
        }

        #region Equals and Hashcode
        public override bool Equals(object obj)
        {
            return obj is OtherAction @base &&
                   ActionName == @base.ActionName;
        }

        public override int GetHashCode()
        {
            return 736796887 + EqualityComparer<string>.Default.GetHashCode(ActionName);
        }
        #endregion
    }
}
