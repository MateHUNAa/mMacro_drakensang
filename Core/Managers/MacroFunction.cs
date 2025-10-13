using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mMacro.Core.Managers
{
    [Flags]
    public enum ActivationMode
    {
        KeybindOnly,
        MenuOnly,
        Both
    }
    [Flags]
    public enum ExecutionType
    {
        Toggleable, // Stays active until toggled off
        RunOnce // executes once per trigger
    }
    public abstract class MacroFunction
    {
        public string Name { get; }
        public Keys Defaultkey { get; set; } = Keys.None;
        public ActivationMode Mode { get; }
        public ExecutionType ExecutionType { get; set; } = ExecutionType.RunOnce;

        private bool m_enabled = false;
        protected MacroFunction(string name, Keys defaultKey, ActivationMode mode = ActivationMode.Both, ExecutionType executionType = ExecutionType.RunOnce)
        {
            Name = name;
            Defaultkey = defaultKey;
            Mode = mode;
            ExecutionType = executionType;

            KeybindManager.Instance.Register(Name, Defaultkey, Execute, KeyModifiers.None);
            Init();
        }
        public virtual void Init()
        {
            FunctionManager.Instance.Register(this);
        }
        public bool Enabled
        {
            get => m_enabled;
            private set => m_enabled = value;
        }
        
        public void Toggle()
        {
            if (ExecutionType != ExecutionType.Toggleable) return;

            Enabled =!Enabled;
            Console.WriteLine($"[{Name}] {(Enabled ? "Enabled" : "Disabled")}");
        }
        private void OnKeyPressed()
        {
            if (ExecutionType == ExecutionType.Toggleable) Toggle();
            if (ExecutionType == ExecutionType.RunOnce) Execute();
        }
        public void ExecuteIfEnabled()
        {
            if(ExecutionType == ExecutionType.Toggleable && Enabled) Execute(); 
        }
        public abstract void Execute();
    }
}
