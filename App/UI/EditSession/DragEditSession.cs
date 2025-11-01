using Core.Attributes.Interface;
using ImGuiNET;
using System.Numerics;

namespace App.UI.EditSession
{
    public class DragEditSession : BaseEditSession
    {
        public Vector2 StartPos { get; private set; }
        public Vector2 EndPos { get; private set; }
        public ShapeType Shape { get; private set; }
        public Action<Vector2, Vector2> OnDragComplete { get; private set; }

        private bool m_isDragging = false;
        private float m_circleRadius;
        private bool m_isMenuHovered = false;
        private bool m_drawLock = false;

        public DragEditSession(ShapeType shape, Action<Vector2, Vector2> onDragComplete, float circleRadius = 10f)
        {
            Shape = shape;
            OnDragComplete=onDragComplete;
            m_circleRadius = circleRadius;
        }

        public override void Draw(Vector2 mousePos)
        {
            if (!m_isDragging) return;

            var drawList = ImGui.GetWindowDrawList();

            Vector2 topLeft = new Vector2(MathF.Min(StartPos.X, EndPos.X), MathF.Min(StartPos.Y, EndPos.Y));
            Vector2 bottomRight = new Vector2(MathF.Max(StartPos.X, EndPos.X), MathF.Max(StartPos.Y, EndPos.Y));
            Vector2 size = bottomRight - topLeft;

            uint color = ImGui.ColorConvertFloat4ToU32(Color);


            switch (Shape)
            {
                case ShapeType.Rectangular:
                case ShapeType.Square:
                    if (Shape == ShapeType.Square)
                    {
                        float minSize = MathF.Min(size.X, size.Y);
                        size = new Vector2(minSize, minSize);
                        bottomRight = topLeft + size;
                    }
                    drawList.AddRect(topLeft, bottomRight, color);
                    break;
                case ShapeType.Circle:
                    drawList.AddCircle(StartPos, m_circleRadius, color);
                    break;
            }

            DrawControlWindow();
        }

        private void DrawControlWindow()
        {

            if (StartPos == Vector2.Zero || EndPos == Vector2.Zero)
            {
                m_isMenuHovered= false;
                return;
            }

            Vector2 windowPos;
            Vector2 windowSize = new Vector2(220, 150);

            switch(Shape)
            {
                case ShapeType.Circle:
                    windowPos = StartPos + new Vector2(m_circleRadius + 10, -m_circleRadius / 2);
                    break;
                case ShapeType.Rectangular:
                case ShapeType.Square:
                    windowPos = EndPos + new Vector2(10, -windowSize.Y + 10);
                    break;
                default:
                    windowPos = EndPos + new Vector2(10, -windowSize.Y + 10);
                    break;
            }

            Vector2 displaySize = ImGui.GetIO().DisplaySize;

            windowPos.X = Math.Clamp(windowPos.X, 0, displaySize.X - windowSize.X);
            windowPos.Y = Math.Clamp(windowPos.Y, 0, displaySize.Y - windowSize.Y);


            ImGui.SetNextWindowPos(windowPos, ImGuiCond.Always);
            ImGui.SetNextWindowSize(windowSize, ImGuiCond.Always);
            ImGui.Begin("Drag Control", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoMove);
            {
                m_isMenuHovered = ImGui.IsWindowHovered(ImGuiHoveredFlags.ChildWindows | ImGuiHoveredFlags.RootWindow) || ImGui.IsAnyItemHovered();

                ImGui.Text($"Shape: {Shape}");
                ImGui.Text($"Start: {StartPos.X:F0}, {StartPos.Y:F0}");
                ImGui.Text($"End: {EndPos.X:F0}, {EndPos.Y:F0}");
                if (Shape == ShapeType.Circle)
                    ImGui.Text($"Radius: {m_circleRadius:F0}");
                else
                {
                    Vector2 size = EndPos - StartPos;
                    ImGui.Text($"Size: {MathF.Abs(size.X):F0} x {MathF.Abs(size.Y):F0}");
                }

                ImGui.Separator();

                Vector4 col = Color;
                if (ImGui.ColorEdit4("Color", ref col))
                {
                    Color = col;
                }

                if (ImGui.Button("Accept"))
                {
                    OnDragComplete?.Invoke(StartPos, EndPos);
                    Stop(); 
                }
                ImGui.SameLine();
                if (ImGui.Button("Restart"))
                {
                    StartPos = Vector2.Zero;
                    EndPos = Vector2.Zero;
                    m_isDragging = false;
                    m_isMenuHovered= false;
                }
                ImGui.SameLine();
                if (ImGui.Button(m_drawLock ? "Unlock;" : "Lock"))
                {
                    m_drawLock = !m_drawLock;
                }

                ImGui.End();
            }
        }

        public void HandleDrag(Vector2 mousePos)
        {
            var io = ImGui.GetIO();

            if (m_isMenuHovered) return;
            if (m_drawLock) return;

            if (!m_isDragging && io.MouseDown[0])
            {
                StartPos = mousePos;
                EndPos = mousePos;
                m_isDragging = true;
            }

            if (m_isDragging)
            {
                if (Shape == ShapeType.Circle)
                {
                    if (io.MouseDown[0] && !io.KeyShift)
                    {
                        StartPos = mousePos;
                    }

                    if ((io.MouseDown[1]) || (io.MouseDown[0] && io.KeyShift))
                    {
                        m_circleRadius = Vector2.Distance(StartPos, mousePos);
                    }
                }
                else if ((Shape == ShapeType.Rectangular || Shape == ShapeType.Square) && io.MouseDown[0])
                {
                    EndPos=  mousePos;
                }
            }
        }
        public override void Start(Vector4? color = null)
        {
            base.Start(color);
            StartPos = Vector2.Zero;
            EndPos = Vector2.Zero;
        }

        public Vector2 GetStartPos() => StartPos;
        public Vector2 GetEndPos() => EndPos;
        public float GetRadius() => Vector2.Distance(StartPos, EndPos);

        public override void OnClick(Vector2 mousePos)
        {
        }
    }
}
