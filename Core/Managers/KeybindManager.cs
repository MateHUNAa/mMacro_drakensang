using mMacro.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mMacro.Core.Managers
{
    [Flags]
    public enum KeyModifiers
    {
        None    = 0,
        Ctrl    = 1,
        Alt     = 2,
        Shift   = 4
    }

    public class Keybind
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Keys Key { get; set; } = Keys.None;
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyModifiers Modifiers { get; set; }
        public bool WasPressed { get; set; } = false;
        [JsonIgnore]
        public Action Action { get; set; }

        public Keybind(Keys key, Action action, KeyModifiers modifiers = KeyModifiers.None)
        {
            Key = key;
            Action = action;
            Modifiers = modifiers;
        }
    }

    public class KeybindManager
    {
        private static KeybindManager? m_instance;
        public static KeybindManager Instance => m_instance ?? throw new InvalidOperationException("Keybind manager not initialized!");
        private readonly Dictionary<string, Keybind> m_bindings = new();
        public IReadOnlyDictionary<string, Keybind> Bindings => m_bindings;
        private string? m_listeningKeybindName = null;
        private bool m_awaitKeyRelease = false;
        private AppConfig m_config;
        public KeybindManager()
        {
            if (m_instance !=null)
                throw new InvalidOperationException("KeybindManager already initialized !");

            m_config = ConfigManager.Load();
            m_instance = this;
            AppDomain.CurrentDomain.ProcessExit +=OnApplicationClosed;
        }

        private void OnApplicationClosed(object? sender, EventArgs e)
        {
            SaveConfig();
        }
        public void Register(string name, Keys key, Action action, KeyModifiers modifiers = KeyModifiers.None)
        {
            if (m_bindings.ContainsKey(name))
            {
                if (m_config.Keybinds.ContainsKey(name)) 
                {
                    Keybind savedBind = m_config.Keybinds[name];
                    m_bindings[name].Key = savedBind.Key;
                    m_bindings[name].Modifiers = savedBind.Modifiers;
                }
                return;
            }

            m_bindings[name] = new Keybind(key, action, modifiers);

            if (m_config.Keybinds.ContainsKey(name))
            {
                Keybind savedBind = m_config.Keybinds[name];
                m_bindings[name].Key = savedBind.Key;
                m_bindings[name].Modifiers = savedBind.Modifiers;
            }

            //Console.WriteLine($"[{name}] Keybind registered: ({FormatKeybind(m_bindings[name].Modifiers, m_bindings[name].Key)})");
        }
        public bool IsListening(string keybindName) => m_listeningKeybindName == keybindName;
        public void Update()
        {
            if (m_awaitKeyRelease)
            {
                if (!AnyKeyDown())
                    m_awaitKeyRelease= false;
                return;
            }
            HandleKeybindListening();
            foreach (var kvp in m_bindings)
            {
                Keybind keybind = kvp.Value;

                bool isDown = Keyboard.IsKeyDown(keybind.Key);
                bool ctrl = Keyboard.IsKeyDown(Keys.LControlKey) || Keyboard.IsKeyDown(Keys.RControlKey);
                bool shift = Keyboard.IsKeyDown(Keys.LShiftKey) || Keyboard.IsKeyDown(Keys.RShiftKey);
                bool alt = Keyboard.IsKeyDown(Keys.Alt);

                bool modifiersMatch =
                    (keybind.Modifiers.HasFlag(KeyModifiers.Ctrl) == ctrl) &&
                    (keybind.Modifiers.HasFlag(KeyModifiers.Alt) == alt) &&
                    (keybind.Modifiers.HasFlag(KeyModifiers.Shift) == shift);

                
                if( isDown && modifiersMatch)
                {
                    if(!keybind.WasPressed)
                    {
                        keybind.Action.Invoke();
                        keybind.WasPressed = true;
                    }
                }
                else
                {
                    keybind.WasPressed = false;
                }
            }
        }
        public void StartListening(string keybindName)
        {
            if(m_bindings.ContainsKey(keybindName))
                m_listeningKeybindName = keybindName;
        }
        private void HandleKeybindListening()
        {
            if (m_listeningKeybindName == null)
                return;

            bool ctrl   = Keyboard.IsKeyDown(Keys.LControlKey) || Keyboard.IsKeyDown(Keys.RControlKey);
            bool shift  = Keyboard.IsKeyDown(Keys.LShiftKey) || Keyboard.IsKeyDown(Keys.RShiftKey);
            bool alt    = Keyboard.IsKeyDown(Keys.Alt);

            Keys? detectedKey = null;
            for (int i=1; i< 256; i++)
            {
                if (Keyboard.IsKeyDown((Keys)i))
                {
                    Keys key = ((Keys)i);
                    if (IsModifierKey(key)) continue;

             
                    if (Keyboard.IsKeyDown(key))
                    {
                        detectedKey = key;
                        break;
                    }
                }
            }

            if (detectedKey == null)
                return;

            KeyModifiers modifiers = KeyModifiers.None;
            if (ctrl) modifiers |= KeyModifiers.Ctrl;
            if (alt) modifiers |= KeyModifiers.Alt;
            if (shift) modifiers |= KeyModifiers.Shift;

            Keybind keybind = m_bindings[m_listeningKeybindName];
            keybind.Key = detectedKey.Value;
            keybind.Modifiers = modifiers;
            keybind.WasPressed = true;

            SaveConfig();
            m_awaitKeyRelease = true;
            m_listeningKeybindName = null;
        }
        public void SaveConfig()
        {
            m_config.Keybinds = new Dictionary<string, Keybind>(m_bindings);
            ConfigManager.Save(m_config);
            Console.WriteLine("All keybinds have been saved");
        }
        public void SetKeybind(string name, Keys key, KeyModifiers modifiers)
        {
            if (m_bindings.TryGetValue(name, out Keybind keybind))
            {
                keybind.Key = key;
                keybind.Modifiers = modifiers;

                SaveConfig();
            }
        }
        public string FormatKeybind(KeyModifiers modifiers, Keys key)
        {
            List<string> parts = new();

            if (modifiers.HasFlag(KeyModifiers.Ctrl)) parts.Add("Ctrl");
            if (modifiers.HasFlag(KeyModifiers.Shift)) parts.Add("Shift");
            if (modifiers.HasFlag(KeyModifiers.Alt)) parts.Add("Alt");

            parts.Add(key.ToString());

            return string.Join(" + ", parts);
        }
        private bool AnyKeyDown()
        {
            for (int i = 1; i<256; i++)
                if (Keyboard.IsKeyDown((Keys)i))
                    return true;
            return false;
        }
        private bool IsModifierKey(Keys key) => key == Keys.Control
                                         || key == Keys.Alt
                                         || key == Keys.Shift
                                         || key == Keys.LControlKey
                                         || key == Keys.ControlKey
                                         || key == Keys.RControlKey
                                         || key == Keys.ShiftKey
                                         || key == Keys.RShiftKey
                                         || key == Keys.LShiftKey;

    }

    internal static class Keyboard
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        public static bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState((int)key) & 0x8000) != 0;
        }
    }
}
