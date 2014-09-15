using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Visuals.Flat;
using Nuclex.Input;

namespace PandaPanicV3
{
    public partial class PandaPanic : Microsoft.Xna.Framework.Game
    {
        public const int WIDTH = 840, HEIGHT = 480;
        
        public static Texture2D defaultTexture;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        

        private GuiManager gui;
        private InputManager input;

        public PandaPanic()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.input = new InputManager(Services, Window.Handle);
            this.gui = new Nuclex.UserInterface.GuiManager(Services);

            Components.Add(input);
            // You can either add the GUI to the Components collection to have it render
            // automatically, or you can call the GuiManager's Draw() method yourself
            // at the appropriate place if you need more control.
            Components.Add(this.gui);
            
            // It's a good idea to show the mouse if the user is supposed to click on
            // the buttons in the GUI :)
            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            

            defaultTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            defaultTexture.SetData(new[] { Color.White });

            // Create a new screen. Screens manage the state of a GUI and its rendering
            // surface. If you have a GUI in your game window, you'd first create a screen
            // for that. If you have an in-game computer display where you want to use
            // a GUI, you can create another screen for that and thus cleanly separate
            // the state of the in-game computer from your game's main menu GUI :)
            Viewport viewport = GraphicsDevice.Viewport;
            Screen mainScreen = new Screen(viewport.Width, viewport.Height);
            this.gui.Screen = mainScreen;

            // Each screen has a 'desktop' control. This invisible control by default
            // stretches across the whole screen (all controls are positioned using both
            // a percentual position/size and absolute position/size). We change this to
            // prevent GUI or HUD elements from appearing outside the title-safe area.
            mainScreen.Desktop.Bounds = new UniRectangle(
              new UniScalar(0.1f, 0.0f), new UniScalar(0.0f, 0.0f), // x and y
              new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f) // width and height
            );

            // Now let's do something funky: add buttons directly to the desktop.
            // This will also show the effect of the title-safe area.
            createDesktopControls(mainScreen);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentManager _content = Content;
            _cont.artist.startMedia(ref _content);
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            ContentManager manager = Content;

            _cont.update(ref gameTime, ref manager);

            if (_cont.isGameOver()) Exit();
            if(_cont.state == GameController.STATE.ENDGAME)
            {
                gui.Visible = true;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _cont.draw(ref spriteBatch);

            base.Draw(gameTime);
        }
    }
}
