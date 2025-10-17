using mMacro.Core.Functions;
using mMacro.Core.Functions.Inventory;
using mMacro.Core.Managers;
using System.Numerics;

namespace mMacro.App
{
    internal class Program
    {
        private static KeybindManager keybindManager;
        private static FunctionManager functionManager;
        static void Main()
        {
            keybindManager = new KeybindManager();
            functionManager = new FunctionManager();

            Sellbot inventoryScan   = Sellbot.Instance;
            SwapCape swapCape       = SwapCape.Instance;
            ReviveBot revivebot     = ReviveBot.Instance;
            AutoPotion autoPotion   = AutoPotion.Instance;
            Meltbot meltBot         = Meltbot.Instance;
            Meltgem metlGem         = Meltgem.Instance;
            AutoClicker autoClicker     = new AutoClicker();
            Renderer renderer           = new Renderer();


            Thread renderThread = new Thread(new ThreadStart(renderer.Start().Wait));
            renderThread.Start();

            Vector2 screenSize = renderer.screenSize;

            Task.Delay(800).Wait();
            while (true) {
                keybindManager.Update();
                functionManager.ExecuteEnabled();
                Thread.Sleep(1);
            }
        }
    }
}
