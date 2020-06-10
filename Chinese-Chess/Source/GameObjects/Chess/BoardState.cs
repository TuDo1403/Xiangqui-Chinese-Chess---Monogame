using ChineseChess.Properties;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Helper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ChineseChess.Source.GameObjects.Chess
{
    public class BoardState
    {
        private int[][] _boardState = new int[10][];
        private readonly Stack<int[][]> _undoStates = new Stack<int[][]>();

        private static readonly int[][] _redCannonPosVal = new int[10][];
        private static readonly int[][] _redChariotPosVal = new int[10][];
        private static readonly int[][] _redHorsePosVal = new int[10][];
        private static readonly int[][] _redSoldierPosVal = new int[10][];

        private static readonly int[][] _blackCannonPosVal = new int[10][];
        private static readonly int[][] _blackChariotPosVal = new int[10][];

        internal BoardState Clone()
        {
            var newBoard = Clone(_boardState);
            return new BoardState(newBoard);
        }

        private static readonly int[][] _blackHorsePosVal = new int[10][];
        private static readonly int[][] _blackSoldierPosVal = new int[10][];



        public BoardState()
        {
            LoadBoard(Resources.MatrixBoard, _boardState);
            LoadPosValBoards();
        }

        private BoardState(int[][] boardState) => _boardState = boardState;

        private void LoadPosValBoards()
        {
            LoadBoard(Resources.R_CannonPosVal, _redCannonPosVal);
            LoadBoard(Resources.R_ChariotPosVal, _redChariotPosVal);
            LoadBoard(Resources.R_HorsePosVal, _redHorsePosVal);
            LoadBoard(Resources.R_SoldierPosVal, _redSoldierPosVal);

            LoadBoard(Resources.B_CannonPosVal, _blackCannonPosVal);
            LoadBoard(Resources.B_ChariotPosVal, _blackChariotPosVal);
            LoadBoard(Resources.B_HorsePosVal, _blackHorsePosVal);
            LoadBoard(Resources.B_SoldierPosVal, _blackSoldierPosVal);
        }

        private static void LoadBoard(string boardStateData, int[][] boardState)
        {
            var board = boardStateData.Split('\n');
            var idx = 0;
            foreach (var line in board)
            {
                var row = line.Replace("\r", "").Split(' ').ToArray();
                boardState[idx++] = Array.ConvertAll(row, s => int.Parse(s, CultureInfo.InvariantCulture));
            }
        }

        public int this[int idx1, int idx2]
        {
            get => _boardState[idx1][idx2];
            set => _boardState[idx1][idx2] = value;
        }


        public BoardState SimulateMove(Point oldIdx, Point newIdx)
        {
            _undoStates.Push(Clone(_boardState));
            MakeMove(oldIdx, newIdx);
            return Clone();
        }
        private static int[][] Clone(int[][] board)
        {
            var newBoard = new int[10][];
            for (int i = 0; i < 10; ++i)
            {
                newBoard[i] = new int[9];
                for (int j = 0; j < 9; ++j)
                    newBoard[i][j] = board[i][j];
            }
            return newBoard;
        }

        public void MakeMove(Point oldIdx, Point newIdx)
        {
            var pieceVal = _boardState[oldIdx.Y][oldIdx.X];
            _boardState[oldIdx.Y][oldIdx.X] = 0;
            _boardState[newIdx.Y][newIdx.X] = pieceVal;
        }

        public void Undo() => _boardState = _undoStates.Pop();



        public int PosVal(int row, int col)
        {
            var pieceVal = _boardState[row][col];
            if (pieceVal == (int)Pieces.B_Cannon)
                return _blackCannonPosVal[row][col];

            if (pieceVal == (int)Pieces.B_Chariot)
                return _blackChariotPosVal[row][col];

            if (pieceVal == (int)Pieces.B_Soldier)
                return _blackSoldierPosVal[row][col];

            if (pieceVal == (int)Pieces.B_Horse)

                return _blackHorsePosVal[row][col];

            if (pieceVal == (int)Pieces.R_Cannon)
                return _redCannonPosVal[row][col];

            if (pieceVal == (int)Pieces.R_Chariot)
                return _redChariotPosVal[row][col];

            if (pieceVal == (int)Pieces.R_Soldier)
                return _redSoldierPosVal[row][col];

            if (pieceVal == (int)Pieces.R_Horse)
                return _redHorsePosVal[row][col];

            return 0;
        }


        public List<Point> GetLegalMoves(Point pieceIdx, bool moveOrder = false)
        {
            var value = _boardState[pieceIdx.Y][pieceIdx.X];
            var key = Math.Abs(value);
            var moves = PieceMoveFactory.CreatePieceMove(key, pieceIdx)
                                        .FindLegalMoves(this);

            // Simple move ordering
            if (moveOrder)
            {
                if (value > 0)
                    return moves.OrderBy(p => _boardState[p.Y][p.X])
                                .ToList();
                else
                    return moves.OrderByDescending(p => _boardState[p.Y][p.X])
                                .ToList();
            }
            return moves;
        }


        public List<Point> GetPieces(bool maxPlayer)
        {
            var pieces = new List<Point>();
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 9; ++j)
                    if (maxPlayer == true && IsTeamRed(i, j) ||
                        maxPlayer == false && IsTeamBlack(i, j))
                        pieces.Add(new Point(j, i));

            return pieces;
        }

        private bool IsTeamBlack(int i, int j) => _boardState[i][j] < 0;
        private bool IsTeamRed(int i, int j) => _boardState[i][j] > 0;
    }
}
