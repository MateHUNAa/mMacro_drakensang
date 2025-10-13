using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mMacro.Core.Managers
{
    public abstract class MacroFunction
    {
        public string Name { get; }
        public Keys Keybind { get; set; } = Keys.None;
        public bool Enabled { get; set; } = false;

        protected MacroFunction(string name, Keys defaultKey)
        {
            Name = name;
            Keybind = defaultKey;
        }

        public void Toggle() => Enabled = !Enabled;
        public abstract void Execute();
    }
}
