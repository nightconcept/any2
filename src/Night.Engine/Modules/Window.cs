using System;

using Night.Types;

using static SDL3.SDL; // For direct access to SDL functions

namespace Night;

/// <summary>
/// Provides functionality for managing the application window.
/// Mimics Love2D's love.window module.
/// </summary>
public static class Window
{
  private static nint _window = nint.Zero;
  private static nint _renderer = nint.Zero;
  private static bool _isVideoInitialized = false;
  private static bool _isWindowOpen = false; // Added for IsOpen()

  /// <summary>
  /// Sets the display mode of the window.
  /// </summary>
  /// <param name="width">The width of the window.</param>
  /// <param name="height">The height of the window.</param>
  /// <param name="flags">Window flags to apply.</param>
  public static void SetMode(int width, int height, WindowFlags flags)
  {
    if (!_isVideoInitialized)
    {
      if (!SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO)) // Corrected: SDLBool check
      {
        string sdlError = SDL_GetError();
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
        SDL_DestroyRenderer(_renderer);
        _renderer = nint.Zero;
      }
      SDL_DestroyWindow(_window);
      _window = nint.Zero;
      _isWindowOpen = false; // Window closed
    }

    SDL_WindowFlags sdlFlags = (SDL_WindowFlags)flags;

    _window = SDL_CreateWindow("Night Engine", width, height, sdlFlags);
    if (_window == nint.Zero)
    {
      string sdlError = SDL_GetError();
      Console.WriteLine($"Error creating SDL window: {sdlError}");
      _isWindowOpen = false; // Window creation failed
      throw new Exception($"SDL Error creating window: {sdlError}");
    }

    // Create a renderer. Passing null for the name lets SDL choose the best available driver.
    // Hardware acceleration is generally preferred and often default.
    // VSync (PRESENTVSYNC) would typically be set via renderer properties in SDL3 if not default.
    // For simplicity in this step, we use the basic SDL_CreateRenderer.
    _renderer = SDL_CreateRenderer(_window, null);
    if (_renderer == nint.Zero)
    {
      string sdlError = SDL_GetError();
      Console.WriteLine($"Error creating SDL renderer: {sdlError}");
      // Clean up window if renderer creation fails
      SDL_DestroyWindow(_window);
      _window = nint.Zero;
      _isWindowOpen = false; // Renderer creation failed, so window is not usable
      throw new Exception($"SDL Error creating renderer: {sdlError}");
    }
    _isWindowOpen = true; // Window and renderer successfully created
  }

  /// <summary>
  /// Sets the title of the window.
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
    if (!SDL_SetWindowTitle(_window, title))
    {
      string sdlError = SDL_GetError();
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
    // Or could be SDL_DestroyWindow(_window); _window = nint.Zero; if Engine.Run doesn't own it.
  }
}
