using System.Numerics;

namespace Core.Attributes.Interface
{
    [Flags]
    public enum ShapeType
    {
        None,
        Circle,
        Square,
        Rectangular, // Only for DragEditSession
    }
    public interface IButtonTemplate
    {
        public string Label { get; }
        public float Width { get; }
        public float Height { get; }
        public bool Inline { get; }
        public int Columns { get; }
        public ShapeType Shape { get; }

        public Vector2 Size => new Vector2(Width, Height);
    }
}
