using mMacro.Core.Functions;
using mMacro.Core.Managers;
using System.Numerics;

namespace mMacro.App
{
    internal class Program
    {
        private static KeybindManager keybindManager;
        private static FunctionManager functionManager;
        private static ReviveBot reviveBot;
        static void Main()
        {
            keybindManager = new KeybindManager();
            functionManager = new FunctionManager();
            InventoryScan inventoryScan = InventoryScan.Instance;
            reviveBot = new ReviveBot();


            Console.Title = "mMacro - Drakensang Online";
            AutoClicker autoClicker = new AutoClicker();
            Renderer renderer = new Renderer();
            Thread renderThread = new Thread(new ThreadStart(renderer.Start().Wait));
            renderThread.Start();

            Vector2 screenSize = renderer.screenSize;

            while (true) {
                keybindManager.Update();
                functionManager.ExecuteEnabled();
                Thread.Sleep(1);
            }
         
        }
    }
}
