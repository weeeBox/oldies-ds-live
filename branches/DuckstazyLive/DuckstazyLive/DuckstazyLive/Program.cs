using System;
using app;

namespace DuckstazyLive
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DuckstazyGame game = new DuckstazyGame())
            {
                game.Run();
            }
        }
    }
}

