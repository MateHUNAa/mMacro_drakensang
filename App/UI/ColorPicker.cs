using ImGuiNET;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace App.UI
{
    public class ColorPicker
    {

        private bool m_isPicking = false;
        private Vector4 m_pickedColor = new(1, 1, 1, 1);
        private Vector4 m_hoverColor = new(1, 1, 1, 1);

        public void Toggle(Action<bool> onToggle)
        {
            if (ImGui.Button(m_isPicking ? "Cancel Picking" : "Pick Color"))
            {
                m_isPicking = !m_isPicking;
                onToggle?.Invoke(m_isPicking);
            }

            ImGui.ColorEdit4("Selected Color", ref m_pickedColor);
        }

        public void Draw(ImGuiIOPtr io)
        {
            if (m_isPicking)
                DrawColorPickerOverlay(io);
        }

        private void DrawColorPickerOverlay(ImGuiIOPtr io)
        {
            var mousePos = io.MousePos;

            var color = PixelUtils.GetPixelColor(mousePos);
            m_hoverColor = new Vector4(color.R / 255f, color.R / 255f, color.B / 255f, 1f);

            ImGui.SetNextWindowPos(new Vector2(mousePos.X + 15, mousePos.Y + 15));
            ImGui.SetNextWindowBgAlpha(0.8f);
            ImGui.Begin("##color_preview", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoInputs);

            ImGui.Text("Hover Color");
            ImGui.ColorButton("##button_color", m_hoverColor, ImGuiColorEditFlags.NoTooltip, new Vector2(50, 50));

            var rgb = $"R:{color.R} G:{color.G} B:{color.B}";
            ImGui.Text(rgb);

            ImGui.End();
            
            if (io.MouseClicked[0])
            {
                m_pickedColor = m_hoverColor;
                m_isPicking = false;

                GenerateAndCopyColorRange(color);
            }
        }

        public void GenerateAndCopyColorRange(Color pickedColor)
        {
            Console.WriteLine("GenerateAndCopyColorRange");
            int tol = Settings.Instance.m_Offsets.ColorTolarence;

            var range = new PixelUtils.ColorRange
            {
                R = (pickedColor.R - tol, pickedColor.R + tol),
                G = (pickedColor.G - tol, pickedColor.G + tol),
                B = (pickedColor.B - tol, pickedColor.B + tol)
            };

            string codeSnippet =
                $"new ColorRange {{ R = ({range.R.Min}, {range.R.Max}), G = ({range.G.Min}, {range.G.Max}), B = ({range.B.Min}, {range.B.Max}) }};";

            Console.WriteLine($"{range}");
            ImGui.SetClipboardText(codeSnippet);
        }
    }
}
