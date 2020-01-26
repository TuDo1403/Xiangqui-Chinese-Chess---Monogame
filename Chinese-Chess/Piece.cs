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

        private bool _isDragging = false;


        protected List<Vector2> validMoves { get; set; } = new List<Vector2>();

        protected Vector2 matrixPos { get; set; }


        public Rectangle Bounds { get; set; }


        protected virtual void FindNextMoves()
        {
            validMoves.Clear();
            matrixPos = Position.ToMatrixPos();
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
                validMoves.Add(new Vector2(x, y));
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
            //PreviousPosition = Position;
            SetBounds();
        }


        public override void Update(MouseState mouseState)
        {
            if (Bounds.Contains(mouseState.Position))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    _isDragging = true;
                    SetPosition(mouseState.Position.X - (texture.Width / 2), mouseState.Position.Y - (texture.Height / 2));
                    SetBounds();

                }
                else if (mouseState.LeftButton == ButtonState.Released && _isDragging == true)
                {
                    SetMove(mouseState.Position.ToVector2());
                    
                    _isDragging = false;
                }
            }
        }

        private void SetMove(Vector2 mousePos)
        {
            var validPos = mousePos.GetValidMovePosition(validMoves);
            if (validPos != Vector2.Zero)
            {
                Position = validPos;
                //PreviousPosition = Position;
                FindNextMoves();
            }
            else
            {
                Position = matrixPos.ToSpritePos();
            }
            SetBounds();
        }

        private void SetBounds()
        {
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
        }

        private void SetPosition(int x, int y)
        {
            var position = new Vector2(x, y);
            Position = position;
        }
    }
}
