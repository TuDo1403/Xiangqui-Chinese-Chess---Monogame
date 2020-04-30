using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace ChineseChess.Source.Players
{
    public class Player
    {
        public List<Piece> Pieces { get; } = new List<Piece>();

        public PlayerTag Tag { get; protected set; }


        public void AddPiece(Piece piece)
        {
            Pieces.Add(piece);
        }

        public void RemovePiece(ChessBoard board, Point index)
        {
            var piece = Pieces.Where(p => p.Index == index)
                              .SingleOrDefault();
            if (piece != null)
            {
                piece.RemoveBoardUpdatedEvent(board);
            }

            Pieces.RemoveAll(p => p.Index == index);
        }

        public virtual void Update(MouseState mouseState) { }

        public virtual void Update(BoardState board) { }

        public virtual void DrawPieces(SpriteBatch spriteBatch)
        {
            foreach (var piece in Pieces)
            {
                piece.Draw(spriteBatch);
            }
        }
    }
}
