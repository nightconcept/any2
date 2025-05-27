using System;

using SDL3;

// No 'using Night.Types;' as no Night.Types are used in this specific class.

namespace Night;

/// <summary>
/// Provides direct access to SDL3 functions using SDL3-CS bindings.
/// This is an internal-facing or low-level API for the engine.
/// </summary>
public static class NightSDL // Renamed from SDL
{
  /// <summary>
  /// Initializes the SDL library. This must be called before any other SDL functions.
  /// </summary>
  /// <param name="flags">Initialization flags for SDL.</param>
  /// <returns>True on success or false on failure.</returns>
  public static bool Init(SDL.InitFlags flags)
  {
    return SDL.Init(flags);
  }

  /// <summary>
  /// Cleans up all initialized subsystems. You should call this function on application exit.
  /// </summary>
  public static void Quit()
  {
    SDL.Quit();
  }

  /// <summary>
  /// Gets the version of SDL that is linked against.
  /// The SDL3-CS binding for SDL.GetVersion returns a packed int.
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
  /// Gets the last error message that was set for the current thread.
  /// </summary>
  /// <returns>A string containing the last error message.</returns>
  public static string GetError()
  {
    return SDL.GetError();
  }

  // Expose SDL.InitFlags enum for convenience if needed by calling code for Init()
  public static SDL.InitFlags InitVideo => SDL.InitFlags.Video;
  public static SDL.InitFlags InitAudio => SDL.InitFlags.Audio;
  // The Timer subsystem is initialized by default in SDL3 and does not have/need a specific InitFlag.
  // public static SDL.InitFlags InitTimer => SDL.InitFlags.Timer;
  public static SDL.InitFlags InitEvents => SDL.InitFlags.Events;
  // Add other flags as needed or expect the caller to use SDL.InitFlags directly.
}
