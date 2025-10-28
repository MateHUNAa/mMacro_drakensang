using Core.Attributes;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
