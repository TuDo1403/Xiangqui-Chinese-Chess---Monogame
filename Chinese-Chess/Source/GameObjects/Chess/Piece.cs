using ChineseChess.Source.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using ChineseChess.Source.Main;
using ChineseChess.Source.GameRule;

namespace ChineseChess.Source.GameObjects.Chess
{
    public class Piece : GameModel
    {
        private bool _isFocusing = false;
        private bool _isDragging = false;


        protected List<Point> LegalMoves { get; private set; } = new List<Point>();


        public int Value { get; protected set; }

        public Point Index { get; set; }

        public Rectangle Bounds { get; set; }


        public event EventHandler<PositionTransitionEventArgs> Moved;
        public event EventHandler Focused;
        public event EventHandler<int> CheckMated;



        public virtual void FindLegalMoves(BoardState board)
        {
            LegalMoves.Clear();
            Index = Position.ToIndex();
            LegalMoves = PieceMoveFactory.CreatePieceMove(Value, Index).FindLegalMoves(board);
        }


        protected void HasCheckMateMove(BoardState board)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            foreach (var move in LegalMoves)
                if (Math.Abs((int)board[move.Y, move.X]) == (int)Pieces.R_General)
                {
                    Console.WriteLine($"{GetType()}[{Index.Y}][{Index.X}] move[{move.X}][{move.Y}]");
                    OnCheckMating();
                }
        }

        private void OnCheckMating() => (CheckMated as EventHandler<int>)?.Invoke(this, Value);

        protected void PrintLegalMove()
        {
            Console.WriteLine();
            foreach (var move in LegalMoves)
                Console.WriteLine($"{GetType()}[{Index.Y}][{Index.X}]: ({move.Y}, {move.X})");
        }



        public Piece(Texture2D txt, Vector2 position, int val, ChessBoard board) : base(txt)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            Value = val;
            board.BoardUpdated += Xiangqui_BoardUpdatedHandler;
            Position = position;
            Index = Position.ToIndex();
            SetBounds();
        }

        private void Xiangqui_BoardUpdatedHandler(object sender, BoardState board)
        {
            FindLegalMoves(board);
            HasCheckMateMove(board);
        }


        public void RemoveBoardUpdatedEventHandler(ChessBoard board)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            board.BoardUpdated -= Xiangqui_BoardUpdatedHandler;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                var layerDepth = _isFocusing ? 1f : 0.5f;
                spriteBatch.Draw(Texture, Position, Texture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }
            else
                throw new ArgumentNullException(nameof(spriteBatch));
        }


        public override void Update(MouseState mouseState)
        {
            if (Bounds.Contains(mouseState.Position))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    DragPiece(mouseState);
                else if (mouseState.LeftButton == ButtonState.Released && _isDragging == true)
                    SetNewMovePosition(mouseState);
            }
        }

        private void SetNewMovePosition(MouseState tileCenter)
        {
            var tilePos = tileCenter.Position.ToTopLeftPosition(Texture.Width, Texture.Height);
            SetMove(tilePos);
            _isFocusing = _isDragging = false;
        }

        private void DragPiece(MouseState mouseState)
        {
            _isFocusing = true;
            OnFocusing();
            _isDragging = true;
            SetPosition(mouseState.Position);
            SetBounds();
        }


        public void OnFocusing() => (Focused as EventHandler)?.Invoke(this, EventArgs.Empty);

        private void SetMove(Vector2 tilePos)
        {
            var legalPos = tilePos.GetLegalMovePosition(LegalMoves);
            if (legalPos != Vector2.Zero)
            {
                var currentIdx = Index;
                var newIdx = legalPos.ToIndex();
                Position = legalPos;

                OnMoving(new PositionTransitionEventArgs(currentIdx, newIdx));
            }
            else
            {
                Position = Index.ToPosition();
                OnMoving(new PositionTransitionEventArgs(Point.Zero, Index));
            }
            SetBounds();
        }

        public void SetMove(Point newIdx)
        {
            Position = newIdx.ToPosition();
            OnMoving(new PositionTransitionEventArgs(Index, newIdx));
            SetBounds();
        }

        private void OnMoving(PositionTransitionEventArgs eventArgs) => (Moved as EventHandler<PositionTransitionEventArgs>)?.Invoke(this, eventArgs);

        private void SetBounds()
        {
            Bounds = new Rectangle((int)Position.X, (int)Position.Y,
                                    Texture.Width, Texture.Height);
        }

        private void SetPosition(Point mousePosition)
        {
            var position = mousePosition.ToTopLeftPosition(Texture.Width, Texture.Height);
            Position = position;
        }
    }
}
