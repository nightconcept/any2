**Epic 1: Project Setup & SDL3 Integration**

**Goal:** Establish the development environment, project structure as defined in the PRD, and ensure SDL3 native libraries are correctly fetched, linked, and usable by the C# projects.

- [x] **Task 1.1:** Initialize Git Repository & Solution Structure
    - [x] Initialize Git repository with a `.gitignore` file suitable for a .NET project.
    - [x] Create the `Night.sln` solution file.
    - [x] Create the main folder structure: `/docs`, `/scripts`, `/Night.Engine`, `/Night.SampleGame` as per PRD Section 4.
    - [x] Add initial `PRD.md` and a placeholder `TASKS.md` to `/docs`.
    - [x] Add a basic `README.md` to the project root.
    - **Verification:** Repository is cloneable, solution opens in IDE, folder structure matches PRD.

- [x] **Task 1.2:** Update SDL3 Fetching Script (`download_sdl3.py`) (Status: Review)
    - [x] Design the script to download SDL3 native binaries (for Windows, Linux, macOS - x64) from a reliable source (e.g., SDL GitHub releases).
    - [x] Implement logic to extract and place binaries into the `/Night.Engine/runtimes/{os-arch}/` folders. (Windows automated; macOS DMG downloaded with manual instructions; Linux asset not found as per GitHub releases).
    - [x] Implement logic to create/update `/Night.Engine/runtimes/sdl3_version.txt` with the fetched SDL3 version.
    - [x] Add error handling and logging to the script.
    - **Verification:** Running `python scripts/download_sdl3.py` successfully downloads and places SDL3 binaries (Windows), downloads DMG (macOS) with instructions, notes missing Linux asset, and creates the version file as expected.

- [ ] **Task 1.3:** Set up C# Projects (`Night.Engine` & `Night.SampleGame`)
    - [ ] Create `Night.Engine.csproj` as a .NET 9 C# class library.
        - [ ] Configure it to use C# 13.
        - [ ] Ensure it's set up to correctly include/load native binaries from the `/runtimes` folder for multiple platforms.
    - [ ] Create `Night.SampleGame.csproj` as a .NET 9 C# console application.
        - [ ] Configure it to use C# 13.
        - [ ] Add a project reference to `Night.Engine`.
    - [ ] Add basic placeholder C# files (`NightAPI.cs`, `Engine.cs` in `Night.Engine`; `Program.cs`, `Game.cs` in `Night.SampleGame`).
    - **Verification:** Both projects build successfully. `Night.SampleGame` can reference types from `Night.Engine`.
- [ ] **Task:** Initial SDL3 P/Invoke Test
    
    - [ ] In `Night.Engine`, add a P/Invoke declaration for a simple SDL3 function (e.g., `SDL_Init`, `SDL_Quit`, `SDL_GetVersion`).
    - [ ] Call this P/Invoke function from a test method within `Night.Engine` or from `Night.SampleGame`'s `Program.cs`.
    - **Verification:** The P/Invoke call executes without errors (e.g., `DllNotFoundException`), and if applicable, returns expected data (like SDL version). SDL can be initialized and quit.
- [ ] **Task:** Setup Coding Standards Enforcement
    
    - [ ] Create and configure `.editorconfig` at the project root to align with the Google C# Style Guide (indentation, column limit, etc.).
    - [ ] Ensure Roslyn Analyzers are active and configured via `.editorconfig` for style and quality checks.
    - **Verification:** Code formatting tools (`dotnet format`) apply styles consistent with `.editorconfig`. IDE shows warnings/errors based on analyzer settings.