using mMacro.Core.Functions;
using mMacro.Core.Functions.Inventory;
using mMacro.Core.Managers;
using System.Net;
using System.Numerics;
using System.Reflection;

namespace mMacro.App
{
    internal class Program
    {
        private static KeybindManager keybindManager;
        private static FunctionManager functionManager;
        static void Main()
        {
            string appFolder = AppDomain.CurrentDomain.BaseDirectory;
            string versionFile = Path.Combine(appFolder, "version.txt");

                string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";
                File.WriteAllText(versionFile, version);

            StartUpdater(appFolder);


            keybindManager = new KeybindManager();
            functionManager = new FunctionManager();

            Sellbot inventoryScan   = Sellbot.Instance;
            SwapCape swapCape       = SwapCape.Instance;
            ReviveBot revivebot     = ReviveBot.Instance;
            AutoPotion autoPotion   = AutoPotion.Instance;
            MeltItems meltBot         = MeltItems.Instance;
            MeltGems metlGem         = MeltGems.Instance;
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

        static void StartUpdater(string appFolder)
        {
            string updaterPath = Path.Combine(appFolder, "Update.exe");
            if (File.Exists(updaterPath))
            {
                try
                {
                    Console.WriteLine("Starting 'Update.exe' !");
                    System.Diagnostics.Process.Start(updaterPath);
                } catch(Exception ex)
                {
                    Console.WriteLine($"Failed to start updater: {ex.Message}");
                }
            }
        }
    }
}
