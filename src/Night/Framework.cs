// <copyright file="Framework.cs" company="Night Circle">
// zlib license
//
// Copyright (c) 2025 Danny Solivan, Night Circle
//
// This software is provided 'as-is', without any express or implied
// warranty. In no event will the authors be held liable for any damages
// arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software. If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using Night;

using SDL3;

namespace Night
{
  /// <summary>
  /// Manages the main game loop and coordination of game states.
  /// Provides the main entry point to run a game.
  /// </summary>
  public static class Framework
  {
    private const int MaxDeltaHistorySamples = 60; // Store up to 1 second of deltas at 60fps

    private static readonly object SdlLock = new object(); // Thread synchronization for SDL operations
    private static bool isSdlInitialized = false; // Tracks if SDL is currently active globally
    private static SDL.InitFlags initializedSubsystemsFlags = 0;

    private static int frameCount = 0;
    private static double fpsTimeAccumulator = 0.0;
    private static List<double> deltaHistory = new List<double>();

    private static bool inErrorState = false;

    /// <summary>
    /// Gets a value indicating whether a flag indicating whether the core SDL systems, particularly for input,
    /// have been successfully initialized by this Framework's Run method.
    /// </summary>
    public static bool IsInputInitialized { get; internal set; } = false;

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

      inErrorState = false;
      IsInputInitialized = false;

      ConfigurationManager.LoadConfig();
      var windowConfig = ConfigurationManager.CurrentConfig.Window;

      string nightVersionString = VersionInfo.GetVersion();
      string sdlVersionString = NightSDL.GetVersion();
      Console.WriteLine($"Night Engine: v{nightVersionString}");
      Console.WriteLine($"SDL: v{sdlVersionString}");
      Console.WriteLine(GetFormattedPlatformString());
      Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");

      bool sdlSuccessfullyInitializedThisRun = false;

      try
      {
        // Check if running in a testing environment (e.g., CI/CD, headless environments)
        bool isTestingEnvironment = IsTestingEnvironment();
        if (isTestingEnvironment)
        {
          Console.WriteLine("Night.Framework.Run: Testing environment detected. Setting SDL video driver to 'dummy'.");
          // Set the video driver to "dummy" for headless testing environments
          // This resolves "No available video device" errors in CI/CD systems
          _ = SDL.SetHint(SDL.Hints.VideoDriver, "dummy");

          Console.WriteLine("Night.Framework.Run: Testing environment detected. Setting SDL render driver to 'software'.");
          // Force software rendering to avoid OpenGL/GLES initialization issues in headless environments
          _ = SDL.SetHint(SDL.Hints.RenderDriver, "software");
        }

        lock (SdlLock)
        {
          if (!isSdlInitialized)
          {
            Console.WriteLine("Night.Framework.Run: Global isSdlInitialized is false. Attempting SDL.Init().");
            initializedSubsystemsFlags = SDL.InitFlags.Video | SDL.InitFlags.Events;
            if (!SDL.Init(initializedSubsystemsFlags))
            {
              Console.WriteLine($"Night.Framework.Run: SDL_Init failed: {SDL.GetError()}");
              return;
            }

            Console.WriteLine("Night.Framework.Run: SDL.Init() successful.");
            isSdlInitialized = true;
            sdlSuccessfullyInitializedThisRun = true;
          }
          else
          {
            Console.WriteLine("Night.Framework.Run: Global isSdlInitialized is true. Skipping SDL.Init().");
          }
        }

        IsInputInitialized = (initializedSubsystemsFlags & SDL.InitFlags.Events) == SDL.InitFlags.Events;
        Console.WriteLine($"Night.Framework.Run: IsInputInitialized set to {IsInputInitialized}.");

        SDL.WindowFlags sdlFlags = 0;
        if (windowConfig.Resizable)
        {
          sdlFlags |= SDL.WindowFlags.Resizable;
        }

        if (windowConfig.Borderless)
        {
          sdlFlags |= SDL.WindowFlags.Borderless;
        }

        if (windowConfig.HighDPI)
        {
          sdlFlags |= SDL.WindowFlags.HighPixelDensity;
        }

        Console.WriteLine($"Night.Framework.Run: Calling Window.SetMode with Width={windowConfig.Width}, Height={windowConfig.Height}, Flags={sdlFlags}");

        bool modeSet = Window.SetMode(windowConfig.Width, windowConfig.Height, sdlFlags);
        Console.WriteLine($"Night.Framework.Run: Window.SetMode returned {modeSet}.");

        if (!modeSet)
        {
          Console.WriteLine($"Night.Framework.Run: Window.SetMode returned false. Window.Handle: {Window.Handle}, Window.IsOpen(): {Window.IsOpen()}. SDL Error: {SDL.GetError()}");
          return;
        }

        Window.SetTitle(windowConfig.Title ?? "Night Game");
        Console.WriteLine($"Night.Framework.Run: Window title set to '{Window.GetMode().Title}'.");

        if (!Window.IsOpen())
        {
          // This condition implies modeSet was true, but IsOpen is now false.
          Console.WriteLine($"Night.Framework.Run: Window.IsOpen() is false AFTER modeSet was true and title was set. Window.Handle: {Window.Handle}. SDL Error: {SDL.GetError()}");
        }
        else
        {
          Console.WriteLine($"Night.Framework.Run: Window.IsOpen() is true after SetMode and SetTitle. Window.Handle: {Window.Handle}");
        }

        if (windowConfig.Fullscreen)
        {
          Console.WriteLine($"Night.Framework.Run: Attempting to set fullscreen: {windowConfig.FullscreenType}.");
          FullscreenType fsType = windowConfig.FullscreenType.ToLowerInvariant() == "exclusive"
                                    ? FullscreenType.Exclusive
                                    : FullscreenType.Desktop;
          if (!Window.SetFullscreen(true, fsType))
          {
            Console.WriteLine($"Night.Framework.Run: Failed to set initial fullscreen mode from configuration: {SDL.GetError()}");
          }
          else
          {
            Console.WriteLine($"Night.Framework.Run: SetFullscreen successful.");
          }
        }

        if (Window.RendererPtr != nint.Zero)
        {
          Console.WriteLine($"Night.Framework.Run: Attempting to set VSync: {windowConfig.VSync}.");
          if (!SDL.SetRenderVSync(Window.RendererPtr, windowConfig.VSync ? 1 : 0))
          {
            Console.WriteLine($"Night.Framework.Run: Failed to set initial VSync mode from configuration: {SDL.GetError()}");
          }
          else
          {
            Console.WriteLine($"Night.Framework.Run: SetRenderVSync successful.");
          }
        }

        if (windowConfig.X.HasValue && windowConfig.Y.HasValue && Window.Handle != nint.Zero)
        {
          Console.WriteLine($"Night.Framework.Run: Setting window position to X={windowConfig.X.Value}, Y={windowConfig.Y.Value}.");
          _ = SDL.SetWindowPosition(Window.Handle, windowConfig.X.Value, windowConfig.Y.Value);
        }

        if (!string.IsNullOrEmpty(windowConfig.IconPath) && Window.Handle != nint.Zero)
        {
          string iconFullPath = windowConfig.IconPath;
          if (!Path.IsPathRooted(iconFullPath))
          {
            iconFullPath = Path.Combine(AppContext.BaseDirectory, iconFullPath);
          }

          Console.WriteLine($"Night.Framework.Run: Setting window icon from '{iconFullPath}'.");
          if (!Window.SetIcon(iconFullPath))
          {
            Console.WriteLine($"Night.Framework.Run: Failed to set window icon from configuration: '{iconFullPath}'. Check path and image format.");
          }
          else
          {
            Console.WriteLine($"Night.Framework.Run: Window icon set successfully.");
          }
        }

        Console.WriteLine($"Night.Framework.Run: Proceeding to game.Load(). Window.IsOpen(): {Window.IsOpen()}, Window.Handle: {Window.Handle}");
        try
        {
          game.Load();
          Console.WriteLine($"Night.Framework.Run: game.Load() completed. Window.IsOpen(): {Window.IsOpen()}, Window.Handle: {Window.Handle}");
          if (!Window.IsOpen())
          {
            Console.WriteLine($"Night.Framework.Run: Window is NOT open after game.Load() completed. SDL Error: {SDL.GetError()}");
          }
        }
        catch (Exception e)
        {
          Console.WriteLine($"Night.Framework.Run: Exception during game.Load(): {e.Message}");
          HandleGameException(e, game);
          if (inErrorState)
          {
            return;
          }
        }

        if (!Window.IsOpen())
        {
          Console.WriteLine($"Night.Framework.Run: CRITICAL CHECK - Window is not open after game.Load() for {game.GetType().FullName}. Exiting game loop early. SDL Error if relevant: {SDL.GetError()}");
          return;
        }

        Console.WriteLine($"Night.Framework.Run: Starting main loop. Window.IsOpen(): {Window.IsOpen()}");
        Night.Timer.Initialize();
        frameCount = 0;
        fpsTimeAccumulator = 0.0;
        deltaHistory.Clear();

        while (Window.IsOpen() && !inErrorState)
        {
          double deltaTime = Night.Timer.Step();
          frameCount++;
          fpsTimeAccumulator += deltaTime;
          if (fpsTimeAccumulator >= 1.0)
          {
            Night.Timer.CurrentFPS = frameCount;
            frameCount = 0;
            fpsTimeAccumulator -= 1.0;
          }

          deltaHistory.Add(deltaTime);
          if (deltaHistory.Count > MaxDeltaHistorySamples)
          {
            deltaHistory.RemoveAt(0);
          }

          if (deltaHistory.Count > 0)
          {
            Night.Timer.CurrentAverageDelta = deltaHistory.Average();
          }

          while (SDL.PollEvent(out SDL.Event e) && !inErrorState)
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
                game.KeyPressed((KeySymbol)e.Key.Key, (KeyCode)e.Key.Scancode, e.Key.Repeat);
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
                game.KeyReleased((KeySymbol)e.Key.Key, (KeyCode)e.Key.Scancode);
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
                game.MousePressed((int)e.Button.X, (int)e.Button.Y, (MouseButton)e.Button.Button, e.Button.Which == SDL.TouchMouseID, e.Button.Clicks);
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
                game.MouseReleased((int)e.Button.X, (int)e.Button.Y, (MouseButton)e.Button.Button, e.Button.Which == SDL.TouchMouseID, e.Button.Clicks);
              }
              catch (Exception exUser)
              {
                HandleGameException(exUser, game);
              }
            }
          }

          if (inErrorState)
          {
            break;
          }

          if (!inErrorState)
          {
            try
            {
              game.Update((float)deltaTime);
            }
            catch (Exception exUser)
            {
              HandleGameException(exUser, game);
              if (inErrorState)
              {
                break;
              }
            }
          }

          if (!inErrorState)
          {
            try
            {
              game.Draw();
              Night.Graphics.Present();
            }
            catch (Exception exUser)
            {
              HandleGameException(exUser, game);
              if (inErrorState)
              {
                break;
              }
            }
          }
        }

        Console.WriteLine($"Night.Framework.Run: Main loop ended. Window.IsOpen(): {Window.IsOpen()}, inErrorState: {inErrorState}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Night.Framework.Run: An UNEXPECTED FRAMEWORK error occurred: {ex.ToString()}");
        HandleGameException(ex, null);
      }
      finally
      {
        Console.WriteLine($"Night.Framework.Run: Entering finally block. sdlSuccessfullyInitializedThisRun: {sdlSuccessfullyInitializedThisRun}, isSdlInitialized (static): {isSdlInitialized}");
        Window.Shutdown();

        lock (SdlLock)
        {
          if (sdlSuccessfullyInitializedThisRun)
          {
            Console.WriteLine($"Night.Framework.Run: SDL was initialized this run. Quitting SDL subsystems and SDL.");
            if (initializedSubsystemsFlags != 0)
            {
              SDL.QuitSubSystem(initializedSubsystemsFlags);
              Console.WriteLine($"Night.Framework.Run: QuitSubSystem({initializedSubsystemsFlags}) called.");
              initializedSubsystemsFlags = 0;
            }

            SDL.Quit();
            Console.WriteLine($"Night.Framework.Run: SDL.Quit() called.");
            isSdlInitialized = false;
          }
          else
          {
            Console.WriteLine($"Night.Framework.Run: SDL was not initialized this run or Init failed. Skipping SDL.Quit(). Global isSdlInitialized: {isSdlInitialized}");
          }
        }

        IsInputInitialized = false;
        Console.WriteLine($"Night.Framework.Run: Exiting finally block. IsInputInitialized: {IsInputInitialized}, isSdlInitialized (static): {isSdlInitialized}");
      }
    }

    private static void HandleGameException(Exception e, IGame? gameInstance)
    {
      Console.WriteLine($"Night.Framework.HandleGameException: Error: {e.Message}");
      inErrorState = true;
      var customHandler = Night.Error.GetHandler();
      if (customHandler != null)
      {
        try
        {
          customHandler(e);
          if (Window.IsOpen())
          {
            Window.Close();
          }
        }
        catch (Exception exHandler)
        {
          Console.WriteLine($"Night.Framework.Run: CRITICAL: Exception in custom error handler: {exHandler.ToString()}");
          Console.WriteLine($"Night.Framework.Run: Original game error: {e.ToString()}");
          if (Window.IsOpen())
          {
            Window.Close();
          }
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
        if (!Window.IsOpen() || (Window.RendererPtr == nint.Zero))
        {
          Console.WriteLine("Night.Framework.Run (DefaultErrorHandler): Window or Graphics not initialized. Attempting to set mode...");
          if (Window.SetMode(800, 600, SDL.WindowFlags.Resizable))
          {
            Console.WriteLine("Night.Framework.Run (DefaultErrorHandler): Window mode set to 800x600.");
            canDrawError = Window.RendererPtr != nint.Zero;
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

        if (IsInputInitialized)
        {
          Mouse.SetVisible(true);
          Mouse.SetGrabbed(false);
          Mouse.SetRelativeMode(false);
        }
      }
      catch (Exception resetEx)
      {
        Console.Error.WriteLine($"Night.Framework.Run (DefaultErrorHandler): Exception during state reset: {resetEx.ToString()}");
        canDrawError = false;
      }

      if (canDrawError)
      {
        try
        {
          string fullErrorText = $"Error: {e.Message}\n\n{e.StackTrace}";
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
              }
            }

            if (!runningErrorLoop)
            {
              break;
            }

            _ = SDL.SetRenderDrawColor(Window.RendererPtr, 30, 30, 30, 255);
            _ = SDL.RenderClear(Window.RendererPtr);
            _ = SDL.RenderPresent(Window.RendererPtr);
            SDL.Delay(16);
          }
        }
        catch (Exception drawEx)
        {
          Console.Error.WriteLine($"Night.Framework.Run (DefaultErrorHandler): Exception during error display loop: {drawEx.ToString()}");
        }
      }

      if (Window.IsOpen())
      {
        Window.Close();
      }
    }

    /// <summary>
    /// Detects if the application is running in a testing environment.
    /// This includes CI/CD systems, automated test runners, or when explicitly configured for testing.
    /// </summary>
    /// <returns>True if running in a testing environment, false otherwise.</returns>
    private static bool IsTestingEnvironment()
    {
      // Check for common CI/CD environment variables
      var ciEnvironmentVars = new[]
      {
        "CI", "CONTINUOUS_INTEGRATION", "GITHUB_ACTIONS", "GITLAB_CI",
        "JENKINS_URL", "BUILDKITE", "CIRCLECI", "TRAVIS", "APPVEYOR",
      };

      foreach (var envVar in ciEnvironmentVars)
      {
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(envVar)))
        {
          return true;
        }
      }

      // Check for test runner processes in the call stack or environment
      try
      {
        var processName = Process.GetCurrentProcess().ProcessName;
        if (processName.Contains("dotnet") || processName.Contains("testhost") ||
            processName.Contains("vstest") || processName.Contains("xunit"))
        {
          return true;
        }
      }
      catch
      {
        // Ignore any exceptions during process name detection
      }

      // Check for SDL_VIDEODRIVER environment variable already set to testing modes
      var videoDriver = Environment.GetEnvironmentVariable("SDL_VIDEODRIVER");
      if (!string.IsNullOrEmpty(videoDriver) &&
          (videoDriver.Equals("dummy", StringComparison.OrdinalIgnoreCase) ||
           videoDriver.Equals("offscreen", StringComparison.OrdinalIgnoreCase)))
      {
        return true;
      }

      return false;
    }

    private static string GetFormattedPlatformString()
    {
      string? osName;
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        osName = "Windows";
      }
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
      {
        osName = "Linux";
      }
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
      {
        osName = "macOS";
      }
      else
      {
        osName = RuntimeInformation.OSDescription;
      }

      return $"Platform: {osName} {RuntimeInformation.OSArchitecture}";
    }
  }
}
