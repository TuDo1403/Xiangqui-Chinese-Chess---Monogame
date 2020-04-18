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

        public event EventHandler<int[][]> BoardUpdated;

        public int[][] ArrayBoard { get; private set; }



        public static ChessBoard GetInstance()
        {
            if (_instance == null)
                _instance = new ChessBoard();
            return _instance;
        }

        private void LoadArrayBoard()
        {
            ArrayBoard = new int[10][];
            for (int i = 0; i < 10; ++i)
            {
                ArrayBoard[i] = new int[9];
                for (int j = 0; j < 9; ++j)
                    ArrayBoard[i][j] = 0;
            }

            ArrayBoard[0][0] = ArrayBoard[0][8] = -8;
            ArrayBoard[0][1] = ArrayBoard[0][7] = -4;
            ArrayBoard[0][2] = ArrayBoard[0][6] = -3;
            ArrayBoard[0][3] = ArrayBoard[0][5] = -2;
            ArrayBoard[0][4] = -100;
            ArrayBoard[2][1] = ArrayBoard[2][7] = -5;
            ArrayBoard[3][0] = ArrayBoard[3][2] = -1;
            ArrayBoard[3][4] = ArrayBoard[3][6] = ArrayBoard[3][8] = -1;

            ArrayBoard[9][0] = ArrayBoard[9][8] = 8;
            ArrayBoard[9][1] = ArrayBoard[9][7] = 4;
            ArrayBoard[9][2] = ArrayBoard[9][6] = 3;
            ArrayBoard[9][3] = ArrayBoard[9][5] = 2;
            ArrayBoard[9][4] = 100;
            ArrayBoard[7][1] = ArrayBoard[7][7] = 5;
            ArrayBoard[6][0] = ArrayBoard[6][2] = 1;
            ArrayBoard[6][4] = ArrayBoard[6][6] = ArrayBoard[6][8] = 1;

        }


        private ChessBoard()
        {
            _gameState = GameState.IDLE;
            _checkCount = 0;

            _turn = new Random().Next(0, 2);
            _messages = new Message[5];

            Pieces = new List<Piece>[2];
            Pieces[(int)GameTeam.BLACK] = new List<Piece>();
            Pieces[(int)GameTeam.RED] = new List<Piece>();

            LoadArrayBoard();
        }


        public void LoadContent(ContentManager contentManager)
        {
            if (contentManager == null)
            {
                throw new ArgumentNullException(nameof(contentManager));
            }

            for (int i = 0; i < (int)BoardRule.ROW; ++i)
                for (int j = 0; j < (int)BoardRule.COL; ++j)
                    if (ArrayBoard[i][j] != 0)
                        PutPieceOnBoard(contentManager, new Point(j, i));

            Board = Board.GetInstance(contentManager.Load<Texture2D>("board"));
            LoadMessage(contentManager);

            OnBoardUpdating();
        }

        private void LoadMessage(ContentManager contentManager)
        {
            var font = contentManager.Load<SpriteFont>(@"Font\GameEnd");
            var boardCenter = new Point(Board.Width / 2, Board.Height / 2);
            _messages[(int)GameState.B_WIN] = new Message(font, Resources.blackWins, boardCenter);
            _messages[(int)GameState.R_WIN] = new Message(font, Resources.redWins, boardCenter);
            _messages[(int)GameState.CHECKMATE] = new Message(font, Resources.checkMate, boardCenter);
            _messages[(int)GameState.R_TURN] = new Message(font, Resources.redTurn, boardCenter);
            _messages[(int)GameState.B_TURN] = new Message(font, Resources.blackTurn, boardCenter);
        }

        private void PutPieceOnBoard(ContentManager contentManager, Point boardIdx)
        {
            var piece = PieceFactory.CreatePiece(ArrayBoard[boardIdx.Y][boardIdx.X], 
                                                 boardIdx, _instance, 
                                                 contentManager);
            piece.Focused += Piece_FocusedHandler;
            piece.Moved += Piece_MovedHandler;
            piece.CheckMated += Piece_CheckMatedHandler;

            if (piece.Value > 0)
                Pieces[(int)GameTeam.RED].Add(piece);
            else
                Pieces[(int)GameTeam.BLACK].Add(piece);
        }

        private void Piece_CheckMatedHandler(object sender, int e)
        {
            _checkCount++;

            // Quadruple check
            if (_checkCount >= 4)
                _gameState = GameState.GAMEOVER;
            else
            {
                _gameState = GameState.CHECKMATE;
                _checkMateSide = e;
            }
        }

        private void Piece_MovedHandler(object sender, PositionTransitionEventArgs e)
        {
            _gameState = GameState.IDLE;

            if (e.NewIdx != _focusingPiece.Index)
            {
                _checkCount = 0;
                _turn = -(_turn) + 1; // switch side

                _messages[(int)GameState.CHECKMATE].ResetTimer();
                _messages[_turn + 3].ResetTimer();
                UpdateBoard(e);
            }
        }


        private void UpdateBoard(PositionTransitionEventArgs e)
        {
            UpdatePieces(e.NewIdx);
            UpdatePosition(e.CurrentIdx, e.NewIdx, e.Value);
            OnBoardUpdating();
        }

        private void OnBoardUpdating()
        {
            (BoardUpdated as EventHandler<int[][]>)?.Invoke(this, ArrayBoard);
        }

        private void UpdatePosition(Point oldIdx, Point newIdx, int value)
        {
            ArrayBoard[oldIdx.Y][oldIdx.X] = 0;
            ArrayBoard[newIdx.Y][newIdx.X] = value;

            // PrintBoard();
        }

        private void PrintBoard()
        {
            for (int i = 0; i < (int)BoardRule.ROW; ++i)
            {
                for (int j = 0; j < (int)BoardRule.COL; ++j)
                    Console.Write($"{ArrayBoard[i][j]}\t");

                Console.WriteLine();
            }
        }

        private void UpdatePieces(Point e)
        {
            // Check if attacking General
            if (Math.Abs(ArrayBoard[e.Y][e.X]) == (int)GameRule.Pieces.R_General)
                _gameState = GameState.GAMEOVER;

            var p = Pieces[_turn].Where(c => c.Index == e)
                                 .SingleOrDefault();
            if (p != null)
                p.RemoveBoardUpdatedEventHandler(this);
            
            Pieces[_turn].RemoveAll(piece => piece.Index == e);
        }

        private void Piece_FocusedHandler(object sender, EventArgs e)
        {
            _gameState = GameState.MOVING;
            _focusingPiece = sender as Piece;
        }


        public override void Update(MouseState mouseState)
        {
            if (_gameState != GameState.GAMEOVER)
            {
                CheckMateUpdate();
                if (_gameState == GameState.MOVING)
                    _focusingPiece.Update(mouseState);
                else
                    UpdatePiecesInTurn(mouseState);
            }

        }

        private void CheckMateUpdate()
        {
            if (_gameState == GameState.CHECKMATE) 
                _messages[(int)_gameState].Update();
        }

        private void UpdatePiecesInTurn(MouseState mouseState)
        {
            _messages[_turn + 3].Update();
            foreach (var piece in Pieces[_turn])
                piece.Update(mouseState);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) throw new ArgumentNullException(nameof(spriteBatch));

            Board.Draw(spriteBatch);
            DrawPieces(spriteBatch);

            if (_gameState == GameState.GAMEOVER)
                DrawGameOverMessage(spriteBatch);
            else if (_gameState == GameState.CHECKMATE)
                DrawCheckMateMessage(spriteBatch);
            else
                DrawTurnMessage(spriteBatch);
        }

        private void DrawTurnMessage(SpriteBatch spriteBatch)
        {
            var color = Color.Red;
            if (_turn + 3 == (int)GameState.B_TURN)
                color = Color.Black;

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
                color = Color.Black;

            _messages[(int)_gameState].DrawString(spriteBatch, color);
        }

        private void DrawGameOverMessage(SpriteBatch spriteBatch)
        {
            if (_focusingPiece.Value < 0)
                _messages[(int)GameState.B_WIN].DrawString(spriteBatch, Color.Black);
            else
                _messages[(int)GameState.R_WIN].DrawString(spriteBatch, Color.Red);
        }
    }
}
