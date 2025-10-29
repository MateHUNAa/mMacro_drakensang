using Core.Attributes.Interface;
using System.Numerics;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute, IButtonTemplate
    {
        public string Label { get; }
        public float Width { get; }
        public float Height { get; }
        public bool Inline { get; }
        public int Columns { get; }
        public ShapeType Shape { get; }


        public ButtonAttribute(string label = "Label not set!", float width = 0, float height = 0, bool inline = true, int columns = 2, ShapeType shape = ShapeType.None)
        {
            Label = label;
            Width = width;
            Height = height;
            Inline=inline;
            Columns=columns;
            Shape=shape;
        }
        public Vector2 Size => new Vector2(Width, Height);
    }
}
