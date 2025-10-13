using mMacro.Core.Managers;

namespace mMacro.Core.Models
{
    public class AppConfig
    {
        public Dictionary<string, Keybind> Keybinds { get; set; } = new();
    }
}
