# `love.timer` Module API Mapping

This document maps the functions available in the `love.timer` module of Love2D to their proposed equivalents in the Night Engine. Most functions in this module are **Out of Scope** for the initial prototype.

| Love2D Function (`love.timer.`) | Night Engine API (`Night.Timer.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|---------------------------------|-----------------------------------|---------------------------|--------------------------|------|
| `love.timer.getDelta()`         | `Night.Timer.GetDeltaTime()`      | `public static double GetDeltaTime()` <br> Time since last frame. This is already provided to `MyGame.Update(deltaTime)`. This function would provide it on demand. | Out of Scope (Covered by `Update`'s `deltaTime`) | [ ] |
| `love.timer.getFPS()`           | `Night.Timer.GetFPS()`            | `public static int GetFPS()` <br> Current frames per second. | In Scope (Useful for debugging/display) | [ ] |
| `love.timer.getAverageDelta()`  | `Night.Timer.GetAverageDeltaTime()` | `public static double GetAverageDeltaTime()` <br> Average delta time over the last second. | Out of Scope | [ ] |
| `love.timer.getTime()`          | `Night.Timer.GetTime()`           | `public static double GetTime()` <br> Time since the game started, in seconds. | In Scope (Useful utility) | [ ] |
| `love.timer.sleep(s)`           | `Night.Timer.Sleep(double seconds)` | `public static void Sleep(double seconds)` <br> Pauses execution. | Out of Scope (Generally not recommended in game loops) | [ ] |
| `love.timer.step()`             | `Night.Timer.Step()`              | `public static double Step()` <br> Measures time between calls. Used internally by Love2D's default `love.run`. Night Engine will have its own internal timing. | Out of Scope (Engine internal) | [ ] |
