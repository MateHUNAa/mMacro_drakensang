using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace mMacro.Core.Utils
{
    public static class GridHelper
    {
        public static PointF GetCellScreenPosition(
                int col,
                int row,
                PointF firstCellPos,
                float cellSize = 76
            )
        {
            row = Math.Clamp(row, 0, 7);
            col = Math.Clamp(col, 0, 4);

            float x = firstCellPos.X + cellSize * row + GetOffset(row);
            float y = firstCellPos.Y + cellSize * col + GetOffset(col);

            return new PointF(x, y);
        }
        public static Vector2 GetBagScreenPosition(
                int bag,
                Vector2 firstBagPos,
                float offset = 49
            )
        {
            return new Vector2(firstBagPos.X + offset * bag, firstBagPos.Y);
        }
        public static int GetOffset(int val)
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
