**Epic 6: Game Loop Implementation** `Status: In-Progress`

**Goal:** Implement the `Night.Engine` class to manage the main game loop. This includes initializing and shutting down SDL, polling for events (especially quit events), calling the user-defined `Load`, `Update`, and `Draw` methods in the correct sequence, managing frame timing (delta time), and handling screen presentation.

- [x] **Task 6.1:** Implement Core `Night.Engine.Run(IGame gameLogic)` Structure `Status: Review`

  - [x] Define an interface (e.g., `Night.IGame`) that user game classes will implement, specifying methods like `Load()`, `Update(double deltaTime)`, `Draw()`, and optional input event handlers like `KeyPressed(KeyCode key, bool isRepeat)` etc., if this event-based approach is chosen for Feature 4. (Verified: Exists in `Types.cs`)
  - [x] Implement the main `Night.Engine.Run(IGame gameLogic)` method. (Verified: Implemented in `FrameworkLoop.cs`)
  - [x] **Initialization:**
    - [x] Inside `Run`, before the loop, call `SDL.SDL_Init(SDL.SDL_INIT_VIDEO | ...other_subsystems_if_needed...)` using SDL3-CS. Log errors if initialization fails. (Verified: `SDL.Init(SDL.InitFlags.Video | SDL.InitFlags.Events)` in `FrameworkLoop.cs`)
    - [x] Call the provided `gameLogic.Load()` method once after successful SDL initialization. (Verified: Implemented in `FrameworkLoop.cs`)
  - [x] **Main Loop:**
    - [x] Implement the primary game loop (e.g., `while (Night.Window.IsOpen()) { ... }`). The `Night.Window.IsOpen()` flag (from Epic 3) will be controlled by quit events. (Verified: Implemented in `FrameworkLoop.cs`)
  - [x] **Shutdown:**
    - [x] After the loop terminates, call appropriate SDL cleanup functions (e.g., destroy window, destroy renderer if not handled elsewhere, `SDL.SDL_QuitSubSystem(...)`, `SDL.SDL_Quit()`). (Verified: `Window.Shutdown()` and `SDL.Quit()` in `FrameworkLoop.cs`)
  - **Verification:** Calling `Night.Engine.Run()` with a simple `IGame` implementation initializes SDL, calls `Load()`, enters a loop, and then quits SDL. The `Night.SampleGame` can be launched using this. (Verified: `SampleGame/Program.cs` updated)

- [x] **Task 6.2:** Implement Event Polling within the Game Loop `Status: Review`
  - [x] Inside the main loop, use SDL3-CS functions to poll for SDL events (e.g., `while (SDL.SDL_PollEvent(out SDL_Event ev) != 0) { ... }`). (Verified: Implemented in `FrameworkLoop.cs`)
  - [x] Handle `SDL_EVENT_QUIT`: If this event is received, set the internal flag that `Night.Window.IsOpen()` checks to `false` to terminate the game loop. (Verified: Implemented in `FrameworkLoop.cs` via `Window.Close()`)
  - [x] **Task 6.2.1:** Implement `IGame.KeyPressed` callback `Status: Review`
    - [x] Add `KeyPressed(Night.KeySymbol key, Night.KeyCode scancode, bool isRepeat)` method to the `Night.IGame` interface. (Verified: Implemented in `Types.cs` with new `KeySymbol` enum)
    - [x] In `FrameworkLoop.cs`, when an `SDL.EventType.KeyDown` event occurs, call `game.KeyPressed` with mapped parameters. (Verified: Implemented in `FrameworkLoop.cs` using `KeySymbol`)
    - [x] Implement `KeyPressed` in `SampleGame` to demonstrate functionality (e.g., log key presses or quit on Escape). (Verified: Implemented in `SampleGame/Program.cs` using `KeySymbol`)
    - **Verification:** Pressing keys in the `SampleGame` triggers the `KeyPressed` callback with correct parameters (correct `KeySymbol` and `KeyCode`), and the sample implementation (e.g., logging or quitting) works as expected.
  - (Optional for initial prototype, can be basic) If pursuing event-based input handlers from Feature 4:
    - [x] Based on `ev.type`, dispatch to relevant `gameLogic` methods (e.g., `gameLogic.KeyPressed(ev.key.keysym.sym, ...)`). This requires mapping SDL event data to `Night` API parameters. (Addressed by Task 6.2.1 with `KeySymbol`)
  - **Verification:** The game loop correctly polls for events. The application closes cleanly when the window's close button is clicked (which generates an `SDL_EVENT_QUIT`). If basic event handlers are implemented, they are triggered. The `KeyPressed` callback is now a basic event handler with corrected key symbol reporting.

- [x] **Task 6.3:** Implement Delta Time Calculation and Pass to `Update` `Status: Review`
  - [x] Before the main loop, get initial timing values using SDL3-CS timing functions (e.g., `SDL.SDL_GetPerformanceCounter()` and `SDL.SDL_GetPerformanceFrequency()` for high-resolution timing, or `SDL.SDL_GetTicks()` for millisecond-based timing).
  - [x] At the beginning of each loop iteration (or end), calculate the time elapsed since the last frame (`deltaTime`) in seconds (e.g., as a `double` or `float`).
  - [x] Call `gameLogic.Update(deltaTime)` method, passing the calculated delta time.
  - **Verification:** The `gameLogic.Update()` method is called each frame and receives a `deltaTime` value that reasonably reflects the actual time elapsed per frame. Frame rate can be roughly monitored (e.g., by logging FPS) for stability.

- [x] **Task 6.4:** Integrate `gameLogic.Draw()` and Screen Presentation `Status: Review`
  - [ ] Inside the main loop, after `gameLogic.Update(deltaTime)`, call `gameLogic.Draw()`.
  - [ ] Immediately after `gameLogic.Draw()` completes, call the SDL function to present the renderer's back buffer to the window (e.g., `SDL.SDL_RenderPresent(rendererHandle)` using the renderer handle established in Epic 3/5).
  - **Verification:** The `gameLogic.Draw()` method is called each frame. Graphics drawn within this method (using `Night.Graphics` calls) are visible on the screen and update frame by frame.

- [x] **Task 6.5:** Basic Game Loop Error Handling and Robustness `Status: Review`
  - [x] Wrap calls to user-provided `gameLogic` methods (`Load`, `Update`, `Draw`, event handlers) in `try-catch` blocks. (Verified: `Load` covered by main try-catch; `Update`, `Draw`, `KeyPressed` have specific try-catch blocks in `FrameworkLoop.cs`)
  - [x] If an exception occurs in user code, log the exception (e.g., `Console.WriteLine`) and decide on a strategy: (Verified: Exceptions are logged, and `Window.Close()` is called to terminate loop gracefully for `Update`, `Draw`, `KeyPressed`. `Load` exceptions also lead to cleanup.)
    - For prototype: an unhandled exception in user code might gracefully terminate the engine loop and ensure SDL is shut down. (Verified)
  - [x] Ensure SDL initialization and shutdown are robust (e.g., `SDL_Quit` is always called even if `Load` throws an error). (Verified: Handled by the main `try-finally` block in `FrameworkLoop.cs`)
  - **Verification:** Unhandled exceptions within the `gameLogic` methods are caught by the `Night.Engine`, an error is logged, and the engine attempts to shut down SDL and exit cleanly rather than crashing without context.
