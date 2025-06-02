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
    /// <summary>
    /// Gets the stopwatch used to measure the duration of the test case.
    /// </summary>
    public Stopwatch TestStopwatch { get; } = new Stopwatch();

    /// <summary>
    /// Gets or sets a value indicating whether the test case has finished its execution.
    /// </summary>
    protected bool IsDone { get; set; } = false;

    /// <summary>
    /// Gets the current status of the test case.
    /// Its value can be asserted by xUnit test methods.
    /// </summary>
    public TestStatus CurrentStatus { get; protected set; } = TestStatus.NotRun; // Made public for xUnit assertions

    /// <summary>
    /// Gets details about the test execution, such as error messages or success information.
    /// Its value can be asserted by xUnit test methods.
    /// </summary>
    public string Details { get; protected set; } = "Test has not started."; // Made public for xUnit assertions


    /// <summary>
    /// Gets the current frame count since the test started. Incremented in Update.
    /// </summary>
    protected int currentFrameCount = 0;

    // These are now abstract and must be implemented by derived test cases.
    /// <inheritdoc/>
    public abstract string Name { get; }
    /// <inheritdoc/>
    public abstract TestType Type { get; }
    /// <inheritdoc/>
    public abstract string Description { get; }

    /// <summary>
    /// Called when the test case is loaded. Reset state here.
    /// Base implementation starts stopwatch and sets initial status.
    /// Override and call base.Load() if you need custom Load logic before stopwatch starts.
    /// </summary>
    public virtual void Load()
    {
      IsDone = false;
      CurrentStatus = TestStatus.NotRun;
      Details = "Test is running...";
      currentFrameCount = 0;
      TestStopwatch.Reset();
      TestStopwatch.Start();
    }

    /// <summary>
    /// Called every frame to update the test case's logic.
    /// This MUST be implemented by deriving test classes.
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last frame.</param>
    public virtual void Update(double deltaTime)
    {
      if (IsDone)
      {
        return;
      }

      currentFrameCount++;


      // Call specific update logic for automated tests
      if (this.Type == TestType.Automated)
      {
        OnUpdateAutomated(deltaTime);
      }
      // For manual tests, their Update override in a BaseManualTestCase-derived class
      // will call base.Update() which now calls this, and then BaseManualTestCase.Update()
      // will handle its specific logic.
    }

    /// <summary>
    /// Provides a dedicated update method for automated test logic.
    /// Called by the base Update method if the test Type is Automated and IsDone is false.
    /// Derived classes should override this method for their automated test update logic.
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last frame.</param>
    protected virtual void OnUpdateAutomated(double deltaTime)
    { }

    /// <summary>
    /// Called every frame to draw the test case.
    /// Base implementation handles drawing of manual confirmation UI if active.
    /// </summary>
    public virtual void Draw()
    { }

    /// <summary>
    /// Called when a key is pressed. Default is empty.
    /// </summary>
    public virtual void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)
    {
      if (IsDone || isRepeat)
      {
        return;
      }
    }

    /// <summary>
    /// Called when a key is released. Default is empty.
    /// </summary>
    public virtual void KeyReleased(KeySymbol key, KeyCode scancode) { }

    /// <summary>
    /// Called when a mouse button is pressed.
    /// Base implementation handles clicks for manual confirmation UI.
    /// </summary>
    public virtual void MousePressed(int x, int y, MouseButton button, bool istouch, int presses)
    { }

    /// <summary>
    /// Called when a mouse button is released. Default is empty.
    /// </summary>
    public virtual void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

    /// <summary>
    /// Public method to record a test failure, typically called by an xUnit wrapper when an exception occurs.
    /// </summary>
    /// <param name="failureDetails">Specific details about the failure.</param>
    /// <param name="ex">The exception that caused the failure, if any.</param>
    public void RecordFailure(string failureDetails, Exception? ex = null)
    {
      CurrentStatus = TestStatus.Failed;
      if (ex != null)
      {
        Details = $"{failureDetails} - Exception: {ex.GetType().Name}: {ex.Message}";
      }
      else
      {
        Details = failureDetails;
      }
    }

    /// <summary>
    /// Helper method to stop the stopwatch, record results, and close the window.
    /// Call this when your test logic determines completion (pass or fail).
    /// Ensure CurrentStatus and Details are set appropriately before calling.
    /// </summary>
    protected virtual void QuitSelf()
    {
      if (IsDone) return;

      TestStopwatch.Stop();

      if (Night.Window.IsOpen())
      {
        Night.Window.Close();
      }
      IsDone = true;
    }

    /// <summary>
    /// Checks if the test should complete based on a duration.
    /// Sets CurrentStatus, Details, and calls QuitSelf if completion occurs.
    /// </summary>
    /// <param name="milliseconds">The duration in milliseconds to wait.</param>
    /// <param name="successCondition">An optional function that must return true for the test to pass. If null, test passes on timeout.</param>
    /// <param name="passDetails">Details message if the test passes.</param>
    /// <param name="failDetailsTimeout">Details message if the test fails due to timeout (and no successCondition or it was false).</param>
    /// <param name="failDetailsCondition">Details message if the test fails because successCondition was false at timeout.</param>
    /// <returns>True if the test completed (passed or failed) by this call, false otherwise.</returns>
    protected bool CheckCompletionAfterDuration(
      double milliseconds,
      Func<bool>? successCondition = null,
      Func<string>? passDetails = null,
      Func<string>? failDetailsTimeout = null,
      Func<string>? failDetailsCondition = null)
    {
      if (IsDone) return true; // Already done, report as handled

      if (TestStopwatch.ElapsedMilliseconds >= milliseconds)
      {
        if (successCondition == null || successCondition())
        {
          CurrentStatus = TestStatus.Passed;
          Details = passDetails != null ? passDetails() : "Test passed: Met condition or reached duration.";
        }
        else
        {
          CurrentStatus = TestStatus.Failed;
          // If condition failed, use failDetailsCondition, otherwise (timeout without specific condition failure) use failDetailsTimeout
          Details = failDetailsCondition != null ? failDetailsCondition() : (failDetailsTimeout != null ? failDetailsTimeout() : "Test failed: Condition not met or timed out.");
        }
        QuitSelf();
        return true; // Test completed
      }
      return false; // Test not yet completed
    }

    /// <summary>
    /// Checks if the test should complete based on a number of frames.
    /// Sets CurrentStatus, Details, and calls QuitSelf if completion occurs.
    /// </summary>
    /// <param name="frameCount">The number of frames to wait.</param>
    /// <param name="successCondition">An optional function that must return true for the test to pass. If null, test passes after frameCount.</param>
    /// <param name="passDetails">Details message if the test passes.</param>
    /// <param name="failDetailsFrameLimit">Details message if the test fails due to exceeding frame limit (and no successCondition or it was false).</param>
    /// <param name="failDetailsCondition">Details message if the test fails because successCondition was false at frame limit.</param>
    /// <returns>True if the test completed (passed or failed) by this call, false otherwise.</returns>
    protected bool CheckCompletionAfterFrames(
      int frameCount,
      Func<bool>? successCondition = null,
      Func<string>? passDetails = null,
      Func<string>? failDetailsFrameLimit = null,
      Func<string>? failDetailsCondition = null)
    {
      if (IsDone) return true; // Already done, report as handled

      if (currentFrameCount >= frameCount)
      {
        if (successCondition == null || successCondition())
        {
          CurrentStatus = TestStatus.Passed;
          Details = passDetails != null ? passDetails() : "Test passed: Met condition or reached frame limit.";
        }
        else
        {
          CurrentStatus = TestStatus.Failed;
          // If condition failed, use failDetailsCondition, otherwise (timeout without specific condition failure) use failDetailsFrameLimit
          Details = failDetailsCondition != null ? failDetailsCondition() : (failDetailsFrameLimit != null ? failDetailsFrameLimit() : "Test failed: Condition not met or frame limit exceeded.");
        }
        QuitSelf();
        return true; // Test completed
      }
      return false; // Test not yet completed
    }
  }
}
