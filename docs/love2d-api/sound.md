# `love.sound` Module API Mapping

This document maps the functions available in the `love.sound` module of Love2D to their proposed equivalents in the Night Engine. This module is primarily for decoding sound data, which would be handled internally by `Night.Audio.NewSource` or `Night.Source` objects if the audio module were implemented. This entire module is **Out of Scope** for the initial prototype.

| Love2D Function (`love.sound.`) | Night Engine API (`Night.Sound` or `Night.Audio` internals) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|---------------------------------|-------------------------------------------------------------|---------------------------|--------------------------|------|
| `love.sound.newDecoder(filedata, bufferSize)` | `Night.Audio.NewDecoder(Night.FileData fileData, int bufferSize = 4096)` | `public static Night.Decoder NewDecoder(...)` <br> Creates a sound decoder. | Out of Scope | [ ] |
| `love.sound.newSoundData(samples, sampleRate, bitDepth, channels)` | `Night.Audio.NewSoundData(int samples, int sampleRate, int bitDepth, int channels)` or `Night.Audio.NewSoundData(byte[] rawPcmData, ...)` | `public static Night.SoundData NewSoundData(...)` <br> Creates raw sound data. | Out of Scope | [ ] |

**Functionality on `Night.Decoder` instances (if implemented):**
*   `decoder.GetBitDepth()`
*   `decoder.GetChannelCount()`
*   `decoder.GetDuration()`
*   `decoder.GetSampleRate()`
*   `decoder.Decode()` (returns a chunk of SoundData)
*   `decoder.Seek(offset)`

**Functionality on `Night.SoundData` instances (if implemented):**
*   `soundData.GetBitDepth()`
*   `soundData.GetChannelCount()`
*   `soundData.GetDuration()`
*   `soundData.GetSampleCount()`
*   `soundData.GetSampleRate()`
*   `soundData.GetSample(index)`
*   `soundData.SetSample(index, value)`
*   `soundData.Clone()`

**Night Engine Specific Types (if module were implemented):**
*   `Night.Decoder`: Represents an object that can decode audio from a stream or file data.
*   `Night.SoundData`: Represents raw PCM audio data in memory.
*   `Night.FileData`: Represents file data in memory (from `Night.Filesystem`).
