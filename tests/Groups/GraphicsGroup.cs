// <copyright file="GraphicsGroup.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System.Collections.Generic;

using NightTest.Core;
using NightTest.Groups.Graphics; // For GraphicsClearColorTest

namespace NightTest.Groups
{
  /// <summary>
  /// Provides test cases for the Night.Graphics module.
  /// </summary>
  public class GraphicsGroup : ITestGroup
  {
    /// <inheritdoc/>
    public IEnumerable<ITestCase> GetTestCases()
    {
      return new List<ITestCase>
            {
                new GraphicsClearColorTest(),
            };
    }
  }
}
