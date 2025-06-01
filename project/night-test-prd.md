# NightTest - Product Requirements Document (Revision 1.1 - Architectural Update)

## 1. Introduction

- **Project Idea:** `NightTest` is a dedicated C# application for testing the functionalities of the `Night` Framework and the future `Night.Engine`. It will serve as an interactive testbed where new engine features can be continuously integrated and visually validated.

- **Problem/Need:** While unit tests are valuable, many game engine features, especially those related to UI, rendering, and input, require actual execution within a game loop to be effectively tested. `NightTest` addresses this by providing a runnable environment that can be easily extended to exercise new and existing engine capabilities. Some tests may require manual interaction or visual confirmation, while others can be fully automated.

- **Development Goal:** To create a maintainable and extensible test game that facilitates the iterative development and testing of the Night engine. This includes implementing a robust reporting mechanism that can track exercised functions, their status (Passed/Failed/Not Run/Skipped), their type (Automated/Manual), and produce machine-readable output for CI validation.

## 2. Core Features

### 2.1. NightTest Foundation

- **Description:** `NightTest` is a C# application. Its `Program.cs` acts as an orchestrator that iterates through a defined list of test cases. Each test case is a self-contained `Night.IGame` instance. `Program.cs` launches each selected test case using `Night.Framework.Run()`. It also parses command-line arguments to control test execution (e.g., filtering for automated tests).

- **Status:** Implemented (Core Orchestration).

### 2.2. Test Case Management

- **Description:** Test cases are organized into "Test Groups". Each group is a class implementing the `NightTest.Core.ITestGroup` interface, which defines a method to provide a collection of individual test cases.
    - Each individual test case is then implemented as a class that fulfills two contracts:
        1.  `NightTest.Core.ITestCase`: Provides metadata like test name, type (Automated/Manual), and description. It also includes a method for the orchestrator to provide a `TestRunner` instance.
        2.  `Night.IGame`: Implements the necessary methods (`Load`, `Update`, `Draw`, input handlers, etc.) to run as a standalone game via `Night.Framework.Run()`.

  - Each test case is responsible for:
    - Managing its own lifecycle (initialization, updates, drawing).
    - Determining its pass/fail status based on its specific test logic.
    - Measuring its own execution duration.
    - Reporting its result (name, type, status, duration, details) to the central `TestRunner` instance upon completion (typically before it signals `Night.Window.Close()`).
    - For `Manual` tests, current implementations (e.g., `ConcreteDummyManualTest`) use simple keyboard inputs (e.g., Space to pass, Escape to fail) within their own `IGame` loop. A more generalized or UI-driven approach for manual test interaction is a future consideration (see Section 6).

  - `Program.cs` (in `tests/` with namespace `NightTest`) discovers/instantiates `NightTest.Core.ITestGroup` implementations (e.g., from a predefined list within `Program.cs` itself, or by scanning assemblies in the `Modules/` directory (ideally renamed to `Groups/`) in the future). It then calls a method on each group to get all its `NightTest.Core.ITestCase` objects.
  - `Program.cs` supports a command-line argument (e.g., `--run-automated`) to filter the execution to only "automated" test cases. If the argument is not provided, all defined tests will be attempted.

- **Status:** Implemented (Core Management, Group-based Execution, Basic Manual Test Input).

### 2.3. Test Reporting Object

- **Description:** A dedicated reporting object (`NightTest.Core.TestRunner`) collects and aggregates results from all executed test cases.
  - `Program.cs` instantiates `NightTest.Core.TestRunner` and provides it to each `NightTest.Core.ITestCase` before it runs.
  - Each `NightTest.Core.ITestCase` (acting as an `IGame`) calls a method on the `NightTest.Core.TestRunner` instance (e.g., `RecordResult()`) to submit its outcome (name, type, status, duration, details) when it finishes.
  - `NightTest.Core.TestRunner` also maintains a registry of all known test cases (even if not run due to filtering) to provide a comprehensive summary.

- **Output:** After all selected test cases have been processed, `Program.cs` instructs the `NightTest.Core.TestRunner` to generate:

    1. **Primary Output: `test_report.json`**: A JSON file detailing the status of all tracked tests (including executed and skipped ones).
        - Example `test_report.json` structure (updated `status` values and `skipped` counts):
            ```json
            {
              "reportTitle": "NightTest Engine Test Report",
              "runTimestamp": "YYYY-MM-DDTHH:mm:ssZ",
              "commandLineArgs": ["--run-automated"],
              "tests": [
                {
                  "name": "Night.Graphics.NewImage",
                  "type": "automated",
                  "status": "Passed",
                  "durationMs": 150,
                  "details": "Image loaded successfully."
                },
                {
                  "name": "Night.Input.ManualKeyPressVerification",
                  "type": "manual",
                  "status": "Skipped",
                  "durationMs": 0,
                  "details": "Skipped: --run-automated flag was used."
                },
                {
                  "name": "Night.Audio.PlaySoundEffect",
                  "type": "automated",
                  "status": "Failed",
                  "durationMs": 50,
                  "details": "Audio device not found or sound file corrupted."
                },
                {
                  "name": "Night.Mouse.GetPositionBoundary",
                  "type": "manual",
                  "status": "Not Run",
                  "durationMs": 0,
                  "details": "Skipped due to --run-automated flag."
                }
              ],
              "summary": {
                "filterApplied": "automated_only",
                "totalRegistered": 4,
                "totalAttemptedToRun": 1,
                "totalPassed": 1,
                "totalFailed": 0,
                "totalSkipped": 3,
                "totalNotRun": 0,
                "automated": {
                  "registered": 2,
                  "attempted": 1,
                  "passed": 1,
                  "failed": 0,
                  "skipped": 1,
                  "notRun": 0
                },
                "manual": {
                  "registered": 2,
                  "attempted": 0,
                  "passed": 0,
                  "failed": 0,
                  "skipped": 2,
                  "notRun": 0
                }
              }
            }
            ```

    2. **Secondary Output: Console/Log**: A human-readable text-based summary output to the console.
        - Example console output (reflecting new status types):
            ```
            Night Engine Test Report:
            -------------------------
            Command: NightTest.exe --run-automated
            [Automated] Night.Graphics.NewImage: Passed (150ms) - Image loaded successfully.
            [Manual]    Night.Input.ManualKeyPressVerification: Skipped (0ms) - Skipped: --run-automated flag was used.
            [Automated] Night.Audio.PlaySoundEffect: Failed (50ms) - Audio device not found or sound file corrupted.
            [Manual]    Night.Mouse.GetPositionBoundary: Not Run (Skipped due to --run-automated flag)
            -------------------------
            Filter Applied: automated_only
            Summary:
              Total Registered: 4 (Automated: 2, Manual: 2)
              Total Attempted: 1
              Passed: 1 (Automated: 1, Manual: 0)
              Failed: 0 (Automated: 0, Manual: 0)
              Skipped: 3 (Automated: 1, Manual: 2)
              Not Run (Explicit): 0 (Automated: 0, Manual: 0)
            ```
- **Status:** Implemented (Core Reporting).

### 2.4. Continuous Integration of Tests

- **Description:** As new features are added to `Night` or `Night.Engine`, corresponding test cases (marked as "automated" or "manual") and reporting calls will be added to `NightTest`. The generated `test_report.json` will be used by CI scripts to validate that all expected automated tests pass.

- **Status:** Ongoing.

## 3. Technical Specifications

- **Primary Language(s):** C# 13 (using .NET 9), consistent with the main Night Engine project.

- **Key Frameworks/Libraries:**

  - `Night.Framework` (and eventually `Night.Engine`)

  - SDL3 (via `Night.Framework`)

  - System.CommandLine (or similar library) for parsing command-line arguments (optional, can be manual parsing for a single flag).

- **Command-Line Interface:**

  - `NightTest.exe [options]`

  - **Options:**

    - `--run-automated`: (Optional) If present, only test cases marked as "automated" will be executed.

    - `--report-path <filepath>`: (Optional) Specify a custom path for the `test_report.json` file.

- **Project Structure:**

  - A project directory: `/tests/` (Houses `NightTest.csproj`, `Program.cs`)
  - Core framework files in: `/tests/Core/` (contains `ITestGroup.cs`, `ITestCase.cs`, `TestRunner.cs`, `TestTypes.cs`, `BaseTestCase.cs` - all in `NightTest.Core` namespace)
  - Test Group implementations in: `/tests/Modules/` (or preferably `/tests/Groups/`)

  - `NightTest.csproj` (in `/tests/`)

  - `Program.cs` (in `/tests/`): Orchestrates test execution. Initializes `NightTest.Core.TestRunner`. Instantiates `NightTest.Core.ITestGroup` implementations, retrieves all `NightTest.Core.ITestCase` objects from them, and then iterates through these test cases, casting them to `Night.IGame` and running each via `Night.Framework.Run()`. Parses command-line arguments. Triggers final report generation.

  - `TestRunner.cs` (in `/tests/Core/`): Collects test results reported by individual test cases. Generates JSON and console reports upon request from `Program.cs`.

  - `ITestGroup.cs` (in `/tests/Core/`): Interface defining a contract for test groups. Each group is responsible for providing a list of `NightTest.Core.ITestCase`s.
        ```csharp
        // Located in tests/Core/ITestGroup.cs
        namespace NightTest.Core;
        public interface ITestGroup
        {
            System.Collections.Generic.IEnumerable<ITestCase> GetTestCases();
        }
        ```

  - `ITestCase.cs` (in `/tests/Core/`): Interface defining metadata for a test case (Name, Type, Description) and a method `SetTestRunner(TestRunner runner)`. Classes implementing `ITestCase` are also required to implement `Night.IGame`.
        ```csharp
        // Located in tests/Core/ITestCase.cs
        namespace NightTest.Core;
        public enum TestType { Automated, Manual }
        public interface ITestCase
        {
            string Name { get; }
            TestType Type { get; }
            string Description { get; }
            void SetTestRunner(TestRunner runner);
        }
        ```
  - `TestTypes.cs` and `BaseTestCase.cs` also reside in `/tests/Core/` under the `NightTest.Core` namespace.

  - Directories for different test cases/groups (e.g., `/tests/Modules/Graphics/` (consider `/tests/Groups/Graphics/`), `/tests/Modules/Input/`), each containing group classes (implementing `NightTest.Core.ITestGroup`) which in turn define and provide their respective `NightTest.Core.ITestCase` classes (which implement `NightTest.Core.ITestCase` and `Night.IGame`).

- **Output:**

  - **Primary:** `test_report.json`

  - **Secondary:** Text-based summary to the console.

## 4. Project Structure Considerations (within `NightTest`)

```graph
graph TD
    A(NightTest Orchestrator - Program.cs in tests/) --> B(NightTest.csproj in tests/);
    A --> D(TestRunner.cs in tests/Core/); // Instantiated by Program.cs, collects results
    A --> G(ITestCase.cs in tests/Core/); // Interface definition
    A --> C(ITestGroup.cs in tests/Core/);
    A --> TT(TestTypes.cs in tests/Core/);
    A --> BTC(BaseTestCase.cs in tests/Core/);
    A --> EM(Groups/ in tests/); // Formerly Modules/

    subgraph Each Test Case is an IGame
        EM --> E1(ConcreteTest1.cs); // Implements ITestCase & Night.IGame
        EM --> E2(ConcreteTest2.cs); // Implements ITestCase & Night.IGame
    end

    E1 -- Night.Framework.Run() --> E1_GameLoop[Own Game Loop for ConcreteTest1];
    E2 -- Night.Framework.Run() --> E2_GameLoop[Own Game Loop for ConcreteTest2];

    E1_GameLoop -- Reports result to --> D;
    E2_GameLoop -- Reports result to --> D;

    A -- Iterates & Runs --> E1;
    A -- Iterates & Runs --> E2;

    A --> F(assets/); // For any specific assets needed for testing by test cases
    A --> H(output/); // Default output directory for reports
    H --> I(test_report.json); // Generated by TestRunner at end
```

## 5. Non-Goals

- This `NightTest` is not intended to be a fully-featured game. Its primary purpose is testing engine functionality.

- It is not a replacement for unit tests but a complement to them for integration, visual, and interactive testing.

- Initially, it will not feature a complex in-game UI for test selection beyond the command-line filtering; tests might be run sequentially as registered, respecting the automated/manual filter.

## 6. Future Considerations

- A simple in-game UI (e.g., using Dear ImGui) to select and run specific tests or suites, view `test_report.json` contents, and re-run failed tests.
- **TODO:** Develop a standardized or UI-driven mechanism for manual test interaction, allowing users to easily signal pass/fail for tests marked as `Manual` without relying on specific key presses hardcoded into each test. This could involve a simple overlay or a dedicated input phase.
- More sophisticated reporting formats (e.g., HTML with visual diffs or screenshots for graphics tests).

- Integration with an automated testing framework that can drive UI interactions if feasible for some "manual" tests.

- Tagging system for tests (e.g., "graphics", "input", "performance", "v0.1.0") to allow more granular filtering from the command line (e.g., `--tags "graphics,v0.1.0"`).

- Ability to specify which manual tests to run via command line if `--run-automated` is not specified (e.g. `--run-tests "TestName1,TestName2"`).
