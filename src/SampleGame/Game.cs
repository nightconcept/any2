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
using System.Threading;
using System.Threading.Tasks;

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
    // RunLoggerPhase2Verification();

    // Run Logger Phase 3 Verification
    // RunLoggerPhase3Verification();

    // Run Logger Phase 4 Verification
    RunLoggerPhase4Verification();
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

  private static void RunLoggerPhase4Verification()
  {
    Console.WriteLine("\n--- Running Logger Phase 4 Verification ---");
    LogManager.MinLevel = LogLevel.Trace; // Ensure a baseline for tests

    // --- 1. Thread Safety ---
    Console.WriteLine("\n1. Testing Thread Safety (`LogManager` and Sinks)...");
    LogManager.ClearSinks(); // Start clean
    LogManager.EnableSystemConsoleSink(true);
    string threadTestLogFile = "thread_test_log.txt";
    if (File.Exists(threadTestLogFile))
    {
      File.Delete(threadTestLogFile);
    }

    LogManager.ConfigureFileSink(threadTestLogFile, LogLevel.Trace);
    var memorySinkThreadTest = new MemorySink(capacity: 2000);
    LogManager.AddSink(memorySinkThreadTest);
    Console.WriteLine("  Configured SystemConsoleSink, FileSink (thread_test_log.txt), and MemorySink (cap 2000).");

    int numThreads = 5;
    int messagesPerThread = 100;
    List<Task> tasks = new List<Task>();

    Console.WriteLine($"  Spawning {numThreads} threads, each logging {messagesPerThread} messages...");
    for (int t = 0; t < numThreads; t++)
    {
      tasks.Add(Task.Run(() =>
      {
        var logger = LogManager.GetLogger($"Thread-{Thread.CurrentThread.ManagedThreadId}");
        for (int i = 0; i < messagesPerThread; i++)
        {
          logger.Info($"Message {i} from thread {Thread.CurrentThread.ManagedThreadId}.");
          if (i % 10 == 0)
          {
            logger.Debug($"Debug detail {i} from thread {Thread.CurrentThread.ManagedThreadId}.");
          }

          if (i % 25 == 0)
          {
            logger.Warn($"Warning {i} from thread {Thread.CurrentThread.ManagedThreadId}!");
          }
        }
      }));
    }

    Task.WhenAll(tasks).GetAwaiter().GetResult(); // Wait for all threads to complete
    Console.WriteLine("  All threads completed.");

    Console.WriteLine("\n  VERIFY Thread Safety:");
    Console.WriteLine("  - Application Stability: Should have completed without deadlocks or exceptions.");
    Console.WriteLine("  - Console Output: Messages from different threads should appear. Complete messages should not be garbled (some interleaving of whole messages is OK).");
    Console.WriteLine($"  - File Output ('{threadTestLogFile}'):");
    Console.WriteLine("    - File should not be corrupted.");
    Console.WriteLine("    - Messages from all threads should be present and formatted correctly.");
    Console.WriteLine($"    - Total messages should be around {numThreads * messagesPerThread} (adjust for debug/warn counts).");
    Console.WriteLine("  - MemorySink Output:");
    var threadEntries = memorySinkThreadTest.GetEntries().ToList();
    Console.WriteLine($"    - Retrieved {threadEntries.Count} entries (Expected up to {memorySinkThreadTest.GetEntries().Count()}). Check for consistency.");
    Console.WriteLine("  (Press Enter to continue after verification...)");
    _ = Console.ReadLine();

    LogManager.DisableFileSink();
    LogManager.ClearSinks(); // Clears memory sink too
    if (File.Exists(threadTestLogFile))
    {
      File.Delete(threadTestLogFile);
    }

    Console.WriteLine("  Cleaned up thread safety test resources.");

    // --- 2. Sink Error Handling (FileSink) ---
    Console.WriteLine("\n2. Testing Sink Error Handling (FileSink)...");
    LogManager.ClearSinks();
    LogManager.EnableSystemConsoleSink(true);
    string errorTestLogFile = "error_test_log.txt";
    if (File.Exists(errorTestLogFile))
    {
      File.Delete(errorTestLogFile);
    }

    LogManager.ConfigureFileSink(errorTestLogFile, LogLevel.Information);
    var errorLogger = LogManager.GetLogger("ErrorTest");

    Console.WriteLine("  Logging 'Initial message - should go to console and file.'");
    errorLogger.Info("Initial message - should go to console and file.");
    Console.WriteLine($"  (Verify console and '{errorTestLogFile}'. Press Enter...)");
    _ = Console.ReadLine();

    Console.WriteLine($"  Simulating FileSink Write Error by making '{errorTestLogFile}' read-only...");
    try
    {
      File.SetAttributes(errorTestLogFile, FileAttributes.ReadOnly);
      Console.WriteLine($"  '{errorTestLogFile}' is now read-only.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"  Could not set file to read-only: {ex.Message}. Skipping read-only part of test.");
    }

    Console.WriteLine("  Logging 'Second message - should go to console, FileSink might fail.'");
    errorLogger.Warn("Second message - should go to console, FileSink might fail.");
    Console.WriteLine("\n  VERIFY Sink Error Handling:");
    Console.WriteLine("  - Second message SHOULD appear in SystemConsoleSink output.");
    Console.WriteLine("  - Application should NOT crash.");
    Console.WriteLine("  - Check System.Diagnostics.Trace output (IDE Debug Output) for a FileSink error message (e.g., 'Access to the path... is denied').");
    Console.WriteLine("  (Press Enter to continue...)");
    _ = Console.ReadLine();

    Console.WriteLine("  Restoring FileSink Operation...");
    try
    {
      File.SetAttributes(errorTestLogFile, FileAttributes.Normal);
      Console.WriteLine($"  '{errorTestLogFile}' attributes set to Normal.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"  Could not reset file attributes: {ex.Message}.");
    }

    // To be absolutely sure, reconfigure, which creates a new stream writer.
    LogManager.ConfigureFileSink(errorTestLogFile, LogLevel.Information);

    Console.WriteLine("  Logging 'Third message - should go to console and restored file.'");
    errorLogger.Info("Third message - should go to console and restored file.");
    Console.WriteLine($"  (Verify console and '{errorTestLogFile}' for the third message. Press Enter...)");
    _ = Console.ReadLine();

    LogManager.DisableFileSink();
    LogManager.EnableSystemConsoleSink(false); // Disable for next test section
    if (File.Exists(errorTestLogFile))
    {
      File.Delete(errorTestLogFile);
    }

    Console.WriteLine("  Cleaned up sink error handling test resources.");

    // --- 3. Sink Configuration Mechanism ---
    Console.WriteLine("\n3. Testing Sink Configuration Mechanism...");
    LogManager.ClearSinks();
    var configLogger = LogManager.GetLogger("ConfigTest");

    Console.WriteLine("  Testing SystemConsoleSink Enable/Disable:");
    LogManager.EnableSystemConsoleSink(true);
    Console.WriteLine("  SystemConsoleSink enabled. Logging 'Msg1: Console On'.");
    configLogger.Info("Msg1: Console On");
    Console.WriteLine($"  IsSystemConsoleSinkEnabled: {LogManager.IsSystemConsoleSinkEnabled()} (Expected: True)");
    Console.WriteLine("  (Verify 'Msg1' in console. Press Enter...)");
    _ = Console.ReadLine();

    LogManager.EnableSystemConsoleSink(false);
    Console.WriteLine("  SystemConsoleSink disabled. Logging 'Msg2: Console Off'.");
    configLogger.Info("Msg2: Console Off (SHOULD NOT APPEAR)");
    Console.WriteLine($"  IsSystemConsoleSinkEnabled: {LogManager.IsSystemConsoleSinkEnabled()} (Expected: False)");
    Console.WriteLine("  (Verify 'Msg2' NOT in console. Press Enter...)");
    _ = Console.ReadLine();

    LogManager.EnableSystemConsoleSink(true);
    Console.WriteLine("  SystemConsoleSink re-enabled. Logging 'Msg3: Console On Again'.");
    configLogger.Info("Msg3: Console On Again");
    Console.WriteLine("  (Verify 'Msg3' in console. Press Enter...)");
    _ = Console.ReadLine();
    LogManager.EnableSystemConsoleSink(false); // Disable for next part

    Console.WriteLine("\n  Testing FileSink Configuration (Path, Level) & Disable:");
    string fileConfigA = "file_config_A.log";
    string fileConfigB = "file_config_B.log";
    if (File.Exists(fileConfigA))
    {
      File.Delete(fileConfigA);
    }

    if (File.Exists(fileConfigB))
    {
      File.Delete(fileConfigB);
    }

    LogManager.ConfigureFileSink(fileConfigA, LogLevel.Information);
    Console.WriteLine($"  Configured FileSink to '{fileConfigA}', MinLevel: Information.");
    configLogger.Debug("File A - Debug (should NOT appear)");
    configLogger.Info("File A - Info (should appear)");
    configLogger.Warn("File A - Warn (should appear)");
    Console.WriteLine($"  (Verify '{fileConfigA}' contains Info and Warn messages. Press Enter...)");
    _ = Console.ReadLine();

    LogManager.ConfigureFileSink(fileConfigB, LogLevel.Debug);
    Console.WriteLine($"  Configured FileSink to '{fileConfigB}', MinLevel: Debug. ('{fileConfigA}' should be closed).");
    configLogger.Debug("File B - Debug (should appear)");
    configLogger.Info("File B - Info (should appear)");
    Console.WriteLine($"  (Verify '{fileConfigB}' contains Debug and Info. '{fileConfigA}' not modified. Press Enter...)");
    _ = Console.ReadLine();

    LogManager.DisableFileSink();
    Console.WriteLine("  FileSink disabled.");
    configLogger.Error("File B - Error (should NOT appear in file after disable)");
    Console.WriteLine($"  (Verify '{fileConfigB}' does NOT contain the error message. Press Enter...)");
    _ = Console.ReadLine();
    if (File.Exists(fileConfigA))
    {
      File.Delete(fileConfigA);
    }

    if (File.Exists(fileConfigB))
    {
      File.Delete(fileConfigB);
    }

    Console.WriteLine("\n  Testing Global LogManager.MinLevel vs. FileSink Specific Level:");
    LogManager.EnableSystemConsoleSink(true); // For easy observation
    string fileLevelTest1 = "file_level_test1.log";
    string fileLevelTest2 = "file_level_test2.log";
    if (File.Exists(fileLevelTest1))
    {
      File.Delete(fileLevelTest1);
    }

    if (File.Exists(fileLevelTest2))
    {
      File.Delete(fileLevelTest2);
    }

    LogManager.MinLevel = LogLevel.Warning;
    LogManager.ConfigureFileSink(fileLevelTest1, LogLevel.Debug); // FileSink wants Debug and up
    Console.WriteLine($"  Global MinLevel: Warning. FileSink ('{fileLevelTest1}') MinLevel: Debug.");
    var levelLogger = LogManager.GetLogger("LevelInteractionTest");
    levelLogger.Debug("GL:Warn, FS:Debug - Debug Msg (Console: No, File: No)");
    levelLogger.Info("GL:Warn, FS:Debug - Info Msg (Console: No, File: No)");
    levelLogger.Warn("GL:Warn, FS:Debug - Warn Msg (Console: Yes, File: Yes)");
    levelLogger.Error("GL:Warn, FS:Debug - Error Msg (Console: Yes, File: Yes)");
    Console.WriteLine($"  (Verify console and '{fileLevelTest1}' show ONLY Warn & Error. Press Enter...)");
    _ = Console.ReadLine();

    LogManager.MinLevel = LogLevel.Trace;
    LogManager.ConfigureFileSink(fileLevelTest2, LogLevel.Information); // FileSink wants Info and up
    Console.WriteLine($"  Global MinLevel: Trace. FileSink ('{fileLevelTest2}') MinLevel: Information.");
    levelLogger.Debug("GL:Trace, FS:Info - Debug Msg (Console: Yes, File: No)");
    levelLogger.Info("GL:Trace, FS:Info - Info Msg (Console: Yes, File: Yes)");
    levelLogger.Warn("GL:Trace, FS:Info - Warn Msg (Console: Yes, File: Yes)");
    Console.WriteLine($"  (Verify console shows Debug,Info,Warn. '{fileLevelTest2}' shows Info,Warn. Press Enter...)");
    _ = Console.ReadLine();

    LogManager.DisableFileSink();
    LogManager.EnableSystemConsoleSink(false);
    if (File.Exists(fileLevelTest1))
    {
      File.Delete(fileLevelTest1);
    }

    if (File.Exists(fileLevelTest2))
    {
      File.Delete(fileLevelTest2);
    }

    LogManager.MinLevel = LogLevel.Information; // Reset global min level
    Console.WriteLine("  Cleaned up level interaction test resources.");

    Console.WriteLine("\n--- Logger Phase 4 Verification Complete ---");
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
}
