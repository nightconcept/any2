// <copyright file="WindowMode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Night
{
  /// <summary>
  /// Represents the display mode of a window, including width, height, and other properties.
  /// </summary>
  public struct WindowMode
  {
    /// <summary>
    /// Gets or sets the window width in pixels.
    /// </summary>
    public int Width;

    /// <summary>
    /// Gets or sets the window height in pixels.
    /// </summary>
    public int Height;

    /// <summary>
    /// Gets or sets a value indicating whether the window is in fullscreen mode.
    /// </summary>
    public bool Fullscreen;

    /// <summary>
    /// Gets or sets the type of fullscreen mode used.
    /// </summary>
    public FullscreenType FullscreenType;

    /// <summary>
    /// Gets or sets a value indicating whether the window is borderless.
    /// </summary>
    public bool Borderless;

    /// <summary>
    /// Gets or sets the VSync mode. 1 if the graphics framerate is synchronized with the monitor's refresh rate, 0 otherwise.
    /// </summary>
    public int Vsync;

    /// <summary>
    /// Gets or sets the number of antialiasing samples used (0 if MSAA is disabled).
    /// </summary>
    public int Msaa;

    /// <summary>
    /// Gets or sets a value indicating whether the window is resizable in windowed mode.
    /// </summary>
    public bool Resizable;

    /// <summary>
    /// Gets or sets a value indicating whether the window is centered in windowed mode.
    /// </summary>
    public bool Centered;

    /// <summary>
    /// Gets or sets the 1-based index of the display the window is currently in.
    /// </summary>
    public int Display;

    /// <summary>
    /// Gets or sets the minimum width of the window, if resizable.
    /// </summary>
    public int MinWidth;

    /// <summary>
    /// Gets or sets the minimum height of the window, if resizable.
    /// </summary>
    public int MinHeight;

    /// <summary>
    /// Gets or sets a value indicating whether high-dpi mode is allowed on Retina displays (macOS).
    /// </summary>
    public bool HighDpi;

    /// <summary>
    /// Gets or sets the refresh rate of the screen's current display mode in Hz (0 if undetermined).
    /// </summary>
    public int RefreshRate;

    /// <summary>
    /// Gets or sets the x-coordinate of the window's position.
    /// </summary>
    public int X;

    /// <summary>
    /// Gets or sets the y-coordinate of the window's position.
    /// </summary>
    public int Y;
  }
}
