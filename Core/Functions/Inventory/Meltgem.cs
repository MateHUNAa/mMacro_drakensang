using mMacro.Core.Managers;
using mMacro.Core.Models;
using System.Numerics;
using static mMacro.Core.Models.UIPositions;

namespace mMacro.Core.Functions.Inventory
{
    public class Meltgem : SingletonMacroFunction<Meltgem>
    {
        private AppConfig m_config = ConfigManager.Load();
        public CraftPositions Positions { get; set; }

        public Meltgem() : base("Melt Gem", Keys.None, ActivationMode.Both, ExecutionType.Toggleable) 
        { 
            Positions  = m_config.CraftPositions;
        }


        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
