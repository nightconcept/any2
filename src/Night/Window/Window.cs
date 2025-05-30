// <copyright file="Window.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Night
{
  using System;
  using System.Collections.Generic;
  using System.Runtime.InteropServices;

  using SDL3;

  /// <summary>
  /// Provides an interface for modifying and retrieving information about the program's window.
  /// </summary>
  public static class Window
  {
    private static nint window = nint.Zero;
    private static nint renderer = nint.Zero;
    private static bool isVideoInitialized = false;
    private static bool isWindowOpen = false;
    private static FullscreenType currentFullscreenType = FullscreenType.Desktop;

    // Internal accessor for the renderer, to be used by Night.Graphics
    internal static nint RendererPtr => renderer;

    // Internal accessor for the window handle, to be used by other Night modules if needed
    // And for the methods within this class that need the handle, which is now _window.
    internal static nint Handle => window;

    /// <summary>
    ///     Sets the display mode and properties of the window.
    /// </summary>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="flags">SDL Window flags to apply.</param>
    /// <returns>True if the mode was set successfully, false otherwise.</returns>
    public static bool SetMode(int width, int height, SDL.WindowFlags flags)
    {
      if (!isVideoInitialized)
      {
        if (!SDL.InitSubSystem(SDL.InitFlags.Video))
        {
          return false;
        }

        isVideoInitialized = true;
      }

      if (window != nint.Zero) // Clean up existing window and renderer
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

      if (isVideoInitialized)
      {
        SDL.QuitSubSystem(SDL.InitFlags.Video);
        isVideoInitialized = false;
      }

      isWindowOpen = false;
    }

    /// <summary>
    /// Gets the number of connected monitors.
    /// </summary>
    /// <returns>The number of currently connected displays.</returns>
    public static int GetDisplayCount()
    {
      if (!isVideoInitialized)
      {
        EnsureVideoInitialized();
      }

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
      if (!isVideoInitialized)
      {
        EnsureVideoInitialized();
      }

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
    /// <returns>A tuple: (bool IsFullscreen, FullscreenType fsType).
    /// IsFullscreen is true if the window is in any fullscreen mode, false otherwise.
    /// fsType indicates the type of fullscreen mode used.</returns>
    public static (bool isFullscreen, FullscreenType fsType) GetFullscreen()
    {
      if (window == nint.Zero)
      {
        return (false, currentFullscreenType);
      }

      var flags = SDL.GetWindowFlags(window);
      if ((flags & SDL.WindowFlags.Fullscreen) != 0)
      {
        return (true, currentFullscreenType);
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
        else // FullscreenType.Desktop
        {
          if (!SDL.SetWindowFullscreenMode(window, nint.Zero))
          {
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
              return false;
            }
          }
          else
          {
            return false;
          }
        }
      }
      else // Not fullscreen (go windowed)
      {
        currentFullscreenType = FullscreenType.Desktop;
        if (!SDL.SetWindowFullscreenMode(window, nint.Zero))
        {
        }

        if (!SDL.SetWindowBordered(window, true))
        {
          return false;
        }

        _ = SDL.RestoreWindow(window);
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
      if (!isVideoInitialized)
      {
        EnsureVideoInitialized();
      }

      var modesList = new List<(int Width, int Height)>();
      var uniqueModes = new HashSet<(int Width, int Height)>(); // Keep track of unique modes

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
        if (uniqueModes.Add(currentModeTuple)) // Add returns true if the item was added (i.e., it was unique)
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
      if (dpiScale == 0)
      {
        return value;
      }

      return value / dpiScale;
    }

    /// <summary>
    /// Ensures the SDL Video subsystem is initialized.
    /// </summary>
    private static void EnsureVideoInitialized()
    {
      if (!isVideoInitialized)
      {
        if (!SDL.InitSubSystem(SDL.InitFlags.Video))
        {
          string sdlError = SDL.GetError();
          throw new Exception($"SDL Error initializing video subsystem: {sdlError}");
        }

        isVideoInitialized = true;
      }
    }

    /// <summary>
    /// Gets the dimensions of the desktop for a specific display ID.
    /// </summary>
    /// <param name="displayID">The actual ID of the display to query.</param>
    /// <returns>A tuple containing the width and height of the desktop, or (0,0) if an error occurs.</returns>
    private static (int Width, int Height) GetDesktopDimensionsForDisplayID(uint displayID)
    {
      if (!isVideoInitialized)
      {
        EnsureVideoInitialized();
      }

      SDL.DisplayMode? mode = SDL.GetDesktopDisplayMode(displayID);
      if (mode == null)
      {
        return (0, 0);
      }

      return (mode.Value.W, mode.Value.H);
    }
  }
}
