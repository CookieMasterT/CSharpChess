using CSharpChess.Game;
using WebServer.HttpService;

namespace WebServer
{
    internal static class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Press R to reset the board.");
            Console.WriteLine("Press Esc to exit.");
            GameLogic.SetupBoard();

            _ = HttpService.Connection.StartConnection(new Uri("http://localhost:54321/"));
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.R)
                {
                    GameLogic.SetupBoard();
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
