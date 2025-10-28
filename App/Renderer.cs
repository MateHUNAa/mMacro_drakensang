using App.UI;
using App.UI.Debug;
using App.UI.Features;
using App.UI.Pages;
using ClickableTransparentOverlay;
using Core.Events;
using ImGuiNET;
using mMacro.Core.Functions;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using System.Numerics;

namespace mMacro.App
{

    public class Renderer(int screenWidth = 1920, int screenHeight = 1080) : Overlay(windowWidth: screenWidth, windowHeight: screenHeight)
    {
        public Vector2 screenSize = new Vector2(screenWidth, screenHeight);
        private ReviveBot reviveBot = ReviveBot.Instance;
        private AppConfig m_config = ConfigManager.Load();
        private EditSession editSession = EditSession.Instance;
        
        private readonly uiRevive reviveUI          = new uiRevive();
        private readonly uiSwapCape swapCapeUI      = new uiSwapCape();
        private readonly DebugDraw debugDrawUI      = new DebugDraw();
        private readonly uiSettings settingsUI      = new uiSettings();

        public bool IsVisible { get; set; } = true;

        protected override Task PostInitialized()
        {
            KeybindManager.Instance.Register("Toggle Panel", Keys.Insert, () => IsVisible =!IsVisible);

            EvtRevive.OnRequestSaveFirstPlayer += () =>
            {
                editSession.Mode = EditMode.Player;
                editSession.Radius = reviveBot.Radius;
                editSession.OnSet = (pos) =>
                {
                    reviveBot.FirstPlayerPos = pos;
                    m_config.FirstPlayerPos = pos;
                    EvtRevive.RaisePlayerPositionSet(pos);
                };
                editSession.Active = true;
            };

            return base.PostInitialized();
        }
 
        protected override void Render()
        {
            var io = ImGui.GetIO();
            var mousePos = io.MousePos;
            editSession.HandleEditSession(mousePos);
            ImGui.StyleColorsClassic();

            #region EditMode
            if (editSession.Active)
            {
                ImGui.SetNextWindowPos(Vector2.Zero);
                ImGui.SetNextWindowSize(screenSize);
                ImGui.Begin("EditMode Overlay", ImGuiWindowFlags.NoTitleBar
                                              | ImGuiWindowFlags.NoBackground
                                              | ImGuiWindowFlags.NoBringToFrontOnFocus);
                ImGui.End();
                return;
            }
            #endregion


            if (settingsUI.ShowOverlay)
                FeatureOverlay.Draw();

            if (!IsVisible) return;

            ImGui.Begin("mMacro - Drakensang", ImGuiWindowFlags.NoBringToFrontOnFocus);

            if (ImGui.BeginTabBar("MainTabs"))
            {
 
                uiKeybinds.Draw();

                swapCapeUI.Draw();
                reviveUI.Draw();
                uiInventory.Draw();
                settingsUI.Draw();

                if (DebugPage.DebugMode)
                    DebugPage.Draw();
                

                if (ImGui.BeginTabItem("Menu Style"))
                {
                    ImGui.ShowStyleEditor();
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }

            debugDrawUI.Draw();

            ImGui.End();
        }
    }
}
