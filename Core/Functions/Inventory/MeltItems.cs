using mMacro.Core.Managers;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System.Numerics;
using static mMacro.Core.Utils.Click;


namespace mMacro.Core.Functions.Inventory
{
    public class MeltItems : SingletonMacroFunction<MeltItems>
    {
        public readonly int CellSize = 75;
        public readonly Vector2 MeltButtonSize = new Vector2(180, 40);
        public Vector2 MeltCloseBtn = Vector2.Zero;
        public Vector2 MeltFirstPos = Vector2.Zero;
        public Vector2 MeltButtonPos = Vector2.Zero;

        private PixelUtils.ColorRange MeltBackground = new PixelUtils.ColorRange
        {
            R = (22, 44),
            G = (39, 79),  
            B = (49, 82),
        };

        private AppConfig m_config;
        public MeltItems() : base("Melt Items", Keys.None, ActivationMode.Both, ExecutionType.RunOnce)
        {
            m_config = ConfigManager.Load();
            MeltFirstPos = m_config.MeltFirstPos;
            MeltButtonPos = m_config.MeltButtonPos;
            MeltCloseBtn = m_config.MeltCloseBtn;
        }

        private bool IsMeltMenuOpen()
        {
            return PixelUtils.CheckPixel(PixelUtils.GetPixelColor(MeltCloseBtn), Color.FromArgb(255, 250, 250, 250));
        }
        private bool HaveItemOn(int row, int col)
        {
            float X = MeltFirstPos.X +  CellSize * row + GridHelper.GetOffset(row);
            float Y = MeltFirstPos.Y +  CellSize * col + GridHelper.GetOffset(col);

            Color color = PixelUtils.GetPixelColor(X, Y);
            bool isBackground = PixelUtils.CheckPixel(color, MeltBackground);

            return !isBackground;
        }

        private bool HaveEmptySlot()
        {
            for (int row = 0; row<3; row++)
            {
                for (int col = 0; col<3; col++)
                {
                    float X = MeltFirstPos.X +  CellSize * row + GridHelper.GetOffset(row);
                    float Y = MeltFirstPos.Y +  CellSize * col + GridHelper.GetOffset(col);

                    if (!HaveItemOn(row, col)) return true;
                }
            }

            return false;
        }
        public override async void Execute()
        {
            if (!IsMeltMenuOpen())
            {
                Toggle();
                return;
            }

            if (!HaveEmptySlot())
            {
                ClickAt(MeltButtonPos);
                await Task.Delay(200);
            }

            for (int bag = 0; bag < Sellbot.Instance.BagCount; bag++)
            {
                if (!IsMeltMenuOpen()) return;

                await ClickAtAsync(Sellbot.Instance.GetBagPosition(bag), 100);

                for (int itr = 0; itr < Sellbot.Instance.scanLevel; itr++)
                {
                    if (!IsMeltMenuOpen()) return;

                    var itemsToMelt = Sellbot.Instance.ScanBagMultiThreaded(bag);
                    if (itemsToMelt.Count <= 0)
                    {
                        continue;
                    }

                    int counter = 0;
                    foreach (Vector2 pos in itemsToMelt)
                    {
                        if (!IsMeltMenuOpen()) return;

                        await ClickAtAsync(pos, Settings.Instance.m_Timeings.MeltItemClickDelay, ClickType.RIGHT);
                        counter++;

                        if (counter >= itemsToMelt.Count)
                        {
                            Console.WriteLine("Counter bigger than items in inventory");
                            await ClickAtAsync(MeltButtonPos, Settings.Instance.m_Timeings.MeltItemClickDelay);
                        }

                        if (!HaveEmptySlot())
                        {
                            Console.WriteLine("Melter full");
                            await ClickAtAsync(MeltButtonPos, Settings.Instance.m_Timeings.MeltItemClickDelay);
                        }

                        await Task.Delay(50);
                    }
                }
            }
        }

    }
}
