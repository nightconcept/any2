# Epic: 0.0.2 Functionality

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

### Phase 3: Implement `Night.Filesystem.File` Class (@Filesystem)

- **Task 3.1: Define `FileMode` and `BufferMode` Enums**
  - **Description:** Define enums for file opening modes (read, write, append) and buffer modes, mirroring LÖVE's API.
  - **Implementation Details:**
    - [x] Define `public enum FileMode { Read, Write, Append }` in a new file `src/Night/Filesystem/FileMode.cs`.
    - [x] Define `public enum BufferMode { None, Line, Full }` in a new file `src/Night/Filesystem/BufferMode.cs`.
  - **Acceptance Criteria:**
    - Enums `Night.Filesystem.FileMode` and `Night.Filesystem.BufferMode` are defined and accessible.
  - **Status:** Done

- **Task 3.2: Define `Night.Filesystem.File` Class Structure**
  - **Description:** Create the basic structure for the `File` class which will encapsulate file operations.
  - **Implementation Details:**
    - [ ] Create `public class File : IDisposable` in a new file `src/Night/Filesystem/File.cs`.
    - [ ] Add private fields: `_filePath` (string), `_stream` (System.IO.FileStream), `_currentFileMode` (FileMode?), `_isOpen` (bool), `_currentBufferMode` (BufferMode), `_bufferSize` (long).
    - [ ] Add a private `_streamWriter` (System.IO.StreamWriter) and `_streamReader` (System.IO.StreamReader) for buffered operations, to be initialized as needed.
  - **Acceptance Criteria:**
    - `src/Night/Filesystem/File.cs` exists with the initial class definition and private members.
  - **Status:** ToDo

- **Task 3.3: Implement Constructor and `Night.Filesystem.NewFile` Factory Method**
  - **Description:** Implement the internal constructor for the `File` class and the public `NewFile` factory method in `Night.Filesystem.Filesystem`.
  - **Implementation Details:**
    - [ ] Implement an internal constructor `internal File(string filePath)` in `File.cs` that initializes `_filePath` and sets default states (e.g., `_isOpen = false`).
    - [ ] Implement `public static File NewFile(string filePath)` in `src/Night/Filesystem/Filesystem.cs`. This method will return `new File(filePath)`.
  - **Acceptance Criteria:**
    - `Night.Filesystem.NewFile(filePath)` returns a `File` instance.
    - The returned `File` instance has its `_filePath` correctly set and is initially closed.
  - **Status:** ToDo

- **Task 3.4: Implement `File.Open(FileMode mode)`**
  - **Description:** Implement the method to open the file in the specified mode.
  - **Implementation Details:**
    - [ ] Define `public (bool success, string? error) Open(FileMode mode)` in `File.cs`.
    - [ ] If already open, return `(false, "File is already open.")`.
    - [ ] Use `System.IO.FileStream` to open `_filePath`. Map `Night.Filesystem.FileMode` to `System.IO.FileMode` (e.g., Read -> Open, Write -> Create, Append -> Append) and corresponding `System.IO.FileAccess`.
    - [ ] Store the opened `_stream`, set `_isOpen = true`, `_currentFileMode = mode`.
    - [ ] Initialize `_streamReader` or `_streamWriter` based on mode if default buffering implies their use.
    - [ ] Handle exceptions (e.g., `IOException`, `UnauthorizedAccessException`, `FileNotFoundException` if mode is Read but file doesn't exist) and return `(false, e.Message)`.
  - **Acceptance Criteria:**
    - File opens successfully in Read, Write, and Append modes.
    - Returns `(true, null)` on success. `_isOpen` is true.
    - Returns `(false, errorMessage)` on failure. `_isOpen` remains false.
  - **Status:** ToDo

- **Task 3.5: Implement `File.Close()` and `IDisposable.Dispose()`**
  - **Description:** Implement methods to close the file and release resources.
  - **Implementation Details:**
    - [ ] Define `public bool Close()` in `File.cs`.
    - [ ] If not `_isOpen`, return `true` (or `false` if strict).
    - [ ] Dispose `_streamReader`, `_streamWriter`, and `_stream` in that order. Nullify them.
    - [ ] Set `_isOpen = false`, `_currentFileMode = null`.
    - [ ] Return `true` on successful close, `false` if an error occurs (though stream disposal rarely fails if open).
    - [ ] Implement `public void Dispose()` which calls `Close()` and `GC.SuppressFinalize(this)`. Add a finalizer `~File()` calling `Close()` as a safeguard if necessary (generally prefer explicit Dispose).
  - **Acceptance Criteria:**
    - File is closed, resources (streams) are released.
    - `Close()` returns `true`. `_isOpen` is false.
    - `Dispose()` correctly calls `Close()`.
  - **Status:** ToDo

- **Task 3.6: Implement `File.IsOpen()`**
  - **Description:** Gets whether the file is currently open.
  - **Implementation Details:**
    - [ ] Define `public bool IsOpen()` in `File.cs`.
    - [ ] Return the value of `_isOpen`.
  - **Acceptance Criteria:**
    - Returns `true` if the file is open, `false` otherwise.
  - **Status:** ToDo

- **Task 3.7: Implement `File.GetFilename()`**
  - **Description:** Gets the filename (path) the File object was created with.
  - **Implementation Details:**
    - [ ] Define `public string GetFilename()` in `File.cs`.
    - [ ] Return `_filePath`.
  - **Acceptance Criteria:**
    - Returns the original file path provided to `NewFile`.
  - **Status:** ToDo

- **Task 3.8: Implement `File.GetMode()`**
  - **Description:** Gets the `FileMode` the file has been opened with.
  - **Implementation Details:**
    - [ ] Define `public FileMode? GetMode()` in `File.cs`.
    - [ ] Return `_currentFileMode` if `_isOpen` is true, otherwise `null`.
  - **Acceptance Criteria:**
    - Returns the correct `FileMode` if the file is open, `null` otherwise.
  - **Status:** ToDo

- **Task 3.9: Implement `File.GetSize()`**
  - **Description:** Returns the file size in bytes.
  - **Implementation Details:**
    - [ ] Define `public (long size, string? error) GetSize()` in `File.cs`.
    - [ ] If `_isOpen` and `_stream.CanSeek`, return `(_stream.Length, null)`.
    - [ ] If not open, try to get size using `System.IO.FileInfo(_filePath).Length`.
    - [ ] Handle `FileNotFoundException` if file does not exist when not open, return `(-1, "File not found.")`.
    - [ ] Return `(-1, errorMessage)` for other errors.
  - **Acceptance Criteria:**
    - Returns correct file size for open and closed files.
    - Returns an error tuple if size cannot be determined.
  - **Status:** ToDo

- **Task 3.10: Implement `File.IsEOF()`**
  - **Description:** Gets whether the end-of-file has been reached during reading.
  - **Implementation Details:**
    - [ ] Define `public bool IsEOF()` in `File.cs`.
    - [ ] Requires file to be open for reading. If not, return `true` or throw an exception.
    - [ ] If `_stream` is readable and seekable: return `_stream.Position >= _stream.Length`.
    - [ ] If using `_streamReader`, this might be `_streamReader.EndOfStream`. Choose one and be consistent. `_streamReader.EndOfStream` is simpler.
  - **Acceptance Criteria:**
    - Correctly indicates if the end of the file has been reached for readable files.
  - **Status:** ToDo

- **Task 3.11: Implement `File.Write(string data, int? size = null)`**
  - **Description:** Write string data to a file. (LÖVE also supports `Data` object, defer for now or plan for `byte[]` overload).
  - **Implementation Details:**
    - [ ] Define `public (bool success, string? error) Write(string data, int? size = null)` in `File.cs`.
    - [ ] Check if `_isOpen` and `_currentFileMode` allows writing (Write or Append). If not, return `(false, "File not open for writing.")`.
    - [ ] Ensure `_streamWriter` is initialized for the `_stream`.
    - [ ] Determine actual data to write: if `size` is provided, take `data.Substring(0, Math.Min(size.Value, data.Length))`. Otherwise, use full `data`.
    - [ ] Use `_streamWriter.Write(stringToWrite)` or `_stream.Write(bytes, 0, bytes.Length)` if directly using stream with UTF8 encoding.
    - [ ] Handle exceptions, return `(false, e.Message)`.
  - **Acceptance Criteria:**
    - Data is written to the file as specified.
    - Handles partial writes based on `size`.
    - Returns `(true, null)` on success.
  - **Status:** ToDo

- **Task 3.12: Implement `File.Read(long? bytesToRead = null)`**
  - **Description:** Read a number of bytes from a file, returning as a string.
  - **Implementation Details:**
    - [ ] Define `public (string? contents, long bytesRead, string? error) Read(long? bytesToRead = null)` in `File.cs`.
    - [ ] Check if `_isOpen` and `_currentFileMode` allows reading. If not, return `(null, 0, "File not open for reading.")`.
    - [ ] Ensure `_streamReader` is initialized.
    - [ ] If `bytesToRead` is null, read to end: `_streamReader.ReadToEnd()`. `bytesRead` would be content length.
    - [ ] If `bytesToRead` is specified: create `char[] buffer = new char[bytesToRead.Value]`. Read using `_streamReader.Read(buffer, 0, (int)bytesToRead.Value)`.
    - [ ] Convert read chars to string. `bytesRead` is the return value of `_streamReader.Read`.
    - [ ] Handle exceptions, return `(null, 0, e.Message)`.
  - **Acceptance Criteria:**
    - Reads correct content and number of bytes.
    - Reads entire file if `bytesToRead` is null.
    - Handles requests for more bytes than available.
  - **Status:** ToDo

- **Task 3.13: Implement `File.Lines()` iterator**
  - **Description:** Iterate over all the lines in a file.
  - **Implementation Details:**
    - [ ] Define `public IEnumerable<string> Lines()` in `File.cs`.
    - [ ] Check if `_isOpen` and `_currentFileMode` allows reading. If not, `yield break` or throw.
    - [ ] Ensure `_streamReader` is initialized.
    - [ ] Reset stream position to beginning if this is intended to be re-iterable or a fresh read: `_stream.Position = 0; _streamReader.DiscardBufferedData();` (Caution: this has side effects if called mid-read elsewhere). LÖVE's iterator is typically forward-only once created.
    - [ ] Loop: `string? line; while ((line = _streamReader.ReadLine()) != null) { yield return line; }`
  - **Acceptance Criteria:**
    - Iterates lines correctly from an open readable file.
    - Works for empty files.
  - **Status:** ToDo

- **Task 3.14: Implement `File.Seek(long position)`**
  - **Description:** Seek to a position in a file.
  - **Implementation Details:**
    - [ ] Define `public bool Seek(long position)` in `File.cs`.
    - [ ] Check if `_isOpen` and `_stream.CanSeek`. If not, return `false`.
    - [ ] Use `_stream.Seek(position, System.IO.SeekOrigin.Begin)`.
    - [ ] If using `_streamReader` or `_streamWriter`, their internal buffers might need to be discarded: `_streamReader?.DiscardBufferedData(); _streamWriter?.Flush();` (Flushing writer before seek is important).
    - [ ] Return `true` on success, `false` on failure (e.g., exception).
  - **Acceptance Criteria:**
    - File position is changed correctly.
    - Returns `true` on success.
  - **Status:** ToDo

- **Task 3.15: Implement `File.Tell()`**
  - **Description:** Returns the current read/write position in the file.
  - **Implementation Details:**
    - [ ] Define `public (long position, string? error) Tell()` in `File.cs`.
    - [ ] Check if `_isOpen` and `_stream.CanSeek`. If not, return `(-1, "File not open or not seekable.")`.
    - [ ] Return `(_stream.Position, null)`.
  - **Acceptance Criteria:**
    - Returns correct current file position if open and seekable.
  - **Status:** ToDo

- **Task 3.16: Implement `File.Flush()`**
  - **Description:** Flushes any buffered written data to the disk.
  - **Implementation Details:**
    - [ ] Define `public (bool success, string? error) Flush()` in `File.cs`.
    - [ ] Check if `_isOpen`. If not, return `(false, "File not open.")`.
    - [ ] Call `_streamWriter?.Flush()` if it exists and is used.
    - [ ] Call `_stream.Flush()`.
    - [ ] Handle exceptions, return `(false, e.Message)`. On success, `(true, null)`.
  - **Acceptance Criteria:**
    - Buffered data is written to disk.
    - Returns `(true, null)` on success.
  - **Status:** ToDo

- **Task 3.17: Implement `File.SetBuffer(BufferMode mode, long size = 0)`**
  - **Description:** Sets the buffer mode for a file. .NET's `FileStream` has its own buffering. `StreamWriter`/`StreamReader` add another layer. This is an approximation.
  - **Implementation Details:**
    - [ ] Define `public (bool success, string? error) SetBuffer(BufferMode mode, long size = 0)` in `File.cs`.
    - [ ] Store `_currentBufferMode = mode` and `_bufferSize = size`.
    - [ ] This method in C# might not directly map to `FileStream`'s constructor-set buffer size after opening.
    - [ ] For `StreamWriter`: `AutoFlush` can be set. `sw.AutoFlush = (mode == BufferMode.Line)`.
    - [ ] `BufferMode.None`: Might imply `FileOptions.WriteThrough` on `FileStream` (constructor only) or frequent flushing.
    - [ ] `BufferMode.Full` with size: `StreamWriter` constructor takes a buffer size.
    - [ ] This might require re-initializing `_streamWriter` or `_streamReader` if they exist and the mode/size change affects their construction. E.g., if changing buffer size, and a `StreamWriter` is active, it would need to be recreated with the new buffer size, which implies flushing the old one first. This is complex.
    - [ ] For now, primarily store the values. If `_streamWriter` or `_streamReader` are active, update relevant properties like `AutoFlush` if possible.
    - [ ] Return `(true, null)` as it's mostly setting internal state for future operations or stream wrapper re-init.
  - **Acceptance Criteria:**
    - Buffer mode and size are stored. `StreamWriter.AutoFlush` is updated if applicable.
    - Returns `(true, null)`. (Deep behavior change testing is complex).
  - **Status:** ToDo

- **Task 3.18: Implement `File.GetBuffer()`**
  - **Description:** Gets the buffer mode and size of a file.
  - **Implementation Details:**
    - [ ] Define `public (BufferMode mode, long size) GetBuffer()` in `File.cs`.
    - [ ] Return `(_currentBufferMode, _bufferSize)`.
  - **Acceptance Criteria:**
    - Returns the currently stored buffer mode and size.
  - **Status:** ToDo

### Phase 4: Unit Testing for `Night.Filesystem.File` (@Filesystem)

- **Task 4.1: Setup Test Infrastructure for `File` Class**
  - **Description:** Create new test file for `File` class tests and update test group.
  - **Implementation Details:**
    - [ ] Create `tests/Groups/Filesystem/FileOperationsTests.cs`. This class should contain `IGame` test cases for the `File` type.
    - [ ] In `FilesystemGroup.cs` ([`tests/Groups/Filesystem/FilesystemGroup.cs`](tests/Groups/Filesystem/FilesystemGroup.cs)), add `[Fact]` methods corresponding to each `IGame` test case in `FileOperationsTests.cs`.
  - **Acceptance Criteria:**
    - Test infrastructure is ready for `File` tests.
  - **Status:** ToDo

- **Task 4.2: Test `NewFile`, `Open`, `Close`, `IsOpen`, `GetFilename`, `GetMode`**
  - **Description:** Test basic file object lifecycle and property retrieval.
  - **Implementation Details:**
    - [ ] Create `IGame` test case `File_LifecycleAndInfoTest` in `FileOperationsTests.cs`.
    - [ ] `Load()`: Define temporary file path (e.g., `Path.Combine(NightTest.Core.TestEnvironment.TempTestFilesPath, "lifecycle_test.txt")`). Ensure `TempTestFilesPath` exists and is cleaned up.
    - [ ] `Update()`:
        - `var file = Night.Filesystem.Filesystem.NewFile(tempFilePath);` Assert not null.
        - Assert `file.GetFilename() == tempFilePath`.
        - Assert `!file.IsOpen()`. Assert `file.GetMode() == null`.
        - `(ok, err) = file.Open(Night.Filesystem.FileMode.Write);` Assert `ok && err == null`. Assert `file.IsOpen()`. Assert `file.GetMode() == Night.Filesystem.FileMode.Write`.
        - Assert `file.Close()`. Assert `!file.IsOpen()`. Assert `file.GetMode() == null`.
        - Test opening in `Read` and `Append` modes similarly.
        - Test `Open` on already open file (should fail or be no-op).
        - Test `Close` on already closed file.
        - Use `try-finally` to ensure file is closed and deleted even if asserts fail.
        - Call `EndTest()`.
    - [ ] `Unload()`: Delete the temporary file if it exists.
  - **Acceptance Criteria:** Tests pass for file creation, opening in different modes, closing, and status/info retrieval methods.
  - **Status:** ToDo

- **Task 4.3: Test `Write` (string) and `Read` (string)**
  - **Description:** Test writing string data and reading it back.
  - **Implementation Details:**
    - [ ] Create `IGame` test case `File_StringWriteReadTest` in `FileOperationsTests.cs`.
    - [ ] `Load()`: `tempFilePath = ... "write_read_test.txt"`.
    - [ ] `Update()`:
        - Create file `f`. Open in `Write` mode.
        - `f.Write("Hello World");` Assert success.
        - `f.Write("
Another Line");` Assert success. `f.Close()`.
        - `f.Open(FileMode.Read);` Assert success.
        - `(contents, bytesRead, err) = f.Read();` Assert `err == null`, `contents == "Hello World
Another Line"`. `bytesRead` should match.
        - `f.Close()`.
        - Re-open, test `Read(5)` -> "Hello", `Read(100)` -> " World
Another Line".
        - Test writing to a file opened in read-only mode (should fail).
        - Call `EndTest()`.
    - [ ] `Unload()`: Delete `tempFilePath`.
  - **Acceptance Criteria:** String data is written and read correctly, including partial reads and full reads.
  - **Status:** ToDo

- **Task 4.4: Test `Lines()` Iterator**
  - **Description:** Test line-by-line iteration over file content.
  - **Implementation Details:**
    - [ ] Create `IGame` test case `File_LinesIteratorTest` in `FileOperationsTests.cs`.
    - [ ] `Load()`: `tempFilePath = ... "lines_test.txt"`. Create this file with content "Line1
Line2
Line3".
    - [ ] `Update()`:
        - `var file = Night.Filesystem.Filesystem.NewFile(tempFilePath);`
        - `file.Open(FileMode.Read);`
        - `var lines = file.Lines().ToList();` Assert `lines.Count == 3`. Assert `lines[0] == "Line1"`, etc.
        - `file.Close()`.
        - Test with empty file.
        - Test with file with mixed line endings if `System.IO.File.ReadLines` (underlying `Night.Filesystem.Lines`) and `StreamReader.ReadLine` handle them.
        - Call `EndTest()`.
    - [ ] `Unload()`: Delete `tempFilePath`.
  - **Acceptance Criteria:** `Lines()` iterator correctly yields each line from the file.
  - **Status:** ToDo

- **Task 4.5: Test `Seek`, `Tell`, `GetSize`, `IsEOF`**
  - **Description:** Test file positioning, size reporting, and end-of-file detection.
  - **Implementation Details:**
    - [ ] Create `IGame` test case `File_PositioningAndSizeTest` in `FileOperationsTests.cs`.
    - [ ] `Load()`: `tempFilePath = ... "seek_tell_test.txt"`. Write "0123456789" to it.
    - [ ] `Update()`:
        - `var file = Night.Filesystem.Filesystem.NewFile(tempFilePath); file.Open(FileMode.Read);`
        - `(size, err) = file.GetSize();` Assert `size == 10 && err == null`.
        - `(pos, err) = file.Tell();` Assert `pos == 0 && err == null`. Assert `!file.IsEOF()`.
        - `Assert.True(file.Seek(5));`
        - `(pos, err) = file.Tell();` Assert `pos == 5`.
        - `(content, bytesRead, readErr) = file.Read(2);` Assert `content == "56"`. `(pos, err) = file.Tell();` Assert `pos == 7`.
        - `file.Seek(10);` Assert `file.IsEOF()` (or `_streamReader.EndOfStream` if that's the EOF source).
        - `(content, bytesRead, readErr) = file.Read(1);` Assert `bytesRead == 0` and `content` is empty.
        - `file.Close()`. Test `GetSize()` on closed file.
        - Call `EndTest()`.
    - [ ] `Unload()`: Delete `tempFilePath`.
  - **Acceptance Criteria:** `Seek`, `Tell`, `GetSize`, and `IsEOF` behave as expected.
  - **Status:** ToDo

- **Task 4.6: Test `Flush`**
  - **Description:** Test the `Flush` method. Verifying actual disk write is hard, so test for successful call.
  - **Implementation Details:**
    - [ ] Create `IGame` test case `File_FlushTest` in `FileOperationsTests.cs`.
    - [ ] `Load()`: `tempFilePath = ... "flush_test.txt"`.
    - [ ] `Update()`:
        - `var file = Night.Filesystem.Filesystem.NewFile(tempFilePath); file.Open(FileMode.Write);`
        - `file.Write("Data to flush");`
        - `(success, err) = file.Flush();` Assert `success && err == null`.
        - `file.Close()`.
        - (Optional: re-open and verify content if feasible).
        - Call `EndTest()`.
    - [ ] `Unload()`: Delete `tempFilePath`.
  - **Acceptance Criteria:** `Flush()` executes without error and returns success.
  - **Status:** ToDo

- **Task 4.7: Test `SetBuffer` and `GetBuffer`**
  - **Description:** Test setting and getting buffer modes. Actual behavior change verification is complex.
  - **Implementation Details:**
    - [ ] Create `IGame` test case `File_BufferModeTest` in `FileOperationsTests.cs`.
    - [ ] `Load()`: `tempFilePath = ... "buffer_test.txt"`.
    - [ ] `Update()`:
        - `var file = Night.Filesystem.Filesystem.NewFile(tempFilePath); file.Open(FileMode.Write);`
        - `(success, err) = file.SetBuffer(BufferMode.Line, 1024);` Assert `success && err == null`.
        - `(mode, size) = file.GetBuffer();` Assert `mode == BufferMode.Line && size == 1024`.
        - Test `BufferMode.None` and `BufferMode.Full`.
        - `file.Close()`.
        - Call `EndTest()`.
    - [ ] `Unload()`: Delete `tempFilePath`.
  - **Acceptance Criteria:** Buffer mode and size can be set and retrieved. `StreamWriter.AutoFlush` might be testable for `Line` mode.
  - **Status:** ToDo

- **Task 4.8: Test Error Conditions and Edge Cases**
  - **Description:** Test how `File` methods handle various errors and edge cases.
  - **Implementation Details:**
    - [ ] Create `IGame` test case `File_ErrorConditionsTest` in `FileOperationsTests.cs`.
    - [ ] `Load()`: `tempFilePath = ... "error_test.txt"`.
    - [ ] `Update()`:
        - `var file = Night.Filesystem.Filesystem.NewFile(tempFilePath);`
        - Attempt `Read`, `Write`, `Seek`, `Tell`, `Lines`, `Flush` on the closed `file`. Verify expected failures (e.g., specific error messages in tuple, or specific exceptions if designed that way).
        - `file.Open(FileMode.Read);`
        - Attempt `Write` (should fail).
        - `file.Close()`.
        - `file.Open(FileMode.Write);`
        - Attempt `Read` (should fail or return nothing).
        - `file.Close()`.
        - Test `NewFile` with potentially invalid path characters (OS dependent, might be hard to test universally).
        - Test opening a non-existent file with `FileMode.Read`.
        - Call `EndTest()`.
    - [ ] `Unload()`: Delete `tempFilePath`.
  - **Acceptance Criteria:** Methods handle errors gracefully, returning appropriate error indicators or throwing documented exceptions.
  - **Status:** ToDo
