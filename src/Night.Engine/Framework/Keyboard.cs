using System;
using System.Runtime.InteropServices;

using Night;

using SDL3;

namespace Night;

/// <summary>
/// Provides an interface to the user's keyboard.
/// </summary>
public static class Keyboard
{
  /// <summary>
  /// Checks whether a certain key is down.
  /// </summary>
  /// <param name="key">The key to check.</param>
  /// <returns>True if the key is down, false otherwise.</returns>
  public static bool IsDown(KeyCode key)
  {
    if (!Framework.IsInputInitialized)
    {
      Console.WriteLine("Warning: Night.Keyboard.IsDown called before input system is initialized. Returning false.");
      return false;
    }

    // The SDL.PumpEvents function should be called in the main event loop
    // to update the keyboard state. Framework.Run() handles this.

    bool[] keyboardState = SDL.GetKeyboardState(out int _);

    if (keyboardState == null)
    {
      Console.WriteLine("Warning: SDL.GetKeyboardState returned a null array.");
      return false;
    }

    // Direct cast is possible because the enum values are identical
    SDL.Scancode sdlScancode = (SDL.Scancode)key;

    if (sdlScancode == SDL.Scancode.Unknown)
    {
      return false;
    }

    // Ensure scancode is within bounds of the returned array.
    // numKeys should be equivalent to keyboardState.Length.
    if ((int)sdlScancode >= keyboardState.Length || (int)sdlScancode < 0)
    {
      Console.WriteLine($"Warning: Scancode {(int)sdlScancode} is out of bounds (numKeys: {keyboardState.Length}).");
      return false;
    }

    // The array is indexed by SDL.Scancode values.
    // The boolean value directly indicates the pressed state.
    return keyboardState[(int)sdlScancode];
  }
}
