using ClickableTransparentOverlay;
using ImGuiNET;
using mMacro.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace mMacro.App
{
    public class Renderer : Overlay
    {
        public Vector2 screenSize = new Vector2(1920, 1080);
        protected override void Render()
        {
            ImGui.Begin("mMacro - Drakensang", ImGuiWindowFlags.NoBringToFrontOnFocus
                                             | ImGuiWindowFlags.NoCollapse
                                             | ImGuiWindowFlags.NoScrollbar
                                             | ImGuiWindowFlags.NoDecoration
                                             | ImGuiWindowFlags.AlwaysAutoResize);

            if (ImGui.BeginTabBar("MainTabs"))
            {
                // ================== General Tab ==================
                if (ImGui.BeginTabItem("General"))
                {
                    ImGui.Text("This is the General tab");
                    ImGui.Separator();

                    ImGui.EndTabItem();
                }

                // ================== Inventory Tab ==================
                if (ImGui.BeginTabItem("Inventory"))
                {
                    ImGui.Text("This is the Inventory tab");
                    ImGui.Separator();

                    ImGui.EndTabItem();
                }

                // ================== Keybinds Tab ==================
                if (ImGui.BeginTabItem("Keybinds"))
                {
                    ImGui.Text("This is the Keybinds tab");
                    ImGui.Separator();

                    foreach(var kvp in KeybindManager.Instance.Bindings)
                    {
                        string name = kvp.Key;
                        Keybind bind = kvp.Value;

                        string keyText = FormatKeybind(bind.Modifiers, bind.Key);

                        ImGui.Text($"{name}");
                        ImGui.SameLine();
                        ImGui.Text($"[{keyText}]");
                        ImGui.SameLine();

                        if (KeybindManager.Instance.IsListening(name))
                        {
                            ImGui.Text("Press any key...");
                        } else if (ImGui.Button($"Change##{name}"))
                        {
                            KeybindManager.Instance.StartListening(name);
                        }
                    }
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }


            ImGui.End();
        }

        private string FormatKeybind(KeyModifiers modifiers, Keys key)
        {
            List<string> parts = new();

            if (modifiers.HasFlag(KeyModifiers.Ctrl)) parts.Add("Ctrl");
            if (modifiers.HasFlag(KeyModifiers.Shift)) parts.Add("Shift");
            if (modifiers.HasFlag(KeyModifiers.Alt)) parts.Add("Alt");

            parts.Add(key.ToString());

            return string.Join(" + ", parts);
        }
    }
}
