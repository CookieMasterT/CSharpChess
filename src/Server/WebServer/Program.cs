using CSharpChess.Game;
using WebServer.HttpService;

namespace WebServer
{
    internal static class Program
    {
        static Program() {
            GameLogicMain = new GameLogic();
        }

        public static GameLogic GameLogicMain { get; }

        static async Task Main()
        {
            Console.WriteLine("Press R to reset the board.");
            Console.WriteLine("Press Esc to exit.");

            GameLogicMain.SetupBoard();

            _ = HttpService.Connection.StartConnection(new Uri("http://localhost:54321/"));
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.R)
                {
                    GameLogicMain.SetupBoard();
                    await ClientManager.SendMessageAll("refreshBoard").ConfigureAwait(false);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}
