using mMacro.Core.Managers;
using mMacro.Core.Models;
using mMacro.Core.Utils;
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
    public class ReviveBot : SingletonMacroFunction<ReviveBot>
    {
        private AppConfig m_config;
        private List<bool> m_reviveOffered  = new List<bool> { false, false, false, false, false};
        public List<bool> m_blockOffers     = new List<bool> { false, false, false, false, false };

        public Vector2 FirstPlayerPos = Vector2.Zero;
        public float Radius = 21f;
        public int Offset = 67;
        public ColorRange ReviveColor = new ColorRange { R = (47, 57), G = (56, 66), B = (50, 60) };

        public ReviveBot() : base("Revive Bot", Keys.None, ActivationMode.MenuOnly, ExecutionType.Toggleable)
        {
            m_config = ConfigManager.Load();
            FirstPlayerPos = m_config.FirstPlayerPos;
        }

        public void ClearOfferedRevives()
        {
            for (int i = 1; i<5; i++) m_reviveOffered[i] = false;
        }

        private async void SendRevive(int playerNum)
        {
            float x = FirstPlayerPos.X;
            float y = FirstPlayerPos.Y + Offset * playerNum;

            m_reviveOffered[playerNum] = true;
            Point oldPos = Cursor.Position;

            await Click.ClickAtAsync(new Vector2(x, y), 90, ClickType.LEFT);

            Cursor.Position = oldPos;
        }
        public override void Execute()
        {
            for (int i = 1; i<5; i++)
            {
                float x = FirstPlayerPos.X;
                float y = FirstPlayerPos.Y + Offset * i;

                bool needRevive = CheckPixel(GetPixelColor(x, y), ReviveColor);

                if (needRevive && !m_reviveOffered[i] && !m_blockOffers[i])
                {
                    SendRevive(i);
                }
                else if (!needRevive && m_reviveOffered[i])
                {
                    Console.WriteLine($"Player {i} No longer need revive !");
                    m_reviveOffered[i] = false;
                }
            }

            Task.Delay(500);
        }
    }
}
