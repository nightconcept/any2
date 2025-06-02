# Test Framework Refactoring and Boilerplate Reduction Plan

This document outlines the plan to refactor the test framework, primarily focusing on reducing boilerplate in test cases by enhancing `BaseTestCase.cs` and to guide the creation of new manual tests.

## Phase 1: Detailed Analysis of Existing Code and Boilerplate

**Objective:** Thoroughly understand the current test framework structure, identify common patterns, and pinpoint areas of repetitive code (boilerplate) in existing test cases.

1. **Deep Dive into `tests/Core/BaseTestCase.cs` ([`tests/Core/BaseTestCase.cs`](tests/Core/BaseTestCase.cs:1)):**
    * **Goal:** Fully understand its current mechanisms for test lifecycle management ([`Load()`](tests/Core/BaseTestCase.cs:95), [`Update(double deltaTime)`](tests/Core/BaseTestCase.cs:111), [`Draw()`](tests/Core/BaseTestCase.cs:117), [`QuitSelf()`](tests/Core/BaseTestCase.cs:219)), status reporting ([`CurrentStatus`](tests/Core/BaseTestCase.cs:34), [`Details`](tests/Core/BaseTestCase.cs:40)), and particularly how it handles manual test interactions ([`RequestManualConfirmation(string consolePrompt)`](tests/Core/BaseTestCase.cs:247), UI buttons, ESC key).
    * **Status:** Complete. Analysis performed during plan formulation.

2. **Analyze Boilerplate in `tests/Groups/Graphics/GraphicsClearColorTest.cs` ([`tests/Groups/Graphics/GraphicsClearColorTest.cs`](tests/Groups/Graphics/GraphicsClearColorTest.cs:1)):**
    * **Goal:** Pinpoint repetitive patterns specific to this manual test that could be generalized.
    * **Key Areas Examined:**
        * Logic in [`Update(double deltaTime)`](tests/Groups/Graphics/GraphicsClearColorTest.cs:42) for prompt delay and timeout.
        * ESC key handling in [`KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)`](tests/Groups/Graphics/GraphicsClearColorTest.cs:79).
    * **Status:** Complete. Analysis performed during plan formulation.

3. **Analyze Boilerplate in `tests/Groups/Timer/TimerTests.cs` ([`tests/Groups/Timer/TimerTests.cs`](tests/Groups/Timer/TimerTests.cs:1)):**
    * **Goal:** Identify common patterns in these automated tests.
    * **Key Areas Examined:**
        * Setup logic in `Load()`.
        * Common patterns in `Update(double deltaTime)` (checking [`IsDone`](tests/Core/BaseTestCase.cs:28), using [`TestStopwatch.ElapsedMilliseconds`](tests/Core/BaseTestCase.cs:23), frame counting, status setting, calling [`QuitSelf()`](tests/Core/BaseTestCase.cs:219)).
        * Special case: [`SleepTest`](tests/Groups/Timer/TimerTests.cs:175) completing in [`Load()`](tests/Groups/Timer/TimerTests.cs:191).
    * **Status:** Complete. Analysis performed during plan formulation.

## Phase 2: Design Enhancements for Reusability

**Objective:** Design modifications to `BaseTestCase.cs` (or introduce new base classes) to abstract common functionalities and reduce boilerplate in derived test cases.

1. **Enhancements for Manual Tests:**
    * **Goal:** Abstract common manual test behaviors.
    * **Option A: Enhance `BaseTestCase.cs` directly:**
        * Introduce protected virtual properties for `ManualTestTimeoutMilliseconds` and `ManualTestPromptDelayMilliseconds`.
        * Modify [`BaseTestCase.Update(double deltaTime)`](tests/Core/BaseTestCase.cs:111) to automatically handle prompt delay and timeout for manual tests.
        * Modify [`BaseTestCase.KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)`](tests/Core/BaseTestCase.cs:151) to automatically handle ESC key failure for manual tests.
    * **Option B: Create `BaseManualTestCase : BaseTestCase`:**
        * Create `tests/Core/BaseManualTestCase.cs`.
        * Move common manual test logic (prompt delay, timeout, ESC handling) into this new class.
    * **Decision for Implementation:** Proceeding with **Option A** (Enhance `BaseTestCase.cs` directly). This approach is simpler for the initial refactoring. If manual-specific logic becomes too extensive in the future, creating a `BaseManualTestCase` (Option B) can be revisited.

2. **Enhancements for Automated Tests:**
    * **Goal:** Provide helper methods or patterns in `BaseTestCase.cs` for common automated test scenarios.
    * **Proposals:**
        * `protected virtual void OnUpdateAutomated(double deltaTime)`: An overridable method intended to be the primary place for automated test logic within the `Update` loop. The base `Update` method in `BaseTestCase` could call this if the test is `Automated` and not `IsDone`.
        * `protected bool CheckCompletionAfterDuration(double milliseconds, Func<bool> successCondition = null, string passDetails = "Test passed: Met condition within duration.", string failDetailsTimeout = "Test failed: Timed out.", string failDetailsCondition = "Test failed: Did not meet condition within duration.")`: This helper will manage a timer.
            * If `successCondition` is `null`, it marks the test as passed after `milliseconds`.
            * If `successCondition` is provided, it checks it each frame. If true within `milliseconds`, marks as passed. If `milliseconds` elapses and condition is not met, marks as failed.
            * It will set `CurrentStatus` and `Details` and call `QuitSelf()`. Returns `true` if the test was completed by this call, `false` otherwise.
        * `protected bool CheckCompletionAfterFrames(int frameCount, Func<bool> successCondition = null, string passDetails = "Test passed: Met condition within frame limit.", string failDetailsTimeout = "Test failed: Exceeded frame limit.", string failDetailsCondition = "Test failed: Did not meet condition within frame limit.")`: Similar to the duration-based helper, but uses a frame counter.
            * It will require a new protected field in `BaseTestCase`: `protected int currentFrameCount = 0;` (incremented in `Update`).
            * It will set `CurrentStatus` and `Details` and call `QuitSelf()`. Returns `true` if the test was completed by this call, `false` otherwise.

## Phase 3: Implementation and Refactoring

**Objective:** Implement the designed enhancements and refactor existing test cases to utilize them. Create the new manual test case.

1. **Implement Base Class Changes:**
    * Modify [`tests/Core/BaseTestCase.cs`](tests/Core/BaseTestCase.cs:1) (and/or create `BaseManualTestCase.cs`).
2. **Refactor `tests/Groups/Graphics/GraphicsClearColorTest.cs` ([`tests/Groups/Graphics/GraphicsClearColorTest.cs`](tests/Groups/Graphics/GraphicsClearColorTest.cs:1)):**
    * Update to use enhanced base class features.
3. **Refactor `tests/Groups/Timer/TimerTests.cs` ([`tests/Groups/Timer/TimerTests.cs`](tests/Groups/Timer/TimerTests.cs:1)):**
    * Update timer tests to use new helpers/patterns.
4. **Create the New Manual Test Case (Task 1.9):**
    * Develop the new manual test, inheriting from the refactored base class.
    * Focus on test-specific logic: `Name`, `Description`, `Load()` for setup, `Draw()` for rendering, and calling `RequestManualConfirmation()`.

## Phase 4: Review and Documentation

**Objective:** Ensure changes are effective, clear, and well-documented.

1. **Code Review:**
    * Verify boilerplate reduction, clarity, and robustness.
2. **Documentation:**
    * Update C# comments.
    * Update this document (`test-refactor.md`) as decisions are made and phases completed.
    * Consider if updates are needed for `project/epics/epic-testing.md`.
