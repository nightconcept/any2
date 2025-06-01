# NightTest - Task List: Infrastructure, SDL & Testing Guidance

## 0. NightTest Infrastructure Setup

### Task 0.1: Implement Core NightTest Framework Infrastructure

- **Goal:** Establish the foundational components of the `NightTest` application, including the project structure, test scenario interface, test runner, and the main application loop to execute tests and generate reports. This task is based on the `NightTest - Product Requirements Document (Revision 1)`.
- **Key Components from PRD:**
  - `NightTest` C# Project (`NightTest.csproj`, `Program.cs`/`Game.cs`)
  - `ITestScenario` interface (conceptualized in PRD)
  - `TestRunner.cs` (for test management and reporting)
  - Command-line argument parsing (`--run-automated`, `--report-path`)
  - JSON report generation (`test_report.json`)
- **Implementation Details:**
    1. **Project Setup:**
        - Create the `NightTest` C# project (e.g., Console App) under `/tests/NightTest/`.
        - Ensure it references the `Night` framework/engine library.
        - Set up basic `Program.cs` to host the `Night.Framework.Run()` loop with an `IGame` implementation tailored for test management.
    2. **`ITestScenario` Interface:**
        - Define `ITestScenario.cs` as per the PRD:

            ```csharp
            public enum TestType { Automated, Manual }
            public interface ITestScenario
            {
                string Name { get; }
                TestType Type { get; }
                string Description { get; }
                void Load(Night.IGraphics graphics, /* other relevant Night systems */);
                void Update(float deltaTime, TestRunner reporter);
                void Draw(Night.IGraphics graphics);
                void OnKey(Night.KeyCode key, bool isPressed, TestRunner reporter);
                // Add other relevant IGame methods like OnMouse, Unload etc.
            }
            ```

    3. **`TestRunner.cs` Implementation:**
        - Create `TestRunner.cs`.
        - Implement methods for:
            - Registering an `ITestScenario`.
            - Tracking the state of each test (Name, Type, Status: Passed/Failed/Not Run, Duration, Details).
            - `ReportPass(string testName, string details)`
            - `ReportFail(string testName, string details)`
            - Executing tests based on the `--run-automated` flag.
            - Calculating test duration.
            - Generating the `test_report.json` file as specified in the PRD (including summary section).
            - Generating the console/log summary.
    4. **Main Application Logic (`Program.cs`/`Game.cs`):**
        - Parse command-line arguments (`--run-automated`, `--report-path`).
        - Instantiate `TestRunner`.
        - Implement a mechanism to discover and register all `ITestScenario` implementations (e.g., using reflection to find classes implementing `ITestScenario`, or a manual registration list).
        - In the `IGame` implementation:
            - `Load()`: Load all registered test scenarios (or filter based on CLI args). Call `Load()` on active test scenarios.
            - `Update(float deltaTime)`: Iterate through active test scenarios and call their `Update()` method, passing the `TestRunner` instance. Manage transitions between scenarios if they run sequentially or one at a time.
            - `Draw()`: Call `Draw()` on the current test scenario. Optionally display test status or instructions on screen.
            - Input methods (`OnKey`, etc.): Forward to the current test scenario.
            - On exit/completion: Trigger `TestRunner` to generate the final `test_report.json` and console output.
- **Acceptance Criteria:**
  - `NightTest` application can be built and run.
  - It can parse the `--run-automated` and `--report-path` command-line arguments.
  - An `ITestScenario` can be created and registered.
  - The `TestRunner` can execute registered scenarios (respecting the automated filter).
  - A `test_report.json` file with the correct structure and a console summary are generated upon completion.
  - The basic framework allows for subsequent tasks (like testing `Night.NightSDL`) to be implemented by creating new `ITestScenario` classes.
- **Status:** To Do

## 1. Night Library Module Tests

### Task 1.1: Test `Night.NightSDL` Functionality

- **Goal:** Verify the core functionality of the `Night.NightSDL` static class, ensuring its methods correctly interact with the underlying SDL3 library and report information as expected.
- **Affected Code:** `Night.NightSDL.cs`

    ```csharp
    // namespace Night
    // {
    //   public static class NightSDL
    //   {
    //     public static string GetVersion()
    //     {
    //       int sdl_version = SDL.GetVersion();
    //       int major = sdl_version / 1000000;
    //       int minor = (sdl_version / 1000) % 1000;
    //       int patch = sdl_version % 1000;
    //       return $"{major}.{minor}.{patch}";
    //     }
    //     public static string GetError()
    //     {
    //       return SDL.GetError();
    //     }
    //   }
    // }
    ```

- **Test Scenarios & Implementation Details:**
  - Create a new `ITestScenario` implementation (e.g., `NightSDLTests.cs`) within the `NightTest` project.
  - **Scenario 1.1.1: Verify `NightSDL.GetVersion()`**
    - **Type:** Automated
    - **Action:**
      - In the `Load` or `Update` method of the test scenario, call `Night.NightSDL.GetVersion()`.
      - Call `Night.NightSDL.GetError()` immediately after to ensure no SDL error was triggered by `GetVersion()`.
    - **Expected Result:**
      - The returned version string should be in the format "X.Y.Z" where X, Y, and Z are non-negative integers.
      - `NightSDL.GetError()` should return an empty string (or a previously existing error, if `SDL.ClearError()` is not called before, which might be a point for refinement in the test or `NightSDL` itself).
    - **Reporting:**
      - Report "Passed" if the format is correct and no new SDL error is reported.
      - Report "Failed" otherwise, with details on the incorrect format or the SDL error message.
  - **Scenario 1.1.2: Verify `NightSDL.GetError()` (No Error State)**
    - **Type:** Automated
    - **Action:**
      - In the `Load` or `Update` method, (optionally call `SDL.ClearError()` if direct SDL calls are permissible in tests, or ensure a known good state) then call `Night.NightSDL.GetError()`.
    - **Expected Result:**
      - The method should return an empty string, assuming no prior SDL error has occurred and remains uncleared.
    - **Reporting:**
      - Report "Passed" if an empty string is returned.
      - Report "Failed" if a non-empty string is returned (unless a preceding intentional error was triggered for testing `GetError` with an actual message).
  - **Scenario 1.1.3: Verify `NightSDL.GetError()` (With Error State - Optional/Advanced)**
    - **Type:** Automated (if an error can be reliably and safely triggered) or Manual
    - **Action:**
      - Attempt to trigger a known, non-fatal SDL error (e.g., loading a non-existent image file if that uses SDL directly and is simple to set up *before* `Night.Graphics` abstracts it). This is tricky and might be better for more direct SDL binding tests rather than `NightSDL` tests.
      - Call `Night.NightSDL.GetError()`.
    - **Expected Result:**
      - The method should return a non-empty string containing the SDL error message.
    - **Reporting:**
      - Report "Passed" if a non-empty, relevant error string is returned.
      - Report "Failed" otherwise.
- **Acceptance Criteria:**
  - All calls to `NightSDL` methods are made through one of the `IGame` interface methods (`Load`, `Update`, `Draw`, `KeyPressed`, etc.) within an `ITestScenario`.
  - The `TestRunner` correctly logs the outcome (Passed/Failed) for each verification.
  - The `test_report.json` includes entries for these tests.
- **Status:** To Do

## 2. Test Writing Guidance

### Task 2.1: Define Guidance for Writing `NightTest` Scenarios

- **Goal:** Create a clear, documented process and set of best practices for developers (and AI assistants) to write new test scenarios using the `NightTest` framework.
- **Content to Define:**
    1. **Overview of `NightTest` Philosophy:**
        - Emphasize testing through the `IGame` loop (`Load`, `Update`, `Draw`, `KeyPressed`, etc.).
        - Explain the role of `ITestScenario` and `TestRunner`.
        - Differentiate between "Automated" and "Manual" tests.
    2. **Steps to Create a New Test Scenario:**
        - Creating the `ITestScenario` class file (e.g., `MyFeatureTests.cs`).
        - Implementing the `ITestScenario` interface:
            - `Name`: Unique and descriptive.
            - `Type`: `TestType.Automated` or `TestType.Manual`.
            - `Description`: Brief explanation of what is being tested.
            - `Load()`: For setting up resources, initial state.
            - `Update(float deltaTime, TestRunner reporter)`: For test logic, assertions, and calls to `reporter.ReportPass()` or `reporter.ReportFail()`. This is the primary place for automated test execution.
            - `Draw(Night.IGraphics graphics)`: For visual feedback, especially for manual tests or visual automated tests.
            - `OnKey()`, `OnMouse()` etc.: For handling input, crucial for manual tests and some automated input simulation tests.
            - `Unload()`: For cleaning up resources (if necessary).
        - Registering the new scenario with the `TestRunner` (mechanism TBD - e.g., manual registration list in `Program.cs` or discovery via reflection).
    3. **Writing Test Logic:**
        - How to call engine APIs.
        - How to assert conditions (e.g., checking return values, state changes).
        - Using `TestRunner.ReportPass(string testName, string details)` and `TestRunner.ReportFail(string testName, string details)`.
        - Structuring multiple checks within a single `ITestScenario` (e.g., one scenario might have multiple `reporter.ReportPass/Fail` calls for sub-tests, or each scenario is one specific test).
        - Timing considerations for automated tests (e.g., allowing a few frames for an action to complete).
    4. **Guidelines for Automated Tests:**
        - Must run without any user interaction.
        - Assertions must be programmatic.
        - Should be deterministic.
        - Focus on API return values, state changes, and non-visual outcomes primarily.
    5. **Guidelines for Manual Tests:**
        - Clearly describe the required user actions in the `Description` or via on-screen instructions.
        - Provide visual feedback in `Draw()`.
        - Use `OnKey` or other input handlers to allow the user to confirm pass/fail if necessary, or for the test to react to user input.
        - Example: "Press Space if the sprite is red. Press Backspace if not."
    6. **Reporting and `test_report.json`:**
        - Briefly explain how the test results contribute to the JSON report.
        - Importance of clear `testName` and `details` in `ReportPass/Fail`.
    7. **Example `ITestScenario` (Simple Automated Test):**

        ```csharp
        // public class ExampleAutomatedTest : ITestScenario
        // {
        //     public string Name => "Night.Module.ExampleFunctionality";
        //     public TestType Type => TestType.Automated;
        //     public string Description => "Tests if ExampleFunctionality returns the expected value.";
        //
        //     private bool testDone = false;
        //
        //     public void Load(Night.IGraphics graphics /*...other systems...*/) { /* No setup needed */ }
        //
        //     public void Update(float deltaTime, TestRunner reporter)
        //     {
        //         if (testDone) return;
        //
        //         // Assume Night.Module.ExampleFunctionality() is what we're testing
        //         var result = Night.Module.ExampleFunctionality();
        //         if (result == "expectedValue")
        //         {
        //             reporter.ReportPass(Name, "ExampleFunctionality returned 'expectedValue' as expected.");
        //         }
        //         else
        //         {
        //             reporter.ReportFail(Name, $"ExampleFunctionality returned '{result}', but 'expectedValue' was expected.");
        //         }
        //         testDone = true;
        //     }
        //
        //     public void Draw(Night.IGraphics graphics) { /* No visual feedback needed for this automated test */ }
        //     public void OnKey(Night.KeyCode key, bool isPressed, TestRunner reporter) { /* Not used */ }
        //     // ... other ITestScenario methods ...
        // }
        ```

- **Output:** A new Markdown document (e.g., `TESTING_GUIDELINES.md`) in the project's `docs` or `NightTest` directory.
- **Acceptance Criteria:** The guidelines are comprehensive enough for a developer or an AI to understand how to add new tests to the `NightTest` framework.
- **Status:** To Do
