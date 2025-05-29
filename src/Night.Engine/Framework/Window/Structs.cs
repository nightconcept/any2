using System;

namespace Night
{
  /// <summary>
  /// Represents the display mode of a window, including width, height, and other properties.
  /// </summary>
  public struct WindowMode
  {
    /// <summary>
    /// Window width.
    /// </summary>
    public int Width;
    /// <summary>
    /// Window height.
    /// </summary>
    public int Height;
    /// <summary>
    /// Fullscreen (true), or windowed (false).
    /// </summary>
    public bool Fullscreen;
    /// <summary>
    /// The type of fullscreen mode used.
    /// </summary>
    public FullscreenType FullscreenType;
    public bool Borderless;

    /// <summary>
    /// 1 if the graphics framerate is synchronized with the monitor's refresh rate, 0 otherwise.
    /// </summary>
    public int Vsync;

    /// <summary>
    /// The number of antialiasing samples used (0 if MSAA is disabled).
    /// </summary>
    public int Msaa;

    /// <summary>
    /// True if the window is resizable in windowed mode, false otherwise.
    /// </summary>
    public bool Resizable;

    /// <summary>
    /// True if the window is centered in windowed mode, false otherwise.
    /// </summary>
    public bool Centered;

    /// <summary>
    /// The index of the display the window is currently in (1-based).
    /// </summary>
    public int Display;

    /// <summary>
    /// The minimum width of the window, if resizable.
    /// </summary>
    public int MinWidth;

    /// <summary>
    /// The minimum height of the window, if resizable.
    /// </summary>
    public int MinHeight;

    /// <summary>
    /// True if high-dpi mode is allowed on Retina displays (macOS).
    /// </summary>
    public bool HighDpi;

    /// <summary>
    /// The refresh rate of the screen's current display mode in Hz (0 if undetermined).
    /// </summary>
    public int RefreshRate;

    /// <summary>
    /// The x-coordinate of the window's position.
    /// </summary>
    public int X;

    /// <summary>
    /// The y-coordinate of the window's position.
    /// </summary>
    public int Y;
  }
}
