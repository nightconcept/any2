// <copyright file="TimerGroup.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

using Night;

using NightTest.Core;

namespace NightTest.Groups
{
  /// <summary>
  /// Tests the Timer.GetTime() method.
  /// </summary>
  public class GetTimeTest : BaseTestCase
  {
    private double _startTime = 0;
    private double _endTime = 0;

    /// <inheritdoc/>
    public override string Name => "Timer.GetTime";
    /// <inheritdoc/>
    public override TestType Type => TestType.Automated;
    /// <inheritdoc/>
    public override string Description => "Tests the Night.Timer.GetTime() method by measuring time passage.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      _startTime = Night.Timer.GetTime();
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      if (IsDone) return;

      if (TestStopwatch.ElapsedMilliseconds > 500)
      {
        _endTime = Night.Timer.GetTime();
        double elapsed = _endTime - _startTime;
        CurrentStatus = TestStatus.Passed;
        Details = $"Timer.GetTime() test completed. Start: {_startTime:F6}s, End: {_endTime:F6}s. Elapsed: {elapsed:F6}s (Expected ~0.5s).";
        QuitSelf();
      }
    }
  }

  /// <summary>
  /// Tests the Timer.GetFPS() method.
  /// </summary>
  public class GetFPSTest : BaseTestCase
  {
    private int _frameCount = 0;

    /// <inheritdoc/>
    public override string Name => "Timer.GetFPS";
    /// <inheritdoc/>
    public override TestType Type => TestType.Automated;
    /// <inheritdoc/>
    public override string Description => "Tests the Night.Timer.GetFPS() method by observing its value over a short period.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      _frameCount = 0;
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      if (IsDone) return;

      _frameCount++;

      if (_frameCount > 10 && TestStopwatch.ElapsedMilliseconds > 200) // Run for a bit, after a few frames
      {
        int finalFps = Night.Timer.GetFPS(); // Get one last reading from the framework-updated value
        CurrentStatus = TestStatus.Passed;
        Details = $"Timer.GetFPS() test observed. Last reported FPS: {finalFps}. Test ran for >200ms and >10 frames.";
        QuitSelf();
      }
    }
  }

  /// <summary>
  /// Tests the Timer.GetDelta() method.
  /// </summary>
  public class GetDeltaTest : BaseTestCase
  {
    private List<float> _deltas = new List<float>();
    private int _frameCount = 0;

    /// <inheritdoc/>
    public override string Name => "Timer.GetDelta";
    /// <inheritdoc/>
    public override TestType Type => TestType.Automated;
    /// <inheritdoc/>
    public override string Description => "Tests the Night.Timer.GetDelta() method by collecting delta values.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      _deltas.Clear();
      _frameCount = 0;
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      if (IsDone) return;
      _frameCount++;
      float currentDelta = Night.Timer.GetDelta(); // Value is updated by framework's call to Timer.Step()
      _deltas.Add(currentDelta);

      if (_frameCount > 10 && TestStopwatch.ElapsedMilliseconds > 200)
      {
        float averageDelta = _deltas.Count > 0 ? _deltas.Average() : 0f;
        CurrentStatus = TestStatus.Passed;
        Details = $"Timer.GetDelta() test collected {_deltas.Count} values. Average delta from Timer.GetDelta(): {averageDelta:F6}. Test ran for >200ms and >10 frames.";
        QuitSelf();
      }
    }
  }

  /// <summary>
  /// Tests the Timer.GetAverageDelta() method.
  /// </summary>
  public class GetAverageDeltaTest : BaseTestCase
  {
    private int _frameCount = 0;

    /// <inheritdoc/>
    public override string Name => "Timer.GetAverageDelta";
    /// <inheritdoc/>
    public override TestType Type => TestType.Automated;
    /// <inheritdoc/>
    public override string Description => "Tests the Night.Timer.GetAverageDelta() method.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      _frameCount = 0;
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      if (IsDone) return;
      _frameCount++;
      // double avgDelta = Night.Timer.GetAverageDelta(); // Value observed by framework

      if (_frameCount > 10 && TestStopwatch.ElapsedMilliseconds > 200)
      {
        double finalAvgDelta = Night.Timer.GetAverageDelta(); // Get one last reading from framework-updated value
        CurrentStatus = TestStatus.Passed;
        Details = $"Timer.GetAverageDelta() observed. Last reported value: {finalAvgDelta:F6}. Test ran for >200ms and >10 frames.";
        QuitSelf();
      }
    }
  }

  /// <summary>
  /// Tests the Timer.Sleep() method.
  /// </summary>
  public class SleepTest : BaseTestCase
  {
    private const double SleepDurationSeconds = 0.25; // Sleep for 250ms
                                                      // Using TestStopwatch from base class for overall test duration
                                                      // Need a separate stopwatch for measuring sleep itself
    private System.Diagnostics.Stopwatch _internalStopwatch = new System.Diagnostics.Stopwatch();

    /// <inheritdoc/>
    public override string Name => "Timer.Sleep";
    /// <inheritdoc/>
    public override string Description => $"Tests the Night.Timer.Sleep() method by sleeping for {SleepDurationSeconds}s.";

    /// <inheritdoc/>
    public override TestType Type => TestType.Automated; // Explicitly Automated

    /// <inheritdoc/>
    public override void Load()
    {
      // We don't call base.Load() here because this test essentially finishes in Load
      // and needs to manage its own primary stopwatch for the result reporting.

      IsDone = false;
      CurrentStatus = TestStatus.NotRun;
      Details = "Test is running...";
      // TestStopwatch (from base) is used by QuitSelf for the RecordResult duration.
      // So, we start it here to measure the time taken for the Load phase itself.
      TestStopwatch.Reset();
      TestStopwatch.Start();

      _internalStopwatch.Reset();
      _internalStopwatch.Start();
      Night.Timer.Sleep(SleepDurationSeconds);
      _internalStopwatch.Stop();

      double elapsedMs = _internalStopwatch.ElapsedMilliseconds;

      if (elapsedMs >= SleepDurationSeconds * 1000 * 0.9 && elapsedMs <= SleepDurationSeconds * 1000 * 1.5)
      {
        CurrentStatus = TestStatus.Passed;
        Details = $"Timer.Sleep({SleepDurationSeconds}) executed. Measured duration: {elapsedMs}ms. Expected ~{SleepDurationSeconds * 1000}ms.";
      }
      else
      {
        CurrentStatus = TestStatus.Failed;
        Details = $"Timer.Sleep({SleepDurationSeconds}) executed. Measured duration: {elapsedMs}ms. Expected ~{SleepDurationSeconds * 1000}ms. Deviation too large.";
      }
      // Since this test completes in Load, call QuitSelf immediately.
      // The duration recorded will be the time spent in this Load method by TestStopwatch.
      QuitSelf();
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      // Test logic is in Load, Update is not used for this specific test.
      if (!IsDone) // Should be done from Load
      {
        Details = "Test did not complete in Load as expected.";
        CurrentStatus = TestStatus.Failed;
        QuitSelf();
      }
    }
  }

  /// <summary>
  /// Tests the Timer.Step() method.
  /// </summary>
  public class StepTest : BaseTestCase
  {
    private int _stepCount = 0;
    private List<double> _stepDeltas = new List<double>();

    /// <inheritdoc/>
    public override string Name => "Timer.Step";
    /// <inheritdoc/>
    public override TestType Type => TestType.Automated;
    /// <inheritdoc/>
    public override string Description => "Tests the Night.Timer.Step() method by calling it multiple times and observing delta values.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      _stepCount = 0;
      _stepDeltas.Clear();

      // Framework calls Timer.Initialize(), which sets LastStepTime.
      // Then Framework calls Timer.Step() before the first Update.
      // So, the first Night.Timer.Step() in Update here should give a small delta.
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime) // deltaTime here is from Framework's own step call
    {
      if (IsDone) return;

      // The framework has already called Timer.Step() and the result is in `deltaTime` / Timer.GetDelta().
      // To test Timer.Step() somewhat independently, we can call it again.
      // Note: The *first* call to Timer.Step() in an application's lifetime (or after a long pause)
      // might have a larger delta if LastStepTime was zero or very old.
      // Timer.Initialize() sets LastStepTime, and framework calls Step() before first Update.

      double directStepDelta = Night.Timer.Step(); // Call it directly to get its current calculation
      _stepDeltas.Add(directStepDelta);
      _stepCount++;

      if (_stepCount > 10 && TestStopwatch.ElapsedMilliseconds > 200)
      {
        double averageDirectStepDelta = _stepDeltas.Count > 0 ? _stepDeltas.Average() : 0.0;
        CurrentStatus = TestStatus.Passed;
        Details = $"Timer.Step() called {_stepCount} times directly. Average delta from these calls: {averageDirectStepDelta:F6}. Test ran for >200ms.";
        QuitSelf();
      }
    }
  }
}
