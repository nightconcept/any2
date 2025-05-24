**Epic 1: Project Setup & SDL3 Integration**

**Goal:** Establish the development environment, project structure as defined in the PRD, and ensure SDL3 native libraries are correctly fetched, linked, and usable by the C# projects.

- [x] **Task 1.0:** Align Project Structure with PRD Section 4 (Revised) (Status: Review)
    - [x] Review current project structure against `docs/PRD.md` Section 4 diagram.
    - [x] Move `Night.Engine/SDL3` submodule to `lib/SDL3-CS`.
    - [x] Remove `Night.Engine/runtimes` directory.
    - [x] Verify no `scripts` directory exists at root (remove if found and not in PRD).
    - [x] Create `lib/TASKS.md`.
    - **Verification:** Project structure matches `docs/PRD.md` Section 4 diagram. `.gitmodules` is updated.
- [x] **Task 1.1:** Initialize Git Repository & Solution Structure
    - [x] Initialize Git repository with a `.gitignore` file suitable for a .NET project.
    - [x] Create the `Night.sln` solution file.
    - [x] Create the main folder structure: `/docs`, `/scripts`, `/Night.Engine`, `/Night.SampleGame` as per PRD Section 4.
    - [x] Add initial `PRD.md` and a placeholder `TASKS.md` to `/docs`.
    - [x] Add a basic `README.md` to the project root.
    - **Verification:** Repository is cloneable, solution opens in IDE, folder structure matches PRD.

- [x] **Task 1.2.1:** Refactor `Platform` Build System and Workflow (Status: Review)
    - [x] Rename `FosterPlatform` to `Night.Platform` in [`src/Night.Platform/CMakeLists.txt`](src/Night.Platform/CMakeLists.txt:0) and update associated variables (e.g., `FOSTER_LIB_NAME` to `NIGHT_LIB_NAME`).
    - [x] Rename `foster_platform.h` to `night_platform.h` and `foster_platform.c` to `night_platform.c`. Update include guards and internal references.
    - [x] Update [`src/Night.Platform/README.md`](src/Night.Platform/README.md:0) to reflect the new naming.
    - [x] Update [` .github/workflows/build-libs.yml`](.github/workflows/build-libs.yml:0) to use `NIGHT_OVERRIDE_TARGET` and reflect any other necessary changes due to renaming.
    - **Verification:** The `Platform` project builds successfully with the new names. The GitHub Actions workflow runs successfully, producing artifacts like `Night.Platform.dll`.

- [x] **Task 1.3:** Set up C# Projects (`Night.Engine` & `Night.SampleGame`) (Status: Review)
    - [x] Create `Night.Engine.csproj` as a .NET 9 C# class library.
    - [x] Configure it to use C# 13.
    - [x] Ensure it's set up to correctly include/load native binaries from the `/runtimes` folder for multiple platforms.
    - [x] Create `Night.SampleGame.csproj` as a .NET 9 C# console application.
    - [x] Configure it to use C# 13.
    - [x] Add a project reference to `Night.Engine`.
    - [x] Add basic placeholder C# files (`API.cs`, `Engine.cs` in `Night.Engine`; `Program.cs`, `Game.cs` in `Night.SampleGame`).
    - **Verification:** Both projects build successfully. `Night.SampleGame` can reference types from `Night.Engine`.

- [x] **Task 1.4:** Initial SDL3 P/Invoke Test (Status: Review)
    - [x] In `Night.Engine` or `Night.SampleGame`, add P/Invoke declarations for simple functions from `src/Night.Platform/` (e.g., for `SDL_Init`, `SDL_Quit`, `SDL_GetVersion` equivalents via `Night.Platform.dll`). (Implemented directly against SDL3.dll in `Program.cs`)
    - [x] Call these P/Invoke functions from `Night.SampleGame`'s `Program.cs`. (Implemented in `Program.cs`)
    - **Verification:** The P/Invoke call executes without errors (e.g., `DllNotFoundException`), and if applicable, returns expected data (like SDL version). SDL can be initialized and quit. (Checked 2025-05-24: `SDL3.dll` copying mechanism via `Night.Engine.csproj` for `win-x64` is correctly configured.)
- [ ] **Task:** Setup Coding Standards Enforcement (Status: In-Progress)

    - [x] Create and configure `.editorconfig` at the project root to align with the Google C# Style Guide (indentation, column limit, `using` directive order, placeholder for Roslyn Analyzers).
    - [x] Updated `.pre-commit-config.yaml` for C# project with `dotnet format` and other standard hooks.
    - [ ] Ensure Roslyn Analyzers are active and *fully* configured via `.editorconfig` for style and quality checks (placeholder added, full configuration pending).
    - **Verification:** Code formatting tools (`dotnet format`) apply styles consistent with `.editorconfig`. IDE shows warnings/errors based on analyzer settings. `.pre-commit` hooks run successfully.

- **Task 1.5:** Integrate `lib/SDL3-CS` Bindings into `Night.Engine` (Status: Review)
    - **Description:** Modify `Night.Engine` to use the C# bindings from `lib/SDL3-CS` for SDL3 interop, and update `Night.SampleGame` to use these new capabilities. This replaces any direct P/Invoke to `SDL3.dll` or reliance on `Night.Platform` for SDL3 functions.
    - **Sub-tasks:**
        - [x] Add a project reference from `src/Night.Engine/Night.Engine.csproj` to `lib/SDL3-CS/SDL3/SDL3.Core.csproj` (or `SDL3.Legacy.csproj` if .NET 8+ is not guaranteed for all targets, though PRD specifies .NET 9).
        - [x] Update `src/Night.Engine/NightAPI.cs` (or a new `NativeMethods.cs` / `SDL3Integration.cs` file) to expose necessary SDL3 functions (e.g., `Init`, `Quit`, `GetVersion`) using the `SDL3-CS` bindings.
        - [x] Remove any direct P/Invoke declarations for SDL3 functions from `src/Night.SampleGame/Program.cs` or other files if they were using `Night.Platform.dll` or `SDL3.dll` directly for these.
        - [x] Update `src/Night.SampleGame/Program.cs` to call the SDL3 functions exposed by `Night.Engine` (which now use `SDL3-CS`).
    - **Verification:** `Night.Engine` and `Night.SampleGame` build successfully. `Night.SampleGame` can initialize and quit SDL, and retrieve version information using the `SDL3-CS` bindings via `Night.Engine`. No direct P/Invokes to `SDL3.dll` (for functions now covered by `Night.Engine`) remain in `Night.SampleGame`.

- **Task 1.6:** Remove `Night.Platform` (Status: In-Progress)
    - **Description:** Remove the `src/Night.Platform` directory and all references to it, as its functionality (primarily SDL3 building and basic interop) is now superseded by `lib/SDL3-CS` and pre-built SDL3 binaries.
    - **Sub-tasks:**
        - [ ] Delete the `src/Night.Platform` directory.
        - [ ] Update or remove ` .github/workflows/build-libs.yml` to eliminate `Night.Platform` build steps.
        - [ ] Remove any references to `Night.Platform` or its output libraries (e.g., `NightPlatform.dll`, `libNightPlatform.so`) from `.csproj` files, `Night.sln`, or other build/configuration files.
        - [ ] Verify that `Night.Engine` and `Night.SampleGame` still build and run correctly using `lib/SDL3-CS` for all SDL3 interactions.
    - **Verification:** The `src/Night.Platform` directory is gone. The project builds and runs without errors. The GitHub Actions workflow, if modified, completes successfully without trying to build `Night.Platform`.
