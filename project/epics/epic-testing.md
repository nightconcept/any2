# Epic: NightTest Framework Enhancements and CI Integration

**Goal:** To enhance the `NightTest` framework for better modularity, usability, and maintainability, and to integrate its execution into the GitHub Actions CI pipeline for automated validation of the `Night` framework.

## User Stories

- As a Developer, I want `NightTest` to dynamically discover test groups so that adding new tests is more modular and requires no orchestrator modification.
- As a Developer, I want a clear and standardized way to interact with manual tests so that they are easier to execute and less prone to specific keybinding issues.
- As a Developer, I want individual test cases or groups to be able to manage their own configurations if needed, for greater flexibility.
- As a Developer, I want test cases to report failures robustly, even in case of crashes, so that `test_report.json` is always informative.
- As a Developer, I want a well-defined `BaseTestCase` to reduce boilerplate and provide common utilities for writing tests.
- As a Developer, I want the `NightTest` project structure to be clean and consistently named (e.g., `Groups/` instead of `Modules/`).
- As a CI System, I want to build and run `NightTest` automated tests and fail the build if any tests fail.
- As a Developer, I want the CI system to archive the `test_report.json` for inspection.
- As a Developer, I want `NightTest.exe` to return an appropriate exit code based on test results for CI integration.

## Tasks

### Phase 1: NightTest Architectural Enhancements

- **Task 1.1: Implement Dynamic Test Discovery**
  - **Description:** Modify `tests/Program.cs` to scan assemblies in `tests/Groups/` (currently `tests/Modules/`) for classes implementing `NightTest.Core.ITestGroup` instead of relying on manual instantiation and registration. This aligns with the PRD suggestion ([`project/night-test-prd.md:33`](project/night-test-prd.md:33)).
  - **Implementation:**
    - [x] Update `tests/Program.cs` to use reflection to find all types implementing `NightTest.Core.ITestGroup` in the relevant assembly/assemblies.
    - [x] Instantiate discovered groups and collect their test cases.
  - **Acceptance Criteria:** `tests/Program.cs` dynamically discovers all test groups. New test groups added to the `tests/Groups/` directory are automatically included in test runs without manual changes to `tests/Program.cs`.
  - **Status:** Done

- **Task 1.2: Standardize Manual Test Interaction**
  - **Description:** Develop a standardized mechanism for manual test pass/fail input, as per the future consideration in [`project/night-test-prd.md:31`](project/night-test-prd.md:31) and [`project/night-test-prd.md:249`](project/night-test-prd.md:249). This moves away from hardcoded keys specific to each manual test.
  - **Implementation:**
    - [ ] Design a simple UI overlay (e.g., using `Night.Graphics` to draw "Pass"/"Fail" text/buttons and listen for mouse clicks on them) or a dedicated input phase managed by `BaseTestCase` or the `TestRunner`.
    - [ ] Update `BaseTestCase` (Task 1.3) to include logic for this standardized interaction.
    - [ ] Refactor existing manual tests (e.g., `ConcreteDummyManualTest` if it exists) to use the new mechanism.
  - **Acceptance Criteria:** Manual tests use a consistent and clear method for user input to signal pass or fail.
  - **Status:** To Do

- **Task 1.3: Implement and Utilize `BaseTestCase`**
  - **Description:** Create and flesh out the `tests/Core/BaseTestCase.cs` as per PRD ([`project/night-test-prd.md:165`](project/night-test-prd.md:165), [`project/night-test-prd.md:197`](project/night-test-prd.md:197)) and `project/testing-plan.md`.
  - **Implementation:**
    - [ ] Create `tests/Core/BaseTestCase.cs` inheriting from `Night.IGame` and implementing `NightTest.Core.ITestCase`.
    - [ ] Provide default (virtual) implementations for all `Night.IGame` methods.
    - [ ] Implement common `ITestCase` properties (e.g., `Name`, `Type`, `Description` can be abstract or virtual with default).
    - [ ] Include a `System.Diagnostics.Stopwatch` (`TestStopwatch`) for measuring total test duration, started in `Load()` and stopped before reporting.
    - [ ] Implement `QuitSelf()` method that signals the test is done, records the duration, and calls `Night.Window.Close()`.
    - [ ] Provide common utility methods for reporting results to the `TestRunner` (e.g., `RecordPass(string details)`, `RecordFail(string details, Exception e = null)`). These methods would set `CurrentStatus` and `Details` properties.
    - [ ] Ensure it correctly receives and stores the `TestRunner` instance via `SetTestRunner()`.
    - [ ] Refactor existing test cases to inherit from `BaseTestCase`.
  - **Acceptance Criteria:** `BaseTestCase.cs` is implemented and provides useful common functionality. Existing and new test cases inherit from it, reducing boilerplate.
  - **Status:** To Do

- **Task 1.4: Refactor Directory Structure: `Modules` to `Groups`**
  - **Description:** Rename the `tests/Modules/` directory to `tests/Groups/` to align with `ITestGroup` terminology, as suggested in [`project/night-test-prd.md:33`](project/night-test-prd.md:33) and [`project/night-test-prd.md:166`](project/night-test-prd.md:166). Update all relevant namespaces and references.
  - **Implementation:**
    - [x] Rename directory `tests/Modules` to `tests/Groups`.
    - [x] Update namespaces in all test group and test case files previously within `tests/Groups/`.
    - [ ] Update `tests/Program.cs` (especially if dynamic discovery from Task 1.1 is implemented) to look in `tests/Groups/`.
    - [ ] Update documentation (`project/night-test-prd.md`, `project/testing-plan.md`) to consistently refer to `tests/Groups/`.
  - **Acceptance Criteria:** Directory is renamed. All code, documentation, and discovery mechanisms reflect the new `Groups` naming.
  - **Status:** To Do

- **Task 1.5: Clarify and Refactor `tests/Game.cs`**
  - **Description:** Determine the precise role of the existing [`tests/Game.cs`](tests/Game.cs:1). If its functionality is better suited for `BaseTestCase` or it's a redundant example, refactor or remove it.
  - **Implementation:**
    - [ ] Analyze the functionality provided by [`tests/Game.cs`](tests/Game.cs:1).
    - [ ] If it provides generic game loop or input handling logic useful for all tests, merge this functionality into `BaseTestCase` (Task 1.3).
    - [ ] If it's intended as a very simple, standalone example of an `IGame` for testing the host, ensure it's minimal and perhaps rename it (e.g., `ExampleHostedTest.cs`) and include it in a relevant `ITestGroup` if it's meant to be run as a test.
    - [ ] If its functionality becomes entirely redundant after `BaseTestCase` implementation and other refactorings, remove [`tests/Game.cs`](tests/Game.cs:1) and update any references.
  - **Acceptance Criteria:** The role of [`tests/Game.cs`](tests/Game.cs:1) is clarified. The codebase is cleaner, and any useful generic logic is consolidated in `BaseTestCase`.
  - **Status:** To Do

- **Task 1.6: Enhance Error Handling within Test Cases and `BaseTestCase`**
  - **Description:** Ensure that test cases, particularly through `BaseTestCase`, robustly report failures to the `TestRunner`, especially in the event of unexpected exceptions during test logic or from `Night.Framework` calls.
  - **Implementation:**
    - [ ] `BaseTestCase` should implement a top-level try-catch mechanism around the execution of `Load`, `Update`, and `Draw` (or a central test execution method if applicable).
    - [ ] Any unhandled exception caught by `BaseTestCase` should result in the test being marked as `TestStatus.Failed`, with exception details included in the `Details` property reported to the `TestRunner`.
  - **Acceptance Criteria:** The `test_report.json` accurately reflects test failures caused by unhandled exceptions within the test case execution, providing error messages and stack traces where possible.
  - **Status:** To Do

- **Task 1.7: Test-Specific Configuration (Optional Enhancement)**
  - **Description:** Investigate and optionally implement a mechanism allowing individual test cases or test groups to load specific configuration files (e.g., a `test_config.json` alongside the test case file).
  - **Implementation:**
    - [ ] Design an approach (e.g., `ITestCase` property for a config file path, convention-based loading like `TestName.config.json`).
    - [ ] If implemented, add helper methods in `BaseTestCase` to load and parse such configuration files.
  - **Acceptance Criteria:** (If implemented) Test cases can load their own configurations, allowing for more varied and isolated test scenarios.
  - **Status:** To Do

- **Task 1.8: Asset Management for Tests (Optional Enhancement)**
  - **Description:** Improve the organization of assets used by test cases. Consider allowing per-group or per-test asset subdirectories under `tests/assets/`.
  - **Implementation:**
    - [ ] Define a clear convention for test asset subdirectories (e.g., `tests/assets/GroupName/AssetName`).
    - [ ] Provide a helper method in `BaseTestCase` to easily resolve paths to these specific assets.
  - **Acceptance Criteria:** (If implemented) Test assets are organized more logically, making it easier to manage assets for specific tests or test groups.
  - **Status:** To Do

### Phase 2: CI Integration for NightTest

- **Task 2.1: Modify `NightTest/Program.cs` for CI-Friendly Exit Codes**
  - **Description:** Update the `Main` method in `tests/Program.cs` to return an appropriate exit code: 0 if all selected automated tests pass, and a non-zero value (e.g., 1) if any selected automated tests fail or if critical errors occur in the test orchestrator.
  - **Implementation:**
    - [ ] After `testRunner.GenerateReport()` in `tests/Program.cs`, query the `TestRunner` instance for the count of failed automated tests and any critical orchestrator errors.
    - [ ] Set `Environment.ExitCode` to 0 or 1 based on this information.
  - **Acceptance Criteria:** `NightTest.exe` (or equivalent executable) returns 0 if all automated tests pass, and 1 (or another non-zero code) if there are any automated test failures or critical errors.
  - **Status:** To Do

- **Task 2.2: Update `.github/workflows/ci.yml` to Run NightTest**
  - **Description:** Modify the existing GitHub Actions CI workflow ([`.github/workflows/ci.yml`](.github/workflows/ci.yml:1)) to build and execute the `NightTest` application.
  - **Implementation:**
    - [ ] Add a new step after the main solution build ("Build Solution") to specifically build the `NightTest` project: `dotnet build tests/NightTest.csproj --configuration Release --no-restore`.
    - [ ] Modify the "Run Tests" step:
      - Remove the current `dotnet test tests/Night.Tests/Night.Tests.csproj` command.
      - Add commands to execute the compiled `NightTest` application. This path will vary by OS (e.g., `tests/bin/Release/net9.0/NightTest.exe` on Windows, `tests/bin/Release/net9.0/NightTest` on Linux/macOS).
      - Pass the command-line arguments `--run-automated --report-path test_report.json` to the `NightTest` executable.
  - **Acceptance Criteria:** The CI workflow successfully builds the `NightTest` project. The "Run Tests" step executes `NightTest.exe --run-automated`. The workflow step correctly fails if `NightTest.exe` returns a non-zero exit code (due to Task 2.1).
  - **Status:** To Do

- **Task 2.3: Archive `test_report.json` in CI Workflow**
  - **Description:** Add a step to the [`.github/workflows/ci.yml`](.github/workflows/ci.yml:1) workflow to upload the `test_report.json` file generated by `NightTest` as a build artifact.
  - **Implementation:**
    - [ ] Add a new step using `actions/upload-artifact@v3` (or a later version).
    - [ ] Configure this step to upload the `test_report.json` file.
    - [ ] Set the `if: always()` condition for this step to ensure the report is uploaded even if previous steps (like test execution) fail.
    - [ ] Assign a clear name to the artifact, incorporating the matrix OS if applicable (e.g., `night-test-report-${{ matrix.os }}`).
  - **Acceptance Criteria:** The `test_report.json` file is available as a downloadable artifact for each CI run, allowing for inspection of test results.
  - **Status:** To Do
