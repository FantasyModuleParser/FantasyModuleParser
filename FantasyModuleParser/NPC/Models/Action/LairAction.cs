using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.Models.Action
{
    public class LairAction : ActionModelBase
    {
        public LairAction() : base()
        {

        }

        public LairAction(string actionName, string actionDescription) : base(0, actionName, actionDescription)
        {

        }

        public LairAction(int actionId, string actionName, string actionDescription) : base(actionId, actionName, actionDescription)
        {

        }
    }
}
