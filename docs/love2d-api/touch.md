# `love.touch` Module API Mapping

This document maps the functions available in the `love.touch` module of Love2D to their proposed equivalents in the Night Engine. This entire module is **Out of Scope** for the initial prototype. Touch event callbacks are noted in the `love` module mapping.

| Love2D Function (`love.touch.`) | Night Engine API (`Night.Touch.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|---------------------------------|-----------------------------------|---------------------------|--------------------------|------|
| `love.touch.getPosition(id)`    | `Night.Touch.GetPosition(long touchId)` | `public static (float x, float y) GetPosition(long touchId)` | Out of Scope | [ ] |
| `love.touch.getPressure(id)`    | `Night.Touch.GetPressure(long touchId)` | `public static float GetPressure(long touchId)` | Out of Scope | [ ] |
| `love.touch.getTouches()`       | `Night.Touch.GetActiveTouches()`  | `public static long[] GetActiveTouches()` <br> Returns IDs of currently active touches. | Out of Scope | [ ] |

**Night Engine Specific Types (if module were implemented):**
*   Touch events in `MyGame` would pass a `Night.TouchEventArgs` object containing `Id`, `X`, `Y`, `DeltaX`, `DeltaY`, `Pressure`.
