using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mMacro.Core.Managers
{
    public class FunctionManager
    {
        public List<MacroFunction> Functions { get; } = new List<MacroFunction>();

        public void Register(MacroFunction function)
        {
            Functions.Add(function);
        }

        public void ExecuteEnabled()
        {
            foreach (var func in Functions)
            {
                if (func.Enabled)
                    func.Execute();
            }
        }
    }
}
