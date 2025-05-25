
# Night Engine - Product Requirements Document (Prototype)

## 1. Introduction

- **Project Idea:** Create a system using C# on top of SDL3, called "Night Engine".
- **Problem/Need:** The primary problem this approach aims to solve is reducing context switching for the developer. Building directly with C# and SDL3 via the "Night" engine will offer a more streamlined workflow compared to methods involving more languages or disparate tools.
- **Prototype Goal:** The main goal for this prototype is to create a development environment or a foundational C# wrapper layer where a developer can leverage SDL3's capabilities primarily through C#, making game or multimedia development easier and more efficient. This involves achieving a comfortable level of C# integration with core SDL3 features, aiming for an API style reminiscent of Love2D, to simplify development by minimizing the need to switch between different programming paradigms or languages.

## 2. Core Features / User Stories

- **Feature 0: Project Foundation & SDL3 Integration Setup**

    - **Description:** Establishes the C# project structure for the "Night" engine, a build process, and a mechanism to automatically fetch/integrate the latest SDL3 library (e.g., from its GitHub repository). This feature underpins the development of all other engine features by ensuring a working development and build environment for "Night" itself.
    - **User Action(s) (from the engine developer's perspective):**
        - Initialize the C# solution and project(s) for the "Night" engine.
        - Implement a script or build process step (e.g., `Workspace_sdl3.py`) to clone/download the SDL3 source or pre-compiled binaries.
        - Configure the C# project(s) for P/Invoke to SDL3, ensuring native library paths are correctly handled.
        - Optionally, set up basic GitHub Actions (or other CI/CD) workflows for building and testing the "Night" engine prototype.
    - **Outcome(s):**
        - A C# solution/project for "Night" exists that can be successfully built.
        - The SDL3 library is reliably fetched and correctly linked by the C# project.
        - The engine developer has a clear process for building the "Night" engine with its SDL3 dependency.
- **Feature 1: Window Management (Love2D Style)**

    - **Description:** Provides capabilities to create, configure, and manage the application window, using the `Night` namespace and aiming for an API style similar to Love2D's `love.window` module.
    - **User Action(s) (from a C# developer's perspective using the wrapper):**
        - Call `Night.Window.SetMode(int width, int height, WindowFlags flags)`
        - Call `Night.Window.SetTitle(string title)`
        - Call `bool Night.Window.IsOpen()`
    - **Outcome(s):**
        - An application window appears with the specified dimensions and properties.
        - The window title can be dynamically changed.
        - The application can control its main loop based on the window's state.
- **Feature 2: Input Handling (Love2D Style)**

    - **Description:** Allows the C# application to poll keyboard and mouse states, or receive input events, using the `Night` namespace and mirroring Love2D's `love.keyboard` and `love.mouse` modules.
    - **User Action(s) (C# developer):**
        - Call `bool Night.Keyboard.IsDown(KeyCode key)`
        - Call `bool Night.Mouse.IsDown(MouseButton button)`
        - Call `(int x, int y) Night.Mouse.GetPosition()`
        - (Alternatively, or in addition, implement event handlers like `Night.KeyPressed(KeyCode key, ...)` as part of Feature 4)
    - **Outcome(s):**
        - The C# application can determine the real-time state of keys/mouse buttons or react to input events.
        - The C# application can get the current mouse position.
- **Feature 3: 2D Sprite Rendering (Love2D Style)**

    - **Description:** Enables loading images and drawing them as sprites, using the `Night` namespace, akin to Love2D's `love.graphics.newImage()` and `love.graphics.draw()`.
    - **User Action(s) (C# developer):**
        - Call `Sprite Night.Graphics.NewImage(string filePath)`
        - Call `Night.Graphics.Draw(Sprite sprite, float x, float y, float rotation = 0, float scaleX = 1, float scaleY = 1, float offsetX = 0, float offsetY = 0)`
        - Call `Night.Graphics.Clear(Color color)` (Typically at the start of the `Night.Draw()` callback)
        - Call `Night.Graphics.Present()` (Typically at the end of the `Night.Draw()` callback, handled by the engine after user's `Draw` finishes)
    - **Outcome(s):**
        - Images are loaded into `Sprite` objects.
        - Sprites are rendered with transformations.
        - The screen is cleared and new frames are presented as part of the game loop.
- **Feature 4: Love2D-Style Game Loop Structure**

    - **Description:** Provides a pre-defined game loop managed by the `Night` engine. The C# developer implements specific callback functions (e.g., `Load`, `Update`, `Draw`, `KeyPressed`) that the engine calls at appropriate times.
    - **User Action(s) (C# developer structures their code this way):**
        - Implement methods within their game class that `Night` will call:
            - `void MyGame.Load()`
            - `void MyGame.Update(double deltaTime)`
            - `void MyGame.Draw()`
            - Optional: `void MyGame.KeyPressed(KeyCode key, bool isRepeat)`, `void MyGame.MousePressed(int x, int y, MouseButton button, int presses)`.
        - Call a main `Night.Engine.Run(MyGameInstance)` or `Night.Engine.Run<MyGameClass>()` to start.
    - **Outcome(s):**
        - The `Night` engine manages the overall loop.
        - Developers structure their game into familiar Love2D lifecycle methods.
        - Input and other engine events can be handled via these callback methods.

## 3. Technical Specifications

- **Primary Language(s):** C# 13 (using .NET 9).
- **Key Frameworks/Libraries:**
    - SDL3 (latest version, fetched as per Feature 0).
    - `flibitijibibo-sdl3-cs` (https://github.com/flibitijibibo/SDL3-CS/): Approved C# bindings for SDL3 (User approved 2025-05-24). To be integrated as a git submodule.
    - No other external runtime libraries are planned for the core "Night" wrapper in this prototype phase.
- **Database (if any):** None for this prototype.
- **Key APIs/Integrations (if any):**
    - Direct P/Invoke integration with the SDL3 native library.
- **Deployment Target (if applicable for prototype):**
    - The "Night" engine will be a C# class library (DLL).
    - A separate C# "Sample Game" project will consume this library to demonstrate its functionality (e.g., a simple platformer).
- **High-Level Architectural Approach:**
    - "Night" will be a C# wrapper library that provides a static API, stylistically similar to Love2D, over the SDL3 native library.
    - It will manage P/Invoke calls to SDL3 internally.
    - The public API of "Night" will be designed for ease of use by C# game developers.
- **Critical Technical Decisions/Constraints:**
    - The public API of "Night" should closely mirror the structure and common function names of the Love2D API where practical and idiomatic for C#. However, specific API names and parameters will be refined during the prototyping process.
    - All interactions with SDL3 will be through C# P/Invoke.
    - A reliable and straightforward mechanism must be established for fetching SDL3 (source or precompiled binaries) and ensuring the native libraries are correctly loaded by C# applications using "Night".
    - The primary focus for this prototype is on simplicity and achieving the core Love2D-like developer experience for the defined features.

## 4. Project Structure

```plaintext
/ any2 (to be renamed to night-engine later)
├── /docs
│   ├── PRD.md
│   └── TASKS.md
├── /src
│   ├── /Night.Engine               # Night C# Class Library Project
│   │   ├── Night.Engine.csproj
│   │   ├── API.cs                  # Main static classes (Night.Window, Night.Graphics, Night.Input, etc.)
│   │   ├── Engine.cs               # Manages the game loop (Night.Engine.Run)
│   │   ├── NativeMethods.cs        # (Placeholder, to be superseded or directory for SDL3-CS bindings)
│   │   ├── Types.cs                # (e.g., Sprite, Color, KeyCode enums, WindowFlags)
│   └── /Night.SampleGame           # C# Project for the sample platformer game
│       ├── Night.SampleGame.csproj
│       ├── Game.cs                 # Implements Night.Load, Night.Update, Night.Draw etc.
│       ├── Program.cs              # Main entry point, calls Night.Engine.Run()
│       └── /assets                 # Game assets (images, sounds for prototype)
│           └── /images
│               └── player.png
├── .gitignore
├── Night.sln                   # Visual Studio Solution File
└── README.md
```
- `/docs`: Contains all project documentation, including this PRD (`PRD.md`) and the upcoming task list (`TASKS.md`).
- `/lib`: Contains all project libraries to be used.
- `/src/Night.Engine`: This is the C# class library project for the "Night" engine itself.
    - `API.cs`: Could hold the primary public static classes that mimic Love2D's modules (e.g., `Night.Window`, `Night.Graphics`, `Night.Keyboard`, `Night.Mouse`).
    - `Engine.cs`: Contains the core game loop logic that will call the user's `Load()`, `Update()`, `Draw()` methods.
    - `NativeMethods.cs` / `/SDL3/`: The C# P/Invoke declarations for SDL3 will be sourced from the `flibitijibibo-sdl3-cs` bindings, located in the `/Night.Engine/SDL3/` subdirectory. The `SDL3.Core.cs` file from this library will provide the actual interop calls.
    - `DataStructures.cs`: Could define various enums (`KeyCode`, `MouseButton`), structs (`Color`, `Rectangle`), or classes (`Sprite`) used by the engine's API.
- `/src/Night.SampleGame`: This is a separate C# project (e.g., a console application) that demonstrates how to use the "Night" engine. It would reference the `Night.Engine` project.
    - `Game.cs`: The main class for the sample game, where you'd implement the `Load()`, `Update()`, `Draw()` and input callback methods.
    - `Program.cs`: The entry point for the sample game application.
    - `/assets`: Contains assets like images or sound files needed for the sample game.
- `Night.sln`: The Visual Studio solution file, now located at the root, that will contain both the `Night.Engine` and `Night.SampleGame` projects.
- `README.md`: The main readme for the project.

## 5. File Descriptions (If applicable)
- **`Night.sln`**:
    - **Purpose:** The Visual Studio Solution file that groups the `Night.Engine` library project and the `Night.SampleGame` project together.
    - **Format:** Standard Visual Studio Solution format (text-based).
    - **Key Contents/Structure:** Defines project paths, configurations (Debug/Release), and dependencies between projects.
- **`Night.Engine/Night.Engine.csproj` and `Night.SampleGame/Night.SampleGame.csproj`**:
    - **Purpose:** These are the MSBuild project files for the engine library and the sample game, respectively. They define how each project is built.
    - **Format:** XML (MSBuild format).
    - **Key Contents/Structure:** Specifies target framework (.NET 9), C# language version, referenced packages, project references (`Night.SampleGame` will reference `Night.Engine`), and how native runtimes are handled.

## 6. Future Considerations / Out of Scope (for this prototype)

**Out of Scope for This Prototype:**

- **Full Love2D API Parity:** While the style is inspired by Love2D, this prototype will only implement a subset of features. Modules like `love.audio`, `love.filesystem`, `love.font`, `love.joystick`, `love.physics`, `love.thread`, etc., are out of scope.
- **Advanced Rendering:** Custom shaders, 3D graphics, complex lighting, particle systems, stencil/scissor operations beyond basic SDL3 capabilities used for 2D sprites.
- **Audio System:** No sound effects or music playback.
- **Font Rendering & Text Display:** The prototype will not support loading fonts or rendering text.
- **Advanced Input:** Joystick/gamepad support beyond what SDL3 might offer for basic key events, touch input, gesture recognition.
- **Physics Engine:** No collision detection or physics simulation capabilities will be part of the engine.
- **Networking Capabilities:** No multiplayer or network communication features.
- **Comprehensive Error Handling:** Error handling will be minimal.
- **Performance Optimization:** The primary goal is functional correctness and API design.
- **Game Packaging/Distribution Tools:** Not included.
- **Editor/GUI Tools:** (Beyond what might be prototyped with ImGui in the future).

**Potential Future Enhancements (Post-Prototype):**

- **Dear ImGui Integration:** Integrate Dear ImGui to facilitate the creation of debug UIs, simple in-game editors, or developer tools directly within "Night" applications.
- **Quake-Style Debug Console:** Implement a toggleable in-game console (e.g., accessed via a tab or backtick key) for runtime command execution, variable inspection/manipulation, and viewing engine/game logs.
- **Entity Component System (ECS) Architecture:** Transition or build the engine's core scene and game object management around an ECS pattern for improved data-oriented design, performance, and flexibility.
- **Audio Module (`Night.Audio`):** Integrate SDL_mixer (or equivalent SDL3 audio functionality) for sound effects and music playback.
- **Font Rendering (`Night.Font`):** Add support for loading fonts (e.g., via SDL_ttf) and rendering text. This would also benefit the debug console and any ImGui interfaces.
- **Expanded Input (`Night.Joystick`, `Night.Touch`):** Full support for game controllers/joysticks and touch interfaces.
- **More Graphics Primitives & Features:** Support for drawing shapes (lines, rectangles, circles), basic shader integration, and a more advanced camera system, potentially designed with ECS in mind.
- **Filesystem Abstraction (`Night.Filesystem`):** A simple, cross-platform API for file I/O.
- **Timing Module (`Night.Timer`):** More advanced control over timing and FPS management.
- **Lua Scripting Interface:** To more closely mirror Love2D's primary development language.
- **Improved Error Handling & Debugging Tools:** Building upon the debug console.
- **Performance Profiling and Optimization.**
- **Expanded Platform Support.**
- **Community Building:** Tutorials, more example projects, and comprehensive API documentation.
