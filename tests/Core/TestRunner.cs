// <copyright file="TestRunner.cs" company="Night Circle">
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

// Night import might not be needed here if ITestCase and TestType are in NightTest.Core
// using Night;

namespace NightTest.Core // Updated namespace
{

/// <summary>
/// Manages and reports the status of various tests within NightTest.
/// </summary>
public class TestRunner
{
  private class TestResultData
  {
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TestType Type { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TestStatus Status { get; set; }

    [JsonPropertyName("durationMs")]
    public long DurationMs { get; set; }

    [JsonPropertyName("details")]
    public string Details { get; set; } = string.Empty;
  }

  private readonly List<TestResultData> _recordedResults = new List<TestResultData>();
  private readonly List<(string Name, TestType Type)> _registeredCases = new List<(string Name, TestType Type)>();
  private readonly string _reportPath;
  private readonly bool _filterAutomatedOnly;
  private readonly string[] _commandLineArgs;

  /// <inheritdoc/>
  public TestRunner(string reportPath, bool filterAutomatedOnly, string[] commandLineArgs)
  {
    _reportPath = string.IsNullOrWhiteSpace(reportPath) ? "test_report.json" : reportPath;
    _filterAutomatedOnly = filterAutomatedOnly;
    _commandLineArgs = commandLineArgs ?? Array.Empty<string>();
  }

  /// <summary>
  /// Informs the TestRunner about a test case that is defined and could potentially run.
  /// </summary>
  /// <param name="testCase">The test case to register.</param>
  public void AddCaseToRegistry(ITestCase testCase)
  {
    if (testCase != null && !_registeredCases.Any(s => s.Name == testCase.Name))
    {
      _registeredCases.Add((testCase.Name, testCase.Type));
    }
  }

  /// <summary>
  /// Called by an ITestCase (which is also an IGame) when it completes its execution.
  /// </summary>
  public void RecordResult(string testName, TestType testType, TestStatus status, long durationMs, string details)
  {
    _recordedResults.Add(new TestResultData
    {
      Name = testName,
      Type = testType,
      Status = status,
      DurationMs = durationMs,
      Details = details
    });

    ConsoleColor color = status switch
    {
      TestStatus.Passed => ConsoleColor.Green,
      TestStatus.Failed => ConsoleColor.Red,
      TestStatus.Skipped => ConsoleColor.Yellow,
      TestStatus.NotRun => ConsoleColor.DarkGray,
      _ => ConsoleColor.Gray,
    };
    Console.ForegroundColor = color;
    Console.WriteLine($"  RESULT RECORDED: [{testType}] {testName}: {status} ({durationMs}ms) - {details}");
    Console.ResetColor();
  }

  /// <inheritdoc/>
  public void GenerateReport()
  {
    Console.WriteLine($"\nGenerating final report at: {_reportPath}");

    var allReportEntries = new List<TestResultData>();
    var executedTestNames = new HashSet<string>(_recordedResults.Select(r => r.Name));

    // Add all actually executed and recorded results
    allReportEntries.AddRange(_recordedResults);

    // Add entries for registered test cases that were not executed (skipped)
    foreach (var (name, type) in _registeredCases)
    {
      if (!executedTestNames.Contains(name))
      {
        allReportEntries.Add(new TestResultData
        {
          Name = name,
          Type = type,
          Status = TestStatus.Skipped,
          DurationMs = 0,
          Details = _filterAutomatedOnly && type == TestType.Manual ? "Skipped: --run-automated flag was used." : "Skipped: Test case was not selected or did not run."
        });
      }
    }

    // Ensure consistent order in the report
    allReportEntries = allReportEntries.OrderBy(r => r.Name).ToList();

    var report = new
    {
      reportTitle = "NightTest Engine Test Report",
      runTimestamp = DateTime.UtcNow.ToString("o"), // ISO 8601
      commandLineArgs = _commandLineArgs,
      tests = allReportEntries.Select(tr => new
      {
        name = tr.Name,
        type = tr.Type.ToString(), // Uses TestType from NightTest.Core
        status = tr.Status.ToString(), // Uses TestStatus from NightTest.Core
        durationMs = tr.DurationMs,
        details = tr.Details
      }).ToList(),
      summary = GenerateSummary(allReportEntries)
    };

    var options = new JsonSerializerOptions
    {
      WriteIndented = true,
      Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };
    string jsonReport = JsonSerializer.Serialize(report, options);
    File.WriteAllText(_reportPath, jsonReport);
    Console.WriteLine("JSON report generated successfully.");

    PrintConsoleSummary(report.tests, report.summary);
  }

  private object GenerateSummary(List<TestResultData> reportEntries)
  {
    return new
    {
      filterApplied = _filterAutomatedOnly ? "automated_only" : "all",
      totalRegistered = _registeredCases.Count,
      totalAttemptedToRun = reportEntries.Count(t => t.Status != TestStatus.Skipped), // Attempted means not skipped
      totalPassed = reportEntries.Count(t => t.Status == TestStatus.Passed),
      totalFailed = reportEntries.Count(t => t.Status == TestStatus.Failed),
      totalSkipped = reportEntries.Count(t => t.Status == TestStatus.Skipped),
      totalNotRun = reportEntries.Count(t => t.Status == TestStatus.NotRun), // Explicitly NotRun
      automated = new
      {
        registered = _registeredCases.Count(s => s.Type == TestType.Automated),
        attempted = reportEntries.Count(t => t.Type == TestType.Automated && t.Status != TestStatus.Skipped),
        passed = reportEntries.Count(t => t.Type == TestType.Automated && t.Status == TestStatus.Passed),
        failed = reportEntries.Count(t => t.Type == TestType.Automated && t.Status == TestStatus.Failed),
        skipped = reportEntries.Count(t => t.Type == TestType.Automated && t.Status == TestStatus.Skipped),
        notRun = reportEntries.Count(t => t.Type == TestType.Automated && t.Status == TestStatus.NotRun)
      },
      manual = new
      {
        registered = _registeredCases.Count(s => s.Type == TestType.Manual),
        attempted = reportEntries.Count(t => t.Type == TestType.Manual && t.Status != TestStatus.Skipped),
        passed = reportEntries.Count(t => t.Type == TestType.Manual && t.Status == TestStatus.Passed),
        failed = reportEntries.Count(t => t.Type == TestType.Manual && t.Status == TestStatus.Failed),
        skipped = reportEntries.Count(t => t.Type == TestType.Manual && t.Status == TestStatus.Skipped),
        notRun = reportEntries.Count(t => t.Type == TestType.Manual && t.Status == TestStatus.NotRun)
      }
    };
  }

  private void PrintConsoleSummary(IEnumerable<object> testDetails, dynamic summary) // Using dynamic for summary for simplicity
  {
    Console.WriteLine("\nNight Engine Test Report (Console Summary):");
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine($"Command: NightTest.exe {string.Join(" ", _commandLineArgs)}");

    // To access properties from the anonymous type list 'testDetails'
    foreach (dynamic testInfo in testDetails)
    {
      string typeStr = $"[{testInfo.type}]".PadRight(11);
      string statusStr = $"{testInfo.status}";
      string durationStr = (testInfo.status != TestStatus.Skipped.ToString() && testInfo.status != TestStatus.NotRun.ToString()) ? $"({testInfo.durationMs}ms)" : "";

      TestStatus statusEnum = Enum.Parse<TestStatus>(testInfo.status, true);

      ConsoleColor color = statusEnum switch
      {
        TestStatus.Passed => ConsoleColor.Green,
        TestStatus.Failed => ConsoleColor.Red,
        TestStatus.Skipped => ConsoleColor.Yellow,
        TestStatus.NotRun => ConsoleColor.DarkGray,
        _ => ConsoleColor.Gray,
      };
      Console.ForegroundColor = color;
      Console.WriteLine($"{typeStr} {testInfo.name}: {statusStr} {durationStr} - {testInfo.details}");
      Console.ResetColor();
    }
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine($"Filter Applied: {summary.filterApplied}");
    Console.WriteLine("Summary:");
    Console.WriteLine($"  Total Registered: {summary.totalRegistered} (Automated: {summary.automated.registered}, Manual: {summary.manual.registered})");
    Console.WriteLine($"  Total Attempted: {summary.totalAttemptedToRun}");
    Console.WriteLine($"  Passed: {summary.totalPassed} (Automated: {summary.automated.passed}, Manual: {summary.manual.passed})");
    Console.WriteLine($"  Failed: {summary.totalFailed} (Automated: {summary.automated.failed}, Manual: {summary.manual.failed})");
    Console.WriteLine($"  Skipped: {summary.totalSkipped} (Automated: {summary.automated.skipped}, Manual: {summary.manual.skipped})");
    Console.WriteLine($"  Not Run (Explicit): {summary.totalNotRun} (Automated: {summary.automated.notRun}, Manual: {summary.manual.notRun})");
  }
}
}
