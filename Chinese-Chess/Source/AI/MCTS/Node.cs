using ChineseChess.Source.GameObjects.Chess;
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

        public int NumberOfSimulations { get; set; }

        public BoardState State { get; set; }

        public List<Node> Children { get; set; }

        public (Point, Point) FromTo { get; set; }

        public Node Parent { get; set; }

        public bool NextTurn { get; set; }



        public Node(Node parent, BoardState state, (Point, Point) fromTo, bool nextTurn)
        {
            Parent = parent;
            TotalScore = 0;
            NumberOfSimulations = 0;
            State = state;
            NextTurn = nextTurn;
            FromTo = fromTo;
        }

        public bool IsVisited()
        {
            return NumberOfSimulations > 0;
        }


        public void Expand()
        {
            var children = new List<Node>();
            foreach (var pieceIdx in State.GetPieces(NextTurn))
            {
                foreach (var move in State.GetLegalMoves(pieceIdx))
                {
                    var node = new Node(this, State.SimulateMove(pieceIdx, move), 
                                        (pieceIdx, move), !NextTurn);
                    children.Add(node);
                    State.Undo();
                }
            }
            Children = children;
        }

        public void BackPropagate()
        {
            if (Parent != null)
            {
                Parent.NumberOfSimulations += 1;
                Parent.TotalScore += TotalScore;
                Parent.BackPropagate();
            }
        }

        public Node FindRandomChild()
        {
            return Children[new Random().Next(Children.Count)];
        }
    }
}
