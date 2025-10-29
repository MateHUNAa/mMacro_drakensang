using App.UI.EditSession;
using Core.Attributes.Interface;
using ImGuiNET;
using mMacro.Core.Functions.Inventory;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using System.Numerics;

namespace App.UI.Features
{
    public class uiMeltgem
    {
        private readonly AppConfig m_config = ConfigManager.Load();

        public void Draw()
        {
            if (ImGui.BeginTabItem("Melt Gems"))
            {
                {
                    ImGui.SeparatorText("Setup");

                    Vector2 buttonSize = new Vector2(ImGui.GetContentRegionAvail().X /4 -6, 0);

                    if (ImGui.Button("Potion Tab", buttonSize))
                    {
                        var session = new ShapeEditSession();
                        session.Start(
                            shape: ShapeType.Square,
                            color: new Vector4(255,0,0,255),
                            onSet: (pos) => {
                                MeltGems.Instance.Positions.PotionPosition = pos;
                                EditSessionManager.Instance.GetConfig().CraftPositions.PotionPosition = pos;
                            },
                            size: new Vector2(100, 55));
                        EditSessionManager.Instance.StartSession(session);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Select the Potion tab in the workbench");
                    ImGui.SameLine();
                    if (ImGui.Button("Max Arrow", buttonSize))
                    {
                        var session = new ShapeEditSession();
                        session.Start(
                            shape: ShapeType.Square,
                            color: new Vector4(255, 0, 0, 255),
                            onSet: (pos) => {
                                MeltGems.Instance.Positions.MaxBtnPosition = pos;
                                EditSessionManager.Instance.GetConfig().CraftPositions.MaxBtnPosition = pos;
                            },
                            size: new Vector2(35, 15));
                        EditSessionManager.Instance.StartSession(session);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Select the Max Arrow in the workbench");
                    ImGui.SameLine();
                    if (ImGui.Button("Minus Arrow", buttonSize))
                    {
                        var session = new ShapeEditSession();
                        session.Start(
                            shape: ShapeType.Square,
                            color: new Vector4(255, 0, 0, 255),
                            onSet: (pos) => {
                                MeltGems.Instance.Positions.MinusBtnPosition = pos;
                                EditSessionManager.Instance.GetConfig().CraftPositions.MinusBtnPosition = pos;
                            },
                            size: new Vector2(18, 18));
                        EditSessionManager.Instance.StartSession(session);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Select the Minus Arrow in the workbench");
                    ImGui.SameLine();
                    if (ImGui.Button("Exit Button", buttonSize))
                    {
                        var session = new ShapeEditSession();
                        session.Start(
                            shape: ShapeType.Square,
                            color: new Vector4(255, 0, 0, 255),
                            onSet: (pos) => {
                                MeltGems.Instance.Positions.ClosePosition = pos;
                                EditSessionManager.Instance.GetConfig().CraftPositions.ClosePosition = pos;
                            },
                            size: new Vector2(25, 25));
                        EditSessionManager.Instance.StartSession(session);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Select the Exit Cross in the workbench");
                    if (ImGui.Button("Combine Button", new Vector2(ImGui.GetContentRegionAvail().X, 0)))
                    {
                        var session = new ShapeEditSession();
                        session.Start(
                            shape: ShapeType.Square,
                            color: new Vector4(255, 0, 0, 255),
                            onSet: (pos) => {
                                MeltGems.Instance.Positions.CombinePosition = pos;
                                EditSessionManager.Instance.GetConfig().CraftPositions.CombinePosition = pos;
                            },
                            size: new Vector2(180, 45));
                        EditSessionManager.Instance.StartSession(session);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Select the 'Combine' button in the workbench");

                }

                ImGui.SeparatorText("Settings");
                int meltGemClickDelay = Settings.Instance.m_Timeings.MeltGemClickDelay;
                if (ImGui.SliderInt("Melt Gem Click Delay (ms)", ref meltGemClickDelay, 1, 1000))
                {
                    Settings.Instance.m_Timeings.MeltGemClickDelay = meltGemClickDelay;
                    m_config.Settings.m_Timeings.MeltGemClickDelay = meltGemClickDelay;
                    ConfigManager.Save(m_config);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Delay between moveing the mouse and clicking an item.");


                int meltGemTaskDelay = Settings.Instance.m_Timeings.MeltGemTaskEndDelay;
                if (ImGui.SliderInt("Melt Task end delay (ms)", ref meltGemTaskDelay, 1, 10000))
                {
                    Settings.Instance.m_Timeings.MeltGemTaskEndDelay = meltGemTaskDelay;
                    m_config.Settings.m_Timeings.MeltGemTaskEndDelay = meltGemClickDelay;
                    ConfigManager.Save(m_config);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Wait after finishing a task.");


                ImGui.EndTabItem();
            }
        }
    }
}
