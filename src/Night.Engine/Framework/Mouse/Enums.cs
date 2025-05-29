using System;

namespace Night
{
  /// <summary>
  /// Represents mouse buttons. Values correspond to SDL_MouseButtonFlags/
  /// SDL3.SDL.Button* constants.
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
}
