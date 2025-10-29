using System.Numerics;

namespace Core.Attributes.Interface
{
    public interface IButtonTemplate
    {
        public string Label { get; }
        public float Width { get; }
        public float Height { get; }
        public bool Inline { get; }
        public int Columns { get; }

        public Vector2 Size => new Vector2(Width, Height);
    }
}
