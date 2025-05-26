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
    Unknown = 0, // SDL_SCANCODE_UNKNOWN

    // Letters
    A = 4,  // SDL_SCANCODE_A
    B = 5,  // SDL_SCANCODE_B
    C = 6,  // SDL_SCANCODE_C
    D = 7,  // SDL_SCANCODE_D
    E = 8,  // SDL_SCANCODE_E
    F = 9,  // SDL_SCANCODE_F
    G = 10, // SDL_SCANCODE_G
    H = 11, // SDL_SCANCODE_H
    I = 12, // SDL_SCANCODE_I
    J = 13, // SDL_SCANCODE_J
    K = 14, // SDL_SCANCODE_K
    L = 15, // SDL_SCANCODE_L
    M = 16, // SDL_SCANCODE_M
    N = 17, // SDL_SCANCODE_N
    O = 18, // SDL_SCANCODE_O
    P = 19, // SDL_SCANCODE_P
    Q = 20, // SDL_SCANCODE_Q
    R = 21, // SDL_SCANCODE_R
    S = 22, // SDL_SCANCODE_S
    T = 23, // SDL_SCANCODE_T
    U = 24, // SDL_SCANCODE_U
    V = 25, // SDL_SCANCODE_V
    W = 26, // SDL_SCANCODE_W
    X = 27, // SDL_SCANCODE_X
    Y = 28, // SDL_SCANCODE_Y
    Z = 29, // SDL_SCANCODE_Z

    // Numbers (Top row)
    Num1 = 30, // SDL_SCANCODE_1
    Num2 = 31, // SDL_SCANCODE_2
    Num3 = 32, // SDL_SCANCODE_3
    Num4 = 33, // SDL_SCANCODE_4
    Num5 = 34, // SDL_SCANCODE_5
    Num6 = 35, // SDL_SCANCODE_6
    Num7 = 36, // SDL_SCANCODE_7
    Num8 = 37, // SDL_SCANCODE_8
    Num9 = 38, // SDL_SCANCODE_9
    Num0 = 39, // SDL_SCANCODE_0

    // Control keys
    Enter = 40,       // SDL_SCANCODE_RETURN
    Escape = 41,      // SDL_SCANCODE_ESCAPE
    Backspace = 42,   // SDL_SCANCODE_BACKSPACE
    Tab = 43,         // SDL_SCANCODE_TAB
    Space = 44,       // SDL_SCANCODE_SPACE
    Minus = 45,       // SDL_SCANCODE_MINUS
    Equals = 46,      // SDL_SCANCODE_EQUALS
    LeftBracket = 47, // SDL_SCANCODE_LEFTBRACKET
    RightBracket = 48,// SDL_SCANCODE_RIGHTBRACKET
    Backslash = 49,   // SDL_SCANCODE_BACKSLASH (Might be #~ for UK keyboards)
    NonUsHash = 50,   // SDL_SCANCODE_NONUSHASH (#~ for Non-US keyboards)
    Semicolon = 51,   // SDL_SCANCODE_SEMICOLON
    Apostrophe = 52,  // SDL_SCANCODE_APOSTROPHE
    Grave = 53,       // SDL_SCANCODE_GRAVE (Accent grave `)
    Comma = 54,       // SDL_SCANCODE_COMMA
    Period = 55,      // SDL_SCANCODE_PERIOD
    Slash = 56,       // SDL_SCANCODE_SLASH
    CapsLock = 57,    // SDL_SCANCODE_CAPSLOCK

    // Function keys
    F1 = 58,  // SDL_SCANCODE_F1
    F2 = 59,  // SDL_SCANCODE_F2
    F3 = 60,  // SDL_SCANCODE_F3
    F4 = 61,  // SDL_SCANCODE_F4
    F5 = 62,  // SDL_SCANCODE_F5
    F6 = 63,  // SDL_SCANCODE_F6
    F7 = 64,  // SDL_SCANCODE_F7
    F8 = 65,  // SDL_SCANCODE_F8
    F9 = 66,  // SDL_SCANCODE_F9
    F10 = 67, // SDL_SCANCODE_F10
    F11 = 68, // SDL_SCANCODE_F11
    F12 = 69, // SDL_SCANCODE_F12

    PrintScreen = 70, // SDL_SCANCODE_PRINTSCREEN
    ScrollLock = 71,  // SDL_SCANCODE_SCROLLLOCK
    Pause = 72,       // SDL_SCANCODE_PAUSE
    Insert = 73,      // SDL_SCANCODE_INSERT
    Home = 74,        // SDL_SCANCODE_HOME
    PageUp = 75,      // SDL_SCANCODE_PAGEUP
    Delete = 76,      // SDL_SCANCODE_DELETE
    End = 77,         // SDL_SCANCODE_END
    PageDown = 78,    // SDL_SCANCODE_PAGEDOWN

    // Arrow keys
    Right = 79, // SDL_SCANCODE_RIGHT
    Left = 80,  // SDL_SCANCODE_LEFT
    Down = 81,  // SDL_SCANCODE_DOWN
    Up = 82,    // SDL_SCANCODE_UP

    NumLockClear = 83, // SDL_SCANCODE_NUMLOCKCLEAR

    // Keypad
    KpDivide = 84,   // SDL_SCANCODE_KP_DIVIDE
    KpMultiply = 85, // SDL_SCANCODE_KP_MULTIPLY
    KpMinus = 86,    // SDL_SCANCODE_KP_MINUS
    KpPlus = 87,     // SDL_SCANCODE_KP_PLUS
    KpEnter = 88,    // SDL_SCANCODE_KP_ENTER
    Kp1 = 89,        // SDL_SCANCODE_KP_1
    Kp2 = 90,        // SDL_SCANCODE_KP_2
    Kp3 = 91,        // SDL_SCANCODE_KP_3
    Kp4 = 92,        // SDL_SCANCODE_KP_4
    Kp5 = 93,        // SDL_SCANCODE_KP_5
    Kp6 = 94,        // SDL_SCANCODE_KP_6
    Kp7 = 95,        // SDL_SCANCODE_KP_7
    Kp8 = 96,        // SDL_SCANCODE_KP_8
    Kp9 = 97,        // SDL_SCANCODE_KP_9
    Kp0 = 98,        // SDL_SCANCODE_KP_0
    KpPeriod = 99,   // SDL_SCANCODE_KP_PERIOD

    NonUsBackslash = 100, // SDL_SCANCODE_NONUSBACKSLASH
    Application = 101,    // SDL_SCANCODE_APPLICATION (Context Menu Key)
    Power = 102,          // SDL_SCANCODE_POWER

    KpEquals = 103, // SDL_SCANCODE_KP_EQUALS
    F13 = 104,      // SDL_SCANCODE_F13
    F14 = 105,      // SDL_SCANCODE_F14
    F15 = 106,      // SDL_SCANCODE_F15
    F16 = 107,      // SDL_SCANCODE_F16
    F17 = 108,      // SDL_SCANCODE_F17
    F18 = 109,      // SDL_SCANCODE_F18
    F19 = 110,      // SDL_SCANCODE_F19
    F20 = 111,      // SDL_SCANCODE_F20
    F21 = 112,      // SDL_SCANCODE_F21
    F22 = 113,      // SDL_SCANCODE_F22
    F23 = 114,      // SDL_SCANCODE_F23
    F24 = 115,      // SDL_SCANCODE_F24

    Execute = 116, // SDL_SCANCODE_EXECUTE
    Help = 117,    // SDL_SCANCODE_HELP
    Menu = 118,    // SDL_SCANCODE_MENU
    Select = 119,  // SDL_SCANCODE_SELECT
    Stop = 120,    // SDL_SCANCODE_STOP
    Again = 121,   // SDL_SCANCODE_AGAIN
    Undo = 122,    // SDL_SCANCODE_UNDO
    Cut = 123,     // SDL_SCANCODE_CUT
    Copy = 124,    // SDL_SCANCODE_COPY
    Paste = 125,   // SDL_SCANCODE_PASTE
    Find = 126,    // SDL_SCANCODE_FIND
    Mute = 127,    // SDL_SCANCODE_MUTE
    VolumeUp = 128,   // SDL_SCANCODE_VOLUMEUP
    VolumeDown = 129, // SDL_SCANCODE_VOLUMEDOWN

    // Skipping some less common international and special keys for brevity,
    // but they can be added if needed by referring to SDL_Scancode.

    KpComma = 133, // SDL_SCANCODE_KP_COMMA

    // Modifier keys
    LeftCtrl = 224,   // SDL_SCANCODE_LCTRL
    LeftShift = 225,  // SDL_SCANCODE_LSHIFT
    LeftAlt = 226,    // SDL_SCANCODE_LALT (Option key on Mac)
    LeftSuper = 227,  // SDL_SCANCODE_LGUI (Windows key, Command key)
    RightCtrl = 228,  // SDL_SCANCODE_RCTRL
    RightShift = 229, // SDL_SCANCODE_RSHIFT
    RightAlt = 230,   // SDL_SCANCODE_RALT (Option key on Mac, Alt Gr)
    RightSuper = 231, // SDL_SCANCODE_RGUI (Windows key, Command key)

    Mode = 257, // SDL_SCANCODE_MODE (AltGr, Multi_key)

    // Media keys (subset)
    MediaPlay = 262,        // SDL_SCANCODE_MEDIA_PLAY
    MediaPause = 263,       // SDL_SCANCODE_MEDIA_PAUSE
    MediaRecord = 264,      // SDL_SCANCODE_MEDIA_RECORD
    MediaFastForward = 265, // SDL_SCANCODE_MEDIA_FAST_FORWARD
    MediaRewind = 266,      // SDL_SCANCODE_MEDIA_REWIND
    MediaNextTrack = 267,   // SDL_SCANCODE_MEDIA_NEXT_TRACK
    MediaPreviousTrack = 268, // SDL_SCANCODE_MEDIA_PREVIOUS_TRACK
    MediaStop = 269,        // SDL_SCANCODE_MEDIA_STOP
    MediaEject = 270,       // SDL_SCANCODE_MEDIA_EJECT
    MediaPlayPause = 271,   // SDL_SCANCODE_MEDIA_PLAY_PAUSE
    MediaSelect = 272,      // SDL_SCANCODE_MEDIA_SELECT

    // Application control keys (subset)
    AppSearch = 280,     // SDL_SCANCODE_AC_SEARCH
    AppHome = 281,       // SDL_SCANCODE_AC_HOME
    AppBack = 282,       // SDL_SCANCODE_AC_BACK
    AppForward = 283,    // SDL_SCANCODE_AC_FORWARD
    AppStop = 284,       // SDL_SCANCODE_AC_STOP
    AppRefresh = 285,    // SDL_SCANCODE_AC_REFRESH
    AppBookmarks = 286,  // SDL_SCANCODE_AC_BOOKMARKS

    // Total number of scancodes. Not a key itself.
    // SDL_SCANCODE_COUNT = 512
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
