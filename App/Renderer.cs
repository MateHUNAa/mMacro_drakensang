using Accessibility;
using ClickableTransparentOverlay;
using ImGuiNET;
using mMacro.Core.Functions;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace mMacro.App
{
    public class Renderer(int screenWidth = 1920, int screenHeight = 1080) : Overlay(windowWidth: screenWidth, windowHeight: screenHeight)
    {
        public Vector2 screenSize = new Vector2(screenWidth, screenHeight);
        private InventoryScan inventoryScan = InventoryScan.Instance;
        private AppConfig m_config;
        private Vector4 ColorRed = new Vector4(1, 0, 0, 1);
        private enum EditMode
        {
            None,
            Drag,
            FirstCell,
            Bag
        }
        private EditMode m_editmode = EditMode.None;
        public bool isVisible { get; set; } = true;


        protected override Task PostInitialized()
        {
            m_config = ConfigManager.Load();
            KeybindManager.Instance.Register("Toggle Panel", Keys.Insert, () => isVisible =!isVisible);
            return base.PostInitialized();
        }
        protected override void Render()
        {
            if (!isVisible) return;
            var io = ImGui.GetIO();
            var mousePos = io.MousePos;

            #region InventoryEditMode
            if (inventoryScan.EditMode)
            {
                ImGui.SetNextWindowPos(Vector2.Zero);
                ImGui.SetNextWindowSize(screenSize);
                ImGui.Begin("EditMode Overlay", ImGuiWindowFlags.NoTitleBar
                                              | ImGuiWindowFlags.NoBackground
                                              | ImGuiWindowFlags.NoBringToFrontOnFocus);

                if (m_editmode == EditMode.None)
                {
                    inventoryScan.EditMode = false;
                    Console.WriteLine("Cannot edit inventory without setting m_editmode !");
                }
                if (m_editmode == EditMode.Drag)
                {
                    if (inventoryScan.isDragging)
                    {
                        inventoryScan.UpdateDrag(mousePos);
                    }

                    if (io.MouseDown[0] && !inventoryScan.isDragging) inventoryScan.StartDrag(mousePos);
                    if (!io.MouseDown[0] && inventoryScan.isDragging) inventoryScan.EndDrag();
                    if (io.MouseDown[0] && inventoryScan.isDragging) inventoryScan.UpdateDrag(mousePos);

                    DrawDragRect(inventoryScan.dragStart, inventoryScan.dragEnd);
                }
                if (m_editmode == EditMode.FirstCell)
                {
                    DrawCursorSquare(mousePos, inventoryScan.CellSize, ColorRed);
                    if (io.MouseDown[0]) inventoryScan.SetFirstCell(mousePos);
                }
                if (m_editmode == EditMode.Bag)
                {
                    DrawCursorSquare(mousePos, inventoryScan.BagSize, ColorRed);
                    if (io.MouseDown[0]) inventoryScan.SetFirstBag(mousePos);
                }

                ImGui.End();
                return;
            }
            #endregion


            //ImGui.SetNextWindowSizeConstraints(new Vector2(600, 450), screenSize);
            ImGui.Begin("mMacro - Drakensang", ImGuiWindowFlags.NoBringToFrontOnFocus
                                             //| ImGuiWindowFlags.AlwaysAutoResize
            );

            if (!inventoryScan.EditMode)
                if (ImGui.BeginTabBar("MainTabs"))
                {
                    // ================== General Tab ==================
                    if (ImGui.BeginTabItem("General"))
                    {
                        foreach (var func in FunctionManager.Instance.Functions.Where(f => f.Mode.HasFlag(ActivationMode.Both) || f.Mode.HasFlag(ActivationMode.MenuOnly)))
                        {
                            string label = $"{(func.ExecutionType == ExecutionType.Toggleable ? (func.Enabled ? "Disable" : "Enable") : "Execute")} {func.Name} ({func.Defaultkey})";
                            if (ImGui.Button(label))
                            {
                                if (func.ExecutionType == ExecutionType.RunOnce) func.Execute();
                                if (func.ExecutionType == ExecutionType.Toggleable) func.Toggle();
                            }
                        }
                        ImGui.EndTabItem();
                    }

                    // ================== Inventory Tab ==================
                    DrawInventory();
                    // ================== Keybinds Tab ==================
                    if (ImGui.BeginTabItem("Keybinds"))
                    {
                        ImGui.Text("This is the Keybinds tab");
                        ImGui.Separator();

                        foreach (var kvp in KeybindManager.Instance.Bindings)
                        {
                            string name = kvp.Key;
                            Keybind bind = kvp.Value;

                            string keyText = KeybindManager.Instance.FormatKeybind(bind.Modifiers, bind.Key);

                            ImGui.Text($"{name}");
                            ImGui.SameLine();
                            ImGui.Text($"[{keyText}]");
                            ImGui.SameLine();

                            if (KeybindManager.Instance.IsListening(name))
                            {
                                ImGui.Text("Press any key...");
                            }
                            else if (ImGui.Button($"Change##{name}"))
                            {
                                KeybindManager.Instance.StartListening(name);
                            }
                        }
                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabBar();
                }


            if (inventoryScan.DebugMode)
            {
                DrawDragRect(inventoryScan.dragStart, inventoryScan.dragEnd);
                Vector2 firstCell = inventoryScan.firstCellPos;
                int cellSize = inventoryScan.CellSize;
                for (int col = 0; col<4; col++) 
                { 
                    for (int row =0;row<7;row++)
                    {
                        DrawCursorSquare(new Vector2(firstCell.X + cellSize * row + inventoryScan.GetOffset(row), firstCell.Y + cellSize * col + inventoryScan.GetOffset(col)), cellSize, ColorRed);
                    }
                }

                for (int i = 0; i<inventoryScan.BagCount; i++) 
                {
                    DrawCursorSquare(new Vector2(inventoryScan.firstBagPos.X +inventoryScan.BagOffsetX * (i), inventoryScan.firstBagPos.Y), inventoryScan.BagSize, new Vector4(0, 0, 1, 1));
                }
            }

            ImGui.End();
        }

        #region DrawShape
        public void DrawCursorSquare(Vector2 pos, int CellSize, Vector4 Color)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddRect(new Vector2(pos.X - CellSize/2, pos.Y - CellSize/2),
                             new Vector2(pos.X + CellSize/2, pos.Y + CellSize/2),
                             ImGui.ColorConvertFloat4ToU32(Color)
            );
        }
        public void DrawDragRect(Vector2 start, Vector2 end)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddRect(start, end, ImGui.ColorConvertFloat4ToU32(new Vector4(0, 1, 0, 1)));
        }
        #endregion

        #region DrawInventory
        private void DrawInventory()
        {
            if (ImGui.BeginTabItem("Inventory"))
            {
                if (ImGui.SliderInt("Bag Count", ref inventoryScan.BagCount, 1, 9))
                {
                    m_config.BagCount = inventoryScan.BagCount;
                    ConfigManager.Save(m_config);
                }

                ImGui.Checkbox("Debug Mode", ref inventoryScan.DebugMode);

                var scanTypes = Enum.GetNames(typeof(ScanType));
                int selectedScan = (int)inventoryScan.scanType;

                if (ImGui.Combo("Scan Type", ref selectedScan, scanTypes, scanTypes.Length))
                {
                    inventoryScan.scanType = (ScanType)selectedScan;
                    m_config.ScanType = (ScanType)selectedScan;
                    ConfigManager.Save(m_config);
                }
                ImGui.SameLine();
                ImGui.PushItemWidth(50);
                if (ImGui.SliderInt("##ScanLevel", ref inventoryScan.scanLevel, 1, 5))
                {
                    m_config.scanLevel = (int)inventoryScan.scanLevel;
                    ConfigManager.Save(m_config);
                }
                ImGui.SameLine();
                ImGui.Text("Scan Iterations");
                ImGui.PopItemWidth();

                ImGui.Separator();
                ImGui.Text("Item Colors:");

         
                ImGui.TextColored(new Vector4(1, 0, 0, 1), "Not recommended to change");
                foreach (var kvp in inventoryScan.ColorRanges)
                {
                    string name = kvp.Key;
                    var item = kvp.Value;

                    // Average color for picker
                    Vector3 avgColor = new Vector3(
                        (item.R.Item1 + item.R.Item2) / 2f / 255f,
                        (item.G.Item1 + item.G.Item2) / 2f / 255f,
                        (item.B.Item1 + item.B.Item2) / 2f / 255f
                    );

                    if (ImGui.ColorEdit3(name, ref avgColor))
                    {
                        // Update max values only
                        item.R = (item.R.Item1, Math.Clamp((int)(avgColor.X * 255f), 0, 255));
                        item.G = (item.G.Item1, Math.Clamp((int)(avgColor.Y * 255f), 0, 255));
                        item.B = (item.B.Item1, Math.Clamp((int)(avgColor.Z * 255f), 0, 255));

                        m_config.ColorRanges[name] = item;

                        ConfigManager.Save(m_config);
                    }

                    ImGui.Text($"R: {item.R.Item1}-{item.R.Item2}  G: {item.G.Item1}-{item.G.Item2}  B: {item.B.Item1}-{item.B.Item2}");
                    ImGui.Separator();
                }
                ImGui.EndTabItem();
            }
        }
        #endregion
    }
}
