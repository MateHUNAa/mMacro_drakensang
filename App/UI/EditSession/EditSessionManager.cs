using ImGuiNET;
using mMacro.Core.Models;
using System.Numerics;
using mMacro.Core.Managers;

namespace App.UI.EditSession
{
    public class EditSessionManager
    {
        private static readonly Lazy<EditSessionManager> m_instance = new(() => new EditSessionManager());
        public static EditSessionManager Instance => m_instance.Value;
        public BaseEditSession? CurrentSession { get; private set; }
        private readonly AppConfig m_config;
        private EditSessionManager() 
        {
            m_config = ConfigManager.Load();
        }

        public void StartSession(BaseEditSession session)
        {
            CurrentSession = session;
            CurrentSession.Start();
        }
        public void StopCurrentSession()
        {
            CurrentSession?.Stop();
            CurrentSession = null;
        }

        public void Handle(Vector2 mousePos)
        {
            if (CurrentSession == null || !CurrentSession.Active) return;
            CurrentSession.Draw(mousePos);
            if (ImGui.GetIO().MouseClicked[0])
            {
                CurrentSession.OnClick(mousePos);
                ConfigManager.Save(m_config);
            }
        }

        public AppConfig GetConfig() => m_config;
    }
}
