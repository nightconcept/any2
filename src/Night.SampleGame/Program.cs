using System; // For AppContext
using System.IO; // For Path.Combine
using Night;     // For IGame, KeyCode, MouseButton, Sprite, Color etc.

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

  public void Update(double deltaTime)
  {
    // Placeholder for game logic updates
    // System.Console.WriteLine($"SampleGame: Update, DeltaTime: {deltaTime}");
  }

  public void Draw()
  {
    // Placeholder for drawing game elements
    // System.Console.WriteLine("SampleGame: Draw");
  }
}

public class Program
{
  public static void Main()
  {
    Night.Framework.Run<Game>();
  }
}
