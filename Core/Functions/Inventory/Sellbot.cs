using mMacro.Core.Managers;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System.Collections.Concurrent;
using System.Numerics;
using WindowsInput;
using static mMacro.Core.Utils.PixelUtils;


namespace mMacro.Core.Functions.Inventory
{
    [Flags]
    public enum ScanType
    {
        Simple,
        Slow,
        Expensive
    }
    public class Sellbot : SingletonMacroFunction<Sellbot>
    {
        #region Constants
        private AppConfig m_config;
        public Dictionary<string, ColorRange> ColorRanges { get; set; } = new()
        {
            { "Blue", new ColorRange { R = (55, 60), G = (113, 120), B = (166, 179) } },
            { "Purple", new ColorRange { R = (150, 162), G = (42, 47), B = (79, 85) } },
            { "Gold", new ColorRange { R = (254, 255), G = (161, 170), B = (73, 77) } },
            { "Unique", new ColorRange { R = (248, 256), G = (248, 255), B = (40, 60) } },
            { "Set", new ColorRange { R = (38,78), G = (188, 199), B = (223, 227) } },
            { "Color Item", new ColorRange { R = (120, 131), G = (96, 107), B = (66, 77) } }
        };
        private InputSimulator InputSimulator = new InputSimulator();


        public int BagCount             = 9;
        public readonly int CellSize    = 76;
        public readonly int BagSize     = 40;
        public readonly int BagOffsetX  = 49;

        public Vector2 firstCellPos;
        public Vector2 firstBagPos;
        public bool isDragging  = false;
        public bool EditMode    = false;
        public bool DebugMode   = false;

        public ScanType scanType = ScanType.Expensive;
        public int scanLevel = 2;
        #endregion


        public Sellbot() : base("Inventory Scan", Keys.F8, ActivationMode.Both, ExecutionType.RunOnce)
        {
            m_config = ConfigManager.Load();
            firstCellPos    = m_config.FirstCellPosition;
            firstBagPos     = m_config.FirstBagPosition;
            BagCount        = m_config.BagCount;
            scanLevel       = m_config.scanLevel;
            scanType        = m_config.ScanType;
            ColorRanges     = m_config.ColorRanges;

            KeybindManager.Instance.Register("Scan All Bag", Keys.F10, ScanAllBag);
        }
        public override void Execute()
        {
            ScanInventory();
        }
        

        private void ScanInventory(int bag=0)
        {
            Console.WriteLine($"Scanning Bag {bag}");

            switch (scanType)
            {
                case ScanType.Simple:
                    ScanBagOnce();
                    break;
                case ScanType.Slow:
                    for (int i = 0; i < scanLevel; i++) 
                    {
                        ScanBagOnce();
                    }
                    break;
                case ScanType.Expensive:
                    ScanBagMultiThreaded(bag);
                    break;
            }
        }
        private void ScanAllBag()
        {
            for (int bag = 0; bag < BagCount; bag++)
            {
                Point bagPos = new Point((int)firstBagPos.X + BagOffsetX * bag, (int)firstBagPos.Y);
                Cursor.Position = bagPos;
                InputSimulator.Mouse.LeftButtonClick();

                ScanInventory(bag+1);

            }
        }
        public Vector2 GetBagPosition(int bag=0)
        {
            return new Vector2(firstBagPos.X + BagOffsetX * bag, firstBagPos.Y);
        }
        private void ScanBagOnce()
        {
            for (int col = 0; col<4; col++)
            {
                for (int row = 0; row<7; row++)
                {
                    float cellX = firstCellPos.X + CellSize * row + GetOffset(row);
                    float cellY = firstCellPos.Y + CellSize * col + GetOffset(col);

                    var color = GetPixelColor((int)cellX, (int)cellY);
                    Console.WriteLine($"({col+1}x{row+1})Color: {color}");
                    if (CheckPixel(color, ColorRanges))
                    {
                        Cursor.Position = new Point((int)cellX, (int)cellY);
                        InputSimulator.Mouse.RightButtonClick();
                    }
                }
            }
        }

        public async void ScanBagOnce(Func<Vector2, Task> OnFound, Dictionary<string, ColorRange> cRanges)
        {
            for (int col = 0; col<4; col++)
            {
                for (int row = 0; row<7; row++)
                {
                    float cellX = firstCellPos.X + CellSize * row + GetOffset(row);
                    float cellY = firstCellPos.Y + CellSize * col + GetOffset(col);

                    var color = GetPixelColor((int)cellX, (int)cellY);
                    Console.WriteLine($"({col+1}x{row+1}) {color}");
                    if (CheckPixel(color, cRanges))
                    {
                        await OnFound(new Vector2(cellX, cellY));
                    }
                }
            }
        }
        public void ScanBagOnce(Action<Vector2> OnFound, ColorRange cRanges) => ScanBagOnce(OnFound, cRanges);

        public List<Vector2> ScanBag(int bag=0)
        {
            List<Vector2> foundItems = new();
            for (int col = 0; col<4; col++)
            {
                for (int row = 0; row<7; row++)
                {
                    float cellX = firstCellPos.X + CellSize * row + GetOffset(row);
                    float cellY = firstCellPos.Y + CellSize * col + GetOffset(col);

                    var color = GetPixelColor((int)cellX, (int)cellY);
                    Console.WriteLine($"({col+1}x{row+1})Color: {color}");
                    if (CheckPixel(color, ColorRanges))
                    {
                        foundItems.Add(new Vector2(cellX, cellY));
                    }
                }
            }
            return foundItems;
        }
        public List<Vector2> ScanBagMultiThreaded(int bag =0)
        {
            var cells = new List<(int col, int row)>();
            for (int col = 0; col<4;col++)
                for (int row = 0;row<7; row++)
                    cells.Add((col, row));

            int numThreads = 3;
            int chunkSize = (int)Math.Ceiling(cells.Count / (double)numThreads);
            var tasks = new List<Task>();

            var foundItems = new ConcurrentBag<Vector2>();


            for (int t = 0; t < numThreads; t++)
            {
                var chunk = cells.Skip(t * chunkSize).Take(chunkSize).ToList();
                tasks.Add(Task.Run(() =>
                {
                    foreach (var (col, row) in chunk)
                    {
                        float cellX = firstCellPos.X + CellSize * row + GetOffset(row);
                        float cellY = firstCellPos.Y + CellSize * col + GetOffset(col);
                        Console.WriteLine($"Bag {bag + 1}, Thread {t + 1}, Cell ({col + 1}x{row + 1})");

                        if (CheckPixel(GetPixelColor((int)cellX, (int)cellY), ColorRanges))
                        {
                            foundItems.Add(new Vector2(cellX, cellY));
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            return foundItems.ToList();
        }
        public int GetOffset(int val) 
        {
            if (val ==0) return 0;
            if (val==1) return 5;
            if (val==2) return 9;
            if (val==3) return 14;
            if (val==4) return 18;

            return 18+ (val-4)*5;
        }
    }
}
