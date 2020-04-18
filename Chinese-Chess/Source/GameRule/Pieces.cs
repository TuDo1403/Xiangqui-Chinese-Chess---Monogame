using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameRule
{
    enum Pieces
    {
        R_Chariot = 8,
        R_Horse = 4,
        R_Elephant = 3,
        R_Advisor = 2,
        R_General = 100,
        R_Cannon = 5,
        R_Soldier = 1,
        B_Soldier = -1,
        B_Cannon = -5,
        B_General = -100,
        B_Advisor = -2,
        B_Elephant = -3,
        B_Horse = -4,
        B_Chariot = -8
    }
}
