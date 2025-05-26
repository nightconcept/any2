// Copyright (c) 2025 Night Engine Contributors
// Distributed under the MIT license. See LICENSE for details.

// Night Engine Data Structures

namespace Night.Types
{
  /// <summary>
  /// Represents keyboard keys. Values correspond to SDL_Scancode.
  /// </summary>
  /// <remarks>
  /// See SDL_scancode.h for the full list of SDL_Scancode values.
  /// This enum maps directly to SDL_Scancode values.
  /// </remarks>
  public enum KeyCode
  {
    Unknown = 0, // SDL.Scancode.Unknown

    // Letters
    A = 4,  // SDL.Scancode.A
    B = 5,  // SDL.Scancode.B
    C = 6,  // SDL.Scancode.C
    D = 7,  // SDL.Scancode.D
    E = 8,  // SDL.Scancode.E
    F = 9,  // SDL.Scancode.F
    G = 10, // SDL.Scancode.G
    H = 11, // SDL.Scancode.H
    I = 12, // SDL.Scancode.I
    J = 13, // SDL.Scancode.J
    K = 14, // SDL.Scancode.K
    L = 15, // SDL.Scancode.L
    M = 16, // SDL.Scancode.M
    N = 17, // SDL.Scancode.N
    O = 18, // SDL.Scancode.O
    P = 19, // SDL.Scancode.P
    Q = 20, // SDL.Scancode.Q
    R = 21, // SDL.Scancode.R
    S = 22, // SDL.Scancode.S
    T = 23, // SDL.Scancode.T
    U = 24, // SDL.Scancode.U
    V = 25, // SDL.Scancode.V
    W = 26, // SDL.Scancode.W
    X = 27, // SDL.Scancode.X
    Y = 28, // SDL.Scancode.Y
    Z = 29, // SDL.Scancode.Z

    // Numbers (Top row)
    Alpha1 = 30, // SDL.Scancode.Alpha1 (Formerly Num1)
    Alpha2 = 31, // SDL.Scancode.Alpha2 (Formerly Num2)
    Alpha3 = 32, // SDL.Scancode.Alpha3 (Formerly Num3)
    Alpha4 = 33, // SDL.Scancode.Alpha4 (Formerly Num4)
    Alpha5 = 34, // SDL.Scancode.Alpha5 (Formerly Num5)
    Alpha6 = 35, // SDL.Scancode.Alpha6 (Formerly Num6)
    Alpha7 = 36, // SDL.Scancode.Alpha7 (Formerly Num7)
    Alpha8 = 37, // SDL.Scancode.Alpha8 (Formerly Num8)
    Alpha9 = 38, // SDL.Scancode.Alpha9 (Formerly Num9)
    Alpha0 = 39, // SDL.Scancode.Alpha0 (Formerly Num0)

    // Control keys
    Return = 40,       // SDL.Scancode.Return (Formerly Enter)
    Escape = 41,      // SDL.Scancode.Escape
    Backspace = 42,   // SDL.Scancode.Backspace
    Tab = 43,         // SDL.Scancode.Tab
    Space = 44,       // SDL.Scancode.Space
    Minus = 45,       // SDL.Scancode.Minus
    Equals = 46,      // SDL.Scancode.Equals
    Leftbracket = 47, // SDL.Scancode.Leftbracket (Formerly LeftBracket)
    Rightbracket = 48,// SDL.Scancode.Rightbracket (Formerly RightBracket)
    Backslash = 49,   // SDL.Scancode.Backslash
    NonUshash = 50,   // SDL.Scancode.NonUshash (Formerly NonUsHash)
    Semicolon = 51,   // SDL.Scancode.Semicolon
    Apostrophe = 52,  // SDL.Scancode.Apostrophe
    Grave = 53,       // SDL.Scancode.Grave
    Comma = 54,       // SDL.Scancode.Comma
    Period = 55,      // SDL.Scancode.Period
    Slash = 56,       // SDL.Scancode.Slash
    Capslock = 57,    // SDL.Scancode.Capslock (Formerly CapsLock)

    // Function keys
    F1 = 58,  // SDL.Scancode.F1
    F2 = 59,  // SDL.Scancode.F2
    F3 = 60,  // SDL.Scancode.F3
    F4 = 61,  // SDL.Scancode.F4
    F5 = 62,  // SDL.Scancode.F5
    F6 = 63,  // SDL.Scancode.F6
    F7 = 64,  // SDL.Scancode.F7
    F8 = 65,  // SDL.Scancode.F8
    F9 = 66,  // SDL.Scancode.F9
    F10 = 67, // SDL.Scancode.F10
    F11 = 68, // SDL.Scancode.F11
    F12 = 69, // SDL.Scancode.F12

    Printscreen = 70, // SDL.Scancode.Printscreen (Formerly PrintScreen)
    Scrolllock = 71,  // SDL.Scancode.Scrolllock (Formerly ScrollLock)
    Pause = 72,       // SDL.Scancode.Pause
    Insert = 73,      // SDL.Scancode.Insert
    Home = 74,        // SDL.Scancode.Home
    Pageup = 75,      // SDL.Scancode.Pageup (Formerly PageUp)
    Delete = 76,      // SDL.Scancode.Delete
    End = 77,         // SDL.Scancode.End
    Pagedown = 78,    // SDL.Scancode.Pagedown (Formerly PageDown)

    // Arrow keys
    Right = 79, // SDL.Scancode.Right
    Left = 80,  // SDL.Scancode.Left
    Down = 81,  // SDL.Scancode.Down
    Up = 82,    // SDL.Scancode.Up

    NumlockClear = 83, // SDL.Scancode.NumlockClear (Formerly NumLockClear)

    // Keypad
    KpDivide = 84,   // SDL.Scancode.KpDivide
    KpMultiply = 85, // SDL.Scancode.KpMultiply
    KpMinus = 86,    // SDL.Scancode.KpMinus
    KpPlus = 87,     // SDL.Scancode.KpPlus
    KpEnter = 88,    // SDL.Scancode.KpEnter
    Kp1 = 89,        // SDL.Scancode.Kp1
    Kp2 = 90,        // SDL.Scancode.Kp2
    Kp3 = 91,        // SDL.Scancode.Kp3
    Kp4 = 92,        // SDL.Scancode.Kp4
    Kp5 = 93,        // SDL.Scancode.Kp5
    Kp6 = 94,        // SDL.Scancode.Kp6
    Kp7 = 95,        // SDL.Scancode.Kp7
    Kp8 = 96,        // SDL.Scancode.Kp8
    Kp9 = 97,        // SDL.Scancode.Kp9
    Kp0 = 98,        // SDL.Scancode.Kp0
    KpPeriod = 99,   // SDL.Scancode.KpPeriod

    NonUsbackslash = 100, // SDL.Scancode.NonUsbackslash (Formerly NonUsBackslash)
    Application = 101,    // SDL.Scancode.Application
    Power = 102,          // SDL.Scancode.Power

    KpEquals = 103, // SDL.Scancode.KpEquals
    F13 = 104,      // SDL.Scancode.F13
    F14 = 105,      // SDL.Scancode.F14
    F15 = 106,      // SDL.Scancode.F15
    F16 = 107,      // SDL.Scancode.F16
    F17 = 108,      // SDL.Scancode.F17
    F18 = 109,      // SDL.Scancode.F18
    F19 = 110,      // SDL.Scancode.F19
    F20 = 111,      // SDL.Scancode.F20
    F21 = 112,      // SDL.Scancode.F21
    F22 = 113,      // SDL.Scancode.F22
    F23 = 114,      // SDL.Scancode.F23
    F24 = 115,      // SDL.Scancode.F24

    Execute = 116, // SDL.Scancode.Execute
    Help = 117,    // SDL.Scancode.Help
    Menu = 118,    // SDL.Scancode.Menu
    Select = 119,  // SDL.Scancode.Select
    Stop = 120,    // SDL.Scancode.Stop
    Again = 121,   // SDL.Scancode.Again
    Undo = 122,    // SDL.Scancode.Undo
    Cut = 123,     // SDL.Scancode.Cut
    Copy = 124,    // SDL.Scancode.Copy
    Paste = 125,   // SDL.Scancode.Paste
    Find = 126,    // SDL.Scancode.Find
    Mute = 127,    // SDL.Scancode.Mute
    VolumeUp = 128,   // SDL.Scancode.VolumeUp
    VolumeDown = 129, // SDL.Scancode.VolumeDown

    // Skipping some less common international and special keys for brevity,
    // but they can be added if needed by referring to SDL.Scancode.

    KpComma = 133, // SDL.Scancode.KpComma

    // Modifier keys
    LCtrl = 224,   // SDL.Scancode.LCtrl (Formerly LeftCtrl)
    LShift = 225,  // SDL.Scancode.LShift (Formerly LeftShift)
    LAlt = 226,    // SDL.Scancode.LAlt (Formerly LeftAlt)
    LGUI = 227,  // SDL.Scancode.LGUI (Formerly LeftSuper)
    RCtrl = 228,  // SDL.Scancode.RCtrl (Formerly RightCtrl)
    RShift = 229, // SDL.Scancode.RShift (Formerly RightShift)
    RAlt = 230,   // SDL.Scancode.RAlt (Formerly RightAlt)
    RGUI = 231, // SDL.Scancode.RGUI (Formerly RightSuper)

    Mode = 257, // SDL.Scancode.Mode

    // Media keys (subset)
    MediaPlay = 262,        // SDL.Scancode.MediaPlay
    MediaPause = 263,       // SDL.Scancode.MediaPause
    MediaRecord = 264,      // SDL.Scancode.MediaRecord
    MediaFastForward = 265, // SDL.Scancode.MediaFastForward
    MediaRewind = 266,      // SDL.Scancode.MediaRewind
    MediaNextTrack = 267,   // SDL.Scancode.MediaNextTrack
    MediaPreviousTrack = 268, // SDL.Scancode.MediaPreviousTrack
    MediaStop = 269,        // SDL.Scancode.MediaStop
    MediaEject = 270,       // SDL.Scancode.MediaEject
    MediaPlayPause = 271,   // SDL.Scancode.MediaPlayPause
    MediaSelect = 272,      // SDL.Scancode.MediaSelect

    // Application control keys (subset)
    AppSearch = 280,     // SDL.Scancode.AppSearch
    AppHome = 281,       // SDL.Scancode.AppHome
    AppBack = 282,       // SDL.Scancode.AppBack
    AppForward = 283,    // SDL.Scancode.AppForward
    AppStop = 284,       // SDL.Scancode.AppStop
    AppRefresh = 285,    // SDL.Scancode.AppRefresh
    AppBookmarks = 286,  // SDL.Scancode.AppBookmarks

    // Total number of scancodes. Not a key itself.
    // SDL.Scancode.Count = 512
  }

  /// <summary>
  /// Represents mouse buttons. Values correspond to SDL3.SDL.Button* constants.
  /// (e.g., Left is 1, Middle is 2, etc.)
  /// </summary>
  public enum MouseButton
  {
    Unknown = 0, // Not a direct SDL button constant
    Left = 1,    // Corresponds to SDL.ButtonLeft
    Middle = 2,  // Corresponds to SDL.ButtonMiddle
    Right = 3,   // Corresponds to SDL.ButtonRight
    X1 = 4,      // Corresponds to SDL.ButtonX1 (Typically "back")
    X2 = 5       // Corresponds to SDL.ButtonX2 (Typically "forward")
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

  // Night.Types.WindowFlags is removed.
  // SDL3.SDL.WindowFlags will be used directly.
  // See epic8.md Task 8.1 and 8.4 notes.

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
