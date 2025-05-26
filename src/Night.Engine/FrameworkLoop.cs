using System;
using System.Runtime.InteropServices;

using Night.Types;
using SDL3; // Added for SDL3#

// Namespace for the public Framework API, consistent with Night.Window, Night.Graphics etc.
namespace Night
{
  /// <summary>
  /// Manages the main game loop and coordination of game states.
  /// Provides the main entry point to run a game.
  /// </summary>
  public static class Framework
  {
    /// <summary>
    /// A flag indicating whether the core SDL systems, particularly for input,
    /// have been successfully initialized.
    /// </summary>
    public static bool IsInputInitialized { get; private set; } = false;

    /// <summary>
    /// Runs the game instance.
    /// The game loop will internally call Load, Update, and Draw methods
    /// on the provided game type.
    /// </summary>
    /// <typeparam name="TGame">The type of the game to run.
    /// Must implement <see cref="Night.Types.IGame"/> and have a parameterless constructor.</typeparam>
    public static void Run<TGame>() where TGame : IGame, new()
    {
      // It's good practice to ensure SDL is initialized before using its functions.
      // Night.Window.SetMode handles SDL_InitSubSystem(SDL.InitFlags.Video).
      // If other subsystems are needed by the engine globally, they should be initialized.
      // NightSDL.Init(SDL.InitFlags.Events) might be useful here if not handled by Window.
      // However, SDL.PollEvent will work if the video subsystem (which often initializes events) is up.

      string sdlVersionString = NightSDL.GetVersion(); // Use our wrapper
      Console.WriteLine($"Night Engine: v0.0.1"); // Placeholder version
      Console.WriteLine($"SDL: v{sdlVersionString}");
      Console.WriteLine($"Platform: {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})");
      Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");

      // Engine.Run expects Night.Window.SetMode to have been called by the application (e.g., in Program.cs)
      // *before* Engine.Run is invoked.

      TGame game = new TGame();
      game.Load();

      ulong perfFrequency = SDL.GetPerformanceFrequency();
      ulong lastCounter = SDL.GetPerformanceCounter();

      // Ensure the window is open before starting the loop.
      // Night.Window.SetMode should have been called by the user application before Engine.Run.
      if (!Window.IsOpen())
      {
        Console.WriteLine("Night.Engine.Run: Window is not open. Ensure Night.Window.SetMode() was called successfully before Run().");
        // Potentially call NightSDL.Quit() here if Engine.Run was responsible for a global SDL_Init.
        return;
      }

      // At this point, Window.IsOpen() is true, implying SetMode was successful
      // and SDL.InitFlags.Video (which includes SDL.InitFlags.Events) has been initialized.
      IsInputInitialized = true;

      while (Window.IsOpen())
      {
        // Event Processing
        while (SDL.PollEvent(out SDL.Event e)) // Updated to use SDL3.SDL
        {
          if ((SDL.EventType)e.Type == SDL.EventType.Quit) // Updated to use SDL3.SDL.EventType and cast e.Type
          {
            Window.Close();
          }
          // Other event handling (keyboard, mouse) will be added in later tasks/epics.
          // else if ((SDL.EventType)e.Type == SDL.EventType.KeyDown) { /* ... */ }
        }

        // If Window.Close() was called due to an event, IsOpen() will now be false,
        // and the outer loop should terminate.
        if (!Window.IsOpen())
        {
          break;
        }

        // Calculate DeltaTime
        ulong currentCounter = SDL.GetPerformanceCounter();
        double deltaTime = (double)(currentCounter - lastCounter) / perfFrequency;
        lastCounter = currentCounter;

        // Clamp deltaTime to avoid large jumps if debugging or system lags
        // A common practice, though the max value can be debated (e.g., 1/15th of a second)
        if (deltaTime > 0.0666) // Approx 15 FPS
        {
          deltaTime = 0.0666;
        }


        game.Update(deltaTime);
        game.Draw();

        try
        {
          // Graphics.Present might not be implemented yet, but the call should be here.
          Graphics.Present();
        }
        catch (NotImplementedException)
        {
          // Silently ignore if Graphics.Present is not yet implemented.
          // Or log once: Console.WriteLine("Night.Graphics.Present() is not yet implemented.");
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error during Graphics.Present(): {ex.Message}");
          // Potentially break loop or handle more gracefully
        }

        // A small delay can be added here if vsync is not enabled or to reduce CPU usage,
        // but typically vsync (via renderer flags in SetMode) is preferred.
        // SDL.Delay(1); // e.g., 1ms delay
      }

      // TODO: Call game.Unload() if it's added to IGame.
      // TODO: Ensure proper SDL cleanup (NightSDL.Quit()), perhaps in a dedicated Engine.Shutdown()
      // or if Engine.Run is the outermost layer that also did NightSDL.Init().
      // For now, if Window.SetMode did SDL_InitSubSystem, a corresponding QuitSubSystem might be needed.
      // NightSDL.Quit(); // This would be too broad if other parts of app still use SDL.
    }
  }
}
