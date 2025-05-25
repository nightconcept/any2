# `love.mouse` Module API Mapping

This document maps the functions available in the `love.mouse` module of Love2D to their proposed equivalents in the Night Engine.

| Love2D Function (`love.mouse.`) | Night Engine API (`Night.Mouse.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|---------------------------------|-----------------------------------|---------------------------|--------------------------|------|
| `love.mouse.getX()`             | `Night.Mouse.GetX()`              | `public static int GetX()` | In Scope | [ ] |
| `love.mouse.getY()`             | `Night.Mouse.GetY()`              | `public static int GetY()` | In Scope | [ ] |
| `love.mouse.getPosition()`      | `Night.Mouse.GetPosition()`       | `public static (int x, int y) GetPosition()` | In Scope | [ ] |
| `love.mouse.isDown(button)`     | `Night.Mouse.IsDown(Night.MouseButton button)` | `public static bool IsDown(Night.MouseButton button)` <br> `Night.MouseButton` enum: `Left`, `Right`, `Middle`, `X1`, `X2`, etc. | In Scope | [ ] |
| `love.mouse.isVisible()`        | `Night.Mouse.IsVisible()`         | `public static bool IsVisible()` | In Scope | [ ] |
| `love.mouse.setX(x)`            | `Night.Mouse.SetX(int x)`         | `public static void SetX(int x)` <br> Warps mouse cursor. | In Scope (Low priority) | [ ] |
| `love.mouse.setY(y)`            | `Night.Mouse.SetY(int y)`         | `public static void SetY(int y)` <br> Warps mouse cursor. | In Scope (Low priority) | [ ] |
| `love.mouse.setPosition(x,y)`   | `Night.Mouse.SetPosition(int x, int y)` | `public static void SetPosition(int x, int y)` <br> Warps mouse cursor. | In Scope (Low priority) | [ ] |
| `love.mouse.setVisible(visible)`| `Night.Mouse.SetVisible(bool visible)` | `public static void SetVisible(bool visible)` | In Scope | [ ] |
| `love.mouse.setGrabbed(grab)`   | `Night.Mouse.SetGrabbed(bool grabbed)` | `public static void SetGrabbed(bool grabbed)` <br> Confines cursor to window. | In Scope (Low priority) | [ ] |
| `love.mouse.isGrabbed()`        | `Night.Mouse.IsGrabbed()`         | `public static bool IsGrabbed()` | In Scope (Low priority) | [ ] |
| `love.mouse.getRelativeMode()`  | `Night.Mouse.GetRelativeMode()`   | `public static bool GetRelativeMode()` | In Scope (Low priority, for FPS-style input) | [ ] |
| `love.mouse.setRelativeMode(enable)` | `Night.Mouse.SetRelativeMode(bool enable)` | `public static void SetRelativeMode(bool enable)` | In Scope (Low priority) | [ ] |
| `love.mouse.getCursor()`        | `Night.Mouse.GetCursor()`         | `public static Night.Cursor GetCursor()` <br> `Night.Cursor` would be a custom cursor object. | Out of Scope | [ ] |
| `love.mouse.setCursor(cursor)`  | `Night.Mouse.SetCursor(Night.Cursor? cursor = null)` | `public static void SetCursor(Night.Cursor? cursor = null)` <br> `null` for default system cursor. | Out of Scope | [ ] |
| `love.mouse.newCursor(imagedata, hotx, hoty)` | `Night.Mouse.NewCursor(Night.ImageData imageData, int hotSpotX, int hotSpotY)` | `public static Night.Cursor NewCursor(...)` | Out of Scope | [ ] |
| `love.mouse.getSystemCursor(ctype)` | `Night.Mouse.GetSystemCursor(Night.SystemCursorType type)` | `public static Night.Cursor GetSystemCursor(Night.SystemCursorType type)` <br> `SystemCursorType` enum: `Arrow`, `IBeam`, `Crosshair`, etc. | Out of Scope | [ ] |

**Night Engine Specific Types:**
*   `Night.MouseButton`: Enum representing mouse buttons (e.g., `Left`, `Right`, `Middle`, `X1`, `X2`).
*   `Night.Cursor`: Represents a mouse cursor (custom or system).
*   `Night.ImageData`: Wrapper for image data, likely from `Night.Image` module.
*   `Night.SystemCursorType`: Enum for standard system cursors.
