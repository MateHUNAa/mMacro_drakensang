using mMacro.Core.Managers;
using mMacro.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using static mMacro.Core.Utils.PixelUtils;
namespace mMacro.Core.Functions
{
    public class ReviveBot : MacroFunction
    {
        private static readonly Lazy<ReviveBot> m_instance = new Lazy<ReviveBot>(() => new ReviveBot());
        public static ReviveBot Instance = m_instance.Value;
        private AppConfig m_config;
        private InputSimulator m_sim = new InputSimulator();
        private List<bool> m_reviveOffered = new List<bool> { false, false, false, false, false};
        public List<bool> m_blockOffers = new List<bool> { false, false, false, false, false };

        public Vector2 FirstPlayerPos = Vector2.Zero;
        public float Radius = 21f;
        public int Offset = 67;
        public Color ReviveColor = Color.FromArgb(255, 97, 112, 101);
        public ReviveBot() : base("Revive Bot", Keys.None, ActivationMode.MenuOnly, ExecutionType.Toggleable)
        {
            m_config = ConfigManager.Load();
            FirstPlayerPos = m_config.FirstPlayerPos;
        }

        public void ClearOfferedRevives()
        {
            for (int i = 1; i<5; i++) m_reviveOffered[i] = false;
        }
        private void SendRevive()
        {
            for (int i = 1; i<5; i++)
            {
                float x = FirstPlayerPos.X;
                float y = FirstPlayerPos.Y + Offset * i;

                bool needRevive = CheckPixel(GetPixelColor(x, y), ReviveColor);

                if (!m_reviveOffered[i]) m_reviveOffered[i] = false;

                if (needRevive && !m_reviveOffered[i] && !m_blockOffers[i])
                {
                    m_reviveOffered[i] = true;
                    Point oldPos = Cursor.Position;

                    Cursor.Position = new Point((int)x, (int)y);
                    Task.Delay(50).Wait();
                    m_sim.Mouse.LeftButtonClick();

                    Cursor.Position = oldPos;
                }
                else if (!needRevive && m_reviveOffered[i])
                {
                    m_reviveOffered[i] = false;
                }
            }
        }
        public override void Execute()
        {
            SendRevive();
            Task.Delay(500);
        }
    }
}
