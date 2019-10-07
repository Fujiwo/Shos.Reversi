using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Shos.Reversi.Core
{
    using Helpers;

    public class Game : BindableBase
    {
        public event Action?                   Start ;
        public event Action<Stone.StoneState>? Update;
        public event Action<Stone.StoneState>? End   ;

        public enum GameMode { ComputerVsComputer, HumanVsComputer, HumanVsHuman };

        public const int PlayerNumber = 2;

        GameMode mode                                 = GameMode.ComputerVsComputer;
        Board    board                                = new Board();
        int      delay                                = 0;
        static Dictionary<string, int> winninngCounts = new Dictionary<string, int>();

        public int PlayCount { get; set; } = 0;
        public static int WinninngCount(Player player) => winninngCounts[player.Name];

        readonly Func<ComputerPlayer>? createPlayer1;
        readonly Func<ComputerPlayer>? createPlayer2;

        public GameMode Mode {
            get => mode;
            set {
                InitializePlayers(value);
                SetProperty(ref mode, value);
            }
        }

        public Board Board {
            get => board;
            private set => SetProperty(ref board, value);
        }

        public int Delay {
            get => delay;
            set => SetProperty(ref delay, value);
        }

        public Stone CurrentStone { get; }  = new Stone();

        public Player[] Players = new Player[PlayerNumber];

        Player currentPlayer;

        public Player CurrentPlayer {
            get         => currentPlayer;
            private set => SetProperty(ref currentPlayer, value);
        }

        bool isPlaying = false;

        public bool IsPlaying {
            get         => isPlaying;
            private set => SetProperty(ref isPlaying, value);
        }

        public Game(Func<ComputerPlayer>? createPlayer1 = null, Func<ComputerPlayer>? createPlayer2 = null)
        {
            this.createPlayer1 = createPlayer1;
            this.createPlayer2 = createPlayer2;
            currentPlayer      = Players[0];

            Intialize();
        }

        public Player GetPlayer(Stone.StoneState state) => GetPlayer(ToCoset(state));
        public Player GetPlayer(Coset playerIndex) => Players[playerIndex.Value];

        public async Task Play()
        {
            StartPlaying();

            var passCount = 0;
            for (var coset = new Coset(PlayerNumber); passCount < PlayerNumber; coset++)
                passCount = await PlayTurn(passCount, coset);

            EndPlaying();
        }

        void StartPlaying()
        {
            Intialize();
            IsPlaying = true;
            Start?.Invoke();
        }

        void EndPlaying()
        {
            IsPlaying = false;
            PlayCount++;
            IncrementWinningCount();
            End?.Invoke(Board.Winner);
        }

        void IncrementWinningCount()
        {
            Players?.ForEach(player => {
                if (!winninngCounts.ContainsKey(player.Name))
                    winninngCounts.Add(player.Name, 0);
            });
            if (Board.Winner != Stone.StoneState.None)
                winninngCounts[GetPlayer(Board.Winner).Name]++;
        }

        async Task<int> PlayTurn(int passCount, Coset coset)
        {
            CurrentPlayer      = GetPlayer(coset);
            CurrentStone.State = ToStoneState(coset);

            var canTurnOverIndexes = Board.CanTurnOverIndexes(CurrentStone.State).ToList();
            Board.SetCanTurnOver(canTurnOverIndexes);
            if (canTurnOverIndexes.Count == 0) {
                passCount++;
            } else {
                var index = await Players[coset.Value].Turn(Board.Clone(), CurrentStone.State, canTurnOverIndexes);
                Contract.Ensures(canTurnOverIndexes.Contains(index));
                Board.TurnOverWith(index, CurrentStone.State);
                Update?.Invoke(CurrentStone.State/*, index*/);
                passCount = 0;
            }
            return passCount;
        }

        void Intialize()
        {
            Board = new Board();
            InitializePlayers(Mode);
        }

        void InitializePlayers(GameMode mode)
        {
            Players.ForEach(player => player?.Dispose());

            switch (mode) {
                case GameMode.ComputerVsComputer:
                    SetNewComputerPlayer(0, "Computer 1", createPlayer1);
                    SetNewComputerPlayer(1, "Computer 2", createPlayer2);
                    break;

                case GameMode.HumanVsComputer   :
                    Players[0] = new HumanPlayer { Index = 0, Name = "Human" };
                    SetNewComputerPlayer(1, "Computer", createPlayer1);
                    break;

                case GameMode.HumanVsHuman      :
                    Players[0] = new HumanPlayer { Index = 0, Name = "Human 1" };
                    Players[1] = new HumanPlayer { Index = 1, Name = "Human 2" };
                    break;

                default:
                    throw new InvalidOperationException();
            };
            Players.Shuffle();
            RaisePropertyChanged(nameof(Players));
        }

        void SetNewComputerPlayer(int index, string playerName, Func<ComputerPlayer>? createPlayer = null)
        {
            var player     = createPlayer == null ? new RandomComputerPlayer() : createPlayer();
            player.Index   = index;
            player.Name    = playerName;
            player.Delay   = Delay;
            Players[index] = player;
        }

        public void OnSelectStone(Board board, TableIndex index) => CurrentPlayer?.OnSelectStone(board, index);

        static Stone.StoneState ToStoneState(Coset coset)
            => coset.Value switch  {
                  0 => Stone.StoneState.Black,
                  1 => Stone.StoneState.White,
                  _ => throw new InvalidOperationException()
               };

        static Coset ToCoset(Stone.StoneState state)
            => state switch {
                  Stone.StoneState.Black => new Coset(2, 0),
                  Stone.StoneState.White => new Coset(2, 1),
                  _                      => throw new InvalidOperationException()
              };
    }
}
