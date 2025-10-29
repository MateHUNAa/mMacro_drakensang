using App.UI.EditSession;
using Core.Attributes.Interface;
using ImGuiNET;
using mMacro.Core.Functions.Inventory;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using System.Numerics;

namespace App.UI.Features
{
    public class uiMeltitems
    {
        private readonly AppConfig m_config = ConfigManager.Load();
        private readonly MeltItems meltBot  = MeltItems.Instance;

        public void Draw()
        {

            if (ImGui.BeginTabItem("Melt Items"))
            {

                ImGui.SeparatorText("Setup");
                Vector2 buttonSize = new Vector2(ImGui.GetContentRegionAvail().X /3 -5, 0);
                if (ImGui.Button("Save Cell", buttonSize))
                {
                    var session = new ShapeEditSession();
                    session.Start(
                        shape: ShapeType.Square,
                        size: new Vector2(meltBot.CellSize, meltBot.CellSize),
                        onSet: pos => {
                            meltBot.MeltFirstPos = pos;
                            EditSessionManager.Instance.GetConfig().MeltFirstPos =pos;
                        });
                    EditSessionManager.Instance.StartSession(session);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Select the first cell in the Smelting Furnace");
                ImGui.SameLine();
                if (ImGui.Button("Set Melt Down", buttonSize))
                {
                    var session = new ShapeEditSession();
                    session.Start(
                        shape: ShapeType.Square,
                        size: meltBot.MeltButtonSize,
                        onSet: pos => {
                            meltBot.MeltButtonPos = pos;
                            EditSessionManager.Instance.GetConfig().MeltButtonPos = pos;
                        });
                    EditSessionManager.Instance.StartSession(session);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Select the 'Melt Down' Button in the Smelting Furnace");
                ImGui.SameLine();
                if (ImGui.Button("Set Close", buttonSize))
                {

                    var session = new ShapeEditSession();
                    session.Start(
                        shape: ShapeType.Square,
                        size: new Vector2(30, 30),
                        onSet: pos => {
                            meltBot.MeltCloseBtn = pos;
                            EditSessionManager.Instance.GetConfig().MeltCloseBtn = pos;
                        });
                    EditSessionManager.Instance.StartSession(session);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Select the 'X' Close button in the Smelting Furnace");
                ImGui.EndTabItem();

                ImGui.SeparatorText("Settings");
                int meltItemClickDelay = Settings.Instance.m_Timeings.MeltItemClickDelay;
                if (ImGui.SliderInt("Melt Item Click Delay (ms)", ref meltItemClickDelay, 1, 1000))
                {
                    Settings.Instance.m_Timeings.MeltItemClickDelay = meltItemClickDelay;
                    m_config.Settings.m_Timeings.MeltItemClickDelay = meltItemClickDelay;
                    ConfigManager.Save(m_config);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Delay between moveing the mouse and clicking an item.");

            }
        }
    }
}
