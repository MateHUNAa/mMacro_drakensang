using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace mMacro.Core.Managers
{
    public class FunctionManager
    {
        private static FunctionManager m_instance;
        public static FunctionManager Instance => m_instance ?? throw new InvalidOperationException("FunctionManager not initialized !");
        public List<MacroFunction> Functions { get; } = new List<MacroFunction>();
        public FunctionManager() {
            if (m_instance != null)
                throw new InvalidOperationException("FunctionManager already initialized!");

            m_instance= this;

            if (KeybindManager.Instance != null)
                KeybindManager.Instance.OnKeybindChanged += OnKeybindChanged;
            else
                Console.WriteLine("[FunctionManager] Warning: KeybindManager not initialized yet!");
        }

        private void OnKeybindChanged(string keybindName, Keybind newKeybind)
        {
            var func = Functions.Find(f => f.Name == keybindName);
            if (func == null)
            {
                Console.WriteLine($"[FunctionManager] Warning: Function '{keybindName}' not found for keybind update.");
                return;
            }

            func.Defaultkey = newKeybind.Key;
            Console.WriteLine($"[FunctionManager] Updated keybind for '{keybindName}' → {newKeybind.Key}");
        }

        public void Register(MacroFunction function)
        {
            if (Functions.Any(f => f.Name == function.Name))
            {
                Console.WriteLine($"[FunctionManager] Function '{function.Name}' already registered. Skipping.");
                return;
            }

            Functions.Add(function);
            Console.WriteLine($"[FunctionManager] '{function.Name}' has been registered!");
        }
        public void ExecuteEnabled()
        {
            foreach (var func in Functions)
            {
                if (func.Enabled)
                    func.ExecuteIfEnabled();
            }
        }
    }
}
