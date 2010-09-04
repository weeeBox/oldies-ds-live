using System;
using DuckstazyLive.foobar;

namespace DuckstazyLive
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            App app = new App(1280, 800);
            using (DuckstazyGame game = new DuckstazyGame(app))
            {
                game.Run();
            }
        }
    }
}

