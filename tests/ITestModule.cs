// <copyright file="ITestModule.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System.Collections.Generic;

namespace NightTest
{
  /// <summary>
  /// Interface defining a contract for test modules.
  /// Each module is responsible for providing a list of <see cref="ITestCase"/>s.
  /// </summary>
  public interface ITestModule
  {
    /// <summary>
    /// Gets all test cases provided by this module.
    /// </summary>
    /// <returns>A collection of <see cref="ITestCase"/> objects.</returns>
    System.Collections.Generic.IEnumerable<ITestCase> GetTestCases();
  }
}
