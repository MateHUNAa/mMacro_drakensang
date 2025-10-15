using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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


        public CraftPositions Crafting {  get; set; } = new CraftPositions();
        public static UIPositions Instance { get; } = new UIPositions();
    }
}
