//#define LoggingOn
//#define MLPlayer
#define AIPlayer

using System;
using System.Threading.Tasks;

namespace Shos.Reversi.Runner
{
    using Shos.Reversi.AI;
    using Shos.Reversi.Core;
    using Shos.Reversi.Core.Helpers;
#if MLPlayer
    using Shos.Reversi.ML;
#endif // MLPlayer

    class GameRunner : IDisposable
    {
#if MLPlayer
        const string modelPath  = @"Data\MLModel.zip";
#endif // MLPlayer
        const int    playCount  = 1000;

#if MLPlayer
        //Game        game        = new Game(() => new MLPlayer(modelPath), () => new MLPlayer(modelPath));
        Game        game        = new Game(() => new MLPlayer(modelPath));
#elif AIPlayer // MLPlayer
#if LoggingOn
        Game game = new Game(() => new AIPlayer { Log = Console.WriteLine });
#else // LoggingOn
        Game game = new Game(() => new AIPlayer());
#endif // LoggingOn
#else // MLPlayer
        Game game        = new Game();
#endif // MLPlayer
        GameHistory gameHistory = new GameHistory();

        public GameRunner()
        {
            game.Mode  = Game.GameMode.ComputerVsComputer;
            game.Delay = 0;

            game.Start  += OnGameStart ;
            game.Update += OnGameUpdate;
            game.End    += OnGameEnd   ;
        }

        public async Task Run()
        {
            try {
                for (var counter = 0; counter < playCount; counter++)
                    await game.Play();
                Report();
            } catch (Exception ex) {
                Log($"Error: {ex}");
            }
        }

        public void Dispose() => gameHistory.Dispose();

        void OnGameStart()
        {
            Log("GameStart");
            gameHistory.Start();
        }

        void OnGameEnd(Stone.StoneState winner)
        {
            var player0 = game.GetPlayer(new Coset(Game.PlayerNumber, 0));
            var player1 = game.GetPlayer(new Coset(Game.PlayerNumber, 1));

            var winnerName = winner == Stone.StoneState.None ? "Nobody" : game.GetPlayer(winner).Name;
            Log($"{game.PlayCount} Won by {winnerName}; Game Count ({player0.Name} {Game.WinninngCount(player0)} : {player1.Name} {Game.WinninngCount(player1)})");

            gameHistory.End(winner);
        }

        void OnGameUpdate(Stone.StoneState state) => gameHistory.Add(game.Board, state);

        void Report()
        {
            var player0 = game.GetPlayer(new Coset(Game.PlayerNumber, 0));
            var player1 = game.GetPlayer(new Coset(Game.PlayerNumber, 1));
            Console.WriteLine($"Play Count: {game.PlayCount} ({player0.Name} {Game.WinninngCount(player0)} : {player1.Name} {Game.WinninngCount(player1)})");
        }

#if LoggingOn
        void Log(string text) => Console.WriteLine(text);
#else // LoggingOn
        void Log(string _) {}
#endif // LoggingOn
    }
}
