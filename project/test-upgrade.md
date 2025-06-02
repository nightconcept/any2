# Epic: Upgrading NightTest to Microsoft Testing Platform (MTP)

**Goal:** Refactor the existing standalone C# executable testing framework, `NightTest`, to fully integrate with the Microsoft Testing Platform (MTP). This will enable test discovery, execution, and reporting through standard .NET CLI commands (e.g., `dotnet test`) and provide first-class support within IDEs like Visual Studio Test Explorer.

**User Stories:**

- As a Developer, I want `NightTest` tests to be discoverable and executable via `dotnet test` so that I can integrate testing into standard .NET workflows.
- As a Developer, I want `NightTest` results to be visible in Visual Studio Test Explorer for a better debugging and development experience.
- As a Developer, I want `NightTest` to produce standard test report formats (e.g., TRX) for easier integration with CI/CD pipelines.
- As a System, `NightTest` needs to adapt its entry point and core logic to be driven by MTP's lifecycle.
- As a System, `NightTest` needs to expose its test cases (`ITestCase` instances) as `TestNode` objects to MTP.
- As a System, `NightTest` needs to report test execution start, progress, and outcomes (pass/fail/skip, duration, errors) to MTP using MTP's reporting APIs.

## Tasks

### Phase 1: Project Setup & MTP Boilerplate

- **Task 1.1: Modify `tests/NightTest.csproj`**
  - **Description:** Update the project file to include MTP dependencies and ensure compatibility.
  - **Implementation:**
    - [ ] Add MTP NuGet Packages:
      - [ ] `Microsoft.Testing.Platform`
      - [ ] `Microsoft.Testing.Framework`
      - [ ] `Microsoft.Testing.Extensions.TrxReport`
      - [ ] `Microsoft.Testing.Extensions.Console` (if not implicitly included)
    - [ ] Verify TargetFramework remains `net9.0`.
    - [ ] Verify OutputType remains `Exe`.
  - **Acceptance Criteria:** The `tests/NightTest.csproj` file is updated with the required MTP dependencies. The project still compiles.
  - **Status:** To Do

- **Task 1.2: Refactor `tests/Program.cs` Main Method**
  - **Description:** Adapt the application's entry point to initialize and run via the MTP.
  - **Implementation:**
    - [ ] Replace existing orchestration logic in `Main` with MTP initialization.
    - [ ] Use `TestApplication.CreateBuilder(args)`.
    - [ ] (Placeholder for Phase 2) Add registration for the custom test framework adapter (e.g., `builder.AddTestFramework(...)`).
    - [ ] Build the MTP application: `var app = await builder.BuildAsync();`.
    - [ ] Run the MTP application: `return await app.RunAsync();`.
    - [ ] Remove old command-line parsing for `--run-automated` and `--report-path` as MTP will handle these.
  - **Acceptance Criteria:** `Program.cs` is updated to use the MTP entry point. The application can be launched (though no tests will run yet).
  - **Status:** To Do

### Phase 2: Implementing MTP Test Discovery

- **Task 2.1: Create Test Framework Adapter Core Components**
  - **Description:** Develop the necessary classes to bridge `NightTest`'s test structure with MTP's discovery mechanism.
  - **Implementation:**
    - [ ] Create `NightTestMtpAdapter.cs` (e.g., in `NightTest.Core` or `NightTest.MtpAdapter` namespace).
    - [ ] This adapter (or related classes) will implement MTP interfaces (e.g., `ITestFramework`, `ITestFrameworkCapabilities`, or `ITestDataSource`).
    - [ ] Create `NightTestMtpAdapterFactory.cs` for MTP to instantiate the adapter.
  - **Acceptance Criteria:** Core adapter classes and factory are created and compile.
  - **Status:** To Do

- **Task 2.2: Implement Test Discovery Logic in Adapter**
  - **Description:** Enable the MTP adapter to find all existing `NightTest` tests and represent them as `TestNode` objects.
  - **Implementation:**
    - [ ] Replicate/adapt `ITestGroup` discovery from current `Program.cs` (instantiating `DummyGroup`, `TimerGroup`, etc., or implement assembly scanning).
    - [ ] For each `ITestGroup`, call `GetTestCases()`.
    - [ ] For each `ITestCase` found:
      - [ ] Construct a `TestNode`.
      - [ ] Populate `TestNode.Uid` (e.g., `FullGroupName.TestCaseName`).
      - [ ] Populate `TestNode.DisplayName` (from `ITestCase.Name`).
      - [ ] Populate `TestNode.SourceInformation` (class name initially, explore file/line later if feasible).
      - [ ] Populate `TestNode.Properties` with `ITestCase.Type` (Automated/Manual) and `ITestCase.Description`.
    - [ ] Provide these `TestNode`s to MTP through the adapter's interface methods.
  - **Acceptance Criteria:** The adapter can discover all `ITestCase` instances and correctly convert them to `TestNode` objects with appropriate metadata.
  - **Status:** To Do

- **Task 2.3: Register the Adapter in `Program.cs`**
  - **Description:** Connect the created test framework adapter to the MTP application builder.
  - **Implementation:**
    - [ ] In the updated `Program.cs Main` method, call `builder.AddTestFramework(new NightTestMtpAdapterFactory(), args);` (or similar registration method).
  - **Acceptance Criteria:** The `NightTestMtpAdapterFactory` is registered with the `TestApplicationBuilder`. Tests can be discovered by `dotnet test --list-tests`.
  - **Status:** To Do

### Phase 3: Implementing MTP Test Execution & Reporting

- **Task 3.1: Adapt Test Execution within MTP Adapter**
  - **Description:** Modify the test execution flow to be invoked by MTP and use MTP's reporting mechanisms.
  - **Implementation:**
    - [ ] The `NightTestMtpAdapter` (or a component like an `ITestInvoker`) will handle MTP's request to execute a specific `TestNode`.
    - [ ] From the `TestNode`, identify the corresponding `NightTest.Core.ITestCase` instance.
    - [ ] Modify `ITestCase.SetTestRunner` to accept an MTP-compatible reporting interface/object (e.g., `ITestExecutionLifecycleCallbacks` or a custom wrapper) instead of the old `TestRunner`.
    - [ ] Update `BaseTestCase` to use this new MTP reporter.
    - [ ] Test Execution Flow for each test:
      - [ ] Call MTP callback for test start (e.g., `callbacks.TestNodeStartAsync(testNode)`).
      - [ ] Execute the test using `Night.Framework.Run((Night.IGame)testCaseInstance);`.
      - [ ] Adapt `BaseTestCase.QuitSelf()`:
        - [ ] Instead of `Runner.RecordResult()`, make outcome (status, duration, details) available to the MTP adapter or have `BaseTestCase` use the MTP reporter directly.
      - [ ] Call MTP callback for test end (e.g., `callbacks.TestNodeEndAsync(testNode, outcome)`), providing the `TestNode`, `Outcome` (Pass/Fail/Skip), `Duration`, error messages/stack traces, and console output.
  - **Acceptance Criteria:** The MTP adapter can execute specific `ITestCase` instances when requested by MTP. Test start and end are reported to MTP.
  - **Status:** To Do

- **Task 3.2: Deprecate `NightTest.Core.TestRunner`**
  - **Description:** Remove the old `TestRunner` as MTP and its loggers will now handle result aggregation and reporting.
  - **Implementation:**
    - [ ] Remove or comment out `NightTest.Core.TestRunner.cs`.
    - [ ] Remove usages of `TestRunner` from `Program.cs` and `BaseTestCase.cs`.
    - [ ] The generation of `test_report.json` and console summaries will now be handled by MTP loggers (e.g., TRX logger).
  - **Acceptance Criteria:** `TestRunner.cs` is no longer used. The project relies on MTP for test reporting.
  - **Status:** To Do

### Phase 4: Configuration and Command-Line Argument Handling

- **Task 4.1: Leverage MTP Test Filtering**
  - **Description:** Ensure `NightTest` respects MTP's built-in filtering mechanisms.
  - **Implementation:**
    - [ ] Confirm `TestNode.Properties` (e.g., `Type=Automated`) are correctly exposed during discovery (Task 2.2).
    - [ ] Users will use standard MTP filter syntax (e.g., `dotnet test --filter "Type=Automated"`), replacing the old `--run-automated` flag.
  - **Acceptance Criteria:** Tests can be filtered using `dotnet test --filter` based on properties like `Type`.
  - **Status:** To Do

- **Task 4.2: Handle Custom Command-Line Arguments (If Any Remain)**
  - **Description:** Adapt or remove any remaining custom command-line arguments.
  - **Implementation:**
    - [ ] The `--report-path` argument is superseded by MTP logger configuration (e.g., `dotnet test --logger "trx;LogFileName=MyReport.trx"`).
    - [ ] If other custom arguments are essential for `NightTest`'s own configuration (not test execution):
      - [ ] Consider implementing `ICommandLineOptionsProvider` in an MTP extension.
      - [ ] Register these options with MTP.
      - [ ] Access their values within the `NightTestMtpAdapter` or other components.
  - **Acceptance Criteria:** All necessary configuration is handled either through MTP mechanisms or a clearly defined MTP extension for custom arguments.
  - **Status:** To Do

### Phase 5: Graceful Shutdown, Error Management, and Testing

- **Task 5.1: Ensure Graceful Shutdown**
  - **Description:** Verify that the MTP application and any custom components shut down cleanly.
  - **Implementation:**
    - [ ] Rely on MTP's `app.RunAsync()` for overall lifecycle management.
    - [ ] If `NightTestMtpAdapter` or related components allocate unmanaged resources, implement `IAsyncDisposable` or `IDisposable` for proper cleanup.
  - **Acceptance Criteria:** The test application exits cleanly after test execution.
  - **Status:** To Do

- **Task 5.2: Implement Robust Error Management**
  - **Description:** Ensure framework-level errors are correctly reported through MTP.
  - **Implementation:**
    - [ ] Exceptions from the adapter during discovery or execution should be caught and reported by MTP.
    - [ ] Review existing error handling in `Program.cs` (group loading, `Framework.Run` exceptions) and integrate or adapt it for MTP reporting.
  - **Acceptance Criteria:** Critical errors within the `NightTest` framework (not just test failures) are visible when running `dotnet test`.
  - **Status:** To Do

- **Task 5.3: Thorough End-to-End Testing**
  - **Description:** Verify the complete MTP integration.
  - **Implementation:**
    - [ ] Test execution via `dotnet test tests/NightTest.csproj`.
    - [ ] Verify tests appear and report status correctly in Visual Studio Test Explorer.
    - [ ] Ensure TRX reports are generated correctly (e.g., `dotnet test --logger trx`).
    - [ ] Test MTP filtering with `Type=Automated` and `Type=Manual`.
    - [ ] Verify correct reporting for passed, failed, and (if applicable) skipped tests, including duration and error details.
  - **Acceptance Criteria:** `NightTest` is fully integrated with MTP, meeting all expected outcomes from the initial objective.
  - **Status:** To Do
