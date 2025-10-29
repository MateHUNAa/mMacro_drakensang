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
                float fullWidth = ImGui.GetContentRegionAvail().X;
                float itemWidth = fullWidth/ 3;

                float nameRatio = 0.6f;
                float keyRatio = 0.6f;
                float buttonRatio = 0.15f;

                float nameWidth = fullWidth * nameRatio;
                float keyWidth = fullWidth * keyRatio;
                float buttonWidth = fullWidth * buttonRatio;


                if (ImGui.BeginTable("KeybindTable", 3, ImGuiTableFlags.SizingStretchProp))
                {
                    ImGui.TableSetupColumn("Action", ImGuiTableColumnFlags.WidthStretch, nameWidth);
                    ImGui.TableSetupColumn("Key", ImGuiTableColumnFlags.WidthStretch, keyWidth);
                    ImGui.TableSetupColumn("Button", ImGuiTableColumnFlags.WidthStretch, buttonWidth);

                    foreach (var kvp in KeybindManager.Instance.Bindings)
                    {
                        string name = kvp.Key;
                        Keybind bind = kvp.Value;

                        string keyText = KeybindManager.Instance.FormatKeybind(bind.Modifiers, bind.Key);

                        ImGui.TableNextRow();

                        ImGui.TableSetColumnIndex(0);
                        ImGui.TextUnformatted(name);

                        ImGui.TableSetColumnIndex(1);
                        ImGui.Text($"[{keyText}]");

                        ImGui.TableSetColumnIndex(2);
                        if (KeybindManager.Instance.IsListening(name))
                        {
                            ImGui.Text("Press any key...");
                        }
                        else if (ImGui.Button($"Change##{name}", new Vector2(80, 0)))
                        {
                            KeybindManager.Instance.StartListening(name);
                        }
                    }

                    ImGui.EndTable();
                }
                ImGui.EndTabItem();
            }

        }
    }
}
