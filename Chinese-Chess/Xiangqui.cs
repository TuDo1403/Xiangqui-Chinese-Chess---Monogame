using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chinese_Chess
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Xiangqui : Game
    {
        private readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D _board;
        private Texture2D _redHorse;
        private Texture2D _blackHorse;

        private Vector2 _position;
        private Vector2 _position1;

        private Horse _horse;



        public Xiangqui()
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
            _position = new Vector2(0, 0);
            _position1 = new Vector2(100, 0);

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

            _board = Content.Load<Texture2D>("board");
            //_redHorse = Content.Load<Texture2D>("red-horse");
            //_blackHorse = Content.Load<Texture2D>("black-horse");
            _horse = new Horse(Content.Load<Texture2D>("red-horse"), new Vector2(0, 0), 10);

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
            //var mouseState = Mouse.GetState();
            //if (mouseState.LeftButton == ButtonState.Pressed)
            //{
            //    if (mouseState.Position.X <= _position.X+_redHorse.Width &&
            //        mouseState.Position.Y <= _position.Y+_redHorse.Height)
            //    {
            //        _position.X = mouseState.Position.X - (_redHorse.Width / 2);
            //        _position.Y = mouseState.Position.Y - (_redHorse.Height / 2);
            //    }

            //    else if (mouseState.Position.X <= _position1.X + _blackHorse.Width &&
            //        mouseState.Position.Y <= _position1.Y + _blackHorse.Height)
            //    {
            //        _position1.X = mouseState.Position.X - (_blackHorse.Width / 2);
            //        _position1.Y = mouseState.Position.Y - (_blackHorse.Height / 2);
            //    }
            //}

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
            spriteBatch.Draw(_board, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(_redHorse, _position, null, Color.White, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(_blackHorse, _position1, null, Color.White, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
