using mMacro.Core.Functions.Inventory;
using mMacro.Core.Managers;
using mMacro.Core.Utils;
using System.Numerics;
using static mMacro.Core.Utils.PixelUtils;

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
          { "Blue", new ColorRange { R = (55, 60), G = (113, 120), B = (166, 179) } },
            { "Purple", new ColorRange { R = (150, 162), G = (42, 47), B = (79, 85) } },
            { "Gold", new ColorRange { R = (254, 255), G = (161, 170), B = (73, 77) } },
            { "Unique", new ColorRange { R = (248, 256), G = (248, 255), B = (40, 60) } },
            { "Set", new ColorRange { R = (38,78), G = (188, 199), B = (223, 227) } },
            { "Color Item", new ColorRange { R = (120, 131), G = (96, 107), B = (66, 77) } }
        };

        // ================== SwapCape ==================
        public Vector2 ClosePoint { get; set; } = Vector2.Zero;
        public int swapBag { get; set; } = 0;
        public int swapRow { get; set; } = 1;
        public int swapCol { get; set; } = 0;
        // ================== ReviveBot ==================
        public Vector2 FirstPlayerPos { get; set; } = Vector2.Zero;
        // ================== MeltBot ==================
        public Vector2 MeltFirstPos { get; set; } = Vector2.Zero;
        public Vector2 MeltButtonPos { get; set; } = Vector2.Zero;
        public Vector2 MeltCloseBtn { get; set; } = Vector2.Zero;

        // ================== MeltGem ==================
        public UIPositions.CraftPositions CraftPositions { get; set; } = new UIPositions.CraftPositions();

    }
}
