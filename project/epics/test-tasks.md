# Epic: TestGame Implementation and Initial Tests

**Goal:** Establish the `Night.TestGame` project, integrate it into the solution, and implement initial tests, focusing on the `Night.Configuration` module. This will provide a dedicated environment for interactive testing of engine features.

**User Stories:**

- As a developer, I want a dedicated `Night.TestGame` project set up and integrated into the main solution so I can easily run and debug engine tests.
- As a developer, I want the existing `Game.cs` content in `Night.TestGame` to be cleared to provide a clean slate for new test scenarios.
- As a developer, I want to implement specific test scenarios within `Night.TestGame` to verify the functionality of the `Night.Configuration` module, ensuring settings are loaded and applied correctly.
- As a developer, I want `Night.TestGame` to have a `TestReporter` object that can track and output the status of exercised functionalities, as defined in `project/night-test.md`.

## Tasks

### Phase 1: TestGame Project Setup

- [ ] **Task TG.1: Create and Integrate `Night.TestGame` Project**
  - **Description:** Create a new C# project named `Night.TestGame` under the `tests/` directory. This project should be a console application that references the `Night` library. Add this project to the `Night.sln` solution file.
  - **Implementation:**
    - [ ] Create the directory `tests/Night.TestGame/`.
    - [ ] Initialize a new C# console project (`Night.TestGame.csproj`) in this directory.
    - [ ] Ensure `Night.TestGame.csproj` references the `Night` project (e.g., `<ProjectReference Include="..\..\src\Night\Night.csproj" />`).
    - [ ] Add `Night.TestGame` to `Night.sln`.
    - [ ] Create a basic `Program.cs` that initializes and runs a new `Game` instance using `Night.Framework.Run()`.
    - [ ] Create a basic `Game.cs` implementing `Night.IGame` with empty `Load`, `Update`, `Draw`, `KeyPressed`, etc. methods.
  - **Acceptance Criteria:** `Night.TestGame` project is created, added to the solution, and can be built and run, showing a blank window.
  - **Status:** To Do

- [ ] **Task TG.2: Clear `Game.cs` in `Night.TestGame`**
  - **Description:** Remove all existing game-specific logic from `tests/Night.TestGame/Game.cs` (if it was copied from `Night.SampleGame` or similar). The file should contain a minimal `IGame` implementation.
  - **Implementation:**
    - [ ] Edit `tests/Night.TestGame/Game.cs`.
    - [ ] Remove all fields, properties, and non-essential logic from the `Game` class.
    - [ ] Ensure `Load`, `Update`, `Draw`, `KeyPressed`, `KeyReleased`, `MousePressed`, `MouseReleased` methods are present but have empty bodies or minimal logging.
  - **Acceptance Criteria:** `tests/Night.TestGame/Game.cs` contains a shell `IGame` implementation ready for test-specific code.
  - **Status:** To Do

### Phase 2: Configuration Module Tests

- [ ] **Task TG.3: Implement `TestReporter` Object**
  - **Description:** Create the `TestReporter` class as specified in `project/night-test.md`. This class will be used to track and report the status of tests.
  - **Implementation:**
    - [ ] Create `tests/Night.TestGame/TestReporter.cs`.
    - [ ] Implement the `TestReporter` class with methods to:
      - Register a test (functionality name).
      - Mark a test as "Passed", "Failed", or "Not Run".
      - Generate a text-based summary report.
    - [ ] Instantiate `TestReporter` in `Game.cs`.
  - **Acceptance Criteria:** `TestReporter.cs` is implemented and can be used by test scenarios. The report can be printed to the console at the end of the `Draw` method or on a specific key press.
  - **Status:** To Do

- [ ] **Task TG.4: Implement `Night.Configuration` Tests in `Night.TestGame`**
  - **Description:** Create test scenarios in `Night.TestGame` to specifically validate the `Night.Configuration` module. This includes testing the loading of default values and the overriding of these values via a `config.json` file.
  - **Implementation:**
    - [ ] Create a `tests/Night.TestGame/ConfigurationTests.cs` (or a similar structure, perhaps a "scene" within `Game.cs`).
    - [ ] **Test Case 4.1: Default Configuration Values**
      - Ensure `Night.Framework.Run()` is called without a `config.json` present (or with an empty one).
      - In `Game.Load()` or `Game.Update()`:
        - Access `ConfigurationManager.CurrentConfig`.
        - Verify that window title, dimensions, VSync, etc., match the default values defined in `GameConfig.cs` and its nested config classes.
        - Use the `TestReporter` to log the pass/fail status of each check (e.g., "Default Window Title: Passed").
    - [ ] **Test Case 4.2: `config.json` Override**
      - Create a `tests/Night.TestGame/assets/config.json` (or similar path, ensure it's copied to output).
      - Populate this `config.json` with specific, non-default values for:
        - `WindowConfig.Title`
        - `WindowConfig.Width`, `WindowConfig.Height`
        - `WindowConfig.VSync`
        - `WindowConfig.Fullscreen` and `WindowConfig.FullscreenType`
        - `WindowConfig.X`, `WindowConfig.Y` (initial position)
        - (Add other relevant config options as they become testable)
      - Run `Night.TestGame`.
      - In `Game.Load()` or `Game.Update()`:
        - Access `ConfigurationManager.CurrentConfig`.
        - Verify that the window properties (title, dimensions, VSync status, fullscreen state, position) match the values from `config.json`.
        - Check `Window.GetTitle()`, `Window.GetMode()`, etc. to confirm settings were applied.
        - Use the `TestReporter` to log the pass/fail status of each check (e.g., "Custom Window Title from config.json: Passed").
    - [ ] Display the `TestReporter` output on screen or on a key press.
  - **Acceptance Criteria:** `Night.TestGame` successfully tests default configuration loading and `config.json` overrides for key settings. The `TestReporter` correctly logs the outcomes.
  - **Status:** To Do

### Phase 3: Future Test Expansion

- [ ] **Task TG.X: (Placeholder) Add Tests for Other Modules**
  - **Description:** As new engine features are developed or existing ones are expanded, add corresponding test scenarios to `Night.TestGame`.
  - **Status:** Ongoing
