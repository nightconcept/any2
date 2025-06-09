// <copyright file="NightFileTests.cs" company="Night Circle">
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
using System.Text;

using Night;

using NightTest.Core;

using Xunit;

namespace NightTest.Groups.Filesystem
{
  /// <summary>
  /// Tests the constructor of the <see cref="NightFile"/> class.
  /// </summary>
  public class NightFile_Constructor : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.NightFile.Constructor";

    /// <inheritdoc/>
    public override string Description => "Tests constructor validation and initial state.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Constructor validated successfully.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange, Act & Assert
      _ = Assert.Throws<ArgumentNullException>(() => new NightFile(null!));
      _ = Assert.Throws<ArgumentNullException>(() => new NightFile(string.Empty));

      var filename = "test.txt";
      var file = new NightFile(filename);

      Assert.Equal(filename, file.Filename);
      Assert.False(file.IsOpen);
    }
  }

  /// <summary>
  /// Tests basic open and close functionality of the <see cref="NightFile"/> class.
  /// </summary>
  public class NightFile_OpenClose : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.NightFile.OpenClose";

    /// <inheritdoc/>
    public override string Description => "Tests basic open and close functionality.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Open and Close functionality works as expected.";

    /// <inheritdoc/>
    public override void Run()
    {
      var tempFile = Path.GetTempFileName();
      try
      {
        var file = new NightFile(tempFile);
        Assert.False(file.IsOpen, "File should not be open initially.");

        // Test Open(Night.FileMode)
        var (openSuccess, openError) = file.Open(Night.FileMode.Read);
        Assert.True(openSuccess, $"Open should succeed: {openError}");
        Assert.True(file.IsOpen, "File should be open after Open().");

        var (closeSuccess, closeError) = file.Close();
        Assert.True(closeSuccess, $"Close should succeed: {closeError}");
        Assert.False(file.IsOpen, "File should be closed after Close().");

        // Test Open(string)
        (openSuccess, openError) = file.Open("r");
        Assert.True(openSuccess, $"Open with 'r' should succeed: {openError}");
        Assert.True(file.IsOpen, "File should be open after Open('r').");

        (closeSuccess, closeError) = file.Close();
        Assert.True(closeSuccess, $"Close should succeed again: {closeError}");
        Assert.False(file.IsOpen, "File should be closed again.");

        // Test closing an already closed file
        (closeSuccess, closeError) = file.Close();
        Assert.True(closeSuccess, $"Closing an already closed file should succeed: {closeError}");
      }
      finally
      {
        File.Delete(tempFile);
      }
    }
  }

  /// <summary>
  /// Tests opening files in various modes (r, w, a) with the <see cref="NightFile"/> class.
  /// </summary>
  public class NightFile_OpenModes : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.NightFile.OpenModes";

    /// <inheritdoc/>
    public override string Description => "Tests opening files in various modes (r, w, a).";

    /// <inheritdoc/>
    public override string SuccessMessage => "File open modes (read, write, append) behave correctly.";

    /// <inheritdoc/>
    public override void Run()
    {
      var nonexistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      var existingFile = Path.GetTempFileName();
      var fileContent = "Hello, Night!";
      File.WriteAllText(existingFile, fileContent);

      try
      {
        // Read Mode
        var file = new NightFile(nonexistentFile);
        var (success, _) = file.Open(Night.FileMode.Read);
        Assert.False(success, "Opening a nonexistent file for read should fail.");

        file = new NightFile(existingFile);
        (success, var error) = file.Open(Night.FileMode.Read);
        Assert.True(success, $"Opening an existing file for read should succeed: {error}");
        Assert.True(file.IsOpen);
        _ = file.Close();

        // Write Mode
        file = new NightFile(nonexistentFile);
        (success, error) = file.Open(Night.FileMode.Write);
        Assert.True(success, $"Opening a nonexistent file for write should succeed: {error}");
        _ = file.Close();
        Assert.True(File.Exists(nonexistentFile), "File should be created in write mode.");
        File.Delete(nonexistentFile);

        file = new NightFile(existingFile);
        (success, error) = file.Open(Night.FileMode.Write);
        Assert.True(success, $"Opening an existing file for write should succeed: {error}");
        _ = file.Close();
        Assert.Equal(0L, new FileInfo(existingFile).Length);

        // Append Mode
        file = new NightFile(nonexistentFile);
        (success, error) = file.Open(Night.FileMode.Append);
        Assert.True(success, $"Opening a nonexistent file for append should succeed: {error}");
        _ = file.Close();
        Assert.True(File.Exists(nonexistentFile), "File should be created in append mode.");
        File.Delete(nonexistentFile);

        file = new NightFile(existingFile);
        File.WriteAllText(existingFile, fileContent); // Restore content
        (success, error) = file.Open(Night.FileMode.Append);
        Assert.True(success, $"Opening an existing file for append should succeed: {error}");
        _ = file.Close();
        Assert.Equal((long)fileContent.Length, new FileInfo(existingFile).Length);
      }
      finally
      {
        if (File.Exists(nonexistentFile))
        {
          File.Delete(nonexistentFile);
        }

        if (File.Exists(existingFile))
        {
          File.Delete(existingFile);
        }
      }
    }
  }

  /// <summary>
  /// Tests invalid open scenarios for the <see cref="NightFile"/> class.
  /// </summary>
  public class NightFile_OpenInvalidCases : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.NightFile.Open.InvalidCases";

    /// <inheritdoc/>
    public override string Description => "Tests invalid open scenarios like opening an already open or disposed file.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Invalid open scenarios handled correctly.";

    /// <inheritdoc/>
    public override void Run()
    {
      var tempFile = Path.GetTempFileName();
      try
      {
        // Already open
        var file = new NightFile(tempFile);
        _ = file.Open("r");
        var (success, _) = file.Open("r");
        Assert.False(success, "Opening an already open file should fail.");
        _ = file.Close();

        // Disposed
        file = new NightFile(tempFile);
        file.Dispose();
        (success, _) = file.Open("r");
        Assert.False(success, "Opening a disposed file should fail.");

        // Invalid mode string
        file = new NightFile(tempFile);
        (success, var error) = file.Open("xyz");
        Assert.False(success, "Opening with an invalid mode string should fail.");
        Assert.Contains("Invalid file mode string", error!);
      }
      finally
      {
        File.Delete(tempFile);
      }
    }
  }

  /// <summary>
  /// Tests reading the entire content of a file as a string with the <see cref="NightFile"/> class.
  /// </summary>
  public class NightFile_Read_Full : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.NightFile.Read.Full";

    /// <inheritdoc/>
    public override string Description => "Tests reading the entire content of a file as a string.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Reading full file content as string works correctly.";

    /// <inheritdoc/>
    public override void Run()
    {
      var tempFile = Path.GetTempFileName();
      var content = "Line 1\nLine 2";
      File.WriteAllText(tempFile, content, Encoding.UTF8);

      try
      {
        var file = new NightFile(tempFile);

        var (data, error) = file.Read();
        Assert.Null(data);
        Assert.NotNull(error);
        Assert.Contains("File is not open for reading", error);

        _ = file.Open(Night.FileMode.Write);
        (data, error) = file.Read();
        Assert.Null(data);
        Assert.NotNull(error);
        Assert.Contains("File is not open for reading", error);
        _ = file.Close();

        // Re-open in read mode to check content. Note: Opening in Write mode truncates the file.
        // So we now expect it to be empty. Let's first re-write the original content to test the read.
        File.WriteAllText(tempFile, content, Encoding.UTF8);

        _ = file.Open(Night.FileMode.Read);
        (data, error) = file.Read();
        Assert.Null(error);
        Assert.Equal(content, data);

        // Second read should return empty string as cursor is at the end
        (data, error) = file.Read();
        Assert.Null(error);
        Assert.Equal(string.Empty, data);

        _ = file.Close();
      }
      finally
      {
        File.Delete(tempFile);
      }
    }
  }

  /// <summary>
  /// Tests reading all remaining bytes from a file with the <see cref="NightFile"/> class.
  /// </summary>
  public class NightFile_Read_Bytes : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.NightFile.Read.Bytes";

    /// <inheritdoc/>
    public override string Description => "Tests reading all remaining bytes from a file.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Reading all bytes works correctly.";

    /// <inheritdoc/>
    public override void Run()
    {
      var tempFile = Path.GetTempFileName();
      var content = new byte[] { 1, 2, 3, 4, 5 };
      File.WriteAllBytes(tempFile, content);

      try
      {
        var file = new NightFile(tempFile);

        var (data, error) = file.ReadBytes();
        Assert.Null(data);
        Assert.Equal("File is not open for reading.", error);

        _ = file.Open(Night.FileMode.Read);
        (data, error) = file.ReadBytes();
        Assert.Null(error);
        Assert.Equal(content, data);

        // Second read should return empty array
        (data, error) = file.ReadBytes();
        Assert.Null(error);
        Assert.Empty(data!);

        _ = file.Close();

        // Reading after a partial read
        _ = file.Open(Night.FileMode.Read);
        var (partialData, _) = file.ReadBytes(2);
        Assert.Equal(new byte[] { 1, 2 }, partialData);
        (data, error) = file.ReadBytes();
        Assert.Null(error);
        Assert.Equal(new byte[] { 3, 4, 5 }, data);
        _ = file.Close();
      }
      finally
      {
        File.Delete(tempFile);
      }
    }
  }

  /// <summary>
  /// Tests reading a specific number of bytes from a file with the <see cref="NightFile"/> class.
  /// </summary>
  public class NightFile_Read_BytesCounted : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.NightFile.Read.BytesCounted";

    /// <inheritdoc/>
    public override string Description => "Tests reading a specific number of bytes from a file.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Reading a specific number of bytes works correctly.";

    /// <inheritdoc/>
    public override void Run()
    {
      var tempFile = Path.GetTempFileName();
      var content = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      File.WriteAllBytes(tempFile, content);

      try
      {
        var file = new NightFile(tempFile);
        _ = file.Open(Night.FileMode.Read);

        // Read 0 or negative bytes
        var (data, error) = file.ReadBytes(0);
        Assert.Null(error);
        Assert.Empty(data!);

        (data, error) = file.ReadBytes(-5);
        Assert.Null(error);
        Assert.Empty(data!);

        // Read specific number of bytes
        (data, error) = file.ReadBytes(4);
        Assert.Null(error);
        Assert.Equal(new byte[] { 0, 1, 2, 3 }, data);

        // Read more bytes than available
        (data, error) = file.ReadBytes(100);
        Assert.Null(error);
        Assert.Equal(new byte[] { 4, 5, 6, 7, 8, 9 }, data);

        // Read at EOF
        (data, error) = file.ReadBytes(1);
        Assert.Null(error);
        Assert.Empty(data!);

        _ = file.Close();
      }
      finally
      {
        File.Delete(tempFile);
      }
    }
  }

  /// <summary>
  /// Tests the Dispose method and behavior of a disposed <see cref="NightFile"/> object.
  /// </summary>
  public class NightFile_Dispose : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Filesystem.NightFile.Dispose";

    /// <inheritdoc/>
    public override string Description => "Tests the Dispose method and behavior of a disposed object.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Dispose works correctly, and disposed object behaves as expected.";

    /// <inheritdoc/>
    public override void Run()
    {
      var tempFile = Path.GetTempFileName();
      try
      {
        // Dispose on a file that was never opened
        var file = new NightFile(tempFile);
        file.Dispose();
        Assert.False(file.IsOpen);

        // Open a file, then dispose it
        file = new NightFile(tempFile);
        _ = file.Open("r");
        Assert.True(file.IsOpen);
        file.Dispose();
        Assert.False(file.IsOpen, "IsOpen should be false after dispose");

        // Calling methods on a disposed object should fail gracefully
        var (success, error) = file.Open("r");
        Assert.False(success);
        Assert.Equal("Cannot open a disposed file.", error);

        var (data, readError) = file.Read();
        Assert.Null(data);
        Assert.NotNull(readError);

        var (closeSuccess, _) = file.Close();
        Assert.True(closeSuccess, "Close on a disposed file should succeed without error.");

        // Call dispose multiple times
        file.Dispose();
      }
      finally
      {
        File.Delete(tempFile);
      }
    }
  }
}
