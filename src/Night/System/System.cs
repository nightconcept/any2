// <copyright file="System.cs" company="Night Circle">
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

using SDL3;

namespace Night
{

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

    /// <summary>
    /// Gets the current text in the system's clipboard.
    /// </summary>
    /// <returns>Clipboard text as a string.</returns>
    public static string GetClipboardText()
    {
      return SDL.GetClipboardText();
    }

    /// <summary>
    /// Gets the current operating system.
    /// This function is similar to LÖVE's love.system.getOS().
    /// </summary>
    /// <returns>
    /// The current operating system: "OS X", "Windows", "Linux", "Android", or "iOS".
    /// Returns the raw platform string from SDL if the OS is not one of the above.
    /// </returns>
    public static string GetOS()
    {
      string sdlPlatform = SDL.GetPlatform();
      switch (sdlPlatform)
      {
        case "Windows":
          return "Windows";
        case "Mac OS X":
          return "OS X";
        case "Linux":
          return "Linux";
        case "Android":
          return "Android";
        case "iOS":
          return "iOS";
        default:
          // If SDL returns something unexpected, pass it through.
          // This helps in identifying new/unhandled platforms.
          return sdlPlatform;
      }
    }
  }
}
