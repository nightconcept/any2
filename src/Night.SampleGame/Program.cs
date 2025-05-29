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
  private Night.Rectangle _goalPlatform; // To store the goal platform details
  private bool _goalReachedMessageShown = false; // To ensure message prints only once

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
    _goalPlatform = new Night.Rectangle(600, 200, 100, 30); // Platform 4 (Goal)
    _platforms.Add(_goalPlatform);

    // Demonstrate Night.Filesystem
    string sampleFilePathRelative = Path.Combine("assets", "data", "sample.txt");
    string sampleFilePathFull = Path.Combine(baseDirectory, sampleFilePathRelative);

    Console.WriteLine($"\n--- Night.Filesystem Demo ---");
    Console.WriteLine($"Checking path: {sampleFilePathFull}");

    Night.FileSystemInfo? info = Night.Filesystem.GetInfo(sampleFilePathFull);
    Console.WriteLine($"Night.Filesystem.GetInfo called for: {sampleFilePathFull}");

    if (info != null)
    {
      Console.WriteLine($"  Info.Type: {info.Type}");
      Console.WriteLine($"  Info.Size: {info.Size?.ToString() ?? "N/A"} bytes");
      Console.WriteLine($"  Info.ModTime: {info.ModTime?.ToString() ?? "N/A"} (Unix Timestamp)");

      bool isFile = info.Type == Night.FileType.File;
      Console.WriteLine($"Is a file: {isFile}");

      bool isDirectory = info.Type == Night.FileType.Directory;
      Console.WriteLine($"Is a directory: {isDirectory}");

      // Example of using filterType
      Night.FileSystemInfo? fileInfoOnly = Night.Filesystem.GetInfo(sampleFilePathFull, Night.FileType.File);
      Console.WriteLine($"GetInfo with FileType.File filter was null: {fileInfoOnly == null}");


      if (info.Type == Night.FileType.File)
      {
        try
        {
          string content = Night.Filesystem.ReadText(sampleFilePathFull);
          Console.WriteLine($"Night.Filesystem.ReadText content:\n{content}");

          byte[] bytes = Night.Filesystem.ReadBytes(sampleFilePathFull);
          Console.WriteLine($"Night.Filesystem.ReadBytes length: {bytes.Length} bytes");
        }
        catch (Exception e)
        {
          Console.WriteLine($"Error reading file with Night.Filesystem: {e.Message}");
        }
      }
    }
    Console.WriteLine($"--- End Night.Filesystem Demo ---\n");
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
      // Night.Window.Close();
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
    // --- Graphics Shape Drawing Demonstration ---
    // Rectangle Demo
    Night.Graphics.SetColor(Night.Color.Red);
    Night.Graphics.Rectangle(Night.DrawMode.Fill, 50, 50, 100, 50); // Filled Red Rectangle
    Night.Graphics.SetColor(Night.Color.Black);
    Night.Graphics.Rectangle(Night.DrawMode.Line, 50, 50, 100, 50); // Black outline for Red Rectangle

    Night.Graphics.SetColor(0, 0, 255, 128); // Semi-transparent Blue
    Night.Graphics.Rectangle(Night.DrawMode.Line, 160, 50, 80, 60); // Outlined Blue Rectangle

    // Circle Demo
    Night.Graphics.SetColor(Night.Color.Green);
    Night.Graphics.Circle(Night.DrawMode.Fill, 300, 80, 30); // Filled Green Circle
    Night.Graphics.SetColor(Night.Color.Black);
    Night.Graphics.Circle(Night.DrawMode.Line, 300, 80, 30, 24); // Black outline, 24 segments

    Night.Graphics.SetColor(Night.Color.Yellow);
    Night.Graphics.Circle(Night.DrawMode.Line, 400, 80, 25, 6); // 6-segment "circle" (hexagon) outline

    // Line Demo
    Night.Graphics.SetColor(Night.Color.Magenta);
    Night.Graphics.Line(50, 120, 250, 150); // Single Magenta Line

    Night.PointF[] linePoints = new Night.PointF[]
    {
      new Night.PointF(280, 120),
      new Night.PointF(320, 160),
      new Night.PointF(360, 120),
      new Night.PointF(400, 160),
      new Night.PointF(440, 120)
    };
    Night.Graphics.SetColor(Night.Color.Cyan);
    Night.Graphics.Line(Night.DrawMode.Line, linePoints); // Polyline in Cyan

    // Polygon Demo
    Night.PointF[] triangleVertices = new Night.PointF[]
    {
      new Night.PointF(500, 50),
      new Night.PointF(550, 100),
      new Night.PointF(450, 100)
    };
    Night.Graphics.SetColor(new Night.Color(255, 165, 0)); // Orange
    Night.Graphics.Polygon(Night.DrawMode.Fill, triangleVertices); // Filled Orange Triangle
    Night.Graphics.SetColor(Night.Color.Black);
    Night.Graphics.Polygon(Night.DrawMode.Line, triangleVertices); // Black outline for Triangle

    Night.PointF[] pentagonVertices = new Night.PointF[]
    {
        new Night.PointF(600, 80),
        new Night.PointF(630, 60),
        new Night.PointF(650, 90),
        new Night.PointF(620, 110),
        new Night.PointF(580, 100)
    };
    Night.Graphics.SetColor(new Night.Color(75, 0, 130)); // Indigo
    Night.Graphics.Polygon(Night.DrawMode.Line, pentagonVertices); // Outlined Indigo Pentagon
    // --- End Graphics Shape Drawing Demonstration ---
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
