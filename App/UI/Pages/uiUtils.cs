using App.UI.Renderers;
using ImGuiNET;
using mMacro.Core.Functions;

namespace App.UI.Pages
{
    public static class uiUtils
    {

        public static void Draw()
        {
            if (ImGui.BeginTabItem("Utils"))
            {
                if(ImGui.BeginTabBar("utils"))
                {

                    if (ImGui.BeginTabItem("Auto Pot"))
                    {
                        AutoPotion.Instance.DrawCustomButtons();
                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabBar();
                }

                ImGui.EndTabItem();
            }
        }
    }
}
