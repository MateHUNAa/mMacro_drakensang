using mMacro.Core.Managers;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System.Numerics;
using WindowsInput;
using WindowsInput.Native;
using static mMacro.Core.Utils.Click;
using static mMacro.Core.Utils.PixelUtils;

namespace mMacro.Core.Functions
{
    public class SwapCape : SingletonMacroFunction<SwapCape>
    {
        #region Variables
        private Color ExitButtonColor = Color.FromArgb(255, 255, 255 ,255);
        private InputSimulator m_sim = new InputSimulator();
        public Vector2 InventoryCloseButtonPosition = Vector2.Zero;
        public int CloseButtonScale = 22;


        public int bag = 0;
        public int col = 0;
        public int row = 1;
        public VirtualKeyCode OpenInventory = VirtualKeyCode.VK_I;
        public VirtualKeyCode RideMount     = VirtualKeyCode.VK_O;
        private AppConfig m_config;

        private int ClickDelay => Settings.Instance.m_Timeings.SwapCapeClickDelay;
        #endregion

        public SwapCape() : base("Cape Swap", Keys.D0, ActivationMode.KeybindOnly, ExecutionType.RunOnce)
        {
            m_config = ConfigManager.Load();
            InventoryCloseButtonPosition = m_config.ClosePoint;
            bag = m_config.swapBag;
            col = m_config.swapCol;
            row = m_config.swapRow;
            // TODO: Save Keys to cfg;
        }

        public override async void Execute()
        {
            Point ogPos = Cursor.Position;
            Color color = GetPixelColor(InventoryCloseButtonPosition);
            bool originalState = CheckPixel(color, ExitButtonColor);
            
            if (!originalState)
            {
                m_sim.Keyboard.KeyPress(OpenInventory);
                await Task.Delay(ClickDelay);
            }

            Vector2 bagPos = GridHelper.GetBagScreenPosition(bag, m_config.FirstBagPosition);
            await ClickAtAsync(new Point((int)bagPos.X, (int)bagPos.Y), ClickDelay);


            PointF pos =  GridHelper.GetCellScreenPosition(col, row, (PointF)m_config.FirstCellPosition);
            Point _pos = new Point((int)pos.X, (int)pos.Y);

            await ClickAtAsync(_pos, ClickDelay, ClickType.LEFT_DOUBLE);
            m_sim.Keyboard.KeyPress(RideMount);
            await ClickAtAsync(_pos, ClickDelay, ClickType.LEFT_DOUBLE);

            await ClickAtAsync(InventoryCloseButtonPosition, ClickDelay, ClickType.LEFT);

            Cursor.Position= ogPos;
        }
    }
}
