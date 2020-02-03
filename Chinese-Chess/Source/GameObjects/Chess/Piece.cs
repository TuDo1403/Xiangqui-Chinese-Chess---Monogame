using ChineseChess.Source.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using ChineseChess.Source.Main;

namespace ChineseChess.Source.GameObjects.Chess
{
    public abstract class Piece : GameModel
    {
        private bool _isFocusing = false;
        private bool _isDragging = false;


        protected List<Point> ValidMoves { get; private set; } = new List<Point>();


        public int Type { get; protected set; }

        public Point MatrixPos { get; set; }

        public Rectangle Bounds { get; set; }


        public event EventHandler<Point> Moved;
        public event EventHandler Focused;
        public event EventHandler CheckMated;


        protected virtual Predicate<Point> OutOfRangeMove()
        {
            return c => false;
        }

        protected virtual void FindNextMoves()
        {
            ValidMoves.Clear();
            MatrixPos = Position.ToMatrixPos();
        }

        protected virtual void RemoveInvalidMoves() { }

        protected virtual void FindVerticalMoves() { }

        protected virtual void FindHorizontalMoves() { }

        protected virtual bool IsBlockedMove(Point point) => false;

        protected bool StillHasValidMoves(int row, int column)
        {
            if (ChessBoard.MatrixBoard[row][column] * Type > 0)
            {
                return false;
            }
            else
            {
                ValidMoves.Add(new Point(column, row));
                if (ChessBoard.MatrixBoard[row][column] * Type < 0)
                {
                    return false;
                }
            }
            return true;
        }

        protected void HasCheckMateMove()
        {
            foreach (var move in ValidMoves)
            {
                if (Math.Abs(ChessBoard.MatrixBoard[move.Y][move.X]) == Rules.GENERAL)
                {
                    Console.WriteLine($"{GetType()}[{MatrixPos.Y}][{MatrixPos.X}] move[{move.Y}][{move.X}]");
                    OnCheckMating();
                }
            }
        }

        private void OnCheckMating() => (CheckMated as EventHandler)?.Invoke(this, EventArgs.Empty);

        protected void PrintValidMove()
        {
            Console.WriteLine();
            foreach (var move in ValidMoves)
            {
                Console.WriteLine($"{GetType().ToString()}[{MatrixPos.Y}][{MatrixPos.X}]: ({move.Y}, {move.X})");
            }
        }



        public Piece(Texture2D texture, Vector2 position, int type) : base(texture)
        {
            Type = type;
            ChessBoard.BoardUpdated += Xiangqui_BoardUpdatedHandler;
            Position = position;
            MatrixPos = Position.ToMatrixPos();
            SetBounds();
        }

        private void Xiangqui_BoardUpdatedHandler(object sender, EventArgs e)
        {
            FindNextMoves();
            HasCheckMateMove();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                var layerDepth = 0.5f;
                if (_isFocusing)
                {
                    layerDepth = 1f;
                }
                spriteBatch.Draw(Texture, Position, Texture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }
            else
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }
        }


        public override void Update(MouseState mouseState)
        {
            if (Bounds.Contains(mouseState.Position))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    MovePieceAlongCursor(mouseState);
                }
                else if (mouseState.LeftButton == ButtonState.Released && _isDragging == true)
                {
                    SetNewMovePosition(mouseState);
                }
            }
        }

        private void SetNewMovePosition(MouseState centerSpritePos)
        {
            var spritePos = centerSpritePos.Position.ToSpriteTopLeftPosition(Texture.Width, Texture.Height);
            SetMove(spritePos);
            _isFocusing = false;
            _isDragging = false;
        }

        private void MovePieceAlongCursor(MouseState mouseState)
        {
            _isFocusing = true;
            OnFocusing();
            _isDragging = true;
            SetPosition(mouseState.Position);
            SetBounds();
        }

        private void OnMoving(Point newMatrixPosition)
        {
            (Moved as EventHandler<Point>)?.Invoke(this, newMatrixPosition);
        }

        private void OnFocusing()
        {
            (Focused as EventHandler)?.Invoke(this, EventArgs.Empty);
        }

        private void SetMove(Vector2 spritePos)
        {
            var validPos = spritePos.GetValidMovePosition(ValidMoves);
            if (validPos != Vector2.Zero)
            {
                Position = validPos;
                OnMoving(validPos.ToMatrixPos());
            }
            else
            {
                Position = MatrixPos.ToSpritePos();
                OnMoving(MatrixPos);
            }
            SetBounds();
        }

        private void SetBounds()
        {
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        private void SetPosition(Point mousePosition)
        {
            var position = mousePosition.ToSpriteTopLeftPosition(Texture.Width, Texture.Height);
            Position = position;
        }
    }
}
