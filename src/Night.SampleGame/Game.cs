using Night.Types;

namespace Night.SampleGame;

public class Game : IGame
{
  public void Load()
  {
    // Placeholder for loading game assets and initial setup
    Night.Window.SetMode(800, 600, WindowFlags.Shown | WindowFlags.Resizable);
    Night.Window.SetTitle("Night Sample Game");
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
