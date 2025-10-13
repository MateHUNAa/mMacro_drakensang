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
        }
        public void Register(MacroFunction function)
        {
            Functions.Add(function);
            Console.WriteLine($"{function.Name} Has been registered!");
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
