# Epic: Implement Filesystem.Lines Functionality

**Status:** Review

**Goal:** To implement a `Night.Filesystem.Lines` function that provides an easy way to iterate over lines in a text file, similar to Love2D's `file:lines()`, and to ensure its reliability through comprehensive unit testing.

## Problem Description

The `Night.Filesystem` module currently lacks a convenient method for iterating line-by-line through a text file. This capability is essential for various game development tasks, such as parsing configuration files, loading dialogue scripts, or reading level data.

## Solution Overview

A new static method, `Night.Filesystem.Lines(string filePath)`, will be introduced. This method will return an `IEnumerable<string>`, allowing developers to use a standard C# `foreach` loop to iterate over the lines of the specified file. The implementation will utilize `System.IO.File.ReadLines` for efficient, deferred line reading. Robust unit tests will be developed within the `NightTest` framework to cover various scenarios, including empty files, files with multiple lines, and error conditions like non-existent files.

## User Stories

- As a Developer, I want to iterate over lines in a file easily so that I can process text-based file content efficiently.
- As a Developer, I want this file iteration functionality to be thoroughly tested so that I can confidently rely on its correctness and error handling.

## Tasks

### Phase 1: Implement `Night.Filesystem.Lines` Function

- **Task 1.1: Implement `Night.Filesystem.Lines`**
  - **Description:** Implement the `Lines` function within the `Night.Filesystem` module. This function will accept a file path string and return an `IEnumerable<string>` that yields each line of the file.
  - **Implementation Details:**
    - [ ] Define the public static method `Lines(string filePath)` in [`src/Night/Filesystem/Filesystem.cs`](src/Night/Filesystem/Filesystem.cs).
    - [ ] Internally, use `System.IO.File.ReadLines(filePath)` to get an `IEnumerable<string>`. This handles deferred reading and efficient memory usage.
    - [ ] Ensure that exceptions from `System.IO.File.ReadLines` (e.g., `FileNotFoundException`, `IOException`) are allowed to propagate naturally, as they are standard .NET exceptions that developers would expect.
  - **Acceptance Criteria:**
    - `Night.Filesystem.Lines(filePath)` returns an `IEnumerable<string>`.
    - Iterating over the result successfully reads all lines from a given text file.
    - The method works correctly with empty files (returns an empty enumerable).
    - The method correctly handles files with various line endings (CRLF, LF).
    - Standard .NET file I/O exceptions are propagated if the file cannot be accessed or read.
  - **Status:** Done

### Phase 2: Unit Testing for `Night.Filesystem.Lines`

- **Task 2.1: Create Test Infrastructure for Filesystem**
  - **Description:** Set up the necessary test files and classes for `Night.Filesystem` tests if they don't already exist.
  - **Implementation Details:**
    - [ ] Ensure a test group class `FilesystemGroup.cs` exists in `tests/Groups/Filesystem/` (e.g., [`tests/Groups/Filesystem/FilesystemGroup.cs`](tests/Groups/Filesystem/FilesystemGroup.cs)). This class should inherit from `NightTest.Core.TestGroup`.
    - [ ] Create a new file for `IGame` test cases related to `Filesystem.Lines` (e.g., [`tests/Groups/Filesystem/LinesTests.cs`](tests/Groups/Filesystem/LinesTests.cs)).
  - **Acceptance Criteria:**
    - The basic structure for filesystem tests is in place.
  - **Status:** Done

- **Task 2.2: Implement Test Case for Reading a Standard File**
  - **Description:** Test reading lines from a file with typical content.
  - **Implementation Details:**
    - [ ] Create an `IGame` test case class (e.g., `Lines_ReadStandardFileTest`) in [`tests/Groups/Filesystem/LinesTests.cs`](tests/Groups/Filesystem/LinesTests.cs) inheriting from `NightTest.Core.BaseTestCase`.
    - [ ] `Name`: "Filesystem.Lines.ReadStandardFile"
    - [ ] `Description`: "Tests reading lines from a standard text file with multiple lines."
    - [ ] `Load()`: Create a temporary test file (e.g., `test_standard.txt`) with 3-5 lines of known content, including varied line endings if possible or assume consistent environment.
    - [ ] `Update()`:
      - Use `Night.Filesystem.Lines()` to read the temporary file.
      - Collect the lines into a list.
      - Assert that the number of lines read matches the expected count.
      - Assert that the content of each line matches the expected content.
      - Set `CurrentStatus` to `TestStatus.Passed` or `TestStatus.Failed` with details.
      - Call `EndTest()`.
    - [ ] `Unload()` (or ensure cleanup in `Load` if run multiple times/handle via `try-finally`): Delete the temporary test file.
    - [ ] Add a corresponding `[Fact]` method in `FilesystemGroup.cs` to run this `IGame` test case using `Run_TestCase()`. Add `[Trait("TestType", "Automated")]`.
  - **Acceptance Criteria:**
    - The test passes when reading a file with multiple lines.
    - Line content and order are correctly verified.
  - **Status:** Done

- **Task 2.3: Implement Test Case for Reading an Empty File**
  - **Description:** Test reading lines from an empty file.
  - **Implementation Details:**
    - [ ] Create an `IGame` test case class (e.g., `Lines_ReadEmptyFileTest`) in [`tests/Groups/Filesystem/LinesTests.cs`](tests/Groups/Filesystem/LinesTests.cs).
    - [ ] `Name`: "Filesystem.Lines.ReadEmptyFile"
    - [ ] `Description`: "Tests reading lines from an empty text file."
    - [ ] `Load()`: Create an empty temporary test file (e.g., `test_empty.txt`).
    - [ ] `Update()`:
      - Use `Night.Filesystem.Lines()` to read the file.
      - Assert that the resulting enumerable is empty (e.g., `!result.Any()` or `result.Count() == 0`).
      - Set `CurrentStatus` to `TestStatus.Passed` or `TestStatus.Failed`.
      - Call `EndTest()`.
    - [ ] `Unload()`: Delete the temporary test file.
    - [ ] Add a corresponding `[Fact]` method in `FilesystemGroup.cs`. Add `[Trait("TestType", "Automated")]`.
  - **Acceptance Criteria:**
    - The test passes when reading an empty file.
    - The returned `IEnumerable<string>` is empty.
  - **Status:** Done

- **Task 2.4: Implement Test Case for File Not Found**
  - **Description:** Test behavior when the specified file does not exist.
  - **Implementation Details:**
    - [ ] Create an `IGame` test case class (e.g., `FilesystemLines_FileNotFoundTest`) in [`tests/Groups/Filesystem/LinesTests.cs`](tests/Groups/Filesystem/LinesTests.cs).
    - [ ] `Name`: "Filesystem.Lines.FileNotFound"
    - [ ] `Description`: "Tests that Night.Filesystem.Lines throws FileNotFoundException for a non-existent file."
    - [ ] `Update()`:
      - Use a `try-catch` block.
      - Attempt to call `Night.Filesystem.Lines("non_existent_file.txt").ToList()` (or iterate it) inside the `try` block.
      - If `FileNotFoundException` is caught, set `CurrentStatus = TestStatus.Passed`.
      - If any other exception is caught, or no exception is caught, set `CurrentStatus = TestStatus.Failed` with details.
      - Call `EndTest()`.
    - [ ] Add a corresponding `[Fact]` method in `FilesystemGroup.cs`. Add `[Trait("TestType", "Automated")]`.
  - **Acceptance Criteria:**
    - The test passes if `System.IO.FileNotFoundException` is thrown when `Night.Filesystem.Lines` is used with a path to a non-existent file.
  - **Status:** Done

- **Task 2.5: Implement Test Case for File with Single Line**
  - **Description:** Test reading lines from a file with only one line.
  - **Implementation Details:**
    - [ ] Create an `IGame` test case class (e.g., `FilesystemLines_ReadSingleLineFileTest`) in [`tests/Groups/Filesystem/LinesTests.cs`](tests/Groups/Filesystem/LinesTests.cs).
    - [ ] `Name`: "Filesystem.Lines.ReadSingleLineFile"
    - [ ] `Description`: "Tests reading lines from a text file containing only a single line."
    - [ ] `Load()`: Create a temporary test file (e.g., `test_single_line.txt`) with one line of known content.
    - [ ] `Update()`:
      - Use `Night.Filesystem.Lines()` to read the temporary file.
      - Collect the lines into a list.
      - Assert that one line was read.
      - Assert that the content of the line matches the expected content.
      - Set `CurrentStatus` to `TestStatus.Passed` or `TestStatus.Failed`.
      - Call `EndTest()`.
    - [ ] `Unload()`: Delete the temporary test file.
    - [ ] Add a corresponding `[Fact]` method in `FilesystemGroup.cs`. Add `[Trait("TestType", "Automated")]`.
  - **Acceptance Criteria:**
    - The test passes when reading a file with a single line.
    - The line content is correctly verified.
  - **Status:** Done

## Risks/Challenges

- Ensuring proper file handle management, although `System.IO.File.ReadLines` largely abstracts this.
- Maintaining consistent error handling for file I/O issues (e.g., file not found, access denied) in a way that aligns with the rest of the `Night` framework and standard .NET practices.
- Test flakiness due to filesystem interactions, though using temporary files in dedicated test asset locations should mitigate this.
