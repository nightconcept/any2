// Namespace for all public Night Engine APIs
namespace Night;

/// <summary>
/// Provides functionality for managing the application window.
/// Mimics Love2D's love.window module.
/// </summary>
public static class Window
{
    // Placeholder for window methods
}

/// <summary>
/// Provides functionality for drawing graphics.
/// Mimics Love2D's love.graphics module.
/// </summary>
public static class Graphics
{
    // Placeholder for graphics methods
}

/// <summary>
/// Provides functionality for handling keyboard input.
/// Mimics Love2D's love.keyboard module.
/// </summary>
public static class Keyboard
{
    // Placeholder for keyboard methods
}

/// <summary>
/// Provides functionality for handling mouse input.
/// Mimics Love2D's love.mouse module.
/// </summary>
public static class Mouse
{
    // Placeholder for mouse methods
}

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
        int versionInt = SDL3.SDL.SDL_GetVersion();
        // This unpacking logic is based on common SDL practice,
        // assuming SDL_VERSIONNUM was used or similar.
        // SDL_COMPILEDVERSION might be different from SDL_VERSIONNUM.
        // For SDL3, the typical way is SDL_VERSIONNUM(X, Y, Z) = (((X)*1000) + ((Y)*100) + (Z))
        // However, SDL_GetVersion() in C returns void and takes SDL_Version*.
        // The SDL3-CS binding `int SDL_GetVersion()` is unusual.
        // Let's assume it's packed as major * 10000 + minor * 100 + patch for now,
        // or a similar scheme. The previous direct PInvoke used /1000, %1000 /100, %100.
        // Let's stick to that known scheme.
        int major = versionInt / 1000;
        int minor = (versionInt % 1000) / 100;
        int patch = versionInt % 100;
        return $"{major}.{minor}.{patch}";
    }

    // Expose SDL_InitFlags enum for convenience if needed by calling code for Init()
    public static SDL3.SDL.SDL_InitFlags InitVideo => SDL3.SDL.SDL_InitFlags.SDL_INIT_VIDEO;
    public static SDL3.SDL.SDL_InitFlags InitAudio => SDL3.SDL.SDL_InitFlags.SDL_INIT_AUDIO;
    public static SDL3.SDL.SDL_InitFlags InitTimer => SDL3.SDL.SDL_InitFlags.SDL_INIT_TIMER;
    public static SDL3.SDL.SDL_InitFlags InitEvents => SDL3.SDL.SDL_InitFlags.SDL_INIT_EVENTS;
    // Add other flags as needed or expect the caller to use SDL3.SDL.SDL_InitFlags directly.
}
