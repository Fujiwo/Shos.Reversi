#nullable enable

using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Media;

namespace Shos.Reversi.Wpf.ViewModels
{
    using Shos.Reversi.Core;

    class StoneViewModel : BindableBase
    {
        public event Action<StoneViewModel>? Click;

        readonly Game game;

        public Stone DataContext { get; private set; }

        public Brush FillColor => DataContext.State switch {
                                                        Stone.StoneState.None  => Brushes.Green,
                                                        Stone.StoneState.Black => Brushes.Black,
                                                        Stone.StoneState.White => Brushes.White,
                                                        _                      => throw new InvalidOperationException()
                                                    };

        public Brush StrokeColor => DataContext.CanTurnOver ? Brushes.Gold : Brushes.Black;

        public DelegateCommand Clicked { get; private set; }

        public StoneViewModel(Game game, Stone stone)
        {
            this.game   = game;
            game.PropertyChanged += (_, arg) => {
                switch (arg.PropertyName) {
                    case nameof(game.CurrentStone ): RaisePropertyChanged(nameof(FillColor )); break;
                }
            };
            DataContext = stone;
            DataContext.PropertyChanged += (_, arg) => {
                switch (arg.PropertyName) {
                    case nameof(DataContext.State      ): RaisePropertyChanged(nameof(FillColor  )); break;
                    case nameof(DataContext.CanTurnOver): RaisePropertyChanged(nameof(StrokeColor)); break;
                }
            };
            Clicked = new DelegateCommand(() => Click?.Invoke(this));
        }
    }
}
