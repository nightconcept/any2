// <copyright file="Joysticks.cs" company="Night Circle">
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

namespace Night.Joysticks
{
  /// <summary>
  /// Provides functionality for managing and querying joysticks.
  /// Corresponds to LÖVE's `love.joystick` module.
  /// </summary>
  public static class Joysticks
  {
    /// <summary>
    /// Gets a list of connected Joysticks.
    /// Corresponds to `love.joystick.getJoysticks()`.
    /// </summary>
    /// <returns>A list of currently connected <see cref="Joystick"/> objects.</returns>
    public static List<Joystick> GetJoysticks()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the number of connected joysticks.
    /// Corresponds to `love.joystick.getJoystickCount()`.
    /// (Renamed from `love.joystick.getNumJoysticks` in LÖVE).
    /// </summary>
    /// <returns>The number of connected joysticks.</returns>
    public static int GetJoystickCount()
    {
      throw new NotImplementedException();
    }
  }
}
