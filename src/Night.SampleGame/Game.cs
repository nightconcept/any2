using Night.Types;

namespace Night.SampleGame;

public class Game : IGame
{
  // Fields to store the previous state of keys for change detection
  private bool _wasSpaceDown = false;
  private bool _wasADown = false;
  private bool _wasEscapeDown = false;

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

    // Test for Space key state change
    bool isSpaceCurrentlyDown = Night.Keyboard.IsDown(Night.Types.KeyCode.Space);
    if (isSpaceCurrentlyDown && !_wasSpaceDown)
    {
      System.Console.WriteLine("Key Pressed: Space");
    }
    else if (!isSpaceCurrentlyDown && _wasSpaceDown)
    {
      System.Console.WriteLine("Key Released: Space");
    }
    _wasSpaceDown = isSpaceCurrentlyDown;

    // Test for A key state change
    bool isACurrentlyDown = Night.Keyboard.IsDown(Night.Types.KeyCode.A);
    if (isACurrentlyDown && !_wasADown)
    {
      System.Console.WriteLine("Key Pressed: A");
    }
    else if (!isACurrentlyDown && _wasADown)
    {
      System.Console.WriteLine("Key Released: A");
    }
    _wasADown = isACurrentlyDown;

    // Test for Escape key state change
    bool isEscapeCurrentlyDown = Night.Keyboard.IsDown(Night.Types.KeyCode.Escape);
    if (isEscapeCurrentlyDown && !_wasEscapeDown)
    {
      System.Console.WriteLine("Key Pressed: Escape. (Consider closing window)");
      // Example of how it might be used to close on press:
      // Night.Window.Close();
    }
    else if (!isEscapeCurrentlyDown && _wasEscapeDown)
    {
      System.Console.WriteLine("Key Released: Escape");
    }
    _wasEscapeDown = isEscapeCurrentlyDown;
  }

  public void Draw()
  {
    // Placeholder for drawing game elements
    // System.Console.WriteLine("SampleGame: Draw");
  }
}
