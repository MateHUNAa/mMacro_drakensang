using mMacro.Core.Managers;

namespace mMacro.Core.Functions.Inventory
{
    public class Meltgem : SingletonMacroFunction<Meltgem>
    {
        public Meltgem() : base("Melt Gem", Keys.None, ActivationMode.Both, ExecutionType.Toggleable) { }



        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
