#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace PandaPanicV3
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public enum STATE {INIT, MAIN, CREDITS, INSTRUCTIONS, REGULAR, ENDGAME, PAUSED}
        
        public static SpriteBatch batch;
        public static Random random;

        const STATE START_STATE = STATE.MAIN;

        GraphicsDeviceManager graphics;
        
        public const int
            DURATION_OF_PAUSE= 5,
            NUM_OF_ROUNDS = 1,
            TIME_OF_ROUNDS = 30,
            SECOND = 60,
            FPS = 60,
            WIDTH = 800,
            HEIGHT = 480;

        Dictionary<STATE,Action> actions;
        Action startgame;

        public STATE state;

        public int currentRound, typeOfMax;

        Collection collection;
        Artist artist;

        Counter roundTimer, pauseTimer;

        public Counter RoundTimer
        {
            get { return roundTimer; }
        }

        public Counter PauseTimer
        {
            get { return pauseTimer; }
        }

        public Collection Collection
        {
            get { return collection; }
        }

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.PreferredBackBufferWidth = WIDTH;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            random = new Random();

            actions = new Dictionary<STATE,Action>();
            actions.Add(STATE.MAIN, actionMain);
            actions.Add(STATE.CREDITS, actionCredit);
            actions.Add(STATE.INSTRUCTIONS, actionInstructions);
            actions.Add(STATE.ENDGAME, actionEnd);
            actions.Add(STATE.PAUSED, actionPause);
            actions.Add(STATE.REGULAR, actionNormal);

            //actionCredit, actionInstructions, actionNormal, actionEnd, actionPause, () => { }, };
            state = START_STATE;
            pauseTimer = new Counter(DURATION_OF_PAUSE * FPS, "PAUSED TIME");
            roundTimer = new Counter(TIME_OF_ROUNDS * FPS, "ROUND TIME");
            artist = new Artist();
            startgame = actionInit;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            batch = new SpriteBatch(GraphicsDevice);

            ContentManager _content = Content;
            artist.start(ref _content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            actions[state]();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Game1.batch.Begin();
            artist.draw(this);
            Game1.batch.End();
            base.Draw(gameTime);
        }

        public void actionReload()
        {
            state = STATE.REGULAR;
        }

        /* 
         * input: none
         * output: none
         * description: prepares for the game to be paused, the max player is retrieved and the state of the controller
         * and the players is set to paused.
         */
        void pause()
        {
            int max = 0; 
            typeOfMax = 0;
            state = STATE.PAUSED;

            collection.setEntities(Player.STATE.PAUSED); // set the controllers state to paused

            foreach (Player _player in collection.Players) // retrievive the max player
            {
                if (_player.Score > max)
                {
                    max = _player.Score;
                    typeOfMax = _player.type;
                }
            }
        }

        void actionMain()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                startgame(); // single player
            else if (Keyboard.GetState().IsKeyDown(Keys.A)) 
                state = STATE.INSTRUCTIONS;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                Exit(); // exit                
            else if (Keyboard.GetState().IsKeyDown(Keys.D))  // co-op
                state = STATE.CREDITS;
        }

        void actionInit()
        {
            collection = new Collection();
            state = STATE.REGULAR;
        }

        void actionCredit()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.B))
                state = STATE.MAIN;
        }

        void actionInstructions()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.B))
                state = STATE.MAIN;
        }

        void actionNormal()
        {
            collection.update();
            foreach (Player _player in collection.Players)
            {
                if (!_player.alive)
                {
                    pause();
                    break;
                }
            }

            if (roundTimer.isReady()) pause();
        }

        void actionPause()
        {
            collection.actionPause();

            if (!pauseTimer.isReady()) return;

            if (++currentRound < NUM_OF_ROUNDS)
            {
                state = STATE.REGULAR;
                roundTimer.reset();
                collection.resetRound();
                //playNewSong(ref content);
            }
            else
            {
                state = STATE.ENDGAME;
                currentRound = 0;
                roundTimer.reset();
                collection.resetGame();
            }
        }

        void actionEnd()
        {
            startgame = actionReload;
            if (Keyboard.GetState().IsKeyDown(Keys.B))
                state = STATE.MAIN;
        }

        
    }
}
