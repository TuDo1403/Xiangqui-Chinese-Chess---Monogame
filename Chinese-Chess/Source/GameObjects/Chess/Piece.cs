using ChineseChess.Source.GameRule;
using ChineseChess.Source.Helper;
using ChineseChess.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ChineseChess.Source.GameObjects.Chess
{
    public class Piece : GameModel
    {
        private bool _isFocusing = false;
        private bool _isDragging = false;
        private bool _isMoving = false;

        private float _speed = 1f;

        private List<Point> _legalMoves = new List<Point>();

        private Texture2D _rect;


        public int Value { get; protected set; }

        public Point Index { get; set; }

        public Rectangle Bounds { get; set; }


        public event EventHandler<PositionTransitionEventArgs> Moved;
        public event EventHandler Focused;
        public event EventHandler<int> CheckMated;



        public virtual void FindLegalMoves(BoardState board)
        {
            _legalMoves.Clear();
            Index = Position.ToIndex();
            _legalMoves = PieceMoveFactory.CreatePieceMove(Value, Index)
                                          .FindLegalMoves(board);
        }


        protected void HasCheckMateMove(BoardState board)
        {
            foreach (var move in _legalMoves)
                if (Math.Abs(board[move.Y, move.X]) == (int)Pieces.R_General)
                    OnCheckMating();
        }

        private void OnCheckMating() => (CheckMated as EventHandler<int>)?.Invoke(this, Value);

        public Piece(Texture2D txt, Texture2D rect, Vector2 position, int val, ChessBoard board) : base(txt)
        {
            Value = val;
            _rect = rect;
            board.BoardUpdated += BoardUpdatedHandler;
            Position = position;
            Index = Position.ToIndex();
            SetBounds();
        }

        private void BoardUpdatedHandler(object sender, BoardState board)
        {
            FindLegalMoves(board);
            HasCheckMateMove(board);
        }


        public void RemoveBoardUpdatedEvent(ChessBoard board) => board.BoardUpdated -= BoardUpdatedHandler;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                var layerDepth = _isFocusing ? 1f : 0.5f;
                spriteBatch.Draw(Texture, Position, Texture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                if (_isFocusing)
                {
                    foreach (var legalMove in _legalMoves)
                    {
                        spriteBatch.Draw(_rect, legalMove.ToPosition(), _rect.Bounds, Color.White, 0f, 
                                        Vector2.Zero, 1f, SpriteEffects.None, 0.75f);
                    }
                }
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
            var legalPos = tilePos.GetLegalMovePosition(_legalMoves);
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
            //Position = newIdx.ToPosition();
            
            
        }

        public void OnMoving(PositionTransitionEventArgs eventArgs) => (Moved as EventHandler<PositionTransitionEventArgs>)?.Invoke(this, eventArgs);

        public void SetBounds()
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
