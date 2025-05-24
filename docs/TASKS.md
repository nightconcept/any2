**Epic 3: Window Management Implementation**

**Goal:** Fully implement the `Night.Window` module's public API (as stubbed in Epic 2) for creating, configuring, and managing the application window, using the SDL3-CS bindings.

- [ ] **Task:** Implement `Night.Window.SetMode(int width, int height, WindowFlags flags)`
    - [ ] Use SDL3-CS functions to create an SDL window (e.g., `SDL.SDL_CreateWindow()`).
        - [ ] Ensure the window is created with the specified `width` and `height`.
        - [ ] Map the `Night.WindowFlags` (e.g., for fullscreen, resizable, borderless) to the corresponding SDL window flags or subsequent SDL function calls (e.g., `SDL.SDL_SetWindowFullscreen()`, `SDL.SDL_SetWindowResizable()`).
    - [ ] If a default renderer is conceptually tied to the window in your design (common for 2D), create an SDL renderer (e.g., `SDL.SDL_CreateRenderer()`) associated with the window.
        - [ ] Store the SDL window handle (and renderer handle, if applicable) internally within a private static part of `Night.Window`.
    - [ ] Handle any necessary SDL initialization for video subsystems (`SDL.SDL_InitSubSystem(SDL.SDL_INIT_VIDEO)`) if not already handled globally.
    - **Verification:** Calling `Night.Window.SetMode()` from `Night.SampleGame` successfully creates and displays a window with the specified dimensions and properties (e.g., fullscreen, resizable). No SDL errors are reported.
- [ ] **Task:** Implement `Night.Window.SetTitle(string title)`

    - [ ] Use the appropriate SDL3-CS function to set the window's title (e.g., `SDL.SDL_SetWindowTitle()`), using the stored window handle.
    - **Verification:** Calling `Night.Window.SetTitle()` from `Night.SampleGame` changes the title displayed in the window's title bar.
- [ ] **Task:** Implement `Night.Window.IsOpen()` (Initial Implementation)

    - [ ] This method's primary role is to control the game loop. For now, its state will likely be tied to whether a `Quit` event has been received (which will be handled more fully in Epic 6: Game Loop).
    - [ ] Create an internal static boolean flag (e.g., `_isWindowOpen` or `_isRunning`, default to `false` until `SetMode` is called, then `true`). `IsOpen()` will return this flag's value. The game loop (Epic 6) will set this to `false` on a quit event.
    - **Verification:** The `Night.Window.IsOpen()` method can be called and returns `true` after a window is created, and its state can be conceptually altered (though full quit logic is later).
- [ ] **Task:** Implement Basic Error Handling for Window Operations

    - [ ] For all SDL3-CS function calls made within `Night.Window` methods, check their return values for errors (e.g., null pointers for window/renderer handles, negative values for error codes).
    - [ ] If an SDL error occurs, retrieve the error message (e.g., using `SDL.SDL_GetError()`).
    - [ ] Log errors using a simple mechanism for the prototype (e.g., `Console.WriteLine($"Error in {methodName}: {SDL.SDL_GetError()}");`).
    - [ ] Decide on an error strategy for the prototype (e.g., throw an exception, return a boolean success/failure from `Night` API methods).
    - **Verification:** Invalid operations (e.g., setting title on a non-existent window if possible, or SDL internal errors) are caught and reported via console logs. The application behaves predictably (e.g., doesn't crash silently if window creation fails).
