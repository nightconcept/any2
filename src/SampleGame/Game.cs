// <copyright file="Game.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;

using Night;

using SDL3;

namespace Night.SampleGame;

public class Game : IGame
{
  private Player player;
  private List<Night.Rectangle> platforms;
  private Night.Sprite? platformSprite;
  private Night.Rectangle goalPlatform;
  private bool goalReachedMessageShown = false; // To ensure message prints only once

  public Game()
  {
    this.player = new Player();
    this.platforms = new List<Night.Rectangle>();
  }

  public void Load()
  {
    // _ = Window.SetMode(800, 600, SDL.WindowFlags.Resizable);
    // Window.SetTitle("Night Platformer Sample");
    // Window settings will now be driven by config.json (or defaults if not present/configured)
    this.player.Load();

    // Load platform sprite
    string baseDirectory = AppContext.BaseDirectory;
    string platformImageRelativePath = Path.Combine("assets", "images", "pixel_green.png");
    string platformImageFullPath = Path.Combine(baseDirectory, platformImageRelativePath);
    this.platformSprite = Graphics.NewImage(platformImageFullPath); // Graphics class will be in Night.Framework
    if (this.platformSprite == null)
    {
      Console.WriteLine($"Game.Load: Failed to load platform sprite at '{platformImageFullPath}'. Platforms will not be drawn.");
    }

    // Initialize platforms (as per docs/epics/epic7-design.md)
    this.platforms.Add(new Night.Rectangle(50, 500, 700, 50));
    this.platforms.Add(new Night.Rectangle(200, 400, 150, 30));
    this.platforms.Add(new Night.Rectangle(450, 300, 100, 30));
    this.goalPlatform = new Night.Rectangle(600, 200, 100, 30);
    this.platforms.Add(this.goalPlatform);
  }

  public void Update(double deltaTime)
  {
    this.player.Update(deltaTime, this.platforms);

    // Check if player reached the goal platform
    // Adjust playerBounds slightly for the goal check to ensure "touching" counts,
    // as player might be perfectly aligned on top.
    Night.Rectangle playerBoundsForGoalCheck = new Night.Rectangle((int)this.player.X, (int)this.player.Y, this.player.Width, this.player.Height + 1);
    if (CheckAABBCollision(playerBoundsForGoalCheck, this.goalPlatform) && !this.goalReachedMessageShown)
    {
      // Simple win condition: print a message.
      // A real game might change state, show a UI, etc.
      Console.WriteLine("Congratulations! Goal Reached!");
      this.goalReachedMessageShown = true; // Set flag so it doesn't print again

      // Optionally, could close the game or trigger another action:
      // Window.Close(); // Window class will be in Night.Framework
    }
  }

  public void Draw()
  {
    Graphics.Clear(new Night.Color(135, 206, 235)); // Sky blue background

    // Draw platforms
    if (this.platformSprite != null)
    {
      foreach (var platform in this.platforms)
      {
        // Scale the 1x1 pixel sprite to the platform's dimensions
        Graphics.Draw(
            this.platformSprite,
            platform.X,
            platform.Y,
            0,
            platform.Width,
            platform.Height);
      }
    }

    this.player.Draw();

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
      new Night.PointF(440, 120),
    };
    Graphics.SetColor(Night.Color.Cyan);
    Graphics.Line(linePoints); // Polyline in Cyan

    // Polygon Demo
    Night.PointF[] triangleVertices = new Night.PointF[]
    {
      new Night.PointF(500, 50),
      new Night.PointF(550, 100),
      new Night.PointF(450, 100),
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
        new Night.PointF(580, 100),
    };
    Graphics.SetColor(new Night.Color(75, 0, 130)); // Indigo
    Graphics.Polygon(Night.DrawMode.Line, pentagonVertices);
  }

  public void KeyPressed(Night.KeySymbol key, Night.KeyCode scancode, bool isRepeat)
  {
    // Minimal key handling for now, primarily for closing the window.
    if (key == Night.KeySymbol.Escape)
    {
      Window.Close();
    }

    // Test error triggering
    if (key == Night.KeySymbol.E && !isRepeat)
    {
      throw new InvalidOperationException("Test error triggered by pressing 'E' in SampleGame!");
    }

    // --- Night.Window Demo: Toggle Fullscreen ---
    if (key == Night.KeySymbol.F11)
    {
      var (isFullscreen, _) = Window.GetFullscreen();
      bool success = Window.SetFullscreen(!isFullscreen, Night.FullscreenType.Desktop);
      Console.WriteLine($"SetFullscreen to {!isFullscreen} (Desktop) attempt: {(success ? "Success" : "Failed")}");
      var newMode = Window.GetMode();
      Console.WriteLine($"New Window Mode: {newMode.Width}x{newMode.Height}, Fullscreen: {newMode.Fullscreen}, Type: {newMode.FullscreenType}, Borderless: {newMode.Borderless}");
    }

    if (key == Night.KeySymbol.F10)
    {
      var (isFullscreen, _) = Window.GetFullscreen();
      bool success = Window.SetFullscreen(!isFullscreen, Night.FullscreenType.Exclusive);
      Console.WriteLine($"SetFullscreen to {!isFullscreen} (Exclusive) attempt: {(success ? "Success" : "Failed")}");
      var newMode = Window.GetMode();
      Console.WriteLine($"New Window Mode: {newMode.Width}x{newMode.Height}, Fullscreen: {newMode.Fullscreen}, Type: {newMode.FullscreenType}, Borderless: {newMode.Borderless}");
    }

    // --- End Night.Window Demo ---
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
}

// Program class removed from here, will be in Program.cs
