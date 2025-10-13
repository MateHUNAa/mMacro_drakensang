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
        private static AutoClicker m_instance;
        private readonly InputSimulator m_input = new();
        private readonly int m_clickDelay = 100; //ms

        public LeftClicker Left { get; }
        public RightClicker Right { get; }
        public AutoClicker()
        {
            if (m_instance != null) throw new InvalidOperationException("AutoClicker already have been initialized!");

            Left = new LeftClicker(m_input, m_clickDelay);
            Right = new RightClicker(m_input, m_clickDelay);

            m_instance = this;
        }
        public abstract class Clicker : MacroFunction
        {
            public bool Enabled { get; private set; } = false;
            protected readonly InputSimulator Input;
            protected readonly int Delay;

            protected Clicker(string name, Keys keybind, InputSimulator input, int delay) : base(name, keybind, ActivationMode.Both, ExecutionType.Toggleable)
            {
                Input = input;
                Delay = delay;
            }

            public void Toggle()
            {
                Enabled = !Enabled;
                Console.WriteLine($"[{Name}] {(Enabled ? "Enabled" : "Disabled")}");
            }
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
