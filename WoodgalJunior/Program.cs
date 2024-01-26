using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        //=======================================================================
        GameSign gameSign;
        ChopGame chopGame;
        public Program()
        {
            GridBlocks.InitBlocks(GridTerminalSystem);
            GridInfo.Init("WoodgalJr",GridTerminalSystem,IGC,Me,Echo);
            GridInfo.Load(Storage);
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            gameSign = new GameSign(GridBlocks.GetTextSurface("sign"));
            chopGame = new ChopGame(GridBlocks.GetTextSurface("main display"));
        }

        public void Save()
        {
            Storage = GridInfo.Save();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (argument == "reset")
            {
                GridInfo.SetVar("highScore", "0");
                chopGame.highScore = 0;
            }
            gameSign.Draw();
            chopGame.Draw();
        }
        //=======================================================================
    }
}
