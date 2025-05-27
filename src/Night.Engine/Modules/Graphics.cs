using System;
using System.IO; // For File.Exists
using System.Runtime.InteropServices; // For Marshal

using Night; // For Sprite, Color

using SDL3;    // For core SDL functions

// SDL3.Image.Load is now used directly, so static import for LoadTexture is not strictly needed
// but can be kept if other SDL3.Image functions are used directly elsewhere.
// For clarity, if only SDL3.Image.Load and SDL3.Image.GetError are used,
// direct calls like SDL3.Image.Load() are clearer.
// using static SDL3.Image;

namespace Night;

/// <summary>
/// Provides functionality for drawing graphics.
/// Mimics Love2D's love.graphics module.
/// </summary>
public static class Graphics
{
  /// <summary>
  /// Creates a new image (Sprite) from a file.
  /// </summary>
  /// <param name="filePath">The path to the image file.</param>
  /// <returns>A new Sprite object.</returns>
  public static Sprite? NewImage(string filePath)
  {
    IntPtr rendererPtr = Window.RendererPtr;
    if (rendererPtr == IntPtr.Zero)
    {
      Console.WriteLine("Error in Graphics.NewImage: Renderer pointer is null. Was Window.SetMode called successfully?");
      return null;
    }

    if (!File.Exists(filePath))
    {
      Console.WriteLine($"Error in Graphics.NewImage: Image file not found at '{filePath}'.");
      return null;
    }

    IntPtr surfacePtr = SDL3.Image.Load(filePath); // Use SDL3.Image.Load directly

    if (surfacePtr == IntPtr.Zero)
    {
      string sdlError = SDL.GetError(); // Use standard SDL.GetError()
      Console.WriteLine($"Error in Graphics.NewImage: Failed to load image into surface from '{filePath}'. SDL_image Error: {sdlError}");
      return null;
    }

    SDL.Surface surface = Marshal.PtrToStructure<SDL.Surface>(surfacePtr);
    int width = surface.Width;
    int height = surface.Height;

    if (width <= 0 || height <= 0)
    {
      Console.WriteLine($"Error: Invalid surface dimensions ({width}x{height}) for '{filePath}'.");
      SDL.DestroySurface(surfacePtr);
      return null;
    }

    IntPtr texturePtr = SDL.CreateTextureFromSurface(rendererPtr, surfacePtr);
    SDL.DestroySurface(surfacePtr); // Surface is no longer needed after texture creation

    if (texturePtr == IntPtr.Zero)
    {
      string sdlError = SDL.GetError();
      Console.WriteLine($"Error in Graphics.NewImage: Failed to create texture from surface for '{filePath}'. SDL Error: {sdlError}");
      // Note: surfacePtr is already destroyed at this point.
      return null;
    }
    return new Sprite(texturePtr, width, height);
  }

  /// <summary>
  /// Draws a sprite to the screen.
  /// </summary>
  /// <param name="sprite">The sprite to draw.</param>
  /// <param name="x">The x-coordinate to draw the sprite at.</param>
  /// <param name="y">The y-coordinate to draw the sprite at.</param>
  /// <param name="rotation">The rotation of the sprite (in radians).</param>
  /// <param name="scaleX">The horizontal scale factor.</param>
  /// <param name="scaleY">The vertical scale factor.</param>
  /// <param name="offsetX">The x-offset for the sprite's origin.</param>
  /// <param name="offsetY">The y-offset for the sprite's origin.</param>
  public static void Draw(
      Sprite sprite,
      float x,
      float y,
      float rotation = 0,
      float scaleX = 1,
      float scaleY = 1,
      float offsetX = 0,
      float offsetY = 0)
  {
    // Implementation for this will be part of Epic 5 (Sprite Rendering)
    throw new NotImplementedException("Graphics.Draw is not yet implemented.");
  }

  /// <summary>
  /// Clears the screen to a specific color.
  /// </summary>
  /// <param name="color">The color to clear the screen with.</param>
  public static void Clear(Color color)
  {
    nint rendererPtr = Window.RendererPtr;
    if (rendererPtr == nint.Zero)
    {
      Console.WriteLine("Error in Graphics.Clear: Renderer pointer is null. Was Window.SetMode called successfully?");
      // Or throw new InvalidOperationException("Renderer not initialized.");
      return;
    }

    if (!SDL.SetRenderDrawColor(rendererPtr, color.R, color.G, color.B, color.A))
    {
      string sdlError = SDL.GetError();
      Console.WriteLine($"Error in Graphics.Clear (SetRenderDrawColor): {sdlError}");
      // Potentially throw an exception or handle error.
    }

    if (!SDL.RenderClear(rendererPtr))
    {
      string sdlError = SDL.GetError();
      Console.WriteLine($"Error in Graphics.Clear (RenderClear): {sdlError}");
      // Potentially throw an exception or handle error.
    }
  }

  /// <summary>
  /// Presents the drawn graphics to the screen (swaps buffers).
  /// </summary>
  public static void Present()
  {
    nint rendererPtr = Window.RendererPtr;
    if (rendererPtr == nint.Zero)
    {
      Console.WriteLine("Error in Graphics.Present: Renderer pointer is null. Was Window.SetMode called successfully?");
      // Or throw new InvalidOperationException("Renderer not initialized.");
      return;
    }

    if (!SDL.RenderPresent(rendererPtr))
    {
      string sdlError = SDL.GetError();
      Console.WriteLine($"Error in Graphics.Present (RenderPresent): {sdlError}");
      // Potentially throw an exception or handle error.
    }
  }
}
