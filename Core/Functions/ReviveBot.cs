using mMacro.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mMacro.Core.Functions
{
    public class ReviveBot : MacroFunction
    {
        public ReviveBot() : base("Revive Bot", Keys.None, ActivationMode.MenuOnly, ExecutionType.Toggleable)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
