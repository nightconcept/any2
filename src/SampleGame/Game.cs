// <copyright file="Game.cs" company="Night Circle">
// zlib license
//
// Copyright (c) 2025 Danny Solivan, Night Circle
//
// This software is provided 'as-is', without any express or implied
// warranty. In no event will the authors be held liable for any damages
// arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software. If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Night;
using Night.Log;
using Night.Log.Sinks;

using SDL3;

namespace SampleGame;

/// <summary>
/// Main game class for the platformer sample.
/// Implements the <see cref="IGame"/> interface for Night.Engine integration.
/// </summary>
public class Game : IGame
{
  private Player player;
  private List<Night.Rectangle> platforms;
  private Night.Sprite? platformSprite;
  private Night.Rectangle goalPlatform;
  private bool goalReachedMessageShown = false; // To ensure message prints only once

  /// <summary>
  /// Initializes a new instance of the <see cref="Game"/> class.
  /// </summary>
  public Game()
  {
    this.player = new Player();
    this.platforms = new List<Night.Rectangle>();
  }

  /// <summary>
  /// Loads game assets and initializes game state.
  /// Called once at the start of the game by the Night.Engine.
  /// </summary>
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
    this.platformSprite = Graphics.NewImage(platformImageFullPath);
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

    // Set the window icon (assuming icon is in assets/icon.ico relative to executable)
    // This path will be resolved by Night.Framework if specified in config.json via IconPath.
    // If not in config, or if this call is made after Framework has set from config,
    // this explicit call can override or set it if not in config.
    // For the sample, we'll rely on the config first, but this shows direct API usage.
    // If you want the SampleGame to ALWAYS use a specific icon regardless of config, call it here.
    // For now, we let config drive it. If you want to test direct SetIcon:
    string iconRelativePath = Path.Combine("assets", "icon.ico");
    string iconFullPath = Path.Combine(AppContext.BaseDirectory, iconRelativePath);
    _ = Window.SetIcon(iconFullPath);
    Console.WriteLine($"Attempted to set icon from Game.Load. Current icon: {Window.GetIcon()}");

    // Run Logger Phase 2 Verification
    //RunLoggerPhase2Verification();

    // Run Logger Phase 3 Verification
    //RunLoggerPhase3Verification();
  }

  private static void RunLoggerPhase2Verification()
  {
    Console.WriteLine("\n--- Running Logger Phase 2 Verification ---");
    var initialLogger = LogManager.GetLogger("InitialTest"); // Used for some initial tests

    // --- 1. LogManager.GetLogger() ---
    Console.WriteLine("\n1. Testing LogManager.GetLogger()...");
    var logger1A = LogManager.GetLogger("TestCategory1");
    Console.WriteLine($"  Got logger1A for 'TestCategory1': {(logger1A != null ? "OK" : "FAIL")}");
    var logger1B = LogManager.GetLogger("TestCategory1");
    Console.WriteLine($"  Got logger1B for 'TestCategory1': {(logger1B != null ? "OK" : "FAIL")}");
    Console.WriteLine($"  logger1A == logger1B: {object.ReferenceEquals(logger1A, logger1B)} (Expected: True)");
    var logger2 = LogManager.GetLogger("TestCategory2");
    Console.WriteLine($"  Got logger2 for 'TestCategory2': {(logger2 != null ? "OK" : "FAIL")}");
    Console.WriteLine($"  logger1A == logger2: {object.ReferenceEquals(logger1A, logger2)} (Expected: False)");

    // --- 2. SystemConsoleSink (Opt-In Behavior) ---
    Console.WriteLine("\n2. Testing SystemConsoleSink (Opt-In Behavior)...");
    Console.WriteLine("  Logging 'Test message 1 (should NOT appear on console)'...");
    initialLogger.Info("Test message 1 (should NOT appear on console)");
    Console.WriteLine("  (Check console output above. Message should NOT be present.)");

    var systemConsoleSink = new SystemConsoleSink();
    LogManager.AddSink(systemConsoleSink);
    Console.WriteLine("  Added SystemConsoleSink.");
    Console.WriteLine("  Logging 'Hello Console! (should APPEAR on console)'...");
    initialLogger.Info("Hello Console! (should APPEAR on console)");
    Console.WriteLine("  (Check console output above. Message SHOULD be present.)");

    LogManager.RemoveSink(systemConsoleSink);
    Console.WriteLine("  Removed SystemConsoleSink.");
    Console.WriteLine("  Logging 'Test message 2 (should NOT appear on console after removal)'...");
    initialLogger.Info("Test message 2 (should NOT appear on console after removal)");
    Console.WriteLine("  (Check console output above. Message should NOT be present.)");

    // --- 3. MemorySink ---
    Console.WriteLine("\n3. Testing MemorySink...");
    var memorySink = new MemorySink(capacity: 5);
    LogManager.AddSink(memorySink);
    Console.WriteLine("  Added MemorySink (capacity 5).");
    var memLogger = LogManager.GetLogger("MemoryTest");
    Console.WriteLine("  Logging 6 messages (Msg1 to Msg6) to MemoryTest logger...");
    for (int i = 1; i <= 6; i++)
    {
      memLogger.Info($"Msg{i}");
    }

    var entries = memorySink.GetEntries().ToList();
    Console.WriteLine($"  Retrieved {entries.Count} entries from MemorySink (Expected: 5)");
    if (entries.Count == 5)
    {
      bool correctEntries = true;
      for (int i = 0; i < 5; i++)
      {
        if (entries[i].Message != $"Msg{i + 2}")
        {
          correctEntries = false;
          Console.WriteLine($"    FAIL: Entry {i} is '{entries[i].Message}', expected 'Msg{i + 2}'");
        }

        Console.WriteLine($"    Entry {i}: [{entries[i].TimestampUtc:o}] [{entries[i].Level}] [{entries[i].CategoryName}] {entries[i].Message}");
      }

      Console.WriteLine($"  Content matches 'Msg2' through 'Msg6': {(correctEntries ? "OK" : "FAIL")}");
    }
    else
    {
      Console.WriteLine("  FAIL: Incorrect number of entries retrieved.");
      foreach (var entry in entries)
      {
        Console.WriteLine($"    Unexpected Entry: [{entry.TimestampUtc:o}] [{entry.Level}] [{entry.CategoryName}] {entry.Message}");
      }
    }

    LogManager.RemoveSink(memorySink); // Clean up for next test
    Console.WriteLine("  Removed MemorySink.");

    // --- 4. Global Minimum Log Level (LogManager.MinLevel) ---
    Console.WriteLine("\n4. Testing Global Minimum Log Level (LogManager.MinLevel)...");
    LogManager.AddSink(systemConsoleSink); // Re-add for this test
    Console.WriteLine("  Added SystemConsoleSink for MinLevel test.");
    var levelTestLogger = LogManager.GetLogger("LevelTest");

    LogManager.MinLevel = LogLevel.Warning;
    Console.WriteLine($"  Set LogManager.MinLevel = LogLevel.Warning ({LogManager.MinLevel}).");
    Console.WriteLine("  Logging Debug A, Info A, Warn A. Expect only Warn A.");
    levelTestLogger.Debug("Debug A (should NOT appear)");
    levelTestLogger.Info("Info A (should NOT appear)");
    levelTestLogger.Warn("Warning A (SHOULD appear)");
    Console.WriteLine("  (Check console output for 'Warning A' ONLY from this batch.)");

    LogManager.MinLevel = LogLevel.Trace;
    Console.WriteLine($"  Set LogManager.MinLevel = LogLevel.Trace ({LogManager.MinLevel}).");
    Console.WriteLine("  Logging Debug B. Expect Debug B to appear.");
    levelTestLogger.Debug("Debug B (SHOULD appear)");
    Console.WriteLine("  (Check console output for 'Debug B' from this batch.)");
    LogManager.RemoveSink(systemConsoleSink); // Clean up
    Console.WriteLine("  Removed SystemConsoleSink after MinLevel test.");

    // --- 5. ClearSinks() ---
    Console.WriteLine("\n5. Testing ClearSinks()...");
    LogManager.AddSink(systemConsoleSink);
    LogManager.AddSink(memorySink); // MemorySink was removed, re-add with default capacity
    Console.WriteLine("  Added SystemConsoleSink and new MemorySink.");
    var clearTestLogger = LogManager.GetLogger("ClearTest");
    Console.WriteLine("  Logging 'Message before ClearSinks (should appear/be captured)'...");
    clearTestLogger.Info("Message before ClearSinks (should appear/be captured)");
    Console.WriteLine("  (Check console for message, MemorySink will have 1 entry)");
    var entriesBeforeClear = memorySink.GetEntries().Count();
    Console.WriteLine($"  MemorySink entries before clear: {entriesBeforeClear} (Expected 1)");

    LogManager.ClearSinks();
    Console.WriteLine("  Called LogManager.ClearSinks().");
    Console.WriteLine("  Logging 'Message after ClearSinks (should NOT appear/be captured)'...");
    clearTestLogger.Info("Message after ClearSinks (should NOT appear/be captured)");
    Console.WriteLine("  (Check console for NO new message. MemorySink should still have its old entries or be empty if re-created differently).");
    var entriesAfterClear = memorySink.GetEntries().Count(); // This memorySink instance is now orphaned from LogManager
    Console.WriteLine($"  Orphaned MemorySink entries after clear: {entriesAfterClear} (Expected: {entriesBeforeClear}, as it's no longer managed)");

    // To properly test MemorySink after ClearSinks, we'd need to re-add it or check a fresh one.
    // The current test confirms no *new* messages go to *any* sink.
    // Let's add the system console sink back to see if it's truly cleared.
    LogManager.AddSink(systemConsoleSink);
    Console.WriteLine("  Re-added SystemConsoleSink.");
    Console.WriteLine("  Logging 'Message after ClearSinks and re-adding SystemConsoleSink (should appear)'...");
    clearTestLogger.Info("Message after ClearSinks and re-adding SystemConsoleSink (should appear)");
    Console.WriteLine("  (Check console for this message.)");
    LogManager.ClearSinks(); // Final cleanup

    Console.WriteLine("\n--- Logger Phase 2 Verification Complete ---");
  }

  private static void RunLoggerPhase3Verification()
  {
    Console.WriteLine("\n--- Running Logger Phase 3 Verification ---");
    var fileTestLogger = LogManager.GetLogger("FileTest");
    var gameConsoleTestLogger = LogManager.GetLogger("GameConsoleTest");
    string testLogFilePath = "test_log.txt";

    // Ensure a clean slate for sinks if any were left from previous tests or game init
    LogManager.ClearSinks();

    // For verification, let's also re-add SystemConsoleSink to see logs in console simultaneously
    var systemConsoleSinkForP3 = new SystemConsoleSink();
    LogManager.AddSink(systemConsoleSinkForP3);
    Console.WriteLine("  (Added SystemConsoleSink for Phase 3 verification visibility)");

    // --- 1. FileSink ---
    Console.WriteLine("\n1. Testing FileSink...");
    if (File.Exists(testLogFilePath))
    {
      File.Delete(testLogFilePath);
      Console.WriteLine($"  Deleted existing '{testLogFilePath}'.");
    }

    var fileSink = new FileSink(testLogFilePath);
    LogManager.AddSink(fileSink);
    Console.WriteLine($"  Added FileSink, logging to '{testLogFilePath}'.");

    Console.WriteLine("  Logging several messages to FileTest logger...");
    fileTestLogger.Info("Log to file - Info message.");
    fileTestLogger.Warn("Log to file - Warning message.");
    try
    {
      throw new InvalidOperationException("Dummy exception for file log.");
    }
    catch (Exception ex)
    {
      fileTestLogger.Error("Log to file - Error message with exception!", ex);
    }

    fileTestLogger.Debug("Log to file - Debug message (will appear if MinLevel is Trace/Debug).");

    Console.WriteLine($"  Please open '{testLogFilePath}' and verify:");
    Console.WriteLine("    - The file was created.");
    Console.WriteLine("    - All logged messages are present.");
    Console.WriteLine("    - Each entry is formatted correctly (timestamp, level, category, message, exception details).");
    Console.WriteLine("  (Press Enter to continue after checking the file...)");
    _ = Console.ReadLine();

    Console.WriteLine("  Logging more messages to append...");
    fileTestLogger.Info("Appending another info message to file.");
    fileTestLogger.Fatal("Appending a fatal message to file!");
    Console.WriteLine($"  Please re-check '{testLogFilePath}' to ensure messages were appended.");
    Console.WriteLine("  (Press Enter to continue after checking the file...)");
    _ = Console.ReadLine();

    LogManager.RemoveSink(fileSink);
    Console.WriteLine("  Removed FileSink.");
    Console.WriteLine("  Logging a message that should NOT go to file...");
    fileTestLogger.Info("This message should NOT be in test_log.txt.");
    Console.WriteLine($"  Please verify '{testLogFilePath}' was NOT modified with the last message.");
    Console.WriteLine("  (Press Enter to continue after checking the file...)");
    _ = Console.ReadLine();

    // Optional: Test with an invalid/unwritable path (conceptual for now as per epic)
    Console.WriteLine("  (Conceptual: Testing FileSink with an unwritable path would log an internal error via System.Diagnostics.Trace, not crash).");

    // --- 2. InGameConsoleSink ---
    Console.WriteLine("\n2. Testing InGameConsoleSink...");
    var gameConsoleSink = new InGameConsoleSink(capacity: 10);
    LogManager.AddSink(gameConsoleSink);
    Console.WriteLine("  Added InGameConsoleSink (capacity 10).");

    Console.WriteLine("  Logging 12 messages (IGMsg1 to IGMsg12) to GameConsoleTest logger...");
    for (int i = 1; i <= 12; i++)
    {
      gameConsoleTestLogger.Info($"IGMsg{i}");
    }

    var gameConsoleEntries = gameConsoleSink.GetEntries().ToList();
    Console.WriteLine($"  Retrieved {gameConsoleEntries.Count} entries from InGameConsoleSink (Expected: 10)");
    if (gameConsoleEntries.Count == 10)
    {
      bool correctGameEntries = true;
      for (int i = 0; i < 10; i++)
      {
        if (gameConsoleEntries[i].Message != $"IGMsg{i + 3}") // Expecting IGMsg3 to IGMsg12
        {
          correctGameEntries = false;
          Console.WriteLine($"    FAIL: Entry {i} is '{gameConsoleEntries[i].Message}', expected 'IGMsg{i + 3}'");
        }

        Console.WriteLine($"    Entry {i}: [{gameConsoleEntries[i].TimestampUtc:o}] [{gameConsoleEntries[i].Level}] [{gameConsoleEntries[i].CategoryName}] {gameConsoleEntries[i].Message}");
      }

      Console.WriteLine($"  Content matches 'IGMsg3' through 'IGMsg12': {(correctGameEntries ? "OK" : "FAIL")}");
    }
    else
    {
      Console.WriteLine("  FAIL: Incorrect number of entries retrieved from InGameConsoleSink.");
      foreach (var entry in gameConsoleEntries)
      {
        Console.WriteLine($"    Unexpected Entry: [{entry.TimestampUtc:o}] [{entry.Level}] [{entry.CategoryName}] {entry.Message}");
      }
    }

    LogManager.RemoveSink(gameConsoleSink);
    Console.WriteLine("  Removed InGameConsoleSink.");

    // Cleanup SystemConsoleSink added for P3 verification
    LogManager.RemoveSink(systemConsoleSinkForP3);
    Console.WriteLine("  (Removed SystemConsoleSink used for Phase 3 verification visibility)");

    // Final cleanup of the test log file
    if (File.Exists(testLogFilePath))
    {
      try
      {
        File.Delete(testLogFilePath);
        Console.WriteLine($"  Cleaned up '{testLogFilePath}'.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"  Warning: Could not delete '{testLogFilePath}'. Error: {ex.Message}");
      }
    }

    Console.WriteLine("\n--- Logger Phase 3 Verification Complete ---");
  }

  /// <summary>
  /// Updates the game state.
  /// Called every frame by the Night.Engine.
  /// </summary>
  /// <param name="deltaTime">The time elapsed since the last frame, in seconds.</param>
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

  /// <summary>
  /// Draws the game scene.
  /// Called every frame by the Night.Engine after Update.
  /// </summary>
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

    // --- Graphics Shape Drawing Demonstration (Top-Left Corner) ---
    // All coordinates and sizes are adjusted to fit in a smaller area.
    // Base offset for the demo shapes
    int demoXOffset = 10;
    int demoYOffset = 10;
    int shapeSize = 20; // General size for smaller shapes
    int spacing = 5;    // Spacing between shapes

    // Rectangle Demo
    Graphics.SetColor(Night.Color.Red);
    Graphics.Rectangle(Night.DrawMode.Fill, demoXOffset, demoYOffset, shapeSize, shapeSize / 2); // Smaller Red Rectangle
    Graphics.SetColor(Night.Color.Black);
    Graphics.Rectangle(Night.DrawMode.Line, demoXOffset, demoYOffset, shapeSize, shapeSize / 2);

    demoXOffset += shapeSize + spacing; // Move right for next shape

    Graphics.SetColor(0, 0, 255, 128); // Semi-transparent Blue
    Graphics.Rectangle(Night.DrawMode.Line, demoXOffset, demoYOffset, shapeSize - 5, shapeSize + 5); // Adjusted Blue Rectangle

    demoXOffset += (shapeSize - 5) + spacing; // Move right

    // Circle Demo
    Graphics.SetColor(Night.Color.Green);
    Graphics.Circle(Night.DrawMode.Fill, demoXOffset + (shapeSize / 2), demoYOffset + (shapeSize / 2), shapeSize / 2); // Smaller Green Circle
    Graphics.SetColor(Night.Color.Black);
    Graphics.Circle(Night.DrawMode.Line, demoXOffset + (shapeSize / 2), demoYOffset + (shapeSize / 2), shapeSize / 2, 12); // 12 segments

    demoXOffset += shapeSize + spacing; // Move right

    Graphics.SetColor(Night.Color.Yellow);
    Graphics.Circle(Night.DrawMode.Line, demoXOffset + (shapeSize / 3), demoYOffset + (shapeSize / 3), shapeSize / 3, 6); // Smaller Hexagon

    // Reset X offset for a new "row" of shapes if needed, or continue right
    // For this demo, we'll just continue right and assume enough horizontal space for this small demo.
    // If more shapes were added, a new row would be demoYOffset += shapeSize + spacing; demoXOffset = 10;
    demoXOffset += (shapeSize / 3 * 2) + spacing; // Move right based on hexagon diameter

    // Line Demo
    Graphics.SetColor(Night.Color.Magenta);
    Graphics.Line(demoXOffset, demoYOffset, demoXOffset + shapeSize, demoYOffset + (shapeSize / 2)); // Smaller Magenta Line

    demoXOffset += shapeSize + spacing;

    Night.PointF[] linePoints = new Night.PointF[]
    {
      new Night.PointF(demoXOffset, demoYOffset),
      new Night.PointF(demoXOffset + (shapeSize / 3), demoYOffset + (shapeSize / 2)),
      new Night.PointF(demoXOffset + (shapeSize * 2 / 3), demoYOffset),
      new Night.PointF(demoXOffset + shapeSize, demoYOffset + (shapeSize / 2)),
    };
    Graphics.SetColor(Night.Color.Cyan);
    Graphics.Line(linePoints); // Smaller Polyline in Cyan

    demoXOffset += shapeSize + spacing;

    // Polygon Demo
    Night.PointF[] triangleVertices = new Night.PointF[]
    {
      new Night.PointF(demoXOffset + (shapeSize / 2), demoYOffset),
      new Night.PointF(demoXOffset + shapeSize, demoYOffset + shapeSize),
      new Night.PointF(demoXOffset, demoYOffset + shapeSize),
    };
    Graphics.SetColor(new Night.Color(255, 165, 0)); // Orange
    Graphics.Polygon(Night.DrawMode.Fill, triangleVertices); // Smaller Orange Triangle
    Graphics.SetColor(Night.Color.Black);
    Graphics.Polygon(Night.DrawMode.Line, triangleVertices);

    demoXOffset += shapeSize + spacing;

    Night.PointF[] pentagonVertices = new Night.PointF[]
    {
        new Night.PointF(demoXOffset + (shapeSize / 2), demoYOffset),
        new Night.PointF(demoXOffset + shapeSize, demoYOffset + (shapeSize / 3)),
        new Night.PointF(demoXOffset + (shapeSize * 2 / 3), demoYOffset + shapeSize),
        new Night.PointF(demoXOffset + (shapeSize / 3), demoYOffset + shapeSize),
        new Night.PointF(demoXOffset, demoYOffset + (shapeSize / 3)),
    };
    Graphics.SetColor(new Night.Color(75, 0, 130)); // Indigo
    Graphics.Polygon(Night.DrawMode.Line, pentagonVertices); // Smaller Pentagon

    // --- Test Large Filled Rectangle ---
    Graphics.SetColor(Night.Color.Blue);
    Graphics.Rectangle(Night.DrawMode.Fill, 300, 200, 200, 150); // Large Blue Filled Rectangle Test

    // --- End Test Large Filled Rectangle ---
  }

  /// <summary>
  /// Handles key press events.
  /// Called by Night.Engine when a key is pressed.
  /// </summary>
  /// <param name="key">The <see cref="Night.KeySymbol"/> of the pressed key.</param>
  /// <param name="scancode">The <see cref="Night.KeyCode"/> (physical key code) of the pressed key.</param>
  /// <param name="isRepeat">True if this is a repeat key event, false otherwise.</param>
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
