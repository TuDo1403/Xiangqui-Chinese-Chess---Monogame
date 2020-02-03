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
        private const int RED_TURN = 1;

        private static ChessBoard _instance;

        private int _turn;

        private uint _checkCount = 0;

        private bool _isCheckMate = false;
        private bool _gameOver = false;
        private bool _isFocusing = false;


        private Message _gameMessage;

        private Piece _focusingPiece;

        private readonly List<Piece> _redPieces;
        private readonly List<Piece> _blackPieces;


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
            _redPieces = new List<Piece>();
            _blackPieces = new List<Piece>();
            _turn = new Random().Next(0, 1);
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
                                       new Vector2(PositionHelper.X_OFFSET_FROM_TOP_LEFT_WIN, 
                                       Board.Height/2));

            OnBoardUpdating();
        }

        private void PutPieceOnBoard(ContentManager contentManager, Point matrixPos)
        {
            var piece = PieceFactory.CreatePiece(MatrixBoard[matrixPos.Y][matrixPos.X], matrixPos, contentManager);
            piece.Focused += Piece_FocusedHandler;
            piece.Moved += Piece_MovedHandler;
            piece.CheckMated += Piece_CheckMatedHandler;

            if (piece.Type > 0)
            {
                _redPieces.Add(piece);
            }
            else
            {
                _blackPieces.Add(piece);
            }
        }

        private void Piece_CheckMatedHandler(object sender, EventArgs e)
        {
            _checkCount++;
            // Quadruple check
            if (_checkCount >= 4)
            {
                _gameOver = true;
            }
            else
            {
                _isCheckMate = true;
            }
        }

        private void Piece_MovedHandler(object sender, Point e)
        {
            _isFocusing = false;

            if (e != _focusingPiece.MatrixPos)
            {
                _checkCount = 0;
                _turn = -_turn + 1; // switch side
                _isCheckMate = false;
                _gameMessage.ResetTimer();
                UpdateBoard(sender as Piece, e);
            }
        }

        private void UpdateBoard(Piece movedPiece, Point e)
        {
            UpdatePieces(movedPiece, e);
            UpdatePosition(movedPiece, e);
            OnBoardUpdating();
        }

        private void OnBoardUpdating() => (BoardUpdated as EventHandler)?.Invoke(this, EventArgs.Empty);

        private void UpdatePosition(Piece movedPiece, Point newMatrixPos)
        {
            var oldMatrixPos = _focusingPiece.MatrixPos;
            MatrixBoard[oldMatrixPos.Y][oldMatrixPos.X] = 0;
            MatrixBoard[newMatrixPos.Y][newMatrixPos.X] = movedPiece.Type;

            //PrintBoard();
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
                _gameOver = true;
            }
            _redPieces.RemoveAll(piece => piece != movedPiece && piece.MatrixPos == e);
            _blackPieces.RemoveAll(piece => piece != movedPiece && piece.MatrixPos == e);
        }

        private void Piece_FocusedHandler(object sender, EventArgs e)
        {
            _isFocusing = true;
            _focusingPiece = (Piece)sender;
        }


        public override void Update(MouseState mouseState)
        {
            if (!_gameOver)
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
            if (_turn == RED_TURN)
            {
                UpdateRedPieces(mouseState);
            }
            else
            {
                UpdateBlackPieces(mouseState);
            }
        }

        private void UpdateBlackPieces(MouseState mouseState)
        {
            foreach (var piece in _blackPieces)
            {
                piece.Update(mouseState);
            }
        }

        private void UpdateRedPieces(MouseState mouseState)
        {
            foreach (var piece in _redPieces)
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
            DrawRedPieces(spriteBatch);
            DrawBlackPieces(spriteBatch);
            if (_gameOver)
            {
                DrawGameOverMessage(spriteBatch);
            }
            else if (_isCheckMate)
            {
                _gameMessage.DrawString(spriteBatch, Resources.checkMate, Color.Red);
            }
        }

        private void DrawGameOverMessage(SpriteBatch spriteBatch)
        {
            _gameMessage.DrawString(spriteBatch, Resources.blackWins, Color.Red);
        }

        private void DrawBlackPieces(SpriteBatch spriteBatch)
        {
            foreach (var piece in _blackPieces)
            {
                piece.Draw(spriteBatch);
            }
        }

        private void DrawRedPieces(SpriteBatch spriteBatch)
        {
            foreach (var piece in _redPieces)
            {
                piece.Draw(spriteBatch);
            }
        }
    }
}
