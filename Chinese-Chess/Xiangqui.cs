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

        private float[][] _board = new float[10][]
        {
            new float[9] {-Piece.CHARIOT,  -Piece.HORSE, -Piece.ELEPHANT, -Piece.ADVISOR, -Piece.GENERAL, -Piece.ADVISOR, -Piece.ELEPHANT,  -Piece.HORSE, -Piece.CHARIOT},
            new float[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new float[9] {             0, -Piece.CANNON,               0,              0,              0,              0,               0, -Piece.CANNON,              0},
            new float[9] {-Piece.SOLDIER,             0,  -Piece.SOLDIER,              0, -Piece.SOLDIER,              0,  -Piece.SOLDIER,             0, -Piece.SOLDIER},
            new float[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new float[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new float[9] { Piece.SOLDIER,             0,   Piece.SOLDIER,              0,  Piece.SOLDIER,              0,   Piece.SOLDIER,             0,  Piece.SOLDIER},
            new float[9] {             0,  Piece.CANNON,               0,              0,              0,              0,               0,  Piece.CANNON,              0},
            new float[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new float[9] { Piece.CHARIOT,   Piece.HORSE,  Piece.ELEPHANT,  Piece.ADVISOR,  Piece.GENERAL,  Piece.ADVISOR,  Piece.ELEPHANT,   Piece.HORSE,  Piece.CHARIOT}
        };

        public Xiangqui()
        {
            _pieces = new List<Piece>();
        }


        public void LoadContent(ContentManager contentManager)
        {
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    if (_board[i][j] != 0)
                    {
                        var piece = PieceFactory.CreatePiece(_board[i][j], new Vector2(j, i), contentManager);
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
