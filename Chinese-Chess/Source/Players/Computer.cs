using ChineseChess.Source.AI;
using ChineseChess.Source.AI.Minimax;
using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Helper;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseChess.Source.Players
{
    public class Computer : Player
    {
        private bool _isCalculating = false;

        private (Point, Point) _move = (Point.Zero, Point.Zero);

        private ComplexityMeasuring _moveInfo;


        public IMoveStrategy AIAgent { get; set; }
        public int Depth { get; set; }
        public Computer(IMoveStrategy agent, int depth)
        {
            Tag = PlayerTag.COM;
            AIAgent = agent;
            Depth = depth;
        }

        public override void Update(BoardState board)
        {
            if (_move == (Point.Zero, Point.Zero))
            {
                if (!_isCalculating)
                {
                    _isCalculating = true;
                    var newBoard = board.Clone();
                    ((Action<BoardState>)AsyncCalculation).BeginInvoke(newBoard, null, this);
                }
            }
            else
            {
                MakeMove(_move);
                _move = (Point.Zero, Point.Zero);
            }
        }

        private void AsyncCalculation(BoardState board)
        {
            _move = CalculateMoveAsync(board).Result;
            _isCalculating = false;
        }

        private async Task<(Point, Point)> CalculateMoveAsync(BoardState board)
        {
            var move = (Point.Zero, Point.Zero);
            await Task.Run(() =>
            {
                move = AIAgent.Search(board, Depth);
            }).ConfigureAwait(false);
            return move;
        }

        private void MakeMove((Point, Point) move)
        {
            var focusingPiece = Pieces.Where(p => p.Index == move.Item1)
                                      .SingleOrDefault();
            focusingPiece.OnFocusing();
            focusingPiece.SetMove(move.Item2);
        }

        private void  WriteReport(string fileName)
        {
            using (StreamWriter writer = File.AppendText(fileName))
            {
                writer.WriteLine($"{_moveInfo.PositionsEvaluated},{_moveInfo.MilliSeconds}");
            }
        }
    }
}
