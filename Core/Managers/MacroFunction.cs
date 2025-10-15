using System;
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
        RunOnce // Executes once per trigger
    }

    // Base class without singleton
    public abstract class MacroFunction
    {
        public string Name { get; }
        public Keys Defaultkey { get; set; } = Keys.None;
        public ActivationMode Mode { get; }
        public ExecutionType ExecutionType { get; set; } = ExecutionType.RunOnce;

        private bool m_enabled = false;

        protected MacroFunction(string name, Keys defaultKey, ActivationMode mode = ActivationMode.MenuOnly, ExecutionType executionType = ExecutionType.RunOnce)
        {
            Name = name;
            Defaultkey = defaultKey;
            Mode = mode;
            ExecutionType = executionType;

            if ((Mode.HasFlag(ActivationMode.KeybindOnly) || Mode.HasFlag(ActivationMode.Both))
                && !Mode.HasFlag(ActivationMode.MenuOnly))
            {
                KeybindManager.Instance.Register(Name, Defaultkey, OnKeyPressed, KeyModifiers.None);
            }

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

            Enabled = !Enabled;
            Console.WriteLine($"[{Name}] {(Enabled ? "Enabled" : "Disabled")}");
        }

        private void OnKeyPressed()
        {
            if (ExecutionType == ExecutionType.Toggleable) Toggle();
            if (ExecutionType == ExecutionType.RunOnce) Execute();
        }

        public void ExecuteIfEnabled()
        {
            if (ExecutionType == ExecutionType.Toggleable && Enabled) Execute();
        }

        public abstract void Execute();
    }

    // Optional singleton wrapper
    public abstract class SingletonMacroFunction<T> : MacroFunction
        where T : SingletonMacroFunction<T>, new()
    {
        private static readonly Lazy<T> m_instance = new Lazy<T>(() => new T());
        public static T Instance => m_instance.Value;

        protected SingletonMacroFunction(string name = "", Keys defaultKey = Keys.None,
            ActivationMode mode = ActivationMode.MenuOnly,
            ExecutionType executionType = ExecutionType.RunOnce)
            : base(name, defaultKey, mode, executionType)
        {
        }
    }
}
