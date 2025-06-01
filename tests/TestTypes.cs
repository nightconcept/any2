// <copyright file="TestTypes.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

namespace NightTest
{
    /// <summary>
    /// Represents the type of a test case.
    /// </summary>
    public enum TestType
    {
        /// <summary>
        /// Automated test.
        /// </summary>
        Automated,

        /// <summary>
        /// Manual test.
        /// </summary>
        Manual,
    }

    /// <summary>
    /// Represents the status of a test case execution.
    /// </summary>
    public enum TestStatus
    {
        /// <summary>
        /// The test has not been run yet.
        /// </summary>
        NotRun,

        /// <summary>
        /// The test completed successfully.
        /// </summary>
        Passed,

        /// <summary>
        /// The test completed with errors.
        /// </summary>
        Failed,

        /// <summary>
        /// The test was intentionally skipped (e.g., due to filtering).
        /// </summary>
        Skipped,
    }
}
