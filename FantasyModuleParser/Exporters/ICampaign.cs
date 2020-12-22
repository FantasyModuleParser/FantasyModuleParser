using FantasyModuleParser.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Exporters
{
    public interface ICampaign
    {
        void CreateCampaign(ModuleModel moduleModel);
    }
}
