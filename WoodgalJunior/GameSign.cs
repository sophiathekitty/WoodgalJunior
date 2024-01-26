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
        // Game Sign
        //----------------------------------------------------------------------
        public class GameSign : Screen
        {
            ScreenSprite title;
            ScreenSprite highScore;
            ScreenSprite highScoreShadow;
            public GameSign(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
                drawingSurface.BackgroundColor = new Color(115,166,193,0.25f);

                title = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, new Vector2(0, 80),0.085f,Vector2.Zero,Color.White,"Monospace",GridBlocks.TitleImage,TextAlignment.CENTER,SpriteType.TEXT);
                highScore = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.BottomCenter, new Vector2(0, -160), 3f, Vector2.Zero, Color.WhiteSmoke, "Monospace", "Hi: 0", TextAlignment.CENTER, SpriteType.TEXT);
                highScoreShadow = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.BottomCenter, new Vector2(0, -170), 3f, Vector2.Zero, Color.HotPink, "Monospace", "Hi: 0", TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(title);
                AddSprite(highScoreShadow);
                AddSprite(highScore);
                GridInfo.AddChangeListener("highScore", UpdateScore);
                highScore.Data = highScoreShadow.Data = "Hi: " + GridInfo.GetVarAs("highScore",0);
            }
            void UpdateScore(string key, string value)
            {
                if (key == "highScore")
                {
                    highScoreShadow.Data = "Hi: " + value;
                    highScore.Data = "Hi: " + value;
                }
            }
        }
    }
}
