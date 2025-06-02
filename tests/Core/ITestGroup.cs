// <copyright file="ITestGroup.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System.Collections.Generic;

namespace NightTest.Core
{
  /// <summary>
  /// Interface defining a contract for test groups.
  /// Each group is responsible for providing a list of <see cref="ITestCase"/>s.
  /// </summary>
  public interface ITestGroup
  {
    /// <summary>
    /// Gets all test cases provided by this group.
    /// </summary>
    /// <returns>A collection of <see cref="ITestCase"/> objects.</returns>
    IEnumerable<ITestCase> GetTestCases();
  }
}
