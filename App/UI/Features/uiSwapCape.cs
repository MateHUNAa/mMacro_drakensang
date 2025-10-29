using App.UI.EditSession;
using Core.Attributes.Interface;
using ImGuiNET;
using mMacro.Core.Functions;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using System.Numerics;

namespace App.UI.Features
{
    public class uiSwapCape
    {
        private readonly SwapCape swapCape = SwapCape.Instance;
        private readonly AppConfig m_config = ConfigManager.Load();
        public void Draw()
        {
            if (ImGui.BeginTabItem("SwapCape"))
            {
                ImGui.SeparatorText("Setup");
                Vector2 buttonSize = new Vector2(ImGui.GetContentRegionAvail().X, 0);
                if (ImGui.Button("Set Close Button", buttonSize))
                {
                    var session = new ShapeEditSession();
                    session.Start(
                        shape: ShapeType.Square,
                        size: new Vector2(swapCape.CloseButtonScale),
                        onSet: pos => {
                            swapCape.InventoryCloseButtonPosition = pos;
                            EditSessionManager.Instance.GetConfig().ClosePoint = pos;
                        });
                    EditSessionManager.Instance.StartSession(session);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Click to save the in-game close button position.");

                ImGui.SeparatorText("Inventory Settings");
                {
                    if (ImGui.SliderInt("Bag", ref swapCape.bag, 0, 8))
                    {
                        m_config.swapBag = swapCape.bag;
                        ConfigManager.Save(m_config);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("The bag where the cape is located.");

                    if (ImGui.SliderInt("Columns", ref swapCape.col, 0, 3))
                    {
                        m_config.swapCol = swapCape.col;
                        ConfigManager.Save(m_config);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("The column where the cape is located.");

                    if (ImGui.SliderInt("Rows", ref swapCape.row, 0, 6))
                    {
                        m_config.swapRow = swapCape.row;
                        ConfigManager.Save(m_config);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("The row where the cape is located.");
                }

                ImGui.SeparatorText("Settings");
                {
                    int clickDelay = Settings.Instance.m_Timeings.SwapCapeClickDelay;
                    if (ImGui.SliderInt("Click Delay (ms)", ref clickDelay, 1, 800))
                    {
                        Settings.Instance.m_Timeings.SwapCapeClickDelay = clickDelay;
                        ConfigManager.Save(m_config);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Time between moveing the cursor and clicking");
                }


                ImGui.SeparatorText("Keybinds");

                ImGui.TextColored(new Vector4(1,0,0,1), "NOT WORKING !");
                ImGui.EndTabItem();
            }
        }
    }
}
