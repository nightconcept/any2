# Night Engine Testing Guidelines

This document provides guidelines for writing tests for the Night Engine, specifically for the `Night.Framework` and future `Night.Engine` components. Our testing framework is built using xUnit and a custom set of base classes that integrate with the `Night.IGame` interface.

## Table of Contents

1. [Overview](#overview)
2. [Core Concepts](#core-concepts)
    * [Test Groups](#test-groups)
    * [Test Cases (IGame Instances)](#test-cases-igame-instances)
    * [Automated vs. Manual Tests](#automated-vs-manual-tests)
3. [Directory Structure](#directory-structure)
4. [Naming Conventions](#naming-conventions)
    * [Files](#files)
    * [Classes](#classes)
    * [Methods in Test Groups](#methods-in-test-groups)
    * [Test Case `Name` Property](#test-case-name-property)
5. [Creating Tests: Step-by-Step](#creating-tests-step-by-step)
    * [Step 1: Identify the Module and Feature](#step-1-identify-the-module-and-feature)
    * [Step 2: Create a Test Case File and Class](#step-2-create-a-test-case-file-and-class)
        * [Automated Test Case Example](#automated-test-case-example)
        * [Manual Test Case Example](#manual-test-case-example)
    * [Step 3: Implement IGame Methods](#step-3-implement-igame-methods)
        * [`Load()`](#load)
        * [`Update(double deltaTime)`](#update)
        * [`Draw()`](#draw)
        * [Input Handling (`KeyPressed`, `MousePressed`)](#input-handling)
    * [Step 4: Manage Test Lifecycle and Status](#step-4-manage-test-lifecycle-and-status)
    * [Step 5: Create or Update the Test Group](#step-5-create-or-update-the-test-group)
6. [Key Base Classes and Interfaces](#key-base-classes-and-interfaces)
7. [Best Practices](#best-practices)

## 1. Overview

The primary goal of our testing strategy is to ensure the reliability and correctness of the `Night Engine`'s features. Tests are designed as `IGame` instances, allowing them to run within the engine's main loop, providing a realistic testing environment. xUnit is used as the test runner to discover and execute these `IGame` test instances via `TestGroup` classes.

## 2. Core Concepts

### Test Groups

* **Purpose:** To group related test cases for a specific module or major feature (e.g., `Filesystem`, `Graphics`).
* **Implementation:** Create a C# class that inherits from `NightTest.Core.TestGroup`.
* **Location:** `tests/Groups/[ModuleName]/[ModuleName]Group.cs`.
* **Example:** [`FilesystemGroup.cs`](../../tests/Groups/Filesystem/FilesystemGroup.cs)

### Test Cases (IGame Instances)

* **Purpose:** An individual test scenario that verifies a specific piece of functionality.
* **Implementation:** Create a C# class that:
  * Implements `Night.IGame`.
  * Inherits from `NightTest.Core.BaseGameTestCase` (for automated tests) or `NightTest.Core.BaseManualTestCase` (for manual tests).
* **Location:** Typically in `tests/Groups/[ModuleName]/[FeatureName]Tests.cs`. A single file can contain multiple related test case classes.
* **Example:** [`LinesTests.cs`](../../tests/Groups/Filesystem/LinesTests.cs) contains `FilesystemLines_ReadStandardFileTest`.

### Automated vs. Manual Tests

* **Automated Tests:**
  * Inherit from `BaseGameTestCase`.
  * Logic within `Update()` determines pass/fail status automatically.
  * `Type` property is `TestType.Automated`.
  * Marked with `[Trait("TestType", "Automated")]` in the `TestGroup`.
* **Manual Tests:**
  * Inherit from `BaseManualTestCase`.
  * Require user interaction to confirm pass/fail (e.g., visual confirmation).
  * Use `RequestManualConfirmation("Prompt message")` in the `Update()` method.
  * `Type` property is `TestType.Manual`.
  * Marked with `[Trait("TestType", "Manual")]` in the `TestGroup`.
  * Example: [`GraphicsClearColorTest`](../../tests/Groups/Graphics/GraphicsClearTest.cs)

## 3. Directory Structure

```
tests/
├── Core/                 # Base classes, interfaces, and core testing utilities
│   ├── BaseGameTestCase.cs
│   ├── BaseManualTestCase.cs
│   ├── ITestCase.cs
│   ├── TestGroup.cs
│   └── ...
└── Groups/               # Contains groups of tests, organized by engine module
    └── [ModuleName]/     # e.g., Filesystem, Graphics, Window
        ├── [ModuleName]Group.cs  # xUnit test class for the group
        ├── [Feature1]Tests.cs    # Contains IGame test case classes for Feature1
        └── [Feature2]Tests.cs    # Contains IGame test case classes for Feature2
```

## 4. Naming Conventions

Consistency in naming is crucial for maintainability.

### Files

* **Test Group Files:** `[ModuleName]Group.cs` (e.g., `FilesystemGroup.cs`, `GraphicsGroup.cs`)
* **Test Case Files:** `[FeatureName]Tests.cs` or `[SpecificTestName]Test.cs`. Generally, group related test cases for a sub-feature into one file. (e.g., `LinesTests.cs` for multiple filesystem line tests, `GraphicsClearTest.cs` for a specific graphics clear test).

### Classes

* **Test Group Classes:** `[ModuleName]Group` or `[ModuleName]Tests` (e.g., `FilesystemGroup`, `GraphicsTests`). Prefer `[ModuleName]Group` for consistency with the directory.
  * Example: `public class FilesystemGroup : TestGroup`
* **Test Case Classes:** `[ModuleName][FeatureName]_[SpecificBehavior]Test`
  * Example: `public class FilesystemLines_ReadStandardFileTest : BaseGameTestCase`
  * Example: `public class GraphicsClear_SkyBlueColorTest : BaseManualTestCase` (Adjusting example for clarity)

### Methods in Test Groups

* Each test case is run by a dedicated method in the Test Group class.
* **Method Name:** `Run_[TestCaseClassName]()`
  * Example: `public void Run_FilesystemLines_ReadStandardFileTest()`
  * Example: `public void Run_GraphicsClearColorTest()` (as seen in [`GraphicsGroup.cs`](../../tests/Groups/Graphics/GraphicsGroup.cs:55))

### Test Case `Name` Property

* This property is part of the `ITestCase` interface and is used for logging and identification.
* **Format:** `"[ModuleName].[FeatureName].[SpecificBehavior]"`
  * Example: `public override string Name => "Filesystem.Lines.ReadStandardFile";`
  * Example: `public override string Name => "Graphics.Clear.SkyBlueColor";`

## 5. Creating Tests: Step-by-Step

### Step 1: Identify the Module and Feature

Determine which part of the `Night Engine` you are testing. This will dictate the directory and naming. Refer to `project/PRD.md` for module names (e.g., `Night.Window`, `Night.Graphics`, `Night.Filesystem`).

### Step 2: Create a Test Case File and Class

1. Navigate to `tests/Groups/[ModuleName]/`.
2. Create a new C# file, e.g., `[FeatureName]Tests.cs`.
3. Inside this file, define your test case class.

#### Automated Test Case Example

```csharp
// In tests/Groups/MyModule/MyFeatureTests.cs
using NightTest.Core;
using Night; // Assuming Night.MyModule.MyFeature exists

namespace NightTest.Groups.MyModule
{
    public class MyModuleMyFeature_ExpectedBehaviorTest : BaseGameTestCase
    {
        public override string Name => "MyModule.MyFeature.ExpectedBehavior";
        public override string Description => "Tests that MyFeature exhibits the expected behavior under specific conditions.";

        // Implement Load, Update, Draw (if needed)
        protected override void Load()
        {
            base.Load(); // Important for base class setup
            // Your test setup logic here
            // e.g., initialize variables, set up mock data
        }

        protected override void Update(double deltaTime)
        {
            if (this.IsDone) return;

            // Your test assertion logic here
            // Example:
            // bool conditionMet = Night.MyModule.MyFeature.DoSomething();
            // if (conditionMet)
            // {
            //     this.CurrentStatus = TestStatus.Passed;
            //     this.Details = "MyFeature.DoSomething() returned true as expected.";
            // }
            // else
            // {
            //     this.CurrentStatus = TestStatus.Failed;
            //     this.Details = "MyFeature.DoSomething() returned false, expected true.";
            // }

            this.EndTest(); // Call when test logic is complete
        }
    }
}
```

#### Manual Test Case Example

```csharp
// In tests/Groups/MyModule/MyVisualFeatureTests.cs
using NightTest.Core;
using Night;

namespace NightTest.Groups.MyModule
{
    public class MyModuleMyVisualFeature_UserConfirmationTest : BaseManualTestCase
    {
        public override string Name => "MyModule.MyVisualFeature.UserConfirmation";
        public override string Description => "User must visually confirm that MyVisualFeature renders correctly.";

        protected override void Load()
        {
            base.Load();
            // Setup for visual output
            this.Details = "Test running, observe the visual output.";
        }

        protected override void Update(double deltaTime)
        {
            if (this.IsDone) return;

            // Logic to trigger the visual effect if needed

            // Request confirmation after a short delay or when ready
            if (this.TestStopwatch.ElapsedMilliseconds > this.ManualTestPromptDelayMilliseconds)
            {
                this.RequestManualConfirmation("Does the screen display [expected visual outcome] correctly?");
            }
        }

        protected override void Draw()
        {
            // Your drawing logic here
            // Night.Graphics.Clear(Color.Black);
            // Night.MyModule.MyVisualFeature.Render();
        }
    }
}
```

### Step 3: Implement IGame Methods

Override methods from `BaseGameTestCase` or `BaseManualTestCase` as needed.

#### `Load()`

* Called once when the test case starts.
* Use for one-time setup:
  * Initializing variables.
  * Creating temporary files (as in `FilesystemLines_ReadStandardFileTest`).
  * Loading assets.
* **Always call `base.Load();`** if you override it, especially if inheriting from `BaseManualTestCase` which has its own `InternalLoad` logic.
* If setup fails, set `this.CurrentStatus = TestStatus.Failed;`, provide `this.Details`, and call `this.EndTest();`.

#### `Update(double deltaTime)`

* Called every frame. This is where the main test logic and assertions reside.
* **Check `if (this.IsDone) return;`** at the beginning.
* Perform actions using the engine's API.
* Check conditions and set:
  * `this.CurrentStatus = TestStatus.Passed;` or `TestStatus.Failed;`
  * `this.Details = "Descriptive message about the outcome.";`
* For automated tests, call `this.EndTest();` when the assertion is complete.
* For manual tests, call `this.RequestManualConfirmation("Your question to the user");` when ready for user input. The `BaseManualTestCase` will handle `EndTest()` based on user input or timeout.
* Use try-catch blocks for operations that might throw exceptions, setting status and details accordingly.

#### `Draw()`

* Called every frame, after `Update()`.
* Use for any rendering required by the test, especially for manual tests where visual output is being verified.
* Example: `Night.Graphics.Clear(someColor); Night.Graphics.Draw(mySprite, ...);`
* For `BaseManualTestCase`, the pass/fail buttons are drawn automatically after your `Draw()` logic if confirmation is active. `Night.Graphics.Present()` is also handled by `BaseManualTestCase`.

#### Input Handling

* Override `KeyPressed`, `KeyReleased`, `MousePressed`, `MouseReleased` if your test needs to react to input.
* `BaseManualTestCase` uses `KeyPressed` (for ESC) and `MousePressed` (for UI buttons). If you override these in a manual test, ensure you call `base.KeyPressed(...)` or `base.MousePressed(...)` or replicate necessary base logic.

### Step 4: Manage Test Lifecycle and Status

* **`this.CurrentStatus`**: Set to `TestStatus.Passed` or `TestStatus.Failed`. Defaults to `TestStatus.NotRun`.
* **`this.Details`**: Provide a clear string explaining the test outcome or failure reason.
* **`this.EndTest()`**: Call this method when the test logic is complete (either passed or failed) to stop the test, close the window, and record the duration.
  * In automated tests, you typically call this at the end of your `Update()` logic.
  * In manual tests, this is often handled by `BaseManualTestCase` after user interaction or timeout, but can be called directly if an early exit condition is met.
* **Helper methods in `BaseGameTestCase`**:
  * `CheckCompletionAfterDuration(...)`: End test after a certain time.
  * `CheckCompletionAfterFrames(...)`: End test after a certain number of frames.
* **Error Handling**: Wrap potentially problematic code in `try-catch` blocks. On exception:

    ```csharp
    catch (System.Exception e)
    {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
        this.EndTest(); // Ensure test terminates
    }
    ```

* **Cleanup**: If your test creates resources (e.g., files), clean them up. This is often done in a `finally` block within the `Update` method, or if `Load` creates resources, ensure `Update` cleans them up before `EndTest`. See `FilesystemLines_ReadStandardFileTest` for an example of file cleanup in a `finally` block.

### Step 5: Create or Update the Test Group

1. Open or create `tests/Groups/[ModuleName]/[ModuleName]Group.cs`.
2. If creating, ensure it inherits from `NightTest.Core.TestGroup` and has a constructor accepting `ITestOutputHelper`.

    ```csharp
    // In tests/Groups/MyModule/MyModuleGroup.cs
    using Xunit;
    using Xunit.Abstractions;
    using NightTest.Core;
    // Potentially: using NightTest.Groups.MyModule.MyFeatureTests; // If test cases are in a different namespace

    namespace NightTest.Groups.MyModule
    {
        [Collection("SequentialTests")] // Important for tests that interact with the game window
        public class MyModuleGroup : TestGroup
        {
            public MyModuleGroup(ITestOutputHelper outputHelper)
                : base(outputHelper)
            {
            }

            // Add methods for each test case here
        }
    }
    ```

3. Add a new xUnit `[Fact]` method to run your test case:

    ```csharp
    [Fact]
    [Trait("TestType", "Automated")] // Or "Manual"
    public void Run_MyModuleMyFeature_ExpectedBehaviorTest() // Matches test case class name
    {
        this.Run_GameTestCase(new MyModuleMyFeature_ExpectedBehaviorTest());
    }
    ```

    * The `[Trait("TestType", ...)]` attribute helps in filtering tests.
    * The method name should be `Run_` followed by the test case class name.
    * Instantiate your test case class and pass it to `this.Run_GameTestCase()`.

## 6. Key Base Classes and Interfaces

* **`Night.IGame`**: Interface from the core engine that test cases implement to hook into the game loop (`Load`, `Update`, `Draw`, input methods).
* **`NightTest.Core.ITestCase`**: Defines properties for test identification (`Name`, `Type`, `Description`). Implemented by `BaseGameTestCase`.
* **`NightTest.Core.BaseGameTestCase`**:
  * Base class for all test cases, providing common functionality:
    * `TestStopwatch`, `CurrentStatus`, `Details`, `IsDone`, `CurrentFrameCount`.
    * Default `IGame` implementations that call virtual `InternalLoad`, `InternalUpdate`, `InternalDraw`, which in turn call protected virtual `Load`, `Update`, `Draw`.
    * Helper methods: `EndTest()`, `RecordFailure()`, `CheckCompletionAfterDuration()`, `CheckCompletionAfterFrames()`.
  * Default `Type` is `TestType.Automated`.
* **`NightTest.Core.BaseManualTestCase`**:
  * Inherits from `BaseGameTestCase`.
  * Overrides `Type` to `TestType.Manual`.
  * Provides UI for manual pass/fail confirmation (buttons, ESC key).
  * Manages timeout for manual tests.
  * Method: `RequestManualConfirmation(string consolePrompt)`.
  * Handles drawing of UI and `Night.Graphics.Present()`.
* **`NightTest.Core.TestGroup`**:
  * Base class for xUnit test classes.
  * Takes `ITestOutputHelper` for logging.
  * Provides `Run_GameTestCase(BaseGameTestCase testCase)` method which:
    * Logs test information.
    * Runs the `IGame` instance using `Night.Framework.Run(testCase)`.
    * Asserts that `testCase.CurrentStatus == TestStatus.Passed`.
* **`NightTest.Core.TestType`**: Enum (`Automated`, `Manual`).
* **`NightTest.Core.TestStatus`**: Enum (`NotRun`, `Passed`, `Failed`, `Skipped`).

## 7. Best Practices

* **Clear Descriptions:** Ensure `ITestCase.Description` clearly states what the test is verifying. `Details` property should provide context on pass/fail.
* **Focused Tests:** Each test case should verify a single, specific piece of functionality or behavior.
* **Idempotency (for automated tests):** Automated tests should produce the same result every time they are run, assuming no code changes. Avoid dependencies on external state that isn't controlled by the test's `Load()` method.
* **Resource Management:** Clean up any resources created during a test (e.g., temporary files) in a `finally` block or at the end of the test.
* **Readability:** Write clean, understandable test code. Comments should explain *why* something is being done if it's not obvious.
* **Use `[Collection("SequentialTests")]`:** For `TestGroup` classes, especially those involving UI or window manipulation, to ensure tests run one after another and don't interfere.
* **Test Both Success and Failure Cases:** For a given feature, consider creating tests for expected successful outcomes and expected failure conditions (e.g., invalid input, file not found).
* **Keep Manual Tests for Visuals/Interaction:** Reserve manual tests for scenarios that genuinely require human observation (e.g., "is this color correct?", "does this animation look right?") or complex interactions not easily automated.
