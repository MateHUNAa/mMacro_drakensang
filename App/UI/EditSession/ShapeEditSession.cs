using Core.Attributes.Interface;
using System.Numerics;
using static App.UI.DrawShape;

namespace App.UI.EditSession
{
    public class ShapeEditSession : BaseEditSession
    {
        public ShapeType Shape { get; private set; } = ShapeType.None;
        public Vector2 Size { get; private set; } = new Vector2(10, 10);
        public float Radius { get; private set; } = 10f;
        public Action<Vector2>? OnSet { get; private set; }

        public void Start(
            ShapeType shape,
            Action<Vector2> onSet,
            Vector2? size = null,
            float radius = 10f,
            Vector4? color = null)
        {
            Shape = shape;
            OnSet = onSet;
            Radius = radius;
            if (size.HasValue) Size = size.Value;

            base.Start(color);
        }

        public override void Draw(Vector2 mousePos)
        {
            if (!Active) return;

            switch(Shape)
            {
                case ShapeType.Circle:
                    DrawCursorCircle(mousePos, Radius, Color);
                    break;
                case ShapeType.Square:
                    DrawCursorSquare(mousePos, Size, Color);
                    break;
            }
        }

        public override void OnClick(Vector2 mousePos)
        {
            if (!Active) return;

            OnSet?.Invoke(mousePos);
            Stop();
        }
    }
}
