namespace Night.Engine;

/// <summary>
/// Manages the main game loop and coordination of game states.
/// </summary>
public static class GameEngine // Renamed from Engine to avoid conflict with potential Night.Engine namespace
{
    /// <summary>
    /// Runs the game instance.
    /// </summary>
    /// <typeparam name="TGame">The type of the game to run, implementing IGame.</typeparam>
    public static void Run<TGame>() where TGame : IGame, new()
    {
        TGame game = new TGame();
        // Placeholder for game loop logic (Load, Update, Draw)
        // game.Load();
        // while (Night.Window.IsOpen()) // Example condition
        // {
        //     game.Update(0.016); // Example delta time
        //     game.Draw();
        //     Night.Graphics.Present(); // Example presentation
        // }
    }
}

/// <summary>
/// Interface for a game that can be run by the Night Engine.
/// </summary>
public interface IGame
{
    void Load();
    void Update(double deltaTime);
    void Draw();
    // Optional input handlers can be added here later
    // void KeyPressed(KeyCode key, bool isRepeat);
    // void MousePressed(int x, int y, MouseButton button, int presses);
}
