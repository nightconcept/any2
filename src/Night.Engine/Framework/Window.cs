using System;

using SDL3;

namespace Night;

/// <summary>
/// Provides an interface for modifying and retrieving information about the program's window.
/// </summary>
public static class Window
{
  private static nint _window = nint.Zero;
  private static nint _renderer = nint.Zero;
  private static bool _isVideoInitialized = false;
  private static bool _isWindowOpen = false;

  // Internal accessor for the renderer, to be used by Night.Graphics
  internal static nint RendererPtr => _renderer;


  /// <summary>
  ///  	Sets the display mode and properties of the window.
  /// </summary>
  /// <param name="width">The width of the window.</param>
  /// <param name="height">The height of the window.</param>
  /// <param name="flags">SDL Window flags to apply.</param>
  public static void SetMode(int width, int height, SDL.WindowFlags flags)
  {
    if (!_isVideoInitialized)
    {
      if (!SDL.InitSubSystem(SDL.InitFlags.Video)) // Use SDL.InitSubSystem and SDL.InitFlags
      {
        string sdlError = SDL.GetError();
        Console.WriteLine($"Error initializing SDL video subsystem: {sdlError}");
        throw new Exception($"SDL Error initializing video subsystem: {sdlError}");
      }
      _isVideoInitialized = true;
    }

    // If a window already exists, destroy it before creating a new one
    if (_window != nint.Zero)
    {
      if (_renderer != nint.Zero)
      {
        SDL.DestroyRenderer(_renderer);
        _renderer = nint.Zero;
      }
      SDL.DestroyWindow(_window);
      _window = nint.Zero;
      _isWindowOpen = false;
    }

    _window = SDL.CreateWindow("Night Engine", width, height, flags); // Use flags directly
    if (_window == nint.Zero)
    {
      string sdlError = SDL.GetError();
      Console.WriteLine($"Error creating SDL window: {sdlError}");
      _isWindowOpen = false; // Window creation failed
      throw new Exception($"SDL Error creating window: {sdlError}");
    }

    // Create a renderer. Passing null for the name lets SDL choose the best available driver.
    // Hardware acceleration is generally preferred and often default.
    // VSync (PRESENTVSYNC) would typically be set via renderer properties in SDL3 if not default.
    _renderer = SDL.CreateRenderer(_window, null); // Pass null for driver name
    if (_renderer == nint.Zero)
    {
      string sdlError = SDL.GetError();
      Console.WriteLine($"Error creating SDL renderer: {sdlError}");
      // Clean up window if renderer creation fails
      SDL.DestroyWindow(_window);
      _window = nint.Zero;
      _isWindowOpen = false; // Renderer creation failed, so window is not usable
      throw new Exception($"SDL Error creating renderer: {sdlError}");
    }
    _isWindowOpen = true; // Window and renderer successfully created
  }

  /// <summary>
  /// Sets the window title.
  /// </summary>
  /// <param name="title">The new window title.</param>
  public static void SetTitle(string title)
  {
    if (_window == nint.Zero)
    {
      string errorMsg = "Error in Night.Window.SetTitle: Window handle is null. Was SetMode called successfully?";
      Console.WriteLine(errorMsg);
      throw new InvalidOperationException(errorMsg);
    }
    if (!SDL.SetWindowTitle(_window, title))
    {
      string sdlError = SDL.GetError();
      Console.WriteLine($"Error in Night.Window.SetTitle: {sdlError}");
      throw new Exception($"SDL Error in Night.Window.SetTitle: {sdlError}");
    }
  }

  /// <summary>
  /// Checks if the window is currently open.
  /// </summary>
  /// <returns>True if the window is open, false otherwise.</returns>
  public static bool IsOpen()
  {
    return _isWindowOpen && _window != nint.Zero;
  }

  /// <summary>
  /// Signals that the window should close.
  /// This is typically called by the engine when a quit event is received.
  /// </summary>
  public static void Close()
  {
    _isWindowOpen = false;
    // Actual window destruction is handled by SetMode re-call or application exit for now.
    // Or could be SDL.DestroyWindow(_window); _window = nint.Zero; if Engine.Run doesn't own it.
  }

  /// <summary>
  /// Internal method to shut down the window and renderer, and quit the video subsystem.
  /// Should be called by the FrameworkLoop at the end of the application.
  /// </summary>
  internal static void Shutdown()
  {
    if (_renderer != nint.Zero)
    {
      SDL.DestroyRenderer(_renderer);
      _renderer = nint.Zero;
    }
    if (_window != nint.Zero)
    {
      SDL.DestroyWindow(_window);
      _window = nint.Zero;
    }
    if (_isVideoInitialized)
    {
      SDL.QuitSubSystem(SDL.InitFlags.Video);
      _isVideoInitialized = false;
    }
    _isWindowOpen = false;
    Console.WriteLine("Night.Window: Shutdown complete.");
  }
}
