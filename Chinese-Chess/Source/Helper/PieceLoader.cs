using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.Helper
{
    public static class PieceLoader
    {
        public static Dictionary<float, string> TextureLoader { get; private set; } = new Dictionary<float, string>()
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
