// <copyright file="ITestModule.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System.Collections.Generic;

namespace NightTest
{
  /// <summary>
  /// Defines the contract for a test module, which can provide a collection of test scenarios.
  /// </summary>
  public interface ITestModule
  {
    /// <summary>
    /// Gets all test scenarios provided by this module.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="ITestScenario"/>.</returns>
    IEnumerable<ITestScenario> GetTestScenarios();
  }
}
