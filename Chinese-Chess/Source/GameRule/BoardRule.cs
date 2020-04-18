using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameRule
{
    enum BoardRule
    {
        COL = 9, // X-axis
        ROW, // Y-axis
        L_CASTLE = 3,
        R_CASTLE = 5,
        FB_CASTLE = 2,
        FR_CASTLE = 7,
        B_BORD = 4,
        R_BORD = 5
    }
}
