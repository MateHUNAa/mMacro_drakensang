using ImGuiNET;
using mMacro.Core.Managers;
using System.Numerics;

namespace App.UI.Pages
{
    public static class uiKeybinds
    {

        public static void Draw()
        {
            if (ImGui.BeginTabItem("Keybinds"))
            {
                ImGui.Text("This is the Keybinds tab");
                ImGui.Separator();

                foreach (var kvp in KeybindManager.Instance.Bindings)
                {
                    string name = kvp.Key;
                    Keybind bind = kvp.Value;

                    string keyText = KeybindManager.Instance.FormatKeybind(bind.Modifiers, bind.Key);

                    float fullWidth = ImGui.GetContentRegionAvail().X;

                    float nameWidth = ImGui.CalcTextSize(name).X;
                    float keyWidth = ImGui.CalcTextSize($"[{keyText}]").X;
                    float buttonWidth = 80.0f;

                    float spaceing = (fullWidth - (nameWidth + keyWidth + buttonWidth)) /2;

                    ImGui.Text($"{name}");
                    ImGui.SameLine((float)((nameWidth + spaceing *1.5) /1.5));
                    ImGui.Text($"[{keyText}]");
                    ImGui.SameLine(nameWidth + spaceing + keyWidth +spaceing - buttonWidth /2);

                    if (KeybindManager.Instance.IsListening(name))
                    {
                        ImGui.Text("Press any key...");
                    }
                    else if (ImGui.Button($"Change##{name}", new Vector2(buttonWidth, 0)))
                    {
                        KeybindManager.Instance.StartListening(name);
                    }
                }
                ImGui.EndTabItem();
            }

        }
    }
}
