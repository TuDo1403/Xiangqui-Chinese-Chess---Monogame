using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MCTS
{
    public class Node
    {
        public int TotalScore { get; set; }

        public int Visits { get; set; }

        public BoardState State { get; set; }

        public List<Node> Children { get; set; }

        public (Point, Point) FromTo { get; set; }

        public Node Parent { get; set; }

        public bool CurrentPlayer { get; set; }



        public Node(Node parent, BoardState state, (Point, Point) fromTo, bool currentPlayer)
        {
            Parent = parent;
            TotalScore = 0;
            Visits = 0;
            State = state;
            CurrentPlayer = currentPlayer;
            FromTo = fromTo;
        }

        public bool IsVisited()
        {
            return Visits > 0;
        }
    }
}
