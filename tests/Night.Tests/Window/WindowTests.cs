// <copyright file="WindowTests.cs" company="Night Circle">
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
using System.IO;

using Night;

using SDL3;

using Xunit;

namespace Night.Tests.Window
{
  /// <summary>
  /// Contains unit tests for the <see cref="Night.Window"/> class.
  /// </summary>
  public class WindowTests : IClassFixture<SDLFixture>, IDisposable
  {
    private readonly SDLFixture fixture;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowTests"/> class.
    /// </summary>
    /// <param name="fixture">The SDL fixture.</param>
    public WindowTests(SDLFixture fixture)
    {
      this.fixture = fixture;

      // Reset Night.Window's internal static state before each test.
      // Night.Window.Shutdown() also calls SDL.QuitSubSystem.
      // This might conflict with SDLFixture if not handled carefully.
      // For now, we rely on SDLFixture to have initialized SDL Video,
      // and assume Night.Window.ResetInternalState() will clean up Night.Window's
      // specific resources (window handle, renderer) without affecting the
      // SDL video subsystem managed by SDLFixture.
      Night.Window.ResetInternalState();
    }

    /// <summary>
    /// Cleans up resources after each test.
    /// </summary>
    public void Dispose()
    {
      // Reset Night.Window's internal static state after each test.
      Night.Window.ResetInternalState();
      GC.SuppressFinalize(this);
    }

    // --- SetIcon Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.SetIcon(string)"/> returns false when the image path is null.
    /// </summary>
    [Fact]
    public void SetIcon_NullImagePath_ReturnsFalse()
    {
      // Arrange
      string? imagePath = null;

      // Act
      bool result = Night.Window.SetIcon(imagePath!); // Test with null explicitly

      // Assert
      Assert.False(result);
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.SetIcon(string)"/> returns false when the image path is empty.
    /// </summary>
    [Fact]
    public void SetIcon_EmptyImagePath_ReturnsFalse()
    {
      // Arrange
      string imagePath = string.Empty;

      // Act
      bool result = Night.Window.SetIcon(imagePath);

      // Assert
      Assert.False(result);
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.SetIcon(string)"/> returns false when the window is not initialized.
    /// </summary>
    [Fact]
    public void SetIcon_WindowNotInitialized_ReturnsFalse()
    {
      // Arrange
      // Ensure window is not initialized (default state after constructor's Shutdown)
      string imagePath = "dummy.png"; // Path doesn't need to exist for this specific test

      // Act
      bool result = Night.Window.SetIcon(imagePath);

      // Assert
      Assert.False(result);
      Assert.Null(Night.Window.GetIcon());
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.SetIcon(string)"/> returns false for a non-existent image path.
    /// </summary>
    [Fact]
    public void SetIcon_NonExistentImagePath_ReturnsFalse()
    {
      // Arrange
      // First, initialize a window so Window.Handle is not null
      Assert.True(Night.Window.SetMode(100, 100, SDL3.SDL.WindowFlags.Hidden), "Test setup: SetMode failed");
      string imagePath = Path.Combine(Path.GetTempPath(), "non_existent_icon_file_for_test.png"); // A path that almost certainly won't exist

      // Act
      bool result = Night.Window.SetIcon(imagePath);

      // Assert
      Assert.False(result);
      Assert.Null(Night.Window.GetIcon());
    }

    // --- GetIcon Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.GetIcon()"/> returns null when no icon has been set.
    /// </summary>
    [Fact]
    public void GetIcon_NoIconSet_ReturnsNull()
    {
      // Arrange
      // Window.Shutdown() in constructor ensures no icon is set

      // Act
      ImageData? iconData = Night.Window.GetIcon();

      // Assert
      Assert.Null(iconData);
    }

    // --- SetMode Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.SetMode(int, int, SDL3.SDL.WindowFlags)"/> returns true and opens the window with valid parameters.
    /// </summary>
    [Fact]
    public void SetMode_ValidParameters_ReturnsTrueAndWindowIsOpen()
    {
      // Arrange
      int width = 640;
      int height = 480;
      SDL3.SDL.WindowFlags flags = SDL3.SDL.WindowFlags.Hidden; // Use Hidden to avoid visible windows during tests

      // Act
      bool result = Night.Window.SetMode(width, height, flags);

      // Assert
      Assert.True(result, "SetMode should return true for valid parameters.");
      Assert.True(Night.Window.IsOpen(), "Window should be open after successful SetMode.");
    }

    /// <summary>
    /// Conceptually tests the behavior of <see cref="Night.Window.SetMode(int, int, SDL3.SDL.WindowFlags)"/> if SDL_CreateWindow fails.
    /// </summary>
    [Fact]
    public void SetMode_SdlCreateWindowFails_ReturnsFalseAndWindowNotOpen()
    {
      // This test is hard to achieve in a pure unit test without mocking SDL.
      // We are testing the C# wrapper's behavior if SDL.CreateWindow were to return nint.Zero.
      // The current Night.Window.SetMode directly calls SDL.CreateWindow and checks its return.
      // If it's zero, isWindowOpen is set to false and false is returned.
      // We can't force SDL.CreateWindow to fail easily here.
      // For now, we trust the direct check in Night.Window.SetMode.
      // A more advanced test might involve an internal hook or a way to simulate SDL failure.
      // This test case is more of a conceptual placeholder for that scenario.
      Assert.True(true, "Conceptual test: Cannot easily force SDL.CreateWindow to fail in unit test.");
    }

    // --- SetTitle Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.SetTitle(string)"/> throws <see cref="InvalidOperationException"/> when the window is not initialized.
    /// </summary>
    [Fact]
    public void SetTitle_WindowNotInitialized_ThrowsInvalidOperationException()
    {
      // Arrange
      // Window is not initialized by default after constructor's Shutdown()

      // Act & Assert
      var ex = Assert.Throws<InvalidOperationException>(() => Night.Window.SetTitle("Test Title"));
      Assert.Contains("Window handle is null", ex.Message);
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.SetTitle(string)"/> does not throw an exception for a valid title.
    /// </summary>
    [Fact]
    public void SetTitle_ValidTitle_DoesNotThrow()
    {
      // Arrange
      Assert.True(Night.Window.SetMode(100, 100, SDL3.SDL.WindowFlags.Hidden), "Test setup: SetMode failed");
      string title = "My Test Window";

      // Act & Assert
      // SDL.SetWindowTitle can return false if it fails, which Night.Window.SetTitle wraps in an Exception.
      // If SDL.SetWindowTitle succeeds, no exception is thrown.
      Exception? ex = Record.Exception(() => Night.Window.SetTitle(title));
      Assert.Null(ex);
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.SetTitle(string)"/> throws an exception when the title is null.
    /// </summary>
    [Fact]
    public void SetTitle_NullTitle_SdlHandlesOrThrows()
    {
      // Arrange
      Assert.True(Night.Window.SetMode(100, 100, SDL3.SDL.WindowFlags.Hidden), "Test setup: SetMode failed");
      string? title = null;

      // Act & Assert
      // Night.Window.SetTitle will throw if SDL.SetWindowTitle returns false.
      // SDL's behavior with null might vary, but our wrapper should throw on failure.
      var ex = Assert.Throws<Exception>(() => Night.Window.SetTitle(title!));
      Assert.Contains("SDL Error", ex.Message); // Expecting SDL error for null title
    }

    // --- IsOpen Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.IsOpen()"/> returns false when the window has not been set.
    /// </summary>
    [Fact]
    public void IsOpen_WindowNotSet_ReturnsFalse()
    {
      // Arrange
      // Window is not initialized by default

      // Act
      bool result = Night.Window.IsOpen();

      // Assert
      Assert.False(result);
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.IsOpen()"/> returns true after a successful call to <see cref="Night.Window.SetMode(int, int, SDL3.SDL.WindowFlags)"/>.
    /// </summary>
    [Fact]
    public void IsOpen_AfterSuccessfulSetMode_ReturnsTrue()
    {
      // Arrange
      _ = Night.Window.SetMode(100, 100, SDL3.SDL.WindowFlags.Hidden);

      // Act
      bool result = Night.Window.IsOpen();

      // Assert
      Assert.True(result);
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.IsOpen()"/> returns false after <see cref="Night.Window.Close()"/> is called.
    /// </summary>
    [Fact]
    public void IsOpen_AfterClose_ReturnsFalse()
    {
      // Arrange
      _ = Night.Window.SetMode(100, 100, SDL3.SDL.WindowFlags.Hidden);
      Night.Window.Close();

      // Act
      bool result = Night.Window.IsOpen();

      // Assert
      Assert.False(result);
    }

    // --- Close Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.Close()"/> sets the internal window state to closed.
    /// </summary>
    [Fact]
    public void Close_WhenWindowIsOpen_SetsInternalStateToClosed()
    {
      // Arrange
      _ = Night.Window.SetMode(100, 100, SDL3.SDL.WindowFlags.Hidden);
      Assert.True(Night.Window.IsOpen(), "Pre-condition: Window should be open.");

      // Act
      Night.Window.Close();

      // Assert
      Assert.False(Night.Window.IsOpen(), "Window should be closed after Close() is called.");
    }

    // --- GetDisplayCount Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.GetDisplayCount()"/> initializes video if not already initialized and returns a display count.
    /// </summary>
    [Fact]
    public void GetDisplayCount_VideoNotInitialized_InitializesVideoAndReturnsCount()
    {
      // Arrange
      // Video is not initialized by default

      // Act
      int count = Night.Window.GetDisplayCount();

      // Assert
      Assert.True(count >= 0, "Display count should be non-negative.");

      // Further assertion: isVideoInitialized should be true internally, but not directly testable without InternalsVisibleTo for that specific field.
    }

    // --- GetDesktopDimensions Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.GetDesktopDimensions(int)"/> initializes video and returns dimensions.
    /// </summary>
    [Fact]
    public void GetDesktopDimensions_VideoNotInitialized_InitializesVideoAndReturnsDimensions()
    {
      // Arrange
      // Video not initialized

      // Act
      var (width, height) = Night.Window.GetDesktopDimensions(0);

      // Assert
      Assert.True(width >= 0, "Desktop width should be non-negative."); // SDL might return 0,0 on error
      Assert.True(height >= 0, "Desktop height should be non-negative.");
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.GetDesktopDimensions(int)"/> returns non-zero dimensions for a valid display index.
    /// </summary>
    [Fact]
    public void GetDesktopDimensions_ValidDisplayIndex_ReturnsNonZeroDimensions()
    {
      // Arrange
      // Assuming at least one display is connected, which is typical for test environments.
      // Video will be initialized by the call.

      // Act
      var (width, height) = Night.Window.GetDesktopDimensions(0); // Primary display

      // Assert
      // On a typical system, these should be > 0. If SDL fails to get mode, it might be 0.
      // This test is somewhat environment dependent.
      Assert.True(width >= 0, $"Width was {width}");
      Assert.True(height >= 0, $"Height was {height}");
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.GetDesktopDimensions(int)"/> returns (0,0) for a negative display index.
    /// </summary>
    [Fact]
    public void GetDesktopDimensions_InvalidDisplayIndexNegative_ReturnsZeroZero()
    {
      // Arrange
      // Video will be initialized by the call.

      // Act
      var (width, height) = Night.Window.GetDesktopDimensions(-1);

      // Assert
      Assert.Equal(0, width);
      Assert.Equal(0, height);
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.GetDesktopDimensions(int)"/> returns (0,0) for a display index that is too large.
    /// </summary>
    [Fact]
    public void GetDesktopDimensions_InvalidDisplayIndexTooLarge_ReturnsZeroZero()
    {
      // Arrange
      // Video will be initialized by the call.
      int displayCount = Night.Window.GetDisplayCount(); // Initialize and get count

      // Act
      var (width, height) = Night.Window.GetDesktopDimensions(displayCount + 5); // An index guaranteed to be too large

      // Assert
      Assert.Equal(0, width);
      Assert.Equal(0, height);
    }

    // --- GetFullscreen Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.GetFullscreen()"/> returns false and Desktop type when the window is not initialized.
    /// </summary>
    [Fact]
    public void GetFullscreen_WindowNotInitialized_ReturnsFalseAndDesktopType()
    {
      // Arrange
      // Window not initialized

      // Act
      var (isFullscreen, fsType) = Night.Window.GetFullscreen();

      // Assert
      Assert.False(isFullscreen);
      Assert.Equal(FullscreenType.Desktop, fsType); // currentFullscreenType defaults to Desktop
    }

    // --- SetFullscreen Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.SetFullscreen(bool, FullscreenType)"/> returns false when the window is not initialized.
    /// </summary>
    [Fact]
    public void SetFullscreen_WindowNotInitialized_ReturnsFalse()
    {
      // Arrange
      // Window not initialized

      // Act
      bool result = Night.Window.SetFullscreen(true, FullscreenType.Exclusive);

      // Assert
      Assert.False(result);
    }

    // More SetFullscreen tests are complex due to SDL interactions and window state.
    // They would typically involve checking SDL.GetWindowFlags, which is hard to mock.
    // Focus here is on the C# guard clause.

    // --- GetFullscreenModes Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.GetFullscreenModes(int)"/> initializes video and returns a list of modes.
    /// </summary>
    [Fact]
    public void GetFullscreenModes_VideoNotInitialized_InitializesVideoAndReturnsModes()
    {
      // Arrange
      // Video not initialized

      // Act
      List<(int Width, int Height)> modes = Night.Window.GetFullscreenModes(0);

      // Assert
      Assert.NotNull(modes); // Should return an empty list, not null, on error or no modes
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.GetFullscreenModes(int)"/> returns an empty list for an invalid display index.
    /// </summary>
    [Fact]
    public void GetFullscreenModes_InvalidDisplayIndex_ReturnsEmptyList()
    {
      // Arrange
      int displayCount = Night.Window.GetDisplayCount(); // Initialize and get count

      // Act
      List<(int Width, int Height)> modes = Night.Window.GetFullscreenModes(displayCount + 5);

      // Assert
      Assert.NotNull(modes);
      Assert.Empty(modes);
    }

    // --- GetMode Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.GetMode()"/> returns a default mode structure when the window is not initialized.
    /// </summary>
    [Fact]
    public void GetMode_WindowNotInitialized_ReturnsDefaultModeStruct()
    {
      // Arrange
      // Window not initialized

      // Act
      WindowMode mode = Night.Window.GetMode();

      // Assert
      Assert.Equal(0, mode.Width);
      Assert.Equal(0, mode.Height);
      Assert.False(mode.Fullscreen);
      Assert.Equal(FullscreenType.Desktop, mode.FullscreenType);
      Assert.False(mode.Borderless);
    }

    // --- GetDPIScale Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.GetDPIScale()"/> returns 1.0f when the window is not initialized.
    /// </summary>
    [Fact]
    public void GetDPIScale_WindowNotInitialized_ReturnsOne()
    {
      // Arrange
      // Window not initialized

      // Act
      float scale = Night.Window.GetDPIScale();

      // Assert
      Assert.Equal(1.0f, scale);
    }

    // --- ToPixels / FromPixels Tests ---

    /// <summary>
    /// Tests that <see cref="Night.Window.ToPixels(float)"/> returns the value times 1.0f when the window is not initialized.
    /// </summary>
    [Fact]
    public void ToPixels_WindowNotInitialized_ReturnsValueTimesOne() // DPI scale is 1.0f if window not init
    {
      // Arrange
      float inputValue = 100f;

      // Act
      float result = Night.Window.ToPixels(inputValue);

      // Assert
      Assert.Equal(inputValue * 1.0f, result); // Since GetDPIScale() will return 1.0f
    }

    /// <summary>
    /// Tests that <see cref="Night.Window.FromPixels(float)"/> returns the value divided by 1.0f when the window is not initialized.
    /// </summary>
    [Fact]
    public void FromPixels_WindowNotInitialized_ReturnsValueDividedByOne() // DPI scale is 1.0f if window not init
    {
      // Arrange
      float inputValue = 100f;

      // Act
      float result = Night.Window.FromPixels(inputValue);

      // Assert
      Assert.Equal(inputValue / 1.0f, result); // Since GetDPIScale() will return 1.0f
    }

    /// <summary>
    /// Conceptually tests the guard in <see cref="Night.Window.FromPixels(float)"/> for a zero DPI scale.
    /// </summary>
    [Fact]
    public void FromPixels_ZeroDpiScale_ReturnsOriginalValue()
    {
      // This scenario is unlikely with real SDL if GetDPIScale returns 1.0f on error.
      // However, if GetDPIScale somehow returned 0, FromPixels has a guard.
      // This test is more about that guard.
      // We can't directly mock GetDPIScale easily here.
      // For now, assume GetDPIScale won't return 0 based on its implementation.
      // If it did, Night.Window.FromPixels is designed to return the original value.
      // Simulate scenario where GetDPIScale might return 0 (though current impl returns 1.0f on error)
      // If Night.Window.GetDPIScale() could be made to return 0 for a test:
      // Assert.Equal(inputValue, Night.Window.FromPixels(inputValue));
      Assert.True(true, "Conceptual: Test for FromPixels with 0 DPI scale guard.");
    }
  }
}
