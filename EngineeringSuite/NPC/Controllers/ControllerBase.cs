using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EngineeringSuite.NPC.Controllers
{
    public class ControllerBase
    {
		public NPCModel GetNPCModel()
		{
			var application = Application.Current;

			if (application is App app)
				return (NPCModel)app.NpcModel;

			return null;
		}
	}
}
