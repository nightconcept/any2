using System;

using Night; // For MouseButton

using SDL3;

namespace Night;

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
    if (!Framework.IsInputInitialized)
    {
      Console.WriteLine("Warning: Night.Mouse.IsDown called before input system is initialized. Returning false.");
      return false;
    }

    SDL.MouseButtonFlags mouseState = SDL.GetMouseState(out float _, out float _);

    SDL.MouseButtonFlags buttonMask;
    switch (button)
    {
      case MouseButton.Left:
        buttonMask = SDL.MouseButtonFlags.Left;
        break;
      case MouseButton.Middle:
        buttonMask = SDL.MouseButtonFlags.Middle;
        break;
      case MouseButton.Right:
        buttonMask = SDL.MouseButtonFlags.Right;
        break;
      case MouseButton.X1:
        buttonMask = SDL.MouseButtonFlags.X1;
        break;
      case MouseButton.X2:
        buttonMask = SDL.MouseButtonFlags.X2;
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
    if (!Framework.IsInputInitialized)
    {
      Console.WriteLine("Warning: Night.Mouse.GetPosition called before input system is initialized. Returning (0,0).");
      return (0, 0);
    }

    float mouseX, mouseY;
    _ = SDL.GetMouseState(out mouseX, out mouseY);
    return ((int)mouseX, (int)mouseY);
  }
}
