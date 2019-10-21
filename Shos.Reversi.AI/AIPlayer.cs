using MathNet.Numerics.Random;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Shos.Reversi.AI
{
    using Shos.Reversi.Core;
    using Shos.Reversi.Core.Helpers;

    public class AIPlayer : ComputerPlayer
    {
        static MersenneTwister random = new MersenneTwister();

        public Action<string>? Log { get; set; } = null;

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
        {}

        protected override TableIndex OnTurn(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes)
            => GetMaximumScoreIndex(board, myState, indexes);

        //void DoTurn(Board board, Stone.StoneState myState, IEnumerable<TableIndex>? indexes = null)
        //{
        //    var (index, _) = GetMaximumScore(board, myState, indexes);
        //    board.TurnOverWith(index, myState);
        //}

        TableIndex GetMaximumScoreIndex(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes)
        {
            const int  alpha     = int.MinValue;
            const int  beta      = int.MaxValue;
            const uint limit     = 3U;

            var bestScore        = int.MinValue;
            var bestScoreIndexes = new List<TableIndex>();

            indexes.ForEach(index => {
                var temporaryBoard = board.Clone();
                temporaryBoard.TurnOverWith(index, myState);

                var enemyState = Stone.GetReverseState(myState);
                var enemyIndexes = temporaryBoard.CanTurnOverIndexes(enemyState).ToList();

                var score = enemyIndexes.Count == 0
                            ?  AlphaBeta(board, myState   , temporaryBoard.CanTurnOverIndexes(myState).ToList(), alpha, beta, limit - 1)
                            : -AlphaBeta(board, enemyState, enemyIndexes                                       , alpha, beta, limit - 1);

                if (score == bestScore) {
                    bestScoreIndexes.Add(index);
                } else if (score > bestScore) {
                    bestScore = score;
                    bestScoreIndexes.Clear();
                    bestScoreIndexes.Add(index);
                }
            });
            var index = random.Next(bestScoreIndexes.Count);
            Debug.Assert(0 <= index && index < bestScoreIndexes.Count);
            return bestScoreIndexes[index];
        }
         
        int AlphaBeta(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes, int alpha, int beta, uint limit)
        {
            if (limit == 0)
                return GetScore(board, myState);

            foreach (var index in indexes) {
                var temporaryBoard = board.Clone();
                temporaryBoard.TurnOverWith(index, myState);
                var enemyState   = Stone.GetReverseState(myState);
                var enemyIndexes = temporaryBoard.CanTurnOverIndexes(enemyState).ToList();
                if (enemyIndexes.Count == 0)
                    return AlphaBeta(board, myState, temporaryBoard.CanTurnOverIndexes(myState).ToList(), alpha, beta, limit - 1);
                var score = -AlphaBeta(board, enemyState, enemyIndexes, -beta, -alpha, limit - 1);
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    return alpha;
            }
            return alpha;
        }

        //(TableIndex, int) GetMaximumScore(Board board, Stone.StoneState myState, IEnumerable<TableIndex>? indexes = null)
        //{
        //    indexes = indexes ?? board.CanTurnOverIndexes(myState);
        //    //var bestScore = (index: new TableIndex(), score: int.MinValue);
        //    var bestScore        = int.MinValue;
        //    var bestScoreIndexes = new List<TableIndex>();

        //    indexes.ForEach(index => {
        //        var temporaryBoard = board.Clone();
        //        temporaryBoard.TurnOverWith(index, myState);
        //        var enemyState = Stone.GetReverseState(myState);

        //        (TableIndex index, int score) enemyScore;
        //        if (GetMaximumEnemyScore(temporaryBoard, enemyState, out enemyScore))
        //            temporaryBoard.TurnOverWith(enemyScore.index, enemyState);
        //        var score = GetScore(temporaryBoard, myState);
        //        if (score == bestScore) {
        //            bestScoreIndexes.Add(index);
        //            Log?.Invoke($"bestScore: {bestScore}, ({bestScoreIndexes.Count})");
        //        } else if (score > bestScore) {
        //            bestScore = score;
        //            bestScoreIndexes.Clear();
        //            bestScoreIndexes.Add(index);
        //        }
        //    });
        //    var index = random.Next(bestScoreIndexes.Count);
        //    Debug.Assert(0 <= index && index < bestScoreIndexes.Count);
        //    return (bestScoreIndexes[index], bestScore);
        //}

        //bool GetMaximumEnemyScore(Board board, Stone.StoneState myState, out (TableIndex index, int score) enemyScore, IEnumerable<TableIndex>? indexes = null)
        //{
        //    indexes              = indexes ?? board.CanTurnOverIndexes(myState);
        //    //var bestScore = (index: new TableIndex(), score: int.MinValue);
        //    var bestScore        = int.MinValue;
        //    var bestScoreIndexes = new List<TableIndex>();

        //    indexes.ForEach(index => {
        //        var temporaryBoard = board.Clone();
        //        temporaryBoard.TurnOverWith(index, myState);
        //        var score = GetScore(temporaryBoard, myState);
        //        if (score == bestScore) {
        //            bestScoreIndexes.Add(index);
        //        } else if (score > bestScore) {
        //            bestScore = score;
        //            bestScoreIndexes.Clear();
        //            bestScoreIndexes.Add(index);
        //        }
        //    });
        //    if (bestScoreIndexes.Count == 0) {
        //        enemyScore = (new TableIndex(), bestScore);
        //        return false;
        //    }
        //    var index = random.Next(bestScoreIndexes.Count);
        //    Debug.Assert(0 <= index && index < bestScoreIndexes.Count);
        //    enemyScore = (bestScoreIndexes[index], bestScore);
        //    return true;
        //}

        static int[] scoreRateTable = new[] { 10, 100 };

        static int GetScore(Board board, Stone.StoneState myState)
            => GetScoreWithScoreTable (board, myState) * scoreRateTable[0] +
               GetScoreWithStoneNumber(board, myState) * scoreRateTable[1];

        static int GetScoreWithStoneNumber(Board board, Stone.StoneState myState)
            => board.GetStoneNumber(myState);

#if DEBUG
        public static int GettingScoreCount = 0;
#endif // DEBUG

        static int GetScoreWithScoreTable(Board board, Stone.StoneState myState)
        {
#if DEBUG
            GettingScoreCount++;
#endif // DEBUG

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
