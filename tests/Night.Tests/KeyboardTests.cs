// <copyright file="KeyboardTests.cs" company="Night Circle">
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

using Night;

using SDL3;

using Xunit;

namespace Night.Tests.Keyboard
{
  /// <summary>
  /// Contains unit tests for the <see cref="Night.Keyboard"/> class.
  /// </summary>
  public class KeyboardTests
  {
    /// <summary>
    /// Tests that <see cref="Night.Keyboard.IsDown(KeyCode)"/> returns false
    /// when the input system is not initialized.
    /// </summary>
    [Fact]
    public void IsDown_InputSystemNotInitialized_ReturnsFalse()
    {
      // Arrange
      bool originalInputState = Framework.IsInputInitialized;
      Framework.IsInputInitialized = false;

      // Act
      bool result = Night.Keyboard.IsDown(Night.KeyCode.A);

      // Assert
      Assert.False(result);

      // Cleanup
      Framework.IsInputInitialized = originalInputState;
    }

    /// <summary>
    /// Tests that <see cref="Night.Keyboard.IsDown(KeyCode)"/> returns false
    /// for an unknown KeyCode.
    /// </summary>
    [Fact]
    public void IsDown_UnknownKeyCode_ReturnsFalse()
    {
      // Arrange
      bool originalInputState = Framework.IsInputInitialized;
      Framework.IsInputInitialized = true; // Ensure this check passes

      // Act
      // Cast an SDL.Scancode that is explicitly 'Unknown' to Night.KeyCode
      // This simulates a scenario where a KeyCode might not have a valid mapping.
      bool result = Night.Keyboard.IsDown((Night.KeyCode)SDL.Scancode.Unknown);

      // Assert
      Assert.False(result);

      // Cleanup
      Framework.IsInputInitialized = originalInputState;
    }

    /// <summary>
    /// Tests that <see cref="Night.Keyboard.IsDown(KeyCode)"/> returns false
    /// when the scancode is out of bounds for the keyboard state array.
    /// </summary>
    [Fact]
    public void IsDown_ScancodeOutOfBounds_ReturnsFalse()
    {
      // Arrange
      bool originalInputState = Framework.IsInputInitialized;
      Framework.IsInputInitialized = true; // Ensure this check passes

      // Act
      // Using a large integer value cast to KeyCode to simulate an out-of-bounds scancode.
      // SDL.GetKeyboardState() would return an array, and if the scancode index is
      // outside this array's bounds, the method should handle it gracefully.
      // The actual Keyboard.cs code checks against keyboardState.Length.
      // We are testing the C# logic path for this condition.
      bool result = Night.Keyboard.IsDown((Night.KeyCode)int.MaxValue);

      // Assert
      Assert.False(result);

      // Cleanup
      Framework.IsInputInitialized = originalInputState;
    }

    // Note: Testing the scenario where SDL.GetKeyboardState() itself returns null
    // is difficult without mocking SDL, which is avoided per the testing plan.
    // The Keyboard.IsDown() method already includes a null check for keyboardState
    // (Keyboard.cs line 52), so this path is covered by defensive programming.

    // Note: Testing the scenario where a key is actually reported as "down" by SDL
    // (i.e., keyboardState[(int)sdlScancode] is true) is also beyond the scope of
    // these unit tests as it would require OS-level interaction or SDL mocking.
    // The tests focus on the C# logic paths within Night.Keyboard.IsDown().
  }
}
