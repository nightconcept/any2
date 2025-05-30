// <copyright file="MouseTests.cs" company="Night Circle">
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
using System.IO;

using Night;

using Xunit;

namespace Night.Tests.Mouse
{
  /// <summary>
  /// Contains unit tests for the <see cref="Night.Mouse"/> class.
  /// These tests focus on the C# logic paths and parameter validation,
  /// without relying on a fully initialized SDL environment or actual mouse hardware.
  /// </summary>
  [Trait("Module", "Mouse")]
  public class MouseTests : IDisposable
  {
    private readonly StringWriter stringWriter;
    private readonly TextWriter originalOutput;

    /// <summary>
    /// Initializes a new instance of the <see cref="MouseTests"/> class.
    /// Sets up console output redirection.
    /// </summary>
    public MouseTests()
    {
      this.stringWriter = new StringWriter();
      this.originalOutput = Console.Out;
      Console.SetOut(this.stringWriter);

      // Ensure a clean state for IsInputInitialized before each test
      Framework.IsInputInitialized = false;

      // Window.Handle will be nint.Zero by default in a test context
      // unless Window.SetMode is called and succeeds.
    }

    /// <summary>
    /// Cleans up resources after each test, restoring console output
    /// and resetting framework state.
    /// </summary>
    public void Dispose()
    {
      Console.SetOut(this.originalOutput);
      this.stringWriter.Dispose();
      Framework.IsInputInitialized = false; // Reset for other test classes
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.IsDown(MouseButton)"/> returns false and logs a warning
    /// when the input system is not initialized.
    /// </summary>
    [Fact]
    public void IsDown_InputNotInitialized_ReturnsFalseAndLogsWarning()
    {
      // Arrange
      Framework.IsInputInitialized = false;

      // Act
      bool result = Night.Mouse.IsDown(MouseButton.Left);

      // Assert
      Assert.False(result);
      Assert.Contains("Warning: Night.Mouse.IsDown called before input system is initialized.", this.stringWriter.ToString());
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.IsDown(MouseButton)"/> returns false
    /// when an unknown mouse button is queried, even if input is initialized.
    /// </summary>
    [Fact]
    public void IsDown_UnknownButton_ReturnsFalse()
    {
      // Arrange
      Framework.IsInputInitialized = true; // Simulate initialized input

      // Act
      bool result = Night.Mouse.IsDown(MouseButton.Unknown);

      // Assert
      Assert.False(result);
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.IsDown(MouseButton)"/> returns false
    /// for a known button when input is initialized but SDL isn't fully mocked (expects SDL.GetMouseState to be callable).
    /// This test primarily ensures no crash and that the logic path for known buttons is hit.
    /// Actual button state depends on SDL and is out of scope for pure C# unit tests.
    /// </summary>
    /// <param name="button">The mouse button to test.</param>
    [Theory]
    [InlineData(MouseButton.Left)]
    [InlineData(MouseButton.Right)]
    [InlineData(MouseButton.Middle)]
    [InlineData(MouseButton.X1)]
    [InlineData(MouseButton.X2)]
    public void IsDown_KnownButtonInputInitialized_ReturnsFalseByDefault(MouseButton button)
    {
      // Arrange
      Framework.IsInputInitialized = true;

      // Act
      // In a test environment without actual SDL mouse state, this should default to false.
      // We are primarily testing that the switch statement and SDL call path doesn't crash.
      bool result = Night.Mouse.IsDown(button);

      // Assert
      Assert.False(result); // Assuming no buttons are pressed in a bare test environment
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.GetPosition()"/> returns (0,0) and logs a warning
    /// when the input system is not initialized.
    /// </summary>
    [Fact]
    public void GetPosition_InputNotInitialized_ReturnsZeroZeroAndLogsWarning()
    {
      // Arrange
      Framework.IsInputInitialized = false;

      // Act
      var (x, y) = Night.Mouse.GetPosition();

      // Assert
      Assert.Equal(0, x);
      Assert.Equal(0, y);
      Assert.Contains("Warning: Night.Mouse.GetPosition called before input system is initialized.", this.stringWriter.ToString());
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.GetPosition()"/> returns (0,0) by default
    /// when input is initialized but SDL isn't fully mocked.
    /// This test primarily ensures no crash. Actual position depends on SDL.
    /// </summary>
    [Fact]
    public void GetPosition_InputInitialized_ReturnsZeroZeroByDefault()
    {
      // Arrange
      Framework.IsInputInitialized = true;

      // Act
      // In a test environment without actual SDL mouse state, this should default to (0,0)
      // after SDL.GetMouseState is called.
      var (x, y) = Night.Mouse.GetPosition();

      // Assert
      Assert.Equal(0, x);
      Assert.Equal(0, y);
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.SetVisible(bool)"/> does not throw an exception
    /// when the input system is not initialized.
    /// </summary>
    [Fact]
    public void SetVisible_InputNotInitialized_DoesNotThrow()
    {
      // Arrange
      Framework.IsInputInitialized = false;

      // Act
      Exception? ex = Record.Exception(() => Night.Mouse.SetVisible(true));

      // Assert
      Assert.Null(ex);
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.SetVisible(bool)"/> does not throw an exception
    /// when the input system is initialized. Actual SDL call behavior is not asserted.
    /// </summary>
    /// <param name="visible">The visibility state to test.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetVisible_InputInitialized_DoesNotThrow(bool visible)
    {
      // Arrange
      Framework.IsInputInitialized = true;

      // Act
      Exception? ex = Record.Exception(() => Night.Mouse.SetVisible(visible));

      // Assert
      Assert.Null(ex);
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.SetGrabbed(bool)"/> does not throw an exception
    /// when the input system is not initialized.
    /// </summary>
    [Fact]
    public void SetGrabbed_InputNotInitialized_DoesNotThrow()
    {
      // Arrange
      Framework.IsInputInitialized = false;

      // Act
      Exception? ex = Record.Exception(() => Night.Mouse.SetGrabbed(true));

      // Assert
      Assert.Null(ex);
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.SetGrabbed(bool)"/> does not throw an exception
    /// when the input system is initialized but the window handle is zero.
    /// </summary>
    [Fact]
    public void SetGrabbed_WindowHandleZero_DoesNotThrow()
    {
      // Arrange
      Framework.IsInputInitialized = true;

      // Window.Handle is nint.Zero by default in tests if Window.SetMode hasn't been called.
      // Act
      Exception? ex = Record.Exception(() => Night.Mouse.SetGrabbed(true));

      // Assert
      Assert.Null(ex);
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.SetGrabbed(bool)"/> does not throw an exception
    /// when the input system and a (mocked/conceptual) window handle are initialized.
    /// Actual SDL call behavior is not asserted.
    /// </summary>
    /// <remarks>
    /// This test assumes that if <see cref="Night.Window.Handle"/> were non-zero, the SDL call would be attempted.
    /// Since we cannot easily set Window.Handle to non-zero without a full SetMode,
    /// this test is similar to WindowHandleZero if SetMode is not called.
    /// The critical path tested here is that it doesn't fail before the SDL call.
    /// </remarks>
    /// <param name="grabbed">The grabbed state to test.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetGrabbed_InputAndWindowInitialized_DoesNotThrow(bool grabbed)
    {
      // Arrange
      Framework.IsInputInitialized = true;

      // For this test, we assume Window.Handle might be non-zero if SetMode was called.
      // However, without calling SetMode, it remains Zero. The method should still not throw
      // in its C# part. If Window.Handle is Zero, it returns early.
      // If it were non-zero, it would attempt SDL_SetWindowMouseGrab.
      // Act
      Exception? ex = Record.Exception(() => Night.Mouse.SetGrabbed(grabbed));

      // Assert
      Assert.Null(ex);
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.SetRelativeMode(bool)"/> does not throw an exception
    /// when the input system is not initialized.
    /// </summary>
    [Fact]
    public void SetRelativeMode_InputNotInitialized_DoesNotThrow()
    {
      // Arrange
      Framework.IsInputInitialized = false;

      // Act
      Exception? ex = Record.Exception(() => Night.Mouse.SetRelativeMode(true));

      // Assert
      Assert.Null(ex);
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.SetRelativeMode(bool)"/> does not throw an exception
    /// when the input system is initialized but the window handle is zero.
    /// </summary>
    [Fact]
    public void SetRelativeMode_WindowHandleZero_DoesNotThrow()
    {
      // Arrange
      Framework.IsInputInitialized = true;

      // Window.Handle is nint.Zero by default in tests.
      // Act
      Exception? ex = Record.Exception(() => Night.Mouse.SetRelativeMode(true));

      // Assert
      Assert.Null(ex);
    }

    /// <summary>
    /// Tests that <see cref="Night.Mouse.SetRelativeMode(bool)"/> does not throw an exception
    /// when the input system and a (mocked/conceptual) window handle are initialized.
    /// Actual SDL call behavior is not asserted.
    /// </summary>
    /// <remarks>
    /// Similar to SetGrabbed, this tests the C# path.
    /// </remarks>
    /// <param name="enabled">The relative mode state to test.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetRelativeMode_InputAndWindowInitialized_DoesNotThrow(bool enabled)
    {
      // Arrange
      Framework.IsInputInitialized = true;

      // Act
      Exception? ex = Record.Exception(() => Night.Mouse.SetRelativeMode(enabled));

      // Assert
      Assert.Null(ex);
    }
  }
}
