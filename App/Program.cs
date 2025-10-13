using mMacro.Core.Functions;
using mMacro.Core.Managers;
using System.Numerics;

namespace mMacro.App
{
    internal class Program
    {
        private static readonly KeybindManager keybindManager = new KeybindManager();
        static void Main()
        {
            Console.Title = "mMacro - Drakensang Online";

            AutoClicker autoClicker = new AutoClicker();
            Renderer renderer = new Renderer();

            keybindManager.Register("Left Clicker", autoClicker.Left.Keybind, autoClicker.Left.Toggle);
            keybindManager.Register("Right Clicker", autoClicker.Right.Keybind, autoClicker.Right.Toggle);

            Thread renderThread = new Thread(new ThreadStart(renderer.Start().Wait));
            renderThread.Start();

            Vector2 screenSize = renderer.screenSize;

            while (true) {
                keybindManager.Update();
                autoClicker.ExecuteEnabled();
                Thread.Sleep(1);
            }
         
        }
    }
}
