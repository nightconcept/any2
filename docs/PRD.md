# Night Engine - Product Requirements Document

## 1. Introduction

- **Project Idea:** Create a layered system for game and multimedia development in C#. This system consists of "Night.Framework," a low-level, Love2D-inspired API built directly on SDL3 (via SDL3-CS bindings), and "Night.Engine," a higher-level, more opinionated set of tools and systems that will be built on top of Night.Framework.
- **Problem/Need:** The primary goal is to provide a streamlined and efficient development workflow for C# developers. Night.Framework offers direct, SDL3-powered capabilities with a familiar API style, reducing context switching compared to using disparate tools or multiple languages directly. Night.Engine (to be developed later) will further simplify complex game development tasks by providing common engine systems.
- **Development Goal:** The main goal for the current development phase is to establish a robust Night.Framework, providing a foundational C# wrapper layer where a developer can leverage SDL3's capabilities primarily through C#. This involves achieving a comfortable level of C# integration with core SDL3 features, aiming for an API style reminiscent of Love2D, to simplify development. Future phases will focus on building out Night.Engine components.


## 2. Core Features

This section outlines the core modules and functionalities of **Night.Framework**, followed by planned features for **Night.Engine**.

**Night.Framework (Love2D-style API):**
- **Feature 0: Project Foundation & SDL3 Integration:**
    - **Description:** Establishes the C# project structure for Night.Framework and Night.Engine, a build process, and utilizes the SDL3-CS C# bindings for SDL3 integration. This ensures a working development and build environment. SDL3 native libraries are managed via pre-built binaries, potentially updated via a script.

- **Feature 1: Window Management (`Night.Window`):**
    - **Description:** Provides capabilities to create, configure, and manage the application window, using the `Night` namespace and an API style similar to Love2D's `love.window` module. This includes setting mode, title, and checking window state.

- **Feature 2: Input Handling (`Night.Input`, encompassing Keyboard and Mouse):**
    - **Description:** Allows the C# application to poll keyboard and mouse states, or receive input events, using the `Night` namespace and mirroring Love2D's `love.keyboard` and `love.mouse` modules. This includes checking key/button presses and getting mouse position.

- **Feature 3: 2D Graphics Rendering (`Night.Graphics`):**
    - **Description:** Enables loading images and drawing them as sprites, along with basic 2D graphics operations. This module uses the `Night` namespace, akin to Love2D's `love.graphics` module. It leverages SDL_Renderer for its operations. Key functionalities include loading images, drawing sprites with transformations, clearing the screen, and presenting the frame.

- **Feature 4: Game Loop Structure (`Night.Framework.Run`):**
    - **Description:** Provides a pre-defined game loop managed by Night.Framework. The C# developer implements specific callback functions (e.g., `Load`, `Update`, `Draw`) defined in the `Night.IGame` interface that the framework calls at appropriate times.

**Night.Engine** (Future Opinionated Systems - to be detailed in later PRD revisions):
- **Entity Component System (ECS):** A data-oriented framework for managing game objects and logic.
- **Scene Management:** Systems for organizing and managing game scenes.
- **Advanced Asset Management:** More sophisticated tools for handling game assets.
- **Physics Integration (Optional):** Wrappers or integration for a 2D/3D physics engine.
- **(Other high-level engine modules as defined)**


## 3. Technical Specifications
- **Primary Language(s):** C# 13 (using .NET 9).
- **Key Frameworks/Libraries:**
    - SDL3 (latest version). Native libraries managed via the `SDL3-CS.Native` NuGet package.
    - `edwardgushchin/SDL3-CS` (hereafter "SDL3#"): Approved C# bindings for SDL3, integrated as a NuGet package. This library provides access to SDL3 core functions. (Note: SDL_image, SDL_mixer, SDL_ttf are separate considerations if needed later and may require additional NuGet packages or bindings).
    - `SDL3-CS.Native`: NuGet package that provides the native SDL3 binaries.
    - No other external runtime libraries are planned for Night.Framework. Night.Engine may introduce dependencies later.

- **Database (if any):** None for Night.Framework or initial Night.Engine.

- **Key APIs/Integrations:**
    - Direct interaction with the SDL3 native library via the SDL3-CS C# bindings.

- **Rendering Backend:**
    - Night.Framework will utilize SDL_Renderer for 2D graphics operations. This is an internal implementation detail not directly exposed to the game developer using Night.Framework.
    - Future enhancements may explore migrating to SDL_GPU for potential performance benefits or advanced features.

- **Deployment Target:**
    - Night.Framework will be a C# class library (DLL).
    - Night.Engine (when developed) will be a separate C# class library (DLL) that depends on Night.Framework. For now, both Night.Framework and Night.Engine will be in a singular C# class library that is output as Night.Engine.
    - A separate C# "Sample Game" project will consume Night.Framework (and eventually Night.Engine) to demonstrate functionality.
    - **Target Platforms:** The long-term goal is to support Windows, macOS, Linux, iOS, and Android. Console support is a distant stretch goal.

- **High-Level Architectural Approach:**
    - **Night.Framework:** A C# library providing a static API, stylistically similar to Love2D, over the SDL3 native library (via SDL3-CS). It will manage interactions with SDL3-CS internally. The public API of Night.Framework will be designed for ease of use by C# game developers, primarily within the `Night` C# namespace.
    - **Night.Engine:** A C# library that will provide more opinionated game development constructs (e.g., ECS, scene management). It will use Night.Framework for its low-level operations and will not interact with SDL3 directly.

- **Critical Technical Decisions/Constraints:**
    - The public API of Night.Framework should closely mirror the structure and common function names of the Love2D API where practical and idiomatic for C#.
    - All interactions with SDL3 will be through the SDL3# (`edwardgushchin/SDL3-CS`) bindings. Night.Engine will not use SDL3# directly.
    - Native SDL3 libraries are managed via the `SDL3-CS.Native` NuGet package, ensuring a reliable loading mechanism.
    - The primary focus for Night.Framework development is on simplicity, achieving the core Love2D-like developer experience for the defined features, and providing a solid foundation for Night.Engine.


## 4. Project Structure

```
/night-engine (root directory, formerly any2)
|-- .editorconfig
|-- .gitattributes
|-- .github/
|   |-- CODEOWNERS
|   |-- copilot-instructions.md
|   |-- dependabot.yml
|   |-- workflows/
|   |   |-- (active workflows, e.g., build, release)
|   |-- scripts/
|       |-- determine_next_version.py
|-- .gitignore
|-- .pre-commit-config.yaml
|-- LICENSE
|-- Night.sln
|-- README.md
|-- docs/
|   |-- PRD.md (this file)
|   |-- TASKS.md
|   |-- operational-guidelines.md
|   |-- epics/
|   |   |-- epic1.md
|   |   |-- ...
|   |-- love2d-api/
|       |-- audio.md
|       |-- graphics.md
|       |-- ...
|-- lib/  (This directory might be removed if no other pre-built libraries are needed)
|-- src/
    |-- Night.Engine/
    |   |-- Night.Engine.csproj
    |   |-- Framework/
    |   |   |-- Window.cs
    |   |   |-- Graphics/
    |   |       |-- Graphics.cs
    |   |   |-- Keyboard.cs
    |   |   |-- Mouse.cs
    |   |   |-- Input.cs (if Keyboard and Mouse are combined or refactored)
    |   |   |-- ... (other framework modules)
    |   |-- FrameworkLoop.cs  (Manages Night.Framework.Run and game loop)
    |   |-- Types.cs          (Structs and interfaces)
    |   |-- Enums.cs          (Enums)
    |   |-- ... (Placeholder for future engine components: ECS, SceneManager, etc.)
    |-- Night.SampleGame/
        |-- Night.SampleGame.csproj
        |-- Program.cs        (Main entry point, calls Night.Framework.Run, provides IGame)
        |-- assets/
            |-- images/
            |-- sounds/
            |-- ...
```

- `/docs`: Project documentation (PRD, operational guidelines, tasks, API mapping, etc.).
- `/lib`: May contain other external libraries if needed in the future. (SDL3 native binaries and C# bindings are now managed via NuGet).
- `/scripts`: Utility scripts for the project. (`update_sdl3.py` is removed as SDL3 native binaries are managed by `SDL3-CS.Native` NuGet package).
- `/src`: Contains all C# source code.
    - `/src/Night.Engine`: C# class library project for the low-level Love2D-style API and opinionated engine. This project references SDL3-CS.
        - `Night.Engine.csproj`: MSBuild project file.
        - `/Modules/`: Directory containing individual C# files or folders for each Love2D-like module (e.g., `Window.cs`, `Graphics.cs`, `Keyboard.cs`, `Mouse.cs`). These will primarily contain static classes within the `Night` namespace.
        - `FrameworkLoop.cs`: Contains the main game loop logic (`Night.Framework.Run()`) and related infrastructure.
        - `Enums.cs`: Defines enums used by Night.Framework and consumer applications.
        - `Types.cs`: Defines core data types and interfaces (e.g., `Night.Color`, `Night.KeyCode`, `Night.Sprite`, `Night.IGame`) used by Night.Framework and consumer applications.
    - `/src/Night.SampleGame`: C# console application project demonstrating the use of Night.Framework (and eventually Night.Engine).
        - `Night.SampleGame.csproj`: MSBuild project file. References `Night.Framework`.
        - `Game.cs`: Implements `Night.IGame` with game-specific logic (`Load`, `Update`, `Draw`).
        - `Program.cs`: Main entry point for the sample game application.
        - `/assets`: Game assets like images or sound files needed for the sample game.
- `Night.sln`: The Visual Studio solution file, grouping `Night.Framework`, `Night.Engine`, and `Night.SampleGame` projects.
- `README.md`: The main readme for the project.
- `.github/`: GitHub-specific files like workflows and issue templates.
- `.editorconfig`, `.pre-commit-config.yaml`: Code style and pre-commit hook configurations.

## 5. File Descriptions

- **`Night.sln`**: Visual Studio Solution file grouping `Night.Engine`, and `Night.SampleGame` projects. Defines project paths, configurations, and dependencies.
- **`src/Night.Engine/Night.Engine.csproj`**: MSBuild project file for the Night.Framework and Night.Engine library. Defines target framework (.NET 9), C# language version, and references (notably `SDL3-CS` and `SDL3-CS.Native` NuGet packages).
- **`src/Night.SampleGame/Night.SampleGame.csproj`**: MSBuild project file for the sample game application. References `Night.Framework`.


## 6. Future Considerations

**Out of Scope for Initial Night.Framework Development (beyond core Window, Input, Graphics, Game Loop):**

- **Full Love2D API Parity:** Only a subset of Love2D modules and features will be implemented initially in Night.Framework. Modules like `love.audio`, `love.filesystem`, `love.font`, `love.joystick`, `love.thread`, etc., are out of scope for the initial set of framework modules. Some may be added to Night.Framework later, or equivalents might be part of Night.Engine.

- **Advanced Rendering in Night.Framework:** Custom shaders, 3D graphics, complex lighting, particle systems beyond what SDL_Renderer offers for 2D sprites.
- **Audio** System, Font Rendering, Advanced Input (Joysticks, Touch), Physics Engine, Networking Capabilities in **Night.Framework:** These are not targeted for the initial Night.Framework release.
- **Comprehensive Error Handling:** Error handling will be functional but may be expanded later.
- **Performance Optimization:** Initial focus is on functional correctness and API design for Night.Framework.
- **Game Packaging/Distribution Tools, Editor/GUI Tools:** Not included.

**Potential Future Enhancements (Post-initial Night.Framework, potentially as part of Night.Engine or additions to Night.Framework):**

- **Night.Engine Core:**
    - **Entity Component System (ECS) Architecture**.
    - **Scene Management & Scene Graph.**
    - **Advanced Game State Management.**

- **Expanded Night.Framework Modules:**
    - **Audio Module (`Night.Audio`):** Integrate SDL_mixer (or equivalent SDL3 audio) for sound and music.
    - **Font Rendering (`Night.Font`):** Add support for loading fonts and rendering text (e.g., via SDL_ttf).
    - **Expanded Input (`Night.Joystick`, `Night.Touch`):** Support for game controllers and touch interfaces.
    - **More Graphics Primitives & Features:** Drawing shapes, basic shader integration via SDL_Renderer functions, and potentially a more advanced camera system.
    - **Filesystem Abstraction (`Night.Filesystem`):** Cross-platform API for file I/O.
    - **Timing** Module **(`Night.Timer`):** Advanced control over timing and FPS management.

- **Tooling & Developer Experience:**
    - **Dear ImGui Integration:** For debug UIs and simple in-game editors.
    - **Quake-Style Debug Console**.
    - **Lua Scripting Interface (long-term consideration)**.

- **General:**
    - **Improved Error Handling & Debugging Tools**.
    - **Performance Profiling and Optimization.**
    - **Expanded Platform Support Verification (Android, iOS).**
    - **Community Building: Tutorials, more examples, comprehensive documentation**.
