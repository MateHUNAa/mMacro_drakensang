using App.UI.Features;
using ImGuiNET;

namespace App.UI.Pages
{
    public static class uiInventory
    {

        private static readonly uiSellbot inventoryUI = new uiSellbot();
        private static readonly uiMeltgem meltgemUI = new uiMeltgem();
        private static readonly uiMeltitems meltitemsUI = new uiMeltitems();
        public static void Draw()
        {
            if (ImGui.BeginTabItem("Inventory"))
            {
                if (ImGui.BeginTabBar("Inventory"))
                {
                    // ================== Sell Bot ==================
                    inventoryUI.DrawInventory();
                    // ================== Melt Bot ==================
                    meltitemsUI.Draw();
                    meltgemUI.Draw();

                    ImGui.EndTabBar();

                }
                ImGui.EndTabItem();
            }
        }
    }
}
