using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

    private static int _frameCount = 0;
    private static double _fpsTimeAccumulator = 0.0;
    private static List<double> _deltaHistory = new List<double>();
    private const int MaxDeltaHistorySamples = 60; // Store up to 1 second of deltas at 60fps


    /// <summary>
    /// A flag indicating whether the core SDL systems, particularly for input,
    /// have been successfully initialized by this Framework's Run method.
    /// </summary>
    public static bool IsInputInitialized { get; private set; } = false;

    private static string GetFormattedPlatformString()
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
      {
        try
        {
          string macOSVersion = string.Empty;
          string darwinVersion = string.Empty;

          // Get macOS version
          ProcessStartInfo swVersPsi = new ProcessStartInfo
          {
            FileName = "sw_vers",
            Arguments = "-productVersion",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
          };
          using (Process swVersProcess = Process.Start(swVersPsi)!)
          {
            macOSVersion = swVersProcess.StandardOutput.ReadToEnd().Trim();
            swVersProcess.WaitForExit();
          }

          // Get Darwin kernel version
          ProcessStartInfo unamePsi = new ProcessStartInfo
          {
            FileName = "uname",
            Arguments = "-r",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
          };
          using (Process unameProcess = Process.Start(unamePsi)!)
          {
            darwinVersion = unameProcess.StandardOutput.ReadToEnd().Trim();
            unameProcess.WaitForExit();
          }

          if (!string.IsNullOrEmpty(macOSVersion) && !string.IsNullOrEmpty(darwinVersion))
          {
            return $"Platform: macOS {macOSVersion} (Darwin {darwinVersion})";
          }
        }
        catch (Exception ex)
        {
          // Log the exception or handle it as needed, then fall back.
          Console.WriteLine($"Night.Framework.Run: Could not retrieve detailed macOS version info: {ex.Message}");
        }
      }
      // Fallback for non-macOS platforms or if macOS version retrieval fails
      return $"Platform: {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})";
    }

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
      Console.WriteLine(GetFormattedPlatformString());
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
          CleanUpSDL();
          return;
        }

        Night.Timer.Initialize(); // Initialize Timer performance frequency and last step time

        _frameCount = 0;
        _fpsTimeAccumulator = 0.0;
        _deltaHistory.Clear();


        // Main game loop
        while (Window.IsOpen())
        {

          // Calculate DeltaTime by calling Night.Timer.Step()
          double deltaTime = Night.Timer.Step();

          // FPS Calculation
          _frameCount++;
          _fpsTimeAccumulator += deltaTime;
          if (_fpsTimeAccumulator >= 1.0)
          {
            Night.Timer.CurrentFPS = _frameCount;
            _frameCount = 0;
            _fpsTimeAccumulator -= 1.0; // Subtract 1 second, keep remainder for accuracy
          }

          // Average Delta Calculation
          _deltaHistory.Add(deltaTime);
          if (_deltaHistory.Count > MaxDeltaHistorySamples)
          {
            _deltaHistory.RemoveAt(0); // Keep the list size bounded
          }
          if (_deltaHistory.Count > 0)
          {
            Night.Timer.CurrentAverageDelta = _deltaHistory.Average();
          }


          // Event Processing
          while (SDL.PollEvent(out SDL.Event e))
          {
            var eventType = (SDL.EventType)e.Type;

            if (eventType == SDL.EventType.Quit)
            {
              Window.Close();
            }
            else if (eventType == SDL.EventType.KeyDown)
            {
              try
              {
                // TODO: Rename these to match love2d
                game.KeyPressed(
                    (KeySymbol)e.Key.Key,
                    (KeyCode)e.Key.Scancode,
                    e.Key.Repeat
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

          try
          {
            game.Update((float)deltaTime); // Pass float deltaTime as per IGame interface
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
