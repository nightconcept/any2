// <copyright file="JoysticksGroup.cs" company="Night Circle">
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

namespace NightTest.Groups.Joysticks
{
  /// <summary>
  /// Test group for Joystick related functionalities.
  /// </summary>
  [Collection("SequentialTests")] // Important for tests that interact with the game window
  public class JoysticksGroup : TestGroup
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="JoysticksGroup"/> class.
    /// </summary>
    /// <param name="outputHelper">The xUnit test output helper.</param>
    public JoysticksGroup(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    /// <summary>
    /// Runs the manual test case for joystick connection and disconnection events.
    /// </summary>
    [Fact]
    [Trait("TestType", "Manual")]
    public void Run_JoystickConnectionEventsTest()
    {
      this.Run_GameTestCase(new JoystickConnectionEventsTest());
    }
  }
}
