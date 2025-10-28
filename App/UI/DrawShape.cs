using ImGuiNET;
using System.Numerics;

namespace App.UI
{
    public static class DrawShape
    {
        #region DrawShape
        public static void DrawCursorSquare(Vector2 pos, int CellSize, Vector4 Color)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddRect(new Vector2(pos.X - CellSize/2, pos.Y - CellSize/2),
                             new Vector2(pos.X + CellSize/2, pos.Y + CellSize/2),
                             ImGui.ColorConvertFloat4ToU32(Color)
            );
        }
        public static void DrawCursorSquare(Vector2 pos, Vector2 CellSize, Vector4 Color)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddRect(new Vector2(pos.X - CellSize.X/2, pos.Y - CellSize.Y/2),
                             new Vector2(pos.X + CellSize.X/2, pos.Y + CellSize.Y/2),
                             ImGui.ColorConvertFloat4ToU32(Color)
            );
        }
        public static void DrawCursorSquare(Vector2 pos, Vector2 CellSize, Color color) => DrawCursorSquare(pos, CellSize, new Vector4(color.R, color.G, color.B, color.A));
        public static void DrawCursorSquare(Vector2 pos, int CellSize, Color color) => DrawCursorSquare(pos, CellSize, new Vector4(color.R, color.G, color.B, color.A));

        public static void DrawDragRect(Vector2 start, Vector2 end)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddRect(start, end, ImGui.ColorConvertFloat4ToU32(new Vector4(0, 1, 0, 1)));
        }
        public static void DrawCursorCircle(Vector2 pos, float radius, Vector4 color, int segments = 32)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddCircle(
                pos,
                radius,
                ImGui.ColorConvertFloat4ToU32(color),
                segments,
                1.0f
            );
        }
        #endregion
    }
}