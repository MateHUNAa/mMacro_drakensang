using mMacro.Core.Functions;
using mMacro.Core.Functions.Inventory;
using mMacro.Core.Managers;
using System.Reflection;
using Update;

namespace mMacro.App
{
    internal class Program
    {
        private static KeybindManager keybindManager;
        private static FunctionManager functionManager;
        static void Main()
        {
            string appFolder = AppDomain.CurrentDomain.BaseDirectory;

            #region Logging Setup
            string logfile = Path.Combine(appFolder, $"applog.txt");
            using var logWriter = new StreamWriter(logfile, append: true) { AutoFlush = true };
            using var multiWriter = new MultiTextWriter(Console.Out, logWriter);
            Console.SetOut(multiWriter);

            LogEntry();
            #endregion

            #region Versioning
            string versionFile = Path.Combine(appFolder, "version.txt");

            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";
            File.WriteAllText(versionFile, version);
            StartUpdater(appFolder);
            #endregion

            #region Initialize Managers
            keybindManager = new KeybindManager();
            functionManager = new FunctionManager();

            Sellbot inventoryScan   = Sellbot.Instance;
            SwapCape swapCape       = SwapCape.Instance;
            ReviveBot revivebot     = ReviveBot.Instance;
            AutoPotion autoPotion   = AutoPotion.Instance;
            MeltItems meltBot         = MeltItems.Instance;
            MeltGems metlGem         = MeltGems.Instance;
            AutoClicker autoClicker     = new AutoClicker();
            #endregion

            #region Calcualte Screen Size
            int totalWidth =0;
            int totalHeight=0;

            foreach (var screen in Screen.AllScreens)
            {
                totalWidth += screen.Bounds.Width;
                totalHeight += screen.Bounds.Height;
            }
            #endregion

    
            Renderer renderer = new Renderer(totalWidth, totalHeight);
            Thread renderThread = new Thread(new ThreadStart(renderer.Start().Wait));

            renderThread.Start();
            Task.Delay(800).Wait();
            while (true) {
                keybindManager.Update();
                functionManager.ExecuteEnabled();
                Thread.Sleep(1);
            }
        }

        private static void StartUpdater(string appFolder)
        {
            string updaterPath = Path.Combine(appFolder, "Update.exe");
            if (File.Exists(updaterPath))
            {
                try
                {
                    Console.WriteLine("Starting 'Update.exe'");
                    System.Diagnostics.Process.Start(updaterPath);
                } catch(Exception ex)
                {
                    Console.WriteLine($"Failed to start updater: {ex.Message}");
                }
            }
        }
        private static void LogEntry()
        {
            Console.WriteLine($"=========== Application Started ===========");
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]");
            Console.WriteLine($"=========== Application Started ===========");
            Console.WriteLine();
        }
    }
}
