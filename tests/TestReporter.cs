// <copyright file="TestReporter.cs" company="Night Circle">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame;

/// <summary>
/// Represents the status of a test.
/// </summary>
public enum TestStatus
{
  /// <summary>
  /// Test has not been run.
  /// </summary>
  NotRun,

  /// <summary>
  /// Test passed successfully.
  /// </summary>
  Passed,

  /// <summary>
  /// Test failed.
  /// </summary>
  Failed,
}

/// <summary>
/// Manages and reports the status of various tests within Night.TestGame.
/// </summary>
public class TestReporter
{
  private readonly Dictionary<string, TestStatus> testStatuses = new Dictionary<string, TestStatus>();
  private readonly List<string> testOrder = new List<string>();

  /// <summary>
  /// Registers a test. If the test is already registered, its status is reset to NotRun.
  /// </summary>
  /// <param name="testName">The name of the test to register.</param>
  public void RegisterTest(string testName)
  {
    if (!this.testStatuses.ContainsKey(testName))
    {
      this.testOrder.Add(testName);
    }

    this.testStatuses[testName] = TestStatus.NotRun;
  }

  /// <summary>
  /// Marks a registered test as Passed.
  /// If the test is not registered, it will be registered and then marked as Passed.
  /// </summary>
  /// <param name="testName">The name of the test.</param>
  public void Pass(string testName)
  {
    if (!this.testStatuses.ContainsKey(testName))
    {
      this.RegisterTest(testName);
    }

    this.testStatuses[testName] = TestStatus.Passed;
    Console.WriteLine($"[PASS] {testName}");
  }

  /// <summary>
  /// Marks a registered test as Failed.
  /// If the test is not registered, it will be registered and then marked as Failed.
  /// </summary>
  /// <param name="testName">The name of the test.</param>
  /// <param name="reason">An optional reason for the failure.</param>
  public void Fail(string testName, string? reason = null)
  {
    if (!this.testStatuses.ContainsKey(testName))
    {
      this.RegisterTest(testName);
    }

    this.testStatuses[testName] = TestStatus.Failed;
    Console.WriteLine($"[FAIL] {testName}{(string.IsNullOrEmpty(reason) ? string.Empty : $": {reason}")}");
  }

  /// <summary>
  /// Generates a string report of all registered test statuses.
  /// </summary>
  /// <returns>A string containing the test report.</returns>
  public string GenerateReport()
  {
    StringBuilder report = new StringBuilder();
    _ = report.AppendLine("\nNight Engine Test Report:");
    _ = report.AppendLine("-------------------------");

    if (!this.testOrder.Any())
    {
      _ = report.AppendLine("No tests registered.");
    }
    else
    {
      foreach (var testName in this.testOrder)
      {
        if (this.testStatuses.TryGetValue(testName, out TestStatus status))
        {
          _ = report.AppendLine($"{testName}: {status}");
        }
      }
    }

    _ = report.AppendLine("-------------------------");
    int passedCount = this.testStatuses.Count(kv => kv.Value == TestStatus.Passed);
    int failedCount = this.testStatuses.Count(kv => kv.Value == TestStatus.Failed);
    int notRunCount = this.testStatuses.Count(kv => kv.Value == TestStatus.NotRun);
    _ = report.AppendLine($"Summary: {passedCount} Passed, {failedCount} Failed, {notRunCount} Not Run");
    _ = report.AppendLine("-------------------------");

    return report.ToString();
  }

  /// <summary>
  /// Prints the generated report to the console.
  /// </summary>
  public void PrintReportToConsole()
  {
    Console.WriteLine(this.GenerateReport());
  }
}
