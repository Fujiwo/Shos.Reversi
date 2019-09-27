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
            Console.ReadKey();
        }
    }
}
