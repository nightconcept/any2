using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using Night;
using Night.Configuration; // Added for configuration management

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

    private static bool _inErrorState = false;

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

      ConfigurationManager.LoadConfig();
      var windowConfig = ConfigurationManager.CurrentConfig.Window;

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

        // Setup initial window based on configuration BEFORE game.Load()
        Console.WriteLine("Night.Framework.Run: Initializing window with config/default settings before game.Load().");
        SDL.WindowFlags sdlFlags = (SDL.WindowFlags)0;
        if (windowConfig.Resizable) sdlFlags |= SDL.WindowFlags.Resizable;
        if (windowConfig.Borderless) sdlFlags |= SDL.WindowFlags.Borderless;
        if (windowConfig.HighDPI) sdlFlags |= SDL.WindowFlags.HighPixelDensity;

        bool modeSet = Window.SetMode(windowConfig.Width, windowConfig.Height, sdlFlags);
        if (!modeSet)
        {
          Console.WriteLine($"Night.Framework.Run: Failed to set initial window mode from configuration: {SDL.GetError()}");
          CleanUpSDL();
          return;
        }

        Window.SetTitle(windowConfig.Title);

        if (windowConfig.Fullscreen)
        {
          FullscreenType fsType = windowConfig.FullscreenType.ToLowerInvariant() == "exclusive"
                                    ? FullscreenType.Exclusive
                                    : FullscreenType.Desktop;
          if (!Window.SetFullscreen(true, fsType))
          {
            Console.WriteLine($"Night.Framework.Run: Failed to set initial fullscreen mode from configuration: {SDL.GetError()}");
          }
        }

        if (Window.RendererPtr != nint.Zero)
        {
          if (!SDL.SetRenderVSync(Window.RendererPtr, windowConfig.VSync))
          {
            Console.WriteLine($"Night.Framework.Run: Failed to set initial VSync mode from configuration: {SDL.GetError()}");
          }
        }

        if (windowConfig.X.HasValue && windowConfig.Y.HasValue && Window.Handle != nint.Zero)
        {
          SDL.SetWindowPosition(Window.Handle, windowConfig.X.Value, windowConfig.Y.Value);
        }
        // End of initial window setup

        try
        {
          game.Load(); // game.Load() can now use Graphics.NewImage(), and can also call Window.SetMode again to override.
        }
        catch (Exception e)
        {
          HandleGameException(e, game);
          if (_inErrorState)
          {
            CleanUpSDLAndWindow();
            return;
          }
        }

        // After game.Load(), check if window is still open. 
        // If game.Load() called Window.Close() or failed to maintain a window, we should not continue.
        if (!Window.IsOpen())
        {
          Console.WriteLine("Night.Framework.Run: Window is not open after game.Load(). Exiting.");
          CleanUpSDLAndWindow(); // Ensure cleanup if window was closed by game.Load()
          return;
        }
        // If game.Load() *did* change window settings (e.g. VSync via a new SetMode call),
        // we don't re-apply config VSync here unless we have a way to know it wasn't touched by game.
        // The current Window.SetMode creates a new renderer, so VSync would be reset anyway if game called SetMode.
        // So, if game called SetMode, it's responsible for its own VSync if it differs from config default for new renderer.
        // If game didn't call SetMode, our initial VSync setting stands.

        Night.Timer.Initialize();

        _frameCount = 0;
        _fpsTimeAccumulator = 0.0;
        _deltaHistory.Clear();


        // Main game loop
        while (Window.IsOpen() && !_inErrorState)
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
          while (SDL.PollEvent(out SDL.Event e) && !_inErrorState)
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
                HandleGameException(exUser, game);
              }
            }
            else if (eventType == SDL.EventType.KeyUp)
            {
              try
              {
                game.KeyReleased(
                    (KeySymbol)e.Key.Key,
                    (KeyCode)e.Key.Scancode
                );
              }
              catch (Exception exUser)
              {
                HandleGameException(exUser, game);
              }
            }
            else if (eventType == SDL.EventType.MouseButtonDown)
            {
              try
              {
                game.MousePressed(
                    (int)e.Button.X,
                    (int)e.Button.Y,
                    (MouseButton)e.Button.Button,
                    e.Button.Which == SDL.TouchMouseID, // istouch
                    e.Button.Clicks
                );
              }
              catch (Exception exUser)
              {
                HandleGameException(exUser, game);
              }
            }
            else if (eventType == SDL.EventType.MouseButtonUp)
            {
              try
              {
                game.MouseReleased(
                    (int)e.Button.X,
                    (int)e.Button.Y,
                    (MouseButton)e.Button.Button,
                    e.Button.Which == SDL.TouchMouseID, // istouch
                    e.Button.Clicks
                );
              }
              catch (Exception exUser)
              {
                HandleGameException(exUser, game);
              }
            }
            // TODO: Add other event handling (mouse, etc.) as per future tasks.
          }

          if (_inErrorState) // Check if error occurred during event processing
          {
            // Error handler (Default or custom) should have run.
            // Default handler enters its own loop or prepares for exit.
            // If it was a custom handler, it might have cleared _inErrorState or decided to continue.
            // If _inErrorState is still true, we break the main loop.
            break;
          }

          // Update
          if (!_inErrorState) // Do not update if an error has occurred and is being handled
          {
            try
            {
              game.Update((float)deltaTime);
            }
            catch (Exception exUser)
            {
              HandleGameException(exUser, game);
              if (_inErrorState) break; // Exit main loop if error sets state
            }
          }

          // Draw
          if (!_inErrorState) // Do not draw if an error has occurred and is being handled
          {
            try
            {
              // Graphics.BeginFrame() / Clear etc. should be called by game.Draw() or a higher level abstraction.
              // For now, FrameworkLoop does not manage the render target clearing directly.
              // It's assumed game.Draw() handles everything from clear to present.
              game.Draw();

              // Present the drawn frame to the screen
              Night.Graphics.Present(); 
            }
            catch (Exception exUser)
            {
              HandleGameException(exUser, game);
              // If Draw fails, we typically still want to try and finish the frame/loop iteration
              // unless _inErrorState is set by the handler to signal a desire to stop.
              if (_inErrorState) break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        // This is for errors within Framework.Run itself, not game code.
        Console.WriteLine($"Night.Framework.Run: An UNEXPECTED FRAMEWORK error occurred: {ex.ToString()}");
        // Attempt to call default error handler for framework errors too, but without game instance.
        HandleGameException(ex, null);
      }
      finally
      {
        // TODO: Call gameLogic.Quit() if it's added to IGame.

        CleanUpSDLAndWindow();
      }
    }

    private static void HandleGameException(Exception e, IGame? gameInstance)
    {
      _inErrorState = true; // Signal that we are now in an error state.

      var customHandler = Night.Error.GetHandler();
      if (customHandler != null)
      {
        try
        {
          customHandler(e);
          // If custom handler returns, we assume it handled the error
          // and the game might want to continue or has already quit.
          // For now, we'll still close the window to be safe, unless custom handler re-opens it.
          // This behavior might need refinement.
          if (Window.IsOpen()) Window.Close();
        }
        catch (Exception exHandler)
        {
          // Error in the custom error handler itself!
          Console.WriteLine($"Night.Framework.Run: CRITICAL: Exception in custom error handler: {exHandler.ToString()}");
          // Fallback to a very minimal default behavior
          Console.WriteLine($"Night.Framework.Run: Original game error: {e.ToString()}");
          if (Window.IsOpen()) Window.Close(); // Ensure shutdown
        }
      }
      else
      {
        DefaultErrorHandler(e, gameInstance);
      }
    }

    private static void DefaultErrorHandler(Exception e, IGame? gameInstance)
    {
      Console.Error.WriteLine("--- Night Engine: Default Error Handler ---");
      Console.Error.WriteLine($"An error occurred in the game: {e.GetType().Name}");
      Console.Error.WriteLine($"Message: {e.Message}");
      Console.Error.WriteLine("Stack Trace:");
      Console.Error.WriteLine(e.StackTrace);
      Console.Error.WriteLine("-------------------------------------------");

      bool canDrawError = false;
      try
      {
        if (!Window.IsOpen() || (Window.RendererPtr == nint.Zero)) // Assuming Graphics.RendererPtr is a good check for active graphics
        {
          Console.WriteLine("Night.Framework.Run (DefaultErrorHandler): Window or Graphics not initialized. Attempting to set mode...");
          // Attempt to set a basic window mode if not already open.
          // Use a default size. WindowFlags can be minimal or Resizable.
          if (Window.SetMode(800, 600, SDL.WindowFlags.Resizable))
          {
            Console.WriteLine("Night.Framework.Run (DefaultErrorHandler): Window mode set to 800x600.");
            canDrawError = (Window.RendererPtr != nint.Zero);
          }
          else
          {
            Console.WriteLine($"Night.Framework.Run (DefaultErrorHandler): Failed to set window mode. SDL Error: {SDL.GetError()}");
          }
        }
        else
        {
          canDrawError = true;
        }

        // Reset input state
        if (IsInputInitialized) // Check if input was ever initialized
        {
          Mouse.SetVisible(true);
          Mouse.SetGrabbed(false);
          Mouse.SetRelativeMode(false);
          // Mouse.SetCursor() - Skipped as per plan if complex; SDL default cursor should apply.
        }
      }
      catch (Exception resetEx)
      {
        Console.Error.WriteLine($"Night.Framework.Run (DefaultErrorHandler): Exception during state reset: {resetEx.ToString()}");
        canDrawError = false; // If reset fails, drawing might be unsafe.
      }


      if (canDrawError)
      {
        try
        {
          // Simple error display loop
          string fullErrorText = $"Error: {e.Message}\n\n{e.StackTrace}";
          // Shorten for display if too long, or make it scrollable if we had font rendering
          // For now, just display what fits or make user copy.

          Window.SetTitle($"Error - {gameInstance?.GetType().Name ?? "Night Game"}");

          bool runningErrorLoop = true;
          while (runningErrorLoop && Window.IsOpen())
          {
            while (SDL.PollEvent(out SDL.Event ev))
            {
              if (ev.Type == (uint)SDL.EventType.Quit)
              {
                runningErrorLoop = false;
                Window.Close();
                break;
              }
              if (ev.Type == (uint)SDL.EventType.KeyDown)
              {
                if (ev.Key.Key == SDL.Keycode.Escape)
                {
                  runningErrorLoop = false;
                  Window.Close();
                  break;
                }
                // Check for Ctrl+C - SDL.Keymod.Ctrl is a flag
                if (ev.Key.Key == SDL.Keycode.C && ((SDL.GetModState() & SDL.Keymod.Ctrl) != 0))
                {
                  try
                  {
                    if (Night.System.SetClipboardText(fullErrorText))
                    {
                      Console.WriteLine("(Error copied to clipboard)");
                    }
                    else
                    {
                      Console.WriteLine($"(Failed to copy error to clipboard: {SDL.GetError()})");
                    }
                  }
                  catch (Exception clipEx)
                  {
                    Console.WriteLine($"(Exception trying to copy to clipboard: {clipEx.Message})");
                  }
                }
              }
            }
            if (!runningErrorLoop) break;

            Graphics.Clear(new Color(89, 157, 220, 255)); // Blue background
            // Graphics.Print functionality is NOT available.
            // We will just show a blue screen and title. User must check console.
            // If Night.Font was available:
            // Graphics.SetColor(Night.Color.Black);
            // Graphics.Print($"Error: {e.Message}", 10, 10, Window.GetWidth() - 20);
            // Graphics.Print($"Press ESC to quit. Ctrl+C to copy.", 10, Window.GetHeight() - 30);


            Graphics.Present();
            Timer.Sleep(0.01f); // Sleep for 10ms
          }
        }
        catch (Exception drawEx)
        {
          Console.Error.WriteLine($"Night.Framework.Run (DefaultErrorHandler): Exception during error display loop: {drawEx.ToString()}");
        }
      }
      else
      {
        Console.WriteLine("Night.Framework.Run (DefaultErrorHandler): Cannot display visual error. Check console. Press Ctrl+C in console to quit if frozen.");
        // Loop to keep process alive for a bit for console reading, or just exit.
        // For now, just let it fall through to finally block.
      }
      // Ensure the main loop knows to terminate
      if (Window.IsOpen()) Window.Close();
    }

    private static void CleanUpSDLAndWindow()
    {
      // Shutdown window and related resources (renderer, etc.)
      // This should happen before SDL.QuitSubSystem for Video.
      if (Window.IsOpen())
      {
        // This case should ideally not be hit if _inErrorState or loop conditions were managed correctly
        Console.WriteLine("Night.Framework.Run (CleanUpSDLAndWindow): Window was still open, attempting to close.");
        Window.Close(); // This will set _isWindowOpen to false
      }
      // Window.Shutdown() handles destroying window, renderer, and SDL.QuitSubSystem(SDL.InitFlags.Video)
      // It's important that Shutdown is called AFTER the error handler's visual loop might have used the window/renderer.
      Window.Shutdown();

      CleanUpSDL();
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
