using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI
{
    interface IMoveStrategy
    {
        int BoardEvaluator(int[][] board, int depth);
    }
}
