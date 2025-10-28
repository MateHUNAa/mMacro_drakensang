using ImGuiNET;
using mMacro.Core.Functions;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using System.Numerics;

namespace App.UI.Pages
{
    public static class uiGeneral
    {

        private static readonly AppConfig m_config = ConfigManager.Load();  
        private static int delay = m_config.ClickDelay;
        public static void Draw()
        {
            if (ImGui.BeginTabItem("General"))
            {
                int i = 0;
                float halfWidth = ImGui.GetContentRegionAvail().X/2 - 10;

                var functions = FunctionManager.Instance.Functions
                    .Where(f => f.Mode.HasFlag(ActivationMode.Both) || f.Mode.HasFlag(ActivationMode.MenuOnly))
                    .ToList();

                int count = functions.Count;
                foreach (var func in functions)
                {
                    bool isLast = (i == count -1);
                    bool isOddCount = (count % 2 != 0);

                    Vector2 size = new Vector2((isLast && isOddCount) ? ImGui.GetContentRegionAvail().X - 12 : halfWidth, 0);

                    string label = $"{(func.ExecutionType == ExecutionType.Toggleable ? (func.Enabled ? "Disable" : "Enable") : "Execute")} {func.Name} ({func.Defaultkey})";
                    if (ImGui.Button(label, size))
                    {
                        if (func.ExecutionType == ExecutionType.RunOnce) func.Execute();
                        if (func.ExecutionType == ExecutionType.Toggleable) func.Toggle();
                    }

                    if (i % 2 == 0 && !(isLast && isOddCount))
                        ImGui.SameLine();

                    i++;
                }
                ImGui.SeparatorText("Auto Clicker");
                if (ImGui.InputInt("Clicker Delay(ms)", ref delay, 1, 10000))
                {
                    AutoClicker.Instance.SetDelay(delay);
                }

                ImGui.EndTabItem();
            }
        }
    }
}
