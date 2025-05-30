// <copyright file="MouseButton.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Night
{
  /// <summary>
  /// Represents mouse buttons. Values correspond to SDL_MouseButtonFlags/
  /// SDL3.SDL.Button* constants.
  /// (e.g., Left is 1, Middle is 2, etc.)
  /// </summary>
  public enum MouseButton
  {
    /// <summary>
    /// An unknown or unspecified mouse button.
    /// </summary>
    Unknown = 0, // Not a direct SDL button constant

    /// <summary>
    /// The left mouse button.
    /// </summary>
    Left = 1,    // Corresponds to SDL.ButtonLeft

    /// <summary>
    /// The middle mouse button.
    /// </summary>
    Middle = 2,  // Corresponds to SDL.ButtonMiddle

    /// <summary>
    /// The right mouse button.
    /// </summary>
    Right = 3,   // Corresponds to SDL.ButtonRight

    /// <summary>
    /// The first extra mouse button (often "back").
    /// </summary>
    X1 = 4,      // Corresponds to SDL.ButtonX1 (Typically "back")

    /// <summary>
    /// The second extra mouse button (often "forward").
    /// </summary>
    X2 = 5,       // Corresponds to SDL.ButtonX2 (Typically "forward")
  }
}
