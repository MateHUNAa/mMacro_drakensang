using System.Numerics;

namespace mMacro.Core.Models
{
    public  class UIPositions
    {
        public class CraftPositions
        {
            public Vector2 ClosePosition { get; set; } = Vector2.Zero;
            public Vector2 CombinePosition { get; set; } = Vector2.Zero;
            public Vector2 PotionPosition { get; set; } = Vector2.Zero;
            public Vector2 MinusBtnPosition {  get; set; } = Vector2.Zero;
            public Vector2 MaxBtnPosition { get; set; } = Vector2.Zero;
        }

        public class AutoPoitionPositions
        {
            public Vector2 HealthBarStart { get; set; } = Vector2.Zero;
            public Vector2 HealthBarEnd { get; set; } = Vector2.Zero;
        }


        public CraftPositions Crafting {  get; set; } = new CraftPositions();
        public AutoPoitionPositions AutoPotion { get; set; } = new AutoPoitionPositions();

        public static UIPositions Instance { get; } = new UIPositions();
    }
}
