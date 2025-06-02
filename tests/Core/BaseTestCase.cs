// <copyright file="BaseTestCase.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;
using System.Diagnostics;

using Night;

namespace NightTest.Core
{
  /// <summary>
  /// Abstract base class for test cases to reduce boilerplate.
  /// Implements ITestCase and Night.IGame.
  /// </summary>
  public abstract class BaseTestCase : ITestCase, IGame
  {
    protected Stopwatch TestStopwatch { get; } = new Stopwatch();
    protected bool IsDone { get; set; } = false;
    public TestStatus CurrentStatus { get; protected set; } = TestStatus.NotRun; // Made public for xUnit assertions
    public string Details { get; protected set; } = "Test has not started."; // Made public for xUnit assertions

    // For Manual Test Interaction
    protected enum ManualInputUIMode { None, AwaitingConfirmation }
    protected ManualInputUIMode CurrentManualInputUIMode = ManualInputUIMode.None;
    protected string ManualConfirmationConsolePrompt { get; private set; } = string.Empty;

    // These are now abstract and must be implemented by derived test cases.
    /// <inheritdoc/>
    public abstract string Name { get; }
    /// <inheritdoc/>
    public abstract TestType Type { get; }
    /// <inheritdoc/>
    public abstract string Description { get; }

    // Removed: SetTestRunner method and Runner field, as TestRunner is no longer used.
    // public void SetTestRunner(TestRunner runner) { /* old implementation */ }
    // protected TestRunner Runner { get; private set; }


    private Night.Rectangle _passButtonRect;
    private Night.Rectangle _failButtonRect;
    private const int ButtonWidth = 120;
    private const int ButtonHeight = 50;
    private const int ButtonPadding = 20;
    private static readonly Night.Color PassButtonColor = new Night.Color(0, 180, 0); // Darker Green
    private static readonly Night.Color FailButtonColor = new Night.Color(200, 0, 0); // Darker Red
    private static readonly Night.Color ButtonBorderColor = Night.Color.White;

    /// <summary>
    /// Called when the test case is loaded. Reset state here.
    /// Base implementation starts stopwatch and sets initial status.
    /// Override and call base.Load() if you need custom Load logic before stopwatch starts.
    /// </summary>
    public virtual void Load()
    {
      Console.WriteLine($"[{Type}] {Name}: Load called.");
      IsDone = false;
      CurrentStatus = TestStatus.NotRun;
      Details = "Test is running...";
      CurrentManualInputUIMode = ManualInputUIMode.None; // Reset manual input state
      ManualConfirmationConsolePrompt = string.Empty;
      TestStopwatch.Reset();
      TestStopwatch.Start();
    }

    /// <summary>
    /// Called every frame to update the test case's logic.
    /// This MUST be implemented by deriving test classes.
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last frame.</param>
    public abstract void Update(double deltaTime);

    /// <summary>
    /// Called every frame to draw the test case.
    /// Base implementation handles drawing of manual confirmation UI if active.
    /// </summary>
    public virtual void Draw()
    {
      if (CurrentManualInputUIMode == ManualInputUIMode.AwaitingConfirmation)
      {
        // Ensure button rects are calculated if window is valid
        if (_passButtonRect.Width == 0 && Night.Window.IsOpen())
        {
          var windowMode = Night.Window.GetMode();
          if (windowMode.Width > 0 && windowMode.Height > 0)
          {
            CalculateButtonPositions(windowMode.Width, windowMode.Height);
          }
        }

        if (_passButtonRect.Width > 0) // Only draw if rects are valid
        {
          // Draw Pass Button (Green)
          Night.Graphics.SetColor(PassButtonColor);
          Night.Graphics.Rectangle(Night.DrawMode.Fill, _passButtonRect.X, _passButtonRect.Y, _passButtonRect.Width, _passButtonRect.Height);
          Night.Graphics.SetColor(ButtonBorderColor); // Border
          Night.Graphics.Rectangle(Night.DrawMode.Line, _passButtonRect.X, _passButtonRect.Y, _passButtonRect.Width, _passButtonRect.Height);

          // Draw Fail Button (Red)
          Night.Graphics.SetColor(FailButtonColor);
          Night.Graphics.Rectangle(Night.DrawMode.Fill, _failButtonRect.X, _failButtonRect.Y, _failButtonRect.Width, _failButtonRect.Height);
          Night.Graphics.SetColor(ButtonBorderColor); // Border
          Night.Graphics.Rectangle(Night.DrawMode.Line, _failButtonRect.X, _failButtonRect.Y, _failButtonRect.Width, _failButtonRect.Height);
        }
      }
    }

    /// <summary>
    /// Called when a key is pressed. Default is empty.
    /// </summary>
    public virtual void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat) { }

    /// <summary>
    /// Called when a key is released. Default is empty.
    /// </summary>
    public virtual void KeyReleased(KeySymbol key, KeyCode scancode) { }

    /// <summary>
    /// Called when a mouse button is pressed.
    /// Base implementation handles clicks for manual confirmation UI.
    /// </summary>
    public virtual void MousePressed(int x, int y, MouseButton button, bool istouch, int presses)
    {
      if (CurrentManualInputUIMode == ManualInputUIMode.AwaitingConfirmation && button == MouseButton.Left && !istouch)
      {
        if (_passButtonRect.Width > 0 && // Ensure buttons are initialized
            x >= _passButtonRect.X && x <= _passButtonRect.X + _passButtonRect.Width &&
            y >= _passButtonRect.Y && y <= _passButtonRect.Y + _passButtonRect.Height)
        {
          Console.WriteLine($"MANUAL TEST '{Name}': PASSED by user click.");
          CurrentStatus = TestStatus.Passed;
          Details = ManualConfirmationConsolePrompt + " - User confirmed: PASSED.";
          CurrentManualInputUIMode = ManualInputUIMode.None;
          QuitSelf();
        }
        else if (_failButtonRect.Width > 0 && // Ensure buttons are initialized
                 x >= _failButtonRect.X && x <= _failButtonRect.X + _failButtonRect.Width &&
                 y >= _failButtonRect.Y && y <= _failButtonRect.Y + _failButtonRect.Height)
        {
          Console.WriteLine($"MANUAL TEST '{Name}': FAILED by user click.");
          CurrentStatus = TestStatus.Failed;
          Details = ManualConfirmationConsolePrompt + " - User confirmed: FAILED.";
          CurrentManualInputUIMode = ManualInputUIMode.None;
          QuitSelf();
        }
      }
    }

    /// <summary>
    /// Called when a mouse button is released. Default is empty.
    /// </summary>
    public virtual void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

    /// <summary>
    /// Helper method to stop the stopwatch, record results, and close the window.
    /// Call this when your test logic determines completion (pass or fail).
    /// Ensure CurrentStatus and Details are set appropriately before calling.
    /// </summary>
    protected virtual void QuitSelf()
    {
      if (IsDone) return;

      // If a manual test is quit externally (e.g. ESC key in test case) before confirmation,
      // and status hasn't been set by button click, mark as failed.
      if (this.Type == TestType.Manual && CurrentManualInputUIMode == ManualInputUIMode.AwaitingConfirmation && CurrentStatus == TestStatus.NotRun)
      {
        CurrentStatus = TestStatus.Failed;
        Details = ManualConfirmationConsolePrompt + " - Test quit prematurely by user before confirmation.";
        Console.WriteLine($"MANUAL TEST '{Name}': Test quit prematurely. Marked as FAILED.");
      }
      CurrentManualInputUIMode = ManualInputUIMode.None; // Reset state

      TestStopwatch.Stop();
      // Runner.RecordResult is removed.
      // The CurrentStatus and Details are already set.
      // The xUnit test method will assert these values after Night.Framework.Run() completes.
      Console.WriteLine($"Test '{Name}' completed with status: {CurrentStatus}, Details: {Details}, Duration: {TestStopwatch.ElapsedMilliseconds}ms");

      if (Night.Window.IsOpen())
      {
        Night.Window.Close();
      }
      IsDone = true;
    }

    /// <summary>
    /// Initiates a manual confirmation step for the test.
    /// Displays a prompt in the console and renders Pass/Fail buttons in the game window.
    /// </summary>
    /// <param name="consolePrompt">The question or instruction to display in the console for the user.</param>
    protected void RequestManualConfirmation(string consolePrompt)
    {
      if (this.Type != TestType.Manual)
      {
        Console.WriteLine($"Warning: RequestManualConfirmation called for a non-manual test: {Name}. Ignoring.");
        return;
      }

      this.ManualConfirmationConsolePrompt = consolePrompt;
      this.CurrentManualInputUIMode = ManualInputUIMode.AwaitingConfirmation;
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine($"\n--- MANUAL CONFIRMATION REQUIRED for test: '{Name}' ---");
      Console.ResetColor();
      Console.WriteLine(this.ManualConfirmationConsolePrompt);
      Console.WriteLine("Please observe the game window. Click the GREEN box to PASS, or the RED box to FAIL.");
      Console.WriteLine("(Alternatively, press ESCAPE to fail and quit this specific test.)");

      // Attempt to calculate button positions immediately if window is available
      if (Night.Window.IsOpen())
      {
        var windowMode = Night.Window.GetMode();
        if (windowMode.Width > 0 && windowMode.Height > 0)
        {
          CalculateButtonPositions(windowMode.Width, windowMode.Height);
        }
      }
    }

    private void CalculateButtonPositions(int windowWidth, int windowHeight)
    {
      int totalButtonsWidth = (ButtonWidth * 2) + ButtonPadding;
      int startX = (windowWidth - totalButtonsWidth) / 2;
      if (startX < ButtonPadding) startX = ButtonPadding; // Ensure buttons are not off-screen left

      int buttonY = windowHeight - ButtonHeight - ButtonPadding;
      if (buttonY < ButtonPadding) buttonY = ButtonPadding; // Ensure buttons are not off-screen bottom

      _passButtonRect = new Night.Rectangle(startX, buttonY, ButtonWidth, ButtonHeight);
      _failButtonRect = new Night.Rectangle(startX + ButtonWidth + ButtonPadding, buttonY, ButtonWidth, ButtonHeight);
    }
  }
}
