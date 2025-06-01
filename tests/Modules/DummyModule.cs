// <copyright file="DummyModule.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Night;

namespace NightTest.Modules
{
  /// <summary>
  /// A module that provides dummy test scenarios.
  /// </summary>
  public class DummyModule : ITestModule
  {
    /// <inheritdoc/>
    public IEnumerable<ITestScenario> GetTestScenarios()
    {
      return new List<ITestScenario>
            {
                new ConcreteDummyAutomatedTest(),
                new ConcreteDummyManualTest() // Example of a second test in the module
            };
    }
  }

  /// <summary>
  /// A concrete dummy test scenario that runs automatically.
  /// </summary>
  public class ConcreteDummyAutomatedTest : ITestScenario, Night.IGame
  {
    private TestRunner? _runner;
    private Stopwatch _stopwatch = new Stopwatch();
    private bool _isDone = false;
    private TestStatus _currentStatus = TestStatus.NotRun;
    private string _details = "Test has not started.";

    /// <inheritdoc/>
    public string Name => "Dummy.Automated.Example";
    /// <inheritdoc/>
    public TestType Type => TestType.Automated;
    /// <inheritdoc/>
    public string Description => "A dummy automated scenario that runs for a short duration and then reports success.";

    /// <inheritdoc/>
    public void SetTestRunner(TestRunner runner)
    {
      _runner = runner;
    }

    /// <inheritdoc/>
    public void Load()
    {
      Console.WriteLine($"[{Type}] {Name}: Load called.");
      _isDone = false;
      _currentStatus = TestStatus.NotRun;
      _details = "Test is running...";
      _stopwatch.Reset();
      _stopwatch.Start();
    }

    /// <inheritdoc/>
    public void Update(double deltaTime)
    {
      if (_isDone) return;

      if (_stopwatch.ElapsedMilliseconds > 1000)
      {
        _currentStatus = TestStatus.Passed;
        _details = "Automated dummy scenario completed successfully after 1 second.";
        _isDone = true;
        QuitSelf();
      }
    }

    /// <inheritdoc/>
    public void Draw()
    {
      // Minimal drawing, or could be more elaborate if needed
    }

    /// <inheritdoc/>
    public void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)
    {
      if (_isDone || isRepeat) return;
      if (scancode == KeyCode.Escape)
      {
        _currentStatus = TestStatus.Failed;
        _details = "Test aborted by Escape key.";
        _isDone = true;
        QuitSelf();
      }
    }

    /// <inheritdoc/>
    public void KeyReleased(KeySymbol key, KeyCode scancode) { }
    /// <inheritdoc/>
    public void MousePressed(int x, int y, MouseButton button, bool istouch, int presses) { }
    /// <inheritdoc/>
    public void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

    private void QuitSelf()
    {
      _stopwatch.Stop();
      if (_runner == null)
      {
        Console.WriteLine($"ERROR in {Name}: TestRunner was not set. Cannot record result.");
        _currentStatus = TestStatus.Failed;
        _details += " | TestRunner not available.";
      }
      else
      {
        _runner.RecordResult(Name, Type, _currentStatus, _stopwatch.ElapsedMilliseconds, _details);
      }
      if (Night.Window.IsOpen())
      {
        Night.Window.Close();
      }
      _isDone = true;
    }
  }

  /// <summary>
  /// A concrete dummy test scenario that requires manual interaction.
  /// </summary>
  public class ConcreteDummyManualTest : ITestScenario, Night.IGame
  {
    private TestRunner? _runner;
    private Stopwatch _stopwatch = new Stopwatch();
    private bool _isDone = false;
    private TestStatus _currentStatus = TestStatus.NotRun;
    private string _details = "Test has not started. Press Space to Pass, Escape to Fail.";

    /// <inheritdoc/>
    public string Name => "Dummy.Manual.Interaction";
    /// <inheritdoc/>
    public TestType Type => TestType.Manual;
    /// <inheritdoc/>
    public string Description => "A dummy manual scenario that waits for user input (Space to Pass, Esc to Fail).";

    /// <inheritdoc/>
    public void SetTestRunner(TestRunner runner)
    {
      _runner = runner;
    }

    /// <inheritdoc/>
    public void Load()
    {
      Console.WriteLine($"[{Type}] {Name}: Load called.");
      _isDone = false;
      _currentStatus = TestStatus.NotRun;
      _details = "Manual test running. Press Space to Pass, Escape to Fail.";
      _stopwatch.Reset();
      _stopwatch.Start();
    }

    /// <inheritdoc/>
    public void Update(double deltaTime)
    {
      // Manual tests often don't do much in Update regarding completion,
      // they wait for input. A timeout could be added here.
      if (_isDone) return;

      // Optional: Timeout for manual tests
      if (_stopwatch.ElapsedMilliseconds > 30000) // 30-second timeout for manual action
      {
        _currentStatus = TestStatus.Failed; // Or Skipped/NotRun based on policy
        _details = "Manual test timed out after 30 seconds.";
        _isDone = true;
        QuitSelf();
      }
    }

    /// <inheritdoc/>
    public void Draw()
    {
      // Night.Graphics.Clear(Night.Color.DarkSlateGray);
      // Night.Graphics.Print($"Running Test: {Name}", 10, 10, Night.Color.White);
      // Night.Graphics.Print($"Elapsed: {_stopwatch.ElapsedMilliseconds / 1000.0:F2}s", 10, 30, Night.Color.White);
      // Night.Graphics.Print(_details, 10, 50, Night.Color.White);
      // if (!_isDone) Night.Graphics.Print("Press Space to Pass, Escape to Fail/Quit this test.", 10, 70, Night.Color.Yellow);
    }

    /// <inheritdoc/>
    public void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)
    {
      if (_isDone || isRepeat) return;
      Console.WriteLine($"[{Type}] {Name}: KeyPressed - {scancode}");

      if (scancode == KeyCode.Space)
      {
        _currentStatus = TestStatus.Passed;
        _details = "User pressed Space to pass.";
        _isDone = true;
        QuitSelf();
      }
      else if (scancode == KeyCode.Escape)
      {
        _currentStatus = TestStatus.Failed;
        _details = "User pressed Escape to fail/quit.";
        _isDone = true;
        QuitSelf();
      }
    }

    /// <inheritdoc/>
    public void KeyReleased(KeySymbol key, KeyCode scancode) { }
    /// <inheritdoc/>
    public void MousePressed(int x, int y, MouseButton button, bool istouch, int presses) { }
    /// <inheritdoc/>
    public void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

    private void QuitSelf()
    {
      _stopwatch.Stop();
      if (_runner == null)
      {
        Console.WriteLine($"ERROR in {Name}: TestRunner was not set. Cannot record result.");
        _currentStatus = TestStatus.Failed;
        _details += " | TestRunner not available.";
      }
      else
      {
        _runner.RecordResult(Name, Type, _currentStatus, _stopwatch.ElapsedMilliseconds, _details);
      }
      if (Night.Window.IsOpen())
      {
        Night.Window.Close();
      }
      _isDone = true;
    }
  }
}
