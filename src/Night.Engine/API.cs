// Namespace for all public Night Engine APIs
namespace Night;

using System;
using System.Runtime.InteropServices; // For Marshal

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
  // TODO: Task 4.4 - Create a comprehensive mapping for all KeyCodes.
  private static SDL_Scancode MapKeyCodeToScancode(KeyCode key)
  {
    switch (key)
    {
      // Letters
      case KeyCode.A: return SDL_Scancode.SDL_SCANCODE_A;
      case KeyCode.B: return SDL_Scancode.SDL_SCANCODE_B;
      case KeyCode.C: return SDL_Scancode.SDL_SCANCODE_C;
      case KeyCode.D: return SDL_Scancode.SDL_SCANCODE_D;
      case KeyCode.E: return SDL_Scancode.SDL_SCANCODE_E;
      case KeyCode.F: return SDL_Scancode.SDL_SCANCODE_F;
      case KeyCode.G: return SDL_Scancode.SDL_SCANCODE_G;
      case KeyCode.H: return SDL_Scancode.SDL_SCANCODE_H;
      case KeyCode.I: return SDL_Scancode.SDL_SCANCODE_I;
      case KeyCode.J: return SDL_Scancode.SDL_SCANCODE_J;
      case KeyCode.K: return SDL_Scancode.SDL_SCANCODE_K;
      case KeyCode.L: return SDL_Scancode.SDL_SCANCODE_L;
      case KeyCode.M: return SDL_Scancode.SDL_SCANCODE_M;
      case KeyCode.N: return SDL_Scancode.SDL_SCANCODE_N;
      case KeyCode.O: return SDL_Scancode.SDL_SCANCODE_O;
      case KeyCode.P: return SDL_Scancode.SDL_SCANCODE_P;
      case KeyCode.Q: return SDL_Scancode.SDL_SCANCODE_Q;
      case KeyCode.R: return SDL_Scancode.SDL_SCANCODE_R;
      case KeyCode.S: return SDL_Scancode.SDL_SCANCODE_S;
      case KeyCode.T: return SDL_Scancode.SDL_SCANCODE_T;
      case KeyCode.U: return SDL_Scancode.SDL_SCANCODE_U;
      case KeyCode.V: return SDL_Scancode.SDL_SCANCODE_V;
      case KeyCode.W: return SDL_Scancode.SDL_SCANCODE_W;
      case KeyCode.X: return SDL_Scancode.SDL_SCANCODE_X;
      case KeyCode.Y: return SDL_Scancode.SDL_SCANCODE_Y;
      case KeyCode.Z: return SDL_Scancode.SDL_SCANCODE_Z;

      // Numbers
      case KeyCode.Num0: return SDL_Scancode.SDL_SCANCODE_0;
      case KeyCode.Num1: return SDL_Scancode.SDL_SCANCODE_1;
      case KeyCode.Num2: return SDL_Scancode.SDL_SCANCODE_2;
      case KeyCode.Num3: return SDL_Scancode.SDL_SCANCODE_3;
      case KeyCode.Num4: return SDL_Scancode.SDL_SCANCODE_4;
      case KeyCode.Num5: return SDL_Scancode.SDL_SCANCODE_5;
      case KeyCode.Num6: return SDL_Scancode.SDL_SCANCODE_6;
      case KeyCode.Num7: return SDL_Scancode.SDL_SCANCODE_7;
      case KeyCode.Num8: return SDL_Scancode.SDL_SCANCODE_8;
      case KeyCode.Num9: return SDL_Scancode.SDL_SCANCODE_9;

      // Function keys
      case KeyCode.F1: return SDL_Scancode.SDL_SCANCODE_F1;
      case KeyCode.F2: return SDL_Scancode.SDL_SCANCODE_F2;
      case KeyCode.F3: return SDL_Scancode.SDL_SCANCODE_F3;
      case KeyCode.F4: return SDL_Scancode.SDL_SCANCODE_F4;
      case KeyCode.F5: return SDL_Scancode.SDL_SCANCODE_F5;
      case KeyCode.F6: return SDL_Scancode.SDL_SCANCODE_F6;
      case KeyCode.F7: return SDL_Scancode.SDL_SCANCODE_F7;
      case KeyCode.F8: return SDL_Scancode.SDL_SCANCODE_F8;
      case KeyCode.F9: return SDL_Scancode.SDL_SCANCODE_F9;
      case KeyCode.F10: return SDL_Scancode.SDL_SCANCODE_F10;
      case KeyCode.F11: return SDL_Scancode.SDL_SCANCODE_F11;
      case KeyCode.F12: return SDL_Scancode.SDL_SCANCODE_F12;

      // Control keys
      case KeyCode.LeftShift: return SDL_Scancode.SDL_SCANCODE_LSHIFT;
      case KeyCode.RightShift: return SDL_Scancode.SDL_SCANCODE_RSHIFT;
      case KeyCode.LeftControl: return SDL_Scancode.SDL_SCANCODE_LCTRL;
      case KeyCode.RightControl: return SDL_Scancode.SDL_SCANCODE_RCTRL;
      case KeyCode.LeftAlt: return SDL_Scancode.SDL_SCANCODE_LALT;
      case KeyCode.RightAlt: return SDL_Scancode.SDL_SCANCODE_RALT;
      case KeyCode.LeftSuper: return SDL_Scancode.SDL_SCANCODE_LGUI; // GUI key often maps to Super/Windows/Command
      case KeyCode.RightSuper: return SDL_Scancode.SDL_SCANCODE_RGUI;
      case KeyCode.Enter: return SDL_Scancode.SDL_SCANCODE_RETURN;
      case KeyCode.Escape: return SDL_Scancode.SDL_SCANCODE_ESCAPE;
      case KeyCode.Space: return SDL_Scancode.SDL_SCANCODE_SPACE;
      case KeyCode.Tab: return SDL_Scancode.SDL_SCANCODE_TAB;
      case KeyCode.Backspace: return SDL_Scancode.SDL_SCANCODE_BACKSPACE;
      case KeyCode.Delete: return SDL_Scancode.SDL_SCANCODE_DELETE;
      case KeyCode.Insert: return SDL_Scancode.SDL_SCANCODE_INSERT;
      case KeyCode.Home: return SDL_Scancode.SDL_SCANCODE_HOME;
      case KeyCode.End: return SDL_Scancode.SDL_SCANCODE_END;
      case KeyCode.PageUp: return SDL_Scancode.SDL_SCANCODE_PAGEUP;
      case KeyCode.PageDown: return SDL_Scancode.SDL_SCANCODE_PAGEDOWN;

      // Arrow keys
      case KeyCode.Up: return SDL_Scancode.SDL_SCANCODE_UP;
      case KeyCode.Down: return SDL_Scancode.SDL_SCANCODE_DOWN;
      case KeyCode.Left: return SDL_Scancode.SDL_SCANCODE_LEFT;
      case KeyCode.Right: return SDL_Scancode.SDL_SCANCODE_RIGHT;

      case KeyCode.Unknown:
      default:
        return SDL_Scancode.SDL_SCANCODE_UNKNOWN;
    }
  }

  /// <summary>
  /// Checks if a specific key is currently pressed down.
  /// </summary>
  /// <param name="key">The key to check.</param>
  /// <returns>True if the key is down, false otherwise.</returns>
  public static bool IsDown(KeyCode key)
  {
    // The SDL_PumpEvents function should be called in the main event loop
    // to update the keyboard state. Assuming Engine.Run() handles this.

    nint keyboardStatePtr = SDL_GetKeyboardState(out int numKeys);

    if (keyboardStatePtr == nint.Zero)
    {
      // This case should ideally not happen if SDL is initialized correctly.
      // Log a warning or handle as per engine's error strategy.
      Console.WriteLine("Warning: SDL_GetKeyboardState returned a null pointer.");
      return false;
    }

    SDL_Scancode sdlScancode = MapKeyCodeToScancode(key);

    if (sdlScancode == SDL_Scancode.SDL_SCANCODE_UNKNOWN)
    {
      return false; // Unknown or unmapped key
    }

    // Ensure scancode is within bounds. numKeys represents the number of scancodes.
    if ((int)sdlScancode >= numKeys)
    {
      Console.WriteLine($"Warning: Scancode {(int)sdlScancode} is out of bounds (numKeys: {numKeys}).");
      return false;
    }

    // The array is indexed by SDL_Scancode values.
    // SDL_GetKeyboardState returns a pointer to an array of Uint8.
    // A value of 1 means pressed, 0 means not pressed.
    byte byteKeyState = Marshal.ReadByte(keyboardStatePtr, (int)sdlScancode);
    return byteKeyState == 1;
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
    // SDL_PumpEvents should be called in the main event loop.
    // Assuming Night.Engine.Run() handles this.

    SDL_MouseButtonFlags mouseState = SDL_GetMouseState(out float _, out float _);

    SDL_MouseButtonFlags buttonMask;
    switch (button)
    {
      case MouseButton.Left:
        buttonMask = SDL_MouseButtonFlags.SDL_BUTTON_LMASK;
        break;
      case MouseButton.Middle:
        buttonMask = SDL_MouseButtonFlags.SDL_BUTTON_MMASK;
        break;
      case MouseButton.Right:
        buttonMask = SDL_MouseButtonFlags.SDL_BUTTON_RMASK;
        break;
      case MouseButton.X1:
        buttonMask = SDL_MouseButtonFlags.SDL_BUTTON_X1MASK;
        break;
      case MouseButton.X2:
        buttonMask = SDL_MouseButtonFlags.SDL_BUTTON_X2MASK;
        break;
      case MouseButton.Unknown:
      default:
        return false; // Unknown or unmapped button
    }

    return (mouseState & buttonMask) != 0;
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
