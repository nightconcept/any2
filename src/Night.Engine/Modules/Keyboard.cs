using System;
using System.Runtime.InteropServices; // For Marshal

using Night.Types; // For KeyCode

using static SDL3.SDL; // For direct access to SDL functions

namespace Night;

/// <summary>
/// Provides functionality for handling keyboard input.
/// Mimics Love2D's love.keyboard module.
/// </summary>
public static class Keyboard
{
  // TODO: Task 4.4 - Create a comprehensive mapping for all KeyCodes.
  private static SDL_Scancode MapKeyCodeToScancode(KeyCode key)
  {
    switch (key)
    {
      // Letters
      case KeyCode.A: return SDL_Scancode.SDL_SCANCODE_A;
      case KeyCode.B: return SDL_Scancode.SDL_SCANCODE_B;
      case KeyCode.C: return SDL_Scancode.SDL_SCANCODE_C;
      case KeyCode.D: return SDL_Scancode.SDL_SCANCODE_D;
      case KeyCode.E: return SDL_Scancode.SDL_SCANCODE_E;
      case KeyCode.F: return SDL_Scancode.SDL_SCANCODE_F;
      case KeyCode.G: return SDL_Scancode.SDL_SCANCODE_G;
      case KeyCode.H: return SDL_Scancode.SDL_SCANCODE_H;
      case KeyCode.I: return SDL_Scancode.SDL_SCANCODE_I;
      case KeyCode.J: return SDL_Scancode.SDL_SCANCODE_J;
      case KeyCode.K: return SDL_Scancode.SDL_SCANCODE_K;
      case KeyCode.L: return SDL_Scancode.SDL_SCANCODE_L;
      case KeyCode.M: return SDL_Scancode.SDL_SCANCODE_M;
      case KeyCode.N: return SDL_Scancode.SDL_SCANCODE_N;
      case KeyCode.O: return SDL_Scancode.SDL_SCANCODE_O;
      case KeyCode.P: return SDL_Scancode.SDL_SCANCODE_P;
      case KeyCode.Q: return SDL_Scancode.SDL_SCANCODE_Q;
      case KeyCode.R: return SDL_Scancode.SDL_SCANCODE_R;
      case KeyCode.S: return SDL_Scancode.SDL_SCANCODE_S;
      case KeyCode.T: return SDL_Scancode.SDL_SCANCODE_T;
      case KeyCode.U: return SDL_Scancode.SDL_SCANCODE_U;
      case KeyCode.V: return SDL_Scancode.SDL_SCANCODE_V;
      case KeyCode.W: return SDL_Scancode.SDL_SCANCODE_W;
      case KeyCode.X: return SDL_Scancode.SDL_SCANCODE_X;
      case KeyCode.Y: return SDL_Scancode.SDL_SCANCODE_Y;
      case KeyCode.Z: return SDL_Scancode.SDL_SCANCODE_Z;

      // Numbers
      case KeyCode.Num0: return SDL_Scancode.SDL_SCANCODE_0;
      case KeyCode.Num1: return SDL_Scancode.SDL_SCANCODE_1;
      case KeyCode.Num2: return SDL_Scancode.SDL_SCANCODE_2;
      case KeyCode.Num3: return SDL_Scancode.SDL_SCANCODE_3;
      case KeyCode.Num4: return SDL_Scancode.SDL_SCANCODE_4;
      case KeyCode.Num5: return SDL_Scancode.SDL_SCANCODE_5;
      case KeyCode.Num6: return SDL_Scancode.SDL_SCANCODE_6;
      case KeyCode.Num7: return SDL_Scancode.SDL_SCANCODE_7;
      case KeyCode.Num8: return SDL_Scancode.SDL_SCANCODE_8;
      case KeyCode.Num9: return SDL_Scancode.SDL_SCANCODE_9;

      // Function keys
      case KeyCode.F1: return SDL_Scancode.SDL_SCANCODE_F1;
      case KeyCode.F2: return SDL_Scancode.SDL_SCANCODE_F2;
      case KeyCode.F3: return SDL_Scancode.SDL_SCANCODE_F3;
      case KeyCode.F4: return SDL_Scancode.SDL_SCANCODE_F4;
      case KeyCode.F5: return SDL_Scancode.SDL_SCANCODE_F5;
      case KeyCode.F6: return SDL_Scancode.SDL_SCANCODE_F6;
      case KeyCode.F7: return SDL_Scancode.SDL_SCANCODE_F7;
      case KeyCode.F8: return SDL_Scancode.SDL_SCANCODE_F8;
      case KeyCode.F9: return SDL_Scancode.SDL_SCANCODE_F9;
      case KeyCode.F10: return SDL_Scancode.SDL_SCANCODE_F10;
      case KeyCode.F11: return SDL_Scancode.SDL_SCANCODE_F11;
      case KeyCode.F12: return SDL_Scancode.SDL_SCANCODE_F12;

      // Control keys
      case KeyCode.LeftShift: return SDL_Scancode.SDL_SCANCODE_LSHIFT;
      case KeyCode.RightShift: return SDL_Scancode.SDL_SCANCODE_RSHIFT;
      case KeyCode.LeftCtrl: return SDL_Scancode.SDL_SCANCODE_LCTRL;
      case KeyCode.RightCtrl: return SDL_Scancode.SDL_SCANCODE_RCTRL;
      case KeyCode.LeftAlt: return SDL_Scancode.SDL_SCANCODE_LALT;
      case KeyCode.RightAlt: return SDL_Scancode.SDL_SCANCODE_RALT;
      case KeyCode.LeftSuper: return SDL_Scancode.SDL_SCANCODE_LGUI; // GUI key often maps to Super/Windows/Command
      case KeyCode.RightSuper: return SDL_Scancode.SDL_SCANCODE_RGUI;
      case KeyCode.Enter: return SDL_Scancode.SDL_SCANCODE_RETURN;
      case KeyCode.Escape: return SDL_Scancode.SDL_SCANCODE_ESCAPE;
      case KeyCode.Space: return SDL_Scancode.SDL_SCANCODE_SPACE;
      case KeyCode.Tab: return SDL_Scancode.SDL_SCANCODE_TAB;
      case KeyCode.Backspace: return SDL_Scancode.SDL_SCANCODE_BACKSPACE;
      case KeyCode.Delete: return SDL_Scancode.SDL_SCANCODE_DELETE;
      case KeyCode.Insert: return SDL_Scancode.SDL_SCANCODE_INSERT;
      case KeyCode.Home: return SDL_Scancode.SDL_SCANCODE_HOME;
      case KeyCode.End: return SDL_Scancode.SDL_SCANCODE_END;
      case KeyCode.PageUp: return SDL_Scancode.SDL_SCANCODE_PAGEUP;
      case KeyCode.PageDown: return SDL_Scancode.SDL_SCANCODE_PAGEDOWN;

      // Arrow keys
      case KeyCode.Up: return SDL_Scancode.SDL_SCANCODE_UP;
      case KeyCode.Down: return SDL_Scancode.SDL_SCANCODE_DOWN;
      case KeyCode.Left: return SDL_Scancode.SDL_SCANCODE_LEFT;
      case KeyCode.Right: return SDL_Scancode.SDL_SCANCODE_RIGHT;

      case KeyCode.Unknown:
      default:
        return SDL_Scancode.SDL_SCANCODE_UNKNOWN;
    }
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

    // The SDL_PumpEvents function should be called in the main event loop
    // to update the keyboard state. Assuming Engine.Run() handles this.

    nint keyboardStatePtr = SDL_GetKeyboardState(out int numKeys);

    if (keyboardStatePtr == nint.Zero)
    {
      // This case should ideally not happen if SDL is initialized correctly.
      // Log a warning or handle as per engine's error strategy.
      Console.WriteLine("Warning: SDL_GetKeyboardState returned a null pointer.");
      return false;
    }

    SDL_Scancode sdlScancode = MapKeyCodeToScancode(key);

    if (sdlScancode == SDL_Scancode.SDL_SCANCODE_UNKNOWN)
    {
      return false; // Unknown or unmapped key
    }

    // Ensure scancode is within bounds. numKeys represents the number of scancodes.
    if ((int)sdlScancode >= numKeys)
    {
      Console.WriteLine($"Warning: Scancode {(int)sdlScancode} is out of bounds (numKeys: {numKeys}).");
      return false;
    }

    // The array is indexed by SDL_Scancode values.
    // SDL_GetKeyboardState returns a pointer to an array of Uint8.
    // A value of 1 means pressed, 0 means not pressed.
    byte byteKeyState = Marshal.ReadByte(keyboardStatePtr, (int)sdlScancode);
    return byteKeyState == 1;
  }
}
