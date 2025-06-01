// <copyright file="DummyGroup.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;
using System.Collections.Generic;

using Night;

using NightTest.Core;

namespace NightTest.Modules
{
  /// <summary>
  /// A group that provides dummy test cases.
  /// </summary>
  public class DummyGroup : ITestGroup
  {
    /// <inheritdoc/>
    public IEnumerable<ITestCase> GetTestCases()
    {
      return new List<ITestCase>
            {
                new ConcreteDummyAutomatedTest(),
                new ConcreteDummyManualTest()
            };
    }
  }

  /// <summary>
  /// A concrete dummy test case that runs automatically.
  /// </summary>
  public class ConcreteDummyAutomatedTest : BaseTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Dummy.Automated.Example";
    /// <inheritdoc/>
    public override NightTest.Core.TestType Type => NightTest.Core.TestType.Automated;
    /// <inheritdoc/>
    public override string Description => "A dummy automated test case that runs for a short duration and then reports success.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      if (IsDone) return;

      if (TestStopwatch.ElapsedMilliseconds > 1000)
      {
        CurrentStatus = NightTest.Core.TestStatus.Passed;
        Details = "Automated dummy test case completed successfully after 1 second.";
        QuitSelf();
      }
    }
  }

  /// <summary>
  /// A concrete dummy test case that requires manual interaction.
  /// </summary>
  public class ConcreteDummyManualTest : NightTest.Core.BaseTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Dummy.Manual.Interaction";
    /// <inheritdoc/>
    public override NightTest.Core.TestType Type => NightTest.Core.TestType.Manual;
    /// <inheritdoc/>
    public override string Description => "A dummy manual test case that waits for user input (Space to Pass, Esc to Fail). Timeout after 30s.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      Details = "Manual test running. Press Space to Pass, Escape to Fail. Timeout in 30s.";
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      if (IsDone) return;

      if (TestStopwatch.ElapsedMilliseconds > 30000)
      {
        CurrentStatus = NightTest.Core.TestStatus.Failed;
        Details = "Manual test timed out after 30 seconds.";
        QuitSelf();
      }
    }

    /// <inheritdoc/>
    public override void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)
    {
      if (IsDone || isRepeat) return;
      Console.WriteLine($"[{Type}] {Name}: KeyPressed - {scancode}");

      if (scancode == KeyCode.Space)
      {
        CurrentStatus = NightTest.Core.TestStatus.Passed;
        Details = "User pressed Space to pass.";
        QuitSelf();
      }
      else if (scancode == KeyCode.Escape)
      {
        CurrentStatus = NightTest.Core.TestStatus.Failed;
        Details = "User pressed Escape to fail/quit.";
        QuitSelf();
      }
    }
  }
}
