using System;

using Night.Types; // For MouseButton

using static SDL3.SDL; // For direct access to SDL functions

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
    if (!Framework.IsInputInitialized)
    {
      Console.WriteLine("Warning: Night.Mouse.GetPosition called before input system is initialized. Returning (0,0).");
      return (0, 0);
    }

    float mouseX, mouseY;
    SDL_GetMouseState(out mouseX, out mouseY); // This SDL3-CS function returns uint for button state, but we only need x, y
    return ((int)mouseX, (int)mouseY);
  }
}
