using System.Numerics;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
    {
        public string Label { get; }
        public float Width { get; }
        public float Height { get; }
        public bool Inline { get; }
        public int Columns { get; }

        public ButtonAttribute(string label = "Label not set!", float width = 0, float height = 0, bool inline = true, int columns = 2)
        {
            Label = label;
            Width = width;
            Height = height;
            Inline=inline;
            Columns=columns;
        }
        public Vector2 Size => new Vector2(Width, Height);
    }
}
