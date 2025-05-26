using System;

// No 'using static SDL3.SDL;' here as SDL3.SDL members are fully qualified.
// No 'using Night.Types;' as no Night.Types are used in this specific class.

namespace Night;

/// <summary>
/// Provides direct access to SDL3 functions using SDL3-CS bindings.
/// This is an internal-facing or low-level API for the engine.
/// </summary>
public static class SDL
{
  /// <summary>
  /// Initializes the SDL library. This must be called before any other SDL functions.
  /// </summary>
  /// <param name="flags">Initialization flags for SDL.</param>
  /// <returns>0 on success or a negative error code on failure.</returns>
  public static int Init(SDL3.SDL.SDL_InitFlags flags)
  {
    // SDL3.SDL.SDL_Init returns an SDLBool, which implicitly converts to bool.
    // We convert this to 0 for success, <0 for failure.
    return SDL3.SDL.SDL_Init(flags) ? 0 : -1;
  }

  /// <summary>
  /// Cleans up all initialized subsystems. You should call this function on application exit.
  /// </summary>
  public static void Quit()
  {
    SDL3.SDL.SDL_Quit();
  }

  /// <summary>
  /// Gets the version of SDL that is linked against.
  /// The SDL3-CS binding for SDL_GetVersion returns a packed int.
  /// </summary>
  /// <returns>A string representing the SDL version "major.minor.patch".</returns>
  public static string GetVersion()
  {
    int sdl_version = SDL3.SDL.SDL_GetVersion();
    int major = sdl_version / 1000000;
    int minor = (sdl_version / 1000) % 1000;
    int patch = sdl_version % 1000;
    return $"{major}.{minor}.{patch}";
  }

  // Expose SDL_InitFlags enum for convenience if needed by calling code for Init()
  public static SDL3.SDL.SDL_InitFlags InitVideo => SDL3.SDL.SDL_InitFlags.SDL_INIT_VIDEO;
  public static SDL3.SDL.SDL_InitFlags InitAudio => SDL3.SDL.SDL_InitFlags.SDL_INIT_AUDIO;
  public static SDL3.SDL.SDL_InitFlags InitTimer => SDL3.SDL.SDL_InitFlags.SDL_INIT_TIMER;
  public static SDL3.SDL.SDL_InitFlags InitEvents => SDL3.SDL.SDL_InitFlags.SDL_INIT_EVENTS;
  // Add other flags as needed or expect the caller to use SDL3.SDL.SDL_InitFlags directly.
}
