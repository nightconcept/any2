using System;

using SDL3;

namespace Night
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
    Alpha1 = 30, // SDL.Scancode.Alpha1
    Alpha2 = 31, // SDL.Scancode.Alpha2
    Alpha3 = 32, // SDL.Scancode.Alpha3
    Alpha4 = 33, // SDL.Scancode.Alpha4
    Alpha5 = 34, // SDL.Scancode.Alpha5
    Alpha6 = 35, // SDL.Scancode.Alpha6
    Alpha7 = 36, // SDL.Scancode.Alpha7
    Alpha8 = 37, // SDL.Scancode.Alpha8
    Alpha9 = 38, // SDL.Scancode.Alpha9
    Alpha0 = 39, // SDL.Scancode.Alpha0

    // Control keys
    Return = 40,       // SDL.Scancode.Return
    Escape = 41,      // SDL.Scancode.Escape
    Backspace = 42,   // SDL.Scancode.Backspace
    Tab = 43,         // SDL.Scancode.Tab
    Space = 44,       // SDL.Scancode.Space
    Minus = 45,       // SDL.Scancode.Minus
    Equals = 46,      // SDL.Scancode.Equals
    Leftbracket = 47, // SDL.Scancode.Leftbracket
    Rightbracket = 48,// SDL.Scancode.Rightbracket
    Backslash = 49,   // SDL.Scancode.Backslash
    NonUshash = 50,   // SDL.Scancode.NonUshash
    Semicolon = 51,   // SDL.Scancode.Semicolon
    Apostrophe = 52,  // SDL.Scancode.Apostrophe
    Grave = 53,       // SDL.Scancode.Grave
    Comma = 54,       // SDL.Scancode.Comma
    Period = 55,      // SDL.Scancode.Period
    Slash = 56,       // SDL.Scancode.Slash
    Capslock = 57,    // SDL.Scancode.Capslock

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

    Printscreen = 70, // SDL.Scancode.Printscreen
    Scrolllock = 71,  // SDL.Scancode.Scrolllock
    Pause = 72,       // SDL.Scancode.Pause
    Insert = 73,      // SDL.Scancode.Insert
    Home = 74,        // SDL.Scancode.Home
    Pageup = 75,      // SDL.Scancode.Pageup
    Delete = 76,      // SDL.Scancode.Delete
    End = 77,         // SDL.Scancode.End
    Pagedown = 78,    // SDL.Scancode.Pagedown

    // Arrow keys
    Right = 79, // SDL.Scancode.Right
    Left = 80,  // SDL.Scancode.Left
    Down = 81,  // SDL.Scancode.Down
    Up = 82,    // SDL.Scancode.Up

    NumlockClear = 83, // SDL.Scancode.NumlockClear

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

    NonUsbackslash = 100, // SDL.Scancode.NonUsbackslash
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

    KpComma = 133, // SDL.Scancode.KpComma

    // Modifier keys
    LCtrl = 224,   // SDL.Scancode.LCtrl
    LShift = 225,  // SDL.Scancode.LShift
    LAlt = 226,    // SDL.Scancode.LAlt
    LGUI = 227,  // SDL.Scancode.LGUI
    RCtrl = 228,  // SDL.Scancode.RCtrl
    RShift = 229, // SDL.Scancode.RShift
    RAlt = 230,   // SDL.Scancode.RAlt
    RGUI = 231, // SDL.Scancode.RGUI

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
  }

  /// <summary>
  /// Represents logical key symbols. Values correspond to SDL_Keycode.
  /// </summary>
  /// <remarks>
  /// This enum maps to SDL_Keycode values, representing the symbol produced by a key press
  /// under the current keyboard layout.
  /// </remarks>
  public enum KeySymbol : uint // Explicitly set underlying type to uint
  {
    Unknown = SDL.Keycode.Unknown,

    // Letters (match SDL.Keycode values, which are ASCII for letters)
    A = SDL.Keycode.A,
    B = SDL.Keycode.B,
    C = SDL.Keycode.C,
    D = SDL.Keycode.D,
    E = SDL.Keycode.E,
    F = SDL.Keycode.F,
    G = SDL.Keycode.G,
    H = SDL.Keycode.H,
    I = SDL.Keycode.I,
    J = SDL.Keycode.J,
    K = SDL.Keycode.K,
    L = SDL.Keycode.L,
    M = SDL.Keycode.M,
    N = SDL.Keycode.N,
    O = SDL.Keycode.O,
    P = SDL.Keycode.P,
    Q = SDL.Keycode.Q,
    R = SDL.Keycode.R,
    S = SDL.Keycode.S,
    T = SDL.Keycode.T,
    U = SDL.Keycode.U,
    V = SDL.Keycode.V,
    W = SDL.Keycode.W,
    X = SDL.Keycode.X,
    Y = SDL.Keycode.Y,
    Z = SDL.Keycode.Z,

    // Numbers (Top row - match SDL.Keycode values, which are ASCII for numbers)
    Alpha0 = SDL.Keycode.Alpha0,
    Alpha1 = SDL.Keycode.Alpha1,
    Alpha2 = SDL.Keycode.Alpha2,
    Alpha3 = SDL.Keycode.Alpha3,
    Alpha4 = SDL.Keycode.Alpha4,
    Alpha5 = SDL.Keycode.Alpha5,
    Alpha6 = SDL.Keycode.Alpha6,
    Alpha7 = SDL.Keycode.Alpha7,
    Alpha8 = SDL.Keycode.Alpha8,
    Alpha9 = SDL.Keycode.Alpha9,

    // Common control keys
    Return = SDL.Keycode.Return,
    Escape = SDL.Keycode.Escape,
    Backspace = SDL.Keycode.Backspace,
    Tab = SDL.Keycode.Tab,
    Space = SDL.Keycode.Space,

    // Punctuation (example, more can be added)
    Minus = SDL.Keycode.Minus,
    Equals = SDL.Keycode.Equals,
    Leftbracket = SDL.Keycode.LeftBracket,
    Rightbracket = SDL.Keycode.RightBracket,
    Backslash = SDL.Keycode.Backslash,
    Semicolon = SDL.Keycode.Semicolon,
    Apostrophe = SDL.Keycode.Apostrophe,
    Grave = SDL.Keycode.Grave,
    Comma = SDL.Keycode.Comma,
    Period = SDL.Keycode.Period,
    Slash = SDL.Keycode.Slash,

    // Function keys
    F1 = SDL.Keycode.F1,
    F2 = SDL.Keycode.F2,
    F3 = SDL.Keycode.F3,
    F4 = SDL.Keycode.F4,
    F5 = SDL.Keycode.F5,
    F6 = SDL.Keycode.F6,
    F7 = SDL.Keycode.F7,
    F8 = SDL.Keycode.F8,
    F9 = SDL.Keycode.F9,
    F10 = SDL.Keycode.F10,
    F11 = SDL.Keycode.F11,
    F12 = SDL.Keycode.F12,

    // Arrow keys
    Right = SDL.Keycode.Right,
    Left = SDL.Keycode.Left,
    Down = SDL.Keycode.Down,
    Up = SDL.Keycode.Up,

    // Modifiers
    LCtrl = SDL.Keycode.LCtrl,
    LShift = SDL.Keycode.LShift,
    LAlt = SDL.Keycode.LAlt,
    LGUI = SDL.Keycode.LGui,
    RCtrl = SDL.Keycode.RCtrl,
    RShift = SDL.Keycode.RShift,
    RAlt = SDL.Keycode.RAlt,
    RGUI = SDL.Keycode.RGUI,
  }
}
