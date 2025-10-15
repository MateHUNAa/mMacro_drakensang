using mMacro.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mMacro.Core.Functions
{
    public class AutoPotion : SingletonMacroFunction<AutoPotion>
    {
        public AutoPotion() : base("Auto Potion", Keys.None, ActivationMode.MenuOnly, ExecutionType.Toggleable)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("This feature is not completed yet!");
        }
    }
}
