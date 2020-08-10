using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace ChineseChess.Source.Main
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ChessGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ChessBoard _game;

        private Song _themeSong;

        private string[] _players = new string[2];

        private int _searchDepth;

        private string _mode = "";



        public ChessGame(string redPlayer, string blackPlayer, int depth=1, string mode="")
        {
            IsMouseVisible = true;

            _graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };

            IsFixedTimeStep = false;

            Content.RootDirectory = "Content";

            _players[0] = blackPlayer;
            _players[1] = redPlayer;

            _searchDepth = depth;

            if (!string.IsNullOrEmpty(mode)) _mode = mode;
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

            _game = new ChessBoard(_players[1], _players[0], _searchDepth);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _game.LoadContent(Content);

            _themeSong = Content.Load<Song>(@"Audio\ChineseChessThemeSong");
            MediaPlayer.Volume = 0;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_themeSong);

            _graphics.PreferredBackBufferWidth = _game.Board.Width;
            _graphics.PreferredBackBufferHeight = _game.Board.Height;
            _graphics.ApplyChanges();
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (_game.Winner >= 0 && _game.Winner <= 2)
            {
                //WriteReport(_game.Winner);
                Exit();
            }
                

            // TODO: Add your update logic here
            _game.Update(Mouse.GetState(), gameTime);

            base.Update(gameTime);
        }

        private void WriteReport(int result)
        {
            if (!string.IsNullOrEmpty(_mode))
            {
                using (StreamWriter writer = File.AppendText($"{_players[1]}-{_players[0]}-{_searchDepth}-{_mode}.txt"))
                {
                    writer.WriteLine(result);
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText($"{_players[1]}-{_players[0]}-{_searchDepth}.txt"))
                {
                    writer.WriteLine(result);
                }
            }
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            _game.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
