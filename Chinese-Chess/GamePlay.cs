using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chinese_Chess
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GamePlay : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;


        private Board _board;

        private Xiangqui _game;





        public GamePlay()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
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

            _game = new Xiangqui();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _board = new Board(Content.Load<Texture2D>("board"));


            //_redHorse = new Horse(Content.Load<Texture2D>("red-horse"), new Vector2(10, 0));
            //_blackHorse = new Horse(Content.Load<Texture2D>("black-horse"), new Vector2(299.5f, 300));
            _game.LoadContent(Content);

            graphics.PreferredBackBufferWidth = _board.Width;
            graphics.PreferredBackBufferHeight = _board.Height;
            graphics.ApplyChanges();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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

            // TODO: Add your update logic here
            //_redHorse.Update(Mouse.GetState());
            //_blackHorse.Update(Mouse.GetState());
            _game.Update(Mouse.GetState());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            _board.Draw(spriteBatch);
            //_redHorse.Draw(spriteBatch);
            //_blackHorse.Draw(spriteBatch);
            _game.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
