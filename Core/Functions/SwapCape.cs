using mMacro.Core.Managers;
using mMacro.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using mMacro.Core.Utils;
using static mMacro.Core.Utils.PixelUtils;
using WindowsInput;
using WindowsInput.Native;
using System.Runtime.InteropServices.Marshalling;

namespace mMacro.Core.Functions
{
    public class SwapCape : MacroFunction
    {
        #region Variables
        private static readonly Lazy<SwapCape> m_instance = new Lazy<SwapCape>(() => new SwapCape());
        public static SwapCape Instance = m_instance.Value;

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

        public override void Execute()
        {
            Color color = GetPixelColor(InventoryCloseButtonPosition);
            bool originalState = CheckPixel(color, ExitButtonColor);
            
            if (!originalState)
            {
                m_sim.Keyboard.KeyPress(OpenInventory);
            }

            Vector2 bagPos = GridHelper.GetBagScreenPosition(bag, m_config.FirstBagPosition);
            Cursor.Position = new Point((int)bagPos.X, (int)bagPos.Y);
            Task.Delay(100).Wait();
            m_sim.Mouse.LeftButtonClick();
            Task.Delay(100).Wait();


            PointF pos =  GridHelper.GetCellScreenPosition(col, row, (PointF)m_config.FirstCellPosition);
            Cursor.Position = new Point((int)pos.X, (int)pos.Y);
            m_sim.Mouse.LeftButtonDoubleClick();

            Task.Delay(50).Wait();
            m_sim.Keyboard.KeyPress(RideMount);
            Task.Delay(50).Wait();
            Cursor.Position = new Point((int)pos.X, (int)pos.Y);
            m_sim.Mouse.LeftButtonDoubleClick();

        }
    }
}
