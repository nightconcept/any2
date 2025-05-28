# `love.keyboard` Module API Mapping

This document maps the functions available in the `love.keyboard` module of Love2D to their proposed equivalents in the Night Engine.

| Love2D Function (`love.keyboard.`) | Night Engine API (`Night.Keyboard.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|------------------------------------|--------------------------------------|---------------------------|--------------------------|------|
| `love.keyboard.isDown(key)`        | `Night.Keyboard.IsDown(Night.KeyCode key)` | `public static bool IsDown(Night.KeyCode key)` <br> Checks if specific keys are held down. `Night.KeyCode` enum will map to SDL scancodes. | In Scope | [ ] |
| `love.keyboard.isScancodeDown(scancode)` | `Night.Keyboard.IsScancodeDown(Night.Scancode scancode)` | `public static bool IsScancodeDown(Night.Scancode scancode)` <br> `Night.Scancode` would be an enum closely matching SDL scancodes. May be internal or less used if `KeyCode` is preferred. | In Scope (Lower priority than `IsDown`) | [ ] |
| `love.keyboard.getKeyFromScancode(scancode)` | `Night.Keyboard.GetKeyFromScancode(Night.Scancode scancode)` | `public static Night.KeyCode GetKeyFromScancode(Night.Scancode scancode)` | In Scope (Helper for input mapping) | [ ] |
| `love.keyboard.getScancodeFromKey(key)` | `Night.Keyboard.GetScancodeFromKey(Night.KeyCode key)` | `public static Night.Scancode GetScancodeFromKey(Night.KeyCode key)` | In Scope (Helper for input mapping) | [ ] |
| `love.keyboard.setKeyRepeat(enable)` | `Night.Keyboard.SetKeyRepeatEnabled(bool enabled)` | `public static void SetKeyRepeatEnabled(bool enabled)` <br> Enables or disables key repeat for `love.keypressed`. SDL handles this by default; this might control if `isRepeat` is true in `MyGame.KeyPressed`. | In Scope (Verify SDL behavior) | [ ] |
| `love.keyboard.hasKeyRepeat()`     | `Night.Keyboard.HasKeyRepeat()`    | `public static bool HasKeyRepeat()` <br> Checks if key repeat is enabled. | In Scope (Verify SDL behavior) | [ ] |
| `love.keyboard.setTextInput(enable, x, y, w, h)` | `Night.Keyboard.SetTextInputRect(bool enable, Night.Rectangle? rect = null)` | `public static void SetTextInputRect(bool enable, Night.Rectangle? rect = null)` <br> For on-screen keyboards on touch devices. `rect` defines text input area. | Out of Scope | [ ] |
| `love.keyboard.hasScreenKeyboard()` | `Night.Keyboard.HasScreenKeyboard()` | `public static bool HasScreenKeyboard()` | Out of Scope | [ ] |

**Night Engine Specific Types:**
*   `Night.KeyCode`: Enum representing keyboard keys (e.g., `A`, `Space`, `Return`). This will be mapped to SDL scancodes.
*   `Night.Scancode`: Enum representing platform-independent physical key codes (e.g., `SDL_SCANCODE_A`).
*   `Night.Rectangle`: Struct/class for a rectangle (X, Y, Width, Height).
