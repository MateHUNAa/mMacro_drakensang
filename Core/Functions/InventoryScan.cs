using mMacro.Core.Managers;
using mMacro.Core.Models;
using System.Numerics;
using WindowsInput;


namespace mMacro.Core.Functions
{

    public struct ColorRange
    {
        public (int Min, int Max) R;
        public (int Min, int Max) G;
        public (int Min, int Max) B;

        public bool IsInRange(System.Drawing.Color color)
        {
            return color.R >= R.Min-1 && color.R <= R.Max+1 &&
                   color.G >= G.Min-1 && color.G <= G.Max+1 &&
                   color.B >= B.Min-1 && color.B <= B.Max+1;
        }
    }

    [Flags]
    public enum ScanType
    {
        Simple,
        Slow,
        Expensive
    }
    public class InventoryScan : MacroFunction
    {
        #region Constants
        private static readonly Lazy<InventoryScan> m_instance = new Lazy<InventoryScan>(() => new InventoryScan());
        private AppConfig m_config;
        public Dictionary<string, ColorRange> ColorRanges { get; set; } = new()
        {
            { "Blue", new ColorRange { R = (55, 57), G = (113, 117), B = (166, 173) } },
            { "Purple", new ColorRange { R = (150, 156), G = (42, 44), B = (79, 82) } },
            { "Gold", new ColorRange { R = (254, 255), G = (161, 162), B = (73, 75) } },
            { "Unique", new ColorRange { R = (249, 256), G = (248, 255), B = (97, 107) } },
            { "Set", new ColorRange { R = (38,78), G = (188, 199), B = (223, 227) } },
            { "Color Item", new ColorRange { R = (120, 131), G = (96, 107), B = (66, 77) } }
        };
        private InputSimulator InputSimulator = new InputSimulator();

        public static InventoryScan Instance => m_instance.Value;

        public int BagCount = 9;
        public readonly int CellSize    = 76;
        public readonly int BagSize     = 40;
        public readonly int BagOffsetX  = 49;

        public Vector2 firstCellPos;
        public Vector2 dragStart;
        public Vector2 dragEnd;
        public Vector2 firstBagPos;
        public bool isDragging  = false;
        public bool EditMode    = false;
        public bool DebugMode   = false;

        public ScanType scanType = ScanType.Expensive;
        public int scanLevel = 2;
        #endregion


        public InventoryScan() : base("Inventory Scan", Keys.F8, ActivationMode.Both, ExecutionType.RunOnce)
        {
            m_config = ConfigManager.Load();

            dragStart       = m_config.DragStart;
            dragEnd         = m_config.DragEnd;
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
            if (dragStart == Vector2.Zero || dragEnd == Vector2.Zero) return;

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
                    if (CheckPixel(color))
                    {
                        Cursor.Position = new Point((int)cellX, ((int)cellY));
                        InputSimulator.Mouse.RightButtonClick();
                    }
                }
            }
        }
        private void ScanBagMultiThreaded(int bag =0)
        {
            var cells = new List<(int col, int row)>();
            for (int col = 0; col<4;col++)
                for (int row = 0;row<7; row++)
                    cells.Add((col, row));

            int chunkSize = (int)Math.Ceiling(cells.Count / 3.0);
            var tasks = new List<Task>();

            for (int t =0; t< chunkSize; t++)
            {
                var chunk = cells.Skip(t * chunkSize).Take(chunkSize).ToList();
                tasks.Add(Task.Run(() =>
                {
                    foreach (var (col, row) in chunk)
                    {
                        float cellX = firstCellPos.X + CellSize * row + GetOffset(row);
                        float cellY = firstCellPos.Y + CellSize * col + GetOffset(col);
                        Console.WriteLine($"Bag {bag + 1}, Thread {t + 1}, Cell ({col + 1}x{row + 1})");

                        if (CheckPixel(GetPixelColor((int)cellX, (int)cellY)))
                        {
                            Cursor.Position = new Point((int)cellX, (int)cellY);
                            InputSimulator.Mouse.RightButtonClick();
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
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
        private bool CheckPixel(Color pixel)
        {
            return ColorRanges.Values.Any(c => c.IsInRange(pixel));
        }
        private Color GetPixelColor(int x, int y)
        {
            using var bmp = new Bitmap(1, 1);
            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen(x, y, 0, 0, new Size(1, 1));
            return bmp.GetPixel(0, 0);
        }
        public void SetFirstCell(Vector2 pos)
        {
            EditMode= false;
            m_config.FirstCellPosition = pos;
            firstCellPos = pos;

            ConfigManager.Save(m_config);
        }
        public void SetFirstBag(Vector2 pos)
        {
            EditMode=false;
            m_config.FirstBagPosition = pos;
            firstBagPos=pos;

            ConfigManager.Save(m_config);
        }
        #region DragFunctions
        public void StartDrag(Vector2 pos)
        {
            isDragging =true;
            dragStart= pos;
        }
        public void EndDrag()
        {
            isDragging  = false;
            EditMode    = false;

            int width = (int)Math.Abs(dragEnd.X - dragStart.X) / CellSize;
            int height = (int)Math.Abs(dragEnd.Y - dragStart.Y) / CellSize;
            Console.WriteLine($"Grid size: {width}x{height}");

            m_config.DragStart = dragStart;
            m_config.DragEnd = dragEnd;

            ConfigManager.Save(m_config);
        }
        public void UpdateDrag(Vector2 pos)
        {
            dragEnd = pos;
        }
        #endregion
    }
}
