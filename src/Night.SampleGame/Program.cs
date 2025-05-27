using System;
using System.IO;

using Night;

using SDL3;

namespace Night.SampleGame;

public class Game : IGame
{
  private Sprite? _testSprite; // To store the loaded image (nullable for CS8618)

  public void Load()
  {
    // Placeholder for loading game assets and initial setup
    Night.Window.SetMode(800, 600, SDL.WindowFlags.Resizable);
    Night.Window.SetTitle("Night Sample Game");

    // Test Night.Graphics.NewImage()
    string baseDirectory = AppContext.BaseDirectory;
    string imageRelativePath = Path.Combine("assets", "images", "test_texture.png");
    string imageFullPath = Path.Combine(baseDirectory, imageRelativePath);

    System.Console.WriteLine($"Attempting to load image from: {imageFullPath}");

    _testSprite = Night.Graphics.NewImage(imageFullPath);
    if (_testSprite != null)
    {
      System.Console.WriteLine(
        $"Successfully loaded '{imageFullPath}'. " +
        $"Width: {_testSprite.Width}, Height: {_testSprite.Height}, " +
        $"TexturePtr: {_testSprite.Texture}"
      );
    }
    else
    {
      System.Console.WriteLine($"Failed to load '{imageFullPath}'. Check console for SDL errors and ensure the file exists and is copied to output.");
    }
  }

  private float _rotationAngle = 0.0f; // Radians

  public void Update(double deltaTime)
  {
    // Placeholder for game logic updates
    // System.Console.WriteLine($"SampleGame: Update, DeltaTime: {deltaTime}");
    _rotationAngle += (float)(0.5 * deltaTime); // Rotate at 0.5 radians per second
    if (_rotationAngle > 2 * Math.PI)
    {
      _rotationAngle -= (float)(2 * Math.PI);
    }
  }

  public void Draw()
  {
    Night.Graphics.Clear(Night.Color.Cyan); // Clear screen to Cyan for testing

    if (_testSprite != null)
    {
      // 1. Draw normally at a fixed position (top-left origin)
      Night.Graphics.Draw(_testSprite, 50, 50);

      // 2. Draw with rotation around its center
      // offsetX and offsetY are relative to the sprite's top-left corner before scaling
      float centerX = _testSprite.Width / 2.0f;
      float centerY = _testSprite.Height / 2.0f;
      Night.Graphics.Draw(_testSprite, 200, 150, _rotationAngle, 1, 1, centerX, centerY);

      // 3. Draw scaled up, at a different position (top-left origin for transformations)
      Night.Graphics.Draw(_testSprite, 400, 50, 0, 1.5f, 1.5f);

      // 4. Draw scaled down, rotated, with offset origin (bottom-right of original sprite as pivot)
      // and different position.
      Night.Graphics.Draw(_testSprite, 500, 300, (float)(Math.PI / 4.0), 0.75f, 0.75f, _testSprite.Width, _testSprite.Height);

      // 5. Draw with different X and Y scaling, and slight rotation, centered origin
      Night.Graphics.Draw(_testSprite, 100, 350, (float)(Math.PI / 6.0), 1.2f, 0.8f, centerX, centerY);

      // 6. Draw with negative scaling (flip) - ensure offset is adjusted if needed for visual centering
      // For example, to flip horizontally and keep it visually centered around its original center:
      // The offsetX for SDL is relative to the (potentially flipped) destination rectangle's corner.
      // If we want the *original* center to be the pivot after flipping,
      // and scaleX is -1, the new 'visual' left edge is where the original right edge was.
      // So, if offsetX was originally sprite.Width/2, for a horizontal flip,
      // the center.X for SDL.RenderTextureRotated should be (sprite.Width * scaleX) - (sprite.Width/2 * scaleX)
      // which simplifies to -sprite.Width/2 if scaleX is -1.
      // However, our Draw function's offsetX/Y are sprite-local *before* scaling.
      // SDL's center point is relative to the top-left of the *final scaled destination rectangle*.
      // Our current implementation: center.X = offsetX * scaleX.
      // If offsetX = sprite.Width/2 and scaleX = -1, then center.X = -sprite.Width/2.
      // This means the rotation point is to the left of the new top-left corner of the flipped image.
      Night.Graphics.Draw(_testSprite, 600, 150, 0, -1, 1, centerX, centerY); // Flipped horizontally, rotating around original center
      Night.Graphics.Draw(_testSprite, 700, 150, 0, 1, -1, centerX, centerY); // Flipped vertically, rotating around original center
    }
    else
    {
      // Optionally, draw some placeholder if sprite failed to load
      // For now, just the black screen.
    }

    // Night.Graphics.Present() is called by the FrameworkLoop after this Draw method.
  }
}

public class Program
{
  public static void Main()
  {
    Night.Framework.Run<Game>();
  }
}
