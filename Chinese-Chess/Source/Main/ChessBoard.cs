using ChineseChess.Source.Helper;
using ChineseChess.Source.GameObjects;
using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using ChineseChess.Properties;

namespace ChineseChess.Source.Main
{
    public class ChessBoard : GameModel
    {
        private static ChessBoard _instance;

        private bool _isCheckMate = false;
        private bool _gameEnd = false;
        private bool _isFocusing = false;


        private Message _gameMessage;

        private Piece _focusingPiece;

        private readonly List<Piece> _pieces;


        public Board Board { get; private set; }

        public static event EventHandler BoardUpdated;

        public static int[][] MatrixBoard { get; private set; } = new int[10][]
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



        public static ChessBoard GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ChessBoard();
            }
            return _instance;
        }

        private ChessBoard()
        {
            _pieces = new List<Piece>();
        }


        public void LoadContent(ContentManager contentManager)
        {
            if (contentManager == null)
            {
                throw new ArgumentNullException(nameof(contentManager));
            }

            for (int i = 0; i < Rules.ROWS; ++i)
            {
                for (int j = 0; j < Rules.COLUMNS; ++j)
                {
                    if (MatrixBoard[i][j] != 0)
                    {
                        PutPieceOnBoard(contentManager, new Point(j, i));
                    }
                }
            }

            Board = Board.GetInstance(contentManager.Load<Texture2D>("board"));

            _gameMessage = new Message(contentManager.Load<SpriteFont>(@"Font\GameEnd"), 
                                   new Vector2(PositionHelper.X_OFFSET_FROM_TOP_LEFT_WIN, Board.Height/2));

            OnBoardUpdating();
        }

        private void PutPieceOnBoard(ContentManager contentManager, Point matrixPos)
        {
            var piece = PieceFactory.CreatePiece(MatrixBoard[matrixPos.Y][matrixPos.X], matrixPos, contentManager);
            piece.Focused += Piece_FocusedHandler;
            piece.Moved += Piece_MovedHandler;
            piece.CheckMated += Piece_CheckMatedHandler;
            _pieces.Add(piece);
        }

        private void Piece_CheckMatedHandler(object sender, EventArgs e)
        {
            _isCheckMate = true;
        }

        private void Piece_MovedHandler(object sender, Point e)
        {
            _isFocusing = false;
            _gameMessage.CurrentFrame = 0;
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
            MatrixBoard[oldMatrixPos.Y][oldMatrixPos.X] = 0;
            MatrixBoard[newMatrixPos.Y][newMatrixPos.X] = movedPiece.Type;

            PrintBoard();
        }

        private static void PrintBoard()
        {
            for (int i = 0; i < Rules.ROWS; ++i)
            {
                for (int j = 0; j < Rules.COLUMNS; ++j)
                {
                    Console.Write($"{MatrixBoard[i][j]}\t");
                }
                Console.WriteLine();
            }
        }

        private void UpdatePieces(Piece movedPiece, Point e)
        {
            if (Math.Abs(MatrixBoard[e.Y][e.X]) == Rules.GENERAL)
            {
                _gameEnd = true;
            }
            _pieces.RemoveAll(piece => piece != movedPiece && piece.MatrixPos == e);
        }

        private void Piece_FocusedHandler(object sender, EventArgs e)
        {
            _isFocusing = true;
            _focusingPiece = (Piece)sender;
        }


        public override void Update(MouseState mouseState)
        {
            if (!_gameEnd)
            {
                CheckMateCheck();
                if (_isFocusing)
                {
                    _focusingPiece.Update(mouseState);
                }
                else
                {
                    UpdatePieces(mouseState);
                }
            }

        }

        private void CheckMateCheck()
        {
            if (_isCheckMate)
            {
                _gameMessage.Update();
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
            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            Board.Draw(spriteBatch);

            foreach (var piece in _pieces)
            {
                piece.Draw(spriteBatch);
            }
            if (_gameEnd)
            {
                _gameMessage.DrawString(spriteBatch, Resources.blackWins, Color.Red);
            }
            if (_isCheckMate)
            {
                _gameMessage.DrawString(spriteBatch, Resources.checkMate, Color.Red);
            }
        }
    }
}
