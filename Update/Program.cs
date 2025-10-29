using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;

namespace Update
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string appFolder = AppDomain.CurrentDomain.BaseDirectory;
            string mainExe = Path.Combine(appFolder, "App.exe");
            string versionFile = Path.Combine(appFolder, "version.txt");
            string repo = "MateHUNAa/mMacro_drakensang";
            string currentVersion = File.Exists(versionFile) ? File.ReadAllText(versionFile).Trim() : "0.0.0.0";


            string timestamp = DateTime.Now.ToString("yyyyMMdd");
            string logfile = Path.Combine(appFolder, $"update_log_{timestamp}.txt");

            using var logWriter = new StreamWriter(logfile, append: false);
            using var multiWriter = new MultiTextWriter(Console.Out, logWriter);
            Console.SetOut(multiWriter);

            Console.WriteLine($"=== mMacro Updater started at {DateTime.Now} ===");

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "mMacro-Update");

            HttpResponseMessage responseMessage;
            try
            {
                responseMessage = await client.GetAsync($"https://api.github.com/repos/{repo}/releases/latest");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                return;
            }

            if (!responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("[Update]: RequestError ->\n" + responseMessage);
                return;
            }


            var response = await responseMessage.Content.ReadAsStringAsync();
          
             JObject root = JObject.Parse(response);

            string latestVersion = root.Value<string>("tag_name");
            var match = Regex.Match(latestVersion ?? "", @"\d+\.\d+\.\d+(?:\.\d+)?");
            if (!match.Success) return;
            latestVersion = match.Value;


            if (!IsNewerVersion(currentVersion, latestVersion))
            {
                Console.WriteLine("App is up to date.");
                return;
            }


            Console.WriteLine($"New version available: {latestVersion}");

            var assets = root["assets"] as JArray;
            if (assets == null || assets.Count == 0)
            {
                Console.WriteLine("No assets found for the release!");
                return;
            }

            var zipAsset = assets.FirstOrDefault(a => a.Value<string>("name").EndsWith(".zip"));
            if (zipAsset == null)
            {
                Console.WriteLine("No ZIP asset found!");
                return;
            }

            string downloadUrl = zipAsset.Value<string>("browser_download_url");

            string tempZip = Path.Combine(Path.GetTempPath(), "mMacro.zip");
            byte[] data = await client.GetByteArrayAsync(downloadUrl);
            await File.WriteAllBytesAsync(tempZip, data);
            string tempExtract = Path.Combine(Path.GetTempPath(), "mMacroUpdate");
            if (Directory.Exists(tempExtract)) Directory.Delete(tempExtract, true);
            ZipFile.ExtractToDirectory(tempZip, tempExtract);
            File.Delete(tempZip);

            string netFolder = Directory.GetDirectories(tempExtract, "net8.0-windows", SearchOption.AllDirectories).FirstOrDefault();
            if (netFolder == null) throw new Exception("net8.0-windows folder not found in ZIP!");

            KillMainIfRunning(mainExe);

            foreach (string file in Directory.GetFiles(netFolder))
            {
                string dest = Path.Combine(appFolder, Path.GetFileName(file));
                File.Copy(file, dest, true);
            }

            Directory.Delete(tempExtract, true);

            File.WriteAllText(versionFile, latestVersion);

            Process.Start(mainExe);
            Console.WriteLine("Update applied and app restarted.");

            Console.WriteLine($"=== Update finished successfully at {DateTime.Now} ===");
            return;
        }
        static bool IsNewerVersion(string current, string latest)
        {
            var cv = current.Split('.');
            var lv = latest.Split('.');
            int length = Math.Max(cv.Length, lv.Length);

            for (int i = 0; i < length; i++)
            {
                int c = i < cv.Length ? int.Parse(cv[i]) : 0;
                int l = i < lv.Length ? int.Parse(lv[i]) : 0;

                if (c < l) return true;
                if (c > l) return false;
            }
            return false;
        }

        static bool IsFileLocked(string filePath)
        {
            try
            {
                using var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                return false;
            }
            catch { return true; }
        }

        static void KillMainIfRunning(string mainExeName)
        {
            string exeName = Path.GetFileNameWithoutExtension(mainExeName);
            var processes = Process.GetProcessesByName(exeName);

            Console.WriteLine("Trying to kill the main process;");
            foreach (var proc in processes)
            {
                try
                {
                    proc.Kill();
                    proc.WaitForExit();
                    Console.WriteLine($"Killed process: {proc.ProcessName} (PID {proc.Id})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not kill process {proc.ProcessName}: {ex.Message}");
                }
            }
        }


    }
}
