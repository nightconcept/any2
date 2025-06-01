# Night.Engine Testing Plan (NightTest Framework)

## 1. Introduction and Philosophy

This document outlines the testing strategy for the `Night.Engine` project, specifically using the `NightTest` integration testing framework. The primary goal is to ensure the reliability and correctness of `Night.Framework` modules by executing them within a real game loop environment.

The testing philosophy is to treat each `Night.Framework` feature or function as a testable unit within a self-contained `NightTest` test case. Each test case runs as an individual `Night.IGame` instance, allowing for interaction with the `Night.Framework` as it would be used in a real application. This approach is crucial for validating features that depend on an initialized SDL environment and a running game loop.

This plan adheres to the standards and guidelines set forth in `guidelines.md` and aligns with the project goals detailed in `PRD.md` and the `NightTest` specific requirements in `project/night-test-prd.md`.

## 2. Testing Framework: NightTest

`NightTest` is the sole testing framework for `Night.Framework` and future `Night.Engine` components. It is a C# application designed for integration testing. Key components (located in `tests/Core/` and using `NightTest.Core` namespace):

-   **`ITestGroup`**: An interface (`NightTest.Core.ITestGroup`) implemented by classes that group related test cases (e.g., `TimerGroup.cs` for `Night.Timer` tests). Each group provides a collection of `ITestCase` objects.
-   **`ITestCase`**: An interface (`NightTest.Core.ITestCase`) defining metadata for a test (Name, Type, Description) and a method to receive a `TestRunner` instance. Classes implementing `ITestCase` MUST also implement `Night.IGame` to run as a test.
-   **`BaseTestCase`**: (Recommended) An abstract base class (`NightTest.Core.BaseTestCase`) that provides common functionality for test cases, such as a stopwatch for timing, basic `IGame` implementations, and helper methods for reporting results. Test cases should inherit from this.
-   **`TestRunner`**: A class (`NightTest.Core.TestRunner`) responsible for collecting results from all executed test cases and generating the final `test_report.json` and console output.
-   **`TestTypes.cs`**: Contains enums like `NightTest.Core.TestType` and `NightTest.Core.TestStatus`.

Other Components:
-   **`Program.cs` (located in `tests/` namespace `NightTest`)**: Orchestrates test discovery from `Modules/` (or `Groups/`), filters them, runs each selected test case by invoking `Night.Framework.Run()`, and instructs the `TestRunner` to generate reports.

## 3. Test Project Structure

-   **Main Project Directory:** `/tests/` (contains `NightTest.csproj`, `Program.cs`, etc.)
-   **Core Framework Files Directory:** `tests/Core/`
    -   `ITestGroup.cs`
    -   `ITestCase.cs`
    -   `BaseTestCase.cs`
    -   `TestRunner.cs`
    -   `TestTypes.cs`
-   **Test Groups Directory:** `tests/Modules/` (It is strongly recommended to rename this directory to `tests/Groups/` for consistency with `ITestGroup` terminology.)
    -   Each subdirectory corresponds to a `Night.Framework` feature group being tested (e.g., `tests/Modules/Timer/` or `tests/Groups/Timer/`).
    -   Group definition files (e.g., `TimerGroup.cs`) are placed directly in the `Modules/` (or `Groups/`) directory.
    -   Individual test case files (e.g., `GetTimeTest.cs`) are placed within their respective group's subdirectory (e.g., `Modules/Timer/GetTimeTest.cs` or `Groups/Timer/GetTimeTest.cs`).

## 4. Naming Conventions

-   **Test Group Files & Classes:** `[FrameworkFeature]Group.cs` (e.g., `TimerGroup.cs`, `GraphicsGroup.cs`).
-   **Test Case Files & Classes:** `[MethodOrFeatureUnderTest]Test.cs` (e.g., `GetTimeTest.cs` for `Timer.GetTime()`, `NewImageTest.cs` for `Graphics.NewImage()`).
-   **`ITestCase.Name` Property:** A string uniquely identifying the test, typically `"[Night.Framework.Module].[MethodOrFeatureShortDescription]"` (e.g., `"Timer.GetTime"`, `"Graphics.NewImage.ValidPath"`, `"Window.SetMode.FullScreen"`). This name is used in `test_report.json`.

## 5. Scope of Testing - Night.Framework Modules (via NightTest)

`NightTest` is used to test the static classes and methods within the `Night.Framework` (typically in the `Night` namespace). Each test runs as an `IGame` instance, allowing direct calls to `Night.Framework` functions.

Examples by module (refer to `project/PRD.md` for `Night.Framework` API details):

-   **`Night.Window` (`Window.cs`)**
    -   Examples: `SetModeBasicTest`, `SetTitleTest`, `WindowIsOpenTest`, `WindowCloseTest`.
    -   Focus: Parameter validation (nulls, invalid dimensions if checkable before SDL), C# logic. Test by setting values and, if possible, using `Night.Window` getters or observing behavior (e.g., does `Close()` actually lead to the test ending?).
-   **`Night.Graphics` (`Graphics.cs`)**
    -   Examples: `NewImageValidPathTest`, `NewImageInvalidPathTest`, `DrawSpriteTest`, `ClearScreenTest`, `PresentTest`.
    -   Focus: Parameter validation. `NewImageTest` attempts to load an image and checks the returned `Sprite` object. `DrawSpriteTest` calls `Night.Graphics.Draw`. Success is often indicated by the absence of crashes and, for `NewImage`, valid sprite properties. Actual rendering output is for manual visual confirmation if a manual test is designed.
-   **`Night.Keyboard` (`Keyboard.cs`)**
    -   Examples: `IsKeyDownValidKeyTest`, `IsKeyDownInvalidEnumTest`.
    -   Focus: Parameter validation. Automated tests can call `IsDown` with various `KeyCode` values. Full key press simulation is complex for automation; specific scenarios requiring actual key presses are better suited for `Manual` tests.
-   **`Night.Mouse` (`Mouse.cs`)**
    -   Examples: `IsMouseButtonDownTest`, `GetMousePositionTest`.
    -   Focus: Similar to Keyboard; parameter validation for button enums. Actual mouse interaction is for `Manual` tests.
-   **`Night.Timer` (`Timer.cs`)** (Refer to `TimerGroup.cs` for gold-standard examples)
    -   Examples: `GetTimeTest`, `GetFPSTest`, `GetDeltaTest`, `SleepAccuracyTest`, `StepFunctionalityTest`.
    -   Focus: Accuracy of timing functions, correct calculation of FPS/delta, behavior of `Sleep`. These tests often involve measuring time using `System.Diagnostics.Stopwatch` within the test case.
-   **`FrameworkLoop.cs` (`Night.Framework.Run`, event handling, `IGame` lifecycle)**
    -   Tested implicitly by every `NightTest` case. Specific aspects like `Night.Timer` integration (delta time, FPS) are tested via `TimerGroup`. Event handling (`KeyPressed`, etc.) can be tested by creating test cases that implement these `IGame` methods and verify they are called.
-   **`Types.cs` (e.g., `Night.Color`, `Night.Rectangle`, `Night.Sprite`)**
    -   Examples: `ColorToSDLColorTest`, `RectangleConstructorTest`, `SpritePropertiesTest`.
    -   Focus: Test constructors, methods, and property access on these data structures. Logic is typically self-contained and verifiable within the test case's `Load()` or `Update()`.

## 6. Test Case Design within NightTest

All test cases, whether automated or manual, are classes that implement `NightTest.Core.ITestCase` and `Night.IGame`. They should ideally inherit from `NightTest.Core.BaseTestCase`.

-   **Automated Tests (`TestType.Automated`):**
    -   Primary logic resides in `Load()` and/or `Update(double deltaTime)`.
    -   The test performs actions using `Night.Framework` functions.
    -   It then asserts conditions to determine pass/fail status (e.g., checking return values, querying framework state, measuring time, ensuring no exceptions for valid operations).
    -   Sets `CurrentStatus` (e.g., `TestStatus.Passed`, `TestStatus.Failed`) and `Details` (a string explaining the outcome).
    -   Calls `QuitSelf()` (a method likely provided by `BaseTestCase`) to signal completion, which then reports the result via the `TestRunner` and closes the test's game window.
    -   **Focus Areas:**
        -   **Parameter Validation:** Test with valid and invalid inputs (nulls, out-of-range). For invalid inputs, the test might pass if the `Night.Framework` method handles it gracefully (e.g., returns null, throws a documented exception that is caught, or doesn't crash).
        -   **Return Value Verification:** Check if methods return expected values or objects with correct properties.
        -   **State Changes:** If a `Night.Framework` function is supposed to change an observable state, verify it.
        -   **Idempotency:** If applicable, test if calling a function multiple times has the same effect as calling it once.
        -   **No Crashes:** A fundamental check is that the test case runs to completion without unhandled exceptions from the `Night.Framework` for valid operations.

-   **Manual Tests (`TestType.Manual`):**
    -   Structurally similar to automated tests but require human interaction.
    -   `Load()` or `Draw()` should display clear on-screen instructions for the tester.
    -   `Update()` or `KeyPressed()` will listen for specific inputs (e.g., Spacebar to indicate "Pass", Escape for "Fail").
    -   Based on user input, set `CurrentStatus` and `Details`, then call `QuitSelf()`.

-   **Error Handling in Tests:**
    -   If a `Night.Framework` method is *expected* to throw an exception for certain inputs (as part of its contract), the test case should catch this specific exception and can be marked as `TestStatus.Passed`.
    -   Any *unexpected* unhandled exceptions originating from the `Night.Framework` during a test's execution should ideally lead to the test being marked as `TestStatus.Failed` by the test harness or by `BaseTestCase`'s global error handling, if implemented.

## 7. Writing a NightTest Test Case: AI Guidance

This section provides a step-by-step guide for an AI to create new `NightTest` test cases. The `tests/Modules/TimerGroup.cs` (or `tests/Groups/TimerGroup.cs`) and its associated test case files serve as the gold standard.

**Step 1: Understand the Target**
   - Identify the `Night.Framework` module (e.g., `Night.Window`).
   - Identify the specific method or feature to test (e.g., `Night.Window.SetTitle(string title)`).
   - Determine the specific scenario (e.g., setting a valid title, setting a null title).

**Step 2: Create or Locate Test Group File**
   - Navigate to `tests/Modules/` (or `tests/Groups/` if renamed).
   - If a group file for the framework feature doesn't exist (e.g., `WindowGroup.cs`), create it in this directory.
   - The group class implements `NightTest.Core.ITestGroup`. Its primary role is to provide a list of `NightTest.Core.ITestCase` objects for that framework feature group.
     ```csharp
     // File: tests/Modules/WindowGroup.cs (or tests/Groups/WindowGroup.cs)
     using System.Collections.Generic;
     using NightTest.Core; // For ITestGroup, ITestCase
     // Potentially: using NightTest.Modules.Window; (if test cases are in a sub-namespace)

     namespace NightTest.Modules // Or NightTest.Groups if you rename the parent folder and adjust namespaces for groups
     {
         public class WindowGroup : ITestGroup
         {
             public IEnumerable<ITestCase> GetTestCases()
             {
                 return new List<ITestCase>
                 {
                     // Example: new NightTest.Modules.Window.SetTitleValidTest(),
                     new SetTitleValidTest(), // If SetTitleValidTest is in the same namespace
                     new SetTitleNullTest()
                 };
             }
         }
     }
     ```

**Step 3: Create Test Case Class File**
   - Create a new C# class file for the specific test case within the appropriate group subdirectory (e.g., `tests/Modules/Window/SetTitleValidTest.cs` or `tests/Groups/Window/SetTitleValidTest.cs`).
   - The class should:
     - Inherit from `NightTest.Core.BaseTestCase`.
     - Implement `Night.IGame` (satisfied by `BaseTestCase`).
     - Override `Name`, `NightTest.Core.TestType`, and `Description` properties from `NightTest.Core.ITestCase`.

     ```csharp
     // File: tests/Modules/Window/SetTitleValidTest.cs (or tests/Groups/Window/SetTitleValidTest.cs)
     using Night; // For IGame, Night.Window, Night.Framework etc.
     using NightTest.Core; // For BaseTestCase, TestType, TestStatus

     namespace NightTest.Modules.Window // Or NightTest.Groups.Window
     {
         public class SetTitleValidTest : BaseTestCase
         {
             public override string Name => "Window.SetTitle.Valid";
             public override TestType Type => TestType.Automated;
             public override string Description => "Tests Night.Window.SetTitle() with a valid string.";

             private string _testTitle = "Valid Test Title";

             public override void Load()
             {
                 base.Load(); // Important: Initializes BaseTestCase resources like TestStopwatch

                 try
                 {
                     Night.Window.SetTitle(_testTitle);
                     // Assuming Night.Window.GetTitle() exists for verification:
                     // string currentTitle = Night.Window.GetTitle();
                     // if (currentTitle == _testTitle)
                     // {
                     //     CurrentStatus = TestStatus.Passed;
                     //     Details = $"Window title set and verified to: '{_testTitle}'.";
                     // }
                     // else
                     // {
                     //     CurrentStatus = TestStatus.Failed;
                     //     Details = $"Failed to set/verify window title. Expected: '{_testTitle}', Got: '{currentTitle}'.";
                     // }

                     // If GetTitle() doesn't exist, and SetTitle doesn't throw for valid input,
                     // this is a basic pass condition (function executed without error).
                     CurrentStatus = TestStatus.Passed;
                     Details = $"Night.Window.SetTitle('{_testTitle}') executed without error.";
                 }
                 catch (Exception e)
                 {
                     CurrentStatus = TestStatus.Failed;
                     Details = $"Night.Window.SetTitle('{_testTitle}') threw an unexpected exception: {e.Message}";
                 }
                 QuitSelf(); // End the test
             }

             public override void Update(double deltaTime)
             {
                 // Often empty if logic is in Load() for simple automated tests.
                 // If test needs to run for a duration, logic goes here.
             }

             public override void Draw()
             {
                 // Often empty for automated tests unless visual feedback is part of the test logic.
                 // Night.Graphics.Clear(Night.Color.Black); // Example
                 // Night.Graphics.Present();
             }

             // Other IGame methods (KeyPressed, KeyReleased, etc.) can often be left
             // to BaseTestCase's default (empty) implementations if not used by the test.
         }
     }
     ```

**Step 4: Implement Test Logic**
   - **`Load()` Method:**
     - Call `base.Load()` if inheriting from `BaseTestCase`.
     - Perform setup specific to the test.
     - Execute the `Night.Framework` function(s) being tested.
     - Perform assertions/checks.
     - Set `CurrentStatus` (`TestStatus.Passed` or `TestStatus.Failed`).
     - Set `Details` to a descriptive string about the outcome.
     - Call `QuitSelf()` if the test concludes in `Load()`.
   - **`Update(double deltaTime)` Method:**
     - Use if the test needs to observe behavior over time or run for a specific duration (see `TimerGroup` tests like `GetTimeTest`).
     - Continuously check conditions.
     - When the end condition is met, set `CurrentStatus`, `Details`, and call `QuitSelf()`.
     - Remember to check `if (IsDone) return;` at the beginning if `BaseTestCase` uses this pattern.
   - **`Draw()` Method:**
     - Usually empty for automated tests unless visual output is part of a manual verification step within an automated test structure (rare). If implementing, ensure `Night.Graphics.Clear()` and `Night.Graphics.Present()` are called appropriately.
   - **Input Methods (`KeyPressed`, etc.):**
     - Relevant for `Manual` tests or automated tests that verify input callback mechanisms.

**Step 5: Gold Standard Example - `TimerGroup.GetTimeTest` Logic** (Conceptual, refer to actual file for exact code)
   ```csharp
   // Conceptual structure from TimerGroup's GetTimeTest
   public class ExampleGetTimeTest : BaseTestCase
   {
       public override string Name => "Timer.GetTime.Example";
       public override TestType Type => TestType.Automated;
       public override string Description => "Measures time passage using Timer.GetTime().";

       private double _startTimeFramework;
       private System.Diagnostics.Stopwatch _validationStopwatch = new System.Diagnostics.Stopwatch(); // For internal validation

       public override void Load()
       {
           base.Load(); // Starts BaseTestCase.TestStopwatch (measures total test duration)
           _startTimeFramework = Night.Timer.GetTime();
           _validationStopwatch.Start();
           // The test logic continues in Update()
       }

       public override void Update(double deltaTime)
       {
           if (IsDone) return;

           if (_validationStopwatch.Elapsed.TotalSeconds >= 0.5) // Let 0.5s of real time pass
           {
               _validationStopwatch.Stop();
               double endTimeFramework = Night.Timer.GetTime();
               double elapsedFramework = endTimeFramework - _startTimeFramework;
               double elapsedValidation = _validationStopwatch.Elapsed.TotalSeconds;

               // Check if the time reported by Night.Timer.GetTime() is close to stopwatch's time
               // Allow a reasonable margin for error (e.g., 10-15%)
               if (Math.Abs(elapsedFramework - elapsedValidation) < elapsedValidation * 0.15)
               {
                   CurrentStatus = TestStatus.Passed;
                   Details = $"Timer.GetTime() elapsed: {elapsedFramework:F3}s. Validation stopwatch: {elapsedValidation:F3}s. Difference is acceptable.";
               }
               else
               {
                   CurrentStatus = TestStatus.Failed;
                   Details = $"Timer.GetTime() elapsed: {elapsedFramework:F3}s. Validation stopwatch: {elapsedValidation:F3}s. Difference is too large.";
               }
               QuitSelf(); // End the test
           }
       }
   }
   ```

**Step 6: Register Test Case in its Group**
   - Open the corresponding group file (e.g., `WindowGroup.cs`).
   - Add an instance of your new test case class to the list returned by `GetTestCases()`.
     ```csharp
     // In WindowGroup.cs
     public IEnumerable<ITestCase> GetTestCases()
     {
         return new List<ITestCase>
         {
             // ... other tests ...
             new SetTitleValidTest(), // Added
             // new SetTitleNullTest(), // If you created this too
         };
     }
     ```

## 8. Dealing with SDL Dependencies in NightTest

-   `NightTest` inherently handles SDL dependencies because it runs each test case as a full `Night.IGame` instance using `Night.Framework.Run()`. This means SDL is initialized, and `Night.Framework` functions that call SDL can be executed directly.
-   Tests should focus on the C# logic within `Night.Framework`, parameter validation, and observable outcomes (return values, state changes readable from C#, or behavior that leads to the test passing/failing).
-   For graphics, an automated test can confirm `Night.Graphics.Draw` doesn't crash with valid parameters. Verifying actual visual output is typically reserved for `Manual` tests or by inspecting screenshots if such a feature is added to `NightTest`.
-   Mocking or faking SDL is generally not required or desired for `NightTest`, as its purpose is integration testing with the real SDL backend.

## 9. Running Tests

-   Tests are executed by running the `NightTest.exe` application (typically found in `tests/NightTest/bin/Debug/netX.X/`).
-   Command-line arguments:
    -   `NightTest.exe`: Runs all registered test cases (automated and manual).
    -   `NightTest.exe --run-automated`: Runs only test cases marked as `TestType.Automated`.
-   Output:
    -   Console: A summary of test results.
    -   `test_report.json`: A detailed JSON report of all tests (run, skipped, passed, failed), including duration and details. This file is generated in the `NightTest` executable's directory or a path specified by `--report-path`.

## 10. Test Maintenance

-   Tests must be kept up-to-date with changes in `Night.Framework`'s API or behavior.
-   Refactor tests alongside production code to maintain clarity, relevance, and accuracy.
-   Remove or update tests for deprecated or significantly changed functionality.
-   Ensure test assets (if any, stored in `tests/NightTest/assets/`) are correctly managed.

## 11. Continuous Integration (Future)

-   The `test_report.json` generated by `NightTest` is the primary artifact for CI.
-   CI scripts will execute `NightTest.exe --run-automated` and then parse `test_report.json` to verify that all automated tests pass and to check for regressions.

This testing plan provides a comprehensive foundation for developing and maintaining integration tests for `Night.Engine` using the `NightTest` framework, with a clear process for AI-assisted test creation.
