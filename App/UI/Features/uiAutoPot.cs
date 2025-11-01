using App.UI.Renderers;
using ImGuiNET;
using mMacro.Core.Functions;

namespace App.UI.Features
{
    public class uiAutoPot
    {

        private readonly AutoPotion autoPotion = AutoPotion.Instance;
        public void Draw()
        {
            if (ImGui.BeginTabItem("Auto Pot"))
            {
                AutoPotion.Instance.DrawCustomButtons();

                ImGui.SliderFloat("Health trashhold", ref autoPotion.HealthThreshold, 20f, 100f);

                ImGui.EndTabItem();
            }
        }
    }
}
