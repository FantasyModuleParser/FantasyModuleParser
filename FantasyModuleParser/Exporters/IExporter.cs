using FantasyModuleParser.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Exporters
{
    public interface IExporter
    {
        void CreateModule(ModuleModel moduleModel);
        void CreateCampaign(ModuleModel moduleModel);
    }
}
