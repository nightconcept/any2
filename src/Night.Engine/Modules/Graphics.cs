using System;
using System.IO; // For File.Exists
using System.Runtime.InteropServices; // For Marshal

using Night; // For Sprite, Color

using SDL3;    // For core SDL functions

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
    IntPtr rendererPtr = Window.RendererPtr;
    if (rendererPtr == IntPtr.Zero)
    {
      Console.WriteLine("Error in Graphics.Draw: Renderer pointer is null. Was Window.SetMode called successfully?");
      return;
    }

    if (sprite == null || sprite.Texture == IntPtr.Zero)
    {
      Console.WriteLine("Error in Graphics.Draw: Sprite or sprite texture is null.");
      return;
    }

    SDL.FRect dstRectStruct = new SDL.FRect
    {
      X = x,
      Y = y,
      W = sprite.Width * scaleX,
      H = sprite.Height * scaleY
    };

    SDL.FPoint centerPointStruct = new SDL.FPoint
    {
      X = offsetX * scaleX,
      Y = offsetY * scaleY
    };

    double angleInDegrees = rotation * (180.0 / Math.PI); // Use Math.PI for double precision

    IntPtr dstRectPtr = IntPtr.Zero;
    IntPtr centerPointPtr = IntPtr.Zero;

    try
    {
      dstRectPtr = Marshal.AllocHGlobal(Marshal.SizeOf(dstRectStruct));
      Marshal.StructureToPtr(dstRectStruct, dstRectPtr, false);

      centerPointPtr = Marshal.AllocHGlobal(Marshal.SizeOf(centerPointStruct));
      Marshal.StructureToPtr(centerPointStruct, centerPointPtr, false);

      // Use IntPtr.Zero for srcrect to draw the entire texture.
      // SDL.RenderTextureRotated expects the angle in degrees.
      // This overload expects IntPtr for dstrect and center.
      if (!SDL.RenderTextureRotated(rendererPtr, sprite.Texture, IntPtr.Zero, dstRectPtr, angleInDegrees, centerPointPtr, SDL.FlipMode.None))
      {
        string sdlError = SDL.GetError();
        Console.WriteLine($"Error in Graphics.Draw (RenderTextureRotated): {sdlError}");
      }
    }
    finally
    {
      if (dstRectPtr != IntPtr.Zero)
      {
        Marshal.FreeHGlobal(dstRectPtr);
      }
      if (centerPointPtr != IntPtr.Zero)
      {
        Marshal.FreeHGlobal(centerPointPtr);
      }
    }
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
    IntPtr rendererPtr = Window.RendererPtr;
    if (rendererPtr == IntPtr.Zero)
    {
      Console.WriteLine("Error in Graphics.Present: Renderer pointer is null. Was Window.SetMode called successfully?");
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
