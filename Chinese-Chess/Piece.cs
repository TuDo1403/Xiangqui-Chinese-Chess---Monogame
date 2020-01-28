using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
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


        protected virtual void FindNextMoves()
        {
            ValidMoves.Clear();
        }

        protected abstract void FindVerticalMoves();

        protected abstract void FindHorizontalMoves();

        protected virtual bool StillHasValidMoves(int x, int y)
        {
            if (Xiangqui.Board[x][y] > 0)
            {
                return false;
            }
            else
            {
                ValidMoves.Add(new Point(x, y));
                if (Xiangqui.Board[x][y] < 0)
                {
                    return false;
                }
            }
            return true;
        }



        public Piece(Texture2D texture, Vector2 position) : base(texture)
        {
            Position = position;
            MatrixPos = Position.ToMatrixPos();
            SetBounds();
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

        private void SetNewMovePosition(MouseState mouseState)
        {
            var spritePos = mouseState.Position.ToSpritePosition(Texture.Width, Texture.Height);
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
                var newMatrixPos = validPos.ToMatrixPos();
                OnMoving(newMatrixPos);
                MatrixPos = newMatrixPos;
                Position = validPos;
                FindNextMoves();
            }
            else
            {
                Position = MatrixPos.ToSpritePos();
            }
            SetBounds();
        }

        private void SetBounds()
        {
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        private void SetPosition(Point mousePosition)
        {

            var position = mousePosition.ToSpritePosition(Texture.Width, Texture.Height);
            Position = position;
        }
    }
}
