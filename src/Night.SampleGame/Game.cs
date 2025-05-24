using Night.Engine; // To access IGame

namespace Night.SampleGame;

public class Game : IGame
{
    public void Load()
    {
        // Placeholder for loading game assets and initial setup
        System.Console.WriteLine("SampleGame: Load");
    }

    public void Update(double deltaTime)
    {
        // Placeholder for game logic updates
        // System.Console.WriteLine($"SampleGame: Update, DeltaTime: {deltaTime}");
    }

    public void Draw()
    {
        // Placeholder for drawing game elements
        // System.Console.WriteLine("SampleGame: Draw");
    }
}
