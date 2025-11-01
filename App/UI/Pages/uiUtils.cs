using App.UI.Features;
using App.UI.Renderers;
using ImGuiNET;
using mMacro.Core.Functions;

namespace App.UI.Pages
{
    public static class uiUtils
    {

        private static readonly uiAutoPot autoPot = new();
        public static void Draw()
        {
            if (ImGui.BeginTabItem("Utils"))
            {
                if(ImGui.BeginTabBar("utils"))
                {
                    autoPot.Draw();

                 

                    ImGui.EndTabBar();
                }

                ImGui.EndTabItem();
            }
        }
    }
}
