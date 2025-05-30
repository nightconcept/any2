// <copyright file="Keyboard.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Night
{
  using System;
  using System.Runtime.InteropServices;

  using Night;
  using SDL3;

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

      bool[] keyboardState = SDL.GetKeyboardState(out int _);

      if (keyboardState == null)
      {
        Console.WriteLine("Warning: SDL.GetKeyboardState returned a null array.");
        return false;
      }

      SDL.Scancode sdlScancode = (SDL.Scancode)key;

      if (sdlScancode == SDL.Scancode.Unknown)
      {
        return false;
      }

      if ((int)sdlScancode >= keyboardState.Length || (int)sdlScancode < 0)
      {
        Console.WriteLine($"Warning: Scancode {(int)sdlScancode} is out of bounds (numKeys: {keyboardState.Length}).");
        return false;
      }

      return keyboardState[(int)sdlScancode];
    }
  }
}
