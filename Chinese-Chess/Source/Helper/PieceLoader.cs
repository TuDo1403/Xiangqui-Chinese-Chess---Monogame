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
            { 600, "red-chariot" },
            { 270, "red-horse" },
            { 130, "red-elephant"},
            { 120, "red-advisor" },
            { 6000, "red-general" },
            { 285, "red-cannon" },
            { 30, "red-soldier" },
            { -600, "black-chariot" },
            { -270, "black-horse" },
            { -130, "black-elephant"},
            { -120, "black-advisor" },
            { -6000, "black-general" },
            { -285, "black-cannon" },
            { -30, "black-soldier" },
        };
    }
}
