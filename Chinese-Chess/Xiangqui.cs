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
        private bool _isFocusing = false;

        private Piece _focusingPiece;

        private readonly List<Piece> _pieces;


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
                        PutPieceOnBoard(contentManager, i, j);
                    }
                }
            }
        }

        private void PutPieceOnBoard(ContentManager contentManager, int i, int j)
        {
            var piece = PieceFactory.CreatePiece(Board[i][j], new Point(i, j), contentManager);
            piece.Focused += Piece_FocusedHandler;
            piece.Moved += Piece_MovedHandler;
            _pieces.Add(piece);
        }

        private void Piece_MovedHandler(object sender, Point e)
        {
            _isFocusing = false;
            UpdateBoard(sender as Piece, e);
        }

        private void UpdateBoard(Piece movedPiece, Point e)
        {
            UpdatePieces(movedPiece, e);
            UpdatePosition(movedPiece, e);
        }

        private void UpdatePosition(Piece movedPiece, Point newMatrixPos)
        {
            var oldMatrixPos = _focusingPiece.MatrixPos;
            Board[oldMatrixPos.X][oldMatrixPos.Y] = 0;
            Board[newMatrixPos.X][newMatrixPos.Y] = movedPiece.Type;

            //PrintBoard();
        }

        private void PrintBoard()
        {
            for (int i = 0; i < Rules.ROWS; ++i)
            {
                for (int j = 0; j < Rules.COLUMNS; ++j)
                {
                    Console.Write($"{Board[i][j]}\t");
                }
                Console.WriteLine();
            }
        }

        private void UpdatePieces(Piece movedPiece, Point e)
        {
            foreach (var piece in _pieces.Where(piece => piece != movedPiece && piece.MatrixPos == e))
            {
                _pieces.Remove(piece);
                break;
            }
        }

        private void Piece_FocusedHandler(object sender, EventArgs e)
        {
            _isFocusing = true;
            _focusingPiece = (Piece)sender;
        }


        public override void Update(MouseState mouseState)
        {
            if (_isFocusing)
            {
                _focusingPiece.Update(mouseState);
            }
            else
            {
                UpdatePieces(mouseState);
            }
        }

        private void UpdatePieces(MouseState mouseState)
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
