using System.Numerics;
using WindowsInput;

namespace mMacro.Core.Utils
{

    public enum ClickType
    {
        LEFT,
        RIGHT,
        LEFT_DOUBLE,
        RIGHT_DOUBLE,
    }
    public static class Click
    {
        private static readonly InputSimulator sim = new InputSimulator();

        public static void ClickAt(Vector2 pos, ClickType type = ClickType.LEFT)
        {
            Cursor.Position = new Point((int)pos.X, (int)pos.Y);

            switch (type)
            {
                case ClickType.LEFT:
                    sim.Mouse.LeftButtonClick();
                    break;
                case ClickType.RIGHT:
                    sim.Mouse.RightButtonClick();
                    break;
                case ClickType.LEFT_DOUBLE:
                    sim.Mouse.LeftButtonDoubleClick();
                    break;
                case ClickType.RIGHT_DOUBLE:
                    sim.Mouse.RightButtonDoubleClick();
                    break;
            }
        }
        public static void ClickAt(Point pos, ClickType type = ClickType.LEFT) =>
            ClickAt(new Vector2(pos.X, pos.Y), type);

        public static void ClickAt(int x, int y, ClickType type = ClickType.LEFT) =>
            ClickAt(new Vector2(x, y), type);


        public static async Task ClickAtAsync(Vector2 pos, int delayMs = 50, ClickType type = ClickType.LEFT)
        {
            Cursor.Position = new Point((int)pos.X, (int)pos.Y);

            if (delayMs > 0) await Task.Delay(delayMs);

            switch (type)
            {
                case ClickType.LEFT:
                    sim.Mouse.LeftButtonClick();
                    break;
                case ClickType.RIGHT:
                    sim.Mouse.RightButtonClick();
                    break;
                case ClickType.LEFT_DOUBLE:
                    sim.Mouse.LeftButtonDoubleClick();
                    break;
                case ClickType.RIGHT_DOUBLE:
                    sim.Mouse.RightButtonDoubleClick();
                    break;
            }

            await Task.Delay(delayMs);
                    
        }
        public static async Task ClickAtAsync(Point pos, int delayMs = 50 ,ClickType type = ClickType.LEFT) =>
          await ClickAtAsync(new Vector2(pos.X, pos.Y), delayMs, type);

        public static async Task ClickAtAsync(int x, int y, int delayMs = 50,ClickType type = ClickType.LEFT) =>
            await ClickAtAsync(new Vector2(x, y), delayMs, type);

    }
}
