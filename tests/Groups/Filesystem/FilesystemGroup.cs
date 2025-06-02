// <copyright file="FilesystemGroup.cs" company="Night Circle">
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

using NightTest.Core;

using Xunit;
using Xunit.Abstractions;

namespace NightTest.Groups.Filesystem
{
  /// <summary>
  /// Contains tests for Filesystem related IGame test cases.
  /// </summary>
  public class FilesystemGroup : TestGroup
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="FilesystemGroup"/> class.
    /// </summary>
    /// <param name="outputHelper">The xUnit test output helper for logging.</param>
    public FilesystemGroup(ITestOutputHelper outputHelper)
      : base(outputHelper)
    {
    }

    // Test methods for Filesystem IGame cases will be added here.
    // For example:
    // [Fact]
    // [Trait("TestType", "Automated")]
    // public void Run_MyFilesystemTest()
    // {
    //   this.Run_TestCase(new MyFilesystemTest());
    // }

    /// <summary>
    /// Runs the FilesystemLines_ReadStandardFileTest IGame instance.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemLines_ReadStandardFileTest()
    {
      this.Run_TestCase(new FilesystemLines_ReadStandardFileTest());
    }

    /// <summary>
    /// Runs the FilesystemLines_ReadEmptyFileTest IGame instance.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemLines_ReadEmptyFileTest()
    {
      this.Run_TestCase(new FilesystemLines_ReadEmptyFileTest());
    }

    /// <summary>
    /// Runs the FilesystemLines_FileNotFoundTest IGame instance.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemLines_FileNotFoundTest()
    {
      this.Run_TestCase(new FilesystemLines_FileNotFoundTest());
    }

    /// <summary>
    /// Runs the FilesystemLines_ReadSingleLineFileTest IGame instance.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemLines_ReadSingleLineFileTest()
    {
      this.Run_TestCase(new FilesystemLines_ReadSingleLineFileTest());
    }
  }
}
