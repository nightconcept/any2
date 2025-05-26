# `love.audio` Module API Mapping

This document maps the functions available in the `love.audio` module of Love2D to their proposed equivalents in the Night Engine. This entire module is **Out of Scope** for the initial prototype.

| Love2D Function (`love.audio.`) | Night Engine API (`Night.Audio.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|---------------------------------|-----------------------------------|---------------------------|--------------------------|------|
| `love.audio.getActiveSourceCount()` | `Night.Audio.GetActiveSourceCount()` | `public static int GetActiveSourceCount()` | Out of Scope | [ ] |
| `love.audio.getDistanceModel()` | `Night.Audio.GetDistanceModel()`  | `public static Night.DistanceModel GetDistanceModel()` <br> `DistanceModel` enum. | Out of Scope | [ ] |
| `love.audio.getDopplerScale()`  | `Night.Audio.GetDopplerScale()`   | `public static double GetDopplerScale()` | Out of Scope | [ ] |
| `love.audio.getEffect(name)`    | `Night.Audio.GetEffect(string name)` | `public static Night.AudioEffect? GetEffect(string name)` <br> `AudioEffect` would be a base class for effects. | Out of Scope | [ ] |
| `love.audio.getOrientation()`   | `Night.Audio.GetListenerOrientation()` | `public static (float fx, float fy, float fz, float ux, float uy, float uz) GetListenerOrientation()` | Out of Scope | [ ] |
| `love.audio.getPosition()`      | `Night.Audio.GetListenerPosition()` | `public static (float x, float y, float z) GetListenerPosition()` | Out of Scope | [ ] |
| `love.audio.getRecordingDevices()` | `Night.Audio.GetRecordingDevices()` | `public static Night.RecordingDevice[] GetRecordingDevices()` | Out of Scope | [ ] |
| `love.audio.getSourceCount()`   | `Night.Audio.GetTotalSourceCount()` | `public static int GetTotalSourceCount()` | Out of Scope | [ ] |
| `love.audio.getVelocity()`      | `Night.Audio.GetListenerVelocity()` | `public static (float x, float y, float z) GetListenerVelocity()` | Out of Scope | [ ] |
| `love.audio.getVolume()`        | `Night.Audio.GetMasterVolume()`   | `public static float GetMasterVolume()` | Out of Scope | [ ] |
| `love.audio.isEffectsSupported()` | `Night.Audio.IsEffectsSupported()` | `public static bool IsEffectsSupported()` | Out of Scope | [ ] |
| `love.audio.newSource(filename, type)` or `love.audio.newSource(decoder, type)` | `Night.Audio.NewSource(string filePath, Night.SourceType type = Static)` or `Night.Audio.NewSource(Night.Decoder decoder, Night.SourceType type = Stream)` | `public static Night.Source NewSource(...)` <br> `SourceType` enum: `Static`, `Stream`. `Decoder` for custom audio formats. | Out of Scope | [ ] |
| `love.audio.pause(source)` or `love.audio.pause()` | `Night.Audio.Pause(Night.Source? source = null)` | `public static void Pause(Night.Source? source = null)` <br> Pauses specific source or all. | Out of Scope | [ ] |
| `love.audio.play(source)`       | `Night.Audio.Play(Night.Source source)` | `public static void Play(Night.Source source)` | Out of Scope | [ ] |
| `love.audio.resume(source)` or `love.audio.resume()` | `Night.Audio.Resume(Night.Source? source = null)` | `public static void Resume(Night.Source? source = null)` | Out of Scope | [ ] |
| `love.audio.setDistanceModel(model)` | `Night.Audio.SetDistanceModel(Night.DistanceModel model)` | `public static void SetDistanceModel(Night.DistanceModel model)` | Out of Scope | [ ] |
| `love.audio.setDopplerScale(scale)` | `Night.Audio.SetDopplerScale(double scale)` | `public static void SetDopplerScale(double scale)` | Out of Scope | [ ] |
| `love.audio.setEffect(name, settings)` | `Night.Audio.SetEffect(string name, Night.AudioEffectSettings settings)` | `public static bool SetEffect(string name, Night.AudioEffectSettings settings)` | Out of Scope | [ ] |
| `love.audio.setMixWithSystem(mix)` | `Night.Audio.SetMixWithSystem(bool mix)` | `public static void SetMixWithSystem(bool mix)` | Out of Scope | [ ] |
| `love.audio.setOrientation(fx, fy, fz, ux, uy, uz)` | `Night.Audio.SetListenerOrientation(...)` | `public static void SetListenerOrientation(float forwardX, ...)` | Out of Scope | [ ] |
| `love.audio.setPosition(x, y, z)` | `Night.Audio.SetListenerPosition(float x, float y, float z)` | `public static void SetListenerPosition(float x, float y, float z)` | Out of Scope | [ ] |
| `love.audio.setRecordingDevice(name)` | `Night.Audio.SetRecordingDevice(string name)` | `public static void SetRecordingDevice(string name)` | Out of Scope | [ ] |
| `love.audio.setVelocity(x, y, z)` | `Night.Audio.SetListenerVelocity(float x, float y, float z)` | `public static void SetListenerVelocity(float x, float y, float z)` | Out of Scope | [ ] |
| `love.audio.setVolume(volume)`  | `Night.Audio.SetMasterVolume(float volume)` | `public static void SetMasterVolume(float volume)` | Out of Scope | [ ] |
| `love.audio.stop(source)` or `love.audio.stop()` | `Night.Audio.Stop(Night.Source? source = null)` | `public static void Stop(Night.Source? source = null)` | Out of Scope | [ ] |

**Night Engine Specific Types:**
*   `Night.Source`: Represents an audio source (sound effect or music). Would have methods like `Play()`, `Pause()`, `Stop()`, `SetVolume()`, `Seek()`, `IsPlaying()`, etc.
*   `Night.SourceType`: Enum (`Static`, `Stream`).
*   `Night.Decoder`: Represents a custom audio decoder.
*   `Night.DistanceModel`: Enum for 3D audio distance attenuation (e.g., `None`, `Inverse`, `Linear`).
*   `Night.AudioEffect`: Base class for audio effects (e.g., reverb, echo).
*   `Night.AudioEffectSettings`: Base class for effect-specific settings.
*   `Night.RecordingDevice`: Represents an audio recording device.
