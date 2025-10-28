using App.UI.Debug;
using ImGuiNET;
using mMacro.Core.Managers;
using mMacro.Core.Models;

namespace App.UI.Pages
{
    public class uiSettings
    {

        private readonly AppConfig m_config = ConfigManager.Load();
        private void SaveConfig() => ConfigManager.Save(m_config);
        private bool m_showOverlay = true;
        public bool ShowOverlay
        {
            get => m_showOverlay;
            set => m_showOverlay = value;
        }

        public void Draw()
        {
            if (ImGui.BeginTabItem("Settings"))
            {
                ImGui.SeparatorText("Debug Settings");
                {
                    int colorTolarence = Settings.Instance.m_Offsets.ColorTolarence;
                    if (ImGui.SliderInt("Color Tolarence", ref colorTolarence, 1, 50))
                    {
                        Settings.Instance.m_Offsets.ColorTolarence = colorTolarence;
                        m_config!.Settings = Settings.Instance;

                        ConfigManager.Save(m_config);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Sets the ColorTolarance of the copy colors");

                    ImGui.Checkbox("Debug Mode", ref DebugPage.m_debugMode);
                }

                ImGui.SeparatorText("App Settings");
                if (ImGui.CollapsingHeader("Sizes"))
                {
                    {
                        int cellSize = Settings.Instance.m_Sizes.CellSize;
                        if (ImGui.InputInt("Cell Size", ref cellSize))
                        {
                            Settings.Instance.m_Sizes.CellSize = cellSize;
                            SaveConfig();
                        }
                        if (ImGui.IsItemHovered())
                            ImGui.SetTooltip("Sets the cell sizes; Default: 76");

                        // BagSize
                        int bagSize = Settings.Instance.m_Sizes.BagSize;
                        if (ImGui.InputInt("Bag Size", ref bagSize))
                        {
                            Settings.Instance.m_Sizes.BagSize = bagSize;
                            SaveConfig();
                        }
                        if (ImGui.IsItemHovered())
                            ImGui.SetTooltip("Sets the bag sizes; Default: 40");
                    }

                }

                ImGui.Spacing();

                if (ImGui.CollapsingHeader("Offsets"))
                {
                    int bagOffsetX = Settings.Instance.m_Offsets.BagOffsetX;
                    if (ImGui.InputInt("Bag Offset X", ref bagOffsetX))
                    {
                        Settings.Instance.m_Offsets.BagOffsetX = bagOffsetX;
                        SaveConfig();
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Sets the bag OffsetX; Default: 49");

                    //ImGui.TextColored(ColorRed, "Cell offsets only changable in the source code !");
                }

                ImGui.Checkbox("Show overlay", ref m_showOverlay);

                ImGui.EndTabItem();
            }
        }
    }
}
