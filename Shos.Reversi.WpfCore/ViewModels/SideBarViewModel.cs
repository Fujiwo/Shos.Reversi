using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Media;

namespace Shos.Reversi.Wpf.ViewModels
{
    using Shos.Reversi.Core;

    public enum GameSpeed
    { VeryFast, Fast, Medium, Slow, VerySlow }

    class SideBarViewModel : BindableBase
    {
        GameSpeed gameSpeed = GameSpeed.Slow;

        GameSpeed ToGameSpeed(int delay)
            =>  delay switch  {
                var x when x <=    0 => GameSpeed.VeryFast,
                var x when x <=    1 => GameSpeed.Fast    ,
                var x when x <=  100 => GameSpeed.Medium  ,
                var x when x <= 1000 => GameSpeed.Slow    ,
                _                    => GameSpeed.VerySlow
            };

        int ToDelay(GameSpeed gameSpeed)
            => gameSpeed switch  {
                GameSpeed.VeryFast =>    0,
                GameSpeed.Fast     =>    1,
                GameSpeed.Medium   =>  100,
                GameSpeed.Slow     => 1000,
                GameSpeed.VerySlow => 2000,
                _                  => throw new InvalidOperationException()
            };

        public Game Game { get; private set; }

        public StoneViewModel StoneViewModel      { get; private set; }
        public StoneViewModel NoneStoneViewModel  { get; private set; }
        public StoneViewModel BlackStoneViewModel { get; private set; }
        public StoneViewModel WhiteStoneViewModel { get; private set; }

        public Game.GameMode GameMode {
            get => Game.Mode        ;
            set => Game.Mode = value;
        }

        public GameSpeed GameSpeed {
            get => gameSpeed;
            set {
                SetProperty(ref gameSpeed, value);
                Game.Delay = ToDelay(value);
            }
        }

        public string PlayerName => Game?.CurrentPlayer?.Name ?? "";
        public string BlackStonePlayerName => Game.GetPlayer(Stone.StoneState.Black).Name;
        public string WhiteStonePlayerName => Game.GetPlayer(Stone.StoneState.White).Name;

        public int BlackStoneNumber => Game.Board.BlackStoneNumber;
        public int WhiteStoneNumber => Game.Board.WhiteStoneNumber;
        public int NoneStoneNumber  => Game.Board.NoneStoneNumber ;

        public bool CanChangeMode => !Game.IsPlaying;
        public string StartButtonText => Game.IsPlaying ? "Playing" : "Start";

        public DelegateCommand StartClicked { get; private set; }

        public SideBarViewModel(Game game)
        {
            Game = game;
            NoneStoneViewModel  = new StoneViewModel(Game, new Stone { State = Stone.StoneState.None  });
            BlackStoneViewModel = new StoneViewModel(Game, new Stone { State = Stone.StoneState.Black });
            WhiteStoneViewModel = new StoneViewModel(Game, new Stone { State = Stone.StoneState.White });

            game.PropertyChanged += (_, arg) => {
                switch (arg.PropertyName) {
                    case nameof(game.Mode         ):
                        RaisePropertyChanged(nameof(GameMode            ));
                        break;
                    case nameof(game.Players      ):
                        RaisePropertyChanged(nameof(PlayerName          ));
                        RaisePropertyChanged(nameof(BlackStonePlayerName));
                        RaisePropertyChanged(nameof(WhiteStonePlayerName));
                        break;
                    case nameof(game.IsPlaying    ):
                        RaisePropertyChanged(nameof(CanChangeMode       ));
                        RaisePropertyChanged(nameof(StartButtonText     ));
                        break;
                    case nameof(game.CurrentStone ):
                        RaisePropertyChanged(nameof(Color               ));
                        break;
                    case nameof(game.CurrentPlayer):
                        RaisePropertyChanged(nameof(PlayerName          ));
                        break;
                    case nameof(game.Delay        ):
                        GameSpeed = ToGameSpeed(game.Delay);
                        break;
                }
            };
            game.Board.PropertyChanged += (_, arg) => {
                switch (arg.PropertyName) {
                    case nameof(game.Board.StoneNumber):
                        RaisePropertyChanged(nameof(BlackStoneNumber));
                        RaisePropertyChanged(nameof(WhiteStoneNumber));
                        RaisePropertyChanged(nameof(NoneStoneNumber ));
                        break;
                }
            };
            StoneViewModel = new StoneViewModel(Game, Game.CurrentStone);
            StartClicked   = new DelegateCommand(async () => await Game.Play(), () => CanChangeMode).ObservesProperty(() => CanChangeMode);
        }
    }
}
