using System;
using System.Runtime.InteropServices;

using Night;

using SDL3;

// Namespace for the public Framework API.
namespace Night
{
  /// <summary>
  /// Manages the main game loop and coordination of game states.
  /// Provides the main entry point to run a game.
  /// </summary>
  public static class Framework
  {
    private static bool _isSdlInitialized = false;
    private static SDL.InitFlags _initializedSubsystems = 0;

    /// <summary>
    /// A flag indicating whether the core SDL systems, particularly for input,
    /// have been successfully initialized by this Framework's Run method.
    /// </summary>
    public static bool IsInputInitialized { get; private set; } = false;

    /// <summary>
    /// Runs the game instance.
    /// The game loop will internally call Load, Update, and Draw methods
    /// on the provided game logic.
    /// This method will initialize and shut down required SDL subsystems.
    /// </summary>
    /// <param name="game">The game interface to run. Must implement <see cref="Night.IGame"/>.</param>
    public static void Run(IGame game)
    {
      if (game == null)
      {
        Console.WriteLine("Night.Framework.Run: gameLogic cannot be null.");
        return;
      }

      string sdlVersionString = NightSDL.GetVersion();
      Console.WriteLine($"Night Engine: v0.0.1");
      Console.WriteLine($"SDL: v{sdlVersionString}");
      Console.WriteLine($"Platform: {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})");
      Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");

      try
      {
        _initializedSubsystems = SDL.InitFlags.Video | SDL.InitFlags.Events;
        if (!SDL.Init(_initializedSubsystems))
        {
          Console.WriteLine($"Night.Framework.Run: SDL_Init failed: {SDL.GetError()}");
          return;
        }
        _isSdlInitialized = true;
        IsInputInitialized = (_initializedSubsystems & SDL.InitFlags.Events) == SDL.InitFlags.Events;

        game.Load();

        // Now, ensure a window is open before proceeding.
        if (!Window.IsOpen())
        {
          Console.WriteLine("Night.Framework.Run: Window is not open after gameLogic.Load(). Ensure Night.Window.SetMode() was called successfully within Load().");
          // SDL was initialized, so it needs to be quit.
          CleanUpSDL();
          return;
        }

        ulong perfFrequency = SDL.GetPerformanceFrequency();
        ulong lastCounter = SDL.GetPerformanceCounter();

        // Main game loop
        while (Window.IsOpen())
        {
          // Event Processing
          while (SDL.PollEvent(out SDL.Event e))
          {
            // Cast e.Type once to avoid repeated casting
            var eventType = (SDL.EventType)e.Type;

            if (eventType == SDL.EventType.Quit)
            {
              Window.Close(); // This will set Window.IsOpen() to false
            }
            else if (eventType == SDL.EventType.KeyDown)
            {
              // The 'key' parameter for love.keypressed is the character of the pressed key (KeyConstant).
              // SDL's e.key.keysym.sym is an SDL.Keycode, which represents the key symbol.
              // The 'scancode' parameter for love.keypressed is the physical key (Scancode).
              // SDL's e.key.keysym.scancode is an SDL.Scancode.
              // Night.KeyCode is currently based on SDL.Scancode.
              // For now, we will cast both sym and scancode to Night.KeyCode.
              // Night.KeyCode is based on SDL.Scancode.
              // e.Key.Key is SDL.Keycode (symbol).
              // e.Key.Scancode is SDL.Scancode (physical).
              // This mapping for 'key' might need refinement.
              try
              {
                game.KeyPressed(
                    (Night.KeySymbol)e.Key.Key,    // Cast SDL.Keycode to Night.KeySymbol
                    (Night.KeyCode)e.Key.Scancode, // Cast SDL.Scancode to Night.KeyCode (Night.KeyCode is based on Scancode)
                    e.Key.Repeat                 // This is already a bool
                );
              }
              catch (Exception exUser)
              {
                Console.WriteLine($"Night.Framework.Run: Error in game.KeyPressed: {exUser.Message}{Environment.NewLine}{exUser.StackTrace}");
                Window.Close(); // Signal loop termination
              }
            }
            // TODO: Add other event handling (mouse, etc.) as per future tasks.
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

          // Clamp deltaTime to avoid large jumps
          if (deltaTime > 0.0666) // Approx 15 FPS
          {
            deltaTime = 0.0666;
          }

          try
          {
            game.Update(deltaTime);
          }
          catch (Exception exUser)
          {
            Console.WriteLine($"Night.Framework.Run: Error in game.Update: {exUser.Message}{Environment.NewLine}{exUser.StackTrace}");
            Window.Close(); // Signal loop termination
            // Skip Draw and Present if Update failed and loop is closing
            if (!Window.IsOpen()) continue;
          }

          try
          {
            game.Draw();
          }
          catch (Exception exUser)
          {
            Console.WriteLine($"Night.Framework.Run: Error in game.Draw: {exUser.Message}{Environment.NewLine}{exUser.StackTrace}");
            Window.Close(); // Signal loop termination
            // Skip Present if Draw failed and loop is closing
            if (!Window.IsOpen()) continue;
          }

          try
          {
            Graphics.Present();
          }
          catch (NotImplementedException)
          {
            // Silently ignore if Graphics.Present is not yet implemented for now.
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Error during Graphics.Present(): {ex.Message}");
            // Consider breaking the loop or handling more gracefully
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Night.Framework.Run: An unexpected error occurred: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
        // Ensure SDL is cleaned up even if an error occurs in Load, Update, or Draw.
      }
      finally
      {
        // TODO: Call gameLogic.Unload() if it's added to IGame.

        // Shutdown window and related resources (renderer, etc.)
        // This should happen before SDL.QuitSubSystem for Video.
        if (Window.IsOpen()) // Should ideally be closed by the loop, but as a safeguard
        {
          Console.WriteLine("Night.Framework.Run: Window was still open in finally block, attempting to close.");
          Window.Close();
        }
        // Window.Shutdown() handles destroying window, renderer, and SDL.QuitSubSystem(SDL.InitFlags.Video)
        Window.Shutdown();

        CleanUpSDL();
      }
    }

    private static void CleanUpSDL()
    {
      if (_isSdlInitialized)
      {
        // SDL.QuitSubSystem was already called for Video by Window.Shutdown().
        // We only need to quit other subsystems explicitly initialized by Run if they weren't covered.
        // However, SDL.Quit() handles all initialized subsystems.
        SDL.Quit();
        _isSdlInitialized = false;
        IsInputInitialized = false;
        _initializedSubsystems = 0;
      }
    }
  }
}
