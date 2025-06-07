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
using System.Threading;

using Night;

using NightTest.Core;

namespace NightTest.Groups.Filesystem
{
  /// <summary>
  /// Tests for Night.Filesystem.GetInfo().
  /// </summary>
  public class FilesystemGetInfo_FileExistsTest : BaseTestCase
  {
    private readonly string _testFileName = Path.Combine(Path.GetTempPath(), "night_test_getinfo_file.txt");
    private DateTimeOffset _expectedModTime;
    private long _expectedSize;

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.FileExists";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for an existing file, checking Type, Size, and ModTime.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      try
      {
        File.WriteAllText(this._testFileName, "Hello Night!");
        var fileInfo = new FileInfo(this._testFileName);
        this._expectedSize = fileInfo.Length;
        this._expectedModTime = new DateTimeOffset(fileInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
        // Allow a small grace period for LastWriteTimeUtc to settle, especially on some filesystems.
        System.Threading.Thread.Sleep(100);
        fileInfo.LastWriteTimeUtc = DateTime.UtcNow; // Explicitly set to ensure consistency for comparison
        this._expectedModTime = new DateTimeOffset(fileInfo.LastWriteTimeUtc).ToUnixTimeSeconds();


      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Setup failed: Could not create test file '{this._testFileName}'. {e.Message}";
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
        if (this.CurrentStatus == TestStatus.Failed) // Setup failed
        {
          return;
        }

        var info = Night.Filesystem.GetInfo(this._testFileName);

        if (info == null)
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = "GetInfo returned null for an existing file.";
        }
        else if (info.Type != FileType.File)
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = $"Expected FileType.File, but got {info.Type}.";
        }
        else if (info.Size != this._expectedSize)
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = $"Expected size {this._expectedSize}, but got {info.Size}.";
        }
        // Comparing ModTime can be tricky due to precision. Allow a small delta.
        else if (info.ModTime == null || Math.Abs(info.ModTime.Value - this._expectedModTime) > 2) // Allow 2s delta
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = $"Expected ModTime around {this._expectedModTime}, but got {info.ModTime}. Difference: {Math.Abs((info.ModTime ?? 0) - this._expectedModTime)}";
        }
        else
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully retrieved correct info for an existing file.";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        if (File.Exists(this._testFileName))
        {
          try
          {
            File.Delete(this._testFileName);
          }
          catch (Exception ex)
          {
            this.Details += $" | Warning: Failed to delete test file '{this._testFileName}': {ex.Message}";
          }
        }
        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests GetInfo for an existing directory.
  /// </summary>
  public class FilesystemGetInfo_DirectoryExistsTest : BaseTestCase
  {
    private readonly string _testDirName = Path.Combine(Path.GetTempPath(), "night_test_getinfo_dir");
    private DateTimeOffset _expectedModTime;

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.DirectoryExists";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for an existing directory, checking Type and ModTime.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      try
      {
        Directory.CreateDirectory(this._testDirName);
        var dirInfo = new DirectoryInfo(this._testDirName);
        // Allow a small grace period for LastWriteTimeUtc to settle.
        System.Threading.Thread.Sleep(100);
        dirInfo.LastWriteTimeUtc = DateTime.UtcNow; // Explicitly set
        this._expectedModTime = new DateTimeOffset(dirInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Setup failed: Could not create test directory '{this._testDirName}'. {e.Message}";
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
        if (this.CurrentStatus == TestStatus.Failed) // Setup failed
        {
          return;
        }

        var info = Night.Filesystem.GetInfo(this._testDirName);

        if (info == null)
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = "GetInfo returned null for an existing directory.";
        }
        else if (info.Type != FileType.Directory)
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = $"Expected FileType.Directory, but got {info.Type}.";
        }
        else if (info.Size != null) // Size should be null for directories as per Filesystem.cs logic
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = $"Expected Size to be null for a directory, but got {info.Size}.";
        }
        else if (info.ModTime == null || Math.Abs(info.ModTime.Value - this._expectedModTime) > 2) // Allow 2s delta
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = $"Expected ModTime around {this._expectedModTime}, but got {info.ModTime}. Difference: {Math.Abs((info.ModTime ?? 0) - this._expectedModTime)}";
        }
        else
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "Successfully retrieved correct info for an existing directory.";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        if (Directory.Exists(this._testDirName))
        {
          try
          {
            Directory.Delete(this._testDirName);
          }
          catch (Exception ex)
          {
            this.Details += $" | Warning: Failed to delete test directory '{this._testDirName}': {ex.Message}";
          }
        }
        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests GetInfo for a path that does not exist.
  /// </summary>
  public class FilesystemGetInfo_PathDoesNotExistTest : BaseTestCase
  {
    private readonly string _nonExistentPath = Path.Combine(Path.GetTempPath(), "night_test_this_should_not_exist");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.PathDoesNotExist";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo for a non-existent path, expecting null.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      // Ensure the path does not exist
      if (File.Exists(this._nonExistentPath))
      {
        File.Delete(this._nonExistentPath);
      }

      if (Directory.Exists(this._nonExistentPath))
      {
        Directory.Delete(this._nonExistentPath);
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
        var info = Night.Filesystem.GetInfo(this._nonExistentPath);

        if (info == null)
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "GetInfo correctly returned null for a non-existent path.";
        }
        else
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = "GetInfo returned a non-null object for a non-existent path.";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests GetInfo with a FileType.File filter on an actual file.
  /// </summary>
  public class FilesystemGetInfo_FilterTypeFile_MatchesTest : BaseTestCase
  {
    private readonly string _testFileName = Path.Combine(Path.GetTempPath(), "night_test_getinfo_filter_file.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.FilterTypeFileMatches";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo with FileType.File filter on a file, expecting a match.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      try
      {
        File.WriteAllText(this._testFileName, "Filter test");
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Setup failed: Could not create test file '{this._testFileName}'. {e.Message}";
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
        if (this.CurrentStatus == TestStatus.Failed) return;

        var info = Night.Filesystem.GetInfo(this._testFileName, FileType.File);

        if (info != null && info.Type == FileType.File)
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "GetInfo with FileType.File filter correctly returned info for a file.";
        }
        else
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = "GetInfo with FileType.File filter failed to return info for a file or returned wrong type.";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        if (File.Exists(this._testFileName))
        {
          try
          {
            File.Delete(this._testFileName);
          }
          catch (Exception ex)
          {
            this.Details += $" | Warning: Failed to delete test file '{this._testFileName}': {ex.Message}";
          }
        }
        this.EndTest();
      }
    }
  }

  /// <summary>
  /// Tests GetInfo with a FileType.File filter on a directory.
  /// </summary>
  public class FilesystemGetInfo_FilterTypeFile_MismatchesTest : BaseTestCase
  {
    private readonly string _testDirName = Path.Combine(Path.GetTempPath(), "night_test_getinfo_filter_file_mismatch_dir");

    /// <inheritdoc/>
    public override string Name => "Filesystem.GetInfo.FilterTypeFileMismatches";

    /// <inheritdoc/>
    public override string Description => "Tests GetInfo with FileType.File filter on a directory, expecting null.";

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      try
      {
        Directory.CreateDirectory(this._testDirName);
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Setup failed: Could not create test directory '{this._testDirName}'. {e.Message}";
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
        if (this.CurrentStatus == TestStatus.Failed) return;

        var info = Night.Filesystem.GetInfo(this._testDirName, FileType.File);

        if (info == null)
        {
          this.CurrentStatus = TestStatus.Passed;
          this.Details = "GetInfo with FileType.File filter correctly returned null for a directory.";
        }
        else
        {
          this.CurrentStatus = TestStatus.Failed;
          this.Details = "GetInfo with FileType.File filter returned non-null for a directory.";
        }
      }
      catch (Exception e)
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"An unexpected error occurred: {e.Message}";
      }
      finally
      {
        if (Directory.Exists(this._testDirName))
        {
          try
          {
            Directory.Delete(this._testDirName);
          }
          catch (Exception ex)
          {
            this.Details += $" | Warning: Failed to delete test directory '{this._testDirName}': {ex.Message}";
          }
        }
        this.EndTest();
      }
    }
  }

  // TODO: Add tests for other GetInfo overloads:
  // - GetInfo(string path, FileSystemInfo info)
  // - GetInfo(string path, FileType filterType, FileSystemInfo info)
  // - GetInfo_EmptyPath_ReturnsNull
  // - GetInfo_SymlinkExists_ReturnsSymlinkType (if feasible)
  // - GetInfo_WithFilterTypeDirectory_Matches
  // - GetInfo_WithFilterTypeDirectory_Mismatches
}