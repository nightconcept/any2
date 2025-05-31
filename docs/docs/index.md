# Night Engine Documentation

Welcome to the official documentation for Night Engine.

Night Engine is a C# game engine built on SDL3, designed to provide a "batteries-included" development experience with a Love2D-inspired API for its foundational framework (`Night.Framework`).

## Key Sections

* **[Introduction](introduction.md)**: Learn about the project's vision, goals, and overall architecture.
* **[Getting Started](getting-started.md)**: Find out how to set up your development environment, build the engine, and run the sample game.

## Night.Framework Modules (Version 0.1.0 Focus)

The initial development phase concentrates on `Night.Framework`, providing core functionalities through a Love2D-like API. Key modules include:

* **`Night.Window`**: For creating and managing the game window (e.g., setting mode, title).
* **`Night.Keyboard`**: For handling keyboard input (e.g., checking if keys are pressed).
* **`Night.Mouse`**: For handling mouse input (e.g., checking button presses, getting cursor position).
* **`Night.Graphics`**: For 2D graphics rendering (e.g., loading images, drawing sprites).
* **`Night.Framework.Run`**: Manages the core game loop, into which you plug your game logic via the `IGame` interface (`Load`, `Update`, `Draw`, `KeyPressed` callbacks).

As the project progresses, more detailed API documentation for each module will be added.
