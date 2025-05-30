// <copyright file="FullscreenType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Night
{
  /// <summary>
  /// Types of fullscreen modes.
  /// </summary>
  public enum FullscreenType
  {
    /// <summary>
    /// Standard exclusive-fullscreen mode. Changes the display mode (actual resolution) of the monitor.
    /// </summary>
    Exclusive,

    /// <summary>
    /// Borderless fullscreen windowed mode. A borderless screen-sized window is created which sits on top of all desktop UI elements.
    /// The window is automatically resized to match the dimensions of the desktop, and its size cannot be changed.
    /// </summary>
    Desktop,
  }
}
