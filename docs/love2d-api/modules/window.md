# `love.window` Module API Mapping

This document maps the functions available in the `love.window` module of Love2D to their proposed equivalents in the Night Engine.

| Love2D Function (`love.window.`) | Night Engine API (`Night.Window.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|----------------------------------|------------------------------------|---------------------------|--------------------------|------|
| `love.window.close()`            | `Night.Window.Close()`             | `public static void Close()` <br> Requests to close the window. The `MyGame.Quit()` callback will be invoked. | In Scope | [x] |
| `love.window.displaySleepEnabled()` | `Night.Window.IsDisplaySleepEnabled()` | `public static bool IsDisplaySleepEnabled()` | Out of Scope | [ ] |
| `love.window.fromPixels(px_x, px_y)` | `Night.Window.FromPixels(double pixelX, double pixelY)` | `public static (double x, double y) FromPixels(double pixelX, double pixelY)` <br> Converts pixel coordinates to density-independent units. | In Scope (if high DPI is handled) | [ ] |
| `love.window.getDesktopDimensions(displayindex)` | `Night.Window.GetDesktopDimensions(int displayIndex = 0)` | `public static (int width, int height) GetDesktopDimensions(int displayIndex = 0)` | In Scope (for default display) | [ ] |
| `love.window.getDimensions()`      | `Night.Window.GetDimensions()`     | `public static (int width, int height) GetDimensions()` | In Scope | [ ] |
| `love.window.getDisplayCount()`    | `Night.Window.GetDisplayCount()`   | `public static int GetDisplayCount()` | In Scope (for default display awareness) | [ ] |
| `love.window.getDisplayName(displayindex)` | `Night.Window.GetDisplayName(int displayIndex = 0)` | `public static string GetDisplayName(int displayIndex = 0)` | Out of Scope | [ ] |
| `love.window.getFullscreen()`      | `Night.Window.IsFullscreen()`      | `public static bool IsFullscreen()` <br> Returns true if fullscreen. Also need `Night.Window.GetFullscreenMode()` for type. | In Scope | [ ] |
| `love.window.getFullscreenModes(displayindex)` | `Night.Window.GetFullscreenModes(int displayIndex = 0)` | `public static Night.FullscreenMode[] GetFullscreenModes(int displayIndex = 0)` <br> `FullscreenMode` struct/class: `int Width, int Height, int RefreshRate`. | In Scope (for setting fullscreen) | [ ] |
| `love.window.getIcon()`            | `Night.Window.GetIcon()`           | `public static Night.ImageData? GetIcon()` | In Scope | [x] |
| `love.window.getMode()`            | `Night.Window.GetMode()`           | `public static (int width, int height, Night.WindowFlags flags) GetMode()` <br> `WindowFlags` would be a struct/class. | In Scope | [ ] |
| `love.window.getPixelDimensions()` | `Night.Window.GetPixelDimensions()` | `public static (int pixelWidth, int pixelHeight) GetPixelDimensions()` | In Scope (if high DPI is handled) | [ ] |
| `love.window.getPixelScale()`      | `Night.Window.GetPixelScale()`     | `public static double GetPixelScale()` | In Scope (if high DPI is handled) | [ ] |
| `love.window.getPosition()`        | `Night.Window.GetPosition()`       | `public static (int x, int y, int displayIndex) GetPosition()` | In Scope | [ ] |
| `love.window.getTitle()`           | `Night.Window.GetTitle()`          | `public static string GetTitle()` | In Scope | [ ] |
| `love.window.hasFocus()`           | `Night.Window.HasFocus()`          | `public static bool HasFocus()` | In Scope | [ ] |
| `love.window.hasMouseFocus()`      | `Night.Window.HasMouseFocus()`     | `public static bool HasMouseFocus()` | In Scope | [ ] |
| `love.window.isMaximized()`        | `Night.Window.IsMaximized()`       | `public static bool IsMaximized()` | In Scope | [ ] |
| `love.window.isMinimized()`        | `Night.Window.IsMinimized()`       | `public static bool IsMinimized()` | In Scope | [ ] |
| `love.window.isOpen()`             | `Night.Window.IsOpen()`            | `public static bool IsOpen()` <br> Checks if the window is open and the game should continue running. | In Scope | [x] |
| `love.window.isVisible()`          | `Night.Window.IsVisible()`         | `public static bool IsVisible()` | In Scope | [ ] |
| `love.window.maximize()`           | `Night.Window.Maximize()`          | `public static void Maximize()` | In Scope | [ ] |
| `love.window.minimize()`           | `Night.Window.Minimize()`          | `public static void Minimize()` | In Scope | [ ] |
| `love.window.requestAttention(continuous)` | `Night.Window.RequestAttention(bool continuous = false)` | `public static void RequestAttention(bool continuous = false)` | Out of Scope | [ ] |
| `love.window.restore()`            | `Night.Window.Restore()`           | `public static void Restore()` <br> Restores after minimize/maximize. | In Scope | [ ] |
| `love.window.setDisplaySleepEnabled(enable)` | `Night.Window.SetDisplaySleepEnabled(bool enable)` | `public static void SetDisplaySleepEnabled(bool enable)` | Out of Scope | [ ] |
| `love.window.setFullscreen(fullscreen, fstype)` | `Night.Window.SetFullscreen(bool fullscreen, Night.FullscreenType type = Night.FullscreenType.Desktop)` | `public static bool SetFullscreen(bool fullscreen, Night.FullscreenType type = Night.FullscreenType.Desktop)` <br> `FullscreenType` enum: `Desktop`, `Exclusive`. Returns success. | In Scope | [ ] |
| `love.window.setIcon(imagedata)`   | `Night.Window.SetIcon(string imagePath)` | `public static bool SetIcon(string imagePath)` | In Scope | [x] |
| `love.window.setMode(width, height, flags)` | `Night.Window.SetMode(int width, int height, Night.WindowFlags? flags = null)` | `public static bool SetMode(int width, int height, Night.WindowFlags? flags = null)` <br> `flags` could include: `Fullscreen`, `Resizable`, `Borderless`, `VSync`, `MinMSAA`, `DepthBits`, `StencilBits`. Returns success. | In Scope | [x] |
| `love.window.setPosition(x, y, displayindex)` | `Night.Window.SetPosition(int x, int y, int displayIndex = -1)` | `public static void SetPosition(int x, int y, int displayIndex = -1)` <br> `displayIndex = -1` could mean current or primary. | In Scope | [ ] |
| `love.window.setTitle(title)`      | `Night.Window.SetTitle(string title)` | `public static void SetTitle(string title)` | In Scope | [x] |
| `love.window.toPixels(x, y)`       | `Night.Window.ToPixels(double x, double y)` | `public static (double pixelX, double pixelY) ToPixels(double x, double y)` <br> Converts density-independent units to pixel coordinates. | In Scope (if high DPI is handled) | [ ] |
| `love.window.updateMode(width, height, flags)` | `Night.Window.UpdateMode(int width, int height, Night.WindowFlags? flags = null)` | `public static bool UpdateMode(int width, int height, Night.WindowFlags? flags = null)` <br> Similar to `SetMode` but for an existing window. | In Scope | [ ] |
| `love.window.showMessageBox(title, message, type, attachtowindow)` | `Night.Window.ShowMessageBox(string title, string message, Night.MessageBoxType type = Night.MessageBoxType.Info, bool attachToWindow = true)` | `public static void ShowMessageBox(string title, string message, Night.MessageBoxType type = Night.MessageBoxType.Info, bool attachToWindow = true)` <br> `MessageBoxType` enum: `Info`, `Warning`, `Error`. | Out of Scope (Low priority) | [ ] |

**Night Engine Specific Types:**
*   `Night.WindowFlags`: A struct or class that might contain boolean properties like `Fullscreen`, `Resizable`, `Borderless`, `VSync`, and potentially integer values for `MinMSAA`, `DepthBits`, `StencilBits`.
*   `Night.FullscreenType`: Enum (`Desktop`, `Exclusive`).
*   `Night.FullscreenMode`: Struct/class (`int Width, int Height, int RefreshRate`).
*   `Night.ImageData`: Wrapper for image data, likely from `Night.Image` module.
*   `Night.MessageBoxType`: Enum (`Info`, `Warning`, `Error`).
