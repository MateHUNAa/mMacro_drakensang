using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mMacro.Core.Models;
using Newtonsoft.Json;

namespace mMacro.Core.Managers
{
    public static class ConfigManager
    {
        private const string ConfigPath = "config.json";

        public static AppConfig Load()
        {
            if (!File.Exists(ConfigPath))
                return new AppConfig();

            var json = File.ReadAllText(ConfigPath);
            return JsonConvert.DeserializeObject<AppConfig>(json) ?? new AppConfig();
        }

        public static void Save(AppConfig config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
        }
    }
}
