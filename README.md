# Night Engine

A C# game engine built on top of SDL3.

This project aims to provide a "batteries-included," in code editor experience to game development. A near-parity Love2D-inspired API for C# developers serves as the framework base. On top of this framework, there is an (optional) engine that will provide opinionated solutions for the engine so game designers can focus on building games.

This project also intends to be AI friendly so that non-programmer game designers can build games faster.

## Library Structure

The core of the project is the `Night.dll` library. This assembly contains:

- The `Night` namespace: Provides the Love2D-inspired framework API for low-level game development tasks.
- The `Night.Engine` namespace: Will house higher-level, more opinionated game engine components (e.g., Scene Management, ECS) built upon the `Night` framework.

## Features

### Project

- [ ] Near-parity Love2D-inspired API
- [ ] LLM friendly documentation automagically generated
- [ ] Game samples

### Engine-Specific Features

- [ ] Manager system (Assets, Scenes, Joystick, etc.)
- [ ] ECS

## Getting Started (Development)

1. Ensure [mise](https://mise.jdx.dev/) is installed.
2. Clone repository
3. Run `mise install`
4. Run `mise build`
