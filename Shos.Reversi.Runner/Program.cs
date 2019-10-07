using System;

namespace Shos.Reversi.Runner
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine($"Started at {DateTime.Now}.");
            using (var gameRunner = new GameRunner())
                gameRunner.Run().Wait();

            Console.WriteLine($"Finished at {DateTime.Now}.");
#if DEBUG
            Console.WriteLine($"GettingScoreCount: {Shos.Reversi.AI.AIPlayer.GettingScoreCount}.");
#endif // DEBUG

            Console.ReadKey();
        }
    }
}
