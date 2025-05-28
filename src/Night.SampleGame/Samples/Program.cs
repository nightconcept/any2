using System;
using System.IO;
using System.Collections.Generic;

using Night;

using SDL3;

namespace Night.SampleGame;

public class Platformer : IGame
{
  private Player _player;
  private List<Night.Rectangle> _platforms;
  private Sprite? _platformSprite;
  private Night.Rectangle _goalPlatform;
  private bool _goalReachedMessageShown = false;

  public Platformer()
  {
    _player = new Player();
    _platforms = new List<Night.Rectangle>();
  }

  public void Load()
  {
    Night.Window.SetMode(800, 600, SDL.WindowFlags.Resizable);
    Night.Window.SetTitle("Night Platformer Sample");

    _player.Load();

    string baseDirectory = AppContext.BaseDirectory;
    string platformImageRelativePath = Path.Combine("assets", "images", "pixel_green.png");
    string platformImageFullPath = Path.Combine(baseDirectory, platformImageRelativePath);
    _platformSprite = Graphics.NewImage(platformImageFullPath);
    if (_platformSprite == null)
    {
      Console.WriteLine($"Game.Load: Failed to load platform sprite at '{platformImageFullPath}'. Platforms will not be drawn.");
    }

    _platforms.Add(new Night.Rectangle(50, 500, 700, 50));
    _platforms.Add(new Night.Rectangle(200, 400, 150, 30));
    _platforms.Add(new Night.Rectangle(450, 300, 100, 30));
    _goalPlatform = new Night.Rectangle(600, 200, 100, 30);
    _platforms.Add(_goalPlatform);
  }

  // Helper for collision detection (AABB)
  private static bool CheckAABBCollision(Night.Rectangle rect1, Night.Rectangle rect2)
  {
    // True if the rectangles are overlapping
    return rect1.X < rect2.X + rect2.Width &&
           rect1.X + rect1.Width > rect2.X &&
           rect1.Y < rect2.Y + rect2.Height &&
           rect1.Y + rect1.Height > rect2.Y;
  }

  public void Update(double deltaTime)
  {
    _player.Update(deltaTime, _platforms);

    Night.Rectangle playerBoundsForGoalCheck = new Night.Rectangle((int)_player.X, (int)_player.Y, _player.Width, _player.Height + 1);
    if (CheckAABBCollision(playerBoundsForGoalCheck, _goalPlatform) && !_goalReachedMessageShown)
    {
      // Simple win condition: print a message.
      Console.WriteLine("Congratulations! Goal Reached!");
      _goalReachedMessageShown = true;
    }
  }

  public void Draw()
  {
    Night.Graphics.Clear(new Night.Color(135, 206, 235)); // Sky blue background

    // Draw platforms
    if (_platformSprite != null)
    {
      foreach (var platform in _platforms)
      {
        // Scale the 1x1 pixel sprite to the platform's dimensions
        Graphics.Draw(
            sprite: _platformSprite,
            x: platform.X,
            y: platform.Y,
            rotation: 0,
            scaleX: platform.Width,
            scaleY: platform.Height
        );
      }
    }

    _player.Draw();
    // Player and Level drawing logic will go here in later tasks.
  }

  public void KeyPressed(Night.KeySymbol key, Night.KeyCode scancode, bool isRepeat)
  {
    // Minimal key handling for now, primarily for closing the window.
    // System.Console.WriteLine($"SampleGame: KeyPressed - KeySymbol: {key}, Scancode: {scancode}, IsRepeat: {isRepeat}");
    if (key == Night.KeySymbol.Escape)
    {
      System.Console.WriteLine("SampleGame: Escape key pressed, closing window.");
      Night.Window.Close();
    }
    // Player input (movement, jump) will be handled in Player.Update using Night.Keyboard.IsDown().
  }
}

public class PlatformerGame
{
  public static void Main()
  {
    Night.Framework.Run(new Platformer());
  }
}
