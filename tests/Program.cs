// <copyright file="Program.cs" company="Night Circle">
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

using Night;

using NightTest.Modules;
using NightTest.Core;

namespace NightTest
{
  /// <summary>
  /// Main program class for NightTest.
  /// Contains the entry point of the application.
  /// </summary>
  public static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// Orchestrates the test cases.
    /// </summary>
    public static void Main(string[] args)
    {
      Console.WriteLine("NightTest Orchestrator Starting...");

      bool runAutomatedOnly = args.Contains("--run-automated");
      string reportPath = "test_report.json"; // Default
      for (int i = 0; i < args.Length - 1; i++)
      {
        if (args[i] == "--report-path" && i + 1 < args.Length)
        {
          reportPath = args[i + 1];
          break;
        }
      }

      Console.WriteLine($"Command line args: {string.Join(" ", args)}");
      Console.WriteLine($"Run automated only: {runAutomatedOnly}");
      Console.WriteLine($"Report path: {reportPath}");

      var testRunner = new NightTest.Core.TestRunner(reportPath, runAutomatedOnly, args);

      // List of GROUPS to load tests from.
      var testGroups = new List<NightTest.Core.ITestGroup>
            {
                new DummyGroup(),
                new TimerGroup(),
                // new GraphicsTest(), // Example for a future test group
                // new InputTest(),    // Example for a future test group
            };

      var allTestCasesFromGroups = new List<NightTest.Core.ITestCase>();
      foreach (var group in testGroups)
      {
        try
        {
          allTestCasesFromGroups.AddRange(group.GetTestCases());
        }
        catch (Exception ex)
        {
          Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine($"!! CRITICAL ERROR loading test cases from group {group.GetType().Name}: {ex.Message}");
          Console.WriteLine(ex.StackTrace);
          Console.ResetColor();
          testRunner.RecordResult("GroupLoad", NightTest.Core.TestType.Automated, NightTest.Core.TestStatus.Failed, 0, $"Failed to load test cases: {ex.Message}");
        }
      }

      // Register all discovered test cases with the runner for comprehensive reporting
      foreach (var testCase in allTestCasesFromGroups)
      {
        testRunner.AddCaseToRegistry(testCase);
      }

      var testCasesToRun = new List<NightTest.Core.ITestCase>();
      if (runAutomatedOnly)
      {
        testCasesToRun.AddRange(allTestCasesFromGroups.Where(s => s.Type == NightTest.Core.TestType.Automated));
        Console.WriteLine($"\n--run-automated specified. Will run {testCasesToRun.Count} automated test case(s) out of {allTestCasesFromGroups.Count} discovered.");
      }
      else
      {
        testCasesToRun.AddRange(allTestCasesFromGroups);
        Console.WriteLine($"\nWill attempt to run all {testCasesToRun.Count} discovered test case(s).");
      }

      foreach (var testCase in testCasesToRun)
      {
        Console.WriteLine($"\n--- Starting Test Case: {testCase.Name} ([{testCase.Type}]) ---");
        testCase.SetTestRunner(testRunner); // Provide the runner to the test case

        if (testCase is Night.IGame gameTestCase)
        {
          try
          {
            // Each test case (which is an IGame) runs in its own Framework.Run call
            Night.Framework.Run(gameTestCase);
            Console.WriteLine($"--- Test Case Finished: {testCase.Name} ---");
          }
          catch (Exception ex)
          {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"!! CRITICAL ERROR during Framework.Run for test case {testCase.Name}: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
            // Record this catastrophic failure directly if the test case didn't get a chance to report
            testRunner.RecordResult(testCase.Name, testCase.Type, NightTest.Core.TestStatus.Failed, 0, $"Catastrophic failure during test case execution: {ex.Message}");
          }
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Error: Test case {testCase.Name} does not implement Night.IGame and cannot be run.");
          Console.ResetColor();
          testRunner.RecordResult(testCase.Name, testCase.Type, NightTest.Core.TestStatus.Failed, 0, "Test case does not implement Night.IGame.");
        }
        // Small delay between tests to make console output more readable
        System.Threading.Thread.Sleep(500);
      }

      Console.WriteLine("\nAll selected test cases have been processed.");
      testRunner.GenerateReport();

      Console.WriteLine("NightTest Orchestrator Exiting.");
    }
  }
}
