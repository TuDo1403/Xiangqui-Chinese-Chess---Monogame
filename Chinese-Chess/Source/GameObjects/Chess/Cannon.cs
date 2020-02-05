using ChineseChess.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChineseChess.Source.GameRule;

namespace ChineseChess.Source.GameObjects.Chess
{
    public sealed class Cannon : Piece
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
            for (int i = MatrixPos.X + 1; i < (int)BoardRule.Columns; ++i)
            {
                if (ChessBoard.MatrixBoard[posY][i] != 0)
                {
                    while (i < (int)BoardRule.Columns - 1)
                    {
                        ++i;
                        if (ChessBoard.MatrixBoard[posY][i] * Type > 0)
                        {
                            break;
                        }
                        if (ChessBoard.MatrixBoard[posY][i] * Type < 0)
                        {
                            ValidMoves.Add(new Point(i, posY));
                            break;
                        }
                    }
                    break;
                }
                else
                {
                    ValidMoves.Add(new Point(i, posY));
                }
            }

            if (MatrixPos.X - 1 < 0) return;
            for (int i = MatrixPos.X - 1; i >= 0; --i)
            {
                if (ChessBoard.MatrixBoard[posY][i] != 0)
                {
                    while (i > 0)
                    {
                        --i;
                        if (ChessBoard.MatrixBoard[posY][i] * Type > 0)
                        {
                            break;
                        }
                        if (ChessBoard.MatrixBoard[posY][i] * Type < 0)
                        {
                            ValidMoves.Add(new Point(i, posY));
                            break;
                        }
                    }
                    break;
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
            for (int i = MatrixPos.Y + 1; i < (int)BoardRule.Rows; ++i)
            {
                if (ChessBoard.MatrixBoard[i][posX] != 0)
                {
                    while (i < (int)BoardRule.Rows - 1)
                    {
                        ++i;
                        if (ChessBoard.MatrixBoard[i][posX] * Type > 0)
                        {
                            break;
                        }
                        if (ChessBoard.MatrixBoard[i][posX] * Type < 0)
                        {
                            ValidMoves.Add(new Point(posX, i));
                            break;
                        }
                    }
                    break;
                }
                else
                {
                    ValidMoves.Add(new Point(posX, i));
                }
            }

            if (MatrixPos.Y - 1 < 0) return;
            for (int i = MatrixPos.Y - 1; i >= 0; --i)
            {
                if (ChessBoard.MatrixBoard[i][posX] != 0)
                {
                    while (i > 0)
                    {
                        --i;
                        if (ChessBoard.MatrixBoard[i][posX] * Type > 0)
                        {
                            break;
                        }
                        if (ChessBoard.MatrixBoard[i][posX] * Type < 0)
                        {
                            ValidMoves.Add(new Point(posX, i));
                            break;
                        }
                    }
                    break;
                }
                else
                {
                    ValidMoves.Add(new Point(posX, i));
                }
            }
        }
    }
}
