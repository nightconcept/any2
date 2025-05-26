
**Epic 4: Input Handling Implementation**

**Goal:** Implement the `Night.Keyboard` and `Night.Mouse` modules for polling keyboard and mouse states, using the SDL3-CS bindings, to allow the game to respond to user input.

- [x] **Task 4.1:** Implement `Night.Keyboard.IsDown(KeyCode key)` (Status: Completed)
    - [x] Use SDL3-CS functions to get the current keyboard state (e.g., `SDL.SDL_GetKeyboardState(out int numkeys)` which returns a pointer to an array of key states).
    - [x] Define the `Night.KeyCode` enum if not already fully specified in Epic 2, ensuring it can be mapped to SDL's key representation (e.g., `SDL_Scancode` values). This mapping might involve looking up values in SDL3-CS's own enums (like `SDL_Scancode`).
    - [x] Implement the logic to check the state of the specified `Night.KeyCode` by looking up its corresponding SDL scancode in the state array returned by SDL.
    - **Verification:** Calling `Night.Keyboard.IsDown()` with various `Night.KeyCode` values correctly returns `true` when the respective keys are held down and `false` otherwise, as tested in `Night.SampleGame`.

- [x] **Task 4.2:** Implement `Night.Mouse.IsDown(MouseButton button)`
    - [x] Use SDL3-CS functions to get the current mouse button state (e.g., `SDL.SDL_GetMouseState(out float x, out float y)` which typically also returns the button mask).
    - [x] Define the `Night.MouseButton` enum (e.g., `Left`, `Middle`, `Right`, `X1`, `X2`) if not already fully specified in Epic 2.
    - [x] Map `Night.MouseButton` enum values to the SDL button masks (e.g., `SDL.SDL_BUTTON_LMASK`, `SDL.SDL_BUTTON_RMASK`).
    - [x] Implement the logic to check if the specified `Night.MouseButton` is currently pressed by checking the bitmask returned by the SDL mouse state function.
    - [x] **Verification:** Calling `Night.Mouse.IsDown()` with various `Night.MouseButton` values correctly returns `true` when the respective buttons are held down and `false` otherwise, as tested in `Night.SampleGame`.

- [x] **Task 4.3:** Implement `Night.Mouse.GetPosition()` (Status: Completed)
    - [x] Use an SDL3-CS function to get the current mouse cursor coordinates relative to the focused window (e.g., `SDL.SDL_GetMouseState(out float x, out float y)` usually provides coordinates relative to the current window, but verify this behavior with SDL3).
    - [x] Ensure the returned coordinates are cast or converted to `(int x, int y)` as per the `Night` API.
    - **Verification:** Calling `Night.Mouse.GetPosition()` returns the correct (x, y) integer coordinates of the mouse cursor within the game window boundaries.

- [ ] **Task 4.4:** Define and Map `Night.KeyCode` and `Night.MouseButton` Enums
    - [ ] Research and define comprehensive `Night.KeyCode` and `Night.MouseButton` enums that align with common keyboard layouts and mouse buttons, and correspond to SDL3's `SDL_Scancode` and mouse button definitions provided by SDL3-CS.
    - [ ] Create any necessary internal mapping functions or structures if a direct cast is not possible or if `Night` enums need to be more abstract than SDL's.
    - **Verification:** `Night.KeyCode` and `Night.MouseButton` enums are clearly defined and accurately map to the underlying SDL input system values.

- [ ] **Task 4.5:** Basic Error Handling and State Management for Input
    - [ ] Ensure that input functions behave gracefully if called before SDL subsystems are fully initialized (e.g., return default/false values, log a warning). (Note: The main `Night.Engine.Run` should handle initialization order).
    - [ ] Review SDL documentation for any specific error conditions or edge cases for the input functions being used.
    - **Verification:** Input functions do not cause crashes if queried at an inappropriate time (though this should be rare with a proper game loop) and provide default 'safe' return values.

**Epic 5: Love2D API Mapping**

- [ ] **Task 5.1:** Map Love2D API to Night Engine API (Status: In-Progress)
    - [ ] Identify all Love2D modules (e.g., `love.graphics`, `love.audio`, `love.filesystem`, `love` itself, etc.).
    - [ ] For each module, use Context7 to fetch its API functions and documentation.
    - [ ] Create a new markdown file for each Love2D module in the `docs/love2d-api/` directory (e.g., `docs/love2d-api/graphics.md`, `docs/love2d-api/audio.md`).
    - [ ] In each file, list the Love2D functions and propose a corresponding Night Engine API name, following the style implied by the (currently empty) `docs/love2d-api/love2d-api-progress.md`.
    - [ ] Ensure the mapping considers the Night Engine's C# conventions and existing architecture.
    - **Verification:** Markdown files for each Love2D module are created with comprehensive API mappings.
