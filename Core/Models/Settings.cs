using mMacro.Core.Managers;

namespace mMacro.Core.Models
{
    public class Settings
    {
        public class Timeings
        {
            public int MeltItemClickDelay { get; set; } = 150;
            public int MeltGemClickDelay { get; set; } = 90;
            public int MeltGemTaskEndDelay { get; set; } = 2000;
            public int SwapCapeClickDelay { get; set; } = 75;
        }

        public class Offsets
        {
            public int ColorTolarence { get; set; } = 5;
            public int BagOffsetX { get; set; } = 49;
        }

        public class Sizes
        {
            public int CellSize { get; set; } = 76;
            public int BagSize { get; set; } = 40;
        }

        public Timeings m_Timeings { get; set; } = new Timeings();
        public Offsets m_Offsets { get; set; } = new Offsets();
        public Sizes m_Sizes { get; set; } = new Sizes();

        private static Settings? m_instance;
        public static Settings Instance
        {
            get
            {
                if (m_instance == null)
                {
                    var config = ConfigManager.Load();
                    m_instance = config?.Settings ?? new Settings();
                }
                return m_instance;
            }
        }

    }
}
