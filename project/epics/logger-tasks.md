# Epic: Implement Flexible Logging Module for Night Library

**User Story:** As a Night Engine developer, I want a flexible and extensible logging module so that I can easily output diagnostic messages to various configurable destinations (file, in-game console, memory, system console) with control over log levels and sink activation, replacing the current `System.Console.WriteLine` usage.

**Status:** Review

**Priority:** High

**Assignee:** Night Agent (AI)

**Reporter:** User

**Estimated Effort:** TBD

**Start Date:** YYYY-MM-DD
**Due Date:** YYYY-MM-DD

---

## Background

The current diagnostic output in the `Night` library relies on `System.Console.WriteLine`. This is inflexible for a game engine requiring configurable log outputs, varying log levels, and the ability to direct logs to different sinks like files or an in-game display. This epic aims to create a robust logging system internal to the `Night` namespace.

## Requirements

-   **Core Interfaces:** Define `ILogger`, `LogLevel` enum, `ILogSink`, and `LogEntry` record/class.
-   **Log Levels:** Support Trace, Debug, Information, Warning, Error, Fatal.
-   **`ILogger` Methods:** Include methods like `Debug(string message)`, `Error(string message, Exception exception)`, and `IsEnabled(LogLevel level)`.
-   **`LogEntry` Structure:** Must contain Timestamp (UTC), LogLevel, formatted Message, optional Exception, and CategoryName/Source string.
-   **`LogManager`:** Static class for obtaining `ILogger` instances (e.g., `LogManager.GetLogger("Renderer")`), managing active `ILogSink`s, global configuration (e.g., minimum log level), and enabling/disabling sinks.
-   **Sink Implementations:**
    -   `FileSink`: Writes to a configurable file path; opt-in.
    -   `InGameConsoleSink`: Buffers messages for in-game UI display.
    -   `MemorySink`: Stores recent log entries in memory.
    -   `SystemConsoleSink`: Writes to `System.Console`; *must be opt-in and not active by default*.
-   **Thread Safety:** Ensure logging operations are thread-safe.
-   **Error Handling:** Failures in one sink should not affect others or crash the application.
-   **Extensibility:** Design should allow adding new custom sinks.
-   **Namespace:** All logging components should be within the `Night` namespace and initially internal to the library.
-   **Configuration:** Provide clear mechanisms to enable/disable and configure sinks, especially `FileSink` (path) and `SystemConsoleSink`.
-   **Internal Documentation:** Add /// comments for all public types and methods within the logging module.

## Acceptance Criteria

1.  All `System.Console.WriteLine` calls within the `Night` library (specifically `Framework.cs` as a starting point) are replaced with the new logging system.
2.  Logging to `System.Console` is disabled by default and can be explicitly enabled via configuration.
3.  Logging to a file can be enabled and the output path configured.
4.  The `MemorySink` correctly captures recent log entries.
5.  The `InGameConsoleSink` provides a mechanism to retrieve buffered log messages.
6.  Log filtering based on global minimum log level works as expected.
7.  Categorized loggers (e.g., `LogManager.GetLogger("Framework")`) correctly identify the source in `LogEntry`.
8.  The system is thread-safe under typical engine usage patterns.
9.  Basic error handling for sink operations is implemented and observable (e.g., logs an internal error if a sink fails).
10. All new logging code is documented with /// comments.

---

## Tasks

### Phase 1: Core Logging Infrastructure

-   [x] **Task 1.1:** Define `LogLevel` enum (Trace, Debug, Information, Warning, Error, Fatal).
    -   *File(s):* `Night/Log/LogLevel.cs`
-   [x] **Task 1.2:** Define `LogEntry` record/class.
    -   *Details:* Include `DateTime TimestampUtc`, `LogLevel Level`, `string Message`, `Exception? Exception`, `string CategoryName`.
    -   *File(s):* `Night/Log/LogEntry.cs`
-   [x] **Task 1.3:** Define `ILogSink` interface.
    -   *Methods:* `void Write(LogEntry entry);`
    -   *File(s):* `Night/Log/ILogSink.cs`
-   [x] **Task 1.4:** Define `ILogger` interface.
    -   *Methods:* `void Log(LogLevel level, string message, Exception? exception = null);`, `bool IsEnabled(LogLevel level);`
    -   *Convenience Methods:* `Trace(string message)`, `Debug(string message)`, `Info(string message)`, `Warn(string message)`, `Error(string message, Exception? exception = null)`, `Fatal(string message, Exception? exception = null)`.
    -   *File(s):* `Night/Log/ILogger.cs`
-   [x] **Task 1.5:** Implement `Logger` class (implements `ILogger`).
    -   *Details:* Takes `LogManager` and `categoryName` in constructor. Forwards logs to `LogManager`.
    -   *File(s):* `Night/Log/Logger.cs`
-   [x] **Task 1.6:** Write manual tests/verification steps for Core Logging Infrastructure.

#### Manual Verification Steps for Phase 1

1.  **Compilation Check:** Ensure the project compiles successfully after creating all files in this phase (`Night/Log/LogLevel.cs`, `Night/Log/LogEntry.cs`, `Night/Log/ILogSink.cs`, `Night/Log/ILogger.cs`, `Night/Log/Logger.cs`).
2.  **API Review:**
    *   Open `LogLevel.cs` and verify all enum values (Trace, Debug, Information, Warning, Error, Fatal) are present.
    *   Open `LogEntry.cs` and verify all specified properties (`DateTime TimestampUtc`, `LogLevel Level`, `string Message`, `Exception? Exception`, `string CategoryName`) are present with correct types.
    *   Open `ILogSink.cs` and verify the `Write(LogEntry entry)` method signature.
    *   Open `ILogger.cs` and verify the `Log(LogLevel level, string message, Exception? exception = null)` and `IsEnabled(LogLevel level)` method signatures, as well as all convenience method signatures (`Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`).
    *   Open `Logger.cs` and verify it implements `ILogger` and has a constructor accepting `LogManager` (or a reference to its dispatching mechanism) and a `categoryName`.
3.  **(Conceptual) Instantiation Check:** Mentally (or with a temporary, isolated code snippet if feasible, though `LogManager` is not yet built), confirm that `Logger` could be instantiated if a `LogManager` instance/surrogate were available.

### Phase 2: LogManager and Basic Sinks

-   [x] **Task 2.1:** Implement `LogManager` static class.
    -   *Responsibilities:* Manage list of `ILogSink`s, provide `GetLogger(string categoryName)`, global minimum log level (`MinLevel`), methods `AddSink(ILogSink sink)`, `RemoveSink(ILogSink sink)`, `ClearSinks()`.
    -   *Details:* `GetLogger` should return an `ILogger` instance (e.g., `Logger` class from Task 1.5). Internal method to dispatch `LogEntry` to all active sinks if `entry.Level >= MinLevel`.
    -   *File(s):* `Night/Log/LogManager.cs`
-   [x] **Task 2.2:** Implement `SystemConsoleSink` (implements `ILogSink`).
    -   *Details:* Writes formatted `LogEntry` to `System.Console`. Ensure it can be added/removed via `LogManager`. *Crucially, this sink should not be added by default.*
    -   *File(s):* `Night/Log/Sinks/SystemConsoleSink.cs`
-   [x] **Task 2.3:** Implement `MemorySink` (implements `ILogSink`).
    -   *Details:* Stores a configurable number of recent `LogEntry` objects in a thread-safe collection (e.g., `ConcurrentQueue` with size limit). Provides a method to retrieve buffered entries.
    -   *File(s):* `Night/Log/Sinks/MemorySink.cs`
-   [ ] **Task 2.4:** Write manual tests/verification steps for LogManager and Basic Sinks.

#### Manual Verification Steps for Phase 2

*Prerequisites: A simple test harness or a temporary console application where you can call `LogManager` methods.*

1.  **`LogManager.GetLogger()`:**
    *   Call `LogManager.GetLogger("TestCategory1")`. Verify it returns a non-null `ILogger` instance.
    *   Call `LogManager.GetLogger("TestCategory1")` again. Verify it returns the same instance (or an equivalent one for the same category).
    *   Call `LogManager.GetLogger("TestCategory2")`. Verify it returns a different instance (or an equivalent one for a new category).
2.  **`SystemConsoleSink` (Opt-In Behavior):**
    *   Without adding any sinks, use an `ILogger` instance to log a message (e.g., `logger.Info("Test message")`). Verify *nothing* is printed to the console.
    *   Create an instance of `SystemConsoleSink`.
    *   Call `LogManager.AddSink(systemConsoleSinkInstance)`.
    *   Log a message (e.g., `logger.Info("Hello Console!")`). Verify the message appears on `System.Console`, formatted appropriately (e.g., showing timestamp, level, category, message).
    *   Call `LogManager.RemoveSink(systemConsoleSinkInstance)`.
    *   Log another message. Verify it no longer appears on the console.
3.  **`MemorySink`:**
    *   Create an instance of `MemorySink` (e.g., `var memorySink = new MemorySink(capacity: 5);`).
    *   Call `LogManager.AddSink(memorySink)`.
    *   Log several messages (e.g., 6 messages: "Msg1" to "Msg6").
    *   Call the method on `memorySink` to retrieve buffered entries (e.g., `memorySink.GetEntries()`).
    *   Verify the retrieved collection contains the last 5 messages ("Msg2" to "Msg6").
    *   Verify each `LogEntry` in the buffer has correct Timestamp, Level, Message, and CategoryName.
4.  **Global Minimum Log Level (`LogManager.MinLevel`):**
    *   Ensure `SystemConsoleSink` is added.
    *   Set `LogManager.MinLevel = LogLevel.Warning;`.
    *   Log messages: `logger.Debug("Debug A")`, `logger.Info("Info A")`, `logger.Warn("Warning A")`.
    *   Verify only "Warning A" (and any higher level messages) appear on the console.
    *   Set `LogManager.MinLevel = LogLevel.Trace;` (or the lowest level).
    *   Log `logger.Debug("Debug B")`. Verify "Debug B" now appears.
5.  **`ClearSinks()`:**
    *   Add `SystemConsoleSink` and `MemorySink`. Log a message and verify output/capture.
    *   Call `LogManager.ClearSinks()`.
    *   Log another message. Verify no output to console and no new entries in the `MemorySink` (its existing entries might still be there until new ones push them out or it's cleared/recreated).

### Phase 3: Advanced Sinks

-   [x] **Task 3.1:** Implement `FileSink` (implements `ILogSink`).
    -   *Details:* Writes formatted `LogEntry` to a specified file. Path should be configurable. Handles file I/O safely. Ensure it can be added/removed.
    -   *File(s):* `Night/Log/Sinks/FileSink.cs`
-   [x] **Task 3.2:** Implement `InGameConsoleSink` (implements `ILogSink`).
    -   *Details:* Buffers `LogEntry` objects in a thread-safe collection. Provides a way for a UI to access these messages (e.g., a public method to retrieve the buffer or an event).
    -   *File(s):* `Night/Log/Sinks/InGameConsoleSink.cs`
-   [x] **Task 3.3:** Write manual tests/verification steps for Advanced Sinks.

#### Manual Verification Steps for Phase 3

*Prerequisites: Test harness from Phase 2. Ensure `SystemConsoleSink` can be optionally active for comparison if needed.*

1.  **`FileSink`:**
    *   Create an instance of `FileSink`, providing a valid, writable file path (e.g., `"./test_log.txt"`).
    *   Call `LogManager.AddSink(fileSinkInstance)`.
    *   Log several messages with different levels and categories (e.g., `LogManager.GetLogger("FileTest").Info("Log to file");`, `LogManager.GetLogger("FileTest").Error("File error!", new System.Exception("dummy exception"));`).
    *   Open `test_log.txt`. Verify:
        *   The file was created.
        *   All logged messages are present.
        *   Each log entry is formatted correctly (timestamp, level, category, message, exception details if any).
    *   Log more messages. Verify they are appended to the file.
    *   (Optional) Test with an invalid/unwritable path if error handling for sink construction/write is part of this phase's scope. Observe behavior (e.g., internal error logged if a fallback exists, no crash).
    *   Call `LogManager.RemoveSink(fileSinkInstance)`. Log a message. Verify it's not written to `test_log.txt`.
2.  **`InGameConsoleSink`:**
    *   Create an instance of `InGameConsoleSink` (e.g., `var gameConsoleSink = new InGameConsoleSink(capacity: 10);`).
    *   Call `LogManager.AddSink(gameConsoleSink)`.
    *   Log several messages (e.g., 12 messages).
    *   Call the method on `gameConsoleSink` designed to retrieve/expose its buffered `LogEntry` objects.
    *   Verify the collection contains the last 10 messages.
    *   Verify the format and content of these `LogEntry` objects (Timestamp, Level, Message, Category).
    *   If an event-based notification is implemented for new messages, try to (conceptually or with a simple handler) verify it fires.

### Phase 4: Thread Safety, Error Handling, and Configuration

-   [ ] **Task 4.1:** Ensure thread safety for `LogManager` and all sink operations.
    -   *Details:* Review and use appropriate synchronization primitives (`lock`, `ConcurrentDictionary`, `ConcurrentQueue`, etc.) where necessary.
-   [ ] **Task 4.2:** Implement error handling within sinks.
    -   *Details:* A failure in one sink (e.g., file I/O error in `FileSink`) should not stop other sinks or crash the application. Consider logging such errors to `System.Diagnostics.Trace` or a fallback mechanism.
-   [ ] **Task 4.3:** Design and implement sink configuration mechanism.
    -   *Details:* Determine how sinks like `FileSink` (path) and `SystemConsoleSink` (enable/disable) will be configured. This could be through methods on `LogManager` or by reading from `ConfigurationManager` if appropriate for internal use.
    -   *Example:* `LogManager.EnableSystemConsoleSink()`, `LogManager.ConfigureFileSink(string path, LogLevel minLevelForFile)`.
-   [ ] **Task 4.4:** Write manual tests/verification steps for Thread Safety, Error Handling, and Configuration.

#### Manual Verification Steps for Phase 4

1.  **Thread Safety:**
    *   Create a test program that initializes `LogManager` with multiple sinks (e.g., `SystemConsoleSink`, `FileSink`, `MemorySink`).
    *   Spawn several threads (e.g., 5-10 threads).
    *   In each thread, create a logger instance (e.g., `LogManager.GetLogger($"Thread-{threadId}")`) and log a significant number of messages (e.g., 100-1000) in a loop.
    *   *Observe:*
        *   Console output (if `SystemConsoleSink` is active): Check for garbled messages or interleaved lines that indicate race conditions. Some interleaving of *whole messages* is expected and fine.
        *   File output (`FileSink`): Check the log file for corruption, garbled lines, or missing messages.
        *   `MemorySink`: Retrieve entries and check for consistency and completeness (considering its capacity).
        *   Application stability: Ensure no exceptions related to concurrent access are thrown and the application doesn't deadlock.
2.  **Sink Error Handling:**
    *   (Requires ability to simulate sink failure, e.g., by modifying a sink temporarily or setting up a specific failing scenario).
    *   Add `FileSink` configured to a valid path, and `SystemConsoleSink`.
    *   Log a message; verify output to both.
    *   Simulate an error in `FileSink` (e.g., make the log file read-only, or if `FileSink` has an internal method to force a failure on next write for testing).
    *   Log another message.
    *   *Verify:*
        *   The message still appears in `SystemConsoleSink`.
        *   The application does not crash.
        *   If a fallback logging mechanism for sink errors is implemented (e.g., to `System.Diagnostics.Trace`), check for an error message indicating the `FileSink` failure.
    *   Restore `FileSink` to normal operation. Log another message and verify it writes to the file again.
3.  **Sink Configuration Mechanism:**
    *   **`SystemConsoleSink` Enable/Disable:**
        *   If using a method like `LogManager.EnableSystemConsoleSink(bool enabled)`:
            *   Call `LogManager.EnableSystemConsoleSink(true)`. Log a message. Verify console output.
            *   Call `LogManager.EnableSystemConsoleSink(false)`. Log a message. Verify no console output.
        *   If using `ConfigurationManager`: Modify the configuration source to enable/disable the console sink, re-initialize/re-read config in `LogManager` (if applicable), and test.
    *   **`FileSink` Configuration (Path, Level):**
        *   If using methods like `LogManager.ConfigureFileSink(string path, LogLevel minLevel)`:
            *   Call `LogManager.ConfigureFileSink("path1.log", LogLevel.Info)`. Log Debug, Info, Warn messages. Verify only Info and Warn messages go to "path1.log".
            *   Call `LogManager.ConfigureFileSink("path2.log", LogLevel.Debug)`. Log Debug, Info messages. Verify Debug and Info messages go to "path2.log". (Ensure old sinks are cleared or removed if `ConfigureFileSink` replaces).
        *   If using `ConfigurationManager`: Modify config for file path and level, re-initialize/re-read, and test.
    *   **Global `LogManager.MinLevel` vs. Sink-Specific Level:**
        *   Set `LogManager.MinLevel = LogLevel.Warning`.
        *   Configure `FileSink` with `minLevelForFile = LogLevel.Debug` (or a level lower than global).
        *   Log Debug, Info, Warning messages. Verify only Warning messages appear in the file (due to global filter).
        *   Set `LogManager.MinLevel = LogLevel.Trace`.
        *   Configure `FileSink` with `minLevelForFile = LogLevel.Info`.
        *   Log Debug, Info, Warning messages. Verify Info and Warning messages appear in the file (FileSink filters Debug, global allows all).

### Phase 5: Integration and Refactoring

-   [ ] **Task 5.1:** Integrate `LogManager` into `Night.Framework`.
    -   *Details:* Initialize `LogManager` (e.g., set default `MinLevel`). Potentially expose a way for the game to add its own sinks if ever needed in the future (though initially internal).
-   [ ] **Task 5.2:** Refactor `Framework.cs` to use the new logging system.
    -   *Details:* Replace all `Console.WriteLine` calls with `LogManager.GetLogger("Framework").Info(...)` or other appropriate levels/methods.
-   [ ] **Task 5.3:** (Optional Stretch) Identify and refactor other `Console.WriteLine` calls in the `Night` library.
-   [ ] **Task 5.4:** Write manual tests/verification steps for Integration and Refactoring.

#### Manual Verification Steps for Phase 5

*Prerequisites: The `Night` engine/framework should be runnable, at least to the point where `Framework.cs` executes its initialization and main loop logic.*

1.  **Configuration for Test:**
    *   Configure the logging system (e.g., via code in `Main` or `Game.Load`, or through `ConfigurationManager` if that's the chosen method) to:
        *   Enable `SystemConsoleSink`.
        *   Set global minimum log level to `LogLevel.Trace` or `LogLevel.Debug` to capture detailed framework messages.
        *   (Optional) Enable `FileSink` to a known file path for persistent log checking.
2.  **Run the Application:** Execute a simple application or test case that uses `Night.Framework.Run()`.
3.  **Observe Logs from `Framework.cs`:**
    *   Check the console output (and file log, if configured).
    *   Look for messages that were previously `Console.WriteLine` in `Framework.cs`. Examples:
        *   "Night Engine: v..."
        *   "SDL: v..."
        *   "Platform: ..."
        *   "Framework: ..."
        *   "Night.Framework.Run: Testing environment detected..." (if applicable)
        *   "Night.Framework.Run: Global isSdlInitialized is false..."
        *   "Night.Framework.Run: SDL_Init failed..." (if an error is forced)
        *   "Night.Framework.Run: Calling Window.SetMode..."
        *   "Night.Framework.Run: Window.SetMode returned..."
        *   "Night.Framework.Run: Proceeding to game.Load()..."
        *   "Night.Framework.Run: Starting main loop..."
        *   Event handling messages (if any were converted from `Console.WriteLine`).
        *   Error messages handled by `HandleGameException` or `DefaultErrorHandler` (if they were converted).
    *   Verify these messages are now formatted by the logging system (e.g., include timestamp, level, category "Framework").
4.  **Verify Log Levels and Categories:**
    *   Ensure the logged messages from `Framework.cs` use appropriate log levels (e.g., Info for general information, Debug for verbose details, Error for errors).
    *   Confirm the category is correctly set (e.g., "Framework", or more specific categories if used within `Framework.cs`).
5.  **Test Specific Scenarios:**
    *   If `Framework.cs` had `Console.WriteLine` for specific error conditions (e.g., SDL init failure), try to trigger those conditions (if feasible in a test environment) and verify the errors are logged via the new system.
    *   Test window creation, fullscreen changes, VSync changes, etc., and look for corresponding log messages that replace previous console outputs.
6.  **(If Task 5.3 was done) Verify Other Refactored Areas:** If other parts of the `Night` library were refactored, run tests or application parts that exercise those areas and check for expected log outputs.

### Phase 6: Documentation and Review

-   [ ] **Task 6.1:** Add /// XML documentation comments to all new public types and methods in the logging module.
-   [ ] **Task 6.2:** Perform a self-review of the implemented logging system against all requirements and acceptance criteria.
-   [ ] **Task 6.3:** Prepare the changes for user review.
-   [ ] **Task 6.4:** Write manual tests/verification steps for Documentation and Review.

#### Manual Verification Steps for Phase 6

1.  **XML Documentation Review (IDE / Generated File):**
    *   In your C# IDE (e.g., Visual Studio, Rider), hover over class names, method names, properties, and enum members within the `Night.Log` namespace (e.g., `LogManager`, `ILogger.Info`, `LogEntry.TimestampUtc`, `LogLevel.Debug`, `FileSink`).
    *   Verify that descriptive /// XML comments appear as tooltips, explaining their purpose, parameters, and return values where applicable.
    *   If your build process generates an XML documentation file, locate it and open it. Browse through the documentation for the logging module components and check for completeness and correctness.
2.  **Code Review (Self-Review):**
    *   Re-read the "Requirements" and "Acceptance Criteria" sections of this epic document (`logger-tasks.md`).
    *   Go through each implemented file in the `Night.Log` namespace (`LogLevel.cs`, `LogEntry.cs`, `ILogSink.cs`, `ILogger.cs`, `Logger.cs`, `LogManager.cs`, and all sink implementations).
    *   For each requirement and acceptance criterion, confirm that the implemented code meets it. For example:
        *   Is `SystemConsoleSink` truly opt-in and not active by default?
        *   Can `FileSink` path be configured?
        *   Is `LogManager.MinLevel` respected?
        *   Is `LogEntry.CategoryName` populated correctly?
3.  **Final Functionality Check (Spot Check):**
    *   Perform a small subset of the manual verification steps from Phases 2-5 to ensure key functionalities are still working as expected after any final tweaks or documentation-related code changes.
    *   For example:
        *   Enable `SystemConsoleSink`, log a message, verify output.
        *   Enable `FileSink`, log a message, verify file output.
        *   Test `LogManager.MinLevel` filtering briefly.
        *   Run a simple `Night.Framework` application and check if `Framework.cs` logs are appearing.
4.  **Epic Document Review:**
    *   Read through the "Notes & Decisions" section. Ensure all documented decisions were followed or updated.
    *   Read through the "Questions for User" section. Ensure any questions raised during development were addressed or are noted as pending.
    *   Update the "Status" of tasks and the main epic status (e.g., to "Ready for Review" or similar).
    *   Fill in "Actual Effort" or "Completion Date" if applicable.

---

## Notes & Decisions

*(This section will be updated as development progresses)*

-   Initial decision: Logging module will reside entirely within the `Night` namespace and its components marked as `internal` where appropriate if they are not meant to be part of the engine's public API yet.
-   Configuration for opt-in sinks (`SystemConsoleSink`, `FileSink`) needs careful design to be user-friendly for developers using the Night engine, even if initially for internal Night library use.
