# Epic 2: Core Engine API - Foundations (Leveraging SDL3-CS)

[ ] **Task 2.1:** Define Core `Night` API Data Structures

- [ ] Create C# enums, structs, or classes for the data types exposed by the `Night` API (e.g., `Night.Color`, `Night.Rectangle`, `Night.Key`, `Night.WindowFlags`, a basic `Night.Sprite` class). These will be part of `Night.Engine`'s public interface.
- [ ] For each `Night` data structure, determine if it will directly wrap or map to/from corresponding C# structs/enums provided by SDL3-CS (e.g., SDL3-CS might provide `SDL_Rect`, `SDL_Color`, `SDL_Keycode`, etc.) or if it's a purely `Night`-level concept.

[ ] **Task 2.2:** Integrate and Verify SDL3-CS Bindings

- [ ] Add the SDL3-CS library to the `Night.Engine` project. This might involve:
    - Adding it as a NuGet package if it's published as one.
    - Including its source code or project as a submodule or directly in your solution if preferred/necessary.
- [ ] Review the specific SDL3-CS generated files (e.g., `SDL3.Core.cs` or `SDL3.Legacy.cs` based on your .NET target) to understand the available functions and data types.

[ ] **Task 2.3:** Stub out `Night` Public API Surface (`NightAPI.cs`, `Engine.cs`)

- [x] In `API.cs`, create the static classes `Night.Window`, `Night.Keyboard`, `Night.Mouse`, and `Night.Graphics`.
- [x] Add public method signatures (as stubs, initially throwing `NotImplementedException` or logging) for the API functions outlined in PRD Features 1, 2, and 3.
    - Example: `public static class Night.Window { public static void SetMode(int width, int height, WindowFlags flags) { throw new NotImplementedException(); /* Future: call SDL.SDL_CreateWindow via SDL3-CS */ } }`
- [x] In `Engine.cs`, create the `Night.Engine` class with a public `Run` method (stubbed).
- [x] Define placeholders for how the `Run` method will invoke the user's `Load()`, `Update()`, and `Draw()` methods.
- **Verification:** The stubbed public `Night` API is callable from `Night.SampleGame`. The structure aligns with the PRD. It's clear where SDL3-CS calls will be made in future implementation steps.
