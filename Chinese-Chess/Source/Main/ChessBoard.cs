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
using System.Linq;
using ChineseChess.Source.GameRule;

namespace ChineseChess.Source.Main
{
    public class ChessBoard : GameModel
    {
        private static ChessBoard _instance;

        private GameState _gameState;

        private int _turn;
        private int _checkMateSide;

        private uint _checkCount;

        private readonly Message[] _messages;

        private Piece _focusingPiece;


        public List<Piece>[] Pieces { get; private set; }

        public Board Board { get; private set; }

        public static event EventHandler BoardUpdated;

        public static int[][] MatrixBoard { get; private set; }



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
            _gameState = GameState.Idle;
            _checkCount = 0;

            _turn = new Random().Next(0, 2);
            _messages = new Message[5];

            Pieces = new List<Piece>[2];
            Pieces[(int)GameTeam.Black] = new List<Piece>();
            Pieces[(int)GameTeam.Red] = new List<Piece>();

            MatrixBoard = new int[10][]
            {
                new int[9] { (int)GameRule.Pieces.B_Chariot,  (int)GameRule.Pieces.B_Horse, (int)GameRule.Pieces.B_Elephant, (int)GameRule.Pieces.B_Advisor, (int)GameRule.Pieces.B_General, (int)GameRule.Pieces.B_Advisor, (int)GameRule.Pieces.B_Elephant,  (int)GameRule.Pieces.B_Horse, (int)GameRule.Pieces.B_Chariot},
                new int[9] {                     0,                    0,                      0,                     0,                     0,                     0,                      0,                    0,                     0},
                new int[9] {                     0, (int)GameRule.Pieces.B_Cannon,                      0,                     0,                     0,                     0,                      0, (int)GameRule.Pieces.B_Cannon,                     0},
                new int[9] { (int)GameRule.Pieces.B_Soldier,                    0,  (int)GameRule.Pieces.B_Soldier,                     0, (int)GameRule.Pieces.B_Soldier,                     0,  (int)GameRule.Pieces.B_Soldier,                    0, (int)GameRule.Pieces.B_Soldier},
                new int[9] {                     0,                    0,                      0,                     0,                     0,                     0,                      0,                    0,                     0},
                new int[9] {                     0,                    0,                      0,                     0,                     0,                     0,                      0,                    0,                     0},
                new int[9] { (int)GameRule.Pieces.R_Soldier,                    0,  (int)GameRule.Pieces.R_Soldier,                     0, (int)GameRule.Pieces.R_Soldier,                     0,  (int)GameRule.Pieces.R_Soldier,                    0, (int)GameRule.Pieces.R_Soldier},
                new int[9] {                     0, (int)GameRule.Pieces.R_Cannon,                      0,                     0,                     0,                     0,                      0, (int)GameRule.Pieces.R_Cannon,                     0},
                new int[9] {                     0,                    0,                      0,                     0,                     0,                     0,                      0,                    0,                     0},
                new int[9] { (int)GameRule.Pieces.R_Chariot,  (int)GameRule.Pieces.R_Horse, (int)GameRule.Pieces.R_Elephant, (int)GameRule.Pieces.R_Advisor, (int)GameRule.Pieces.R_General, (int)GameRule.Pieces.R_Advisor, (int)GameRule.Pieces.R_Elephant,  (int)GameRule.Pieces.R_Horse, (int)GameRule.Pieces.R_Chariot}
            };
        }


        public void LoadContent(ContentManager contentManager)
        {
            if (contentManager == null)
            {
                throw new ArgumentNullException(nameof(contentManager));
            }

            for (int i = 0; i < (int)BoardRule.Rows; ++i)
            {
                for (int j = 0; j < (int)BoardRule.Columns; ++j)
                {
                    if (MatrixBoard[i][j] != 0)
                    {
                        PutPieceOnBoard(contentManager, new Point(j, i));
                    }
                }
            }

            Board = Board.GetInstance(contentManager.Load<Texture2D>("board"));
            LoadMessage(contentManager);

            OnBoardUpdating();
        }

        private void LoadMessage(ContentManager contentManager)
        {
            var font = contentManager.Load<SpriteFont>(@"Font\GameEnd");
            var boardCenterPos = new Point(Board.Width / 2, Board.Height / 2);
            _messages[(int)GameState.BlackWins] = new Message(font, Resources.blackWins, boardCenterPos);
            _messages[(int)GameState.RedWins] = new Message(font, Resources.redWins, boardCenterPos);
            _messages[(int)GameState.CheckMate] = new Message(font, Resources.checkMate, boardCenterPos);
            _messages[(int)GameState.RedTurn] = new Message(font, Resources.redTurn, boardCenterPos);
            _messages[(int)GameState.BlackTurn] = new Message(font, Resources.blackTurn, boardCenterPos);
        }

        private void PutPieceOnBoard(ContentManager contentManager, Point matrixPos)
        {
            var piece = PieceFactory.CreatePiece(MatrixBoard[matrixPos.Y][matrixPos.X], 
                                                 matrixPos, 
                                                 contentManager);
            piece.Focused += Piece_FocusedHandler;
            piece.Moved += Piece_MovedHandler;
            piece.CheckMated += Piece_CheckMatedHandler;

            if (piece.Type > 0)
            {
                Pieces[(int)GameTeam.Red].Add(piece);
            }
            else
            {
                Pieces[(int)GameTeam.Black].Add(piece);
            }
        }

        private void Piece_CheckMatedHandler(object sender, int e)
        {
            _checkCount++;

            // Quadruple check
            if (_checkCount >= 4)
            {
                _gameState = GameState.GameOver;
            }
            else
            {
                _gameState = GameState.CheckMate;
                _checkMateSide = e;
            }
        }

        private void Piece_MovedHandler(object sender, Point e)
        {
            _gameState = GameState.Idle;

            if (e != _focusingPiece.MatrixPos)
            {
                _checkCount = 0;
                _turn = -(_turn) + 1; // switch side
                _messages[(int)GameState.CheckMate].ResetTimer();
                _messages[_turn + 3].ResetTimer();
                UpdateBoard(sender as Piece, e);
            }
        }


        private void UpdateBoard(Piece movedPiece, Point e)
        {
            UpdatePieces(e);
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
            for (int i = 0; i < (int)BoardRule.Rows; ++i)
            {
                for (int j = 0; j < (int)BoardRule.Columns; ++j)
                {
                    Console.Write($"{MatrixBoard[i][j]}\t");
                }
                Console.WriteLine();
            }
        }

        private void UpdatePieces(Point e)
        {
            if (Math.Abs(MatrixBoard[e.Y][e.X]) == (int)GameRule.Pieces.R_General)
            {
                _gameState = GameState.GameOver;
            }

            var p = Pieces[_turn].Where(c => c.MatrixPos == e)
                                  .SingleOrDefault();
            if (p != null)
            {
                p.RemoveBoardUpdatedEventHandler();
            }
            
            Pieces[_turn].RemoveAll(piece => piece.MatrixPos == e);
        }

        private void Piece_FocusedHandler(object sender, EventArgs e)
        {
            _gameState = GameState.PieceMoving;
            _focusingPiece = sender as Piece;
        }


        public override void Update(MouseState mouseState)
        {
            if (_gameState != GameState.GameOver)
            {
                CheckMateUpdate();
                if (_gameState == GameState.PieceMoving)
                {
                    _focusingPiece.Update(mouseState);
                }
                else
                {
                    UpdatePiecesInTurn(mouseState);
                }
            }

        }

        private void CheckMateUpdate()
        {
            if (_gameState == GameState.CheckMate)
            {
                _messages[(int)_gameState].Update();
            }
        }

        private void UpdatePiecesInTurn(MouseState mouseState)
        {
            _messages[_turn + 3].Update();
            foreach (var piece in Pieces[_turn])
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
            DrawPieces(spriteBatch);

            if (_gameState == GameState.GameOver)
            {
                DrawGameOverMessage(spriteBatch);
            }
            else if (_gameState == GameState.CheckMate)
            {
                DrawCheckMateMessage(spriteBatch);
            }
            else
            {
                DrawTurnMessage(spriteBatch);
            }
        }

        private void DrawTurnMessage(SpriteBatch spriteBatch)
        {
            var color = Color.Red;
            if (_turn + 3 == (int)GameState.BlackTurn)
            {
                color = Color.Black;
            }
            _messages[_turn + 3].DrawString(spriteBatch, color);
        }

        private void DrawPieces(SpriteBatch spriteBatch)
        {
            foreach (var piece in from team in Pieces
                                  from piece in team
                                  select piece)
            {
                piece.Draw(spriteBatch);
            }
        }

        private void DrawCheckMateMessage(SpriteBatch spriteBatch)
        {
            var color = Color.Red;
            if (_checkMateSide < 0)
            {
                color = Color.Black;
            }
            _messages[(int)_gameState].DrawString(spriteBatch, color);
        }

        private void DrawGameOverMessage(SpriteBatch spriteBatch)
        {
            if (_focusingPiece.Type < 0)
            {
                _messages[(int)GameState.BlackWins].DrawString(spriteBatch, Color.Black);
            }
            else
            {
                _messages[(int)GameState.RedWins].DrawString(spriteBatch, Color.Red);
            }
        }
    }
}
