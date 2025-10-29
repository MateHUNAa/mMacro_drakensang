using App.UI;
using App.UI.Debug;
using App.UI.EditSession;
using App.UI.Features;
using App.UI.Pages;
using ClickableTransparentOverlay;
using Core.Events;
using ImGuiNET;
using mMacro.Core.Functions;
using mMacro.Core.Managers;
using System.Numerics;

namespace mMacro.App
{

    public class Renderer(int screenWidth = 1920, int screenHeight = 1080) : Overlay(windowWidth: screenWidth, windowHeight: screenHeight)
    {
        public Vector2 screenSize = new Vector2(screenWidth, screenHeight);
        private ReviveBot reviveBot = ReviveBot.Instance;
        
        private readonly uiRevive reviveUI          = new uiRevive();
        private readonly uiSwapCape swapCapeUI      = new uiSwapCape();
        private readonly DebugDraw debugDrawUI      = new DebugDraw();
        private readonly uiSettings settingsUI      = new uiSettings();

        public bool IsVisible { get; set; } = true;

        private string ConfigDirectory { get; set; }
        private string ConfigFile { get; set; } 
        protected override Task PostInitialized()
        {
            ConfigDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mMacro_dso");
            ConfigFile = Path.Combine(ConfigDirectory, "imgui.ini");

            ImGui.LoadIniSettingsFromDisk(ConfigFile);

            KeybindManager.Instance.Register("Toggle Panel", Keys.Insert, () => IsVisible =!IsVisible);
            EvtRevive.OnRequestSaveFirstPlayer += () =>
            {

                var session = new ShapeEditSession();

                session.Start(
                    shape: ShapeType.Circle,
                    onSet: (pos) =>
                    {
                        reviveBot.FirstPlayerPos = pos;
                        EditSessionManager.Instance.GetConfig().FirstPlayerPos = pos;
                        EvtRevive.RaisePlayerPositionSet(pos);
                    },
                    radius: reviveBot.Radius);
                EditSessionManager.Instance.StartSession(session);
            };

            AppDomain.CurrentDomain.ProcessExit +=OnApplicationExit;

            return base.PostInitialized();
        }

        private void OnApplicationExit(object? sender, EventArgs e)
        {
            ImGui.SaveIniSettingsToDisk(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mMacro_dso", "imgui.ini"));
        }

        protected override void Render()
        {
            var io = ImGui.GetIO();
            var mousePos = io.MousePos;
            EditSessionManager.Instance.Handle(mousePos);
            ImGui.StyleColorsClassic();

            #region EditMode
            if (EditSessionManager.Instance.CurrentSession != null && EditSessionManager.Instance.CurrentSession.Active)
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

            if (ImGui.BeginTabBar("MainTabs", ImGuiTabBarFlags.Reorderable))
            {

                uiGeneral.Draw();
                uiKeybinds.Draw();

                swapCapeUI.Draw();
                reviveUI.Draw();
                uiInventory.Draw();
                settingsUI.Draw();

                if (DebugPage.DebugMode)
                    DebugPage.Draw();
                

                if (ImGui.BeginTabItem("Menu Style"))
                {
                    ImGui.TextColored(new Vector4(1, 0, 0, 1), "Saveing is not completed !");
                    ImGui.ShowStyleEditor();
                    if (ImGui.Button("Save Style", new Vector2(ImGui.GetContentRegionAvail().X, 0)))
                        ImGui.SaveIniSettingsToDisk(ConfigFile);

                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }

            debugDrawUI.Draw();

            ImGui.End();
        }
    }
}
