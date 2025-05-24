using System;
// We will now use Night.SDL which uses SDL3-CS
using Night; // This brings Night.SDL into scope

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Night Sample Game Starting...");
        Console.WriteLine("--- SDL Test Start (via Night.Engine with SDL3-CS) ---");

        // Use Night.SDL which wraps SDL3-CS
        // Note: SDL_InitFlags are now accessed via Night.SDL or directly from SDL3.SDL if preferred/needed for more flags
        if (Night.SDL.Init(Night.SDL.InitVideo) < 0) // SDL_Init returns 0 on success, <0 on error
        {
            // To get error, we'd ideally have a Night.SDL.GetError() or use SDL3.SDL.SDL_GetError() directly
            // For now, let's assume SDL3-CS might throw an exception or we check its error handling.
            // For simplicity in this step, we'll just print a generic error.
            // A proper GetError() wrapper should be added to Night.SDL if needed.
            Console.WriteLine($"SDL_Init Error. SDL Error: {SDL3.SDL.SDL_GetError()}");
        }
        else
        {
            Console.WriteLine("SDL_Init Succeeded.");
            string versionString = Night.SDL.GetVersion();
            Console.WriteLine($"SDL Version (from Night.Engine via SDL3-CS): {versionString}");

            Night.SDL.Quit();
            Console.WriteLine("SDL_Quit Succeeded.");
        }
        Console.WriteLine("--- SDL Test End ---");
        Console.WriteLine(); // Add a blank line for readability

        // Example of how the engine might be run:
        // GameEngine.Run<Game>(); // Temporarily commented out to focus on P/Invoke test

        Console.WriteLine("Night Sample Game Exited.");
    }
}
