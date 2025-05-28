# `love.graphics` Module API Mapping

This document maps the functions available in the `love.graphics` module of Love2D to their proposed equivalents in the Night Engine.

| Love2D Function (`love.graphics.`) | Night Engine API (`Night.Graphics.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|------------------------------------|--------------------------------------|---------------------------|--------------------------|------|
| `love.graphics.arc(mode, arcType, x, y, radius, angle1, angle2, segments)` | `Night.Graphics.DrawArc(Night.DrawMode mode, Night.ArcType arcType, float x, float y, float radius, float angle1, float angle2, int? segments = null)` | `public static void DrawArc(Night.DrawMode mode, Night.ArcType arcType, float x, float y, float radius, float angle1, float angle2, int? segments = null)` <br> `DrawMode` enum: `Fill`, `Line`. `ArcType` enum: `Open`, `Closed`, `Pie`. Segments auto-calculated if null. | Out of Scope | [ ] |
| `love.graphics.circle(mode, x, y, radius, segments)` | `Night.Graphics.DrawCircle(Night.DrawMode mode, float x, float y, float radius, int? segments = null)` | `public static void DrawCircle(Night.DrawMode mode, float x, float y, float radius, int? segments = null)` | Out of Scope | [ ] |
| `love.graphics.clear(r, g, b, a)` or `love.graphics.clear(color)` | `Night.Graphics.Clear(Night.Color color)` or `Night.Graphics.Clear(byte r, byte g, byte b, byte a = 255)` | `public static void Clear(Night.Color color)` <br> `public static void Clear(byte r, byte g, byte b, byte a = 255)` | In Scope | [ ] |
| `love.graphics.discard(discardColor, discardStencil)` | `Night.Graphics.Discard(bool discardColor = true, bool discardStencil = true)` | `public static void Discard(bool discardColor = true, bool discardStencil = true)` <br> Discards render target contents. | Out of Scope | [ ] |
| `love.graphics.draw(drawable, x, y, r, sx, sy, ox, oy, kx, ky)` | `Night.Graphics.Draw(Night.IDrawable drawable, float x, float y, float rotation = 0, float scaleX = 1, float scaleY = 1, float offsetX = 0, float offsetY = 0, float shearX = 0, float shearY = 0)` | `public static void Draw(Night.IDrawable drawable, float x, float y, float rotation = 0, float scaleX = 1, float scaleY = 1, float offsetX = 0, float offsetY = 0, float shearX = 0, float shearY = 0)` <br> `IDrawable` could be `Sprite`, `Text`, `Shape`, etc. | In Scope (for Sprites) | [ ] |
| `love.graphics.draw(texture, quad, x, y, r, sx, sy, ox, oy, kx, ky)` | `Night.Graphics.Draw(Night.Texture texture, Night.Quad quad, float x, float y, float rotation = 0, float scaleX = 1, float scaleY = 1, float offsetX = 0, float offsetY = 0, float shearX = 0, float shearY = 0)` | `public static void Draw(Night.Texture texture, Night.Quad quad, ...)` <br> For drawing parts of a texture. | In Scope (for Sprites with Quads) | [ ] |
| `love.graphics.ellipse(mode, x, y, radiusx, radiusy, segments)` | `Night.Graphics.DrawEllipse(Night.DrawMode mode, float x, float y, float radiusX, float radiusY, int? segments = null)` | `public static void DrawEllipse(Night.DrawMode mode, float x, float y, float radiusX, float radiusY, int? segments = null)` | Out of Scope | [ ] |
| `love.graphics.getBackgroundColor()` | `Night.Graphics.GetBackgroundColor()` | `public static Night.Color GetBackgroundColor()` | In Scope | [ ] |
| `love.graphics.getBlendMode()`     | `Night.Graphics.GetBlendMode()`    | `public static (Night.BlendMode mode, Night.BlendAlphaMode alphaMode) GetBlendMode()` | Out of Scope | [ ] |
| `love.graphics.getCanvas()`        | `Night.Graphics.GetRenderTarget()` | `public static Night.IRenderTarget GetRenderTarget()` <br> Returns current render target (Canvas or screen). | Out of Scope | [ ] |
| `love.graphics.getCanvasFormats()` | `Night.Graphics.GetSupportedRenderTargetFormats()` | `public static Night.RenderTargetFormat[] GetSupportedRenderTargetFormats()` | Out of Scope | [ ] |
| `love.graphics.getColor()`         | `Night.Graphics.GetColor()`        | `public static Night.Color GetColor()` | In Scope | [ ] |
| `love.graphics.getColorMask()`     | `Night.Graphics.GetColorMask()`    | `public static (bool r, bool g, bool b, bool a) GetColorMask()` | Out of Scope | [ ] |
| `love.graphics.getDefaultFilter()` | `Night.Graphics.GetDefaultFilter()` | `public static Night.FilterMode GetDefaultFilter()` <br> `FilterMode` enum: `Linear`, `Nearest`. | In Scope | [ ] |
| `love.graphics.getDepthMode()`     | `Night.Graphics.GetDepthMode()`    | `public static (Night.CompareMode? mode, bool write) GetDepthMode()` | Out of Scope | [ ] |
| `love.graphics.getDimensions()`    | `Night.Graphics.GetDimensions()`   | `public static (int width, int height) GetDimensions()` <br> Gets dimensions of current render target (screen or canvas). | In Scope | [ ] |
| `love.graphics.getFont()`          | `Night.Graphics.GetFont()`         | `public static Night.Font GetFont()` | Out of Scope | [ ] |
| `love.graphics.getHeight()`        | `Night.Graphics.GetHeight()`       | `public static int GetHeight()` <br> Height of current render target. | In Scope | [ ] |
| `love.graphics.getLineWidth()`     | `Night.Graphics.GetLineWidth()`    | `public static float GetLineWidth()` | Out of Scope | [ ] |
| `love.graphics.getLineStyle()`     | `Night.Graphics.GetLineStyle()`    | `public static Night.LineStyle GetLineStyle()` <br> `LineStyle` enum: `Smooth`, `Rough`. | Out of Scope | [ ] |
| `love.graphics.getLineJoin()`      | `Night.Graphics.GetLineJoin()`     | `public static Night.LineJoin GetLineJoin()` <br> `LineJoin` enum: `None`, `Miter`, `Bevel`. | Out of Scope | [ ] |
| `love.graphics.getPointSize()`     | `Night.Graphics.GetPointSize()`    | `public static float GetPointSize()` | Out of Scope | [ ] |
| `love.graphics.getRendererInfo()`  | `Night.Graphics.GetRendererInfo()` | `public static Night.RendererInfo GetRendererInfo()` <br> `RendererInfo` class: `Name`, `Version`, `Vendor`, `Device`. | In Scope | [ ] |
| `love.graphics.getScissor()`       | `Night.Graphics.GetScissor()`      | `public static Night.Rectangle? GetScissor()` | Out of Scope | [ ] |
| `love.graphics.getShader()`        | `Night.Graphics.GetShader()`       | `public static Night.Shader GetShader()` | Out of Scope | [ ] |
| `love.graphics.getStats()`         | `Night.Graphics.GetStats()`        | `public static Night.GraphicsStats GetStats()` <br> `GraphicsStats` class: `DrawCalls`, `CanvasSwitches`, `ShaderSwitches`, etc. | In Scope (Basic stats) | [ ] |
| `love.graphics.getStencilTest()`   | `Night.Graphics.GetStencilTest()`  | `public static (Night.CompareMode? mode, int value) GetStencilTest()` | Out of Scope | [ ] |
| `love.graphics.getWidth()`         | `Night.Graphics.GetWidth()`        | `public static int GetWidth()` <br> Width of current render target. | In Scope | [ ] |
| `love.graphics.intersectScissor(x, y, width, height)` | `Night.Graphics.IntersectScissor(int x, int y, int width, int height)` | `public static void IntersectScissor(int x, int y, int width, int height)` | Out of Scope | [ ] |
| `love.graphics.isWireframe()`      | `Night.Graphics.IsWireframe()`     | `public static bool IsWireframe()` | Out of Scope | [ ] |
| `love.graphics.line(x1, y1, x2, y2, ...)` or `love.graphics.line(points)` | `Night.Graphics.DrawLine(params float[] points)` or `Night.Graphics.DrawLine(Night.PointF[] points)` | `public static void DrawLine(params float[] points)` <br> `public static void DrawLine(Night.PointF[] points)` | Out of Scope | [ ] |
| `love.graphics.newCanvas(width, height, format, msaa)` | `Night.Graphics.NewRenderTarget(int width, int height, Night.RenderTargetFormat format = Default, int msaa = 0)` | `public static Night.IRenderTarget NewRenderTarget(...)` | Out of Scope | [ ] |
| `love.graphics.newFont(filename, size)` or `love.graphics.newFont(size)` | `Night.Graphics.NewFont(string filePath, int size)` or `Night.Graphics.NewFont(int size)` | `public static Night.Font NewFont(...)` <br> Uses default font if no path. | Out of Scope | [ ] |
| `love.graphics.newImage(filename)` | `Night.Graphics.NewImage(string filePath)` | `public static Night.Sprite NewImage(string filePath)` <br> PRD refers to `Sprite` as return type. | In Scope | [ ] |
| `love.graphics.newImageFont(filename, glyphs, extraspacing)` | `Night.Graphics.NewImageFont(string filePath, string glyphs, int extraSpacing = 0)` | `public static Night.Font NewImageFont(...)` | Out of Scope | [ ] |
| `love.graphics.newQuad(x, y, width, height, sw, sh)` | `Night.Graphics.NewQuad(float x, float y, float width, float height, float sourceWidth, float sourceHeight)` | `public static Night.Quad NewQuad(...)` | In Scope | [ ] |
| `love.graphics.newShader(pixelcode, vertexcode)` | `Night.Graphics.NewShader(string pixelShaderCode, string vertexShaderCode = null)` | `public static Night.Shader NewShader(...)` | Out of Scope | [ ] |
| `love.graphics.newSpriteBatch(texture, size, usagehint)` | `Night.Graphics.NewSpriteBatch(Night.Texture texture, int size, Night.UsageHint hint = Dynamic)` | `public static Night.SpriteBatch NewSpriteBatch(...)` | Out of Scope | [ ] |
| `love.graphics.newText(font, textparts)` | `Night.Graphics.NewText(Night.Font font, params (string text, Night.Color? color)[] textParts)` | `public static Night.Text NewText(...)` | Out of Scope | [ ] |
| `love.graphics.newVideo(filename, options)` | `Night.Graphics.NewVideo(string filePath, Night.VideoOptions? options = null)` | `public static Night.Video NewVideo(...)` | Out of Scope | [ ] |
| `love.graphics.origin()`           | `Night.Graphics.ResetTransform()`  | `public static void ResetTransform()` <br> Resets current transformation to identity. | In Scope | [ ] |
| `love.graphics.points(coords, colors)` | `Night.Graphics.DrawPoints(Night.PointF[] positions, Night.Color[]? colors = null)` | `public static void DrawPoints(...)` | Out of Scope | [ ] |
| `love.graphics.polygon(mode, vertices)` | `Night.Graphics.DrawPolygon(Night.DrawMode mode, params Night.PointF[] vertices)` | `public static void DrawPolygon(...)` | Out of Scope | [ ] |
| `love.graphics.pop()`              | `Night.Graphics.PopTransform()`    | `public static void PopTransform()` | In Scope | [ ] |
| `love.graphics.present()`          | `Night.Graphics.Present()`         | `public static void Present()` <br> Called by engine after `MyGame.Draw()`. | In Scope | [ ] |
| `love.graphics.print(text, x, y, r, sx, sy, ox, oy, kx, ky)` | `Night.Graphics.Print(string text, float x, float y, float rotation = 0, ...)` | `public static void Print(string text, float x, float y, ...)` <br> Uses current font. | Out of Scope | [ ] |
| `love.graphics.printf(text, x, y, limit, align, r, sx, sy, ox, oy, kx, ky)` | `Night.Graphics.PrintF(string text, float x, float y, float wrapLimit, Night.TextAlign align = Left, ...)` | `public static void PrintF(...)` | Out of Scope | [ ] |
| `love.graphics.push(stacktype)`    | `Night.Graphics.PushTransform(Night.StackType type = All)` | `public static void PushTransform(Night.StackType type = Night.StackType.All)` <br> `StackType` enum: `All`, `Transform`. | In Scope | [ ] |
| `love.graphics.rectangle(mode, x, y, width, height, rx, ry, segments)` | `Night.Graphics.DrawRectangle(Night.DrawMode mode, float x, float y, float width, float height, float cornerRadiusX = 0, float cornerRadiusY = 0, int? segments = null)` | `public static void DrawRectangle(...)` | Out of Scope | [ ] |
| `love.graphics.reset()`            | `Night.Graphics.ResetState()`      | `public static void ResetState()` <br> Resets all graphics state (color, blend mode, etc.) | In Scope | [ ] |
| `love.graphics.rotate(angle)`      | `Night.Graphics.Rotate(float angleInRadians)` | `public static void Rotate(float angleInRadians)` | In Scope | [ ] |
| `love.graphics.scale(sx, sy)`      | `Night.Graphics.Scale(float scaleX, float scaleY)` | `public static void Scale(float scaleX, float scaleY)` | In Scope | [ ] |
| `love.graphics.shear(kx, ky)`      | `Night.Graphics.Shear(float shearX, float shearY)` | `public static void Shear(float shearX, float shearY)` | In Scope | [ ] |
| `love.graphics.setBackgroundColor(r, g, b, a)` or `love.graphics.setBackgroundColor(color)` | `Night.Graphics.SetBackgroundColor(Night.Color color)` or `Night.Graphics.SetBackgroundColor(byte r, byte g, byte b, byte a = 255)` | `public static void SetBackgroundColor(...)` | In Scope | [ ] |
| `love.graphics.setBlendMode(mode, alphamode)` | `Night.Graphics.SetBlendMode(Night.BlendMode mode, Night.BlendAlphaMode alphaMode = Multiply)` | `public static void SetBlendMode(...)` | Out of Scope | [ ] |
| `love.graphics.setCanvas(canvas)` or `love.graphics.setCanvas()` | `Night.Graphics.SetRenderTarget(Night.IRenderTarget? target = null)` | `public static void SetRenderTarget(Night.IRenderTarget? target = null)` <br> `null` sets to screen. | Out of Scope | [ ] |
| `love.graphics.setColor(r, g, b, a)` or `love.graphics.setColor(color)` | `Night.Graphics.SetColor(Night.Color color)` or `Night.Graphics.SetColor(byte r, byte g, byte b, byte a = 255)` | `public static void SetColor(...)` | In Scope | [ ] |
| `love.graphics.setColorMask(r, g, b, a)` | `Night.Graphics.SetColorMask(bool r, bool g, bool b, bool a)` | `public static void SetColorMask(bool r, bool g, bool b, bool a)` | Out of Scope | [ ] |
| `love.graphics.setDefaultFilter(min, mag, anisotropy)` | `Night.Graphics.SetDefaultFilter(Night.FilterMode min, Night.FilterMode? mag = null, float anisotropy = 1.0f)` | `public static void SetDefaultFilter(...)` <br> `mag` defaults to `min` if null. | In Scope | [ ] |
| `love.graphics.setDepthMode(mode, write)` | `Night.Graphics.SetDepthMode(Night.CompareMode? mode, bool write)` | `public static void SetDepthMode(Night.CompareMode? mode, bool write)` | Out of Scope | [ ] |
| `love.graphics.setFont(font)`      | `Night.Graphics.SetFont(Night.Font font)` | `public static void SetFont(Night.Font font)` | Out of Scope | [ ] |
| `love.graphics.setLineWidth(width)` | `Night.Graphics.SetLineWidth(float width)` | `public static void SetLineWidth(float width)` | Out of Scope | [ ] |
| `love.graphics.setLineStyle(style)` | `Night.Graphics.SetLineStyle(Night.LineStyle style)` | `public static void SetLineStyle(Night.LineStyle style)` | Out of Scope | [ ] |
| `love.graphics.setLineJoin(join)`  | `Night.Graphics.SetLineJoin(Night.LineJoin join)` | `public static void SetLineJoin(Night.LineJoin join)` | Out of Scope | [ ] |
| `love.graphics.setPointSize(size)` | `Night.Graphics.SetPointSize(float size)` | `public static void SetPointSize(float size)` | Out of Scope | [ ] |
| `love.graphics.setScissor(x, y, width, height)` or `love.graphics.setScissor()` | `Night.Graphics.SetScissor(int? x, int? y, int? width, int? height)` or `Night.Graphics.SetScissor(Night.Rectangle? rect)` | `public static void SetScissor(Night.Rectangle? rect)` <br> `null` disables scissor. | Out of Scope | [ ] |
| `love.graphics.setShader(shader)` or `love.graphics.setShader()` | `Night.Graphics.SetShader(Night.Shader? shader = null)` | `public static void SetShader(Night.Shader? shader = null)` | Out of Scope | [ ] |
| `love.graphics.setStencilTest(comparemode, comparevalue)` or `love.graphics.setStencilTest()` | `Night.Graphics.SetStencilTest(Night.CompareMode? mode = null, int value = 0)` | `public static void SetStencilTest(Night.CompareMode? mode = null, int value = 0)` | Out of Scope | [ ] |
| `love.graphics.setWireframe(enable)` | `Night.Graphics.SetWireframe(bool enable)` | `public static void SetWireframe(bool enable)` | Out of Scope | [ ] |
| `love.graphics.stencil(stencilfunction, action, value, keepvalues)` | `Night.Graphics.Stencil(Action stencilFunction, Night.StencilAction action = Replace, int value = 1, bool keepValues = false)` | `public static void Stencil(...)` <br> Complex. | Out of Scope | [ ] |
| `love.graphics.translate(dx, dy)`  | `Night.Graphics.Translate(float deltaX, float deltaY)` | `public static void Translate(float deltaX, float deltaY)` | In Scope | [ ] |
| `love.graphics.transformPoint(worldX, worldY)` | `Night.Graphics.TransformPoint(float worldX, float worldY)` | `public static (float screenX, float screenY) TransformPoint(float worldX, float worldY)` | In Scope | [ ] |
| `love.graphics.inverseTransformPoint(screenX, screenY)` | `Night.Graphics.InverseTransformPoint(float screenX, float screenY)` | `public static (float worldX, float worldY) InverseTransformPoint(float screenX, float screenY)` | In Scope | [ ] |

**Night Engine Specific Types:**
*   `Night.DrawMode`: Enum (`Fill`, `Line`).
*   `Night.ArcType`: Enum (`Open`, `Closed`, `Pie`).
*   `Night.IDrawable`: Interface for drawable objects (Sprite, Text, etc.).
*   `Night.Texture`: Represents a texture (likely part of `Night.Image` or `Night.Sprite`).
*   `Night.Quad`: Represents a portion of a texture.
*   `Night.Color`: Struct/class for color (RGBA).
*   `Night.BlendMode`: Enum for blending (e.g., `Alpha`, `Add`, `Subtract`, `Multiply`).
*   `Night.BlendAlphaMode`: Enum for alpha blending (e.g., `Multiply`, `PreMultiplied`).
*   `Night.IRenderTarget`: Interface for render targets (Canvas or screen).
*   `Night.RenderTargetFormat`: Enum for pixel formats of render targets.
*   `Night.FilterMode`: Enum (`Linear`, `Nearest`).
*   `Night.CompareMode`: Enum for depth/stencil tests (e.g., `Less`, `Equal`, `Greater`, `Always`).
*   `Night.Font`: Represents a font.
*   `Night.LineStyle`: Enum (`Smooth`, `Rough`).
*   `Night.LineJoin`: Enum (`None`, `Miter`, `Bevel`).
*   `Night.RendererInfo`: Class with properties like `Name`, `Version`, `Vendor`, `Device`.
*   `Night.Rectangle`: Struct/class for a rectangle (X, Y, Width, Height).
*   `Night.Shader`: Represents a shader program.
*   `Night.GraphicsStats`: Class for graphics statistics.
*   `Night.PointF`: Struct for a 2D point with float coordinates.
*   `Night.Sprite`: Represents an image that can be drawn. (Corresponds to Love2D Image)
*   `Night.SpriteBatch`: For optimized drawing of many sprites from the same texture.
*   `Night.Text`: Represents renderable text.
*   `Night.Video`: Represents a video that can be drawn.
*   `Night.VideoOptions`: Options for video loading.
*   `Night.StackType`: Enum (`All`, `Transform`).
*   `Night.TextAlign`: Enum (`Left`, `Center`, `Right`, `Justify`).
*   `Night.StencilAction`: Enum for stencil operations (e.g., `Keep`, `Replace`, `Increment`).
*   `Night.UsageHint`: Enum for SpriteBatch (`Static`, `Dynamic`, `Stream`).
