using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public static class Rules
    {
        public const int CHARIOT = 1;
        public const int HORSE = 2;
        public const int ELEPHANT = 3;
        public const int ADVISOR = 4;
        public const int GENERAL = 5;
        public const int CANNON = 6;
        public const int SOLDIER = 7;

        public const int ROWS = 10;
        public const int COLUMNS = 9;

        public const bool REDSIDE = true;
        public const bool BLACKSIDE = false;


        public static Dictionary<float, string> PieceDictionary { get; private set; } = new Dictionary<float, string>()
        {
            { 1, "red-chariot" },
            { 2, "red-horse" },
            { 3, "red-elephant"},
            { 4, "red-advisor" },
            { 5, "red-general" },
            { 6, "red-cannon" },
            { 7, "red-soldier" },
            { -1, "black-chariot" },
            { -2, "black-horse" },
            { -3, "black-elephant"},
            { -4, "black-advisor" },
            { -5, "black-general" },
            { -6, "black-cannon" },
            { -7, "black-soldier" },
        };
    }
}
