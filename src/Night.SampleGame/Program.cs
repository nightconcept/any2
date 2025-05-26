using Night.Types;

namespace Night.SampleGame;

public class Game : IGame
{
  // Fields to store the previous state of keys for change detection
  private bool _wasSpaceDown = false;
  private bool _wasADown = false;
  private bool _wasEscapeDown = false;

  // Fields to store the previous state of mouse buttons for change detection
  private bool _wasLeftMouseDown = false;
  private bool _wasRightMouseDown = false;
  private bool _wasMiddleMouseDown = false;

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

    // Test for Left Mouse Button state change
    bool isLeftMouseCurrentlyDown = Night.Mouse.IsDown(Night.Types.MouseButton.Left);
    if (isLeftMouseCurrentlyDown && !_wasLeftMouseDown)
    {
      System.Console.WriteLine("Mouse Button Pressed: Left");
    }
    else if (!isLeftMouseCurrentlyDown && _wasLeftMouseDown)
    {
      System.Console.WriteLine("Mouse Button Released: Left");
    }
    _wasLeftMouseDown = isLeftMouseCurrentlyDown;

    // Test for Right Mouse Button state change
    bool isRightMouseCurrentlyDown = Night.Mouse.IsDown(Night.Types.MouseButton.Right);
    if (isRightMouseCurrentlyDown && !_wasRightMouseDown)
    {
      System.Console.WriteLine("Mouse Button Pressed: Right");
    }
    else if (!isRightMouseCurrentlyDown && _wasRightMouseDown)
    {
      System.Console.WriteLine("Mouse Button Released: Right");
    }
    _wasRightMouseDown = isRightMouseCurrentlyDown;

    // Test for Middle Mouse Button state change
    bool isMiddleMouseCurrentlyDown = Night.Mouse.IsDown(Night.Types.MouseButton.Middle);
    if (isMiddleMouseCurrentlyDown && !_wasMiddleMouseDown)
    {
      System.Console.WriteLine("Mouse Button Pressed: Middle");
    }
    else if (!isMiddleMouseCurrentlyDown && _wasMiddleMouseDown)
    {
      System.Console.WriteLine("Mouse Button Released: Middle");
    }
    _wasMiddleMouseDown = isMiddleMouseCurrentlyDown;

    // Test Mouse Position
    (int mouseX, int mouseY) = Night.Mouse.GetPosition();
    System.Console.WriteLine($"Mouse Position: ({mouseX}, {mouseY})");
  }

  public void Draw()
  {
    // Placeholder for drawing game elements
    // System.Console.WriteLine("SampleGame: Draw");
  }
}

public class Program
{
  public static void Main()
  {
    Night.Framework.Run<Game>();
  }
}
