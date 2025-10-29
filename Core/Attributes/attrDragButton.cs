using Core.Attributes.Interface;

namespace Core.Attributes
{
    public class DragButtonAttribute : Attribute, IButtonTemplate
    {
        public string Label { get; }

        public float Width { get; }

        public float Height { get; }

        public bool Inline { get; }

        public int Columns { get; }

        public ShapeType Shape { get; }

        public DragButtonAttribute(string label = "Drag button", float width = 0, float height = 0, bool inline = true, int columns = 1, ShapeType shape = ShapeType.Circle)
        {
            Label = label;
            Width = width;
            Height = height;
            Inline = inline;
            Columns = columns;
            Shape = shape;
        }
    }
}
