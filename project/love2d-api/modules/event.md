# `love.event` Module API Mapping

This document maps the functions available in the `love.event` module of Love2D to their proposed equivalents in the Night Engine. In Night Engine, event handling is primarily managed by the engine invoking specific callback methods on the user's game class (e.g., `MyGame.KeyPressed`). Direct manipulation of an event queue by the user is **Out of Scope** for the initial prototype.

| Love2D Function (`love.event.`) | Night Engine API (`Night.Event` or Engine Internals) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|---------------------------------|------------------------------------------------------|---------------------------|--------------------------|------|
| `love.event.clear()`            | `Night.Event.ClearQueue()` (Engine internal or not exposed) | `internal static void ClearQueue()` <br> Clears pending events. Engine might do this per frame. | Out of Scope | [ ] |
| `love.event.poll()`             | `Night.Event.Poll()` (Engine internal) | `internal static Night.EventData? Poll()` <br> Returns next event if any. Engine uses this in its loop. | Out of Scope | [ ] |
| `love.event.pump()`             | `Night.Event.PumpEvents()` (Engine internal) | `internal static void PumpEvents()` <br> Processes OS events into LÖVE events. Engine does this. | Out of Scope | [ ] |
| `love.event.push(e, ...)`       | `Night.Event.PushCustomEvent(string eventName, params object[] args)` | `public static void PushCustomEvent(string eventName, params object[] args)` <br> Allows user to push custom events. Would require a `MyGame.CustomEvent(name, args)` callback. | Out of Scope | [ ] |
| `love.event.quit(exitstatus)`   | `Night.Engine.RequestQuit(int exitStatus = 0)` | `public static void RequestQuit(int exitStatus = 0)` <br> Pushes a quit event. | In Scope (as `Night.Engine.RequestQuit`) | [ ] |
| `love.event.wait()`             | `Night.Event.Wait()` (Engine internal or not exposed) | `internal static Night.EventData Wait()` <br> Waits for next event. Not typical for game loops. | Out of Scope | [ ] |

**Night Engine Specific Types (if module were implemented for custom events):**
*   `Night.EventData`: A base class or struct for event information, potentially with derived types for specific events if not handled by direct callbacks.
*   Custom event callbacks in `MyGame` like `MyGame.OnCustomEvent(string name, object[] args)`.
