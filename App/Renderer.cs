using ClickableTransparentOverlay;
using ImGuiNET;
using mMacro.Core.Functions;
using mMacro.Core.Functions.Inventory;
using mMacro.Core.Managers;
using mMacro.Core.Models;
using mMacro.Core.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using WindowsInput.Native;

namespace mMacro.App
{

    public class Renderer(int screenWidth = 1920, int screenHeight = 1080) : Overlay(windowWidth: screenWidth, windowHeight: screenHeight)
    {

        private class EditSession
        {
            public bool Active { get; set; } = false;
            public EditMode Mode { get; set; } = EditMode.None;
            public Vector2 Size { get; set; } = new Vector2(10,10);
            public float Radius { get; set; } = 10;
            public Action<Vector2> OnSet { get; set; } = null;
        }

        public Vector2 screenSize = new Vector2(screenWidth, screenHeight);
        private Sellbot inventoryScan = Sellbot.Instance;
        private SwapCape swapCape = SwapCape.Instance;
        private ReviveBot reviveBot = ReviveBot.Instance;
        private Meltbot meltBot = Meltbot.Instance;
        private AppConfig m_config;
        private Vector4 ColorRed = new Vector4(1, 0, 0, 1);
        private bool DebugMode = true;
        private bool editMode = false;
        private EditSession editSession = new EditSession();
        private enum EditMode
        {
            None,
            FirstCell,
            Bag,
            SetClose,
            Player,
            MeltBot
        }
        private EditMode m_editmode = EditMode.None;
        public bool isVisible { get; set; } = true;
        private int delay = 1;
        protected override Task PostInitialized()
        {
            m_config = ConfigManager.Load();
            delay = m_config.ClickDelay;
            KeybindManager.Instance.Register("Toggle Panel", Keys.Insert, () => isVisible =!isVisible);
            return base.PostInitialized();
        }
        protected override void Render()
        {
            if (!isVisible) return;
            var io = ImGui.GetIO();
            var mousePos = io.MousePos;
            HandleEditSession(mousePos);
            
            #region EditMode
            if (editSession.Active)
            {
                ImGui.SetNextWindowPos(Vector2.Zero);
                ImGui.SetNextWindowSize(screenSize);
                ImGui.Begin("EditMode Overlay", ImGuiWindowFlags.NoTitleBar
                                              | ImGuiWindowFlags.NoBackground
                                              | ImGuiWindowFlags.NoBringToFrontOnFocus);
                ImGui.End();
                return;
            }
            #endregion

            ImGui.Begin("mMacro - Drakensang", ImGuiWindowFlags.NoBringToFrontOnFocus
                                             //| ImGuiWindowFlags.AlwaysAutoResize
            );

            if (!editMode)
            {
                if (ImGui.BeginTabBar("MainTabs"))
                {
                    // ================== General Tab ==================
                    if (ImGui.BeginTabItem("General"))
                    {
                        int i = 0;
                        Vector2 size = new Vector2(ImGui.GetContentRegionAvail().X/2-10, 0);
                        foreach (var func in FunctionManager.Instance.Functions.Where(f => f.Mode.HasFlag(ActivationMode.Both) || f.Mode.HasFlag(ActivationMode.MenuOnly)))
                        {
                            
                            string label = $"{(func.ExecutionType == ExecutionType.Toggleable ? (func.Enabled ? "Disable" : "Enable") : "Execute")} {func.Name} ({func.Defaultkey})";
                            if (ImGui.Button(label, size))
                            {
                                if (func.ExecutionType == ExecutionType.RunOnce) func.Execute();
                                if (func.ExecutionType == ExecutionType.Toggleable) func.Toggle();
                            }
                            if (i%2==0) ImGui.SameLine();
                            i++;
                        }
                        ImGui.Separator();
                        if (ImGui.InputInt("Clicker Delay(ms)", ref delay, 1, 10000))
                        {
                            AutoClicker.Instance.SetDelay(delay);
                        }
                        ImGui.Checkbox("Debug Mode", ref DebugMode);

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

                            float fullWidth = ImGui.GetContentRegionAvail().X;

                            float nameWidth = ImGui.CalcTextSize(name).X;
                            float keyWidth  = ImGui.CalcTextSize($"[{keyText}]").X;
                            float buttonWidth = 80.0f;

                            float spaceing = (fullWidth - (nameWidth + keyWidth + buttonWidth)) /2;

                            ImGui.Text($"{name}");
                            ImGui.SameLine((float)((nameWidth + spaceing *1.5) /1.5));
                            ImGui.Text($"[{keyText}]");
                            ImGui.SameLine(nameWidth + spaceing + keyWidth +spaceing - buttonWidth /2);

                            if (KeybindManager.Instance.IsListening(name))
                            {
                                ImGui.Text("Press any key...");
                            }
                            else if (ImGui.Button($"Change##{name}", new Vector2(buttonWidth, 0)))
                            {
                                KeybindManager.Instance.StartListening(name);
                            }
                        }
                        ImGui.EndTabItem();
                    }
                    // ================== SwapCape Tab ==================
                    DrawSwapCape();
                    // ================== Revive Bot ==================
                    DrawReviveBot();
                
                    if(ImGui.BeginTabItem("Inventory"))
                    {
                        if (ImGui.BeginTabBar("Inventory"))
                        {
                            // ================== Sell Bot ==================
                            DrawInventory();
                            // ================== Melt Bot ==================
                            DrawMeltBot();

                            ImGui.EndTabBar();

                        }
                        ImGui.EndTabItem();
                    }
                    ImGui.EndTabBar();
                }
            }


            if (DebugMode)
            {
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

                // Player Section

                Vector2 FirstPlayer = reviveBot.FirstPlayerPos;
                
                for (int i=0;i<5;i++)
                {
                    bool blocked = reviveBot.m_blockOffers[i];
                    DrawCursorCircle(new Vector2(FirstPlayer.X, FirstPlayer.Y + reviveBot.Offset * i), reviveBot.Radius, blocked ? new Vector4(1, 0, 1, 1) : new Vector4(1, 1, 0, 1));
                }

                // Melt Bot

                for (int  row=0;row<3;row++)
                {
                    for (int col=0;col<3;col++)
                    {
                        DrawCursorSquare(new Vector2(meltBot.MeltFirstPos.X +  meltBot.CellSize* row + GridHelper.GetOffset(row), meltBot.MeltFirstPos.Y + meltBot.CellSize * col +GridHelper.GetOffset(col)), meltBot.CellSize, new Vector4(0.2f,0.5f,1,1));
                    }
                }

                DrawCursorSquare(meltBot.MeltButtonPos, meltBot.MeltButtonSize, ColorRed);
                DrawCursorSquare(meltBot.MeltCloseBtn, 30, ColorRed);
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
        public void DrawCursorSquare(Vector2 pos, Vector2 CellSize, Vector4 Color)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddRect(new Vector2(pos.X - CellSize.X/2, pos.Y - CellSize.Y/2),
                             new Vector2(pos.X + CellSize.X/2, pos.Y + CellSize.Y/2),
                             ImGui.ColorConvertFloat4ToU32(Color)
            );
        }
        public void DrawDragRect(Vector2 start, Vector2 end)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddRect(start, end, ImGui.ColorConvertFloat4ToU32(new Vector4(0, 1, 0, 1)));
        }
        public void DrawCursorCircle(Vector2 pos, float radius, Vector4 color, int segments = 32)
        {
            var drawList = ImGui.GetForegroundDrawList();

            drawList.AddCircle(
                pos,             
                radius,                  
                ImGui.ColorConvertFloat4ToU32(color), 
                segments,                 
                1.0f                       
            );
        }
        #endregion


        #region DrawInventory
        private void DrawInventory()
        {
            if (ImGui.BeginTabItem("Sell Bot"))
            {
                if (ImGui.SliderInt("Bag Count", ref inventoryScan.BagCount, 1, 9))
                {
                    m_config.BagCount = inventoryScan.BagCount;
                    ConfigManager.Save(m_config);
                }

                var scanTypes = Enum.GetNames(typeof(ScanType));
                int selectedScan = (int)inventoryScan.scanType;

                if (ImGui.Combo("Scan Type", ref selectedScan, scanTypes, scanTypes.Length))
                {
                    inventoryScan.scanType = (ScanType)selectedScan;
                    m_config.ScanType = (ScanType)selectedScan;
                    ConfigManager.Save(m_config);
                }
                //ImGui.SameLine();
                ImGui.PushItemWidth(ImGui.CalcItemWidth());
                if (ImGui.SliderInt("##ScanLevel", ref inventoryScan.scanLevel, 1, 5))
                {
                    m_config.scanLevel = (int)inventoryScan.scanLevel;
                    ConfigManager.Save(m_config);
                }
                ImGui.SameLine();
                ImGui.Text("Scan Iterations");
                ImGui.PopItemWidth();

                ImGui.Separator();
                ImGui.Text("Initialize");
                if (ImGui.Button("Set First Cell"))
                {
                    editSession.Mode = EditMode.FirstCell;
                    editSession.Size = new Vector2(inventoryScan.CellSize, inventoryScan.CellSize);
                    editSession.OnSet = (pos) =>
                    {
                        inventoryScan.firstCellPos = pos;
                        m_config.FirstCellPosition = pos;
                    };
                    editSession.Active =true;
                }
                ImGui.SameLine();
                if (ImGui.Button("Set Bag"))
                {
                    editSession.Mode = EditMode.Bag;
                    editSession.Size = new Vector2(inventoryScan.BagSize, inventoryScan.BagSize);
                    editSession.OnSet = (pos) =>
                    {
                        inventoryScan.firstBagPos = pos;
                        m_config.FirstBagPosition = pos;
                    };
                    editSession.Active =true;
                }

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


        #region DrawSwapCape
        private void DrawSwapCape()
        {
            if (ImGui.BeginTabItem("SwapCape"))
            {
                ImGui.Text("SwapCape");
                if (ImGui.Button($"{(editMode ? "Select the Inventory Closeing button!" : "Set Close Button")}"))
                {
                    editSession.Mode = EditMode.SetClose;
                    editSession.Size = new Vector2(swapCape.CloseButtonScale, swapCape.CloseButtonScale);
                    editSession.OnSet = (pos) =>
                    {
                        swapCape.InventoryCloseButtonPosition = pos;
                        m_config.ClosePoint = pos;
                    };
                    editSession.Active=true;
                }

                ImGui.Text("Inventory Settings:");
                if (ImGui.SliderInt("Bag", ref swapCape.bag, 0, 8))
                {
                    m_config.swapBag = swapCape.bag;
                    ConfigManager.Save(m_config);
                }
                if(ImGui.SliderInt("Colums", ref swapCape.col, 0, 3))
                {
                    m_config.swapCol = swapCape.col;
                    ConfigManager.Save(m_config);
                }
                if (ImGui.SliderInt("Rows", ref swapCape.row, 0, 6))
                {
                    m_config.swapRow = swapCape.row;
                    ConfigManager.Save(m_config);
                }

                ImGui.Separator();

                ImGui.Text("Keybinds");
                ImGui.TextColored(ColorRed, "NOT WORKING !");
                RenderKeyBinding("Open Inventory", ref swapCape.OpenInventory);
                RenderKeyBinding("Ride Mount", ref swapCape.RideMount);
                
                ImGui.EndTabItem();
            }
        }

        private bool waitingForKeyInput = false;
        private string keyToChange = "";
        private void RenderKeyBinding(string label, ref VirtualKeyCode key)
        {
            ImGui.Text(label);
            ImGui.SameLine();

            string buttonLabel = waitingForKeyInput && keyToChange == label ? "Press any key..." : key.ToString();

            if (ImGui.Button(buttonLabel))
            {
                waitingForKeyInput = true;
                keyToChange = label;
            }

            if (waitingForKeyInput && keyToChange == label)
            {
                for (int vk = 0x08; vk <= 0xFE; vk++)
                {
                    if (ImGui.IsKeyPressed((ImGuiKey)vk))
                    {
                        key = (VirtualKeyCode)vk;
                        waitingForKeyInput = false;
                        keyToChange = "";
                        break;
                    }
                }
            }
        }
        #endregion

        #region DrawReviveBot
        private void DrawReviveBot()
        {
            if(ImGui.BeginTabItem("Revive Bot"))
            {
                ImGui.Text("Initialize");
                ImGui.SameLine();
                if(ImGui.Button("Save pos"))
                {
                    editSession.Mode= EditMode.Player;
                    editSession.Radius = reviveBot.Radius;
                    editSession.OnSet = (pos) =>
                    {
                        reviveBot.FirstPlayerPos = pos;
                        m_config.FirstPlayerPos= pos;
                    };
                    editSession.Active = true;
                }

                ImGui.Separator();

                for (int i=0;i<5;i++)
                {
                    bool value = reviveBot.m_blockOffers[i];

                    if (ImGui.Checkbox($"Block offer - Player {i}##{i}", ref value))
                    {
                        reviveBot.m_blockOffers[i] = value;
                    }
                }

                ImGui.EndTabItem();
            }
        }
        #endregion

        #region MeltBot
        private void DrawMeltBot()
        {
            
            if (ImGui.BeginTabItem("Melt Bot"))
            {

                ImGui.Text("Initialize");
                if (ImGui.Button("Set First Smelt Cell"))
                {
                    editSession.Mode    = EditMode.FirstCell;
                    editSession.Size    = new Vector2(meltBot.CellSize, meltBot.CellSize);
                    editSession.OnSet   = pos =>
                    {
                        meltBot.MeltFirstPos = pos;
                        m_config.MeltFirstPos =pos;
                    };
                    editSession.Active  = true;
                }
                if (ImGui.Button("Set Smelt Button"))
                {
                    editSession.Mode    = EditMode.MeltBot;
                    editSession.Size    =meltBot.MeltButtonSize;
                    editSession.OnSet   = pos =>
                    {
                        meltBot.MeltButtonPos = pos;
                        m_config.MeltButtonPos =pos;
                    };
                    editSession.Active  = true;
                }
                if (ImGui.Button("Set Smelt Close Btn"))
                {
                    editSession.Mode    = EditMode.SetClose;
                    editSession.Size    = new Vector2(30,30);
                    editSession.OnSet   = pos =>
                    {
                        meltBot.MeltCloseBtn = pos;
                        m_config.MeltCloseBtn =pos;
                    };
                    editSession.Active  = true;
                }
                ImGui.Separator();
                ImGui.EndTabItem();
            }
        }
        #endregion

        private void HandleEditSession(Vector2 mousePos)
        {
            if (!editSession.Active) return;


            switch(editSession.Mode)
            {
                case EditMode.FirstCell:
                case EditMode.Bag:
                case EditMode.SetClose:
                case EditMode.MeltBot:
                    DrawCursorSquare(mousePos, editSession.Size, ColorRed);
                    break;
                case EditMode.Player:
                    DrawCursorCircle(mousePos, editSession.Radius, ColorRed); 
                    break;
            }

            if (ImGui.GetIO().MouseClicked[0])
            {
                editSession.OnSet?.Invoke(mousePos);
                editSession.Active = false;
                ConfigManager.Save(m_config);
            }
        }

    }
}
