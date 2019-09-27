#nullable enable

using System;

namespace Shos.Reversi.Wpf.ViewModels
{
    using Shos.Reversi.Core;
    using Shos.Reversi.Core.Helpers;

    class BoardViewModel
    {
        public event Action<TableIndex, StoneViewModel>? StoneClick;

        StoneViewModel[, ] stoneViewModels = new StoneViewModel[Board.RowNumber, Board.ColumnNumber];

        public StoneViewModel this[TableIndex index]
            => stoneViewModels.Get(index);

        public BoardViewModel(Game game)
            => stoneViewModels.ForEachWithIndex(
                  (index, _) => {
                      var stoneViewModel = new StoneViewModel(game, game.Board[index]);
                      stoneViewModel.Click += sender => OnStoneClick(index, sender);
                      stoneViewModels.Set(index, stoneViewModel);
                  });

        public void ForEach(Action<TableIndex, StoneViewModel> action)
            => stoneViewModels.ForEachWithIndex(action);

        void OnStoneClick(TableIndex index, StoneViewModel stoneViewModel) => StoneClick?.Invoke(index, stoneViewModel);
    }
}
