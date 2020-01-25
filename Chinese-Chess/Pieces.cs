using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public static class Pieces
    {
        public static Dictionary<float, string> PieceDictionary { get; set; } = new Dictionary<float, string>()
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
