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


        public static event EventHandler BoardUpdated;

        public static int[][] Board { get; private set; } = new int[10][]
        {
            new int[9] {-Rules.CHARIOT,  -Rules.HORSE, -Rules.ELEPHANT, -Rules.ADVISOR, -Rules.GENERAL, -Rules.ADVISOR, -Rules.ELEPHANT,  -Rules.HORSE, -Rules.CHARIOT},
            new int[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new int[9] {             0, -Rules.CANNON,               0,              0,              0,              0,               0, -Rules.CANNON,              0},
            new int[9] {-Rules.SOLDIER,             0,  -Rules.SOLDIER,              0, -Rules.SOLDIER,              0,  -Rules.SOLDIER,             0, -Rules.SOLDIER},
            new int[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new int[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new int[9] { Rules.SOLDIER,             0,   Rules.SOLDIER,              0,  Rules.SOLDIER,              0,   Rules.SOLDIER,             0,  Rules.SOLDIER},
            new int[9] {             0,  Rules.CANNON,               0,              0,              0,              0,               0,  Rules.CANNON,              0},
            new int[9] {             0,             0,               0,              0,              0,              0,               0,             0,              0},
            new int[9] { Rules.CHARIOT,   Rules.HORSE,  Rules.ELEPHANT,  Rules.ADVISOR,  Rules.GENERAL,  Rules.ADVISOR,  Rules.ELEPHANT,   Rules.HORSE,  Rules.CHARIOT}
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
                        PutPieceOnBoard(contentManager, new Point(j, i));
                    }
                }
            }
            OnBoardUpdating();
        }

        private void PutPieceOnBoard(ContentManager contentManager, Point matrixPos)
        {
            var piece = PieceFactory.CreatePiece(Board[matrixPos.Y][matrixPos.X], matrixPos, contentManager);
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
            OnBoardUpdating();
        }

        private void OnBoardUpdating()
        {
            (BoardUpdated as EventHandler)?.Invoke(this, EventArgs.Empty);
        }

        private void UpdatePosition(Piece movedPiece, Point newMatrixPos)
        {
            var oldMatrixPos = _focusingPiece.MatrixPos;
            Board[oldMatrixPos.Y][oldMatrixPos.X] = 0;
            Board[newMatrixPos.Y][newMatrixPos.X] = movedPiece.Type;

            PrintBoard();
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
            _pieces.RemoveAll(piece => piece != movedPiece && piece.MatrixPos == e);
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
