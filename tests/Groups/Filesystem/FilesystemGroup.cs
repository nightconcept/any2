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

using System;
using System.Collections.Generic;

using Night;

using NightTest.Core;

using Xunit;
using Xunit.Abstractions;

namespace NightTest.Groups.Filesystem
{
  /// <summary>
  /// Tests for the Night.Filesystem functionality.
  /// </summary>
  [Collection("SequentialTests")]
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

    /// <summary>
    /// Runs the <see cref="FilesystemLines_ReadStandardFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemLines_ReadStandardFileTest()
    {
      this.Run_ModTestCase(new FilesystemLines_ReadStandardFileTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemLines_ReadEmptyFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemLines_ReadEmptyFileTest()
    {
      this.Run_ModTestCase(new FilesystemLines_ReadEmptyFileTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemLines_FileNotFoundTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemLines_FileNotFoundTest()
    {
      this.Run_ModTestCase(new FilesystemLines_FileNotFoundTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemLines_ReadSingleLineFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemLines_ReadSingleLineFileTest()
    {
      this.Run_ModTestCase(new FilesystemLines_ReadSingleLineFileTest());
    }

    // Tests from FilesystemGetInfoTests.cs

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_NullPath_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_NullPath_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_NullPath_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_EmptyPath_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_EmptyPath_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_EmptyPath_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PathDoesNotExist_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PathDoesNotExist_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PathDoesNotExist_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_FileExists_NoFilter_ReturnsFileInfoTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_FileExists_NoFilter_ReturnsFileInfoTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_FileExists_NoFilter_ReturnsFileInfoTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_FileExists_MatchingFilter_ReturnsFileInfoTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_FileExists_MatchingFilter_ReturnsFileInfoTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_FileExists_MatchingFilter_ReturnsFileInfoTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_FileExists_NonMatchingFilter_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_FileExists_NonMatchingFilter_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_FileExists_NonMatchingFilter_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_DirectoryExists_NoFilter_ReturnsDirectoryInfoTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_DirectoryExists_NoFilter_ReturnsDirectoryInfoTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_DirectoryExists_NoFilter_ReturnsDirectoryInfoTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_DirectoryExists_MatchingFilter_ReturnsDirectoryInfoTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_DirectoryExists_MatchingFilter_ReturnsDirectoryInfoTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_DirectoryExists_MatchingFilter_ReturnsDirectoryInfoTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_DirectoryExists_NonMatchingFilter_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_DirectoryExists_NonMatchingFilter_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_DirectoryExists_NonMatchingFilter_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateNullInfoObject_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateNullInfoObject_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateNullInfoObject_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateValidInfo_FileExists_PopulatesAndReturnsInfoTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateValidInfo_FileExists_PopulatesAndReturnsInfoTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateValidInfo_FileExists_PopulatesAndReturnsInfoTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateValidInfo_DirectoryExists_PopulatesAndReturnsInfoTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateValidInfo_DirectoryExists_PopulatesAndReturnsInfoTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateValidInfo_DirectoryExists_PopulatesAndReturnsInfoTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateValidInfo_PathDoesNotExist_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateValidInfo_PathDoesNotExist_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateValidInfo_PathDoesNotExist_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateWithFilter_NullInfoObject_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateWithFilter_NullInfoObject_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateWithFilter_NullInfoObject_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateWithFilter_FileExists_MatchingFilter_PopulatesInfoTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateWithFilter_FileExists_MatchingFilter_PopulatesInfoTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateWithFilter_FileExists_MatchingFilter_PopulatesInfoTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateWithFilter_FileExists_NonMatchingFilter_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateWithFilter_FileExists_NonMatchingFilter_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateWithFilter_FileExists_NonMatchingFilter_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateWithFilter_DirectoryExists_MatchingFilter_PopulatesInfoTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateWithFilter_DirectoryExists_MatchingFilter_PopulatesInfoTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateWithFilter_DirectoryExists_MatchingFilter_PopulatesInfoTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateWithFilter_DirectoryExists_NonMatchingFilter_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateWithFilter_DirectoryExists_NonMatchingFilter_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateWithFilter_DirectoryExists_NonMatchingFilter_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemGetInfo_PopulateWithFilter_PathDoesNotExist_ReturnsNullTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_PopulateWithFilter_PathDoesNotExist_ReturnsNullTest_Mod()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_PopulateWithFilter_PathDoesNotExist_ReturnsNullTest());
    }

    /// <summary>
    /// Runs the FilesystemGetInfo_SymbolicLinkTest ModTestCase.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetInfo_SymbolicLinkTest()
    {
      this.Run_ModTestCase(new FilesystemGetInfo_SymbolicLinkTest());
    }

    // Tests from ReadWriteTests.cs

    /// <summary>
    /// Runs the <see cref="FilesystemReadBytes_ReadExistingFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemReadBytes_ReadExistingFileTest()
    {
      this.Run_ModTestCase(new FilesystemReadBytes_ReadExistingFileTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemReadText_ReadExistingFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemReadText_ReadExistingFileTest()
    {
      this.Run_ModTestCase(new FilesystemReadText_ReadExistingFileTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemReadBytes_FileNotFoundTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemReadBytes_FileNotFoundTest()
    {
      this.Run_ModTestCase(new FilesystemReadBytes_FileNotFoundTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemReadText_FileNotFoundTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemReadText_FileNotFoundTest()
    {
      this.Run_ModTestCase(new FilesystemReadText_FileNotFoundTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemAppend_AppendToNewFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemAppend_AppendToNewFileTest()
    {
      this.Run_ModTestCase(new FilesystemAppend_AppendToNewFileTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemAppend_AppendToExistingFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemAppend_AppendToExistingFileTest()
    {
      this.Run_ModTestCase(new FilesystemAppend_AppendToExistingFileTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemAppend_PartialDataTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemAppend_PartialDataTest()
    {
      this.Run_ModTestCase(new FilesystemAppend_PartialDataTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemAppend_ArgumentValidationTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemAppend_ArgumentValidationTest()
    {
      this.Run_ModTestCase(new FilesystemAppend_ArgumentValidationTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemRead_String_ReadExistingFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemRead_String_ReadExistingFileTest()
    {
      this.Run_ModTestCase(new FilesystemRead_String_ReadExistingFileTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemRead_Data_ReadExistingFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemRead_Data_ReadExistingFileTest()
    {
      this.Run_ModTestCase(new FilesystemRead_Data_ReadExistingFileTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemRead_FileNotFoundTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemRead_FileNotFoundTest()
    {
      this.Run_ModTestCase(new FilesystemRead_FileNotFoundTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemRead_String_PartialReadTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemRead_String_PartialReadTest()
    {
      this.Run_ModTestCase(new FilesystemRead_String_PartialReadTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemRead_Data_PartialReadTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemRead_Data_PartialReadTest()
    {
      this.Run_ModTestCase(new FilesystemRead_Data_PartialReadTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemRead_EmptyFileTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemRead_EmptyFileTest()
    {
      this.Run_ModTestCase(new FilesystemRead_EmptyFileTest());
    }

    /// <summary>
    /// Runs the <see cref="FilesystemRead_ArgumentValidationTest"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemRead_ArgumentValidationTest()
    {
      this.Run_ModTestCase(new FilesystemRead_ArgumentValidationTest());
    }

    /// <summary>
    /// Runs a test for creating a single new directory.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemCreateDirectory_NewSingleDirTest()
    {
      this.Run_ModTestCase(new FilesystemCreateDirectory_NewSingleDirTest());
    }

    /// <summary>
    /// Runs a test for creating an already existing directory.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemCreateDirectory_ExistingDirTest()
    {
      this.Run_ModTestCase(new FilesystemCreateDirectory_ExistingDirTest());
    }

    /// <summary>
    /// Runs a test for creating nested directories.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemCreateDirectory_NestedDirTest()
    {
      this.Run_ModTestCase(new FilesystemCreateDirectory_NestedDirTest());
    }

    /// <summary>
    /// Runs a test for argument validation in the CreateDirectory method.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemCreateDirectory_ArgumentValidationTest()
    {
      this.Run_ModTestCase(new FilesystemCreateDirectory_ArgumentValidationTest());
    }

    /// <summary>
    /// Runs a test to ensure GetAppdataDirectory returns a valid path.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_FilesystemGetAppdataDirectory_ReturnsValidPathTest()
    {
      this.Run_ModTestCase(new FilesystemGetAppdataDirectory_ReturnsValidPathTest());
    }

    // Tests from NightFileTests.cs

    /// <summary>
    /// Runs the <see cref="NightFile_Constructor"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_NightFile_Constructor_Mod()
    {
      this.Run_ModTestCase(new NightFile_Constructor());
    }

    /// <summary>
    /// Runs the <see cref="NightFile_OpenClose"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_NightFile_OpenClose_Mod()
    {
      this.Run_ModTestCase(new NightFile_OpenClose());
    }

    /// <summary>
    /// Runs the <see cref="NightFile_OpenModes"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_NightFile_OpenModes_Mod()
    {
      this.Run_ModTestCase(new NightFile_OpenModes());
    }

    /// <summary>
    /// Runs the <see cref="NightFile_OpenInvalidCases"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_NightFile_OpenInvalidCases_Mod()
    {
      this.Run_ModTestCase(new NightFile_OpenInvalidCases());
    }

    /// <summary>
    /// Runs the <see cref="NightFile_Read_Full"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_NightFile_Read_Full_Mod()
    {
      this.Run_ModTestCase(new NightFile_Read_Full());
    }

    /// <summary>
    /// Runs the <see cref="NightFile_Read_Bytes"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_NightFile_Read_Bytes_Mod()
    {
      this.Run_ModTestCase(new NightFile_Read_Bytes());
    }

    /// <summary>
    /// Runs the <see cref="NightFile_Read_BytesCounted"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_NightFile_Read_BytesCounted_Mod()
    {
      this.Run_ModTestCase(new NightFile_Read_BytesCounted());
    }

    /// <summary>
    /// Runs the <see cref="NightFile_Dispose"/> mod test case.
    /// </summary>
    [Fact]
    [Trait("TestType", "Automated")]
    public void Run_NightFile_Dispose_Mod()
    {
      this.Run_ModTestCase(new NightFile_Dispose());
    }
  }
}
