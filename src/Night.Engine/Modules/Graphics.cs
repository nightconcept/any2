using System;
using Night.Types;
using SDL3;

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
  public static Sprite NewImage(string filePath)
  {
    // Implementation for this will be part of Epic 5 (Texture Loading)
    throw new NotImplementedException("Graphics.NewImage is not yet implemented.");
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
