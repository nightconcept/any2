// <copyright file="GetInfoTests.cs" company="Night Circle">
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

using Night;

using NightTest.Core;

using Xunit;

namespace NightTest.Groups.Filesystem
{
  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?)"/> returns null when the path is null.
  /// </summary>
  public class FilesystemGetInfo_NullPath_ReturnsNullTest : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.NullPathMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo with a null path, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo with null path correctly returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      string? path = null;

      // Act
#pragma warning disable CS8604 // Possible null reference argument. Test case specifically for null.
      var info = Night.Filesystem.GetInfo(path);
#pragma warning restore CS8604

      // Assert
      Assert.Null(info);
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?)"/> returns null when the path is empty.
  /// </summary>
  public class FilesystemGetInfo_EmptyPath_ReturnsNullTest : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.EmptyPathMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo with an empty path, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo with empty path correctly returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      string path = string.Empty;

      // Act
      var info = Night.Filesystem.GetInfo(path);

      // Assert
      Assert.Null(info);
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?)"/> returns null for a path that does not exist.
  /// </summary>
  public class FilesystemGetInfo_PathDoesNotExist_ReturnsNullTest : ModTestCase
  {
    private readonly string nonExistentPath = Path.Combine(Path.GetTempPath(), $"night_test_non_existent_mod_{Guid.NewGuid()}");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PathDoesNotExistMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for a non-existent path, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo for a non-existent path correctly returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      // Ensure path does not exist (highly unlikely for a GUID-based name, but good practice)
      if (File.Exists(this.nonExistentPath))
      {
        File.Delete(this.nonExistentPath);
      }

      if (Directory.Exists(this.nonExistentPath))
      {
        Directory.Delete(this.nonExistentPath);
      }

      // Act
      var info = Night.Filesystem.GetInfo(this.nonExistentPath);

      // Assert
      Assert.Null(info);
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?)"/> returns correct info for an existing file when no filter is applied.
  /// </summary>
  public class FilesystemGetInfo_FileExists_NoFilter_ReturnsFileInfoTest : ModTestCase
  {
    private const string TestFileContent = "Hello Night!";
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), $"night_test_file_exists_mod_{Guid.NewGuid()}.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.FileExistsNoFilterMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for an existing file with no filter, expecting correct FileSystemInfo (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo for an existing file with no filter returned correct FileSystemInfo.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      long expectedSize = 0;
      long expectedModTime = 0;
      try
      {
        File.WriteAllText(this.testFileName, TestFileContent);
        var fileInfo = new FileInfo(this.testFileName);
        expectedSize = fileInfo.Length;

        // Ensure LastWriteTimeUtc is fresh for comparison
        fileInfo.LastWriteTimeUtc = DateTime.UtcNow;
        expectedModTime = new DateTimeOffset(fileInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test file '{this.testFileName}'. {ex.Message}");
      }

      try
      {
        // Act
        var info = Night.Filesystem.GetInfo(this.testFileName);

        // Assert
        Assert.NotNull(info);
        Assert.Equal(FileType.File, info.Type);
        Assert.Equal(expectedSize, info.Size);
        _ = Assert.NotNull(info.ModTime);
        Assert.True(Math.Abs(info.ModTime.Value - expectedModTime) <= 2, $"Expected ModTime around {expectedModTime}, but got {info.ModTime}. Difference: {Math.Abs((info.ModTime ?? 0) - expectedModTime)}");
      }
      finally
      {
        // Cleanup
        if (File.Exists(this.testFileName))
        {
          File.Delete(this.testFileName);
        }
      }
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> returns correct info for a file that matches the filter.
  /// </summary>
  public class FilesystemGetInfo_FileExists_MatchingFilter_ReturnsFileInfoTest : ModTestCase
  {
    private const string TestFileContent = "Filter Match!";
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), $"night_test_file_match_filter_mod_{Guid.NewGuid()}.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.FileExistsMatchingFilterMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for an existing file with FileType.File filter, expecting correct FileSystemInfo (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo for an existing file with FileType.File filter returned correct FileSystemInfo.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      try
      {
        File.WriteAllText(this.testFileName, TestFileContent);

        // Ensure LastWriteTimeUtc is fresh for comparison
        new FileInfo(this.testFileName).LastWriteTimeUtc = DateTime.UtcNow;
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test file '{this.testFileName}'. {ex.Message}");
      }

      try
      {
        // Act
        var info = Night.Filesystem.GetInfo(this.testFileName, FileType.File);

        // Assert
        Assert.NotNull(info);
        Assert.Equal(FileType.File, info.Type);
      }
      finally
      {
        // Cleanup
        if (File.Exists(this.testFileName))
        {
          File.Delete(this.testFileName);
        }
      }
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> returns null for a file that does not match the filter.
  /// </summary>
  public class FilesystemGetInfo_FileExists_NonMatchingFilter_ReturnsNullTest : ModTestCase
  {
    private const string TestFileContent = "Filter No Match!";
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), $"night_test_file_nonmatch_filter_mod_{Guid.NewGuid()}.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.FileExistsNonMatchingFilterMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for an existing file with FileType.Directory filter, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo for an existing file with FileType.Directory filter correctly returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      try
      {
        File.WriteAllText(this.testFileName, TestFileContent);
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test file '{this.testFileName}'. {ex.Message}");
      }

      try
      {
        // Act
        var info = Night.Filesystem.GetInfo(this.testFileName, FileType.Directory);

        // Assert
        Assert.Null(info);
      }
      finally
      {
        // Cleanup
        if (File.Exists(this.testFileName))
        {
          File.Delete(this.testFileName);
        }
      }
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?)"/> returns correct info for a directory with no filter.
  /// </summary>
  public class FilesystemGetInfo_DirectoryExists_NoFilter_ReturnsDirectoryInfoTest : ModTestCase
  {
    private readonly string testDirName = Path.Combine(Path.GetTempPath(), $"night_test_dir_exists_mod_{Guid.NewGuid()}");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.DirectoryExistsNoFilterMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for an existing directory with no filter, expecting correct FileSystemInfo (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo for an existing directory with no filter returned correct FileSystemInfo.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      long expectedModTime = 0;
      try
      {
        _ = Directory.CreateDirectory(this.testDirName);
        var dirInfo = new DirectoryInfo(this.testDirName);

        // Ensure LastWriteTimeUtc is fresh for comparison
        dirInfo.LastWriteTimeUtc = DateTime.UtcNow;
        expectedModTime = new DateTimeOffset(dirInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test directory '{this.testDirName}'. {ex.Message}");
      }

      try
      {
        // Act
        var info = Night.Filesystem.GetInfo(this.testDirName);

        // Assert
        Assert.NotNull(info);
        Assert.Equal(FileType.Directory, info.Type);
        Assert.Null(info.Size); // Size should be null for directories
        _ = Assert.NotNull(info.ModTime);
        Assert.True(Math.Abs(info.ModTime.Value - expectedModTime) <= 2, $"Expected ModTime around {expectedModTime}, but got {info.ModTime}. Difference: {Math.Abs((info.ModTime ?? 0) - expectedModTime)}");
      }
      finally
      {
        // Cleanup
        if (Directory.Exists(this.testDirName))
        {
          Directory.Delete(this.testDirName, true);
        }
      }
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> returns correct info for a directory that matches the filter.
  /// </summary>
  public class FilesystemGetInfo_DirectoryExists_MatchingFilter_ReturnsDirectoryInfoTest : ModTestCase
  {
    private readonly string testDirName = Path.Combine(Path.GetTempPath(), $"night_test_dir_match_filter_mod_{Guid.NewGuid()}");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.DirectoryExistsMatchingFilterMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for an existing directory with FileType.Directory filter, expecting correct FileSystemInfo (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo for an existing directory with FileType.Directory filter returned correct FileSystemInfo.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      try
      {
        _ = Directory.CreateDirectory(this.testDirName);

        // Ensure LastWriteTimeUtc is fresh for comparison
        new DirectoryInfo(this.testDirName).LastWriteTimeUtc = DateTime.UtcNow;
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test directory '{this.testDirName}'. {ex.Message}");
      }

      try
      {
        // Act
        var info = Night.Filesystem.GetInfo(this.testDirName, FileType.Directory);

        // Assert
        Assert.NotNull(info);
        Assert.Equal(FileType.Directory, info.Type);
      }
      finally
      {
        // Cleanup
        if (Directory.Exists(this.testDirName))
        {
          Directory.Delete(this.testDirName, true);
        }
      }
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> returns null for a directory that does not match the filter.
  /// </summary>
  public class FilesystemGetInfo_DirectoryExists_NonMatchingFilter_ReturnsNullTest : ModTestCase
  {
    private readonly string testDirName = Path.Combine(Path.GetTempPath(), $"night_test_dir_nonmatch_filter_mod_{Guid.NewGuid()}");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.DirectoryExistsNonMatchingFilterMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for an existing directory with FileType.File filter, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo for an existing directory with FileType.File filter correctly returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      try
      {
        _ = Directory.CreateDirectory(this.testDirName);
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test directory '{this.testDirName}'. {ex.Message}");
      }

      try
      {
        // Act
        var info = Night.Filesystem.GetInfo(this.testDirName, FileType.File);

        // Assert
        Assert.Null(info);
      }
      finally
      {
        // Cleanup
        if (Directory.Exists(this.testDirName))
        {
          Directory.Delete(this.testDirName, true);
        }
      }
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileSystemInfo)"/> returns null when the info object to populate is null.
  /// </summary>
  public class FilesystemGetInfo_PopulateNullInfoObject_ReturnsNullTest : ModTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), $"night_test_populate_null_info_{Guid.NewGuid()}.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateNullInfoObjectMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo with a null FileSystemInfo object to populate, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo with a null FileSystemInfo object correctly returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      Night.FileSystemInfo? infoToPopulate = null;
      try
      {
        File.WriteAllText(this.testFileName, "content"); // File needs to exist for GetInfo to proceed
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test file '{this.testFileName}'. {ex.Message}");
      }

      try
      {
        // Act
#pragma warning disable CS8604 // Possible null reference argument. Test case specifically for null.
        var resultInfo = Night.Filesystem.GetInfo(this.testFileName, infoToPopulate!);
#pragma warning restore CS8604

        // Assert
        Assert.Null(resultInfo);
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
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileSystemInfo)"/> correctly populates a valid info object for an existing file.
  /// </summary>
  public class FilesystemGetInfo_PopulateValidInfo_FileExists_PopulatesAndReturnsInfoTest : ModTestCase
  {
    private const string TestFileContent = "Populate Me!";
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), $"night_test_populate_file_mod_{Guid.NewGuid()}.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateValidInfoFileExistsMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo with a valid FileSystemInfo object for an existing file, expecting it to be populated (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo with a valid FileSystemInfo object for an existing file populated and returned it.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      var infoToPopulate = new Night.FileSystemInfo(FileType.Other, null, null); // Initial dummy values
      long expectedSize = 0;
      long expectedModTime = 0;

      try
      {
        File.WriteAllText(this.testFileName, TestFileContent);
        var fileInfo = new FileInfo(this.testFileName);
        expectedSize = fileInfo.Length;
        fileInfo.LastWriteTimeUtc = DateTime.UtcNow;
        expectedModTime = new DateTimeOffset(fileInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test file '{this.testFileName}'. {ex.Message}");
      }

      try
      {
        // Act
        var resultInfo = Night.Filesystem.GetInfo(this.testFileName, infoToPopulate);

        // Assert
        Assert.NotNull(resultInfo);
        Assert.Same(infoToPopulate, resultInfo); // Should be the same instance
        Assert.Equal(FileType.File, resultInfo.Type);
        Assert.Equal(expectedSize, resultInfo.Size);
        _ = Assert.NotNull(resultInfo.ModTime);
        Assert.True(Math.Abs(resultInfo.ModTime.Value - expectedModTime) <= 2, $"Expected ModTime around {expectedModTime}, but got {resultInfo.ModTime}. Difference: {Math.Abs((resultInfo.ModTime ?? 0) - expectedModTime)}");
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
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileSystemInfo)"/> correctly populates a valid info object for an existing directory.
  /// </summary>
  public class FilesystemGetInfo_PopulateValidInfo_DirectoryExists_PopulatesAndReturnsInfoTest : ModTestCase
  {
    private readonly string testDirName = Path.Combine(Path.GetTempPath(), $"night_test_populate_dir_mod_{Guid.NewGuid()}");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateValidInfoDirectoryExistsMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo with a valid FileSystemInfo object for an existing directory, expecting it to be populated (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo with a valid FileSystemInfo object for an existing directory populated and returned it.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      var infoToPopulate = new Night.FileSystemInfo(FileType.Other, 123, 123); // Initial dummy values
      long expectedModTime = 0;

      try
      {
        _ = Directory.CreateDirectory(this.testDirName);
        var dirInfo = new DirectoryInfo(this.testDirName);
        dirInfo.LastWriteTimeUtc = DateTime.UtcNow;
        expectedModTime = new DateTimeOffset(dirInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test directory '{this.testDirName}'. {ex.Message}");
      }

      try
      {
        // Act
        var resultInfo = Night.Filesystem.GetInfo(this.testDirName, infoToPopulate);

        // Assert
        Assert.NotNull(resultInfo);
        Assert.Same(infoToPopulate, resultInfo);
        Assert.Equal(FileType.Directory, resultInfo.Type);
        Assert.Null(resultInfo.Size); // Size should be null for directories
        _ = Assert.NotNull(resultInfo.ModTime);
        Assert.True(Math.Abs(resultInfo.ModTime.Value - expectedModTime) <= 2, $"Expected ModTime around {expectedModTime}, but got {resultInfo.ModTime}. Difference: {Math.Abs((resultInfo.ModTime ?? 0) - expectedModTime)}");
      }
      finally
      {
        if (Directory.Exists(this.testDirName))
        {
          Directory.Delete(this.testDirName, true);
        }
      }
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileSystemInfo)"/> returns null when the path does not exist.
  /// </summary>
  public class FilesystemGetInfo_PopulateValidInfo_PathDoesNotExist_ReturnsNullTest : ModTestCase
  {
    private readonly string nonExistentPath = Path.Combine(Path.GetTempPath(), $"night_test_populate_non_existent_mod_{Guid.NewGuid()}");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateValidInfoPathDoesNotExistMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo with a valid FileSystemInfo object for a non-existent path, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo with a valid FileSystemInfo object for a non-existent path correctly returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      var infoToPopulate = new Night.FileSystemInfo(FileType.File, 10, 100); // Initial dummy values
      if (File.Exists(this.nonExistentPath))
      {
        File.Delete(this.nonExistentPath);
      }

      if (Directory.Exists(this.nonExistentPath))
      {
        Directory.Delete(this.nonExistentPath, true);
      }

      // Act
      var resultInfo = Night.Filesystem.GetInfo(this.nonExistentPath, infoToPopulate);

      // Assert
      Assert.Null(resultInfo);

      // Original infoToPopulate should remain unchanged by the GetInfo call if path doesn't exist
      Assert.Equal(FileType.File, infoToPopulate.Type);
      Assert.Equal(10, infoToPopulate.Size);
      Assert.Equal(100, infoToPopulate.ModTime);
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> returns null when the info object is null.
  /// </summary>
  public class FilesystemGetInfo_PopulateWithFilter_NullInfoObject_ReturnsNullTest : ModTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), $"night_test_populate_filter_null_info_{Guid.NewGuid()}.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateWithFilterNullInfoObjectMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo (filter overload) with a null FileSystemInfo object, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo (filter overload) with a null FileSystemInfo object correctly returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      Night.FileSystemInfo? infoToPopulate = null;
      try
      {
        File.WriteAllText(this.testFileName, "content");
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test file '{this.testFileName}'. {ex.Message}");
      }

      try
      {
        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var resultInfo = Night.Filesystem.GetInfo(this.testFileName, FileType.File, infoToPopulate);
#pragma warning restore CS8604

        // Assert
        Assert.Null(resultInfo);
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
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> populates info for a file with a matching filter.
  /// </summary>
  public class FilesystemGetInfo_PopulateWithFilter_FileExists_MatchingFilter_PopulatesInfoTest : ModTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), $"night_test_populate_file_match_filter_mod_{Guid.NewGuid()}.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateWithFilterFileMatchingMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo (filter overload) for a file with matching filter, expecting populated info (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo (filter overload) for a file with matching filter populated info.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      var infoToPopulate = new Night.FileSystemInfo(FileType.Other, 0, 0);
      long expectedSize = 0;
      long expectedModTime = 0;
      try
      {
        File.WriteAllText(this.testFileName, "content");
        var fileInfo = new FileInfo(this.testFileName);
        expectedSize = fileInfo.Length;
        fileInfo.LastWriteTimeUtc = DateTime.UtcNow;
        expectedModTime = new DateTimeOffset(fileInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test file '{this.testFileName}'. {ex.Message}");
      }

      try
      {
        // Act
        var resultInfo = Night.Filesystem.GetInfo(this.testFileName, FileType.File, infoToPopulate);

        // Assert
        Assert.NotNull(resultInfo);
        Assert.Same(infoToPopulate, resultInfo);
        Assert.Equal(FileType.File, resultInfo.Type);
        Assert.Equal(expectedSize, resultInfo.Size);
        _ = Assert.NotNull(resultInfo.ModTime);
        Assert.True(Math.Abs(resultInfo.ModTime.Value - expectedModTime) <= 2);
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
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> returns null for a file with a non-matching filter.
  /// </summary>
  public class FilesystemGetInfo_PopulateWithFilter_FileExists_NonMatchingFilter_ReturnsNullTest : ModTestCase
  {
    private readonly string testFileName = Path.Combine(Path.GetTempPath(), $"night_test_populate_file_nonmatch_filter_mod_{Guid.NewGuid()}.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateWithFilterFileNonMatchingMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo (filter overload) for a file with non-matching filter, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo (filter overload) for a file with non-matching filter returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      var infoToPopulate = new Night.FileSystemInfo(FileType.Other, 0, 0);
      try
      {
        File.WriteAllText(this.testFileName, "content");
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test file '{this.testFileName}'. {ex.Message}");
      }

      try
      {
        // Act
        var resultInfo = Night.Filesystem.GetInfo(this.testFileName, FileType.Directory, infoToPopulate);

        // Assert
        Assert.Null(resultInfo);

        // Original infoToPopulate should remain unchanged
        Assert.Equal(FileType.Other, infoToPopulate.Type);
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
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> populates info for a directory with a matching filter.
  /// </summary>
  public class FilesystemGetInfo_PopulateWithFilter_DirectoryExists_MatchingFilter_PopulatesInfoTest : ModTestCase
  {
    private readonly string testDirName = Path.Combine(Path.GetTempPath(), $"night_test_populate_dir_match_filter_mod_{Guid.NewGuid()}");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateWithFilterDirectoryMatchingMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo (filter overload) for a directory with matching filter, expecting populated info (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo (filter overload) for a directory with matching filter populated info.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      var infoToPopulate = new Night.FileSystemInfo(FileType.Other, 0, 0);
      long expectedModTime = 0;
      try
      {
        _ = Directory.CreateDirectory(this.testDirName);
        var dirInfo = new DirectoryInfo(this.testDirName);
        dirInfo.LastWriteTimeUtc = DateTime.UtcNow;
        expectedModTime = new DateTimeOffset(dirInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test directory '{this.testDirName}'. {ex.Message}");
      }

      try
      {
        // Act
        var resultInfo = Night.Filesystem.GetInfo(this.testDirName, FileType.Directory, infoToPopulate);

        // Assert
        Assert.NotNull(resultInfo);
        Assert.Same(infoToPopulate, resultInfo);
        Assert.Equal(FileType.Directory, resultInfo.Type);
        Assert.Null(resultInfo.Size);
        _ = Assert.NotNull(resultInfo.ModTime);
        Assert.True(Math.Abs(resultInfo.ModTime.Value - expectedModTime) <= 2);
      }
      finally
      {
        if (Directory.Exists(this.testDirName))
        {
          Directory.Delete(this.testDirName, true);
        }
      }
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> returns null for a directory with a non-matching filter.
  /// </summary>
  public class FilesystemGetInfo_PopulateWithFilter_DirectoryExists_NonMatchingFilter_ReturnsNullTest : ModTestCase
  {
    private readonly string testDirName = Path.Combine(Path.GetTempPath(), $"night_test_populate_dir_nonmatch_filter_mod_{Guid.NewGuid()}");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateWithFilterDirectoryNonMatchingMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo (filter overload) for a directory with non-matching filter, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo (filter overload) for a directory with non-matching filter returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      var infoToPopulate = new Night.FileSystemInfo(FileType.Other, 0, 0);
      try
      {
        _ = Directory.CreateDirectory(this.testDirName);
      }
      catch (Exception ex)
      {
        Assert.Fail($"Test setup failed: Could not create test directory '{this.testDirName}'. {ex.Message}");
      }

      try
      {
        // Act
        var resultInfo = Night.Filesystem.GetInfo(this.testDirName, FileType.File, infoToPopulate);

        // Assert
        Assert.Null(resultInfo);
        Assert.Equal(FileType.Other, infoToPopulate.Type);
      }
      finally
      {
        if (Directory.Exists(this.testDirName))
        {
          Directory.Delete(this.testDirName, true);
        }
      }
    }
  }

  /// <summary>
  /// Tests that <see cref="Night.Filesystem.GetInfo(string?, Night.FileType, Night.FileSystemInfo?)"/> returns null for a non-existent path.
  /// </summary>
  public class FilesystemGetInfo_PopulateWithFilter_PathDoesNotExist_ReturnsNullTest : ModTestCase
  {
    private readonly string nonExistentPath = Path.Combine(Path.GetTempPath(), $"night_test_populate_filter_non_existent_mod_{Guid.NewGuid()}");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PopulateWithFilterPathDoesNotExistMod";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo (filter overload) for a non-existent path, expecting null (Mod Test).";

    /// <inheritdoc/>
    public override string SuccessMessage => "GetInfo (filter overload) for a non-existent path correctly returned null.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      var infoToPopulate = new Night.FileSystemInfo(FileType.File, 10, 100);
      if (File.Exists(this.nonExistentPath))
      {
        File.Delete(this.nonExistentPath);
      }

      if (Directory.Exists(this.nonExistentPath))
      {
        Directory.Delete(this.nonExistentPath, true);
      }

      // Act
      var resultInfo = Night.Filesystem.GetInfo(this.nonExistentPath, FileType.File, infoToPopulate);

      // Assert
      Assert.Null(resultInfo);
      Assert.Equal(FileType.File, infoToPopulate.Type);
      Assert.Equal(10, infoToPopulate.Size);
      Assert.Equal(100, infoToPopulate.ModTime);
    }
  }

  // More test cases for existing files, directories, filters, and other overloads will be added here.
}
