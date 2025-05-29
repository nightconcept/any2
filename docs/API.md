# Night / Love2D API

## Filesystem

### Types (Filesystem)

- FileSystemInfo

### Functions (Filesystem)

- GetInfo() - love.filesystem.getInfo
  - GetInfo(string path, FileSystemInfo info)
  - GetInfo(string path, FileType filterType, FileSystemInfo info)
  - GetInfo(string path, FileType? filterType)
- ReadBytes() - love.filesystem.readBytes
  - ReadBytes(string path)
- ReadText() - love.filesystem.readText
  - ReadText(string path)

### Enums (Filesystem)

- FileMode
- FileType

## Graphics

### Types (Graphics)

- Sprite

### Functions (Graphics)

- Circle() - love.graphics.circle
  - Circle(DrawMode mode, float x, float y, float radius, int segments)
- Clear() - love.graphics.clear
  - Clear(Color color)
- Draw() - love.graphics.draw
  - Draw(Sprite sprite, float x, float y, float rotation, float scaleX, float scaleY, float offsetX, float offsetY)
- Line() - love.graphics.line
  - Line(DrawMode mode, PointF[] points)
  - Line(float x1, float y1, float x2, float y2)
- NewImage() - love.graphics.newImage
  - NewImage(string filePath)
- Polygon() - love.graphics.polygon
  - Polygon(DrawMode mode, PointF[] vertices)
- Present() - love.graphics.present
- Rectangle() - love.graphics.rectangle
  - Rectangle(DrawMode mode, float x, float y, float width, float height)
- SetColor() - love.graphics.setColor
  - SetColor(Color color)
  - SetColor(byte r, byte g, byte b, byte a)

### Enums (Graphics)

- DrawMode

## Keyboard

### Functions (Keyboard)

- IsDown() - love.keyboard.isDown
  - IsDown(KeyCode key)

### Enums (Keyboard)

- KeyCode
- KeySymbol

## Mouse

### Functions (Mouse)

- GetPosition() - love.mouse.getPosition
- IsDown() - love.mouse.isDown
  - IsDown(MouseButton button)
- SetGrabbed() - love.mouse.setGrabbed
  - SetGrabbed(bool grabbed)
- SetRelativeMode() - love.mouse.setRelativeMode
  - SetRelativeMode(bool enabled)
- SetVisible() - love.mouse.setVisible
  - SetVisible(bool visible)

### Enums (Mouse)

- MouseButton

## System

### Functions (System)

- GetClipboardText() - love.system.getClipboardText
- SetClipboardText() - love.system.setClipboardText
  - SetClipboardText(string text)

## Timer

### Functions (Timer)

- GetAverageDelta() - love.timer.getAverageDelta
- GetDelta() - love.timer.getDelta
- GetFPS() - love.timer.getFPS
- GetTime() - love.timer.getTime
- Sleep() - love.timer.sleep
  - Sleep(double seconds)
- Step() - love.timer.step

## Window

### Functions (Window)

- Close() - love.window.close
- FromPixels() - love.window.fromPixels
  - FromPixels(float value)
- GetDPIScale() - love.window.getDPIScale
- GetDesktopDimensions() - love.window.getDesktopDimensions
  - GetDesktopDimensions(int displayIndex)
- GetDisplayCount() - love.window.getDisplayCount
- GetFullscreen() - love.window.getFullscreen
- GetFullscreenModes() - love.window.getFullscreenModes
  - GetFullscreenModes(int displayIndex)
- GetMode() - love.window.getMode
- IsOpen() - love.window.isOpen
- SetFullscreen() - love.window.setFullscreen
  - SetFullscreen(bool fullscreen, FullscreenType fsType)
- SetMode() - love.window.setMode
  - SetMode(int width, int height, SDL.WindowFlags flags)
- SetTitle() - love.window.setTitle
  - SetTitle(string title)
- ToPixels() - love.window.toPixels
  - ToPixels(float value)

### Enums (Window)

- FullscreenType
