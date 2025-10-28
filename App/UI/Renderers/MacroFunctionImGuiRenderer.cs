using Core.Attributes;
using ImGuiNET;
using mMacro.Core.Managers;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace App.UI.Renderers
{
    public static class MacroFunctionImGuiRenderer
    {
        public static void DrawCustomButtons(this MacroFunction macro)
        {
            var methods = macro.GetType()
                .GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                .Where(m => m.GetCustomAttribute<ButtonAttribute>() != null)
                .ToArray();

            if (methods.Length == 0) return;

            int i = 0;
            float rowMargin = 8f;

            while (i < methods.Length)
            {
                var method = methods[i];
                var attr = method.GetCustomAttribute<ButtonAttribute>();
                int columns = Math.Max(1, attr.Columns); // default columns
                int buttonsLeft = methods.Length - i;

                bool isLastRow = buttonsLeft < columns;
                int buttonsInRow = isLastRow ? buttonsLeft : columns;

                float totalWidth = ImGui.GetContentRegionAvail().X;
                if (!isLastRow) totalWidth -= rowMargin;

                float buttonWidth = totalWidth / buttonsInRow;

                for (int j = 0; j < buttonsInRow; j++)
                {
                    var m = methods[i + j];
                    var a = m.GetCustomAttribute<ButtonAttribute>();
                    string label = a.Label ?? m.Name;

                    Vector2 size = a.Inline ? new Vector2(buttonWidth, a.Height) : a.Size;

                    if (ImGui.Button(label, size))
                        m.Invoke(macro, null);

                    if (j < buttonsInRow - 1)
                        ImGui.SameLine();
                }

                if (!isLastRow)
                {
                    ImGui.SameLine();
                    ImGui.Dummy(new Vector2(rowMargin, 0));
                }

                i += buttonsInRow;
            }
        }

        /// <summary>
        /// Draws UI for activation
        /// </summary>
        public static void DrawActivation(this MacroFunction macro)
        {
            switch (macro.ExecutionType)
            {
                case ExecutionType.Toggleable:
                    macro.DrawToggle();
                    break;
                case ExecutionType.RunOnce:
                    macro.DrawExecute();
                    break;
            }
        }

        /// <summary>
        /// Draws toggle button ( for toggleable )
        /// </summary>
        public static void DrawToggle(this MacroFunction macro)
        {
            if (ImGui.Button(macro.Enabled ? $"Disable {macro.Name}" : $"Enable {macro.Name}", new Vector2(ImGui.GetContentRegionAvail().X, 0)))
                macro.Toggle();
        }

        /// <summary>
        /// Draw execute button ( for run once )
        /// </summary>
        public static void DrawExecute(this MacroFunction macro)
        {
            if (ImGui.Button($"Execute {macro.Name}", new Vector2(ImGui.GetContentRegionAvail().X, 0)))
                macro.Execute();
        }

    }
}
