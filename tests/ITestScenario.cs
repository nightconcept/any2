// <copyright file="ITestScenario.cs" company="Night Circle">
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
using System.IO;

using Night;

using SDL3;

namespace NightTest;

/// <summary>
/// Main game class for the platformer sample.
/// Implements the <see cref="IGame"/> interface for Night.Engine integration.
/// </summary>
public interface ITestScenario
{
    /// <summary>
    /// Gets the unique name of the test scenario.
    /// Used for reporting.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the type of the test (Automated or Manual).
    /// Used for filtering and reporting.
    /// </summary>
    TestType Type { get; }

    /// <summary>
    /// Gets a brief description of what the test scenario covers.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Sets the TestRunner instance for this scenario to use for reporting results.
    /// This will be called by the orchestrator (Program.cs) before the scenario is run.
    /// </summary>
    /// <param name="runner">The central TestRunner instance.</param>
    void SetTestRunner(TestRunner runner);
}
