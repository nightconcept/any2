// <copyright file="NightSDL.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

using SDL3;

namespace Night
{
  /// <summary>
  /// Provides direct access to some SDL3 functions using SDL3-CS bindings.
  /// </summary>
  public static class NightSDL
  {
    /// <summary>
    /// Get the version of SDL that is linked against the Night Engine.
    /// Calls the SDL3-CS binding for SDL_GetVersion() and returns a packed int.
    /// https://wiki.libsdl.org/SDL3/SDL_GetVersion.
    /// </summary>
    /// <returns>A string representing the SDL version "major.minor.patch".</returns>
    public static string GetVersion()
    {
      int sdl_version = SDL.GetVersion();
      int major = sdl_version / 1000000;
      int minor = (sdl_version / 1000) % 1000;
      int patch = sdl_version % 1000;
      return $"{major}.{minor}.{patch}";
    }

    /// <summary>
    /// Retrieve a message about the last error that occurred on the current thread.
    /// Calls the SDL3-CS binding for SDL_GetError() and returns a string.
    /// https://wiki.libsdl.org/SDL3/SDL_GetError.
    /// </summary>
    /// <returns>Returns a message with information about the specific error that occurred, or an empty string if there hasn't been an error message set since the last call to SDL_ClearError().</returns>
    public static string GetError()
    {
      return SDL.GetError();
    }
  }
}
