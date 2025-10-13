using mMacro.Core.Managers;
using mMacro.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace mMacro.Core.Functions
{
    public class InventoryScan : MacroFunction
    {
        #region Constants
        private static readonly Lazy<InventoryScan> m_instance = new Lazy<InventoryScan>(() => new InventoryScan());
        private AppConfig m_config;
        public static InventoryScan Instance => m_instance.Value;
        public readonly int CellSize = 76;
        public Vector2 firstCellPos;
        public bool isDragging = false;
        public Vector2 dragStart;
        public Vector2 dragEnd;
        public bool EditMode = false;
        public bool DebugMode = true;
        #endregion


        public InventoryScan() : base("Inventory Scan", Keys.F8, ActivationMode.Both, ExecutionType.RunOnce)
        {
            m_config = ConfigManager.Load();

            dragStart = m_config.DragStart;
            dragEnd = m_config.DragEnd;
            firstCellPos = m_config.FirstCellPosition;
        }
        public override void Execute()
        {
            ScanInventory();
        }
        

        private void ScanInventory()
        {
            if (dragStart == Vector2.Zero || dragEnd == Vector2.Zero) return;
           
        }
        public void SetFirstCell(Vector2 pos)
        {
            EditMode= false;
            m_config.FirstCellPosition = pos;

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
            isDragging=false;
            EditMode=false;

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
