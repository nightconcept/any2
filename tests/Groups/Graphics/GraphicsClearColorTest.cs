// <copyright file="GraphicsClearColorTest.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;

using Night;

using NightTest.Core;

namespace NightTest.Groups.Graphics
{
  /// <summary>
  /// Tests the Graphics.Clear() method with a specific color.
  /// Requires manual confirmation that the color is correct.
  /// </summary>
  public class GraphicsClearColorTest : BaseManualTestCase
  {
    private readonly Color _skyBlue = new Color(135, 206, 235);
    private bool _promptRequested = false;

    /// <inheritdoc/>
    public override string Name => "Graphics.Clear.SkyBlueColor";

    /// <inheritdoc/>
    public override TestType Type => TestType.Manual;

    /// <inheritdoc/>
    public override string Description => "Tests clearing the screen to sky blue (135, 206, 235). User must confirm color.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();
      // Initial details, will be updated by manual confirmation result
      Details = "Test running, displaying sky blue color.";
    }

    /// <inheritdoc/>
    public override void Update(double deltaTime)
    {
      // Call base update to handle IsDone checks and manual test timeout
      base.Update(deltaTime);
      if (IsDone) // Check again in case base.Update finished the test
      {
        return;
      }

      // Request manual confirmation once after a short delay (using value from base) to ensure window is visible
      if (!_promptRequested && TestStopwatch.ElapsedMilliseconds > ManualTestPromptDelayMilliseconds)
      {
        RequestManualConfirmation("Is the screen cleared to a SKY BLUE color (like a clear daytime sky)?");
        _promptRequested = true;
      }
    }

    /// <inheritdoc/>
    public override void Draw()
    {
      Night.Graphics.Clear(_skyBlue);

      base.Draw();

      Night.Graphics.Present();
    }

    /// <inheritdoc/>
    public override void KeyPressed(KeySymbol key, KeyCode scancode, bool isRepeat)
    {
      base.KeyPressed(key, scancode, isRepeat);
      if (IsDone)
      {
        return;
      }
      // Custom key handling for this specific test can go here, if any.
      // The ESC to fail logic is now handled in BaseTestCase.KeyPressed()
    }
  }
}
