// <copyright file="Window.cs" company="Night Circle">
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
using System.Runtime.InteropServices;

using SDL3;

namespace Night
{
  /// <summary>
  /// Provides an interface for modifying and retrieving information about the program's window.
  /// </summary>
  public static class Window
  {
    private static nint window = nint.Zero;
    private static nint renderer = nint.Zero;

    // private static bool isVideoInitialized = false; // Removed: SDL lifecycle managed externally (e.g., by SDLFixture or Framework.Run)
    private static bool isWindowOpen = false;
    private static FullscreenType currentFullscreenType = FullscreenType.Desktop;
    private static ImageData? currentIconData = null;

    /// <summary>
    /// Gets the pointer to the internal SDL renderer. For use by Night.Graphics.
    /// </summary>
    internal static nint RendererPtr => renderer;

    /// <summary>
    /// Gets the handle to the internal SDL window. For use by other Night modules or internal methods.
    /// </summary>
    internal static nint Handle => window;

    /// <summary>
    /// Sets the window icon.
    /// </summary>
    /// <param name="imagePath">The path to the icon image file (e.g., .ico, .png, .bmp).
    /// Uses SDL_image for loading, so supports various formats.</param>
    /// <returns>True if the icon was set successfully, false otherwise.</returns>
    public static bool SetIcon(string imagePath)
    {
      currentIconData = null;

      if (window == nint.Zero)
      {
        Console.WriteLine("Night.Window.SetIcon: Window handle is null. Icon not set.");
        return false;
      }

      if (string.IsNullOrEmpty(imagePath))
      {
        Console.WriteLine("Night.Window.SetIcon: imagePath is null or empty. Icon not set.");
        return false;
      }

      _ = SDL.ClearError();
      nint loadedSurfacePtr = SDL3.Image.Load(imagePath);
      if (loadedSurfacePtr == nint.Zero)
      {
        string imgError = SDL.GetError();
        Console.WriteLine($"Night.Window.SetIcon: Failed to load image '{imagePath}' using SDL_image. Error: {imgError}");
        return false;
      }

      SDL.PixelFormat targetFormatEnum = SDL.PixelFormat.RGBA8888;
      nint convertedSurfacePtr = SDL.ConvertSurface(loadedSurfacePtr, targetFormatEnum);

      if (convertedSurfacePtr == nint.Zero)
      {
        string sdlError = SDL.GetError();
        Console.WriteLine($"Night.Window.SetIcon: Failed to convert surface to target format. SDL Error: {sdlError}");
        SDL.DestroySurface(loadedSurfacePtr);
        return false;
      }

      try
      {
        if (!SDL.SetWindowIcon(window, convertedSurfacePtr))
        {
          string sdlError = SDL.GetError();
          Console.WriteLine($"Night.Window.SetIcon: SDL_SetWindowIcon failed. SDL Error: {sdlError}");
          return false;
        }

        SDL.Surface convertedSurfaceStruct = Marshal.PtrToStructure<SDL.Surface>(convertedSurfacePtr);
        int width = convertedSurfaceStruct.Width;
        int height = convertedSurfaceStruct.Height;

        IntPtr detailsPtr = SDL.GetPixelFormatDetails(convertedSurfaceStruct.Format);
        if (detailsPtr == IntPtr.Zero)
        {
          string sdlError = SDL.GetError();
          Console.WriteLine($"Night.Window.SetIcon: Failed to get pixel format details. SDL Error: {sdlError}");
          return false;
        }

        SDL.PixelFormatDetails pixelFormatDetails = Marshal.PtrToStructure<SDL.PixelFormatDetails>(detailsPtr);
        int bytesPerPixel = pixelFormatDetails.BytesPerPixel;

        if (bytesPerPixel != 4)
        {
          Console.WriteLine($"Night.Window.SetIcon: Converted surface is not 4bpp as expected for RGBA. Actual bpp: {bytesPerPixel}, Format: {convertedSurfaceStruct.Format}");
          return false;
        }

        byte[] pixelData = new byte[width * height * bytesPerPixel];
        Marshal.Copy(convertedSurfaceStruct.Pixels, pixelData, 0, pixelData.Length);

        currentIconData = new ImageData(width, height, pixelData);
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine($"Night.Window.SetIcon: Error processing surface or creating ImageData. Error: {e.Message}");
        return false;
      }
      finally
      {
        if (convertedSurfacePtr != nint.Zero)
        {
          SDL.DestroySurface(convertedSurfacePtr);
        }

        if (loadedSurfacePtr != nint.Zero)
        {
          SDL.DestroySurface(loadedSurfacePtr);
        }
      }
    }

    /// <summary>
    /// Gets the image data of the currently set window icon.
    /// </summary>
    /// <returns>The <see cref="ImageData"/> of the icon, or null if no icon has been set or an error occurred.</returns>
    public static ImageData? GetIcon()
    {
      return currentIconData;
    }

    /// <summary>
    ///     Sets the display mode and properties of the window.
    /// </summary>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="flags">SDL Window flags to apply.</param>
    /// <returns>True if the mode was set successfully, false otherwise.</returns>
    public static bool SetMode(int width, int height, SDL.WindowFlags flags)
    {
      // Assuming SDL Video subsystem is initialized by the caller (e.g., Framework.Run or SDLFixture for tests)
      // if (!isVideoInitialized) // Removed
      // {
      //   if (!SDL.InitSubSystem(SDL.InitFlags.Video)) // Removed
      //   {
      //     return false;
      //   }
      //   isVideoInitialized = true; // Removed
      // }
      if (window != nint.Zero)
      {
        if (renderer != nint.Zero)
        {
          SDL.DestroyRenderer(renderer);
          renderer = nint.Zero;
        }

        SDL.DestroyWindow(window);
        window = nint.Zero;
        isWindowOpen = false;
      }

      window = SDL.CreateWindow("Night Engine", width, height, flags);
      if (window == nint.Zero)
      {
        isWindowOpen = false;
        return false;
      }

      renderer = SDL.CreateRenderer(window, null);
      if (renderer == nint.Zero)
      {
        SDL.DestroyWindow(window);
        window = nint.Zero;
        isWindowOpen = false;
        return false;
      }

      isWindowOpen = true;
      return true;
    }

    /// <summary>
    /// Sets the window title.
    /// </summary>
    /// <param name="title">The new window title.</param>
    public static void SetTitle(string title)
    {
      if (window == nint.Zero)
      {
        string errorMsg = "Error in Night.Window.SetTitle: Window handle is null. Was SetMode called successfully?";
        throw new InvalidOperationException(errorMsg);
      }

      if (!SDL.SetWindowTitle(window, title))
      {
        string sdlError = SDL.GetError();
        throw new Exception($"SDL Error in Night.Window.SetTitle: {sdlError}");
      }
    }

    /// <summary>
    /// Checks if the window is open.
    /// </summary>
    /// <returns>True if the window is open, false otherwise.</returns>
    public static bool IsOpen()
    {
      return isWindowOpen && window != nint.Zero;
    }

    /// <summary>
    /// Signals that the window should close.
    /// This is typically called by the engine when a quit event is received.
    /// TODO: Does this need to align with Love2D more? https://love2d.org/wiki/love.window.close.
    /// </summary>
    public static void Close()
    {
      isWindowOpen = false;
    }

    /// <summary>
    /// Gets the number of connected monitors.
    /// </summary>
    /// <returns>The number of currently connected displays.</returns>
    public static int GetDisplayCount()
    {
      // Assuming SDL Video subsystem is initialized by the caller
      // if (!isVideoInitialized) // Removed
      // {
      //   EnsureVideoInitialized(); // Removed
      // }
      uint[]? displays = SDL.GetDisplays(out int count);
      if (displays == null || count < 0)
      {
        return 0;
      }

      return count;
    }

    /// <summary>
    /// Gets the width and height of the desktop.
    /// </summary>
    /// <param name="displayIndex">The index of the display to query (0 for the primary display).</param>
    /// <returns>A tuple containing the width and height of the desktop, or (0,0) if an error occurs.</returns>
    public static (int Width, int Height) GetDesktopDimensions(int displayIndex = 0)
    {
      // Assuming SDL Video subsystem is initialized by the caller
      // if (!isVideoInitialized) // Removed
      // {
      //   EnsureVideoInitialized(); // Removed
      // }
      uint[]? actualDisplayIDs = SDL.GetDisplays(out int displayCount);
      if (actualDisplayIDs == null || displayCount <= 0 || displayIndex < 0 || displayIndex >= displayCount)
      {
        return (0, 0);
      }

      uint targetDisplayID = actualDisplayIDs[displayIndex];

      SDL.DisplayMode? mode = SDL.GetDesktopDisplayMode(targetDisplayID);
      if (mode == null)
      {
        return (0, 0);
      }

      return (mode.Value.W, mode.Value.H);
    }

    /// <summary>
    /// Gets whether the window is fullscreen.
    /// </summary>
    /// <returns>A tuple: (bool IsFullscreen, FullscreenType FsType).
    /// IsFullscreen is true if the window is in any fullscreen mode, false otherwise.
    /// FsType indicates the type of fullscreen mode used.</returns>
    public static (bool IsFullscreen, FullscreenType FsType) GetFullscreen()
    {
      if (window == nint.Zero)
      {
        return (false, currentFullscreenType);
      }

      var flags = SDL.GetWindowFlags(window);

      // Check for SDL's native/exclusive fullscreen first
      if ((flags & SDL.WindowFlags.Fullscreen) != 0)
      {
        return (true, FullscreenType.Exclusive);
      }

      // Check for our "Desktop Fullscreen" mode
      if (currentFullscreenType == FullscreenType.Desktop && (flags & SDL.WindowFlags.Borderless) != 0)
      {
        return (true, FullscreenType.Desktop);
      }

      return (false, currentFullscreenType);
    }

    /// <summary>
    /// Enters or exits fullscreen. The display to use when entering fullscreen is chosen
    /// based on which display the window is currently in, if multiple monitors are connected.
    /// </summary>
    /// <param name="fullscreen">Whether to enter or exit fullscreen mode.</param>
    /// <param name="fsType">The type of fullscreen mode to use (Desktop or Exclusive).</param>
    /// <returns>True if the operation was successful, false otherwise.</returns>
    public static bool SetFullscreen(bool fullscreen, FullscreenType fsType = FullscreenType.Desktop)
    {
      if (window == nint.Zero)
      {
        return false;
      }

      if (fullscreen)
      {
        currentFullscreenType = fsType;
        if (fsType == FullscreenType.Exclusive)
        {
          uint displayID = SDL.GetDisplayForWindow(window);
          if (displayID == 0 && SDL.GetError() != null && SDL.GetError().Length > 0)
          {
            return false;
          }

          SDL.DisplayMode? dm = SDL.GetDesktopDisplayMode(displayID);
          if (dm.HasValue)
          {
            if (!SDL.SetWindowFullscreenMode(window, dm.Value))
            {
              return false;
            }
          }
          else
          {
            return false;
          }
        }
        else
        {
          if (!SDL.SetWindowFullscreenMode(window, nint.Zero))
          {
            // This might not be critical if it fails.
          }

          if (!SDL.SetWindowBordered(window, false))
          {
            return false;
          }

          uint displayID = SDL.GetDisplayForWindow(window);
          if (displayID == 0 && SDL.GetError() != null && SDL.GetError().Length > 0)
          {
            return false;
          }

          var (desktopW, desktopH) = GetDesktopDimensionsForDisplayID(displayID);

          if (desktopW > 0 && desktopH > 0)
          {
            _ = SDL.SetWindowPosition(window, 0, 0);
            if (!SDL.SetWindowSize(window, desktopW, desktopH))
            {
              // Even if this fails to resize, we've set it borderless.
              // The issue of it not resizing is separate from the borderless toggle.
            }
          }
          else
          {
            return false;
          }
        }
      }
      else
      {
        currentFullscreenType = FullscreenType.Desktop; // Conceptually, when we exit, we are aiming for a non-fullscreen desktop window.
        _ = SDL.SetWindowFullscreenMode(window, nint.Zero); // Turn off SDL's exclusive fullscreen

        if (!SDL.SetWindowBordered(window, true))
        {
          return false;
        }

        _ = SDL.RestoreWindow(window);
        _ = SDL.SetWindowSize(window, 800, 600); // Explicitly set a defined windowed size.
        _ = SDL.RaiseWindow(window);
      }

      return true;
    }

    /// <summary>
    /// Gets a list of available fullscreen display modes for a given display.
    /// </summary>
    /// <param name="displayIndex">The index of the display (0 for primary).</param>
    /// <returns>A list of (Width, Height) tuples representing available modes, or an empty list on error.</returns>
    public static List<(int Width, int Height)> GetFullscreenModes(int displayIndex = 0)
    {
      // Assuming SDL Video subsystem is initialized by the caller
      // if (!isVideoInitialized) // Removed
      // {
      //   EnsureVideoInitialized(); // Removed
      // }
      var modesList = new List<(int Width, int Height)>();
      var uniqueModes = new HashSet<(int Width, int Height)>();

      uint[]? actualDisplayIDs = SDL.GetDisplays(out int displayCount);
      if (actualDisplayIDs == null || displayCount <= 0 || displayIndex < 0 || displayIndex >= displayCount)
      {
        return modesList;
      }

      uint targetDisplayID = actualDisplayIDs[displayIndex];

      SDL.DisplayMode[]? displayModes = SDL.GetFullscreenDisplayModes(targetDisplayID, out int count);

      if (displayModes == null || count <= 0 || displayModes.Length != count)
      {
        return modesList;
      }

      foreach (var mode in displayModes)
      {
        var currentModeTuple = (mode.W, mode.H);
        if (uniqueModes.Add(currentModeTuple))
        {
          modesList.Add(currentModeTuple);
        }
      }

      return modesList;
    }

    /// <summary>
    /// Gets the current window mode (width, height, and flags).
    /// </summary>
    /// <returns>A WindowMode struct containing width, height, and current flags.</returns>
    public static WindowMode GetMode()
    {
      if (window == nint.Zero)
      {
        return new WindowMode { Width = 0, Height = 0, Fullscreen = false, FullscreenType = currentFullscreenType, Borderless = false };
      }

      _ = SDL.GetWindowSize(window, out int w, out int h);
      var flags = SDL.GetWindowFlags(window);

      bool isSdlExclusiveFullscreen = (flags & SDL.WindowFlags.Fullscreen) != 0;
      bool isSdlBorderless = (flags & SDL.WindowFlags.Borderless) != 0;
      FullscreenType reportedFsType = currentFullscreenType;

      bool actualReportedFullscreenState;

      if (isSdlExclusiveFullscreen)
      {
        actualReportedFullscreenState = true;
        reportedFsType = FullscreenType.Exclusive;
      }
      else if (isSdlBorderless)
      {
        if (currentFullscreenType == FullscreenType.Desktop)
        {
          uint currentDisplayID = SDL.GetDisplayForWindow(window);
          if (currentDisplayID != 0)
          {
            var (desktopW, desktopH) = GetDesktopDimensionsForDisplayID(currentDisplayID);
            if (w == desktopW && h == desktopH)
            {
              actualReportedFullscreenState = true;
            }
            else
            {
              actualReportedFullscreenState = false;
            }
          }
          else
          {
            actualReportedFullscreenState = false;
          }
        }
        else
        {
          actualReportedFullscreenState = false;
        }
      }
      else
      {
        actualReportedFullscreenState = false;
      }

      return new WindowMode
      {
        Width = w,
        Height = h,
        Fullscreen = actualReportedFullscreenState,
        FullscreenType = reportedFsType,
        Borderless = isSdlBorderless,
      };
    }

    /// <summary>
    /// Gets the DPI scale factor of the display containing the window.
    /// </summary>
    /// <returns>The DPI scale factor, or 1.0f on error or if not applicable.</returns>
    public static float GetDPIScale()
    {
      if (window == nint.Zero)
      {
        return 1.0f;
      }

      float dpiScale = SDL.GetWindowDisplayScale(window);
      if (dpiScale <= 0f)
      {
        return 1.0f;
      }

      return dpiScale;
    }

    /// <summary>
    /// Converts a value from density-independent units to pixels, using the window's current DPI scale.
    /// </summary>
    /// <param name="value">The value in density-independent units.</param>
    /// <returns>The equivalent value in pixels.</returns>
    public static float ToPixels(float value)
    {
      return value * GetDPIScale();
    }

    /// <summary>
    /// Converts a value from pixels to density-independent units, using the window's current DPI scale.
    /// </summary>
    /// <param name="value">The value in pixels.</param>
    /// <returns>The equivalent value in density-independent units.</returns>
    public static float FromPixels(float value)
    {
      float dpiScale = GetDPIScale();
      if (dpiScale == 0f)
      {
        return value;
      }

      return value / dpiScale;
    }

    /// <summary>
    /// Internal method to shut down the window and renderer, and quit the video subsystem.
    /// Should be called by the FrameworkLoop at the end of the application.
    /// </summary>
    internal static void Shutdown()
    {
      if (renderer != nint.Zero)
      {
        SDL.DestroyRenderer(renderer);
        renderer = nint.Zero;
      }

      if (window != nint.Zero)
      {
        SDL.DestroyWindow(window);
        window = nint.Zero;
      }

      // Do not call SDL.QuitSubSystem here. Lifecycle managed externally.
      // if (isVideoInitialized) // Removed
      // {
      //   SDL.QuitSubSystem(SDL.InitFlags.Video); // Removed
      //   isVideoInitialized = false; // Removed
      // }
      isWindowOpen = false;
    }

    /// <summary>
    /// Resets the internal static state of the Window class without quitting the SDL video subsystem.
    /// This is intended for use in testing scenarios where the SDL lifecycle is managed externally.
    /// </summary>
    internal static void ResetInternalState()
    {
      if (renderer != nint.Zero)
      {
        SDL.DestroyRenderer(renderer);
        renderer = nint.Zero;
      }

      if (window != nint.Zero)
      {
        SDL.DestroyWindow(window);
        window = nint.Zero;
      }

      // Do not call SDL.QuitSubSystem(SDL.InitFlags.Video) here.
      // Only reset the internal flag for Night.Window's own state.
      // isVideoInitialized = false; // Removed as the field is removed.
      isWindowOpen = false;
      currentIconData = null;
      currentFullscreenType = FullscreenType.Desktop;
    }

    // EnsureVideoInitialized() method removed as Night.Window no longer manages SDL video subsystem init.

    /// <summary>
    /// Gets the dimensions of the desktop for a specific display ID.
    /// </summary>
    /// <param name="displayID">The actual ID of the display to query.</param>
    /// <returns>A tuple containing the width and height of the desktop, or (0,0) if an error occurs.</returns>
    private static (int Width, int Height) GetDesktopDimensionsForDisplayID(uint displayID)
    {
      // Assuming SDL Video subsystem is initialized by the caller
      // if (!isVideoInitialized) // Removed
      // {
      //   EnsureVideoInitialized(); // Removed
      // }
      SDL.DisplayMode? mode = SDL.GetDesktopDisplayMode(displayID);
      if (mode == null)
      {
        return (0, 0);
      }

      return (mode.Value.W, mode.Value.H);
    }
  }
}
