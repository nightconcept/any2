// <copyright file="TimerTests.cs" company="Night Circle">
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
using System.Diagnostics;
using System.Threading;

using Night;

using SDL3;

using Xunit;

namespace Night.Tests.Timer
{
  /// <summary>
  /// Contains unit tests for the <see cref="Night.Timer"/> class.
  /// </summary>
  public class TimerTests : IDisposable
  {
    private const double AcceptableTimeEpsilon = 0.15; // 150ms epsilon for time comparisons (increased due to CI variability)
    private const float AcceptableDeltaEpsilon = 0.001f; // Epsilon for float delta comparisons
    private readonly ulong initialPerformanceFrequency;
    private readonly ulong initialLastStepTime;
    private readonly float initialCurrentDelta;
    private readonly double initialCurrentAverageDelta;
    private readonly int initialCurrentFPS;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimerTests"/> class.
    /// Sets up SDL timer subsystem for the tests.
    /// </summary>
    public TimerTests()
    {
      // Initialize SDL with no specific flags, as timer functions are generally available.
      if (!SDL3.SDL.Init(0))
      {
        throw new InvalidOperationException($"SDL_Init Error: {SDL3.SDL.GetError()}");
      }

      // Store initial Timer static values to restore them later if tests modify them directly.
      // This is important because Timer is a static class and state persists between tests.
      this.initialPerformanceFrequency = Night.Timer.PerformanceFrequency;
      this.initialLastStepTime = Night.Timer.LastStepTime;
      this.initialCurrentDelta = Night.Timer.CurrentDelta;
      this.initialCurrentAverageDelta = Night.Timer.CurrentAverageDelta;
      this.initialCurrentFPS = Night.Timer.CurrentFPS;

      // TimerStartTime is readonly.
      // For simplicity, we'll re-initialize Timer fully in Dispose or before specific tests
      // if a test heavily manipulates static state in a way that Initialize() can't fix.
      // For now, Initialize() should reset most things.
      Night.Timer.Initialize(); // Ensure a baseline initialization
    }

    /// <summary>
    /// Disposes of resources used by the test class.
    /// Quits the SDL timer subsystem.
    /// </summary>
    public void Dispose()
    {
      // Restore initial static values to ensure test isolation for subsequent test classes
      // or if a test runner reuses the AppDomain.
      Night.Timer.PerformanceFrequency = this.initialPerformanceFrequency;
      Night.Timer.LastStepTime = this.initialLastStepTime;
      Night.Timer.CurrentDelta = this.initialCurrentDelta;
      Night.Timer.CurrentAverageDelta = this.initialCurrentAverageDelta;
      Night.Timer.CurrentFPS = this.initialCurrentFPS;

      // TimerStartTime cannot be reset directly.
      // Re-calling Initialize might be needed if tests mess with it too much,
      // but GetTime() relies on the original TimerStartTime.
      // For most tests, a fresh Initialize() at the start is good.
      SDL3.SDL.QuitSubSystem(0); // Quit with no specific flags
      SDL3.SDL.Quit();
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.Initialize()"/> sets performance frequency and last step time.
    /// </summary>
    [Fact]
    public void Initialize_WhenSdlTimerSubsystemIsActive_SetsPerformanceFrequencyAndLastStepTime()
    {
      Night.Timer.Initialize();
      Assert.True(Night.Timer.PerformanceFrequency > 0, "PerformanceFrequency should be greater than 0.");
      Assert.True(Night.Timer.LastStepTime > 0, "LastStepTime should be greater than 0.");
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.GetTime()"/> returns a non-negative value after initialization.
    /// </summary>
    [Fact]
    public void GetTime_AfterInitialization_ReturnsNonNegativeValue()
    {
      Night.Timer.Initialize();
      double time = Night.Timer.GetTime();
      Assert.True(time >= 0.0, $"GetTime() returned {time}, expected non-negative.");
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.GetTime()"/> returns an increased value after waiting.
    /// </summary>
    [Fact]
    public void GetTime_AfterWaiting_ReturnsIncreasedValue()
    {
      Night.Timer.Initialize();
      double startTime = Night.Timer.GetTime();
      Thread.Sleep(10); // Wait for 10 milliseconds
      double endTime = Night.Timer.GetTime();
      Assert.True(endTime > startTime, $"endTime ({endTime}) was not greater than startTime ({startTime}).");
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.GetTime()"/> returns zero when performance frequency is zero.
    /// </summary>
    [Fact]
    public void GetTime_WhenPerformanceFrequencyIsZero_ReturnsZero()
    {
      Night.Timer.Initialize(); // Initialize to get a valid LastStepTime etc.
      ulong originalFrequency = Night.Timer.PerformanceFrequency; // Store to restore
      Night.Timer.PerformanceFrequency = 0;
      try
      {
        double time = Night.Timer.GetTime();
        Assert.Equal(0.0, time);
      }
      finally
      {
        Night.Timer.PerformanceFrequency = originalFrequency; // Restore
      }
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.GetFPS()"/> returns the correct internally set FPS.
    /// </summary>
    [Fact]
    public void GetFPS_WhenCurrentFPSSetInternally_ReturnsCorrectValue()
    {
      Night.Timer.Initialize();
      Night.Timer.CurrentFPS = 30;
      Assert.Equal(30, Night.Timer.GetFPS());
      Night.Timer.CurrentFPS = 60;
      Assert.Equal(60, Night.Timer.GetFPS());
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.GetDelta()"/> returns the correct internally set delta.
    /// </summary>
    [Fact]
    public void GetDelta_WhenCurrentDeltaSetInternally_ReturnsCorrectValue()
    {
      Night.Timer.Initialize();
      Night.Timer.CurrentDelta = 0.016f;
      Assert.Equal(0.016f, Night.Timer.GetDelta(), AcceptableDeltaEpsilon);
      Night.Timer.CurrentDelta = 0.032f;
      Assert.Equal(0.032f, Night.Timer.GetDelta(), AcceptableDeltaEpsilon);
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.GetAverageDelta()"/> returns the correct internally set average delta.
    /// </summary>
    [Fact]
    public void GetAverageDelta_WhenCurrentAverageDeltaSetInternally_ReturnsCorrectValue()
    {
      Night.Timer.Initialize();
      Night.Timer.CurrentAverageDelta = 0.033;
      Assert.Equal(0.033, Night.Timer.GetAverageDelta(), 5); // Default precision for double
      Night.Timer.CurrentAverageDelta = 0.017;
      Assert.Equal(0.017, Night.Timer.GetAverageDelta(), 5);
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.Sleep(double)"/> pauses execution for approximately the specified positive duration.
    /// </summary>
    [Fact]
    public void Sleep_WithPositiveDuration_PausesExecutionApproximately()
    {
      Night.Timer.Initialize();
      double sleepDurationSeconds = 0.02; // 20 ms
      Stopwatch sw = Stopwatch.StartNew();
      Night.Timer.Sleep(sleepDurationSeconds);
      sw.Stop();
      double elapsedSeconds = sw.Elapsed.TotalSeconds;
      Assert.True(elapsedSeconds >= sleepDurationSeconds - 0.005, $"Sleep was too short. Expected ~{sleepDurationSeconds}, got {elapsedSeconds}");

      // Allow for some overhead, so don't check upper bound too strictly
      Assert.True(elapsedSeconds < sleepDurationSeconds + AcceptableTimeEpsilon, $"Sleep was too long. Expected ~{sleepDurationSeconds}, got {elapsedSeconds}");
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.Sleep(double)"/> with zero duration returns immediately.
    /// </summary>
    [Fact]
    public void Sleep_WithZeroDuration_ReturnsImmediately()
    {
      Night.Timer.Initialize();
      Stopwatch sw = Stopwatch.StartNew();
      Night.Timer.Sleep(0);
      sw.Stop();
      Assert.True(sw.Elapsed.TotalSeconds < AcceptableTimeEpsilon, $"Sleep(0) took too long: {sw.Elapsed.TotalSeconds}s");
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.Sleep(double)"/> with negative duration returns immediately.
    /// </summary>
    [Fact]
    public void Sleep_WithNegativeDuration_ReturnsImmediately()
    {
      Night.Timer.Initialize();
      Stopwatch sw = Stopwatch.StartNew();
      Night.Timer.Sleep(-1.0);
      sw.Stop();
      Assert.True(sw.Elapsed.TotalSeconds < AcceptableTimeEpsilon, $"Sleep(-1.0) took too long: {sw.Elapsed.TotalSeconds}s");
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.Step()"/> after initialization returns a small positive delta and updates CurrentDelta.
    /// </summary>
    [Fact]
    public void Step_AfterInitialization_ReturnsSmallPositiveDeltaAndUpdateCurrentDelta()
    {
      Night.Timer.Initialize();

      // Allow a very small delay for the first step to be non-zero
      Thread.Sleep(1);
      double stepDelta = Night.Timer.Step();

      Assert.True(stepDelta > 0, $"stepDelta ({stepDelta}) should be positive.");

      // Max clamp is 0.0666
      Assert.True(stepDelta < 0.0667, $"stepDelta ({stepDelta}) should be less than max clamp value initially.");
      Assert.Equal((float)stepDelta, Night.Timer.GetDelta(), AcceptableDeltaEpsilon);
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.Step()"/> called sequentially updates delta and last step time.
    /// </summary>
    [Fact]
    public void Step_CalledSequentially_UpdatesDeltaAndLastStepTime()
    {
      Night.Timer.Initialize();

      // Ensure first step is non-zero
      Thread.Sleep(1);
      _ = Night.Timer.Step();
      ulong lastStepTime1 = Night.Timer.LastStepTime;

      // Wait a bit for time to pass
      Thread.Sleep(5);

      _ = Night.Timer.Step();
      ulong lastStepTime2 = Night.Timer.LastStepTime;
      float delta2 = Night.Timer.GetDelta();

      Assert.True(lastStepTime2 > lastStepTime1, "LastStepTime should increase after sequential Step calls.");
      Assert.True(delta2 > 0, "Second delta should be positive.");

      // Delta can be similar if calls are very fast, so primarily check LastStepTime update
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.Step()"/> clamps delta to max when calculated delta exceeds it.
    /// </summary>
    [Fact]
    public void Step_WhenCalculatedDeltaExceedsMax_ClampsDeltaToMax()
    {
      Night.Timer.Initialize();
      ulong perfFrequency = Night.Timer.PerformanceFrequency;

      // Guard against invalid frequency from SDL_Init failure or mock
      if (perfFrequency == 0 || perfFrequency == 1)
      {
        Night.Timer.PerformanceFrequency = 1000000000; // Mock a realistic frequency if needed
        perfFrequency = Night.Timer.PerformanceFrequency;
      }

      // Simulate 1 second having passed since last step
      Night.Timer.LastStepTime = SDL3.SDL.GetPerformanceCounter() - perfFrequency;

      double stepDelta = Night.Timer.Step();
      const double expectedClampedDelta = 0.0666;

      Assert.Equal(expectedClampedDelta, stepDelta, 4); // Compare with tolerance
      Assert.Equal((float)expectedClampedDelta, Night.Timer.GetDelta(), 4);
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.Step()"/> returns zero delta and sets LastStepTime when LastStepTime was zero.
    /// </summary>
    [Fact]
    public void Step_WhenLastStepTimeIsZero_ReturnsZeroDeltaAndSetsLastStepTime()
    {
      // Standard init
      Night.Timer.Initialize();

      // Force condition
      Night.Timer.LastStepTime = 0;

      double stepDelta = Night.Timer.Step();

      Assert.Equal(0.0, stepDelta);
      Assert.True(Night.Timer.LastStepTime > 0, "LastStepTime should be set after Step() if it was zero.");
      Assert.Equal(0.0f, Night.Timer.GetDelta());
    }

    /// <summary>
    /// Tests that <see cref="Night.Timer.Step()"/> returns zero delta when PerformanceFrequency is zero.
    /// </summary>
    [Fact]
    public void Step_WhenPerformanceFrequencyIsZero_ReturnsZeroDelta()
    {
      // Standard init
      Night.Timer.Initialize();
      ulong originalFrequency = Night.Timer.PerformanceFrequency;

      // Force condition
      Night.Timer.PerformanceFrequency = 0;
      Night.Timer.LastStepTime = SDL3.SDL.GetPerformanceCounter() - 1000; // Ensure LastStepTime is not zero

      try
      {
        double stepDelta = Night.Timer.Step();
        Assert.Equal(0.0, stepDelta);
        Assert.Equal(0.0f, Night.Timer.GetDelta());
      }
      finally
      {
        // Restore
        Night.Timer.PerformanceFrequency = originalFrequency;
      }
    }
  }
}
