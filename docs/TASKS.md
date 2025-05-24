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
- [x] **Task 1.2.1:** Refactor `Platform` Build System and Workflow (Status: Review)
    - [x] Rename `FosterPlatform` to `Night.Platform` in [`Platform/CMakeLists.txt`](Platform/CMakeLists.txt:0) and update associated variables (e.g., `FOSTER_LIB_NAME` to `NIGHT_LIB_NAME`).
    - [x] Rename `foster_platform.h` to `night_platform.h` and `foster_platform.c` to `night_platform.c`. Update include guards and internal references.
    - [x] Update [`Platform/README.md`](Platform/README.md:0) to reflect the new naming.
    - [x] Update [` .github/workflows/build-sdl3.yml`](.github/workflows/build-sdl3.yml:0) to use `NIGHT_OVERRIDE_TARGET` and reflect any other necessary changes due to renaming.
    - **Verification:** The `Platform` project builds successfully with the new names. The GitHub Actions workflow runs successfully, producing artifacts like `Night.Platform.dll`.
- [ ] **Task 1.2.2:** Integrate `flibitijibibo-sdl3-cs` Bindings (Status: In-Progress, Partially Blocked)
    - User approved `flibitijibibo-sdl3-cs` (https://github.com/flibitijibibo/SDL3-CS/) on 2025-05-24.
    - [x] Add `flibitijibibo-sdl3-cs` as a git submodule to `/Night.Engine/SDL3`. (Submodule files appear to be cloned; .gitmodules updated).
    - [ ] Record the specific submodule commit hash used. (Action: Pending - git commands interrupted; may require manual check or retry after environment stabilization).
    - [ ] Integrate `SDL3.Core.cs` (and its `LICENSE` file) from the submodule into the `Night.Engine.csproj` for compilation. (Action: Blocked by Task 1.3 - `Night.Engine.csproj` does not exist yet).
    - **Verification:** `Night.Engine` compiles successfully. The `SDL3.Core.cs` file is included in the `Night.Engine` project and compiled. The git submodule is correctly added and initialized. (Verification: Partially pending due to blocked/pending actions).

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
- [ ] **Task:** Setup Coding Standards Enforcement (Status: In-Progress)
    
    - [x] Create and configure `.editorconfig` at the project root to align with the Google C# Style Guide (indentation, column limit, `using` directive order, placeholder for Roslyn Analyzers).
    - [x] Updated `.pre-commit-config.yaml` for C# project with `dotnet format` and other standard hooks.
    - [ ] Ensure Roslyn Analyzers are active and *fully* configured via `.editorconfig` for style and quality checks (placeholder added, full configuration pending).
    - **Verification:** Code formatting tools (`dotnet format`) apply styles consistent with `.editorconfig`. IDE shows warnings/errors based on analyzer settings. `.pre-commit` hooks run successfully.