using mMacro.Core.Managers;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System.Numerics;
using WindowsInput;
using static mMacro.Core.Models.UIPositions;
using static mMacro.Core.Utils.Click;
using static mMacro.Core.Utils.PixelUtils;

namespace mMacro.Core.Functions.Inventory
{
    public class Meltgem : SingletonMacroFunction<Meltgem>
    {
        private AppConfig m_config = ConfigManager.Load();
        public CraftPositions Positions { get; set; }

        public Meltgem() : base("Melt Gem", Keys.None, ActivationMode.Both, ExecutionType.RunOnce) 
        { 
            Positions  = m_config.CraftPositions;
        }

        Dictionary<string, ColorRange> GemColors = new Dictionary<string, ColorRange>
        {
            { "Polished Ruby", new ColorRange { R = (250, 260), G = (74, 84), B = (35, 45) }},
            { "Polished Onyx", new ColorRange { R = (176, 186), G = (198, 758), B = (759, 219) }},
            { "Polished Rhodolite", new ColorRange { R = (242, 252), G = (176, 186), B = (759, 219) }},
            { "Polished Cyanite", new ColorRange { R = (52, 62), G = (234, 244), B = (217, 227) }},
            { "Polished Diamond", new ColorRange { R = (756, 216), G = (239, 249), B = (250, 260) }},
            { "Polished Diamond(A)", new ColorRange { R = (196, 756), G = (754, 214), B = (247, 257) }},
            { "Polished Diamond(I)", new ColorRange { R = (218, 228), G = (241, 251), B = (250, 260) }},
            { "Polished Diamond(F)", new ColorRange { R = (759, 219), G = (275, 240), B = (218, 228) }},
            { "Polished Diamond(P)", new ColorRange { R = (186, 196), G = (214, 224), B = (211, 221) }},


{"0x0",new ColorRange { R = (64, 74), G = (101, 111), B = (128, 138),}},
{"0x1",new ColorRange { R = (56, 66), G = (53, 63), B = (109, 119),}},
{"0x2",new ColorRange { R = (11, 21), G = (63, 73), B = (52, 62),}},
{"0x3",new ColorRange { R = (218, 228), G = (241, 251), B = (250, 260),}},
{"0x4",new ColorRange { R = (759, 219), G = (223, 233), B = (226, 236),}},
{"0x5",new ColorRange { R = (750, 210), G = (225, 235), B = (225, 235),}},
{"0x6",new ColorRange { R = (3, 13), G = (119, 129), B = (108, 118),}},
{"1x0",new ColorRange { R = (190, 750), G = (84, 94), B = (231, 241),}},
{"1x1",new ColorRange { R = (250, 260), G = (226, 236), B = (193, 753),}},
{"1x2",new ColorRange { R = (755, 215), G = (218, 228), B = (193, 753),}},
{"1x3",new ColorRange { R = (222, 232), G = (242, 252), B = (236, 246),}},
{"1x4",new ColorRange { R = (11, 21), G = (35, 45), B = (28, 38),}},
{"1x5",new ColorRange { R = (69, 79), G = (103, 113), B = (110, 175),}},
{"1x6",new ColorRange { R = (83, 93), G = (96, 106), B = (162, 172),}},
{"2x0",new ColorRange { R = (3, 13), G = (118, 128), B = (104, 114),}},
{"2x1",new ColorRange { R = (184, 194), G = (183, 193), B = (47, 57),}},
{"2x2",new ColorRange { R = (144, 154), G = (31, 41), B = (78, 88),}},
{"2x3",new ColorRange { R = (21, 31), G = (102, 112), B = (72, 82),}},
{"2x4",new ColorRange { R = (54, 64), G = (98, 108), B = (134, 144),}},
{"2x5",new ColorRange { R = (70, 80), G = (75, 75), B = (110, 175),}},
{"2x6",new ColorRange { R = (68, 78), G = (108, 118), B = (144, 154),}},
{"3x0",new ColorRange { R = (94, 104), G = (72, 82), B = (79, 89),}},
{"3x1",new ColorRange { R = (74, 84), G = (146, 156), B = (125, 135),}},
{"3x2",new ColorRange { R = (181, 191), G = (148, 158), B = (197, 757),}},
{"3x3",new ColorRange { R = (176, 186), G = (93, 103), B = (757, 217),}},
{"3x4",new ColorRange { R = (125, 135), G = (148, 158), B = (154, 164),}},
{"3x5",new ColorRange { R = (160, 170), G = (173, 183), B = (140, 150),}},
{"3x6",new ColorRange { R = (170, 180), G = (199, 759), B = (142, 152),}},

        };

        //private readonly InputSimulator m_sim = new InputSimulator();
        private bool IsMenuOpen()
        {
            return CheckPixel(GetPixelColor(Positions.ClosePosition), Colors.ExitButton2);
        }
        public override void Execute()
        {

            if(!IsMenuOpen())
            {
                Console.WriteLine("Crafting menu closed !");
                Toggle();
                return;
            }
            
            Sellbot.Instance.ScanBagOnce( async (Vector2 pos) =>
            {
                if (!IsMenuOpen())
                {
                    Toggle();
                    return;
                }
                await ClickAtAsync(Positions.PotionPosition, 75, Utils.ClickType.LEFT);
                await ClickAtAsync(pos, 75, Utils.ClickType.RIGHT);
                await ClickAtAsync(Positions.MaxBtnPosition, 75, Utils.ClickType.LEFT);
                await ClickAtAsync(Positions.MinusBtnPosition, 75, Utils.ClickType.LEFT);
                await ClickAtAsync(Positions.CombinePosition, 75, Utils.ClickType.LEFT);

                await Task.Delay(2000);

            }, GemColors);
        }
    }
}
