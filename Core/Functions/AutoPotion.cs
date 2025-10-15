using mMacro.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mMacro.Core.Functions
{
    public class AutoPotion : MacroFunction
    {
        private static readonly Lazy<AutoPotion> m_instance = new Lazy<AutoPotion>(() => new AutoPotion());
        public static AutoPotion Instance => m_instance.Value;
        public AutoPotion() : base("Auto Potion", Keys.None, ActivationMode.MenuOnly, ExecutionType.Toggleable)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("This feature is not completed yet!");
        }
    }
}
