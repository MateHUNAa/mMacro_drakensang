using mMacro.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mMacro.Core.Functions
{
    internal class InventoryScan : MacroFunction
    {
        public InventoryScan(string name, Keys defaultKey, ActivationMode mode = ActivationMode.Both, ExecutionType executionType = ExecutionType.RunOnce) : base(name, defaultKey, mode, executionType)
        {
            Init();
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
