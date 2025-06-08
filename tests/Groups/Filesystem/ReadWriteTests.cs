// <copyright file="ReadWriteTests.cs" company="Night Circle">
// zlib license
//
// Copyright (c) 2025 Danny Solivan, Night Circle
//
// This software is provided 'as-is', without any express or implied
// warranty. In no event will the authors be held liable for any damages
// arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software. If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Text;

using Night;

using NightTest.Core;

namespace NightTest.Groups.Filesystem
{
  /// <summary>
  /// Tests for Night.Filesystem.ReadBytes().
  /// </summary>
  public class FilesystemReadBytes_ReadExistingFileTest : GameTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_readbytes_file.bin");
    private readonly byte[] expectedContent = { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x4E, 0x69, 0x67, 0x68, 0x74 }; // "Hello Night"

    /// <inheritdoc/>
    public override string Name => "Filesystem.ReadBytes.ReadExistingFile";

    /// <inheritdoc/>
    public override string Description => "Tests ReadBytes for an existing binary file.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      try
      {
        File.WriteAllBytes(this.testFileName, this.expectedContent);
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Setup failed: Could not create test file '{this.testFileName}'. {e.Message}";
        this.EndTest();
      }
    }

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      try
      {
        if (this.CurrentStatus == TestStatus.Failed)
        {
          return;
        }

        byte[] actualContent = Night.Filesystem.ReadBytes(this.testFileName);

        if (actualContent != null && actualContent.SequenceEqual(this.expectedContent))
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully read bytes from an existing file.";
        }
        else
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = "ReadBytes content did not match expected content or was null.";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          try
          {
            File.Delete(this.testFileName);
          }
          catch (Exception ex)
          {
            this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
          }
        }

        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests for Night.Filesystem.ReadText().
  /// </summary>
  public class FilesystemReadText_ReadExistingFileTest : GameTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_readtext_file.txt");
    private readonly string expectedContent = "Hello Night Text!";

    /// <inheritdoc/>
    public override string Name => "Filesystem.ReadText.ReadExistingFile";

    /// <inheritdoc/>
    public override string Description => "Tests ReadText for an existing text file.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      try
      {
        File.WriteAllText(this.testFileName, this.expectedContent);
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Setup failed: Could not create test file '{this.testFileName}'. {e.Message}";
        this.EndTest();
      }
    }

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      try
      {
        if (this.CurrentStatus == TestStatus.Failed)
        {
          return;
        }

        string actualContent = Night.Filesystem.ReadText(this.testFileName);

        if (actualContent == this.expectedContent)
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully read text from an existing file.";
        }
        else
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = $"ReadText content did not match. Expected: '{this.expectedContent}', Got: '{actualContent}'.";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          try
          {
            File.Delete(this.testFileName);
          }
          catch (Exception ex)
          {
            this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
          }
        }

        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests Night.Filesystem.ReadBytes for a non-existent file.
  /// </summary>
  public class FilesystemReadBytes_FileNotFoundTest : GameTestCase
  {
    private readonly string nonExistentFile = Path.Combine(Path.GetTempPath(), "night_test_readbytes_nonexistent.bin");

    /// <inheritdoc/>
    public override string Name => "Filesystem.ReadBytes.FileNotFound";

    /// <inheritdoc/>
    public override string Description => "Tests ReadBytes throws FileNotFoundException for a non-existent file.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      if (File.Exists(this.nonExistentFile))
      {
        File.Delete(this.nonExistentFile); // Ensure it doesn't exist
      }
    }

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      try
      {
        _ = Night.Filesystem.ReadBytes(this.nonExistentFile);
        this.CurrentStatus = TestStatus.Failed;
        this.Details = "FileNotFoundException was not thrown for ReadBytes.";
      }
      catch (FileNotFoundException)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = "Successfully caught FileNotFoundException for ReadBytes.";
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected exception occurred: {e.GetType().Name} - {e.Message}";
      }
      finally
      {
        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests Night.Filesystem.ReadText for a non-existent file.
  /// </summary>
  public class FilesystemReadText_FileNotFoundTest : GameTestCase
  {
    private readonly string nonExistentFile = Path.Combine(Path.GetTempPath(), "night_test_readtext_nonexistent.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.ReadText.FileNotFound";

    /// <inheritdoc/>
    public override string Description => "Tests ReadText throws FileNotFoundException for a non-existent file.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      if (File.Exists(this.nonExistentFile))
      {
        File.Delete(this.nonExistentFile); // Ensure it doesn't exist
      }
    }

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      try
      {
        _ = Night.Filesystem.ReadText(this.nonExistentFile);
        this.CurrentStatus = TestStatus.Failed;
        this.Details = "FileNotFoundException was not thrown for ReadText.";
      }
      catch (FileNotFoundException)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = "Successfully caught FileNotFoundException for ReadText.";
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected exception occurred: {e.GetType().Name} - {e.Message}";
      }
      finally
      {
        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests for Night.Filesystem.Append().
  /// </summary>
  public class FilesystemAppend_AppendToNewFileTest : GameTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_append_new.txt");
    private readonly byte[] dataToAppend = Encoding.UTF8.GetBytes("First line.");

    /// <inheritdoc/>
    public override string Name => "Filesystem.Append.NewFile";

    /// <inheritdoc/>
    public override string Description => "Tests Append to a new file.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      if (File.Exists(this.testFileName))
      {
        File.Delete(this.testFileName);
      }
    }

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      try
      {
        Night.Filesystem.Append(this.testFileName, this.dataToAppend);
        byte[] fileContent = File.ReadAllBytes(this.testFileName);

        if (fileContent.SequenceEqual(this.dataToAppend))
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully appended data to a new file.";
        }
        else
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = "Appended content did not match expected content.";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          try
          {
            File.Delete(this.testFileName);
          }
          catch (Exception ex)
          {
            this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
          }
        }

        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests appending data to an existing file.
  /// </summary>
  public class FilesystemAppend_AppendToExistingFileTest : GameTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_append_existing.txt");
    private readonly byte[] initialData = Encoding.UTF8.GetBytes("Initial content. ");
    private readonly byte[] dataToAppend = Encoding.UTF8.GetBytes("Appended data.");
    private byte[] expectedData = Array.Empty<byte>();

    /// <inheritdoc/>
    public override string Name => "Filesystem.Append.ExistingFile";

    /// <inheritdoc/>
    public override string Description => "Tests Append to an existing file.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      try
      {
        File.WriteAllBytes(this.testFileName, this.initialData);
        this.expectedData = this.initialData.Concat(this.dataToAppend).ToArray();
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Setup failed: Could not create initial test file '{this.testFileName}'. {e.Message}";
        this.EndTest();
      }
    }

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      try
      {
        if (this.CurrentStatus == TestStatus.Failed)
        {
          return;
        }

        Night.Filesystem.Append(this.testFileName, this.dataToAppend);
        byte[] fileContent = File.ReadAllBytes(this.testFileName);

        if (fileContent.SequenceEqual(this.expectedData))
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully appended data to an existing file.";
        }
        else
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = $"Appended content did not match. Expected: [{string.Join(", ", this.expectedData)}], Got: [{string.Join(", ", fileContent)}]";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          try
          {
            File.Delete(this.testFileName);
          }
          catch (Exception ex)
          {
            this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
          }
        }

        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests appending a partial amount of data using the size parameter.
  /// </summary>
  public class FilesystemAppend_PartialDataTest : GameTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_append_partial.txt");
    private readonly byte[] fullData = Encoding.UTF8.GetBytes("FullDataString");
    private readonly long sizeToAppend = 5; // "FullD"
    private byte[] expectedData = Array.Empty<byte>();

    /// <inheritdoc/>
    public override string Name => "Filesystem.Append.PartialData";

    /// <inheritdoc/>
    public override string Description => "Tests Append with a specific size to append only part of the data.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      if (File.Exists(this.testFileName))
      {
        File.Delete(this.testFileName);
      }

      this.expectedData = new byte[this.sizeToAppend];
      Array.Copy(this.fullData, this.expectedData, this.sizeToAppend);
    }

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      try
      {
        Night.Filesystem.Append(this.testFileName, this.fullData, this.sizeToAppend);
        byte[] fileContent = File.ReadAllBytes(this.testFileName);

        if (fileContent.SequenceEqual(this.expectedData))
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully appended partial data using the size parameter.";
        }
        else
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = "Partially appended content did not match expected content.";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          try
          {
            File.Delete(this.testFileName);
          }
          catch (Exception ex)
          {
            this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
          }
        }

        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests argument validation for Filesystem.Append.
  /// </summary>
  public class FilesystemAppend_ArgumentValidationTest : GameTestCase
  {
    private readonly string validFileName = Path.Combine(Path.GetTempPath(), "night_test_append_validation.txt");
    private readonly byte[] validData = { 1, 2, 3 };

    /// <inheritdoc/>
    public override string Name => "Filesystem.Append.ArgumentValidation";

    /// <inheritdoc/>
    public override string Description => "Tests argument validation for Filesystem.Append (null filename, null data, empty filename).";

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      int checksPassed = 0;
      string failureDetails = string.Empty;

      try
      {
        // Test null filename
        try
        {
          Night.Filesystem.Append(null!, this.validData);
          failureDetails += "ArgumentNullException not thrown for null filename. ";
        }
        catch (ArgumentNullException)
        {
          checksPassed++;
        }
        catch (Exception e)
        {
          failureDetails += $"Unexpected exception for null filename: {e.GetType().Name}. ";
        }

        // Test null data
        try
        {
          Night.Filesystem.Append(this.validFileName, null!);
          failureDetails += "ArgumentNullException not thrown for null data. ";
        }
        catch (ArgumentNullException)
        {
          checksPassed++;
        }
        catch (Exception e)
        {
          failureDetails += $"Unexpected exception for null data: {e.GetType().Name}. ";
        }

        // Test empty filename
        try
        {
          Night.Filesystem.Append(string.Empty, this.validData);
          failureDetails += "ArgumentException not thrown for empty filename. ";
        }
        catch (ArgumentException)
        {
          checksPassed++;
        }
        catch (Exception e)
        {
          failureDetails += $"Unexpected exception for empty filename: {e.GetType().Name}. ";
        }

        if (checksPassed == 3)
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully validated arguments for Append.";
        }
        else
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = $"Argument validation failed. Checks passed: {checksPassed}/3. Details: {failureDetails.Trim()}";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred during validation tests: {e.Message}";
      }
      finally
      {
        // Clean up the valid file name if it was created by a failed test part
        if (File.Exists(this.validFileName))
        {
          try
          {
            File.Delete(this.validFileName);
          }
          catch
          { /* best effort */
          }
        }

        this.EndTest();
      }
    }
  }
}

/// <summary>
/// Tests for Night.Filesystem.Read(name, sizeToRead) - string overload.
/// </summary>
public class FilesystemRead_String_ReadExistingFileTest : GameTestCase
{
  private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_read_string_existing.txt");
  private readonly string expectedContent = "Hello Night from Read String Test!";

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.String.ReadExistingFile";

  /// <inheritdoc/>
  public override string Description => "Tests Read(name, size) for an existing text file, returning string.";

  /// <inheritdoc/>
  protected override void Load()
  {
    base.Load();
    try
    {
      File.WriteAllText(this.testFileName, this.expectedContent, new UTF8Encoding(false));
    }
    catch (Exception e)
    {
      this.RecordFailure($"Setup failed: Could not create test file '{this.testFileName}'. {e.Message}", e);
    }
  }

  /// <inheritdoc/>
  protected override void Update(double deltaTime)
  {
    if (this.IsDone)
    {
      return;
    }

    if (this.CurrentStatus == TestStatus.Failed)
    {
      this.EndTest();
      return;
    }

    try
    {
      (string? actualContent, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(this.testFileName);

      if (errorMsg != null)
      {
        this.RecordFailure($"Read failed with error: {errorMsg}");
      }
      else if (actualContent == this.expectedContent)
      {
        if (bytesRead == Encoding.UTF8.GetByteCount(this.expectedContent))
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully read string content and byte count matches.";
        }
        else
        {
          this.RecordFailure($"Content matches, but byte count is incorrect. Expected: {Encoding.UTF8.GetByteCount(this.expectedContent)}, Got: {bytesRead}.");
        }
      }
      else
      {
        this.RecordFailure($"Read content did not match. Expected (len {this.expectedContent.Length}): '{this.expectedContent}' (Bytes: [{string.Join(", ", Encoding.UTF8.GetBytes(this.expectedContent))}]), Got (len {actualContent?.Length ?? -1}): '{actualContent}' (Bytes: [{string.Join(", ", actualContent == null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(actualContent))}]).");
      }
    }
    catch (Exception e)
    {
      this.RecordFailure($"An unexpected error occurred: {e.Message}", e);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        try
        {
          File.Delete(this.testFileName);
        }
        catch (Exception ex)
        {
          this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
        }
      }

      this.EndTest();
    }
  }
}

/// <summary>
/// Tests for Night.Filesystem.Read(ContainerType.Data, name, sizeToRead).
/// </summary>
public class FilesystemRead_Data_ReadExistingFileTest : GameTestCase
{
  private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_read_data_existing.bin");
  private readonly byte[] expectedContent = { 0x01, 0x02, 0x03, 0xAA, 0xBB, 0xCC };

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.Data.ReadExistingFile";

  /// <inheritdoc/>
  public override string Description => "Tests Read(ContainerType.Data, name, size) for an existing binary file.";

  /// <inheritdoc/>
  protected override void Load()
  {
    base.Load();
    try
    {
      File.WriteAllBytes(this.testFileName, this.expectedContent);
    }
    catch (Exception e)
    {
      this.RecordFailure($"Setup failed: Could not create test file '{this.testFileName}'. {e.Message}", e);
    }
  }

  /// <inheritdoc/>
  protected override void Update(double deltaTime)
  {
    if (this.IsDone)
    {
      return;
    }

    if (this.CurrentStatus == TestStatus.Failed)
    {
      this.EndTest();
      return;
    }

    try
    {
      (object? actualContentObj, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(Night.Filesystem.ContainerType.Data, this.testFileName);

      if (errorMsg != null)
      {
        this.RecordFailure($"Read failed with error: {errorMsg}");
      }
      else if (actualContentObj is byte[] actualContent && actualContent.SequenceEqual(this.expectedContent))
      {
        if (bytesRead == this.expectedContent.Length)
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully read byte[] content and byte count matches.";
        }
        else
        {
          this.RecordFailure($"Content matches, but byte count is incorrect. Expected: {this.expectedContent.Length}, Got: {bytesRead}.");
        }
      }
      else
      {
        this.RecordFailure($"Read byte[] content did not match or was not byte[]. Expected: [{string.Join(", ", this.expectedContent)}], Got: [{(actualContentObj is byte[] ac ? string.Join(", ", ac) : "Not byte[]")}]");
      }
    }
    catch (Exception e)
    {
      this.RecordFailure($"An unexpected error occurred: {e.Message}", e);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        try
        {
          File.Delete(this.testFileName);
        }
        catch (Exception ex)
        {
          this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
        }
      }

      this.EndTest();
    }
  }
}

/// <summary>
/// Tests Night.Filesystem.Read for a non-existent file.
/// </summary>
public class FilesystemRead_FileNotFoundTest : GameTestCase
{
  private readonly string nonExistentFile = Path.Combine(Path.GetTempPath(), "night_test_read_nonexistent.txt");

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.FileNotFound";

  /// <inheritdoc/>
  public override string Description => "Tests Read returns (null, null, 'File not found.') for a non-existent file.";

  /// <inheritdoc/>
  protected override void Load()
  {
    base.Load();
    if (File.Exists(this.nonExistentFile))
    {
      try
      {
        File.Delete(this.nonExistentFile);
      } // Ensure it doesn't exist
      catch (Exception e)
      {
        this.RecordFailure($"Setup: Could not delete pre-existing test file '{this.nonExistentFile}'. {e.Message}", e);
      }
    }
  }

  /// <inheritdoc/>
  protected override void Update(double deltaTime)
  {
    if (this.IsDone)
    {
      return;
    }

    if (this.CurrentStatus == TestStatus.Failed)
    {
      this.EndTest();
      return;
    }

    try
    {
      (string? contents, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(this.nonExistentFile);

      if (contents == null && bytesRead == null && errorMsg == "File not found.")
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = "Correctly returned (null, null, 'File not found.') for non-existent file.";
      }
      else
      {
        this.RecordFailure($"Expected (null, null, 'File not found.'), but got ({(contents == null ? "null" : $"'{contents}'")}, {(bytesRead.HasValue ? bytesRead.Value.ToString() : "null")}, {(errorMsg == null ? "null" : $"'{errorMsg}'")}).");
      }
    }
    catch (Exception e)
    {
      this.RecordFailure($"An unexpected exception occurred: {e.Message}", e);
    }
    finally
    {
      this.EndTest();
    }
  }
}

/// <summary>
/// Tests Night.Filesystem.Read for partial string read.
/// </summary>
public class FilesystemRead_String_PartialReadTest : GameTestCase
{
  private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_read_string_partial.txt");
  private readonly string fullContent = "This is the full content.";
  private readonly string expectedContent = "This is"; // 7 bytes
  private readonly long bytesToRead = 7;

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.String.PartialRead";

  /// <inheritdoc/>
  public override string Description => "Tests Read(name, size) for a partial read, returning string.";

  /// <inheritdoc/>
  protected override void Load()
  {
    base.Load();
    try
    {
      File.WriteAllText(this.testFileName, this.fullContent, new UTF8Encoding(false));
    }
    catch (Exception e)
    {
      this.RecordFailure($"Setup failed: Could not create test file '{this.testFileName}'. {e.Message}", e);
    }
  }

  /// <inheritdoc/>
  protected override void Update(double deltaTime)
  {
    if (this.IsDone)
    {
      return;
    }

    if (this.CurrentStatus == TestStatus.Failed)
    {
      this.EndTest();
      return;
    }

    try
    {
      // First, read as data to inspect the bytes
      (object? dataObj, long? dataBytesRead, string? dataErrorMsg) = Night.Filesystem.Read(Night.Filesystem.ContainerType.Data, this.testFileName, this.bytesToRead);
      string dataBytesString = "null";
      if (dataObj is byte[] dataBuffer)
      {
        dataBytesString = $"[{string.Join(", ", dataBuffer)}]";
      }

      (string? actualContent, long? actualBytesRead, string? errorMsg) = Night.Filesystem.Read(this.testFileName, this.bytesToRead);

      if (errorMsg != null)
      {
        this.RecordFailure($"Read failed with error: {errorMsg}. Data read attempt: Error='{dataErrorMsg}', BytesRead='{dataBytesRead}', Bytes='{dataBytesString}'.");
      }
      else if (actualContent == this.expectedContent && actualBytesRead == this.bytesToRead)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = $"Successfully read partial string content and byte count matches. Data bytes read: {dataBytesString}";
      }
      else
      {
        this.RecordFailure($"Partial read content or byte count mismatch. Expected Content: '{this.expectedContent}', Got: '{actualContent}'. Expected Bytes: {this.bytesToRead}, Got: {actualBytesRead}. Data read attempt: Error='{dataErrorMsg}', BytesRead='{dataBytesRead}', Bytes='{dataBytesString}'.");
      }
    }
    catch (Exception e)
    {
      this.RecordFailure($"An unexpected error occurred: {e.Message}", e);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        try
        {
          File.Delete(this.testFileName);
        }
        catch (Exception ex)
        {
          this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
        }
      }

      this.EndTest();
    }
  }
}

/// <summary>
/// Tests Night.Filesystem.Read for partial data read.
/// </summary>
public class FilesystemRead_Data_PartialReadTest : GameTestCase
{
  private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_read_data_partial.bin");
  private readonly byte[] fullContent = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
  private readonly byte[] expectedContent = { 0x01, 0x02, 0x03, 0x04 };
  private readonly long bytesToRead = 4;

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.Data.PartialRead";

  /// <inheritdoc/>
  public override string Description => "Tests Read(ContainerType.Data, name, size) for a partial read.";

  /// <inheritdoc/>
  protected override void Load()
  {
    base.Load();
    try
    {
      File.WriteAllBytes(this.testFileName, this.fullContent);
    }
    catch (Exception e)
    {
      this.RecordFailure($"Setup failed: Could not create test file '{this.testFileName}'. {e.Message}", e);
    }
  }

  /// <inheritdoc/>
  protected override void Update(double deltaTime)
  {
    if (this.IsDone)
    {
      return;
    }

    if (this.CurrentStatus == TestStatus.Failed)
    {
      this.EndTest();
      return;
    }

    try
    {
      (object? actualContentObj, long? actualBytesRead, string? errorMsg) = Night.Filesystem.Read(Night.Filesystem.ContainerType.Data, this.testFileName, this.bytesToRead);

      if (errorMsg != null)
      {
        this.RecordFailure($"Read failed with error: {errorMsg}");
      }
      else if (actualContentObj is byte[] actualContent && actualContent.SequenceEqual(this.expectedContent) && actualBytesRead == this.bytesToRead)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = "Successfully read partial byte[] content and byte count matches.";
      }
      else
      {
        this.RecordFailure($"Partial read byte[] content or byte count mismatch. Expected Content: [{string.Join(", ", this.expectedContent)}], Got: [{(actualContentObj is byte[] ac ? string.Join(", ", ac) : "Not byte[]")}]. Expected Bytes: {this.bytesToRead}, Got: {actualBytesRead}.");
      }
    }
    catch (Exception e)
    {
      this.RecordFailure($"An unexpected error occurred: {e.Message}", e);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        try
        {
          File.Delete(this.testFileName);
        }
        catch (Exception ex)
        {
          this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
        }
      }

      this.EndTest();
    }
  }
}

/// <summary>
/// Tests Night.Filesystem.Read for an empty file.
/// </summary>
public class FilesystemRead_EmptyFileTest : GameTestCase
{
  private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_read_empty.txt");

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.EmptyFile";

  /// <inheritdoc/>
  public override string Description => "Tests Read for an empty file, expecting empty string/data and 0 bytes read.";

  /// <inheritdoc/>
  protected override void Load()
  {
    base.Load();
    try
    {
      File.WriteAllBytes(this.testFileName, Array.Empty<byte>()); // Create empty file
    }
    catch (Exception e)
    {
      this.RecordFailure($"Setup failed: Could not create empty test file '{this.testFileName}'. {e.Message}", e);
    }
  }

  /// <inheritdoc/>
  protected override void Update(double deltaTime)
  {
    if (this.IsDone)
    {
      return;
    }

    if (this.CurrentStatus == TestStatus.Failed)
    {
      this.EndTest();
      return;
    }

    try
    {
      // Test as string
      (string? strContents, long? strBytesRead, string? strErrorMsg) = Night.Filesystem.Read(this.testFileName);
      bool stringTestPassed = strErrorMsg == null && strContents == string.Empty && strBytesRead == 0;

      // Test as data
      (object? dataContentsObj, long? dataBytesRead, string? dataErrorMsg) = Night.Filesystem.Read(Night.Filesystem.ContainerType.Data, this.testFileName);
      bool dataTestPassed = dataErrorMsg == null && dataContentsObj is byte[] dataContents && dataContents.Length == 0 && dataBytesRead == 0;

      if (stringTestPassed && dataTestPassed)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = "Successfully read empty file as string and data (empty content, 0 bytes read).";
      }
      else
      {
        string failureDetails = string.Empty;
        if (!stringTestPassed)
        {
          failureDetails += $"String test failed: Content='{strContents}', Bytes={strBytesRead}, Error='{strErrorMsg}'. ";
        }

        if (!dataTestPassed)
        {
          failureDetails += $"Data test failed: Content is {dataContentsObj?.GetType().Name ?? "null"}, Bytes={dataBytesRead}, Error='{dataErrorMsg}'.";
        }

        this.RecordFailure(failureDetails.Trim());
      }
    }
    catch (Exception e)
    {
      this.RecordFailure($"An unexpected error occurred: {e.Message}", e);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        try
        {
          File.Delete(this.testFileName);
        }
        catch (Exception ex)
        {
          this.Details += $" | Warning: Failed to delete test file '{this.testFileName}': {ex.Message}";
        }
      }

      this.EndTest();
    }
  }
}

/// <summary>
/// Tests Night.Filesystem.Read with invalid arguments.
/// </summary>
public class FilesystemRead_ArgumentValidationTest : GameTestCase
{
  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.ArgumentValidation";

  /// <inheritdoc/>
  public override string Description => "Tests Read with various invalid arguments (null/empty filename, negative size).";

  /// <inheritdoc/>
  protected override void Update(double deltaTime)
  {
    if (this.IsDone)
    {
      return;
    }

    try
    {
      bool nullNameTest = false;
      (_, _, string? errorMsgNull) = Night.Filesystem.Read(null!);
      if (errorMsgNull == "File name cannot be null or empty.")
      {
        nullNameTest = true;
      }

      bool emptyNameTest = false;
      (_, _, string? errorMsgEmpty) = Night.Filesystem.Read(string.Empty);
      if (errorMsgEmpty == "File name cannot be null or empty.")
      {
        emptyNameTest = true;
      }

      // Need a dummy file for negative size test, as it checks existence first
      string dummyFile = Path.Combine(Path.GetTempPath(), "night_test_read_dummy_arg.txt");
      File.WriteAllText(dummyFile, "dummy");
      bool negativeSizeTest = false;
      (_, _, string? errorMsgNegative) = Night.Filesystem.Read(dummyFile, -5);
      if (errorMsgNegative == "Size to read cannot be negative.")
      {
        negativeSizeTest = true;
      }

      if (File.Exists(dummyFile))
      {
        File.Delete(dummyFile);
      }

      bool zeroSizeTest = false;
      File.WriteAllText(dummyFile, "content");
      (string? zeroContent, long? zeroBytes, string? zeroError) = Night.Filesystem.Read(dummyFile, 0);
      if (zeroError == null && zeroContent == string.Empty && zeroBytes == 0)
      {
        zeroSizeTest = true;
      }

      if (File.Exists(dummyFile))
      {
        File.Delete(dummyFile);
      }

      if (nullNameTest && emptyNameTest && negativeSizeTest && zeroSizeTest)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = "All argument validation tests passed for Read.";
      }
      else
      {
        this.RecordFailure($"Argument validation failed. NullName: {nullNameTest}, EmptyName: {emptyNameTest}, NegativeSize: {negativeSizeTest}, ZeroSize: {zeroSizeTest}. Errors: Null='{errorMsgNull}', Empty='{errorMsgEmpty}', Negative='{errorMsgNegative}', Zero='{zeroError}'");
      }
    }
    catch (Exception e)
    {
      this.RecordFailure($"An unexpected error occurred: {e.Message}", e);
    }
    finally
    {
      this.EndTest();
    }
  }
}
