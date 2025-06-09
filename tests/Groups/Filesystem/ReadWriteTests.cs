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

using Xunit;

namespace NightTest.Groups.Filesystem
{
  /// <summary>
  /// Tests for Night.Filesystem.ReadBytes().
  /// </summary>
  public class FilesystemReadBytes_ReadExistingFileTest : ModTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_readbytes_file.bin");
    private readonly byte[] expectedContent = { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x4E, 0x69, 0x67, 0x68, 0x74 }; // "Hello Night"

    /// <inheritdoc/>
    public override string Name => "Filesystem.ReadBytes.ReadExistingFile";

    /// <inheritdoc/>
    public override string Description => "Tests ReadBytes for an existing binary file.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully read bytes from an existing file.";

    /// <inheritdoc/>
    public override void Run()
    {
      try
      {
        File.WriteAllBytes(this.testFileName, this.expectedContent);
        byte[] actualContent = Night.Filesystem.ReadBytes(this.testFileName);
        Assert.Equal(this.expectedContent, actualContent);
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          File.Delete(this.testFileName);
        }
      }
    }
  }

  /// <summary>
  /// Tests for Night.Filesystem.ReadText().
  /// </summary>
  public class FilesystemReadText_ReadExistingFileTest : ModTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_readtext_file.txt");
    private readonly string expectedContent = "Hello Night Text!";

    /// <inheritdoc/>
    public override string Name => "Filesystem.ReadText.ReadExistingFile";

    /// <inheritdoc/>
    public override string Description => "Tests ReadText for an existing text file.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully read text from an existing file.";

    /// <inheritdoc/>
    public override void Run()
    {
      try
      {
        File.WriteAllText(this.testFileName, this.expectedContent);
        string actualContent = Night.Filesystem.ReadText(this.testFileName);
        Assert.Equal(this.expectedContent, actualContent);
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          File.Delete(this.testFileName);
        }
      }
    }
  }

  /// <summary>
  /// Tests Night.Filesystem.ReadBytes for a non-existent file.
  /// </summary>
  public class FilesystemReadBytes_FileNotFoundTest : ModTestCase
  {
    private readonly string nonExistentFile = Path.Combine(Path.GetTempPath(), "night_test_readbytes_nonexistent.bin");

    /// <inheritdoc/>
    public override string Name => "Filesystem.ReadBytes.FileNotFound";

    /// <inheritdoc/>
    public override string Description => "Tests ReadBytes throws FileNotFoundException for a non-existent file.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully caught FileNotFoundException for ReadBytes.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (File.Exists(this.nonExistentFile))
      {
        File.Delete(this.nonExistentFile); // Ensure it doesn't exist
      }

      _ = Assert.Throws<FileNotFoundException>(() => Night.Filesystem.ReadBytes(this.nonExistentFile));
    }
  }

  /// <summary>
  /// Tests Night.Filesystem.ReadText for a non-existent file.
  /// </summary>
  public class FilesystemReadText_FileNotFoundTest : ModTestCase
  {
    private readonly string nonExistentFile = Path.Combine(Path.GetTempPath(), "night_test_readtext_nonexistent.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.ReadText.FileNotFound";

    /// <inheritdoc/>
    public override string Description => "Tests ReadText throws FileNotFoundException for a non-existent file.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully caught FileNotFoundException for ReadText.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (File.Exists(this.nonExistentFile))
      {
        File.Delete(this.nonExistentFile); // Ensure it doesn't exist
      }

      _ = Assert.Throws<FileNotFoundException>(() => Night.Filesystem.ReadText(this.nonExistentFile));
    }
  }

  /// <summary>
  /// Tests for Night.Filesystem.Append().
  /// </summary>
  public class FilesystemAppend_AppendToNewFileTest : ModTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_append_new.txt");
    private readonly byte[] dataToAppend = Encoding.UTF8.GetBytes("First line.");

    /// <inheritdoc/>
    public override string Name => "Filesystem.Append.NewFile";

    /// <inheritdoc/>
    public override string Description => "Tests Append to a new file.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully appended data to a new file.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (File.Exists(this.testFileName))
      {
        File.Delete(this.testFileName);
      }

      try
      {
        Night.Filesystem.Append(this.testFileName, this.dataToAppend);
        byte[] fileContent = File.ReadAllBytes(this.testFileName);
        Assert.Equal(this.dataToAppend, fileContent);
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          File.Delete(this.testFileName);
        }
      }
    }
  }

  /// <summary>
  /// Tests appending data to an existing file.
  /// </summary>
  public class FilesystemAppend_AppendToExistingFileTest : ModTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_append_existing.txt");
    private readonly byte[] initialData = Encoding.UTF8.GetBytes("Initial content. ");
    private readonly byte[] dataToAppend = Encoding.UTF8.GetBytes("Appended data.");

    /// <inheritdoc/>
    public override string Name => "Filesystem.Append.ExistingFile";

    /// <inheritdoc/>
    public override string Description => "Tests Append to an existing file.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully appended data to an existing file.";

    /// <inheritdoc/>
    public override void Run()
    {
      try
      {
        File.WriteAllBytes(this.testFileName, this.initialData);
        byte[] expectedData = this.initialData.Concat(this.dataToAppend).ToArray();

        Night.Filesystem.Append(this.testFileName, this.dataToAppend);
        byte[] fileContent = File.ReadAllBytes(this.testFileName);

        Assert.Equal(expectedData, fileContent);
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          File.Delete(this.testFileName);
        }
      }
    }
  }

  /// <summary>
  /// Tests appending a partial amount of data using the size parameter.
  /// </summary>
  public class FilesystemAppend_PartialDataTest : ModTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_append_partial.txt");
    private readonly byte[] fullData = Encoding.UTF8.GetBytes("FullDataString");
    private readonly long sizeToAppend = 5; // "FullD"

    /// <inheritdoc/>
    public override string Name => "Filesystem.Append.PartialData";

    /// <inheritdoc/>
    public override string Description => "Tests Append with a specific size to append only part of the data.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully appended partial data using the size parameter.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (File.Exists(this.testFileName))
      {
        File.Delete(this.testFileName);
      }

      byte[] expectedData = new byte[this.sizeToAppend];
      Array.Copy(this.fullData, expectedData, this.sizeToAppend);

      try
      {
        Night.Filesystem.Append(this.testFileName, this.fullData, this.sizeToAppend);
        byte[] fileContent = File.ReadAllBytes(this.testFileName);
        Assert.Equal(expectedData, fileContent);
      }
      finally
      {
        if (File.Exists(this.testFileName))
        {
          File.Delete(this.testFileName);
        }
      }
    }
  }

  /// <summary>
  /// Tests argument validation for Filesystem.Append.
  /// </summary>
  public class FilesystemAppend_ArgumentValidationTest : ModTestCase
  {
    private readonly string validFileName = Path.Combine(Path.GetTempPath(), "night_test_append_validation.txt");
    private readonly byte[] validData = { 1, 2, 3 };

    /// <inheritdoc/>
    public override string Name => "Filesystem.Append.ArgumentValidation";

    /// <inheritdoc/>
    public override string Description => "Tests argument validation for Filesystem.Append (null filename, null data, empty filename).";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully validated arguments for Append.";

    /// <inheritdoc/>
    public override void Run()
    {
      try
      {
        // Test null filename
        _ = Assert.Throws<ArgumentNullException>(() => Night.Filesystem.Append(null!, this.validData));

        // Test null data
        _ = Assert.Throws<ArgumentNullException>(() => Night.Filesystem.Append(this.validFileName, null!));

        // Test empty filename
        _ = Assert.Throws<ArgumentException>(() => Night.Filesystem.Append(string.Empty, this.validData));
      }
      finally
      {
        // Clean up the valid file name if it was created by a failed test part
        if (File.Exists(this.validFileName))
        {
          File.Delete(this.validFileName);
        }
      }
    }
  }
}

/// <summary>
/// Tests for Night.Filesystem.Read(name, sizeToRead) - string overload.
/// </summary>
public class FilesystemRead_String_ReadExistingFileTest : ModTestCase
{
  private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_read_string_existing.txt");
  private readonly string expectedContent = "Hello Night from Read String Test!";

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.String.ReadExistingFile";

  /// <inheritdoc/>
  public override string Description => "Tests Read(name, size) for an existing text file, returning string.";

  /// <inheritdoc/>
  public override string SuccessMessage => "Successfully read string content and byte count matches.";

  /// <inheritdoc/>
  public override void Run()
  {
    try
    {
      File.WriteAllText(this.testFileName, this.expectedContent, new UTF8Encoding(false));

      (string? actualContent, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(this.testFileName);

      Assert.Null(errorMsg);
      Assert.Equal(this.expectedContent, actualContent);
      Assert.Equal(Encoding.UTF8.GetByteCount(this.expectedContent), bytesRead);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        File.Delete(this.testFileName);
      }
    }
  }
}

/// <summary>
/// Tests for Night.Filesystem.Read(ContainerType.Data, name, sizeToRead).
/// </summary>
public class FilesystemRead_Data_ReadExistingFileTest : ModTestCase
{
  private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_read_data_existing.bin");
  private readonly byte[] expectedContent = { 0x01, 0x02, 0x03, 0xAA, 0xBB, 0xCC };

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.Data.ReadExistingFile";

  /// <inheritdoc/>
  public override string Description => "Tests Read(ContainerType.Data, name, size) for an existing binary file.";

  /// <inheritdoc/>
  public override string SuccessMessage => "Successfully read byte[] content and byte count matches.";

  /// <inheritdoc/>
  public override void Run()
  {
    try
    {
      File.WriteAllBytes(this.testFileName, this.expectedContent);
      (object? actualContentObj, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(Night.Filesystem.ContainerType.Data, this.testFileName);

      Assert.Null(errorMsg);
      byte[]? actualContent = Assert.IsType<byte[]>(actualContentObj);
      Assert.Equal(this.expectedContent, actualContent);
      Assert.Equal(this.expectedContent.Length, bytesRead);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        File.Delete(this.testFileName);
      }
    }
  }
}

/// <summary>
/// Tests Night.Filesystem.Read for a non-existent file.
/// </summary>
public class FilesystemRead_FileNotFoundTest : ModTestCase
{
  private readonly string nonExistentFile = Path.Combine(Path.GetTempPath(), "night_test_read_nonexistent.txt");

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.FileNotFound";

  /// <inheritdoc/>
  public override string Description => "Tests Read returns (null, null, 'File not found.') for a non-existent file.";

  /// <inheritdoc/>
  public override string SuccessMessage => "Correctly returned (null, null, 'File not found.') for non-existent file.";

  /// <inheritdoc/>
  public override void Run()
  {
    if (File.Exists(this.nonExistentFile))
    {
      File.Delete(this.nonExistentFile); // Ensure it doesn't exist
    }

    (string? contents, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(this.nonExistentFile);

    Assert.Null(contents);
    Assert.Null(bytesRead);
    Assert.Equal("File not found.", errorMsg);
  }
}

/// <summary>
/// Tests Night.Filesystem.Read for partial string read.
/// </summary>
public class FilesystemRead_String_PartialReadTest : ModTestCase
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
  public override string SuccessMessage => "Successfully read partial string content.";

  /// <inheritdoc/>
  public override void Run()
  {
    try
    {
      File.WriteAllText(this.testFileName, this.fullContent, new UTF8Encoding(false));
      (string? actualContent, long? actualBytesRead, string? errorMsg) = Night.Filesystem.Read(this.testFileName, this.bytesToRead);

      Assert.Null(errorMsg);
      Assert.Equal(this.expectedContent, actualContent);
      Assert.Equal(this.bytesToRead, actualBytesRead);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        File.Delete(this.testFileName);
      }
    }
  }
}

/// <summary>
/// Tests Night.Filesystem.Read for partial data read.
/// </summary>
public class FilesystemRead_Data_PartialReadTest : ModTestCase
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
  public override string SuccessMessage => "Successfully read partial byte[] content and byte count matches.";

  /// <inheritdoc/>
  public override void Run()
  {
    try
    {
      File.WriteAllBytes(this.testFileName, this.fullContent);
      (object? actualContentObj, long? actualBytesRead, string? errorMsg) = Night.Filesystem.Read(Night.Filesystem.ContainerType.Data, this.testFileName, this.bytesToRead);

      Assert.Null(errorMsg);
      byte[]? actualContent = Assert.IsType<byte[]>(actualContentObj);
      Assert.Equal(this.expectedContent, actualContent);
      Assert.Equal(this.bytesToRead, actualBytesRead);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        File.Delete(this.testFileName);
      }
    }
  }
}

/// <summary>
/// Tests Night.Filesystem.Read for an empty file.
/// </summary>
public class FilesystemRead_EmptyFileTest : ModTestCase
{
  private readonly string testFileName = Path.Combine(Path.GetTempPath(), "night_test_read_empty.txt");

  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.EmptyFile";

  /// <inheritdoc/>
  public override string Description => "Tests Read for an empty file, expecting empty string/data and 0 bytes read.";

  /// <inheritdoc/>
  public override string SuccessMessage => "Successfully read empty file as string and data (empty content, 0 bytes read).";

  /// <inheritdoc/>
  public override void Run()
  {
    try
    {
      File.WriteAllBytes(this.testFileName, Array.Empty<byte>()); // Create empty file

      // Test as string
      (string? strContents, long? strBytesRead, string? strErrorMsg) = Night.Filesystem.Read(this.testFileName);
      Assert.Null(strErrorMsg);
      Assert.Equal(string.Empty, strContents);
      Assert.Equal(0, strBytesRead);

      // Test as data
      (object? dataContentsObj, long? dataBytesRead, string? dataErrorMsg) = Night.Filesystem.Read(Night.Filesystem.ContainerType.Data, this.testFileName);
      Assert.Null(dataErrorMsg);
      byte[]? dataContents = Assert.IsType<byte[]>(dataContentsObj);
      Assert.Empty(dataContents);
      Assert.Equal(0, dataBytesRead);
    }
    finally
    {
      if (File.Exists(this.testFileName))
      {
        File.Delete(this.testFileName);
      }
    }
  }
}

/// <summary>
/// Tests Night.Filesystem.Read with invalid arguments.
/// </summary>
public class FilesystemRead_ArgumentValidationTest : ModTestCase
{
  /// <inheritdoc/>
  public override string Name => "Filesystem.Read.ArgumentValidation";

  /// <inheritdoc/>
  public override string Description => "Tests Read with various invalid arguments (null/empty filename, negative size).";

  /// <inheritdoc/>
  public override string SuccessMessage => "All argument validation tests passed for Read.";

  /// <inheritdoc/>
  public override void Run()
  {
    // Test null name
    (_, _, string? errorMsgNull) = Night.Filesystem.Read(null!);
    Assert.Equal("File name cannot be null or empty.", errorMsgNull);

    // Test empty name
    (_, _, string? errorMsgEmpty) = Night.Filesystem.Read(string.Empty);
    Assert.Equal("File name cannot be null or empty.", errorMsgEmpty);

    string dummyFile = Path.Combine(Path.GetTempPath(), "night_test_read_dummy_arg.txt");
    try
    {
      // Test negative size
      File.WriteAllText(dummyFile, "dummy");
      (_, _, string? errorMsgNegative) = Night.Filesystem.Read(dummyFile, -5);
      Assert.Equal("Size to read cannot be negative.", errorMsgNegative);

      // Test zero size
      (string? zeroContent, long? zeroBytes, string? zeroError) = Night.Filesystem.Read(dummyFile, 0);
      Assert.Null(zeroError);
      Assert.Equal(string.Empty, zeroContent);
      Assert.Equal(0, zeroBytes);
    }
    finally
    {
      if (File.Exists(dummyFile))
      {
        File.Delete(dummyFile);
      }
    }
  }
}
