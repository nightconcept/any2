## filesystem

- GetInfo() - love.filesystem.getInfo
  - GetInfo(string path, FileSystemInfo info)
  - GetInfo(string path, FileType filterType, FileSystemInfo info)
  - GetInfo(string path, FileType? filterType)
- ReadBytes() - love.filesystem.readBytes
  - ReadBytes(string path)
- ReadText() - love.filesystem.readText
  - ReadText(string path)

## graphics

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

## keyboard

- IsDown() - love.keyboard.isDown
  - IsDown(KeyCode key)

## mouse

- GetPosition() - love.mouse.getPosition
- IsDown() - love.mouse.isDown
  - IsDown(MouseButton button)

## nightsdl

- GetError() - love.nightsdl.getError
- GetVersion() - love.nightsdl.getVersion

## window

- Close() - love.window.close
- IsOpen() - love.window.isOpen
- SetMode() - love.window.setMode
  - SetMode(int width, int height, SDL.WindowFlags flags)
- SetTitle() - love.window.setTitle
  - SetTitle(string title)
