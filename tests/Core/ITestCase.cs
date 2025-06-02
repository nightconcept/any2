// <copyright file="ITestCase.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

namespace NightTest.Core
{
  /// <summary>
  /// Defines the contract for a runnable test case.
  /// Test cases must also implement Night.IGame to be executed by the framework.
  /// </summary>
  public interface ITestCase // Renamed from ITestScenario
  {
    /// <summary>
    /// Gets the unique name of the test case.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the type of the test case (e.g., Automated, Manual).
    /// </summary>
    TestType Type { get; }

    /// <summary>
    /// Gets a brief description of what the test case does.
    /// </summary>
    string Description { get; }
  }
}
