// Namespace for all public Night Engine APIs
namespace Night;

using System;

using Night.Types; // For KeyCode, MouseButton, Color, Rectangle, WindowFlags, Sprite

using static SDL3.SDL; // For direct access to SDL functions

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
        Console.WriteLine($"Error initializing SDL video subsystem: {SDL_GetError()}");
        // Consider throwing an exception or returning a bool for failure
        return;
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
      Console.WriteLine($"Error creating SDL window: {SDL_GetError()}");
      _isWindowOpen = false; // Window creation failed
      // Consider throwing an exception or returning a bool for failure
      return;
    }

    // Create a renderer. Passing null for the name lets SDL choose the best available driver.
    // Hardware acceleration is generally preferred and often default.
    // VSync (PRESENTVSYNC) would typically be set via renderer properties in SDL3 if not default.
    // For simplicity in this step, we use the basic SDL_CreateRenderer.
    _renderer = SDL_CreateRenderer(_window, null);
    if (_renderer == nint.Zero)
    {
      Console.WriteLine($"Error creating SDL renderer: {SDL_GetError()}");
      // Clean up window if renderer creation fails
      SDL_DestroyWindow(_window);
      _window = nint.Zero;
      _isWindowOpen = false; // Renderer creation failed, so window is not usable
      // Consider throwing an exception or returning a bool for failure
      return;
    }
    _isWindowOpen = true; // Window and renderer successfully created

    // TODO: Task 3.3 - Decide on a more robust error strategy (e.g., throw an exception, return a boolean).
    // For now, errors are logged to the console.
  }

  /// <summary>
  /// Sets the title of the window.
  /// </summary>
  /// <param name="title">The new window title.</param>
  public static void SetTitle(string title)
  {
    if (_window == nint.Zero)
    {
      Console.WriteLine("Error in Night.Window.SetTitle: Window handle is null. Was SetMode called successfully?");
      return;
    }
    SDL_SetWindowTitle(_window, title);
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

/// <summary>
/// Provides functionality for drawing graphics.
/// Mimics Love2D's love.graphics module.
/// </summary>
public static class Graphics
{
  /// <summary>
  /// Creates a new image (Sprite) from a file.
  /// </summary>
  /// <param name="filePath">The path to the image file.</param>
  /// <returns>A new Sprite object.</returns>
  public static Sprite NewImage(string filePath)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Draws a sprite to the screen.
  /// </summary>
  /// <param name="sprite">The sprite to draw.</param>
  /// <param name="x">The x-coordinate to draw the sprite at.</param>
  /// <param name="y">The y-coordinate to draw the sprite at.</param>
  /// <param name="rotation">The rotation of the sprite (in radians).</param>
  /// <param name="scaleX">The horizontal scale factor.</param>
  /// <param name="scaleY">The vertical scale factor.</param>
  /// <param name="offsetX">The x-offset for the sprite's origin.</param>
  /// <param name="offsetY">The y-offset for the sprite's origin.</param>
  public static void Draw(
      Sprite sprite,
      float x,
      float y,
      float rotation = 0,
      float scaleX = 1,
      float scaleY = 1,
      float offsetX = 0,
      float offsetY = 0)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Clears the screen to a specific color.
  /// </summary>
  /// <param name="color">The color to clear the screen with.</param>
  public static void Clear(Color color)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Presents the drawn graphics to the screen (swaps buffers).
  /// </summary>
  public static void Present()
  {
    throw new NotImplementedException();
  }
}

/// <summary>
/// Provides functionality for handling keyboard input.
/// Mimics Love2D's love.keyboard module.
/// </summary>
public static class Keyboard
{
  /// <summary>
  /// Checks if a specific key is currently pressed down.
  /// </summary>
  /// <param name="key">The key to check.</param>
  /// <returns>True if the key is down, false otherwise.</returns>
  public static bool IsDown(KeyCode key)
  {
    throw new NotImplementedException();
  }
}

/// <summary>
/// Provides functionality for handling mouse input.
/// Mimics Love2D's love.mouse module.
/// </summary>
public static class Mouse
{
  /// <summary>
  /// Checks if a specific mouse button is currently pressed down.
  /// </summary>
  /// <param name="button">The mouse button to check.</param>
  /// <returns>True if the button is down, false otherwise.</returns>
  public static bool IsDown(MouseButton button)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Gets the current position of the mouse cursor.
  /// </summary>
  /// <returns>A tuple (int x, int y) representing the mouse coordinates.</returns>
  public static (int x, int y) GetPosition()
  {
    throw new NotImplementedException();
  }
}

/// <summary>
/// Provides direct access to SDL3 functions using SDL3-CS bindings.
/// This is an internal-facing or low-level API for the engine.
/// </summary>
public static class SDL
{
  /// <summary>
  /// Initializes the SDL library. This must be called before any other SDL functions.
  /// </summary>
  /// <param name="flags">Initialization flags for SDL.</param>
  /// <returns>0 on success or a negative error code on failure.</returns>
  public static int Init(SDL3.SDL.SDL_InitFlags flags)
  {
    // SDL3.SDL.SDL_Init returns an SDLBool, which implicitly converts to bool.
    // We convert this to 0 for success, <0 for failure.
    return SDL3.SDL.SDL_Init(flags) ? 0 : -1;
  }

  /// <summary>
  /// Cleans up all initialized subsystems. You should call this function on application exit.
  /// </summary>
  public static void Quit()
  {
    SDL3.SDL.SDL_Quit();
  }

  /// <summary>
  /// Gets the version of SDL that is linked against.
  /// The SDL3-CS binding for SDL_GetVersion returns a packed int.
  /// </summary>
  /// <returns>A string representing the SDL version "major.minor.patch".</returns>
  public static string GetVersion()
  {
    int sdl_version = SDL3.SDL.SDL_GetVersion();
    int major = sdl_version / 1000000;
    int minor = (sdl_version / 1000) % 1000;
    int patch = sdl_version % 1000;
    return $"{major}.{minor}.{patch}";
  }

  // Expose SDL_InitFlags enum for convenience if needed by calling code for Init()
  public static SDL3.SDL.SDL_InitFlags InitVideo => SDL3.SDL.SDL_InitFlags.SDL_INIT_VIDEO;
  public static SDL3.SDL.SDL_InitFlags InitAudio => SDL3.SDL.SDL_InitFlags.SDL_INIT_AUDIO;
  public static SDL3.SDL.SDL_InitFlags InitTimer => SDL3.SDL.SDL_InitFlags.SDL_INIT_TIMER;
  public static SDL3.SDL.SDL_InitFlags InitEvents => SDL3.SDL.SDL_InitFlags.SDL_INIT_EVENTS;
  // Add other flags as needed or expect the caller to use SDL3.SDL.SDL_InitFlags directly.
}
