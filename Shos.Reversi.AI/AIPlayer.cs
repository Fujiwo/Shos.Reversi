using System.Collections.Generic;

namespace Shos.Reversi.AI
{
    using Shos.Reversi.Core;
    using Shos.Reversi.Core.Helpers;

    public class AIPlayer : ComputerPlayer
    {
        class ScoreTable
        {
            readonly int[,] scores = new int[Board.RowNumber, Board.ColumnNumber] {
                { 100, -50,  10,   0,   0,  10, -50, 100 },
                { -50, -70, - 5, -10, -10, - 5, -70, -50 },
                {  10, - 5, -10, - 5, - 5, -10, - 5,  10 },
                {   0, -10, - 5,   0,   0, - 5, -10,   0 },
                {   0, -10, - 5,   0,   0, - 5, -10,   0 },
                {  10, - 5, -10, - 5, - 5, -10, - 5,  10 },
                { -50, -70, - 5, -10, -10, - 5, -70, -50 },
                { 100, -50,  10,   0,   0,  10, -50, 100 }
            };

            public int this[TableIndex index] => scores.Get(index);
        }

        static readonly ScoreTable scoreTable = new ScoreTable();

        public AIPlayer()
        {
        }

        protected override void Reset()
        {
        }

        protected override TableIndex OnTurn(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes)
        {
            var (index, _) = GetMaximumScore(board, myState, indexes);
            return index;
        }

        void DoTurn(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes = null)
        {
            var (index, _) = GetMaximumScore(board, myState, indexes);
            board.TurnOverWith(index, myState);
        }

        (TableIndex, int) GetMaximumScore(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes = null)
        {
            indexes       = indexes ?? board.CanTurnOverIndexes(myState);
            var bestScore = (index: new TableIndex(), score: int.MinValue);
            indexes.ForEach(index => {
                var temporaryBoard = board.Clone();
                temporaryBoard.TurnOverWith(index, myState);
                var score = GetScore(temporaryBoard, myState);
                if (score > bestScore.score)
                    bestScore = (index, score);
            });
            return bestScore;
        }

        static int GetScore(Board board, Stone.StoneState myState) => GetScoreWithScoreTable(board, myState);

        static int GetScoreWithScoreTable(Board board, Stone.StoneState myState)
        {
            var enemyState = Stone.GetReverseState(myState);
            var myScore    = 0;
            var enemyScore = 0;
            foreach (var index in board.AllIndexes) {
                var cellState = board[index].State;
                if      (cellState == myState   )
                    myScore += scoreTable[index];
                else if (cellState == enemyState)
                    enemyScore += scoreTable[index];
            }
            return myScore - enemyScore;
        }
    }
}
