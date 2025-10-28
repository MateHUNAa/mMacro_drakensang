using mMacro.Core.Models;
using System;
using System.Windows.Forms;
using ImGuiNET;
using System.Reflection;
using Core.Attributes;
using System.Numerics;

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

        private AppConfig m_conifg;

        protected MacroFunction(string name, Keys defaultKey, ActivationMode mode = ActivationMode.MenuOnly, ExecutionType executionType = ExecutionType.RunOnce)
        {
            m_conifg = ConfigManager.Load();
            Name = name;
            Defaultkey = defaultKey;
            Mode = mode;
            ExecutionType = executionType;


            if (m_conifg.Keybinds.ContainsKey(Name))
                Defaultkey = m_conifg.Keybinds[Name].Key;

            if (
                    (Mode.HasFlag(ActivationMode.KeybindOnly) || 
                    Mode.HasFlag(ActivationMode.Both)) && 
                    !Mode.HasFlag(ActivationMode.MenuOnly)
                )
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


        /// <summary>
        /// Draws UI for activation
        /// </summary>
        public void DrawActivation()
        {
            switch (ExecutionType)
            {
                case ExecutionType.Toggleable:
                    DrawToggle();
                    break;
                case ExecutionType.RunOnce:
                    DrawExecute();
                    break;
            }
        }

        public void DrawSetup()
        {
            DrawCustomButtons();
        }
        public void DrawCustomButtons()
        {
            var methods = GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                .Where(m => m.GetCustomAttribute<ButtonAttribute>() != null).ToArray();

            if (methods.Length == 0) return;

            int i = 0;
            float rowMargin = 8f;

            while (i < methods.Length)
            {
                var method = methods[i];
                var attr = method.GetCustomAttribute<ButtonAttribute>();
                int columns = Math.Max(1, attr.Columns); // default columns
                int buttonsLeft = methods.Length - i;

                bool isLastRow = buttonsLeft < columns;
                int buttonsInRow = isLastRow ? buttonsLeft : columns;

                // Compute total width for the row
                float totalWidth = ImGui.GetContentRegionAvail().X;
                if (!isLastRow)
                    totalWidth -= rowMargin; // only subtract margin for full rows

                float buttonWidth = totalWidth / buttonsInRow;

                for (int j = 0; j < buttonsInRow; j++)
                {
                    var m = methods[i + j];
                    var a = m.GetCustomAttribute<ButtonAttribute>();
                    string label = a.Label ?? m.Name;

                    Vector2 size = a.Inline ? new Vector2(buttonWidth, a.Height) : a.Size;

                    if (ImGui.Button(label, size))
                    {
                        m.Invoke(this, null);
                    }

                    // SameLine for all buttons except the last in the row
                    if (j < buttonsInRow - 1)
                        ImGui.SameLine();
                }

                // Only add row margin for full rows
                if (!isLastRow)
                {
                    ImGui.SameLine();
                    ImGui.Dummy(new Vector2(rowMargin, 0));
                }

                i += buttonsInRow;
            }
        }

        /// <summary>
        /// Draws toggle button ( for toggleable )
        /// </summary>
        protected virtual void DrawToggle()
        {
            if (ImGui.Button(Enabled ? $"Disable {Name}" : $"Enable {Name}", new Vector2(ImGui.GetContentRegionAvail().X,0)))
                Toggle();
        }

        /// <summary>
        /// Draw execute button ( for run once )
        /// </summary>
        protected virtual void DrawExecute()
        {
            if (ImGui.Button($"Execute {Name}", new Vector2(ImGui.GetContentRegionAvail().X, 0)))
                Execute();
        }

    }

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
