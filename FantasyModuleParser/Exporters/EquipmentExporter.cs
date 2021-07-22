using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Main.Models;
using log4net;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class EquipmentExporter
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(EquipmentExporter));

		static public string EquipmentNameToXML(EquipmentModel equipmentModel)
		{
			string name = equipmentModel.Name.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("(", "_").Replace(")", "");
		}

		static public void EquipmentLocked(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			if (equipmentModel.IsLocked == true)
			{
				xmlWriter.WriteStartElement("locked"); /* <locked> */
				xmlWriter.WriteAttributeString("type", "number");
				xmlWriter.WriteString("1");
				xmlWriter.WriteEndElement(); /* </locked> */
			}
			else
			{
				xmlWriter.WriteStartElement("locked"); /* <locked> */
				xmlWriter.WriteAttributeString("type", "number");
				xmlWriter.WriteString("0");
				xmlWriter.WriteEndElement(); /* </locked> */
			}
		}
	}
}
