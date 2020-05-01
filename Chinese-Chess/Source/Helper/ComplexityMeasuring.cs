using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.Helper
{
    public class ComplexityMeasuring
    {
        public int PositionsEvaluated { get; set; }
        public long MilliSeconds { get; set; }

        public ComplexityMeasuring(int posEvaluated, long millisecs)
        {
            PositionsEvaluated = posEvaluated;
            MilliSeconds = millisecs;
        }

    }
}
