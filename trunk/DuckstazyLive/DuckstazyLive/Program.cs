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
            ApplicationSettings settings;
            settings.width = 1280;
            settings.height = 800;
            settings.maxPlayersCount = 1;
            settings.maxTimersCount = 32;

            App app = new App(settings);
            using (DuckstazyGame game = new DuckstazyGame(app))
            {
                game.Run();
            }
        }
    }
}

