using Core.Attributes.Interface;
using ImGuiNET;
using System.Numerics;

namespace App.UI.Renderers
{
    internal class ImGuiButtonRenderer : IButtonRenderer
    {
        public void Draw(string label, Action onClick, Vector2 size)
        {
            if (ImGui.Button(label, size))
                onClick.Invoke();
        }
    }
}
