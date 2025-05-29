# Texture Atlas C# Source Generator: Project Overview

## 1. Introduction & Goal

This project will create a C# Source Generator to process JSON texture atlas files. The primary goal is to provide type-safe, auto-generated C# constants and data structures for accessing sprite information within a C# game engine, improving developer workflow and reducing runtime errors.

## 2. Problem Statement

Manually referencing sprite data using strings (e.g., "Orc-Attack01.png") or hardcoded coordinates is error-prone (typos, outdated data) and difficult to maintain. Changes in the atlas require manual updates in code, leading to potential desynchronization and bugs. This generator will eliminate these issues.

## 3. Proposed Solution: Key Features

The C# Source Generator will:

- **Input:** Consume one or more `.json` atlas files (matching the provided structure) marked as `AdditionalFiles` in the consuming C# project.

- **Parsing:** Deserialize the JSON structure, including atlas metadata and the list of images with their properties (Name, X, Y, W, H, TrimOffsetX, TrimOffsetY, UntrimmedWidth, UntrimmedHeight).

- **Code Generation:** For each atlas JSON file, generate a C# static class. For an atlas named "orcs_atlas.json":

  - **Namespace:** The generated code will reside in a configurable or default namespace (e.g., `GameEngine.Generated.Atlases`).

  - **Main Class:** `public static class OrcsAtlasSprites` (derived from the JSON file name).

  - **Sprite Keys:** An enum `public enum SpriteKey { Orc, Orc_Attack02, Orc_Attack01, ... }` for unique sprite identification. Names will be sanitized (e.g., "Orc-Attack01.png" -> `Orc_Attack01`).

  - **Sprite Data Struct:** `public readonly struct SpriteData { public string OriginalName { get; } public int X { get; } ... public int UntrimmedHeight { get; } ... }`.

  - **Access Mechanism:** A static, readonly dictionary: `public static readonly System.Collections.Generic.IReadOnlyDictionary<SpriteKey, SpriteData> Definitions;` providing access to all sprites.

- **Robustness:** Handle potential naming collisions for sanitized sprite keys gracefully (e.g., by appending a unique suffix if necessary, though simple sanitization is the initial focus).

## 4. Success Criteria

- The Source Generator produces C# code that compiles successfully within the game engine project.

- Generated sprite data (coordinates, dimensions) accurately reflects the JSON input.

- Developers can use the generated enums and structs with IDE autocompletion and compile-time type checking.

- Changes to the input JSON files trigger regeneration of the C# code upon the next build.

- Basic error reporting for malformed JSON or critical processing issues.

## 5. Out of Scope (Initial Version)

- Generating code from atlas formats other than the specified JSON structure.

- Aggregating multiple distinct atlas files into a single generated C# class (each JSON will initially generate its own corresponding static class).

- Advanced animation sequence generation from sprite names (focus on individual sprite definitions).

- A GUI or editor interface for managing atlases or the generator.
