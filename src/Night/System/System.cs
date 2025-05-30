// <copyright file="System.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Night
{
  using SDL3;

  /// <summary>
  /// Provides access to system-level information and functions.
  /// </summary>
  public static class System
  {
    /// <summary>
    /// Puts text in the system's clipboard.
    /// </summary>
    /// <param name="text">The new text to hold in the system's clipboard.</param>
    /// <returns>True if the operation was successful, false otherwise.</returns>
    public static bool SetClipboardText(string text)
    {
      return SDL.SetClipboardText(text);
    }

    // TODO: Consider adding GetClipboardText if in scope for future versions.
    // public static string GetClipboardText()
    // {
    //     return SDL.GetClipboardText();
    // }
  }
}
