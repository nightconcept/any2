// Copyright (c) 2025 Night Engine Contributors
// Distributed under the MIT license. See LICENSE for details.

// TODO: Expand KeyCode with more comprehensive SDL key codes.
// Night Engine Data Structures

namespace Night.Types
{
  /// <summary>
  /// Represents keyboard keys.
  /// </summary>
  /// <remarks>
  /// This is a preliminary list and will be expanded based on SDL3 key codes.
  /// </remarks>
  public enum KeyCode
  {
    Unknown,

    // Letters
    A, B, C, D, E, F, G, H, I, J, K, L, M,
    N, O, P, Q, R, S, T, U, V, W, X, Y, Z,

    // Numbers
    Num0, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9,

    // Function keys
    F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,

    // Control keys
    LeftShift, RightShift, LeftControl, RightControl,
    LeftAlt, RightAlt, LeftSuper, RightSuper, // Super key (Windows/Command key)
    Enter, Escape, Space, Tab, Backspace, Delete, Insert,
    Home, End, PageUp, PageDown,

    // Arrow keys
    Up, Down, Left, Right,
  }

  /// <summary>
  /// Represents mouse buttons.
  /// </summary>
  public enum MouseButton
  {
    Unknown,
    Left,
    Middle,
    Right,
    X1, // Typically the first extra mouse button (e.g., "back")
    X2  // Typically the second extra mouse button (e.g., "forward")
  }

  /// <summary>
  /// Represents a color with Red, Green, Blue, and Alpha components.
  /// </summary>
  public struct Color
  {
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public Color(byte r, byte g, byte b, byte a = 255)
    {
      R = r;
      G = g;
      B = b;
      A = a;
    }

    // Common color presets
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color White = new(255, 255, 255);
    public static readonly Color Red = new(255, 0, 0);
    public static readonly Color Green = new(0, 255, 0);
    public static readonly Color Blue = new(0, 0, 255);
    public static readonly Color Yellow = new(255, 255, 0);
    public static readonly Color Magenta = new(255, 0, 255);
    public static readonly Color Cyan = new(0, 255, 255);
    public static readonly Color Transparent = new(0, 0, 0, 0);
  }

  /// <summary>
  /// Represents a rectangle with position (X, Y) and dimensions (Width, Height).
  /// </summary>
  public struct Rectangle
  {
    public int X;
    public int Y;
    public int Width;
    public int Height;

    public Rectangle(int x, int y, int width, int height)
    {
      X = x;
      Y = y;
      Width = width;
      Height = height;
    }
  }

  /// <summary>
  /// Specifies window creation flags. Values correspond to SDL_WindowFlags.
  /// </summary>
  /// <remarks>
  /// See SDL_video.h for the full list of SDL_WindowFlags.
  /// </remarks>
  [System.Flags]
  public enum WindowFlags : uint
  {
    None = 0,
    Fullscreen = 0x00000001U,    // SDL_WINDOW_FULLSCREEN
    OpenGL = 0x00000002U,        // SDL_WINDOW_OPENGL
    Shown = 0x00000004U,         // SDL_WINDOW_SHOWN
    Hidden = 0x00000008U,        // SDL_WINDOW_HIDDEN
    Borderless = 0x00000010U,    // SDL_WINDOW_BORDERLESS
    Resizable = 0x00000020U,     // SDL_WINDOW_RESIZABLE
    Minimized = 0x00000040U,     // SDL_WINDOW_MINIMIZED
    Maximized = 0x00000080U,     // SDL_WINDOW_MAXIMIZED
                                 // MouseGrabbed = 0x00000100U, // SDL_WINDOW_MOUSE_GRABBED (Consider if needed for Night API)
                                 // InputFocus = 0x00000200U,   // SDL_WINDOW_INPUT_FOCUS (Managed by SDL)
                                 // MouseFocus = 0x00000400U,   // SDL_WINDOW_MOUSE_FOCUS (Managed by SDL)
    HighDpi = 0x00002000U,       // SDL_WINDOW_HIGH_PIXEL_DENSITY
                                 // MouseCapture = 0x00004000U, // SDL_WINDOW_MOUSE_CAPTURE (Consider if needed for Night API)
                                 // AlwaysOnTop = 0x00008000U,  // SDL_WINDOW_ALWAYS_ON_TOP (Consider if useful)
    Vulkan = 0x10000000U,        // SDL_WINDOW_VULKAN
    Metal = 0x20000000U,         // SDL_WINDOW_METAL
                                 // Transparent = 0x40000000U, // SDL_WINDOW_TRANSPARENT (Consider if useful)
                                 // NotFocusable = 0x80000000U // SDL_WINDOW_NOT_FOCUSABLE (Consider if useful)
  }

  /// <summary>
  /// Represents a 2D sprite.
  /// </summary>
  /// <remarks>
  /// This is a placeholder structure. It will be expanded in Epic 5.
  /// It might include texture references, source rectangles, etc.
  /// </remarks>
  public class Sprite
  {
    // Placeholder for texture identifier (e.g., an IntPtr or a managed object)
    // internal object? _textureHandle; // Example, to be refined

    // Placeholder for dimensions, if not derived from texture
    // public int Width { get; internal set; }
    // public int Height { get; internal set; }

    // Placeholder for source rectangle within a texture atlas
    // public Rectangle? SourceRectangle { get; internal set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sprite"/> class.
    /// </summary>
    /// <remarks>
    /// Constructor is internal or protected if creation is managed by the engine.
    /// For now, public for flexibility during early prototyping.
    /// </remarks>
    public Sprite()
    {
      // Initialization logic will be added in Epic 5.
    }
  }

  /// <summary>
  /// Interface for a game that can be run by the Night Engine.
  /// Game developers will implement this interface in their main game class.
  /// </summary>
  public interface IGame
  {
    /// <summary>
    /// Called once when the game starts, for loading resources.
    /// </summary>
    void Load();

    /// <summary>
    /// Called repeatedly every frame, for updating game logic.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last frame, in seconds.</param>
    void Update(double deltaTime);

    /// <summary>
    /// Called repeatedly every frame, for drawing the game state.
    /// </summary>
    void Draw();

    // Optional input handlers can be added here later as per PRD Feature 4.
    // For example:
    // void KeyPressed(KeyCode key, bool isRepeat);
    // void MousePressed(int x, int y, MouseButton button, int presses);
  }
}
