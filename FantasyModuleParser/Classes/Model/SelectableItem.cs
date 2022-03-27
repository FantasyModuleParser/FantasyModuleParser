using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Classes.Model
{
    public class SelectableItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }

        public SelectableItem()
        {
        }
    }
}
