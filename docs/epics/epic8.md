**Epic 8: Migrate SDL C# Bindings to SDL3# NuGet Package**

**Goal:** Successfully integrate the `edwardgushchin/SDL3-CS` (SDL3#) NuGet package into `Night.Engine` to replace the previously removed SDL3 bindings. This includes updating all engine code that interacts with SDL3, revising the native library management strategy, and ensuring the `Night.SampleGame` is fully functional with the new bindings.

- [x] **Task 8.1:** Research and Plan SDL3# Integration Strategy
  **Status: Completed**
    - [x] Thoroughly review the API of the `edwardgushchin/SDL3-CS` (SDL3#) NuGet package. Compare its namespaces (e.g., `SDL3`), static class names (e.g., `SDL`), enums (e.g., for InitFlags, EventType, KeyCode/Scancode, WindowFlags, MouseButton), structs (e.g., `SDL_Event` or its equivalent), and function signatures/return types with what was previously used or expected.
    - [x] Document key differences that will impact existing `Night.Engine` code in files like `FrameworkLoop.cs`, `Modules/Window.cs`, `Modules/Keyboard.cs`, `Modules/Mouse.cs`, `Modules/Graphics.cs`, `Modules/SDL.cs`, and `Types.cs`.
    - [x] **Decision Point:** Determine the strategy for managing native SDL3 libraries with SDL3#:
        - **Option A (Recommended):** Utilize the `SDL3-CS.Native` NuGet package alongside `SDL3-CS`. This would likely replace the current `scripts/update_sdl3.py` and `lib/SDL3-Prebuilt/` system for managing native binaries.
        - **Option B:** Continue managing native binaries manually (e.g., keep `scripts/update_sdl3.py` and `lib/SDL3-Prebuilt/`) if the `SDL3-CS.Native` package is unsuitable or if the existing pre-built binaries from `nightconcept/build-sdl3` (mentioned in `scripts/update_sdl3.py` ) are preferred or customized.
### Migration Plan & API Research (Task 8.1)

**Problem Description:**
The `Night.Engine` currently lacks SDL3 bindings after the removal of the previous `flibitijibibo-sdl3-cs` submodule. The task is to integrate the `edwardgushchin/SDL3-CS` (SDL3#) NuGet package, update all engine code, and decide on a native library management strategy.

**Solution Overview:**
The `edwardgushchin/SDL3-CS` (SDL3#) library, built locally from the submodule in `lib/SDL3-CS`, will be adopted as the new C# binding for SDL3. Native SDL3 libraries will be managed manually (Option B), likely by ensuring `scripts/update_sdl3.py` provides compatible .NET 9 binaries or by updating that script and `lib/SDL3-Prebuilt/`. Engine code will be updated to use the new SDL3# API, focusing on changes in namespaces, static class access, function signatures (especially return types like `bool` for `SDL.Init`), and enum/struct mappings.

**Implementation Steps (Derived from Task 8.1 research):**

1.  **Native Library Management Strategy:**
    *   **Decision:** Adopt **Option B (Modified)**. The `SDL3-CS` C# bindings will be built locally from the submodule (`lib/SDL3-CS`) and referenced as a DLL. Native SDL3 binaries (e.g., `SDL3.dll`, `libSDL3.so`) will be managed manually, likely by updating/using `scripts/update_sdl3.py` and the `lib/SDL3-Prebuilt/` directory to ensure .NET 9 compatible binaries are available. The `SDL3-CS.Native` NuGet package will **not** be used.
    *   **Rationale:** Per user feedback, to use locally built .NET 9 DLLs. This requires manual management of both the C# binding DLL and the underlying native SDL3 binaries.

2.  **Key API Differences and Impact on `Night.Engine`:**

    *   **General:**
        *   **Namespace:** Old bindings might have used a different namespace or none for static P/Invoke style. SDL3# uses the `SDL3` namespace. All relevant files will need `using SDL3;`.
        *   **Static Class:** SDL functions are called via the static `SDL` class (e.g., `SDL.Init()`, `SDL.GetError()`). Previous direct P/Invokes or wrapper classes will need to be updated.
        *   **Return Types:**
            *   `SDL.Init()` now returns `bool` directly, instead of an `int` that `Night.SDL.Init` previously converted from `SDLBool`. [`src/Night.Engine/Modules/SDL.cs`](src/Night.Engine/Modules/SDL.cs:1) will need significant changes here.
            *   Many other functions likely return `bool` for success/failure instead of integer codes. This needs to be checked for each function call being replaced.
        *   **Error Handling:** `SDL.GetError()` remains the standard way to get error messages.

    *   **`FrameworkLoop.cs`:**
        *   Event polling loop: `while (SDL.PollEvent(out var e))` is the new pattern. The `SDL.Event` struct (`e`) is a C# union-like struct.
        *   Event type checking: `(SDL.EventType)e.Type == SDL.EventType.Quit`.

    *   **`Modules/Window.cs`:**
        *   Window creation: Functions like `SDL.CreateWindow()` or `SDL.CreateWindowAndRenderer()` will be used. Parameter types and order, and especially `WindowFlags`, need to be mapped.
        *   `SDL.WindowFlags` (e.g., `SDL.WindowFlags.Fullscreen`, `SDL.WindowFlags.Resizable`) will replace any previous window flag enums. `Night.WindowFlags` in [`src/Night.Engine/Types.cs`](src/Night.Engine/Types.cs:1) will need to be updated or removed if directly using `SDL.WindowFlags`.

    *   **`Modules/Keyboard.cs`:**
        *   Event handling: `SDL.KeyboardEvent` (from `SDL.Event.Key`) will provide key press/release info.
        *   Key codes:
            *   `SDL.Scancode` enum (physical keys, e.g., `SDL.Scancode.A`). This is the likely equivalent if `Night.KeyCode` was based on scancodes.
            *   `SDL.Keycode` enum (virtual keys, layout-dependent, e.g., `SDL.Keycode.A`).
            *   The existing `Night.KeyCode` in [`src/Night.Engine/Types.cs`](src/Night.Engine/Types.cs:1) (which maps to `SDL_Scancode` values) needs to be carefully compared and mapped to `SDL.Scancode`.
        *   Modifier keys: `SDL.Keymod` enum will be used for checking Ctrl, Shift, Alt states, likely part of `SDL.KeyboardEvent`.

    *   **`Modules/Mouse.cs`:**
        *   Event handling: `SDL.MouseButtonEvent` (from `SDL.Event.Button`) for clicks, `SDL.MouseMotionEvent` (from `SDL.Event.Motion`) for movement, `SDL.MouseWheelEvent` (from `SDL.Event.Wheel`) for scroll.
        *   Mouse button identification:
            *   `SDL.MouseButtonEvent.button` will contain raw indices (1 for Left, 2 for Middle, etc., from `SDL.ButtonLeft`, `SDL.ButtonMiddle` constants).
            *   `SDL.GetMouseState()` will return a bitmask of `SDL.MouseButtonFlags` (e.g., `SDL.MouseButtonFlags.Left`).
            *   `Night.MouseButton` in [`src/Night.Engine/Types.cs`](src/Night.Engine/Types.cs:1) will need to be mapped.
        *   Mouse position: Likely available in `SDL.MouseMotionEvent` and via functions like `SDL.GetMouseState()`.

    *   **`Modules/Graphics.cs`:**
        *   Renderer and Window handles: Obtained from `SDL.CreateWindowAndRenderer()` or similar.
        *   Drawing functions: `SDL.RenderClear()`, `SDL.RenderPresent()`, `SDL.SetRenderDrawColor()`, etc., will be used. Signatures need checking.

    *   **`Modules/SDL.cs`:**
        *   This file, which currently wraps/passes through calls, will need substantial updates.
        *   `Init` method needs a complete rewrite due to the `bool` return type.
        *   Other wrapped SDL functions will need to be updated to call `SDL.Function()` from the new bindings.

    *   **`Types.cs`:**
        *   `Night.KeyCode`: Verify and map to `SDL.Scancode` (as per epic note) or `SDL.Keycode` if the intent was virtual keys.
        *   `Night.WindowFlags`: Update to map to/utilize `SDL.WindowFlags` or be replaced by it.
        *   `Night.MouseButton`: Update to map to raw button indices (1,2,3...) or `SDL.MouseButtonFlags` depending on usage context.
        *   Any other SDL-dependent types (e.g., event structs if they were previously exposed differently) will need review.

3.  **Detailed Checklist of Code Sections for Modification (Initial List):**
    *   **Project Files:**
        *   `src/Night.Engine/Night.Engine.csproj`: Remove old binding reference (if any remains). Add a `<ProjectReference>` to the local `SDL3-CS.csproj` (e.g., `../../lib/SDL3-CS/SDL3-CS/SDL3-CS.csproj`). Ensure the `SDL3-CS` submodule is built (e.g., via `dotnet build -c Release` in `lib/SDL3-CS`).
        *   `src/Night.SampleGame/Night.SampleGame.csproj`: Retain or update native library copy steps from `lib/SDL3-Prebuilt/` to ensure the correct native SDL3 binaries are copied to the output directory.
    *   **`src/Night.Engine/FrameworkLoop.cs`:**
        *   Event polling loop (`SDL.PollEvent`, `SDL.Event`, `SDL.EventType`).
        *   Quit event handling.
    *   **`src/Night.Engine/Modules/SDL.cs`:**
        *   `Init()` method.
        *   `Quit()` method.
        *   `GetError()` wrapping.
        *   All other SDL function wrappers.
    *   **`src/Night.Engine/Modules/Window.cs`:**
        *   Window creation functions (e.g., `CreateWindow`).
        *   Window property functions (title, size, position, flags).
        *   Usage of `WindowFlags`.
    *   **`src/Night.Engine/Modules/Keyboard.cs`:**
        *   Key press/release detection.
        *   Mapping/usage of `Night.KeyCode` with `SDL.Scancode` / `SDL.Keycode`.
        *   Modifier key state.
    *   **`src/Night.Engine/Modules/Mouse.cs`:**
        *   Mouse button press/release detection.
        *   Mapping/usage of `Night.MouseButton`.
        *   Mouse motion/position retrieval.
        *   Mouse wheel event handling.
    *   **`src/Night.Engine/Modules/Graphics.cs`:** (Primarily SDL function call updates)
        *   `Clear`
        *   `Present`
        *   `SetDrawColor`
        *   Any other rendering calls.
    *   **`src/Night.Engine/Types.cs`:**
        *   `Night.KeyCode` enum definition and mapping.
        *   `Night.WindowFlags` enum definition and mapping.
        *   `Night.MouseButton` enum definition and mapping.
        *   Any other internal structs/enums that wrap or mirror SDL types.
    *   **All files using SDL functionality:**
        *   Update `using` statements to `using SDL3;`.
        *   Change direct P/Invokes or old wrapper calls to `SDL.FunctionName()`.
        *   Adapt to new struct/enum names and function signatures.

**Risks/Challenges:**
*   **API Completeness:** Ensuring SDL3# provides all necessary functions that were used from the previous bindings. The "Readiness" table in SDL3# README is promising.
*   **Subtle Behavioral Changes:** Differences in how SDL3 itself or the new bindings handle certain edge cases or return values compared to the old setup.
*   **Enum/Struct Mapping:** Correctly mapping existing `Night.*` enums/structs to their new SDL3# counterparts (e.g., `KeyCode` to `Scancode` vs. `Keycode`, `MouseButton` values). This requires careful attention to the previous intent.
*   **Build/Runtime Issues with Native Libraries:** Manual management of native SDL3 binaries requires ensuring the correct versions (compatible with .NET 9 and the locally built SDL3-CS) are obtained (e.g., via `scripts/update_sdl3.py`) and correctly placed/copied for both `Night.Engine` and `Night.SampleGame` to run. This includes potential cross-platform considerations if testing on multiple OS.
*   **SDL3-CS Submodule Build:** The `lib/SDL3-CS` submodule must be successfully built before `Night.Engine` can reference its output.
- - [x] Create a detailed checklist of specific code sections in `Night.Engine` that will require modification.
    - [x] **Verification:** A clear migration plan is documented, including the chosen native library strategy (Option A or B) and a comprehensive list of anticipated code changes and potential challenges.
- [ ] **Task 8.2:** Branch for Migration (If not already on a dedicated branch)
    
    - [ ] Ensure work is being done on a new feature branch in Git (e.g., `feat/migrate-sdl3sharp-bindings`).
    - [ ] Confirm the current project state (with old bindings removed) is committed.
    - **Verification:** Work is being done on a dedicated Git branch.
[ ] **Task 8.3:** Integrate Locally Built `SDL3-CS` (SDL3#) Library

- [ ] Confirm that any `<ProjectReference>` to the old `flibitijibibo-sdl3-cs` submodule has been removed from `src/Night.Engine/Night.Engine.csproj`.
- [ ] Ensure the `lib/SDL3-CS` submodule is updated and buildable (e.g., `git submodule update --init --recursive`, then `dotnet build -c Release` within `lib/SDL3-CS`).
- [ ] Add a `<ProjectReference>` to the local `SDL3-CS.csproj` (e.g., `../../lib/SDL3-CS/SDL3-CS/SDL3-CS.csproj`) in `src/Night.Engine/Night.Engine.csproj`.
- [ ] Verify that native SDL3 binaries (e.g., `SDL3.dll` for Windows) are correctly managed (e.g., via `scripts/update_sdl3.py` and `lib/SDL3-Prebuilt/`) and accessible by `Night.Engine` and `Night.SampleGame`.
- - **Verification:** `Night.Engine` and `Night.SampleGame` projects restore and build successfully, referencing the locally built `SDL3-CS.dll`. The application can locate and load the native SDL3 binaries at runtime.
- [ ] **Task 8.4:** Update `Night.Engine` Code to Utilize SDL3# Bindings
    
    - [ ] Referencing the checklist from Task 8.1, systematically update all C# files within `Night.Engine` that previously interacted with SDL3.
    - [ ] Modify `using` statements if the namespace structure or static class access of SDL3# differs. The target seems to be `using SDL3;` and then `SDL.FunctionName()`.
    - [ ] Update all calls to SDL functions, enums, structs, and constants to match the API provided by the new `SDL3-CS` (SDL3#) package. This will involve careful comparison of function signatures, parameter types, return types (e.g., `SDL_GetError()` vs. `SDL.GetError()`), and naming conventions (e.g., `SDL_INIT_VIDEO` from `flibitijibibo-sdl3-cs` vs. `SDL.InitFlags.Video` in the SDL3# example).
    - [ ] Pay special attention to `Night.Types.cs`; ensure `Night.KeyCode`, `Night.WindowFlags`, etc., correctly map to or utilize the new SDL3# enum values. The existing `KeyCode` enum, for example, maps directly to `SDL_Scancode` values, which will need verification against the new bindings.
    - [ ] Refactor or rewrite `src/Night.Engine/Modules/SDL.cs` to correctly wrap or pass through calls to the new SDL3# API. For instance, the existing `Night.SDL.Init` converts `SDLBool` to `int`; this will need to adapt to SDL3#'s `SDL.Init` which directly returns a `bool`.
- - **Verification:** `Night.Engine` compiles successfully against the new SDL3# bindings without any errors.
- [ ] **Task 8.5:** Test `Night.SampleGame` and Refactor for Compatibility
    
    - [ ] Build and run the `Night.SampleGame` project.
    - [ ] Address any compilation errors or runtime issues that arise due to changes in the SDL3 bindings or the `Night.Engine` API.
    - [ ] Thoroughly test all existing functionalities of the sample game (window creation, input handling, graphics rendering (currently stubbed), game loop operation ) to ensure they perform as expected with the new bindings.
	- [ ] **Verification:** `Night.SampleGame` builds, runs, and all features previously demonstrated (or stubbed out and called) function correctly with the migrated SDL3# bindings.
- [ ] **Task 8.6:** Update Project Documentation (PRD)

- [ ] In `docs/PRD.md`, update Section 3 ("Technical Specifications") to list `edwardgushchin/SDL3-CS` (SDL3#) NuGet package as the C# binding for SDL3, removing mention of `flibitijibibo-sdl3-cs`.
[ ] If the native library management strategy changed (e.g., by adopting `SDL3-CS.Native`):

- Update PRD Section 4 ("Project Structure") to reflect the removal of `lib/SDL3-CS` (already done by user) and potentially `lib/SDL3-Prebuilt/` and `scripts/update_sdl3.py`.
- - - Update PRD Section 5 ("File Descriptions") accordingly.
    - **Verification:** The `docs/PRD.md` accurately reflects the new SDL3 binding dependency and the chosen native library management strategy.
- [ ] **Task 8.7:** Clean Up or Update Native Library Management System
    
    - [ ] If `SDL3-CS.Native` (or an equivalent mechanism from the new SDL3# package) is adopted and successfully manages native SDL3 binaries (Option A from Task 8.1):
        - [ ] Delete the `scripts/update_sdl3.py` script.
        - [ ] Delete the `lib/SDL3-Prebuilt/` directory and its contents (including `version.txt` ).
[ ] Remove the `<Content Include...>` items from `src/Night.SampleGame/Night.SampleGame.csproj` that copied native libraries from `lib/SDL3-Prebuilt/`.
- [ ] If manual management of native libraries is retained (Option B from Task 8.1), ensure the `scripts/update_sdl3.py` script is still functional or update it as necessary to provide compatible binaries for the new SDL3# bindings.
- **Verification:** The project's method for handling native SDL3 libraries is clean, consistent with the chosen strategy, and functional. All obsolete files, scripts, and project configurations related to the old system are removed or appropriately updated.