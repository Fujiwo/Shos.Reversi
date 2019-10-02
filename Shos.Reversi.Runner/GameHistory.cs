using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Shos.Reversi.Runner
{
    using Shos.CsvHelper;
    using Shos.Reversi.Core;
    using Shos.Reversi.Core.Helpers;

    class GameHistory : IDisposable
    {
        public class Aspect
        {
            public float Cell00 { get; set; }
            public float Cell01 { get; set; }
            public float Cell02 { get; set; }
            public float Cell03 { get; set; }
            public float Cell04 { get; set; }
            public float Cell05 { get; set; }
            public float Cell06 { get; set; }
            public float Cell07 { get; set; }
            public float Cell10 { get; set; }
            public float Cell11 { get; set; }
            public float Cell12 { get; set; }
            public float Cell13 { get; set; }
            public float Cell14 { get; set; }
            public float Cell15 { get; set; }
            public float Cell16 { get; set; }
            public float Cell17 { get; set; }
            public float Cell20 { get; set; }
            public float Cell21 { get; set; }
            public float Cell22 { get; set; }
            public float Cell23 { get; set; }
            public float Cell24 { get; set; }
            public float Cell25 { get; set; }
            public float Cell26 { get; set; }
            public float Cell27 { get; set; }
            public float Cell30 { get; set; }
            public float Cell31 { get; set; }
            public float Cell32 { get; set; }
            public float Cell33 { get; set; }
            public float Cell34 { get; set; }
            public float Cell35 { get; set; }
            public float Cell36 { get; set; }
            public float Cell37 { get; set; }
            public float Cell40 { get; set; }
            public float Cell41 { get; set; }
            public float Cell42 { get; set; }
            public float Cell43 { get; set; }
            public float Cell44 { get; set; }
            public float Cell45 { get; set; }
            public float Cell46 { get; set; }
            public float Cell47 { get; set; }
            public float Cell50 { get; set; }
            public float Cell51 { get; set; }
            public float Cell52 { get; set; }
            public float Cell53 { get; set; }
            public float Cell54 { get; set; }
            public float Cell55 { get; set; }
            public float Cell56 { get; set; }
            public float Cell57 { get; set; }
            public float Cell60 { get; set; }
            public float Cell61 { get; set; }
            public float Cell62 { get; set; }
            public float Cell63 { get; set; }
            public float Cell64 { get; set; }
            public float Cell65 { get; set; }
            public float Cell66 { get; set; }
            public float Cell67 { get; set; }
            public float Cell70 { get; set; }
            public float Cell71 { get; set; }
            public float Cell72 { get; set; }
            public float Cell73 { get; set; }
            public float Cell74 { get; set; }
            public float Cell75 { get; set; }
            public float Cell76 { get; set; }
            public float Cell77 { get; set; }

            public float Victory { get; set; }
        }

        class Move : Aspect
        {
            public float IsBlack { get; set; }
        }

        class AllMoves : IEnumerable<Move>
        {
            List<Move> moves = new List<Move>();

            public void Add(Move move) => moves.Add(move);

            public void SetWinner(Stone.StoneState winner)
                => moves.ForEach(move => move.Victory = ToVictory(move.IsBlack, winner));

            public IEnumerator<Move> GetEnumerator() => moves.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            static float ToVictory(float isBlack, Stone.StoneState winner)
                => winner switch {
                       Stone.StoneState.Black => isBlack > 0.5f ? 1.0f : 0.0f,
                       Stone.StoneState.White => isBlack < 0.5f ? 1.0f : 0.0f,
                       _                      => 0.5f
                   };
        }

        int      allMoveCount = 0;
        AllMoves allMoves     = new AllMoves();
        Stream   stream       = new FileStream(FileName, FileMode.Create);

        public void Start() => allMoves = new AllMoves();

        public void End(Stone.StoneState winner)
        {
            Debug.Assert(allMoves != null);

            allMoves.SetWinner(winner);
            Save(stream, ++allMoveCount, allMoves).Wait();
        }

        public void Add(Board board, Stone.StoneState myState)
        {
            Debug.Assert(allMoves != null);
            allMoves.Add(ToMove(myState, board));
        }

        public void Dispose() => stream.Dispose();

        async Task Save(Stream stream, int count, AllMoves allMoves)
            => await allMoves.Select(move => (Aspect)move).WriteCsvAsync (stream: stream, bufferSize:1024, leaveOpen:true, hasHeader: count == 1);

        Move ToMove(Stone.StoneState myState, Board board)
        {
            var move = new Move {
                IsBlack = ToIsBlack(myState),

                Cell00 = ToCell(myState, board, 0, 0),
                Cell01 = ToCell(myState, board, 0, 1),
                Cell02 = ToCell(myState, board, 0, 2),
                Cell03 = ToCell(myState, board, 0, 3),
                Cell04 = ToCell(myState, board, 0, 4),
                Cell05 = ToCell(myState, board, 0, 5),
                Cell06 = ToCell(myState, board, 0, 6),
                Cell07 = ToCell(myState, board, 0, 7),
                Cell10 = ToCell(myState, board, 1, 0),
                Cell11 = ToCell(myState, board, 1, 1),
                Cell12 = ToCell(myState, board, 1, 2),
                Cell13 = ToCell(myState, board, 1, 3),
                Cell14 = ToCell(myState, board, 1, 4),
                Cell15 = ToCell(myState, board, 1, 5),
                Cell16 = ToCell(myState, board, 1, 6),
                Cell17 = ToCell(myState, board, 1, 7),
                Cell20 = ToCell(myState, board, 2, 0),
                Cell21 = ToCell(myState, board, 2, 1),
                Cell22 = ToCell(myState, board, 2, 2),
                Cell23 = ToCell(myState, board, 2, 3),
                Cell24 = ToCell(myState, board, 2, 4),
                Cell25 = ToCell(myState, board, 2, 5),
                Cell26 = ToCell(myState, board, 2, 6),
                Cell27 = ToCell(myState, board, 2, 7),
                Cell30 = ToCell(myState, board, 3, 0),
                Cell31 = ToCell(myState, board, 3, 1),
                Cell32 = ToCell(myState, board, 3, 2),
                Cell33 = ToCell(myState, board, 3, 3),
                Cell34 = ToCell(myState, board, 3, 4),
                Cell35 = ToCell(myState, board, 3, 5),
                Cell36 = ToCell(myState, board, 3, 6),
                Cell37 = ToCell(myState, board, 3, 7),
                Cell40 = ToCell(myState, board, 4, 0),
                Cell41 = ToCell(myState, board, 4, 1),
                Cell42 = ToCell(myState, board, 4, 2),
                Cell43 = ToCell(myState, board, 4, 3),
                Cell44 = ToCell(myState, board, 4, 4),
                Cell45 = ToCell(myState, board, 4, 5),
                Cell46 = ToCell(myState, board, 4, 6),
                Cell47 = ToCell(myState, board, 4, 7),
                Cell50 = ToCell(myState, board, 5, 0),
                Cell51 = ToCell(myState, board, 5, 1),
                Cell52 = ToCell(myState, board, 5, 2),
                Cell53 = ToCell(myState, board, 5, 3),
                Cell54 = ToCell(myState, board, 5, 4),
                Cell55 = ToCell(myState, board, 5, 5),
                Cell56 = ToCell(myState, board, 5, 6),
                Cell57 = ToCell(myState, board, 5, 7),
                Cell60 = ToCell(myState, board, 6, 0),
                Cell61 = ToCell(myState, board, 6, 1),
                Cell62 = ToCell(myState, board, 6, 2),
                Cell63 = ToCell(myState, board, 6, 3),
                Cell64 = ToCell(myState, board, 6, 4),
                Cell65 = ToCell(myState, board, 6, 5),
                Cell66 = ToCell(myState, board, 6, 6),
                Cell67 = ToCell(myState, board, 6, 7),
                Cell70 = ToCell(myState, board, 7, 0),
                Cell71 = ToCell(myState, board, 7, 1),
                Cell72 = ToCell(myState, board, 7, 2),
                Cell73 = ToCell(myState, board, 7, 3),
                Cell74 = ToCell(myState, board, 7, 4),
                Cell75 = ToCell(myState, board, 7, 5),
                Cell76 = ToCell(myState, board, 7, 6),
                Cell77 = ToCell(myState, board, 7, 7),
            };
            return move;
        }

        static float ToIsBlack(Stone.StoneState state)
            => state switch {
                   Stone.StoneState.Black => 1.0f,
                   Stone.StoneState.White => 0.0f,
                    _                     => 0.5f
               };

        static float ToCell(Stone.StoneState myState, Board board, int row, int column)
            => ToFloat(myState, board[new TableIndex { Row = row, Column = column }].State);

        static float ToFloat(Stone.StoneState myState, Stone.StoneState state)
            => state switch {
                Stone.StoneState.None   => 0.5f,
                var x when x == myState => 1.0f,
                _                       => 0.0f
            };

        static string GetFileName(int count) => $"Shos.Reversi.{count:D5}.csv";

        static string? FileName {
            get {
                for (var count = 1; count < int.MaxValue; count++) {
                    var fileName = GetFileName(count);
                    if (File.Exists(fileName))
                        continue;
                    else
                        return fileName;
                }
                return null;
            }
        }
    }
}
