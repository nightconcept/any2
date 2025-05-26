# `love` Module API Mapping

This document maps the functions available in the base `love` module of Love2D to their proposed equivalents in the Night Engine.

| Love2D Function (`love.`) | Night Engine API (`Night.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|---------------------------|-----------------------------|---------------------------|--------------------------|------|
| `love.getVersion()`       | `Night.Engine.GetVersion()` | `public static string GetVersion()` <br> Returns a string like "Major.Minor.Revision Codename". | In Scope | [ ] |
| `love.setDeprecationOutput(boolean enabled)` | `Night.Engine.SetDeprecationOutput(bool enabled)` | `public static void SetDeprecationOutput(bool enabled)` <br> Controls whether Love2D's deprecation warnings are output. May or may not be relevant for Night. | Out of Scope (Low Priority) | [ ] |
| `love.run()`              | `Night.Engine.Run<T>()` or `Night.Engine.Run(IGame gameInstance)` | `public static void Run<T>() where T : IGame, new()` <br> `public static void Run(IGame gameInstance)` <br> This is the main entry point that starts the game loop. The user provides a game class/instance. | In Scope | [x] |
| `love.load(arg)`          | `MyGame.Load(string[] args)` | Implemented by the user in their game class: `void Load(string[] args);` <br> Called once at the beginning. `arg` in Love2D contains command-line arguments. | In Scope | [x] |
| `love.update(dt)`         | `MyGame.Update(double deltaTime)` | Implemented by the user: `void Update(double deltaTime);` <br> Called every frame. | In Scope | [x] |
| `love.draw()`             | `MyGame.Draw()`             | Implemented by the user: `void Draw();` <br> Called every frame after update. | In Scope | [x] |
| `love.quit()`             | `MyGame.Quit()` or `Night.Engine.Quit()` | User implementation: `bool Quit();` (return true to allow quit) <br> Engine initiated: `Night.Engine.RequestQuit()` or similar. Love2D `love.quit` can also be an event. | In Scope (Basic window close event handling) | [ ] |
| `love.focus(f)`           | `MyGame.FocusChanged(bool hasFocus)` | User implementation: `void FocusChanged(bool hasFocus);` | In Scope | [ ] |
| `love.mousefocus(f)`      | `MyGame.MouseFocusChanged(bool hasFocus)` | User implementation: `void MouseFocusChanged(bool hasFocus);` | Out of Scope (Covered by general focus) | [ ] |
| `love.visible(v)`         | `MyGame.VisibilityChanged(bool isVisible)` | User implementation: `void VisibilityChanged(bool isVisible);` | In Scope | [ ] |
| `love.keypressed(key, scancode, isrepeat)` | `MyGame.KeyPressed(Night.KeyCode key, string scancode, bool isRepeat)` | User implementation: `void KeyPressed(Night.KeyCode key, /* SDL_Scancode scancode, */ bool isRepeat);` <br> `scancode` might be abstracted away or be an internal SDL detail. | In Scope | [ ] |
| `love.keyreleased(key, scancode)` | `MyGame.KeyReleased(Night.KeyCode key, string scancode)` | User implementation: `void KeyReleased(Night.KeyCode key /*, SDL_Scancode scancode */);` | In Scope | [ ] |
| `love.textinput(text)`    | `MyGame.TextInput(string text)` | User implementation: `void TextInput(string text);` | In Scope (but low priority for prototype) | [ ] |
| `love.mousepressed(x, y, button, istouch, presses)` | `MyGame.MousePressed(int x, int y, Night.MouseButton button, bool isTouch, int presses)` | User implementation: `void MousePressed(int x, int y, Night.MouseButton button, int presses);` <br> `isTouch` might be handled separately if touch events are distinct. | In Scope | [ ] |
| `love.mousereleased(x, y, button, istouch)` | `MyGame.MouseReleased(int x, int y, Night.MouseButton button, bool isTouch)` | User implementation: `void MouseReleased(int x, int y, Night.MouseButton button);` | In Scope | [ ] |
| `love.mousemoved(x, y, dx, dy, istouch)` | `MyGame.MouseMoved(int x, int y, int deltaX, int deltaY, bool isTouch)` | User implementation: `void MouseMoved(int x, int y, int deltaX, int deltaY);` | In Scope | [ ] |
| `love.wheelmoved(x, y)`   | `MyGame.MouseWheelMoved(int deltaX, int deltaY)` | User implementation: `void MouseWheelMoved(int deltaX, int deltaY);` | In Scope (Basic support) | [ ] |
| `love.joystickpressed(joystick, button)` | `MyGame.JoystickPressed(Night.Joystick joystick, int button)` | User implementation: `void JoystickPressed(Night.Joystick joystick, int button);` | Out of Scope | [ ] |
| `love.joystickreleased(joystick, button)` | `MyGame.JoystickReleased(Night.Joystick joystick, int button)` | User implementation: `void JoystickReleased(Night.Joystick joystick, int button);` | Out of Scope | [ ] |
| `love.joystickaxis(joystick, axis, value)` | `MyGame.JoystickAxisMoved(Night.Joystick joystick, int axis, float value)` | User implementation: `void JoystickAxisMoved(Night.Joystick joystick, int axis, float value);` | Out of Scope | [ ] |
| `love.joystickhat(joystick, hat, direction)` | `MyGame.JoystickHatMoved(Night.Joystick joystick, int hat, Night.HatDirection direction)` | User implementation: `void JoystickHatMoved(Night.Joystick joystick, int hat, Night.HatDirection direction);` | Out of Scope | [ ] |
| `love.joystickadded(joystick)` | `MyGame.JoystickAdded(Night.Joystick joystick)` | User implementation: `void JoystickAdded(Night.Joystick joystick);` | Out of Scope | [ ] |
| `love.joystickremoved(joystick)` | `MyGame.JoystickRemoved(Night.Joystick joystick)` | User implementation: `void JoystickRemoved(Night.Joystick joystick);` | Out of Scope | [ ] |
| `love.touchpressed(id, x, y, dx, dy, pressure)` | `MyGame.TouchPressed(long id, float x, float y, float deltaX, float deltaY, float pressure)` | User implementation: `void TouchPressed(long id, float x, float y, float deltaX, float deltaY, float pressure);` | Out of Scope | [ ] |
| `love.touchreleased(id, x, y, dx, dy, pressure)` | `MyGame.TouchReleased(long id, float x, float y, float deltaX, float deltaY, float pressure)` | User implementation: `void TouchReleased(long id, float x, float y, float deltaX, float deltaY, float pressure);` | Out of Scope | [ ] |
| `love.touchmoved(id, x, y, dx, dy, pressure)` | `MyGame.TouchMoved(long id, float x, float y, float deltaX, float deltaY, float pressure)` | User implementation: `void TouchMoved(long id, float x, float y, float deltaX, float deltaY, float pressure);` | Out of Scope | [ ] |
| `love.lowmemory()`        | `MyGame.LowMemory()`        | User implementation: `void LowMemory();` | Out of Scope | [ ] |
| `love.threaderror(thread, errorstr)` | `MyGame.ThreadError(Night.Thread thread, string error)` | User implementation: `void ThreadError(Night.Thread thread, string error);` | Out of Scope | [ ] |
| `love.directorydropped(path)` | `MyGame.DirectoryDropped(string path)` | User implementation: `void DirectoryDropped(string path);` | Out of Scope | [ ] |
| `love.filedropped(file)`  | `MyGame.FileDropped(Night.File file)` | User implementation: `void FileDropped(Night.File file);` <br> `Night.File` would be a wrapper for file data. | Out of Scope | [ ] |
| `love.resize(w, h)`       | `MyGame.WindowResized(int width, int height)` | User implementation: `void WindowResized(int width, int height);` | In Scope | [ ] |

*Note: Many `love` module functions are event callbacks. In Night Engine, these will be methods the user implements in their game class, which are then called by `Night.Engine`.*
