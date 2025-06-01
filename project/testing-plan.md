# Night.Engine Testing Plan

## 1. Introduction and Philosophy

This document outlines the testing strategy for the `Night.Engine` project, specifically focusing on unit testing its constituent modules. The primary goal is to ensure the reliability and correctness of individual components within the engine.

The testing philosophy draws inspiration from Love2D's module-based testing approach, aiming to test each logical module of `Night.Engine` as independently as possible. We will use xUnit as the primary testing framework for C#.

This plan adheres to the standards and guidelines set forth in the `guidelines.md` and aligns with the project goals detailed in `PRD.md`.

## 2. Testing Framework

- **Framework:** xUnit.net
- **Assertion Library:** xUnit's built-in assertions.

## 3. Test Project Structure

A dedicated test project will be created for `Night.Engine` tests:

- **Project Name:** `Night.Tests`
- **Location:** `tests/Night.Tests/` (This is a recommendation; final location to be decided based on solution structure).
- **Dependencies:** This project will reference the `Night` project.

## 4. Naming Conventions

Consistency in naming is crucial for maintainability and readability of tests.

- **Test Classes:**
  - Named after the class or module being tested, suffixed with `Tests`.
  - Example: `GraphicsTests.cs` for testing `Night.Graphics`, `WindowTests.cs` for `Night.Window`.
- **Test Methods:**
  - Follow the pattern: `[MethodUnderTest]_[ScenarioOrCondition]_[ExpectedBehavior]`
  - `[MethodUnderTest]`: The name of the method being tested.
  - `[ScenarioOrCondition]`: A brief description of the specific test case or input conditions.
  - `[ExpectedBehavior]`: The expected outcome or state.
  - Example: `SetMode_ValidResolution_ReturnsTrue`, `IsOpen_WhenWindowIsActive_ReturnsTrue`, `Draw_NullSprite_ThrowsArgumentNullException`.

## 5. Scope of Testing - Night.Engine Modules

The following modules and components within `Night.Engine` are the primary targets for unit testing. Given that `Night.Engine` (specifically `Night.Framework`) largely consists of static classes providing a Love2D-like API over SDL3, tests will focus on the C# logic, parameter validation, and correct invocation patterns where feasible.

### 5.1. `Night.Framework` Modules

These modules are typically static classes in the `Night` namespace.

- **`Night.Window` (`Window.cs`)**
  - Test methods like `SetMode`, `SetTitle`, `IsOpen`, `Close`.
  - Focus on parameter validation (e.g., null title, invalid dimensions for `SetMode` if applicable before SDL call).
  - Testing the actual window manipulation might be difficult in pure unit tests and leans towards integration testing (covered by `Night.SampleGame`). We can, however, test the C# logic paths before SDL calls.
- **`Night.Graphics` (`Graphics.cs`)**
  - Test methods like `NewImage`, `Draw`, `Clear`, `Present`.
  - Parameter validation (e.g., null paths for `NewImage`, null sprites for `Draw`).
  - Testing actual rendering output is an integration concern. Unit tests should focus on the C# logic (e.g., does `Draw` handle transform parameters correctly before passing to SDL?).
- **`Night.Keyboard` (`Keyboard.cs`)**
  - Test methods like `IsDown`.
  - Focus on validating input parameters (e.g., specific `KeyCode` values).
  - Directly testing key states requires OS-level interaction, which is beyond typical unit test scope. Tests might focus on internal logic if any exists separate from direct SDL calls.
- **`Night.Mouse` (`Mouse.cs`)**
  - Test methods like `IsDown`, `GetPosition`.
  - Similar to Keyboard, parameter validation for `MouseButton`.
  - Testing actual mouse states/positions is an integration concern.
- **`Night.SDL` (`SDL.cs`)**
  - This module directly wraps SDL3 P/Invoke calls.
  - Unit testing P/Invoke wrappers is complex and often provides limited value compared to the effort.
  - Focus should be on any C# helper methods within this class that do not directly call SDL or that perform significant logic before/after an SDL call.
  - Most testing for `SDL.cs` functionality will be indirect, through the higher-level framework modules and `Night.SampleGame`.
- **`FrameworkLoop.cs`**
  - Testing the main game loop (`Run` method) in isolation is challenging.
  - Focus on unit testing helper methods or individual components within the loop's logic if they can be isolated (e.g., delta time calculation if it's a separate utility).
  - The overall loop functionality is best tested via `Night.SampleGame`.
- **`Types.cs`**
  - Contains data structures (e.g., `Color`, `Rectangle`, `Sprite`) and interfaces (`IGame`).
  - Test constructors and any methods on these types if they contain logic (e.g., `Color.ToSDLColor`, methods on `Rectangle`).
  - Interfaces themselves are not tested directly but are implemented by mocks or test classes.

### 5.2. `Night.Engine` (Future High-Level Systems)

As `Night.Engine` evolves with higher-level systems (ECS, Scene Management, etc.), dedicated test classes will be created for each new component, following the same principles.

## 6. Test Case Design

- **Positive Tests:** Verify that methods work correctly with valid inputs.
- **Negative Tests (Error Handling):**
  - Verify behavior with invalid inputs (e.g., null arguments, out-of-range values).
  - Ensure appropriate exceptions are thrown as per API contracts (e.g., `ArgumentNullException`, `ArgumentOutOfRangeException`).
- **Edge Cases:** Test boundary conditions and less common scenarios.
- **Idempotency:** For methods that should be idempotent, verify this behavior.

## 7. Dealing with SDL Dependencies

Directly testing code that relies heavily on SDL3 native calls can be difficult in unit tests, as it often requires an initialized SDL environment and may involve external system state (like window handles or graphics contexts).

- **Focus on C# Logic:** Prioritize testing the C# logic that wraps or precedes SDL calls. This includes parameter validation, state management within the C# layer, and correct translation of parameters for SDL functions.
- **Abstraction (If Necessary):** For more complex C# logic interacting with SDL, consider if introducing a thin abstraction over direct SDL calls (internal to the module) could facilitate testing. This should be weighed against added complexity.
- **Integration Tests as a Complement:** Acknowledge that `Night.SampleGame` serves as the primary integration test suite where the full interaction with SDL is validated. Unit tests are meant to catch issues at a more granular C# level.
- **Mocking/Faking SDL (Use with Caution):**
  - Creating mocks or fakes for SDL functions is possible but can be very time-consuming and complex to maintain.
  - This approach should generally be avoided unless a critical piece of C# logic cannot be tested otherwise and its correctness is paramount.
  - If used, these fakes would need to simulate SDL behavior, which can be error-prone.

## 8. Running Tests

- Tests can be run using the `dotnet test` command from the command line in the solution root or the test project directory.
- Test runners integrated into IDEs (like Visual Studio, VS Code, Rider) can also be used.

## 9. Test Maintenance

- Tests should be kept up-to-date with code changes.
- Refactor tests along with production code to maintain clarity and relevance.
- Remove or update tests for deprecated or changed functionality.

## 10. Continuous Integration (Future)

Once a CI/CD pipeline is established, automated execution of these unit tests will be a key component to ensure code quality with every change.

This testing plan provides a foundation for building a robust suite of unit tests for `Night.Engine`. It will evolve as the engine grows and new features are added.
