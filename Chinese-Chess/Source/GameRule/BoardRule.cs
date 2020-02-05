using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameRule
{
    enum BoardRule
    {
        Columns = 9, // X-axis
        Rows, // Y-axis
        LeftCastle = 3,
        RightCastle = 5,
        FrontBlackCastle = 2,
        FrontRedCastle = 7,
        BlackBorder = 4,
        RedBorder = 5
    }
}
