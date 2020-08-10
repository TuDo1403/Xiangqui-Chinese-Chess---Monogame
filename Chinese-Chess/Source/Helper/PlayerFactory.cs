using ChineseChess.Source.AI.MCTS;
using ChineseChess.Source.AI.Minimax;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.Helper
{
    public static class PlayerFactory
    {
        public static Player CreatePlayer(string name, Team team, int depth)
        {
            if (name == "human") return new Human();
            if (name == "minimax") return new Computer(new MoveOrdering(team), depth);
            if (name == "s-uct") return new Computer(new SimpleUCTSearch(team), depth);
            if (name == "s-uct-m") return new Computer(new SimpleUCTSearch(team, "max"), depth);
            if (name == "s-uct-mr") return new Computer(new SimpleUCTSearch(team, "max-robust"), depth);
            if (name == "s-uct-r") return new Computer(new SimpleUCTSearch(team, "robust"), depth);
            if (name == "m-uct") return new Computer(new ModifiedUCTSearch(team), depth);
            if (name == "m-uct2") return new Computer(new MinimaxUCT(team), depth);
            throw new ArgumentException($"Create player error: key values {name}");
        }
    }
}
