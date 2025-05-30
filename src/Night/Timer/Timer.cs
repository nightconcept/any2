// <copyright file="Timer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading;

using SDL3;

namespace Night
{
  /// <summary>
  /// Provides high-resolution timing functionality.
  /// </summary>
  public static class Timer
  {
    // _timerStartTime is initialized when the Timer class is first loaded.
    private static readonly ulong TimerStartTime = SDL.GetPerformanceCounter();

    /// <summary>
    /// Gets or sets the current frames per second. Updated by the framework.
    /// </summary>
    internal static int CurrentFPS { get; set; } = 0;

    /// <summary>
    /// Gets or sets the delta time of the current frame. Updated by <see cref="Step()"/>.
    /// </summary>
    internal static float CurrentDelta { get; set; } = 0.0f;

    /// <summary>
    /// Gets or sets the average delta time over recent frames. Updated by the framework.
    /// </summary>
    internal static double CurrentAverageDelta { get; set; } = 0.0;

    /// <summary>
    /// Gets or sets the timestamp of the last call to <see cref="Step()"/>. Initialized by <see cref="Initialize()"/>.
    /// </summary>
    internal static ulong LastStepTime { get; set; } = 0;

    /// <summary>
    /// Gets the performance counter frequency. Initialized by <see cref="Initialize()"/>.
    /// </summary>
    internal static ulong PerformanceFrequency { get; private set; } = 1;

    /// <summary>
    /// Gets the time elapsed since the Timer module was loaded, in seconds.
    /// </summary>
    /// <returns>The time in seconds. Given as a decimal, accurate to the microsecond.</returns>
    public static double GetTime()
    {
      if (PerformanceFrequency == 0)
      {
        return 0.0;
      }

      ulong currentTimeCounter = SDL.GetPerformanceCounter();
      return (double)(currentTimeCounter - TimerStartTime) / PerformanceFrequency;
    }

    /// <summary>
    /// Gets the current frames per second (FPS).
    /// </summary>
    /// <returns>The current FPS.</returns>
    public static int GetFPS()
    {
      return CurrentFPS;
    }

    /// <summary>
    /// Gets the time elapsed since the last frame, in seconds.
    /// This is the same value passed to. <code>IGame.Update(float deltaTime)</code>.
    /// </summary>
    /// <returns>The delta time in seconds.</returns>
    public static float GetDelta()
    {
      return CurrentDelta;
    }

    /// <summary>
    /// Returns the average delta time (seconds per frame) over the last second.
    /// </summary>
    /// <returns>The average delta time over the last second.</returns>
    public static double GetAverageDelta()
    {
      return CurrentAverageDelta;
    }

    /// <summary>
    /// Pauses the current thread for the specified amount of time.
    /// This function causes the entire thread to pause. Graphics will not draw,
    /// input events will not trigger, code will not run, and the window will
    /// be unresponsive if you use this in the main game thread.
    /// </summary>
    /// <param name="seconds">Seconds to sleep for.</param>
    public static void Sleep(double seconds)
    {
      if (seconds < 0)
      {
        return;
      }

      Thread.Sleep(TimeSpan.FromSeconds(seconds));
    }

    /// <summary>
    /// Measures the time between the last call to this function and the current one.
    /// Calling this function updates the value returned by <see cref="GetDelta()"/>.
    /// This is typically called once per frame by the game loop to determine the delta time for that frame.
    /// </summary>
    /// <returns>The time passed (in seconds) since the last call to Step().</returns>
    public static double Step()
    {
      ulong now = SDL.GetPerformanceCounter();
      double deltaTimeSeconds = 0;

      // Ensure LastStepTime and PerformanceFrequency have been initialized
      if (LastStepTime > 0 && PerformanceFrequency > 0)
      {
        ulong elapsedTicks = now - LastStepTime;
        deltaTimeSeconds = (double)elapsedTicks / PerformanceFrequency;
      }

      // Clamp deltaTime to avoid large jumps
      // Approx 15 FPS, or 66.6ms
      if (deltaTimeSeconds > 0.0666)
      {
        deltaTimeSeconds = 0.0666;
      }

      LastStepTime = now;
      CurrentDelta = (float)deltaTimeSeconds;

      return deltaTimeSeconds;
    }

    /// <summary>
    /// Initializes essential timer values. Must be called once by the framework
    /// before the game loop begins and after SDL has been initialized.
    /// </summary>
    internal static void Initialize()
    {
      PerformanceFrequency = SDL.GetPerformanceFrequency();
      if (PerformanceFrequency == 0)
      {
        // Avoid division by zero, though SDL should provide valid freq.
        PerformanceFrequency = 1;
      }

      // Initialize for the first call to Step()
      LastStepTime = SDL.GetPerformanceCounter();

      // _timerStartTime is already initialized at class load (line 14) and should remain as such
      // to reflect "time since module loaded" for GetTime().
      // Do not re-assign _timerStartTime here.
    }
  }
}
