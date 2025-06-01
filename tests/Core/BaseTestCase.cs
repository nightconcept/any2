// <copyright file="BaseTestCase.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;
using System.Diagnostics;
using Night;

namespace NightTest.Core // Updated namespace
{
    /// <summary>
    /// Abstract base class for test cases to reduce boilerplate.
    /// Implements ITestCase and Night.IGame.
    /// </summary>
    public abstract class BaseTestCase : ITestCase, Night.IGame // Renamed from BaseTestScenario, implements ITestCase
    {
        protected TestRunner? Runner { get; private set; }
        protected Stopwatch TestStopwatch { get; } = new Stopwatch();
        protected bool IsDone { get; set; } = false;
        protected TestStatus CurrentStatus { get; set; } = TestStatus.NotRun;
        protected string Details { get; set; } = "Test has not started.";

        /// <inheritdoc/>
        public abstract string Name { get; }
        /// <inheritdoc/>
        public abstract TestType Type { get; }
        /// <inheritdoc/>
        public abstract string Description { get; }

        /// <inheritdoc/>
        public void SetTestRunner(TestRunner runner)
        {
            Runner = runner;
        }

        /// <summary>
        /// Called when the test case is loaded. Reset state here.
        /// Base implementation starts stopwatch and sets initial status.
        /// Override and call base.Load() if you need custom Load logic before stopwatch starts.
        /// </summary>
        public virtual void Load()
        {
            Console.WriteLine($"[{Type}] {Name}: Load called.");
            IsDone = false;
            CurrentStatus = TestStatus.NotRun;
            Details = "Test is running...";
            TestStopwatch.Reset();
            TestStopwatch.Start();
        }

        /// <summary>
        /// Called every frame to update the test case's logic.
        /// This MUST be implemented by deriving test classes.
        /// </summary>
        /// <param name="deltaTime">Time elapsed since the last frame.</param>
        public abstract void Update(double deltaTime);

        /// <summary>
        /// Called every frame to draw the test case. Default is empty.
        /// </summary>
        public virtual void Draw() { }

        /// <summary>
        /// Called when a key is pressed. Default is empty.
        /// </summary>
        public virtual void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat) { }

        /// <summary>
        /// Called when a key is released. Default is empty.
        /// </summary>
        public virtual void KeyReleased(KeySymbol key, KeyCode scancode) { }

        /// <summary>
        /// Called when a mouse button is pressed. Default is empty.
        /// </summary>
        public virtual void MousePressed(int x, int y, MouseButton button, bool istouch, int presses) { }

        /// <summary>
        /// Called when a mouse button is released. Default is empty.
        /// </summary>
        public virtual void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

        /// <summary>
        /// Helper method to stop the stopwatch, record results, and close the window.
        /// Call this when your test logic determines completion (pass or fail).
        /// Ensure CurrentStatus and Details are set appropriately before calling.
        /// </summary>
        protected virtual void QuitSelf()
        {
            if (IsDone) return;

            TestStopwatch.Stop();
            if (Runner == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR in {Name}: TestRunner was not set. Cannot record result.");
                Console.ResetColor();
                if (CurrentStatus != TestStatus.Failed)
                {
                    CurrentStatus = TestStatus.Failed;
                    Details = "Critical Error: TestRunner not available. " + Details;
                }
            }
            else
            {
                Runner.RecordResult(Name, Type, CurrentStatus, TestStopwatch.ElapsedMilliseconds, Details);
            }

            if (Night.Window.IsOpen())
            {
                Night.Window.Close();
            }
            IsDone = true;
        }
    }
}
