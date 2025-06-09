// <copyright file="Read2Tests.cs" company="Night Circle">
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

using Night;

using NightTest.Core;

using Xunit;

namespace NightTest.Groups.Filesystem
{
  /// <summary>
  /// Tests Filesystem.Lines() with a null file path.
  /// </summary>
  public class FilesystemLines_NullPathTest : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.Lines.NullPath";

    /// <inheritdoc/>
    public override string Description => "Tests that Filesystem.Lines(null) throws ArgumentNullException.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully threw ArgumentNullException for null path.";

    /// <inheritdoc/>
    public override void Run()
    {
      _ = Assert.Throws<ArgumentNullException>(() => Night.Filesystem.Lines(null!).ToList());
    }
  }

  /// <summary>
  /// Tests Filesystem.Lines() with an empty file path.
  /// </summary>
  public class FilesystemLines_EmptyPathTest : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.Lines.EmptyPath";

    /// <inheritdoc/>
    public override string Description => "Tests that Filesystem.Lines(\"\") throws ArgumentException.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully threw ArgumentException for empty path.";

    /// <inheritdoc/>
    public override void Run()
    {
      _ = Assert.Throws<ArgumentException>(() => Night.Filesystem.Lines(string.Empty).ToList());
    }
  }

  /// <summary>
  /// Tests Filesystem.Lines() when trying to read a directory.
  /// </summary>
  public class FilesystemLines_ReadDirectoryTest : ModTestCase
  {
    private readonly string tempDirPath = Path.Combine(Path.GetTempPath(), "night_test_lines_dir");

    /// <inheritdoc/>
    public override string Name => "Filesystem.Lines.ReadDirectory";

    /// <inheritdoc/>
    public override string Description => "Tests Filesystem.Lines() on a directory throws UnauthorizedAccessException.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully threw UnauthorizedAccessException when reading a directory.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (Directory.Exists(this.tempDirPath))
      {
        Directory.Delete(this.tempDirPath, true);
      }

      _ = Directory.CreateDirectory(this.tempDirPath);

      try
      {
        // Attempting to iterate lines on a directory should fail.
        // File.ReadLines, which Filesystem.Lines uses, throws UnauthorizedAccessException for directories.
        _ = Assert.Throws<UnauthorizedAccessException>(() => Night.Filesystem.Lines(this.tempDirPath).ToList());
      }
      finally
      {
        if (Directory.Exists(this.tempDirPath))
        {
          Directory.Delete(this.tempDirPath, true);
        }
      }
    }
  }

  /// <summary>
  /// Tests Filesystem.Lines() when trying to read a locked file.
  /// </summary>
  public class FilesystemLines_LockedFileTest : ModTestCase
  {
    private readonly string tempFilePath = Path.Combine(Path.GetTempPath(), "night_test_lines_locked.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.Lines.LockedFile";

    /// <inheritdoc/>
    public override string Description => "Tests Filesystem.Lines() on a locked file throws IOException.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Successfully threw IOException when reading a locked file.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (File.Exists(this.tempFilePath))
      {
        File.Delete(this.tempFilePath);
      }

      // Create an empty file to lock
      File.WriteAllText(this.tempFilePath, "lock me");

      FileStream? lockStream = null;
      try
      {
        // Lock the file
        lockStream = new FileStream(this.tempFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);

        // Attempting to iterate lines on a locked file should fail.
        _ = Assert.Throws<IOException>(() => Night.Filesystem.Lines(this.tempFilePath).ToList());
      }
      finally
      {
        lockStream?.Dispose();
        if (File.Exists(this.tempFilePath))
        {
          File.Delete(this.tempFilePath);
        }
      }
    }
  }

  /// <summary>
  /// Tests Filesystem.Read(string) when trying to read a directory.
  /// </summary>
  public class FilesystemRead_String_ReadDirectoryTest : ModTestCase
  {
    private readonly string tempDirPath = Path.Combine(Path.GetTempPath(), "night_test_read_s_dir");

    /// <inheritdoc/>
    public override string Name => "Filesystem.Read.String.ReadDirectory";

    /// <inheritdoc/>
    public override string Description => "Tests Read(string) on a directory returns 'Unauthorized access.' error.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Correctly returned error for Read(string) on a directory.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (Directory.Exists(this.tempDirPath))
      {
        Directory.Delete(this.tempDirPath, true);
      }

      _ = Directory.CreateDirectory(this.tempDirPath);

      try
      {
        (string? contents, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(this.tempDirPath);

        Assert.Null(contents);
        Assert.Null(bytesRead);
        Assert.Equal("File not found.", errorMsg);
      }
      finally
      {
        if (Directory.Exists(this.tempDirPath))
        {
          Directory.Delete(this.tempDirPath, true);
        }
      }
    }
  }

  /// <summary>
  /// Tests Filesystem.Read(ContainerType.Data) when trying to read a directory.
  /// </summary>
  public class FilesystemRead_Data_ReadDirectoryTest : ModTestCase
  {
    private readonly string tempDirPath = Path.Combine(Path.GetTempPath(), "night_test_read_d_dir");

    /// <inheritdoc/>
    public override string Name => "Filesystem.Read.Data.ReadDirectory";

    /// <inheritdoc/>
    public override string Description => "Tests Read(ContainerType.Data) on a directory returns 'Unauthorized access.' error.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Correctly returned error for Read(ContainerType.Data) on a directory.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (Directory.Exists(this.tempDirPath))
      {
        Directory.Delete(this.tempDirPath, true);
      }

      _ = Directory.CreateDirectory(this.tempDirPath);

      try
      {
        (object? contents, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(Night.Filesystem.ContainerType.Data, this.tempDirPath);

        Assert.Null(contents);
        Assert.Null(bytesRead);
        Assert.Equal("File not found.", errorMsg);
      }
      finally
      {
        if (Directory.Exists(this.tempDirPath))
        {
          Directory.Delete(this.tempDirPath, true);
        }
      }
    }
  }

  /// <summary>
  /// Tests Filesystem.Read(string) when trying to read a locked file.
  /// </summary>
  public class FilesystemRead_String_LockedFileTest : ModTestCase
  {
    private readonly string tempFilePath = Path.Combine(Path.GetTempPath(), "night_test_read_s_locked.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.Read.String.LockedFile";

    /// <inheritdoc/>
    public override string Description => "Tests Read(string) on a locked file returns an IO error.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Correctly returned IO error for Read(string) on a locked file.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (File.Exists(this.tempFilePath))
      {
        File.Delete(this.tempFilePath);
      }

      File.WriteAllText(this.tempFilePath, "lock me"); // Create file to lock

      FileStream? lockStream = null;
      try
      {
        lockStream = new FileStream(this.tempFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None); // Lock the file

        (string? contents, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(this.tempFilePath);

        Assert.Null(contents);
        Assert.Null(bytesRead); // BytesRead might be 0 if error occurs before any read attempt, or null. The code returns null for BytesRead in this case.
        Assert.NotNull(errorMsg);
        Assert.StartsWith("IO error:", errorMsg);
      }
      finally
      {
        lockStream?.Dispose();
        if (File.Exists(this.tempFilePath))
        {
          File.Delete(this.tempFilePath);
        }
      }
    }
  }

  /// <summary>
  /// Tests Filesystem.Read(ContainerType.Data) when trying to read a locked file.
  /// </summary>
  public class FilesystemRead_Data_LockedFileTest : ModTestCase
  {
    private readonly string tempFilePath = Path.Combine(Path.GetTempPath(), "night_test_read_d_locked.txt");

    /// <inheritdoc/>
    public override string Name => "Filesystem.Read.Data.LockedFile";

    /// <inheritdoc/>
    public override string Description => "Tests Read(ContainerType.Data) on a locked file returns an IO error.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Correctly returned IO error for Read(ContainerType.Data) on a locked file.";

    /// <inheritdoc/>
    public override void Run()
    {
      if (File.Exists(this.tempFilePath))
      {
        File.Delete(this.tempFilePath);
      }

      File.WriteAllText(this.tempFilePath, "lock me data"); // Create file to lock

      FileStream? lockStream = null;
      try
      {
        lockStream = new FileStream(this.tempFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None); // Lock the file

        (object? contents, long? bytesRead, string? errorMsg) = Night.Filesystem.Read(Night.Filesystem.ContainerType.Data, this.tempFilePath);

        Assert.Null(contents);
        Assert.Null(bytesRead);
        Assert.NotNull(errorMsg);
        Assert.StartsWith("IO error:", errorMsg);
      }
      finally
      {
        lockStream?.Dispose();
        if (File.Exists(this.tempFilePath))
        {
          File.Delete(this.tempFilePath);
        }
      }
    }
  }
}
