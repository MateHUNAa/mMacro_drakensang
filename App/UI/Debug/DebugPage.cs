using ImGuiNET;
using mMacro.Core.Functions.Inventory;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System.Numerics;
using static App.UI.DrawShape;

namespace App.UI.Debug
{
    public static class DebugPage
    {

        private static readonly EditSession editSession    = EditSession.Instance;
        private static readonly Sellbot inventoryScan      = Sellbot.Instance;
        private static readonly ColorPicker colorPicker    = new ColorPicker();

        public static bool m_debugDraw = false;
        public static bool DebugDraw
        {
            get => m_debugDraw;
            set => m_debugDraw = value;
        }
        public static bool m_debugMode = false;
        public static bool DebugMode
        {
            get => m_debugMode;
            set => m_debugMode = value;
        }

        public static void Draw()
        {
            if (ImGui.BeginTabItem("Debug"))
            {

                Vector4 avgColor = new Vector4(0.5f, 1, 1, 1);
                if (ImGui.Button("Copy Item Color"))
                {
                    editSession.StartEditSession(editSession, (pos) =>
                    {
                        Color pickedColor = PixelUtils.GetPixelColor(pos);

                        PixelUtils.ColorRange range = new PixelUtils.ColorRange
                        {
                            R = (pickedColor.R - Settings.Instance.m_Offsets.ColorTolarence, pickedColor.R + Settings.Instance.m_Offsets.ColorTolarence),
                            G = (pickedColor.G - Settings.Instance.m_Offsets.ColorTolarence, pickedColor.G + Settings.Instance.m_Offsets.ColorTolarence),
                            B = (pickedColor.B - Settings.Instance.m_Offsets.ColorTolarence, pickedColor.B + Settings.Instance.m_Offsets.ColorTolarence),
                        };
                        string codeSnippet = $"new ColorRange {{ R = ({range.R.Min}, {range.R.Max}), G = ({range.G.Min}, {range.G.Max}), B = ({range.B.Min}, {range.B.Max}) }};";

                        ImGui.SetClipboardText(codeSnippet);
                    }, EditMode.FirstCell, Color.FromArgb(1, 1, 0, 1), inventoryScan.CellSize);
                }

                colorPicker.Toggle((bool toggled) => {
                    Console.WriteLine("TOGGLE");
                    if (toggled)
                    {
                        editSession.Mode = EditMode.ColorPicker;
                        editSession.Active = true;
                    }
                    else
                    {
                        editSession.Mode = EditMode.None;
                        editSession.Active = false;
                    }
                });

                if (ImGui.Button("Get Colors"))
                {
                    Sellbot instance = Sellbot.Instance;
                    string txt = "";
                    for (int col = 0; col<4; col++)
                    {
                        for (int row = 0; row<7; row++)
                        {

                            float cellX = instance.firstCellPos.X + instance.CellSize * row + instance.GetOffset(row);
                            float cellY = instance.firstCellPos.Y + instance.CellSize * col + instance.GetOffset(col);

                            DrawCursorSquare(new Vector2(cellX, cellY), instance.CellSize, Color.FromArgb(255, 0, 0, 255));
                            var color = PixelUtils.GetPixelColor((int)cellX, (int)cellY);
                            txt = txt + "\n" + '{' +$"\"{col}x{row}\",new ColorRange {{ R = ({color.R - Settings.Instance.m_Offsets.ColorTolarence}, {color.R + Settings.Instance.m_Offsets.ColorTolarence}), G = ({color.G - Settings.Instance.m_Offsets.ColorTolarence}, {color.G + Settings.Instance.m_Offsets.ColorTolarence}), B = ({color.B - Settings.Instance.m_Offsets.ColorTolarence}, {color.B + Settings.Instance.m_Offsets.ColorTolarence})," + "}},";
                            Task.Delay(50);
                        }
                    }
                    ImGui.SetClipboardText(txt);
                }

                ImGui.Checkbox("Debug Draw", ref m_debugDraw);

                ImGui.EndTabItem();
            }
        }
    }
}
