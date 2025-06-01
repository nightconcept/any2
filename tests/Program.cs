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
    /// Orchestrates the test scenarios.
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

      var testRunner = new TestRunner(reportPath, runAutomatedOnly, args);

      // List of MODULES to load tests from.
      var testModules = new List<ITestModule>
            {
                new DummyModule(),
                new TimerModule(),
                // new GraphicsModule(), // Example for a future test module
                // new InputModule(),    // Example for a future test module
            };

      var allScenariosFromModules = new List<ITestScenario>();
      foreach (var module in testModules)
      {
        try
        {
          allScenariosFromModules.AddRange(module.GetTestScenarios());
        }
        catch (Exception ex)
        {
          Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine($"!! CRITICAL ERROR loading scenarios from module {module.GetType().Name}: {ex.Message}");
          Console.WriteLine(ex.StackTrace);
          Console.ResetColor();
          // Optionally, add a failed test result to the runner for this module loading failure.
          // testRunner.RecordResult($"ModuleLoadFailure.{module.GetType().Name}", TestType.Automated, TestStatus.Failed, 0, $"Failed to load scenarios: {ex.Message}");
        }
      }

      // Register all discovered scenarios with the runner for comprehensive reporting
      foreach (var scenario in allScenariosFromModules)
      {
        testRunner.AddScenarioToRegistry(scenario);
      }

      var scenariosToRun = new List<ITestScenario>();
      if (runAutomatedOnly)
      {
        scenariosToRun.AddRange(allScenariosFromModules.Where(s => s.Type == TestType.Automated));
        Console.WriteLine($"\n--run-automated specified. Will run {scenariosToRun.Count} automated scenario(s) out of {allScenariosFromModules.Count} discovered.");
      }
      else
      {
        scenariosToRun.AddRange(allScenariosFromModules);
        Console.WriteLine($"\nWill attempt to run all {scenariosToRun.Count} discovered scenario(s).");
      }

      foreach (var scenario in scenariosToRun)
      {
        Console.WriteLine($"\n--- Starting Scenario: {scenario.Name} ([{scenario.Type}]) ---");
        scenario.SetTestRunner(testRunner); // Provide the runner to the scenario

        if (scenario is Night.IGame gameScenario)
        {
          try
          {
            // Each scenario (which is an IGame) runs in its own Framework.Run call
            Night.Framework.Run(gameScenario);
            Console.WriteLine($"--- Scenario Finished: {scenario.Name} ---");
          }
          catch (Exception ex)
          {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"!! CRITICAL ERROR during Framework.Run for scenario {scenario.Name}: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
            // Record this catastrophic failure directly if the scenario didn't get a chance to report
            testRunner.RecordResult(scenario.Name, scenario.Type, TestStatus.Failed, 0, $"Catastrophic failure during scenario execution: {ex.Message}");
          }
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Error: Scenario {scenario.Name} does not implement Night.IGame and cannot be run.");
          Console.ResetColor();
          testRunner.RecordResult(scenario.Name, scenario.Type, TestStatus.Failed, 0, "Scenario does not implement Night.IGame.");
        }
        // Small delay between tests to make console output more readable
        System.Threading.Thread.Sleep(500);
      }

      Console.WriteLine("\nAll selected scenarios have been processed.");
      testRunner.GenerateReport();

      Console.WriteLine("NightTest Orchestrator Exiting.");
    }
  }
}
