// <copyright file="Mouse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

using Night;

using SDL3;

namespace Night
{
  /// <summary>
  /// Provides an interface to the user's mouse.
  /// </summary>
  public static class Mouse
  {
    /// <summary>
    /// Checks whether a certain mouse button is down.
    /// This function does not detect mouse wheel scrolling.
    /// </summary>
    /// <param name="button">The index of a button to check. 1 is the primary mouse button, 2 is the secondary mouse button, 3 is the middle button, 4 is the X1 button (typically "back"), and 5 is the X2 button (typically "forward").</param>
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
          return false;
      }

      return (mouseState & buttonMask) != 0;
    }

    /// <summary>
    /// Gets the current position of the mouse cursor in the window.
    /// </summary>
    /// <returns>A tuple (int X, int Y) representing the mouse cursor coordinates.</returns>
    public static (int X, int Y) GetPosition()
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

    /// <summary>
    /// Sets the visibility of the mouse cursor.
    /// </summary>
    /// <param name="visible">True to show the cursor, false to hide it.</param>
    public static void SetVisible(bool visible)
    {
      if (!Framework.IsInputInitialized)
      {
        return;
      }

      if (visible)
      {
        _ = SDL.ShowCursor();
      }
      else
      {
        _ = SDL.HideCursor();
      }
    }

    /// <summary>
    /// Grabs the mouse, confining it to the window.
    /// </summary>
    /// <param name="grabbed">True to grab the mouse, false to release it.</param>
    public static void SetGrabbed(bool grabbed)
    {
      if (!Framework.IsInputInitialized || Window.Handle == nint.Zero)
      {
        return;
      }

      _ = SDL.SetWindowMouseGrab(Window.Handle, grabbed);
    }

    /// <summary>
    /// Sets whether relative mouse mode is enabled for the current window.
    /// In relative mode, the cursor is hidden and mouse motion is reported as relative to the last position.
    /// </summary>
    /// <param name="enabled">True to enable relative mode, false to disable it.</param>
    public static void SetRelativeMode(bool enabled)
    {
      if (!Framework.IsInputInitialized || Window.Handle == nint.Zero)
      {
        return;
      }

      _ = SDL.SetWindowRelativeMouseMode(Window.Handle, enabled);
    }
  }
}
