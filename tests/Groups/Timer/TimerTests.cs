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

namespace NightTest.Groups.Timer
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
    public override string Description => "Tests the Night.Timer.GetTime() method by measuring time passage.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      _startTime = Night.Timer.GetTime();
    }

    /// <inheritdoc/>
    protected override void UpdateAutomated(double deltaTime)
    {
      // The IsDone check is handled by BaseTestCase.Update before calling this.
      _ = CheckCompletionAfterDuration(500,
        successCondition: () =>
        {
          _endTime = Night.Timer.GetTime();
          return true; // Condition for passing is simply reaching the duration
        },
        passDetails: () => // Use a lambda to construct details with captured values
        {
          double elapsed = _endTime - _startTime;
          return $"Timer.GetTime() test completed. Start: {_startTime:F6}s, End: {_endTime:F6}s. Elapsed: {elapsed:F6}s (Expected ~0.5s).";
        });
    }
  }

  /// <summary>
  /// Tests the Timer.GetFPS() method.
  /// </summary>
  public class GetFPSTest : BaseTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Timer.GetFPS";
    /// <inheritdoc/>
    public override string Description => "Tests the Night.Timer.GetFPS() method by observing its value over a short period.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
    }

    /// <inheritdoc/>
    protected override void UpdateAutomated(double deltaTime)
    {
      int finalFps = 0; // To capture FPS in the success condition

      _ = CheckCompletionAfterDuration(201, // > 200ms
        successCondition: () =>
        {
          if (currentFrameCount > 10)
          {
            finalFps = Night.Timer.GetFPS(); // Get FPS when conditions are met
            return true;
          }
          return false;
        },
        passDetails: () => $"Timer.GetFPS() test observed. Last reported FPS: {finalFps}. Test ran for >200ms and >10 frames.",
        failDetailsTimeout: null,
        failDetailsCondition: () => "Timer.GetFPS() test failed: Did not exceed 10 frames within 200ms."
      );
    }
  }

  /// <summary>
  /// Tests the Timer.GetDelta() method.
  /// </summary>
  public class GetDeltaTest : BaseTestCase
  {
    private List<float> _deltas = new List<float>();

    /// <inheritdoc/>
    public override string Name => "Timer.GetDelta";
    /// <inheritdoc/>
    public override string Description => "Tests the Night.Timer.GetDelta() method by collecting delta values.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      _deltas.Clear();
    }

    /// <inheritdoc/>
    protected override void UpdateAutomated(double deltaTime)
    {
      _deltas.Add(Night.Timer.GetDelta()); // Collect delta each frame this update is called

      _ = CheckCompletionAfterDuration(201, // > 200ms
        successCondition: () => currentFrameCount > 10,
        passDetails: () =>
        {
          float averageDelta = _deltas.Count > 0 ? _deltas.Average() : 0f;
          return $"Timer.GetDelta() test collected {_deltas.Count} values. Average delta from Timer.GetDelta(): {averageDelta:F6}. Test ran for >200ms and >10 frames.";
        },
        failDetailsTimeout: null,
        failDetailsCondition: () => "Timer.GetDelta() test failed: Did not exceed 10 frames collecting deltas within 200ms."
      );
    }
  }

  /// <summary>
  /// Tests the Timer.GetAverageDelta() method.
  /// </summary>
  public class GetAverageDeltaTest : BaseTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Timer.GetAverageDelta";
    /// <inheritdoc/>
    public override string Description => "Tests the Night.Timer.GetAverageDelta() method.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
    }

    /// <inheritdoc/>
    protected override void UpdateAutomated(double deltaTime)
    {
      double finalAvgDelta = 0;

      _ = CheckCompletionAfterDuration(201, // > 200ms
        successCondition: () =>
        {
          if (currentFrameCount > 10)
          {
            finalAvgDelta = Night.Timer.GetAverageDelta();
            return true;
          }
          return false;
        },
        passDetails: () => $"Timer.GetAverageDelta() observed. Last reported value: {finalAvgDelta:F6}. Test ran for >200ms and >10 frames.",
        failDetailsTimeout: null,
        failDetailsCondition: () => "Timer.GetAverageDelta() test failed: Did not exceed 10 frames within 200ms."
      );
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
    public override void Load()
    {
      // We don't call base.Load() here because this test essentially finishes in Load
      // and needs to manage its own primary stopwatch for the result reporting.

      IsDone = false;
      CurrentStatus = TestStatus.NotRun;
      Details = "Test is running...";
      // TestStopwatch (from base) is used by EndTest for the RecordResult duration.
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
      // Since this test completes in Load, call EndTest immediately.
      // The duration recorded will be the time spent in this Load method by TestStopwatch.
      EndTest();
    }

    /// <inheritdoc/>
    protected override void UpdateAutomated(double deltaTime)
    {
      if (!IsDone)
      {
        Details = "Test did not complete in Load as expected.";
        CurrentStatus = TestStatus.Failed;
        EndTest(); // Ensure it quits if it somehow reaches here and isn't done.
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
    protected override void UpdateAutomated(double deltaTime)
    {
      // The framework has already called Timer.Step() and the result is in `deltaTime` / Timer.GetDelta().
      // To test Timer.Step() somewhat independently, we can call it again.
      // Note: The *first* call to Timer.Step() in an application's lifetime (or after a long pause)
      // might have a larger delta if LastStepTime was zero or very old.
      // Timer.Initialize() sets LastStepTime, and framework calls Step() before first Update.

      double directStepDelta = Night.Timer.Step(); // Call it directly to get its current calculation
      _stepDeltas.Add(directStepDelta);
      _stepCount++; // Still need _stepCount for the number of direct calls.

      _ = CheckCompletionAfterDuration(201,
        successCondition: () => _stepCount > 10,
        passDetails: () =>
        {
          double averageDirectStepDelta = _stepDeltas.Count > 0 ? _stepDeltas.Average() : 0.0;
          return $"Timer.Step() called {_stepCount} times directly. Average delta from these calls: {averageDirectStepDelta:F6}. Test ran for >200ms.";
        },
        failDetailsTimeout: null,
        failDetailsCondition: () => "Timer.Step() test failed: Did not make >10 direct calls within 200ms."
      );
    }
  }
}
