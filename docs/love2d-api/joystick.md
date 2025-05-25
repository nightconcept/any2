# `love.joystick` Module API Mapping

This document maps the functions available in the `love.joystick` module of Love2D to their proposed equivalents in the Night Engine. This entire module is **Out of Scope** for the initial prototype. Joystick event callbacks are noted in the `love` module mapping.

| Love2D Function (`love.joystick.`) | Night Engine API (`Night.Joystick` or `Joystick` instance methods) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|------------------------------------|--------------------------------------------------------------------|---------------------------|--------------------------|------|
| `love.joystick.getJoysticks()`     | `Night.Joystick.GetJoysticks()`    | `public static Night.Joystick[] GetJoysticks()` <br> Returns all connected joysticks. | Out of Scope | [ ] |
| `love.joystick.getJoystickCount()` | `Night.Joystick.GetJoystickCount()` | `public static int GetJoystickCount()` | Out of Scope | [ ] |
| `love.joystick.loadGamepadMappings(filename)` or `love.joystick.loadGamepadMappings(string)` | `Night.Joystick.LoadGamepadMappings(string pathOrString)` | `public static bool LoadGamepadMappings(string pathOrString)` | Out of Scope | [ ] |
| `love.joystick.saveGamepadMappings(joystick)` | `(Night.Joystick).SaveGamepadMappings()` | `public string SaveGamepadMappings()` (Method on `Joystick` instance) | Out of Scope | [ ] |
| `love.joystick.setGamepadMapping(guid, buttonOrAxis, inputtype, inputindex, hatdirection)` | `Night.Joystick.SetGamepadMapping(string guid, ...)` | Complex mapping function. | Out of Scope | [ ] |

**Functionality on `Night.Joystick` instances (if implemented):**
*   `joystick.isConnected()`
*   `joystick.getName()`
*   `joystick.getID()` (instance ID)
*   `joystick.getGUID()`
*   `joystick.getAxisCount()`
*   `joystick.getButtonCount()`
*   `joystick.getHatCount()`
*   `joystick.getAxis(axisindex)`
*   `joystick.getAxes()`
*   `joystick.isDown(buttonindex, ...)`
*   `joystick.getHat(hatindex)`
*   `joystick.isGamepad()`
*   `joystick.getGamepadAxis(axis)`
*   `joystick.isGamepadDown(button)`
*   `joystick.setVibration(left, right, duration)`
*   `joystick.hasVibration()`

**Night Engine Specific Types:**
*   `Night.Joystick`: Represents a joystick/gamepad device.
*   `Night.GamepadAxis`: Enum for standard gamepad axes (e.g., `LeftX`, `LeftY`, `RightX`, `RightY`, `TriggerLeft`, `TriggerRight`).
*   `Night.GamepadButton`: Enum for standard gamepad buttons (e.g., `A`, `B`, `X`, `Y`, `Start`, `Select`, `DPadUp`).
*   `Night.HatDirection`: Enum for hat switch directions (e.g., `Centered`, `Up`, `Down`, `Left`, `Right`, `UpLeft`).
