using System;
using System.IO;
using System.Collections.Generic; // Required for List<Rectangle>

using Night;

using SDL3;

namespace Night.SampleGame;

public class Game : IGame
{
  private Player _player;
  private List<Night.Rectangle> _platforms;
  private Sprite? _platformSprite;

  public Game()
  {
    _player = new Player();
    _platforms = new List<Night.Rectangle>();
  }

  public void Load()
  {
    Night.Window.SetMode(800, 600, SDL.WindowFlags.Resizable);
    Night.Window.SetTitle("Night Platformer Sample"); // Updated title

    _player.Load();

    // Load platform sprite
    string baseDirectory = AppContext.BaseDirectory;
    string platformImageRelativePath = Path.Combine("assets", "images", "pixel_green.png");
    string platformImageFullPath = Path.Combine(baseDirectory, platformImageRelativePath);
    _platformSprite = Graphics.NewImage(platformImageFullPath);
    if (_platformSprite == null)
    {
      Console.WriteLine($"Game.Load: Failed to load platform sprite at '{platformImageFullPath}'. Platforms will not be drawn.");
    }

    // Initialize platforms (as per docs/epics/epic7-design.md)
    _platforms.Add(new Night.Rectangle(50, 500, 700, 50));  // Platform 1 (Ground)
    _platforms.Add(new Night.Rectangle(200, 400, 150, 30)); // Platform 2
    _platforms.Add(new Night.Rectangle(450, 300, 100, 30)); // Platform 3
    _platforms.Add(new Night.Rectangle(600, 200, 100, 30)); // Platform 4 (Goal)
  }

  public void Update(double deltaTime)
  {
    _player.Update(deltaTime, _platforms);
    // Player and Level update logic will go here in later tasks.
    // For now, this can remain minimal.
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
            _platformSprite,
            platform.X,
            platform.Y,
            0, // rotation
            platform.Width,  // scaleX
            platform.Height  // scaleY
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

public class Program
{
  public static void Main()
  {
    Night.Framework.Run(new Game());
  }
}
