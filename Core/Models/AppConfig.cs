using mMacro.Core.Functions;
using mMacro.Core.Managers;
using System.Numerics;
using mMacro.Core.Utils;

namespace mMacro.Core.Models
{
    public class AppConfig
    {
        // =================== General ===================
        public Dictionary<string, Keybind> Keybinds { get; set; } = new();
        public int ClickDelay { get; set; } = 1;

        // ================== Inventory ==================
        public Vector2 FirstCellPosition { get; set; } = new Vector2(1371.0f, 642.0f);
        public Vector2 FirstBagPosition {  get; set; } = new Vector2(1345.0f, 576.0f);  
        
        public Vector2 DragStart { get; set; } = new Vector2(1327.0f, 598.0f);
        public Vector2 DragEnd { get; set; } = new Vector2(1894.0f, 925.0f);

        public int BagCount { get; set; } = 9;
        public ScanType ScanType { get; set; } = ScanType.Simple;
        public int scanLevel { get; set; } = 2;
        public Dictionary<string, PixelUtils.ColorRange> ColorRanges { get; set; } = new() {
            { "Blue", new PixelUtils.ColorRange { R = (55, 57), G = (113, 117), B = (166, 173) } },
            { "Purple", new PixelUtils.ColorRange { R = (150, 156), G = (42, 44), B = (79, 82) } },
            { "Gold", new PixelUtils.ColorRange { R = (254, 255), G = (161, 162), B = (73, 75) } },
            { "Unique", new PixelUtils.ColorRange { R = (249, 256), G = (248, 255), B = (97, 107) } },
            { "Set", new PixelUtils.ColorRange  { R = (38,78), G = (188, 199), B = (223, 227) } },
            { "Color Item", new PixelUtils.ColorRange { R = (120, 131), G = (96, 107), B = (66, 77) } }
        };

        // ================== SwapCape ==================
        public Vector2 ClosePoint { get; set; } = Vector2.Zero;
        public int swapBag { get; set; } = 0;
        public int swapRow { get; set; } = 1;
        public int swapCol { get; set; } = 0;
        // ================== ReviveBot ==================
        public Vector2 FirstPlayerPos { get; set; } = Vector2.Zero;
    }
}
