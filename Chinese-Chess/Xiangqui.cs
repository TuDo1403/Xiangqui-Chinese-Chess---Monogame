using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chinese_Chess
{
    public class Xiangqui : GameModel
    {

        private List<Piece> _pieces;

        public static float[][] Board { get; private set; } = new float[10][]
        {
            new float[9] {-Rules.CHARIOT,  -Rules.HORSE, -Rules.ELEPHANT, -Rules.ADVISOR, -Rules.GENERAL, -Rules.ADVISOR, -Rules.ELEPHANT,  -Rules.HORSE, -Rules.CHARIOT},
            new float[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new float[9] {             0, -Rules.CANNON,               0,              0,              0,              0,               0, -Rules.CANNON,              0},
            new float[9] {-Rules.SOLDIER,             0,  -Rules.SOLDIER,              0, -Rules.SOLDIER,              0,  -Rules.SOLDIER,             0, -Rules.SOLDIER},
            new float[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new float[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new float[9] { Rules.SOLDIER,             0,   Rules.SOLDIER,              0,  Rules.SOLDIER,              0,   Rules.SOLDIER,             0,  Rules.SOLDIER},
            new float[9] {             0,  Rules.CANNON,               0,              0,              0,              0,               0,  Rules.CANNON,              0},
            new float[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new float[9] { Rules.CHARIOT,   Rules.HORSE,  Rules.ELEPHANT,  Rules.ADVISOR,  Rules.GENERAL,  Rules.ADVISOR,  Rules.ELEPHANT,   Rules.HORSE,  Rules.CHARIOT}
        };

        public Xiangqui()
        {
            _pieces = new List<Piece>();
        }


        public void LoadContent(ContentManager contentManager)
        {
            for (int i = 0; i < Rules.ROWS; ++i)
            {
                for (int j = 0; j < Rules.COLUMNS; ++j)
                {
                    if (Board[i][j] != 0)
                    {
                        var piece = PieceFactory.CreatePiece(Board[i][j], new Vector2(i, j), contentManager);
                        _pieces.Add(piece);
                    }
                }
            }
        }


        public override void Update(MouseState mouseState)
        {
            foreach (var piece in _pieces)
            {
                piece.Update(mouseState);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var piece in _pieces)
            {
                piece.Draw(spriteBatch);
            }
        }
    }
}
