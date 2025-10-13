using mMacro.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace mMacro.Core.Functions
{
    public class AutoClicker
    {
        private readonly InputSimulator m_input = new();
        private readonly int m_clickDelay = 100; //ms

        public LeftClicker Left { get; }
        public RightClicker Right { get; }
        public AutoClicker()
        {
            Left = new LeftClicker(m_input, m_clickDelay);
            Right = new RightClicker(m_input, m_clickDelay);
        }

        public void ExecuteEnabled()
        {
            if (Left.Enabled)
                Left.Execute();

            if (Right.Enabled)
                Right.Execute();
        }

        public abstract class Clicker
        {
            public String Name { get; }
            public Keys Keybind { get; }
            public bool Enabled { get; private set; } = false;
            protected readonly InputSimulator Input;
            protected readonly int Delay;

            protected Clicker(string name, Keys keybind, InputSimulator input, int delay)
            {
                Name = name;
                Keybind = keybind;
                Input = input;
                Delay = delay;
            }

            public void Toggle()
            {
                Enabled = !Enabled;
                Console.WriteLine($"[{Name}] {(Enabled ? "Enabled" : "Disabled")}");
            }
            public abstract void Execute();
        }

        public class LeftClicker : Clicker
        {
            public LeftClicker(InputSimulator input, int delay) : base("Left AutoClicker", Keys.F6, input, delay) { }

            public override void Execute()
            {
                Input.Mouse.LeftButtonClick();
                Thread.Sleep(Delay);
            }
        }
        public class RightClicker : Clicker
        {
            public RightClicker(InputSimulator input, int delay) : base("Right AutoClicker", Keys.F7, input, delay) { }

            public override void Execute()
            {
                Input.Mouse.RightButtonClick();
                Thread.Sleep(Delay);
            }
        }
    }
}
