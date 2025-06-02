// <copyright file="TestGroup.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;
using System.Diagnostics;

using Night;

using Xunit;
using Xunit.Abstractions;

namespace NightTest.Core
{
  /// <summary>
  /// Base class for grouping xUnit tests for IGame test cases.
  /// </summary>
  public class TestGroup
  {
    private readonly ITestOutputHelper _outputHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestGroup"/> class.
    /// </summary>
    /// <param name="outputHelper">The xUnit test output helper for logging.</param>
    public TestGroup(ITestOutputHelper outputHelper)
    {
      this._outputHelper = outputHelper;
    }

    /// <summary>
    /// Run the IGame test case.
    /// </summary>
    public void Run_TestCase(BaseTestCase testCase)
    {
      Assert.NotNull(testCase); // Ensure testCase is not null at the start

      _outputHelper.WriteLine($"Starting IGame test: {testCase.Name}");
      _outputHelper.WriteLine($"  Description: {testCase.Description}");
      _outputHelper.WriteLine($"  Type: {testCase.Type}");

      try
      {
        // Run the IGame test case
        // This will block until the Night.Window is closed by the test itself (e.g., via QuitSelf)
        Night.Framework.Run(testCase);
      }
      catch (Exception ex)
      {
        // testCase is guaranteed to be non-null here due to the Assert.NotNull above.
        // If Night.Framework.Run throws before testCase.Load() or if testCase itself is problematic early,
        // the Assert.NotNull would have already caught a null testCase argument.
        // If the exception happens *during* testCase execution, testCase is still the same valid object.
        _outputHelper.WriteLine($"IGame test '{testCase.Name}' threw an unhandled exception: {ex.Message}\n{ex.StackTrace}");
        testCase.RecordFailure($"Unhandled exception: {ex.Message}", ex);
      }

      // testCase is guaranteed to be non-null here.
      _outputHelper.WriteLine($"IGame test '{testCase.Name}' completed.");
      _outputHelper.WriteLine($"  Status: {testCase.CurrentStatus}");
      _outputHelper.WriteLine($"  Details: {testCase.Details}");
      _outputHelper.WriteLine($"  Duration: {testCase.TestStopwatch.ElapsedMilliseconds}ms");

      Assert.Equal(TestStatus.Passed, testCase.CurrentStatus);
    }
  }
}
