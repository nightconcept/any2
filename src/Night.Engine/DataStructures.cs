// Copyright (c) 2025 Night Engine Contributors
// Distributed under the MIT license. See LICENSE for details.

// TODO: Expand KeyCode with more comprehensive SDL key codes.
// TODO: Review WindowFlags against SDL3 capabilities for completeness.

namespace Night.Types
{
    /// <summary>
    /// Represents keyboard keys.
    /// </summary>
    /// <remarks>
    /// This is a preliminary list and will be expanded based on SDL3 key codes.
    /// </remarks>
    public enum KeyCode
    {
        Unknown,

        // Letters
        A, B, C, D, E, F, G, H, I, J, K, L, M,
        N, O, P, Q, R, S, T, U, V, W, X, Y, Z,

        // Numbers
        Num0, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9,

        // Function keys
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,

        // Control keys
        LeftShift, RightShift, LeftControl, RightControl,
        LeftAlt, RightAlt, LeftSuper, RightSuper, // Super key (Windows/Command key)
        Enter, Escape, Space, Tab, Backspace, Delete, Insert,
        Home, End, PageUp, PageDown,

        // Arrow keys
        Up, Down, Left, Right,
    }

    /// <summary>
    /// Represents mouse buttons.
    /// </summary>
    public enum MouseButton
    {
        Unknown,
        Left,
        Middle,
        Right,
        X1, // Typically the first extra mouse button (e.g., "back")
        X2  // Typically the second extra mouse button (e.g., "forward")
    }

    /// <summary>
    /// Represents a color with Red, Green, Blue, and Alpha components.
    /// </summary>
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        // Common color presets
        public static readonly Color Black = new(0, 0, 0);
        public static readonly Color White = new(255, 255, 255);
        public static readonly Color Red = new(255, 0, 0);
        public static readonly Color Green = new(0, 255, 0);
        public static readonly Color Blue = new(0, 0, 255);
        public static readonly Color Yellow = new(255, 255, 0);
        public static readonly Color Magenta = new(255, 0, 255);
        public static readonly Color Cyan = new(0, 255, 255);
        public static readonly Color Transparent = new(0, 0, 0, 0);
    }

    /// <summary>
    /// Represents a rectangle with position (X, Y) and dimensions (Width, Height).
    /// </summary>
    public struct Rectangle
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// Specifies window creation flags.
    /// </summary>
    /// <remarks>
    /// This is a preliminary list and will be aligned with SDL3 window flags.
    /// </remarks>
    [System.Flags]
    public enum WindowFlags
    {
        None = 0,
        Fullscreen = 1 << 0,    // Corresponds to SDL_WINDOW_FULLSCREEN
        Borderless = 1 << 1,    // Corresponds to SDL_WINDOW_BORDERLESS
        Resizable = 1 << 2,     // Corresponds to SDL_WINDOW_RESIZABLE
        Shown = 1 << 3,         // Corresponds to SDL_WINDOW_SHOWN
        Hidden = 1 << 4,        // Corresponds to SDL_WINDOW_HIDDEN
        Minimized = 1 << 5,     // Corresponds to SDL_WINDOW_MINIMIZED
        Maximized = 1 << 6,     // Corresponds to SDL_WINDOW_MAXIMIZED
        HighDpi = 1 << 7,       // Corresponds to SDL_WINDOW_HIGH_PIXEL_DENSITY
        OpenGL = 1 << 8,        // Corresponds to SDL_WINDOW_OPENGL (if needed directly)
        Vulkan = 1 << 9,        // Corresponds to SDL_WINDOW_VULKAN (if needed directly)
        Metal = 1 << 10,       // Corresponds to SDL_WINDOW_METAL (if needed directly)
    }

    /// <summary>
    /// Represents a 2D sprite.
    /// </summary>
    /// <remarks>
    /// This is a placeholder structure. It will be expanded in Epic 5.
    /// It might include texture references, source rectangles, etc.
    /// </remarks>
    public class Sprite
    {
        // Placeholder for texture identifier (e.g., an IntPtr or a managed object)
        // internal object? _textureHandle; // Example, to be refined

        // Placeholder for dimensions, if not derived from texture
        // public int Width { get; internal set; }
        // public int Height { get; internal set; }

        // Placeholder for source rectangle within a texture atlas
        // public Rectangle? SourceRectangle { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <remarks>
        /// Constructor is internal or protected if creation is managed by the engine.
        /// For now, public for flexibility during early prototyping.
        /// </remarks>
        public Sprite()
        {
            // Initialization logic will be added in Epic 5.
        }
    }

    /// <summary>
    /// Interface for a game that can be run by the Night Engine.
    /// Game developers will implement this interface in their main game class.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Called once when the game starts, for loading resources.
        /// </summary>
        void Load();

        /// <summary>
        /// Called repeatedly every frame, for updating game logic.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last frame, in seconds.</param>
        void Update(double deltaTime);

        /// <summary>
        /// Called repeatedly every frame, for drawing the game state.
        /// </summary>
        void Draw();

        // Optional input handlers can be added here later as per PRD Feature 4.
        // For example:
        // void KeyPressed(KeyCode key, bool isRepeat);
        // void MousePressed(int x, int y, MouseButton button, int presses);
    }
}
