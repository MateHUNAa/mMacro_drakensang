using mMacro.Core.Managers;
using System.Numerics;

namespace mMacro.Core.Models
{
    public class AppConfig
    {
        public Dictionary<string, Keybind> Keybinds { get; set; } = new();
        public Vector2 FirstCellPosition { get; set; } = Vector2.Zero;
        
        public Vector2 DragStart { get; set; } = Vector2.Zero;
        public Vector2 DragEnd { get; set; } = Vector2.Zero;
    }
}
