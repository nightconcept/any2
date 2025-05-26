# `love.font` Module API Mapping

This document maps the functions available in the `love.font` module of Love2D to their proposed equivalents in the Night Engine. This entire module is **Out of Scope** for the initial prototype. The primary way to get a font object in Night Engine would be `Night.Graphics.NewFont()`.

| Love2D Function (`love.font.`) | Night Engine API (`Night.Font` methods or `Night.Graphics`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|--------------------------------|-------------------------------------------------------------|---------------------------|--------------------------|------|
| `love.font.newRasterizer(filename, size)` or `love.font.newRasterizer(filedata, size)` or `love.font.newRasterizer(size)` | `Night.Graphics.NewFontRasterizer(string filePath, int size)` etc. | `public static Night.FontRasterizer NewFontRasterizer(...)` <br> Creates a font rasterizer. `Night.Font` would likely encapsulate this. | Out of Scope | [ ] |
| `love.font.newGlyphData(rasterizer, glyph)` | `(Night.FontRasterizer).NewGlyphData(char glyph)` or `(Night.FontRasterizer).NewGlyphData(uint glyph)` | `public Night.GlyphData NewGlyphData(char glyph)` (method on `FontRasterizer` or `Font`) | Out of Scope | [ ] |

**Related functionality in Night Engine (on `Night.Font` objects):**
*   Getting font height: `myFont.GetHeight()`
*   Getting ascent/descent: `myFont.GetAscent()`, `myFont.GetDescent()`
*   Getting baseline: `myFont.GetBaseline()`
*   Getting line height: `myFont.GetLineHeight()`
*   Getting text width: `myFont.GetWidth(string text)`
*   Wrapping text: `myFont.Wrap(string text, float wrapLimit)`
*   Setting fallback fonts: `myFont.SetFallback(Night.Font fallback1, ...)`

**Night Engine Specific Types:**
*   `Night.Font`: Represents a loaded font. Created via `Night.Graphics.NewFont()`. Would have methods for metrics and properties.
*   `Night.FontRasterizer`: Internal or advanced type for rasterizing glyphs.
*   `Night.GlyphData`: Represents rasterized data for a single glyph.
