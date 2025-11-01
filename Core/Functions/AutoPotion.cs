using Core.Attributes;
using Core.Attributes.Interface;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System.Numerics;
using System.Windows.Automation;
using WindowsInput;

namespace mMacro.Core.Functions
{
    public class AutoPotion : SingletonMacroFunction<AutoPotion>
    {
     

        public UIPositions.AutoPoitionPositions m_positions = UIPositions.Instance.AutoPotion;
        private readonly InputSimulator inputSimulator = new InputSimulator();

        public float HealthThreshold = 80f;

        private AppConfig m_config = ConfigManager.Load();

        public AutoPotion() : base("Auto Potion", Keys.None, ActivationMode.MenuOnly, ExecutionType.Toggleable)
        {
            m_positions = m_config.AutoPotionPositions;
        }

        [DragButton("Drag over the health bar RECT", shape: ShapeType.Rectangular)]
        private void SetHealthBar(Vector2 start, Vector2 end)
        {
            m_positions.HealthBarStart = start;
            m_positions.HealthBarEnd = end;
            m_config.AutoPotionPositions = m_positions;

            Console.WriteLine($"HealthBar saved: {m_positions.HealthBarStart} {m_positions.HealthBarEnd}");
            ConfigManager.Save(m_config);
        }

        public override void Execute()
        {

            Vector2 topLeft = new Vector2(MathF.Min(m_positions.HealthBarStart.X, m_positions.HealthBarEnd.X), MathF.Min(m_positions.HealthBarStart.Y, m_positions.HealthBarEnd.Y));
            Vector2 bottomRight = new Vector2(MathF.Max(m_positions.HealthBarStart.X, m_positions.HealthBarEnd.X), MathF.Max(m_positions.HealthBarStart.Y, m_positions.HealthBarEnd.Y));
            Vector2 size = bottomRight - topLeft;
            Vector2 center = topLeft + size / 2f;

            float thresholdPercent = Math.Clamp(HealthThreshold / 100f, 0f, 1f);
            float thresholdX = topLeft.X + size.X * thresholdPercent;

            Color pixelColor = PixelUtils.GetPixelColor(new Vector2(thresholdX, center.Y));
            bool needPotion = PixelUtils.GetPixelMatchPercentage(pixelColor, Color.FromArgb(255, 55, 45, 30)) > 80.0;

            //Console.WriteLine($"{pixelColor}, {needPotion}");

            if (needPotion)
            {
                inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_R);
                Task.Delay(200);
            }
        }
    }
}
