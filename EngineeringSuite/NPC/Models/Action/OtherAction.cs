using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.Action
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
    }
}
