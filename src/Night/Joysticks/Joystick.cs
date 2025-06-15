// <copyright file="Joystick.cs" company="Night Circle">
// zlib license
//
// Copyright (c) 2025 Danny Solivan, Night Circle
//
// This software is provided 'as-is', without any express or implied
// warranty. In no event will the authors be held liable for any damages
// arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software. If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
// </copyright>

using System;
using System.Collections.Generic;

using SDL3;

namespace Night.Joysticks
{
  /// <summary>
  /// Virtual gamepad axes.
  /// </summary>
  public enum GamepadAxis
  {
    /// <summary>
    /// The horizontal axis of the left analog stick.
    /// </summary>
    LeftX,

    /// <summary>
    /// The vertical axis of the left analog stick.
    /// </summary>
    LeftY,

    /// <summary>
    /// The horizontal axis of the right analog stick.
    /// </summary>
    RightX,

    /// <summary>
    /// The vertical axis of the right analog stick.
    /// </summary>
    RightY,

    /// <summary>
    /// The left trigger axis.
    /// </summary>
    TriggerLeft,

    /// <summary>
    /// The right trigger axis.
    /// </summary>
    TriggerRight,
  }

  /// <summary>
  /// Virtual gamepad buttons.
  /// </summary>
  public enum GamepadButton
  {
    /// <summary>
    /// The 'A' button (often cross or bottom face button).
    /// </summary>
    A,

    /// <summary>
    /// The 'B' button (often circle or right face button).
    /// </summary>
    B,

    /// <summary>
    /// The 'X' button (often square or left face button).
    /// </summary>
    X,

    /// <summary>
    /// The 'Y' button (often triangle or top face button).
    /// </summary>
    Y,

    /// <summary>
    /// The 'Back' or 'Select' button.
    /// </summary>
    Back,

    /// <summary>
    /// The 'Guide' or 'Home' button (e.g., Xbox button).
    /// </summary>
    Guide,

    /// <summary>
    /// The 'Start' button.
    /// </summary>
    Start,

    /// <summary>
    /// The D-pad left button.
    /// </summary>
    DPLeft,

    /// <summary>
    /// The D-pad right button.
    /// </summary>
    DPRight,

    /// <summary>
    /// The D-pad up button.
    /// </summary>
    DPUp,

    /// <summary>
    /// The D-pad down button.
    /// </summary>
    DPDown,

    /// <summary>
    /// The left shoulder button (bumper).
    /// </summary>
    LeftShoulder,

    /// <summary>
    /// The right shoulder button (bumper).
    /// </summary>
    RightShoulder,

    /// <summary>
    /// The left analog stick click button.
    /// </summary>
    LeftStick,

    /// <summary>
    /// The right analog stick click button.
    /// </summary>
    RightStick,
  }

  /// <summary>
  /// Joystick hat positions. (LÖVE 0.5.0+)
  /// Note: LÖVE uses lowercase strings ('c', 'u', 'r', 'd', 'l', 'ru', 'rd', 'lu', 'ld').
  /// </summary>
  public enum JoystickHat
  {
    /// <summary>
    /// Hat is centered.
    /// </summary>
    Centered,

    /// <summary>
    /// Hat is pressed up.
    /// </summary>
    Up,

    /// <summary>
    /// Hat is pressed right.
    /// </summary>
    Right,

    /// <summary>
    /// Hat is pressed down.
    /// </summary>
    Down,

    /// <summary>
    /// Hat is pressed left.
    /// </summary>
    Left,

    /// <summary>
    /// Hat is pressed up and right.
    /// </summary>
    RightUp,

    /// <summary>
    /// Hat is pressed down and right.
    /// </summary>
    RightDown,

    /// <summary>
    /// Hat is pressed up and left.
    /// </summary>
    LeftUp,

    /// <summary>
    /// Hat is pressed down and left.
    /// </summary>
    LeftDown,
  }

  /// <summary>
  /// Types of Joystick inputs.
  /// </summary>
  public enum JoystickInputType
  {
    /// <summary>
    /// The input is an axis.
    /// </summary>
    Axis,

    /// <summary>
    /// The input is a button.
    /// </summary>
    Button,

    /// <summary>
    /// The input is a hat.
    /// </summary>
    Hat,
  }

  /// <summary>
  /// OS-independent device info of the joystick.
  /// </summary>
  public struct DeviceInfo
  {
    /// <summary>
    /// The vendor ID of the joystick.
    /// </summary>
    public int Vendor;

    /// <summary>
    /// The product ID of the joystick.
    /// </summary>
    public int Product;

    /// <summary>
    /// The version number of the joystick.
    /// </summary>
    public int Version;

    /// <summary>
    /// The GUID of the joystick.
    /// </summary>
    public string Guid;
  }

  /// <summary>
  /// Represents the vibration motor strengths.
  /// </summary>
  public struct VibrationStrength
  {
    /// <summary>
    /// The strength of the left vibration motor (0-1).
    /// </summary>
    public float Left;

    /// <summary>
    /// The strength of the right vibration motor (0-1).
    /// </summary>
    public float Right;
  }

  /// <summary>
  /// Represents the result of a gamepad mapping query.
  /// </summary>
  public struct GamepadMappingResult
  {
    /// <summary>
    /// Indicates whether a valid mapping exists.
    /// In LÖVE, this would be nil if no mapping exists.
    /// </summary>
    public bool IsValid;

    /// <summary>
    /// The type of the input (axis, button, or hat).
    /// </summary>
    public JoystickInputType Type;

    /// <summary>
    /// The index of the input (e.g., axis index, button index).
    /// </summary>
    public int InputIndex;

    /// <summary>
    /// The hat value, only relevant if <see cref="Type"/> is <see cref="JoystickInputType.Hat"/>.
    /// </summary>
    public JoystickHat HatValue;
  }

  /// <summary>
  /// Represents a physical joystick.
  /// </summary>
  public class Joystick : IDisposable
  {
    private readonly uint joystickID;
    private readonly IntPtr joystickPtr;
    private bool disposed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="Joystick"/> class. Joystick instances are typically obtained via a method in the <c>Night.Joysticks.Joysticks</c> class.
    /// </summary>
    /// <param name="joystickId">The SDL instance ID of the joystick.</param>
    /// <exception cref="InvalidOperationException">Thrown if the joystick cannot be opened.</exception>
    internal Joystick(uint joystickId)
    {
      this.joystickID = joystickId;
      this.joystickPtr = SDL.OpenJoystick(joystickId);
      if (this.joystickPtr == IntPtr.Zero)
      {
        throw new InvalidOperationException($"Failed to open joystick with ID {joystickId}: {SDL.GetError()}");
      }
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="Joystick"/> class.
    /// </summary>
    ~Joystick()
    {
      this.Dispose(false);
    }

    /// <summary>
    /// Gets the direction of each axis.
    /// </summary>
    /// <returns>An array of floats, one for each axis direction.</returns>
    public float[] GetAxes()
    {
      if (this.disposed)
      {
        return Array.Empty<float>();
      }

      int axisCount = SDL.GetNumJoystickAxes(this.joystickPtr);
      if (axisCount < 0)
      {
        // Error case, perhaps log SDL.GetError() or return empty array
        return Array.Empty<float>();
      }

      float[] axes = new float[axisCount];
      for (int i = 0; i < axisCount; i++)
      {
        axes[i] = this.GetAxis(i);
      }

      return axes;
    }

    /// <summary>
    /// Gets the direction of an axis.
    /// </summary>
    /// <param name="axisIndex">The index of the axis.</param>
    /// <returns>The direction of the specified axis.</returns>
    public float GetAxis(int axisIndex)
    {
      if (this.disposed)
      {
        return 0.0f;
      }

      short rawValue = SDL.GetJoystickAxis(this.joystickPtr, axisIndex);

      if (rawValue == 0)
      {
        return 0.0f;
      }
      else if (rawValue > 0)
      {
        return rawValue / 32767.0f;
      }
      else
      {
        // rawValue < 0
        return rawValue / 32768.0f;
      }
    }

    /// <summary>
    /// Gets the number of axes on the joystick.
    /// </summary>
    /// <returns>The number of axes.</returns>
    public int GetAxisCount()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the number of buttons on the joystick.
    /// </summary>
    /// <returns>The number of buttons.</returns>
    public int GetButtonCount()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the OS-independent device info of the joystick.
    /// </summary>
    /// <returns>The device information.</returns>
    public DeviceInfo GetDeviceInfo()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets a stable GUID unique to the type of the physical joystick.
    /// </summary>
    /// <returns>The joystick's GUID.</returns>
    public string GetGuid()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the direction of a virtual gamepad axis.
    /// </summary>
    /// <param name="axis">The virtual gamepad axis.</param>
    /// <returns>The direction of the gamepad axis.</returns>
    public float GetGamepadAxis(GamepadAxis axis)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the button, axis or hat that a virtual gamepad axis is bound to.
    /// </summary>
    /// <param name="axis">The virtual gamepad axis to check.</param>
    /// <returns>A GamepadMappingResult indicating the physical input.</returns>
    public GamepadMappingResult GetGamepadMapping(GamepadAxis axis)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the button, axis or hat that a virtual gamepad button is bound to.
    /// </summary>
    /// <param name="button">The virtual gamepad button to check.</param>
    /// <returns>A GamepadMappingResult indicating the physical input.</returns>
    public GamepadMappingResult GetGamepadMapping(GamepadButton button)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the full gamepad mapping string of this Joystick, or null if it's not recognized as a gamepad.
    /// </summary>
    /// <returns>The gamepad mapping string, or null.</returns>
    public string? GetGamepadMappingString()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the direction of a hat.
    /// </summary>
    /// <param name="hatIndex">The index of the hat.</param>
    /// <returns>The direction of the specified hat.</returns>
    public JoystickHat GetHat(int hatIndex)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the number of hats on the joystick.
    /// </summary>
    /// <returns>The number of hats.</returns>
    public int GetHatCount()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the joystick's unique identifier.
    /// </summary>
    /// <returns>The joystick's ID.</returns>
    public int GetId()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the name of the joystick.
    /// </summary>
    /// <returns>The joystick's name.</returns>
    public string GetName()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the current vibration motor strengths on a Joystick with rumble support.
    /// </summary>
    /// <returns>The current vibration strengths.</returns>
    public VibrationStrength GetVibration()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets whether the Joystick is connected.
    /// </summary>
    /// <returns>True if connected, false otherwise.</returns>
    public bool IsConnected()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Checks if a button on the Joystick is pressed.
    /// </summary>
    /// <param name="buttonIndex">The index of the button.</param>
    /// <returns>True if the button is pressed, false otherwise.</returns>
    public bool IsDown(int buttonIndex)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets whether the Joystick is recognized as a gamepad.
    /// </summary>
    /// <returns>True if it's a gamepad, false otherwise.</returns>
    public bool IsGamepad()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Checks if a virtual gamepad button on the Joystick is pressed.
    /// </summary>
    /// <param name="button">The virtual gamepad button.</param>
    /// <returns>True if the gamepad button is pressed, false otherwise.</returns>
    public bool IsGamepadDown(GamepadButton button)
    {
      if (this.disposed)
      {
        return false;
      }

      // The C# binding for the C function SDL_JoystickGetGamepadButton (which takes an SDL_Joystick*)
      // was not found in the reviewed SDL3-CS PInvoke files.
      // SDL.GetGamepadButton() takes an SDL_Gamepad* and is not directly applicable here.
      throw new NotImplementedException("Binding for SDL_JoystickGetGamepadButton not found.");
    }

    /// <summary>
    /// Gets whether the Joystick supports vibration.
    /// </summary>
    /// <returns>True if vibration is supported, false otherwise.</returns>
    public bool IsVibrationSupported()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Sets the vibration motor speeds on a Joystick with rumble support.
    /// </summary>
    /// <param name="left">The strength of the left motor (0-1).</param>
    /// <param name="right">The strength of the right motor (0-1).</param>
    public void SetVibration(float left, float right)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Releases the resources used by the <see cref="Joystick"/> object.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Joystick"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          // Dispose managed state (managed objects).
        }

        if (this.joystickPtr != IntPtr.Zero)
        {
          SDL.CloseJoystick(this.joystickPtr);

          // Note: _joystickPtr is readonly, so cannot set to IntPtr.Zero here.
          // Consider if it should be mutable if we want to allow re-opening or null check.
          // For now, the dispose pattern assumes it's closed and not used again.
        }

        this.disposed = true;
      }
    }

    // LÖVE's Object supertype methods (Release, GetType, IsInstanceOf) are not
    // implemented here, consistent with other LÖVE object wrappers in this project
    // (e.g., Night.Sprite, Night.ImageData) which do not have a common base class
    // for these methods.

    /*
    // Helper method to map Night GamepadButton to SDL.GamepadButton
    private static SDL.GamepadButton MapGamepadButton(GamepadButton button)
    {
      switch (button)
      {
        case GamepadButton.A:
          return SDL.GamepadButton.South; // Typically 'A' on Xbox, Cross on PlayStation
        case GamepadButton.B:
          return SDL.GamepadButton.East;  // Typically 'B' on Xbox, Circle on PlayStation
        case GamepadButton.X:
          return SDL.GamepadButton.West;  // Typically 'X' on Xbox, Square on PlayStation
        case GamepadButton.Y:
          return SDL.GamepadButton.North; // Typically 'Y' on Xbox, Triangle on PlayStation
        case GamepadButton.Back:
          return SDL.GamepadButton.Back;
        case GamepadButton.Guide:
          return SDL.GamepadButton.Guide;
        case GamepadButton.Start:
          return SDL.GamepadButton.Start;
        case GamepadButton.LeftStick:
          return SDL.GamepadButton.LeftStick;
        case GamepadButton.RightStick:
          return SDL.GamepadButton.RightStick;
        case GamepadButton.LeftShoulder:
          return SDL.GamepadButton.LeftShoulder;
        case GamepadButton.RightShoulder:
          return SDL.GamepadButton.RightShoulder;
        case GamepadButton.DPUp:
          return SDL.GamepadButton.DpadUp;
        case GamepadButton.DPDown:
          return SDL.GamepadButton.DpadDown;
        case GamepadButton.DPLeft:
          return SDL.GamepadButton.DpadLeft;
        case GamepadButton.DPRight:
          return SDL.GamepadButton.DpadRight;
        default:
          throw new ArgumentOutOfRangeException(nameof(button), $"Unmapped gamepad button: {button}");
      }
    }
    */
  }
}
