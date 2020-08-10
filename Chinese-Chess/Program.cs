using ChineseChess.Source.Main;
using System;

namespace ChineseChess
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //for (int i = 0; i < 1; ++i)
            //{
            //    using (var game = new ChessGame("human", "minimax", 3, ""))
            //    {
            //        game.Run();
            //    }
            //}

            //for (int i = 0; i < 1; ++i)
            //{
            //    using (var game = new ChessGame("minimax", "s-uct", 3, ""))
            //    {
            //        game.Run();
            //    }
            //}

            for (int i = 0; i < 1; ++i)
            {
                using (var game = new ChessGame("minimax", "m-uct", 2, ""))
                {
                    game.Run();
                }
            }

            //for (int i = 0; i < 1; ++i)
            //{
            //    using (var game = new ChessGame("minimax", "m-uct2", 3, ""))
            //    {
            //        game.Run();
            //    }
            //}
        }
    }
#endif
}
