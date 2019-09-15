using System;
using System.Collections.Generic;
using System.Linq;

namespace Shos.Reversi.AI
{
    using Shos.Reversi.Core;
    using Shos.Reversi.Core.Helpers;

    public class AIPlayer : ComputerPlayer
    {
        public AIPlayer()
        {
        }

        protected override void Reset()
        {
        }

        protected override TableIndex OnTurn(Board board, Stone.StoneState state, IEnumerable<TableIndex> indexes)
        {
            return indexes.First();
        }
    }
}
