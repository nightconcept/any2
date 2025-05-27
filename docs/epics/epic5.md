**Epic 5: 2D Graphics & Rendering Implementation**

**Goal:** Implement the core functionalities of the `Night.Graphics` module, enabling the loading of images as sprites and rendering them to the window. This includes screen clearing and handling the presentation of the rendered frame, all utilizing SDL3-CS bindings.

- [X] **Task 5.1:** Implement `Night.Graphics.NewImage(string filePath)` (Status: Review)
    - [X] Use SDL3-CS functions (specifically `SDL3.Image.LoadTexture()`) to load an image from a file path into an SDL Texture. This requires an active SDL Renderer.
    - [X] Refine the `Night.Sprite` class to store the SDL Texture handle, width, and height (queried using `SDL.GetTextureProperties()` and `SDL.GetNumberProperty()`).
    - [X] Implement error handling for file loading (file not found, texture load errors, property query errors, invalid dimensions) and log appropriately. Return `null` if loading fails.
    - **Verification:** Calling `Night.Graphics.NewImage()` with a path to a valid image file (e.g., a PNG) returns a `Night.Sprite` object. This object contains a non-null texture handle and correct width/height attributes. Attempting to load an invalid file results in a clear error message and no crash.

- [X] **Task 5.2:** Implement `Night.Graphics.Draw(Sprite sprite, float x, float y, float rotation = 0, float scaleX = 1, float scaleY = 1, float offsetX = 0, offsetY = 0)` (Status: Review)
    - [X] Use SDL3-CS functions to render the `SDL_Texture` associated with the `Night.Sprite` object (e.g., `SDL.SDL_RenderTexture()` or `SDL.SDL_RenderTextureRotated()`, or similar SDL3 equivalents that support rotation and scaling).
    - [X] Define the source rectangle (to draw the whole texture) and destination rectangle (`SDL_FRect` for float precision) based on the sprite's dimensions and the `x, y` parameters.
    - [X] Apply `rotation` (in degrees), `scaleX`, `scaleY`. The `offsetX` and `offsetY` parameters should define the origin point for these transformations (e.g., if (0,0), top-left; if (sprite.Width/2, sprite.Height/2), center).
    - [X] Ensure the correct SDL Renderer (obtained during window creation) is used for drawing.
    - **Verification:** Calling `Night.Graphics.Draw()` renders the specified `Night.Sprite` at the correct screen position, with the specified rotation and scale applied accurately. The origin of transformation (`offsetX`, `offsetY`) works as expected.

- [X] **Task 5.3:** Implement `Night.Graphics.Clear(Color color)` (Status: Review)
    - [X] Map the `Night.Color` struct (R, G, B, A byte values) to the format required by SDL.
    - [X] Use SDL3-CS functions to set the renderer's current drawing color (e.g., `SDL.SDL_SetRenderDrawColor()`).
    - [X] Use SDL3-CS functions to clear the entire rendering target with the set color (e.g., `SDL.SDL_RenderClear()`).
    - **Verification:** Calling `Night.Graphics.Clear()` fills the game window with the specified `Night.Color`.

- [ ] **Task 5.4:** Conceptualize `Night.Graphics.Present()` (Actual call in Game Loop)
    - [ ] The `Night.Graphics.Present()` method, if called directly by the user, will likely be a conceptual placeholder in this module. The actual call to SDL's frame presentation function (e.g., `SDL.SDL_RenderPresent()`) will be managed by the `Night.Engine`'s main loop (Epic 6) after all `Draw()` calls are complete for a frame.
    - [ ] For now, this method in `Night.Graphics` can be empty or log a debug message indicating it's a placeholder for the engine's render presentation step.
    - **Verification:** The method `Night.Graphics.Present()` exists and can be called without error. (Full visual verification of frame presentation is part of Epic 6).

- [ ] **Task 5.5:** Renderer Initialization and Management
    - [ ] Confirm that the SDL Renderer instance is properly created (typically alongside the SDL Window in Epic 3, e.g., via `SDL.SDL_CreateRenderer()`) and stored internally where `Night.Graphics` methods can access it.
    - [ ] Ensure renderer flags are appropriately set during creation (e.g., for hardware acceleration, vsync if desired by default for the prototype).
    - [ ] Implement logic for destroying the SDL Renderer when the window is closed or the application quits (e.g., `SDL.SDL_DestroyRenderer()`).
    - **Verification:** Graphics operations use a valid, initialized SDL Renderer. The renderer is cleanly destroyed on application exit.
- [ ] **Task 5.6:** Basic Error Handling for Graphics Operations
    
    - [ ] For all relevant SDL3-CS graphics function calls, check return values for errors.
    - [ ] Retrieve and log specific SDL error messages (e.g., using `SDL.SDL_GetError()`) via `Console.WriteLine` or a similar simple logging mechanism for the prototype.
    - **Verification:** Errors during graphics operations (e.g., texture loading failure, issues during rendering calls) are reported with meaningful messages. The application does not crash silently due to graphics errors.
