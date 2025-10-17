using mMacro.Core.Managers;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System.Numerics;
using static mMacro.Core.Models.UIPositions;
using static mMacro.Core.Utils.Click;
using static mMacro.Core.Utils.PixelUtils;

namespace mMacro.Core.Functions.Inventory
{
    public class MeltGems : SingletonMacroFunction<MeltGems>
    {
        private AppConfig m_config = ConfigManager.Load();
        public CraftPositions Positions { get; set; }

        public MeltGems() : base("Melt Gem", Keys.None, ActivationMode.Both, ExecutionType.RunOnce) 
        { 
            Positions  = m_config.CraftPositions;
        }

        Dictionary<string, ColorRange> GemColors = new Dictionary<string, ColorRange>
        {
            {"0x0", new ColorRange { R = (60, 70), G = (51, 61), B = (32, 42),}},
            {"0x1", new ColorRange { R = (36, 46), G = (190, 185), B = (93, 103),}},
            {"0x2", new ColorRange { R = (189, 199), G = (31, 41), B = (138, 148),}},
            {"0x3", new ColorRange { R = (233, 243), G = (233, 243), B = (76, 86),}},
            {"0x4", new ColorRange { R = (84, 94), G = (95, 105), B = (93, 103),}},
            {"0x5", new ColorRange { R = (226, 236), G = (11, 21), B = (11, 21),}},
            {"0x6", new ColorRange { R = (179, 189), G = (98, 108), B = (208, 218),}},
            {"1x0", new ColorRange { R = (59, 69), G = (50, 60), B = (32, 42),}},
            {"1x1", new ColorRange { R = (26, 36), G = (116, 126), B = (90, 85),}},
            {"1x2", new ColorRange { R = (149, 159), G = (31, 41), B = (83, 93),}},
            {"1x3", new ColorRange { R = (187, 197), G = (187, 197), B = (50, 60),}},
            {"1x4", new ColorRange { R = (11, 21), G = (35, 45), B = (28, 38),}},
            {"1x5", new ColorRange { R = (133, 143), G = (4, 14), B = (-5, 5),}},
            {"1x6", new ColorRange { R = (90, 85), G = (16, 26), B = (130, 140),}},
            {"2x0", new ColorRange { R = (60, 70), G = (51, 61), B = (32, 42),}},
            {"2x1", new ColorRange { R = (211, 221), G = (218, 228), B = (195, 205),}},
            {"2x2", new ColorRange { R = (242, 252), G = (178, 188), B = (209, 219),}},
            {"2x3", new ColorRange { R = (250, 260), G = (224, 234), B = (197, 207),}},
            {"2x4", new ColorRange { R = (183, 193), G = (201, 211), B = (216, 226),}},
            {"2x5", new ColorRange { R = (250, 260), G = (74, 84), B = (36, 46),}},
            {"2x6", new ColorRange { R = (191, 201), G = (86, 96), B = (232, 242),}},
            {"3x0", new ColorRange { R = (59, 69), G = (50, 60), B = (32, 42),}},
            {"3x1", new ColorRange { R = (11, 21), G = (64, 74), B = (52, 62),}},
            {"3x2", new ColorRange { R = (84, 94), G = (15, 25), B = (43, 53),}},
            {"3x3", new ColorRange { R = (134, 144), G = (135, 145), B = (27, 37),}},
            {"3x4", new ColorRange { R = (9, 19), G = (16, 26), B = (9, 19),}},
            {"3x5", new ColorRange { R = (118, 128), G = (9, 19), B = (3, 13),}},
            {"3x6", new ColorRange { R = (93, 103), G = (23, 33), B = (149, 159),}},

            {"0x1_2", new ColorRange { R = (165, 190), G = (176, 186), B = (147, 157),}},
            {"0x2_2", new ColorRange { R = (170, 180), G = (199, 209), B = (142, 152),}},
            {"0x3_2", new ColorRange { R = (183, 193), G = (154, 164), B = (199, 209),}},
            {"0x4_2", new ColorRange { R = (184, 194), G = (165, 190), B = (137, 147),}},
            {"0x5_2", new ColorRange { R = (50, 60), G = (235, 245), B = (215, 225),}},
            {"0x6_2", new ColorRange { R = (131, 141), G = (157, 167), B = (163, 173),}},
            {"1x1_2", new ColorRange { R = (71, 81), G = (105, 115), B = (112, 122),}},
            {"1x2_2", new ColorRange { R = (87, 97), G = (151, 161), B = (116, 126),}},
            {"1x3_2", new ColorRange { R = (82, 92), G = (95, 105), B = (161, 171),}},
            {"1x4_2", new ColorRange { R = (102, 112), G = (79, 89), B = (82, 92),}},
            {"1x5_2", new ColorRange { R = (3, 13), G = (125, 135), B = (110, 120),}},
            {"1x6_2", new ColorRange { R = (74, 84), G = (116, 126), B = (154, 164),}},
            {"2x1_2", new ColorRange { R = (197, 207), G = (223, 233), B = (221, 231),}},
            {"2x2_2", new ColorRange { R = (217, 227), G = (242, 252), B = (244, 254),}},
            {"2x3_2", new ColorRange { R = (199, 209), G = (210, 220), B = (242, 252),}},
            {"2x4_2", new ColorRange { R = (210, 220), G = (226, 236), B = (227, 237),}},
            {"2x5_2", new ColorRange { R = (54, 64), G = (234, 244), B = (218, 228),}},
            {"2x6_2", new ColorRange { R = (210, 220), G = (242, 252), B = (250, 260),}},
            {"3x1_2", new ColorRange { R = (44, 54), G = (76, 86), B = (69, 79),}},
            {"3x2_2", new ColorRange { R = (141, 151), G = (151, 161), B = (36, 46),}},
            {"3x3_2", new ColorRange { R = (58, 68), G = (54, 64), B = (113, 123),}},
            {"3x4_2", new ColorRange { R = (58, 68), G = (60, 70), B = (69, 79),}},
            {"3x5_2", new ColorRange { R = (3, 13), G = (117, 127), B = (107, 117),}},
            {"3x6_2", new ColorRange { R = (41, 51), G = (72, 82), B = (91, 101),}},

            {"0x0_3", new ColorRange { R = (145, 155), G = (186, 196), B = (196, 206),}},
            {"0x1_3", new ColorRange { R = (168, 178), G = (194, 204), B = (151, 161),}},
            {"0x2_3", new ColorRange { R = (193, 203), G = (172, 182), B = (151, 161),}},
            {"0x3_3", new ColorRange { R = (140, 150), G = (162, 172), B = (128, 138),}},
            {"0x4_3", new ColorRange { R = (134, 144), G = (154, 164), B = (162, 172),}},
            {"0x5_3", new ColorRange { R = (149, 159), G = (129, 139), B = (184, 194),}},
            {"0x6_3", new ColorRange { R = (78, 88), G = (126, 136), B = (118, 128),}},
            {"1x0_3", new ColorRange { R = (49, 59), G = (97, 107), B = (131, 141),}},
            {"1x1_3", new ColorRange { R = (136, 146), G = (155, 165), B = (52, 62),}},
            {"1x2_3", new ColorRange { R = (110, 120), G = (99, 109), B = (88, 98),}},
            {"1x3_3", new ColorRange { R = (94, 104), G = (100, 110), B = (52, 62),}},
            {"1x4_3", new ColorRange { R = (54, 64), G = (81, 91), B = (92, 102),}},
            {"1x5_3", new ColorRange { R = (71, 81), G = (72, 82), B = (123, 133),}},
            {"1x6_3", new ColorRange { R = (122, 132), G = (28, 38), B = (171, 181),}},
            {"2x0_3", new ColorRange { R = (18, 28), G = (70, 80), B = (59, 69),}},
            {"2x1_3", new ColorRange { R = (11, 21), G = (118, 128), B = (101, 111),}},
            {"2x2_3", new ColorRange { R = (126, 136), G = (-5, 5), B = (-5, 5),}},
            {"2x3_3", new ColorRange { R = (77, 87), G = (14, 24), B = (132, 142),}},
            {"2x4_3", new ColorRange { R = (139, 149), G = (141, 151), B = (27, 37),}},
            {"2x5_3", new ColorRange { R = (18, 28), G = (17, 27), B = (18, 28),}},
            {"2x6_3", new ColorRange { R = (109, 119), G = (23, 33), B = (52, 62),}},
            {"3x0_3", new ColorRange { R = (28, 38), G = (155, 165), B = (85, 95),}},
            {"3x1_3", new ColorRange { R = (74, 84), G = (237, 247), B = (216, 226),}},
            {"3x2_3", new ColorRange { R = (250, 260), G = (44, 54), B = (30, 40),}},
            {"3x3_3", new ColorRange { R = (197, 207), G = (68, 78), B = (250, 260),}},
            {"3x4_3", new ColorRange { R = (216, 226), G = (216, 226), B = (68, 78),}},
            {"3x5_3", new ColorRange { R = (139, 149), G = (145, 155), B = (147, 157),}},
            {"3x6_3", new ColorRange { R = (182, 192), G = (31, 41), B = (125, 135),}},
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

                await ClickAtAsync(Positions.PotionPosition, Settings.Instance.m_Timeings.MeltGemClickDelay, Utils.ClickType.LEFT);
                await ClickAtAsync(pos, Settings.Instance.m_Timeings.MeltItemClickDelay, Utils.ClickType.RIGHT);
                await ClickAtAsync(Positions.MaxBtnPosition, Settings.Instance.m_Timeings.MeltItemClickDelay, Utils.ClickType.LEFT);
                await ClickAtAsync(Positions.MinusBtnPosition, Settings.Instance.m_Timeings.MeltGemClickDelay, Utils.ClickType.LEFT);
                await ClickAtAsync(Positions.CombinePosition, Settings.Instance.m_Timeings.MeltGemClickDelay, Utils.ClickType.LEFT);

                await Task.Delay(Settings.Instance.m_Timeings.MeltGemTaskEndDelay);

            }, GemColors);
        }
    }
}
