// <copyright file="DummyScenario.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;
using System.Diagnostics; // For Stopwatch
using Night; // For IGame, KeyCode, KeySymbol, MouseButton, Window, Graphics, Color etc.

namespace NightTest.Modules // Placed in a sub-namespace as per convention
{
    public class DummyScenario : ITestScenario, Night.IGame
    {
        private TestRunner? _runner;
        private Stopwatch _stopwatch = new Stopwatch();
        private bool _isDone = false;
        private TestStatus _currentStatus = TestStatus.NotRun;
        private string _details = "Test has not started.";

        // ITestScenario Properties
        public string Name => "Dummy.Example.Scenario";
        public TestType Type => TestType.Automated; // Can be TestType.Manual for a manual dummy
        public string Description => "A dummy scenario that runs for a short duration and then reports success.";

        public void SetTestRunner(TestRunner runner)
        {
            _runner = runner;
        }

        // Night.IGame Methods
        public void Load()
        {
            Console.WriteLine($"[{Type}] {Name}: Load called.");
            _isDone = false;
            _currentStatus = TestStatus.NotRun; // Default to NotRun, explicitly set Pass/Fail
            _details = "Test is running...";
            _stopwatch.Reset();
            _stopwatch.Start();
            // Example: Night.Graphics.SetBackgroundColor(Night.Color.LightGray);
        }

        public void Update(double deltaTime) // IGame.Update takes double
        {
            if (_isDone) return;

            // Simulate some work or condition for an automated test
            if (_stopwatch.ElapsedMilliseconds > 1000) // Let it run for 1 second
            {
                _currentStatus = TestStatus.Passed;
                _details = "Dummy scenario completed successfully after 1 second.";
                _isDone = true;
                // For an automated test, we might want to signal quit to end this specific Framework.Run call
                QuitSelf();
            }
        }

        public void Draw()
        {
            // if (_isDone) return; // Optionally stop drawing when done
            // Example: Night.Graphics.Clear(Night.Color.DarkSlateGray);
            // Night.Graphics.Print($"Running Test: {Name}", 10, 10);
            // Night.Graphics.Print($"Elapsed: {_stopwatch.ElapsedMilliseconds / 1000.0:F2}s", 10, 30);
            // Night.Graphics.Print(_details, 10, 50);
            // if (Type == TestType.Manual && !_isDone) Night.Graphics.Print("Press Space to Pass, Escape to Fail/Quit this test.", 10, 70);
        }

        public void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)
        {
            if (_isDone || isRepeat) return;
            Console.WriteLine($"[{Type}] {Name}: KeyPressed - {scancode}");

            if (Type == TestType.Manual)
            {
                if (scancode == KeyCode.Space) // Assuming KeyCode.Space
                {
                    _currentStatus = TestStatus.Passed;
                    _details = "User pressed Space to pass.";
                    _isDone = true;
                    QuitSelf();
                }
                else if (scancode == KeyCode.Escape) // Assuming KeyCode.Escape
                {
                    _currentStatus = TestStatus.Failed;
                    _details = "User pressed Escape to fail/quit.";
                    _isDone = true;
                    QuitSelf();
                }
            }
            else // For Automated tests, Escape might still be a way to abort
            {
                 if (scancode == KeyCode.Escape)
                {
                    _currentStatus = TestStatus.Failed; // Or Skipped/NotRun if aborted
                    _details = "Test aborted by Escape key.";
                    _isDone = true;
                    QuitSelf();
                }
            }
        }

        public void KeyReleased(KeySymbol key, KeyCode scancode)
        {
            // Not used in this dummy scenario
        }

        public void MousePressed(int x, int y, MouseButton button, bool istouch, int presses)
        {
            // Not used in this dummy scenario
        }

        public void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses)
        {
            // Not used in this dummy scenario
        }

        // This method is part of the IGame pattern, called by Night.Framework.Run usually on window close event
        // OR, we can call it ourselves to terminate this scenario's game loop.
        private void QuitSelf()
        {
            _stopwatch.Stop();
            if (_runner == null)
            {
                 Console.WriteLine($"ERROR in {Name}: TestRunner was not set. Cannot record result.");
                 _currentStatus = TestStatus.Failed;
                 _details += " | TestRunner not available.";
            }
            else
            {
                _runner.RecordResult(Name, Type, _currentStatus, _stopwatch.ElapsedMilliseconds, _details);
            }

            // Ensure Night.Framework.Run() for this scenario exits
            if (Night.Window.IsOpen()) // Check if window is managed by Night.Window
            {
                Night.Window.Close();
            }
            _isDone = true; // Defensive: ensure no more updates try to re-report
        }

        // In this new model, Night.Framework will call Quit() on the IGame instance when it exits.
        // This can happen if the window is closed by user, or if we call Night.Window.Close() ourselves.
        // This is where the scenario should report its final result to the TestRunner.
        // Note: `Framework.cs` does not explicitly call a `Quit()` method on `IGame`. It manages loop by `Window.IsOpen()`.
        // So, the scenario must report its result *before* it causes the window to close or its `Update` loop naturally ends.
        // The `QuitSelf()` method handles this. If we want an explicit `Unload` type method called by an orchestrator *after* Framework.Run finishes, that's different.
        // For now, the scenario is responsible for reporting its result and then closing its own window.
    }
}
