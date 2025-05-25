# `love.video` Module API Mapping

This document maps the functions available in the `love.video` module of Love2D to their proposed equivalents in the Night Engine. This entire module is **Out of Scope** for the initial prototype. The primary way to get a video object in Night Engine would be `Night.Graphics.NewVideo()`.

| Love2D Function (`love.video.`) | Night Engine API (`Night.Video` methods or `Night.Graphics`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|---------------------------------|--------------------------------------------------------------|---------------------------|--------------------------|------|
| `love.video.newVideoStream(filename)` | `Night.Graphics.NewVideo(string filePath, Night.VideoOptions? options = null)` | `public static Night.Video NewVideo(...)` <br> This is the main entry point. `VideoStream` in Love2D is just `Video`. | Out of Scope | [ ] |

**Functionality on `Night.Video` instances (if implemented):**
*   `video.Play()`
*   `video.Pause()`
*   `video.Seek(offset)`
*   `video.Tell()` (get current playback time)
*   `video.GetSource()` (audio source associated with video)
*   `video.IsPlaying()`
*   `video.SetSync(audioSource)`
*   `video.GetWidth()`, `video.GetHeight()` (as an `IDrawable`)
*   `video.GetFilename()`
*   `video.GetFilter()`
*   `video.SetFilter(min, mag)`

**Night Engine Specific Types (if module were implemented):**
*   `Night.Video`: Represents a video object. It would be an `IDrawable` and might internally manage a `Night.Source` for audio. Created via `Night.Graphics.NewVideo()`.
*   `Night.VideoOptions`: Class for video loading options (e.g., `EnableAudio`).
*   `Night.Source`: Audio source from `Night.Audio` module.
*   `Night.FilterMode`: Enum (`Linear`, `Nearest`).
