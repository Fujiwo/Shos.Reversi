using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shos.Reversi.Core
{
    using Helpers;

    public class Board : BindableBase, ICloneable<Board>
    {
        public const int RowNumber    = 8;
        public const int ColumnNumber = RowNumber;

        Stone[,] stones = new Stone[RowNumber, ColumnNumber];

        public int StoneNumber              => RowNumber * ColumnNumber;
        public int BlackStoneNumber         => GetStoneNumber(Stone.StoneState.Black);
        public int WhiteStoneNumber         => GetStoneNumber(Stone.StoneState.White);
        public int NoneStoneNumber          => GetStoneNumber(Stone.StoneState.None );
        public int BlackAndWhiteStoneNumber => StoneNumber - NoneStoneNumber;

        public Stone.StoneState Winner {
            get {
                var blackStoneNumber = BlackStoneNumber;
                var whiteStoneNumber = WhiteStoneNumber;
                return blackStoneNumber == whiteStoneNumber
                       ? Stone.StoneState.None
                       : (blackStoneNumber > whiteStoneNumber ? Stone.StoneState.Black : Stone.StoneState.White);
            }
        }

        public int GetStoneNumber(Stone.StoneState state) => stones.Count(stone => stone.State == state);

        static Board()
        {
            TableIndex.MaximumRowNumber    = RowNumber   ;
            TableIndex.MaximumColumnNumber = ColumnNumber;
        }

        public Board() => InitializeStones();

        public Board Clone()
        {
            var board = new Board();
            AllIndexes.ForEach(index => board[index].State = this[index].State);
            return board;
        }

        public Stone this[TableIndex index] {
            get => stones.Get(index       );
            set => stones.Set(index, value);
        }

        public IEnumerable<Stone              > AllStones          => stones.ToSequence         ();
        public IEnumerable<(TableIndex, Stone)> AllStonesWithIndex => stones.ToSequenceWithIndex();
        public IEnumerable<TableIndex         > AllIndexes         => stones.AllIndexes         ();

        public void ForEachWithIndex(Action<TableIndex, Stone> action)
            => stones.ForEachWithIndex(action);

        public void ForEach(Action<Stone> action)
            => stones.ForEach(action);

        public void TurnOverWith(TableIndex index, Stone.StoneState state)
        {
            ReversibleStoneIndexes(index, state).ToList().ForEach(turnOverIndex => this[turnOverIndex].State = state);
            this[index].State = state;
            RaisePropertyChanged(nameof(StoneNumber));
        }

        public void SetCanTurnOver(List<TableIndex> canTurnOverIndexes)
        {
                               ForEach(stone => stone      .CanTurnOver = false);
            canTurnOverIndexes.ForEach(index => this[index].CanTurnOver = true );
        }

        public IEnumerable<TableIndex> ReversibleStoneIndexes(TableIndex index, Stone.StoneState state)
            => TableIndex.AllDirections.SelectMany(direction => ReversibleStoneIndexes(index, direction, state));

        public IEnumerable<TableIndex> CanTurnOverIndexes(Stone.StoneState state)
            => NoneStoneIndexes.Where(index => CanTurnOver(index, state));

        public bool CanTurnOver(TableIndex index, Stone.StoneState state)
            => TableIndex.AllDirections.Any(direction => ReversibleStoneIndexes(index, direction, state).Count() > 0);

        void InitializeStones()
        {
            AllIndexes.ForEach(index => this[index] = new Stone());

            this[new TableIndex { Row = 3, Column = 3 }].State =
            this[new TableIndex { Row = 4, Column = 4 }].State = Stone.StoneState.White;
            this[new TableIndex { Row = 3, Column = 4 }].State =
            this[new TableIndex { Row = 4, Column = 3 }].State = Stone.StoneState.Black;
        }

        IEnumerable<TableIndex> ReversibleStoneIndexes(TableIndex index, TableIndex.Direction direction, Stone.StoneState state)
        {
            var indexes = new LinkedList<TableIndex>();
            for (var reverseState = Stone.GetReverseState(state); index.Forward(direction); ) {
                if (this[index].State == reverseState)
                    indexes.AddLast(index);
                else if (this[index].State == state)
                    return indexes;
                else
                    break;
            }
            return new TableIndex[0];
        }

        IEnumerable<TableIndex> NoneStoneIndexes => AllIndexes.Where(index => this[index].State == Stone.StoneState.None);
    }
}
