using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Enums
{
    public enum TreasureEnum
    {
        [Description("Art Objects (25gp)")]
        Art_25 = 0,

        [Description("Art Objects (250 gp)")]
        Art_250 = 1,

        [Description("Art Objects (750 gp)")]
        Art_750 = 2,

        [Description("Art Objects (2500 gp)")]
        Art_2500 = 3,

        [Description("Art Objects (7500 gp)")]
        Art_7500 = 4,

        [Description("Gemstones (10 gp)")]
        Gem_10 = 5,

        [Description("Gemstones (50 gp)")]
        Gem_50 = 6,

        [Description("Gemstones (100 gp)")]
        Gem_100 = 7,

        [Description("Gemstones (500 gp)")]
        Gem_500 = 8,

        [Description("Gemstones (1000 gp)")]
        Gem_1000 = 9,

        [Description("Gemstones (5000 gp)")]
        Gem_5000 = 10,
    }
}
