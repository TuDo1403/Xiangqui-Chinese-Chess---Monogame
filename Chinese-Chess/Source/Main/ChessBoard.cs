using ChineseChess.Properties;
using ChineseChess.Source.GameObjects;
using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Helper;
using ChineseChess.Source.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ChineseChess.Source.Main
{
    public class ChessBoard : GameModel
    {
        private static ChessBoard _instance;

        private GameState _gameState;

        private int _turn;
        private int _checkMateSide;

        private uint _checkCount;

        private readonly int _searchDepth;

        private readonly Message[] _messages;

        private readonly Player[] _players;

        private readonly BoardState _matrixBoard;

        private Piece _focusingPiece;

        public Board Board { get; private set; }

        public event EventHandler<BoardState> BoardUpdated;



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
            _gameState = GameState.IDLE;
            _checkCount = 0;

            //_turn = new Random().Next(0, 2);
            _turn = 1;
            _messages = new Message[5];

            _players = new Player[2];
            _searchDepth = 5;
            _players[(int)Team.BLACK] = new Computer(Team.BLACK, _searchDepth);
            _players[(int)Team.RED] = new Human();
            //_players[(int)Team.BLACK] = new Human();
            //_players[(int)GameTeam.RED] = new Computer(GameTeam.RED, _searchDepth + 1);

            _matrixBoard = new BoardState();
        }


        public void LoadContent(ContentManager contentManager)
        {
            if (contentManager == null)
            {
                throw new ArgumentNullException(nameof(contentManager));
            }

            for (int i = 0; i < (int)Rule.ROW; ++i)
            {
                for (int j = 0; j < (int)Rule.COL; ++j)
                {
                    if (_matrixBoard[i, j] != 0)
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
            var boardCenter = new Point(Board.Width / 2, Board.Height / 2);
            _messages[(int)GameState.B_WIN] = new Message(font, Resources.blackWins, boardCenter);
            _messages[(int)GameState.R_WIN] = new Message(font, Resources.redWins, boardCenter);
            _messages[(int)GameState.CHECKMATE] = new Message(font, Resources.checkMate, boardCenter);
            _messages[(int)GameState.R_TURN] = new Message(font, Resources.redTurn, boardCenter);
            _messages[(int)GameState.B_TURN] = new Message(font, Resources.blackTurn, boardCenter);
        }

        private void PutPieceOnBoard(ContentManager contentManager, Point boardIdx)
        {
            var piece = PieceFactory.CreatePiece(_matrixBoard[boardIdx.Y, boardIdx.X],
                                                 boardIdx, _instance,
                                                 contentManager);
            piece.Focused += Piece_FocusedHandler;
            piece.Moved += Piece_MovedHandler;
            piece.CheckMated += Piece_CheckMatedHandler;


            if (piece.Value > 0)
            {
                _players[(int)Team.RED].AddPiece(piece);
            }
            else
            {
                _players[(int)Team.BLACK].AddPiece(piece);
            }
        }

        private void Piece_CheckMatedHandler(object sender, int e)
        {
            _checkCount++;

            // Quadruple check
            if (_checkCount >= 4)
            {
                _gameState = GameState.GAMEOVER;
            }
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
            //UpdatePosition(e.CurrentIdx, e.NewIdx);
            _matrixBoard.MakeMove(e.CurrentIdx, e.NewIdx);
            OnBoardUpdating();
        }

        private void OnBoardUpdating()
        {
            (BoardUpdated as EventHandler<BoardState>)?.Invoke(this, _matrixBoard);
        }

        private void UpdatePieces(Point e)
        {
            // Check if attacking General
            if (Math.Abs(_matrixBoard[e.Y, e.X]) == (int)Pieces.R_General)
            {
                _gameState = GameState.GAMEOVER;
            }

            _players[_turn].RemovePiece(this, e);
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
            if (_gameState == GameState.CHECKMATE)
            {
                _messages[(int)_gameState].Update();
            }
        }

        private void UpdatePiecesInTurn(MouseState mouseState)
        {
            _messages[_turn + 3].Update();
            if (_players[_turn].GetType() == typeof(Computer))
            {
                _players[_turn].Update(_matrixBoard);
            }
            else
            {
                _players[_turn].Update(mouseState);
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

            if (_gameState == GameState.GAMEOVER)
            {
                DrawGameOverMessage(spriteBatch);
            }
            else if (_gameState == GameState.CHECKMATE)
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
            if (_turn + 3 == (int)GameState.B_TURN)
            {
                color = Color.Black;
            }

            _messages[_turn + 3].DrawString(spriteBatch, color);
        }

        private void DrawPieces(SpriteBatch spriteBatch)
        {
            foreach (var player in _players)
            {
                player.DrawPieces(spriteBatch);
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
            if (_focusingPiece.Value < 0)
            {
                _messages[(int)GameState.B_WIN].DrawString(spriteBatch, Color.Black);
            }
            else
            {
                _messages[(int)GameState.R_WIN].DrawString(spriteBatch, Color.Red);
            }
        }
    }
}
