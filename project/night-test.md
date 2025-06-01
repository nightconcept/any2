# Night TestGame - Product Requirements Document

## 1. Introduction

- **Project Idea:** `Night.TestGame` is a dedicated C# application for testing the functionalities of the `Night.Framework` and the future `Night.Engine`. It will serve as an interactive testbed where new engine features can be continuously integrated and visually validated.
- **Problem/Need:** While unit tests are valuable, many game engine features, especially those related to UI, rendering, and input, require actual execution within a game loop to be effectively tested. `Night.TestGame` addresses this by providing a runnable environment that can be easily extended to exercise new and existing engine capabilities.
- **Development Goal:** To create a maintainable and extensible test game that facilitates the iterative development and testing of the Night engine. This includes implementing a reporting mechanism to track exercised functions and their status.

## 2. Core Features

### 2.1. TestGame Foundation

- **Description:** `Night.TestGame` will be a new C# project, similar in structure to `Night.SampleGame`. It will initialize the `Night.Framework` and provide a basic game loop.
- **Status:** To be implemented.

### 2.2. Test Scenario Management

- **Description:** The game will allow for different "test scenarios" or "scenes" to be loaded, each designed to test specific engine features. For example, a scene for testing graphics primitives, another for input handling, another for UI elements, etc.
- **Status:** To be implemented.

### 2.3. Test Reporting Object

- **Description:** A dedicated reporting object will be implemented to track which engine functions or features are exercised during the TestGame's execution.
    - It will maintain a list of "tests" or "verified functionalities."
    - Each test will have a status (e.g., "Passed", "Failed", "Not Run").
    - The TestGame will include logic to call methods on this reporting object when specific functionalities are successfully used or validated.
- **Output:** The reporting object will be capable of generating a simple text-based output (e.g., to a console window or a log file) summarizing the status of all tracked tests.
    - Example output:
        ```
        Night Engine Test Report:
        -------------------------
        Night.Graphics.NewImage: Passed
        Night.Graphics.Draw (Sprite): Passed
        Night.Window.SetMode: Passed
        Night.Keyboard.IsDown: Passed
        Night.Mouse.GetPosition: Not Run
        -------------------------
        Summary: 3 Passed, 0 Failed, 1 Not Run
        ```
- **Status:** To be implemented.

### 2.4. Continuous Integration of Tests

- **Description:** As new features are added to `Night.Framework` or `Night.Engine`, corresponding test scenarios and reporting calls will be added to `Night.TestGame`.
- **Status:** Ongoing.

## 3. Technical Specifications

- **Primary Language(s):** C# 13 (using .NET 9), consistent with the main Night Engine project.
- **Key Frameworks/Libraries:**
    - `Night.Framework` (and eventually `Night.Engine`)
    - SDL3 (via `Night.Framework`)
- **Project Structure:**
    - A new project directory: `/src/Night.TestGame/`
    - `Night.TestGame.csproj`
    - `Program.cs` (or `Game.cs`) for the main game logic and `IGame` implementation.
    - `TestReporter.cs` for the reporting object.
    - Directories for different test scenarios/modules (e.g., `/src/Night.TestGame/GraphicsTests/`, `/src/Night.TestGame/InputTests/`).
- **Output:** Test reports will initially be text-based, output to the console or a log file.

## 4. Project Structure Considerations (within `Night.TestGame`)

```mermaid
graph TD
    A(Night.TestGame) --> B(Night.TestGame.csproj);
    A --> C(Program.cs); // or Game.cs
    A --> D(TestReporter.cs);
    A --> E(Scenes/); // Or Modules/Tests for organizing different test areas
    E --> E1(GraphicsTestScene.cs);
    E --> E2(InputTestScene.cs);
    E --> E3(TimerTestScene.cs);
    A --> F(assets/); // For any specific assets needed for testing
```

## 5. Non-Goals

- This TestGame is not intended to be a fully-featured game. Its primary purpose is testing engine functionality.
- It is not a replacement for unit tests but a complement to them for integration and visual testing.
- Initially, it will not feature a complex UI for test selection; tests might be run sequentially or based on simple input commands.

## 6. Future Considerations

- A simple in-game UI to select and run specific tests.
- More sophisticated reporting formats (e.g., HTML, XML).
- Integration with an automated testing framework, if feasible.
