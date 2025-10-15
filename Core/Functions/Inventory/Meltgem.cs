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
            { "Polished Onyx", new ColorRange { R = (176, 186), G = (198, 208), B = (209, 219) }},
            { "Polished Diamond", new ColorRange { R = (206, 216), G = (239, 249), B = (250, 260) }},
            { "Polished Rhodolite", new ColorRange { R = (242, 252), G = (176, 186), B = (209, 219) }},
            { "Polished Diamond(A)", new ColorRange { R = (196, 206), G = (204, 214), B = (247, 257) }},
            { "Polished Diamond(I)", new ColorRange { R = (208, 218), G = (239, 249), B = (250, 260) }},
            { "Polished Diamond(F)", new ColorRange { R = (209, 219), G = (230, 240), B = (218, 228) }},
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
                await ClickAtAsync(Positions.PotionPosition, 20, Utils.ClickType.LEFT);
                await ClickAtAsync(pos, 20, Utils.ClickType.RIGHT);
                await ClickAtAsync(Positions.MaxBtnPosition, 20, Utils.ClickType.LEFT);
                await ClickAtAsync(Positions.MinusBtnPosition, 20, Utils.ClickType.LEFT);
                await ClickAtAsync(Positions.CombinePosition, 20, Utils.ClickType.LEFT);

                await Task.Delay(3000);

            }, GemColors);
        }
    }
}
