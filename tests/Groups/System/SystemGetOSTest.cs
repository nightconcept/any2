// <copyright file="SystemGetOSTest.cs" company="Night Circle">
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

using System.Runtime.InteropServices; // For RuntimeInformation

using NightTest.Core;

using SDL3;

using Xunit;

namespace NightTest.Groups.System
{
  /// <summary>
  /// Tests the <see cref="Night.System.GetOS()"/> method.
  /// </summary>
  public class SystemGetOS_ReturnsCorrectPlatformStringTest : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "System.GetOS.ReturnsCorrectPlatformString";

    /// <inheritdoc/>
    public override string Description => "Tests that Night.System.GetOS() returns a platform string " +
                                         "consistent with .NET's RuntimeInformation and LÖVE API conventions.";

    /// <inheritdoc/>
    public override string SuccessMessage => "Night.System.GetOS() returned a platform string " +
                                             "consistent with .NET's RuntimeInformation and LÖVE API conventions.";

    /// <inheritdoc/>
    public override void Run()
    {
      // Arrange
      string expectedOsString;

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        expectedOsString = "Windows";
      }
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
      {
        expectedOsString = "OS X";
      }

      // TODO: Implement when Android and iOS get supported by Night.
      /*       else if (RuntimeInformation.IsOSPlatform(OSPlatform.Android))
            {
              expectedOsString = "Android";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.IOS))
            {
              expectedOsString = "iOS";
            } */
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
      {
        // If RuntimeInformation identifies the OS as Linux, Night.System.GetOS() (which uses SDL)
        // should return "Android" if SDL specifically identifies it as "Android",
        // otherwise it should return "Linux".
        string sdlPlatform = SDL.GetPlatform();
        if (sdlPlatform == "Android")
        {
          expectedOsString = "Android";
        }
        else
        {
          expectedOsString = "Linux";
        }
      }
      else
      {
        // Fallback for platforms not directly identified by the RuntimeInformation checks above,
        // or to test the passthrough logic of Night.System.GetOS() for unknown SDL platforms.
        string sdlPlatform = SDL.GetPlatform();
        switch (sdlPlatform)
        {
          case "Windows": expectedOsString = "Windows"; break;
          case "Mac OS X": expectedOsString = "OS X"; break;
          case "Linux": expectedOsString = "Linux"; break;
          case "Android": expectedOsString = "Android"; break;
          case "iOS": expectedOsString = "iOS"; break;
          default: expectedOsString = sdlPlatform; break; // Passthrough
        }
      }

      // Act
      string actualOsString = Night.System.GetOS();

      // Assert
      Assert.Equal(expectedOsString, actualOsString);
    }
  }
}
