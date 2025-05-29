using System;
// SDL3 using is not strictly necessary here anymore as KeySymbol/KeyCode are now fully qualified in IGame
// However, if other SDL types were used directly by IGame, it would be.

namespace Night
{
  // Only IGame interface remains here.
  // All other enums, structs, and classes have been moved to module-specific files.

  /// <summary>
  /// Interface for a game that can be run by the Night Engine.
  /// Game developers will implement this interface in their main game class.
  /// </summary>
  public interface IGame
  {
    /// <summary>
    /// Called exactly once when the game starts for loading resources.
    /// </summary>
    void Load();

    /// <summary>
    /// Callback function used to update the state of the game every frame.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last frame, in seconds.</param>
    void Update(double deltaTime);

    /// <summary>
    /// Callback function used to draw on the screen every frame.
    /// </summary>
    void Draw();

    /// <summary>
    /// Callback function triggered when a key is pressed.
    /// </summary>
    /// <param name="key">The logical key symbol that was pressed.</param>
    /// <param name="scancode">The physical key (scancode) that was pressed.</param>
    /// <param name="isRepeat">True if this is a key repeat event, false otherwise.</param>
    void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat);
  }

}
