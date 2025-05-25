
**Epic 7: Sample Game & Integration Testing**

**Goal:** Develop a simple platformer game using the "Night Engine." This sample game will serve as a comprehensive integration test, verifying that all core engine features (Window, Input, Graphics, Game Loop) function correctly and cohesively as defined in the PRD.

- [ ] **Task 7.1:** Design Basic Platformer Game Mechanics for `Night.SampleGame`
    
    - [ ] Define the player character: appearance (e.g., a simple colored rectangle or a basic sprite), size.
    - [ ] Define player actions: move left, move right, jump.
    - [ ] Define basic level elements: static platforms (rectangles) for the player to stand on and jump between.
    - [ ] Define a simple objective or success state for the prototype (e.g., navigate to a specific point, or simply demonstrate stable movement and interaction).
    - **Verification:** A minimal design document or sketch outlining the platformer's mechanics, player abilities, and level structure is created.
- [ ] **Task 7.2:** Implement Player Character in `Night.SampleGame`
    
    - [ ] Create a `Player` class within the `Night.SampleGame` project.
    - [ ] **Loading:** In a `Player.Load()` method (or equivalent called from `Game.Load()`), if using a sprite, load it using `Night.Graphics.NewImage()`. Initialize player position, size, and movement properties (e.g., speed, jump height, gravity).
    - [ ] **Updating:** In a `Player.Update(double deltaTime)` method:
        - [ ] Handle horizontal movement input using `Night.Keyboard.IsDown(Night.KeyCode.Left)` and `Night.Keyboard.IsDown(Night.KeyCode.Right)`.
        - [ ] Implement jump logic (e.g., on `Night.Keyboard.IsDown(Night.KeyCode.Space)`), applying an upward velocity.
        - [ ] Apply basic gravity to the player's vertical velocity.
        - [ ] Update player position based on velocity and `deltaTime`.
    - [ ] **Drawing:** In a `Player.Draw()` method, render the player (rectangle or sprite) at its current position using `Night.Graphics.Draw()`.
    - **Verification:** The player character is displayed on the screen. It responds to left/right arrow key presses by moving horizontally. Pressing the jump key makes the player move upwards and then fall due to gravity.
- [ ] **Task 7.3:** Implement Basic Level (Platforms) in `Night.SampleGame`
    
    - [ ] Define platform data (e.g., an array or list of `Night.Rectangle` structs for position and size).
    - [ ] In `Game.Load()` or a `Level.Load()` method, initialize these platforms.
    - [ ] In `Game.Update()` or `Player.Update()`, implement simple Axis-Aligned Bounding Box (AABB) collision detection between the player and the platforms.
        - [ ] Resolve collisions by preventing the player from passing through platforms (e.g., stop downward movement when landing on top of a platform, block horizontal movement into the side of a platform).
    - [ ] In `Game.Draw()` or a `Level.Draw()` method, render the platforms (e.g., as filled rectangles using a conceptual `Night.Graphics.DrawRectangle()` if added, or by drawing placeholder sprites for each). _Self-correction: The PRD doesn't specify `DrawRectangle`. For the prototype, platforms can be represented by loaded sprites or this might highlight a small graphics primitive need for the sample._
    - **Verification:** Platforms are rendered on the screen. The player character can land on top of platforms and is appropriately stopped by them. The player does not fall through platforms.
- [ ] **Task 7.4:** Implement Main Game Logic in `Game.cs` (integrating `IGameLogic`)
    
    - [ ] Ensure `Night.SampleGame.Game` class properly implements the `Night.IGameLogic` interface (from Epic 6).
    - [ ] **`Game.Load()`:** Initialize the player object, platform data/level objects, and load any other necessary assets.
    - [ ] **`Game.Update(double deltaTime)`:** Call the `Player.Update(deltaTime)` method. Update any other game state logic (e.g., checking simple win/lose conditions if designed).
    - [ ] **`Game.Draw()`:**
        - [ ] Call `Night.Graphics.Clear(backgroundColor)` at the beginning.
        - [ ] Call draw methods for platforms and the player, ensuring correct layering if relevant.
    - **Verification:** The `Night.SampleGame` runs via `Night.Engine.Run(new Game())`. All game elements (player, platforms) are initialized, updated, and drawn correctly each frame, demonstrating the integrated use of `Night.Window`, `Night.Input`, `Night.Graphics`, and the `Night.Engine` game loop.
- [ ] **Task 7.5:** End-to-End Feature Verification & Bug Fixing
    
    - [ ] Play through the `Night.SampleGame` platformer, systematically testing all implemented `Night` engine features:
        - Window creation and title (`Night.Window`).
        - Keyboard input for player control (`Night.Keyboard`).
        - Mouse input for position checking, if used for any debug (`Night.Mouse`).
        - Sprite loading and rendering for player/platforms (`Night.Graphics`).
        - Screen clearing (`Night.Graphics`).
        - Game loop operation, delta time, and event handling (`Night.Engine`).
    - [ ] Compare observed behavior against the feature descriptions in the PRD.
    - [ ] Document any bugs, unexpected behaviors, or deviations from the PRD.
    - [ ] Iterate on bug fixes within the `Night.Engine` or `Night.SampleGame` code until the prototype functions as intended for the defined features.
    - **Verification:** The sample platformer game is playable and all core `Night` engine features (PRD Features 1-4) are demonstrably working as expected. Any significant bugs identified during testing have been addressed.
    