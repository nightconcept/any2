using System;
using System.Runtime.InteropServices; // For Marshal

using Night.Types;

using SDL3;

namespace Night;

/// <summary>
/// Provides functionality for handling keyboard input.
/// Mimics Love2D's love.keyboard module.
/// </summary>
public static class Keyboard
{
  // Since Night.Types.KeyCode now aligns with SDL.Scancode names and values,
  // this mapping function essentially becomes a cast.
  // It's kept for potential future divergence or for clarity at call sites.
  private static SDL.Scancode MapKeyCodeToScancode(KeyCode key)
  {
    // Direct cast is possible because the enum values are identical.
    return (SDL.Scancode)key;
  }

  /// <summary>
  /// Checks if a specific key is currently pressed down.
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

    // SDL.GetKeyboardState in SDL3-CS returns a bool[] directly.
    bool[] keyboardState = SDL.GetKeyboardState(out int numKeys);

    if (keyboardState == null) // Check if the array itself is null (e.g., if SDL isn't initialized)
    {
      Console.WriteLine("Warning: SDL.GetKeyboardState returned a null array.");
      return false;
    }

    SDL.Scancode sdlScancode = MapKeyCodeToScancode(key);

    if (sdlScancode == SDL.Scancode.Unknown)
    {
      return false; // Unknown or unmapped key
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
