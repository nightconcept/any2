# `love.thread` Module API Mapping

This document maps the functions available in the `love.thread` module of Love2D to their proposed equivalents in the Night Engine. This entire module is **Out of Scope** for the initial prototype, as .NET provides comprehensive threading capabilities via `System.Threading`.

| Love2D Function (`love.thread.`) | Night Engine API (`Night.Thread` or `System.Threading`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|----------------------------------|---------------------------------------------------------|---------------------------|--------------------------|------|
| `love.thread.newThread(filename)` or `love.thread.newThread(codestring)` | `Night.Thread.NewThread(string luaScriptPathOrCode)` or `new System.Threading.Thread(...)` | `public static Night.Thread NewThread(string luaScriptPathOrCode)` <br> Love2D threads run Lua code. Night Engine would use C# delegates/lambdas with `System.Threading.Thread` or `Task`. | Out of Scope | [ ] |
| `love.thread.getChannel(name)`   | `Night.Thread.GetChannel<T>(string name)` | `public static Night.Channel<T> GetChannel<T>(string name)` <br> Channels are for inter-thread communication. | Out of Scope | [ ] |
| `love.thread.newChannel()`       | `Night.Thread.NewChannel<T>()`    | `public static Night.Channel<T> NewChannel<T>()` | Out of Scope | [ ] |

**Functionality on `Night.Thread` instances (if implemented, wrapping `System.Threading.Thread`):**
*   `thread.Start()`
*   `thread.Wait()`
*   `thread.IsRunning()`
*   `thread.GetError()`

**Functionality on `Night.Channel<T>` instances (if implemented, similar to `System.Threading.Channels.Channel<T>`):**
*   `channel.Push(T value)`
*   `channel.Pop()` (non-blocking, returns nullable T)
*   `channel.Demand()` (blocking, returns T)
*   `channel.Peek()`
*   `channel.GetCount()`
*   `channel.HasRead(id)`
*   `channel.Clear()`
*   `channel.PerformAtomic(Func<bool> operation)`

**Night Engine Specific Types (if module were implemented):**
*   `Night.Thread`: A wrapper around `System.Threading.Thread` or `Task`, potentially with easier error handling or specific Love2D-like behaviors if Lua interop were a goal.
*   `Night.Channel<T>`: A thread-safe communication channel, similar to `System.Threading.Channels.Channel<T>`.
