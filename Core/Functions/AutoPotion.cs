using mMacro.Core.Managers;

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
