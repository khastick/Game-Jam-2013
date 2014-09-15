using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using BmFont;

namespace PandaPanicV3
{
    class Artist
    {
        // constants
        readonly static Vector2 FINAL_SCORE_DISPLACEMENT, DISPLAY_SIZE;
        public static FontRenderer fontRenderer;
        readonly static int CENTERED_TEXT;
        public static Dictionary<string, Texture2D> textures;

        Rectangle                           screen;
        string[]                            finalTextures;
        string                              bg;
        public Dictionary<string, Action>   drawBackground;
        Label[]                             labels;

        static Artist()
        {
            FINAL_SCORE_DISPLACEMENT = new Vector2(70, 150);
            DISPLAY_SIZE = new Vector2(270, 32);
            CENTERED_TEXT = 840 / 2 - (int)DISPLAY_SIZE.X/2;
        }

        public Artist()
        {
            screen = new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT);

            drawBackground = new Dictionary<string, Action>();

            labels = new Label[]{
                new Label(new Vector2(190, 370), "Back To Main Page (B)"),
                new Label(new Vector2(CENTERED_TEXT,0), "", (int)DISPLAY_SIZE.X, (int)DISPLAY_SIZE.Y),
                new Label(new Vector2(CENTERED_TEXT,480 - DISPLAY_SIZE.Y), "", (int)DISPLAY_SIZE.X, (int)DISPLAY_SIZE.Y),
            new Label(new Vector2(190, 370), "Single Player Game (W)"),
            new Label(new Vector2(80, 410), "Controls (A)"),
            new Label(new Vector2(390, 410), "Credits (D)"),
            new Label(new Vector2(255, 450), "Exit Game (S)")
            };

            textures = new Dictionary<string, Texture2D>();
            finalTextures = new string[] { "FinalScorePink", "FinalScoreGreen", "FinalScoreBlue", "FinalScoreWhite", "FinalScoreDraw" };
        }

        public void start(ref ContentManager Content)
        {
            string[] names = { "GameBackground", "GrassPixel", "credit", "HowToPlay", "FINAL_MENU", "FINAL_MENU2", "skull", "floor", "square", "fillOval", "oval", 
                                 "Pandas","FinalScorePink", "FinalScoreGreen", "FinalScoreBlue", "FinalScoreWhite","FinalScoreDraw"};
            
            // load textures
            foreach(string n in names){
                textures.Add(n,Content.Load<Texture2D>(n));
            }

            bg = Game1.random.Next(2) == 0 ? textures["FINAL_MENU"].Name : textures["FINAL_MENU2"].Name;
            
            drawBackground.Add("background", () => Game1.batch.Draw(textures[bg], screen, Color.White));
            drawBackground.Add("credit", () => Game1.batch.Draw(textures["credit"], screen, Color.White));
            drawBackground.Add("how", () => Game1.batch.Draw(textures["HowToPlay"], screen, Color.White));

            // the fontRenderer enables the drawing of text in monogame
            var fontFilePath = Path.Combine(Content.RootDirectory, "Arial.fnt");
            var fontFile = FontLoader.Load(fontFilePath);
            var fontTexture = Content.Load<Texture2D>("Arial_0.png");
            fontRenderer = new FontRenderer(fontFile, fontTexture);
        }

        public void draw(Game1 game)
        {
            switch(game.state)
            {
                case Game1.STATE.MAIN:
                    drawMain();
                    break;
                case Game1.STATE.CREDITS:
                    drawCredit();
                    break;
                case Game1.STATE.INSTRUCTIONS:
                    drawInstruction();
                    break;
                case Game1.STATE.REGULAR:
                    drawNormal(ref game);
                    break;
                case Game1.STATE.ENDGAME:
                    drawEnd(ref game);
                    break;
                case Game1.STATE.PAUSED:
                    drawPause(ref game);
                    break;
            }
        }

        void drawMenu()
        {
            for (int i = 3; i < labels.Length; i++)
            {
                labels[i].Draw();
            }
        }

        void drawEntities(List<Entity> entities)
        {

            foreach (Entity entity in entities)
            {
                entity.draw();
            }
        }

        void drawTrails(List<Trail> trails)
        {
            foreach (Trail trail in trails)
            {
                trail.draw();
            }
        }

        void drawScores(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                //fontRenderer.DrawText(Game1.batch, (int)corner[i].X, (int)corner[i].Y, players[i].Score.ToString());
            }
        }

        void drawDisplay(Counter timer, int round)
        {
            string []msg = {timer.Name + " " + (timer.Current / Game1.FPS) + " / " + (timer.Limit / Game1.FPS),
                    "Round#: " + (round + 1) + " / " + Game1.NUM_OF_ROUNDS};

            for (int i = 1; i <= 2; i++)
            {
                labels[i].Text = msg[i - 1];
                labels[i].Draw();
            }
        }
        
        void drawFloor()
        {
            Color color = Color.Black;
            color.A = 100;

            Game1.batch.Draw(textures["floor"], screen, Color.White);
            Game1.batch.Draw(textures["square"], screen, color);
        }

        void drawMain()
        {
            drawBackground["background"]();
            drawMenu();
        }

        void drawPause(ref Game1 game)
        {
            drawFloor();
            drawDisplay(game.state == Game1.STATE.PAUSED ? game.PauseTimer : game.RoundTimer, game.currentRound);
            drawTrails(game.Collection.Trails);
            drawEntities(game.Collection.Entities);
        }

        void drawNormal(ref Game1 game)
        {
            drawFloor();
            drawDisplay(game.RoundTimer, game.currentRound);
            drawEntities(game.Collection.Entities);
        }

        void drawEnd(ref Game1 game)
        {
            String msg;

            Game1.batch.Draw(textures[finalTextures[game.typeOfMax]], screen, Color.White);

            for (int i = 0; i < Collection.NUM_OF_PLAYERS; i++)
            {
                msg = "Player " + i + " ( " + Entity.colors.ElementAt(i).Value + " ) Score: " + game.Collection.Players[i].Score;
                fontRenderer.DrawText(Game1.batch, (int)FINAL_SCORE_DISPLACEMENT.X, (int)FINAL_SCORE_DISPLACEMENT.Y + (50 * i), msg);
            }

            labels[0].Draw();
        }

        void drawInstruction()
        {
            drawBackground["how"]();
            labels[0].Draw();
        }

        void drawCredit()
        {
            drawBackground["credit"]();
            labels[0].Draw();
        }
    }

   
}
