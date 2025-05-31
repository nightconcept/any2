# Epic 7: Night.SampleGame - Platformer Design

This document outlines the design for the simple platformer game to be built using the Night Engine, as part of Epic 7.

## 1. Player Character

- **Appearance:** A solid **blue** colored rectangle.
- **Size:**
  - Width: 32 pixels
  - Height: 64 pixels
- **Initial Position:** Centered horizontally, resting on the first platform.

## 2. Player Actions

Player actions are controlled via keyboard input.

- **Move Left:**
  - **Input:** `Night.KeyCode.Left` (Left Arrow Key)
  - **Action:** Player character moves horizontally to the left at a defined speed.
- **Move Right:**
  - **Input:** `Night.KeyCode.Right` (Right Arrow Key)
  - **Action:** Player character moves horizontally to the right at a defined speed.
- **Jump:**
  - **Input:** `Night.KeyCode.Space` (Spacebar)
  - **Action:** Player character gains an initial upward vertical velocity. Gravity will then affect the player, bringing them back down. The player can only jump if they are currently on a platform (grounded).

## 3. Level Elements

The level will consist of static platforms.

- **Platforms:**
  - **Appearance:** Solid **green** colored rectangles.
  - **Properties:** Each platform will be defined by its position (X, Y coordinates of the top-left corner) and size (width, height).
  - **Arrangement (Example for initial prototype):**
    - Platform 1 (Ground):
      - Position: (X: 50, Y: 500)
      - Size: (Width: 700, Height: 50)
    - Platform 2:
      - Position: (X: 200, Y: 400)
      - Size: (Width: 150, Height: 30)
    - Platform 3:
      - Position: (X: 450, Y: 300)
      - Size: (Width: 100, Height: 30)
    - Platform 4 (Goal Platform):
      - Position: (X: 600, Y: 200)
      - Size: (Width: 100, Height: 30)
      - **Special:** Reaching this platform signifies the objective.

## 4. Game Objective / Success State

- **Objective:** The player must navigate their character from the starting platform to **Platform 4 (Goal Platform)**.
- **Success State:** The game successfully demonstrates stable player movement (left, right, jump), collision with platforms (landing, not passing through), and the ability to reach the designated goal platform. For the prototype, simply reaching the platform is sufficient; no complex "win" screen is required.

## 5. Implementation Checklist (for Tasks 7.2+)

This section can be used to track progress for subsequent implementation tasks.

### Task 7.2: Implement Player Character

- [ ] Create `Player` class in `Night.SampleGame`.
- [ ] `Player.Load()`:
  - [ ] Initialize player position (e.g., centered on Platform 1).
  - [ ] Initialize player size (32x64).
  - [ ] Initialize movement properties (speed, jump height, gravity value).
  - [ ] (No sprite loading needed, will draw a rectangle).
- [ ] `Player.Update(double deltaTime)`:
  - [ ] Handle horizontal movement input (`Night.Keyboard.IsDown(Night.KeyCode.Left)` / `Right`).
  - [ ] Implement jump logic (`Night.Keyboard.IsDown(Night.KeyCode.Space)`), apply upward velocity (only if grounded).
  - [ ] Apply gravity to vertical velocity.
  - [ ] Update player position based on velocity and `deltaTime`.
- [ ] `Player.Draw()`:
  - [ ] Render the player as a blue rectangle at its current position. (Requires a way to draw filled rectangles with `Night.Graphics`. If not directly available, a 1x1 white pixel sprite could be loaded and scaled/colored, or this highlights a need for basic primitive drawing).
        *Self-correction: PRD Feature 3 for `Night.Graphics` focuses on "loading images and drawing them as sprites". It does not explicitly mention drawing geometric primitives like rectangles. For the prototype, if `Night.Graphics.DrawRectangle(x, y, w, h, color)` is not available, the player (and platforms) might need to be represented by simple 1x1 pixel sprites that are then scaled and tinted, or use a pre-made colored square image if tinting is not yet supported. The design assumes a simple way to draw a colored rectangle will be feasible, potentially by creating a small colored texture in memory if direct drawing isn't an option.*

### Task 7.3: Implement Basic Level (Platforms)

- [ ] Define platform data (e.g., array/list of `Night.Rectangle` or custom struct for position, size, color).
  - [ ] Platform 1: (X: 50, Y: 500), Size: (700x50), Color: Green
  - [ ] Platform 2: (X: 200, Y: 400), Size: (150x30), Color: Green
  - [ ] Platform 3: (X: 450, Y: 300), Size: (100x30), Color: Green
  - [ ] Platform 4: (X: 600, Y: 200), Size: (100x30), Color: Green (Goal)
- [ ] `Game.Load()` or `Level.Load()`: Initialize platform objects/data.
- [ ] `Game.Update()` or `Player.Update()`:
  - [ ] Implement AABB collision detection between player and platforms.
  - [ ] Resolve collisions:
    - [ ] Prevent player from falling through platforms (stop downward movement).
    - [ ] Prevent player from moving horizontally into platforms.
- [ ] `Game.Draw()` or `Level.Draw()`:
  - [ ] Render platforms as green rectangles. (Same rendering consideration as the player character).

### Task 7.4: Implement Main Game Logic in `Game.cs`

- [ ] Ensure `Night.SampleGame.Game` class implements `Night.IGame`.
- [ ] `Game.Load()`:
  - [ ] Initialize/Load player object.
  - [ ] Initialize/Load platform data/level objects.
  - [ ] Set window title to "Night Platformer Sample".
  - [ ] Set window size (e.g., 800x600).
- [ ] `Game.Update(double deltaTime)`:
  - [ ] Call `Player.Update(deltaTime)`.
  - [ ] (Optional: Check if player reached Platform 4 - simple log message for now).
- [ ] `Game.Draw()`:
  - [ ] `Night.Graphics.Clear(backgroundColor)` (e.g., light gray or sky blue).
  - [ ] Call draw methods for platforms.
  - [ ] Call draw method for the player.
- [ ] **Verification:** `Night.SampleGame` runs via `Night.Engine.Run(new Game())`. All elements are present and interactive.
