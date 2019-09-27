using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shos.Reversi.Core
{
    using Shos.Reversi.Core.Helpers;

    public abstract class Player : IDisposable
    {
        public int Index { get; set; } = 0;
        public string Name { get; set; } = "";

        public void Dispose() => Reset();

        public abstract Task<TableIndex> Turn(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes);

        public virtual void OnSelectStone(Board board, TableIndex index)
        {}
        protected virtual void Reset()
        {}
    }

    public abstract class ComputerPlayer : Player
    {
        public int Delay { get; set; } = 1000;

        public async override Task<TableIndex> Turn(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes)
        {
            if (Delay > 0)
                await Task.Delay(Delay);
            return OnTurn(board, myState, indexes);
        }

        protected abstract TableIndex OnTurn(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes);
    }

    public class RandomComputerPlayer : ComputerPlayer
    {
        static MersenneTwister random = new MersenneTwister();

        protected override TableIndex OnTurn(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes)
        {
            var indexList = indexes.ToList();
            return indexList[random.Next(indexList.Count())];
        }
    }

    public class HumanPlayer : Player
    {
        IList<TableIndex>? indexes   = null;
        TableIndex         index;
        SemaphoreSlim?     semaphore = null;

        public HumanPlayer() => Name = "Human";

        public async override Task<TableIndex> Turn(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes)
        {
            Interlocked.Exchange(ref this.indexes, indexes.ToList());
            await Wait();
            return index;
        }

        public override void OnSelectStone(Board board, TableIndex index)
        {
            if (indexes != null && indexes.Contains(index)) {
                Set(index);
                Interlocked.Exchange(ref indexes, null);
                semaphore?.Release(releaseCount: 2);
            }
        }

        protected override void Reset()
        {
            if (semaphore != null) {
                semaphore.Release(releaseCount: 2);
                semaphore.Dispose();
                semaphore = null;
            }
        }

        async Task Wait()
        {
            semaphore = new SemaphoreSlim(initialCount: 0, maxCount: 2);
            await semaphore.WaitAsync();
            semaphore.Dispose();
            semaphore = null;
        }

        void Set(TableIndex index)
        {
            Interlocked.Exchange(ref this.index.Row   , index.Row   );
            Interlocked.Exchange(ref this.index.Column, index.Column);
        }
    }
}
