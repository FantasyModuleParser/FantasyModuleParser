
using System.ComponentModel;

namespace FantasyModuleParser.Tables.ViewModels.Enums
{
    public enum RollMethodEnum
    {
        [Description("Based on table values")]
        TableValues = 0,

        [Description("Preset range")]
        PresetRange = 1,

        [Description("Custom dice roll")]
        CustomDiceRoll = 2
    }
}
