using System.ComponentModel;

namespace FantasyModuleParser.Tables.ViewModels.Enums
{
    public enum OutputTypeEnum
    {
        [Description("Chat")]
        Chat = 0,

        [Description("Story")]
        Story = 1,

        [Description("Parcel")]
        Parcel = 2,

        [Description("Encounter")]
        Encounter = 3
    }
}
