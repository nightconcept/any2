// <copyright file="GraphicsClearColorTest.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;

using Night;

using NightTest.Core;

namespace NightTest.Groups.Graphics
{
  /// <summary>
  /// Tests the Graphics.Clear() method with a specific color.
  /// Requires manual confirmation that the color is correct.
  /// </summary>
  public class GraphicsClearColorTest : BaseTestCase
  {
    private readonly Night.Color _skyBlue = new Night.Color(135, 206, 235);
    private bool _promptRequested = false;

    /// <inheritdoc/>
    public override string Name => "Graphics.Clear.SkyBlueColor";

    /// <inheritdoc/>
    public override TestType Type => TestType.Manual;

    /// <inheritdoc/>
    public override string Description => "Tests clearing the screen to sky blue (135, 206, 235). User must confirm color.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      // Initial details, will be updated by manual confirmation result
      Details = "Test running, displaying sky blue color.";
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      if (IsDone)
      {
        return;
      }

      // Request manual confirmation once after a short delay to ensure window is visible
      if (!_promptRequested && TestStopwatch.ElapsedMilliseconds > 200)
      {
        RequestManualConfirmation("Is the screen cleared to a SKY BLUE color (like a clear daytime sky)?");
        _promptRequested = true;
      }

      // Manual tests rely on user input (handled by BaseTestCase) or timeout.
      // Add a timeout for manual tests to prevent them from running indefinitely.
      if (TestStopwatch.ElapsedMilliseconds > 30000 && CurrentManualInputUIMode == ManualInputUIMode.AwaitingConfirmation) // 30 second timeout
      {
        Console.WriteLine($"MANUAL TEST '{Name}': Timed out after 30 seconds.");
        CurrentStatus = TestStatus.Failed;
        Details = ManualConfirmationConsolePrompt + " - Test timed out.";
        QuitSelf();
      }
    }

    /// <inheritdoc/>
    public override void Draw()
    {
      Night.Graphics.Clear(_skyBlue); // Clear to sky blue

      // Base.Draw() will handle drawing Pass/Fail buttons if manual confirmation is active
      base.Draw();

      Night.Graphics.Present();
    }

    /// <inheritdoc/>
    public override void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)
    {
      if (IsDone || isRepeat) return;

      // Allow ESC to fail the test if in confirmation mode
      if (CurrentManualInputUIMode == ManualInputUIMode.AwaitingConfirmation && scancode == KeyCode.Escape)
      {
        Console.WriteLine($"MANUAL TEST '{Name}': FAILED by user pressing ESCAPE.");
        CurrentStatus = TestStatus.Failed;
        Details = ManualConfirmationConsolePrompt + " - User pressed ESCAPE to fail.";
        QuitSelf();
      }
    }
  }
}
