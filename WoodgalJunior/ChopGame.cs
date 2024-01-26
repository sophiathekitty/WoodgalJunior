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
        // ChopGame
        //----------------------------------------------------------------------
        public class ChopGame : Screen
        {
            ScreenSprite title;
            List<ScreenSprite> tree = new List<ScreenSprite>();
            List<int> treeArea = new List<int>();
            string[] treeSprites;
            ScreenSprite ground;
            ScreenSprite player;
            Dictionary<string, string[]> playerSprites = new Dictionary<string, string[]>();
            Vector2 playerLeft = new Vector2(-100, -130);
            Vector2 playerRight = new Vector2(100, -130);
            Vector2 bonkLeft = new Vector2(-100, -210);
            Vector2 bonkRight = new Vector2(100, -210);
            GameInput input = new GameInput();
            ScreenSprite bonk;
            ScreenSprite CurrentScore;
            ScreenSprite CurrentScoreShadow;
            ScreenSprite TimeBar;
            ScreenSprite GameOverText;
            ScreenSprite GameOverShadow;
            int score = 0;
            public int highScore = 0;
            float timeLeft = 400;
            float timeMax = 400;
            IMySoundBlock effectsSpeaker;
            IMySoundBlock musicSpeaker;
            public ChopGame(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
                treeSprites = GridBlocks.TreeSprites.Split(',');
                string[] cells = GridBlocks.WoodgalSprites.Split('|');
                foreach (string cell in cells)
                {
                    string[] parts = cell.Split(':');
                    playerSprites.Add(parts[0], parts[1].Split(','));
                }
                drawingSurface.BackgroundColor = new Color(115, 166, 193);
                ground = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.BottomCenter, new Vector2(0, 0), 0f, new Vector2(1000,160), new Color(63,122,51), "", "SquareSimple", TextAlignment.CENTER, SpriteType.TEXTURE);
                AddSprite(ground);
                float y = -130;
                for (int i = 0; i < 5; i++)
                {
                    tree.Add(new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, new Vector2(0, y), 0.1f, Vector2.Zero, Color.White, "Monospace", treeSprites[0], TextAlignment.CENTER, SpriteType.TEXT));
                    AddSprite(tree[i]);
                    y += 135;
                }
                title = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, new Vector2(0, 60), 0.1f, Vector2.Zero, Color.White, "Monospace", GridBlocks.TitleImage, TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(title);
                player = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.BottomCenter, playerLeft, 0.1f, Vector2.Zero, Color.White, "Monospace", playerSprites["ready"][0], TextAlignment.CENTER, SpriteType.TEXT);
                player.Visible = false;
                AddSprite(player);
                bonk = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.BottomCenter, bonkLeft, 0.1f, Vector2.Zero, Color.White, "Monospace", playerSprites["bonk"][0], TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(bonk);
                TimeBar = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopLeft, new Vector2(30, 60), 0, new Vector2(timeLeft, 60), Color.HotPink, "", "SquareSimple", TextAlignment.LEFT, SpriteType.TEXTURE);
                CurrentScore = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopRight, new Vector2(-20, 25), 2f, Vector2.Zero, Color.White, "Monospace", "", TextAlignment.RIGHT, SpriteType.TEXT);
                CurrentScoreShadow = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopRight, new Vector2(-20, 20), 2f, Vector2.Zero, Color.HotPink, "Monospace", "", TextAlignment.RIGHT, SpriteType.TEXT);
                TimeBar.Visible = false;
                AddSprite(TimeBar);
                AddSprite(CurrentScoreShadow);
                AddSprite(CurrentScore);
                GameOverText = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, new Vector2(0, 60), 2f, Vector2.Zero, Color.White, "Monospace", "Score\n0\nHigh\nScore\n0", TextAlignment.CENTER, SpriteType.TEXT);
                GameOverShadow = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, new Vector2(0, 55), 2f, Vector2.Zero, Color.HotPink, "Monospace", "Score\n0\nHigh\nScore\n0", TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(GameOverShadow);
                AddSprite(GameOverText);
                Reset();
                highScore = GridInfo.GetVarAs<int>("highScore",0);
                effectsSpeaker = GridBlocks.GetSoundBlock("effects");
                musicSpeaker = GridBlocks.GetSoundBlock("music");
            }
            void PlaySound(string sound)
            {
                List<string> sounds = new List<string>();
                effectsSpeaker.GetSounds(sounds);
                if (!sounds.Contains(sound)) return;
                effectsSpeaker.SelectedSound = sound;
                effectsSpeaker.Play();
            }
            Random random = new Random();
            void Reset()
            {
                treeArea.Clear();
                bool hasLeft = false;
                bool hasRight = false;
                for (int i = 0; i < 5; i++)
                {
                    treeArea.Add(random.Next(-1,1));
                    if (treeArea[i] == -1) hasLeft = true;
                    if (treeArea[i] == 1) hasRight = true;
                    if(i > 3) treeArea[i] = 0;
                    else if (treeArea[i] == 0 && !hasLeft)
                    {
                        treeArea[i] = -1;
                        hasLeft = true;
                    } else if (treeArea[i] == 0 && !hasRight)
                    {
                        treeArea[i] = 1;
                        hasRight = true;
                    }
                    tree[i].Data = treeSprites[treeArea[i]+1];
                }
                tree[3].Data = treeSprites[1];
                tree[4].Data = treeSprites[1];
                //title.Visible = true;
                player.Data = playerSprites["ready"][0];
                player.Position = playerLeft;
                chopped = false;
                chopCooldown = 0;
                playerSide = 0;
                isPlaying = false;
                bonk.Visible = false;
                score = 0;
                CurrentScore.Data = "";
                CurrentScoreShadow.Data = "";
                timeLeft = timeMax;
                GameOverShadow.Visible = false;
                GameOverText.Visible = false;
                gameOverCooldown = gameOverCooldownMax;
            }
            bool chopped = false;
            int chopCooldown = 0;
            int chopCooldownMax = 25;
            int playerSide = 0;
            bool playerPresencet = false;
            bool isPlaying = false;
            bool firstChop = true;
            int gameOverCooldown = 0;
            int gameOverCooldownMax = 40;
            bool gameOver = false;
            public override void Draw()
            {
                if (input.PlayerPresent)
                {
                    if (gameOver)
                    {
                        if (gameOverCooldown < gameOverCooldownMax * 0.25f) bonk.Visible = false;
                        gameOverCooldown--;
                        if (gameOverCooldown <= 0)
                        {
                            gameOver = false;
                            gameOverCooldown = gameOverCooldownMax;
                        }
                    }
                    else
                    {
                        if (!playerPresencet)
                        {
                            Reset();
                            playerPresencet = true;
                            title.Visible = false;
                            player.Visible = true;
                            TimeBar.Visible = true;
                            musicSpeaker.SelectedSound = "MusCalm_03";
                            musicSpeaker.Play();
                        }
                        if (input.A && !chopped)
                        {
                            player.Data = playerSprites["chop"][0];
                            player.Position = playerLeft;
                            playerSide = 0;
                            DoChop();
                        }
                        else if (input.D && !chopped)
                        {
                            player.Data = playerSprites["chop"][1];
                            player.Position = playerRight;
                            playerSide = 1;
                            DoChop();
                        }
                        if (chopCooldown > 0 && chopped && isPlaying)
                        {
                            chopCooldown--;
                            if (chopCooldown == (int)(chopCooldownMax * 0.75f)) player.Data = playerSprites["ready"][playerSide];
                            if (chopCooldown == 0)
                            {
                                if (!input.A && !input.D) chopped = false;
                                DropTree();
                            }
                        }
                        else if (!input.A && !input.D) chopped = false;
                        if (isPlaying)
                        {
                            timeLeft -= (0.75f + ((float)score/1000));
                            TimeBar.Size = new Vector2(timeLeft, 60);
                            if (timeLeft <= 0)
                            {
                                GameOver();
                            }
                        }

                    }
                } 
                else if (playerPresencet)
                {
                    playerPresencet = false;
                    Reset();
                    player.Visible = false;
                    firstChop = true;
                    title.Visible = true;
                    TimeBar.Visible = false;
                    CurrentScore.Data = "";
                    CurrentScoreShadow.Data = "";
                    musicSpeaker.Stop();
                }
                base.Draw();
            }
            void DoChop()
            {
                if(!isPlaying && !firstChop) Reset();
                chopped = true;
                chopCooldown = chopCooldownMax;
                tree[4].Visible = false;
                isPlaying = true;
                firstChop = false;
                TimeBar.Visible = true;
                timeLeft += 30;
                if (timeLeft > timeMax) timeLeft = timeMax;
                GameOverText.Visible = false;
                GameOverShadow.Visible = false;
                PlaySound("WoodChop");
            }
            int lastTreeArea = 0;
            void DropTree()
            {
                player.Data = playerSprites["ready"][playerSide];
                tree[4].Visible = true;
                for(int i = 4; i > 0; i--)
                {
                    tree[i].Data = tree[i - 1].Data;
                    treeArea[i] = treeArea[i - 1];
                }
                treeArea[0] = random.Next(-1,1);
                if (treeArea[0] == lastTreeArea) treeArea[0]++;
                if (treeArea[0] == 2) treeArea[0] = -1;
                lastTreeArea = treeArea[0];
                tree[0].Data = treeSprites[treeArea[0]+1];
                if (treeArea[treeArea.Count - 1] == 1 && playerSide == 1 || treeArea[treeArea.Count-1] == -1 && playerSide == 0)
                {
                    // down
                    bonk.Visible = true;
                    bonk.Position = playerSide == 0 ? bonkLeft : bonkRight;
                    GameOver();
                }
                else
                {
                    score++;
                    CurrentScore.Data = score.ToString();
                    CurrentScoreShadow.Data = score.ToString();
                }
            }
            void GameOver()
            {
                player.Data = playerSprites["down"][playerSide];
                isPlaying = false;
                CurrentScore.Data = "";
                CurrentScoreShadow.Data = "";
                TimeBar.Visible = false;
                gameOver = true;
                if (score > highScore)
                {
                    highScore = score;
                    GridInfo.SetVar("highScore", highScore.ToString());
                }
                GameOverText.Data = "Score\n" + score + "\nHigh\nScore\n" + highScore;
                GameOverShadow.Data = "Score\n" + score + "\nHigh\nScore\n" + highScore;
                GameOverText.Visible = true;
                GameOverShadow.Visible = true;
                PlaySound("GameDeath");
            }
        }
        //----------------------------------------------------------------------
    }
}
