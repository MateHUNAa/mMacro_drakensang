using App.UI.EditSession;
using Core.Attributes.Interface;
using ImGuiNET;
using mMacro.Core.Functions.Inventory;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System.Numerics;
using Color = System.Drawing.Color;

namespace App.UI.Features
{
    public class uiSellbot
    {
        private readonly AppConfig m_config     = ConfigManager.Load();
        private readonly Sellbot inventoryScan  = Sellbot.Instance;

        public void DrawInventory()
        {
            if (ImGui.BeginTabItem("Sell Bot"))
            {

                ImGui.SeparatorText("Setup");
                {

                    Vector2 buttonSize = new Vector2(ImGui.GetContentRegionAvail().X / 2 -5, 0);
                    if (ImGui.Button("Set First Cell", buttonSize))
                    {
                        var session = new ShapeEditSession();
                        session.Start(
                            shape: ShapeType.Square,
                            size: new Vector2(inventoryScan.CellSize, inventoryScan.CellSize),
                            onSet: pos => {
                                inventoryScan.firstCellPos = pos;
                                EditSessionManager.Instance.GetConfig().FirstCellPosition = pos;
                            });
                        EditSessionManager.Instance.StartSession(session);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Select the first cell of the inventory");
                    ImGui.SameLine();
                    if (ImGui.Button("Set Bag", buttonSize))
                    {

                        var session = new ShapeEditSession();
                        session.Start(
                            shape: ShapeType.Square,
                            size: new Vector2(inventoryScan.BagSize, inventoryScan.BagSize),
                            onSet: pos => {
                                inventoryScan.firstBagPos = pos;
                                EditSessionManager.Instance.GetConfig().FirstBagPosition = pos;
                            });
                        EditSessionManager.Instance.StartSession(session);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Select the first bag");
                }

                ImGui.SeparatorText("Scan Settings");
                {
                    if (ImGui.SliderInt("Bag Count", ref inventoryScan.BagCount, 1, 9))
                    {
                        m_config.BagCount = inventoryScan.BagCount;
                        ConfigManager.Save(m_config);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Number of bags to scan.");
                    var scanTypes = Enum.GetNames(typeof(ScanType));
                    int selectedScan = (int)inventoryScan.scanType;

                    if (ImGui.Combo("Scan Type", ref selectedScan, scanTypes, scanTypes.Length))
                    {
                        inventoryScan.scanType = (ScanType)selectedScan;
                        m_config.ScanType = (ScanType)selectedScan;
                        ConfigManager.Save(m_config);
                    }
                    ImGui.PushItemWidth(ImGui.CalcItemWidth());
                    if (ImGui.SliderInt("Scan Level", ref inventoryScan.scanLevel, 1, 5))
                    {
                        m_config.scanLevel = (int)inventoryScan.scanLevel;
                        ConfigManager.Save(m_config);
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Sets how many times each bag will be re-checked when using Slow Scan.");
                }

                ImGui.SeparatorText("Color Settings");
                ImGui.TextColored(new Vector4(1, 0, 0, 1), "Not recommended to change");
                foreach (var kvp in inventoryScan.ColorRanges)
                {
                    string name = kvp.Key;
                    var item = kvp.Value;

                    Vector4 avgColor = new Vector4(
                        (item.R.Item1 + item.R.Item2) / 2f / 255f,
                        (item.G.Item1 + item.G.Item2) / 2f / 255f,
                        (item.B.Item1 + item.B.Item2) / 2f / 255f,
                        255
                    );

                    if (ImGui.ColorButton(name, avgColor))
                    {

                        var session = new ShapeEditSession();
                        session.Start(
                            shape: ShapeType.Square,
                            size: new Vector2(inventoryScan.CellSize, inventoryScan.CellSize),
                            color: new Vector4(255,0,255,255),
                            onSet: pos => {
                                Color pickedColor = PixelUtils.GetPixelColor(pos);

                                avgColor = new Vector4(
                                    pickedColor.R,
                                    pickedColor.G,
                                    pickedColor.B,
                                    pickedColor.A
                                );
                                var range = inventoryScan.ColorRanges[kvp.Key];
                                range.R = (Math.Min(range.R.Min, pickedColor.R)-5, Math.Max(range.R.Max, pickedColor.R)+5);
                                range.G = (Math.Min(range.G.Min, pickedColor.G)-5, Math.Max(range.G.Max, pickedColor.G)+5);
                                range.B = (Math.Min(range.B.Min, pickedColor.B)-5, Math.Max(range.B.Max, pickedColor.B) + 5);
                                inventoryScan.ColorRanges[kvp.Key] = range;
                            });
                        EditSessionManager.Instance.StartSession(session);
                    }
                    ImGui.SameLine();
                    ImGui.Text(name);
                }
                ImGui.EndTabItem();
            }
        }
    }
}
