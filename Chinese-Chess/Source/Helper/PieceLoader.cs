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
            { 8, "red-chariot" },
            { 4, "red-horse" },
            { 3, "red-elephant"},
            { 2, "red-advisor" },
            { 100, "red-general" },
            { 5, "red-cannon" },
            { 1, "red-soldier" },
            { -8, "black-chariot" },
            { -4, "black-horse" },
            { -3, "black-elephant"},
            { -2, "black-advisor" },
            { -100, "black-general" },
            { -5, "black-cannon" },
            { -1, "black-soldier" },
        };
    }
}
