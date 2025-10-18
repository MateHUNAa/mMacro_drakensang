using ImGuiNET;
using mMacro.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace App.UI
{
    public static class FeatureOverlay
    {
        public static void Draw()
        {


            ImGui.Begin("FeatureOverlay", ImGuiWindowFlags.NoCollapse |   
                                          ImGuiWindowFlags.NoScrollbar 
                                          );
            ImGui.SetWindowSize(new Vector2(120, 180), ImGuiCond.FirstUseEver);


            var functions = FunctionManager.Instance.Functions
                .Where(f => f.Mode.HasFlag(ActivationMode.Both) || f.Mode.HasFlag(ActivationMode.MenuOnly))
                .ToList();

            foreach ( var f in functions )
            {
                if (f.Enabled)
                {
                    ImGui.BulletText(f.Name);
                }
            }
            ImGui.End();
        }
    }
}
