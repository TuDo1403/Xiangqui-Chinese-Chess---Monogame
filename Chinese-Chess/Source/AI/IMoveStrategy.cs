using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI
{
    public interface IMoveStrategy
    {
        int BoardEvaluator(BoardState board);
    }
}
