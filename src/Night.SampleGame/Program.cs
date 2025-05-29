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
  private Night.Sprite? _platformSprite;
  private Night.Rectangle _goalPlatform;
  private bool _goalReachedMessageShown = false; // To ensure message prints only once

  public Game()
  {
    _player = new Player();
    _platforms = new List<Night.Rectangle>();
  }

  public void Load()
  {
    Window.SetMode(800, 600, SDL.WindowFlags.Resizable);
    Window.SetTitle("Night Platformer Sample");

    _player.Load();

    // Load platform sprite
    string baseDirectory = AppContext.BaseDirectory;
    string platformImageRelativePath = Path.Combine("assets", "images", "pixel_green.png");
    string platformImageFullPath = Path.Combine(baseDirectory, platformImageRelativePath);
    _platformSprite = Graphics.NewImage(platformImageFullPath); // Graphics class will be in Night.Framework
    if (_platformSprite == null)
    {
      Console.WriteLine($"Game.Load: Failed to load platform sprite at '{platformImageFullPath}'. Platforms will not be drawn.");
    }

    // Initialize platforms (as per docs/epics/epic7-design.md)
    _platforms.Add(new Night.Rectangle(50, 500, 700, 50));
    _platforms.Add(new Night.Rectangle(200, 400, 150, 30));
    _platforms.Add(new Night.Rectangle(450, 300, 100, 30));
    _goalPlatform = new Night.Rectangle(600, 200, 100, 30);
    _platforms.Add(_goalPlatform);

    // Demonstrate Night.Filesystem
    string sampleFilePathRelative = Path.Combine("assets", "data", "sample.txt");
    string sampleFilePathFull = Path.Combine(baseDirectory, sampleFilePathRelative);
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

    // Check if player reached the goal platform
    // Adjust playerBounds slightly for the goal check to ensure "touching" counts,
    // as player might be perfectly aligned on top.
    Night.Rectangle playerBoundsForGoalCheck = new Night.Rectangle((int)_player.X, (int)_player.Y, _player.Width, _player.Height + 1);
    if (CheckAABBCollision(playerBoundsForGoalCheck, _goalPlatform) && !_goalReachedMessageShown)
    {
      // Simple win condition: print a message.
      // A real game might change state, show a UI, etc.
      Console.WriteLine("Congratulations! Goal Reached!");
      _goalReachedMessageShown = true; // Set flag so it doesn't print again
      // Optionally, could close the game or trigger another action:
      // Window.Close(); // Window class will be in Night.Framework
    }
  }

  public void Draw()
  {
    Graphics.Clear(new Night.Color(135, 206, 235)); // Sky blue background

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
            0,
            platform.Width,
            platform.Height
        );
      }
    }

    _player.Draw();
    // --- Graphics Shape Drawing Demonstration ---
    // Rectangle Demo
    Graphics.SetColor(Night.Color.Red);
    Graphics.Rectangle(Night.DrawMode.Fill, 50, 50, 100, 50); // Filled Red Rectangle
    Graphics.SetColor(Night.Color.Black);
    Graphics.Rectangle(Night.DrawMode.Line, 50, 50, 100, 50); // Black outline for Red Rectangle

    Graphics.SetColor(0, 0, 255, 128); // Semi-transparent Blue
    Graphics.Rectangle(Night.DrawMode.Line, 160, 50, 80, 60); // Outlined Blue Rectangle

    // Circle Demo
    Graphics.SetColor(Night.Color.Green);
    Graphics.Circle(Night.DrawMode.Fill, 300, 80, 30); // Filled Green Circle
    Graphics.SetColor(Night.Color.Black);
    Graphics.Circle(Night.DrawMode.Line, 300, 80, 30, 24); // Black outline, 24 segments

    Graphics.SetColor(Night.Color.Yellow);
    Graphics.Circle(Night.DrawMode.Line, 400, 80, 25, 6); // 6-segment "circle" (hexagon) outline

    // Line Demo
    Graphics.SetColor(Night.Color.Magenta);
    Graphics.Line(50, 120, 250, 150); // Single Magenta Line

    Night.PointF[] linePoints = new Night.PointF[]
    {
      new Night.PointF(280, 120),
      new Night.PointF(320, 160),
      new Night.PointF(360, 120),
      new Night.PointF(400, 160),
      new Night.PointF(440, 120)
    };
    Graphics.SetColor(Night.Color.Cyan);
    Graphics.Line(Night.DrawMode.Line, linePoints); // Polyline in Cyan

    // Polygon Demo
    Night.PointF[] triangleVertices = new Night.PointF[]
    {
      new Night.PointF(500, 50),
      new Night.PointF(550, 100),
      new Night.PointF(450, 100)
    };
    Graphics.SetColor(new Night.Color(255, 165, 0)); // Orange
    Graphics.Polygon(Night.DrawMode.Fill, triangleVertices); // Filled Orange Triangle
    Graphics.SetColor(Night.Color.Black);
    Graphics.Polygon(Night.DrawMode.Line, triangleVertices); // Black outline for Triangle

    Night.PointF[] pentagonVertices = new Night.PointF[]
    {
        new Night.PointF(600, 80),
        new Night.PointF(630, 60),
        new Night.PointF(650, 90),
        new Night.PointF(620, 110),
        new Night.PointF(580, 100)
    };
    Graphics.SetColor(new Night.Color(75, 0, 130)); // Indigo
    Graphics.Polygon(Night.DrawMode.Line, pentagonVertices);
  }

  private double _lastTimerPrintTime = 0;
  private const double TimerPrintInterval = 1.0; // Print every 1 second


  public void KeyPressed(Night.KeySymbol key, Night.KeyCode scancode, bool isRepeat)
  {
    // Minimal key handling for now, primarily for closing the window.
    // System.Console.WriteLine($"SampleGame: KeyPressed - KeySymbol: {key}, Scancode: {scancode}, IsRepeat: {isRepeat}");
    if (key == Night.KeySymbol.Escape)
    {
      Window.Close(); // Window class will be in Night.Framework
    }
  }
}

public class Program
{
  public static void Main()
  {
    Framework.Run(new Game());
  }
}
