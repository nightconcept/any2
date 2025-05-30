// <copyright file="FilesystemTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;

using Night;

using Xunit;

public class FilesystemTests : IDisposable
{
  private const string TestDir = "test_filesystem_temp";
  private const string TestFile = "test_file.txt";
  private const string TestSubDir = "test_subdir";
  private readonly string testFilePath;
  private readonly string testDirPath;
  private readonly string testSubDirPath;
  private readonly string testSymlinkFilePath;
  private readonly string testSymlinkDirPath;

  public FilesystemTests()
  {
    // Create a temporary directory for test files relative to the test execution directory
    var executionPath = Path.GetDirectoryName(typeof(FilesystemTests).Assembly.Location);
    var testDirFullPath = Path.Combine(executionPath!, TestDir);
    _ = Directory.CreateDirectory(testDirFullPath);

    this.testFilePath = Path.Combine(testDirFullPath, TestFile);
    this.testDirPath = Path.Combine(testDirFullPath, "actual_dir_for_symlink");
    this.testSubDirPath = Path.Combine(testDirFullPath, TestSubDir);
    this.testSymlinkFilePath = Path.Combine(testDirFullPath, "symlink_file.txt");
    this.testSymlinkDirPath = Path.Combine(testDirFullPath, "symlink_dir");

    File.WriteAllText(this.testFilePath, "Hello Night Engine!");
    _ = Directory.CreateDirectory(this.testDirPath);
    _ = Directory.CreateDirectory(this.testSubDirPath);

    // Create symlinks if supported (Windows requires admin rights or dev mode)
    try
    {
      _ = File.CreateSymbolicLink(this.testSymlinkFilePath, this.testFilePath);
    }
    catch (IOException ex)
    {
      Console.WriteLine($"Could not create file symlink: {ex.Message}. This test might be skipped or fail if symlinks are essential.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An unexpected error occurred during file symlink creation: {ex.Message}");
    }

    try
    {
      _ = Directory.CreateSymbolicLink(this.testSymlinkDirPath, this.testDirPath);
    }
    catch (IOException ex)
    {
      Console.WriteLine($"Could not create directory symlink: {ex.Message}. This test might be skipped or fail if symlinks are essential.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An unexpected error occurred during directory symlink creation: {ex.Message}");
    }
  }

  public void Dispose()
  {
    // Clean up the temporary directory
    var executionPath = Path.GetDirectoryName(typeof(FilesystemTests).Assembly.Location);
    var testDirFullPath = Path.Combine(executionPath!, TestDir);
    if (Directory.Exists(testDirFullPath))
    {
      Directory.Delete(testDirFullPath, true);
    }
  }

  [Fact]
  public void GetInfo_NullPath_ReturnsNull()
  {
    Assert.Null(Night.Filesystem.GetInfo(null!));
  }

  [Fact]
  public void GetInfo_EmptyPath_ReturnsNull()
  {
    Assert.Null(Night.Filesystem.GetInfo(string.Empty));
  }

  [Fact]
  public void GetInfo_NonExistentPath_ReturnsNull()
  {
    var executionPath = Path.GetDirectoryName(typeof(FilesystemTests).Assembly.Location);
    var testDirFullPath = Path.Combine(executionPath!, TestDir);
    Assert.Null(Night.Filesystem.GetInfo(Path.Combine(testDirFullPath, "non_existent_file.txt")));
  }

  [Fact]
  public void GetInfo_ExistingFile_ReturnsFileInfo()
  {
    var info = Night.Filesystem.GetInfo(this.testFilePath);
    Assert.NotNull(info);
    Assert.Equal(Night.FileType.File, info.Type);
    Assert.Equal(new FileInfo(this.testFilePath).Length, info.Size);
    _ = Assert.NotNull(info.ModTime);
  }

  [Fact]
  public void GetInfo_ExistingFile_WithFilterTypeFile_ReturnsFileInfo()
  {
    var info = Night.Filesystem.GetInfo(this.testFilePath, Night.FileType.File);
    Assert.NotNull(info);
    Assert.Equal(Night.FileType.File, info.Type);
  }

  [Fact]
  public void GetInfo_ExistingFile_WithFilterTypeDirectory_ReturnsNull()
  {
    var info = Night.Filesystem.GetInfo(this.testFilePath, Night.FileType.Directory);
    Assert.Null(info);
  }

  [Fact]
  public void GetInfo_ExistingDirectory_ReturnsDirectoryInfo()
  {
    var info = Night.Filesystem.GetInfo(this.testSubDirPath);
    Assert.NotNull(info);
    Assert.Equal(Night.FileType.Directory, info.Type);
    Assert.Null(info.Size); // Size is null for directories in our implementation
    _ = Assert.NotNull(info.ModTime);
  }

  [Fact]
  public void GetInfo_ExistingDirectory_WithFilterTypeDirectory_ReturnsDirectoryInfo()
  {
    var info = Night.Filesystem.GetInfo(this.testSubDirPath, Night.FileType.Directory);
    Assert.NotNull(info);
    Assert.Equal(Night.FileType.Directory, info.Type);
  }

  [Fact]
  public void GetInfo_ExistingDirectory_WithFilterTypeFile_ReturnsNull()
  {
    var info = Night.Filesystem.GetInfo(this.testSubDirPath, Night.FileType.File);
    Assert.Null(info);
  }

  [Fact]
  public void GetInfo_FileSymlink_ReturnsSymlinkInfo()
  {
    if (!File.Exists(this.testSymlinkFilePath) && !Directory.Exists(this.testSymlinkFilePath) /* Symlink could point to dir or file, check both just in case File.Exists is tricky with broken file symlinks */)
    {
      // Skip if symlink creation failed (e.g. permissions on Windows or if it points to a now-deleted item and File.Exists returns false)
      Console.WriteLine($"Skipping symlink test for file: {this.testSymlinkFilePath} as it does not exist or could not be verified.");
      return;
    }

    var info = Night.Filesystem.GetInfo(this.testSymlinkFilePath);
    Assert.NotNull(info);
    Assert.Equal(Night.FileType.Symlink, info.Type);
  }

  [Fact]
  public void GetInfo_DirectorySymlink_ReturnsSymlinkInfo()
  {
    if (!Directory.Exists(this.testSymlinkDirPath))
    {
      // Skip if symlink creation failed (e.g. permissions on Windows)
      Console.WriteLine($"Skipping symlink test for directory: {this.testSymlinkDirPath} as it does not exist or could not be verified.");
      return;
    }

    var info = Night.Filesystem.GetInfo(this.testSymlinkDirPath);
    Assert.NotNull(info);
    Assert.Equal(Night.FileType.Symlink, info.Type);
  }

  [Fact]
  public void GetInfo_PopulatesExistingObject_ValidPath()
  {
    var existingInfo = new Night.FileSystemInfo(Night.FileType.File, 0, 0); // Dummy initial values
    var result = Night.Filesystem.GetInfo(this.testFilePath, existingInfo);

    Assert.NotNull(result);
    Assert.Same(existingInfo, result);
    Assert.Equal(Night.FileType.File, existingInfo.Type);
    Assert.Equal(new FileInfo(this.testFilePath).Length, existingInfo.Size);
  }

  [Fact]
  public void GetInfo_PopulatesExistingObject_NonExistentPath_ReturnsNullAndDoesNotChangeObject()
  {
    var originalType = Night.FileType.Other; // Different from what GetInfo might set
    long? originalSize = 12345;
    long? originalModTime = 67890;
    var executionPath = Path.GetDirectoryName(typeof(FilesystemTests).Assembly.Location);
    var testDirFullPath = Path.Combine(executionPath!, TestDir);

    var existingInfo = new Night.FileSystemInfo(originalType, originalSize, originalModTime);
    var result = Night.Filesystem.GetInfo(Path.Combine(testDirFullPath, "non_existent_file.txt"), existingInfo);

    Assert.Null(result);

    // Check that the original object was not modified
    Assert.Equal(originalType, existingInfo.Type);
    Assert.Equal(originalSize, existingInfo.Size);
    Assert.Equal(originalModTime, existingInfo.ModTime);
  }

  [Fact]
  public void GetInfo_PopulatesExistingObjectWithFilter_ValidPathAndType()
  {
    var existingInfo = new Night.FileSystemInfo(Night.FileType.Directory, 0, 0); // Dummy initial values
    var result = Night.Filesystem.GetInfo(this.testFilePath, Night.FileType.File, existingInfo);

    Assert.NotNull(result);
    Assert.Same(existingInfo, result);
    Assert.Equal(Night.FileType.File, existingInfo.Type);
  }

  [Fact]
  public void GetInfo_PopulatesExistingObjectWithFilter_PathExistsButWrongType_ReturnsNull()
  {
    var originalType = Night.FileType.Other;
    var existingInfo = new Night.FileSystemInfo(originalType, null, null);
    var result = Night.Filesystem.GetInfo(this.testFilePath, Night.FileType.Directory, existingInfo); // File exists, but asking for Directory type

    Assert.Null(result);
    Assert.Equal(originalType, existingInfo.Type); // Should not have been modified
  }

  [Fact]
  public void ReadBytes_ExistingFile_ReturnsCorrectBytes()
  {
    var expectedBytes = Encoding.UTF8.GetBytes("Hello Night Engine!");
    var actualBytes = Night.Filesystem.ReadBytes(this.testFilePath);
    Assert.Equal(expectedBytes, actualBytes);
  }

  [Fact]
  public void ReadBytes_NonExistentFile_ThrowsFileNotFound()
  {
    var executionPath = Path.GetDirectoryName(typeof(FilesystemTests).Assembly.Location);
    var testDirFullPath = Path.Combine(executionPath!, TestDir);
    _ = Assert.Throws<FileNotFoundException>(() => Night.Filesystem.ReadBytes(Path.Combine(testDirFullPath, "non_existent_file_for_read.txt")));
  }

  [Fact]
  public void ReadText_ExistingFile_ReturnsCorrectText()
  {
    var expectedText = "Hello Night Engine!";
    var actualText = Night.Filesystem.ReadText(this.testFilePath);
    Assert.Equal(expectedText, actualText);
  }

  [Fact]
  public void ReadText_NonExistentFile_ThrowsFileNotFound()
  {
    var executionPath = Path.GetDirectoryName(typeof(FilesystemTests).Assembly.Location);
    var testDirFullPath = Path.Combine(executionPath!, TestDir);
    _ = Assert.Throws<FileNotFoundException>(() => Night.Filesystem.ReadText(Path.Combine(testDirFullPath, "non_existent_file_for_read.txt")));
  }
}
