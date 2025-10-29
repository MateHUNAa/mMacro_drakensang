using Core.Attributes;
using Core.Attributes.Interface;
using mMacro.Core.Managers;
using System.Numerics;

namespace mMacro.Core.Functions
{
    public class AutoPotion : SingletonMacroFunction<AutoPotion>
    {
        public AutoPotion() : base("Auto Potion", Keys.None, ActivationMode.MenuOnly, ExecutionType.Toggleable)
        {
        }

        [DragButton("Drag over the health bar RECT", shape: ShapeType.Rectangular)]
        private void SetHealthBar(Vector2 start, Vector2 end)
        {
            Console.WriteLine($"{start} {end}");
        }
        [DragButton("Drag over the health bar CIRCLE", shape: ShapeType.Circle)]
        private void SetHealthBarC(Vector2 start, Vector2 end)
        {
            Console.WriteLine($"{start} {end}");
        }
        [DragButton("Drag over the health bar SQAURE", shape: ShapeType.Square)]
        private void SetHealthBarS(Vector2 start, Vector2 end)
        {
            Console.WriteLine($"{start} {end}");
        }

        public override void Execute()
        {
            Console.WriteLine("This feature is not completed yet!");
        }
    }
}
