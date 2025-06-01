// <copyright file="TimerModule.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Night;

namespace NightTest.Modules
{
    /// <summary>
    /// A module that provides test scenarios for the Night.Timer class.
    /// </summary>
    public class TimerModule : ITestModule
    {
        /// <inheritdoc/>
        public IEnumerable<ITestScenario> GetTestScenarios()
        {
            return new List<ITestScenario>
            {
                new GetTimeTest(),
                new GetFPSTest(),
                new GetDeltaTest(),
                new GetAverageDeltaTest(),
                new SleepTest(),
                new StepTest(),
            };
        }
    }

    /// <summary>
    /// Tests the Timer.GetTime() method.
    /// </summary>
    public class GetTimeTest : ITestScenario, Night.IGame
    {
        private TestRunner? _runner;
        private Stopwatch _stopwatch = new Stopwatch();
        private bool _isDone = false;
        private TestStatus _currentStatus = TestStatus.NotRun;
        private string _details = "Test has not started.";
        private double _startTime = 0;
        private double _endTime = 0;

        /// <inheritdoc/>
        public string Name => "Timer.GetTime";
        /// <inheritdoc/>
        public TestType Type => TestType.Automated;
        /// <inheritdoc/>
        public string Description => "Tests the Night.Timer.GetTime() method by measuring time passage.";

        /// <inheritdoc/>
        public void SetTestRunner(TestRunner runner)
        {
            _runner = runner;
        }

        /// <inheritdoc/>
        public void Load()
        {
            Console.WriteLine($"[{Type}] {Name}: Load called.");
            _isDone = false;
            _currentStatus = TestStatus.NotRun;
            _details = "Test is running...";
            _stopwatch.Reset();
            _stopwatch.Start();

            _startTime = Night.Timer.GetTime();
            Console.WriteLine($"[{Type}] {Name}: Initial time from Timer.GetTime(): {_startTime:F6} seconds.");
        }

        /// <inheritdoc/>
        public void Update(double deltaTime)
        {
            if (_isDone) return;

            if (_stopwatch.ElapsedMilliseconds > 500) // Run for 0.5 second
            {
                _endTime = Night.Timer.GetTime();
                Console.WriteLine($"[{Type}] {Name}: End time from Timer.GetTime(): {_endTime:F6} seconds.");
                double elapsed = _endTime - _startTime;
                _currentStatus = TestStatus.Passed;
                _details = $"Timer.GetTime() test completed. Start: {_startTime:F6}s, End: {_endTime:F6}s. Elapsed: {elapsed:F6}s (Expected ~0.5s).";
                _isDone = true;
                QuitSelf();
            }
        }

        /// <inheritdoc/>
        public void Draw() { }
        /// <inheritdoc/>
        public void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat) { }
        /// <inheritdoc/>
        public void KeyReleased(KeySymbol key, KeyCode scancode) { }
        /// <inheritdoc/>
        public void MousePressed(int x, int y, MouseButton button, bool istouch, int presses) { }
        /// <inheritdoc/>
        public void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

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
            if (Night.Window.IsOpen())
            {
                Night.Window.Close();
            }
            _isDone = true;
        }
    }

    /// <summary>
    /// Tests the Timer.GetFPS() method.
    /// </summary>
    public class GetFPSTest : ITestScenario, Night.IGame
    {
        private TestRunner? _runner;
        private Stopwatch _stopwatch = new Stopwatch();
        private bool _isDone = false;
        private TestStatus _currentStatus = TestStatus.NotRun;
        private string _details = "Test has not started.";
        private int _frameCount = 0;

        /// <inheritdoc/>
        public string Name => "Timer.GetFPS";
        /// <inheritdoc/>
        public TestType Type => TestType.Automated;
        /// <inheritdoc/>
        public string Description => "Tests the Night.Timer.GetFPS() method by observing its value over a short period.";

        /// <inheritdoc/>
        public void SetTestRunner(TestRunner runner)
        {
            _runner = runner;
        }

        /// <inheritdoc/>
        public void Load()
        {
            Console.WriteLine($"[{Type}] {Name}: Load called.");
            _isDone = false;
            _currentStatus = TestStatus.NotRun;
            _details = "Test is running...";
            _stopwatch.Reset();
            _stopwatch.Start();
            _frameCount = 0;
        }

        /// <inheritdoc/>
        public void Update(double deltaTime)
        {
            if (_isDone) return;

            _frameCount++;
            int currentFps = Night.Timer.GetFPS();
            // Console.WriteLine($"[{Type}] {Name}: Current FPS from Timer.GetFPS(): {currentFps}");

            if (_frameCount > 10 && _stopwatch.ElapsedMilliseconds > 200) // Run for a bit, after a few frames
            {
                currentFps = Night.Timer.GetFPS(); // Get one last reading
                _currentStatus = TestStatus.Passed;
                _details = $"Timer.GetFPS() test observed. Last reported FPS: {currentFps}. Test ran for >200ms and >10 frames.";
                _isDone = true;
                QuitSelf();
            }
        }
        /// <inheritdoc/>
        public void Draw() { }
        /// <inheritdoc/>
        public void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat) { }
        /// <inheritdoc/>
        public void KeyReleased(KeySymbol key, KeyCode scancode) { }
        /// <inheritdoc/>
        public void MousePressed(int x, int y, MouseButton button, bool istouch, int presses) { }
        /// <inheritdoc/>
        public void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

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
            if (Night.Window.IsOpen())
            {
                Night.Window.Close();
            }
            _isDone = true;
        }
    }

    /// <summary>
    /// Tests the Timer.GetDelta() method.
    /// </summary>
    public class GetDeltaTest : ITestScenario, Night.IGame
    {
        private TestRunner? _runner;
        private Stopwatch _stopwatch = new Stopwatch();
        private bool _isDone = false;
        private TestStatus _currentStatus = TestStatus.NotRun;
        private string _details = "Test has not started.";
        private List<float> _deltas = new List<float>();
        private int _frameCount = 0;

        /// <inheritdoc/>
        public string Name => "Timer.GetDelta";
        /// <inheritdoc/>
        public TestType Type => TestType.Automated;
        /// <inheritdoc/>
        public string Description => "Tests the Night.Timer.GetDelta() method by collecting delta values.";

        /// <inheritdoc/>
        public void SetTestRunner(TestRunner runner)
        {
            _runner = runner;
        }

        /// <inheritdoc/>
        public void Load()
        {
            Console.WriteLine($"[{Type}] {Name}: Load called.");
            _isDone = false;
            _currentStatus = TestStatus.NotRun;
            _details = "Test is running...";
            _stopwatch.Reset();
            _stopwatch.Start();
            _deltas.Clear();
            _frameCount = 0;
        }

        /// <inheritdoc/>
        public void Update(double deltaTime)
        {
            if (_isDone) return;
            _frameCount++;
            float currentDelta = Night.Timer.GetDelta();
            _deltas.Add(currentDelta);
            // Console.WriteLine($"[{Type}] {Name}: Current Delta from Timer.GetDelta(): {currentDelta:F6}");

            if (_frameCount > 10 && _stopwatch.ElapsedMilliseconds > 200)
            {
                float averageDelta = _deltas.Count > 0 ? _deltas.Average() : 0f;
                _currentStatus = TestStatus.Passed;
                _details = $"Timer.GetDelta() test collected {_deltas.Count} values. Average delta: {averageDelta:F6}. Test ran for >200ms and >10 frames.";
                _isDone = true;
                QuitSelf();
            }
        }

        /// <inheritdoc/>
        public void Draw() { }
        /// <inheritdoc/>
        public void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat) { }
        /// <inheritdoc/>
        public void KeyReleased(KeySymbol key, KeyCode scancode) { }
        /// <inheritdoc/>
        public void MousePressed(int x, int y, MouseButton button, bool istouch, int presses) { }
        /// <inheritdoc/>
        public void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

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
            if (Night.Window.IsOpen())
            {
                Night.Window.Close();
            }
            _isDone = true;
        }
    }

    /// <summary>
    /// Tests the Timer.GetAverageDelta() method.
    /// </summary>
    public class GetAverageDeltaTest : ITestScenario, Night.IGame
    {
        private TestRunner? _runner;
        private Stopwatch _stopwatch = new Stopwatch();
        private bool _isDone = false;
        private TestStatus _currentStatus = TestStatus.NotRun;
        private string _details = "Test has not started.";
        private int _frameCount = 0;

        /// <inheritdoc/>
        public string Name => "Timer.GetAverageDelta";
        /// <inheritdoc/>
        public TestType Type => TestType.Automated;
        /// <inheritdoc/>
        public string Description => "Tests the Night.Timer.GetAverageDelta() method.";

        /// <inheritdoc/>
        public void SetTestRunner(TestRunner runner)
        {
            _runner = runner;
        }

        /// <inheritdoc/>
        public void Load()
        {
            Console.WriteLine($"[{Type}] {Name}: Load called.");
            _isDone = false;
            _currentStatus = TestStatus.NotRun;
            _details = "Test is running...";
            _stopwatch.Reset();
            _stopwatch.Start();
            _frameCount = 0;
        }

        /// <inheritdoc/>
        public void Update(double deltaTime)
        {
            if (_isDone) return;
            _frameCount++;
            double avgDelta = Night.Timer.GetAverageDelta();
            // Console.WriteLine($"[{Type}] {Name}: Current Average Delta from Timer.GetAverageDelta(): {avgDelta:F6}");

            if (_frameCount > 10 && _stopwatch.ElapsedMilliseconds > 200)
            {
                avgDelta = Night.Timer.GetAverageDelta(); // Get one last reading
                _currentStatus = TestStatus.Passed;
                _details = $"Timer.GetAverageDelta() observed. Last reported value: {avgDelta:F6}. Test ran for >200ms and >10 frames.";
                _isDone = true;
                QuitSelf();
            }
        }

        /// <inheritdoc/>
        public void Draw() { }
        /// <inheritdoc/>
        public void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat) { }
        /// <inheritdoc/>
        public void KeyReleased(KeySymbol key, KeyCode scancode) { }
        /// <inheritdoc/>
        public void MousePressed(int x, int y, MouseButton button, bool istouch, int presses) { }
        /// <inheritdoc/>
        public void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

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
            if (Night.Window.IsOpen())
            {
                Night.Window.Close();
            }
            _isDone = true;
        }
    }

    /// <summary>
    /// Tests the Timer.Sleep() method.
    /// </summary>
    public class SleepTest : ITestScenario, Night.IGame
    {
        private TestRunner? _runner;
        private Stopwatch _stopwatch = new Stopwatch();
        private Stopwatch _internalStopwatch = new Stopwatch();
        private bool _isDone = false;
        private TestStatus _currentStatus = TestStatus.NotRun;
        private string _details = "Test has not started.";
        private const double SleepDurationSeconds = 0.25; // Sleep for 250ms

        /// <inheritdoc/>
        public string Name => "Timer.Sleep";
        /// <inheritdoc/>
        public TestType Type => TestType.Automated;
        /// <inheritdoc/>
        public string Description => $"Tests the Night.Timer.Sleep() method by sleeping for {SleepDurationSeconds}s.";

        /// <inheritdoc/>
        public void SetTestRunner(TestRunner runner)
        {
            _runner = runner;
        }

        /// <inheritdoc/>
        public void Load()
        {
            Console.WriteLine($"[{Type}] {Name}: Load called. Will attempt to sleep for {SleepDurationSeconds}s.");
            _isDone = false;
            _currentStatus = TestStatus.NotRun;
            _details = "Test is running...";
            _stopwatch.Reset();
            _stopwatch.Start(); // Overall test duration

            _internalStopwatch.Reset();
            _internalStopwatch.Start();
            Night.Timer.Sleep(SleepDurationSeconds);
            _internalStopwatch.Stop();

            double elapsedMs = _internalStopwatch.ElapsedMilliseconds;
            Console.WriteLine($"[{Type}] {Name}: Timer.Sleep({SleepDurationSeconds}) took {elapsedMs}ms.");

            // Allow a small margin for timing inaccuracies
            if (elapsedMs >= SleepDurationSeconds * 1000 * 0.9 && elapsedMs <= SleepDurationSeconds * 1000 * 1.5)
            {
                _currentStatus = TestStatus.Passed;
                _details = $"Timer.Sleep({SleepDurationSeconds}) executed. Measured duration: {elapsedMs}ms. Expected ~{SleepDurationSeconds * 1000}ms.";
            }
            else
            {
                _currentStatus = TestStatus.Failed;
                _details = $"Timer.Sleep({SleepDurationSeconds}) executed. Measured duration: {elapsedMs}ms. Expected ~{SleepDurationSeconds * 1000}ms. Deviation too large.";
            }
            _isDone = true;
            QuitSelf(); // This test finishes in Load
        }

        /// <inheritdoc/>
        public void Update(double deltaTime) { /* Test finishes in Load */ }
        /// <inheritdoc/>
        public void Draw() { }
        /// <inheritdoc/>
        public void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat) { }
        /// <inheritdoc/>
        public void KeyReleased(KeySymbol key, KeyCode scancode) { }
        /// <inheritdoc/>
        public void MousePressed(int x, int y, MouseButton button, bool istouch, int presses) { }
        /// <inheritdoc/>
        public void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

        private void QuitSelf()
        {
            _stopwatch.Stop(); // Stops overall test timer
            if (_runner == null)
            {
                Console.WriteLine($"ERROR in {Name}: TestRunner was not set. Cannot record result.");
                _currentStatus = TestStatus.Failed;
                _details += " | TestRunner not available.";
            }
            else
            {
                // Use _internalStopwatch for actual sleep duration if available, otherwise overall.
                long reportedDuration = _internalStopwatch.IsRunning ? _stopwatch.ElapsedMilliseconds : _internalStopwatch.ElapsedMilliseconds;
                if(reportedDuration == 0 && _stopwatch.ElapsedMilliseconds > 0) reportedDuration = _stopwatch.ElapsedMilliseconds;

                _runner.RecordResult(Name, Type, _currentStatus, reportedDuration, _details);
            }
            if (Night.Window.IsOpen())
            {
                Night.Window.Close();
            }
            _isDone = true;
        }
    }

    /// <summary>
    /// Tests the Timer.Step() method.
    /// </summary>
    public class StepTest : ITestScenario, Night.IGame
    {
        private TestRunner? _runner;
        private Stopwatch _stopwatch = new Stopwatch();
        private bool _isDone = false;
        private TestStatus _currentStatus = TestStatus.NotRun;
        private string _details = "Test has not started.";
        private int _stepCount = 0;
        private List<double> _stepDeltas = new List<double>();

        /// <inheritdoc/>
        public string Name => "Timer.Step";
        /// <inheritdoc/>
        public TestType Type => TestType.Automated;
        /// <inheritdoc/>
        public string Description => "Tests the Night.Timer.Step() method by calling it multiple times and observing delta values.";

        /// <inheritdoc/>
        public void SetTestRunner(TestRunner runner)
        {
            _runner = runner;
        }

        /// <inheritdoc/>
        public void Load()
        {
            Console.WriteLine($"[{Type}] {Name}: Load called.");
            _isDone = false;
            _currentStatus = TestStatus.NotRun;
            _details = "Test is running...";
            _stopwatch.Reset();
            _stopwatch.Start();
            _stepCount = 0;
            _stepDeltas.Clear();

            // Call Step once in Load to initialize its internal state if needed, similar to how framework would.
            // The result of this first call might be less predictable.
            Night.Timer.Step();
        }

        /// <inheritdoc/>
        public void Update(double deltaTime) // deltaTime here is from Framework's step
        {
            if (_isDone) return;

            // We are testing Timer.Step(), which is also what the framework uses.
            // To get an independent measure, we can call it again or rely on the fact
            // that the framework calls it and updates Timer.CurrentDelta.
            // For this test, let's call it explicitly a few times.
            // However, calling Timer.Step() multiple times per frame might not be its intended use.
            // The framework calls Timer.Step() once per frame. This test will simulate a few frames.

            double stepDelta = Night.Timer.Step(); // This is the primary value we are interested in from calling Step()
            _stepDeltas.Add(stepDelta);
            // Console.WriteLine($"[{Type}] {Name}: Timer.Step() returned: {stepDelta:F6}. Framework deltaTime: {deltaTime:F6}");
            _stepCount++;


            if (_stepCount > 10 && _stopwatch.ElapsedMilliseconds > 200)
            {
                // The values from Timer.Step() should be similar to Timer.GetDelta() after framework updates.
                double averageStepDelta = _stepDeltas.Count > 0 ? _stepDeltas.Average() : 0.0;
                _currentStatus = TestStatus.Passed;
                _details = $"Timer.Step() called {_stepCount} times. Average delta from Step(): {averageStepDelta:F6}. Test ran for >200ms.";
                _isDone = true;
                QuitSelf();
            }
        }
        /// <inheritdoc/>
        public void Draw() { }
        /// <inheritdoc/>
        public void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat) { }
        /// <inheritdoc/>
        public void KeyReleased(KeySymbol key, KeyCode scancode) { }
        /// <inheritdoc/>
        public void MousePressed(int x, int y, MouseButton button, bool istouch, int presses) { }
        /// <inheritdoc/>
        public void MouseReleased(int x, int y, MouseButton button, bool istouch, int presses) { }

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
            if (Night.Window.IsOpen())
            {
                Night.Window.Close();
            }
            _isDone = true;
        }
    }
}
