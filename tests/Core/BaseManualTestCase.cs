// <copyright file="BaseManualTestCase.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;

using Night; // Assuming Night.Color, Night.Rectangle, Night.Graphics, Night.Window, KeyCode, MouseButton are here

namespace NightTest.Core
{
  /// <summary>
  /// Abstract base class for manual test cases, providing common UI and interaction logic.
  /// Inherits from BaseTestCase.
  /// </summary>
  public abstract class BaseManualTestCase : BaseTestCase
  {
    /// <summary>
    /// Defines the UI mode for manual test input confirmation.
    /// </summary>
    protected enum ManualInputUIMode
    {
      /// <summary>
      /// No manual input UI is active.
      /// </summary>
      None,

      /// <summary>
      /// The test is awaiting user confirmation via the UI (Pass/Fail buttons).
      /// </summary>
      AwaitingConfirmation
    }

    /// <summary>
    /// Gets or sets the current UI mode for manual input.
    /// </summary>
    protected ManualInputUIMode CurrentManualInputUIMode = ManualInputUIMode.None;

    /// <summary>
    /// Gets the console prompt message displayed during manual confirmation.
    /// </summary>
    protected string ManualConfirmationConsolePrompt { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the timeout in milliseconds for manual tests awaiting confirmation. Defaults to 30 seconds.
    /// </summary>
    protected double ManualTestTimeoutMilliseconds { get; } = 30000;

    /// <summary>
    /// Gets the suggested delay in milliseconds before a manual test prompt is shown. Defaults to 200ms.
    /// Derived classes can use this value to time their call to RequestManualConfirmation.
    /// </summary>
    protected double ManualTestPromptDelayMilliseconds { get; } = 200;

    private Night.Rectangle _passButtonRect;
    private Night.Rectangle _failButtonRect;
    private const int ButtonWidth = 120;
    private const int ButtonHeight = 50;
    private const int ButtonPadding = 20;
    private static readonly Night.Color PassButtonColor = new Night.Color(0, 180, 0); // Darker Green
    private static readonly Night.Color FailButtonColor = new Night.Color(200, 0, 0); // Darker Red
    private static readonly Night.Color ButtonBorderColor = Night.Color.White;

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      CurrentManualInputUIMode = ManualInputUIMode.None;
      ManualConfirmationConsolePrompt = string.Empty;
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      base.Update(deltaTime);
      if (IsDone)
      {
        return;
      }

      // Handle timeout for manual tests
      if (this.Type == TestType.Manual && CurrentManualInputUIMode == ManualInputUIMode.AwaitingConfirmation)
      {
        if (TestStopwatch.ElapsedMilliseconds > ManualTestTimeoutMilliseconds)
        {
          Console.WriteLine($"MANUAL TEST '{Name}': Timed out after {ManualTestTimeoutMilliseconds / 1000} seconds.");
          CurrentStatus = TestStatus.Failed;
          Details = ManualConfirmationConsolePrompt + " - Test timed out.";
          QuitSelf();
          return; // Test is over
        }
      }
      // Note: OnUpdateAutomated is not called here as this class is for manual tests.
      // Manual tests will implement their specific update logic directly in their override if needed,
      // or rely on interaction for completion.
    }

    /// <inheritdoc/>
    public override void Draw()
    {
      base.Draw(); // Call base for any future generic drawing logic

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

    /// <inheritdoc/>
    public override void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)
    {
      base.KeyPressed(key, scancode, isRepeat); // Call base for IsDone check
      if (IsDone || isRepeat) // Check again as base might have set IsDone
      {
        return;
      }

      // Handling for ESC key to fail manual tests during confirmation
      if (this.Type == TestType.Manual && CurrentManualInputUIMode == ManualInputUIMode.AwaitingConfirmation && scancode == KeyCode.Escape)
      {
        Console.WriteLine($"MANUAL TEST '{Name}': FAILED by user pressing ESCAPE.");
        CurrentStatus = TestStatus.Failed;
        Details = ManualConfirmationConsolePrompt + " - User pressed ESCAPE to fail.";
        QuitSelf();
        return; // Test is over
      }
    }

    /// <inheritdoc/>
    public override void MousePressed(int x, int y, MouseButton button, bool istouch, int presses)
    {
      base.MousePressed(x, y, button, istouch, presses); // Call base for any future generic mouse logic
      if (IsDone)
      {
        return;
      }

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

    /// <inheritdoc/>
    protected override void QuitSelf()
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
      CurrentManualInputUIMode = ManualInputUIMode.None; // Ensure this is reset before base call

      base.QuitSelf(); // Call base to stop stopwatch, close window, set IsDone
    }
  }
}
