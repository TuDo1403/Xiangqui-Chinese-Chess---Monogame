using ChineseChess.Source.GameRule;
using ChineseChess.Source.Helper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI
{
    public class MiniMax : IMoveStrategy
    {
        private int[][] _board;

        public int BoardEvaluator(int[][] board, int depth)
        {
            throw new NotImplementedException();
        }

        public MiniMax(int[][] board)
        {
            _board = board;
        }

        public void DoSomeThing()
        {
            //for (int i = 0; i < (int)BoardRule.ROW; ++i)
            //{
            //    for (int j = 0; j < (int)BoardRule.COL; ++j)
            //    {
            //        var piece = PieceFactory.CreatePiece(_board[j][i], new Point());
            //    }
            //}
        }
    }
}
