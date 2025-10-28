using App.UI.Renderers;
using ImGuiNET;
using mMacro.Core.Functions;

namespace App.UI.Features
{
    public class uiRevive
    {
 
        private readonly ReviveBot reviveBot  = ReviveBot.Instance;
        public void Draw()
        {
            if (ImGui.BeginTabItem("Revive Bot"))
            {
                ImGui.SeparatorText("Activation");
                reviveBot.DrawActivation();

                ImGui.SeparatorText("Setup");

                reviveBot.DrawCustomButtons();

                ImGui.SeparatorText("Block Offers");
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Prevents automatically sending revive offers to the selected player.");

                for (int i = 0; i<5; i++)
                {
                    bool value = reviveBot.m_blockOffers[i];

                    if (ImGui.Checkbox($"Block offer - Player {i}##{i}", ref value))
                    {
                        reviveBot.m_blockOffers[i] = value;
                    }
                }

                ImGui.EndTabItem();
            }
        }

    }
}
