using ImGuiNET;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using System.Numerics;
using static App.UI.DrawShape;
using Color = System.Drawing.Color;

namespace App.UI
{

    [Flags]
    public enum EditMode
    {
        None,
        FirstCell,
        Bag,
        SetClose,
        Player,
        MeltBot,
        ColorPicker
    }

    public class EditSession
    {
        private static readonly Lazy<EditSession> m_instance = new(() => new EditSession());
        public static EditSession Instance => m_instance.Value;

        public bool Active { get; set; } = false;
        public EditMode Mode { get; set; } = EditMode.None;
        public Vector2 Size { get; set; } = new Vector2(10, 10);
        public float Radius { get; set; } = 10;
        public Vector4 Color { get; set; } = new Vector4(255, 0, 0, 255);
        public Action<Vector2> OnSet { get; set; } = null;

        private readonly ColorPicker colorPicker;
        private readonly AppConfig m_config;

        private EditSession()
        {
            m_config = ConfigManager.Load();
            colorPicker = new ColorPicker();
        }
        public void HandleEditSession(Vector2 mousePos)
        {
            if (!Active) return;


            switch (Mode)
            {
                case EditMode.FirstCell:
                case EditMode.Bag:
                case EditMode.SetClose:
                case EditMode.MeltBot:
                    DrawCursorSquare(mousePos, Size, Color);
                    break;
                case EditMode.Player:
                    DrawCursorCircle(mousePos, Radius, Color);
                    break;
                case EditMode.ColorPicker:
                    colorPicker.Draw(ImGui.GetIO());
                    break;
            }

            if (ImGui.GetIO().MouseClicked[0])
            {
                OnSet?.Invoke(mousePos);
                Active = false;
                ConfigManager.Save(m_config);
            }
        }
        public void StartEditSession(EditSession editSession, Action<Vector2> OnSet, EditMode mode, Vector4? color, Vector2? size, int radius = 10)
        {
            editSession.Mode = mode;
            editSession.Radius = radius;
            if (color.HasValue) editSession.Color = color.Value;
            if (size.HasValue) editSession.Size = size.Value;

            editSession.OnSet  = OnSet;
            editSession.Active = true;
        }
        public void StartEditSession(EditSession editSession, Action<Vector2> OnSet, EditMode mode, Vector4? color, int size = 15, int radius = 15) => StartEditSession(editSession, OnSet, mode, color, new Vector2(size, size), radius);
        public void StartEditSession(EditSession editSession, Action<Vector2> OnSet, EditMode mode, Color color, int size = 15, int radius = 15) => StartEditSession(editSession, OnSet, mode, new Vector4(color.R, color.G, color.B, color.A), new Vector2(size, size), radius);
        public void StartEditSession(EditSession editSession, Action<Vector2> OnSet, EditMode mode, Color color, Vector2? size, int radius = 15) => StartEditSession(editSession, OnSet, mode, new Vector4(color.R, color.G, color.B, color.A), size.Value, radius);
    }
}
