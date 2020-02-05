using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameRule
{
    enum GameState
    {
        BlackWins,
        RedWins,
        CheckMate,
        BlackTurn,
        RedTurn,
        PieceMoving,
        GameOver,
        Idle
    }
}
