# Introduction to Night Engine

Welcome to Night Engine! This document provides a high-level overview of the project, its goals, and its architecture.

## What is Night Engine?

Night Engine is a C# game engine built on the powerful [SDL3](https://www.libsdl.org/) library. It's designed to offer a "batteries-included" development experience, making game and multimedia application development in C# more streamlined and efficient.

The project has two main components:

1. **`Night.Framework`**: This is the foundational layer of the engine. It provides a Love2D-inspired API, offering a familiar and productive environment for developers accustomed to that style. `Night.Framework` aims to be a robust C# wrapper around SDL3, giving direct access to its capabilities. The current development focus is on completing Version 0.1.0 of the `Night.Framework` API.
2. **`Night.Engine`** (Future): Planned as a higher-level, more opinionated engine built on top of `Night.Framework`. This component will offer common game systems (like ECS, scene management, etc.) to further simplify complex game development tasks.

Night Engine also aims to be AI-friendly, with the long-term goal of assisting non-programmers in the game development process.

## Project Goals

* **Streamlined Workflow:** Provide C# developers with an efficient and enjoyable workflow for creating games and multimedia applications.
* **Familiar API:** Offer a Love2D-style API through `Night.Framework` to reduce context switching and leverage a well-liked API design.
* **SDL3 Power:** Harness the cross-platform capabilities and performance of SDL3.
* **Extensibility:** Lay a solid foundation with `Night.Framework` for the future development of the more comprehensive `Night.Engine`.

## Architectural Approach

* **`Night` Framework**:
  * A C# class library (part of `Night.dll`) providing a static API, stylistically similar to Love2D.
  * Resides primarily within the `Night` C# namespace.
  * Interacts directly with SDL3 via the `SDL3-CS` C# bindings.
  * Focuses on simplicity and achieving the core Love2D-like developer experience for its initial version.

* **`Night.Engine`** (Future):
  * A C# library (also part of `Night.dll`) that will provide more opinionated game development constructs.
  * Will reside in the `Night.Engine` namespace.
  * Will use `Night.Framework` for low-level operations and will not interact with SDL3-CS directly.
