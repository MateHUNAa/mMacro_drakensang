using System.Numerics;

namespace Core.Attributes.Interface
{
    public interface IButtonRenderer
    {
        void Draw(string label, Action onClick, Vector2 size);
    }
}
