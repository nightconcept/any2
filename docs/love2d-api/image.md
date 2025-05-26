# `love.image` Module API Mapping

This document maps the functions available in the `love.image` module of Love2D to their proposed equivalents in the Night Engine. The functionality of this module is often integrated into `Night.Sprite` or `Night.Texture` objects, or handled during image loading. Most direct `love.image` functions are **Out of Scope** for the initial prototype as standalone static methods.

| Love2D Function (`love.image.`) | Night Engine API (`Night.Image` or `Texture`/`Sprite` methods) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|---------------------------------|----------------------------------------------------------------|---------------------------|--------------------------|------|
| `love.image.newImageData(width, height, format, data)` | `Night.Image.NewImageData(int width, int height, Night.PixelFormat format = RGBA8, byte[]? data = null)` | `public static Night.ImageData NewImageData(...)` <br> Creates raw image data. `Night.ImageData` would be a class/struct. | Out of Scope | [ ] |
| `love.image.isCompressed(filename)` or `love.image.isCompressed(filedata)` | `Night.Image.IsCompressed(string filePath)` or `Night.Image.IsCompressed(Night.FileData fileData)` | `public static bool IsCompressed(...)` <br> Checks if an image file is a compressed format LÖVE can load. | Out of Scope | [ ] |
| `love.image.newCompressedData(filename)` | `Night.Image.NewCompressedData(string filePath)` | `public static Night.CompressedImageData NewCompressedData(string filePath)` <br> Loads a compressed image file (e.g. DDS, KTX) into a special data object. | Out of Scope | [ ] |

**Related functionality in Night Engine (on `Sprite` or `Texture` or `ImageData` objects):**
*   Getting dimensions: `mySprite.GetWidth()`, `mySprite.GetHeight()`
*   Getting format: `myImageData.GetFormat()`
*   Manipulating pixel data: `myImageData.GetPixel(x,y)`, `myImageData.SetPixel(x,y,color)` (Likely Out of Scope for prototype)
*   Encoding/Decoding: Functionality to save an `ImageData` to a file (e.g., `myImageData.Encode("png", "filename.png")`) is Out of Scope.

**Night Engine Specific Types:**
*   `Night.ImageData`: Represents raw, uncompressed image data. Could have methods like `GetWidth()`, `GetHeight()`, `GetPixel()`, `SetPixel()`.
*   `Night.PixelFormat`: Enum for pixel formats (e.g., `RGBA8`, `RGB8`, `Luminance8`).
*   `Night.FileData`: Represents file data in memory (from `Night.Filesystem`).
*   `Night.CompressedImageData`: Represents compressed image data.
*   `Night.Sprite`: The primary object for loaded images, returned by `Night.Graphics.NewImage()`. It would internally manage texture data.
