using ImGuiNET;
using mMacro.Core.Functions.Inventory;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace App.UI.Features
{
    public class uiMeltitems
    {
        private EditSession editSession     = EditSession.Instance;
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
                    editSession.Mode    = EditMode.FirstCell;
                    editSession.Size    = new Vector2(meltBot.CellSize, meltBot.CellSize);
                    editSession.OnSet   = pos =>
                    {
                        meltBot.MeltFirstPos = pos;
                        m_config.MeltFirstPos =pos;
                    };
                    editSession.Active  = true;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Select the first cell in the Smelting Furnace");
                ImGui.SameLine();
                if (ImGui.Button("Set Melt Down", buttonSize))
                {
                    editSession.Mode    = EditMode.MeltBot;
                    editSession.Size    =meltBot.MeltButtonSize;
                    editSession.OnSet   = pos =>
                    {
                        meltBot.MeltButtonPos = pos;
                        m_config.MeltButtonPos =pos;
                    };
                    editSession.Active  = true;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Select the 'Melt Down' Button in the Smelting Furnace");
                ImGui.SameLine();
                if (ImGui.Button("Set Close", buttonSize))
                {
                    editSession.Mode    = EditMode.SetClose;
                    editSession.Size    = new Vector2(30, 30);
                    editSession.OnSet   = pos =>
                    {
                        meltBot.MeltCloseBtn = pos;
                        m_config.MeltCloseBtn =pos;
                    };
                    editSession.Active  = true;
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
