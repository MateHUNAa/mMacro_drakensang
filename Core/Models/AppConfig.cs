using mMacro.Core.Functions;
using mMacro.Core.Managers;
using System.Numerics;

namespace mMacro.Core.Models
{
    public class AppConfig
    {
        public Dictionary<string, Keybind> Keybinds { get; set; } = new();
        public Vector2 FirstCellPosition { get; set; } = Vector2.Zero;
        public Vector2 FirstBagPosition {  get; set; } = Vector2.Zero;  
        
        public Vector2 DragStart { get; set; } = Vector2.Zero;
        public Vector2 DragEnd { get; set; } = Vector2.Zero;

        public int BagCount { get; set; } = 9;
        public ScanType ScanType { get; set; } = ScanType.Simple;
        public int scanLevel { get; set; } = 2;
        public Dictionary<string, ColorRange> ColorRanges { get; set; } = new() {
            { "Blue", new ColorRange { R = (55, 57), G = (113, 117), B = (166, 173) } },
            { "Purple", new ColorRange { R = (150, 156), G = (42, 44), B = (79, 82) } },
            { "Gold", new ColorRange { R = (254, 255), G = (161, 162), B = (73, 75) } },
            { "Unique", new ColorRange { R = (249, 256), G = (248, 255), B = (97, 107) } },
            { "Set", new ColorRange { R = (91, 124), G = (205, 219), B = (232, 242) } },
            { "Color Item", new ColorRange { R = (120, 131), G = (96, 107), B = (66, 77) } }
        };
    }
}
