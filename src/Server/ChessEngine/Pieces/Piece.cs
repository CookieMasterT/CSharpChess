using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public abstract class Piece
    {
        public Piece(Team team)
        {
            Team = team;
        }

        public virtual string[] Name { get => ChessNotation.PiecePlaceHolder; }
        public string NotationName { get => this.Name[0]; }
        public string ImageFileName
        {
            get
            {
                string teamName = "";
                switch (Team)
                {
                    case Team.white: teamName = ChessNotation.WhiteTeam; break;
                    case Team.black: teamName = ChessNotation.BlackTeam; break;
                }
                return $"{this.Name[1]}-{teamName}.svg";
            }
        }

        public readonly Team Team;

        abstract public List<BoardSquare> LegalMoves { get; } // list of moves the piece can do
    }
}
