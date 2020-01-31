using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public class Cannon : Piece
    {
        public Cannon(Texture2D texture, Vector2 position, int type) : base(texture, position, type)
        {
        }


        protected override void FindNextMoves()
        {
            base.FindNextMoves();
            FindHorizontalMoves();
            FindVerticalMoves();
        }

        protected override void FindHorizontalMoves()
        {
            int posY = MatrixPos.Y;
            for (int i = MatrixPos.X + 1; i < Rules.COLUMNS; ++i)
            {
                if (Xiangqui.Board[posY][i] != 0)
                {
                    while (i < Rules.COLUMNS-1)
                    {
                        ++i;
                        if (Xiangqui.Board[posY][i] * Type > 0)
                        {
                            break;
                        }
                        if (Xiangqui.Board[posY][i] * Type < 0)
                        {
                            ValidMoves.Add(new Point(i, posY));
                            break;
                        }
                    }
                }
                else
                {
                    ValidMoves.Add(new Point(i, posY));
                }
            }

            if (MatrixPos.X - 1 < 0) return;
            for (int i = MatrixPos.X - 1; i >= 0; --i)
            {
                if (Xiangqui.Board[posY][i] != 0)
                {
                    while (i > 0)
                    {
                        --i;
                        if (Xiangqui.Board[posY][i] * Type > 0)
                        {
                            break;
                        }
                        if (Xiangqui.Board[posY][i] * Type < 0)
                        {
                            ValidMoves.Add(new Point(i, posY));
                            break;
                        }
                    }
                }
                else
                {
                    ValidMoves.Add(new Point(i, posY));
                }
            }
        }

        protected override void FindVerticalMoves()
        {
            int posX = MatrixPos.X;
            for (int i = MatrixPos.Y + 1; i < Rules.ROWS; ++i)
            {
                if (Xiangqui.Board[i][posX] != 0)
                {
                    while (i < Rules.ROWS - 1)
                    {
                        ++i;
                        if (Xiangqui.Board[i][posX] * Type > 0)
                        {
                            break;
                        }
                        if (Xiangqui.Board[i][posX] * Type < 0)
                        {
                            ValidMoves.Add(new Point(posX, i));
                            break;
                        }
                    }
                }
                else
                {
                    ValidMoves.Add(new Point(posX, i));
                }
            }

            if (MatrixPos.Y - 1 < 0) return;
            for (int i = MatrixPos.Y - 1; i >= 0; --i)
            {
                if (Xiangqui.Board[i][posX] != 0)
                {
                    while (i > 0)
                    {
                        --i;
                        if (Xiangqui.Board[i][posX] * Type > 0)
                        {
                            break;
                        }
                        if (Xiangqui.Board[i][posX] * Type < 0)
                        {
                            ValidMoves.Add(new Point(posX, i));
                            break;
                        }
                    }
                }
                else
                {
                    ValidMoves.Add(new Point(posX, i));
                }
            }
        }
    }
}
