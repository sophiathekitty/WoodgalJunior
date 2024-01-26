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
    partial class Program
    {
        //----------------------------------------------------------------------
        // GridBlocks
        //----------------------------------------------------------------------
        public class GridBlocks
        {
            public static List<IMyShipController> controllers = new List<IMyShipController>();
            public static List<IMyTextPanel> textPanels = new List<IMyTextPanel>();
            public static List<IMySoundBlock> soundBlocks = new List<IMySoundBlock>();
            public static void InitBlocks(IMyGridTerminalSystem GridTerminalSystem)
            {
                GridTerminalSystem.GetBlocksOfType(controllers);
                GridTerminalSystem.GetBlocksOfType(textPanels);
                GridTerminalSystem.GetBlocksOfType(soundBlocks);
            }
            public static IMySoundBlock GetSoundBlock(string name)
            {
                return soundBlocks.Find(x => x.CustomName.ToLower().Contains(name.ToLower()));
            }
            public static IMyShipController GetPlayerOne()
            {
                if (controllers.Count == 1) return controllers[0];
                return controllers.Find(x => x.CustomName.ToLower().Contains("one"));
            }
            public static IMyShipController GetPlayerTwo()
            {
                if (controllers.Count == 1) return controllers[0];
                return controllers.Find(x => x.CustomName.ToLower().Contains("two"));
            }
            public static IMyTextSurface GetTextSurface(string name)
            {
                return textPanels.Find(x => x.CustomName.ToLower().Contains(name.ToLower()));
            }
            public static string TitleImage
            {
                get
                {
                    IMyTextPanel panel = textPanels.Find(x => x.CustomName.ToLower().Contains("sign"));
                    if (panel != null)
                    {
                        return panel.GetText();
                    }
                    return "";
                }
            }
            public static string WoodgalSprites
            {
                get
                {
                    IMyTextPanel panel = textPanels.Find(x => x.CustomName.ToLower().Contains("sign"));
                    if (panel != null)
                    {
                        return panel.CustomData;
                    }
                    return "";
                }
            }
            public static string TreeSprites
            {
                get
                {
                    IMyTextPanel panel = textPanels.Find(x => x.CustomName.ToLower().Contains("main display"));
                    if (panel != null)
                    {
                        return panel.CustomData;
                    }
                    return "";
                }
            }
        }
        //----------------------------------------------------------------------
    }
}
