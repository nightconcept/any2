using System; // For NotImplementedException
using Night.Types; // For IGame
using System.Runtime.InteropServices;
using static SDL3.SDL; // SDL_GetVersion is in SDL3.SDL

// Namespace for the public Engine API, consistent with Night.Window, Night.Graphics etc.
namespace Night
{
    /// <summary>
    /// Manages the main game loop and coordination of game states.
    /// Provides the main entry point to run a game.
    /// </summary>
    public static class Engine
    {
        /// <summary>
        /// Runs the game instance.
        /// The game loop will internally call Load, Update, and Draw methods
        /// on the provided game type.
        /// </summary>
        /// <typeparam name="TGame">The type of the game to run.
        /// Must implement <see cref="Night.Types.IGame"/> and have a parameterless constructor.</typeparam>
        public static void Run<TGame>() where TGame : IGame, new()
        {
            var sdlv = SDL_GetVersion();
            Console.WriteLine($"Night Engine: v0.0.1"); // Placeholder version
            Console.WriteLine($"SDL: v{sdlv / 1000000}.{(sdlv / 1000) % 1000}.{sdlv % 1000}");
            Console.WriteLine($"Platform: {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})");
            Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");
            // Stub for Task 2.2: Actual game loop logic to be implemented later.
            // This method will eventually:
            // 1. Create an instance of TGame.
            // 2. Call game.Load().
            // 3. Enter a loop (e.g., while (Night.Window.IsOpen())).
            //    a. Process events.
            //    b. Call game.Update(deltaTime).
            //    c. Call game.Draw().
            //    d. Call Night.Graphics.Present().
            // 4. Clean up when the loop exits.
            throw new NotImplementedException("The Night.Engine.Run method is not yet implemented.");
        }
    }
}
