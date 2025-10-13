using ClickableTransparentOverlay;
using ImGuiNET;
using mMacro.Core.Functions;
using mMacro.Core.Managers;
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

        private enum EditMode
        {
            None,
            Drag,
            Click
        }

        private EditMode m_editmode = EditMode.None;
        protected override void Render()
        {
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

                if (m_editmode == EditMode.Click)
                {
                    DrawCursorSquare(mousePos);
                    if (io.MouseDown[0]) inventoryScan.SetFirstCell(mousePos);
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

                ImGui.End();
                return;
            }
            #endregion


            ImGui.SetNextWindowSizeConstraints(new Vector2(600, 450), screenSize);
            ImGui.Begin("mMacro - Drakensang", ImGuiWindowFlags.NoBringToFrontOnFocus
                                             | ImGuiWindowFlags.NoCollapse
                                             | ImGuiWindowFlags.NoScrollbar
                                             | ImGuiWindowFlags.NoDecoration
                                             | ImGuiWindowFlags.AlwaysAutoResize
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
                    if (ImGui.BeginTabItem("Inventory"))
                    {
                        ImGui.Text("Drag over Inventory grid");
                        if (ImGui.Button("Start Dragging"))
                        {
                            m_editmode = EditMode.Drag;
                            inventoryScan.EditMode = !inventoryScan.EditMode;
                        }

                        ImGui.Text("Save first inventory cell");
                        if (ImGui.Button("Save Cell"))
                        {
                            m_editmode = EditMode.Click;
                            inventoryScan.EditMode = !inventoryScan.EditMode;
                        }
                        ImGui.EndTabItem();
                    }

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
                DrawCursorSquare(inventoryScan.firstCellPos);
            }

            ImGui.End();
        }
        public void DrawCursorSquare(Vector2 pos)
        {
            var drawList = ImGui.GetForegroundDrawList();

            int CellSize = inventoryScan.CellSize;
            drawList.AddRect(new Vector2(pos.X - CellSize/2, pos.Y - CellSize/2),
                             new Vector2(pos.X + CellSize/2, pos.Y + CellSize/2),
                             ImGui.ColorConvertFloat4ToU32(new Vector4(1, 0, 0, 1))
            );
        }
        public void DrawDragRect(Vector2 start, Vector2 end)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddRect(start, end, ImGui.ColorConvertFloat4ToU32(new Vector4(0, 1, 0, 1)));
        }
    }
}
